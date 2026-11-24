using BAL;
using BAL.ClientRepository;
using BAL.HRRepository;
using BAL.ProvidedInfoRepository;
using BAL.VerificationRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;
using System.Data;
using ViewModels.ClientViewModel;



namespace WebAppBGV.Controllers
{
    [CustomAuthorize]
    public class InsuffController : Controller
    {
        #region ******  Repository Objects ******
        ClientCustomize cc;
        IClientServicingRepository repoDashboard;
        IDropDownMasterRepository repoDDL;
        IPQPersonalRepository repoPQPersonal;
        IPQClientRepository repoPQClient;
        IAbbreviationRepository repoAbbreviation;
        ILocationRepository repoLocation;
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
        IInsuranceInfoRepository repoInsuranceInfo;
        IHolidayRepository repoHoliday;
        IPQClientHolidayRepository repoPQClientHoliday;
        ISubCheckFamilyRepository repoSubCheckFamily;
        IPQVerifiedUploadDocRepository repoPQVerifiedUploadDoc;
        IPQClientCompnayUploadDocRepository repoClientCmpUplod;
        IEducationResearchRepository repoEducationResearch;
        IEmploymentResearchRepository repoEmploymentResearch;
        ICheckActionHistoryRepository repoCheckActionHistory;
        IVerificationInfoRepository repoVerificationInfo;
        ISpecialCheckInfoRepository repoSplCheckInfo;

        public InsuffController(
            IClientServicingRepository _repoDashboard,
            IDropDownMasterRepository _repoDDL
            , IPQPersonalRepository _repoPQPersonal
            , IPQClientRepository _repoPQClient
            , IAbbreviationRepository _repoAbbreviation
            , ILocationRepository _repoLocation
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
            , IInsuranceInfoRepository _repoInsuranceInfo
            , IHolidayRepository _repoHoliday
            , IPQClientHolidayRepository _repoPQClientHoliday
            , ISubCheckFamilyRepository _repoSubCheckFamily
            , IPQVerifiedUploadDocRepository _repoPQVerifiedUploadDoc
            , IPQClientCompnayUploadDocRepository _repoClientCmpUplod
            , IEducationResearchRepository _repoEducationResearch
            , IEmploymentResearchRepository _repoEmploymentResearch
            , ICheckActionHistoryRepository _repoCheckActionHistory
            , IVerificationInfoRepository _repoVerificationInfo
            , ISpecialCheckInfoRepository _repoSplCheckInfo
            )
        {
            repoDDL = _repoDDL;
            repoPQPersonal = _repoPQPersonal;
            repoPQClient = _repoPQClient;
            repoAbbreviation = _repoAbbreviation;
            repoLocation = _repoLocation;
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
            repoInsuranceInfo = _repoInsuranceInfo;
            repoHoliday = _repoHoliday;
            repoPQClientHoliday = _repoPQClientHoliday;
            repoSubCheckFamily = _repoSubCheckFamily;
            repoPQVerifiedUploadDoc = _repoPQVerifiedUploadDoc;
            repoClientCmpUplod = _repoClientCmpUplod;
            repoEducationResearch = _repoEducationResearch;
            repoEmploymentResearch = _repoEmploymentResearch;
            repoCheckActionHistory = _repoCheckActionHistory;
            repoVerificationInfo = _repoVerificationInfo;
            repoSplCheckInfo = _repoSplCheckInfo;
        }

        #endregion

        // GET: Insuff
        [SessionExpireFilterAttribute]
        public ActionResult Index()
        {
            CreateNavigationforInsuffCaseForDETL();
            return View();
        }

        #region ****** Address Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageAddressInsuff(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CheckType = "")
        {
            PQAddressInsuffListPagedModel model = new PQAddressInsuffListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(teamMemerRowId);

               // model = repoAddressInfo.GetAllAddressInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", teamMemerRowId);
                model = repoPQPersonal.GetAddressInsuffChecksDetailForDETL1(page, pageSize, Search, CheckType);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult AddressInsuffCheckClear(int id = 0)
        {
            UpdatePQAddressInsuffClearViewModel model = new UpdatePQAddressInsuffClearViewModel();
            try
            {
                if (id > 0)
                {
                    model = repoAddressInfo.GetPQAddressDetailsById(id);

                    var CandDetail = repoPQPersonal.GetCandidateNameOrCmpIDById(model.PersonalRowID);
                    Session["CandidateName"] = CandDetail.CandTitle + " " + CandDetail.Name + " " + CandDetail.MiddleName + " " + CandDetail.LastName;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddressInsuffCheckClear(UpdatePQAddressInsuffClearViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.AddressRowID > 0)
                    {
                        model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.ModifiedDate = DateTime.Now;
                        model.InfSuffClearBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.InfSuffClearDate = DateTime.Now;

                        repoAddressInfo.UpdatePQAddressInsuffClear(model);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "AddressInsuffCheckClear", "Insuff Rectified", model.UniqueComponentID, "Update");
                        #endregion
                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            //43 for L1 insuff
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "L1-DataEntry TL", model.InfSuffClearBy);//45 means L1 Insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.INFRemarks, "L1-DataEntry TL", model.InfSuffClearBy);
                        }
                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Insuff Rectified", model.INFRemarks);
                        #endregion

                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("Index");
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
            }
            return View(model);
        }
        #endregion

        #region ****** Employment Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageEmploymentInsuff(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "",string CheckType="")
        {
            PQEmploymentInsuffListPagedModel model = new PQEmploymentInsuffListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(teamMemerRowId);

                //model = repoEmploymentInfo.GetAllEmploymentInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", teamMemerRowId);
                model = repoPQPersonal.GetEmploymentInsuffChecksDetailForDETL1(page, pageSize, Search, CheckType);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult EmploymentInsuffCheckClear(int id = 0)
        {
            UpdatePQEmploymentInsuffClearViewModel model = new UpdatePQEmploymentInsuffClearViewModel();
            try
            {
                if (id > 0)
                {
                    model = repoEmploymentInfo.GetPQEmploymentDetailsById(id);

                    var CandDetail = repoPQPersonal.GetCandidateNameOrCmpIDById(model.PersonalRowID);
                    Session["CandidateName"] = CandDetail.CandTitle + " " + CandDetail.Name + " " + CandDetail.MiddleName + " " + CandDetail.LastName;
                }
                else
                {
                    throw new Exception("Invalid Id!");
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EmploymentInsuffCheckClear(UpdatePQEmploymentInsuffClearViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.EmploymentRowID > 0)
                    {
                        model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.ModifiedDate = DateTime.Now;
                        model.InfSuffClearBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.InfSuffClearDate = DateTime.Now;

                        repoEmploymentInfo.UpdatePQEmploymentInsuffClear(model);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "EmploymentInsuffCheckClear", "Insuff Rectified", model.UniqueComponentID, "Update");
                        #endregion
                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            //43 for L1 insuff
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "L1-DataEntry TL", model.InfSuffClearBy);//45 means L1 Insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.INFRemarks, "L1-DataEntry TL", model.InfSuffClearBy);
                        }
                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Insuff Rectified", model.INFRemarks);
                        #endregion

                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("Index");
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
            }
            return View(model);
        }
        #endregion

        #region ****** Education Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageEducationInsuff(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CheckType = "")
        {
            PQEducationInsuffListPagedModel model = new PQEducationInsuffListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(teamMemerRowId);

                //model = repoEducationInfo.GetAllEducationInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", teamMemerRowId);
                model = repoPQPersonal.GetEducationInsuffChecksDetailForDETL1(page, pageSize, Search, CheckType);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult EducationInsuffCheckClear(int id = 0)
        {
            UpdatePQEducationInsuffClearViewModel model = new UpdatePQEducationInsuffClearViewModel();
            try
            {
                if (id > 0)
                {
                    model = repoEducationInfo.GetPQEducationDetailsById(id);

                    var CandDetail = repoPQPersonal.GetCandidateNameOrCmpIDById(model.PersonalRowID);
                    Session["CandidateName"] = CandDetail.CandTitle + " " + CandDetail.Name + " " + CandDetail.MiddleName + " " + CandDetail.LastName;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EducationInsuffCheckClear(UpdatePQEducationInsuffClearViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.EducationRowID > 0)
                    {
                        model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.ModifiedDate = DateTime.Now;
                        model.InfSuffClearBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.InfSuffClearDate = DateTime.Now;

                        repoEducationInfo.UpdatePQEducationInsuffClear(model);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "EducationInsuffCheckClear", "Insuff Rectified", model.UniqueComponentID, "Update");
                        #endregion
                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            //43 for L1 insuff
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "L1-DataEntry TL", model.InfSuffClearBy);//45 means L1 Insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.INFRemarks, "L1-DataEntry TL", model.InfSuffClearBy);
                        }
                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Insuff Rectified", model.INFRemarks);
                        #endregion

                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("Index");
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
            }
            return View(model);
        }
        #endregion

        #region ****** Criminal Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageCriminalInsuff(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CheckType = "")
        {
            PQCriminalInsuffListPagedModel model = new PQCriminalInsuffListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(teamMemerRowId);

                // model = repoCriminalInfo.GetAllCriminalInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", teamMemerRowId);
                model = repoPQPersonal.GetCriminalInsuffChecksDetailForDETL1(page, pageSize, Search, CheckType);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult CriminalInsuffCheckClear(int id = 0)
        {
            UpdatePQCriminalInsuffClearViewModel model = new UpdatePQCriminalInsuffClearViewModel();
            try
            {
                if (id > 0)
                {
                    model = repoCriminalInfo.GetPQCriminalDetailsById(id);

                    var CandDetail = repoPQPersonal.GetCandidateNameOrCmpIDById(model.PersonalRowID);
                    Session["CandidateName"] = CandDetail.CandTitle + " " + CandDetail.Name + " " + CandDetail.MiddleName + " " + CandDetail.LastName;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CriminalInsuffCheckClear(UpdatePQCriminalInsuffClearViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.CriminalRowID > 0)
                    {
                        model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.ModifiedDate = DateTime.Now;
                        model.InfSuffClearBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.InfSuffClearDate = DateTime.Now;

                        repoCriminalInfo.UpdatePQCriminalInsuffClear(model);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "CriminalInsuffCheckClear", "Insuff Rectified", model.UniqueComponentID, "Update");
                        #endregion
                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            //43 for L1 insuff
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "L1-DataEntry TL", model.InfSuffClearBy);//45 means L1 Insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.INFRemarks, "L1-DataEntry TL", model.InfSuffClearBy);
                        }
                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Insuff Rectified", model.INFRemarks);
                        #endregion

                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("Index");
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
            }
            return View(model);
        }
        #endregion

        #region ****** NationalIdentity Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]

        public ActionResult ManageNationalIdentityInsuff(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CheckType = "")
        {
            PQNationalIdentityInsuffListPagedModel model = new PQNationalIdentityInsuffListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(teamMemerRowId);

                //model = repoNationalIdentInfo.GetAllNationalIdentityInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", teamMemerRowId);
                model = repoPQPersonal.GetNationalInsuffChecksDetailForDETL1(page, pageSize, Search, CheckType);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult NationalIdentityInsuffCheckClear(int id = 0)
        {
            UpdatePQNationalIdentityInsuffClearViewModel model = new UpdatePQNationalIdentityInsuffClearViewModel();
            try
            {
                if (id > 0)
                {
                    model = repoNationalIdentInfo.GetPQNationalIdentityDetailsById(id);

                    var CandDetail = repoPQPersonal.GetCandidateNameOrCmpIDById(model.PersonalRowID);
                    Session["CandidateName"] = CandDetail.CandTitle + " " + CandDetail.Name + " " + CandDetail.MiddleName + " " + CandDetail.LastName;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult NationalIdentityInsuffCheckClear(UpdatePQNationalIdentityInsuffClearViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.NationalIdentityRowID > 0)
                    {
                        model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.ModifiedDate = DateTime.Now;
                        model.InfSuffClearBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.InfSuffClearDate = DateTime.Now;

                        repoNationalIdentInfo.UpdatePQNationalIdentityInsuffClear(model);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "NationalIdentityInsuffCheckClear", "Insuff Rectified", model.UniqueComponentID, "Insert");
                        #endregion
                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            //43 for L1 insuff
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "L1-DataEntry TL", model.InfSuffClearBy);//45 means L1 Insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.INFRemarks, "L1-DataEntry TL", model.InfSuffClearBy);
                        }
                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Insuff Rectified", model.INFRemarks);
                        #endregion

                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("Index");
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
            }
            return View(model);
        }
        #endregion

        #region ****** Reference Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageReferenceInsuff(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CheckType = "")
        {
            PQReferenceInsuffListPagedModel model = new PQReferenceInsuffListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(teamMemerRowId);

                //model = repoReferenceInfo.GetAllReferenceInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", teamMemerRowId);
                model = repoPQPersonal.GetReferenceInsuffChecksDetailForDETL1(page, pageSize, Search, CheckType);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ReferenceInsuffCheckClear(int id = 0)
        {
            UpdatePQReferenceInsuffClearViewModel model = new UpdatePQReferenceInsuffClearViewModel();
            try
            {
                if (id > 0)
                {
                    model = repoReferenceInfo.GetPQReferenceDetailsById(id);

                    var CandDetail = repoPQPersonal.GetCandidateNameOrCmpIDById(model.PersonalRowID);
                    Session["CandidateName"] = CandDetail.CandTitle + " " + CandDetail.Name + " " + CandDetail.MiddleName + " " + CandDetail.LastName;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ReferenceInsuffCheckClear(UpdatePQReferenceInsuffClearViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.ReferenceRowID > 0)
                    {
                        model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.ModifiedDate = DateTime.Now;
                        model.InfSuffClearBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.InfSuffClearDate = DateTime.Now;

                        repoReferenceInfo.UpdatePQReferenceInsuffClear(model);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "ReferenceInsuffCheckClear", "Insuff Rectified", model.UniqueComponentID, "Update");
                        #endregion
                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            //43 for L1 insuff
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "L1-DataEntry TL", model.InfSuffClearBy);//45 means L1 Insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.INFRemarks, "L1-DataEntry TL", model.InfSuffClearBy);
                        }
                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Insuff Rectified", model.INFRemarks);
                        #endregion

                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("Index");
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
            }
            return View(model);
        }

        #endregion

        #region ****** Special Check Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageSpecialCheckInsuff(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CheckType = "")
        {
            PQSpecialCheckInsuffListPagedModel model = new PQSpecialCheckInsuffListPagedModel();
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(teamMemerRowId);

                //model = repoSplCheckInfo.GetAllSpecialCheckInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", teamMemerRowId);
                model = repoPQPersonal.GetSpecialInsuffChecksDetailForDETL1(page, pageSize, Search, CheckType);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult SpecialCheckCheckClear(int id = 0)
        {
            UpdatePQSpecialCheckInsuffClearViewModel model = new UpdatePQSpecialCheckInsuffClearViewModel();
            try
            {
                if (id > 0)
                {
                    model = repoSplCheckInfo.GetPQSpecialCheckDetailsById(id);

                    var CandDetail = repoPQPersonal.GetCandidateNameOrCmpIDById(model.PersonalRowID);
                    Session["CandidateName"] = CandDetail.CandTitle + " " + CandDetail.Name + " " + CandDetail.MiddleName + " " + CandDetail.LastName;
                }
                else
                {
                    throw new Exception("Invalid Id!");
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SpecialCheckCheckClear(UpdatePQSpecialCheckInsuffClearViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.SpecialCheckRowId > 0)
                    {
                        model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.ModifiedDate = DateTime.Now;
                        model.InfSuffClearBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.InfSuffClearDate = DateTime.Now;

                        repoSplCheckInfo.UpdatePQSpecialChackInsuffClear(model);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "SpecialChackInsuffCheckClear", "Insuff Rectified", model.UniqueComponentID, "Update");
                        #endregion
                        cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "L1-DataEntry TL", model.InfSuffClearBy);//45 means L1 Insuff
                        cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.INFRemarks, "L1-DataEntry TL", model.InfSuffClearBy);
                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Insuff Rectified", model.INFRemarks);
                        #endregion

                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("Index");
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
            }
            return View(model);
        }

        #endregion

        #region ***** Manage Insuff Checks For Upload Documents - DE-Scan *****

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageInsuffChecksForUploadDoc(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            BindClientListBaseOnLogin(teamMemerRowId);

            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                model = repoPQVerifiedUploadDoc.GetInsuffUploadedDocList(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, CheckStatus, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //BindClientList();
            return View(model);

        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult UploadInsuffDocumentForCheck(int id = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0, int PersonalRowID = 0, string CompanyRefNo = "", string rdpage = "")
        {
            UplodDocsForDEScanViewModel model = new UplodDocsForDEScanViewModel();
            if (model != null)
            {
                model.VerificationRowID = id;
                model.ClientRowID = ClientRowId;
                model.ChFamilRowId = ChFamilRowId;
                model.PersonalRowID = PersonalRowID;
                model.SubCheckRowID = subCheckId;
                model.SecuritasID = CompanyRefNo;
                model.RedirectPage = rdpage;

            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult UploadInsuffDocumentForCheck(UplodDocsForDEScanViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                if (model.PersonalRowID > 0 && !string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
                {
                    int count = 0;
                    string[] DocumentTypes = UploadHid.Split(',');
                    foreach (var item in UploadDoc)
                    {
                        model.FileName = item.FileName.ToString();
                        model.DocumentType = DocumentTypes[count];
                        if (model.DocumentType != null && model.FileName != null && item.ContentLength > 0)
                        {
                            SaveInsuffUploadDoc(model.VerificationRowID, model.PersonalRowID, model.SecuritasID, model.ChFamilRowId, model.SubCheckRowID, "Provided", model.DocumentType, "provideddocuments", item, model.Remarks, model.NADoc);
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "No item found!";
                            break;
                        }
                        count++;
                    }

                    #region Save LogTransaction Details
                    clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "UploadInsuffDocumentForCheck", "Insuff", model.DocumentType, "Uploaded");
                    #endregion

                    #region Case/Check Action History Save 
                    clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "WIP-Pending", "Insuff/Missing Documents Uploaded");
                    #endregion

                    int PID = model.PersonalRowID;
                    ViewBag.SuccessMsg = "Record Added Successfully!";
                    ModelState.Clear();

                    if (!string.IsNullOrEmpty(model.RedirectPage) && model.RedirectPage.Trim().ToLower() == "rejected")
                    {
                        return RedirectToAction("ManageRejectedChecksForDEScan");
                    }
                    else
                    {
                        return RedirectToAction("ManageInsuffChecksForUploadDoc");
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Please select at least one document!";
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

        #region ***** Manage Rejected Checks By Data Entry For DE-Scan *****

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageRejectedChecksForDEScan(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            BindClientListBaseOnLogin(teamMemerRowId);

            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                model = repoPQVerifiedUploadDoc.GetRejectedListForDEScan(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, CheckStatus, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //BindClientList();
            return View(model);

        }

        #endregion

        #region ***** Manage Review Requested Checks By Data Entry For DE TeamLeader *****

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageReviewRequestedChecksForDETL(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string CheckStatus = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            short teamMemberId = Convert.ToInt16(Session["TeamMemberRowID"]);
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                model = repoPQVerifiedUploadDoc.GetReviewRequestedListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, CheckStatus, teamMemberId);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientListBaseOnLogin(teamMemberId);
            return View(model);

        }


        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageReviewRequestedChecksForDepartmentTL(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string CheckStatus = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            short teamMemberId = Convert.ToInt16(Session["TeamMemberRowID"]);
           string designation = Convert.ToString(Session["Designation"]);
            string desig = "Manager-SPL,Team Leader-SPL,Team Member-SPL,Team Member-SPL-QC";
            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                int CheckFamilyRowID = 0;
                int CheckStatusID = 0;
                if (desig.Contains(designation))
                {
                    CheckFamilyRowID = 40;
                    CheckStatusID = 19;
                }
                //model = repoPQVerifiedUploadDoc.GetReviewRequestedListForDepartmentTL(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, CheckStatus, teamMemberId);
                model = repoPQVerifiedUploadDoc.GetReviewRequestedListForDepartmentTLNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, CheckStatusID, teamMemberId, CheckFamilyRowID);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            BindClientListBaseOnLogin(teamMemberId);
            return View(model);

        }
        #endregion

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageRejectedChecksByDepTMForDEQC(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0, string CheckStatus = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            short TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);

            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                model = repoPQVerifiedUploadDoc.GetRejectedChecksByDepTMForDEQC(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, ClientRowID, TeamMemberRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            BindClientListBaseOnLogin(TeamMemberRowID);
            return View(model);

        }


        #region ******* NonAction *******

        [NonAction]
        private void CreateNavigationforInsuffCaseForDETL()
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            //var AllChecks = repoPQPersonal.GetAllInsuffChecksCountForDETL(teamMemerRowId);
            var AllChecks = repoPQPersonal.GetAllInsuffChecksCountForDETL1();
            string NavTeamMemberDE = string.Empty;
            if (Session["Designation"].ToString() == "Team Leader-DE")
            {
                NavTeamMemberDE += "<li><a href='" + Url.Action("ManageAddressInsuff", "Insuff",new { CheckType = "Address Verification" }) + "'> Address (" + AllChecks.AddressCount + ") </a></li>";
                NavTeamMemberDE += "<li><a href='" + Url.Action("ManageEmploymentInsuff", "Insuff", new { CheckType = "Employment Verification" }) + "'> Employment (" + AllChecks.EmploymentCount + ") </a></li>";
                NavTeamMemberDE += "<li><a href='" + Url.Action("ManageEducationInsuff", "Insuff", new { CheckType = "Education Verification" }) + "'> Education (" + AllChecks.EducationCount + ") </a></li>";
                NavTeamMemberDE += "<li><a href='" + Url.Action("ManageCriminalInsuff", "Insuff", new { CheckType = "Criminal Record Verification" }) + "'> Criminal (" + AllChecks.CriminalCount + ") </a></li>";
                NavTeamMemberDE += "<li><a href='" + Url.Action("ManageNationalIdentityInsuff", "Insuff", new { CheckType = "National Identity Check" }) + "'> NationalIdentity (" + AllChecks.NatIdentityCount + ") </a></li>";
                NavTeamMemberDE += "<li><a href='" + Url.Action("ManageReferenceInsuff", "Insuff", new { CheckType = "Reference Verification" }) + "'> Reference (" + AllChecks.ReferenceCount + ") </a></li>";
                NavTeamMemberDE += "<li><a href='" + Url.Action("ManageSpecialcheckInsuff", "Insuff", new { CheckType = "Special Checks" }) + "'> Special Chack (" + AllChecks.SpecialcheckCount + ") </a></li>";
            }
            Session["NavLeftInsuffMenuDETL"] = NavTeamMemberDE;
        }

        [NonAction]
        void BindClientList()
        {
            ViewBag.ClientList = repoDDL.GetClientListWithAbbAndLocationForDDL(); ;
        }

        public void SaveInsuffUploadDoc(int VerificationRowID = 0, int PersonalRowID = 0, string secRefId = "", short ChFamilRowId = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "",
            string DocumentType = "", string FolderNameForSave = "", HttpPostedFileBase UploadDoc = null, string remarks = "", byte NADoc = 0)
        {
            try
            {               
                string FileName = (secRefId + "_" + SubCheckRowID.ToString() + " " + DocumentType).Replace(" ", "_").Replace("-", "_");
                PQAddVerifiedUploadDocViewModel DocModel = new PQAddVerifiedUploadDocViewModel();
                DocModel.VerifiedUploadRowID = VerificationRowID;
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.SubCheckRowID = SubCheckRowID;
                DocModel.CheckFamilyRowID = ChFamilRowId;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //  Provided / Verified
                DocModel.DocumentType = DocumentType; //Name of the selected docs
                if (NADoc == 0)
                {
                    string FldPathDoc = string.Empty;
                    FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();

                    string existFile = repoPQVerifiedUploadDoc.ExistFileByPersonalIdSubCheckIdDocType(PersonalRowID, SubCheckRowID, DocumentType);
                    if (!string.IsNullOrEmpty(existFile))
                    {
                        string path = Server.MapPath(FldPathDoc + existFile);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)//check file exsit or not
                        {
                            file.Delete();
                        }
                    }

                    string proImg = string.Empty;
                    string ext = Path.GetExtension(UploadDoc.FileName).ToLower();
                    proImg = FileName + ext;
                    UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                    DocModel.FileName = Path.GetFileName(proImg);
                    DocModel.FilePath = FldPathDoc;

                    //string FldPathDoc = string.Empty;
                    //FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();
                    //string proImg = string.Empty;
                    //string ext = Path.GetExtension(UploadDoc.FileName).ToLower();
                    //proImg = (secRefId + "_" + SubCheckRowID.ToString() + " " + DocumentType).Replace(" ", "_").Replace("-", "_") + ext;
                    //UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                    //DocModel.FileName = Path.GetFileName(proImg);
                    //DocModel.FilePath = FldPathDoc;
                }
                else
                {
                    DocModel.FileName = "NA";
                    DocModel.FilePath = "NA";
                }
                DocModel.UploadDate = DateTime.Now;
                DocModel.StatusDocxConvert = 0;
                DocModel.StatusPDF2DOCXConvert = 0;
                DocModel.PDF_FileName = "";
                DocModel.Status = 1;
                DocModel.Remarks = remarks;
                repoPQVerifiedUploadDoc.AdInsuffUploadDoc(DocModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        void BindCheckActionHistory(int PersonalRowID = 0, short subCheckId = 0)
        {
            try
            {
                ViewBag.CheckActionHistories = repoDDL.GetCheckActionHistory(PersonalRowID, subCheckId);
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
        #endregion

        //public ActionResult AddCheck(string Search = "", int page = 1)
        //{

        //    VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();

        //    try
        //    {
        //        int pageSize = clsGlobalCommon.PageSize;
        //        if (!string.IsNullOrEmpty(Search))
        //        {
        //            DataTable DT = new DataTable();
        //            cc = new ClientCustomize();
        //            DT = cc.GetCandidateChecks(Search);
        //            List<ClientDashboardCheckSummaryDetailViewModel> lmd = new List<ClientDashboardCheckSummaryDetailViewModel>();
        //            if (DT.Rows.Count > 0)
        //            {

        //                foreach (DataRow dr in DT.Rows) // loop for adding add from dataset to list<modeldata>
        //                {
        //                    lmd.Add(new ClientDashboardCheckSummaryDetailViewModel
        //                    {
        //                        //SecuritasID = (dr["CompanyRefNo"].ToString()),
        //                        //CheckName = (dr["SubCheckName"].ToString()),
        //                        //CheckStatus = dr["CheckStatus"].ToString(),
        //                        //CheckStatusID = (dr["CheckStatusID"].ToString()),
        //                        //OrderDate = (dr["orderDate"].ToString()),
        //                        //OutDate = (dr["outDate"].ToString()),
        //                        //CreatedDate = (dr["CreationDate"].ToString()),
        //                        //ColorName = dr["ColorName"].ToString(),

        //                    });

        //                }
        //                model.TotalRecords = DT.Rows.Count;
        //            }
        //            else
        //            {
        //                model.TotalRecords = 0;
        //            }
        //            //model.PageSize = pageSize;
        //            //model.ClientServiceChecks = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
        //            //return model;

        //        }
        //        else
        //        {
        //            TempData["ErrorMsg"] = "invlid id!";
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.ErrorMsg = Ex.Message;
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //   return View(model);

        //}
        // [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddCheck(string Search = "", int pageNo=1, int pageSize=100 , string SaveWork="")
        {


            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                if(Search=="")
                {
                    BindSubcheckname(0,0);
                }


                ClientServiceListViewModel model = new ClientServiceListViewModel();
                // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetCandidateChecks(Search);

                List<ClientDashboardCheckSummaryDetailViewModel> lmd = new List<ClientDashboardCheckSummaryDetailViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new ClientDashboardCheckSummaryDetailViewModel
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
                            Name= (dr["Name"].ToString()),

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                    BindSubcheckname(Convert.ToInt32(dt.Rows[0]["ClientRowID"]), Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));


                }


                



                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.ClientServiceChecks = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                
                    if ((SaveWork != "" && SaveWork == "Add") && (Request.Form["ddlColor"].ToString() != "0") && (dt.Rows[0]["PersonalRowID"].ToString() != ""))
                    {
                    DataTable dtcheck = new DataTable();
                    dtcheck = cc.CheckCasestatus(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                    if (dtcheck.Rows.Count>0)
                    {


                        int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);

                        cc.PQCandidateChecksAddCheck(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(dt.Rows[0]["ClientRowID"]), Convert.ToInt32(Request.Form["ddlColor"]));
                        //  return RedirectToAction("AddCheck",model);
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        string clientRowid = dt.Rows[0]["ClientRowID"].ToString();
                        if (clientRowid == "249" || clientRowid == "277")
                        {
                            new API().UpdateAPINewCaseInitiateStatus(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), clientRowid, "AddCheck");
                        }
                        DataTable DTO = new DataTable();
                        DTO = cc.GetPQClientChecksDetails(Convert.ToInt32(Request.Form["ddlColor"]));
                        if (dt.Rows.Count > 0)
                        {
                            int SubCheckRowID = Convert.ToInt32(DTO.Rows[0]["SubCheckRowID"]);
                            string designation = Convert.ToString(Session["Designation"]);
                            string username = Convert.ToString(Session["CRPUserName"]); 
                            clsCommonMethods.AddCheckActionHistory(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt16(SubCheckRowID),  "Add Check By" , designation + " : " + username);



                        }
                        return RedirectToAction("AddCheck", "Insuff", new { Search = Search });
                    }
                    else
                    {
                        
                        TempData["SuccessMsg"] = "You can not add check on Initiation stage";
                    }
                }
                    else
                    {
                        if(SaveWork == "Add")
                        {
                            
                        TempData["SuccessMsg"] = "Please fill detail";
                    }

                    }
                
                
                return View(model);
                
               

            }
            catch (Exception)
            {
                throw;
            }

        }
        
        void BindSubcheckname(int ClientRowId,int PersonalRowID)
        {
            try
            {
               
                ViewBag.chek = repoPQPersonal.getSubcheckdata(ClientRowId, PersonalRowID);
                
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

    }
}