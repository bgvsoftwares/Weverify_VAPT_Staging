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
    public class ReportCheckMappingRepository : IReportCheckMappingRepository
    {
        DataContext db;
        public ReportCheckMappingRepository()
        {
            db = new DataContext();
        }

        public IEnumerable<ReportCheckMappingViewModel> GetReportCheckMapping()
        {
            try
            {
                List<ReportCheckMappingViewModel> lst = new List<ReportCheckMappingViewModel>();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetReportCheckMapping();
                lst = (from DataRow dr in dt.Rows
                       select new ReportCheckMappingViewModel()
                       {
                           ReportCheckMappingRowID = Convert.ToInt32(dr["ReportCheckMappingRowID"]),
                           ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                           ReportType = Convert.ToString(dr["ReportType"]),
                           AddressChk = Convert.ToByte(dr["AddressChk"]),
                           CriminalChk = Convert.ToByte(dr["CriminalChk"]),
                           EducationChk = Convert.ToByte(dr["EducationChk"]),
                           EmploymentChk = Convert.ToByte(dr["EmploymentChk"]),
                           IdentityChk = Convert.ToByte(dr["IdentityChk"]),
                           ReferenceChk = Convert.ToByte(dr["ReferenceChk"]),
                           Status = Convert.ToByte(dr["Status"]),
                       }).ToList();
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReportCheckMappingListPagedModel GetReportCheckMapping(int pageNo, int pageSize, string sort, string sortDir,short ClientRowID = 0, string Search = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<MasterReportCheckMapping> data = db.MasterReportCheckMappings.Where(a=>a.Status==1);
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    if (ClientRowID > 0)
                    {
                        data = data.Where(d => d.ClientRowID == ClientRowID);
                    }
                    if (!string.IsNullOrEmpty(Search))
                    {
                        data = data.Where(d => d.ReportType.Contains(Search));
                    }
                }

                if (data == null)
                {
                    throw new Exception("No data found matching the filter criteria");
                }

                switch (sort)
                {
                    case "reporttype":
                        data = sortDir == "asc" ? data.OrderBy(c => c.ClientRowID) : data.OrderByDescending(c => c.ClientRowID);
                        break;                    
                    default:
                        data = sortDir == "asc" ? data.OrderBy(c => c.ClientRowID).ThenBy(c=> c.ReportType).OrderByDescending(c => c.ReportCheckMappingRowID) : data.OrderByDescending(c => c.ClientRowID);
                        break;
                }
                ReportCheckMappingListPagedModel model = new ReportCheckMappingListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                //model.TotalRecords = data.AsEnumerable().Count();
                model.ReportCheckMapping = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new ReportCheckMappingViewModel
                {
                    ReportCheckMappingRowID = item.ReportCheckMappingRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.ClientCode1,
                    AddressChk = item.AddressChk,
                    CriminalChk = item.CriminalChk,
                    EducationChk = item.EducationChk,
                    EmploymentChk = item.EmploymentChk,
                    IdentityChk = item.IdentityChk,
                    ReferenceChk = item.ReferenceChk,
                    SpecialChk = item.SpecialChk,
                    ReportType = item.ReportType,
                    Status = item.Status
                }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public UpdateReportCheckMappingViewModel GetReportCheckMappingForUpdateById(int RCMRowID)
        {
            try
            {
                UpdateReportCheckMappingViewModel model = new UpdateReportCheckMappingViewModel();
                var entity = db.MasterReportCheckMappings.Find(RCMRowID);
                if (entity != null)
                {
                    model.ReportCheckMappingRowID = entity.ReportCheckMappingRowID;
                    model.ClientRowID = entity.ClientRowID;
                    model.ReportType = entity.ReportType;
                    model.AddressChk = entity.AddressChk;
                    model.CriminalChk = entity.CriminalChk;
                    model.EducationChk = entity.EducationChk;
                    model.EmploymentChk = entity.EmploymentChk;
                    model.IdentityChk = entity.IdentityChk;
                    model.ReferenceChk = entity.ReferenceChk;
                    model.SpecialChk = entity.SpecialChk;
                    model.Status = entity.Status;
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateReportCheckMapping(UpdateReportCheckMappingViewModel model)
        {
            try
            {
                if (model != null && model.ReportCheckMappingRowID > 0)
                {
                    db.MasterReportCheckMappings.Single(c => c.ReportCheckMappingRowID == model.ReportCheckMappingRowID).ClientRowID = model.ClientRowID;
                    db.MasterReportCheckMappings.Single(c => c.ReportCheckMappingRowID == model.ReportCheckMappingRowID).ReportType = model.ReportType;
                    db.MasterReportCheckMappings.Single(c => c.ReportCheckMappingRowID == model.ReportCheckMappingRowID).AddressChk = model.AddressChk;
                    db.MasterReportCheckMappings.Single(c => c.ReportCheckMappingRowID == model.ReportCheckMappingRowID).CriminalChk = model.CriminalChk;
                    db.MasterReportCheckMappings.Single(c => c.ReportCheckMappingRowID == model.ReportCheckMappingRowID).EducationChk = model.EducationChk;
                    db.MasterReportCheckMappings.Single(c => c.ReportCheckMappingRowID == model.ReportCheckMappingRowID).EmploymentChk = model.EmploymentChk;
                    db.MasterReportCheckMappings.Single(c => c.ReportCheckMappingRowID == model.ReportCheckMappingRowID).IdentityChk = model.IdentityChk;
                    db.MasterReportCheckMappings.Single(c => c.ReportCheckMappingRowID == model.ReportCheckMappingRowID).ReferenceChk = model.ReferenceChk;
                    db.MasterReportCheckMappings.Single(c => c.ReportCheckMappingRowID == model.ReportCheckMappingRowID).SpecialChk = model.SpecialChk;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsReportCheckMappingExist(short ClientRowID, string ReportType, int ReportCheckMappingRowID)
        {
            try
            {                
                var ReportCheckMapping = db.MasterReportCheckMappings
                    .Where(s => (s.ReportType.Trim().ToLower() == ReportType.Trim().ToLower() && s.ClientRowID == ClientRowID) && s.ReportCheckMappingRowID != ReportCheckMappingRowID)
                    .FirstOrDefault();
                if (ReportCheckMapping != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SaveChanges()
        {
            try
            {
                return db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public IEnumerable<ReportCheckMappingExportViewModel> GetReportCheckMappingForExport()
        {
            try
            {
                return db.MasterReportCheckMappings.Select(c =>
                new ReportCheckMappingExportViewModel
                {
                    ReportCheckMappingRowID = c.ReportCheckMappingRowID,
                    ClientName = c.PQClientMaster.ClientCode1,
                    ReportType = c.ReportType,
                    AddressChk = c.AddressChk,
                    CriminalChk = c.CriminalChk,
                    EducationChk = c.EducationChk,
                    EmploymentChk = c.EmploymentChk,
                    IdentityChk = c.IdentityChk,
                    ReferenceChk = c.ReferenceChk,
                    SpecialChk = c.SpecialChk
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
