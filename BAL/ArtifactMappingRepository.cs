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
    public class ArtifactMappingRepository : IArtifactMappingRepository
    {
        DataContext db;
        public ArtifactMappingRepository()
        {
            db = new DataContext();
        }

        public IEnumerable<ArtifactMappingViewModel> GetArtifactMapping()
        {
            try
            {
                List<ArtifactMappingViewModel> lst = new List<ArtifactMappingViewModel>();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetArtifactMapping();
                lst = (from DataRow dr in dt.Rows
                       select new ArtifactMappingViewModel()
                       {
                           ArtifactMappingRowID = Convert.ToInt32(dr["ArtifactMappingRowID"]),
                           ReportCheckMappingRowID = Convert.ToInt32(dr["ReportCheckMappingRowID"]),
                           ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                           ClientName = Convert.ToString(dr["ClientName"]),
                           ReportType = Convert.ToString(dr["ReportType"]),
                           ClientSeverityRowId = Convert.ToInt32(dr["ClientSeverityRowId"]),
                           ColorCode = Convert.ToString(dr["ColorCode"]),
                           CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]),
                           CheckFamilyName = Convert.ToString(dr["CheckFamilyName"]),
                           DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]),
                           DocumentType = Convert.ToString(dr["DocumentType"]),
                           DocumentSeqNo = Convert.ToInt16(dr["DocumentSeqNo"]),
                           Status = Convert.ToByte(dr["Status"]),
                       }).ToList();
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ArtifactMappingListPagedModel GetArtifactMapping(int pageNo, int pageSize, string sort, string sortDir,  string Search = "", int RCMRowID = 0, short CheckFamilyRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<MasterArtifactMapping> data = db.MasterArtifactMappings.Include("MasterReportCheckMappings")
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
                        data = data.OrderBy(c => c.MasterReportCheckMapping.ReportType).ThenBy(c => c.CheckFamilyRowID).ThenBy(c => c.ColorCode).ThenBy(c => c.DocumentSeqNo);
                        break;                    
                    default:
                        data = sortDir == "asc" ? data.OrderBy(c => c.CheckFamilyRowID).ThenBy(c => c.ColorCode).ThenBy(c => c.DocumentSeqNo) : data.OrderBy(c => c.CheckFamilyRowID).ThenBy(c => c.ColorCode).ThenBy(c => c.DocumentSeqNo);
                        break;
                }
                ArtifactMappingListPagedModel model = new ArtifactMappingListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.ArtifactMappings = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new ArtifactMappingViewModel
                {
                    ReportCheckMappingRowID = item.ReportCheckMappingRowID,
                    ClientRowID = item.MasterReportCheckMapping.ClientRowID,
                    ClientName = item.MasterReportCheckMapping.PQClientMaster.ClientCode1,
                    ReportType = item.MasterReportCheckMapping.ReportType,
                    ClientSeverityRowId = item.ClientSeverityRowId,
                    ColorCode = item.PQClientSeverity.ClientColorName,
                    CheckFamilyRowID = item.CheckFamilyRowID,
                    CheckFamilyName = item.MasterCheckFamily.CheckFamilyName,
                    DocumentTypeId = item.DocumentTypeId,                    
                    DocumentType = item.DocumentType,
                    DocumentSeqNo = item.DocumentSeqNo,                    
                    Status = item.Status
                }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
                
        public IEnumerable<ArtifactMappingViewModel> GetArtifactMappingByColor(int RCMRowID, short CheckFamilyRowID, int ClientSeverityRowId)
        {
            try
            {
                List<ArtifactMappingViewModel> lst = new List<ArtifactMappingViewModel>();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetArtifactMappingByColor(RCMRowID, CheckFamilyRowID, ClientSeverityRowId);
                lst = (from DataRow dr in dt.Rows
                       select new ArtifactMappingViewModel()
                       {
                           ArtifactMappingRowID = Convert.ToInt32(dr["ArtifactMappingRowID"]),
                           ReportCheckMappingRowID = Convert.ToInt32(dr["ReportCheckMappingRowID"]),
                           ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                           ClientName = Convert.ToString(dr["ClientName"]),
                           ReportType = Convert.ToString(dr["ReportType"]),
                           ClientSeverityRowId = Convert.ToInt32(dr["ClientSeverityRowId"]),
                           ColorCode = Convert.ToString(dr["ColorCode"]),
                           CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]),
                           CheckFamilyName = Convert.ToString(dr["CheckFamilyName"]),
                           DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]),
                           DocumentType = Convert.ToString(dr["DocumentType"]),
                           DocumentSeqNo = Convert.ToInt16(dr["DocumentSeqNo"]),
                           Status = Convert.ToByte(dr["Status"]),
                       }).ToList();
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
