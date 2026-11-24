using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ViewModels;
using ViewModels.ReportQCViewModel;

namespace BAL
{
   public interface ITMProductivityRptRepository
    {
        
        IEnumerable<TeamProductivityReportViewModel> GetTeamProductivityReports(DateTime selectedDate, int? departmentId = null);
        IEnumerable<SelectListItem> GetDepartments();
        IEnumerable<UserCheckDetailViewModel> GetTotalDetailsForAllUser(DateTime selectedDate, string deptName);
        IEnumerable<UserCheckDetailViewModel> GetSlotWiseTotalDetails(DateTime selectedDate, string slot, string deptName);
        IEnumerable<UserCheckDetailViewModel> GetUserTotalDetails(int userId, DateTime selectedDate, string deptName);
        IEnumerable<UserCheckDetailViewModel> UserSlotDetails(int userId, DateTime selectedDate, string slot, List<string> statusList, string deptName);
        IEnumerable<TeamProductivityExportViewModel> GetTeamProductivityForExport(DateTime selectedDate, int? departmentId = null);
    }
}
