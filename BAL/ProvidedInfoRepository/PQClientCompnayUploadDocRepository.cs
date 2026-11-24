using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public class PQClientCompnayUploadDocRepository : IPQClientCompnayUploadDocRepository
    {
        DataContext db;
        public PQClientCompnayUploadDocRepository()
        {
            db = new DataContext();
        }
        public void AddPQClientCompnayUploadDoc(ClientByBulkUploadViewModel model)
        {
            try
            {
                if (model != null)
                {
                    PQClientBulkUpload Entity = new PQClientBulkUpload();
                    Entity.ClientRowID = model.ClientRowID;
                    Entity.ExcelFileName = model.ExcelFileName;
                    Entity.AttachDocName = model.AttachDocName;
                    Entity.Status = model.Status;
                    Entity.UploadedBy = model.UploadedBy;
                    Entity.UploadedDate = model.UploadedDate;
                    db.PQClientBulkUploads.Add(Entity);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ClientByBulkUploadListPagedModel GetCompanyListForUpload(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string UploadfromDate = "", string UploadToDate = "",string CaseStatusId="")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                
                ClientByBulkUploadListPagedModel model = new ClientByBulkUploadListPagedModel();
                ClientCustomize CCObj = new ClientCustomize();
                List<ClientByBulkUploadListViewModel> lst = new List<ClientByBulkUploadListViewModel>();
                DataTable Dt = CCObj.getClientByBulkUploadList(ClientRowID.ToString(), CaseStatusId, Search, UploadfromDate, UploadToDate, pageNo.ToString());
                DataTable CountDT = CCObj.GetPQClientBulkUploadsCount(ClientRowID.ToString(), CaseStatusId, Search, UploadfromDate, UploadToDate, pageNo.ToString());
                model.PageSize = pageSize; 
                if (CountDT.Rows.Count>0)
                {
                    model.TotalRecords = Convert.ToInt32(CountDT.Rows[0]["Count"]);
                }
             
                lst = (from DataRow dr in Dt.Rows
                       select new ClientByBulkUploadListViewModel()
                       {
                           ClientBulkUploadRowID = Convert.ToInt32(dr["ClientBulkUploadRowID"]),
                           ClientRowId = Convert.ToInt16(dr["ClientRowId"]),
                           ClientName = Convert.ToString(dr["ClientName"]),
                           ExcelFileName = Convert.ToString(dr["ExcelFileName"]),
                           AttachDocName = Convert.ToString(dr["AttachDocName"]),
                           UploadedBy = Convert.ToString(dr["UploadedBy"]),
                           UploadedDate = Convert.ToDateTime(dr["UploadedDate"]),
                           IsInitiated= Convert.ToString(dr["CaseStatus"]),
                           MoveTo= Convert.ToString(dr["MoveTo"]),                        
                       }).ToList();
                model.PQClientCompany = lst;                 
                return model;
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
