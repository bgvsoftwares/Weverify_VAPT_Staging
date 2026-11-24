using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CandidateViewModel;

namespace BAL.CandidateRepository
{
    
    public class PQCandidateLoginRepository : IPQCandidateLoginRepository
    {
        DataContext db;
        ClientCustomize cc = new ClientCustomize();

        public PQCandidateLoginRepository()
        {
            db = new DataContext();
        }

        public void ActiveCandidateLogin(short id = 0, string checkeds = "")
        {
            try
            {
                if (id != 0 && checkeds != null)
                {
                    db.PQCandidateLogins.Single(b => b.CandidateUserRowID == id).Status = Convert.ToByte(checkeds);
                }
                else
                {
                    throw new Exception("Candidate User could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddCandidateLogin(AddPQCandidateLoginViewModel model)
        {
            try
            {
                if (model != null)
                {
                    PQCandidateLogin entity = new PQCandidateLogin();
                    entity.ClientRowID = model.ClientRowID;
                    entity.CandidateName = model.CandidateName;
                    entity.CandidateEmail = model.CandidateEmail;
                    entity.UserID = model.UserID;
                    entity.UPass = model.UPass;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedTime = model.CreatedTime;
                    entity.UserType = model.UserType;
                    entity.Status = model.Status;
                    entity.EmployeeID = model.EmployeeID;
                    entity.CantactNo = model.CantactNo;
                    entity.CreatedUserId = model.CreatedUserId;
                    entity.YTRApplicable = model.YTRApplicable;
                    entity.TempPersonalRowID = model.TempPersonalRowID;
                    entity.PersonalRowID = model.PersonalRowID;

                    db.PQCandidateLogins.Add(entity);
                }
                else
                {
                    throw new Exception("All * fields are mandatory!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteCandidateLoginSubGroups(short CandidateLoginID)
        {
            try
            {
                var entity = db.PQCandidateLogins.Find(CandidateLoginID);
                if (entity != null)
                {
                    db.PQCandidateLogins.Remove(entity);
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UpdatePQCandidateLoginViewModel GetCandidateLoginForPassUpdateById(int tempPersonalId = 0)
        {
            UpdatePQCandidateLoginViewModel model = new UpdatePQCandidateLoginViewModel();

            DataTable dUser = new DataTable();
            dUser = cc.getCandidatLoginDetails(tempPersonalId);
            //var entity = db.PQCandidateLogins.Where(c => c.TempPersonalRowID == PersonalId && c.Status == 1).FirstOrDefault();
            try
            {
                if (dUser.Rows.Count > 0)
                {
                    model.CandidateUserRowID = Convert.ToInt32(dUser.Rows[0]["CandidateUserRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dUser.Rows[0]["ClientRowID"].ToString());
                    model.CandidateName = Convert.ToString(dUser.Rows[0]["CandidateName"]);
                    model.CandidateEmail = Convert.ToString(dUser.Rows[0]["CandidateEmail"]);
                    model.UserID = Convert.ToString(dUser.Rows[0]["UserID"]);
                    model.UPass = Convert.ToString(dUser.Rows[0]["UPass"]);
                    model.CantactNo = Convert.ToString(dUser.Rows[0]["CantactNo"]);
                    model.UserType = Convert.ToByte(dUser.Rows[0]["UserType"].ToString());

                }
                else
                {
                    throw new Exception("Invalid Id!");
                }

                //var entity = db.PQCandidateLogins.Where(p => p.TempPersonalRowID == tempPersonalId).FirstOrDefault();
                //try
                //{
                //    if (entity != null)
                //    {
                //        model.CandidateUserRowID = entity.CandidateUserRowID;
                //        model.ClientRowID = entity.ClientRowID;
                //        model.CandidateName = entity.CandidateName;
                //        model.CandidateEmail = entity.CandidateEmail;
                //        model.UserID = entity.UserID;
                //        model.UPass = entity.UPass;
                //        model.UserType = entity.UserType;
                //        model.EmployeeID = entity.EmployeeID;
                //        model.CantactNo = !string.IsNullOrEmpty(entity.CantactNo) ? entity.CantactNo : entity.TempPQPersonal.MobileNo;
                //        model.YTRApplicable = entity.YTRApplicable;
                //    }
                //    else
                //    {
                //        throw new Exception("Invalid Id!");
                //    }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCandidatePasswordByID(UpdatePQCandidateLoginViewModel model)
        {

            ClientCustomize cc = new ClientCustomize();
            int k=cc.updateCandidateLoginExpiration(model.CandidateUserRowID, model.UPass,model.UserType);
            //try
            //{
            //    if (model != null && model.CandidateUserRowID > 0)
            //    {

            //        db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).UPass = model.UPass;
            //        db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).ModifiedTime = model.ModifiedTime;
            //        db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).UserType = model.UserType;
            //        db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).ModifiedTime = DateTime.Now;
            //        //db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).UserID = "1";
            //        db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).Status = 1;
            //    }
            //    else
            //    {
            //        throw new Exception("All * fields are mandatory!");
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public PQCandidateUserListPagedModel GetCandidateUsers(int pageNo, int pageSize, string sort, string sortDir, string Search = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQCandidateLogin> data = db.PQCandidateLogins;
                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => b.PQClientMaster.MasterAbbreviation.ClientName.ToString().Contains(Search) || b.CandidateName.ToString().Contains(Search) || b.UserID.ToString().Contains(Search) || b.CandidateEmail.ToString().Contains(Search) || b.CantactNo.ToString().Contains(Search));
                }

                switch (sort)
                {
                    case "clientname":
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQClientMaster.MasterAbbreviation.ClientName) : data.OrderByDescending(d => d.PQClientMaster.MasterAbbreviation.ClientName);
                        break;
                    case "userid":
                        data = sortDir == "asc" ? data.OrderBy(d => d.UserID) : data.OrderByDescending(d => d.UserID);
                        break;
                    case "modifiedtime":
                        data = sortDir == "asc" ? data.OrderBy(d => d.ModifiedTime != null ? d.ModifiedTime : d.CreatedTime) : data.OrderByDescending(d => d.ModifiedTime != null ? d.ModifiedTime : d.CreatedTime);
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.CreatedTime) : data.OrderByDescending(d => d.CreatedTime);
                        break;
                }

                PQCandidateUserListPagedModel model = new PQCandidateUserListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();

                model.CandidateUsers = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQCandidateLoginListViewModel
                {
                    CandidateUserRowID = item.CandidateUserRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateName = item.CandidateName,
                    CandidateEmail = item.CandidateEmail,
                    UserID = item.UserID,
                    CreatedBy = item.CreatedBy,
                    SentMailStatus = item.SentMailStatus,
                    SentMailDate = item.SentMailDate,
                    ModifiedTime = item.ModifiedTime != null ? item.ModifiedTime : item.CreatedTime,
                    Status = item.Status,

                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetClientAbbreviation(short ClientRowID = 0)
        {
            try
            {
                return db.PQClientMasters.Find(ClientRowID).MasterAbbreviation.ClientAbbreviation.ToUpper();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAllCandidateUserByClientId(short ClientRowID = 0)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                int j= cc.ClientTodayRegisteredCaseCount(ClientRowID);

                //int j = db.PQCandidateLogins.Where(c => c.ClientRowID == ClientRowID).ToList().Count();
                if (ClientRowID == 8)
                    j = j + 3;
                return j;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsCandidateLoginExist(string UserId, string CandidateEmail)
        {
            try
            {
                var CandidateUserID = db.PQCandidateLogins.Where(c => c.UserID.Trim().ToLower() == UserId.Trim().ToLower() && c.CandidateEmail.Trim().ToLower() == CandidateEmail.Trim().ToLower() && c.Status==1).FirstOrDefault();
                if (CandidateUserID != null && CandidateUserID.UserID.Length > 0)
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

        public bool IsCandidateLoginExist(string UserId, int Candidateid)
        {
            var CandidateUser = db.PQCandidateLogins.Where(c => c.UserID.Trim().ToLower() == UserId.Trim().ToLower()
             && c.CandidateUserRowID != Candidateid).FirstOrDefault();
            if (CandidateUser != null && CandidateUser.UserID.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
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

        public void UpdateCandidateLogin(UpdatePQCandidateLoginViewModel model)
        {
            try
            {
                if (model != null && model.CandidateUserRowID > 0)
                {
                    db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).ClientRowID = model.ClientRowID;
                    db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).CandidateName = model.CandidateName;
                    db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).UPass = model.UPass;
                    db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).ModifiedTime = model.ModifiedTime;
                    db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).UserType = model.UserType;
                    db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).EmployeeID = model.EmployeeID;
                    db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).CantactNo = model.CantactNo;
                    db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).CreatedUserId = model.CreatedUserId;
                    db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).YTRApplicable = model.YTRApplicable;
                }
                else
                {
                    throw new Exception("All * fields are mandatory!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQCandidateLoginForSendMailViewModel GetCandidateLoginDetailsForMailSendById(int PersonalId = 0)
        {
            PQCandidateLoginForSendMailViewModel model = new PQCandidateLoginForSendMailViewModel();
            
            DataTable dUser = new DataTable();
            dUser=cc.getCandidatLoginDetails(PersonalId);
            //var entity = db.PQCandidateLogins.Where(c => c.TempPersonalRowID == PersonalId && c.Status == 1).FirstOrDefault();
            try
            {
                if (dUser.Rows.Count>0)
                {
                    model.CandidateUserRowID = Convert.ToInt32(dUser.Rows[0]["CandidateUserRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dUser.Rows[0]["ClientRowID"].ToString());
                    model.CandidateName = Convert.ToString(dUser.Rows[0]["CandidateName"]);
                    model.CandidateEmail = Convert.ToString(dUser.Rows[0]["CandidateEmail"]);
                    model.UserID = Convert.ToString(dUser.Rows[0]["UserID"]);
                    model.UPass = Convert.ToString(dUser.Rows[0]["UPass"]);
                    //model.UserType = UserType;
                    //model.EmployeeID = entity.EmployeeID;
                    model.CantactNo = Convert.ToString(dUser.Rows[0]["CantactNo"]);
                    //model.YTRApplicable = entity.YTRApplicable;
                    model.SentMailStatus = Convert.ToInt16(dUser.Rows[0]["SentMailStatus"]);
                    //model.SentMailDate = Convert.ToString(dUser.Rows[0]["SentMailDate"]);
                    model.AlternateEmailID = Convert.ToString(dUser.Rows[0]["AlternateEmailID"]);

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

        public int GetCandidateLoginIDForMailSendById(string UserId = "", string Email = "")
        {
            try
            {
                int Id = 0;
                Id = db.PQCandidateLogins.Where(c => c.UserID == UserId && c.CandidateEmail == Email).FirstOrDefault().CandidateUserRowID;
                return Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCandidateLoginSendMailStatusById(UpdatePQCandidateLoginForSendMailViewModel model)
        {
            cc.updateCandidateMailSentStatus(model.CandidateUserRowID);
            //db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).SentMailStatus = model.SentMailStatus;
            //db.PQCandidateLogins.Single(c => c.CandidateUserRowID == model.CandidateUserRowID).SentMailDate = model.SentMailDate;
        }

        #region PQCandidateInsuffLogins 

        public AddPQCandidateLoginViewModel AddInsuffCandidateLogin(AddPQCandidateLoginViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            var candidate = context.PQPersonals.Find(model.PersonalRowID);
                            var CandidateUser = db.PQCandidateInsuffLogins.Where(c => c.UserID.Trim().ToLower() == model.UserID.Trim().ToLower() && c.PersonalRowID == model.PersonalRowID).FirstOrDefault();

                            if (CandidateUser == null)
                            {
                                PQCandidateInsuffLogin entity = new PQCandidateInsuffLogin();
                                entity.ClientRowID = model.ClientRowID;
                                entity.PersonalRowID = model.PersonalRowID;
                                entity.CandidateName = candidate.CandTitle + " " + candidate.Name + " " + candidate.MiddleName + " " + candidate.LastName;
                                entity.UserID = model.UserID;
                                entity.UPass = model.UPass;
                                entity.CreatedBy = model.CreatedBy;
                                entity.CreatedTime = model.CreatedTime;
                                entity.Status = model.Status;

                                context.PQCandidateInsuffLogins.Add(entity);
                                context.SaveChanges();

                                model.CandidateUserRowID = entity.CInsuffUserRowID;
                            }
                            else
                            {
                                context.PQCandidateInsuffLogins.Single(p => p.CInsuffUserRowID == CandidateUser.CInsuffUserRowID).Status = 1;
                                context.SaveChanges();
                                model.CandidateUserRowID = CandidateUser.CInsuffUserRowID;
                            }

                            transaction.Commit();
                            return model;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("All * fields are mandatory!");
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
        public AddPQCandidateLoginViewModelInfo AddInsuffCandidateLoginInfo(AddPQCandidateLoginViewModelInfo model)
        {
            AddPQCandidateLoginViewModelInfo objs = new AddPQCandidateLoginViewModelInfo();
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "usp_InsertPQCandidateInsuffLogins",
                DAS.CreateParameter("@ClientRowID", SqlDbType.Int, model.ClientRowID, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@PersonalRowID", SqlDbType.BigInt, model.PersonalRowID, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@CheckFamilyRowID", SqlDbType.Int, model.CheckFamilyRowID, ParameterDirection.Input, 100, false),
                DAS.CreateParameter("@SubCheckRowID", SqlDbType.Int, model.SubCheckRowID, ParameterDirection.Input, 100, false),
                DAS.CreateParameter("@CandidateName", SqlDbType.VarChar, model.CandidateName, ParameterDirection.Input, 100, false),
                DAS.CreateParameter("@UserID", SqlDbType.NVarChar, model.UserID, ParameterDirection.Input, 100, false),
                DAS.CreateParameter("@UPass", SqlDbType.NVarChar, model.UPass, ParameterDirection.Input, 100, false),
                DAS.CreateParameter("@CreatedBy", SqlDbType.VarChar, model.CreatedBy, ParameterDirection.Input, 100, false),
                DAS.CreateParameter("@Status", SqlDbType.Int, model.Status, ParameterDirection.Input, 100, false)
                );

                if (dt.Rows.Count > 0)
                {
                    objs = (from DataRow dr in dt.Rows
                            select new AddPQCandidateLoginViewModelInfo()
                            {
                                ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                                PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                                CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]),
                                SubCheckRowID = Convert.ToInt32(dr["SubCheckRowID"]),
                                CandidateName = Convert.ToString(dr["CandidateName"]),
                                UPass = Convert.ToString(dr["UPass"]),
                                UserID = Convert.ToString(dr["UserID"]),

                            }).Where(x => x.PersonalRowID == model.PersonalRowID && x.ClientRowID == model.ClientRowID && x.SubCheckRowID == model.SubCheckRowID).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
            return objs;
        }

        #endregion

    }
}
