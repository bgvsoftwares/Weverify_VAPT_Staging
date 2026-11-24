using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BAL
{
    public class EduMailMappingRepository : IEduMailMappingRepository 
    {
        DataContext db;
        public EduMailMappingRepository()
        {
            db = new DataContext();
        }
                        
        public EduMailMappingListPagedModel GetEduMailMapping(int pageNo, int pageSize, string sort, string sortDir, short UniversityRowID, int CollegeRowID, string Search = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<MasterEduMailMapping> data = db.MasterEduMailMappings;
                if (UniversityRowID > 0)
                {
                    if (UniversityRowID > 0)
                    {
                        data = data.Where(d => d.UniversityRowID == UniversityRowID);
                    }
                    if (CollegeRowID > 0)
                    {
                        data = data.Where(d => d.CollegeRowID == CollegeRowID);
                    }
                }
                if (data == null)
                {
                    throw new Exception("No data found matching the filter criteria");
                }

                switch (sort)
                {
                    case "UniversityName":
                        data = sortDir == "asc"
                            ? data.OrderBy(d => d.MasterUniversity.UniversityName)
                            : data.OrderByDescending(d => d.MasterUniversity.UniversityName);
                        break;

                    case "CollegeName":
                        data = sortDir == "asc"
                            ? data.OrderBy(d => d.MasterCollege.CollegeName)
                            : data.OrderByDescending(d => d.MasterCollege.CollegeName);
                        break;

                    default:
                        data = sortDir == "desc"
                            ? data.OrderBy(d => d.EduMailMappingRowID)
                            : data.OrderByDescending(d => d.EduMailMappingRowID);
                        break;
                }
                EduMailMappingListPagedModel model = new EduMailMappingListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.EduMailMapping = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new EduMailMappingViewModel
                {
                    EduMailMappingRowID = item.EduMailMappingRowID,
                    UniversityRowID = item.UniversityRowID,
                    UniversityName = item.MasterUniversity.UniversityName,
                    CollegeRowID = item.CollegeRowID,
                    //CollegeName = item.MasterCollege.CollegeName,
                    CollegeName = (item.CollegeRowID == 1) ? "NA" : (item.MasterCollege != null ? item.MasterCollege.CollegeName : "NA"),
                    EMailType = item.EMailType,
                    EMailID = item.EMailID,
                    Status = item.Status
                }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public UpdateEduMailMappingViewModel GetEduMailMappingForUpdateById(int EduMailMappingRowID)
        {
            try
            {
                UpdateEduMailMappingViewModel model = new UpdateEduMailMappingViewModel();
                var entity = db.MasterEduMailMappings.Find(EduMailMappingRowID);
                if (entity != null)
                {
                    model.EduMailMappingRowID = entity.EduMailMappingRowID;
                    model.UniversityRowID = entity.UniversityRowID;
                    model.CollegeRowID = entity.CollegeRowID;
                    model.EMailType = entity.EMailType;
                    model.EMailID = entity.EMailID;                    
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

        public string UpdateEduMailMapping(UpdateEduMailMappingViewModel model)
        {
            try
            {
                var email = model.EMailID.Trim().ToLower();

                // Find record with same University, College, EmailType
                var existing = db.MasterEduMailMappings
                    .FirstOrDefault(x => x.UniversityRowID == model.UniversityRowID
                                      && x.CollegeRowID == model.CollegeRowID
                                      && x.EMailType == model.EMailType);

                if (existing != null)
                {
                   
                    if (existing.Status == 0)
                    {
                        existing.EMailID = email;   
                        existing.Status = 1;        
                        db.Entry(existing).State = EntityState.Modified;
                        db.SaveChanges();
                        return "activated";  
                    }
                    else
                    {
                        
                        existing.EMailID = email;   
                        db.Entry(existing).State = EntityState.Modified;
                        db.SaveChanges();
                        return "updated";
                    }
                }
                else
                {
                    
                    model.EMailID = email; 
                    var entity = new MasterEduMailMapping
                    {
                        UniversityRowID = model.UniversityRowID,
                        CollegeRowID = model.CollegeRowID,
                        EMailType = model.EMailType,
                        EMailID = email,
                        Status = 1
                    };
                    db.MasterEduMailMappings.Add(entity);
                    db.SaveChanges();
                    return "inserted";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsEduMailMappingExist(short UniversityRowID, int CollegeRowID, string EMailType, int EduMailMappingRowID)
        {
            try
            {
                var EduMailMapping = db.MasterEduMailMappings
                    .Where(s => (s.EMailType.Trim().ToLower() == EMailType.Trim().ToLower() && s.UniversityRowID == UniversityRowID && s.CollegeRowID == CollegeRowID) && s.EduMailMappingRowID != EduMailMappingRowID)
                    .FirstOrDefault();
                if (EduMailMapping != null)
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
                
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
        
        public void ActiveEduMailMapping(short id = 0, string checkeds = "")
        {
            try
            {
                if (id != 0 && checkeds != null)
                {
                    db.MasterEduMailMappings.Single(b => b.EduMailMappingRowID == id).Status = Convert.ToByte(checkeds);
                }
                else
                {
                    throw new Exception("EduMailMappingRowID could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<EduMailMappingExportViewModel> GetEduMailMappingForExport()
        {
            try
            {
                return db.MasterEduMailMappings.Select(c =>
                new EduMailMappingExportViewModel
                {
                    EduMailMappingRowID = c.EduMailMappingRowID,
                    UniversityName = c.MasterUniversity.UniversityName,
                    CollegeName = c.MasterCollege.CollegeName,
                    EMailType = c.EMailType,
                    EMailID = c.EMailID,
                    Status = (c.Status == 1 ? "Active" : "Inactive")
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
