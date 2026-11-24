using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public class PQClientCandiBulkUploadRepository : IPQClientCandiBulkUploadRepository
    {
        DataContext db;
        public PQClientCandiBulkUploadRepository()
        {
            db = new DataContext();
        }

        public void AddPQClientCandiBulkUpload(AddPQClientCandiBulkUploadViewModel model)
        {
            try
            {
                if (model != null)
                {
                    PQClientCandidateBulkUpload entity = new PQClientCandidateBulkUpload();
                    entity.ClientRowID = model.ClientRowID;
                    entity.ClientCheckRowID = model.ClientCheckRowID;
                    entity.ClientPackageRowID = model.ClientPackageRowID;
                    entity.ExcelFileName = model.ExcelFileName;
                    entity.ImportBy = model.ImportBy;
                    entity.Remarks = model.Remarks;
                    entity.Status = model.Status;
                    db.PQClientCandidateBulkUploads.Add(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetNoOfExcelFileByClientID(short ClientId = 0)
        {
            try
            {
                return db.PQClientCandidateBulkUploads.Where(p => p.ClientRowID == ClientId).ToList().Count();
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
