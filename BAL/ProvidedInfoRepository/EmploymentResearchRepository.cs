using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using System.Data;
using System.Data.SqlClient;

namespace BAL.ProvidedInfoRepository
{
    public class EmploymentResearchRepository : IEmploymentResearchRepository
    {
        DataContext db;
        private const string SP_PARAM_RETURN_VALUE = "@ReturnValue";
        public EmploymentResearchRepository()
        {
            db = new DataContext();
        }

        public void AddEmploymentResearchInfo(AddPQEmploymentResearchViewModel model)
        {
            try
            {
                if (model != null)
                {
                    PQEmploymentResearch entity = new PQEmploymentResearch();
                    entity.EmploymentRowId = model.EmploymentRowId;
                    entity.OtherEmployer = model.OtherEmployer;
                    entity.AddedBy = model.AddedBy;
                    entity.AddedNameDesig = model.AddedNameDesig;
                    entity.AddedOn = model.AddedOn;
                    //entity.UpdatedBy = model.UpdatedBy;
                    //entity.UpdatedNameDesig = model.UpdatedNameDesig;
                    //entity.UpdatedOn = model.UpdatedOn;
                    entity.Status = model.Status;
                    db.PQEmploymentResearchs.Add(entity);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PQEmploymentResearchListPagedModel GetEmploymentCheckListForResearch(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEmploymentResearch> data = db.PQEmploymentResearchs.Include("PQEmployment").Where(p => p.PQEmployment.CheckStatus == "Research" && p.UpdatedBy == 0);
                
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }
                if (!string.IsNullOrEmpty(RecievingDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingDate);
                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.ModifiedDate) == startDate.Date);
                }
                if (!string.IsNullOrEmpty(CompletedDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedDate);
                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) == startDate.Date);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Contains(Search) || b.PQEmployment.PQPersonal.ClientRefID == Search);
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEmployment.CheckStatus == DCheckStatus);
                }
                //i wll ask sir how do this 
                //if (!string.IsNullOrEmpty(EmployerName))
                //{
                //    var EmployerId = db.MasterEmployers.Where(a => a.CompanyName == EmployerName).FirstOrDefault().CountryRowID;
                //    data = data.Where(b => b.PQEmployment.EV_OrganisationName == EmployerId.ToString());
                //}
                //if (!string.IsNullOrEmpty(Allocatedto))
                //{
                //    var TmId = db.TeamMembers.Where(t => t.TMFirstName == Allocatedto.Trim()).FirstOrDefault().TeamMemberRowID;
                //    data = data.Where(b => b.TLAllocatedTo == TmId);
                //}
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.EmploymentRowId) : data.OrderBy(d => d.EmploymentRowId);
                        break;
                }

                PQEmploymentResearchListPagedModel model = new PQEmploymentResearchListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQEmploymentResearchs = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQEmploymentResearchViewModel
                {
                    EmpResearchRowID = item.EmpResearchRowID,
                    EmploymentRowId = item.EmploymentRowId,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQEmployment.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    Name = item.PQEmployment.PQPersonal.CandTitle + " " + item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OtherEmployer = item.OtherEmployer,
                    AddedOn = item.AddedOn,
                    //AddedBy = item.AddedBy,
                    //AddedNameDesig = item.AddedNameDesig,
                    //Status = item.Status
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQEmploymentResearchListPagedModel GetEmploymentCheckListForResearch1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQEmploymentResearchListPagedModel model = new PQEmploymentResearchListPagedModel();
                ClientCustomize cc = new ClientCustomize();
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    if(Search=="")
                    {
                        Search = null;
                    }
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getEmployementResearchDetails(Search, ClientId);
                }
                else
                {

                    dt = cc.getEmployementResearchDetails();
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQEmploymentResearchViewModel> lmd = new List<PQEmploymentResearchViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQEmploymentResearchViewModel mm = new PQEmploymentResearchViewModel();
                        //mm.EmpResearchRowID =  Convert.ToInt32(dr["EmpResearchRowID"]);
                        //mm.EmploymentRowId = Convert.ToInt32(dr["EmploymentRowId"]);
                        mm.CandCheckRowID = Convert.ToInt32(dr["CandCheckRowID"]);
                        mm.ClientName = (dr["ClientName"].ToString());
                        mm.Name =  (dr["Name"].ToString());
                        mm.SubCheckName = (dr["SubCheckName"].ToString());
                        mm.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        mm.CompanyRefNo = (dr["CompanyRefNo"].ToString());
                        mm.OtherEmployer = (dr["OtherEmployer"].ToString());
                        mm.AddedOn = Convert.ToDateTime(dr["AddedOn"]);
                        mm.SubCheckRowId=Convert.ToInt32(dr["SubCheckRowID"]);
                        mm.PersonalRowId = Convert.ToInt32(dr["PersonalRowID"]);
                        lmd.Add(mm);
                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }               
                model.PageSize = pageSize;
                model.PQEmploymentResearchs = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQUpdateEmploymentResearcherViewModel UpdateEmploymentResearchInfo(UpdatePQEmploymentResearchViewModel model, int EmploymentRowID = 0)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmploymentResearchs.Single(c => c.EmpResearchRowID == model.EmpResearchRowID).UpdatedBy = model.UpdatedBy;
                            context.PQEmploymentResearchs.Single(c => c.EmpResearchRowID == model.EmpResearchRowID).UpdatedNameDesig = model.UpdatedNameDesig;
                            context.PQEmploymentResearchs.Single(c => c.EmpResearchRowID == model.EmpResearchRowID).UpdatedOn = model.UpdatedOn;
                            context.SaveChanges();
                        }

                        context.PQEmployments.Single(c => c.EmploymentRowId == EmploymentRowID).CheckStatus = "WIP-Pending";
                        context.PQEmployments.Single(c => c.EmploymentRowId == EmploymentRowID).ModifiedBy = model.UpdatedBy;
                        context.PQEmployments.Single(c => c.EmploymentRowId == EmploymentRowID).ModifiedDate = model.UpdatedOn;
                        context.SaveChanges();

                        int StatusId = context.PQEmploymentStatus.Where(c => c.EmploymentRowId == EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                        context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == StatusId).DETMCheckStatus = "WIP-Pending";
                        context.SaveChanges();

                        transaction.Commit();

                        //Get More ids for check status and action history ang log save  
                        PQUpdateEmploymentResearcherViewModel emodel = new PQUpdateEmploymentResearcherViewModel();
                        var EduResearch = context.PQEmployments.Where(c => c.EmploymentRowId == EmploymentRowID).FirstOrDefault();
                        emodel.PersonalRowID = EduResearch.PersonalRowID;
                        emodel.SubCheckRowID = EduResearch.SubCheckRowID;
                        emodel.UniqueComponentID = EduResearch.UniqueComponentID;
                        return emodel;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
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
       
        public IEnumerable<UploadResearchDoc> UploadResearchDoc( int personalRowID, int subCheckRowID)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                PQEmploymentResearchListPagedModel model = new PQEmploymentResearchListPagedModel();
                DataTable dt = new DataTable();
                dt = cc.getEmployementResearchDetails( personalRowID, subCheckRowID);
                //return new DashboardManagerViewModel
                //{
               
                List<UploadResearchDoc> lmd = new List<UploadResearchDoc>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        UploadResearchDoc mm = new UploadResearchDoc();
                        mm.VerifiedUploadRowID = Convert.ToInt32(dr["VerifiedUploadRowID"].ToString());
                        mm.SubCheckName = (dr["SubCheckName"].ToString());
                        mm.DocumentUploadFrom = dr["DocumentUploadFrom"].ToString();
                        mm.DocumentType = (dr["DocumentType"].ToString());
                        mm.FileName = (dr["FileName"].ToString());
                        mm.FilePath = (dr["FilePath"].ToString());
                        lmd.Add(mm);
                    }
                    model.TotalRecords = dt.Rows.Count;
                    model.UploadResearchDocList = lmd;
                   
                }
                return model.UploadResearchDocList;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SaveDetailForResearch(AddPQEmploymentResearchViewModel er)
        {
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "SaveDetailForResearch_Employment",
            DAS.CreateParameter("@PersonalRowID", SqlDbType.BigInt, er.PersonlRowID, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowID", SqlDbType.Int, er.SubCheckRowID, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@EmpResearchRowID", SqlDbType.VarChar, er.EmpResearchRowID, ParameterDirection.Input, 1000, false),
            DAS.CreateParameter("@otherEmployer", SqlDbType.VarChar, er.OtherEmployer, ParameterDirection.Input, 1000, false),
            DAS.CreateParameter("@createdBy", SqlDbType.Int, er.AddedBy, ParameterDirection.Input, 10, false),
            objReturnValueParam
            );
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;
        }

    }
}
