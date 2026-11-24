using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BAL
{
    public class CaseFlowMappingRepository : ICaseFlowMappingRepository
    {
        DataContext db;
        public CaseFlowMappingRepository()
        {
            db = new DataContext();
        }

        public IEnumerable<CaseFlowMappingViewModel> GetCaseFlowMapping()
        {
            try
            {
                List<CaseFlowMappingViewModel> lst = new List<CaseFlowMappingViewModel>();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetCaseFlowMapping();
                lst = (from DataRow dr in dt.Rows
                       select new CaseFlowMappingViewModel()
                       {
                           CaseFlowMappingRowID = Convert.ToInt32(dr["CaseFlowMappingRowID"]),
                           ReportCheckMappingRowID = Convert.ToInt32(dr["ReportCheckMappingRowID"]),
                           ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                           ReportType = Convert.ToString(dr["ReportType"]),
                           ColorCode = Convert.ToString(dr["ColorCode"]),
                           MoveToRWQC = Convert.ToByte(dr["MoveToRWQC"]),
                           ColorCodeReview = Convert.ToByte(dr["ColorCodeReview"]),
                           SendToClient = Convert.ToByte(dr["SendToClient"]),                           
                           Status = Convert.ToByte(dr["Status"]),
                       }).ToList();
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CaseFlowMappingListPagedModel GetCaseFlowMapping(int pageNo, int pageSize, string sort, string sortDir,  string Search = "", int RCMRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<MasterCaseFlowMapping> data = db.MasterCaseFlowMappings.Include("MasterReportCheckMappings")
                    .Where(c => c.ReportCheckMappingRowID == RCMRowID);

                if (!string.IsNullOrEmpty(Search))
                {
                    //data = data.Where(c => c.MasterReportCheckMapping.ReportType.ToString().Contains(Search));
                }                

                if (data == null)
                {
                    throw new Exception("No data found matching the filter criteria");
                }

                switch (sort)
                {
                    case "colorcode":
                        data = sortDir == "asc" ? data.OrderBy(c => c.ReportCheckMappingRowID) : data.OrderByDescending(c => c.ReportCheckMappingRowID);
                        break;                    
                    default:
                        data = sortDir == "asc" ? data.OrderBy(c => c.CaseFlowMappingRowID) : data.OrderByDescending(c => c.CaseFlowMappingRowID);
                        break;
                }
                CaseFlowMappingListPagedModel model = new CaseFlowMappingListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.CaseFlowMappings = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new CaseFlowMappingViewModel
                {
                    ReportCheckMappingRowID = item.ReportCheckMappingRowID,
                    ClientRowID = item.MasterReportCheckMapping.ClientRowID,
                    ClientName = item.MasterReportCheckMapping.PQClientMaster.ClientCode1,
                    ReportType = item.MasterReportCheckMapping.ReportType,
                    MoveToRWQC = item.MoveToRWQC,
                    ColorCodeReview = item.ColorCodeReview,
                    SendToClient = item.SendToClient,                    
                    ColorCode = item.ColorCode,
                    Status = item.Status
                }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
