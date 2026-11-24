using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.HRViewModels;
using ViewModels.ClientViewModel;
using ViewModels.CandidateViewModel;
using ViewModels.PartnerViewModels;
using System.Data;

namespace BAL.HRRepository
{
    public class AccountRepository : IAccountRepository
    {
        DataContext db;
        public AccountRepository()
        {
            db = new DataContext();
        }
        public CompanyUserLoggedViewModel GetWebUserLoggedDetails(string UserName, string EcryptedPassword)
        {
            CompanyUserLoggedViewModel LoggedUser = new CompanyUserLoggedViewModel();
            try
            {
                ClientCustomize cc = new ClientCustomize();

                DataTable DT = cc.GetWebUserLoggedDetails(UserName, EcryptedPassword);
                if (DT.Rows.Count > 0)
                { 
                    LoggedUser.WebUserRowID = Convert.ToInt16(DT.Rows[0]["WebUserRowID"].ToString());
                    LoggedUser.TeamMemberRoWID = Convert.ToInt16(DT.Rows[0]["TeamMemberRoWID"].ToString());
                    LoggedUser.TeamMemberName = Convert.ToString(DT.Rows[0]["TeamMemberName"].ToString());
                    LoggedUser.TeamMemberEmail = Convert.ToString(DT.Rows[0]["TeamMemberEmail"].ToString());
                    LoggedUser.CRPUserName = Convert.ToString(DT.Rows[0]["CRPUserName"].ToString());
                    LoggedUser.CRPPassWord = Convert.ToString(DT.Rows[0]["CRPPassWord"].ToString());
                    LoggedUser.encPassWord = Convert.ToString(DT.Rows[0]["encPassWord"].ToString());
                    LoggedUser.IsExportedExcel = Convert.ToByte(DT.Rows[0]["IsExportedExcel"].ToString());
                    LoggedUser.IsExportedWord = Convert.ToByte(DT.Rows[0]["IsExportedWord"].ToString());
                    LoggedUser.IsExportedPDF = Convert.ToByte(DT.Rows[0]["IsExportedPDF"].ToString());
                    LoggedUser.CreatedDate = Convert.ToDateTime(DT.Rows[0]["CreatedDate"].ToString());
                    LoggedUser.ModifiedDate = Convert.ToDateTime(DT.Rows[0]["ModifiedDate"].ToString());
                    LoggedUser.Status = Convert.ToByte(DT.Rows[0]["Status"].ToString());
                    LoggedUser.ExpiryDays = Convert.ToInt32(DT.Rows[0]["ExpiryDays"].ToString());
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //throw;
            }
            return LoggedUser;
        }


        public CompanyUserLoggedViewModel GetWebUserSignInDetails(string UserName, string EcryptedPassword)
        {
            CompanyUserLoggedViewModel LoggedUser = new CompanyUserLoggedViewModel();
            try
            {
                ClientCustomize cc = new ClientCustomize();

                DataTable DT = cc.GetWebUserSignInDetails(UserName, EcryptedPassword);
                if (DT.Rows.Count > 0)
                {
                    if (DT.Columns.Contains("Message") && DT.Rows[0]["Message"] != DBNull.Value)
                    {
                        // Return the message if it exists
                        LoggedUser.Message = DT.Rows[0]["Message"].ToString();
                    }
                    else
                    {
                        LoggedUser.WebUserRowID = Convert.ToInt16(DT.Rows[0]["WebUserRowID"].ToString());
                        LoggedUser.TeamMemberRoWID = Convert.ToInt16(DT.Rows[0]["TeamMemberRoWID"].ToString());
                        LoggedUser.TeamMemberName = Convert.ToString(DT.Rows[0]["TeamMemberName"].ToString());
                        LoggedUser.TeamMemberEmail = Convert.ToString(DT.Rows[0]["TeamMemberEmail"].ToString());
                        LoggedUser.CRPUserName = Convert.ToString(DT.Rows[0]["CRPUserName"].ToString());
                        LoggedUser.CRPPassWord = Convert.ToString(DT.Rows[0]["CRPPassWord"].ToString());
                        LoggedUser.encPassWord = Convert.ToString(DT.Rows[0]["encPassWord"].ToString());
                        LoggedUser.IsExportedExcel = Convert.ToByte(DT.Rows[0]["IsExportedExcel"].ToString());
                        LoggedUser.IsExportedWord = Convert.ToByte(DT.Rows[0]["IsExportedWord"].ToString());
                        LoggedUser.IsExportedPDF = Convert.ToByte(DT.Rows[0]["IsExportedPDF"].ToString());
                        LoggedUser.CreatedDate = Convert.ToDateTime(DT.Rows[0]["CreatedDate"].ToString());
                        LoggedUser.ModifiedDate = Convert.ToDateTime(DT.Rows[0]["ModifiedDate"].ToString());
                        LoggedUser.Status = Convert.ToByte(DT.Rows[0]["Status"].ToString());
                        LoggedUser.ExpiryDays = Convert.ToInt32(DT.Rows[0]["ExpiryDays"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //throw;
            }
            return LoggedUser;
        }


        public CompanyUserLoggedViewModel GetCompanyUserLoggedDetails(string UserName, string EcryptedPassword)
        {
            try
            {
                CompanyUserLoggedViewModel LoggedUser = new CompanyUserLoggedViewModel();

                LoggedUser = db.WebUsers.Where(w => w.CRPUserName.Trim() == UserName && w.CRPPassWord.Trim() == EcryptedPassword && w.Status == 1).Select(user => new CompanyUserLoggedViewModel
                {
                    WebUserRowID = user.WebUserRowID,
                    TeamMemberRoWID = user.TeamMemberRoWID,
                    TeamMemberName = (user.TeamMember.TMFirstName + " " + user.TeamMember.TMLastName).Trim(),
                    TeamMemberEmail = user.TeamMember.EmailID,
                    CRPUserName = user.CRPUserName.Trim(),
                    CRPPassWord = user.CRPPassWord.Trim(),
                    IsExportedExcel = user.IsExportedExcel,
                    IsExportedWord = user.IsExportedWord,
                    IsExportedPDF = user.IsExportedPDF,
                    CreatedDate = user.CreatedDate,
                    ModifiedDate = user.ModifiedDate,
                    Status = user.Status
                }).FirstOrDefault();

                return LoggedUser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CompanyLoginViewModel GetAllActiveWebUsers(string UserName)
        {
            try
            {
                CompanyLoginViewModel model = new CompanyLoginViewModel();
                ClientCustomize cc = new ClientCustomize();
                DataTable DT = cc.GetActiveWebUsers(UserName);
                if (DT.Rows.Count > 0)
                {

                    model.CRPUserName = Convert.ToString(DT.Rows[0]["CRPUserName"].ToString());
                    model.CRPPassWord = Convert.ToString(DT.Rows[0]["CRPPassWord"].ToString());
                    model.encPwd = Convert.ToString(DT.Rows[0]["encPassword"].ToString());
                }
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BlockedCompanyUser(UpdateCompanyUserBlockStatusViewModel model)
        {
            try
            {
                if (model != null)
                {
                    short WebUserRowID = db.WebUsers.Where(w => w.CRPUserName.Trim() == model.CRPUserName).FirstOrDefault().WebUserRowID;
                    if (WebUserRowID > 0)
                    {
                        db.WebUsers.Single(w => w.WebUserRowID == WebUserRowID).BlockedDate = model.BlockedDate;
                        db.WebUsers.Single(w => w.WebUserRowID == WebUserRowID).Status = model.Status;
                    }
                }
                else
                {
                    throw new Exception("Invalid block user details!");
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

        public int AddUserLoggedInDetails(AddUserLoggedInDetailViewModel model)
        {
            try
            {
                UserLoginLogOutDetail entity = new UserLoginLogOutDetail();
                entity.LoginName = model.LoginName;
                entity.LoginType = model.LoginType;
                entity.LoginId = model.LoginId;
                entity.LoginBy = model.LoginBy;
                entity.IPDetails = model.IPDetails;
                entity.LoginDate = model.LoginDate;
                entity.LoginTime = DateTime.Now;
                entity.LogoutTime = model.LogoutTime;
                entity.LoginStatus = model.LoginStatus;
                entity.Deviceidentifiers = model.Deviceidentifiers;
                entity.BrowserName = model.BrowserName;


                db.UserLoginLogOutDetails.Add(entity);
                db.SaveChanges();
                LoginDetail.LoginDetailRowID = entity.LoginDetailRowID;

                return entity.LoginDetailRowID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateUserLogOutDetails(UpdateUserLogOutDetailViewModel model)
        {
            try
            {
                db.UserLoginLogOutDetails.Single(l => l.LoginDetailRowID == model.LoginDetailRowID && l.LoginStatus == 1).LogoutTime = model.LogoutTime;
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ClientUserLoggedViewModel GetClientUserLoggedDetails(string UserName, string EcryptedPassword)
        {
            ClientUserLoggedViewModel model = new ClientUserLoggedViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                DataTable DT = cc.GetClientUserLoggedDetails(UserName, EcryptedPassword);
                if (DT.Rows.Count > 0)
                {
                    model.ClientUserRowID = Convert.ToInt32(DT.Rows[0]["ClientUserRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(DT.Rows[0]["ClientRowID"].ToString());
                    model.ClientName = DT.Rows[0]["ClientName"].ToString();
                    model.ClientBranch = DT.Rows[0]["ClientBranch"].ToString();
                    model.ClientSubGroupName = DT.Rows[0]["ClientSubGroupName"].ToString();
                    model.LocationName = DT.Rows[0]["LocationName"].ToString();
                    model.HRApprovalRequired = Convert.ToByte(DT.Rows[0]["HRApprovalRequired"].ToString());
                    model.Abbreviation = DT.Rows[0]["Abbreviation"].ToString();
                    model.CodeGeneration = Convert.ToByte(DT.Rows[0]["CodeGeneration"].ToString());
                    model.UserID = DT.Rows[0]["UserID"].ToString();
                    model.UPass = DT.Rows[0]["UPass"].ToString();
                    model.CSpocName = DT.Rows[0]["CSpocName"].ToString();
                    model.CSpocEmailID = DT.Rows[0]["CSpocEmailID"].ToString();
                    model.CreatedBy = DT.Rows[0]["CreatedBy"].ToString();
                    model.CreatedTime = Convert.ToDateTime(DT.Rows[0]["CreatedTime"].ToString());
                    //model.ModifiedTime = Convert.ToDateTime(DT.Rows[0]["ModifiedTime"].ToString());
                    model.Status = Convert.ToByte(DT.Rows[0]["Status"].ToString());
                    model.ClientAccessType = Convert.ToInt32(DT.Rows[0]["ClientAccessType"].ToString());
                    model.ExpiryDays = Convert.ToInt32(DT.Rows[0]["Expirydays"].ToString());
                    model.isMFA = Convert.ToInt32(DT.Rows[0]["isMFA"].ToString());
                    model.UType = Convert.ToInt32(DT.Rows[0]["UType"].ToString());
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ClientUserLoggedViewModel GetClientNGRUserLoggedDetails(string UserName, string UserKay)
        {
            ClientUserLoggedViewModel model = new ClientUserLoggedViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                DataTable DT = cc.GetClientNGRUserLoggedDetails(UserName, UserKay);
                if (DT.Rows.Count > 0)
                {
                    if (DT.Columns.Contains("Message") && DT.Rows[0]["Message"] != DBNull.Value)
                    {
                        model.Message = DT.Rows[0]["Message"].ToString();
                    }
                    else
                    {
                        model.ClientUserRowID = Convert.ToInt32(DT.Rows[0]["ClientUserRowID"].ToString());
                        model.ClientRowID = Convert.ToInt16(DT.Rows[0]["ClientRowID"].ToString());
                        model.ClientName = DT.Rows[0]["ClientName"].ToString();
                        model.ClientCode1 = DT.Rows[0]["ClientCode1"].ToString();
                        model.ClientBranch = DT.Rows[0]["ClientBranch"].ToString();
                        model.ClientSubGroupName = DT.Rows[0]["ClientSubGroupName"].ToString();
                        model.LocationName = DT.Rows[0]["LocationName"].ToString();
                        model.HRApprovalRequired = Convert.ToByte(DT.Rows[0]["HRApprovalRequired"].ToString());
                        model.Abbreviation = DT.Rows[0]["Abbreviation"].ToString();
                        model.CodeGeneration = Convert.ToByte(DT.Rows[0]["CodeGeneration"].ToString());
                        model.UserID = DT.Rows[0]["UserID"].ToString();
                        model.UPass = DT.Rows[0]["UPass"].ToString();
                        model.CSpocName = DT.Rows[0]["CSpocName"].ToString();
                        model.CSpocEmailID = DT.Rows[0]["CSpocEmailID"].ToString();
                        model.CreatedBy = DT.Rows[0]["CreatedBy"].ToString();
                        model.CreatedTime = Convert.ToDateTime(DT.Rows[0]["CreatedTime"].ToString());
                        //model.ModifiedTime = Convert.ToDateTime(DT.Rows[0]["ModifiedTime"].ToString());
                        model.Status = Convert.ToByte(DT.Rows[0]["Status"].ToString());
                        model.ClientAccessType = Convert.ToInt32(DT.Rows[0]["ClientAccessType"].ToString());
                        model.ExpiryDays = Convert.ToInt32(DT.Rows[0]["Expirydays"].ToString());
                        model.ValidUser = Convert.ToInt32(DT.Rows[0]["ValidUser"].ToString());
                        model.isMFA = Convert.ToInt32(DT.Rows[0]["isMFA"].ToString());
                        model.UType = Convert.ToInt32(DT.Rows[0]["UType"].ToString());
                    }
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ClientUserLoggedViewModel GetClientNGRUserLoggedDetails_API_SSO(string UserName)
        {
            ClientUserLoggedViewModel model = new ClientUserLoggedViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                DataTable DT = cc.GetClientNGRUserLoggedDetails_API_SSO(UserName);
                if (DT.Rows.Count > 0)
                {
                    if (DT.Columns.Contains("Message") && DT.Rows[0]["Message"] != DBNull.Value)
                    {
                        model.Message = DT.Rows[0]["Message"].ToString();
                    }
                    else
                    {
                        model.ClientUserRowID = Convert.ToInt32(DT.Rows[0]["ClientUserRowID"].ToString());
                        model.ClientRowID = Convert.ToInt16(DT.Rows[0]["ClientRowID"].ToString());
                        model.ClientName = DT.Rows[0]["ClientName"].ToString();
                        model.ClientCode1 = DT.Rows[0]["ClientCode1"].ToString();
                        model.ClientBranch = DT.Rows[0]["ClientBranch"].ToString();
                        model.ClientSubGroupName = DT.Rows[0]["ClientSubGroupName"].ToString();
                        model.LocationName = DT.Rows[0]["LocationName"].ToString();
                        model.HRApprovalRequired = Convert.ToByte(DT.Rows[0]["HRApprovalRequired"].ToString());
                        model.Abbreviation = DT.Rows[0]["Abbreviation"].ToString();
                        model.CodeGeneration = Convert.ToByte(DT.Rows[0]["CodeGeneration"].ToString());
                        model.UserID = DT.Rows[0]["UserID"].ToString();
                        model.UPass = DT.Rows[0]["UPass"].ToString();
                        model.CSpocName = DT.Rows[0]["CSpocName"].ToString();
                        model.CSpocEmailID = DT.Rows[0]["CSpocEmailID"].ToString();
                        model.CreatedBy = DT.Rows[0]["CreatedBy"].ToString();
                        model.CreatedTime = Convert.ToDateTime(DT.Rows[0]["CreatedTime"].ToString());
                        //model.ModifiedTime = Convert.ToDateTime(DT.Rows[0]["ModifiedTime"].ToString());
                        model.Status = Convert.ToByte(DT.Rows[0]["Status"].ToString());
                        model.ClientAccessType = Convert.ToInt32(DT.Rows[0]["ClientAccessType"].ToString());
                        model.ExpiryDays = Convert.ToInt32(DT.Rows[0]["Expirydays"].ToString());
                        model.ValidUser = Convert.ToInt32(DT.Rows[0]["ValidUser"].ToString());
                        model.isMFA = Convert.ToInt32(DT.Rows[0]["isMFA"].ToString());
                        model.UType = Convert.ToInt32(DT.Rows[0]["UType"].ToString());
                    }
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetLastLoginDetails(string UserName)
        {
            UserLoginLogOutDetail userLoginLogOutDetail = new UserLoginLogOutDetail();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                DataTable DT = cc.GetLastLoginDetails(UserName);
                if (DT.Rows.Count > 0)
                {
                    userLoginLogOutDetail.LoginTime = Convert.ToDateTime(DT.Rows[0]["LoginTime"].ToString());
                }

                return userLoginLogOutDetail.LoginTime.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ClientLoginViewModel GetAllActiveClientUsers(string UserName)
        {
            try
            {
                ClientLoginViewModel model = new ClientLoginViewModel();
                ClientCustomize cc = new ClientCustomize();
                DataTable DT = cc.GetActiveClientUsers(UserName);
                if (DT.Rows.Count > 0)
                {

                    model.CRPUserName = Convert.ToString(DT.Rows[0]["UserID"].ToString());
                    model.CRPPassWord = Convert.ToString(DT.Rows[0]["UPass"].ToString());
                    model.encPwd = Convert.ToString(DT.Rows[0]["encPassword"].ToString());
                }

                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ClientLoginViewModel> GetAllActiveClientUsers()
        {
            try
            {
                return db.PQClientLogins.Where(w => w.Status == 1).Select(w => new ClientLoginViewModel
                {
                    CRPUserName = w.UserID.Trim(),
                    CRPPassWord = w.UPass.Trim()
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BlockedClientUser(UpdateClientUserBlockStatusViewModel model)
        {
            try
            {
                if (model != null)
                {
                    int ClientUserRowID = db.PQClientLogins.Where(w => w.UserID.Trim() == model.UserID).FirstOrDefault().ClientUserRowID;
                    if (ClientUserRowID > 0)
                    {
                        db.PQClientLogins.Single(w => w.ClientUserRowID == ClientUserRowID).BlockedDate = model.BlockedDate;
                        db.PQClientLogins.Single(w => w.ClientUserRowID == ClientUserRowID).Status = model.Status;
                    }
                }
                else
                {
                    throw new Exception("Invalid block user details!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //For Candidate User Login

        public CandidateUserLoggedViewModel GetCandidateUserLoggedDetails(string UserName, string EcryptedPassword)
        {
            try
            {
                return db.PQCandidateLogins.Where(w => w.UserID.Trim() == UserName && w.UPass.Trim() == EcryptedPassword && w.UserType == 1 && w.isDeleted == 0).Select(user => new CandidateUserLoggedViewModel
                {
                    CandidateUserRowID = user.CandidateUserRowID,
                    ClientRowID = user.ClientRowID,
                    HRApprovalRequired = user.PQClientMaster.HRApprovalRequired,
                    CandidateName = user.CandidateName,
                    CandidateEmail = user.CandidateEmail,
                    UserID = user.UserID.Trim(),
                    UPass = user.UPass.Trim(),
                    CreatedBy = user.CreatedBy.Trim(),
                    CreatedTime = user.CreatedTime,
                    ModifiedTime = user.ModifiedTime,
                    Status = user.Status,
                    //YTRApplicable = user.YTRApplicable,
                    TempPersonalRowID = user.TempPersonalRowID
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CandidateUserLoggedViewModel GetInsuffCandidateUserLoggedDetails(string UserName, string EcryptedPassword)
        {
            try
            {
                return db.PQCandidateInsuffLogins.Where(w => w.UserID.Trim() == UserName && w.UPass.Trim() == EcryptedPassword && w.Status == 1).Select(user => new CandidateUserLoggedViewModel
                {
                    CandidateUserRowID = user.CInsuffUserRowID,
                    ClientRowID = user.ClientRowID,
                    HRApprovalRequired = user.PQClientMaster.HRApprovalRequired,
                    CandidateName = user.CandidateName,
                    CandidateEmail = user.UserID,
                    UserID = user.UserID.Trim(),
                    UPass = user.UPass.Trim(),
                    CreatedBy = user.CreatedBy.Trim(),
                    CreatedTime = user.CreatedTime,
                    ModifiedTime = user.ModifiedTime,
                    YTRApplicable = user.PQClientMaster.YTRStatus,
                    TempPersonalRowID = user.PersonalRowID
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateLoginViewModel> GetAllActiveCandidateUsers()
        {
            try
            {
                return db.PQCandidateLogins.Where(w => w.Status == 1).Select(w => new CandidateLoginViewModel
                {
                    CRPUserName = w.UserID.Trim(),
                    CRPPassWord = w.UPass.Trim()
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ExpireCandidateLogin(UpdateCandidateLoginExpireStatusViewModel model)
        {
            try
            {
                if (model != null)
                {
                    int CandidateUserRowID = db.PQCandidateLogins.Where(w => w.UserID.Trim() == model.UserID).FirstOrDefault().CandidateUserRowID;
                    if (CandidateUserRowID > 0)
                    {
                        db.PQCandidateLogins.Single(w => w.CandidateUserRowID == CandidateUserRowID).UserType = model.UserType;
                        db.PQCandidateLogins.Single(w => w.CandidateUserRowID == CandidateUserRowID).Status = model.Status;
                    }
                }
                else
                {
                    throw new Exception("Invalid block user details!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region ***** For Vendor User Login *****

        public VendorUserLoggedViewModel GetVendorUserLoggedDetails(string UserName, string EcryptedPassword)
        {
            try
            {
                var listUser = db.MasterVendorLogins.Where(w => w.UserID.Trim() == UserName && w.UPass.Trim() == EcryptedPassword && w.Status == 1).Select(user => new VendorUserLoggedViewModel
                {
                    VendorLoginRowID = user.VendorLoginRowID,
                    VendorRowID = user.VendorRowID,
                    VendorName = user.MasterVendor.VendorName.Trim(),
                    VendorEmaiID = user.MasterVendor.VendorEmaiID.Trim(),
                    ContactPerson = user.ContactPerson,
                    MobileNo = user.MobileNo,
                    UserID = user.UserID.Trim(),
                    UPass = user.UPass.Trim(),
                    UserType = user.UserType,
                    CreatedBy = user.CreatedBy.Trim(),
                    CreatedTime = user.CreatedTime,
                    ModifiedTime = user.ModifiedTime,
                    Status = user.Status
                }).FirstOrDefault();
                return listUser;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<VendorLoginViewModel> GetAllActiveVendorUsers()
        {
            try
            {
                return db.MasterVendorLogins.Where(w => w.Status == 1).Select(w => new VendorLoginViewModel
                {
                    CRPUserName = w.UserID.Trim(),
                    CRPPassWord = w.UPass.Trim()
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BlockedVendorUser(UpdateVendorUserBlockStatusViewModel model)
        {
            try
            {
                if (model != null)
                {
                    int VendorLoginRowID = db.MasterVendorLogins.Where(w => w.UserID.Trim() == model.UserID).FirstOrDefault().VendorLoginRowID;
                    if (VendorLoginRowID > 0)
                    {
                        db.MasterVendorLogins.Single(w => w.VendorLoginRowID == VendorLoginRowID).BlockedDate = model.BlockedDate;
                        db.MasterVendorLogins.Single(w => w.VendorLoginRowID == VendorLoginRowID).Status = model.Status;
                    }
                }
                else
                {
                    throw new Exception("Invalid block user details!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ************************ Doc Download LTI *********************
        public DataTable GetAPICurrentCheckStatus(int personalRowID, int subcheckrowid, string Checkname)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "ltiMindtree_CheckStatus_Fail_cases",
                 DAS.CreateParameter("@personalRowID", SqlDbType.Int, personalRowID, ParameterDirection.Input, 8, false),
                 DAS.CreateParameter("@subcheckrowid", SqlDbType.Int, subcheckrowid, ParameterDirection.Input, 8, false),
                DAS.CreateParameter("@Checkname", SqlDbType.VarChar, Checkname, ParameterDirection.Input, 8, false)
                );
            return dt;
        }

        public DataTable GetPersonalRowId(string CompanyRefNo)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetPersonalRowId",
                DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 200, false)
                );
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable GetCheckmappingdetails(string Checkname)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetCheckmappingdetails",
                DAS.CreateParameter("@Checkname", SqlDbType.VarChar, Checkname, ParameterDirection.Input, 5000, false)
                );
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion *******************Doc Download LTI************************

    }
}
