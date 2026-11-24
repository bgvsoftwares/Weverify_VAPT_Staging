using BAL;
using BAL.CandidateRepository;
using BAL.ClientRepository;
using BAL.HRRepository;
using BAL.PartnerRepository;
using BAL.ProvidedInfoRepository;
using BAL.VerificationRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ViewModels.CandidateViewModel;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mime;
using System.Net;
using System.Text;
using GoogleMaps.LocationServices;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI;
using System.ComponentModel;
using System.Reflection;
using Renci.SshNet;
using ViewModels.ReportQCViewModel;
using BAL.ReportQCRepository;
using System.Dynamic;

namespace WebAppBGV.Controllers
{
    [CustomAuthorize]
    public class VerificationController : Controller
    {
        private const string host = "sftp.sutherlandglobal.com";
        private const int port = 22;
        private const string username = "EDMSBGV_BGV.Process";
        private const string password = "*z4Rvkqtc-!@Fr";
        private const string Source = "/Reports";

        #region ******  Repository Objects ******

        IDropDownMasterRepository repoDDL;
        IPQPersonalRepository repoPQPersonal;
        IPQClientRepository repoPQClient;
        IAbbreviationRepository repoAbbreviation;

        IVerificationInfoRepository repoVerificationInfo;

        IClientPackageRepository repoClientPackage;
        IClientCheckRepository repoClientCheck;
        IAntecedentRepository repoAntecedent;
        IPQLogTrasactionRepository repoPQLogTrasaction;
        IPQCandidateCheckRepository repoPQCandidateCheck;
        ITeamMemberRepository repoTeamMember;
        IPQClientTMemberRepository repoPQClientTMember;
        IAddressInfoRepository repoAddressInfo;
        IEmploymentInfoRepository repoEmploymentInfo;
        IEducationInfoRepository repoEducationInfo;
        ICriminalInfoRepository repoCriminalInfo;
        INationalIdentityInfoRepository repoNationalIdentInfo;
        IReferenceInfoRepository repoReferenceInfo;
        ISubCheckFamilyRepository repoSubCheckFamily;
        IPQVerifiedUploadDocRepository repoPQVerifiedUploadDoc;
        IPQClientCompnayUploadDocRepository repoClientCmpUplod;
        IPQAddressVerRepository repoAddressVer;
        IDashboardVerificationRepository repoDashboard;
        IPQEmploymentVerRepository repoEmploymentVer;
        IPQEducationVerRepository repoEducationVer;
        IPQCriminalVerRepository repoCriminalVer;
        IPQReferenceVerRepository repoReferenceVer;
        IPQNationalIdentityVerRepository repoNationalIdentityVer;
        IEmailVerificationRepository repoEmailVerification;
        ITeamDepartmentRepository repoTeamDepartment;
        IPartnerVerificationRepository repoPartnerVerification;
        IPQCandidateLoginRepository repoPQCandidateLogin;
        ISpecialVerificationRepository repoSplChkVer;
        IPQVerifiedUploadCaseDocRepository repoPQVerifiedUploadCaseDoc;
        ICollegeRepository repoCollege;
        ICompanyRepository repoCompany;
        IReportQCRepository repoReportQC;
        public VerificationController(
            IDropDownMasterRepository _repoDDL
            , IPQPersonalRepository _repoPQPersonal
            , IPQClientRepository _repoPQClient
            , IAbbreviationRepository _repoAbbreviation
             , IVerificationInfoRepository _repoVerificationInfo
            , IClientPackageRepository _repoClientPackage
            , IClientCheckRepository _repoClientCheck
            , IAntecedentRepository _repoAntecedent
            , IPQLogTrasactionRepository _repoPQLogTrasaction
            , IPQCandidateCheckRepository _repoPQCandidateCheck
            , ITeamMemberRepository _repoTeamMember
            , IPQClientTMemberRepository _repoPQClientTMember
            , IAddressInfoRepository _repoAddressInfo
            , IEmploymentInfoRepository _repoEmploymentInfo
            , IEducationInfoRepository _repoEducationInfo
            , ICriminalInfoRepository _repoCriminalInfo
            , INationalIdentityInfoRepository _repoNationalIdentInfo
            , IReferenceInfoRepository _repoReferenceInfo
            , ISubCheckFamilyRepository _repoSubCheckFamily
            , IPQVerifiedUploadDocRepository _repoPQVerifiedUploadDoc
            , IPQClientCompnayUploadDocRepository _repoClientCmpUplod
            , IPQAddressVerRepository _repoAddressVer
            , IDashboardVerificationRepository _repoDashboard
            , IPQEmploymentVerRepository _repoEmploymentVer
            , IPQEducationVerRepository _repoEducationVer
            , IPQCriminalVerRepository _repoCriminalVer
            , IPQReferenceVerRepository _repoReferenceVer
            , IPQNationalIdentityVerRepository _repoNationalIdentityVer
            , IEmailVerificationRepository _repoEmailVerification
            , ITeamDepartmentRepository _repoTeamDepartment
            , IPartnerVerificationRepository _repoPartnerVerification
            , IPQCandidateLoginRepository _repoPQCandidateLogin
            , ISpecialVerificationRepository _repoSplChkVer
            , IPQVerifiedUploadCaseDocRepository _repoPQVerifiedUploadCaseDoc
             , ICollegeRepository _repoCollege
            , ICompanyRepository _repoCompany
            , IReportQCRepository _repoReportQC
            )
        {
            repoDDL = _repoDDL;
            repoPQPersonal = _repoPQPersonal;
            repoPQClient = _repoPQClient;
            repoAbbreviation = _repoAbbreviation;
            repoVerificationInfo = _repoVerificationInfo;
            repoClientPackage = _repoClientPackage;
            repoClientCheck = _repoClientCheck;
            repoAntecedent = _repoAntecedent;
            repoPQLogTrasaction = _repoPQLogTrasaction;
            repoPQCandidateCheck = _repoPQCandidateCheck;
            repoTeamMember = _repoTeamMember;
            repoPQClientTMember = _repoPQClientTMember;
            repoAddressInfo = _repoAddressInfo;
            repoEmploymentInfo = _repoEmploymentInfo;
            repoEducationInfo = _repoEducationInfo;
            repoCriminalInfo = _repoCriminalInfo;
            repoNationalIdentInfo = _repoNationalIdentInfo;
            repoReferenceInfo = _repoReferenceInfo;
            repoSubCheckFamily = _repoSubCheckFamily;
            repoPQVerifiedUploadDoc = _repoPQVerifiedUploadDoc;
            repoClientCmpUplod = _repoClientCmpUplod;
            repoAddressVer = _repoAddressVer;
            repoDashboard = _repoDashboard;
            repoEmploymentVer = _repoEmploymentVer;
            repoEducationVer = _repoEducationVer;
            repoCriminalVer = _repoCriminalVer;
            repoReferenceVer = _repoReferenceVer;
            repoNationalIdentityVer = _repoNationalIdentityVer;
            repoEmailVerification = _repoEmailVerification;
            repoTeamDepartment = _repoTeamDepartment;
            repoPartnerVerification = _repoPartnerVerification;
            repoPQCandidateLogin = _repoPQCandidateLogin;
            repoSplChkVer = _repoSplChkVer;
            repoPQVerifiedUploadCaseDoc = _repoPQVerifiedUploadCaseDoc;
            repoCollege = _repoCollege;
            repoCompany = _repoCompany;
            repoReportQC = _repoReportQC;
        }

        #endregion

        ClientCustomize cc = new ClientCustomize();

        // GET: Verification
        [SessionExpireFilterAttribute]
        public ActionResult Index()
        {
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                int DepartmentId = Convert.ToInt32(Session["DepartmentRowId"].ToString());

                //Department Manager
                bool flagMgr = false;
                if (Designation == "Manager-Add")
                {
                    flagMgr = true;
                    //CreateNavigationforInsuffCaseForMAAdd();
                    ViewBag.Dashboard = "Address Verification Manager Dashboard!";
                }
                else if (Designation == "Manager-Emp")
                {
                    flagMgr = true;
                    ViewBag.Dashboard = "Employment Verification Manager Dashboard!";
                }
                else if (Designation == "Manager-Edu")
                {
                    flagMgr = true;
                    ViewBag.Dashboard = "Education Verification Manager Dashboard!";
                }
                else if (Designation == "Manager-Cri")
                {
                    flagMgr = true;
                   // CreateNavigationforInsuffCaseForMAAdd();
                    ViewBag.Dashboard = "Criminal Verification Manager Dashboard!";
                }
                else if (Designation == "Manager-Ref")
                {
                    flagMgr = true;
                    ViewBag.Dashboard = "Reference Verification Manager Dashboard!";
                }
                else if (Designation == "Manager-Ide")
                {
                    flagMgr = true;
                    ViewBag.Dashboard = "National Identity Verification Manager Dashboard!";
                }
                else if (Designation == "Manager-SPL")
                {
                    flagMgr = true;
                    //CreateNavigationforInsuffCaseForMAAdd();
                    ViewBag.Dashboard = "Special Check Verification Manager Dashboard!";
                }

                if (flagMgr)
                {
                    ViewBag.ManagerDashboard = repoDashboard.GetManagerDashBoardInfo(teamMemerRowId, Designation);
                }

                //Department TeamLeader
                bool flagTL = false;
                if (Designation == "Team Leader-Add")
                {
                    flagTL = true;
                    ViewBag.Dashboard = "Address Verification TeamLeader Dashboard!";
                }
                else if (Designation == "Team Leader-Emp")
                {
                    flagTL = true;
                    ViewBag.Dashboard = "Employment Verification TeamLeader Dashboard!";
                }
                else if (Designation == "Team Leader-Edu")
                {
                    flagTL = true;
                    ViewBag.Dashboard = "Education Verification TeamLeader Dashboard!";
                }
                else if (Designation == "Team Leader-Cri")
                {
                    flagTL = true;
                    ViewBag.Dashboard = "Criminal Verification TeamLeader Dashboard!";
                }
                else if (Designation == "Team Leader-Ref")
                {
                    flagTL = true;
                    ViewBag.Dashboard = "Reference Verification TeamLeader Dashboard!";
                }
                else if (Designation == "Team Leader-Ide")
                {
                    flagTL = true;
                    ViewBag.Dashboard = "National Identity Verification TeamLeader Dashboard!";
                }
                else if (Designation == "Team Leader-SPL")
                {
                    flagTL = true;
                    ViewBag.Dashboard = "Special Check Verification TeamLeader Dashboard!";
                }

                if (flagTL)
                {
                    ViewBag.TeamLeaderDashboard = repoDashboard.GetTeamLeaderDashBoardInfo(teamMemerRowId, Designation, DepartmentId);
                }

                //Department TeamMember
                bool flagTM = false;
                if (Designation == "Team Member-Add")
                {
                    flagTM = true;
                    ViewBag.Dashboard = "Address Verification TeamMember Dashboard!";
                }
                else if (Designation == "Team Member-Emp")
                {
                    flagTM = true;
                    ViewBag.Dashboard = "Employment Verification TeamMember Dashboard!";
                }
                else if (Designation == "Team Member-Edu")
                {
                    flagTM = true;
                    ViewBag.Dashboard = "Education Verification TeamMember Dashboard!";
                }
                else if (Designation == "Team Member-Cri")
                {
                    flagTM = true;
                    ViewBag.Dashboard = "Criminal Verification TeamMember Dashboard!";
                }
                else if (Designation == "Team Member-Ref")
                {
                    flagTM = true;
                    ViewBag.Dashboard = "Reference Verification TeamMember Dashboard!";
                }
                else if (Designation == "Team Member-Ide")
                {
                    flagTM = true;
                    ViewBag.Dashboard = "National Identity Verification TeamMember Dashboard!";
                }
                else if (Designation == "Team Member-SPL")
                {
                    flagTM = true;
                    ViewBag.Dashboard = "Special Check Verification TeamMember Dashboard!";
                }

                else if (Designation == "Auto Revert-QC")
                {
                    flagTM = true;
                    ViewBag.Dashboard = "Automation Dashboard!";
                }
                if (flagTM)
                {
                    ViewBag.TeamMemberDashboard = repoDashboard.GetTeamMemberDashBoardInfo(teamMemerRowId, Designation, DepartmentId);
                }

                //Department TeamMemberQC
                bool flagTMQC = false;
                if (Designation == "Team Member-Add-QC")
                {
                    flagTMQC = true;
                    ViewBag.Dashboard = "Address Verification TeamMemberQC Dashboard!";
                }
                else if (Designation == "Team Member-Emp-QC")
                {
                    flagTMQC = true;
                    ViewBag.Dashboard = "Employment Verification TeamMemberQC Dashboard!";
                }
                else if (Designation == "Team Member-Edu-QC")
                {
                    flagTMQC = true;
                    ViewBag.Dashboard = "Education Verification TeamMemberQC Dashboard!";
                }
                else if (Designation == "Team Member-Cri-QC")
                {
                    flagTMQC = true;
                    ViewBag.Dashboard = "Criminal Verification TeamMemberQC Dashboard!";
                }
                else if (Designation == "Team Member-Ref-QC")
                {
                    flagTMQC = true;
                    ViewBag.Dashboard = "Reference Verification TeamMemberQC Dashboard!";
                }
                else if (Designation == "Team Member-Ide-QC")
                {
                    flagTMQC = true;
                    ViewBag.Dashboard = "National Identity Verification TeamMemberQC Dashboard!";
                }
                else if (Designation == "Team Member-SPL-QC")
                {
                    flagTMQC = true;
                    ViewBag.Dashboard = "Special Check Verification TeamMemberQC Dashboard!";
                }               

                if (flagTMQC)
                {
                    ViewBag.TeamMemberQCDashboard = repoDashboard.GetTeamMemberQCDashBoardInfo(teamMemerRowId, Designation,DepartmentId);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View();
        }

       

        [NonAction]
        private void CreateNavigationforInsuffCaseForMAAdd()
        {
            short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
            int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
            string CreatedBy = Convert.ToString(Session["CreatedBy"]);
            PQAllChecksCountViewModel AllChecks;
            if (!string.IsNullOrEmpty(CreatedBy) && CreatedBy == "Client-User")
            {
                AllChecks = repoPQPersonal.GetAllInsuffChecksCountForClient1(ClientRowID, ClientUserRowID);
            }
            else
            {
                AllChecks = repoPQPersonal.GetAllInsuffChecksCountForClient1(ClientRowID, 0);
            }

            string NavTeamMemberTL = string.Empty;
            NavTeamMemberTL += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-51px;'><span class='count_top' style='margin-right:38px'><b>Address</b></span><div class='count' style='margin-right:41px'> " + AllChecks.AddressCount + "</div></div>";
            NavTeamMemberTL += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>Employment</b></span><div class='count' style='margin-right:41px'> " + AllChecks.EmploymentCount + " </div></div>";
            NavTeamMemberTL += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>Education</b></span><div class='count' style='margin-right:41px'> " + AllChecks.EducationCount + " </div></div>";
            NavTeamMemberTL += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>Criminal</b></span><div class='count' style='margin-right:41px'> " + AllChecks.CriminalCount + "</div></div>";
            NavTeamMemberTL += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>National Identity</b></span><div class='count' style='margin-right:41px'> " + AllChecks.NatIdentityCount + "</div></div>";
            NavTeamMemberTL += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>Reference</b></span><div class='count' style='margin-right:41px'> " + AllChecks.ReferenceCount + "</div></div>";
            NavTeamMemberTL += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>Special Check</b></span><div class='count' style='margin-right:41px'> " + AllChecks.SpecialcheckCount + "</div></div>";
            Session["NavLeftInsuffMenuForMAAdd"] = NavTeamMemberTL;
        }


      

        #region ******** Department Allocation Management ********

        [SessionExpireFilterAttribute]
        public ActionResult DepartmentManagerAllocation(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string  version="New", short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0,int CheckStatusId=0)
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                string department = "";
                string statusid = "";
                if (CheckStatus == "Allocated" && Designation == "Manager-Emp")
                {
                    statusid = "18";
                }

                else if (CheckStatus == "Review" && Designation == "Manager-Emp")
                {
                    statusid = "17";
                }
                else if (CheckStatus == "Review" && Designation == "Manager-Add")
                {
                    statusid = "17";
                }

                else if (CheckStatus == "Review" && Designation == "Manager-Ref")
                {
                    statusid = "17";
                }

                else if (CheckStatus == "Review" && Designation == "Manager-Ide")
                {
                    statusid = "17";
                }

                else if (CheckStatus == "Review" && Designation == "Manager-Cri")
                {
                    statusid = "17";
                }
                else if (CheckStatus == "Review" && Designation == "Manager-SPL")
                {
                    statusid = "17";
                }


                else if (CheckStatus == "Allocated" && Designation == "Manager-Add")
                {
                    statusid = "18,27,38,62,63";
                }

                else if(CheckStatus == "Allocated" && Designation == "Manager-Ref")
                {
                    statusid = "18";
                }
                else if(CheckStatus == "Allocated" && Designation == "Manager-SPL")
                {
                    statusid = "18";
                }

                else if(CheckStatus == "Allocated" && Designation == "Manager-Cri")
                {
                    statusid = "18";
                }
                else if(CheckStatus == "Allocated" && Designation == "Manager-Ide")
                {
                    statusid = "18";
                }

                else if(CheckStatus == "Allocated" && Designation == "Manager-Edu")
                {
                    statusid = "18";
                }
                else if (CheckStatus == "Review" && Designation == "Manager-Edu")
                {
                    statusid = "17";
                }

                else if(CheckStatus == "WIP" && Designation == "Manager-Add")
                {
                    statusid = "27,62,63,64";
                }
                else if (CheckStatus == "Closed" && Designation == "Manager-Add")
                {
                    statusid = "36,37";
                }
                else if (CheckStatus == "Closed" && Designation == "Manager-Cri")
                {
                    statusid = "36,37";
                }
                else if (CheckStatus == "Closed" && Designation == "Manager-Ide")
                {
                    statusid = "36,37";
                }
                else if (CheckStatus == "Closed" && Designation == "Manager-SPL")
                {
                    statusid = "36,37";
                }
                else if (CheckStatus == "Closed" && Designation == "Manager-Edu")
                {
                    statusid = "36,37";
                }
                else if (CheckStatus == "Closed" && Designation == "Manager-Emp")
                {
                    statusid = "36,37";
                }
                else if (CheckStatus == "Closed" && Designation == "Manager-Ref")
                {
                    statusid = "36,37";
                }
                else if (CheckStatus == "Reverification" )
                {
                    statusid = "16";
                }
                else
                {
                    statusid = CheckStatusId.ToString();

                }


                //else if (CheckStatus == "Closed" && Designation == "Manager-Emp")
                //{
                //    statusid = "36";
                //}
                //else if (CheckStatus == "WIP" && Designation == "Manager-Emp")
                //{
                //    statusid = "16";
                //}

                BindClientListBaseOnLogin(teamMemerRowId);
                CaseStatusOptionDropDown();
                CheckStatusOptionDropDown();
                if (Designation == "Manager-Add")
                {
                    BindCities();
                    BindStates();
                    BindPartneres();
                    if(version=="New")
                    {
                        department = "Address";
                        model = repoVerificationInfo.GetCandidateAddressListByDepartmentManager1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, State, City, Partner);
                        if (CheckStatus == "Reverification")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count;

                        }

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateAddressListByDepartmentManager(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, State, City, Partner);

                    }
                }
                else if (Designation == "Manager-Emp")
                {                    
                    if(version=="New")
                    {
                        department = "Employment";
                        model = repoVerificationInfo.GetCandidateEmploymentListByDepartmentManager1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,  department,  statusid , teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);
                        if(CheckStatus== "Reverification")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count;

                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEmploymentListByDepartmentManager(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);

                    }
                }
                else if (Designation == "Manager-Edu")
                {
                    if(version=="New")
                    {
                        department = "Education";
                        model = repoVerificationInfo.GetCandidateEducationListByDepartmentManager1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId ,  ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);
                        if (CheckStatus == "Reverification")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count;

                        }

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEducationListByDepartmentManager(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);

                    }

                }
                else if (Designation == "Manager-Cri")
                {
                    if(version=="New")
                    {
                        department = "Criminal";
                        model = repoVerificationInfo.GetCandidateCriminalListByDepartmentManager1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, State, City, Partner);
                        if (CheckStatus == "Reverification")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count;

                        }

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateCriminalListByDepartmentManager(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, State, City, Partner);

                    }
                }
                else if (Designation == "Manager-Ref")
                {
                    if(version=="New")
                    {
                        department = "Reference";
                        model = repoVerificationInfo.GetCandidateReferenceListByDepartmentManager1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                        if (CheckStatus == "Reverification")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count;

                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateReferenceListByDepartmentManager(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                    }
                }
                else if (Designation == "Manager-Ide")
                {

                    if (version == "New")
                    {
                        department = "National";
                        model = repoVerificationInfo.GetCandidateIdentityListByDepartmentManager1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                        if (CheckStatus == "Reverification")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count;

                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateIdentityListByDepartmentManager(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                    }
                }
                else if (Designation == "Manager-SPL")
                {
                    if(version=="New")
                    {
                        department = "Special";
                        model = repoVerificationInfo.GetCandidateSpecialCheckListByManager1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                        if (CheckStatus == "Reverification")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count;

                        }

                    }
                    else
                        {
                        model = repoVerificationInfo.GetCandidateSpecialCheckListByManager(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        public ActionResult AgingChecksDetail(int page = 1, string Search = "", string version = "", short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "",  string CaseStatus = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            int pageSize = clsGlobalCommon.PageSize;
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string Designation = Session["Designation"].ToString();
            string department = "";
            if (Designation== "Manager-Emp")
            {
                department = "Employment";
            }
            else if(Designation == "Manager-Ref")
            {
                department = "Reference";
            }

            else if (Designation == "Manager-SPL")
            {
                department = "special";
            }

            else if (Designation == "Manager-Add")
            {
                department = "Address";
            }

            else if (Designation == "Manager-Cri")
            {
                department = "Criminal";
            }
            else if (Designation == "Manager-Edu")
            {
                department = "Education";
            }

            else if (Designation == "Manager-Ide")
            {
                department = "National";
            }

            else if (Designation == "Team Leader-Add")
            {
                department = "Address";
            }

            else if (Designation == "Team Leader-Cri")
            {
                department = "Criminal";
            }

            else if (Designation == "Team Leader-Edu")
            {
                department = "Education";
            }

            else if (Designation == "Team Leader-Ref")
            {
                department = "Reference";
            }

            else if (Designation == "Team Leader-Emp")
            {
                department = "Employment";
            }

            else if (Designation == "Team Leader-Ide")
            {
                department = "National";
            }

            else if (Designation == "Team Leader-SPL")
            {
                department = "special";
            }



            try
            {
                model = repoVerificationInfo.GetAgingDetails(page, pageSize,  Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);

        }
        [SessionExpireFilterAttribute]
        public ActionResult ManagerAllocationToTeamLeader(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string version="New", string VerificationIds = "", short AllocateTL = 0, short ClientRowID = 0, string CheckStatus = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                string department = "";
                string statusid = "";
                if (CheckStatus == "UnAllocated" && Designation == "Manager-Emp")
                {
                    statusid = "16";
                }

                if (CheckStatus == "Review" && Designation == "Manager-Emp")
                {
                    statusid = "17";
                }

                else if (CheckStatus == "UnAllocated" && Designation == "Manager-Add")
                {
                    statusid = "16";
                }

                else if (CheckStatus == "UnAllocated" && Designation == "Manager-Ref")
                {
                    statusid = "16";
                }

                else if(CheckStatus == "UnAllocated" && Designation == "Manager-SPL")
                {
                    statusid = "16";
                }

                else if (CheckStatus == "UnAllocated" && Designation == "Manager-Cri")
                {
                    statusid = "16";
                }

                else if(CheckStatus == "UnAllocated" && Designation == "Manager-Ide")
                {
                    statusid = "16";
                }

                else if(CheckStatus == "UnAllocated" && Designation == "Manager-Edu")
                {
                    statusid = "16";
                }

                //else if (CheckStatus == "Closed" && Designation == "Manager-Emp")
                //{
                //    statusid = "36";
                //}
                //else if (CheckStatus == "WIP" && Designation == "Manager-Emp")
                //{
                //    statusid = "16";
                //}

                BindClientListBaseOnLogin(teamMemerRowId);
                GetTLMemberForAllocate();

                byte UnAllocatedCount = 0;
                if (Designation == "Manager-Add")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTL > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateAddressManagerViewModel addModel = new PQUpdateAddressManagerViewModel();
                        addModel.ModifiedBy = teamMemerRowId;
                        addModel.ModifiedDate = DateTime.Now;
                        addModel.CheckStatus = "Allocated-TL";
                        addModel.TLAllocatedTo = AllocateTL;
                        addModel.TLAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());
                                //addModel.AddressRowID = item;
                            //var addDetails = repoAddressInfo.GetPQAddressDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, addModel.TLAllocatedTo, "Address", "Team Leader-Add");

                            if (result)
                            {
                                //repoAddressInfo.UpdateAddressInfoByManager(addModel);
                                    cc.OperationAllocation(item, addModel.TLAllocatedTo, "OPTL");
                                    cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 18, "Check", teamMemerRowId);
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(personalRowid, "ManagerAllocationToTeamLeader", addModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, addModel.CheckStatus, "");
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Address";
                        model = repoVerificationInfo.GetCandidateAddressListForTLAllocationByMgr1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateAddressListForTLAllocationByMgr(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }
                else if (Designation == "Manager-Emp")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTL > 0)
                    {
                        
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateEmploymentManagerViewModel empModel = new PQUpdateEmploymentManagerViewModel();
                        empModel.ModifiedBy = teamMemerRowId;
                        empModel.ModifiedDate = DateTime.Now;
                        empModel.CheckStatus = "Allocated-TL";
                        empModel.TLAllocatedTo = AllocateTL;
                        empModel.TLAllocatedDate = DateTime.Now;  
                        
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck=cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());

                                //empModel.EmploymentRowID = item;
                                //var empDetails = repoEmploymentInfo.GetPQEmploymentDetailsById(item);
                                bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, empModel.TLAllocatedTo, "Employment", "Team Leader-Emp");

                                if (result)
                                {
                                    //repoEmploymentInfo.UpdateEmploymentInfoByManager(empModel);
                                    cc.OperationAllocation(item, empModel.TLAllocatedTo, "OPTL");
                                    cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 18, "Check", teamMemerRowId);
                                    #region Save LogTransaction Details
                                    clsCommonMethods.AddPQLogTrasaction(personalRowid, "ManagerAllocationToTeamLeader", empModel.CheckStatus, CheckUniqueID, "Update");
                                    #endregion

                                    #region Case/Check Action History Save 
                                    clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, empModel.CheckStatus, "");
                                    #endregion
                                }
                                else
                                {
                                    UnAllocatedCount++;
                                }
                            }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Employment";
                        model = repoVerificationInfo.GetCandidateEmploymentListForTLAllocationByMgr1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus);
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEmploymentListForTLAllocationByMgr(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }
                else if (Designation == "Manager-Edu")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTL > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateEducationManagerViewModel eduModel = new PQUpdateEducationManagerViewModel();
                        eduModel.ModifiedBy = teamMemerRowId;
                        eduModel.ModifiedDate = DateTime.Now;
                        eduModel.CheckStatus = "Allocated-TL";
                        eduModel.TLAllocatedTo = AllocateTL;
                        eduModel.TLAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());

                                //eduModel.EducationRowID = item;
                            //var eduDetails = repoEducationInfo.GetPQEducationDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, eduModel.TLAllocatedTo, "Education", "Team Leader-Edu");

                            if (result)
                            {
                                //repoEducationInfo.UpdateEducationInfoByManager(eduModel);
                                    cc.OperationAllocation(item, eduModel.TLAllocatedTo, "OPTL");
                                    cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 18, "Check", teamMemerRowId);
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(personalRowid, "ManagerAllocationToTeamLeader", eduModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, eduModel.CheckStatus, "");
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Education";
                        model = repoVerificationInfo.GetCandidateEducationListForTLAllocationByMgr1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEducationListForTLAllocationByMgr(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }
                else if (Designation == "Manager-Cri")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTL > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateCriminalManagerViewModel criModel = new PQUpdateCriminalManagerViewModel();
                        criModel.ModifiedBy = teamMemerRowId;
                        criModel.ModifiedDate = DateTime.Now;
                        criModel.CheckStatus = "Allocated-TL";
                        criModel.TLAllocatedTo = AllocateTL;
                        criModel.TLAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());

                                //criModel.CriminalRowID = item;
                            //var criDetails = repoCriminalInfo.GetPQCriminalDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, criModel.TLAllocatedTo, "Criminal", "Team Leader-Cri");

                            if (result)
                            {
                                    cc.OperationAllocation(item, criModel.TLAllocatedTo, "OPTL");
                                    //repoCriminalInfo.UpdateCriminalInfoByManager(criModel);
                                cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 18, "Check", teamMemerRowId);
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(personalRowid, "ManagerAllocationToTeamLeader", criModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, criModel.CheckStatus, "");
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Criminal";
                        model = repoVerificationInfo.GetCandidateCriminalListForTLAllocationByMgr1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateCriminalListForTLAllocationByMgr(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }
                else if (Designation == "Manager-Ref")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTL > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateReferenceManagerViewModel refModel = new PQUpdateReferenceManagerViewModel();
                        refModel.ModifiedBy = teamMemerRowId;
                        refModel.ModifiedDate = DateTime.Now;
                        refModel.CheckStatus = "Allocated-TL";
                        refModel.TLAllocatedTo = AllocateTL;
                        refModel.TLAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());

                                //refModel.ReferenceRowID = item;
                            //var refDetails = repoReferenceInfo.GetPQReferenceDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, refModel.TLAllocatedTo, "Reference", "Team Leader-Ref");

                            if (result)
                            {
                                    cc.OperationAllocation(item, refModel.TLAllocatedTo, "OPTL");
                                    //repoReferenceInfo.UpdateReferenceInfoByManager(refModel);
                                cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 18, "Check", teamMemerRowId);
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(personalRowid, "ManagerAllocationToTeamLeader", refModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, refModel.CheckStatus, "");
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Reference";
                        model = repoVerificationInfo.GetCandidateReferenceListForTLAllocationByMgr1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus);
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateReferenceListForTLAllocationByMgr(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);
                    }
                   
                }
                else if (Designation == "Manager-Ide")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTL > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateNationalIdentityManagerViewModel ideModel = new PQUpdateNationalIdentityManagerViewModel();
                        ideModel.ModifiedBy = teamMemerRowId;
                        ideModel.ModifiedDate = DateTime.Now;
                        ideModel.CheckStatus = "Allocated-TL";
                        ideModel.TLAllocatedBy = teamMemerRowId;
                        ideModel.TLAllocatedTo = AllocateTL;
                        ideModel.TLAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());

                                //ideModel.NationalIdentityRowID = item;
                            //var ideDetails = repoNationalIdentInfo.GetPQNationalIdentityDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, ideModel.TLAllocatedTo, "National Identity", "Team Leader-Ide");

                            if (result)
                            {
                                    cc.OperationAllocation(item, ideModel.TLAllocatedTo, "OPTL");
                                    //repoNationalIdentInfo.UpdateNationalIdentityInfoByManager(ideModel);
                                cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 18, "Check", teamMemerRowId);
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(personalRowid, "ManagerAllocationToTeamLeader", ideModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, ideModel.CheckStatus, "");
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "National";
                        model = repoVerificationInfo.GetCandidateIdentityListForTLAllocationByMgr1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateIdentityListForTLAllocationByMgr(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }
                else if (Designation == "Manager-SPL")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTL > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();

                        ManagerAllocateCheckToTeamLeaderViewModel ideModel = new ManagerAllocateCheckToTeamLeaderViewModel();
                        ideModel.ModifiedBy = teamMemerRowId;
                        ideModel.ModifiedDate = DateTime.Now;
                        ideModel.CheckStatus = "Allocated-TL";
                        ideModel.TLAllocatedBy = teamMemerRowId;
                        ideModel.TLAllocatedTo = AllocateTL;
                        ideModel.TLAllocatedDate = DateTime.Now;

                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());

                                //ideModel.SpecialCheckRowId = item;
                            //var ideDetails = repoSplChkVer.GetCheckCommonInfoForAllocation(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, ideModel.TLAllocatedTo, "Special Check", "Team Leader-SPL");

                            if (result)
                            {
                                //repoSplChkVer.ManagerAllocateCheckToTeamLeader(ideModel);
                                    cc.OperationAllocation(item, ideModel.TLAllocatedTo, "OPTL");
                                    cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 18, "Check", teamMemerRowId);
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(personalRowid, "ManagerAllocationToTeamLeader", ideModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, ideModel.CheckStatus, "");
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Special Check";
                        model = repoVerificationInfo.GetCandidateSpecialCheckListForTLAllocationByMgr1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateSpecialCheckListForTLAllocationByMgr(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }

                if (UnAllocatedCount > 0)
                {
                    ViewBag.ErrorMsg = UnAllocatedCount.ToString() + " Check(s) not allocated because teamleader not mapped with client!";
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        public ActionResult DepartmentTeamLeaderAllocation(int page = 1, string sort = "", string sortDir = "desc", string Search = "", short ClientRowID = 0,string version="New", string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateTM = 0, short AllocateTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0,int CheckStatusId=0)
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                string department = "";
                string  statusid = "";

                if (CheckStatus == "Allocated" && Designation == "Team Leader-Add")
                {
                    statusid = "20,27,62,63";
                }

                else if (CheckStatus == "Allocated" && Designation == "Team Leader-Edu")
                {
                    statusid = "20,27";
                }

                else if (CheckStatus == "Allocated" && Designation == "Team Leader-Emp")
                {
                    statusid = "20,27";
                }

                else if (CheckStatus == "Allocated" && Designation == "Team Leader-Cri")
                {
                    statusid = "20,27";
                }

                else if (CheckStatus == "Allocated" && Designation == "Team Leader-Ide")
                {
                    statusid = "20,27";
                }

                else if (CheckStatus == "Allocated" && Designation == "Team Leader-SPL")
                {
                    statusid = "20,27";
                }

                else if (CheckStatus == "Allocated" && Designation == "Team Leader-Ref")
                {
                    statusid = "20,27";
                }

                else if (CheckStatus == "Closed" && Designation == "Team Leader-Add")
                {
                    statusid = "24";
                }

                else if (CheckStatus == "Closed" && Designation == "Team Leader-Emp")
                {
                    statusid = "24";
                }

                else if (CheckStatus == "Closed" && Designation == "Team Leader-Ref")
                {
                    statusid = "24";
                }

                else if (CheckStatus == "Closed" && Designation == "Team Leader-Edu")
                {
                    statusid = "24";
                }

                else if (CheckStatus == "Closed" && Designation == "Team Leader-Cri")
                {
                    statusid = "24";
                }

                else if (CheckStatus == "Closed" && Designation == "Team Leader-Ide")
                {
                    statusid = "24";
                }
                else if (CheckStatus == "Closed" && Designation == "Team Leader-SPL")
                {
                    statusid = "24";
                }
                else if (CheckStatus == "WIP" && Designation == "Team Member-Add")
                {
                    statusid = "20";
                }
                else if (CheckStatus == "Review" && Designation == "Team Leader-SPL")
                {
                    statusid = "19";
                }
                
                
                else
                {
                    statusid = CheckStatusId.ToString();
                }

                //BindClientList();

                BindClientListBaseOnLogin(teamMemerRowId);
                CheckStatusOptionDropDown();
                CaseStatusOptionDropDown();

                if (Designation == "Team Leader-Add")
                {
                    BindCities();
                    BindStates();
                    BindPartneres();
                    if(version=="New")
                    {
                        department = "Address";
                        model = repoVerificationInfo.GetCandidateAddressListByDepartmentTeamLeader1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId,department,statusid, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, State, City, Partner);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateAddressListByDepartmentTeamLeader(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, State, City, Partner);

                    }

                }
                else if (Designation == "Team Leader-Emp")
                {
                    if(version=="New")
                    {
                        department = "Employment";
                        model = repoVerificationInfo.GetCandidateEmploymentListByDepartmentTeamLeader1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);

                    }
                    else if(version=="Auto")
                    {
                        model = repoVerificationInfo.GetAutoCandidateEmploymentListByDepartmentTeamLeader1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEmploymentListByDepartmentTeamLeader(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);
                    }
                }
                else if (Designation == "Team Leader-Edu")
                {
                    if (version=="New")
                    {
                        department = "Education";
                        model = repoVerificationInfo.GetCandidateEducationListByDepartmentTeamLeader1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEducationListByDepartmentTeamLeader(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);

                    }
                }
                else if (Designation == "Team Leader-Cri")
                {
                    if (version == "New")
                    {
                        department = "Criminal";
                        model = repoVerificationInfo.GetCandidateCriminalListByDepartmentTeamLeader1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department,statusid, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, State, City, Partner);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateCriminalListByDepartmentTeamLeader(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, State, City, Partner);

                    }
                }
                else if (Designation == "Team Leader-Ref")
                {
                    if(version=="New")
                    {
                        department = "Reference";
                        model = repoVerificationInfo.GetCandidateReferenceListByDepartmentTeamLeader1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department,statusid, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateReferenceListByDepartmentTeamLeader(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                }
                else if (Designation == "Team Leader-Ide")
                {
                    if(version=="New")
                    {
                        department = "National";
                        model = repoVerificationInfo.GetCandidateIdentityListByDepartmentTeamLeader1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateIdentityListByDepartmentTeamLeader(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                }
                else if (Designation == "Team Leader-SPL")
                {
                    if(version=="New")
                    {
                        department = "Special";
                        model = repoVerificationInfo.GetCandidateSpecialCheckListByTeamLeader1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateSpecialCheckListByTeamLeader(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, dtm, dtmqc, AllocateTM, AllocateTMQC, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        public ActionResult TeamLeaderAllocationToTeamMember(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string version = "New",string VerificationIds = "", short AllocateTM = 0, short AllocateTMQC = 0, short ClientRowID = 0, string OCVerIDs = "", string CheckStatus = "")
        {
             VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                string department = "";
                string  statusid = "";
                
                if (CheckStatus == "Unallocated" && Designation == "Team Leader-Add")
                {
                    statusid = "18";
                }

              else  if (CheckStatus == "Unallocated" && Designation == "Team Leader-Edu")
                {
                    statusid = "18";
                }

                else if(CheckStatus == "Unallocated" && Designation == "Team Leader-Cri")
                {
                    statusid = "18";
                }

                else if (CheckStatus == "Unallocated" && Designation == "Team Leader-Emp")
                {
                    statusid = "18";
                }

                else if(CheckStatus == "Unallocated" && Designation == "Team Leader-Ide")
                {
                    statusid = "18";
                }
                else if (CheckStatus == "Unallocated" && Designation == "Team Leader-SPL")
                {
                    statusid = "18";
                }

                else if (CheckStatus == "Unallocated" && Designation == "Team Leader-Ref")
                {
                    statusid = "18";
                }

                


                //BindStates();
                //BindClientList();
                BindClientListBaseOnLogin(teamMemerRowId);
                GetTMMemberForAllocate();
                GetTMQCMemberForAllocate();

                byte UnAllocatedCount = 0;
                if (Designation == "Team Leader-Add")
                {

                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTM > 0 && AllocateTMQC > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateAddressTeamLeaderViewModel addModel = new PQUpdateAddressTeamLeaderViewModel();
                        addModel.ModifiedBy = teamMemerRowId;
                        addModel.ModifiedDate = DateTime.Now;
                        addModel.CheckStatus = "Allocated-TM";
                        addModel.TMAllocatedTo = AllocateTM;
                        addModel.TMAllocatedDate = DateTime.Now;
                        addModel.TMQCAllocatedTo = AllocateTMQC;
                        addModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());
                                //addModel.AddressRowID = item;
                            //var addDetails = repoAddressInfo.GetPQAddressDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, addModel.TMAllocatedTo, addModel.TMQCAllocatedTo, "Address", "Team Member-Add", "Team Member-Add-QC");

                            if (result)
                            {
                                //repoAddressInfo.UpdateAddressInfoByTeamLeader(addModel);
                                    cc.OperationAllocation(item, addModel.TMAllocatedTo, "OPTM");
                                    cc.OperationAllocation(item, addModel.TMQCAllocatedTo, "OPQC");

                                    #region Save LogTransaction Details
                                    clsCommonMethods.AddPQLogTrasaction(personalRowid, "TeamLeaderAllocationToTeamMember", addModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, addModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion

                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if (version == "New")
                    {
                        department = "Address";
                        model = repoVerificationInfo.GetCandidateAddressListForTMQCAllocationByTL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateAddressListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }
                else if (Designation == "Team Leader-Emp")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTM > 0 && AllocateTMQC > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateEmploymentTeamLeaderViewModel empModel = new PQUpdateEmploymentTeamLeaderViewModel();
                        empModel.ModifiedBy = teamMemerRowId;
                        empModel.ModifiedDate = DateTime.Now;
                        empModel.CheckStatus = "Allocated-TM";
                        empModel.TMAllocatedTo = AllocateTM;
                        empModel.TMAllocatedDate = DateTime.Now;
                        empModel.TMQCAllocatedTo = AllocateTMQC;
                        empModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());
                                //empModel.EmploymentRowID = item;
                                //var addDetails = repoEmploymentInfo.GetPQEmploymentDetailsById(item);
                                bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, empModel.TMAllocatedTo, empModel.TMQCAllocatedTo, "Employment", "Team Member-Emp", "Team Member-Emp-QC");

                                if (result)
                                {
                                    //repoEmploymentInfo.UpdateEmploymentInfoByTeamLeader(empModel);
                                    cc.OperationAllocation(item, empModel.TMAllocatedTo, "OPTM");
                                    cc.OperationAllocation(item, empModel.TMQCAllocatedTo, "OPQC");
                                    #region Save LogTransaction Details
                                    clsCommonMethods.AddPQLogTrasaction(personalRowid, "TeamLeaderAllocationToTeamMember", empModel.CheckStatus, CheckUniqueID, "Update");
                                    #endregion

                                    #region Case/Check Action History Save 
                                    clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, empModel.CheckStatus, "");
                                    #endregion

                                    #region update Check status from Operation TM                                
                                    cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 73, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    #endregion
                                }
                                else
                                {
                                    UnAllocatedCount++;
                                }
                            }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Employment";
                        model = repoVerificationInfo.GetCandidateEmploymentListForTMQCAllocationByTL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEmploymentListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);
                    }
                }
                else if (Designation == "Team Leader-Edu")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTM > 0 && AllocateTMQC > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateEducationTeamLeaderViewModel eduModel = new PQUpdateEducationTeamLeaderViewModel();
                        eduModel.ModifiedBy = teamMemerRowId;
                        eduModel.ModifiedDate = DateTime.Now;
                        eduModel.CheckStatus = "Allocated-TM";
                        eduModel.TMAllocatedTo = AllocateTM;
                        eduModel.TMAllocatedDate = DateTime.Now;
                        eduModel.TMQCAllocatedTo = AllocateTMQC;
                        eduModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());
                                //eduModel.EducationRowID = item;
                            //var addDetails = repoEducationInfo.GetPQEducationDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, eduModel.TMAllocatedTo, eduModel.TMQCAllocatedTo, "Education", "Team Member-Edu", "Team Member-Edu-QC");

                            if (result)
                            {
                                    cc.OperationAllocation(item, eduModel.TMAllocatedTo, "OPTM");
                                    cc.OperationAllocation(item, eduModel.TMQCAllocatedTo, "OPQC");
                                    //repoEducationInfo.UpdateEducationInfoByTeamLeader(eduModel);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(personalRowid, "TeamLeaderAllocationToTeamMember", eduModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, eduModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Education";
                        model = repoVerificationInfo.GetCandidateEducationListForTMQCAllocationByTL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEducationListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }
                else if (Designation == "Team Leader-Cri")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTM > 0 && AllocateTMQC > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateCriminalTeamLeaderViewModel criModel = new PQUpdateCriminalTeamLeaderViewModel();
                        criModel.ModifiedBy = teamMemerRowId;
                        criModel.ModifiedDate = DateTime.Now;
                        criModel.CheckStatus = "Allocated-TM";
                        criModel.TMAllocatedTo = AllocateTM;
                        criModel.TMAllocatedDate = DateTime.Now;
                        criModel.TMQCAllocatedTo = AllocateTMQC;
                        criModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());
                                //criModel.CriminalRowID = item;
                            //var addDetails = repoCriminalInfo.GetPQCriminalDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, criModel.TMAllocatedTo, criModel.TMQCAllocatedTo, "Criminal", "Team Member-Cri", "Team Member-Cri-QC");

                            if (result)
                            {
                                //repoCriminalInfo.UpdateCriminalInfoByTeamLeader(criModel);
                                    cc.OperationAllocation(item, criModel.TMAllocatedTo, "OPTM");
                                    cc.OperationAllocation(item, criModel.TMQCAllocatedTo, "OPQC");

                                    #region Save LogTransaction Details
                                    clsCommonMethods.AddPQLogTrasaction(personalRowid, "TeamLeaderAllocationToTeamMember", criModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, criModel.CheckStatus, "");
                                #endregion
                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Criminal";
                        model = repoVerificationInfo.GetCandidateCriminalListForTMQCAllocationByTL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateCriminalListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }
                else if (Designation == "Team Leader-Ref")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTM > 0 && AllocateTMQC > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateReferenceTeamLeaderViewModel refModel = new PQUpdateReferenceTeamLeaderViewModel();
                        refModel.ModifiedBy = teamMemerRowId;
                        refModel.ModifiedDate = DateTime.Now;
                        refModel.CheckStatus = "Allocated-TM";
                        refModel.TMAllocatedTo = AllocateTM;
                        refModel.TMAllocatedDate = DateTime.Now;
                        refModel.TMQCAllocatedTo = AllocateTMQC;
                        refModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());
                                //refModel.ReferenceRowID = item;
                            //var addDetails = repoReferenceInfo.GetPQReferenceDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, refModel.TMAllocatedTo, refModel.TMQCAllocatedTo, "Reference", "Team Member-Ref", "Team Member-Ref-QC");

                            if (result)
                            {
                                    cc.OperationAllocation(item, refModel.TMAllocatedTo, "OPTM");
                                    cc.OperationAllocation(item, refModel.TMQCAllocatedTo, "OPQC");
                                    //repoReferenceInfo.UpdateReferenceInfoByTeamLeader(refModel);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(personalRowid, "TeamLeaderAllocationToTeamMember", refModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, refModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Reference";
                        model = repoVerificationInfo.GetCandidateReferenceListForTMQCAllocationByTL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateReferenceListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }
                else if (Designation == "Team Leader-Ide")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTM > 0 && AllocateTMQC > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateNationalIdentityTeamLeaderViewModel ideModel = new PQUpdateNationalIdentityTeamLeaderViewModel();
                        ideModel.ModifiedBy = teamMemerRowId;
                        ideModel.ModifiedDate = DateTime.Now;
                        ideModel.CheckStatus = "Allocated-TM";
                        ideModel.TMAllocatedTo = AllocateTM;
                        ideModel.TMAllocatedDate = DateTime.Now;
                        ideModel.TMQCAllocatedTo = AllocateTMQC;
                        ideModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());
                                //ideModel.NationalIdentityRowID = item;
                            //var addDetails = repoNationalIdentInfo.GetPQNationalIdentityDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, ideModel.TMAllocatedTo, ideModel.TMQCAllocatedTo, "National Identity", "Team Member-Ide", "Team Member-Ide-QC");

                            if (result)
                            {
                                    cc.OperationAllocation(item, ideModel.TMAllocatedTo, "OPTM");
                                    cc.OperationAllocation(item, ideModel.TMQCAllocatedTo, "OPQC");
                                    //repoNationalIdentInfo.UpdateNationalIdentityInfoByTeamLeader(ideModel);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(personalRowid, "TeamLeaderAllocationToTeamMember", ideModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, ideModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "National";
                        model = repoVerificationInfo.GetCandidateIdentityListForTMQCAllocationByTL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateIdentityListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }
                else if (Designation == "Team Leader-SPL")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTM > 0 && AllocateTMQC > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        TeamLeaderAllocateCheckToTeamMemberViewModel ideModel = new TeamLeaderAllocateCheckToTeamMemberViewModel();
                        ideModel.ModifiedBy = teamMemerRowId;
                        ideModel.ModifiedDate = DateTime.Now;
                        ideModel.CheckStatus = "Allocated-TM";
                        ideModel.TMAllocatedTo = AllocateTM;
                        ideModel.TMAllocatedDate = DateTime.Now;
                        ideModel.TMQCAllocatedTo = AllocateTMQC;
                        ideModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());
                                //ideModel.SpecialCheckRowId = item;
                            //var addDetails = repoSplChkVer.GetCheckCommonInfoForAllocation(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, ideModel.TMAllocatedTo, ideModel.TMQCAllocatedTo, "Special Check", "Team Member-SPL", "Team Member-SPL-QC");

                            if (result)
                            {
                                    cc.OperationAllocation(item, ideModel.TMAllocatedTo, "OPTM");
                                    cc.OperationAllocation(item, ideModel.TMQCAllocatedTo, "OPQC");
                                    //repoSplChkVer.TeamLeaderAllocateCheckToTeamMember(ideModel);

                                    #region Save LogTransaction Details
                                    clsCommonMethods.AddPQLogTrasaction(personalRowid, "TeamLeaderAllocationToTeamMember", ideModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, ideModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if(version=="New")
                    {
                        department = "Special Check";
                        model = repoVerificationInfo.GetCandidateSpecialCheckListForTMQCAllocationByTL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department,statusid, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateSpecialCheckListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }

                if (UnAllocatedCount > 0)
                {
                    ViewBag.ErrorMsg = UnAllocatedCount.ToString() + " Check(s) not allocated because teammember/teammemberQC not mapped with client!";
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        public ActionResult TeamLeaderReAllocationToTeamMember(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string VerificationIds = "", short AllocateTM = 0, short AllocateTMQC = 0, short ClientRowID = 0, string OCVerIDs = "", string CheckStatus = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                BindStates();
               int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();

                //BindClientList();
                BindClientListBaseOnLogin(teamMemerRowId);
                GetTMMemberForAllocate();
                GetTMQCMemberForAllocate();
                string ReAllocation = "ReAllocation";
                byte UnAllocatedCount = 0;
                if (Designation == "Team Leader-Add")
                {

                    if (!string.IsNullOrEmpty(VerificationIds) && (AllocateTM > 0 || AllocateTMQC > 0))
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateAddressTeamLeaderViewModel addModel = new PQUpdateAddressTeamLeaderViewModel();
                        addModel.ModifiedBy = teamMemerRowId;
                        addModel.ModifiedDate = DateTime.Now;
                        addModel.CheckStatus = "Allocated-TM";
                        addModel.TMAllocatedTo = AllocateTM;
                        addModel.TMAllocatedDate = DateTime.Now;
                        addModel.TMQCAllocatedTo = AllocateTMQC;
                        addModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            addModel.AddressRowID = item;
                            var addDetails = repoAddressInfo.GetPQAddressDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExistForReAllocation(addDetails.ClientRowID, addModel.TMAllocatedTo, addModel.TMQCAllocatedTo, "Address", "Team Member-Add", "Team Member-Add-QC");

                            if (result)
                            {
                                repoAddressInfo.UpdateAddressInfoByTeamLeaderReAllocated(addModel);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(addDetails.PersonalRowID, "TeamLeaderAllocationToTeamMember", addModel.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(addDetails.PersonalRowID, addDetails.SubCheckRowID, addModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(addDetails.PersonalRowID, addDetails.SubCheckRowID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                    }
                    model = repoVerificationInfo.GetCandidateAddressListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, ReAllocation, AllocateTM, AllocateTMQC);
                }
                else if (Designation == "Team Leader-Emp")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && (AllocateTM > 0 || AllocateTMQC > 0))
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateEmploymentTeamLeaderViewModel empModel = new PQUpdateEmploymentTeamLeaderViewModel();
                        empModel.ModifiedBy = teamMemerRowId;
                        empModel.ModifiedDate = DateTime.Now;
                        empModel.CheckStatus = "Allocated-TM";
                        empModel.TMAllocatedTo = AllocateTM;
                        empModel.TMAllocatedDate = DateTime.Now;
                        empModel.TMQCAllocatedTo = AllocateTMQC;
                        empModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            empModel.EmploymentRowID = item;
                            var addDetails = repoEmploymentInfo.GetPQEmploymentDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExistForReAllocation(addDetails.ClientRowID, empModel.TMAllocatedTo, empModel.TMQCAllocatedTo, "Employment", "Team Member-Emp", "Team Member-Emp-QC");

                            if (result)
                            {
                                repoEmploymentInfo.UpdateEmploymentInfoByTeamLeaderReAllocated(empModel);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(addDetails.PersonalRowID, "TeamLeaderAllocationToTeamMember", empModel.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(addDetails.PersonalRowID, addDetails.SubCheckRowID, empModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                               //cc.UpdateCheckStatus(addDetails.PersonalRowID, addDetails.SubCheckRowID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                    }
                    model = repoVerificationInfo.GetCandidateEmploymentListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, ReAllocation, AllocateTM, AllocateTMQC);
                }
                else if (Designation == "Team Leader-Edu")
                {   
                    if (!string.IsNullOrEmpty(VerificationIds) && (AllocateTM > 0 || AllocateTMQC > 0))
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                             PQUpdateEducationTeamLeaderViewModel eduModel = new PQUpdateEducationTeamLeaderViewModel();
                        eduModel.ModifiedBy = teamMemerRowId;
                        eduModel.ModifiedDate = DateTime.Now;
                        eduModel.CheckStatus = "Allocated-TM";
                        eduModel.TMAllocatedTo = AllocateTM;
                        eduModel.TMAllocatedDate = DateTime.Now;
                        eduModel.TMQCAllocatedTo = AllocateTMQC;
                        eduModel.TMQCAllocatedDate = DateTime.Now;    
                      foreach (var item in verIDs)
                        {
                            eduModel.EducationRowID = item;
                            var addDetails = repoEducationInfo.GetPQEducationDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExistForReAllocation(addDetails.ClientRowID, eduModel.TMAllocatedTo, eduModel.TMQCAllocatedTo, "Education", "Team Member-Edu", "Team Member-Edu-QC");

                            if (result)
                            {
                                repoEducationInfo.UpdateEducationInfoByTeamLeaderReAllocated(eduModel);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(addDetails.PersonalRowID, "TeamLeaderAllocationToTeamMember", eduModel.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(addDetails.PersonalRowID, addDetails.SubCheckRowID, eduModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(addDetails.PersonalRowID, addDetails.SubCheckRowID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                    }
                    model = repoVerificationInfo.GetCandidateEducationListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, ReAllocation, AllocateTM, AllocateTMQC);
     }
                else if (Designation == "Team Leader-Cri")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && (AllocateTM > 0 || AllocateTMQC > 0))
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateCriminalTeamLeaderViewModel criModel = new PQUpdateCriminalTeamLeaderViewModel();
                        criModel.ModifiedBy = teamMemerRowId;
                        criModel.ModifiedDate = DateTime.Now;
                        criModel.CheckStatus = "Allocated-TM";
                        criModel.TMAllocatedTo = AllocateTM;
                        criModel.TMAllocatedDate = DateTime.Now;
                        criModel.TMQCAllocatedTo = AllocateTMQC;
                        criModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            criModel.CriminalRowID = item;
                            var addDetails = repoCriminalInfo.GetPQCriminalDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExistForReAllocation(addDetails.ClientRowID, criModel.TMAllocatedTo, criModel.TMQCAllocatedTo, "Criminal", "Team Member-Cri", "Team Member-Cri-QC");

                            if (result)
                            {
                                repoCriminalInfo.UpdateCriminalInfoByTeamLeaderReAllocated(criModel);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(addDetails.PersonalRowID, "TeamLeaderAllocationToTeamMember", criModel.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(addDetails.PersonalRowID, addDetails.SubCheckRowID, criModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(addDetails.PersonalRowID, addDetails.SubCheckRowID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                    }
                    model = repoVerificationInfo.GetCandidateCriminalListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, ReAllocation, AllocateTM, AllocateTMQC);
                }
                else if (Designation == "Team Leader-Ref")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && (AllocateTM > 0 || AllocateTMQC > 0))
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateReferenceTeamLeaderViewModel refModel = new PQUpdateReferenceTeamLeaderViewModel();
                        refModel.ModifiedBy = teamMemerRowId;
                        refModel.ModifiedDate = DateTime.Now;
                        refModel.CheckStatus = "Allocated-TM";
                        refModel.TMAllocatedTo = AllocateTM;
                        refModel.TMAllocatedDate = DateTime.Now;
                        refModel.TMQCAllocatedTo = AllocateTMQC;
                        refModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            refModel.ReferenceRowID = item;
                            var addDetails = repoReferenceInfo.GetPQReferenceDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExistForReAllocation(addDetails.ClientRowID, refModel.TMAllocatedTo, refModel.TMQCAllocatedTo, "Reference", "Team Member-Ref", "Team Member-Ref-QC");

                            if (result)
                            {
                                repoReferenceInfo.UpdateReferenceInfoByTeamLeaderReAllocated(refModel);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(addDetails.PersonalRowID, "TeamLeaderAllocationToTeamMember", refModel.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(addDetails.PersonalRowID, addDetails.SubCheckRowID, refModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(addDetails.PersonalRowID, addDetails.SubCheckRowID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                    }
                    model = repoVerificationInfo.GetCandidateReferenceListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, ReAllocation, AllocateTM, AllocateTMQC);
                }
                else if (Designation == "Team Leader-Ide")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && (AllocateTM > 0 || AllocateTMQC > 0))
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateNationalIdentityTeamLeaderViewModel ideModel = new PQUpdateNationalIdentityTeamLeaderViewModel();
                        ideModel.ModifiedBy = teamMemerRowId;
                        ideModel.ModifiedDate = DateTime.Now;
                        ideModel.CheckStatus = "Allocated-TM";
                        ideModel.TMAllocatedTo = AllocateTM;
                        ideModel.TMAllocatedDate = DateTime.Now;
                        ideModel.TMQCAllocatedTo = AllocateTMQC;
                        ideModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            ideModel.NationalIdentityRowID = item;
                            var addDetails = repoNationalIdentInfo.GetPQNationalIdentityDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExistForReAllocation(addDetails.ClientRowID, ideModel.TMAllocatedTo, ideModel.TMQCAllocatedTo, "National Identity", "Team Member-Ide", "Team Member-Ide-QC");

                            if (result)
                            {
                                repoNationalIdentInfo.UpdateNationalIdentityInfoByTeamLeaderReAllocated(ideModel);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(addDetails.PersonalRowID, "TeamLeaderAllocationToTeamMember", ideModel.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(addDetails.PersonalRowID, addDetails.SubCheckRowID, ideModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(addDetails.PersonalRowID, addDetails.SubCheckRowID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                    }
                    model = repoVerificationInfo.GetCandidateIdentityListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, ReAllocation, AllocateTM, AllocateTMQC);
                }
                else if (Designation == "Team Leader-SPL")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && (AllocateTM > 0 || AllocateTMQC > 0))
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateSpecialCheckTeamLeaderViewModel ideModel = new PQUpdateSpecialCheckTeamLeaderViewModel();
                        ideModel.ModifiedBy = teamMemerRowId;
                        ideModel.ModifiedDate = DateTime.Now;
                        ideModel.CheckStatus = "Allocated-TM";
                        ideModel.TMAllocatedTo = AllocateTM;
                        ideModel.TMAllocatedDate = DateTime.Now;
                        ideModel.TMQCAllocatedTo = AllocateTMQC;
                        ideModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            ideModel.SpecialCheckRowID = item;
                            var addDetails = repoSplChkVer.GetPQSpecialCheckDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExistForReAllocation(addDetails.ClientRowID, ideModel.TMAllocatedTo, ideModel.TMQCAllocatedTo, "Special Check", "Team Member-SPL", "Team Member-SPL-QC");

                            if (result)
                            {
                                repoSplChkVer.UpdateSpecialCheckInfoByTeamLeaderReAllocated(ideModel);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(addDetails.PersonalRowID, "TeamLeaderAllocationToTeamMember", ideModel.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(addDetails.PersonalRowID, addDetails.SubCheckRowID, ideModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(addDetails.PersonalRowID, addDetails.SubCheckRowID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                    }
                    model = repoVerificationInfo.GetCandidateSpecialCheckListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, ReAllocation, AllocateTM, AllocateTMQC);
                }
                if (UnAllocatedCount > 0)
                {
                    ViewBag.ErrorMsg = UnAllocatedCount.ToString() + " Check(s) not allocated because teammember/teammemberQC not mapped with client!";
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        public ActionResult ManageTeamMemberChecks(int page = 1, string sort = "", string sortDir = "desc", string Search = "",  short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0,string version="New",string CheckStatusId="",string Aging="",string VEmploymentId="")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            if (Aging == "True")
            {
                return RedirectToAction("Index", "Verification");
            }

            try
            {
                int pageSize = 500;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();

                //BindClientList();
                BindClientListBaseOnLogin(teamMemerRowId);
                CheckStatusOptionDropDown();
                CaseStatusOptionDropDown();

                if (Designation == "Team Member-Add")
                {
                    string department = "Address";
                    string  statusid = "";
                    if (CheckStatus == "WIP" )
                    {
                        statusid = "20,27,62,63";
                    }
                    else if (CheckStatus == "Closed")
                    {
                        statusid = "24";
                    }
                    else if (CheckStatus == "PartnerClosed")
                    {
                        statusid = "38";
                    }
                    else if (CheckStatus == "InsufficentRaised")
                    {
                        statusid = "53";
                    }
                    else if (CheckStatus == "PartnerClosed")
                    {
                        statusid = "38";
                    }
                    else if (CheckStatus == "FieldPartnerClosed")
                    {
                        statusid = "66";
                    }

                    else if (CheckStatus == "InsufficencyResolved")
                    {
                        statusid = "56";
                    }
                    else if (CheckStatus == "Rejected")
                    {
                        statusid = "21";
                    }
                    else if (CheckStatus == "PartnerRejected")
                    {
                        statusid = "64";
                    }
                    else if (CheckStatus == "Reverification")
                    {
                        statusid = "20";
                    }
                    else
                    {
                        statusid = CheckStatusId;
                    }
                    //BindCities();
                    //BindStates();
                    //BindPartneres();
                    if(version=="New")
                    {
                        model = repoVerificationInfo.GetCandidateAddressListForTMM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid,teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto, State, City, Partner);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateAddressListForTMM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto, State, City, Partner);

                    }
                }
                else if (Designation == "Team Member-Emp" || Designation== "Team Member-Emp-QC")
                {
                    if (version == "New")
                    {
                        string department = "Employment";
                        string statusid = "";
                        if (CheckStatus == "WIP")
                        {
                            statusid = "20,27,62,63,64,73";
                        }
                        else if (CheckStatus == "Closed")
                        {
                            statusid = "24";
                        }

                        else if (CheckStatus == "InsufficentRaised")
                        {
                            statusid = "53";
                        }

                        else if (CheckStatus == "InsufficencyResolved")
                        {
                            statusid = "56";
                        }


                        else if (CheckStatus == "Research")
                        {
                            statusid = "24";
                        }
                        else if (CheckStatus == "FundsPending")
                        {
                            statusid = "26";
                        }
                        else if (CheckStatus == "Rejected")
                        {
                            statusid = "21";
                        }
                        else if (CheckStatus == "PVClosed")
                        {
                            statusid = "67";
                        }
                        else if (CheckStatus == "ErrorInMail")
                        {
                            statusid = "92";
                        }

                        else if (CheckStatus == "AutoEmailer")
                        {
                            statusid = "74";
                        }
                        else if (CheckStatus == "Sent Reminder1")
                        {
                            statusid = "82";
                        }
                        else if (CheckStatus == "Sent Reminder2")
                        {
                            statusid = "75";
                        }
                        else if (CheckStatus == "Sent Reminder3")
                        {
                            statusid = "76";
                        }
                        else if (CheckStatus == "Sent Reminder4")
                        {
                            statusid = "77";
                        }
                        else if (CheckStatus == "Sent Reminder5")
                        {
                            statusid = "78";
                        }

                        else if (CheckStatus == "GreaterThan5")
                        {
                            statusid = "79";
                        }

                        else if (CheckStatus == "RevertNotRec")
                        {
                            statusid = "81";
                        }
                        else if(CheckStatus == "RevertRejected")
                        {
                            statusid = "90";
                        }
                        else if (CheckStatus == "GroupInitial")
                        {
                            statusid = "91";
                        }
                        else if (CheckStatus == "WIPALL")
                        {
                            statusid = "73,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90";

                        }
                        else if (CheckStatus == "Reverification")
                        {
                            statusid = "20";

                        }

                        else
                        {
                            statusid = CheckStatusId;
                        }
                        if (Aging == "Yes")
                        {
                            statusid = "20";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            if (VEmploymentId == "" && !CheckStatus.Contains( "Group"))
                            {
                                model = repoVerificationInfo.GetCandidateEmploymentListForTMM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);
                            }
                            else if(VEmploymentId == "" && CheckStatus.Contains("Group"))
                            {
                                model = repoVerificationInfo.GetAutoGroupMailDetail(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);

                            }
                            else
                            {
                                int[] verIDs = VEmploymentId.Split(',').Select(str => int.Parse(str)).ToArray();
                                int MAutoId = 0;
                                string AutoId = "";
                                bool abb = false;
                                string Abbreviations = "";
                                foreach (var item in verIDs)
                                {
                                    AutoEmploymentDetail objAuto = new AutoEmploymentDetail();
                                    objAuto = repoVerificationInfo.GetAutoEmployementDetail(item);
                                    if (abb == false)
                                    {
                                        string Comp_Name = "";
                                        string[] ssize = objAuto.CompanyName.Split(' ');
                                        if (ssize[0].Length > 5)
                                        {
                                            Comp_Name = ssize[0].Replace(@"\r\n", "").Replace("\n", string.Empty).Replace("\r", string.Empty);
                                        }
                                        else
                                        {
                                            Comp_Name = ssize[0].Replace(@"\r\n", "").Replace("\n", string.Empty).Replace("\r", string.Empty) + ssize[1].Replace(@"\r\n", "").Replace("\n", string.Empty).Replace("\r", string.Empty) ;
                                        }
                                        DataTable CAbb= cc.GetCompAbbCount(Comp_Name);
                                        if(CAbb.Rows.Count>0)
                                        {
                                            objAuto.AbbCount = Convert.ToInt32(CAbb.Rows[0]["AbbCount"].ToString());
                                        }
                                        if (objAuto.AbbCount == 0)
                                        {
                                            Abbreviations = Comp_Name + "-" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "-" + 1;
                                        }
                                        else
                                        {
                                            objAuto.AbbCount = objAuto.AbbCount + 1;
                                            Abbreviations = Comp_Name + "-" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "-" + objAuto.AbbCount ;
                                        }
                                        abb = true;
                                    }


                                    AutoId = cc.CreateMasterEmailGroup(objAuto.PersonalRowId, objAuto.SubCheckRowId, Abbreviations, objAuto.ClientRowID);
                                    MAutoId = int.Parse(AutoId);
                                    if (MAutoId > 0)
                                    {
                                        cc.UpdateCheckStatus(objAuto.PersonalRowId, objAuto.SubCheckRowId, 1, 91, "Manual Group Automation", 101);
                                    }

                                }
                            }

                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEmploymentListForTMM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);

                    }
                }


                else if (Designation == "Auto Revert QC")
                {
                    if (version == "New")
                    {
                        string department = "Employment";
                        string statusid = "";
                        if (CheckStatus == "WIP" || CheckStatus == "Reverification")
                        {
                            statusid = "20";
                        }
                        else if (CheckStatus == "Closed")
                        {
                            statusid = "24";
                        }

                        else if (CheckStatus == "InsufficentRaised")
                        {
                            statusid = "53";
                        }

                        else if (CheckStatus == "InsufficencyResolved")
                        {
                            statusid = "56";
                        }


                        else if (CheckStatus == "Research")
                        {
                            statusid = "24";
                        }
                        else if (CheckStatus == "FundsPending")
                        {
                            statusid = "26";
                        }
                        else if (CheckStatus == "Rejected")
                        {
                            statusid = "21";
                        }
                        else if (CheckStatus == "PVClosed")
                        {
                            statusid = "67";
                        }
                        else if (CheckStatus == "AutoEmailer")
                        {
                            statusid = "74";
                        }
                        else if (CheckStatus == "initialMailSent")
                        {
                            statusid = "82";
                        }
                        else if (CheckStatus == "Reminder1")
                        {
                            statusid = "75";
                        }
                        else if (CheckStatus == "Reminder2")
                        {
                            statusid = "76";
                        }
                        else if (CheckStatus == "Reminder3")
                        {
                            statusid = "77";
                        }
                        else if (CheckStatus == "Reminder4")
                        {
                            statusid = "78";
                        }

                        else if (CheckStatus == "RevertNotRec")
                        {
                            statusid = "81";
                        }
                        else
                        {
                            statusid = CheckStatusId;
                        }
                        if (Aging == "Yes")
                        {
                            statusid = "20";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateEmploymentListForTMM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);

                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEmploymentListForTMM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);

                    }
                }
                else if (Designation == "Team Member-Edu")
                {
                    if(version=="New")
                    {
                        string department = "Education";
                        string statusid = "";
                        if (CheckStatus == "WIP" || CheckStatus == "Reverification")
                        {
                            statusid = "20";
                        }
                        else if (CheckStatus == "Closed")
                        {
                            statusid = "24";
                        }

                        else if (CheckStatus == "InsufficentRaised")
                        {
                            statusid = "53";
                        }

                        else if (CheckStatus == "InsufficencyResolved")
                        {
                            statusid = "56";
                        }
                        else if (CheckStatus == "PartnerClosed")
                        {
                            statusid = "38";
                        }

                        else if (CheckStatus == "Research")
                        {
                            statusid = "24";
                        }
                        else if (CheckStatus == "FundsPending")
                        {
                            statusid = "26";
                        }
                        else if (CheckStatus == "Rejected")
                        {
                            statusid = "21";
                        }
                        else if (CheckStatus == "PVClosed")
                        {
                            statusid = "67";
                        }
                        else
                        {
                            statusid = CheckStatusId;
                        }
                        if (Aging == "Yes")
                        {
                            statusid = "20";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateEducationListForTMM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);
                            if (CheckStatus == "Reverification")
                            {
                                var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                                model.PQVerificationInfo = list;
                                model.TotalRecords = list.Count();
                            }
                        }

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEducationListForTMM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);

                    }
                }
                else if (Designation == "Team Member-Cri")
                {
                    if (version == "New")
                    {
                        string department = "Criminal";
                        string statusid = "";
                        if (CheckStatus == "WIP" || CheckStatus == "Reverification")
                        {
                            statusid = "20";
                        }
                        else if (CheckStatus == "Closed")
                        {
                            statusid = "24";
                        }

                        else if (CheckStatus == "InsufficentRaised")
                        {
                            statusid = "53";
                        }

                        else if(CheckStatus == "InsufficencyResolved")
                        {
                            statusid = "56";
                        }

                        else if (CheckStatus == "Research")
                        {
                            statusid = "24";
                        }
                        else if (CheckStatus == "FundsPending")
                        {
                            statusid = "26";
                        }
                        else if (CheckStatus == "Rejected")
                        {
                            statusid = "21";
                        }
                        else if (CheckStatus == "PVClosed")
                        {
                            statusid = "67";
                        }
                        else
                        {
                            statusid = CheckStatusId;
                        }
                        if (Aging == "Yes")
                        {
                            statusid = "20";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateCriminalListForTMM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                            if (CheckStatus == "Reverification")
                            {
                                var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                                model.PQVerificationInfo = list;
                                model.TotalRecords = list.Count();
                            }
                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateCriminalListForTMM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                }
                else if (Designation == "Team Member-Ref")
                {
                    if (version == "New")
                    {
                        string department = "Reference";
                        string statusid = "";
                        {
                            if (CheckStatus == "WIP" || CheckStatus == "Reverification")
                            {
                                statusid = "20";
                            }
                            else if (CheckStatus == "Closed")
                            {
                                statusid = "24";
                            }

                            else if (CheckStatus == "InsufficentRaised")
                            {
                                statusid = "53";
                            }

                            else if (CheckStatus == "InsufficencyResolved")
                            {
                                statusid = "56";
                            }

                            else if (CheckStatus == "Rejected")
                            {
                                statusid = "21";
                            }
                            else if (CheckStatus == "FundsPending")
                            {
                                statusid = "26";
                            }
                            else
                            {
                                statusid = CheckStatusId;
                            }
                            if (Aging == "Yes")
                            {
                                statusid = "20";
                                model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                            }
                            else
                            {
                                model = repoVerificationInfo.GetCandidateReferenceListForTMM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                                if (CheckStatus == "Reverification")
                                {
                                    var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                                    model.PQVerificationInfo = list;
                                    model.TotalRecords = list.Count();
                                }
                            }

                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateReferenceListForTMM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                }
                else if (Designation == "Team Member-Ide")
                {
                    if (version == "New")
                    {
                        string department = "National";
                        string statusid = "";
                        if (CheckStatus == "WIP" || CheckStatus == "Reverification")
                        {
                            statusid = "20";
                        }
                        else if (CheckStatus == "Closed")
                        {
                            statusid = "24";
                        }

                        else if (CheckStatus == "InsufficentRaised")
                        {
                            statusid = "53";
                        }

                        else if (CheckStatus == "InsufficencyResolved")
                        {
                            statusid = "56";
                        }
                        else if (CheckStatus == "FundsPending")
                        {
                            statusid = "26";
                        }
                        else if (CheckStatus == "Rejected")
                        {
                            statusid = "21";
                        }
                        else
                        {
                            statusid = CheckStatusId;
                        }
                        if (Aging == "Yes")
                        {
                            statusid = "20";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateIdentityListForTMM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                            if (CheckStatus == "Reverification")
                            {
                                var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                                model.PQVerificationInfo = list;
                                model.TotalRecords = list.Count();
                            }
                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateIdentityListForTMM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                }
                else if (Designation == "Team Member-SPL")
                {
                    if (version == "New")
                    {
                        string department = "Special Check";
                        string statusid = "";
                        if (CheckStatus == "WIP" || CheckStatus == "Reverification")
                        {
                            statusid = "20";
                        }
                        else if (CheckStatus == "Closed")
                        {
                            statusid = "24";
                        }

                        else if (CheckStatus == "InsufficentRaised")
                        {
                            statusid = "53";
                        }

                        else if (CheckStatus == "InsufficencyResolved")
                        {
                            statusid = "56";
                        }
                        else if (CheckStatus == "FundsPending")
                        {
                            statusid = "26";
                        }
                        else if (CheckStatus == "Rejected")
                        {
                            statusid = "21";
                        }
                        else
                        {
                            statusid = CheckStatusId;
                        }
                        if (Aging == "Yes")
                        {
                            statusid = "20";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateSpecialCheckListForTMM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                            if (CheckStatus == "Reverification")
                            {
                                var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                                model.PQVerificationInfo = list;
                                model.TotalRecords = list.Count();
                            }

                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateSpecialCheckListForTMM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                }

                else if (Designation == "Auto Revert-QC")
                {
                    if (version == "New")
                    {
                        string department = "Employment";
                        string statusid = "";
                        if (CheckStatus == "AutoEmailer")
                        {
                            statusid = "74";
                        }

                        else if (CheckStatus == "RevNotRec")
                        {
                            statusid = "81";
                        }
                        else if (CheckStatus == "WIPALL")
                        {
                            statusid = "73,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89";
                        }
                        // WIPALL
                        else
                        {
                            statusid = CheckStatusId;
                        }
                        if (Aging == "Yes")
                        {
                            statusid = "20";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateEmploymentListForTMM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateSpecialCheckListForTMM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);

                    }
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        public ActionResult TeamMemberFundsResolved(int page = 1, string sort = "", string sortDir = "desc", string Search = "", short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "",  string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0, string version = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();

                //BindClientList();
                BindClientListBaseOnLogin(teamMemerRowId);
                CheckStatusOptionDropDown();
                CaseStatusOptionDropDown();

                if (Designation == "Team Member-Add")
                {
                    string department = "Address";
                    string statusid = "";
                    // 61 for funds request closed (resolved)
                    if (CheckStatus == "FundsResolved")
                    {
                        statusid = "61";
                    }
                   
                    BindCities();
                    BindStates();
                    BindPartneres();
                    if (version == "New")
                    {


                        model = repoVerificationInfo.GetCandidateAllFundsResolved(page, pageSize, teamMemerRowId,  sortDir.ToLower(), CheckStatus);

                    }

                }
                else if (Designation == "Team Member-Emp")
                {
                    if (version == "New")
                    {
                        string department = "Employment";
                        string statusid = "";
                        // 61 for funds request closed (resolved)
                        
                        model = repoVerificationInfo.GetCandidateAllFundsResolved(page, pageSize, teamMemerRowId, sortDir.ToLower(), department);
                    }

                }
                else if (Designation == "Team Member-Edu")
                {
                    if (version == "New")
                    {
                        string department = "Education";
                        string statusid = "";
                        // 61 for funds request closed (resolved)
                       
                        model = repoVerificationInfo.GetCandidateAllFundsResolved(page, pageSize, teamMemerRowId, sortDir.ToLower(), department);

                    }

                }
                else if (Designation == "Team Member-Cri")
                {
                    if (version == "New")
                    {
                        string department = "Criminal";
                        string statusid = "";
                        // 61 for funds request closed (resolved)
                        
                        model = repoVerificationInfo.GetCandidateAllFundsResolved(page, pageSize, teamMemerRowId, sortDir.ToLower(), department);
                    }

                }
                else if (Designation == "Team Member-Ref")
                {
                    if (version == "New")
                    {
                        string department = "Reference";
                        string statusid = "";
                        // 61 for funds request closed (resolved)
                        if (CheckStatus == "FundsResolved")
                        {
                            statusid = "61";
                        }



                        model = repoVerificationInfo.GetCandidateAllFundsResolved(page, pageSize, teamMemerRowId, sortDir.ToLower(), department);
                    }

                }
                else if (Designation == "Team Member-Ide")
                {
                    if (version == "New")
                    {
                        string department = "National";
                        string statusid = "";
                        // 61 for funds request closed (resolved)
                        if (CheckStatus == "FundsResolved")
                        {
                            statusid = "61";
                        }
                        model = repoVerificationInfo.GetCandidateAllFundsResolved(page, pageSize, teamMemerRowId, sortDir.ToLower(), department);
                    }

                }
                else if (Designation == "Team Member-SPL")
                {
                    if (version == "New")
                    {
                        string department = "Special";
                        string statusid = "";
                        // 61 for funds request closed (resolved)
                        if (CheckStatus == "FundsResolved")
                        {
                            statusid = "61";
                        }
                        model = repoVerificationInfo.GetCandidateAllFundsResolved(page, pageSize, teamMemerRowId, sortDir.ToLower(), department);
                    }

                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }


        [SessionExpireFilterAttribute]
        [SessionExpireFilterAttribute]
        public ActionResult DepartmentTeamMemberQCChecks(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string version = "New", short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0,int CheckStatusId=0,string Aging="")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                if(Aging=="True")
                {
                    return RedirectToAction("Index", "Verification");
                }
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();

                //BindClientList();
                BindClientListBaseOnLogin(teamMemerRowId);
                CheckStatusOptionDropDown();
                CaseStatusOptionDropDown();

                string statusid = "";
                if (CheckStatus == "Pending")
                    statusid = "24";
                else if (CheckStatus == "Closed")
                    statusid = "36";
                else if (CheckStatus == "FundsPending")
                    statusid = "58";
                else if (CheckStatus == "InsuffPending")
                    statusid = "53";
                else if (CheckStatus == "Rejected")
                    statusid = "25";
                else if (CheckStatus == "Review")
                    statusid = "70";
                else if (CheckStatus == "ReviewResolved")
                    statusid = "71";
                else if (CheckStatus == "AutoEmailer")
                    statusid = "74";

                else if (CheckStatus == "RevNotRec")
                    statusid = "81";
                else if (CheckStatus == "WIPALL")
                    statusid = "73,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90";
                
                else
                    statusid = CheckStatusId.ToString();
                string department = "";
                if (Designation == "Team Member-Add-QC")
                {
                    department = "Address";

                    BindCities();
                    BindStates();
                    BindPartneres();
                    if (version == "New")
                    {
                        if (Aging == "Yes")
                        {
                            statusid = "24";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateListForTMMQCNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, EmployerName, Allocatedto, CaseStatus, State, City, Partner);
                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateAddressListForTMMQC(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, EmployerName, Allocatedto, CaseStatus, State, City, Partner);

                    }
                }
                else if (Designation == "Team Member-Emp-QC")
                {
                    department = "Employment";
                    if (version == "New")
                    {
                        if(Aging=="Yes")                            
                        {
                            statusid = "24";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateListForTMMQCNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, EmployerName, Allocatedto, CaseStatus, State, City, Partner);

                        }

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEmploymentListForTMMQC(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);
                    }
                }
                else if (Designation == "Team Member-Edu-QC")
                {
                    department = "Education";
                    if (version == "New")
                    {
                        if (Aging == "Yes")
                        {
                            statusid = "24";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateListForTMMQCNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, EmployerName, Allocatedto, CaseStatus, State, City, Partner);
                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEducationListForTMMQC(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus, EmployerName, Allocatedto);
                    }
                }
                else if (Designation == "Team Member-Cri-QC")
                {
                    department = "Criminal";
                    if (version == "New")
                    {
                        if (Aging == "Yes")
                        {
                            statusid = "24";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateListForTMMQCNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, EmployerName, Allocatedto, CaseStatus, State, City, Partner);
                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateCriminalListForTMMQC(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                    }
                }
                else if (Designation == "Team Member-Ref-QC")
                {
                    department = "Reference";
                    if (version == "New")
                    {
                        if (Aging == "Yes")
                        {
                            statusid = "24";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateListForTMMQCNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, EmployerName, Allocatedto, CaseStatus, State, City, Partner);
                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateReferenceListForTMMQC(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                    }
                }
                else if (Designation == "Team Member-Ide-QC")
                {
                    department = "National";
                    if (version == "New")
                    {
                        if (Aging == "Yes")
                        {
                            statusid = "24";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateListForTMMQCNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, EmployerName, Allocatedto, CaseStatus, State, City, Partner);
                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateIdentityListForTMMQC(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                    }
                }
                else if (Designation == "Team Member-SPL-QC")
                {
                    department = "Special Check";
                    if (version == "New")
                    {
                        if (Aging == "Yes")
                        {
                            statusid = "24";
                            model = repoVerificationInfo.GetAgingDetails_Action(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, statusid);
                        }
                        else
                        {
                            model = repoVerificationInfo.GetCandidateListForTMMQCNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, EmployerName, Allocatedto, CaseStatus, State, City, Partner);
                        }
                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateSpecialCheckListForTMMQC(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus, tatdays, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, DCheckStatus, CaseStatus);
                    }
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        #endregion

        #region ******** Manage By Department Team Member ********

        #region ******** Partner Allocation By Address TeamMember ********

        [SessionExpireFilterAttribute]
        public ActionResult AddressTeamMemberAllocationToPartnerMgr(FormCollection form,int page = 1, string sort = "", string sortDir = "desc", string Search = "", string version="New",string VerificationIds = "", short AllocatePTR = 0, short StateRowId = 0, string CheckStatus = "", short LocationId = 0)
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                short LocationRowID1 = Convert.ToInt16( form["LocationId"]);
                //LocationRowID = LocationRowID1;
                int pageSize = clsGlobalCommon.PageSize;

                
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                string department = "Address";
                string  statusid = "";
                if (CheckStatus == "Pending")
                {
                    statusid = "20";
                }


                else if(CheckStatus== "PartnerClosed")
                {
                    statusid = "38";
                }
                else if(CheckStatus == "NotDoable")
                {
                    statusid = "72";
                }
                else if(CheckStatus == "Reverification")
                {
                    statusid = "20";
                }
                BindStateVendor(teamMemerRowId, CheckStatus);
                GetVendorDDL(StateRowId);

                BindCityByStateId(StateRowId,teamMemerRowId);
                if (Designation == "Team Member-Add")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocatePTR > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateAddressStatusTeamMemberViewModel addModel = new PQUpdateAddressStatusTeamMemberViewModel();
                        addModel.ModifiedBy = teamMemerRowId;
                        addModel.ModifiedDate = DateTime.Now;
                        addModel.CheckStatus = "Allocated-PTR";
                        addModel.PTRMgrAllocatedTo = AllocatePTR;

                        foreach (var item in verIDs)
                        {
                            addModel.AddressRowID = item;
                            var addDetails = repoAddressInfo.GetPQAddressDetailsById(item);

                            repoAddressInfo.UpdateAddressStatusByTeamMember(addModel);
                            cc.UpdateCheckStatus(addDetails.PersonalRowID, addDetails.SubCheckRowID, 1, 27, "Check", teamMemerRowId);
                            cc.UpdatePartnerUserAllocation(addModel.AddressRowID);// Set partner login rowId 0 in pqaddressverPartner
                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(addDetails.PersonalRowID, "AddressTeamMemberAllocationToPartnerMgr", addModel.CheckStatus, addDetails.UniqueComponentID, "Update");
                            #endregion

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(addDetails.PersonalRowID, addDetails.SubCheckRowID, addModel.CheckStatus, "Team Member Allocated To Partner");
                            #endregion
                        }
                    }
                    if (version == "New")
                    {
                        model = repoVerificationInfo.GetCandidateAddressListForPTRMgrAllocationByTM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search,department,statusid, teamMemerRowId, StateRowId, CheckStatus, LocationId);
                        if (CheckStatus == "Reverification")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count();
                        }
                        else if(CheckStatus == "Pending")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType != "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count();
                        }
                    }
                    else
                    {
                        //model = repoVerificationInfo.GetCandidateAddressListForPTRMgrAllocationByTM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, StateRowId, CheckStatus);
                        model = repoVerificationInfo.GetCandidateAddressListForPTRMgrAllocationByTM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, StateRowId, CheckStatus, LocationId);

                    }
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        #endregion

        #region ******** Address Verification By Team Member ********

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult AddAddressInfoByTeamMember(int id = 0,string msg="", int PersonalRowID = 0, int subCheckId = 0, int ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowID));
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult AddAddressInfoByTeamMember(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            int SubCheckid = Convert.ToInt32(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPTM";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckId = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("AddAddressInfoByTeamMember", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });

        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult VerificationCloserDetails(int id = 0, int PersonalRowID = 0, int SubcheckRowID = 0, short ClientRowID = 0)
        {
            VerificationCloserViewModel model = new VerificationCloserViewModel();

            try
            {
                DataTable dtOpd = new DataTable();
                dtOpd = cc.GetOpsTeamValue(PersonalRowID, SubcheckRowID);
                if (dtOpd.Rows.Count > 0)
                {
                    BindDisposition(2, ClientRowID);
                }
                else
                {
                    ViewBag.ErrorMsg = "No detail found!";
                }
            }

            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult VerificationCloserDetails(VerificationCloserViewModel model,FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model!=null)
                    {
                        short TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.InfSuffRaiseBy = TeamMemberRowID;
                        model.InfSuffRaisedDate = DateTime.Now;
                        model.StoppedCheckStatus = 1;
                        model.StoppedCheckBy = TeamMemberRowID;
                        model.StoppedCheckDate = DateTime.Now;
                        int SeverityId = Convert.ToInt32(formCollection["SeverityId"].ToString());

                        //repoAddressVer.AddVerificationCloserDetails(model);
                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.ActionHistory);
                        ClientCustomize cc = new ClientCustomize();
                        if(model.CheckStatus=="Completed")
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.UpdatePQCandidateChecksColorCode(Convert.ToInt32(model.DispositionID), SeverityId, model.ColorName, model.PersonalRowID, model.SubCheckRowID);
                        }
                        else
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                        }
                        #endregion
                        ModelState.Clear();
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        //return RedirectToAction("AddAddressInfoByTeamMember", new { id = model.AddressRowID });
                        return RedirectToAction("AddAddressInfoByTeamMember", new { PersonalRowID = model.PersonalRowID, subCheckId = model.SubCheckRowID, ClientRowId = model.ClientRowID });


                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UplodVerificationDocs(short scid = 0, int pe = 0, string cstatus = "")
        {
            VerifiedUploadDocViewModel model = new VerifiedUploadDocViewModel();
            try
            {
                if (scid > 0 && pe > 0)
                {
                    //model.CheckfiledRowID = id;
                    //model.ClientRowID = cid;
                    //model.CheckFamilyRowID = cfid;
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Address id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UplodVerificationDocs(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        if (model.DocumentType == null)
                        {
                            model.DocumentType = "Varification Document";
                        }
                        model.DocumentUploadFrom = "Verified";
                        model.StatusDocxConvert = 0;
                        model.StatusPDF2DOCXConvert = 0;
                        model.UploadDate = DateTime.Now;
                        model.Status = 1;
                        string ext = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                        cc.UpdateImageArtifact(model.PersonalRowID, model.SubCheckRowID);
                        proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + model.SubCheckRowID + "_" + model.UploadDate.Value.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                        model.UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = proImg;
                        model.FilePath = FldPathDoc;
                        repoPQVerifiedUploadDoc.VerifiedUploadDoc(model);
                        int Res = repoPQVerifiedUploadDoc.SaveChanges();
                        if (Res > 0)
                        {
                            #region Check Action History Save                           
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Verification information Upload Doc ");
                            #endregion
                            ModelState.Clear();
                            TempData["SuccessMsg"] = "Record Added Successfully!";
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "There is some problem to save record. Please try again later!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "please select upload doc!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        //Get Personal Details and Get Special Instrucation And In Address Verification 
        [NonAction]
        void AddressVerList(int id = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                //Bind AddressVer Details 
                ViewBag.modelBind = repoAddressVer.GetAddressForTM(id);

                ViewBag.ListAddressVer = repoDDL.GetAntecedentVerList(ClientRowId, ChFamilRowId, subCheckId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        #endregion

        #region ******** Employment Verifiation By TeamMember ********

        [SessionExpireFilterAttribute]
        public ActionResult AddEmploymentInfoByTeamMember(int id = 0,bool Aging = false,string msg="",int PersonalRowID=0,short subCheckId=0,int ClientRowId=0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowID));
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            BindDocumentList(PersonalRowID, subCheckId);
            BindMailList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            //GetEmploymentList();
            return View(marksModel);
        }

        public void GetCaseDocumentList(int personalRowID = 0)
        {
            try
            {
                ViewBag.CaseDocList = repoPQVerifiedUploadCaseDoc.GetCaseDocumentListByPersonalId(personalRowID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult AddEmploymentInfoByTeamMember(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            int SubCheckid = Convert.ToInt32(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPTM";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckId = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("AddEmploymentInfoByTeamMember",new { PersonalRowID= ViewBag.PersonalRowid, subCheckId= ViewBag.SubCheckId, ClientRowId= ViewBag.clientRowId });
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult EmploymentVerCloserDetails(int id = 0, int PersonalRowID=0,int SubcheckRowID=0,short ClientRowID=0)
        {
            EmploymentVerCloserViewModel model = new EmploymentVerCloserViewModel();
            try
            {
                DataTable dtOpd = new DataTable();
                dtOpd=cc.GetOpsTeamValue(PersonalRowID, SubcheckRowID);
                if (dtOpd.Rows.Count>0)
                {
                    BindDisposition(12, ClientRowID);                    
                }
                else
                {
                    ViewBag.ErrorMsg = "No detail found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult EmploymentVerCloserDetails(EmploymentVerCloserViewModel model,FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string Designation = Session["Designation"].ToString();
                    if (model != null )
                    {
                        int SeverityId = Convert.ToInt32(formCollection["SeverityId"].ToString());
                        //short TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
                        //model.InfSuffRaiseBy = TeamMemberRowID;
                        //model.InfSuffRaisedDate = DateTime.Now;
                        //model.StoppedCheckStatus = 1;
                        //model.StoppedCheckBy = TeamMemberRowID;
                        //model.StoppedCheckDate = DateTime.Now;
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if(Dtstatusid.Rows.Count>0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        //repoEmploymentVer.AddEmploymentVerCloserDetails(model);
                        if (model.CheckStatus == "Completed" && model.ColorName.ToLower()=="green" && Designation!= "Team Member-Emp-QC")
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.UpdatePQCandidateChecksColorCode(Convert.ToInt32(model.DispositionID), SeverityId, model.ColorName, model.PersonalRowID, model.SubCheckRowID);
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.ActionHistory);
                        }
                        
                        else if (checkstatusid == 74 && model.ColorName.ToLower() == "green")
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                        }
                        else
                        {
                            if(model.ColorName.ToLower() != "green")
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 70, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "RW-Approval-TL", model.ActionHistory);

                            }
                        }
                        #region Check Action History Save                           
                        

                        #endregion

                        ModelState.Clear();
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        if (model.Aging == true)
                            return RedirectToAction("Index");
                        else
                        //return RedirectToAction("AddEmploymentInfoByTeamMember", new { id = model.EmploymentRowID });
                        return RedirectToAction("AddEmploymentInfoByTeamMember", new { PersonalRowID = model.PersonalRowID, subCheckId = model.SubCheckRowID, ClientRowId = model.ClientRowID });

                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UplodEmploymentVerDocs(short scid = 0, int pe = 0, string cstatus = "")
        {
            VerifiedUploadDocViewModel model = new VerifiedUploadDocViewModel();
            try
            {
                if (scid > 0 && pe > 0)
                {
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                }
                else
                {
                    ViewBag.ErrorMsg = "invlid Employment id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UplodEmploymentVerDocs(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        if (model.DocumentType == null)
                        {
                            model.DocumentType = "Varification Document";
                        }
                        model.DocumentUploadFrom = "Verified";
                        model.StatusDocxConvert = 0;
                        model.StatusPDF2DOCXConvert = 0;
                        model.UploadDate = DateTime.Now;
                        model.Status = 1;
                        string ext = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                        proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + model.SubCheckRowID + "_" + model.UploadDate.Value.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                        model.UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = proImg;
                        cc.UpdateImageArtifact(model.PersonalRowID,model.SubCheckRowID);
                        repoPQVerifiedUploadDoc.VerifiedUploadDoc(model);
                        int Res = repoPQVerifiedUploadDoc.SaveChanges();
                        if (Res > 0)
                        {
                            #region Check Action History Save                           
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Verification information Upload Doc ");
                            #endregion
                            ModelState.Clear();
                            ViewBag.SuccessMsg = "Record Added Successfully!";
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "There is no Change!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "please select upload doc!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [NonAction]
        void EmployementVerList(int EmploymentRowID = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                
                //Bind EmployementVer Details 
                var modelBind = repoEmploymentVer.GetEmploymentForTM(EmploymentRowID);

                ViewBag.modelBind = modelBind;
                ViewBag.ListEmploymentVer = repoDDL.GetAntecedentVerList(ClientRowId, ChFamilRowId, subCheckId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        #endregion

        #region ******** Education Verifiation By TeamMember ********

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult AddEducationInfoByTeamMember(int id = 0, bool Aging = false,string msg="", int PersonalRowID = 0, int subCheckId = 0, int ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowID));
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult AddEducationInfoByTeamMember(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            int SubCheckid = Convert.ToInt32(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPTM";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckId = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("AddEducationInfoByTeamMember", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult EducationVerCloserDetails(int id = 0, int PersonalRowID = 0, int SubcheckRowID = 0, short ClientRowID = 0)
        {
            EducationVerCloserViewModel model = new EducationVerCloserViewModel();
            try
            {
                DataTable dtOpd = new DataTable();
                dtOpd = cc.GetOpsTeamValue(PersonalRowID, SubcheckRowID);
                if (dtOpd.Rows.Count > 0)
                {
                    BindDisposition(11, ClientRowID);
                }
                else
                {
                    ViewBag.ErrorMsg = "No detail found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult EducationVerCloserDetails(EducationVerCloserViewModel model, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model!=null)
                    {
                        short TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
                        int SeverityId = Convert.ToInt32(formCollection["SeverityId"].ToString());
                        //repoEducationVer.AddEducationVerCloserDetails(model);
                        //if (model.CheckStatus == "Completed")
                        //    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                        //else
                        //    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 70, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                        if (model.CheckStatus == "Completed" && model.ColorName.ToLower() == "green" )
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.UpdatePQCandidateChecksColorCode(Convert.ToInt32(model.DispositionID), SeverityId, model.ColorName, model.PersonalRowID, model.SubCheckRowID);
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.ActionHistory);
                        }                       
                        else
                        {
                            if (model.ColorName.ToLower() != "green")
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 70, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "RW-Approval-TL", model.ActionHistory);

                            }
                        }


                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.ActionHistory);
                        #endregion

                        ModelState.Clear();
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        return RedirectToAction("AddEducationInfoByTeamMember", new { PersonalRowID = model.PersonalRowID, subCheckId = model.SubCheckRowID, ClientRowId = model.ClientRowID });

                        //return RedirectToAction("AddEducationInfoByTeamMember", new { id = model.EducationRowID, ClientRowId = model.ClientRowID, ChFamilRowId = model.CheckFamilyRowID, subCheckId = model.SubCheckRowID, PersonalRowID = model.PersonalRowID, });
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UplodEducationVerDocs(short scid = 0, int pe = 0, string cstatus = "")
        {
            VerifiedUploadDocViewModel model = new VerifiedUploadDocViewModel();
            try
            {
                if (scid > 0 && pe > 0)
                {
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UplodEducationVerDocs(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        if (model.DocumentType == null)
                        {
                            model.DocumentType = "Varification Document";
                        }
                        model.DocumentUploadFrom = "Verified";
                        model.StatusDocxConvert = 0;
                        model.StatusPDF2DOCXConvert = 0;
                        model.UploadDate = DateTime.Now;
                        model.Status = 1;
                        string ext = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                        cc.UpdateImageArtifact(model.PersonalRowID, model.SubCheckRowID);
                        proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + model.SubCheckRowID + "_" + model.UploadDate.Value.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                        model.UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = proImg;
                        repoPQVerifiedUploadDoc.VerifiedUploadDoc(model);
                        int Res = repoPQVerifiedUploadDoc.SaveChanges();
                        if (Res > 0)
                        {
                            #region Check Action History Save                           
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Education Verification information Upload Doc ");
                            #endregion
                            ModelState.Clear();
                            ViewBag.SuccessMsg = "Record Added Successfully!";
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "There is some problem to save record. Please try again later!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "please select upload doc!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [NonAction]
        void EducationVerList(int EducationRowId = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var modelBind = repoEducationVer.GetEducationForTM(EducationRowId);
                ViewBag.modelBind = modelBind;
                ViewBag.ListEducationVer = repoDDL.GetAntecedentVerList(ClientRowId, ChFamilRowId, subCheckId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ******** Criminal Verifiation By TeamMember ********

        [SessionExpireFilterAttribute]
        public ActionResult AddCriminalInfoByTeamMember(int id = 0,string msg="", int PersonalRowID = 0, int subCheckId = 0, int ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowID));
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult AddCriminalInfoByTeamMember(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            int SubCheckid = Convert.ToInt32(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPTM";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckId = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("AddEducationInfoByTeamMember", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });

        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult CriminalVerCloserDetails(int id = 0, int PersonalRowID = 0, int SubcheckRowID = 0, short ClientRowID = 0)
        {
            CriminalVerCloserViewModel model = new CriminalVerCloserViewModel();
            try
            {
                DataTable dtOpd = new DataTable();
                dtOpd = cc.GetOpsTeamValue(PersonalRowID, SubcheckRowID);
                if (dtOpd.Rows.Count > 0)
                {
                    BindDisposition(5, ClientRowID);
                }
                else
                {
                    ViewBag.ErrorMsg = "No detail found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult CriminalVerCloserDetails(CriminalVerCloserViewModel model, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.CriminalVerRowID > 0)
                    {
                        short TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
                        int SeverityId = Convert.ToInt32(formCollection["SeverityId"].ToString());
                        //repoCriminalVer.AddCriminalVerCloserDetails(model);
                        if (model.CheckStatus == "Completed" && model.ColorName.ToLower() == "green")
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.UpdatePQCandidateChecksColorCode(Convert.ToInt32(model.DispositionID), SeverityId, model.ColorName, model.PersonalRowID, model.SubCheckRowID);

                        }
                        else
                        { 
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 70, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                        }

                        //if (model.CheckStatus == "Completed")
                        //    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                        //else
                        //    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.ActionHistory);
                        #endregion

                        ModelState.Clear();
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        //return RedirectToAction("AddCriminalInfoByTeamMember", new { id = model.CriminalRowID });
                        return RedirectToAction("AddCriminalInfoByTeamMember", new { PersonalRowID = model.PersonalRowID, subCheckId = model.SubCheckRowID, ClientRowId = model.ClientRowID });

                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UplodCriminalVerDocs(short scid = 0, int pe = 0, string cstatus = "")
        {
            VerifiedUploadDocViewModel model = new VerifiedUploadDocViewModel();
            try
            {
                if (scid > 0 && pe > 0)
                {
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Criminal id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UplodCriminalVerDocs(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        if (model.DocumentType == null)
                        {
                            model.DocumentType = "Varification Document";
                        }
                        model.DocumentUploadFrom = "Verified";
                        model.StatusDocxConvert = 0;
                        model.StatusPDF2DOCXConvert = 0;
                        model.UploadDate = DateTime.Now;
                        model.Status = 1;
                        string ext = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                        cc.UpdateImageArtifact(model.PersonalRowID, model.SubCheckRowID);
                        proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + model.SubCheckRowID + "_" + model.UploadDate.Value.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                        model.UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = proImg;
                        repoPQVerifiedUploadDoc.VerifiedUploadDoc(model);
                        int Res = repoPQVerifiedUploadDoc.SaveChanges();
                        if (Res > 0)
                        {
                            #region Check Action History Save                           
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Crimianl Verification information Upload Doc ");
                            #endregion

                            ModelState.Clear();
                            ViewBag.SuccessMsg = "Record Added Successfully!";
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "There is No Changes!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "please select upload doc!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [NonAction]
        void CriminalVerList(int id = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            ViewBag.modelBind = repoCriminalVer.GetCriminalForTM(id);
            ViewBag.ListCriminalVer = repoDDL.GetAntecedentVerList(ClientRowId, ChFamilRowId, subCheckId);
        }

        #endregion

        #region ******** Reference Verification By Team Member ********

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult AddReferenceInfoByTeamMember(int id = 0, bool Aging = false,string msg="", int PersonalRowID = 0, int subCheckId = 0, int ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;


            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult AddReferenceInfoByTeamMember(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            int SubCheckid = Convert.ToInt32(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPTM";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckId = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("AddReferenceInfoByTeamMember", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult ReferenceVerCloserDetails(int id = 0 ,int PersonalRowID = 0, int SubcheckRowID = 0, short ClientRowID = 0)
        {
            ReferenceVerCloserViewModel model = new ReferenceVerCloserViewModel();
            try
            {
                DataTable dtOpd = new DataTable();
                dtOpd = cc.GetOpsTeamValue(PersonalRowID, SubcheckRowID);
                if (dtOpd.Rows.Count > 0)
                {
                    BindDisposition(30, ClientRowID);
                }
                else
                {
                    ViewBag.ErrorMsg = "No detail found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult ReferenceVerCloserDetails(ReferenceVerCloserViewModel model, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.ReferenceVerRowID > 0)
                    {
                        short TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
                        int SeverityId = Convert.ToInt32(formCollection["SeverityId"].ToString());

                        //repoReferenceVer.AddReferenceVerCloserDetails(model);
                        //if(model.CheckStatus=="Completed")
                        //cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", TeamMemberRowID);
                        //else
                        //    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 19, "Check", TeamMemberRowID);
                        if (model.CheckStatus == "Completed" && model.ColorName.ToLower() == "green")
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", TeamMemberRowID);
                            cc.UpdatePQCandidateChecksColorCode(Convert.ToInt32(model.DispositionID), SeverityId, model.ColorName, model.PersonalRowID, model.SubCheckRowID);

                        }
                        else
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 70, "Check", TeamMemberRowID);                            
                        }

                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.ActionHistory);
                        #endregion
                        ModelState.Clear();
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        //return RedirectToAction("AddReferenceInfoByTeamMember", new { id = model.ReferenceCheckRowID });
                        return RedirectToAction("AddReferenceInfoByTeamMember", new { PersonalRowID = model.PersonalRowID, subCheckId = model.SubCheckRowID, ClientRowId = model.ClientRowID });

                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UplodReferenceVerDocs(short scid = 0, int pe = 0, string cstatus = "")
        {
            VerifiedUploadDocViewModel model = new VerifiedUploadDocViewModel();
            try
            {
                if (scid > 0 && pe > 0)
                {
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Criminal id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UplodReferenceVerDocs(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        if (model.DocumentType == null)
                        {
                            model.DocumentType = "Varification Document";
                        }
                        model.DocumentUploadFrom = "Verified";
                        model.StatusDocxConvert = 0;
                        model.StatusPDF2DOCXConvert = 0;
                        model.UploadDate = DateTime.Now;
                        model.Status = 1;
                        string ext = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                        cc.UpdateImageArtifact(model.PersonalRowID, model.SubCheckRowID);
                        proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + model.SubCheckRowID + "_" + model.UploadDate.Value.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                        model.UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = proImg;
                        repoPQVerifiedUploadDoc.VerifiedUploadDoc(model);
                        int Res = repoPQVerifiedUploadDoc.SaveChanges();
                        if (Res > 0)
                        {
                            #region Check Action History Save                           
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Reference Verification information Upload Doc ");
                            #endregion
                            ModelState.Clear();
                            ViewBag.SuccessMsg = "Record Added Successfully!";
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "There is some problem to save record. Please try again later!";
                        }
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "please select upload doc!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [NonAction]
        void ReferenceVerList(int id = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            //Bind AddressVer Details 
            ViewBag.modelBind = repoReferenceVer.GetReferenceForTM(id);

            ViewBag.ListReferenceVer = repoDDL.GetAntecedentVerList(ClientRowId, ChFamilRowId, subCheckId);
        }
        #endregion

        #region ******** NationalIdentity Verifiation By TeamMember ********

        [SessionExpireFilterAttribute]
        public ActionResult AddNationalIdentityInfoByTeamMember(int id = 0, bool Aging = false,string msg="", int PersonalRowID = 0, int subCheckId = 0, int ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult AddNationalIdentityInfoByTeamMember(FormCollection formCollection)
        {

            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            int SubCheckid = Convert.ToInt32(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPTM";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckId = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("AddNationalIdentityInfoByTeamMember", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult NationalIdentityVerCloserDetails(int id = 0, int PersonalRowID = 0, int SubcheckRowID = 0, short ClientRowID = 0)
        {
            NationalIdentityVerCloserViewModel model = new NationalIdentityVerCloserViewModel();
            try
            {
                DataTable dtOpd = new DataTable();
                dtOpd = cc.GetOpsTeamValue(PersonalRowID, SubcheckRowID);
                if (dtOpd.Rows.Count > 0)
                {
                    BindDisposition(26, ClientRowID);
                }
                else
                {
                    ViewBag.ErrorMsg = "No detail found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult NationalIdentityVerCloserDetails(NationalIdentityVerCloserViewModel model,FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.NationalIdentityVerRowID > 0)
                    {
                        short TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
                        int SeverityId = Convert.ToInt32(formCollection["SeverityId"].ToString());

                        repoNationalIdentityVer.AddNationalIdentityVerCloserDetails(model);
                        if (model.CheckStatus == "Completed" && model.ColorName.ToLower() == "green")
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.UpdatePQCandidateChecksColorCode(Convert.ToInt32(model.DispositionID), SeverityId, model.ColorName, model.PersonalRowID, model.SubCheckRowID);

                        }
                        else
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 70, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));                           
                        }
                        //if (model.CheckStatus == "Completed")
                        //    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                        //else
                        //    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.ActionHistory);
                        #endregion
                        ModelState.Clear();
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        //return RedirectToAction("AddNationalIdentityInfoByTeamMember", new { id = model.NationalIdentityRowID });
                        return RedirectToAction("AddNationalIdentityInfoByTeamMember", new { PersonalRowID = model.PersonalRowID, subCheckId = model.SubCheckRowID, ClientRowId = model.ClientRowID });

                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UplodNationalIdentityVerDocs(short scid = 0, int pe = 0, string cstatus = "")
        {
            VerifiedUploadDocViewModel model = new VerifiedUploadDocViewModel();
            try
            {
                if (scid > 0 && pe > 0)
                {
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid National Identity id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UplodNationalIdentityVerDocs(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        if (model.DocumentType == null)
                        {
                            model.DocumentType = "Varification Document";
                        }
                        model.DocumentUploadFrom = "Verified";
                        model.StatusDocxConvert = 0;
                        model.StatusPDF2DOCXConvert = 0;
                        model.UploadDate = DateTime.Now;
                        model.Status = 1;
                        string ext = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                        cc.UpdateImageArtifact(model.PersonalRowID, model.SubCheckRowID);
                        proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + model.SubCheckRowID + "_" + model.UploadDate.Value.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                        model.UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = proImg;
                        repoPQVerifiedUploadDoc.VerifiedUploadDoc(model);
                        int Res = repoPQVerifiedUploadDoc.SaveChanges();
                        if (Res > 0)
                        {
                            #region Check Action History Save                           
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "National Identity Verification information Upload Doc ");
                            #endregion
                            ModelState.Clear();
                            ViewBag.SuccessMsg = "Record Added Successfully!";
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "There is no change!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "please select upload doc!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [NonAction]
        void NationalIdentityVerList(int id = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                ViewBag.modelBind = repoNationalIdentityVer.GetNationalIdentityForTM(id);
                ViewBag.ListNationalIdentityVer = repoDDL.GetAntecedentVerList(ClientRowId, ChFamilRowId, subCheckId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region ******** Capture/Show Action History For All Checks ********

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult CheckActionHistory(short scid = 0, int pe = 0)
        {
            try
            {
                ViewBag.CheckActionHistories = repoDDL.GetCheckActionHistory(pe, scid);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return PartialView("_CheckActionHistories");
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult CaptureActionHistoryByVerTM(int id = 0, short cid = 0, short scid = 0, int pe = 0)
        {
            CaptureActionHistoryByVerTM model = new CaptureActionHistoryByVerTM();
            try
            {
                if (id > 0 && cid > 0 && scid > 0 && pe > 0)
                {
                    model.CheckRowID = id;
                    model.ClientRowID = cid;
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    //cc.UpdateCheckStatus(model.PersonalRowID, Convert.ToInt16(model.SubCheckRowID), 1, 64, "Check", -1);
                }
                else
                {
                    TempData["ErrorMsg"] = "invlid id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]

        public ActionResult CaptureActionHistoryByVerTM(CaptureActionHistoryByVerTM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.CheckRowID > 0)
                    {
                        ClientCustomize cc = new ClientCustomize();

                        DataSet dd = new DataSet();

                        int personalRowid = 0;
                        int subCheckRowID = 0;

                        short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                        string Designation = Session["Designation"].ToString();

                        if (Designation == "Team Member-Add")
                        {
                            #region Capture Action For Address

                            dd = cc.getInformationFromDepartmentRowID(model.CheckRowID, "Address");
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }

                            bool IsSuccess = false;

                            PQUpdateAddressTeamMemberViewModel empModel = new PQUpdateAddressTeamMemberViewModel();
                            empModel.ModifiedBy = teamMemerRowId;
                            empModel.ModifiedDate = DateTime.Now;
                            empModel.CheckStatus = model.CheckStatus;
                            empModel.AddressRowID = model.CheckRowID;
                            empModel.ActionRemark = model.ActionHistory;
                            var addDetails = repoAddressInfo.GetPQAddressDetailsById(model.CheckRowID);

                            if (model.CheckStatus == "Re-Visit")
                            {
                                if (repoAddressInfo.IsAddressCheckAllocatedToPartner(model.CheckRowID))
                                {
                                    repoAddressInfo.UpdateAddressByTeamMember(empModel);
                                    ViewBag.SuccessMsg = "Record update successfully";
                                    IsSuccess = true;
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = "Check not allocated to partner yet!";
                                }
                            }
                            else if (model.CheckStatus == "Pre Calling" || model.CheckStatus == "Post Calling")
                            {
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Funds Pending")
                            {
                                repoAddressInfo.UpdateAddressByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 58, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 58, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Insufficent")
                            {
                                repoAddressInfo.UpdateAddressByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 22, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 22, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Rejected")
                            {
                                repoAddressInfo.UpdateAddressByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 64, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 64, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "T-Green")
                            {
                                repoAddressInfo.UpdateAddressByTeamMember(empModel);
                                cc.UpdateTGreencolorCode(personalRowid, subCheckRowID, 2);
                                //cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                //cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Wip On Hold")
                            {
                                repoAddressInfo.UpdateAddressByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 72, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 72, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else
                            {
                                repoAddressInfo.UpdateAddressByTeamMember(empModel);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }

                            if (IsSuccess)
                            {
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "CaptureActionHistoryByVerTM", model.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, empModel.CheckStatus, model.ActionHistory);
                                #endregion  
                            }

                            #endregion
                        }
                        else if (Designation == "Team Member-Emp")
                        {
                            #region Capture Action For Employment

                            bool IsSuccess = false;
                            dd = cc.getInformationFromDepartmentRowID(model.CheckRowID, "Employment");
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }

                            PQUpdateEmploymentTeamMemberViewModel empModel = new PQUpdateEmploymentTeamMemberViewModel();
                            empModel.ModifiedBy = teamMemerRowId;
                            empModel.ModifiedDate = DateTime.Now;
                            empModel.CheckStatus = model.CheckStatus;
                            empModel.EmploymentRowID = model.CheckRowID;
                            empModel.ActionRemark = model.ActionHistory;
                            empModel.Amount = model.Amount;
                            var addDetails = repoEmploymentInfo.GetPQEmploymentDetailsById(model.CheckRowID);

                            if (model.CheckStatus == "SendToResearch")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 23, "Check", teamMemerRowId);
                                SendAlertMassageToReSearcher(Designation, model.ClientRowID, model.CheckRowID, model.ActionHistory);
                                ViewBag.SuccessMsg = "Notification sent to researcher!";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "PV Raised")
                            {
                                short result = repoPQClientTMember.GetClientTeamPVManager(model.ClientRowID, "Physical Verification", "Manager-PV");
                                if (result > 0)
                                {
                                    empModel.PVMgrAllocatedTo = result;
                                    repoEmploymentInfo.UpdateEmploymentInfoByTeamMember(empModel);

                                    cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 28, "Check", teamMemerRowId);
                                    ViewBag.SuccessMsg = "Record update successfully";
                                    IsSuccess = true;
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = "Please assign physical manager for this client";
                                }
                            }
                            if (model.CheckStatus == "Funds Pending")
                            {
                                repoEmploymentInfo.UpdateEmploymentInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 58, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 58, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                cc.InsertFundsAmount(personalRowid, subCheckRowID, empModel.Amount, empModel.ActionRemark, teamMemerRowId.ToString(), teamMemerRowId.ToString());
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Insufficent")
                            {
                                repoEmploymentInfo.UpdateEmploymentInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 53, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 53, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Rejected")
                            {
                                repoEmploymentInfo.UpdateEmploymentInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "T-Green")
                            {
                                repoEmploymentInfo.UpdateEmploymentInfoByTeamMember(empModel);
                                cc.UpdateTGreencolorCode(personalRowid, subCheckRowID, 12);
                                //cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                //cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else
                            {
                                repoEmploymentInfo.UpdateEmploymentInfoByTeamMember(empModel);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }

                            if (IsSuccess)
                            {
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "CaptureActionHistoryByVerTM", model.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, empModel.CheckStatus, model.ActionHistory);
                                #endregion  
                            }

                            #endregion
                        }

                        else if (Designation == "Team Member-Emp-QC")
                        {
                            #region Capture Action For Employment

                            bool IsSuccess = false;
                            dd = cc.getInformationFromDepartmentRowID(model.CheckRowID, "Employment");
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }

                            PQUpdateEmploymentTeamMemberViewModel empModel = new PQUpdateEmploymentTeamMemberViewModel();
                            empModel.ModifiedBy = teamMemerRowId;
                            empModel.ModifiedDate = DateTime.Now;
                            empModel.CheckStatus = model.CheckStatus;
                            empModel.EmploymentRowID = model.CheckRowID;
                            empModel.ActionRemark = model.ActionHistory;
                            var addDetails = repoEmploymentInfo.GetPQEmploymentDetailsById(model.CheckRowID);

                            
                             if (model.CheckStatus == "InAccurate Revert")
                            {
                                repoEmploymentInfo.UpdateEmploymentInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 90, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 90, empModel.ActionRemark, "Auto Revert-QC", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Insufficent")
                            {
                                repoEmploymentInfo.UpdateEmploymentInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 22, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 22, empModel.ActionRemark, "Auto Revert-QC", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else
                            {
                                repoEmploymentInfo.UpdateEmploymentInfoByTeamMember(empModel);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }

                            if (IsSuccess)
                            {
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "CaptureActionHistoryByVerTM", model.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, empModel.CheckStatus, model.ActionHistory);
                                #endregion  
                            }

                            #endregion
                        }
                        else if (Designation == "Team Member-Edu")
                        {
                            #region Capture Action For Education

                            bool IsSuccess = false;
                            dd = cc.getInformationFromDepartmentRowID(model.CheckRowID, "Education");
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }

                            PQUpdateEducationTeamMemberViewModel empModel = new PQUpdateEducationTeamMemberViewModel();
                            empModel.ModifiedBy = teamMemerRowId;
                            empModel.ModifiedDate = DateTime.Now;
                            empModel.CheckStatus = model.CheckStatus;
                            empModel.EducationRowID = model.CheckRowID;
                            empModel.ActionRemark = model.ActionHistory;
                            empModel.Amount = model.Amount;
                            var addDetails = repoEducationInfo.GetPQEducationDetailsById(model.CheckRowID);


                            if (model.CheckStatus == "Funds Pending")
                            {
                                repoEducationInfo.UpdateEducationInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 58, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 58, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                cc.InsertFundsAmount(personalRowid, subCheckRowID, empModel.Amount, empModel.ActionRemark, teamMemerRowId.ToString(), teamMemerRowId.ToString());
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Insufficent")
                            {
                                repoEducationInfo.UpdateEducationInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 53, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 53, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "SendToResearch")
                            {
                                SendAlertMassageToReSearcher(Designation, model.ClientRowID, model.CheckRowID, model.ActionHistory);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 23, "Check", teamMemerRowId);
                                ViewBag.SuccessMsg = "Notification sent to researcher!";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "PV Raised")
                            {
                                short result = repoPQClientTMember.GetClientTeamPVManager(model.ClientRowID, "Physical Verification", "Manager-PV");
                                if (result > 0)
                                {
                                    empModel.PVMgrAllocatedTo = result;
                                    repoEducationInfo.UpdateEducationInfoByTeamMember(empModel);
                                    cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 28, "Check", teamMemerRowId);
                                    ViewBag.SuccessMsg = "Record update successfully";
                                    IsSuccess = true;
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Please assign physical manager for this client";
                                }
                            }
                            else if (model.CheckStatus == "Rejected")
                            {
                                repoEducationInfo.UpdateEducationInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "T-Green")
                            {
                                repoEducationInfo.UpdateEducationInfoByTeamMember(empModel);
                                cc.UpdateTGreencolorCode(personalRowid, subCheckRowID, 11);
                                //cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                //cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else
                            {
                                repoEducationInfo.UpdateEducationInfoByTeamMember(empModel);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }

                            if (IsSuccess)
                            {
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "CaptureActionHistoryByVerTM", model.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, empModel.CheckStatus, model.ActionHistory);
                                #endregion  
                            }

                            #endregion
                        }
                        else if (Designation == "Team Member-Cri")
                        {
                            #region Capture Action For Criminal

                            bool IsSuccess = false;
                            dd = cc.getInformationFromDepartmentRowID(model.CheckRowID, "Criminal");
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }

                            PQUpdateCriminalTeamMemberViewModel empModel = new PQUpdateCriminalTeamMemberViewModel();
                            empModel.ModifiedBy = teamMemerRowId;
                            empModel.ModifiedDate = DateTime.Now;
                            empModel.CheckStatus = model.CheckStatus;
                            empModel.CriminalRowID = model.CheckRowID;
                            empModel.ActionRemark = model.ActionHistory;
                            var addDetails = repoCriminalInfo.GetPQCriminalDetailsById(model.CheckRowID);

                            if (model.CheckStatus == "PV Raised")
                            {
                                short result = repoPQClientTMember.GetClientTeamPVManager(model.ClientRowID, "Physical Verification", "Manager-PV");
                                if (result > 0)
                                {
                                    empModel.PVMgrAllocatedTo = result;
                                    repoCriminalInfo.UpdateCriminalInfoByTeamMember(empModel);
                                    cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 28, "Check", teamMemerRowId);
                                    ViewBag.SuccessMsg = "Record update successfully";
                                    IsSuccess = true;
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Please assign physical manager for this client";
                                }
                            }
                            else if (model.CheckStatus == "Funds Pending")
                            {
                                repoCriminalInfo.UpdateCriminalInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 58, "Check", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Insufficent")
                            {
                                repoCriminalInfo.UpdateCriminalInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 53, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 53, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Rejected")
                            {
                                repoCriminalInfo.UpdateCriminalInfoByTeamMember(empModel);
                              
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "T-Green")
                            {
                                repoCriminalInfo.UpdateCriminalInfoByTeamMember(empModel);
                                cc.UpdateTGreencolorCode(personalRowid, subCheckRowID, 5);
                                //cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                //cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else
                            {
                                repoCriminalInfo.UpdateCriminalInfoByTeamMember(empModel);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }

                            if (IsSuccess)
                            {
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "CaptureActionHistoryByVerTM", model.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, empModel.CheckStatus, model.ActionHistory);
                                #endregion  
                            }

                            #endregion
                        }
                        else if (Designation == "Team Member-Ref")
                        {
                            #region Capture Action For Reference

                            bool IsSuccess = false;
                            dd = cc.getInformationFromDepartmentRowID(model.CheckRowID, "Reference");
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }

                            PQUpdateReferenceTeamMemberViewModel empModel = new PQUpdateReferenceTeamMemberViewModel();
                            empModel.ModifiedBy = teamMemerRowId;
                            empModel.ModifiedDate = DateTime.Now;
                            empModel.CheckStatus = model.CheckStatus;
                            empModel.ReferenceRowID = model.CheckRowID;
                            empModel.ActionRemark = model.ActionHistory;
                            var addDetails = repoReferenceInfo.GetPQReferenceDetailsById(model.CheckRowID);

                            if (model.CheckStatus == "PV Raised")
                            {
                                short result = repoPQClientTMember.GetClientTeamPVManager(model.ClientRowID, "Physical Verification", "Manager-PV");
                                if (result > 0)
                                {
                                    empModel.PVMgrAllocatedTo = result;
                                    repoReferenceInfo.UpdateReferenceInfoByTeamMember(empModel);
                                    ViewBag.SuccessMsg = "Record update successfully";
                                    IsSuccess = true;
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Please assign physical manager for this client";
                                }
                            }
                            else if (model.CheckStatus == "Funds Pending")
                            {
                                repoReferenceInfo.UpdateReferenceInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 58, "Check", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Insufficent")
                            {
                                repoReferenceInfo.UpdateReferenceInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 53, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 53, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Rejected")
                            {
                                repoReferenceInfo.UpdateReferenceInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "T-Green")
                            {
                                repoReferenceInfo.UpdateReferenceInfoByTeamMember(empModel);
                                cc.UpdateTGreencolorCode(personalRowid, subCheckRowID, 30);
                                //cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                //cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else
                            {
                                repoReferenceInfo.UpdateReferenceInfoByTeamMember(empModel);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }

                            if (IsSuccess)
                            {
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "CaptureActionHistoryByVerTM", model.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, empModel.CheckStatus, model.ActionHistory);
                                #endregion  
                            }

                            #endregion
                        }
                        else if (Designation == "Team Member-Ide")
                        {
                            #region Capture Action For National Identity

                            bool IsSuccess = false;
                            dd = cc.getInformationFromDepartmentRowID(model.CheckRowID, "National Identity");
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }
                            PQUpdateNationalIdentityTeamMemberViewModel empModel = new PQUpdateNationalIdentityTeamMemberViewModel();
                            empModel.ModifiedBy = teamMemerRowId;
                            empModel.ModifiedDate = DateTime.Now;
                            empModel.CheckStatus = model.CheckStatus;
                            empModel.NationalIdentityRowID = model.CheckRowID;
                            empModel.ActionRemark = model.ActionHistory;
                            var addDetails = repoNationalIdentInfo.GetPQNationalIdentityDetailsById(model.CheckRowID);

                            if (model.CheckStatus == "PV Raised")
                            {
                                short result = repoPQClientTMember.GetClientTeamPVManager(model.ClientRowID, "Physical Verification", "Manager-PV");
                                if (result > 0)
                                {
                                    empModel.PVMgrAllocatedTo = result;
                                    repoNationalIdentInfo.UpdateNationalIdentityInfoByTeamMember(empModel);
                                    ViewBag.SuccessMsg = "Record update successfully";
                                    IsSuccess = true;
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Please assign physical manager for this client";
                                }
                            }
                            else if (model.CheckStatus == "Funds Pending")
                            {
                                repoNationalIdentInfo.UpdateNationalIdentityInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 58, "Check", teamMemerRowId);                                
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Insufficent")
                            {
                                repoNationalIdentInfo.UpdateNationalIdentityInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 53, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 53, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Rejected")
                            {
                                repoNationalIdentInfo.UpdateNationalIdentityInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "T-Green")
                            {
                                repoNationalIdentInfo.UpdateNationalIdentityInfoByTeamMember(empModel);
                                cc.UpdateTGreencolorCode(personalRowid, subCheckRowID, 26);
                                //cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                //cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else
                            {
                                repoNationalIdentInfo.UpdateNationalIdentityInfoByTeamMember(empModel);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }

                            if (IsSuccess)
                            {
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "CaptureActionHistoryByVerTM", model.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, empModel.CheckStatus, model.ActionHistory);
                                #endregion  
                            }

                            #endregion
                        }
                        else if (Designation == "Team Member-SPL")
                        {
                            #region Capture Action For National Identity

                            bool IsSuccess = false;
                            dd = cc.getInformationFromDepartmentRowID(model.CheckRowID, "Special Checks");
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }

                            PQUpdateSpecialCheckTeamMemberViewModel empModel = new PQUpdateSpecialCheckTeamMemberViewModel();
                            empModel.ModifiedBy = teamMemerRowId;
                            empModel.ModifiedDate = DateTime.Now;
                            empModel.CheckStatus = model.CheckStatus;
                            empModel.SpecialCheckRowID = model.CheckRowID;
                            empModel.ActionRemark = model.ActionHistory;
                            var addDetails = repoSplChkVer.GetPQSpecialCheckDetailsById(model.CheckRowID);

                            if (model.CheckStatus == "PV Raised")
                            {
                                short result = repoPQClientTMember.GetClientTeamPVManager(model.ClientRowID, "Physical Verification", "Manager-PV");
                                if (result > 0)
                                {
                                    empModel.PVMgrAllocatedTo = result;
                                    repoSplChkVer.UpdateSpecialCheckInfoByTeamMember(empModel);
                                    ViewBag.SuccessMsg = "Record update successfully";
                                    IsSuccess = true;
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Please assign physical manager for this client";
                                }
                            }
                            else if (model.CheckStatus == "Funds Pending")
                            {
                                repoSplChkVer.UpdateSpecialCheckInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 58, "Check", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Insufficent")
                            {
                                repoSplChkVer.UpdateSpecialCheckInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 53, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 53, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "Rejected")
                            {
                                repoSplChkVer.UpdateSpecialCheckInfoByTeamMember(empModel);
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else if (model.CheckStatus == "T-Green")
                            {
                                repoSplChkVer.UpdateSpecialCheckInfoByTeamMember(empModel);
                                cc.UpdateTGreencolorCode(personalRowid, subCheckRowID, 40);
                                //cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", teamMemerRowId);
                                //cc.Process_Remarks_Insert(personalRowid, subCheckRowID, 15, empModel.ActionRemark, "Operation-TM", teamMemerRowId);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }
                            else
                            {
                                repoSplChkVer.UpdateSpecialCheckInfoByTeamMember(empModel);
                                ViewBag.SuccessMsg = "Record update successfully";
                                IsSuccess = true;
                            }

                            if (IsSuccess)
                            {
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "CaptureActionHistoryByVerTM", model.CheckStatus, addDetails.UniqueComponentID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, empModel.CheckStatus, model.ActionHistory);
                                #endregion  
                            }

                            #endregion
                        }
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }


        [SessionExpireFilterAttribute]
        [HttpGet]
        public JsonResult GetClientExtraExpensesAllowed(short id = 0)
        {
            try
            {
                var data = repoPQClient.GetClientExtraExpensesAllowedByID(id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ******** Email Templetes for Verifier ********

        //Send Email to Researcher (Education / Employer)

        private void SendAlertMassageToReSearcher(string Designation, short ClientRowID, int CheckRowID, string Remarks)
        {
            bool flag = false;
            string To = string.Empty;
            string Subject = string.Empty;
            string MailSignature = string.Empty;
            string From = string.Empty;
            string DisplayName = string.Empty;
            bool result = false;

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/NotifyResearch.html")))
            {
                body = reader.ReadToEnd();
            }

            if (Designation == "Team Member-Emp")
            {
                var ResearchTM = repoPQClientTMember.GetDepManagerClientToTeam(ClientRowID, "Team Member-Emp-Research").FirstOrDefault();
                if (ResearchTM != null && !string.IsNullOrEmpty(ResearchTM.TeamMemberEmail) && ResearchTM.TeamMemberEmail.Trim().Length > 0)
                {
                    To = ResearchTM.TeamMemberEmail.Trim();
                    flag = true;
                }
                else
                {
                    ViewBag.ErrorMsg = "Employment Research Team Member Not Found!";
                }

                Subject = "Update Company Information!";
                body = body.Replace("{EduEmpType}", "Company");
                string CompanyName = repoEmploymentVer.GetCompanyNameForResearch(CheckRowID);
                if (!string.IsNullOrEmpty(CompanyName))
                {
                    body = body.Replace("{EmpOrColgName}", "Company Name : " + CompanyName);
                }
            }

            if (Designation == "Team Member-Edu")
            {
                var ResearchTM = repoPQClientTMember.GetDepManagerClientToTeam(ClientRowID, "Team Member-Edu-Research").FirstOrDefault();
                if (ResearchTM != null && !string.IsNullOrEmpty(ResearchTM.TeamMemberEmail) && ResearchTM.TeamMemberEmail.Trim().Length > 0)
                {
                    To = ResearchTM.TeamMemberEmail.Trim();
                    flag = true;
                }
                else
                {
                    ViewBag.ErrorMsg = "Education Research Team Member Not Found!";
                }

                Subject = "Update College Information!";
                body = body.Replace("{EduEmpType}", "College");
                string CollegeName = repoEducationVer.GetCollegeNameForResearch(CheckRowID);
                if (!string.IsNullOrEmpty(CollegeName))
                {
                    body = body.Replace("{EmpOrColgName}", "College Name : " + CollegeName);
                }
            }

            if (flag)
            {
                clsSendMail mail = new clsSendMail();
                var signature = mail.GetCompanyMailSignature();

                if (signature != null && signature.GeneralEmail != null && signature.GeneralEmail.Trim().Length > 0)
                {
                    From = signature.GeneralEmail;
                    DisplayName = signature.CompanyName.Trim();

                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br /><b>Mobile: ", signature.MobileNo,
                        " | Direct Tel: ", signature.PhoneNo, " | Fax: ", signature.FaxNo, "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", signature.GeneralEmail, "'>", signature.GeneralEmail, "</a>");


                    body = body.Replace("{RSearchRemarks}", Remarks);

                    body = body.Replace("{MailSignature}", MailSignature);

                    result = mail.SendMail(DisplayName, From, To, Subject, body);
                    if (result)
                    {
                        ViewBag.SuccessMsg = "Mail Sent Successfully!";
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Mail sending failed!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "From mail id invalid!";
                }
            }
        }

        public ActionResult EmploymentVerEmailDetails(int id = 0)
        {
            EmploymentEmailVerificationViewModel model = new EmploymentEmailVerificationViewModel();
            if (id > 0)
            {
                model = repoEmailVerification.GetEmploymentEmailVerification(id);
            }
            return View(model);
        }

        public ActionResult EducationVerEmailDetails(int id = 0)
        {
            EducationEmailVerificationViewModel model = new EducationEmailVerificationViewModel();
            if (id > 0)
            {
                model = repoEmailVerification.GetEducationEmailVerification(id);
            }
            return View(model);
        }

        public ActionResult ReferenceVerEmailDetails(int id = 0)
        {
            return View();
        }

        #endregion

        #region ******** Spacial Check Verifiation By TeamMember ********
        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult AddSpecialCheckInfoByTeamMember(int id = 0,int CheckStatusId=0, bool Aging = false,string msg="", int PersonalRowID = 0, int subCheckId = 0, short ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;


            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            BindSeverityGrids(ClientRowId);
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult AddSpecialCheckInfoByTeamMember(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
           string CheckStatus= formCollection["CheckStatus"].ToString();
            int Severity = Convert.ToInt32(formCollection["Severity"].ToString());


            string ColorName = formCollection["ColorsName"].ToString();
            // 


            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPTM";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                #region Update Education                             
                //repoSplChkVer.UpdateSpecialCheckInfoByDataEntry(model);
                if (CheckStatus == "Completed" && ColorName.ToLower() == "green")
                {
                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 24, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                    cc.UpdatePQCandidateChecksColorCode(0, Severity, ColorName, PersonalRowid, SubCheckid);
                }
                else
                {
                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                }
                //if (model.CheckStatus=="Completed") 
                //    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                //else
                //    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 18, "Check", Convert.ToInt32(Session["WebUserRowid"].ToString()));
                #region Save LogTransaction Details
                clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateSpecialCheckVerificationInfo", CheckStatus, "SpecialCheck Verification", "Update");
                #endregion

                #region Case/Check Action History Save
                clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "Verification information Updated");
                #endregion

                ModelState.Clear();
                //return RedirectToAction("AddSpecialCheckInfoByTeamMember", new { id = model.SpecialCheckRowID });

                #endregion




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckId = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("AddSpecialCheckInfoByTeamMember", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });

        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UplodSpecialCheckVerDocs(short scid = 0, int pe = 0, string cstatus = "",int clientid=0,int specialCheckId=0,string pdf="")
        {
            VerifiedUploadDocViewModel model = new VerifiedUploadDocViewModel();
            try
            {
                if (scid > 0 && pe > 0)
                {
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                    model.ClientRowId = clientid;
                    model.SpecialCheckId = specialCheckId;
                    if(clientid==697 && pdf=="pdf")
                    {
                        ViewBag.ErrorMsg = "please not upload pdf file for this client!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UplodSpecialCheckVerDocs(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        if (model.DocumentType == null)
                        {
                            model.DocumentType = "Varification Document";
                        }
                        model.DocumentUploadFrom = "Verified";
                        model.StatusDocxConvert = 0;
                        model.StatusPDF2DOCXConvert = 0;
                        model.UploadDate = DateTime.Now;
                        model.Status = 1;                        
                        string ext = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                        if (model.ClientRowId == 697 && ext.ToLower()==".pdf" )
                        {
                            ViewBag.ErrorMsg = "please not upload pdf file for this client!";                            
                            return RedirectToAction("UplodSpecialCheckVerDocs", new  {  scid = model.SubCheckRowID,  pe = model.PersonalRowID,  cstatus = model.CheckStatus,  clientid = model.ClientRowId,  specialCheckId = model.SpecialCheckId,pdf="pdf" });
                        }
                        cc.UpdateImageArtifact(model.PersonalRowID, model.SubCheckRowID);
                        proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + model.SubCheckRowID + "_" + model.UploadDate.Value.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                        model.UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = proImg;
                        repoPQVerifiedUploadDoc.VerifiedUploadDoc(model);
                        int Res = repoPQVerifiedUploadDoc.SaveChanges();
                        if (Res > 0)
                        {
                            #region Check Action History Save                           
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Special Check Vrification information Upload Doc ");
                            #endregion
                            ModelState.Clear();
                            ViewBag.SuccessMsg = "Record Added Successfully!";
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "There is some problem to save record. Please try again later!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "please select upload doc!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [NonAction]
        void SpecialCheckVerList(int SpecialCheckRowId = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var modelBind = repoSplChkVer.GetSpecialCheckForTM(SpecialCheckRowId);
                ViewBag.modelBind = modelBind;
                ViewBag.ListSpecialCheckVer = repoDDL.GetAntecedentVerList(ClientRowId, ChFamilRowId, subCheckId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        void BindSeverityGrids(short ClientRowID)
        {
            try
            {
                ViewBag.SeverityGrids = repoDDL.GetClientSeveritiesGridDDL(ClientRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        #endregion
        #endregion

        #region ******** Manage By Department Team Leader for Review Requested ********

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult ViewAddressVerInfo(int id)
        {
            ViewAddressViewModel model = new ViewAddressViewModel();
            try
            {
                if (id > 0)
                {
                    var verInfo = repoAddressVer.GetAddressInfoForTMQC(id);
                    if (verInfo != null)
                    {
                        model.AddressRowID = verInfo.AddressRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoAddressVer.GetVerificationCloserDetails(verInfo.AddressVerRowID);
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        AddressVerList(model.AddressRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoAddressVer.GetPersonalDetailsForTM(model.AddressRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Address id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult ViewAddressVerInfo(ViewAddressViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null && model.AddressRowID > 0)
                    {
                        #region Update Address Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            model.CheckStatus = "WIP-Pending";
                            repoAddressInfo.ViewAddressInfoByVrifierTL(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "ViewAddressVerInfo", model.CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 

                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            ModelState.Clear();
                            return RedirectToAction("ManageReviewRequestedChecksForDepartmentTL", "Insuff");
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoAddressVer.GetAddressInfoForTMQC(model.AddressRowID);
            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = repoAddressVer.GetVerificationCloserDetails(verInfo.AddressVerRowID);
            }
            AddressVerList(model.AddressRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID); model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult ViewEmploymentVerInfo(int id)
        {
            ViewEmploymentViewModel model = new ViewEmploymentViewModel();
            try
            {
                if (id > 0)
                {
                    var verInfo = repoEmploymentVer.GetEmploymentInfoForTMQC(id);
                    if (verInfo != null)
                    {
                        model.EmploymentRowID = verInfo.EmploymentRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoEmploymentVer.GetEmploymentVerCloserDetails(verInfo.EmploymentVerRowID);
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        EmployementVerList(model.EmploymentRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoEmploymentVer.GetPersonalDetailsForTM(model.EmploymentRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Employment id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            RatingNumber();
            BindCountry();
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult ViewEmploymentVerInfo(ViewEmploymentViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null && model.EmploymentRowID > 0)
                    {
                        #region Update Employment Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            model.CheckStatus = "WIP-Pending";
                            repoEmploymentInfo.ViewEmploymentInfoByVrifierTL(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "ViewEmploymentVerInfo", model.CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 

                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            //int PID = model.PersonalRowID;
                            ModelState.Clear();
                            return RedirectToAction("ManageReviewRequestedChecksForDepartmentTL", "Insuff");
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.ListEmployment = repoDDL.GetAntecedentList(model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);

            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult ViewEducationVerInfo(int id)
        {
            ViewEducationViewModel model = new ViewEducationViewModel();
            try
            {
                if (id > 0)
                {
                    var verInfo = repoEducationVer.GetEducationInfoForTMQC(id);

                    if (verInfo != null)
                    {
                        model.EducationRowID = verInfo.EducationRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoEducationVer.GetEducationVerCloserDetails(verInfo.EducationVerRowID);
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        EducationVerList(model.EducationRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoEducationVer.GetPersonalDetailsForTM(model.EducationRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult ViewEducationVerInfo(ViewEducationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null && model.EducationRowID > 0)
                    {
                        #region Update Employment Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            model.CheckStatus = "WIP-Pending";
                            repoEducationInfo.ViewEducationInfoByVrifierTL(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "ViewEducationVerInfo", model.CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 

                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            //int PID = model.PersonalRowID;
                            ModelState.Clear();
                            return RedirectToAction("ManageReviewRequestedChecksForDepartmentTL", "Insuff");
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoEducationVer.GetEducationInfoForTMQC(model.EducationRowID);
            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = repoEducationVer.GetEducationVerCloserDetails(verInfo.EducationVerRowID);
            }
            EducationVerList(model.EducationRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult ViewReferenceVerInfo(int id)
        {
            ViewReferenceViewModel model = new ViewReferenceViewModel();
            try
            {
                if (id > 0)
                {
                    var verInfo = repoReferenceVer.GetReferenceInfoForTMQC(id);

                    if (verInfo != null)
                    {
                        model.ReferenceRowID = verInfo.ReferenceCheckRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoReferenceVer.GetReferenceVerCloserDetails(verInfo.ReferenceCheckVerRowID);
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        ReferenceVerList(model.ReferenceRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoReferenceVer.GetPersonalDetailsForTM(model.ReferenceRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Reference id!";
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult ViewReferenceVerInfo(ViewReferenceViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null && model.ReferenceRowID > 0)
                    {
                        #region Update Reference Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            model.CheckStatus = "WIP-Pending";
                            repoReferenceInfo.ViewReferenceInfoByVrifierTL(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "ViewReferenceVerInfo", model.CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 

                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            //int PID = model.PersonalRowID;
                            ModelState.Clear();
                            return RedirectToAction("ManageReviewRequestedChecksForDepartmentTL", "Insuff");
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoReferenceVer.GetReferenceInfoForTMQC(model.ReferenceRowID);
            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = repoReferenceVer.GetReferenceVerCloserDetails(verInfo.ReferenceCheckVerRowID);
            }
            ReferenceVerList(model.ReferenceRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult ViewCriminalVerInfo(int id)
        {
            ViewCriminalViewModel model = new ViewCriminalViewModel();
            try
            {
                if (id > 0)
                {
                    var verInfo = repoCriminalVer.GetCriminalForTMQC(id);
                    if (verInfo != null)
                    {
                        model.CriminalRowID = verInfo.CriminalRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoCriminalVer.GetCriminalVerCloserDetails(verInfo.CriminalVerRowId);
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        CriminalVerList(model.CriminalRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoCriminalVer.GetPersonalDetailsForTM(model.CriminalRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult ViewCriminalVerInfo(ViewCriminalViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null && model.CriminalRowID > 0)
                    {
                        #region Update Employment Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            model.CheckStatus = "WIP-Pending";
                            repoCriminalInfo.ViewCriminalInfoByVrifierTL(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "ViewCriminalVerInfo", model.CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 

                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            //int PID = model.PersonalRowID;
                            ModelState.Clear();
                            return RedirectToAction("ManageReviewRequestedChecksForDepartmentTL", "Insuff");
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoCriminalVer.GetCriminalForTMQC(model.CriminalRowID);
            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = repoCriminalVer.GetCriminalVerCloserDetails(verInfo.CriminalVerRowId);
            }
            CriminalVerList(model.CriminalRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult ViewNationalIdentityVerInfo(int id)
        {
            ViewNationalIdentityViewModel model = new ViewNationalIdentityViewModel();
            try
            {
                if (id > 0)
                {
                    var verInfo = repoNationalIdentityVer.GetNationalIdentityForTMQC(id);
                    if (verInfo != null)
                    {
                        model.NationalIdentityRowID = verInfo.NationalIdentityRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoNationalIdentityVer.GetNationalIdentityVerCloserDetails(verInfo.NationalIdentityVerRowID);
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        NationalIdentityVerList(model.NationalIdentityRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoNationalIdentityVer.GetPersonalDetailsForTM(model.NationalIdentityRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult ViewNationalIdentityVerInfo(ViewNationalIdentityViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null && model.NationalIdentityRowID > 0)
                    {
                        #region Update NationalIdentity Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            model.CheckStatus = "WIP-Pending";
                            repoNationalIdentInfo.ViewNationalIdentityInfoByVrifierTL(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "ViewNationalIdentityVerInfo", model.CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 

                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            //int PID = model.PersonalRowID;
                            ModelState.Clear();
                            return RedirectToAction("ManageReviewRequestedChecksForDepartmentTL", "Insuff");
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoNationalIdentityVer.GetNationalIdentityForTMQC(model.NationalIdentityRowID);
            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = repoNationalIdentityVer.GetNationalIdentityVerCloserDetails(verInfo.NationalIdentityVerRowID);
            }
            NationalIdentityVerList(model.NationalIdentityRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult ViewSpecialCheckVerInfo(int id)
        {
            ViewSpecialCheckViewModel model = new ViewSpecialCheckViewModel();
            try
            {
                if (id > 0)
                {
                    var verInfo = repoSplChkVer.GetSpecialCheckInfoForTMQC(id);

                    if (verInfo != null)
                    {
                        model.SpecialCheckRowId = verInfo.SpecialCheckRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        ViewBag.VerifyInfo = verInfo;

                        // verInfo.CloserViewModel = repoReferenceVer.GetReferenceVerCloserDetails(verInfo.SpecialCheckVerRowID);
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        SpecialCheckVerList(model.SpecialCheckRowId, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoSplChkVer.GetPersonalDetailsForTM(model.SpecialCheckRowId);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Reference id!";
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult ViewSpecialCheckVerInfo(ViewSpecialCheckViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null && model.SpecialCheckRowId > 0)
                    {
                        #region Update Reference Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            model.CheckStatus = "WIP-Pending";
                            repoSplChkVer.ViewSpecialCheckInfoByVrifierTL(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "ViewSpecialCheckVerInfo", model.CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 

                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            //int PID = model.PersonalRowID;
                            ModelState.Clear();
                            return RedirectToAction("ManageReviewRequestedChecksForDepartmentTL", "Insuff");
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoSplChkVer.GetSpecialCheckInfoForTMQC(model.SpecialCheckRowId);

            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = null;//repoReferenceVer.GetReferenceVerCloserDetails(verInfo.ReferenceCheckVerRowID);
            }
            SpecialCheckVerList(model.SpecialCheckRowId, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }
        #endregion

        #region ******** Manage FundPending By Department Team Leader ********

        [SessionExpireFilterAttribute]
        public ActionResult ManageFundPendingChecksForDeprtTL(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0)
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemberId = Convert.ToInt16(Session["TeamMemberRowID"]);
                model = repoPQVerifiedUploadDoc.GetFundPendingCheckListForDepartTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, teamMemberId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientList();
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult FundPendingApproveByDepartmentTL(int PersonalId = 0, int VerId = 0, short chkFmlyId = 0, short SubchkFmlyId = 0)
        {
            FundPendingApprovalViewModel model = new FundPendingApprovalViewModel();
            try
            {
                if (PersonalId > 0 && VerId > 0 && chkFmlyId > 0 && SubchkFmlyId > 0)
                {
                    model.PersonalRowID = PersonalId;
                    model.VerificationRowID = VerId;
                    model.CheckFamilyRowID = chkFmlyId;
                    model.SubCheckRowID = SubchkFmlyId;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult FundPendingApproveByDepartmentTL(FundPendingApprovalViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.PersonalRowID > 0 && model.VerificationRowID > 0 && model.CheckFamilyRowID > 0 && model.SubCheckRowID > 0)
                    {
                        model.CheckStatus = "WIP-Pending";
                        model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.ModifiedDate = DateTime.Now;
                        repoVerificationInfo.FundPendingChecksUpdateByDepartmentTL(model);

                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                        #endregion

                        ModelState.Clear();
                        ViewBag.SuccessMsg = "Record Updated Successfully!";

                        //TempData["SuccessMsg"] = "Record Updated Successfully!";
                        //RedirectToAction("ManageFundPendingChecksForDeprtTL");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "All fields are mandatory!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        #endregion

        #region ******** Manage By Department Team Member Quality Check ********

        #region  Update Address Verification By Quality Check

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UpdateAddressInfoByTMQC(int id = 0, bool Aging = false)
        {
            UpdatePQAddressVerViewModel model = new UpdatePQAddressVerViewModel();
            try
            {
                if (id > 0)
                {
                    model = repoAddressVer.GetAddressInfoForTMQC(id);
                    if (model != null)
                    {
                        int CheckstatusId = cc.getCurrentCheckStatus(model.PersonalRowID,model.SubCheckRowID);
                        model.CloserViewModel = repoAddressVer.GetVerificationCloserDetails(model.AddressVerRowID);
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        AddressVerList(model.AddressRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        model.CheckStatusId = CheckstatusId;
                        model.Aging = Aging;
                        ViewBag.PersonalBind = repoAddressVer.GetPersonalDetailsForTM(model.AddressRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Address id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            TotalNumber();
            BindCountry();
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UpdateAddressInfoByTMQC(UpdatePQAddressVerViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();

                    if (model != null && model.AddressRowID > 0 && model.ClientRowID > 0 && model.CheckFamilyRowID > 0 && model.SubCheckRowID > 0)
                    {
                        int checkstatusid = 0;
                        checkstatusid = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);

                        if (model.CheckStatus == "Rejected")
                        {
                            repoAddressVer.UpdateAddressByTMQC(model);
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UpdateAddressInfoByTMQC", model.CheckStatus, "Address Verification TMQC", "Updated");
                            #endregion

                            #region Case/Check Action History Save
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.CloserViewModel.ActionHistory);
                            #endregion

                            ModelState.Clear();
                            if (model.Aging == true)
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return RedirectToAction("DepartmentTeamMemberQCChecks", new { CheckStatusId = model.CheckStatusId });
                            }
                            
                        }
                        else
                        {
                            if (model.CheckStatus == "Funds Pending")
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 26, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(model.PersonalRowID);
                                SendAlertMassageForFundPending(model.ClientRowID, model.AddressRowID, model.CheckFamilyRowID, model.SubCheckRowID,CandidateData.ClientUserRowId);
                            }

                            #region If check Insuff then send alert mail for Client/Spoc/Candidate By the Company
                            if (model.CheckStatusId == 53)
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 22, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                var data = repoPQClient.GetClientContractConditions(model.ClientRowID);
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(model.PersonalRowID);
                                if (data.CandidateContact == 1 && !string.IsNullOrEmpty(CandidateData.EmailID) && IsValidEmail(CandidateData.EmailID)) // send mail Client/Spoc/Candidate By the Company
                                {
                                    //Insert PQCandidateInsuffLogin Table data for candidate login
                                    // SendAlertMassageForInsuffClearationForCandidate(model.ClientRowID, model.AddressRowID, model.CheckFamilyRowID, model.SubCheckRowID, model.PersonalRowID, CandidateData.EmailID);
                                    if (model.ClientRowID == 249)
                                    {
                                        SendAlertMassageForInsuffClearationForClientSpoc(model.ClientRowID, model.AddressRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);
                                    }
                                   else if (model.ClientRowID == 277 || model.ClientRowID == 206) //Sutherland Healthcare Solutions Pvt. Ltd.
                                    {
                                        SendAlertMassageForInsuffClearationForClientSpoc(model.ClientRowID, model.AddressRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);

                                    }

                                    else
                                    {
                                        SendAlertMassageForInsuffClearationForClientSpoc(model.ClientRowID, model.AddressRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId);
                                    }

                                }

                                else if (!string.IsNullOrEmpty(CandidateData.EmailID) && IsValidEmail(CandidateData.EmailID)) // send mail Client/Spoc/Candidate By the Company
                                {

                                    if (model.ClientRowID == 277) //Sutherland Healthcare Solutions Pvt. Ltd.
                                    {
                                        SendAlertMassageForInsuffClearationForClientSpoc(model.ClientRowID, model.AddressRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);

                                    }

                                }
                                else if (model.ClientRowID == 249)
                                {
                                    SendAlertMassageForInsuffClearationForClientSpoc(model.ClientRowID, model.AddressRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);
                                }
                                else  // send mail Client/Spoc By the Company
                                {
                                    SendAlertMassageForInsuffClearationForClientSpoc(model.ClientRowID, model.AddressRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId);
                                }
                            }
                            if (checkstatusid == 24)
                            {

                                if (model.CloserViewModel.ColorName == "Green")
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                }
                                else
                                {
                                    SendAlertMassageForCheckClearationForClientSpoc(model.ClientRowID, model.AddressRowID, model.CheckFamilyRowID, model.SubCheckRowID, model.CloserViewModel.ColorName, model.CloserViewModel.Remarks);
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                }
                                
                            }
                            #endregion

                            if (model.AddressVerRowID > 0)
                            {
                                bool flag = false;
                                #region Update Address Information
                                if (model.CloserViewModel.ColorName == null)
                                    model.CloserViewModel.ColorName = "";
                                
                                    if(checkstatusid == 24)
                                        {
                                        cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    }
                               else if (model.CheckStatusId == 70 && model.CloserViewModel.ColorName.ToUpper() != "GREEN")
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                                }
                                else if (model.CheckStatusId == 71)
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                                }
                                //short rptQCMgrId = repoTeamDepartment.GetTeamMemberRptWriterManager("Manager-Rpt-QC");
                                var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(model.ClientRowID, "Manager-Rpt-QC").FirstOrDefault();
                                    if (mgrData != null && mgrData.TeamMemberRowID > 0)
                                    {
                                        repoAddressVer.UpdateAddressByTMQC(model, mgrData.TeamMemberRowID);
                                    if(Session["Designation"].ToString() != "Team Member-Rpt-QC")
                                    {
                                        flag = true;
                                        //Check All Checks are completed then put case status completed
                                        UpdateCaseCompleted(model.PersonalRowID);
                                    }
                                    }
                                    else
                                    {
                                        repoAddressVer.UpdateAddressByTMQC(model);
                                        TempData["ErrorMsg"] = "Report QC Manager Not Found!";
                                    if (model.Aging == true)
                                    {
                                        return RedirectToAction("Index");
                                    }
                                    else
                                    {
                                        return RedirectToAction("UpdateAddressInfoByTMQC", new { id = model.AddressRowID });
                                    }
                                    
                                    }
                                
                                

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UpdateAddressInfoByTMQC", model.CheckStatus, "Approved By Verifier TMQC", "Update");
                                #endregion

                                #region Case/Check Action History Save
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Approved By Verifier TMQC");
                                if (flag)
                                {
                                    if (model.CheckStatus == "Rejected-RWQC")
                                    {
                                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Allocated-TM", "Allocated to Report QC Team Member");
                                    }
                                    else
                                    {
                                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Allocated-Mgr", "Allocated to Report QC Manager");
                                    }
                                }
                                #endregion

                                ModelState.Clear();
                                if (Session["Designation"].ToString() == "Team Member-Rpt-QC")
                                {
                                    return RedirectToAction("Index", "ReportQC");
                                }
                                if (model.Aging==true)
                                {
                                    return RedirectToAction("Index"); 
                                }
                                else
                                {
                                    return RedirectToAction("DepartmentTeamMemberQCChecks", new { CheckStatusId = model.CheckStatusId });
                                }
                                
                                #endregion
                            }
                            else
                            {
                                #region Add Address Information

                                repoAddressVer.UpdateAddressByTMQC(model);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UpdateAddressVerificationInfo", model.CheckStatus, "Address Verification TMQC", "Update");
                                #endregion

                                #region Case/Check Action History Save
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Verification information Updated TMQC");
                                #endregion

                                ModelState.Clear();
                                //TempData["SuccessMsg"] = "Record Added Successfully!";
                                if (model.Aging == true)
                                {
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    return RedirectToAction("DepartmentTeamMemberQCChecks", new { CheckStatusId = model.CheckStatusId });
                                }
                                    
                                #endregion
                            }
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "invlid Address id and model!";
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            model.CloserViewModel = repoAddressVer.GetVerificationCloserDetails(model.AddressVerRowID);
            BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
            BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
            AddressVerList(model.AddressRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            ViewBag.PersonalBind = repoAddressVer.GetPersonalDetailsForTM(model.AddressRowID);

            TotalNumber();
            BindCountry();
            return View(model);
        }
        #endregion

        #region  Update Employment Verification By Quality Check

        [SessionExpireFilterAttribute]
        public ActionResult UpdateEmploymentInfoByTMQC(int id = 0,int CheckStatusId=0,bool Aging=false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowID));
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            BindDisposition(12, ClientRowId);
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UpdateEmploymentInfoByTMQC(FormCollection formCollection, string action = "")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();

            int hdnDispositionID = Convert.ToInt32(formCollection["hdnDispositionID"].ToString());
            int hdnSeverity = Convert.ToInt32(formCollection["hdnSeverity"].ToString());

            


                      //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

                      //string Checkstatus = formCollection["CheckStatus"].ToString();
                      //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                      var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPQC";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if ( clientRowid > 0 &&  SubCheckid > 0)
                    {
                        int checkstatusid = 0;                        
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        if (CheckStatus == "Rejected")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentInfoByTMQC", CheckStatus, "Employment Verification TMQC", "Updated");
                            #endregion

                            #region Case/Check Action History Save
                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "");
                            #endregion

                            ModelState.Clear();
                            
                                return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });
                        }
                        else
                        {
                            if (CheckStatus == "Funds Pending")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 26, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.InsertFundsAmount(PersonalRowid, SubCheckid, model.Amount, model.CloserViewModel.ActionHistory, Session["TeamMemberRowID"].ToString(), Session["TeamMemberRowID"].ToString());
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                SendAlertMassageForFundPending(clientRowid, PersonalRowid, 12, SubCheckid, CandidateData.ClientUserRowId);
                            }

                            #region If check Insuff then send alert mail for Client/Spoc/Candidate By the Company
                            if (CheckStatusId == 53)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 22, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                var data = repoPQClient.GetClientContractConditions(clientRowid);
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                if (clientRowid == 249)
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);
                                }
                                else if (clientRowid == 277 || clientRowid == 206) //Sutherland Healthcare Solutions Pvt. Ltd.insuff mail candidate and save mailhistory table
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, 12, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);

                                }
                                else
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId);

                                }



                            }
                            #endregion

                                bool flag = false;
                                #region Update Employment Information

                                if (checkstatusid == 24)
                                {
                                    if (ColorName == "Green")
                                    {
                                        cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                        cc.UpdatePQCandidateChecksColorCode(hdnDispositionID,hdnSeverity, ColorName, PersonalRowid, SubCheckid);
                                    }
                                    else
                                    {

                                        //SendAlertMassageForCheckClearationForClientSpoc(model.ClientRowID, model.EmploymentRowID, 12, SubCheckid, model.CloserViewModel.ColorName, model.CloserViewModel.Remarks);
                                        cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(hdnDispositionID, hdnSeverity, ColorName, PersonalRowid, SubCheckid);

                                }

                            }

                                else if (CheckStatusId == 70 && ColorName.ToLower() != "green")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                                }
                                else if (CheckStatusId == 71)
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                                }
                                else if (CheckStatusId == 25)
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                                }
                                //short rptQCMgrId = repoTeamDepartment.GetTeamMemberRptWriterManager("Manager-Rpt-QC");
                                var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(clientRowid, "Manager-Rpt-QC").FirstOrDefault();
                                if (mgrData != null && mgrData.TeamMemberRowID > 0)
                                {
                                    //repoEmploymentVer.UpdateEmploymentByTMQC(model, mgrData.TeamMemberRowID);
                                    flag = true;
                                    //Check All Checks are completed then put case status completed
                                    UpdateCaseCompleted(PersonalRowid);
                                }
                                else
                                {
                                    //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                    TempData["ErrorMsg"] = "Report QC Manager Not Found!";
                                
                                    return RedirectToAction("UpdateEmploymentInfoByTMQC", new { CheckStatusId= CheckStatusId, PersonalRowID= PersonalRowid, subCheckId = SubCheckid, ClientRowId = clientRowid });
                                }
                                #region Save LogTransaction Details

                                clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentInfoByTMQC", CheckStatus, "Approved By Verifier TMQC", "Update");

                                #endregion

                                #region Case/Check Action History Save

                                clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "Approved By Verifier TMQC");
                                if (flag)
                                {
                                    if (CheckStatus == "Rejected-RWQC")
                                    {
                                        cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 25, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                        clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-TM", "Allocated to Report QC Team Member");
                                    }
                                    else
                                    {
                                        clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-Mgr", "Allocated to Report QC Manager");
                                    }
                                }

                                #endregion

                                ModelState.Clear();
                                
                                    return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });


                                #endregion
                            
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("UpdateEmploymentInfoByTMQC", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });

        }

        #endregion

        #region  Update Education Verification By Quality Check

        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UpdateEducationInfoByTMQC(int id = 0, bool Aging = false, int CheckStatusId = 0,  int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowID));
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            //model.CloserViewModel = repoEducationVer.GetEducationVerCloserDetails(model.EducationVerRowID);
            BindDisposition(11, ClientRowId);
            BindDocumentList(PersonalRowID, subCheckId);
            //EducationVerList(model.EducationRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            //ViewBag.PersonalBind = repoEducationVer.GetPersonalDetailsForTM(model.EducationRowID);
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UpdateEducationInfoByTMQC(FormCollection formCollection, string action = "")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();

            int hdnDispositionID = Convert.ToInt32(formCollection["hdnDispositionID"].ToString());
            int hdnSeverity = Convert.ToInt32(formCollection["hdnSeverity"].ToString());




            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPQC";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        if (CheckStatus == "Rejected")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEducationInfoByTMQC", CheckStatus, "Employment Verification TMQC", "Updated");
                            #endregion

                            #region Case/Check Action History Save
                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "");
                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });
                        }
                        else
                        {
                            if (CheckStatus == "Funds Pending")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 26, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.InsertFundsAmount(PersonalRowid, SubCheckid, model.Amount, model.CloserViewModel.ActionHistory, Session["TeamMemberRowID"].ToString(), Session["TeamMemberRowID"].ToString());
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                SendAlertMassageForFundPending(clientRowid, PersonalRowid, 12, SubCheckid, CandidateData.ClientUserRowId);
                            }

                            #region If check Insuff then send alert mail for Client/Spoc/Candidate By the Company
                            if (CheckStatusId == 53)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 22, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                var data = repoPQClient.GetClientContractConditions(clientRowid);
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                if (clientRowid == 249)
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);
                                }
                                else if (clientRowid == 277 || clientRowid == 206) //Sutherland Healthcare Solutions Pvt. Ltd.insuff mail candidate and save mailhistory table
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, 12, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);

                                }
                                else
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId);

                                }



                            }
                            #endregion

                            bool flag = false;
                            #region Update Employment Information

                            if (checkstatusid == 24)
                            {
                                if (ColorName == "Green")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(hdnDispositionID, hdnSeverity, ColorName, PersonalRowid, SubCheckid);
                                }
                                else
                                {

                                    //SendAlertMassageForCheckClearationForClientSpoc(model.ClientRowID, model.EmploymentRowID, 12, SubCheckid, model.CloserViewModel.ColorName, model.CloserViewModel.Remarks);
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(hdnDispositionID, hdnSeverity, ColorName, PersonalRowid, SubCheckid);

                                }

                            }

                            else if (CheckStatusId == 70 && ColorName.ToLower() != "green")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            else if (CheckStatusId == 71)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            else if (CheckStatusId == 25)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            //short rptQCMgrId = repoTeamDepartment.GetTeamMemberRptWriterManager("Manager-Rpt-QC");
                            var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(clientRowid, "Manager-Rpt-QC").FirstOrDefault();
                            if (mgrData != null && mgrData.TeamMemberRowID > 0)
                            {
                                //repoEmploymentVer.UpdateEmploymentByTMQC(model, mgrData.TeamMemberRowID);
                                flag = true;
                                //Check All Checks are completed then put case status completed
                                UpdateCaseCompleted(PersonalRowid);
                            }
                            else
                            {
                                //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                TempData["ErrorMsg"] = "Report QC Manager Not Found!";

                                return RedirectToAction("UpdateEducationInfoByTMQC", new { CheckStatusId = CheckStatusId, PersonalRowID = PersonalRowid, subCheckId = SubCheckid, ClientRowId = clientRowid });
                            }
                            #region Save LogTransaction Details

                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEducationInfoByTMQC", CheckStatus, "Approved By Verifier TMQC", "Update");

                            #endregion

                            #region Case/Check Action History Save

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "Approved By Verifier TMQC");
                            if (flag)
                            {
                                if (CheckStatus == "Rejected-RWQC")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 25, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-TM", "Allocated to Report QC Team Member");
                                }
                                else
                                {
                                    clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-Mgr", "Allocated to Report QC Manager");
                                }
                            }

                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });


                            #endregion

                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("UpdateEducationInfoByTMQC", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }
        #endregion

        #region  Update Criminal Verification By Quality Check

        [SessionExpireFilterAttribute]
        public ActionResult UpdateCriminalInfoByTMQC(int id = 0, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowID));
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            BindDisposition(5, ClientRowId);
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UpdateCriminalInfoByTMQC(FormCollection formCollection, string action="")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();

            int hdnDispositionID = Convert.ToInt32(formCollection["hdnDispositionID"].ToString());
            int hdnSeverity = Convert.ToInt32(formCollection["hdnSeverity"].ToString());




            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPQC";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        if (CheckStatus == "Rejected")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCriminalInfoByTMQC", CheckStatus, "Employment Verification TMQC", "Updated");
                            #endregion

                            #region Case/Check Action History Save
                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "");
                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });
                        }
                        else
                        {
                            if (CheckStatus == "Funds Pending")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 26, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.InsertFundsAmount(PersonalRowid, SubCheckid, model.Amount, model.CloserViewModel.ActionHistory, Session["TeamMemberRowID"].ToString(), Session["TeamMemberRowID"].ToString());
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                SendAlertMassageForFundPending(clientRowid, PersonalRowid, 12, SubCheckid, CandidateData.ClientUserRowId);
                            }

                            #region If check Insuff then send alert mail for Client/Spoc/Candidate By the Company
                            if (CheckStatusId == 53)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 22, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                var data = repoPQClient.GetClientContractConditions(clientRowid);
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                if (clientRowid == 249)
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);
                                }
                                else if (clientRowid == 277 || clientRowid == 206) //Sutherland Healthcare Solutions Pvt. Ltd.insuff mail candidate and save mailhistory table
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, 12, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);

                                }
                                else
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId);

                                }



                            }
                            #endregion

                            bool flag = false;
                            #region Update Employment Information

                            if (checkstatusid == 24)
                            {
                                if (ColorName == "Green")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(hdnDispositionID, hdnSeverity, ColorName, PersonalRowid, SubCheckid);
                                }
                                else
                                {

                                    //SendAlertMassageForCheckClearationForClientSpoc(model.ClientRowID, model.EmploymentRowID, 12, SubCheckid, model.CloserViewModel.ColorName, model.CloserViewModel.Remarks);
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(hdnDispositionID, hdnSeverity, ColorName, PersonalRowid, SubCheckid);

                                }

                            }

                            else if (CheckStatusId == 70 && ColorName.ToLower() != "green")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            else if (CheckStatusId == 71)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            else if (CheckStatusId == 25)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            //short rptQCMgrId = repoTeamDepartment.GetTeamMemberRptWriterManager("Manager-Rpt-QC");
                            var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(clientRowid, "Manager-Rpt-QC").FirstOrDefault();
                            if (mgrData != null && mgrData.TeamMemberRowID > 0)
                            {
                                //repoEmploymentVer.UpdateEmploymentByTMQC(model, mgrData.TeamMemberRowID);
                                flag = true;
                                //Check All Checks are completed then put case status completed
                                UpdateCaseCompleted(PersonalRowid);
                            }
                            else
                            {
                                //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                TempData["ErrorMsg"] = "Report QC Manager Not Found!";

                                return RedirectToAction("UpdateCriminalInfoByTMQC", new { CheckStatusId = CheckStatusId, PersonalRowID = PersonalRowid, subCheckId = SubCheckid, ClientRowId = clientRowid });
                            }
                            #region Save LogTransaction Details

                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCriminalInfoByTMQC", CheckStatus, "Approved By Verifier TMQC", "Update");

                            #endregion

                            #region Case/Check Action History Save

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "Approved By Verifier TMQC");
                            if (flag)
                            {
                                if (CheckStatus == "Rejected-RWQC")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 25, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-TM", "Allocated to Report QC Team Member");
                                }
                                else
                                {
                                    clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-Mgr", "Allocated to Report QC Manager");
                                }
                            }

                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });


                            #endregion

                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("UpdateCriminalInfoByTMQC", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }
        #endregion

        #region  Update National Identity Verification By Quality Check
        [SessionExpireFilterAttribute]
        public ActionResult UpdateNationalIdentityInfoByTMQC(int id = 0, int CheckStatusId = 0,  bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowID));
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            BindDisposition(26, ClientRowId);
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UpdateNationalIdentityInfoByTMQC(UpdatePQNationalIdentityVerViewModel model,FormCollection formCollection,string action="")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();

            int hdnDispositionID = Convert.ToInt32(formCollection["hdnDispositionID"].ToString());
            int hdnSeverity = Convert.ToInt32(formCollection["hdnSeverity"].ToString());




            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPQC";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        if (CheckStatus == "Rejected")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateNationalIdentityInfoByTMQC", CheckStatus, "Employment Verification TMQC", "Updated");
                            #endregion

                            #region Case/Check Action History Save
                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "");
                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });
                        }
                        else
                        {
                            if (CheckStatus == "Funds Pending")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 26, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.InsertFundsAmount(PersonalRowid, SubCheckid, model.Amount, model.CloserViewModel.ActionHistory, Session["TeamMemberRowID"].ToString(), Session["TeamMemberRowID"].ToString());
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                SendAlertMassageForFundPending(clientRowid, PersonalRowid, 12, SubCheckid, CandidateData.ClientUserRowId);
                            }

                            #region If check Insuff then send alert mail for Client/Spoc/Candidate By the Company
                            if (CheckStatusId == 53)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 22, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                var data = repoPQClient.GetClientContractConditions(clientRowid);
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                if (clientRowid == 249)
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);
                                }
                                else if (clientRowid == 277 || clientRowid == 206) //Sutherland Healthcare Solutions Pvt. Ltd.insuff mail candidate and save mailhistory table
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, 12, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);

                                }
                                else
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId);

                                }



                            }
                            #endregion

                            bool flag = false;
                            #region Update Employment Information

                            if (checkstatusid == 24)
                            {
                                if (ColorName == "Green")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(hdnDispositionID, hdnSeverity, ColorName, PersonalRowid, SubCheckid);
                                }
                                else
                                {

                                    //SendAlertMassageForCheckClearationForClientSpoc(model.ClientRowID, model.EmploymentRowID, 12, SubCheckid, model.CloserViewModel.ColorName, model.CloserViewModel.Remarks);
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(hdnDispositionID, hdnSeverity, ColorName, PersonalRowid, SubCheckid);

                                }

                            }

                            else if (CheckStatusId == 70 && ColorName.ToLower() != "green")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            else if (CheckStatusId == 71)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            else if (CheckStatusId == 25)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            //short rptQCMgrId = repoTeamDepartment.GetTeamMemberRptWriterManager("Manager-Rpt-QC");
                            var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(clientRowid, "Manager-Rpt-QC").FirstOrDefault();
                            if (mgrData != null && mgrData.TeamMemberRowID > 0)
                            {
                                //repoEmploymentVer.UpdateEmploymentByTMQC(model, mgrData.TeamMemberRowID);
                                flag = true;
                                //Check All Checks are completed then put case status completed
                                UpdateCaseCompleted(PersonalRowid);
                            }
                            else
                            {
                                //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                TempData["ErrorMsg"] = "Report QC Manager Not Found!";

                                return RedirectToAction("UpdateNationalIdentityInfoByTMQC", new { CheckStatusId = CheckStatusId, PersonalRowID = PersonalRowid, subCheckId = SubCheckid, ClientRowId = clientRowid });
                            }
                            #region Save LogTransaction Details

                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateNationalIdentityInfoByTMQC", CheckStatus, "Approved By Verifier TMQC", "Update");

                            #endregion

                            #region Case/Check Action History Save

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "Approved By Verifier TMQC");
                            if (flag)
                            {
                                if (CheckStatus == "Rejected-RWQC")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 25, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-TM", "Allocated to Report QC Team Member");
                                }
                                else
                                {
                                    clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-Mgr", "Allocated to Report QC Manager");
                                }
                            }

                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });


                            #endregion

                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("UpdateNationalIdentityInfoByTMQC", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }
        #endregion

        #region Update Reference Verification By Quality Check
        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UpdateReferenceInfoByTMQC(int id = 0, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowID));
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            BindDisposition(30, ClientRowId);
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UpdateReferenceInfoByTMQC(UpdatePQReferenceVerViewModel model, FormCollection formCollection,string action="")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();

            int hdnDispositionID = Convert.ToInt32(formCollection["hdnDispositionID"].ToString());
            int hdnSeverity = Convert.ToInt32(formCollection["hdnSeverity"].ToString());




            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPQC";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        if (CheckStatus == "Rejected")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateReferenceInfoByTMQC", CheckStatus, "Employment Verification TMQC", "Updated");
                            #endregion

                            #region Case/Check Action History Save
                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "");
                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });
                        }
                        else
                        {
                            if (CheckStatus == "Funds Pending")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 26, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.InsertFundsAmount(PersonalRowid, SubCheckid, model.Amount, model.CloserViewModel.ActionHistory, Session["TeamMemberRowID"].ToString(), Session["TeamMemberRowID"].ToString());
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                SendAlertMassageForFundPending(clientRowid, PersonalRowid, 12, SubCheckid, CandidateData.ClientUserRowId);
                            }

                            #region If check Insuff then send alert mail for Client/Spoc/Candidate By the Company
                            if (CheckStatusId == 53)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 22, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                var data = repoPQClient.GetClientContractConditions(clientRowid);
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                if (clientRowid == 249)
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);
                                }
                                else if (clientRowid == 277 || clientRowid == 206) //Sutherland Healthcare Solutions Pvt. Ltd.insuff mail candidate and save mailhistory table
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, 12, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);

                                }
                                else
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId);

                                }



                            }
                            #endregion

                            bool flag = false;
                            #region Update Employment Information

                            if (checkstatusid == 24)
                            {
                                if (ColorName == "Green")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(hdnDispositionID, hdnSeverity, ColorName, PersonalRowid, SubCheckid);
                                }
                                else
                                {

                                    //SendAlertMassageForCheckClearationForClientSpoc(model.ClientRowID, model.EmploymentRowID, 12, SubCheckid, model.CloserViewModel.ColorName, model.CloserViewModel.Remarks);
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(hdnDispositionID, hdnSeverity, ColorName, PersonalRowid, SubCheckid);

                                }

                            }

                            else if (CheckStatusId == 70 && ColorName.ToLower() != "green")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            else if (CheckStatusId == 71)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            else if (CheckStatusId == 25)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            //short rptQCMgrId = repoTeamDepartment.GetTeamMemberRptWriterManager("Manager-Rpt-QC");
                            var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(clientRowid, "Manager-Rpt-QC").FirstOrDefault();
                            if (mgrData != null && mgrData.TeamMemberRowID > 0)
                            {
                                //repoEmploymentVer.UpdateEmploymentByTMQC(model, mgrData.TeamMemberRowID);
                                flag = true;
                                //Check All Checks are completed then put case status completed
                                UpdateCaseCompleted(PersonalRowid);
                            }
                            else
                            {
                                //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                TempData["ErrorMsg"] = "Report QC Manager Not Found!";

                                return RedirectToAction("UpdateReferenceInfoByTMQC", new { CheckStatusId = CheckStatusId, PersonalRowID = PersonalRowid, subCheckId = SubCheckid, ClientRowId = clientRowid });
                            }
                            #region Save LogTransaction Details

                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateReferenceInfoByTMQC", CheckStatus, "Approved By Verifier TMQC", "Update");

                            #endregion

                            #region Case/Check Action History Save

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "Approved By Verifier TMQC");
                            if (flag)
                            {
                                if (CheckStatus == "Rejected-RWQC")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 25, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-TM", "Allocated to Report QC Team Member");
                                }
                                else
                                {
                                    clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-Mgr", "Allocated to Report QC Manager");
                                }
                            }

                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });


                            #endregion

                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("UpdateReferenceInfoByTMQC", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }
        #endregion

        #region Update Special Check Verification By Quality Check
        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult UpdateSpecialCheckInfoByTMQC(int id = 0, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowID));
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable dbValue = new DataTable();
            dbValue = cc.GetDataEntryValue(PersonalRowID, subCheckId);
            DataTable dts=cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
            int SeverityRowID = 0;
            string ColorCode = "";
            string SeverityGrid = "";


            if (dts.Rows.Count>0)
            {
                 SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                 ColorCode = dts.Rows[0]["ColorCode"].ToString();
                SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();

                
                ViewBag.SeverityRowID = SeverityRowID;
                ViewBag.ColorCode = ColorCode;
                ViewBag.SeverityGrid = SeverityGrid;


            }


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();

            
            BindDocumentList(PersonalRowID, subCheckId);
            //SpecialCheckVerList(model.SpecialCheckRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
           // ViewBag.PersonalBind = repoSplChkVer.GetPersonalDetailsForTM(model.SpecialCheckRowID);
            //GetEmploymentList();
            return View(marksModel);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult UpdateSpecialCheckInfoByTMQC(FormCollection formCollection, string action="")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            //int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();
            int hdnSeverity = Convert.ToInt32(formCollection["SeverityRowID"].ToString());

            //int hdnDispositionID = Convert.ToInt32(formCollection["hdnDispositionID"].ToString());
            




            //int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            //string Checkstatus = formCollection["CheckStatus"].ToString();
            //model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPQC";

                // foreach (string key in formCollection.AllKeys)
                //{
                //    Response.Write("Key=" + key + " ");
                //    Response.Write("Value=" + formCollection[key]);
                //    Response.Write("<br/>");
                //}           



                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
                            }

                        }


                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                            }
                        }
                    }


                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "checkbox")
                    {
                        int chkselect = 0;
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_TRUE", d[0].ToString());
                                chkselect = 1;
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        if (CheckStatus == "Rejected")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateSpecialCheckInfoByTMQC", CheckStatus, "Employment Verification TMQC", "Updated");
                            #endregion

                            #region Case/Check Action History Save
                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "");
                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });
                        }
                        else
                        {
                            if (CheckStatus == "Funds Pending")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 26, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.InsertFundsAmount(PersonalRowid, SubCheckid, model.Amount, model.CloserViewModel.ActionHistory, Session["TeamMemberRowID"].ToString(), Session["TeamMemberRowID"].ToString());
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                SendAlertMassageForFundPending(clientRowid, PersonalRowid, 12, SubCheckid, CandidateData.ClientUserRowId);
                            }

                            #region If check Insuff then send alert mail for Client/Spoc/Candidate By the Company
                            if (CheckStatusId == 53)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 22, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                var data = repoPQClient.GetClientContractConditions(clientRowid);
                                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowid);
                                if (clientRowid == 249)
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);
                                }
                                else if (clientRowid == 277 || clientRowid == 206) //Sutherland Healthcare Solutions Pvt. Ltd.insuff mail candidate and save mailhistory table
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, 12, model.SubCheckRowID, CandidateData.ClientUserRowId, CandidateData.EmailID);

                                }
                                else
                                {
                                    //SendAlertMassageForInsuffClearationForClientSpoc(clientRowid, model.EmploymentRowID, model.CheckFamilyRowID, model.SubCheckRowID, CandidateData.ClientUserRowId);

                                }



                            }
                            #endregion

                            bool flag = false;
                            #region Update Employment Information

                            if (checkstatusid == 24)
                            {
                                if (ColorName == "Green")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(0, hdnSeverity, ColorName, PersonalRowid, SubCheckid);
                                }
                                else
                                {

                                    //SendAlertMassageForCheckClearationForClientSpoc(model.ClientRowID, model.EmploymentRowID, 12, SubCheckid, model.CloserViewModel.ColorName, model.CloserViewModel.Remarks);
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    cc.UpdatePQCandidateChecksColorCode(0, hdnSeverity, ColorName, PersonalRowid, SubCheckid);

                                }

                            }

                            else if (CheckStatusId == 70 && ColorName.ToLower() != "green")
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 19, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            else if (CheckStatusId == 71)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            else if (CheckStatusId == 25)
                            {
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 36, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            }
                            //short rptQCMgrId = repoTeamDepartment.GetTeamMemberRptWriterManager("Manager-Rpt-QC");
                            var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(clientRowid, "Manager-Rpt-QC").FirstOrDefault();
                            if (mgrData != null && mgrData.TeamMemberRowID > 0)
                            {
                                //repoEmploymentVer.UpdateEmploymentByTMQC(model, mgrData.TeamMemberRowID);
                                flag = true;
                                //Check All Checks are completed then put case status completed
                                UpdateCaseCompleted(PersonalRowid);
                            }
                            else
                            {
                                //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                TempData["ErrorMsg"] = "Report QC Manager Not Found!";

                                return RedirectToAction("UpdateSpecialCheckInfoByTMQC", new { CheckStatusId = CheckStatusId, PersonalRowID = PersonalRowid, subCheckId = SubCheckid, ClientRowId = clientRowid });
                            }
                            #region Save LogTransaction Details

                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateSpecialCheckInfoByTMQC", CheckStatus, "Approved By Verifier TMQC", "Update");

                            #endregion

                            #region Case/Check Action History Save

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, "Approved By Verifier TMQC");
                            if (flag)
                            {
                                if (CheckStatus == "Rejected-RWQC")
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 25, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                    clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-TM", "Allocated to Report QC Team Member");
                                }
                                else
                                {
                                    clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, "Allocated-Mgr", "Allocated to Report QC Manager");
                                }
                            }

                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("DepartmentTeamMemberQCChecks", new { checkstatusid = checkstatusid });


                            #endregion

                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }




            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;
            // int PersonalRowID=0,int subCheckId=0,int ClientRowId=0
            return RedirectToAction("UpdateSpecialCheckInfoByTMQC", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });

        }

        #endregion

        #endregion

        #region ******** Manage RWQC Approval Checks For Department TL ********

        [SessionExpireFilterAttribute]
        public ActionResult ManageRWQCApprovalChecksForDepartmentManager(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0)
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();

            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemberId = Convert.ToInt16(Session["TeamMemberRowID"]);
                model = repoVerificationInfo.GetRWApprovalCheckListForDepartmentManager(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, teamMemberId);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientList();
            return View(model);

        }        
        [SessionExpireFilterAttribute]
        public ActionResult ManageRWQCApprovalChecksForDepartmentTL(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0,string CheckStatus="", string version="New",string CheckStatusId="")
        {
            string statusid = "";
            string department = "";
             if (CheckStatus == "Review" )
            {
                statusid = "19";
            }
            else if (CheckStatus == "Reverification")
            {
                statusid = "20";
            }
            else
            {
                statusid = CheckStatusId;
            }
                        
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemberId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                if (version=="New")
                {
                    if(Designation== "Team Leader-Emp")
                    {
                        department = "Employment";
                    }
                    else if (Designation == "Team Leader-Ide")
                    {
                        department = "National";
                    }
                    else if (Designation == "Team Leader-Ref")
                    {
                        department = "Reference";
                    }
                    else if (Designation == "Team Leader-SPL")
                    {
                        department = "Special";
                    }
                    else if (Designation == "Team Leader-Cri")
                    {
                        department = "Criminal";
                    }
                    else if (Designation == "Team Leader-Add")
                    {
                        department = "Address";
                    }
                    else if (Designation == "Team Leader-Edu")
                    {
                        department = "Education";
                    }
                    model = repoVerificationInfo.GetCandidateListForTMMQCNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemberId, ClientRowID, CheckStatus);
                    if(CheckStatus == "Reverification")
                    {
                        var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                        model.PQVerificationInfo = list;
                        model.TotalRecords = list.Count();
                    }

                }
                else
                {
                    model = repoVerificationInfo.GetRWApprovalCheckListForDepartmentTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, teamMemberId);

                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientList();
            return View(model);

        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateAddressVerInfoByTeamLeader(int id)
        {
            ViewAddressViewModel model = new ViewAddressViewModel();
            try
            {
                if (id > 0)
                {
                    ClientCustomize cc = new ClientCustomize();
                    var verInfo = repoAddressVer.GetAddressInfoForTMQC(id);
                    if (verInfo != null)
                    {
                        model.AddressRowID = verInfo.AddressRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        model.CheckStatusID = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoAddressVer.GetVerificationCloserDetails(verInfo.AddressVerRowID);
                        model.ColorName = verInfo.CloserViewModel.ColorName;
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        AddressVerList(model.AddressRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoAddressVer.GetPersonalDetailsForTM(model.AddressRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Employment id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateAddressVerInfoByTeamLeader(ViewAddressViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null && model.AddressRowID > 0)
                    {
                        short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                        int CheckStatusId = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        #region Update Address Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            string DesignationName = Convert.ToString(Session["Designation"]);

                            if (model.CheckStatus != "Rejected")
                            {
                                var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(model.ClientRowID, "Manager-Rpt-QC").FirstOrDefault();
                                if (mgrData != null && mgrData.TeamMemberRowID > 0)
                                {
                                    repoVerificationInfo.UpdateAddressCheckRejectionByMgrOrTL(model, DesignationName);
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Report QC Manager Not Found!";
                                }
                            }
                            else
                            {
                                repoVerificationInfo.UpdateAddressCheckRejectionByMgrOrTL(model, DesignationName);
                            }

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UpdateAddressVerInfoByTeamLeader", model.CheckStatus, "", "Update");
                            #endregion
                            if (model.CheckStatus != "Rejected")
                            {
                                if (model.ColorName == "Red")
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 17, "Check", teamMemerRowId);
                                }
                                else
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 71, "Check", teamMemerRowId);
                                }
                            }
                            else
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 20, "Check", teamMemerRowId);
                            }

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            ModelState.Clear();
                            if (Session["Designation"] != null && Session["Designation"].ToString() == "Team Leader-Add")
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentTL", "Verification",new {CheckStatusId= CheckStatusId });
                            }
                            else
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentManager", "Verification");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoAddressVer.GetAddressInfoForTMQC(model.AddressRowID);
            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = repoAddressVer.GetVerificationCloserDetails(verInfo.AddressVerRowID);
            }
            AddressVerList(model.AddressRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID); model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateEmploymentVerInfoByTeamLeader(int id,int checkStatusid=0,string ColorName = "")
        {
            ViewEmploymentViewModel model = new ViewEmploymentViewModel();
            try
            {
                if (id > 0)
                {
                    var verInfo = repoEmploymentVer.GetEmploymentInfoForTMQC(id);
                    if (verInfo != null)
                    {
                        model.EmploymentRowID = verInfo.EmploymentRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        model.CheckStatusId = checkStatusid;
                        model.ColorName = ColorName;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoEmploymentVer.GetEmploymentVerCloserDetails(verInfo.EmploymentVerRowID);
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        EmployementVerList(model.EmploymentRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoEmploymentVer.GetPersonalDetailsForTM(model.EmploymentRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Employment id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            RatingNumber();
            BindCountry();
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateEmploymentVerInfoByTeamLeader(ViewEmploymentViewModel model)
        {
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (ModelState.IsValid)
                {
                    if (model != null && model.EmploymentRowID > 0)
                    {
                        int CheckStatusId = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        #region Update Employment Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            string DesignationName = Convert.ToString(Session["Designation"]);
                            if (model.CheckStatus != "Rejected")
                            {
                                var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(model.ClientRowID, "Manager-Rpt-QC").FirstOrDefault();
                                if (mgrData != null && mgrData.TeamMemberRowID > 0)
                                {
                                    repoVerificationInfo.UpdateEmploymentCheckRejectionByMgrOrTL(model, DesignationName, mgrData.TeamMemberRowID);
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Report QC Manager Not Found!";
                                }
                            }
                            else
                            {
                                repoVerificationInfo.UpdateEmploymentCheckRejectionByMgrOrTL(model, DesignationName);
                            }

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UpdateEmploymentVerInfoByTeamLeader", model.CheckStatus, "", "Update");
                            #endregion
                            if(model.CheckStatus != "Rejected")
                            {
                                if (model.ColorName == "Red")
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 17, "Check", teamMemerRowId);
                                }
                                else 
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 71, "Check", teamMemerRowId);
                                }
                            }
                            else
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 20, "Check", teamMemerRowId);
                            }
                         

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            ModelState.Clear();
                            if (Session["Designation"] != null && Session["Designation"].ToString() == "Team Leader-Emp")
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentTL", "Verification", new { CheckStatusId = CheckStatusId });
                            }
                            else
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentManager", "Verification");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.ListEmployment = repoDDL.GetAntecedentList(model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);

            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateEducationVerInfoByTeamLeader(int id, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0)
        {
            ViewEducationViewModel model = new ViewEducationViewModel();
            try
            {
                if (id > 0)
                {

                    var verInfo = repoEducationVer.GetEducationInfoForTMQC(id);

                    if (verInfo != null)
                    {
                        model.EducationRowID = verInfo.EducationRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoEducationVer.GetEducationVerCloserDetails(verInfo.EducationVerRowID);
                        EducationVerList(model.EducationRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoEducationVer.GetPersonalDetailsForTM(model.EducationRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateEducationVerInfoByTeamLeader(ViewEducationViewModel model)
        {
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (ModelState.IsValid)
                {
                    if (model != null && model.EducationRowID > 0)
                    {
                        int CheckStatusId = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        #region Update Education Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            string DesignationName = Convert.ToString(Session["Designation"]);
                            if (model.CheckStatus != "Rejected")
                            {
                                var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(model.ClientRowID, "Manager-Rpt-QC").FirstOrDefault();
                                if (mgrData != null && mgrData.TeamMemberRowID > 0)
                                {
                                    repoVerificationInfo.UpdateEducationCheckRejectionByMgrOrTL(model, DesignationName, mgrData.TeamMemberRowID);
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Report QC Manager Not Found!";
                                }
                            }
                            else
                            {
                                repoVerificationInfo.UpdateEducationCheckRejectionByMgrOrTL(model, DesignationName);
                            }

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UpdateEducationVerInfoByTeamLeader", model.CheckStatus, "", "Update");
                            #endregion
                            if (model.CheckStatus != "Rejected")
                            {
                                if (model.ColorName == "Red")
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 17, "Check", teamMemerRowId);
                                }
                                else
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 71, "Check", teamMemerRowId);
                                }
                            }
                            else
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 20, "Check", teamMemerRowId);
                            }

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            ModelState.Clear();
                            if (Session["Designation"] != null && Session["Designation"].ToString() == "Team Leader-Edu")
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentTL", "Verification",new {CheckStatusId= CheckStatusId });
                            }
                            else
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentManager", "Verification");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoEducationVer.GetEducationInfoForTMQC(model.EducationRowID);
            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = repoEducationVer.GetEducationVerCloserDetails(verInfo.EducationVerRowID);
            }
            EducationVerList(model.EducationRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID); model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateReferenceVerInfoByTeamLeader(int id, int checkStatusid = 0, string ColorName = "")
        {
            ViewReferenceViewModel model = new ViewReferenceViewModel();
            try
            {
                if (id > 0)
                {

                    var verInfo = repoReferenceVer.GetReferenceInfoForTMQC(id);

                    if (verInfo != null)
                    {
                        model.ReferenceRowID = verInfo.ReferenceCheckRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        model.CheckStatusId = checkStatusid;
                        model.ColorName = ColorName;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoReferenceVer.GetReferenceVerCloserDetails(verInfo.ReferenceCheckVerRowID);
                        ReferenceVerList(model.ReferenceRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoReferenceVer.GetPersonalDetailsForTM(model.ReferenceRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }


        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateSpecialVerInfoByTeamLeader(int id, int checkStatusid = 0, string ColorName = "")
        {
            ViewSpecialCheckViewModel model = new ViewSpecialCheckViewModel();
            try
            {
                if (id > 0)
                {
                    var verInfo = repoSplChkVer.GetSpecialCheckInfoForTMQC(id);

                    if (verInfo != null)
                    {
                        model.SpecialCheckRowId = verInfo.SpecialCheckRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        ViewBag.VerifyInfo = verInfo;

                        // verInfo.CloserViewModel = repoReferenceVer.GetReferenceVerCloserDetails(verInfo.SpecialCheckVerRowID);
                        BindDisposition(model.CheckFamilyRowID, model.ClientRowID);
                        BindDocumentList(model.PersonalRowID, model.SubCheckRowID);
                        SpecialCheckVerList(model.SpecialCheckRowId, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoSplChkVer.GetPersonalDetailsForTM(model.SpecialCheckRowId);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Reference id!";
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateSpecialVerInfoByTeamLeader(ViewSpecialCheckViewModel model)
        {

            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (ModelState.IsValid)
                {
                    if (model != null && model.SpecialCheckRowId > 0)
                    {
                        int CheckStatusId = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        #region Update Reference Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {

                            string DesignationName = Convert.ToString(Session["Designation"]);
                            if (model.CheckStatus != "Rejected")
                            {
                                var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(model.ClientRowID, "Manager-Rpt-QC").FirstOrDefault();
                                if (mgrData != null && mgrData.TeamMemberRowID > 0)
                                {
                                    repoVerificationInfo.UpdateSpecialCheckRejectionByMgrOrTL(model, DesignationName, mgrData.TeamMemberRowID);
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Report QC Manager Not Found!";
                                }
                            }
                            else
                            {
                                repoVerificationInfo.UpdateSpecialCheckRejectionByMgrOrTL(model, DesignationName);
                            }

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UpdateSpecialVerInfoByTeamLeader", model.CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            if (model.CheckStatus != "Rejected")
                            {
                                if (model.ColorName == "Red")
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 17, "Check", teamMemerRowId);
                                }
                                else
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 71, "Check", teamMemerRowId);
                                }
                            }
                            else
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 20, "Check", teamMemerRowId);
                            }

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            ModelState.Clear();
                            if (Session["Designation"] != null && (Session["Designation"].ToString() == "Team Leader-Ref" || Session["Designation"].ToString() == "Team Leader-SPL"))
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentTL", "Verification", new { CheckStatusId = CheckStatusId });
                            }
                            else
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentManager", "Verification");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoSplChkVer.GetSpecialCheckInfoForTMQC(model.SpecialCheckRowId);
            //if (verInfo != null)
            //{
            //    ViewBag.VerifyInfo = verInfo;
            //    verInfo.CloserViewModel = repoSplChkVer.GetspecialVerCloserDetails(verInfo.ReferenceCheckVerRowID);
            //}
            SpecialCheckVerList(model.SpecialCheckRowId, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateReferenceVerInfoByTeamLeader(ViewReferenceViewModel model)
        {

            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (ModelState.IsValid)
                {
                    if (model != null && model.ReferenceRowID > 0)
                    {
                        int CheckStatusId = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        #region Update Reference Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                           
                            string DesignationName = Convert.ToString(Session["Designation"]);
                            if (model.CheckStatus != "Rejected")
                            {
                                var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(model.ClientRowID, "Manager-Rpt-QC").FirstOrDefault();
                                if (mgrData != null && mgrData.TeamMemberRowID > 0)
                                {
                                    repoVerificationInfo.UpdateReferenceCheckRejectionByMgrOrTL(model, DesignationName, mgrData.TeamMemberRowID);
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Report QC Manager Not Found!";
                                }
                            }
                            else
                            {
                                repoVerificationInfo.UpdateReferenceCheckRejectionByMgrOrTL(model, DesignationName);
                            }

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UpdateReferenceVerInfoByTeamLeader", model.CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            if (model.CheckStatus != "Rejected")
                            {
                                if (model.ColorName == "Red")
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 17, "Check", teamMemerRowId);
                                }
                                else
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 71, "Check", teamMemerRowId);
                                }
                            }
                            else
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 20, "Check", teamMemerRowId);
                            }

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            ModelState.Clear();
                            if (Session["Designation"] != null && Session["Designation"].ToString() == "Team Leader-Ref")
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentTL", "Verification",new {CheckStatusId= CheckStatusId });
                            }
                            else
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentManager", "Verification");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoReferenceVer.GetReferenceInfoForTMQC(model.ReferenceRowID);
            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = repoReferenceVer.GetReferenceVerCloserDetails(verInfo.ReferenceCheckVerRowID);
            }
            ReferenceVerList(model.ReferenceRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateCriminalVerInfoByTeamLeader(int id, int checkStatusid = 0, string ColorName = "")
        {

            ViewCriminalViewModel model = new ViewCriminalViewModel();
            try
            {
                if (id > 0)
                {

                    var verInfo = repoCriminalVer.GetCriminalForTMQC(id);

                    if (verInfo != null)
                    {
                        model.CriminalRowID = verInfo.CriminalRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        model.CheckStatusId = checkStatusid;
                        model.ColorName = ColorName;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoCriminalVer.GetCriminalVerCloserDetails(verInfo.CriminalVerRowId);
                        CriminalVerList(model.CriminalRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoCriminalVer.GetPersonalDetailsForTM(model.CriminalRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateCriminalVerInfoByTeamLeader(ViewCriminalViewModel model)
        {
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (ModelState.IsValid)
                {
                    if (model != null && model.CriminalRowID > 0)
                    {
                        int CheckStatusId = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        #region Update Criminal Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            string DesignationName = Convert.ToString(Session["Designation"]);
                            if (model.CheckStatus != "Rejected")
                            {
                                var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(model.ClientRowID, "Manager-Rpt-QC").FirstOrDefault();
                                if (mgrData != null && mgrData.TeamMemberRowID > 0)
                                {
                                    repoVerificationInfo.UpdateCriminalCheckRejectionByMgrOrTL(model, DesignationName, mgrData.TeamMemberRowID);
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Report QC Manager Not Found!";
                                }
                            }
                            else
                            {
                                repoVerificationInfo.UpdateCriminalCheckRejectionByMgrOrTL(model, DesignationName);
                            }

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UpdateCriminalVerInfoByTeamLeader", model.CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            if (model.CheckStatus != "Rejected")
                            {
                                if (model.ColorName == "Red")
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 17, "Check", teamMemerRowId);
                                }
                                else
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 71, "Check", teamMemerRowId);
                                }
                            }
                            else
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 20, "Check", teamMemerRowId);
                            }

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            ModelState.Clear();
                            if (Session["Designation"] != null && Session["Designation"].ToString() == "Team Leader-Cri")
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentTL", "Verification",new {CheckStatusId= CheckStatusId });
                            }
                            else
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentManager", "Verification");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoCriminalVer.GetCriminalForTMQC(model.CriminalRowID);
            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = repoCriminalVer.GetCriminalVerCloserDetails(verInfo.CriminalVerRowId);
            }
            CriminalVerList(model.CriminalRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateNationalIdentityVerInfoByTeamLeader(int id, int checkStatusid = 0, string ColorName = "")
        {

            ViewNationalIdentityViewModel model = new ViewNationalIdentityViewModel();
            try
            {
                if (id > 0)
                {

                    var verInfo = repoNationalIdentityVer.GetNationalIdentityForTMQC(id);

                    if (verInfo != null)
                    {
                        model.NationalIdentityRowID = verInfo.NationalIdentityRowID;
                        model.CheckFamilyRowID = verInfo.CheckFamilyRowID;
                        model.ClientRowID = verInfo.ClientRowID;
                        model.PersonalRowID = verInfo.PersonalRowID;
                        model.SubCheckRowID = verInfo.SubCheckRowID;
                        model.CheckStatus = verInfo.CheckStatus;
                        model.CheckStatusId = checkStatusid;
                        model.ColorName = ColorName;
                        ViewBag.VerifyInfo = verInfo;
                        verInfo.CloserViewModel = repoNationalIdentityVer.GetNationalIdentityVerCloserDetails(verInfo.NationalIdentityVerRowID);
                        NationalIdentityVerList(model.NationalIdentityRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
                        model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);
                        ViewBag.PersonalBind = repoNationalIdentityVer.GetPersonalDetailsForTM(model.NationalIdentityRowID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No detail found!";
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult UpdateNationalIdentityVerInfoByTeamLeader(ViewNationalIdentityViewModel model)
        {
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (ModelState.IsValid)
                {
                    if (model != null && model.NationalIdentityRowID > 0)
                    {
                        int CheckStatusId = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        #region Update NationalIdentity Information
                        if (!string.IsNullOrEmpty(model.Remarks))
                        {
                            string DesignationName = Convert.ToString(Session["Designation"]);
                            if (model.CheckStatus != "Rejected")
                            {
                                var mgrData = repoPQClientTMember.GetDepManagerClientToTeam(model.ClientRowID, "Manager-Rpt-QC").FirstOrDefault();
                                if (mgrData != null && mgrData.TeamMemberRowID > 0)
                                {
                                    repoVerificationInfo.UpdateNationalIdentityCheckRejectionByMgrOrTL(model, DesignationName, mgrData.TeamMemberRowID);
                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Report QC Manager Not Found!";
                                }
                            }
                            else
                            {
                                repoVerificationInfo.UpdateNationalIdentityCheckRejectionByMgrOrTL(model, DesignationName);
                            }

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UpdateNationalIdentityVerInfoByTeamLeader", model.CheckStatus, "", "Update");
                            #endregion

                            if (model.CheckStatus != "Rejected")
                            {
                                if (model.ColorName == "Red")
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 17, "Check", teamMemerRowId);
                                }
                                else
                                {
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 71, "Check", teamMemerRowId);
                                }
                            }
                            else
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 20, "Check", teamMemerRowId);
                            }

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion

                            TempData["SuccessMsg"] = "Record Update Successfully!";
                            ModelState.Clear();
                            if (Session["Designation"] != null && Session["Designation"].ToString() == "Team Leader-Ide")
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentTL", "Verification", new { CheckStatusId = CheckStatusId });
                            }
                            else
                            {
                                return RedirectToAction("ManageRWQCApprovalChecksForDepartmentManager", "Verification");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid model!");
                    }
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }

                    ModelState.AddModelError("", strError);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            var verInfo = repoNationalIdentityVer.GetNationalIdentityForTMQC(model.NationalIdentityRowID);
            if (verInfo != null)
            {
                ViewBag.VerifyInfo = verInfo;
                verInfo.CloserViewModel = repoNationalIdentityVer.GetNationalIdentityVerCloserDetails(verInfo.NationalIdentityRowID);
            }
            NationalIdentityVerList(model.NationalIdentityRowID, model.ClientRowID, model.CheckFamilyRowID, model.SubCheckRowID);
            model.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(model.PersonalRowID, model.SubCheckRowID);

            return View(model);
        }

        #endregion

        #region NonAction/Json 

        private void UpdateCaseCompleted(int PersonalId)
        {
            try
            {
                bool res = repoPQPersonal.UpdateCaseCompletedByPersonalId(PersonalId);
                if (res)
                {
                    #region Case/Check Action History Save 
                    clsCommonMethods.AddCaseActionHistory(PersonalId, "Completed", "Case Completed By TeamMember QC");
                    #endregion
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var r = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");

                return !string.IsNullOrEmpty(email) && r.IsMatch(email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Send Mail To Client/Spoc/Candidate For Insuff Case

        bool SendAlertMassageForInsuffClearationForClientSpoc(short ClientRowID, int CheckRowID, short CheckFamilyRowID, short SubCheckRowID,int clientuserRowId)
        {
            bool result = false;
            string MailSignature = string.Empty;
            string From = string.Empty;
            string DisplayName = string.Empty;
            string Cc = string.Empty; //Session["CSpocEmailID"].ToString();
            clsSendMail mail;
            mail = new clsSendMail();
            var signature = mail.GetCompanyMailSignature();
            if (signature != null)
            {
                MailSignature = string.Concat("For Securitas India. <br /><br />");
                MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br /><b>Mobile: ", signature.MobileNo,
                    " | Direct Tel: ", signature.PhoneNo, " | Fax: ", signature.FaxNo, "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", signature.GeneralEmail, "'>", signature.GeneralEmail, "</a>");
            }
            From = signature.GeneralEmail;
            DisplayName = signature.CompanyName.Trim();

            /////////////////////////////////////////////////////////
            string spocEmail = "";
            string cSpocEmail = "";
            DataTable dtEmail = new DataTable();            
            //dtEmail = cc.GetEmailStructureForAlert(ClientRowID, clientuserRowId, "Insuff");


            DataTable dtAccess = cc.GetClientAccessType(ClientRowID);
            var personalData = repoPQPersonal.GetCheckOrPersonalDetailsForSendMailById(CheckFamilyRowID, SubCheckRowID, CheckRowID);
            int ClientAcctessType = 0;
            if (dtAccess.Rows.Count > 0)
            {
                ClientAcctessType = Convert.ToInt32(dtAccess.Rows[0]["ClientAccessType"].ToString());
                if (ClientAcctessType == 3)
                {
                    DataTable DtAm = new DataTable();
                    DtAm = cc.userwise_ReportingEmailAccess(personalData.ClientUserRowID);
                    try
                    {
                        if (DtAm.Rows.Count > 0)
                        {
                            spocEmail = DtAm.Rows[0]["ClientUserEmailID"].ToString();
                            cSpocEmail = DtAm.Rows[0]["ReportingTo_EmailID"].ToString();

                        }

                    }
                    catch
                    {

                    }
                }
                else
                {
                    dtEmail = cc.GetEmailStructureForAlert(ClientRowID, personalData.ClientUserRowID, "Insuff");


                    //if(model.ClientRowID==71)
                    //{

                    if (dtEmail.Rows.Count > 0)
                    {
                        spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                        cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();

                    }

                }
            }

            //if (dtEmail.Rows.Count > 0)
            //{
            //     spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
            //     cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();

            //}


            /////////////////////////////////////////////////////////////////////////
            
            if (personalData != null)
            {
                string body = string.Empty;
                if (personalData.ClientRowID == 635 || personalData.ClientRowID == 593)
                {
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/InsuffTataElxsi.html")))
                    {
                        body = reader.ReadToEnd();
                    }

                }
                else
                {
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/Insufficiency.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                }
                if (personalData.ClientRowID == 635 || personalData.ClientRowID == 593)
                {
                    DataTable dt = cc.GetTataElxsiInsuffMailDetail(personalData.PersonalRowID, personalData.CheckFamilyRowID, personalData.SubCheckRowID);
                    if (personalData.CheckFamilyRowID == 12)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            body = body.Replace("#Content", "EmployeeID");
                            body = body.Replace("{#value}", dt.Rows[0]["EmployeeID"].ToString());
                        }
                    }
                    //else if (personalData.CheckFamilyRowID == 30)
                    //{
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        body = body.Replace("#Content", "Refree name and Contact No");// 
                    //        body = body.Replace("{#value}", dt.Rows[0]["details"].ToString());
                    //    }
                    //}
                }
                body = body.Replace("{FCandidateName}", personalData.CandidateName);
                body = body.Replace("{FCheckName}", personalData.SubCheckName);
                body = body.Replace("{Verificationsource}", personalData.Verificationsource);
                body = body.Replace("{FClientRefId}", personalData.ClientRefID);
                body = body.Replace("{FReferenceID}", personalData.CompanyRefNo);
                body = body.Replace("{FInsuffRemarks}", personalData.InsuffRemarks);

                string lblempid = string.Empty;
                string empid = string.Empty;
                if (personalData.ClientRowID == 535 || personalData.ClientRowID == 533)
                {
                    DataTable dtemp = new DataTable();
                    dtemp = cc.GetEmployeeID(personalData.PersonalRowID);
                    if (dtemp.Rows.Count > 0)
                    {
                        empid = Convert.ToString(dtemp.Rows[0]["EmployeeID"]);
                        lblempid = "<td><b> Employee ID </b></td>";
                        empid = "<td> <b>" + empid + "</b></td>";
                    }
                }
                body = body.Replace("{lblEmployeeID}", lblempid);
                body = body.Replace("{valEmployeeID}", empid);

                body = body.Replace("{MailSignature}", MailSignature);
                string Subject = "";
                if (personalData.ClientRowID == 649)
                {
                    DataTable dt = cc.GetEmployeIdForInsuffmail(personalData.PersonalRowID, personalData.CheckFamilyRowID, personalData.SubCheckRowID);

                    if (dt.Rows.Count > 0)
                    {
                        string EmpId = dt.Rows[0]["EmployeeID"].ToString();
                        if (EmpId != "")
                        {
                            Subject = "Insufficiency " + personalData.CandidateName + "  " + personalData.SubCheckName + " for Employeeid " + EmpId;
                        }
                        else
                        {
                            Subject = "Insufficiency " + personalData.CandidateName + "  " + personalData.SubCheckName;
                        }

                    }
                    else
                    {
                        Subject = "Insufficiency " + personalData.CandidateName + "  " + personalData.SubCheckName;
                    }


                }
                else
                {
                     Subject = "Insufficiency " + personalData.CandidateName + "  " + personalData.SubCheckName;
                }
                
                string To = string.Empty;
                if (personalData.CompanyRefNo.ToLower().Contains("infogain"))// Stop Cvent insuff mail  remove by prateek mail                                                                    
                   To = spocEmail;    
                else
                   To = spocEmail + "," + cSpocEmail;


                string ToMail = To.ToLower();

                //ToMail = ToMail.Replace(",", ";");
                //string[] arToMail = ToMail.Split(';');
                //ToMail = "";
                //foreach (string a in arToMail)
                //{
                //    if (!ToMail.Contains(a))
                //    {
                //        ToMail = ToMail + a.Trim() + ",";
                //    }
                //}
                //if (ToMail!="")
                //{
                //    if ((ToMail.Substring(ToMail.Length - 1, 1) == ",") || (ToMail.Substring(ToMail.Length - 1, 1) == ";"))
                //        ToMail = ToMail.Substring(0, ToMail.Length - 1);
                //}



                bool flag = false;
                if (ClientAcctessType == 3)
                {
                    string cc = "";
                    string Tos = "";
                    Tos = spocEmail;
                    cc = cSpocEmail;

                    flag = mail.SendMail(DisplayName, From, Tos, cc, Subject, body);
                }
                else
                {
                    if(ClientRowID==257)
                    {
                        string bcc = "";
                        string Tos = "";
                        Tos = spocEmail;
                        bcc = cSpocEmail;
                        flag = mail.SendMail(DisplayName, From, Tos, "", bcc, Subject, body);
                    } 
                    else if(ClientRowID == 8)
                    {
                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                        cc.saveCandidatemail(personalData.PersonalRowID, "Insuff mail", ToMail, "", Subject, body, ClientRowID, 1, personalData.SubCheckRowID);
                    }
                    else
                    {
                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                    }
                    
                }
                if (flag)
                {
                    result = true;
                }
                else
                {
                    TempData["ErrorMsg"] = "Mail sending failed!";
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Candidate login details not found!";
            }
            return result;
        }

        bool SendAlertMassageForInsuffClearationForClientSpoc(short ClientRowID, int CheckRowID, short CheckFamilyRowID, short SubCheckRowID, int clientuserRowId, string CandidateEmail)
        {
            bool result = false;
            string MailSignature = string.Empty;
            string From = string.Empty;
            string DisplayName = string.Empty;
            string Cc = string.Empty; //Session["CSpocEmailID"].ToString();
            clsSendMail mail;
            mail = new clsSendMail();
            var signature = mail.GetCompanyMailSignature();
            if (signature != null)
            {
                if (ClientRowID == 249)
                {
                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" +
                    "</b><br /><b>Contact No:</b><br />  <b>Satya :</b>+91 8377022632 <br />  <b>Pradeep Devrani :</b> +91 8373907723<br /><b>Himanshu Pathak :</b> +91 7291974515 <br /> <br />" +
                    " <b>Note-You can’t login on portal now, please share the required document / details by email only and mark email to satya.prakash@securitas-india.com. </b>");
                    //Please mention the contact number of Satya - +91 8377022632 / Pradeep Devrani - 91 - 8373907723 / Himanshu Pathak - +91 7291974515
                }
                else
                {
                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br /><b>Mobile: ", signature.MobileNo,
                        " | Direct Tel: ", signature.PhoneNo, " | Fax: ", signature.FaxNo, "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", signature.GeneralEmail, "'>", signature.GeneralEmail, "</a>");
                }
            }
            From = signature.GeneralEmail;
            DisplayName = signature.CompanyName.Trim();

            /////////////////////////////////////////////////////////
            string spocEmail = "";
            string cSpocEmail = "";
            DataTable dtEmail = new DataTable();
            //dtEmail = cc.GetEmailStructureForAlert(ClientRowID, clientuserRowId, "Insuff");
            var personalData = repoPQPersonal.GetCheckOrPersonalDetailsForSendMailById(CheckFamilyRowID, SubCheckRowID, CheckRowID);
            DataTable dtAccess = cc.GetClientAccessType(ClientRowID);            
            int ClientAcctessType = 0;
            if (dtAccess.Rows.Count > 0)
            {
                ClientAcctessType = Convert.ToInt32(dtAccess.Rows[0]["ClientAccessType"].ToString());
                if (ClientAcctessType == 3)
                {
                    DataTable DtAm = new DataTable();
                    DtAm = cc.userwise_ReportingEmailAccess(personalData.ClientUserRowID);
                    try
                    {
                        if (DtAm.Rows.Count > 0)
                        {
                            spocEmail = DtAm.Rows[0]["ClientUserEmailID"].ToString();
                            cSpocEmail = DtAm.Rows[0]["ReportingTo_EmailID"].ToString();
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    dtEmail = cc.GetEmailStructureForAlert(ClientRowID, personalData.ClientUserRowID, "Insuff");
                    if (dtEmail.Rows.Count > 0)
                    {
                        spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                        cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                    }
                }
            }


            //if(model.ClientRowID==71)
            //{

            //if (dtEmail.Rows.Count > 0)
            //{
            //    spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
            //    cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();

            //}



            /////////////////////////////////////////////////////////////////////////
            if (personalData != null)
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/Insufficiency.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{FCandidateName}", personalData.CandidateName);
                body = body.Replace("{FCheckName}", personalData.SubCheckName);
                body = body.Replace("{Verificationsource}", personalData.Verificationsource);
                body = body.Replace("{FClientRefId}", personalData.ClientRefID);
                body = body.Replace("{FReferenceID}", personalData.CompanyRefNo);
                body = body.Replace("{FInsuffRemarks}", personalData.InsuffRemarks);

                string lblempid = string.Empty;
                string empid = string.Empty;
                if (personalData.ClientRowID == 535 || personalData.ClientRowID == 533)
                {
                    DataTable dtemp = new DataTable();
                    dtemp = cc.GetEmployeeID(personalData.PersonalRowID);
                    if (dtemp.Rows.Count > 0)
                    {
                        empid = Convert.ToString(dtemp.Rows[0]["EmployeeID"]);
                        lblempid = "<td><b> Employee ID </b></td>";
                        empid = "<td>" + empid + "</td>";
                    }
                }
                body = body.Replace("{lblEmployeeID}", lblempid);
                body = body.Replace("{valEmployeeID}", empid);

                body = body.Replace("{MailSignature}", MailSignature);
                string Subject = "Insufficiency " + personalData.CandidateName + "  " + personalData.SubCheckName;
                string To = string.Empty;
                if (personalData.CompanyRefNo.ToLower().Contains("infogain") || ClientRowID == 8)// Stop Cvent insuff mail    
                {
                    To = spocEmail;
                }
                else if (ClientRowID == 277 || ClientRowID == 249)
                    {
                    To = spocEmail + "," + cSpocEmail ;
                }
                else
                {
                    To = spocEmail + "," + cSpocEmail + "," + CandidateEmail;

                }


                string ToMail = To.ToLower();

                ToMail = ToMail.Replace(",", ";");
                string[] arToMail = ToMail.Split(';');
                ToMail = "";
                foreach (string a in arToMail)
                {
                    if (!ToMail.Contains(a))
                    {
                        ToMail = ToMail + a.Trim() + ",";
                    }
                }
                if (ToMail.Length > 1)
                    ToMail = ToMail.Substring(0, ToMail.Length - 1);

                bool flag = false;
                if (ClientAcctessType == 3)
                {
                    string ccc = "";
                    string Tos = "";
                    Tos = spocEmail;
                    ccc = cSpocEmail;
                   flag = mail.SendMail(DisplayName, From, Tos, ccc, Subject, body);
                }
                else
                {
                    if (ClientRowID == 257)
                    {
                        string bcc = "";
                        string Tos = "";
                        Tos = spocEmail;
                        bcc = cSpocEmail;
                      flag = mail.SendMail(DisplayName, From, Tos, "", bcc, Subject, body);
                    }
                    if ((ClientRowID == 277 || ClientRowID == 249 || ClientRowID == 206) && CandidateEmail!=null)
                    {

                       
                        string ccc = "";
                        string Tos = "";
                        Tos = CandidateEmail;
                        ccc = ToMail;
                        if(ClientRowID == 206)
                        {
                            To = ToMail ;
                            flag = mail.SendMail(DisplayName, From, To, Subject, body);
                            cc.saveCandidatemail(personalData.PersonalRowID, "Insuff mail", To, "", Subject, body, ClientRowID, 1, personalData.SubCheckRowID);

                        }
                        else
                        {
                            flag = mail.SendMail(DisplayName, From, Tos, ccc, Subject, body);
                            cc.saveCandidatemail(personalData.PersonalRowID, "Insuff mail", Tos, ccc, Subject, body, ClientRowID, 1, personalData.SubCheckRowID);

                        }

                    }
                    else
                    {
                       flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                    }
                }
                 
                if (flag)
                {
                    result = true;
                }
                else
                {
                    TempData["ErrorMsg"] = "Mail sending failed!";
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Candidate login details not found!";
            }
            return result;
        }

        bool SendAlertMassageForInsuffClearationForCandidate(short ClientRowID, int CheckRowID, short CheckFamilyRowID, short SubCheckRowID, int PersonalRowID, string CandidateEmail)
        {
            bool result = false;
            string MailSignature = string.Empty;
            string From = string.Empty;
            string DisplayName = string.Empty;

            #region Save CandidateLogin Details
            AddPQCandidateLoginViewModel cModel = new AddPQCandidateLoginViewModel();
            string passwrd = clsCryptoRandomNumber.GetRandomAlphaNumeric();
            string EncryptedPasswd = clsCommonMethods.PasswordEncrypt(passwrd);
            cModel.ClientRowID = ClientRowID;
            cModel.PersonalRowID = PersonalRowID;
            cModel.UserID = CandidateEmail;
            cModel.UPass = EncryptedPasswd;
            cModel.CreatedBy = "Company";
            cModel.CreatedTime = DateTime.Now;
            cModel.Status = 1;
            cModel = repoPQCandidateLogin.AddInsuffCandidateLogin(cModel);
            #endregion

            clsSendMail mail;
            var cSmtp = repoPQClient.GetPQClientSendMailFromById(ClientRowID);
            if (cSmtp != null && cSmtp.MailSendBy > 0 && !string.IsNullOrEmpty(cSmtp.SMTPServer) && !string.IsNullOrEmpty(cSmtp.SMTPPort) && !string.IsNullOrEmpty(cSmtp.SMTPUserName) && !string.IsNullOrEmpty(cSmtp.SMTPPassword))
            {
                mail = new clsSendMail(cSmtp.SMTPServer, cSmtp.SMTPPort, cSmtp.SMTPUserName, clsCommonMethods.PasswordDecrypt(cSmtp.SMTPPassword), Convert.ToBoolean(cSmtp.EnableSsl));
                From = cSmtp.CSendInsuffEmail != null ? cSmtp.CSendInsuffEmail : cSmtp.SMTPUserName;
                DisplayName = cSmtp.CSendInsuffDisplay != null ? cSmtp.CSendInsuffDisplay : cSmtp.ClientName.Trim();
                MailSignature = string.Concat("<b>", cSmtp.ClientName.Trim(), "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", cSmtp.SMTPUserName, "'>", cSmtp.SMTPUserName, "</a>");
            }
            else
            {
                mail = new clsSendMail();
                var signature = mail.GetCompanyMailSignature();
                if (signature != null)
                {
                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br /><b>Mobile: ", signature.MobileNo,
                        " | Direct Tel: ", signature.PhoneNo, " | Fax: ", signature.FaxNo, "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", signature.GeneralEmail, "'>", signature.GeneralEmail, "</a>");
                }
                From = signature.GeneralEmail;
                DisplayName = signature.CompanyName.Trim();
            }

            var personalData = repoPQPersonal.GetCheckOrPersonalDetailsForSendMailById(CheckFamilyRowID, SubCheckRowID, CheckRowID);
            if (personalData != null)
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/insuffCandidate.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{CandidateName}", personalData.CandidateName);
                body = body.Replace("{ClientRefId}", personalData.ClientRefID);
                body = body.Replace("{ExpireDate}", DateTime.Now.AddDays(5).ToString());
                body = body.Replace("{UName}", cModel.UserID);
                body = body.Replace("{UPasswd}", clsCommonMethods.PasswordDecrypt(cModel.UPass.Trim()));
                body = body.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                body = body.Replace("{CheckName}", personalData.SubCheckName);
                body = body.Replace("{Remarks}", personalData.InsuffRemarks);

                body = body.Replace("{MailSignature}", MailSignature);
                string Subject = "Insufficiency";
                string To = cModel.UserID; // personalData.SpocEmailID + "," + personalData.ClientEmailID; 

                string ToMail = To.ToLower();

                ToMail = ToMail.Replace(",", ";");
                string[] arToMail = ToMail.Split(';');
                ToMail = "";
                foreach (string a in arToMail)
                {
                    if (!ToMail.Contains(a))
                    {
                        ToMail = ToMail + a.Trim() + ",";
                    }
                }
                if (ToMail.Length > 1)
                    ToMail = ToMail.Substring(0, ToMail.Length - 1);



                bool flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                if (flag)
                {
                    result = true;
                }
                else
                {
                    TempData["ErrorMsg"] = "Mail sending failed!";
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Candidate login details not found!";
            }
            return result;
        }
        #endregion

        //Get ActionHistory In Address Verification
        //[NonAction]
        //void BindCheckActionHistories(int PersonalRowID = 0, short subCheckId = 0)
        //{
        //    try
        //    {
        //        ViewBag.CheckActionHistories = repoDDL.GetCheckActionHistory(PersonalRowID, subCheckId);
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.ErrorMsg = Ex.Message;
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //}

        //Get DocumentList In Address Verification 


        //[NonAction]
        //void BindMCADocumentList(int PersonalRowID = 0, short subCheckId = 0)
        //{
        //    try
        //    {
        //        ViewBag.GetMCADocumentList = repoDDL.GetMCADocumentList(PersonalRowID, subCheckId);
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.ErrorMsg = Ex.Message;
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //}
        [NonAction]
        void BindDocumentList(int PersonalRowID = 0, short subCheckId = 0)
        {
            try
            {
                ViewBag.GetDocumentList = repoDDL.PQAddressVerGetDocumentList(PersonalRowID, subCheckId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        #region Send Mail To Client Or Spoc

        bool SendAlertMassageForFundPending(short ClientRowID, int CheckRowID, short CheckFamilyRowID, short SubCheckRowID, int clientuserRowId)
        {
            bool result = false;
            string MailSignature = string.Empty;
            string From = string.Empty;
            string DisplayName = string.Empty;
            string Cc = string.Empty; //Session["CSpocEmailID"].ToString();

            clsSendMail mail;
            mail = new clsSendMail();
            var signature = mail.GetCompanyMailSignature();

            if (signature != null)
            {
                if(ClientRowID==249)
                {
                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" +
                    "</b><br /><b>Contact No:</b><br />  <b>Satya :</b>+91 8377022632 <br />  <b>Pradeep Devrani :</b> +91 8373907723<br /><b>Himanshu Pathak :</b> +91 7291974515 ");
                    //Please mention the contact number of Satya - +91 8377022632 / Pradeep Devrani - 91 - 8373907723 / Himanshu Pathak - +91 7291974515
                }
                else
                {
                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br /><b>Mobile: ", signature.MobileNo,
                        " | Direct Tel: ", signature.PhoneNo, " | Fax: ", signature.FaxNo, "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", signature.GeneralEmail, "'>", signature.GeneralEmail, "</a>");

                }

            }
            //string cmpSpocEmails = repoPQClient.GetCompanySPOCEmailIdFromPQClientMasterById(ClientRowID);
            //Cc = cmpSpocEmails;



            From = signature.GeneralEmail;
            DisplayName = signature.CompanyName.Trim();
            string spocEmail = "";
            string cSpocEmail = "";
            DataTable dtEmail = new DataTable();
            DataTable dtAccess = cc.GetClientAccessType(ClientRowID);
            var personalData = repoPQPersonal.GetCheckOrPersonalDetailsForSendMailById(CheckFamilyRowID, SubCheckRowID, CheckRowID);
            int ClientAcctessType = 0;
            if (dtAccess.Rows.Count > 0)
            {
                ClientAcctessType = Convert.ToInt32(dtAccess.Rows[0]["ClientAccessType"].ToString());
                if (ClientAcctessType == 3)
                {
                    DataTable DtAm = new DataTable();
                    DtAm = cc.userwise_ReportingEmailAccess(personalData.ClientUserRowID);
                    try
                    {
                        if (DtAm.Rows.Count > 0)
                        {
                            spocEmail = DtAm.Rows[0]["ClientUserEmailID"].ToString();
                            cSpocEmail = DtAm.Rows[0]["ReportingTo_EmailID"].ToString();

                        }

                    }
                    catch
                    {

                    }
                }
                else
                {
                    dtEmail = cc.GetEmailStructureForAlert(ClientRowID, personalData.ClientUserRowID, "Funds Pending");


                    //if(model.ClientRowID==71)
                    //{

                    if (dtEmail.Rows.Count > 0)
                    {
                        spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                        cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();

                    }

                }
            }

            //dtEmail = cc.GetEmailStructureForAlert(ClientRowID, clientuserRowId, "Funds Pending");
           
            

           

            //if (dtEmail.Rows.Count > 0)
            //{
            //    spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
            //    cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();

            //}
            
            if (personalData != null)
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/fundpending.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{FCandidateName}", personalData.CandidateName);
                body = body.Replace("{FTAT}", Convert.ToString(personalData.InternalTAT));
                body = body.Replace("{FCheckName}", personalData.SubCheckName);
                body = body.Replace("{FPRemarks}", personalData.Remarks);
                body = body.Replace("{FRefNo}", personalData.CompanyRefNo);

                body = body.Replace("{MailSignature}", MailSignature);


                string Subject = "";
                if (personalData.ClientRowID == 649)
                {
                    DataTable dt = cc.GetEmployeIdForInsuffmail(personalData.PersonalRowID, personalData.CheckFamilyRowID, personalData.SubCheckRowID);

                    if (dt.Rows.Count > 0)
                    {
                        string EmpId = dt.Rows[0]["EmployeeID"].ToString();
                        if (EmpId != "")
                        {
                            Subject = "Funds Pending " + personalData.CandidateName + "  " + personalData.SubCheckName + " for Employeeid " + EmpId;
                        }
                        else
                        {
                            Subject = "Funds Pending " + personalData.CandidateName + "  " + personalData.SubCheckName;
                        }

                    }
                    else
                    {
                        Subject = "Funds Pending " + personalData.CandidateName + "  " + personalData.SubCheckName;
                    }


                }
                else
                {
                    Subject = "Funds Pending " + personalData.CandidateName + "  " + personalData.SubCheckName;
                }


                //string Subject = "Funds Pending " + personalData.CandidateName + "  " + personalData.SubCheckName;
                //string To = personalData.SpocEmailID + "," + personalData.ClientEmailID;
         
                string To = string.Empty;
                if (personalData.CompanyRefNo.ToLower().Contains("infogain") )// Stop fund pending  mail
                    To = spocEmail;
                else
                    To = spocEmail + "," + cSpocEmail;


                string ToMail = To.ToLower();

                ToMail = ToMail.Replace(",", ";");
                string[] arToMail = ToMail.Split(';');
                ToMail = "";
                foreach (string a in arToMail)
                {
                    if (!ToMail.Contains(a))
                    {
                        ToMail = ToMail + a.Trim() + ",";
                    }
                }
                if (ToMail.Length > 1)
                    ToMail = ToMail.Substring(0, ToMail.Length - 1);
                bool flag = false;
                if (ClientAcctessType == 3)
                {
                    string cc = "";
                    string Tos = "";
                    Tos = spocEmail;
                    cc = cSpocEmail;

                    flag = mail.SendMail(DisplayName, From, Tos, cc, Subject, body);
                }
                else
                {
                    if (ClientRowID == 257)
                    {
                        string bcc = "";
                        string Tos = "";
                        Tos = spocEmail;
                        bcc = cSpocEmail;
                        flag = mail.SendMail(DisplayName, From, Tos, "", bcc, Subject, body);
                    }
                    else
                    {
                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                    }
                }


                //bool flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                if (flag)
                {
                    result = true;
                }
                else
                {
                    TempData["ErrorMsg"] = "Mail sending failed!";
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Candidate Check details not found!";
            }
            return result;
        }
        #endregion

        [NonAction]
        void BindClientList()
        {
            try
            {
                ViewBag.ClientList = repoDDL.GetClientListWithAbbAndLocationForDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindClientListBaseOnLogin(short TeamMemberRowId = 0)
        {
            try
            {
                ViewBag.ClientList = repoDDL.GetClientBaseOnloginMemberForDDL(TeamMemberRowId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        void BindStateList()
        {
            short CountryRowID = 106;
            try
            {
                ViewBag.StateList = repoDDL.GetStatesByCountryIdForDDL(CountryRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        void BindStateVendor(short teamMemerRowId,string CheckStatus)
        {
            short CountryRowID = 106;
            try
            {
                ViewBag.StateList = repoDDL.GetStatesByCountryIdForDDL(CountryRowID, teamMemerRowId, CheckStatus);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
       

        public JsonResult GetSeverityColor(short ClientRowId = 0, string Dispositions = "")
        {
            try
            {

                if (ClientRowId > 0 && !String.IsNullOrEmpty(Dispositions))
                {

                    PQAddressVerGetColorNameViewModel model = new PQAddressVerGetColorNameViewModel();
                    try
                    {
                        model = repoAddressVer.PQAddressVerGetColorName(ClientRowId, Dispositions);
                    }
                    catch (Exception Ex)
                    {

                        ViewBag.ErrorMsg = Ex.Message;
                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                    }
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetColorBySeverity(short ClientRowId = 0, byte SeverityRowId = 0)
        {
            try
            {
                if (SeverityRowId > 0)
                {
                    string ColorName = "0";
                    try
                    {
                        ColorName = repoAddressVer.VerGetColorName(ClientRowId, SeverityRowId);
                    }
                    catch (Exception Ex)
                    {
                        ViewBag.ErrorMsg = Ex.Message;
                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                    }
                    return Json(ColorName, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetTLMemberForAllocate(short id = 0)
        {
            try
            {

                if (id > 0)
                {
                    string Designation = Session["Designation"].ToString();

                    if (Designation == "Manager-Add")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Leader-Add");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Manager-Emp")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Leader-Emp");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Manager-Edu")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Leader-Edu");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Manager-Cri")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Leader-Cri");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Manager-Ref")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Leader-Ref");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Manager-Ide")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Leader-Ide");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }

                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GetTLMemberForAllocate()
        {
            try
            {
                short depMgrID = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (depMgrID > 0)
                {
                    string Designation = Session["Designation"].ToString();

                    if (Designation == "Manager-Add")
                    {
                        ViewBag.depTeamLeader = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Leader-Add");
                    }
                    else if (Designation == "Manager-Emp")
                    {
                        ViewBag.depTeamLeader = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Leader-Emp");
                    }
                    else if (Designation == "Manager-Edu")
                    {
                        ViewBag.depTeamLeader = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Leader-Edu");
                    }
                    else if (Designation == "Manager-Cri")
                    {
                        ViewBag.depTeamLeader = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Leader-Cri");
                    }
                    else if (Designation == "Manager-Ref")
                    {
                        ViewBag.depTeamLeader = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Leader-Ref");
                    }
                    else if (Designation == "Manager-Ide")
                    {
                        ViewBag.depTeamLeader = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Leader-Ide");
                    }
                    else if (Designation == "Manager-SPL")
                    {
                        ViewBag.depTeamLeader = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Leader-SPL");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult GetTMMemberForAllocate(short id = 0)
        {
            try
            {

                if (id > 0)
                {
                    string Designation = Session["Designation"].ToString();

                    if (Designation == "Team Leader-Add")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Add");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Emp")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Emp");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Edu")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Edu");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Cri")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Cri");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Ref")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Ref");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Ide")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Ide");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }

                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult GetTMQCMemberForAllocate(short id = 0)
        {
            try
            {

                if (id > 0)
                {
                    string Designation = Session["Designation"].ToString();

                    if (Designation == "Administrator")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Add-QC");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Add")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Add-QC");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Emp")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Emp-QC");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Edu")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Edu-QC");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Cri")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Cri-QC");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Ref")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Ref-QC");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }
                    else if (Designation == "Team Leader-Ide")
                    {
                        var DETM = repoPQClientTMember.GetRepoClientToTeam(id, "Team Member-Ide-QC");
                        return Json(new SelectList(DETM.ToArray(), "TeamMemberRowID", "TeamMemberName"), JsonRequestBehavior.AllowGet);
                    }

                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GetTMMemberForAllocate()
        {
            try
            {
                short depMgrID = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (depMgrID > 0)
                {
                    string Designation = Session["Designation"].ToString();

                    if (Designation == "Team Leader-Add")
                    {
                        ViewBag.depTM = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Member-Add");
                    }
                    else if (Designation == "Team Leader-Emp")
                    {
                        ViewBag.depTM = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Member-Emp");
                    }
                    else if (Designation == "Team Leader-Edu")
                    {
                        ViewBag.depTM = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Member-Edu");
                    }
                    else if (Designation == "Team Leader-Cri")
                    {
                        ViewBag.depTM = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Member-Cri");
                    }
                    else if (Designation == "Team Leader-Ref")
                    {
                        ViewBag.depTM = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Member-Ref");
                    }
                    else if (Designation == "Team Leader-Ide")
                    {
                        ViewBag.depTM = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Member-Ide");
                    }
                    else if (Designation == "Team Leader-SPL")
                    {
                        ViewBag.depTM = repoPQClientTMember.GetRepoClientToTeam(depMgrID, "Team Member-SPL");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GetTMQCMemberForAllocate()
        {
            try
            {
                short depTMQCID = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (depTMQCID > 0)
                {
                    string Designation = Session["Designation"].ToString();

                    if (Designation == "Administrator")
                    {
                        ViewBag.depTMQC = repoPQClientTMember.GetRepoClientToTeam(depTMQCID, "Team Member-Add-QC");
                    }
                    else if (Designation == "Team Leader-Add")
                    {
                        ViewBag.depTMQC = repoPQClientTMember.GetRepoClientToTeam(depTMQCID, "Team Member-Add-QC");
                    }
                    else if (Designation == "Team Leader-Emp")
                    {
                        ViewBag.depTMQC = repoPQClientTMember.GetRepoClientToTeam(depTMQCID, "Team Member-Emp-QC");
                    }
                    else if (Designation == "Team Leader-Edu")
                    {
                        ViewBag.depTMQC = repoPQClientTMember.GetRepoClientToTeam(depTMQCID, "Team Member-Edu-QC");
                    }
                    else if (Designation == "Team Leader-Cri")
                    {
                        ViewBag.depTMQC = repoPQClientTMember.GetRepoClientToTeam(depTMQCID, "Team Member-Cri-QC");
                    }
                    else if (Designation == "Team Leader-Ref")
                    {
                        ViewBag.depTMQC = repoPQClientTMember.GetRepoClientToTeam(depTMQCID, "Team Member-Ref-QC");
                    }
                    else if (Designation == "Team Leader-Ide")
                    {
                        ViewBag.depTMQC = repoPQClientTMember.GetRepoClientToTeam(depTMQCID, "Team Member-Ide-QC");
                    }
                    else if (Designation == "Team Leader-SPL")
                    {
                        ViewBag.depTMQC = repoPQClientTMember.GetRepoClientToTeam(depTMQCID, "Team Member-SPL-QC");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GetPTRMgrForAllocate()
        {
            try
            {
                short depTMID = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (depTMID > 0)
                {
                    string Designation = Session["Designation"].ToString();

                    if (Designation == "Team Member-Add")
                    {
                        ViewBag.PTRManagerList = repoPQClientTMember.GetRepoClientToTeam(depTMID, "Allocate-PTR");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //short GetCheckFamilyIdByDesignationName(string designation ="")
        //{
        //    short id = 0;
        //    if (designation== "Manager-Add")
        //    {
        //        id = 2;
        //    }
        //    if (designation == "Manager-Emp")
        //    {
        //        id = 12;
        //    }
        //    if (designation == "Manager-Edu")
        //    {
        //        id = 11;
        //    }
        //    if (designation == "Manager-Cri")
        //    {
        //        id = 5;
        //    }
        //    if (designation == "Manager-OC")
        //    {
        //        id = 0;
        //    }

        //    return id;
        //}

        [NonAction]
        void BindCountry()
        {
            try
            {
                ViewBag.Countries = repoDDL.GetCountriesForDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindStates()
        {
            try
            {
                ViewBag.States = repoDDL.GetStatesByArressStatesIdForDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindPartneres()
        {
            try
            {
                ViewBag.Partner = repoDDL.GetPartnerByArressPartnerIdForDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindCities()
        {
            try
            {
                ViewBag.Cities = repoDDL.GetCityByArressStatesIdForDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindDisposition(short ChFamilRowId = 0, short ClientRowId = 0)
        {
            try
            {

                var DispositionList = repoDDL.DispositionForDDL(ChFamilRowId, ClientRowId);
                ViewBag.Disposition = new MultiSelectList(DispositionList, "DispositionRowId", "Disposition");
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindColorCode()
        {
            try
            {
                ViewBag.ColorCode = repoDDL.GetColorDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindRelationship()
        {
            try
            {
                IEnumerable<SelectListItem> RelationshipList = new List<SelectListItem> {
            new SelectListItem { Text = "Parent", Value = "Parent" },
            new SelectListItem { Text = "Father", Value = "Father" },
            new SelectListItem { Text = "Mother ", Value = "Mother" },
            new SelectListItem { Text = "Child", Value = "Child" },
            new SelectListItem { Text = "Spouse", Value = "Spouse" },
            new SelectListItem { Text = "Sibling", Value = "Sibling" },
            new SelectListItem { Text = "Grandparents", Value = "Grandparents" },
            new SelectListItem { Text = "Grandchild", Value = "Grandchild" },
            new SelectListItem { Text = "Parent's Sibling", Value = "Parent's Sibling" },
            };
                ViewBag.RelationshipList = RelationshipList;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void TotalNumber()
        {
            try
            {
                IEnumerable<SelectListItem> Number = new List<SelectListItem>
            {
                new SelectListItem { Text = "1" , Value = "1" },
                new SelectListItem { Text = "2" , Value = "2" },
                new SelectListItem { Text = "3" , Value = "3" },
                new SelectListItem { Text = "4" , Value = "4" },
                new SelectListItem { Text = "5" , Value = "5" },
                new SelectListItem { Text = "6" , Value = "6" },
                new SelectListItem { Text = "7" , Value = "7" },
                new SelectListItem { Text = "8" , Value = "8" },
                new SelectListItem { Text = "9" , Value = "9" },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "11", Value = "11" },
                new SelectListItem { Text = "12", Value = "12" },
                new SelectListItem { Text = "13", Value = "13" },
                new SelectListItem { Text = "14", Value = "14" },
                new SelectListItem { Text = "15", Value = "15" },
                new SelectListItem { Text = "16", Value = "16" },
                new SelectListItem { Text = "17", Value = "17" },
                new SelectListItem { Text = "18", Value = "18" },
                new SelectListItem { Text = "19", Value = "19" },
                new SelectListItem { Text = "20", Value = "20" },
            };
                ViewBag.TotalNumberes = Number;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void RatingNumber()
        {
            try
            {
                IEnumerable<SelectListItem> Numbers = new List<SelectListItem>
            {
              new SelectListItem { Text = "1", Value = "1" },
              new SelectListItem { Text = "2", Value = "2" },
              new SelectListItem { Text = "3", Value = "3" },
              new SelectListItem { Text = "4", Value = "4" },
              new SelectListItem { Text = "5", Value = "5" },
              new SelectListItem { Text = "6", Value = "6" },
              new SelectListItem { Text = "7", Value = "7" },
              new SelectListItem { Text = "8", Value = "8" },
              new SelectListItem { Text = "9", Value = "9" },
              new SelectListItem { Text = "10", Value = "10" },
            };
                ViewBag.RatingNumbers = Numbers;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void CriminalOptionDropDown()
        {
            try
            {
                IEnumerable<SelectListItem> CriminalOption = new List<SelectListItem>
            {
              new SelectListItem { Text = "Match Found", Value = "Match Found" },
              new SelectListItem { Text = "No Match Found ", Value = "No Match Found " },
              new SelectListItem { Text = "Possible  Match found", Value = " Possible  Match found" },
              new SelectListItem { Text = "Record Found", Value = "Record Found" },
              new SelectListItem { Text = "No Record found ", Value = "No Record found" },
              new SelectListItem { Text = "Other", Value = "Other" },
            };
                ViewBag.CriminalOptionList = CriminalOption;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        public void BindAffliatiatedfrom()
        {
            try
            {
                IEnumerable<SelectListItem> AffliatiatedfromList = new List<SelectListItem> {
            new SelectListItem { Text = "UGC", Value = "UGC" },
            new SelectListItem { Text = "AICTE", Value = "AICTE" },
            new SelectListItem { Text = "DEC", Value = "DEC" },
            new SelectListItem { Text = "AIMA", Value = "AIMA" },
            new SelectListItem { Text = "Others", Value = "Others" },
            };
                ViewBag.AffliatiatedfromList = AffliatiatedfromList;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        public void BindUniversityStatusList()
        {
            try
            {

                IEnumerable<SelectListItem> UniverSityStatusList = new List<SelectListItem> {
            new SelectListItem { Text = "Fake", Value = "Fake" },
            new SelectListItem { Text = "Genuine", Value = "Genuine" },
            new SelectListItem { Text = "Shutdown", Value = "Shutdown" },
            new SelectListItem { Text = "Shifted", Value = "Shifted" },
            new SelectListItem { Text = "Operation ceased", Value = "Operation ceased" },
            new SelectListItem { Text = "Small Institute", Value = "Small Institute" },
            //new SelectListItem { Text = "AIMA", Value = "AIMA" },
            new SelectListItem { Text = "Others", Value = "Others" },
            };
                ViewBag.UniverSityStatusList = UniverSityStatusList;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [NonAction]
        public void GetDegreeList()
        {
            try
            {
                ViewBag.DegreeList = repoDDL.GetDegreeDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        public void GetUniversityList()
        {
            try
            {
                ViewBag.UniversityList = repoDDL.GetUniversityForDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        //[NonAction]
        //public void GetDegreeList(int EV_Degree_Name)
        //{
        //    try
        //    {
        //        if (EV_Degree_Name > 0)
        //        {                    
        //            ViewBag.DegreeList = repoDDL.GetDegreeDDLForVerifier(EV_Degree_Name);
        //        }
        //        else
        //        {
        //            ViewBag.DegreeList = repoDDL.GetDegreeDDL();
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.ErrorMsg = Ex.Message;
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //}

        [NonAction]
        public void GetCollegeDDL()
        {
            try
            {
                ViewBag.CollegeList = repoDDL.GetCollegeDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        //[NonAction]
        //public void GetCollegeDDL(int CollegeRowID)
        //{
        //    try
        //    {
        //        if (CollegeRowID>0)
        //        {                 
        //            ViewBag.CollegeList = repoDDL.GetCollegeDDLForVerification(CollegeRowID);
        //        }
        //        else
        //        {
        //            ViewBag.CollegeList = repoDDL.GetCollegeDDL();
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.ErrorMsg = Ex.Message;
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //}

        public void GetVendorDDL(short StateRowId = 0)
        {
            try
            {
                ViewBag.VendorList = repoDDL.GetVendorForDDL(StateRowId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public void BindCityByStateId(short StateRowId = 0, short teamMemerRowId=0)
        {
            try
            {
                if(StateRowId!=0)
                {
                    ViewBag.CityList = repoDDL.GetCityForDDL(StateRowId, teamMemerRowId);
                }
                
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void CheckStatusOptionDropDown()
        {
            try
            {
                IEnumerable<SelectListItem> CheckStatusOption = new List<SelectListItem>
            {
                    new SelectListItem { Text = "Allocated-Mgr", Value = "Allocated-Mgr" },
                    new SelectListItem { Text = "Allocated-TL", Value = "Allocated-TL" },
                    new SelectListItem { Text = "Completed-QC", Value = "Completed-QC" },
                    new SelectListItem { Text = "RW-Approval-TL", Value = "RW-Approval-TL" },
                    new SelectListItem { Text = "RW-Approval-Mgr", Value = "RW-Approval-Mgr" },
                    new SelectListItem { Text = "WIP-Pending", Value = "WIP-Pending" },
                    new SelectListItem { Text = "Completed", Value = "Completed" },
                    new SelectListItem { Text = "Stop", Value = "Stop" },
                    new SelectListItem { Text = "Insufficent", Value = "Insufficent" },
                    new SelectListItem { Text = "Review request", Value = "Review request" },
                    new SelectListItem { Text = "YTR - Hold", Value = "YTR - Hold" },
                    new SelectListItem { Text = "Rejected", Value = "Rejected" },
                    new SelectListItem { Text = "PV Raised", Value = "PV Raised" },
                    new SelectListItem { Text = "PV Rejected", Value = "PV Rejected" },
                    new SelectListItem { Text = "Funds Pending", Value = "Funds Pending" },
                    new SelectListItem { Text = "Verified - Clarification", Value = "Verified - Clarification" },
                    new SelectListItem { Text = "Insuff Rectified", Value = "Insuff Rectified" },
                    new SelectListItem { Text = "Research", Value = "Research" },
                    new SelectListItem { Text = "Allocated-PTR", Value = "Allocated-PTR" },
            };
                ViewBag.CheckStatusOptionList = CheckStatusOption;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void CaseStatusOptionDropDown()
        {
            try
            {
                IEnumerable<SelectListItem> CaseStatusOption = new List<SelectListItem>
            {
              new SelectListItem { Text = "Allocated", Value = "Allocated" },
              new SelectListItem { Text = "WIP", Value = "WIP" },
              new SelectListItem { Text = "Funds Pending", Value = "Funds Pending" },
              new SelectListItem { Text = "Insufficent", Value = "Insufficent" },
              new SelectListItem { Text = "Review Pending", Value = "Review Pending" },
              new SelectListItem { Text = "YTR Hold", Value = "YTR Hold" },
              new SelectListItem { Text = "Completed", Value = "Completed" },
              new SelectListItem { Text = "Re-open-WIP", Value = "Re-open-WIP" },
              new SelectListItem { Text = "Re-open- Insuff", Value = "Re-open- Insuff" },
              new SelectListItem { Text = "Re-open Completed", Value = "Re-open Completed" },
              new SelectListItem { Text = "QC Pending", Value = "QC Pending" },
              new SelectListItem { Text = "QC Rejected", Value = "QC Rejected" },
              new SelectListItem { Text = "Stopped", Value = "Stopped" },
            };
                ViewBag.CaseStatusOptionList = CaseStatusOption;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        #endregion

        #region ********** Export Excel To Address **********
        [HttpGet]
        [SessionExpireFilterAttribute]
        public void ExportAddressMge(short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "", string ReAllocation = "")
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string Designation = Convert.ToString(Session["Designation"]);
            try
            {

                #region Address Excel Export
                if (Designation == "Manager-Add")
                {
                    var data = repoVerificationInfo.GetExportCandidateAddressListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);

                    if (CheckStatus == "Allocated-Mgr")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "AddressMge");
                    }
                    else
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new ExportVerificationAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTL = item.AllocatedTLName,
                            AllocatedTM = item.AllocatedTMName,
                            Partner = item.Partner,
                            Location = item.Location,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "AddressMge");
                    }

                }
                if (Designation == "Team Leader-Add" || Designation == "Team Member-Add-QC")
                {
                    var data = repoVerificationInfo.GetExportCandidateAddressListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "AllocationToTM" || CheckStatus == "ReAllocation")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "AddressMge");
                    }
                    else
                    {
                        var data2 = data.PQVerificationInfo.Select(item => new ExportVerificationAddTLViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTM = item.AllocatedTMName,
                            Partner = item.Partner,
                            Location = item.Location,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data2, "AddressTLOrQC");
                    }
                }
                if (Designation == "Team Member-Add")
                {
                    var data = repoVerificationInfo.GetExportCandidateAddressListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "NonInitiated")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "AddressTm");
                    }
                    else
                    {
                        var data3 = data.PQVerificationInfo.Select(item => new ExportVerificationAddTMViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            Partner = item.Partner,
                            Location = item.Location,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data3, "AddressTM");
                    }
                }
                #endregion

                #region Criminal Excel Export

                if (Designation == "Manager-Cri")
                {
                    var data = repoVerificationInfo.GetExportCandidateCriminalListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "Allocated-Mgr")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "CriminalMge");
                    }
                    else
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new ExportVerificationAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTL = item.AllocatedTLName,
                            AllocatedTM = item.AllocatedTMName,
                            Partner = item.Partner,
                            Location = item.Location,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "CriminalMge");
                    }
                }
                if (Designation == "Team Leader-Cri" || Designation == "Team Member-Cri-QC")
                {
                    var data = repoVerificationInfo.GetExportCandidateCriminalListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus, ReAllocation);
                    if (CheckStatus == "AllocationToTM" || CheckStatus == "ReAllocation")
                    {
                        ClientCustomize cc = new ClientCustomize();
                        DataSet ds = new DataSet();
                        ds=cc.ExportDataForTLAllocation(5);//5 is for criminal family

                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];

                        using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add(dt);
                            ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                            //wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            wb.Style.Font.Bold = true;
                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=PendingWithCandidate.xlsx");

                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }





                        //var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        //{
                        //    //VerificationRowID = item.VerificationRowID,
                        //    ClientName = item.ClientName,
                        //    SecuritasID = item.CompanyRefNo,
                        //    CandidateName = item.PersonalName,
                        //    CheckName = item.SubCheckName,
                        //    TAT = item.OutDate,
                        //    InternalTAT = item.InternalOutDate,
                        //    UniqueComponentId = item.UniqueComponentID,
                        //    CheckStatus = item.CheckStatus
                        //}).ToList();
                        //ExportToExcel(data1, "CriminalTLOrQC");
                    }
                    else
                    {
                        var data2 = data.PQVerificationInfo.Select(item => new ExportVerificationAddTLViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTM = item.AllocatedTMName,
                            Partner = item.Partner,
                            Location = item.Location,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data2, "CriminalTLOrQC");
                    }
                }
                if (Designation == "Team Member-Cri")
                {
                    var data = repoVerificationInfo.GetExportCandidateCriminalListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    var data3 = data.PQVerificationInfo.Select(item => new ExportVerificationAddTMViewModel
                    {
                        VerificationRowID = item.VerificationRowID,
                        ClientName = item.ClientName,
                        SecuritasID = item.CompanyRefNo,
                        CandidateName = item.PersonalName,
                        CheckName = item.SubCheckName,
                        Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                        TAT = item.OutDate,
                        SPOC = item.SecuritasSPOC,
                        Partner = item.Partner,
                        Location = item.Location,
                        CheckStatus = item.CheckStatus
                    }).ToList();
                    ExportToExcel(data3, "CriminalTM");
                }
                #endregion

                #region Education Excel Export

                if (Designation == "Manager-Edu")
                {
                    var data = repoVerificationInfo.GetExportCandidateEducationListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "Allocated-Mgr")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "EducationMge");
                    }
                    else
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new ExportVerificationEduMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTL = item.AllocatedTLName,
                            AllocatedTM = item.AllocatedTMName,
                            AllocatedQC = item.AllocatedQCName,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "EducationMge");
                    }
                }
                if (Designation == "Team Leader-Edu" || Designation == "Team Member-Edu-QC")
                {
                    var data = repoVerificationInfo.GetExportCandidateEducationListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "AllocationToTM" || CheckStatus == "ReAllocation")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "CriminalTLOrQC");
                    }
                    else
                    {
                        var data2 = data.PQVerificationInfo.Select(item => new ExportVerificationEduTLViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTM = item.AllocatedTMName,
                            AllocatedQC = item.AllocatedQCName,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data2, "EducationTLOrQC");
                    }
                }
                if (Designation == "Team Member-Edu")
                {
                    var data = repoVerificationInfo.GetExportCandidateEducationListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    var data3 = data.PQVerificationInfo.Select(item => new ExportVerificationTMMgrViewModel
                    {
                        VerificationRowID = item.VerificationRowID,
                        ClientName = item.ClientName,
                        SecuritasID = item.CompanyRefNo,
                        CandidateName = item.PersonalName,
                        CheckName = item.SubCheckName,
                        Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                        TAT = item.OutDate,
                        SPOC = item.SecuritasSPOC,
                        CheckStatus = item.CheckStatus
                    }).ToList();
                    ExportToExcel(data3, "CriminalTM");
                }
                #endregion

                #region Employee Excel Export

                if (Designation == "Manager-Emp")
                {
                    var data = repoVerificationInfo.GetExportCandidateEmployeeListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "Allocated-Mgr")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "EmployeeMge");
                    }
                    else
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new ExportVerificationEduMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTL = item.AllocatedTLName,
                            AllocatedTM = item.AllocatedTMName,
                            AllocatedQC = item.AllocatedQCName,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "EmployeeMge");
                    }
                }
                if (Designation == "Team Leader-Emp" || Designation == "Team Member-Emp-QC")
                {
                    var data = repoVerificationInfo.GetExportCandidateEmployeeListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "AllocationToTM" || CheckStatus == "ReAllocation")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "EmployeeMge");
                    }
                    else
                    {
                        var data2 = data.PQVerificationInfo.Select(item => new ExportVerificationEduTLViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTM = item.AllocatedTMName,
                            AllocatedQC = item.AllocatedQCName,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data2, "EmployeeTLOrQC");
                    }
                }
                if (Designation == "Team Member-Emp")
                {
                    var data = repoVerificationInfo.GetExportCandidateEmployeeListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    var data3 = data.PQVerificationInfo.Select(item => new ExportVerificationTMMgrViewModel
                    {
                        VerificationRowID = item.VerificationRowID,
                        ClientName = item.ClientName,
                        SecuritasID = item.CompanyRefNo,
                        CandidateName = item.PersonalName,
                        CheckName = item.SubCheckName,
                        Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                        TAT = item.OutDate,
                        SPOC = item.SecuritasSPOC,
                        CheckStatus = item.CheckStatus
                    }).ToList();
                    ExportToExcel(data3, "EmployeeTM");
                }
                #endregion

                #region Reference Excel Export

                if (Designation == "Manager-Ref")
                {
                    var data = repoVerificationInfo.GetExportCandidateReferenceListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "Allocated-Mgr")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "ReferenceMge");
                    }
                    else
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new ExportVerificationEduMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTL = item.AllocatedTLName,
                            AllocatedTM = item.AllocatedTMName,
                            AllocatedQC = item.AllocatedQCName,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "ReferenceMge");
                    }
                }
                if (Designation == "Team Leader-Ref" || Designation == "Team Member-Ref-QC")
                {
                    var data = repoVerificationInfo.GetExportCandidateReferenceListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "AllocationToTM" || CheckStatus == "ReAllocation")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "ReferenceTLOrQC");
                    }
                    else
                    {
                        var data2 = data.PQVerificationInfo.Select(item => new ExportVerificationEduTLViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTM = item.AllocatedTMName,
                            AllocatedQC = item.AllocatedQCName,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data2, "ReferenceTLOrQC");
                    }
                }
                if (Designation == "Team Member-Ref")
                {
                    var data = repoVerificationInfo.GetExportCandidateReferenceListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    var data3 = data.PQVerificationInfo.Select(item => new ExportVerificationTMMgrViewModel
                    {
                        VerificationRowID = item.VerificationRowID,
                        ClientName = item.ClientName,
                        SecuritasID = item.CompanyRefNo,
                        CandidateName = item.PersonalName,
                        CheckName = item.SubCheckName,
                        Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                        TAT = item.OutDate,
                        SPOC = item.SecuritasSPOC,
                        CheckStatus = item.CheckStatus
                    }).ToList();
                    ExportToExcel(data3, "ReferenceTM");
                }
                #endregion

                #region Reference Excel Export

                if (Designation == "Manager-Ide")
                {
                    var data = repoVerificationInfo.GetExportCandidateNationalIdentitieListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "Allocated-Mgr")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "NationalIdentitieMge");
                    }
                    else
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new ExportVerificationEduMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTL = item.AllocatedTLName,
                            AllocatedTM = item.AllocatedTMName,
                            AllocatedQC = item.AllocatedQCName,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "NationalIdentitieMge");
                    }
                }
                if (Designation == "Team Leader-Ide" || Designation == "Team Member-Ide-QC")
                {
                    var data = repoVerificationInfo.GetExportCandidateNationalIdentitieListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    if (CheckStatus == "AllocationToTM" || CheckStatus == "ReAllocation")
                    {
                        var data1 = data.PQVerificationInfo.Select(item => new AllocationonAddMgrViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            TAT = item.OutDate,
                            InternalTAT = item.InternalOutDate,
                            UniqueComponentId = item.UniqueComponentID,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data1, "NationalIdentitieTLOrQC");
                    }
                    else
                    {
                        var data2 = data.PQVerificationInfo.Select(item => new ExportVerificationEduTLViewModel
                        {
                            VerificationRowID = item.VerificationRowID,
                            ClientName = item.ClientName,
                            SecuritasID = item.CompanyRefNo,
                            CandidateName = item.PersonalName,
                            CheckName = item.SubCheckName,
                            Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                            TAT = item.OutDate,
                            SPOC = item.SecuritasSPOC,
                            AllocatedTM = item.AllocatedTMName,
                            AllocatedQC = item.AllocatedQCName,
                            CheckStatus = item.CheckStatus
                        }).ToList();
                        ExportToExcel(data2, "NationalIdentitieTLOrQC");
                    }
                }
                if (Designation == "Team Member-Ide")
                {
                    var data = repoVerificationInfo.GetExportCandidateNationalIdentitieListByDepartmentManager(Designation, teamMemerRowId, ClientRowID, CheckStatus, AllocatedToTL, tatdays, DCheckStatus, CaseStatus);
                    var data3 = data.PQVerificationInfo.Select(item => new ExportVerificationTMMgrViewModel
                    {
                        VerificationRowID = item.VerificationRowID,
                        ClientName = item.ClientName,
                        SecuritasID = item.CompanyRefNo,
                        CandidateName = item.PersonalName,
                        CheckName = item.SubCheckName,
                        Ageing = item.InternalOutDate != null ? (item.InternalOutDate - DateTime.Now).Value.Days : 0,
                        TAT = item.OutDate,
                        SPOC = item.SecuritasSPOC,
                        CheckStatus = item.CheckStatus
                    }).ToList();
                    ExportToExcel(data3, "NationalIdentitieTM");
                }
                #endregion
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        #endregion

        #region ********** Export To Excel **********

        public void ExportToExcel<T>(IEnumerable<T> list, string strFileName)
        {
            // Step 1 - get the data from database
            var data = list.ToList();

            // instantiate the GridView control from System.Web.UI.WebControls namespace
            // set the data source
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();

            // Clear all the content from the current response
            Response.ClearContent();
            Response.Buffer = true;

            // set the header
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            clsExportDataToExcel.WriteHtmlTable(data, Response.Output);

            Response.Flush();
            Response.End();
        }

        #endregion


        [SessionExpireFilterAttribute]
        [HttpGet]
        public ActionResult InsuffRemarks(int PersonalRowID=0, int SubCheckRowID=0,string processName="")
        {
            Insuff_FundsAllRemarks model = new Insuff_FundsAllRemarks();
            
            try
            {
                if (PersonalRowID > 0 && SubCheckRowID > 0 )
                {
                    model= repoDashboard.getAllProcess_Remark(processName, PersonalRowID,SubCheckRowID);
                    //model.CheckRowID = CheckRowid;
                    //model.PersonalRowID = PersonalRowID;
                    //model.SubCheckRowID = Convert.ToInt16(SubCheckRowID);
                    //model.CheckstatusId = checkstatusid;
                }
                else
                {
                    TempData["ErrorMsg"] = "invlid id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }


        [SessionExpireFilterAttribute]
        [HttpPost]

        public ActionResult InsuffRemarks(Insuff_FundsAllRemarks model)
        {
            try
            {
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                bool IsSuccess = false;
                if (ModelState.IsValid)
                {
                    if (model.SubCheckRowID > 0)
                    {
                        ClientCustomize cc = new ClientCustomize();
                        cc.Process_Remarks_Insert(model.PersonalRowID,model.SubCheckRowID,54,model.Remarks,"Operation-QC", teamMemerRowId);
                        ViewBag.SuccessMsg = "Record update successfully";
                        IsSuccess = true;


                        //int personalRowid = 0;
                        //int subCheckRowID = 0;

                        //short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                        //string Designation = Session["Designation"].ToString();


                    }
                }
               
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }


        [SessionExpireFilterAttribute]
        [HttpGet]
        public PartialViewResult FundsPendindRemark(int PersonalRowID = 0, int SubCheckRowID = 0,int checkstatusid=0,int CheckRowid=0, string processName = "")
        {
            Insuff_FundsAllRemarks model = new Insuff_FundsAllRemarks();

            try
            {
                if (PersonalRowID > 0 && SubCheckRowID > 0)
                {
                    string Remark = "";
                    int amount = 0;
                    DataTable dt;
                    dt = cc.GetsFundsPendingAmount(PersonalRowID, SubCheckRowID);
                    if (dt.Rows.Count > 0)
                    {
                        amount = Convert.ToInt32(dt.Rows[0]["Amount"].ToString());
                        Remark = dt.Rows[0]["Remarks"].ToString();
                    }
                    model.Remarks = "Amount: "+ amount + "\n"+ Remark;
                    //model = repoDashboard.getAllProcess_Remark(processName, PersonalRowID, SubCheckRowID);
                    model.CheckRowID = CheckRowid;
                    model.PersonalRowID = PersonalRowID;
                    model.SubCheckRowID = Convert.ToInt16(SubCheckRowID);
                    model.CheckstatusId = checkstatusid;
                }
                else
                {
                    TempData["ErrorMsg"] = "invlid id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //return View(model);
            return PartialView("~/Views/Shared/FundsPendingRemark.cshtml", model);
        }



        [SessionExpireFilterAttribute]
        [HttpPost]

        public PartialViewResult FundsPendindRemark(Insuff_FundsAllRemarks model)
        {
            try
            {
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                bool IsSuccess = false;
                if (ModelState.IsValid)
                {
                    if (model.SubCheckRowID > 0)
                    {
                        ClientCustomize cc = new ClientCustomize();
                        cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 59, model.Remarks, "Operation-QC", teamMemerRowId);
                        ViewBag.SuccessMsg = "Record update successfully";
                        IsSuccess = true;


                        //int personalRowid = 0;
                        //int subCheckRowID = 0;

                        //short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                        //string Designation = Session["Designation"].ToString();


                    }
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return PartialView("~/Views/Shared/FundsPendingRemark.cshtml", model);
        }


        [HttpGet]

        public void ExportChecksReport(string CheckStatus, string CompletedFromDate = "", string CompletedToDate = "")
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string Designation = Convert.ToString(Session["Designation"]);
            DataTable dt = new DataTable();
            string department = "";
            try
            {
                if(Designation == "Manager-Add" || Designation == "Team Leader-Add" || Designation == "Team Member-Add")
                {
                    department = "Address";
                    string CheckStatusId = "";
                    if (CheckStatus == "Allocated" && Designation == "Manager-Add")
                    {
                        CheckStatusId = "18,27,38,62,63";
                    }
                    else if (CheckStatus == "UnAllocated" && Designation == "Manager-Add")
                    {
                        CheckStatusId = "16";
                    }
                    else if (CheckStatus == "WIP" && Designation == "Manager-Add")
                    {
                        CheckStatusId = "27,62,63,64";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Manager-Add")
                    {
                        CheckStatusId = "36,37";
                    }

                    else if (CheckStatus == "Allocated" && Designation == "Team Leader-Add")
                    {
                        CheckStatusId = "20,27,62,63";
                    }

                    else if (CheckStatus == "Closed" && Designation == "Team Leader-Add")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "Unallocated" && Designation == "Team Leader-Add")
                    {
                        CheckStatusId = "18";
                    }

                   else if (CheckStatus == "Pending" && Designation == "Team Member-Add")
                    {
                        CheckStatusId = "20";
                    }
                    else if (CheckStatus == "Reverification" && Designation == "Team Member-Add")
                    {
                        CheckStatusId = "20";
                    }
                    else if (CheckStatus == "WIP" && Designation == "Team Member-Add")
                    {
                        CheckStatusId = "27,62,63";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Team Member-Add")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "InsufficentRaised" && Designation == "Team Member-Add")
                    {
                        CheckStatusId = "53";
                    }

                    else if (CheckStatus == "InsufficencyResolved" && Designation == "Team Member-Add")
                    {
                        CheckStatusId = "56";
                    }
                    else if (CheckStatus == "FundsPending" && Designation == "Team Member-Add")
                    {
                        CheckStatusId = "26";
                    }
                    else if (CheckStatus == "Rejected" && Designation == "Team Member-Add")
                    {
                        CheckStatusId = "21";
                    }
                    else if (CheckStatus == "PartnerClosed" && Designation == "Team Member-Add")
                    {
                        CheckStatusId = "38";
                    }
                    else if (CheckStatus == "PartnerRejected" && Designation == "Team Member-Add")
                    {
                        CheckStatusId = "64";
                    }
                    else if(CheckStatus== "NotDoable")
                    {
                        CheckStatusId = "72";
                        department = "NotDoable";

                    }

                    dt = cc.ExportToExcelAddressManagerNew(2, CheckStatusId, department, teamMemerRowId,CompletedFromDate,CompletedToDate);
                    //var data = repoVerificationInfo.GetExportAddressManager(2, CheckStatusId, department);                   

        //.Select(i => new { i.category_id, i.category_name })
        //var value = data.Select(i => new { i.ClientName, i.CompanyRefNo, i.Name, i.AddressType, i.address, i.AV_Pincode , i.Location, i.StateName, i.MobileNo });               
                    ExportToExcel(dt, "AddressMge");
                }

                else if (Designation == "Manager-Edu" || Designation == "Team Leader-Edu" || Designation=="Team Member-Edu" || Designation == "Team Member-Edu-QC")
                {
                    department = "Education";
                    string CheckStatusId = "";
                    if (CheckStatus == "Allocated" && Designation == "Manager-Edu")
                    {
                        CheckStatusId = "18";
                    }
                    else if (CheckStatus == "UnAllocated" && Designation == "Manager-Edu")
                    {
                        CheckStatusId = "16";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Manager-Edu")
                    {
                        CheckStatusId = "36,37";
                    }

                    else if (CheckStatus == "Allocated" && Designation == "Team Leader-Edu")
                    {
                        CheckStatusId = "20,27";
                    }

                    else if (CheckStatus == "Closed" && Designation == "Team Leader-Edu")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "Unallocated" && Designation == "Team Leader-Edu")
                    {
                        CheckStatusId = "18";
                    }

                    if (CheckStatus == "WIP" && Designation == "Team Member-Edu")
                    {
                        CheckStatusId = "20";
                    }
                    if (CheckStatus == "WIP" && Designation == "Team Member-Edu")
                    {
                        CheckStatusId = "20";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Team Member-Edu")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "InsufficentRaised" && Designation == "Team Member-Edu")
                    {
                        CheckStatusId = "53";
                    }

                    else if (CheckStatus == "InsufficencyResolved" && Designation == "Team Member-Edu")
                    {
                        CheckStatusId = "56";
                    }
                    else if (CheckStatus == "FundsPending" && Designation == "Team Member-Edu")
                    {
                        CheckStatusId = "26";
                    }
                    else if (CheckStatus == "Rejected" && Designation == "Team Member-Edu")
                    {
                        CheckStatusId = "21";
                    }

                    else if (CheckStatus == "FundsApproval" && Designation == "Team Member-Edu")
                    {
                        CheckStatusId = "61";
                    }
                    else if (CheckStatus == "Pending" && Designation == "Team Member-Edu-QC")
                    {
                        CheckStatusId = "24";
                    }
                    //var data = repoVerificationInfo.GetExportEducationManager(11, CheckStatusId, department);
                    dt = cc.ExportToExcelAddressManager(11, CheckStatusId, department, teamMemerRowId);
                    //var value = data.Select(i => new { i.ClientName, i.CandidateName, i.SPOC, i.CheckName, i.CheckStatus, i.SecuritasID,i.AllocatedTL,i.AllocatedTM,i.AllocatedQC  });               
                    ExportToExcel(dt, "Education Check Report");
                }

                else if (Designation == "Manager-Ide" || Designation == "Team Leader-Ide" || Designation == "Team Member-Ide" || Designation == "Team Member-Ide-QC")
                {
                    department = "National";
                    string CheckStatusId = "";
                     if (CheckStatus == "UnAllocated" && Designation == "Manager-Ide")
                    {
                        CheckStatusId = "16";
                    }


                    else if (CheckStatus == "Allocated" && Designation == "Manager-Ide")
                    {
                        CheckStatusId = "18";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Manager-Ide")
                    {
                        CheckStatusId = "36,37";
                    }

                    else if (CheckStatus == "Unallocated" && Designation == "Team Leader-Ide")
                    {
                        CheckStatusId = "18";
                    }

                    else if (CheckStatus == "Allocated" && Designation == "Team Leader-Ide")
                    {
                        CheckStatusId = "20,27";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Team Leader-Ide")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "Review" && Designation == "Team Leader-Ide")
                    {
                        CheckStatusId = "19";
                    }

                    else if(CheckStatus == "WIP" && Designation == "Team Member-Ide")
                    {
                        CheckStatusId = "20";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Team Member-Ide")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "InsufficentRaised" && Designation == "Team Member-Ide")
                    {
                        CheckStatusId = "53";
                    }

                    else if (CheckStatus == "InsufficencyResolved" && Designation == "Team Member-Ide")
                    {
                        CheckStatusId = "56";
                    }
                    else if (CheckStatus == "FundsPending" && Designation == "Team Member-Ide")
                    {
                        CheckStatusId = "26";
                    }
                    else if (CheckStatus == "Rejected" && Designation == "Team Member-Ide")
                    {
                        CheckStatusId = "21";
                    }

                    else if (CheckStatus == "Rejected" && Designation == "Team Member-Ide-QC")
                    {
                        CheckStatusId = "25";
                    }

                    //var data = repoVerificationInfo.GetExportNational(26, CheckStatusId, department);
                    dt = cc.ExportToExcelAddressManager(26, CheckStatusId, department, teamMemerRowId);
                    //.Select(i => new { i.category_id, i.category_name })
                    //var value = data.Select(i => new { i.ClientName, i.CompanyRefNo, i.Name, i.AddressType, i.address, i.AV_Pincode , i.Location, i.StateName, i.MobileNo });               
                    ExportToExcel(dt, "National Identity Report");
                }


                else if (Designation == "Manager-SPL" || Designation == "Team Leader-SPL" || Designation == "Team Member-SPL-QC")
                {
                    department = "Special";
                    string CheckStatusId = "";
                     if (CheckStatus == "Allocated" && Designation == "Manager-SPL")
                    {
                        CheckStatusId = "18";
                    }

                    else if (CheckStatus == "Review" && Designation == "Manager-SPL") 
                    {
                        CheckStatusId = "17";
                    }

                    else if (CheckStatus == "UnAllocated" && Designation == "Manager-SPL")
                    {
                        CheckStatusId = "16";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Manager-SPL")
                    {
                        CheckStatusId = "36,37";
                    }

                    else if (CheckStatus == "Allocated" && Designation == "Team Leader-SPL")
                    {
                        CheckStatusId = "20,27";
                    }

                    else if (CheckStatus == "Closed" && Designation == "Team Leader-SPL")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "Unallocated" && Designation == "Team Leader-SPL")
                    {
                        CheckStatusId = "18";
                    }
                    else if (CheckStatus == "Rejected" && Designation == "Team Member-SPL-QC")
                    {
                        CheckStatusId = "25";
                    }


                    //var data = repoVerificationInfo.GetExportSpecial(40, CheckStatusId, department);
                    dt = cc.ExportToExcelAddressManager(40, CheckStatusId, department, teamMemerRowId);
                    //.Select(i => new { i.category_id, i.category_name })
                    //var value = data.Select(i => new { i.ClientName, i.CompanyRefNo, i.Name, i.AddressType, i.address, i.AV_Pincode , i.Location, i.StateName, i.MobileNo });               
                    ExportToExcel(dt, "Special Check Report");
                }

                else if (Designation == "Manager-Cri" || Designation == "Team Leader-Cri")
                {
                    department = "Criminal";
                    string CheckStatusId = "";
                    if (CheckStatus == "Allocated" && Designation == "Manager-Cri")
                    {
                        CheckStatusId = "18";
                    }

                    else if (CheckStatus == "Review" && Designation == "Manager-Cri")
                    {
                        CheckStatusId = "17";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Manager-Cri")
                    {
                        CheckStatusId = "36,37";
                    }

                    else if (CheckStatus == "UnAllocated" && Designation == "Manager-Cri")
                    {
                        CheckStatusId = "16";
                    }

                    else if (CheckStatus == "Allocated" && Designation == "Team Leader-Cri")
                    {
                        CheckStatusId = "20,27";
                    }

                    else if (CheckStatus == "Closed" && Designation == "Team Leader-Cri")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "Unallocated" && Designation == "Team Leader-Cri")
                    {
                        CheckStatusId = "18";
                    }

                    //var data = repoVerificationInfo.GetExportCriminal(5, CheckStatusId, department);
                    dt = cc.ExportToExcelAddressManager(5, CheckStatusId, department, teamMemerRowId);
                    //.Select(i => new { i.category_id, i.category_name })
                    //var value = data.Select(i => new { i.ClientName, i.CompanyRefNo, i.Name, i.AddressType, i.address, i.AV_Pincode , i.Location, i.StateName, i.MobileNo });               
                    ExportToExcel(dt, "Criminal Check Report");
                }
                else if (Designation == "Manager-Emp" || Designation == "Team Leader-Emp" || Designation == "Team Member-Emp")
                {
                    department = "Emp";
                    string CheckStatusId = "";
                    if (CheckStatus == "Allocated" && Designation == "Manager-Emp")
                    {
                        CheckStatusId = "18";
                    }

                    else if (CheckStatus == "Review" && Designation == "Manager-Emp")
                    {
                        CheckStatusId = "17";
                    }

                    else if (CheckStatus == "UnAllocated" && Designation == "Manager-Emp")
                    {
                        CheckStatusId = "16";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Manager-Emp")
                    {
                        CheckStatusId = "36,37";
                    }

                    else if (CheckStatus == "Allocated" && Designation == "Team Leader-Emp")
                    {
                        CheckStatusId = "20,27";
                    }

                    else if (CheckStatus == "Closed" && Designation == "Team Leader-Emp")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "Unallocated" && Designation == "Team Leader-Emp")
                    {
                        CheckStatusId = "18";
                    }

                    else  if (CheckStatus == "WIP" && Designation == "Team Member-Emp")
                    {
                        CheckStatusId = "20";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Team Member-Emp")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "InsufficentRaised" && Designation == "Team Member-Emp")
                    {
                        CheckStatusId = "53";
                    }

                    else if (CheckStatus == "InsufficencyResolved" && Designation == "Team Member-Emp")
                    {
                        CheckStatusId = "56";
                    }
                    else if (CheckStatus == "FundsPending" && Designation == "Team Member-Emp")
                    {
                        CheckStatusId = "26";
                    }
                    else if (CheckStatus == "Rejected" && Designation == "Team Member-Emp")
                    {
                        CheckStatusId = "21";
                    }

                    //var data = repoVerificationInfo.GetExportEmployeemant(12, CheckStatusId, department);
                    dt = cc.ExportToExcelAddressManager(12, CheckStatusId, department, teamMemerRowId);
                    //.Select(i => new { i.category_id, i.category_name })
                    //var value = data.Select(i => new { i.ClientName, i.CompanyRefNo, i.Name, i.AddressType, i.address, i.AV_Pincode , i.Location, i.StateName, i.MobileNo });               
                    ExportToExcel(dt, "Employeement Check Report");
                }
                

                else if (Designation == "Manager-Ref" || Designation == "Team Leader-Ref")
                {
                    department = "Reference";
                    string CheckStatusId = "";
                    if (CheckStatus == "Allocated" && Designation == "Manager-Ref")
                    {
                        CheckStatusId = "18";
                    }

                    else if (CheckStatus == "Review" && Designation == "Manager-Ref")
                    {
                        CheckStatusId = "17";
                    }

                    else if (CheckStatus == "UnAllocated" && Designation == "Manager-Ref")
                    {
                        CheckStatusId = "16";
                    }
                    else if (CheckStatus == "Closed" && Designation == "Manager-Ref")
                    {
                        CheckStatusId = "36,37";
                    }

                    else if (CheckStatus == "Allocated" && Designation == "Team Leader-Ref")
                    {
                        CheckStatusId = "20,27";
                    }

                    else if (CheckStatus == "Closed" && Designation == "Team Leader-Ref")
                    {
                        CheckStatusId = "24";
                    }

                    else if (CheckStatus == "Unallocated" && Designation == "Team Leader-Ref")
                    {
                        CheckStatusId = "18";
                    }

                    //var data = repoVerificationInfo.GetExportReference(30, CheckStatusId, department);
                    dt = cc.ExportToExcelAddressManager(30, CheckStatusId, department, teamMemerRowId);
                    //.Select(i => new { i.category_id, i.category_name })
                    //var value = data.Select(i => new { i.ClientName, i.CompanyRefNo, i.Name, i.AddressType, i.address, i.AV_Pincode , i.Location, i.StateName, i.MobileNo });               
                    ExportToExcel(dt, "Employeement Check Report");
                }
                else if(Designation == "Team Member-SPL")
                {
                    department = "Special";
                    string statusid = "";
                    if (CheckStatus == "WIP")
                    {
                        statusid = "20";
                    }
                    else if (CheckStatus == "Closed")
                    {
                        statusid = "24";
                    }

                    else if (CheckStatus == "InsufficentRaised")
                    {
                        statusid = "53";
                    }

                    else if (CheckStatus == "InsufficencyResolved")
                    {
                        statusid = "56";
                    }
                    else if (CheckStatus == "FundsPending")
                    {
                        statusid = "26";
                    }
                    else if (CheckStatus == "Rejected")
                    {
                        statusid = "21";
                    }


                    //var data = repoVerificationInfo.GetExportSpecial(40, statusid, department);
                    dt = cc.ExportToExcelAddressManager(40, statusid, department, teamMemerRowId);
                    //.Select(i => new { i.category_id, i.category_name })
                    //var value = data.Select(i => new { i.ClientName,i.CompanyRefNo, i.Name, i.Gender, i.DOB, i.FatherName, i.address,i.AV_Pincode,i.SpecialType });               
                    ExportToExcel(dt, "SpecialCheckReport");
                }

                else if (Designation == "Team Member-Cri")
                {
                    department = "Criminal";
                    string statusid = "";
                    if (CheckStatus == "WIP")
                    {
                        statusid = "20";
                    }
                    else if (CheckStatus == "Closed")
                    {
                        statusid = "24";
                    }

                    else if (CheckStatus == "InsufficentRaised")
                    {
                        statusid = "53";
                    }

                    else if (CheckStatus == "InsufficencyResolved")
                    {
                        statusid = "56";
                    }
                    else if (CheckStatus == "FundsPending")
                    {
                        statusid = "26";
                    }
                    else if (CheckStatus == "Rejected")
                    {
                        statusid = "21";
                    }
                    else if (CheckStatus == "FundsApproval")
                    {
                        statusid = "61";
                    }



                    //var data = repoVerificationInfo.GetExportCriminal(5, statusid, department);
                    dt = cc.ExportToExcelAddressManager(5, statusid, department, teamMemerRowId);
                    //.Select(i => new { i.category_id, i.category_name })
                    //var value = data.Select(i => new { i.ClientName, i.CompanyRefNo, i.Name, i.AddressType, i.address, i.AV_Pincode , i.Location, i.StateName, i.MobileNo });               
                    ExportToExcel(dt, "Criminal Check Report");
                }
         
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public void ExportAllDeptMIS(string Department)
        {
            try
            {
                DataTable DT = new DataTable();
                short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                if(Department=="Address")
                {
                    DT=cc.ExportAllDeptMIS("MIS_addressDepartment");
                    ExportToExcel(DT, "AddressMISReport");
                }
                else if (Department == "Education")
                {
                    DT = cc.ExportAllDeptMIS("MIS_EducationDepartment");
                    ExportToExcel(DT, "EducationMISReport");
                }
                else if (Department == "Employment")
                {
                    DT = cc.ExportAllDeptMIS("MIS_EmploymentDepartment");

                    ExportToExcel(DT, "EmployeementMISReport","");
                }

                else if (Department == "Reference")
                {
                    DT = cc.ExportAllDeptMIS("MIS_ReferenceDepartment");
                    ExportToExcel(DT, "ReferenceMISReport");
                }

                else if (Department == "Criminal")
                {
                    DT = cc.ExportAllDeptMIS("MIS_CriminalDepartment");
                    ExportToExcel(DT, "CriminalMISReport");
                }

                else if (Department == "National Identity")
                {
                    DT = cc.ExportAllDeptMIS("MIS_NationalIdentityDepartment");
                    ExportToExcel(DT, "NationalIdentityMISReport");
                }

                else if (Department == "Special Check")
                {
                    DT = cc.ExportAllDeptMIS("MIS_SpecialDepartment");
                    ExportToExcel(DT, "SpecialMISReport");
                }

                else if (Department == "Report QC")
                {
                    DT = cc.ExportAllDeptMIS("MIS_RWQC_procedure");
                    ExportToExcel(DT, "RWQCMISReport");
                }




            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }



        [HttpGet]
        [SessionExpireFilterAttribute]
        public void E2eTrackerForCS_clientwise()
        {
            try
            {
                string teamMemerRowId = Session["TeamMemberRowID"].ToString();
                string department = Session["TeamMemberRowID"].ToString();
                
                DataTable DT = new DataTable();         
                DT = cc.e2eTrackerForCS_clientwise("3", teamMemerRowId, department);
                ExportToExcel(DT, "E2ETrackerforCS");
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public IEnumerable<T> GetEntities<T>(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            List<T> returnValue = new List<T>();
            List<string> typeProperties = new List<string>();

            T typeInstance = Activator.CreateInstance<T>();

            foreach (DataColumn column in dt.Columns)
            {
                var prop = typeInstance.GetType().GetProperty(column.ColumnName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                if (prop != null)
                {
                    typeProperties.Add(column.ColumnName);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                T entity = Activator.CreateInstance<T>();

                foreach (var propertyName in typeProperties)
                {

                    if (row[propertyName] != DBNull.Value)
                    {
                        string str = row[propertyName].GetType().FullName;

                        if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.String))
                        {
                            object Val = row[propertyName].ToString();
                            entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                        }
                        else if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.Guid))
                        {
                            object Val = Guid.Parse(row[propertyName].ToString());
                            entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                        }
                        else
                        {
                            entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, row[propertyName], BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                        }
                    }
                    else
                    {
                        entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, null, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                    }
                }

                returnValue.Add(entity);
            }

            return returnValue.AsEnumerable();
        }




        public void WriteHtmlTable<T>(IEnumerable<T> data, TextWriter output)
        {
            //Writes markup characters and text to an ASP.NET server control output stream. This class provides formatting capabilities that ASP.NET server controls use when rendering markup to clients.
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {

                    //  Create a form to contain the List
                    Table table = new Table();
                    TableRow row = new TableRow();
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                    foreach (PropertyDescriptor prop in props)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = prop.Name;
                        hcell.BackColor = System.Drawing.Color.Yellow;
                        row.Cells.Add(hcell);
                    }

                    table.Rows.Add(row);

                    //  add each of the data item to the table
                    foreach (T item in data)
                    {
                        row = new TableRow();
                        foreach (PropertyDescriptor prop in props)
                        {
                            TableCell cell = new TableCell();
                            cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                            row.Cells.Add(cell);
                        }
                        table.Rows.Add(row);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    output.Write(sw.ToString());
                }
            }

        }

        public void ExportToExcel(DataTable dt, string strFileName)
        {
            if (dt.Rows.Count > 0)
            {
                Response.Clear();
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();
                dgGrid.GridLines = GridLines.None;
                dgGrid.BorderStyle = BorderStyle.None;


                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName + ".xls");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());             
               


                Response.Write(tw.ToString());
                Response.End();
            }
        }


        public void ExportToExcel (DataTable dt, string strFileName, string formatting)
        {
            var data = GetEntities<MISEmployment>(dt);

            Response.ClearContent();
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName + ".xls");
            WriteHtmlTable(data, Response.Output);
            Response.End();
        }


        [HttpGet]
        [SessionExpireFilterAttribute]
        public ActionResult UplodAddressCheckDocsAndComplete(short scid = 0, int pe = 0, string cstatus = "")
        {
            VerifiedUploadAddressDocViewModel model = new VerifiedUploadAddressDocViewModel();
            try
            {
                if (scid > 0 && pe > 0)
                {
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult UplodAddressCheckDocsAndComplete(VerifiedUploadAddressDocViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    model.Remarks = "Uploaded By TM";
                    if (model.UploadSignatureDoc != null && model.UploadSignatureDoc.ContentLength > 0)
                    {
                        model.DocumentType = "Signature";
                        model.DocumentUploadFrom = "Verified";
                        model.Status = 1;
                        model.UploadDate = DateTime.Now;

                        string existFile = repoPQVerifiedUploadDoc.ExistFileByPersonalIdSubCheckIdDocType(model.PersonalRowID, model.SubCheckRowID, model.DocumentType);
                        if (!string.IsNullOrEmpty(existFile))
                        {
                            string path = Server.MapPath(FldPathDoc + existFile);
                            FileInfo file = new FileInfo(path);
                            if (file.Exists)//check file exsit or not
                            {
                                file.Delete();
                            }
                        }

                        string ext = Path.GetExtension(model.UploadSignatureDoc.FileName).ToLower();
                        cc.UpdateImageArtifact(model.PersonalRowID, model.SubCheckRowID);
                        proImg = (model.SubCheckRowID + "_" + model.PersonalRowID + "_" + model.UploadDate.Value.ToString("ddmmyyyy") + "_" + model.DocumentType).Replace("'", "_").Replace("&", "_").Replace(":", "_").Replace("-", "_").Replace(" ", "_") + ext;
                        model.UploadSignatureDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = Path.GetFileName(proImg);
                        model.FilePath = FldPathDoc;
                        repoPQVerifiedUploadDoc.VerifiedAddressUploadDoc(model);
                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Address Verification information Upload Doc ");
                        #endregion
                    }
                    if (model.UploadIDProofDoc != null && model.UploadIDProofDoc.ContentLength > 0)
                    {
                        model.DocumentType = "IDProof";
                        model.DocumentUploadFrom = "Verified";
                        model.Status = 1;
                        model.UploadDate = DateTime.Now;

                        string existFile = repoPQVerifiedUploadDoc.ExistFileByPersonalIdSubCheckIdDocType(model.PersonalRowID, model.SubCheckRowID, model.DocumentType);
                        if (!string.IsNullOrEmpty(existFile))
                        {
                            string path = Server.MapPath(FldPathDoc + existFile);
                            FileInfo file = new FileInfo(path);
                            if (file.Exists)//check file exsit or not
                            {
                                file.Delete();
                            }
                        }

                        string ext = Path.GetExtension(model.UploadIDProofDoc.FileName).ToLower();
                        proImg = (model.SubCheckRowID + "_" + model.PersonalRowID + "_" + model.UploadDate.Value.ToString("ddmmyyyy") + "_" + model.DocumentType).Replace("'", "_").Replace("&", "_").Replace(":", "_").Replace("-", "_").Replace(" ", "_") + ext;
                        model.UploadIDProofDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = Path.GetFileName(proImg);
                        model.FilePath = FldPathDoc;
                        repoPQVerifiedUploadDoc.VerifiedAddressUploadDoc(model);
                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Address Verification information Upload Doc ");
                        #endregion
                    }
                    if (model.UploadHousephotoDoc != null && model.UploadHousephotoDoc.ContentLength > 0)
                    {
                        model.DocumentType = "Housephoto";
                        model.DocumentUploadFrom = "Verified";
                        model.Status = 1;
                        model.UploadDate = DateTime.Now;

                        string existFile = repoPQVerifiedUploadDoc.ExistFileByPersonalIdSubCheckIdDocType(model.PersonalRowID, model.SubCheckRowID, model.DocumentType);
                        if (!string.IsNullOrEmpty(existFile))
                        {
                            string path = Server.MapPath(FldPathDoc + existFile);
                            FileInfo file = new FileInfo(path);
                            if (file.Exists)//check file exsit or not
                            {
                                file.Delete();
                            }
                        }

                        string ext = Path.GetExtension(model.UploadHousephotoDoc.FileName).ToLower();
                        proImg = (model.SubCheckRowID + "_" + model.PersonalRowID + "_" + model.UploadDate.Value.ToString("ddmmyyyy") + "_" + model.DocumentType).Replace("'", "_").Replace("&", "_").Replace(":", "_").Replace("-", "_").Replace(" ", "_") + ext;
                        model.UploadHousephotoDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = Path.GetFileName(proImg);
                        model.FilePath = FldPathDoc;
                        repoPQVerifiedUploadDoc.VerifiedAddressUploadDoc(model);
                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Address Verification information Upload Doc ");
                        #endregion
                    }
                    if (model.UploadLandmarkPhotoDoc != null && model.UploadLandmarkPhotoDoc.ContentLength > 0)
                    {
                        model.DocumentType = "LandmarkPhoto";
                        model.DocumentUploadFrom = "Verified";
                        model.Status = 1;
                        model.UploadDate = DateTime.Now;

                        string existFile = repoPQVerifiedUploadDoc.ExistFileByPersonalIdSubCheckIdDocType(model.PersonalRowID, model.SubCheckRowID, model.DocumentType);
                        if (!string.IsNullOrEmpty(existFile))
                        {
                            string path = Server.MapPath(FldPathDoc + existFile);
                            FileInfo file = new FileInfo(path);
                            if (file.Exists)//check file exsit or not
                            {
                                file.Delete();
                            }
                        }

                        string ext = Path.GetExtension(model.UploadLandmarkPhotoDoc.FileName).ToLower();
                        proImg = (model.SubCheckRowID + "_" + model.PersonalRowID + "_" + model.UploadDate.Value.ToString("ddmmyyyy") + "_" + model.DocumentType).Replace("'", "_").Replace("&", "_").Replace(":", "_").Replace("-", "_").Replace(" ", "_") + ext;
                        model.UploadLandmarkPhotoDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        model.FileName = Path.GetFileName(proImg);
                        model.FilePath = FldPathDoc;
                        repoPQVerifiedUploadDoc.VerifiedAddressUploadDoc(model);
                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Address Verification information Upload Doc ");
                        #endregion
                    }
                    if (model.UploadLocation != null && model.UploadLocation.ContentLength > 0)
                    {
                        model.DocumentType = "LocationPhoto";
                        model.DocumentUploadFrom = "Verified";
                        model.Status = 1;
                        model.UploadDate = DateTime.Now;

                        string existFile = repoPQVerifiedUploadDoc.ExistFileByPersonalIdSubCheckIdDocType(model.PersonalRowID, model.SubCheckRowID, model.DocumentType);
                        if (!string.IsNullOrEmpty(existFile))
                        {
                            string path = Server.MapPath(FldPathDoc + existFile);
                            FileInfo file = new FileInfo(path);
                            if (file.Exists)//check file exsit or not
                            {
                                file.Delete();
                            }
                        }

                        string ext = Path.GetExtension(model.UploadLocation.FileName).ToLower();
                        proImg = (model.SubCheckRowID + "_" + model.PersonalRowID + "_" + model.UploadDate.Value.ToString("ddmmyyyy") + "_" + model.DocumentType).Replace("'", "_").Replace("&", "_").Replace(":", "_").Replace("-", "_").Replace(" ", "_") + ".Jpeg" ;
                        string  ImagefileName = (model.SubCheckRowID + "_" + model.PersonalRowID + "_" + model.UploadDate.Value.ToString("ddmmyyyy") + "_" + model.DocumentType).Replace("'", "_").Replace("&", "_").Replace(":", "_").Replace("-", "_").Replace(" ", "_") ;
                        string ToPath = "";
                        //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MobileImageUpload"].ToString()))
                        //{
                        //    ToPath = System.Configuration.ConfigurationManager.AppSettings["MobileImageUpload"].ToString();
                        //}

                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["mailpath"].ToString()))
                        {
                            ToPath = System.Configuration.ConfigurationManager.AppSettings["mailpath"].ToString();
                        }


                        var MobileUploadDocs = repoPartnerVerification.GetMobileVerifiedUploadDocs(model.PersonalRowID,model.SubCheckRowID);
                        if (MobileUploadDocs!="")
                        {
                            //string SourcePath = ConfigurationManager.AppSettings["DropboxPath"].ToString();
                            //string DestinationPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();


                            try
                            {


                                model.UploadLocation.SaveAs(Server.MapPath(FldPathDoc + proImg));

                                writeOnImage(MobileUploadDocs, ToPath + proImg, FldPathDoc + proImg, FldPathDoc+ImagefileName+"1"  +".Jpeg", model);


                            }
                            catch(Exception Ex)
                            {

                            }




                        }
                        if (!string.IsNullOrEmpty(FldPathDoc + ImagefileName))
                        {
                            string path = FldPathDoc + ImagefileName + ".Jpeg";
                            FileInfo file = new FileInfo(Server.MapPath(path));
                            if (file.Exists)//check file exsit or not
                            {

                                file.Delete();
                            }
                        }

                        //model.UploadLocation.SaveAs(Server.MapPath(FldPathDoc + proImg));

                        model.FileName = Path.GetFileName(ImagefileName + "1" + ".Jpeg");
                        model.FilePath = FldPathDoc;
                        repoPQVerifiedUploadDoc.VerifiedAddressUploadDoc(model);
                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Address Verification information Upload Doc ");
                        #endregion
                    }

                    ModelState.Clear();
                    ViewBag.SuccessMsg = "Record Added Successfully!";
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errorList = query.ToList();
                    string strError = string.Empty;
                    foreach (string str in errorList)
                    {
                        strError += str + "\n";
                    }
                    ModelState.AddModelError("", strError);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }


 public void writeOnImage(string point, string fromimagePath, string toImagePath, string filename, VerifiedUploadAddressDocViewModel model)
        {
            PointF firstLocation = new PointF(200f, 10f);
            PointF secondLocation = new PointF(500f, 30f);

            //string imageFilePath = @"path\picture.bmp";
            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(fromimagePath );//load the image file
            if (point != "00.000")
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (System.Drawing.Font arialFont = new System.Drawing.Font("Arial", 12))
                    {
                        //graphics.DrawImage(bitmap, 0, 0, width, height);
                        graphics.DrawString("GEO Coordinate:-"+point, arialFont, Brushes.Red, firstLocation);
                        //graphics.DrawString(point, new System.Drawing.Font("Arial", 12, FontStyle.Bold), SystemBrushes.WindowText, new Point(100, 250));

                    }
                }
            }
            //string outputFileName = "Test";
            using (MemoryStream memory = new MemoryStream())
            {

                using (FileStream fs = new FileStream( Server.MapPath( filename), FileMode.Create, FileAccess.ReadWrite))
                {
                    bitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                    bitmap.Dispose();
                }
            }

            // bitmap.Save("/Documents/CandidateDocuments/ProvidedDocuments/" + filename, ImageFormat.Jpeg); // hear  i m getting error 
        }

        public ViewResult MIS_MIS()
        {
            BindClientList();
            return View();
        }

        
        public ViewResult Generate_MIS(string NM="", string DateFrom="",string DateTo="",int clid= 0)
        {
            try
            {   
                DataTable DT = new DataTable();
                DT = cc.MIS_All(NM, DateFrom, DateTo, clid);
                if (DT.Rows.Count > 0)
                {
                    ExportToExcel(DT, "All_MIS");
                    BindClientList();
                }
                else
                    BindClientList();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //return View("MIS_MIS");
            //return View(model);
            
            return View("~/Views/Verification/MIS_MIS.cshtml");
        }

        public PartialViewResult E2ETrackerClientWise(int ClientRowId=0,string fromdate="",string Todate="")
        {
            try
            {
                if(ClientRowId==0)
                {
                    BindClientList();
                }
                else
                {
                    BindClientList();
                    DataTable DT = new DataTable();
                    DT = cc.e2eTrackerForCS_clientwise(ClientRowId.ToString(), "0", "", fromdate, Todate);
                    ExportToExcel(DT, "E2ETrackerforCS");
                }  
                

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //return View(model);
            return PartialView("~/Views/Shared/E2ETrackerClientWise.cshtml");
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public void ExportVerificationDetails(short ClientRowID = 0, string Search = "", string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "", string ReAllocation = "", int CheckStatusId = 0, string CompletedFromDate = "", string CompletedToDate = "")
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string Designation = Session["Designation"].ToString();
            string department = "";
            string statusid = "";
            if (CheckStatus == "Allocated" && Designation.Contains("Manager"))
            {
                if (Designation == "Manager-Add")
                    statusid = "18,27,38,62,63";
                else
                    statusid = "18";
            }
            else if (CheckStatus == "Review" && Designation.Contains("Manager"))
                statusid = "17";
            else if (CheckStatus == "WIP" && Designation == "Manager-Add")
                statusid = "27,62,63,64";

            else if (CheckStatus == "Closed" && Designation.Contains("Manager"))
                statusid = "36,37";
            else
                statusid = CheckStatusId.ToString();

            if (Designation.Contains("Add"))
                department = "Address";
            if (Designation.Contains("Emp"))
                department = "Employment";
            if (Designation.Contains("Edu"))
                department = "Education";
            if (Designation.Contains("Ref"))
                department = "Reference";
            if (Designation.Contains("Ide"))
                department = "National";
            if (Designation.Contains("Cri"))
                department = "Criminal";
            if (Designation.Contains("Spl"))
                department = "Special";

            List<AllocationonAddMgrViewModel> lmd = new List<AllocationonAddMgrViewModel>();
            DataTable dt = new DataTable();
            dt = cc.getDashboardOperationTLDetail(department, teamMemerRowId, statusid, Search, CompletedFromDate, CompletedToDate);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new AllocationonAddMgrViewModel
                    {
                        //VerificationRowID = Convert.ToInt32(dr["AddressRowID"].ToString()),
                        ClientName = Convert.ToString(dr["ClientName"]),
                        SecuritasID = Convert.ToString(dr["Securitas ID"]),
                        CandidateName = Convert.ToString(dr["Candidate Name"]),
                        CheckName = Convert.ToString(dr["SubCheckName"]),
                        TAT = Convert.ToDateTime(dr["OutDate"]),
                        InternalTAT = Convert.ToDateTime(dr["InternalOutDate"]),
                        UniqueComponentId = Convert.ToString(dr["UniqueComponentID"]),
                        CheckStatus = Convert.ToString(dr["CheckStatus"])

                    });

                }

            }

            ExportToExcel(lmd, "AddressMge");

        }
    


    public PartialViewResult ReminderMail(int PersonalRowID=0, int SubCheckRowID=0,string ReminderStatus="",int CheckStatusId=0,string UniqueCompId="",string VerificationId="",string GroupId="",string CandName="",string CandCheckRowID="",string OrgnName="",string mailStatus="")
        {
            ReminderMail model = new ReminderMail();
            try
            {
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                string AutoEmail = "";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["AutoEmailFrom"].ToString()))
                {
                    AutoEmail = ConfigurationManager.AppSettings["AutoEmailFrom"].ToString();
                }
                //bool IsSuccess = false;
                if(mailStatus=="Bounce")
                {
                    ReminderStatus = "Bounce";
                }
                if (ModelState.IsValid)
                {
                    if (SubCheckRowID > 0)
                    {
                        string TMEmail = "";
                        ClientCustomize cc = new ClientCustomize();
                        model = repoDashboard.get_AutoFindReminderEmailer(PersonalRowID, SubCheckRowID, ReminderStatus);
                        if (model.Body != null)
                        {
                            if (mailStatus != "Bounce")
                            {
                                string Follow_Body = "";
                                string MSentTime = "";

                                MSentTime = model.Creation_Date.ToString("dd MMMM yyyy hh:mm tt");
                                model.Body = Follow_Body + "<br></br><br></br><div>From:" + AutoEmail + "</div><div>Sent:" + MSentTime + "</div><div>To:" + model.Tomail + "</div><div>Subject:" + model.subject + "</div><br />" + model.Body;
                            }
                        }
                        model.ReminderStatus = ReminderStatus;
                        model.CheckStatusId = CheckStatusId;
                        model.UniqueComponentId = UniqueCompId;
                        model.CandCheckRowID = CandCheckRowID;
                        if (string.IsNullOrEmpty(model.subject))
                        {
                            model.subject = "Employment Verification for " + CandName + "_" + OrgnName + " - (" + CandCheckRowID + ")";
                        }

                        TMEmail = Session["TeamMemberEmail"].ToString() + ";swarna.rasmi@securitas-india.com";
                        model.ccmail = TMEmail;
                        BindDocumentList(PersonalRowID, Convert.ToInt16(SubCheckRowID));
                    }
                    if (GroupId != "")
                    {
                        ClientCustomize cc = new ClientCustomize();
                        model = repoDashboard.get_AutoFindReminderEmailer(PersonalRowID, SubCheckRowID, ReminderStatus);
                        if (model.Body != null)
                        {
                            string Follow_Body = "";
                            string MSentTime = "";
                            MSentTime = model.Creation_Date.ToString("dd MMMM yyyy hh:mm tt");
                            model.Body = Follow_Body + "<br></br><br></br><div>From:" + AutoEmail + "</div><div>Sent:" + MSentTime + "</div><div>To:" + model.Tomail + "</div><div>Subject:" + model.subject + "</div><br />" + model.Body;
                        }
                        model.CheckStatusId = CheckStatusId;
                        model.GroupId = GroupId;
                        model.ReminderStatus = ReminderStatus;
                    }
                }            

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return PartialView("~/Views/Shared/ReminderMail.cshtml", model);
        }


        [SessionExpireFilterAttribute]
        [HttpPost, ValidateInput(false)]
        public PartialViewResult ReminderMail(ReminderMail model)
        {
            try
            {
                //if()
                string AutoEmail = "";
                string MailPath = "";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["AutoEmailFrom"].ToString()))
                {
                    AutoEmail = ConfigurationManager.AppSettings["AutoEmailFrom"].ToString();
                }
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ManualEmailPath"].ToString()))
                {
                    MailPath = ConfigurationManager.AppSettings["ManualEmailPath"].ToString();
                }
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                bool result = false;
                bool IsSuccess = false;
                string sendOrRec = "";
                string status = "";
                string From = string.Empty;
                string DisplayName = string.Empty;
                int checkStatusid = 0;
                //AutoEmail = "Ankit.pathak@securitas-india.com";
                clsSendMail mail = new clsSendMail();
                var signature = mail.GetCompanyMailSignature();
                if (signature != null && signature.GeneralEmail != null && signature.GeneralEmail.Trim().Length > 0)
                {
                    From = signature.GeneralEmail;
                    DisplayName = signature.CompanyName.Trim();
                }
                if (ModelState.IsValid)
                {
                    if (model.SubCheckRowID > 0)
                    {
                        DataTable dt1 = new DataTable();
                        ClientCustomize cc = new ClientCustomize();
                        string filename = "";

                        // Send Attachment/////

                        if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                        {
                            //provideddocuments

                            // MailPath = "D:\\Attachment\\Mails\\";                           
                            filename = "ManualEmail_" + model.PersonalRowID+model.SubCheckRowID + Path.GetExtension(model.UploadDoc.FileName);
                            model.UploadDoc.SaveAs(Server.MapPath(MailPath + filename));
                            //cc.autouploadDoc(model.PersonalRowID, Convert.ToInt32(model.SubCheckRowID), filename, MailPath);                          
                        }
                        //else
                        //{
                        //    ViewBag.ErrorMsg = "please select upload doc!";

                        //}
                        /////////

                        int MAutoId = 0;
                        string MToEmail = "";
                        string MSentTime = "";
                        string MBody = "";

                        DataTable dt = new DataTable();

                        if (model.ReminderStatus == "Auto Revert mail in process" || model.ReminderStatus == "Error in Mail Sending")
                        {
                            sendOrRec = "Initail Mail send";
                            status = "13";
                            checkStatusid = 88;
                            model.Follow_Body = model.Body;
                            string AutoId= cc.insertEmailAutoVerification(model.Body,model.PersonalRowID,model.SubCheckRowID,model.subject,101,DateTime.Now,13,model.UniqueComponentId,"",2, model.CandCheckRowID);
                            MAutoId = int.Parse (AutoId);
                            cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, model.ccmail, model.subject, status, "", "101", filename);
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);
                            ViewBag.SuccessMsg = "Mail Sent Successfully!";
                            // cc.insertEmailAutoVerification_Detail(int.Parse(AutoID),model.Body,"Send",model.Tomail,"",model.subject,"13","","100");
                            //cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Automation", 100);
                            //cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, "", model.subject, status, "", "100");


                        }

                        if (model.ReminderStatus == "5")
                        {
                            //dt1 = cc.getManualAutomationRemDetail(model.PersonalRowID, model.SubCheckRowID, "Bounce");
                            sendOrRec = "Initail Mail send";
                            status = "13";
                            checkStatusid = 88;
                            model.Follow_Body = model.Body;
                            //cc.UpdateMailStatus(model.PersonalRowID, model.SubCheckRowID, 0, 1);
                            cc.UpdateMailStatus(model.PersonalRowID, model.SubCheckRowID, 6, 1);
                            //cc.UpdateBounceMailDetail(model.PersonalRowID, model.SubCheckRowID);
                            string AutoId = cc.insertEmailAutoVerification(model.Body, model.PersonalRowID, model.SubCheckRowID, model.subject, 101, DateTime.Now, 13, model.UniqueComponentId, "", 2, model.CandCheckRowID);
                            MAutoId = int.Parse(AutoId);
                            cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, model.ccmail, model.subject, status, "", "101", filename);
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);

                            ViewBag.SuccessMsg = "Mail Sent Successfully!";


                        }

                        if (model.ReminderStatus== "Initial Mail Send")
                        {
                            sendOrRec = "Send REM1";
                            status = "8";
                            checkStatusid = 83;
                            dt1 = cc.getManualAutomationRemDetail(model.PersonalRowID, model.SubCheckRowID, model.ReminderStatus);
                            if (dt1.Rows.Count > 0)
                            {
                                MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                                MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                                MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                                //dt = cc.getAlertMailingHTML("Employment Check Follow Up 1");
                                model.Follow_Body = model.Body;
                                cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, model.ccmail, model.subject, status, "", "101", filename);
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);
                                ViewBag.SuccessMsg = "Mail Sent Successfully!";
                                //if (dt.Rows.Count > 0)
                                //{
                                //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                                //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + model.Body;
                                //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                                //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                                //}


                            }


                        }
                        else if (model.ReminderStatus == "Sent Reminder1")
                        {
                            sendOrRec = "Send REM2";
                            status = "9";
                            checkStatusid = 84;
                            dt1 = cc.getManualAutomationRemDetail(model.PersonalRowID, model.SubCheckRowID, model.ReminderStatus);
                            if (dt1.Rows.Count > 0)
                            {
                                MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                                MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                                MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                                MBody = model.Body;
                                model.Follow_Body = model.Body;
                                cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, model.ccmail, model.subject, status, "", "101", filename);
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);
                                ViewBag.SuccessMsg = "Mail Sent Successfully!";
                                //dt = cc.getAlertMailingHTML("Employment Check Follow Up 2");
                                //if (dt.Rows.Count > 0)
                                //{
                                //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                                //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + MBody;
                                //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                                //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                                //}


                            }

                        }
                        else if (model.ReminderStatus == "Sent Reminder2")
                        {
                            sendOrRec = "Send REM3";
                            status = "10";
                            checkStatusid = 85;
                            dt1 = cc.getManualAutomationRemDetail(model.PersonalRowID, model.SubCheckRowID, model.ReminderStatus);
                            if (dt1.Rows.Count > 0)
                            {
                                MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                                MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                                MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                                MBody = model.Body;
                                model.Follow_Body = model.Body;
                                cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, model.ccmail, model.subject, status, "", "101", filename);
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);
                                ViewBag.SuccessMsg = "Mail Sent Successfully!";
                                //dt = cc.getAlertMailingHTML("Employment Check Follow Up 3");
                                //if (dt.Rows.Count > 0)
                                //{
                                //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                                //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + MBody;
                                //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                                //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                                //}


                            }

                        }

                        else if (model.ReminderStatus == "Sent Reminder3")
                        {
                            sendOrRec = "Send REM4";
                            status = "11";
                            checkStatusid = 86;
                            dt1 = cc.getManualAutomationRemDetail(model.PersonalRowID, model.SubCheckRowID, model.ReminderStatus);
                            if (dt1.Rows.Count > 0)
                            {
                                MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                                MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                                MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                                MBody = model.Body;
                                model.Follow_Body = model.Body;
                                cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, model.ccmail, model.subject, status, "", "101", filename);
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);
                                ViewBag.SuccessMsg = "Mail Sent Successfully!";
                                //dt = cc.getAlertMailingHTML("Employment Check Follow Up 4");
                                //if (dt.Rows.Count > 0)
                                //{
                                //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                                //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + MBody;
                                //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                                //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                                //}


                            }

                        }
                        else if (model.ReminderStatus == "Sent Reminder4")
                        {
                            sendOrRec = "Send REM5";
                            status = "12";
                            checkStatusid = 87;
                            dt1 = cc.getManualAutomationRemDetail(model.PersonalRowID, model.SubCheckRowID, model.ReminderStatus);
                            if (dt1.Rows.Count > 0)
                            {
                                MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                                MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                                MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                                MBody = model.Body;
                                model.Follow_Body = model.Body;
                                cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, model.ccmail, model.subject, status, "", "101", filename);
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);
                                ViewBag.SuccessMsg = "Mail Sent Successfully!";
                                //dt = cc.getAlertMailingHTML("Employment Check Follow Up 5");
                                //if (dt.Rows.Count > 0)
                                //{
                                //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                                //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + MBody;
                                //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                                //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                                //}


                            }

                        }

                        else if (model.ReminderStatus == "Sent Reminder5")
                        {
                            sendOrRec = "Send Greater REM5";
                            status = "14";
                            checkStatusid = 89;
                            dt1 = cc.getManualAutomationRemDetail(model.PersonalRowID, model.SubCheckRowID, model.ReminderStatus);
                            if (dt1.Rows.Count > 0)
                            {
                                MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                                MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                                MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                                MBody = model.Body;
                                model.Follow_Body = model.Body;
                                cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, model.ccmail, model.subject, status, "", "101", filename);
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);
                                ViewBag.SuccessMsg = "Mail Sent Successfully!";
                                //dt = cc.getAlertMailingHTML("Employment Check Follow Up 5");
                                //if (dt.Rows.Count > 0)
                                //{
                                //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                                //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + MBody;
                                //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                                //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                                //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                                //}


                            }

                        }

                        if (model.ReminderStatus == "Revert Mail Rejected To OPTM" || model.ReminderStatus == "L2- Insuff Cleared" || model.ReminderStatus== "Funds Request Closed")
                        {
                            sendOrRec = "Initail Mail send";
                            status = "13";
                            checkStatusid = 88;
                            model.Follow_Body = model.Body;
                            //cc.UpdateMailStatus(model.PersonalRowID, model.SubCheckRowID, 0, 1);
                            cc.UpdateBounceMailDetail(model.PersonalRowID, model.SubCheckRowID);
                            string AutoId = cc.insertEmailAutoVerification(model.Body, model.PersonalRowID, model.SubCheckRowID, model.subject, 101, DateTime.Now, 13, model.UniqueComponentId, "", 2,model.CandCheckRowID);
                            MAutoId = int.Parse(AutoId);
                            cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, model.ccmail, model.subject, status, "", "101", filename);
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);

                            ViewBag.SuccessMsg = "Mail Sent Successfully!";
                            // cc.insertEmailAutoVerification_Detail(int.Parse(AutoID),model.Body,"Send",model.Tomail,"",model.subject,"13","","100");
                            //cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Automation", 100);
                            //cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, "", model.subject, status, "", "100");


                        }


                        // result = mail.SendMail(DiDoDsplayName, From, model.Tomail, model.subject, model.Follow_Body);
                        //if (result)
                        //{
                        //cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, model.ccmail, model.subject, status, "", "101", filename);
                        //    cc.UpdateCheckStatus(model.PersonalRowID,model.SubCheckRowID,1,checkStatusid,"Manual Automation",101);
                        //    ViewBag.SuccessMsg = "Mail Sent Successfully!";
                        // }
                        //else
                        //{
                        //    ViewBag.ErrorMsg = "Mail sending failed!";
                        //}
                        // cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 59, model.Remarks, "Operation-QC", teamMemerRowId);
                        //ViewBag.SuccessMsg = "Record update successfully";
                        //IsSuccess = true;
                    }
                    //else if(!string.IsNullOrEmpty(model.GroupId))
                    //{
                    //    DataTable dt1 = new DataTable();
                    //    ClientCustomize cc = new ClientCustomize();
                    //    string filename = "";
                    //    // Send Attachment/////

                    //    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    //    {
                    //        //provideddocuments

                    //        // MailPath = "D:\\Attachment\\Mails\\";                           
                    //        filename = "ManualEmail_" + model.PersonalRowID + model.SubCheckRowID + Path.GetExtension(model.UploadDoc.FileName);
                    //        model.UploadDoc.SaveAs(Server.MapPath(MailPath + filename));
                    //        cc.autouploadDoc(model.PersonalRowID, Convert.ToInt32(model.SubCheckRowID), filename, MailPath);
                    //    }


                    //    int MAutoId = 0;
                    //    string MToEmail = "";
                    //    string MSentTime = "";
                    //    string MBody = "";

                    //    DataTable dt = new DataTable();

                    //    if (model.ReminderStatus == "Group Creation")
                    //    {
                    //        sendOrRec = "Initail Mail send";
                    //        status = "13";
                    //        checkStatusid = 88;
                    //        model.Follow_Body = model.Body;
                    //        string AutoId = cc.insertEmailAutoVerification(model.Body, model.PersonalRowID, model.SubCheckRowID, model.subject, 101, DateTime.Now, 13, model.UniqueComponentId, model.GroupId);
                    //        MAutoId = int.Parse(AutoId);
                    //        cc.updateAutoGroupMailStatus(model.GroupId,2,1);
                    //        // cc.insertEmailAutoVerification_Detail(int.Parse(AutoID),model.Body,"Send",model.Tomail,"",model.subject,"13","","100");
                    //        //cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Automation", 100);
                    //        //cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail, "", model.subject, status, "", "100");


                    //    }

                    //    else if (model.ReminderStatus == "Initial Mail Send")
                    //    {
                    //        sendOrRec = "Send REM1";
                    //        status = "8";
                    //        checkStatusid = 83;
                    //        dt1 = cc.getManualAutomationGroupRemDetail(model.GroupId, model.ReminderStatus);
                    //        if (dt1.Rows.Count > 0)
                    //        {
                    //            MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                    //            MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                    //            MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                    //            //dt = cc.getAlertMailingHTML("Employment Check Follow Up 1");
                    //            model.Follow_Body = model.Body;
                    //            //if (dt.Rows.Count > 0)
                    //            //{
                    //            //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                    //            //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + model.Body;
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                    //            //}


                    //        }

                    //    }
                    //    else if (model.ReminderStatus == "Sent Reminder1")
                    //    {
                    //        sendOrRec = "Send REM2";
                    //        status = "9";
                    //        checkStatusid = 84;
                    //        dt1 = cc.getManualAutomationGroupRemDetail(model.GroupId, model.ReminderStatus);
                    //        if (dt1.Rows.Count > 0)
                    //        {
                    //            MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                    //            MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                    //            MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                    //            MBody = model.Body;
                    //            model.Follow_Body = model.Body;
                    //            //dt = cc.getAlertMailingHTML("Employment Check Follow Up 2");
                    //            //if (dt.Rows.Count > 0)
                    //            //{
                    //            //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                    //            //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + MBody;
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                    //            //}


                    //        }

                    //    }
                    //    else if (model.ReminderStatus == "Sent Reminder2")
                    //    {
                    //        sendOrRec = "Send REM3";
                    //        status = "10";
                    //        checkStatusid = 85;
                    //        dt1 = cc.getManualAutomationGroupRemDetail(model.GroupId, model.ReminderStatus);
                    //        if (dt1.Rows.Count > 0)
                    //        {
                    //            MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                    //            MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                    //            MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                    //            MBody = model.Body;
                    //            model.Follow_Body = model.Body;
                    //            //dt = cc.getAlertMailingHTML("Employment Check Follow Up 3");
                    //            //if (dt.Rows.Count > 0)
                    //            //{
                    //            //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                    //            //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + MBody;
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                    //            //}


                    //        }

                    //    }

                    //    else if (model.ReminderStatus == "Sent Reminder3")
                    //    {
                    //        sendOrRec = "Send REM4";
                    //        status = "11";
                    //        checkStatusid = 86;
                    //        dt1 = cc.getManualAutomationGroupRemDetail(model.GroupId, model.ReminderStatus);
                    //        if (dt1.Rows.Count > 0)
                    //        {
                    //            MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                    //            MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                    //            MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                    //            MBody = model.Body;
                    //            model.Follow_Body = model.Body;
                    //            //dt = cc.getAlertMailingHTML("Employment Check Follow Up 4");
                    //            //if (dt.Rows.Count > 0)
                    //            //{
                    //            //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                    //            //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + MBody;
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                    //            //}


                    //        }

                    //    }
                    //    else if (model.ReminderStatus == "Sent Reminder4")
                    //    {
                    //        sendOrRec = "Send REM5";
                    //        status = "12";
                    //        checkStatusid = 87;
                    //        dt1 = cc.getManualAutomationGroupRemDetail(model.GroupId, model.ReminderStatus);
                    //        if (dt1.Rows.Count > 0)
                    //        {
                    //            MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                    //            MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                    //            MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                    //            MBody = model.Body;
                    //            model.Follow_Body = model.Body;
                    //            //dt = cc.getAlertMailingHTML("Employment Check Follow Up 5");
                    //            //if (dt.Rows.Count > 0)
                    //            //{
                    //            //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                    //            //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + MBody;
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                    //            //}


                    //        }

                    //    }

                    //    else if (model.ReminderStatus == "Sent Reminder5")
                    //    {
                    //        sendOrRec = "Send Greater REM5";
                    //        status = "14";
                    //        checkStatusid = 89;
                    //        dt1 = cc.getManualAutomationGroupRemDetail(model.GroupId, model.ReminderStatus);
                    //        if (dt1.Rows.Count > 0)
                    //        {
                    //            MAutoId = Convert.ToInt32(dt1.Rows[0]["AutoID"].ToString());
                    //            MToEmail = dt1.Rows[0]["ToEmail"].ToString();
                    //            MSentTime = Convert.ToDateTime(dt1.Rows[0]["CreationDate"].ToString()).ToString("dd MMMM yyyy hh:mm tt");
                    //            MBody = model.Body;
                    //            model.Follow_Body = model.Body;
                    //            //dt = cc.getAlertMailingHTML("Employment Check Follow Up 5");
                    //            //if (dt.Rows.Count > 0)
                    //            //{
                    //            //    model.Follow_Body = dt.Rows[0]["HTML"].ToString();
                    //            //    model.Follow_Body = model.Follow_Body + "<br></br><br></br><div>From:#FromMail1</div><div>Sent:#SentTime1</div><div>To:#ToMail1</div><div>Subject:#Subject1</div><br />" + MBody;
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#FromMail1", AutoEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#SentTime1", MSentTime);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#ToMail1", MToEmail);
                    //            //    model.Follow_Body = model.Follow_Body.Replace("#Subject1", model.subject);
                    //            //}


                    //        }

                    //    }


                    //    // result = mail.SendMail(DisplayName, From, model.Tomail, model.subject, model.Follow_Body);
                    //    //if (result)
                    //    //{
                    //    cc.insertEmailAutoVerification_Detail(MAutoId, model.Follow_Body, sendOrRec, model.Tomail,model.ccmail , model.subject, status, "", "101", filename);
                    //    if(!string.IsNullOrEmpty(model.GroupId))
                    //    {
                    //        DataTable dtcheck;
                    //        dtcheck=cc.GetCheckStatusDetailByGroup(model.GroupId);
                    //        if(dtcheck.Rows.Count>0)
                    //        {
                    //            foreach (DataRow dr in dtcheck.Rows) // loop for adding add from dataset to list<modeldata>
                    //            {
                    //          cc.UpdateCheckStatus(Convert.ToInt32(dr["PersonalRowId"].ToString()), Convert.ToInt32(dr["SubCheckRowId"].ToString()), 1, checkStatusid, "Manual Automation", 101);

                    //            }

                    //        }
                    //    }
                    //    else
                    //    {
                    //        cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);
                    //    }
                    //    ViewBag.SuccessMsg = "Mail Sent Successfully!";
                    //    // }
                    //    //else
                    //    //{
                    //    //    ViewBag.ErrorMsg = "Mail sending failed!";
                    //    //}
                    //    // cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 59, model.Remarks, "Operation-QC", teamMemerRowId);
                    //    //ViewBag.SuccessMsg = "Record update successfully";
                    //    //IsSuccess = true;
                    //}

                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return PartialView("~/Views/Shared/ReminderMail.cshtml", model);
        }


        private void ExtractPages(string sourcePDFpath, string outputPDFpath, int startpage, int endpage)
        {
            sourcePDFpath = "d:\\aa\\test.pdf";
            outputPDFpath = sourcePDFpath + "_auto" + startpage.ToString()+"_" + endpage.ToString() + ".pdf";
            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;

            reader = new PdfReader(sourcePDFpath);
            sourceDocument = new Document(reader.GetPageSizeWithRotation(startpage));
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPDFpath, System.IO.FileMode.Create));
            sourceDocument.Open();
            for (int i = startpage; i <= endpage; i++)
            {
                importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                pdfCopyProvider.AddPage(importedPage);
            }
            sourceDocument.Close();
            reader.Close();
        }

        public ActionResult RevertDetail(int page = 1, string Search = "", string version = "", short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CaseStatus = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            int pageSize = 500;
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string Designation = "";
            string department = "";
            int status = 0;
            try
            {
                if(CheckStatus== "SubNotMatch")
                {
                    status = 1;
                }
                else if (CheckStatus == "Bounce")
                {
                    status = 1;
                }
                else if(CheckStatus == "Discarted")
                {
                    status = 3;
                }
                model = repoVerificationInfo.getAutoRevertDetail(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus,"", status);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);


            //AutoRevertViewModel model = new AutoRevertViewModel();
            //model= repoVerificationInfo.getAutoRevertDetail();
            //return View(model);
        }

        [SessionExpireFilterAttribute]
        
        public PartialViewResult AutoRevertProceed(int AutoId,string subject)
        {
            AutoRevertViewModel model = new AutoRevertViewModel();

            try
            {
                DataTable dt= cc.GetAutoNotMatchSubject(AutoId);
                model.Subject = dt.Rows[0][0].ToString();
                model.id = AutoId;
                BindSubCheckList();
                //if (PersonalRowID > 0 && SubCheckRowID > 0)
                //{
                //    model = repoDashboard.getAllProcess_Remark(processName, PersonalRowID, SubCheckRowID);
                //    model.CheckRowID = CheckRowid;
                //    model.PersonalRowID = PersonalRowID;
                //    model.SubCheckRowID = Convert.ToInt16(SubCheckRowID);
                //    model.CheckstatusId = checkstatusid;
                //}
                //else
                //{
                //    TempData["ErrorMsg"] = "invlid id!";
                //}
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //return View(model);
            return PartialView("~/Views/Shared/AutoRevertProceed.cshtml", model);
        }

        public PartialViewResult BounceAutoRevertProceed(int AutoId, string subject)
        {
            ReminderMail model = new ReminderMail();

            try
            {
                
                //DataTable dt = cc.GetAutoNotMatchSubject(AutoId);
                //model.subject = dt.Rows[0][0].ToString();
                model.BounceId = AutoId;
                
                //BindSubCheckList();
                //ClientCustomize cc = new ClientCustomize();
                //model = repoDashboard.get_AutoFindReminderEmailer(1, 1, "");
                //if (PersonalRowID > 0 && SubCheckRowID > 0)
                //{
                //    model = repoDashboard.getAllProcess_Remark(processName, PersonalRowID, SubCheckRowID);
                //    model.CheckRowID = CheckRowid;
                //    model.PersonalRowID = PersonalRowID;
                //    model.SubCheckRowID = Convert.ToInt16(SubCheckRowID);
                //    model.CheckstatusId = checkstatusid;
                //}
                //else
                //{
                //    TempData["ErrorMsg"] = "invlid id!";
                //}
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //return View(model);
            return PartialView("~/Views/Shared/BounceAutoRevertProceed.cshtml", model);
        }


              [SessionExpireFilterAttribute]
        [HttpPost, ValidateInput(false)]
        public PartialViewResult BounceAutoRevertProceed(ReminderMail Model,string Search)
        {
            if (!string.IsNullOrEmpty(Search))
            {
               DataTable Dt= cc.GetAutoBounceMailDetail(Model.CandCheckRowID);
                if(Dt.Rows.Count>0)
                {
                    // PersonalRowID,SubCheckRowID,Subject,body,UniqueComponentID,CandCheckRowID
                    Model.AutoId = Convert.ToInt32(Dt.Rows[0]["AutoID"].ToString());
                    Model.Body = Dt.Rows[0]["body"].ToString();
                    Model.subject = Dt.Rows[0]["Subject"].ToString();
                    Model.UniqueComponentId = Dt.Rows[0]["UniqueComponentID"].ToString();
                    Model.CandCheckRowID = Dt.Rows[0]["CandCheckRowID"].ToString();
                    Model.PersonalRowID = Convert.ToInt32(Dt.Rows[0]["PersonalRowID"].ToString());
                    Model.SubCheckRowID = Convert.ToInt32(Dt.Rows[0]["SubCheckRowID"].ToString());
                    Model.ccmail = Dt.Rows[0]["CCEmail"].ToString();
                    Model.ReminderStatus = "Bounce Mail";

                    //Model.ReminderStatus =
                }

            }
            else {
               
                    string filename = "";
                    string MailPath = "";
                    
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ManualEmailPath"].ToString()))
                    {
                        MailPath = ConfigurationManager.AppSettings["ManualEmailPath"].ToString();
                    }

                    if (Model.UploadDoc != null && Model.UploadDoc.ContentLength > 0)
                    {
                        //provideddocuments

                        // MailPath = "D:\\Attachment\\Mails\\";                           
                        filename = "ManualEmail_" + Model.PersonalRowID + Model.SubCheckRowID + Path.GetExtension(Model.UploadDoc.FileName);
                        Model.UploadDoc.SaveAs(Server.MapPath(MailPath + filename));
                        //cc.autouploadDoc(model.PersonalRowID, Convert.ToInt32(model.SubCheckRowID), filename, MailPath);                          
                    }
                    //string sendOrRec = "Initail Mail send";
                    //string status = "13";
                    //int checkStatusid = 88;
                    //Model.Follow_Body = Model.Body;
                    //cc.UpdateBounceMailDetail(Model.AutoId);
                    ////cc.UpdateMailStatus(Model.PersonalRowID, Model.SubCheckRowID, 0, 1);
                    ////string AutoId = cc.insertEmailAutoVerification(Model.Body, Model.PersonalRowID, Model.SubCheckRowID, model.subject, 101, DateTime.Now, 13, model.UniqueComponentId, "", 2);
                    ////MAutoId = int.Parse(AutoId);
                    //cc.insertEmailAutoVerification_Detail(Model.AutoId, Model.Follow_Body, sendOrRec, Model.Tomail, Model.ccmail, Model.subject, status, "", "101", filename);
                    //cc.UpdateCheckStatus(Model.PersonalRowID, Model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);
                    //ViewBag.SuccessMsg = "Mail Sent Successfully!";


                    string sendOrRec = "Initail Mail send";
                    string status = "13";
                    int checkStatusid = 88;
                    Model.Follow_Body = Model.Body;
                    cc.UpdateBounceMailDetail(Model.PersonalRowID, Model.SubCheckRowID);
                    string AutoId = cc.insertEmailAutoVerification(Model.Body, Model.PersonalRowID, Model.SubCheckRowID, Model.subject, 101, DateTime.Now, 13, Model.UniqueComponentId, "", 2);
                   int MAutoId = int.Parse(AutoId);
                    cc.insertEmailAutoVerification_Detail(MAutoId, Model.Follow_Body, sendOrRec, Model.Tomail, Model.ccmail, Model.subject, status, "", "101", filename);
                    cc.UpdateCheckStatus(Model.PersonalRowID, Model.SubCheckRowID, 1, checkStatusid, "Manual Automation", 101);
                cc.updateSubjectNotMatched(Model.AutoId, 2, Model.PersonalRowID, Model.SubCheckRowID);
                ViewBag.SuccessMsg = "Mail Sent Successfully!";



                
            }
            
            
            //return View(model);
            return PartialView("~/Views/Shared/BounceAutoRevertProceed.cshtml", Model);
        }


        public ActionResult BounceRevertDetailToTM(int page = 1, string Search = "", string version = "", short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CaseStatus = "")

        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            int pageSize = clsGlobalCommon.PageSize;
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string Designation = "";
            string department = "";
            int status = 0;
            try
            {
                if (CheckStatus == "SubNotMatch")
                {
                    status = 1;
                }
                else if (CheckStatus == "Bounce")
                {
                    status = 1;
                }
                else if (CheckStatus == "Discarted")
                {
                    status = 3;
                }
                model = repoVerificationInfo.getAutoRevertDetail(page, pageSize, Search, department, CheckStatus, teamMemerRowId, ClientRowID, CheckStatus, "", status);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);


            //AutoRevertViewModel model = new AutoRevertViewModel();
            //model= repoVerificationInfo.getAutoRevertDetail();
            //return View(model);
        }


        [SessionExpireFilterAttribute]
        [HttpPost, ValidateInput(false)]      
        public PartialViewResult AutoRevertProceed(AutoRevertViewModel Model)
        {
            int teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            DataTable dt = new DataTable();
            string filename = "";
            string MailPath = "D:\\Attachment\\Mails\\";
            try
            {
                if(!string.IsNullOrEmpty( Model.companyRefNo))
                {
                    dt = cc.getAutoExistCompRefNo(Model.companyRefNo);
                    int CheckStatusId = 0;
                    if (dt.Rows.Count > 0)
                    {
                         filename = "NotRec_" + Model.id + ".msg";
                        Model.PersonalRowId = Convert.ToInt32(dt.Rows[0][0].ToString());
                        CheckStatusId=   cc.getCurrentCheckStatus(Model.PersonalRowId, int.Parse(Model.CheckName));
                        if(CheckStatusId==36 || CheckStatusId == 37)
                        {
                            @ViewBag.ErrorMsg = "Check has been Already Completed";
                        }
                        else if (CheckStatusId == 20 || CheckStatusId == 73 || CheckStatusId == 74 || CheckStatusId == 75 || CheckStatusId == 76 
                            || CheckStatusId == 77 || CheckStatusId == 78 || CheckStatusId == 79 || CheckStatusId == 80
                            || CheckStatusId == 81 || CheckStatusId == 82 || CheckStatusId == 83 || CheckStatusId == 84
                            || CheckStatusId == 85 || CheckStatusId == 86 || CheckStatusId == 87 || CheckStatusId == 88
                            || CheckStatusId == 89 || CheckStatusId == 90 || CheckStatusId == 91 || CheckStatusId == 92)
                        {
                            cc.UpdateMailStatus(Model.PersonalRowId, int.Parse(Model.CheckName), 3, 1);
                            cc.UpdateCheckStatus(Model.PersonalRowId, Convert.ToInt32(Model.CheckName), 1, 74, "", teamMemerRowId);
                            cc.updateSubjectNotMatched(Model.id, 2,Model.PersonalRowId, Convert.ToInt32(Model.CheckName));
                            cc.autouploadDoc(Model.PersonalRowId, Convert.ToInt32(Model.CheckName), filename, MailPath);
                            @ViewBag.SuccessMsg = "Proceed Successfully";
                        }          
                        
                    }
                    else
                    {
                        
                        ViewBag.ErrorMsg = "Invalid Company Ref No.";
                    }
                }
                else
                {
                    if(Model.MailStatus=="NotMatch")
                    {
                        cc.updateSubjectNotMatched(Model.id, 3,0,0);
                       // cc.UpdateMailStatus(Model.PersonalRowId, int.Parse(Model.CheckName), 3, 1);
                        ViewBag.SuccessMsg = "Not Match process has been done";
                    }
                    
                }
                
                BindSubCheckList();

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //return View(model);
            return PartialView("~/Views/Shared/AutoRevertProceed.cshtml", Model);
        }




        [NonAction]
        public void BindSubCheckList()
        {
            ViewBag.EmployementSubCheckList = repoDDL.GetSubCheckFamilyForDDL(12);
        }


        [SessionExpireFilterAttribute]
        public FileResult GetSubNotMatchMail(string ImageName)
        {
            string ToPath = System.Configuration.ConfigurationManager.AppSettings["mailpath"].ToString();
            byte[] fileBytes = new byte[] { };
            if (!System.IO.File.Exists(ToPath + ImageName))
            {
                // copy file from mail server to application server 
                ReadRevertdocument(ToPath, "", ImageName);
            }
            if (System.IO.File.Exists(ToPath + ImageName))
            {
                try
                {
                    fileBytes = System.IO.File.ReadAllBytes(ToPath + ImageName);
                    string fileName = ImageName;
                }
                catch
                {
                    return null;
                };

            }
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, ImageName);
        }

        private void ReadRevertdocument(string localPath, string folderName, string fileName)
        {
            try
            {
                //localPath = @"d:\attachments\";
                //fileName = "test.txt";
                string ftppath = "";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ftppathRevertRec"].ToString()))
                {
                    ftppath = ConfigurationManager.AppSettings["ftppathRevertRec"].ToString();
                }
                //FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create("ftp://10.80.0.76/"+ folderName +"/" + fileName);
                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftppath +  fileName);
                requestFileDownload.Credentials = new NetworkCredential("delsecuritas", "pass@123");
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                FileStream writeStream = new FileStream(localPath + fileName, FileMode.Create);
                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }
                responseStream.Close();
                writeStream.Close();
                requestFileDownload = null;
                responseFileDownload = null;
            }
            catch (System.Exception ex)
            {
                //string body = "<div>Exception:" + ex.ToString() + "</div><br></br><div>LocalPath:" + localPath + "</div><br></br><div>fileName:" + fileName + "</div><br></br><div>Exception:" + ex.ToString() + " </div><br></br><div></div><br></br><div></div><br></br>";
                //SendErrorEmail("BGV-Automation Error on Upload Documents", "bgv.helpdesk@securitas-india.com", body, "");
            }


        }

        [NonAction]
        void BindMailList(int PersonalRowID = 0, short subCheckId = 0)
        {
            try
            {
                ViewBag.GetMailList = repoVerificationInfo.GetAutoUploadMails(PersonalRowID, subCheckId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [SessionExpireFilterAttribute]
        public PartialViewResult ManualGroupDetail( string GroupId = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
               
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                string AutoEmail = "";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["AutoEmailFrom"].ToString()))
                {
                    model = repoVerificationInfo.GetAutoManualGroupDetails(GroupId);
                    AutoEmail = ConfigurationManager.AppSettings["AutoEmailFrom"].ToString();
                }        

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return PartialView("~/Views/Shared/ManualGroupDetail.cshtml", model);
        }

        [SessionExpireFilterAttribute]
        public ActionResult EmailAutomationCheckStatus(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string datefrom="", string dateTo = "",int CheckStatusId = 0)
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                int pageSize = 20;
                //string SpocEmail = Session["TeamMemberEmail"].ToString();
                int teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);     
                if(CheckStatusId!=0)
                {
                    model = repoVerificationInfo.EmailAutomationCheckStatus(page, pageSize, CheckStatusId, Search, datefrom, dateTo,  teamMemerRowId );

                }


                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                
                BindMasterCheckStatus();                
                
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        public void ExportAutomationMIS()
        {
            DataTable dt = new DataTable();
             dt = cc.EmailAutomationCheckStatus(0, null, null, null);
            
                ExportToExcel(dt, "AutomationMIS");
            
        }

        [NonAction]
        void BindMasterCheckStatus()
        {
            try
            {
                ViewBag.CheckId = repoDDL.GetMastercheckId();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        bool SendAlertMassageForCheckClearationForClientSpoc(short ClientRowID, int CheckRowID, short CheckFamilyRowID, short SubCheckRowID,string Colorname, string Remarks)
        {
            bool result = false;
            string MailSignature = string.Empty;
            string From = string.Empty;
            string DisplayName = string.Empty;
            string Cc = string.Empty; //Session["CSpocEmailID"].ToString();
            clsSendMail mail;
            mail = new clsSendMail();
            var signature = mail.GetCompanyMailSignature();
            if (signature != null)
            {
                MailSignature = string.Concat("For Securitas India. <br /><br />");
                MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br /><b>Mobile: ", signature.MobileNo,
                    " | Direct Tel: ", signature.PhoneNo, " | Fax: ", signature.FaxNo, "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", signature.GeneralEmail, "'>", signature.GeneralEmail, "</a>");
            }
            From = signature.GeneralEmail;
            DisplayName = signature.CompanyName.Trim();
            var personalData = repoPQPersonal.GetCheckOrPersonalDetailsForSendMailById(CheckFamilyRowID, SubCheckRowID, CheckRowID);
            if (personalData != null)
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/CheckClear.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{FCandidateName}", personalData.CandidateName);
                body = body.Replace("{FCheckName}", personalData.SubCheckName);
                body = body.Replace("{FClientRefId}", personalData.ClientRefID);
                body = body.Replace("{FReferenceID}", personalData.CompanyRefNo);
                body = body.Replace("{FInsuffRemarks}", personalData.InsuffRemarks);
                body = body.Replace("{Checkcolor}", Colorname);
                body = body.Replace("{Remarks}", Remarks);

                body = body.Replace("{MailSignature}", MailSignature);
                string Subject = personalData.SubCheckName + " is done " + " " + Colorname + " " +"for"+" "+  personalData.CandidateName;
                string To = string.Empty;
                if (personalData.CompanyRefNo.ToLower().Contains("infogain") || ClientRowID == 8)// Stop Cvent insuff mail
                    To = personalData.SpocEmailID;
                else
                    To = personalData.SpocEmailID + "," + personalData.ClientEmailID;

                string ToMail = To.ToLower();
                
                ToMail = ToMail.Replace(",", ";");
                string[] arToMail = ToMail.Split(';');
                ToMail = "";
                foreach (string a in arToMail)
                {
                    if (!ToMail.Contains(a))
                    {
                        ToMail = ToMail + a.Trim() + ",";
                    }
                }
                if (ToMail.Length > 1)
                    ToMail = ToMail.Substring(0, ToMail.Length - 1);

                //bool flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                //if (flag)
                //{
                //    result = true;
                //}
                //else
                //{
                //    TempData["ErrorMsg"] = "Mail sending failed!";
                //}
            }
            else
            {
                TempData["ErrorMsg"] = "Candidate login details not found!";
            }
            return result;
        }
       
       

       

        public ActionResult showMap(string address = "",int PersonalRowID=0,int SubCheckRowID = 0)
        {
            string apikey= System.Configuration.ConfigurationManager.AppSettings["GoogleMAPAPIKey"].ToString();
            DataTable dt=  cc.GetAddressForMap(PersonalRowID, SubCheckRowID);
            if (dt.Rows.Count> 0)
            {
                geoModel gm = new geoModel();
                var locationService = new GoogleLocationService(apikey);
                gm.address = dt.Rows[0]["av_add"].ToString();
                var point = locationService.GetLatLongFromAddress(gm.address);
                if (point == null)
                {
                    gm.latitude = 0;
                    gm.Longitude = 0;
                }
                else
                {
                    gm.latitude = point.Latitude;
                    gm.Longitude = point.Longitude;
                }

                if (gm.latitude != 0 && gm.Longitude != 0)
                {
                    cc.UpdateLatitudeLongitude(gm.latitude, gm.Longitude, PersonalRowID, SubCheckRowID);
                }
                return View(gm);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult showMap(geoModel gm)
        {
            string address1 = gm.address;

            return View(gm);
        }
        [HttpGet]
        public JsonResult getlatlong(string add = "")
        {
            string apikey = System.Configuration.ConfigurationManager.AppSettings["GoogleMAPAPIKey"].ToString();
            var locationService = new GoogleLocationService(apikey);
            if (add != "")
            {
                var point = locationService.GetLatLongFromAddress(add);
              
                return Json(new { pass = point }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                add = "wipro udyog vihar phase -4 gurgaon, haryana india";
                var myObj = locationService.GetLatLongFromAddress(add);
                myObj.Latitude = 0;
                myObj.Longitude = 0;
                return Json(new { pass = myObj }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportAllDeptMISAddress(string Department ="",string CompletedFromDate = "" , string CompletedToDate = "")
        {
            AddressMIS AddressModel = new AddressMIS();
            if (CompletedFromDate != "" && CompletedToDate!="")
            {
                Department = "Address";
               
                DataTable DT = new DataTable();
                short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                if (Department == "Address")
                {
                    DT = cc.ExportAllDeptMIS_Datefilter("MIS_addressDepartmentdateFilter", CompletedFromDate, CompletedToDate);
                    ExportToExcel(DT, "AddressMISReport");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Fill to date and from date!";
            }
          

            return View(AddressModel);
        }


        [SessionExpireFilterAttribute]
        public ActionResult CheckMovementProcess(FormCollection form, string Search = "", string Remarks = " ", string SaveWork = "", String CheckDel = "", int pageNo = 1, int pageSize = 100)
        {
            try
            {

                string Department = Session["Department"].ToString();
                string ddlCheckStatus = form["ddlCheckStatus"];
                string ColorName = form["ddlColor"];
                checkViewModel model = new checkViewModel();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                if (Search == "")
                {
                   //BindSubchecknameDelete(0, 0);
                    BindSubcheckname(0, 0,Department);
                    BindMasterCheckStatusForSpoc(Department);
                }

                else
                {

                    // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                    DataTable dt = new DataTable();
                    ClientCustomize cc = new ClientCustomize();
                    dt = cc.GetCandidateChecksForaddress(Search, Department);

                    List<CheckSummaryDetailViewModel> lmd = new List<CheckSummaryDetailViewModel>();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            lmd.Add(new CheckSummaryDetailViewModel
                            {
                                SecuritasID = (dr["CompanyRefNo"].ToString()),
                                CheckName = (dr["SubCheckName"].ToString()),
                                CheckStatus = dr["CheckStatus"].ToString(),
                                CheckStatusID = (dr["CheckStatusID"].ToString()),
                                OrderDate = (dr["orderDate"].ToString()),
                                OutDate = (dr["outDate"].ToString()),
                                CreatedDate = (dr["CreationDate"].ToString()),
                                ColorName = dr["ColorName"].ToString(),
                                ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"]),
                                Name = (dr["Name"].ToString()),

                                PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),

                            });

                        }
                        model.TotalRecords = dt.Rows.Count;
                        BindSubcheckname(Convert.ToInt32(dt.Rows[0]["ClientRowID"]), Convert.ToInt32(dt.Rows[0]["PersonalRowID"]),Department);
                        BindMasterCheckStatusForSpoc(Department);

                    }






                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.ClientServiceChecks = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);



                    DataTable dtvalidation = cc.ValidationOnCheckMovment(Search, Convert.ToInt32(ColorName));

                    if (dtvalidation.Rows.Count > 0)
                    {

                        TempData["SuccessMsg"] = "Check alrady compalted . Please contact to Spoc!";

                    }
                    else
                    {


                        if (ColorName != null && ColorName != "0" && ddlCheckStatus != "0" && Remarks != "") //this is use for check move
                        {
                            if (Remarks != "")
                            {
                                DataTable dtNotapplicable = cc.CheckNotApplicable(Search, Convert.ToInt32(Request.Form["ddlColor"]));
                                if (dtNotapplicable.Rows.Count > 0 && Convert.ToInt32(ddlCheckStatus) != 8)
                                {
                                    TempData["SuccessMsg"] = "Not applicable check move only data entry TM!";
                                }
                                else
                                {
                                    int MoveCheckId = Convert.ToInt32(ddlCheckStatus);


                                    int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);

                                    cc.PQCandidateChecksDeleteCheck(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(Request.Form["ddlColor"]), 1, MoveCheckId, "helpdesk", Convert.ToInt32(Session["TeamMemberRowID"]), 0, Remarks);
                                    cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), strDDLValue, MoveCheckId, Remarks, "Support", Convert.ToInt32(Session["TeamMemberRowID"]));
                                    //Remarks = "Re verification -" + Remarks;
                                    #region Check Action History Save                           
                                    clsCommonMethods.AddCheckActionHistory(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt16(strDDLValue), "Re verification", Remarks);
                                    #endregion
                                    //cc.UpdateVertable(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), MoveCheckId);
                                    TempData["SuccessMsg"] = "Check Move Successfully!";
                                    return RedirectToAction("CheckMovementProcess", "Verification", new { Search = Search });

                                }






                            }
                            //else
                            //{
                            //    TempData["SuccessMsg"] = "Please fill all Details and move check!";

                            //}

                        }
                        else
                        {
                            TempData["SuccessMsg"] = "Please Select check details!";
                        }
                    }





                }

                return View(model);



            }
            catch (Exception)
            {
                throw;
            }
        }

        void BindSubcheckname(int ClientRowId, int PersonalRowID,string Department)
        {
            try
            {

                //ViewBag.chek = repoPQPersonal.getSubcheckdataforDelete(ClientRowId, PersonalRowID);
                ViewBag.chek = repoPQPersonal.getSubcheckdataforaddress(ClientRowId, PersonalRowID,Department);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        void BindMasterCheckStatusForSpoc(string Department)
        {
            try
            {

                ViewBag.CheckId = repoPQPersonal.GetMastercheckIdForSpoc(Department);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public ActionResult ExportAllDeptMISDateFilter(int ClientRowId = 0, string fromdate = "", string Todate = "")
        {
            Datefilter model = new Datefilter();
            try
            {
                if (fromdate == "" && Todate == "")
                {

                }
                else
                {
                    string Department = Session["Department"].ToString();
                    DataTable DT = new DataTable();
                    model.ClientRowId = Convert.ToInt16(Session["ClientRowID"]);
                    model.fromdate = fromdate.ToString();
                    model.Todate = Todate.ToString();


                    if (Department == "Address")
                    {
                        DT = cc.ExportAllDeptMISFilter(Department.ToString(), fromdate, Todate);
                        ExportToExcel(DT, "AddressMISReport");
                    }
                    else if (Department == "Education")
                    {
                        DT = cc.ExportAllDeptMISFilter(Department.ToString(), fromdate, Todate);
                        ExportToExcel(DT, "EducationMISReport");
                    }
                    else if (Department == "Employment")
                    {
                        DT = cc.ExportAllDeptMISFilter(Department.ToString(), fromdate, Todate);

                        ExportToExcel(DT, "EmployeementMISReport");
                    }

                    else if (Department == "Reference")
                    {
                        DT = cc.ExportAllDeptMISFilter(Department.ToString(), fromdate, Todate);
                        ExportToExcel(DT, "ReferenceMISReport");
                    }

                    else if (Department == "Criminal")
                    {
                        DT = cc.ExportAllDeptMISFilter(Department.ToString(), fromdate, Todate);
                        ExportToExcel(DT, "CriminalMISReport");
                    }

                    else if (Department == "National Identity")
                    {
                        DT = cc.ExportAllDeptMISFilter(Department.ToString(), fromdate, Todate);
                        ExportToExcel(DT, "NationalIdentityMISReport");
                    }

                    else if (Department == "Special Check")
                    {
                        DT = cc.ExportAllDeptMISFilter(Department.ToString(), fromdate, Todate);
                        ExportToExcel(DT, "SpecialMISReport");
                    }

                    else if (Department == "Report QC")
                    {
                        DT = cc.ExportAllDeptMISFilter(Department.ToString(), fromdate, Todate);
                        ExportToExcel(DT, "RWQCMISReport");
                    }



                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View();

        }

        [HttpPost]
        public JsonResult getcolglist(string CollegeName)
        {

            var result = repoCompany.getCollegelist(CollegeName);



            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult getUniversitylist(string UniversityName)
        {

            var result = repoCompany.getUniversitylist(UniversityName);



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilterAttribute]
        public ActionResult AddressTeamMemberAllocationToReverification(FormCollection form, int page = 1, string sort = "", string sortDir = "desc", string Search = "", string version = "New", string VerificationIds = "", short AllocatePTR = 0, short StateRowId = 0, string CheckStatus = "", short LocationId = 0)
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                short LocationRowID1 = Convert.ToInt16(form["LocationId"]);
                //LocationRowID = LocationRowID1;
                int pageSize = clsGlobalCommon.PageSize;


                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                string department = "Reverification";
                string statusid = "";
               
                if (CheckStatus == "Reverification")
                {
                    statusid = "20";
                }
                BindStateVendor(teamMemerRowId, CheckStatus);
                GetVendorDDL(StateRowId);

                BindCityByStateId(StateRowId, teamMemerRowId);
                if (Designation == "Team Member-Add")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocatePTR > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateAddressStatusTeamMemberViewModel addModel = new PQUpdateAddressStatusTeamMemberViewModel();
                        addModel.ModifiedBy = teamMemerRowId;
                        addModel.ModifiedDate = DateTime.Now;
                        addModel.CheckStatus = "Allocated-PTR";
                        addModel.PTRMgrAllocatedTo = AllocatePTR;

                        foreach (var item in verIDs)
                        {
                            addModel.AddressRowID = item;
                            var addDetails = repoAddressInfo.GetPQAddressDetailsById(item);

                            repoAddressInfo.UpdateAddressStatusByTeamMember(addModel);
                            cc.UpdateCheckStatus(addDetails.PersonalRowID, addDetails.SubCheckRowID, 1, 27, "Check", teamMemerRowId);
                            cc.UpdatePartnerUserAllocation(addModel.AddressRowID);// Set partner login rowId 0 in pqaddressverPartner
                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(addDetails.PersonalRowID, "AddressTeamMemberAllocationToPartnerMgr", addModel.CheckStatus, addDetails.UniqueComponentID, "Update");
                            #endregion

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(addDetails.PersonalRowID, addDetails.SubCheckRowID, addModel.CheckStatus, "Team Member Allocated To Partner");
                            #endregion
                        }
                    }
                    if (version == "New")
                    {
                        model = repoVerificationInfo.GetReverification(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, StateRowId, CheckStatus, LocationId);
                        if (CheckStatus == "Reverification")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType == "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count();
                        }
                        else if (CheckStatus == "Pending")
                        {
                            var list = model.PQVerificationInfo.Where(T => T.CheckType != "ReVerification").ToList();
                            model.PQVerificationInfo = list;
                            model.TotalRecords = list.Count();
                        }
                    }
                    //else
                    //{
                    //    //model = repoVerificationInfo.GetCandidateAddressListForPTRMgrAllocationByTM(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, StateRowId, CheckStatus);
                    //    model = repoVerificationInfo.GetCandidateAddressListForPTRMgrAllocationByTM1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, StateRowId, CheckStatus, LocationId);

                    //}
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        public ActionResult TeamLeaderEduAllocationToTeamMember(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string version = "New", string VerificationIds = "", short AllocateTM = 0, short AllocateTMQC = 0, short ClientRowID = 0, string OCVerIDs = "", string CheckStatus = "",string universityName = "", string collegeName = "", string CourseName = "", string YrOfPassing = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                string department = "";
                string statusid = "";

                if (CheckStatus == "Unallocated" && Designation == "Team Leader-Edu")
                {
                    statusid = "18";
                }
                //BindStates();
                //BindClientList();
                BindClientListBaseOnLogin(teamMemerRowId);
                GetTMMemberForAllocate();
                GetTMQCMemberForAllocate();

                byte UnAllocatedCount = 0;

                if (Designation == "Team Leader-Edu")
                {
                    if (!string.IsNullOrEmpty(VerificationIds) && AllocateTM > 0 && AllocateTMQC > 0)
                    {
                        int[] verIDs = VerificationIds.Split(',').Select(str => int.Parse(str)).ToArray();
                        PQUpdateEducationTeamLeaderViewModel eduModel = new PQUpdateEducationTeamLeaderViewModel();
                        eduModel.ModifiedBy = teamMemerRowId;
                        eduModel.ModifiedDate = DateTime.Now;
                        eduModel.CheckStatus = "Allocated-TM";
                        eduModel.TMAllocatedTo = AllocateTM;
                        eduModel.TMAllocatedDate = DateTime.Now;
                        eduModel.TMQCAllocatedTo = AllocateTMQC;
                        eduModel.TMQCAllocatedDate = DateTime.Now;
                        foreach (var item in verIDs)
                        {
                            DataTable DtCheck = new DataTable();
                            DtCheck = cc.GetCheckDetails(item);
                            if (DtCheck.Rows.Count > 0)
                            {
                                int personalRowid = Convert.ToInt32(DtCheck.Rows[0]["personalrowid"]);
                                short Clientid = Convert.ToInt16(DtCheck.Rows[0]["ClientRowID"]);
                                short SubCheckFamilyID = Convert.ToInt16(DtCheck.Rows[0]["SubCheckFamilyID"]);
                                string CheckUniqueID = (DtCheck.Rows[0]["CheckUniqueID"].ToString());

                                //eduModel.EducationRowID = item;
                            //var addDetails = repoEducationInfo.GetPQEducationDetailsById(item);
                            bool result = repoPQClientTMember.IsClientDepartTeamMembersExist(Clientid, eduModel.TMAllocatedTo, eduModel.TMQCAllocatedTo, "Education", "Team Member-Edu", "Team Member-Edu-QC");

                            if (result)
                            {
                                    //repoEducationInfo.UpdateEducationInfoByTeamLeader(eduModel);
                                    cc.OperationAllocation(item, eduModel.TMAllocatedTo, "OPTM");
                                    cc.OperationAllocation(item, eduModel.TMQCAllocatedTo, "OPQC");
                                    #region Save LogTransaction Details
                                    clsCommonMethods.AddPQLogTrasaction(personalRowid, "TeamLeaderAllocationToTeamMember", eduModel.CheckStatus, CheckUniqueID, "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(personalRowid, SubCheckFamilyID, eduModel.CheckStatus, "");
                                #endregion

                                #region update Check status from Operation TM                                
                                cc.UpdateCheckStatus(personalRowid, SubCheckFamilyID, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #endregion
                            }
                            else
                            {
                                UnAllocatedCount++;
                            }
                        }
                        }
                    }
                    if (version == "New")
                    {
                        department = "Education";
                       model = repoVerificationInfo.GetCandidateEducationListForTLAllocation(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, department, statusid, teamMemerRowId, ClientRowID, CheckStatus,"",universityName,collegeName,CourseName,YrOfPassing);

                    }
                    else
                    {
                        model = repoVerificationInfo.GetCandidateEducationListForTMQCAllocationByTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, teamMemerRowId, ClientRowID, CheckStatus);

                    }
                }


                if (UnAllocatedCount > 0)
                {
                    ViewBag.ErrorMsg = UnAllocatedCount.ToString() + " Check(s) not allocated because teammember/teammemberQC not mapped with client!";
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        private void uploadFilesOnSFTP(string ClientRefID, string colorCode, string reportType, string filename, string sourcepath, int clientRowid)
        {
            try
            {
                // string   FldPathDoc = ConfigurationManager.AppSettings["CandidateReportdoc".Trim().ToLower()].ToString();
                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    string uploadpath = "/" + ClientRefID + "/Reports/";
                    //string uploadpath = "/Reports" + "/" + ClientRefID + "/";
                    if (!client.Exists(uploadpath))
                    {
                        client.CreateDirectory(uploadpath);
                    }
                    client.ChangeDirectory(uploadpath);
                    string sourcefile = ClientRefID + "_" + reportType + "-" + colorCode + ".pdf";
                    using (FileStream fs = new FileStream(sourcepath, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, System.IO.Path.GetFileName(sourcefile));
                        string remark = "file uploaded";
                        cc.SFTPFile_Insert(ClientRefID, uploadpath, sourcepath, remark);
                    }
                }
            }
            catch (Exception ex)
            {
                clsSendMail mail;
                mail = new clsSendMail();
                var signature = mail.GetCompanyMailSignature();
                string uploadpath = "/Reports" + "/" + ClientRefID + "/";
                cc.SFTPFile_Insert(ClientRefID, uploadpath, sourcepath, ex.Message);
                string Body = "<div>ClientRefID-" + ClientRefID.ToString() + "</div><br></br><div>uploadpath: " + uploadpath.ToString() + "</div><br></br><div>source path=" + sourcepath + "</div><br></br><div>Exception=" + ex.Message + "</div><br></br><div></div><br></br><div></div><br></br>";
                string From = signature.GeneralEmail;
                string DisplayName = signature.CompanyName.Trim();
                bool flag = mail.SendMail(DisplayName, From, "Ankit.pathak@securitas-india.com", "", "Ankit.pathak@securitas-india.com", "Sutherland SFTP File upload", Body);
            }
        }


        public ActionResult ReportToPDF(int personalrowid)
        {
            InterimReportGenerateViewModel model = new InterimReportGenerateViewModel();
            model.PersonalRowID = personalrowid;
            model.ReportTemplate = 20;
            try
            {

                Session["Designation"] = "";
                int id = personalrowid;
                string Report_Type = "Final";
                string BA = "";
                Session["Type"] = "ReprtToPDF";
                DataTable DT = new DataTable();
                string ReportStatus = "";
                cc = new ClientCustomize();
                //get resolution for client 
                //int resolutionDPI = cc.getResolutionForReport(8);
                model = repoReportQC.GenerateCandidateInterimReportByReportQCTM(id);
                CandidatePdfLists(id, 200, model.ClientRowID);                
                //CandidatePdfListsNew(id);
                // CandidateCasePdfListsNew(id);
                model.CandidateDocList = repoReportQC.GetCandidateDocListForReportArtifactWithApprove(id);
                CandidateCasePdfLists(id, 200);


                Session["SecritadRefId"] = model.CompanyRefNo;
                model.CandidateCaseDocList = repoReportQC.GetCandidateCaseDocListForReport(id);
                model.ReportDate = DateTime.Now;
                model.Report_Type = Report_Type;
                model.DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();

                //model.CandidateDocList = repoReportQC.GetCandidateDocListForReport(id);



                DT = cc.GetColorCode(model.PersonalRowID, model.ClientRowID);
                if (DT.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DT.Rows[0]["colorName"].ToString()))
                    {
                        model.FinalColor = DT.Rows[0]["colorName"].ToString();
                        model.FinalColorCode = DT.Rows[0]["colorCode"].ToString();
                    }
                    else
                    {
                        model.FinalColorCode = "#fff";
                    }
                }


                model.PersonalChecksBriefDetails = repoReportQC.SutherlandCriminalChecksBriefDetails_Report(id);

                foreach (var a in model.PersonalChecksBriefDetails)
                {
                    a.CheckCompleteDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                }




                //model.AddressCheckList = repoAddressVer.GetAddressInfoForTMQC(id);

                #region ***** All Checks *****


                model.AntecedentList = repoReportQC.GetAntecedentVerList(model.ClientRowID);


                var CriminalProvidedList = repoReportQC.GetProvidedCriminalForReportQC(id);
                var CriminalVerifiedList = repoReportQC.GetVerifiedCriminalForReportQC(id);
                foreach (var a in CriminalVerifiedList)
                {
                    if(a.ColorName.ToUpper()=="GREEN")
                    {
                        a.ColorCode = "#339933";
                    }
                    else if (a.ColorName == "RED")
                    {
                        a.ColorCode = "#ff0000";
                    }
                    else if (a.ColorName == "YELLOW")
                    {
                        a.ColorCode = "#ffff00";
                    }
                    else if (a.ColorName == "GREY")
                    {
                        a.ColorCode = "#999999";
                    }
                    else if (a.ColorName == "AMBER")
                    {
                        a.ColorCode = "#FF9933";
                    }


                    model.FinalColor = a.ColorName;
                        //model.FinalColorCode = "#999999";
                    
                }




                model.colorCodeOrderinglist = repoReportQC.getCheckColorCodeOrdering(id);





                CriminalReportViewModel CriminalList = new CriminalReportViewModel();
                CriminalReportViewModel CriminalVerList = new CriminalReportViewModel();


                foreach (var x in model.colorCodeOrderinglist)
                {
                    

                    CriminalList = CriminalProvidedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    CriminalVerList = CriminalVerifiedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    


                   
                    


                    
                    
                   
                   
                  

                }




                //MsgList.Where(x => !SentList.Any(y => y.MsgID == x.MsgID))


                model.CriminalProvidedList = CriminalProvidedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.CriminalVerifiedList = CriminalVerifiedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                //if (model.ClientRowID == 446)
                //{
                //    DataTable dtreportType = cc.GetReportType(id);
                //    if (dtreportType.Rows.Count > 0)
                //    {
                //        model.ReportTypeBYSystum = dtreportType.Rows[0]["ReportType"].ToString();
                //    }


                //}


                //BindColorCode(model.ClientRowID);


                #endregion
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View(model);
        }

        [NonAction]
        void BindColorCode(int ClientRowId)
        {
            try
            {
                ViewBag.Color = repoReportQC.getcolorcodeByClient(ClientRowId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public void CandidateCasePdfLists(int id, int dpi)
        {

            string DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
            var CandidatePdfList = repoReportQC.GetCandidateCasePdfListForReport(id);
            foreach (var item in CandidatePdfList)
            {
                SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
                f.Serial = "10273420380"; //License for server 
                //f.Serial = "10273316474"; //License for development machine
                f.OpenPdf(Server.MapPath(DocPath + item.DocFile));
                if (f.PageCount > 0)
                {
                    string jpgFileName = item.DocFile.Replace(".pdf", "");
                    string ImageName = string.Empty;
                    for (int i = 1; i <= f.PageCount; i++)
                    {
                        ImageName += jpgFileName + i + ".jpg,";
                    }
                    if (!string.IsNullOrEmpty(ImageName) && ImageName.Length > 0)
                    {
                        ImageName = ImageName.Substring(0, ImageName.Length - 1);
                        //bool res = repoReportQC.UpdateCaseImageName(item.CaseDocRowID, ImageName);
                        //if (res)
                        //{
                        //int width = 644; // Width in Px
                        //int height = 800; // Height in Px
                        f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        //f.ImageOptions.Resize(new System.Drawing.Size { Width = width, Height = height }, false);
                        f.ImageOptions.Dpi = dpi;
                        f.ToImage(Server.MapPath(DocPath), jpgFileName);
                        //}
                    }
                }
            }
        }

        public void CandidatePdfLists(int id, int dpi, int clientid)
        {
            string DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
            var CandidatePdfList = Enumerable.Empty<CandidateAllDocumentListViewModel>();
            if (clientid == 762 || clientid == 249 || clientid == 277)
            {
                CandidatePdfList = repoReportQC.GetCandidatePdfListForConversionWithOutApprove(id);
            }
            else
            {
                CandidatePdfList = repoReportQC.GetCandidatePdfListForConversion_spoc(id);
            }
            foreach (var item in CandidatePdfList)
            {
                SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
                f.Serial = "10273420380"; //License for server 
                //f.Serial = "10273316474"; //License for development machine
                f.OpenPdf(Server.MapPath(DocPath + item.DocFile));

                if (f.PageCount > 0)
                {
                    string jpgFileName = item.DocFile.Replace(".pdf", "");
                    string ImageName = string.Empty;
                    for (int i = 1; i <= f.PageCount; i++)
                    {
                        ImageName += jpgFileName + i + ".jpg,";
                    }
                    if (!string.IsNullOrEmpty(ImageName) && ImageName.Length > 0)
                    {
                        ImageName = ImageName.Substring(0, ImageName.Length - 1);
                        if (clientid == 762)
                        {
                            bool res = repoReportQC.UpdateImageName(item.VerifiedUploadRowID, ImageName);
                            if (res)
                            {
                                //int width = 644; // Width in Px
                                //int height = 911; // Height in Px
                                f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                                //f.ImageOptions.Resize(new System.Drawing.Size { Width = width, Height = height }, false);
                                f.ImageOptions.Dpi = dpi;
                                f.ToImage(Server.MapPath(DocPath), jpgFileName);
                            }
                            //cc.updateReportPDFToImagePages(item.VerifiedUploadRowID, ImageName);

                        }
                        else
                        {
                            //bool res = repoReportQC.UpdateImageName(item.VerifiedUploadRowID, ImageName);
                            // if (res)
                            //{
                            //int width = 644; // Width in Px
                            //int height = 911; // Height in Px
                            f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                            //f.ImageOptions.Resize(new System.Drawing.Size { Width = width, Height = height }, false);
                            f.ImageOptions.Dpi = dpi;
                            f.ToImage(Server.MapPath(DocPath), jpgFileName);
                            //
                            if ( clientid == 249 || clientid == 277)
                            {
                                cc.updateReportPDFToImagePagesSutherland(item.VerifiedUploadRowID, ImageName);
                            }
                            else {
                                cc.updateReportPDFToImagePages(item.VerifiedUploadRowID, ImageName);
                            }
                            
                        }
                    }
                }
            }
        }

        public InterimReportGenerateViewModel ConverdReportPDFFileToWord(int PersonalRowID, short ClientRowID)
        {
            InterimReportGenerateViewModel model = new InterimReportGenerateViewModel();
            try
            {

                string ReportToPDF = "Export to PDF";

                if (ReportToPDF == "Export to PDF")
                {
                    model = repoReportQC.GenerateCandidateInterimReportByReportQCTM(PersonalRowID);
                    //model.PersonalRowID = PersonalRowID;
                    model.Report_Type = "Final";
                    //model.Designation = Session["Designation"].ToString();
                    model.ClientRowID = ClientRowID;
                    var root = Server.MapPath("~/ExportPDF/");
                    var pdfname = string.Format("{0}.pdf", model.PersonalRowID.ToString());
                    var path = System.IO.Path.Combine(root, pdfname);
                    path = System.IO.Path.GetFullPath(path);

                    var pdfResult = new Rotativa.ActionAsPdf("ReportToPDF", model)
                    {
                        FileName = model.PersonalRowID + "_Report" + ".pdf",
                        SaveOnServerPath = path


                    };
                    byte[] data = pdfResult.BuildPdf(ControllerContext);
                    var CriminalProvidedList = repoReportQC.GetProvidedCriminalForReportQC(model.PersonalRowID);
                    var CriminalVerifiedList = repoReportQC.GetVerifiedCriminalForReportQC(model.PersonalRowID);
                    foreach (var a in CriminalVerifiedList)
                    {
                        
                            
                            model.FinalColor = a.ColorName;
                            
                        
                    }



                    //using (MemoryStream input = new MemoryStream(data))
                    //{
                    //    using (MemoryStream output = new MemoryStream())
                    //    {
                    //        if (model.ClientRowID == 113)
                    //        {
                    //            //string password = model.PersonalRowID.ToString();
                    //            string password = "mlbg8";
                    //            PdfReader reader = new PdfReader(input);
                    //            PdfEncryptor.Encrypt(reader, output, true, password, password, PdfWriter.ALLOW_SCREENREADERS);
                    //            data = output.ToArray();
                    //            Response.ContentType = "application/pdf";
                    //            Response.AddHeader("content-disposition", "attachment;filename=" + model.PersonalRowID + "_Report" + ".pdf");
                    //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //            Response.BinaryWrite(data);
                    //            Response.End();
                    //        }
                    //        else
                    //        {
                    //            PdfReader reader = new PdfReader(input);
                    //            data = output.ToArray();
                    //            Response.ContentType = "application/pdf";
                    //            Response.AppendHeader("content-disposition", "attachment; filename=" + pdfname);
                    //            Response.ContentType = "application/octet-stream";
                    //            Response.WriteFile(path);
                    //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //            Response.BinaryWrite(data);
                    //            Response.End();
                    //        }
                    //    }
                    //}
                    //return pdfResult;

                }
                //else
                //{
                //    SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
                //    f.Serial = "10273420380"; //License for server 
                //    f.OpenPdf(file.InputStream);
                //    byte[] rtf = null;

                //    if (f.PageCount > 0)
                //    {
                //        f.WordOptions.Format = SautinSoft.PdfFocus.CWordOptions.eWordDocument.Rtf;
                //        //Let's whole PDF document to Word (RTF)
                //        rtf = f.ToWord();
                //    }

                //    //show Word/rtf
                //    if (rtf != null)
                //    {
                //        Response.Buffer = true;
                //        Response.Clear();
                //        Response.ContentType = "application/msword";
                //        Response.AddHeader("Content-Disposition:", "attachment; filename=ReportInWordFormet.doc");
                //        Response.BinaryWrite(rtf);
                //        Response.Flush();
                //        Response.End();
                //    }
                //}
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //return View();
            return model;
        }

        private void uploadCriminalReportOnSFTP(string ClientRefID, string colorCode, string reportType, string filename, string sourcepath, int clientRowid)
        {
            try
            {

                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    client.Connect();

                    //  “2378702_CRC_FINAL_GREEN.pdf”.              
                    string uploadpath = "/" + ClientRefID + "/Reports/";
                    //string uploadpath = "/Reports" + "/" + ClientRefID + "/";
                    if (!client.Exists(uploadpath))
                    {
                        client.CreateDirectory(uploadpath);
                    }
                    client.ChangeDirectory(uploadpath);
                    string sourcefile = ClientRefID + "_CRC_" + reportType + "_" + colorCode.ToUpper() + ".pdf";
                    using (FileStream fs = new FileStream(sourcepath, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, System.IO.Path.GetFileName(sourcefile));
                        string remark = "file uploaded";
                        cc.SFTPFile_Insert(ClientRefID, uploadpath, sourcepath, remark);
                    }
                }




            }
            catch (Exception ex)
            {
                clsSendMail mail;
                mail = new clsSendMail();
                var signature = mail.GetCompanyMailSignature();
                string uploadpath = "/Reports" + "/" + ClientRefID + "/";
                cc.SFTPFile_Insert(ClientRefID, uploadpath, sourcepath, ex.Message);
                string Body = "<div>ClientRefID-" + ClientRefID.ToString() + "</div><br></br><div>uploadpath: " + uploadpath.ToString() + "</div><br></br><div>source path=" + sourcepath + "</div><br></br><div>Exception=" + ex.Message + "</div><br></br><div></div><br></br><div></div><br></br>";
                string From = signature.GeneralEmail;
                string DisplayName = signature.CompanyName.Trim();
                bool flag = mail.SendMail(DisplayName, From, "Ankit.pathak@securitas-india.com", "", "Ankit.pathak@securitas-india.com", "Sutherland SFTP Criminal Report Upload", Body);
            }
        }



    }

}

