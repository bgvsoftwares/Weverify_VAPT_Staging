using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using ViewModels.ReportQCViewModel;
using static ViewModels.TeamProductivityReportViewModel;

namespace BAL
{
    public class TMProductivityRptRepository : ITMProductivityRptRepository
    {
        private readonly DataContext db;
        private readonly ClientCustomize cc;
        public TMProductivityRptRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public IEnumerable<SelectListItem> GetDepartments()
        {
            var ids = new List<int> { 1, 5, 7, 8, 9, 10, 17 };
            return db.MasterDepartments.Where(d => ids.Contains(d.DepartmentRowID) && d.Status == 1).Select(d => new SelectListItem
            {
                Value = d.DepartmentRowID.ToString(),
                Text = d.DepartmentName
            }).ToList();
        }

        public IEnumerable<TeamProductivityReportViewModel> GetTeamProductivityReports(DateTime selectedDate, int? departmentId = null)
        {
            var timeSlots = ViewModels.TimeSlotHelper.TimeSlots;
            DateTime fromDate = selectedDate.Date;
            DateTime toDate = selectedDate.Date.AddDays(1);
            int? userIdParam = 0;
            DataTable dt = cc.GetTeamProductivityData(fromDate, toDate, userIdParam, departmentId);
            if (dt == null || dt.Rows.Count == 0)
                return new List<TeamProductivityReportViewModel>();
            var rows = dt.AsEnumerable()
                .Select(r => new
                {
                    UserId = r["TeamMemberRowID"] != DBNull.Value ? Convert.ToInt32(r["TeamMemberRowID"]) : 0,
                    SubCheckRowID = r["CheckUniqueID"] != DBNull.Value ? Convert.ToInt32(r["CheckUniqueID"]) : 0,
                    UserName = r["TeamMemberName"]?.ToString() ?? "",
                    DepartmentId = r["DepartmentRowID"] != DBNull.Value ? Convert.ToInt32(r["DepartmentRowID"]) : 0,
                    DepartmentName = r["DepartmentName"]?.ToString() ?? "",
                    Status = r["Status"]?.ToString() ?? "",
                    UpdatedDate = r["UpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["UpdatedDate"])
                }).ToList();

            var grouped = rows.GroupBy(x => x.UserId)
                .Select(g =>
                {
                    var first = g.First();
                    var report = new TeamProductivityReportViewModel
                    {
                        UserId = g.Key,
                        UserName = first.UserName,
                        DepartmentName = first.DepartmentName,
                        SelectedDate = selectedDate,
                        TimeSlotCounts = new Dictionary<string, int>()
                    };
                    int count = 0;
                    foreach (var slot in timeSlots)
                    {
                        count = g.Count(x => x.UpdatedDate != null && x.UpdatedDate.Value.TimeOfDay >= slot.Item1 && x.UpdatedDate.Value.TimeOfDay <= slot.Item2 && x.SubCheckRowID != 0);
                        report.TimeSlotCounts[slot.Item3] = count;
                    }
                    report.TotalCount = report.TimeSlotCounts.Values.Sum();
                    return report;
                }).OrderBy(x => x.UserName).ToList();
            return grouped;
        }

        public IEnumerable<UserCheckDetailViewModel> GetUserTotalDetails(int userId, DateTime selectedDate, string deptName)
        {
            return UserSlotDetails(userId, selectedDate, "", null, deptName);
        }

        public IEnumerable<UserCheckDetailViewModel> GetSlotWiseTotalDetails(DateTime selectedDate, string slot, string deptName)
        {
            return UserSlotDetails(0, selectedDate, slot, null, deptName);
        }

        public IEnumerable<UserCheckDetailViewModel> GetTotalDetailsForAllUser(DateTime selectedDate, string deptName)
        {
            return UserSlotDetails(0, selectedDate, "", null, deptName);
        }

        public IEnumerable<UserCheckDetailViewModel> UserSlotDetails(int userId, DateTime selectedDate, string slot, List<string> statusList, string deptName)
        {

            var selectedSlot = TimeSlotHelper.TimeSlots.FirstOrDefault(s => s.Item3.Replace(" ", "").Equals(slot.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                ?? Tuple.Create(TimeSpan.Zero, TimeSpan.FromDays(1), "FULLDAY");

            DateTime fromDate = selectedDate.Date.Add(selectedSlot.Item1);
            DateTime toDate = selectedDate.Date.Add(selectedSlot.Item2);

            int? departmentId = null;
            if (!string.IsNullOrEmpty(deptName) && deptName != "0")
            {
                ushort deptIdValue = Convert.ToUInt16(deptName);
                departmentId = db.MasterDepartments.Where(d => d.DepartmentRowID == deptIdValue).Select(d => d.DepartmentRowID).FirstOrDefault();
            }
            int? userIds;
            if (userId != 0)
            {
                userIds = userId;
            }
            else
            {
                userIds = 0;
            }
            DataTable dt = cc.GetTeamProductivityData(fromDate, toDate, userIds, departmentId);

            var filtered = dt.AsEnumerable()
                .Where(dr => !string.IsNullOrWhiteSpace(dr.Field<string>("CandidateFullName")) && !string.IsNullOrWhiteSpace(dr.Field<string>("SubCheckName")))
                .Select(dr => new UserCheckDetailViewModel
                {
                    ClientName = dr.Field<string>("ClientName"),
                    CompanyRefNo = dr.Field<string>("CompanyRefNo"),
                    SubCheckName = dr.Field<string>("SubCheckName"),
                    CandidateName = dr.Field<string>("CandidateFullName"),
                    CheckStatus = dr.Field<string>("Status"),
                    Remarks = dr.Field<string>("Remarks"),
                    UpdatedByNameDesig = dr.Field<string>("UpdatedByNameDesig"),
                    UpdatedDate = dr["UpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["UpdatedDate"])
                }).ToList();
            return filtered;
        }

        public IEnumerable<TeamProductivityExportViewModel> GetTeamProductivityForExport(DateTime selectedDate, int? departmentId = null)
        {
            var reportData = GetTeamProductivityReports(selectedDate, departmentId).ToList();

            if (reportData == null || !reportData.Any())
            {
                return new List<TeamProductivityExportViewModel>();
            }

            return reportData.Select(r => new TeamProductivityExportViewModel
            {
                UserName = r.UserName,
                TimeSlotCounts = r.TimeSlotCounts,
                Total = r.TimeSlotCounts.Values.Sum()
            }).ToList();
        }

    }
}
