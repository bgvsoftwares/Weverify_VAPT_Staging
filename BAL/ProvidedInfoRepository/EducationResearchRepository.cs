using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public class EducationResearchRepository : IEducationResearchRepository
    {
        DataContext db;
        private const string SP_PARAM_RETURN_VALUE = "@ReturnValue";
        public EducationResearchRepository()
        {
            db = new DataContext();
        }

        public void AddEducationResearchInfo(AddPQEducationResearchViewModel model)
        {
            try
            {
                if (model != null)
                {
                    PQEducationResearch entity = new PQEducationResearch();
                    entity.EducationRowId = model.EducationRowId;
                    entity.OtherInstitute = model.OtherInstitute;
                    entity.AddedBy = model.AddedBy;
                    entity.AddedNameDesig = model.AddedNameDesig;
                    entity.AddedOn = model.AddedOn;
                    //entity.UpdatedBy = model.UpdatedBy;
                    //entity.UpdatedNameDesig = model.UpdatedNameDesig;
                    //entity.UpdatedOn = model.UpdatedOn;
                    entity.Status = model.Status;
                    db.PQEducationResearchs.Add(entity);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int SaveDetailForResearch(AddPQEducationResearchViewModel er)
        {
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "SaveDetailForResearch_Education",
            DAS.CreateParameter("@PersonalRowID", SqlDbType.BigInt, er.PersonlRowID, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowID", SqlDbType.Int, er.SubCheckRowID, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@EduResearchRowID", SqlDbType.Int, er.EduResearchRowID, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@otherInstitute", SqlDbType.VarChar, er.OtherInstitute, ParameterDirection.Input, 1000, false),
            DAS.CreateParameter("@createdBy", SqlDbType.Int, er.AddedBy, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@degreeID", SqlDbType.Int, er.DegreeId, ParameterDirection.Input, 10, false),
            objReturnValueParam
            );
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;
        }

        public int SaveandUpdateInsuffRemark(int PersonlRowID , int SubCheckRowID , string RemarkRaised, string InsuffRaisedDate , string check)
        {
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "Add_UpdateInsuffDateAndRemark",
            DAS.CreateParameter("@PersonalRowID", SqlDbType.BigInt, PersonlRowID, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowID", SqlDbType.Int, SubCheckRowID, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@Remark", SqlDbType.VarChar, RemarkRaised, ParameterDirection.Input, 1000, false),
            DAS.CreateParameter("@InsuffRaisedDate", SqlDbType.VarChar, InsuffRaisedDate, ParameterDirection.Input, 1000, false),
            DAS.CreateParameter("@Check", SqlDbType.VarChar, check, ParameterDirection.Input, 1000, false),

            objReturnValueParam
            );
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;
        }

        public PQEducationResearchListPagedModel GetEducationCheckListForResearch(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", string DCheckStatus = "", string CaseStatus = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEducationResearch> data = db.PQEducationResearchs.Include("PQEducation").Where(p => p.PQEducation.CheckStatus == "Research" && p.UpdatedBy==0);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingDate);
                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.ModifiedDate) == startDate.Date);
                }
                if (!string.IsNullOrEmpty(CompletedDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedDate);
                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) == startDate.Date);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEducation.PQPersonal.ClientRefID == Search);
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEducation.CheckStatus == DCheckStatus);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.EducationRowId) : data.OrderBy(d => d.EducationRowId);
                        break;
                }

                PQEducationResearchListPagedModel model = new PQEducationResearchListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQEducationResearchs = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQEducationResearchViewModel
                {
                    EduResearchRowID = item.EduResearchRowID,
                    EducationRowId = item.EducationRowId,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQEducation.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    Name = item.PQEducation.PQPersonal.CandTitle + " " + item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    CompanyRefNo=item.PQEducation.EV_Sec_Ref_Id,
                    OtherInstitute = item.OtherInstitute,
                    AddedBy = item.AddedBy,
                    AddedOn = item.AddedOn,
                    AddedNameDesig = item.AddedNameDesig,
                    Status = item.Status
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQEducationResearchListPagedModel GetEducationCheckListForResearch1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", string DCheckStatus = "", string CaseStatus = "",string SearchType="")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQEducationResearchListPagedModel model = new PQEducationResearchListPagedModel();
                ClientCustomize cc = new ClientCustomize();
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    if (Search == "")
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
                    dt = cc.getEducationResearchDetails(Search, ClientId,SearchType);
                }
                else
                {

                    dt = cc.getEducationResearchDetails();
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQEducationResearchViewModel> lmd = new List<PQEducationResearchViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {                     

                        PQEducationResearchViewModel mm = new PQEducationResearchViewModel();
                        mm.EduResearchRowID = Convert.ToInt32(dr["EduResearchRowID"]);
                        mm.EducationRowId = Convert.ToInt32(dr["EducationRowId"]);
                        mm.ClientName = (dr["ClientName"].ToString());
                        mm.Name = (dr["Name"].ToString());
                        mm.SubCheckName = (dr["SubCheckName"].ToString());
                        mm.CompanyRefNo = (dr["CompanyRefNo"].ToString());
                        mm.OtherInstitute = (dr["OtherInstitute"].ToString());
                        mm.AddedOn = Convert.ToDateTime(dr["AddedOn"]);
                        mm.AddedBy = Convert.ToInt16(dr["AddedBy"]);
                        mm.SubCheckRowId = Convert.ToInt32(dr["SubCheckRowID"]);
                        mm.PersonalRowId = Convert.ToInt32(dr["PersonalRowID"]);
                        mm.Status = byte.Parse(dr["Status"].ToString());
                        mm.DETM = (dr["DETM"].ToString());
                        mm.UniqueComponentID= (dr["UniqueComponentID"].ToString());
                        mm.TLAllocationDate = (dr["TLAllocationDate"].ToString());
                        mm.TMAllocationDate = (dr["TMAllocationDate"].ToString());
                        mm.CheckCompletionDate = (dr["CheckCompletionDate"].ToString());
                        lmd.Add(mm);
                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQEducationResearchs = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQUpdateEducationResearcherViewModel UpdateEducationResearchInfo(UpdatePQEducationResearchViewModel model, int EducationRowID = 0)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEducationResearchs.Single(c => c.EduResearchRowID == model.EduResearchRowID).UpdatedBy = model.UpdatedBy;
                            context.PQEducationResearchs.Single(c => c.EduResearchRowID == model.EduResearchRowID).UpdatedNameDesig = model.UpdatedNameDesig;
                            context.PQEducationResearchs.Single(c => c.EduResearchRowID == model.EduResearchRowID).UpdatedOn = model.UpdatedOn;
                            context.SaveChanges();
                        }
                        
                        context.PQEducations.Single(c => c.EducationRowId == EducationRowID).CheckStatus = "WIP-Pending";
                        context.PQEducations.Single(c => c.EducationRowId == EducationRowID).ModifiedBy = model.UpdatedBy;
                        context.PQEducations.Single(c => c.EducationRowId == EducationRowID).ModifiedDate = model.UpdatedOn;
                        context.SaveChanges();

                        int StatusId = context.PQEducationStatus.Where(c => c.EducationRowId == EducationRowID).FirstOrDefault().EducationStatusRowID;

                        context.PQEducationStatus.Single(c => c.EducationStatusRowID == StatusId).DETMCheckStatus = "WIP-Pending";
                        context.SaveChanges();
                        
                        transaction.Commit();

                        //Get More ids for check status and action history ang log save  
                        PQUpdateEducationResearcherViewModel emodel = new PQUpdateEducationResearcherViewModel();
                        var EduResearch = context.PQEducations.Where(c => c.EducationRowId == EducationRowID).FirstOrDefault();
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
    }
}
