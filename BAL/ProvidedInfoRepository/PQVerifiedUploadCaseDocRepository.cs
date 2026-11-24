using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public class PQVerifiedUploadCaseDocRepository : IPQVerifiedUploadCaseDocRepository
    {
        DataContext db;
        public PQVerifiedUploadCaseDocRepository()
        {
            db = new DataContext();
        }

        public void AddPQVerifiedUploadCaseDoc(AddPQVerifiedUploadCaseDocViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            var existData = context.PQVerifiedUploadCaseDocs.Where(p => p.PersonalRowID == model.PersonalRowID && p.DocumentType == model.DocumentType && p.FileName == model.FileName).FirstOrDefault();
                            if (existData == null)
                            {
                                PQVerifiedUploadCaseDoc entity = new PQVerifiedUploadCaseDoc();
                                entity.PersonalRowID = model.PersonalRowID;
                                entity.DocumentUploadFrom = model.DocumentUploadFrom;
                                entity.DocumentType = model.DocumentType;
                                entity.FileName = model.FileName;
                                entity.FilePath = model.FilePath;
                                entity.Remarks = model.Remarks;
                                entity.UploadDate = model.UploadDate;
                                entity.Status = model.Status;

                                context.PQVerifiedUploadCaseDocs.Add(entity);
                            }
                            else
                            {
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == existData.CaseDocRowID).FileName = model.FileName;
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == existData.CaseDocRowID).FilePath = model.FilePath;
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == existData.CaseDocRowID).Remarks = model.Remarks;
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == existData.CaseDocRowID).UploadDate = model.UploadDate;
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == existData.CaseDocRowID).PdfConvertedToImage = false;
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == existData.CaseDocRowID).ImageFileName = "";

                            }
                            context.SaveChanges();

                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("Candidate could not be blank!");
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public string ExistFileByPersonalIdAndDocType(int PersonalRowID = 0, string DocumentType = "")
        {
            try
            {
                var existData = db.PQVerifiedUploadCaseDocs.Where(p => p.PersonalRowID == PersonalRowID && p.DocumentType == DocumentType).FirstOrDefault();
                if (existData == null)
                {
                    return null;
                }
                else
                {
                    return existData.FileName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string ExistcaseFileByPersonalIdAndDocType(int PersonalRowID = 0, string DocumentType = "", string fileName = "")
        {
            try
            {
                var existData = db.PQVerifiedUploadCaseDocs.Where(p => p.PersonalRowID == PersonalRowID && p.DocumentType == DocumentType && p.FileName == fileName).FirstOrDefault();
                if (existData == null)
                {
                    return null;
                }
                else
                {
                    return existData.FileName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQVerifiedUploadCaseDocViewModel> GetCaseDocumentListByPersonalId(int PersonalRowID = 0, int SubCheckId = 0)
        {
            try
            {
                ClientCustomize CCObj = new ClientCustomize();
                List<PQVerifiedUploadCaseDocViewModel> lst = new List<PQVerifiedUploadCaseDocViewModel>();
                DataTable Dt = CCObj.GetPQVerifiedUploadCaseDoc(PersonalRowID, SubCheckId);
                lst = (from DataRow dr in Dt.Rows
                       select new PQVerifiedUploadCaseDocViewModel()
                       {
                           VerifiedUploadRowID = Convert.ToInt32(dr["CaseDocRowID"]),
                           PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                           DocumentUploadFrom = Convert.ToString(dr["DocumentUploadFrom"]),
                           DocumentType = Convert.ToString(dr["DocumentType"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Remarks = Convert.ToString(dr["Remarks"]),
                           UploadDate = Convert.ToDateTime(dr["UploadDate"]),
                           Status = Convert.ToByte(dr["Status"]),
                       }).ToList();
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQVerifiedUploadCaseDocViewModel> GetCaseDocumentList1ByPersonalId(int PersonalRowID = 0, int SubCheckId = 0, int ClientRowId = 0)
        {
            try
            {
                ClientCustomize CCObj = new ClientCustomize();
                List<PQVerifiedUploadCaseDocViewModel> lst = new List<PQVerifiedUploadCaseDocViewModel>();
                DataTable Dt = CCObj.GetPQVerifiedUploadCaseDoc(PersonalRowID, SubCheckId, ClientRowId);
                lst = (from DataRow dr in Dt.Rows
                       select new PQVerifiedUploadCaseDocViewModel()
                       {
                           VerifiedUploadRowID = Convert.ToInt32(dr["CaseDocRowID"]),
                           PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                           DocumentUploadFrom = Convert.ToString(dr["DocumentUploadFrom"]),
                           DocumentType = Convert.ToString(dr["DocumentType"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Remarks = Convert.ToString(dr["Remarks"]),
                           UploadDate = Convert.ToDateTime(dr["UploadDate"]),
                           Status = Convert.ToByte(dr["Status"]),
                       }).ToList();
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<PQVerifiedUploadCaseDocViewModel> GetCaseDocumentList(int PersonalRowID = 0)
        {
            try
            {
                ClientCustomize CCObj = new ClientCustomize();
                List<PQVerifiedUploadCaseDocViewModel> lst = new List<PQVerifiedUploadCaseDocViewModel>();
                DataTable Dt = CCObj.GetPQVerifiedUploadCaseDoc(PersonalRowID);
                lst = (from DataRow dr in Dt.Rows
                       select new PQVerifiedUploadCaseDocViewModel()
                       {
                           VerifiedUploadRowID = Convert.ToInt32(dr["CaseDocRowID"]),
                           PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                           DocumentUploadFrom = Convert.ToString(dr["DocumentUploadFrom"]),
                           DocumentType = Convert.ToString(dr["DocumentType"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Remarks = Convert.ToString(dr["Remarks"]),
                           UploadDate = Convert.ToDateTime(dr["UploadDate"]),
                           Status = Convert.ToByte(dr["Status"]),
                       }).ToList();
                return lst;
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
    }
}
