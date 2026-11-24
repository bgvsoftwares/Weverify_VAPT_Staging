using BAL;
using BAL.ProvidedInfoRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProvidedInfoViewModel;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;

namespace WebAppBGV.Areas.Candidate.Controllers
{
    [CustomAuthorizeCandidate]
    public class CandidateInsuffController : Controller
    {
        #region ******  Repository Objects ******
        //IDropDownMasterRepository repoDDL;
        //IAbbreviationRepository repoAbbreviation;
        //IClientLoginRepository repoLogin;
        IPQPersonalRepository repoPQPersonal;
        IAddressInfoRepository repoAddressInfo;
        IEmploymentInfoRepository repoEmploymentInfo;
        IEducationInfoRepository repoEducationInfo;
        ICriminalInfoRepository repoCriminalInfo;
        INationalIdentityInfoRepository repoNationalIdentInfo;
        IReferenceInfoRepository repoReferenceInfo;
        //IInsuranceInfoRepository repoInsuranceInfo;
        ISubCheckFamilyRepository repoSubCheck;
        IPQVerifiedUploadDocRepository repoPQVerifiedUploadDoc;
        ISpecialCheckInfoRepository repoSplCheckInfo;
        public CandidateInsuffController(
             //  IDropDownMasterRepository _repoDDL
             //, IAbbreviationRepository _repoAbbreviation
             //, IClientLoginRepository _repoLogin
             IPQPersonalRepository _repoPQPersonal
            , IAddressInfoRepository _repoAddressInfo
            , IEmploymentInfoRepository _repoEmploymentInfo
            , IEducationInfoRepository _repoEducationInfo
            , ICriminalInfoRepository _repoCriminalInfo
            , INationalIdentityInfoRepository _repoNationalIdentInfo
            , IReferenceInfoRepository _repoReferenceInfo
            , ISubCheckFamilyRepository _repoSubCheck
            , IPQVerifiedUploadDocRepository _repoPQVerifiedUploadDoc
        , ISpecialCheckInfoRepository _repoSplCheckInfo
        )
        {
            //repoDDL = _repoDDL;
            //repoAbbreviation = _repoAbbreviation;
            //repoLogin = _repoLogin;
            repoPQPersonal = _repoPQPersonal;
            repoAddressInfo = _repoAddressInfo;
            repoEmploymentInfo = _repoEmploymentInfo;
            repoEducationInfo = _repoEducationInfo;
            repoCriminalInfo = _repoCriminalInfo;
            repoNationalIdentInfo = _repoNationalIdentInfo;
            repoReferenceInfo = _repoReferenceInfo;
            repoSubCheck = _repoSubCheck;
            repoPQVerifiedUploadDoc = _repoPQVerifiedUploadDoc;
            repoSplCheckInfo = _repoSplCheckInfo;
        }
        #endregion

        // GET: Candidate/CandidateInsuff
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult Index()
        {
            CreateNavigationforInsuffCaseForDETL();
            return View();
        }

        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult InsufficiencyCheckClearByCandidate(int id = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0, int PersonalRowID = 0, string CompanyRefNo = "", string InfRemarks = "")
        {
            InsuffCheckClearViewModel model = new InsuffCheckClearViewModel();
            if (model != null)
            {
                model.CheckRowID = id;
                model.ClientRowID = ClientRowId;
                model.ChFamilRowId = ChFamilRowId;
                model.PersonalRowID = PersonalRowID;
                model.SubCheckRowID = subCheckId;
                model.SecuritasID = CompanyRefNo;
                model.INFReason = InfRemarks;
                model.SubCheckName = repoSubCheck.GetSubCheckFamilyForUpdateById(subCheckId).SubCheckName;
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult InsufficiencyCheckClearByCandidate(InsuffCheckClearViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.PersonalRowID > 0 && ((!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null) || (!string.IsNullOrEmpty(model.Remarks))))
                    {
                        int count = 0;
                        string[] DocumentTypes = UploadHid.Split(',');
                        foreach (var item in UploadDoc)
                        {
                            model.FileName = item.FileName.ToString();
                            model.DocumentType = DocumentTypes[count];
                            if (model.DocumentType != null && model.FileName != null && item.ContentLength > 0)
                            {
                                SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "Provided", model.DocumentType, "provideddocuments", item, model.Remarks, 0, model.SecuritasID);
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "Invalid Document!";
                                break;
                            }
                            count++;
                        }

                        if (string.IsNullOrEmpty(model.Remarks))
                        {
                            model.Remarks = "Insuff Document Uploaded By Candidate";
                        }
                        repoPQPersonal.UpdateInsuffCheckByPeraonalIdAndSubCheckID(model.ChFamilRowId, model.CheckRowID, model.Remarks);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasactionByCandidate(model.PersonalRowID, "InsufficiencyCheckClearByCandidate", "Insuff Clear By Candidate", model.DocumentType, "Insert");
                        #endregion

                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistoryByCandidate(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Candidate", model.Remarks);
                        #endregion

                        int PID = model.PersonalRowID;
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Please select at least one document or mention remarks!";
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
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        #region ****** Address Insuff Cases ******

        [SessionExpireFilterAttributeCandidate]
        public ActionResult ManageAddressInsuffForCandidate(int page = 1, string sort = "", string sortDir = "asc")
        {
            PQAddressInsuffListPagedModel model = new PQAddressInsuffListPagedModel();
            try
            {
                int pageSize = 20;
                int PersonalId = Convert.ToInt32(Session["TempPersonalRowID"]);
                model = repoAddressInfo.GetAllAddressInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), "", 0, "", "", "", "", PersonalId);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
            }
            return View(model);
        }

        #endregion

        #region ****** Employment Insuff Cases ******

        [SessionExpireFilterAttributeCandidate]
        public ActionResult ManageEmploymentInsuffForCandidate(int page = 1, string sort = "", string sortDir = "asc")
        {
            PQEmploymentInsuffListPagedModel model = new PQEmploymentInsuffListPagedModel();
            try
            {
                int pageSize = 20;
                int PersonalId = Convert.ToInt32(Session["TempPersonalRowID"]);
                model = repoEmploymentInfo.GetAllEmploymentInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), "", 0, "", "", "", "", PersonalId);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
            }
            return View(model);
        }

        #endregion

        #region ****** Education Insuff Cases ******

        [SessionExpireFilterAttributeCandidate]
        public ActionResult ManageEducationInsuffForCandidate(int page = 1, string sort = "", string sortDir = "asc")
        {
            PQEducationInsuffListPagedModel model = new PQEducationInsuffListPagedModel();
            try
            {
                int pageSize = 20;
                int PersonalId = Convert.ToInt32(Session["TempPersonalRowID"]);

                model = repoEducationInfo.GetAllEducationInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), "", 0, "", "", "", "", PersonalId);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
            }
            return View(model);
        }

        #endregion

        #region ****** Criminal Insuff Cases ******

        [SessionExpireFilterAttributeCandidate]
        public ActionResult ManageCriminalInsuffForCandidate(int page = 1, string sort = "", string sortDir = "asc")
        {
            PQCriminalInsuffListPagedModel model = new PQCriminalInsuffListPagedModel();
            try
            {
                int pageSize = 20;
                int PersonalId = Convert.ToInt32(Session["TempPersonalRowID"]);

                model = repoCriminalInfo.GetAllCriminalInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), "", 0, "", "", "", "", PersonalId);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
            }
            return View(model);
        }

        #endregion

        #region ****** NationalIdentity Insuff Cases ******

        [SessionExpireFilterAttributeCandidate]
        public ActionResult ManageNationalIdentityInsuffForCandidate(int page = 1, string sort = "", string sortDir = "asc")
        {
            PQNationalIdentityInsuffListPagedModel model = new PQNationalIdentityInsuffListPagedModel();
            try
            {
                int pageSize = 20;
                int PersonalId = Convert.ToInt32(Session["TempPersonalRowID"]);

                model = repoNationalIdentInfo.GetAllNationalIdentityInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), "", 0, "", "", "", "", PersonalId);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
            }
            return View(model);
        }

        #endregion

        #region ****** Reference Insuff Cases ******

        [SessionExpireFilterAttributeCandidate]
        public ActionResult ManageReferenceInsuffForCandidate(int page = 1, string sort = "", string sortDir = "asc")
        {
            PQReferenceInsuffListPagedModel model = new PQReferenceInsuffListPagedModel();
            try
            {
                int pageSize = 20;
                int PersonalId = Convert.ToInt32(Session["TempPersonalRowID"]);
                model = repoReferenceInfo.GetAllReferenceInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), "", 0, "", "", "", "", PersonalId);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
            }
            return View(model);
        }

        #endregion

        #region ****** SpecialChack Insuff Cases ******

        [SessionExpireFilterAttributeCandidate]
        public ActionResult ManageSpecialChackInsuffForCandidate(int page = 1, string sort = "", string sortDir = "asc")
        {
            PQSpecialCheckInsuffListPagedModel model = new PQSpecialCheckInsuffListPagedModel();
            try
            {
                int pageSize = 20;
                int PersonalId = Convert.ToInt32(Session["TempPersonalRowID"]);
                model = repoSplCheckInfo.GetAllSpecialCheckInsuffListForDETL(page, pageSize, sort.ToLower(), sortDir.ToLower(), "", 0, "", "", "", "", PersonalId);

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
            }
            return View(model);
        }

        #endregion

        #region ****** NonAction ******

        [NonAction]
        private void CreateNavigationforInsuffCaseForDETL()
        {
            int PersonalId = Convert.ToInt32(Session["TempPersonalRowID"]);
            var AllChecks = repoPQPersonal.GetAllInsuffChecksCountForCandidate(PersonalId);
            string NavTeamMemberDE = string.Empty;
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageAddressInsuffForCandidate", "CandidateInsuff") + "'> Address (" + AllChecks.AddressCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageEmploymentInsuffForCandidate", "CandidateInsuff") + "'> Employment (" + AllChecks.EmploymentCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageEducationInsuffForCandidate", "CandidateInsuff") + "'> Education (" + AllChecks.EducationCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageCriminalInsuffForCandidate", "CandidateInsuff") + "'> Criminal (" + AllChecks.CriminalCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageNationalIdentityInsuffForCandidate", "CandidateInsuff") + "'> NationalIdentity (" + AllChecks.NatIdentityCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageReferenceInsuffForCandidate", "CandidateInsuff") + "'> Reference (" + AllChecks.ReferenceCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageSpecialChackInsuffForCandidate", "CandidateInsuff") + "'> Specialcheck (" + AllChecks.SpecialcheckCount + ") </a></li>";

            Session["NavLeftInsuffMenuForCandidate"] = NavTeamMemberDE;
        }

        public void SavePQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", HttpPostedFileBase UploadDoc = null, string remarks = "", byte NADoc = 0, string secRefId = "")
        {
            try
            {
                //string secRefId = Session["SecuritasRefID"].ToString();
                PQAddVerifiedUploadDocViewModel DocModel = new PQAddVerifiedUploadDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.SubCheckRowID = SubCheckRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //  Provided / Verified
                DocModel.DocumentType = DocumentType;            //Name of the selected docs
                if (NADoc == 0)
                {
                    string FldPathDoc = string.Empty;
                    FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();
                    string proImg = string.Empty;
                    string ext = Path.GetExtension(UploadDoc.FileName).ToLower();
                    proImg = (secRefId + "_" + SubCheckRowID.ToString() + " " + DocumentType).Replace(" ", "_").Replace("-", "_") + ext;
                    UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                    DocModel.FileName = Path.GetFileName(proImg);
                    DocModel.FilePath = FldPathDoc;
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
                DocModel.InsuffDoc = 1;
                DocModel.RptArtiFacts = 0;
                repoPQVerifiedUploadDoc.AddPQVerifiedUploadDoc(DocModel);
                repoPQVerifiedUploadDoc.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}