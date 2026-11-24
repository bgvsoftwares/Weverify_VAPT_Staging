using BAL;
using BAL.CandidateRepository;
using BAL.ClientRepository;
using BAL.OCR;
using BAL.Perfios;
using BAL.ProvidedInfoRepository;
using DAL;
using DocumentFormat.OpenXml.EMMA;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.X509.SigI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using Unity.Utility;
using ViewModels;
using ViewModels.ClientViewModel;
using ViewModels.Perfios;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;
using WebAppBGV.Areas.ClientArea.Models;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;
using static BAL.WhatsAppAPI;
using static System.Net.WebRequestMethods;
using static WebAppBGV.CommonMethods.EmailSmartping;
using Image = System.Drawing.Image;
using Rectangle = iTextSharp.text.Rectangle;
using iTextSharp.text.pdf.security;
using com.itextpdf.text.pdf.security;
using System.Security.Cryptography;
using eSignProject.Helpers;
using Org.BouncyCastle.X509;

namespace WebAppBGV.Areas.Candidate.Controllers
{
    [CustomAuthorizeCandidate]
    public class HomeController : Controller
    {
        private readonly EsignXmlSigner _esignXmlSigner;
        private const string SignatureFieldName = "Signature1";



        #region ******  Repository Objects ******
        IDropDownMasterRepository repoDDL;
        IPQClientRepository repoPQClient;
        IAbbreviationRepository repoAbbreviation;
        IClientLoginRepository repoLogin;
        IClientCheckRepository repoClientCheck;
        IClientPackageRepository repoClientPackage;
        IPQCandidateCheckRepository repoPQCandidateCheck;
        IPQPersonalRepository repoPQPersonal;
        IPQClientHolidayRepository repoPQClientHoliday;
        IAddressInfoRepository repoAddressInfo;
        IEmploymentInfoRepository repoEmploymentInfo;
        IEducationInfoRepository repoEducationInfo;
        ICriminalInfoRepository repoCriminalInfo;
        INationalIdentityInfoRepository repoNationalIdentInfo;
        IReferenceInfoRepository repoReferenceInfo;
        //IInsuranceInfoRepository repoInsuranceInfo;
        ICheckTempCandidateCaseStatusRepository repoTempCandCaseStatus;
        IPQVerifiedUploadDocRepository repoPQVerifiedUploadDoc;
        ISubCheckFamilyRepository repoSubCheckFamily;
        ISpecialCheckInfoRepository repoSplCheckInfo;
        IPQCandidateLoginRepository repoCandidate;
        ICompanyRepository repoCompany;


        public HomeController(
              IDropDownMasterRepository _repoDDL
            , IPQClientRepository _repoPQClient
        , IAbbreviationRepository _repoAbbreviation
        , IClientLoginRepository _repoLogin
        , IClientCheckRepository _repoClientCheck
        , IPQCandidateCheckRepository _repoPQCandidateCheck
        , IClientPackageRepository _repoClientPackage
        , IPQPersonalRepository _repoPQPersonal
        , IPQClientHolidayRepository _repoPQClientHoliday
            , IAddressInfoRepository _repoAddressInfo
            , IEmploymentInfoRepository _repoEmploymentInfo
            , IEducationInfoRepository _repoEducationInfo
            , ICriminalInfoRepository _repoCriminalInfo
            , INationalIdentityInfoRepository _repoNationalIdentInfo
            , IReferenceInfoRepository _repoReferenceInfo
            //, IInsuranceInfoRepository _repoInsuranceInfo
            , ICheckTempCandidateCaseStatusRepository _repoTempCandCaseStatus
            , IPQVerifiedUploadDocRepository _repoPQVerifiedUploadDoc
        , ISubCheckFamilyRepository _repoSubCheckFamily
        , ISpecialCheckInfoRepository _repoSplCheckInfo
            , IPQCandidateLoginRepository _repoCandidate
            , ICompanyRepository _repoCompany
        )
        {
            repoDDL = _repoDDL;
            repoPQClient = _repoPQClient;
            repoAbbreviation = _repoAbbreviation;
            repoLogin = _repoLogin;
            repoClientCheck = _repoClientCheck;
            repoPQCandidateCheck = _repoPQCandidateCheck;
            repoClientPackage = _repoClientPackage;
            repoPQPersonal = _repoPQPersonal;
            repoPQClientHoliday = _repoPQClientHoliday;
            repoAddressInfo = _repoAddressInfo;
            repoEmploymentInfo = _repoEmploymentInfo;
            repoEducationInfo = _repoEducationInfo;
            repoCriminalInfo = _repoCriminalInfo;
            repoNationalIdentInfo = _repoNationalIdentInfo;
            repoReferenceInfo = _repoReferenceInfo;
            repoTempCandCaseStatus = _repoTempCandCaseStatus;
            repoPQVerifiedUploadDoc = _repoPQVerifiedUploadDoc;
            repoSubCheckFamily = _repoSubCheckFamily;
            repoSplCheckInfo = _repoSplCheckInfo;
            repoCandidate = _repoCandidate;
            repoCompany = _repoCompany;
        }

        ClientCustomize bl = new ClientCustomize();
        TempPQCandidateCheck tpc = new TempPQCandidateCheck();
        #endregion

        // GET: Candidate/Home
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult Index()
        {
            if (Session["CInsufficiency"] != null && Session["CInsufficiency"].ToString() == "YES")
            {
                return RedirectToAction("Index", "CandidateInsuff");
            }
            Int64 PersonalRowID = Convert.ToInt64(Session["TempPersonalRowID"]);
            int PersonalRowID1 = Convert.ToInt32(Session["TempPersonalRowID"]);

            DataTable dt = bl.GetAgreementdata(PersonalRowID);
            List<string> docTable = new List<string>();

            //string[]  docTable=new string[7];
            int c = 0;
            foreach (DataRow d in dt.Rows)
            {
                if (d["SubCheckFamilyID"].ToString() == "2")
                {
                    c++;
                    Session["SubCheckFamilyIDforcheck"] = "true";
                }
                else if (c == 0)
                {
                    Session["SubCheckFamilyIDforcheck"] = "false";
                }
                docTable.Add(d["CheckFamilyRowID"].ToString());
            }
            ViewBag.CheckFamily = docTable;

            CreateMenuitemforDataEntryOperator(PersonalRowID);
            CreateMenuitemforDataEntrySubmit(PersonalRowID);
            int ClientRowID = Convert.ToInt32(Session["ClientRowID"].ToString());
            var CandDetail = repoPQPersonal.GetTempPQPersonalInfoForSession(PersonalRowID1);
            ClientCustomize cc = new ClientCustomize();

            ViewBag.BGVInstructionslist = repoPQPersonal.BGVInstructionslist(PersonalRowID1, ClientRowID);
            string ssnCandidateName = CandDetail.Name.Trim();
            if (!string.IsNullOrEmpty(CandDetail.MiddleName))
            {
                ssnCandidateName += " " + CandDetail.MiddleName.Trim();
            }
            if (!string.IsNullOrEmpty(CandDetail.LastName))
            {
                ssnCandidateName += " " + CandDetail.LastName.Trim();
            }

            Session["TCandidateName"] = ssnCandidateName;
            Session["TSecuritasRefID"] = CandDetail.CompanyRefNo;

            Session["TscnCompanyRefNo"] = CandDetail.CompanyRefNo;
            Session["TscnCandidateCode"] = CandDetail.CandidateCode;
            Session["TscnCandTitle"] = CandDetail.CandTitle;
            Session["TscnName"] = CandDetail.Name;

            return View();
        }

        [ValidateSessionFilterCandidate]
        public ActionResult ReIndex()
        {

            Int64 PersonalRowID = Convert.ToInt64(Session["TempPersonalRowID"]);
            int PersonalRowID1 = Convert.ToInt32(Session["TempPersonalRowID"]);
            DataTable dt = bl.GetAgreementdata(PersonalRowID);
            List<string> docTable = new List<string>();
            foreach (DataRow d in dt.Rows)
            {
                docTable.Add(d["CheckFamilyRowID"].ToString());
            }
            ViewBag.CheckFamily = docTable;
            CreateMenuitemforCandidate(PersonalRowID);
            var CandDetail = repoPQPersonal.GetTempPQPersonalInfoForSession(PersonalRowID1);
            string ssnCandidateName = CandDetail.Name.Trim();

            if (!string.IsNullOrEmpty(CandDetail.MiddleName))
            {
                ssnCandidateName += " " + CandDetail.MiddleName.Trim();
            }

            if (!string.IsNullOrEmpty(CandDetail.LastName))
            {
                ssnCandidateName += " " + CandDetail.LastName.Trim();
            }

            Session["TCandidateName"] = ssnCandidateName;

            Session["TSecuritasRefID"] = CandDetail.CompanyRefNo;
            Session["TscnCompanyRefNo"] = CandDetail.CompanyRefNo;

            Session["TscnCandidateCode"] = CandDetail.CandidateCode;
            Session["TscnCandTitle"] = CandDetail.CandTitle;
            Session["TscnName"] = CandDetail.Name;
            ViewBag.PersonalRowID = PersonalRowID1.ToString();
            return View();
        }


        #region ***** Document Upload *****

        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult ManageCandidateUploadDocmentList(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short SubCheckRowID = 0)
        {
            TempPQVerifiedUploadDocListPagedModel model = new TempPQVerifiedUploadDocListPagedModel();
            try
            {
                int PersonalRowID = Convert.ToInt32(Session["TempPersonalRowID"]);

                int pageSize = 20;
                GetAllDocUploadedCheckList(PersonalRowID);
                GetAddressProofDocs();
                model = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedList(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, SubCheckRowID, PersonalRowID);
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }



        // Updated 18-01-2018 Rahul

        [SessionExpireFilterAttributeCandidate]
        public ActionResult ManageCandidateUploadDocmentListUpdated(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short SubCheckRowID = 0)
        {
            TempPQVerifiedUploadDocListPagedModel model = new TempPQVerifiedUploadDocListPagedModel();
            try
            {
                int PersonalRowID = Convert.ToInt32(Session["TempPersonalRowID"]);

                int pageSize = 20;
                GetAllDocUploadedCheckList(PersonalRowID);
                GetAddressProofDocs();
                model = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedList(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, SubCheckRowID, PersonalRowID);
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        // Updated 18-01-2018 End








        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult UploadCandidateDocument()
        {
            AddTempPQVerifiedUploadDocViewModel model = new AddTempPQVerifiedUploadDocViewModel();
            try
            {
                model.PersonalRowID = Convert.ToInt32(Session["TempPersonalRowID"]);
                GetAllCheckListforDocUpload(model.PersonalRowID);
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        // Updated Rahul 17-01-2018
        // Updated Harsh 18-08-2020


        public ActionResult backtoAddCriminalInfo(string PersonalRowIDs = "", string ChFamilRowIds = "", string subCheckIds = "", string clientId = "")
        {
            //int dePersonalId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowIDs.ToString()));
            int deChFamId = Convert.ToInt32(clsCommonMethods.PDecrypt(ChFamilRowIds.ToString()));
            int dEsubcheckid = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckIds.ToString()));
            //int deClientId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientId.ToString()));
            if (deChFamId != 0)
            {
                if (deChFamId > 0)
                {
                    if (deChFamId == 2)
                    {
                        return RedirectToAction("AddAddressInfo", new { PersonalRowID = PersonalRowIDs, ChFamilRowId = ChFamilRowIds, subCheckId = subCheckIds, ClientRowId = clientId });
                    }
                    if (deChFamId == 5)
                    {
                        return RedirectToAction("AddCriminalInfo", new { PersonalRowID = PersonalRowIDs, ChFamilRowId = ChFamilRowIds, subCheckId = subCheckIds, ClientRowId = clientId });
                    }
                    if (deChFamId == 11)
                    {
                        return RedirectToAction("AddEducationInfo", new { PersonalRowID = PersonalRowIDs, ChFamilRowId = ChFamilRowIds, subCheckId = subCheckIds, ClientRowId = clientId });
                    }
                    if (deChFamId == 12)
                    {
                        return RedirectToAction("AddEmploymentInfo", new { PersonalRowID = PersonalRowIDs, ChFamilRowId = ChFamilRowIds, subCheckId = subCheckIds, ClientRowId = clientId });
                    }
                    if (deChFamId == 26)
                    {
                        return RedirectToAction("AddNationalIdentityInfo", new { PersonalRowID = PersonalRowIDs, ChFamilRowId = ChFamilRowIds, subCheckId = subCheckIds, ClientRowId = clientId });
                    }
                    if (deChFamId == 30)
                    {
                        return RedirectToAction("AddReferenceInfo", new { PersonalRowID = PersonalRowIDs, ChFamilRowId = ChFamilRowIds, subCheckId = subCheckIds, ClientRowId = clientId });
                    }
                    if (deChFamId == 40)
                    {
                        return RedirectToAction("AddSpecialCheckInfo", new { PersonalRowID = PersonalRowIDs, ChFamilRowId = ChFamilRowIds, subCheckId = subCheckIds, ClientRowId = clientId });
                    }
                }
            }
            return View("Index");
        }

        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult UploadCandidateDocumentUpdated(int page = 1, string sort = "", string sortDir = "asc", string Search = "", string subCheckId = "", short checkId = 0, short mainCheckId = 0, short clientRowId = 0, string msg = "")
        {
            AddTempPQVerifiedUploadDocViewModel model = new AddTempPQVerifiedUploadDocViewModel();
            try
            {
                if (clsCommonMethods.PDecrypt(msg) != "")
                {
                    if (msg == "Document Can not be Blank !!")
                    {
                        ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
                    }
                    else if (msg == "Please check file name !!")
                    {
                        ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
                    }
                    else if (msg == "Document Uploaded Successfully !!")
                    {
                        ViewBag.Success = clsCommonMethods.PDecrypt(msg);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
                    }
                }

                model.PersonalRowID = Convert.ToInt32(Session["TempPersonalRowID"]);
                model.SubCheckRowID = Int16.Parse(clsCommonMethods.PDecrypt(subCheckId));

                model.CheckFamilyRowID = mainCheckId;
                //model.ClientRowID = clientRowId;
                int PersonalRowID = Convert.ToInt32(Session["TempPersonalRowID"]);
                // Updated by harsh 18-08-2020
                string UploadHeaderName = string.Empty;
                if (checkId == 0)
                {
                    DataSet dd = new DataSet();
                    dd = bl.getDetailsForDocUpload(model.SubCheckRowID, PersonalRowID);
                    checkId = short.Parse(dd.Tables[0].Rows[0]["checkfamilyrowid"].ToString());
                    clientRowId = short.Parse(dd.Tables[0].Rows[0]["clientrowid"].ToString());
                    UploadHeaderName = Convert.ToString(dd.Tables[0].Rows[0]["SubCheckName"].ToString());
                    model.ClientRowID = clientRowId;
                }

                ViewBag.UploadPageHeader = UploadHeaderName;
                model.CheckFamilyRowID = checkId;
                // int pageSize = 20;
                // GetAllCheckListforDocUpload(model.PersonalRowID);
                ViewBag.GetDocumentListCandidate = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(model.SubCheckRowID, PersonalRowID);
                if (ViewBag.GetDocumentListCandidate.Count > 0)
                {
                    bl.updateCheckStatus_Candidate_temp(PersonalRowID, model.SubCheckRowID, "Completed", "Docs");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(PersonalRowID, model.SubCheckRowID, "Pending", "Docs");
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

        [NonAction]
        public string GatZipFileMessage(HttpPostedFileBase postedFile, string clientRowid, string filenm)
        {
            string NowTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            GetMimeTypeTrue MimeType = new GetMimeTypeTrue();
            string ErrorMessage = "";
            //string StartupPath = Server.MapPath("~/Uploads/TempFile/");
            string StartupPath = ConfigurationManager.AppSettings["UploadsTempAPI"].ToString();
            StartupPath += clientRowid + "_" + filenm + "_" + NowTime;
            // string TempPath = Server.MapPath("~/Uploads/");
            string TempPath = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
            if (!Directory.Exists(StartupPath + "/"))
            {
                Directory.CreateDirectory(StartupPath);
            }
            ValidateParentFileUploadedByCandidate obj = new ValidateParentFileUploadedByCandidate();
            string ext = Path.GetExtension(postedFile.FileName);
            string filename = clientRowid + "_" + filenm + "_" + NowTime + ext;
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                postedFile.InputStream.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            string base64String = Convert.ToBase64String(fileBytes);
            string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(Path.GetFileName(filename), TempPath, base64String);
            //postedFile.SaveAs(TempPath + Path.GetFileName(filename));
            if (postedFile != null)
            {
                if (postedFile.ContentLength > 0)
                {
                    string[] AllowedExtension = new string[] { ".ZIP", ".zip" };
                    string strbase64date = FileSaveDownloadAPI.DocAndReportDownloadDynamic(Path.GetFileName(filename), TempPath);
                    byte[] bytes = Convert.FromBase64String(strbase64date);
                    //var bytes = System.IO.File.ReadAllBytes(TempPath + Path.GetFileName(filename));
                    if (MimeType.GetMimeType(bytes, TempPath + Path.GetFileName(filename)) == false)
                    {
                        ErrorMessage = "Only,.doc,.xls,.pdf,Image and Zip File allowed";
                    }
                    else
                    {
                        if (!obj.IsValid(TempPath + Path.GetFileName(filename)))
                        {
                            ErrorMessage = obj.ErrorMessage;
                        }
                        if (ErrorMessage == "")
                        {
                            if (AllowedExtension.Contains(ext))
                            {
                                ZipFile.ExtractToDirectory(TempPath + Path.GetFileName(filename), StartupPath);
                                using (ZipArchive zipArchive = ZipFile.OpenRead(TempPath + Path.GetFileName(filename)))
                                {
                                    foreach (ZipArchiveEntry item in zipArchive.Entries)
                                    {
                                        ValidateChildFileUploadedByCandidate obj1 = new ValidateChildFileUploadedByCandidate();
                                        string[] Extension = item.Name.ToString().Split('.');
                                        string filePath = StartupPath + "/" + item.FullName;
                                        if (Extension.Length == 2 || Extension.Length == 3)
                                        {
                                            bytes = System.IO.File.ReadAllBytes(filePath);
                                            if (MimeType.GetMimeType(bytes, filePath) == false)
                                            {
                                                ErrorMessage = "Only,.doc,.pdf,Image files allowed in Zip File ";
                                            }
                                            if (!obj1.IsValid(filePath))
                                            {
                                                ErrorMessage = obj1.ErrorMessage;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

            //FileInfo file1 = new FileInfo(TempPath + Path.GetFileName(filename));
            //if (file1.Exists)
            //{
            //    file1.Delete();
            //}
            //if (Directory.Exists(StartupPath))
            //{
            //    Directory.Delete(StartupPath, true);
            //}
            string base64 = FileSaveDownloadAPI.DocReportdeleteDynamic(Path.GetFileName(filename), TempPath);
            return ErrorMessage;
        }



        public static bool IsMaliciousPdf(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return true;

            byte[] fileBytes;

            using (var ms = new MemoryStream())
            {
                file.InputStream.Position = 0;
                file.InputStream.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            

            string content = Encoding.ASCII.GetString(fileBytes);

            string[] blockedPatterns =
            {
               "alert",
        "/JavaScript",
       // "/JS",
        "/Launch",
        "/OpenAction",
        "/EmbeddedFile",
       // "/RichMedia",

        "<script",
        "</script>",
        "javascript:",
        "onload=",
        "onclick=",
        "onerror=",
       // "ontoggle=",
       // "<iframe",
       // "<object",
        "<embed",
       // "<details"
    };

          

            foreach (var pattern in blockedPatterns)
            {
                if (content.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true; // Malicious pattern found
                }
            }
            return false;
        }


        [HttpPost]
        //[SessionExpireFilterAttributeCandidate]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UploadCandidateDocumentUpdated(AddTempPQVerifiedUploadDocViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc, FormCollection formCollection)
        {
            string msg = "";
            int PersonalRowId = 0;
            short SubCheckId = 0;
            int clientRowId = 0;
            short CheckFamilyId = Convert.ToInt16(formCollection["CheckFamilyId"].ToString());
            try
            {


                string PersonalRowid = formCollection["PersonalRowid"].ToString();
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

                string SubCheckid = formCollection["SubCheckId"].ToString();
                SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

                string ddlDocumentType = formCollection["ddlDocumentType"].ToString();



                string clientRowid = formCollection["clientRowId"].ToString();
                clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));
                if (UploadDoc != null)
                {
                    foreach (var item in UploadDoc)
                    {
                        string errorMessage = "";                        
                            errorMessage = GatZipFileMessage(item, PersonalRowId.ToString(), CheckFamilyId.ToString());
                            if (errorMessage == "")
                            {
                                string fileName = Path.GetFileName(model.UploadDoc.FileName);
                                string[] parts = fileName.Split('.');

                                if (parts.Length > 2)
                                {

                                    errorMessage = "Files with multiple extensions are not allowed.";

                                }
                                if (errorMessage == "")
                                {
                                    bool MaliciousFile = ValidateMaliciousFile(item);
                                    if (MaliciousFile == false)
                                    {
                                        errorMessage = "Upload file extension not allowed.";
                                    }
                            }
                            string ext = Path.GetExtension(model.UploadDoc.FileName);
                            if (errorMessage == "" && ext.ToLower() == ".pdf")
                            {
                                bool MaliciousFile = IsMaliciousPdf(item);
                                if (MaliciousFile == true)
                                {
                                    errorMessage = "Upload file extension not allowed.";
                                }
                            }
                        }
                       

                        if (errorMessage != "")
                        {
                            if (CheckFamilyId == 2)
                            {
                                ViewBag.ErrorMsg += errorMessage;
                                return RedirectToAction("AddAddressInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                            }
                            else if (CheckFamilyId == 5)
                            {
                                ViewBag.ErrorMsg += errorMessage;
                                return RedirectToAction("AddCriminalInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                            }
                            else if (CheckFamilyId == 11)
                            {
                                ViewBag.ErrorMsg += errorMessage;
                                return RedirectToAction("AddEducationInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                            }

                            else if (CheckFamilyId == 12)
                            {
                                ViewBag.ErrorMsg += errorMessage;
                                return RedirectToAction("AddEmploymentInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                            }
                            else if (CheckFamilyId == 26)
                            {
                                ViewBag.ErrorMsg += errorMessage;
                                return RedirectToAction("AddNationalIdentityInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                            }

                            else if (CheckFamilyId == 30)
                            {
                                ViewBag.ErrorMsg += errorMessage;
                                return RedirectToAction("AddReferenceInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                            }
                            else if (CheckFamilyId == 40)
                            {
                                ViewBag.ErrorMsg += errorMessage;
                                return RedirectToAction("AddSpecialCheckInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                            }
                        }
                    }
                }
                else
                {
                    if (CheckFamilyId == 2)
                    {
                        ViewBag.ErrorMsg = "Document Can not be Blank";
                        return RedirectToAction("AddAddressInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                    }
                    else if (CheckFamilyId == 5)
                    {
                        ViewBag.ErrorMsg = "Document Can not be Blank";
                        return RedirectToAction("AddCriminalInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                    }
                    else if (CheckFamilyId == 11)
                    {
                        ViewBag.ErrorMsg = "Document Can not be Blank";
                        return RedirectToAction("AddEducationInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                    }

                    else if (CheckFamilyId == 12)
                    {
                        ViewBag.ErrorMsg = "Document Can not be Blank";
                        return RedirectToAction("AddEmploymentInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                    }
                    else if (CheckFamilyId == 26)
                    {
                        ViewBag.ErrorMsg = "Document Can not be Blank";
                        return RedirectToAction("AddNationalIdentityInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                    }

                    else if (CheckFamilyId == 30)
                    {
                        ViewBag.ErrorMsg = "Document Can not be Blank";
                        return RedirectToAction("AddReferenceInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                    }
                    else if (CheckFamilyId == 40)
                    {
                        ViewBag.ErrorMsg = "Document Can not be Blank";
                        return RedirectToAction("AddSpecialCheckInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                    }

                }

                if (PersonalRowId > 0 && UploadDoc != null)
                {
                    int count = 0;
                    string[] DocumentTypes = UploadHid.Split(',');

                    int countdoc = model.UploadDoc.FileName.Split('.').Length - 1;
                    if (countdoc > 1)
                    {
                        if (CheckFamilyId == 2)
                        {
                            TempData["SuccessMsg"] = "Please check file name !";
                            ViewBag.ErrorMsg = "Please check file name !!";
                            ModelState.Clear();
                            return RedirectToAction("AddAddressInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                        else if (CheckFamilyId == 5)
                        {
                            TempData["SuccessMsg"] = "Please check file name !";
                            ViewBag.ErrorMsg = "Please check file name !!";
                            ModelState.Clear();
                            return RedirectToAction("AddCriminalInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                        else if (CheckFamilyId == 11)
                        {
                            TempData["SuccessMsg"] = "Please check file name !";
                            ViewBag.ErrorMsg = "Please check file name !!";
                            ModelState.Clear();
                            return RedirectToAction("AddEducationInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }

                        else if (CheckFamilyId == 12)
                        {
                            TempData["SuccessMsg"] = "Please check file name !";
                            ViewBag.ErrorMsg = "Please check file name !!";
                            ModelState.Clear();
                            return RedirectToAction("AddEmploymentInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                        else if (CheckFamilyId == 26)
                        {
                            TempData["SuccessMsg"] = "Please check file name !";
                            ViewBag.ErrorMsg = "Please check file name !!";
                            ModelState.Clear();
                            return RedirectToAction("AddNationalIdentityInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }

                        else if (CheckFamilyId == 30)
                        {
                            TempData["SuccessMsg"] = "Please check file name !";
                            ViewBag.ErrorMsg = "Please check file name !!";
                            ModelState.Clear();
                            return RedirectToAction("AddReferenceInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                        else if (CheckFamilyId == 40)
                        {
                            TempData["SuccessMsg"] = "Please check file name !";
                            ViewBag.ErrorMsg = "Please check file name !!";
                            ModelState.Clear();
                            return RedirectToAction("AddSpecialCheckInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                    }
                    else
                    {
                        foreach (var item in UploadDoc)
                        {
                            HttpPostedFileBase NewUpload = item;
                            model.FileName = item.FileName.ToString();
                            model.DocumentType = DocumentTypes[count];
                            if (model.DocumentType != null && model.FileName != null && item.ContentLength > 0)
                            {
                                byte[] fileBytesS;
                                using (var ms = new MemoryStream())
                                {
                                    item.InputStream.CopyTo(ms);
                                    fileBytesS = ms.ToArray();

                                    // Pointer wapas reset karein taaki pehla method padh sake
                                    if (item.InputStream.CanSeek)
                                        item.InputStream.Position = 0;
                                }

                                SavePQVerifiedUploadDoc(PersonalRowId, SubCheckId, "Provided", ddlDocumentType, "provideddocumentsAPI", item, model.Remarks);

                                if (clientRowId == 576)
                                {
                                    ClientCustomize cmc = new ClientCustomize();
                                    string exttt = Path.GetExtension(item.FileName).ToLower();
                                    DataTable autofetched = cmc.AutoFetchingDocuments(PersonalRowId, SubCheckId, ddlDocumentType, model.Remarks, exttt);
                                    if (autofetched.Rows.Count > 0)
                                    {
                                        for (int j = 0; j < autofetched.Rows.Count; j++)
                                        {
                                            int pid = Convert.ToInt32(autofetched.Rows[j]["PersonalRowID"].ToString());
                                            int sid = Convert.ToInt32(autofetched.Rows[j]["SubCheckRowID"].ToString());
                                            string FileName = autofetched.Rows[j]["FileName"].ToString();




                                            SavePQVerifiedUploadDocwithoutEntry(pid, Convert.ToInt16(sid), "Provided", ddlDocumentType, "provideddocumentsAPI", fileBytesS, model.Remarks, FileName);
                                        }

                                    }
                                }

                            }
                            else
                            {
                                ViewBag.ErrorMsg = "No item found!";
                                break;
                            }
                            count++;
                        }
                        if (clientRowId == 1749 || clientRowId == 2)
                        {
                            new API().UpdateAPICheckStatus(clientRowId.ToString().Trim(), PersonalRowId, SubCheckId, "PARTIALLY_UPLOADED");
                        }
                        CreateMenuitemforDataEntryOperator(PersonalRowId);

                        int PID = PersonalRowId;
                        if (CheckFamilyId == 2)
                        {

                            ViewBag.Success = "Document Uploaded Successfully !";
                            ModelState.Clear();
                            return RedirectToAction("AddAddressInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                        else if (CheckFamilyId == 5)
                        {
                            ViewBag.Success = "Document Uploaded Successfully !";
                            ModelState.Clear();
                            return RedirectToAction("AddCriminalInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                        else if (CheckFamilyId == 11)
                        {
                            ViewBag.Success = "Document Uploaded Successfully !";
                            ModelState.Clear();
                            return RedirectToAction("AddEducationInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }

                        else if (CheckFamilyId == 12)
                        {
                            ViewBag.Success = "Document Uploaded Successfully !";
                            ModelState.Clear();
                            return RedirectToAction("AddEmploymentInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                        else if (CheckFamilyId == 26)
                        {
                            ViewBag.Success = "Document Uploaded Successfully !";
                            ModelState.Clear();
                            return RedirectToAction("AddNationalIdentityInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }

                        else if (CheckFamilyId == 30)
                        {
                            ViewBag.Success = "Document Uploaded Successfully !";
                            ModelState.Clear();
                            return RedirectToAction("AddReferenceInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                        else if (CheckFamilyId == 40)
                        {
                            ViewBag.Success = "Document Uploaded Successfully !";
                            ModelState.Clear();
                            return RedirectToAction("AddSpecialCheckInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            // GetAllCheckListforDocUpload(model.PersonalRowID);

            string frm = "";
            if (CheckFamilyId == 2)
            {
                frm = "AddAddressInfo";
            }
            else if (CheckFamilyId == 5)
            {
                frm = "AddCriminalInfo";
            }
            else if (CheckFamilyId == 11)
            {
                frm = "AddEducationInfo";
            }
            else if (CheckFamilyId == 12)
            {
                frm = "AddEmploymentInfo";
            }
            else if (CheckFamilyId == 26)
            {
                frm = "AddNationalIdentityInfo";
            }
            else if (CheckFamilyId == 30)
            {
                frm = "AddReferenceInfo";
            }
            else if (CheckFamilyId == 40)
            {
                frm = "AddSpecialCheckInfo";
            }
            return RedirectToAction(frm, "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });

        }



        // Updated Rahul 17-01-2018 End


        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UploadCandidateDocument(AddTempPQVerifiedUploadDocViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.PersonalRowID > 0 && !string.IsNullOrEmpty(UploadHid) && UploadDoc != null)
                    {
                        int count = 0;
                        string[] DocumentTypes = UploadHid.Split(',');
                        foreach (var item in UploadDoc)
                        {
                            model.FileName = item.FileName.ToString();
                            model.DocumentType = DocumentTypes[count];
                            if (model.DocumentType != null && model.FileName != null && item.ContentLength > 0)
                            {
                                SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "Provided", model.DocumentType, "provideddocumentsAPI", item, model.Remarks);
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "No item found!";
                                break;
                            }
                            count++;
                        }
                        CreateMenuitemforDataEntryOperator(model.PersonalRowID);
                        int PID = model.PersonalRowID;
                        int ClientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(Session["ClientRowID"].ToString()));
                        if (ClientRowId == 1749)
                        {
                            new API().UpdateAPICheckStatus(ClientRowId.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "PARTIALLY_UPLOADED");
                        }
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("ManageCandidateUploadDocmentList");
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
            GetAllCheckListforDocUpload(model.PersonalRowID);
            return View(model);
        }

        #endregion

        // Class for SQL Keywords
        public class Sql_Key_Val
        {
            public string SQl_Check { get; set; }
            public bool Status { get; set; }
        }

        // list of SQL Keyword
        public List<Sql_Key_Val> Get_List_SQLKeyword()
        {
            var Val = new List<Sql_Key_Val>() {
                new Sql_Key_Val(){ SQl_Check = "ADD" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ADDCONSTRAINT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ALTER" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ALTERCOLUMN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ALTERTABLE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ALL" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "AND" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ANY" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "AS" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ASC" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "BACKUPDATABASE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "BETWEEN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CASE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CHECK" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "COLUMN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CONSTRAINT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CREATE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CREATEDATABASE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CREATEINDEX" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CREATEOR REPLACE VIEW" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CREATETABLE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CREATEPROCEDURE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CREATEUNIQUE INDEX" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "CREATEVIEW" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DATABASE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DEFAULT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DELETE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DESC" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DISTINCT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DROP" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DROPCOLUMN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DROPCONSTRAINT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DROPDATABASE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DROPDEFAULT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DROPINDEX" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DROPTABLE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "DROPVIEW" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "EXEC" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "EXISTS" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "FOREIGN KEY" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "FROM" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "FULLOUTERJOIN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "GROUPBY" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "HAVING" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "IN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "INDEX" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "INNERJOIN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "INSERTINTO" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "INSERTINTOSELECT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ISNULL" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ISNOTNULL" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "JOIN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "LEFTJOIN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "LIKE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "LIMIT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "NOT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "NOTNULL" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "OR" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ORDERBY" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "OUTERJOIN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "PRIMARYKEY" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "PROCEDURE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "RIGHTJOIN" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "ROWNUM" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "SELECT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "SELECTDISTINCT" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "SELECTINTO" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "SELECTTOP" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "SET" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "TABLE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "TOP" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "TRUNCATETABLE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "UNION" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "UNIONALL" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "UNIQUE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "UPDATE" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "VALUES" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "VIEW" ,Status = false},
                new Sql_Key_Val(){ SQl_Check = "WHERE" ,Status = false}
            };

            return Val;
        }


        // Check Value for SQL keyword
        public string getval(string val)
        {
            string st = null;
            if (val != null)
            {

                st = "";
                var vals = val.ToUpper();
                vals = Regex.Replace(vals, @"\s", "");

                var Val = Get_List_SQLKeyword();

                if (val != null)
                {
                    var query = Val.Where(d => d.SQl_Check.Trim() == vals.Trim()).FirstOrDefault();
                    if (query != null)
                    {
                        if (query.Status == false)
                        {
                            st = "false";
                        }
                        else
                        {
                            st = "true";
                        }
                    }
                    if (st == "")
                    {
                        st = null;
                    }

                }
            }
            return st;
        }

        // For Criminal Info
        public string Validation_HTMLSanitizer(TempPQReferenceViewModel model)
        {
            try
            {
                var sata = getval(model.RV_OrganisationName);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.RV_RefereeName);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.RV_Refere_Desig);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.RV_Refre_Eid);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.RV_Refre_MobNo);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.RV_Tenure_Emp);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.RV_Reason_Leav);
                if (sata == "false")
                {
                    return sata;
                }
                return sata;

            }
            catch (Exception)
            {

                throw;
            }
        }
        // For Criminal Info
        public string Validation_HTMLSanitizer(AddTempPQCriminalViewModel model)
        {
            try
            {
                var sata = getval(model.CRV_Father_Name);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.CRV_Cand_Name);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.CRV_Others);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.CRV_Nationality);
                if (sata == "false")
                {
                    return sata;
                }
                return sata;

            }
            catch (Exception)
            {

                throw;
            }
        }
        // For Education Info
        public string Validation_HTMLSanitizer(AddTempPQEducationViewModel model)
        {
            try
            {
                var sata = getval(model.EV_Cand_Name);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.CollegeNameLabel);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.Remarks);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_Colg_Schl_Inst_Add_ConDet);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_Division_CGPA);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_Dur_Course);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_Enroll_RollNo);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_Specialization);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_Yr_Passing);
                if (sata == "false")
                {
                    return sata;
                }
                return sata;

            }
            catch (Exception)
            {

                throw;
            }
        }

        // For National Identity Info
        public string Validation_HTMLSanitizer(AddTempPQNationalIdentityViewModel model)
        {
            try
            {
                var sata = getval(model.NIC_Cand_Name);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.NIC_ID_No);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.NIC_OtherDetails10);
                if (sata == "false")
                {
                    return sata;
                }
                return sata;

            }
            catch (Exception)
            {

                throw;
            }
        }

        // For Employement Info
        public string Validation_HTMLSanitizer(AddTempEmploymentViewModel model)
        {
            try
            {
                var sata = getval(model.OrgNameLabel);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.Remarks);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_CmpNameAddress);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_Department);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_EmpType);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_EmploymentPeriod);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_Designation);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_EmployeeCode);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_CmpCost);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_EmployeeTenure);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EV_ExitFormalitiesPending);
                if (sata == "false")
                {
                    return sata;

                }
                sata = getval(model.EV_HandledResponsibilities);
                if (sata == "false")
                {
                    return sata;

                }
                sata = getval(model.EV_leavingReason);
                if (sata == "false")
                {
                    return sata;

                }
                sata = getval(model.EV_ReportingManagerName);
                if (sata == "false")
                {
                    return sata;

                }
                sata = getval(model.EV_Rptng_Mgr_Name_ConDet);
                if (sata == "false")
                {
                    return sata;

                }
                return sata;

            }
            catch (Exception)
            {

                throw;
            }
        }

        // For Special Case info
        public string Validation_HTMLSanitizer(AddTempSpecialCheckViewModel model)
        {
            try
            {
                var sata = getval(model.SC_Cand_Name);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.SC_Father_Name);
                if (sata == "false")
                {
                    return sata;
                }
                if (model.SC_Others1 != null || model.SC_Others2 != null)
                {
                    sata = getval(model.SC_Others1);
                    if (sata == "false")
                    {
                        return sata;
                    }
                    sata = getval(model.SC_Others2);
                    if (sata == "false")
                    {
                        return sata;
                    }
                }
                return sata;

            }
            catch (Exception)
            {

                throw;
            }
        }

        // For Address info
        public string Validation_HTMLSanitizer(TempPQAddressViewModel model)
        {
            try
            {
                var sata = getval(model.AV_Cand_Name);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.AV_Add);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.AV_Landmark);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.AV_Dur_of_Stay);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.AV_OtherDetails);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.AV_Owner_ConNo);
                if (sata == "false")
                {
                    return sata;
                }
                return sata;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // For Personal Info
        public string Validation_HTMLSanitizer(UpdateTempPQPersonalViewModel model)
        {

            try
            {
                var sata = getval(model.MiddleName);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.LastName);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.FatherName);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.FatherMiddleName);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.FatherLastName);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.AlternateNo);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.Nationality);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.EmployeeID);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.Designation);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.PlaceofJoining);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.AadhaarCardNo);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.DrivingLicenseNo);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.PassportNo);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.PanCardNo);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(model.VoterIDCardNo);
                if (sata == "false")
                {
                    return sata;
                }
                return sata;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private DataTable getLastFiveRecordsOfToday(int clientrowid, int clientuserRowid)
        {
            //return datatable
            DataTable dt = new DataTable();
            return dt;
        }

        #region ******  Personal Info ******

        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult PersonalInfo()
        {
            var model = new PQPersonalInfoViewModel();
            long id = Convert.ToInt32(Session["TempPersonalRowID"]);
            int pid = Convert.ToInt32(Session["TempPersonalRowID"]);
            string inEncript = clsCommonMethods.PEncrypt(id.ToString());
            model = repoPQPersonal.GetTempPersonalInfoByPersonalRowID(pid);
            model.ClientRowID = Convert.ToInt32(Session["ClientRowID"].ToString());
            model.PersonalRowID = pid;
            model.LOA_TimeStamp = DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss tt");
            model.EncPersonalID = inEncript;
            if (!string.IsNullOrEmpty(model.hfSign))
            {
                ViewBag.LoaImage = LoadImage(model.hfSign);
                ViewBag.LoaImageB64 = model.hfSign;

            }
            else
            {
                ViewBag.CanLOASignature = "";
                ViewBag.LoaImageB64 = "";
            }
            ////BindCandTitleList();
            ViewBag.Message = 0;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [ValidateSessionFilterCandidate]
        public ActionResult PersonalInfo(PQPersonalInfoViewModel objs, HttpPostedFileBase file)
        {
            ClientCustomize cc = new ClientCustomize();
            string newimage = objs.hfSign;
            string Name = objs.LOA_Name;
            string CTStamp = objs.LOA_TimeStamp;
            string[] base64 = newimage.Split(',');
            string Base64Image = "";
            if (base64.Length > 1)
            {
                Base64Image = base64[1].ToString();
            }

            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            string fileSavePath = "";
            string fileSavePath2 = "";
            long personalrowid = Convert.ToInt64(Session["TempPersonalRowID"]);
            if (file != null && file.ContentLength > 0 && System.IO.Path.GetExtension(file.FileName).ToLower() == ".jpg")
            {

                string FldPathDoc = string.Empty;
                string FldPathDoc2 = string.Empty;
                FldPathDoc = ConfigurationManager.AppSettings["provideddocumentsAPI".Trim().ToLower()].ToString();
                FldPathDoc2 = ConfigurationManager.AppSettings["provideddocuments".Trim().ToLower()].ToString();

                string SecuId = Convert.ToString(Session["SecritadRefId"]);
                string ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                string filename = personalrowid + "_" + "LOA_Pic";
                fileSavePath = FldPathDoc + filename + ext;
                fileSavePath2 = FldPathDoc2 + filename + ext;
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                string base64String = Convert.ToBase64String(fileBytes);
                string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename + ext, FldPathDoc, base64String);
                //file.SaveAs(fileSavePath);
            }

            int PersonalRowid = Convert.ToInt32(Session["TempPersonalRowID"]);
            cc.SaveDigitalSignature_PersonalNLoa(personalrowid, Base64Image, Name, fileSavePath2, CTStamp, ipaddress);
            repoPQPersonal.UpdateTempPQPersonalInfo(objs);
            //// BindCandTitleList();
            var model = repoPQPersonal.GetTempPersonalInfoByPersonalRowID(PersonalRowid);
            if (!string.IsNullOrEmpty(model.hfSign))
            {
                ViewBag.LoaImage = LoadImage(model.hfSign);
                ViewBag.LoaImageB64 = model.hfSign;
                ViewBag.Message = 1;
            }
            else
            {
                ViewBag.CanLOASignature = "";
                ViewBag.LoaImageB64 = "";
                ViewBag.Message = 0;
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdatetempPQCandidateChecks(int crid, long prid)
        {
            var result = repoPQPersonal.UpdatetempPQCandidateChecksData(crid, prid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinalSubmit1015(string id = "")
        {
            try
            {
                long ids = Convert.ToInt64(clsCommonMethods.PDecrypt(id));
                DataTable dt = new DataTable();
                dt = tpc.GetCandidateFormCompletenessStatus(ids);

                DataTable dt2 = new DataTable();
                dt2 = tpc.GetCandidateFormDigitalStatus(Convert.ToInt64(ids));
                string loa = "";
                if (dt2.Rows.Count > 0)
                {
                    loa = dt2.Rows[0]["Loa"].ToString();
                }
                if (loa == "")
                {
                    TempData["AlertMsg"] = "<div class='alert alert-danger' style='padding:5px'><strong>Attention ! </strong>Please Fill  Singnature.</div> ";
                    return RedirectToAction("PersonalInfo");
                }


                string MissingInformation = "";
                string MissingDocs = "";

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["InoStatus"].ToString() == "Pending")
                    {
                        MissingInformation = " " + dr["InformationType"].ToString();
                        break;
                    }
                    if (dr["DocStatus"].ToString() == "Pending")
                    {
                        MissingDocs = dr["InformationType"].ToString();
                        break;
                    }
                }


                if (MissingDocs != "")
                {
                    TempData["AlertMsg"] = "<div class='alert alert-danger' style='padding:5px'><strong>Attention ! </strong>Request you to upload <b> " + MissingDocs + " </b> document to move further.</div> ";
                }
                else if (MissingInformation != "")
                {
                    TempData["AlertMsg"] = "<div class='alert alert-danger' style='padding:5px'><strong>Attention ! </strong>Request you to complete <b> " + MissingInformation + " </b> check to move further.</div> ";
                }
                else
                    TempData["AlertMsg"] = "<div class='alert alert-success' style='padding:5px'><strong>Form Uploaded Successfully. ! </strong>Thanks for providing the complete details</div> ";

                Int16 ClientRowID = Convert.ToInt16(Session["ClientRowID"]);

                if (MissingInformation == "" && MissingDocs == "")
                {
                    int createdBy = int.Parse(Session["CandidateUserRowID"].ToString());
                    if (Convert.ToByte(Session["HRApproval"]) == 0)
                    {

                        string result = tpc.ApproveOrFinalSubmitByCandidate(ids, createdBy);
                        if (result == "Success")
                        {
                            SendMail(ids, ClientRowID);
                        }
                        if (result == "Success")
                        {
                            MoveLOADocUploadedByCandidate(ids);
                            return RedirectToAction("LogOutCandidate", "Account", new { area = "" });
                        }

                    }
                    if (Convert.ToByte(Session["HRApproval"]) == 1)
                    {
                        DataTable dt1 = bl.getCaseStatusWithCandidate(ids);

                        if (dt1.Rows[0]["CaseStatus"].ToString() == "1")
                        {
                            tpc.ApproveOrFinalSubmitByCandidateToHR(ids);
                            try
                            {
                                SendMail(ids, ClientRowID);
                            }
                            catch { }

                            return RedirectToAction("LogOutCandidate", "Account", new { area = "" });
                        }

                        bl.UpdateCaseStatus(ids, 2, 0);//userid 0 means candidate and case status 2 means- showing HR to Approve
                        return RedirectToAction("LogOutCandidate", "Account", new { area = "" });
                    }

                }



            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return RedirectToAction("PersonalInfo");
        }


        #endregion

        #region ****** Update Personal Details ******
        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult UpdateCandidateDetails(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "")
        {

            ViewBag.Information = "Please first fill the information, Click on Save, Upload the document, Click on Submit & then click on Next Button";
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            //int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            int clientRowId = Convert.ToInt32(Session["ClientRowID"].ToString());
            int PersonalRowId = Convert.ToInt32(Session["TempPersonalRowID"].ToString());

            string FirstamCheckFamily = "";
            string FirstamSubCheckID = "";
            ViewBag.FirstAmericanCheckID = "";
            if (clientRowId == 559)
            {
                DataSet Firstamericandt = new DataSet();
                Firstamericandt = tpc.GetTempCandidateChecksDT(PersonalRowId);
                if (Firstamericandt.Tables[1].Rows.Count > 0)
                {
                    FirstamSubCheckID = Convert.ToString(Firstamericandt.Tables[1].Rows[0]["SubCheckFamilyID"]);
                    FirstamCheckFamily = Convert.ToString(Firstamericandt.Tables[1].Rows[0]["CheckFamilyRowID"]);
                }
                if (FirstamSubCheckID == "739")
                {
                    ViewBag.FirstAmericanCheckID = FirstamSubCheckID;
                }
            }

            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, 0, "Candidate", "Case");
            if (clientRowId == 1255)// Infinx services pvt ltd  client UAN column remove for fresher package
            {
                DataTable dtpac = new DataTable();
                dtpac = bl.PackageNameClientWise(PersonalRowId);
                if (dtpac.Rows.Count > 0)
                {
                    string packname = dtpac.Rows[0]["PackageOrCheckDetails"].ToString();
                    if (packname.Contains("INFINX_Freshers (CRCP)"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["AntecedentsRowID"].ToString() == "125")
                                dr.Delete();
                        }
                        dt.AcceptChanges();
                    }
                }
            }

            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, 0);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                    {
                        ViewBag.getviewbagerrormsg = "Data blank";
                    }
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
                    {

                        //marksModel.Add(d[1].ToString(), d[0].ToString().ToString());
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }

                }
                else
                {
                    ViewBag.getviewbagerrormsg = "Data blank";
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
            DataSet ChecksAndDocStatus = new DataSet();
            ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(Convert.ToInt32(PersonalRowId), 0);
            if (ChecksAndDocStatus != null)
            {
                ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CaseInfoStatus"].ToString());
            }

            ViewBag.PersonalRowid = PersonalRowId.ToString();

            ViewBag.clientRowId = clientRowId.ToString();
            //GetEmploymentList();
            return View(marksModel);
        }

        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult UpdateCandidateDetails(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());

            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowid;

                cd.clientRowId = clientRowid;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());
                string FirstamCheckFamily = "";
                string FirstamSubCheckID = "";
                ViewBag.FirstAmericanCheckID = "";
                if (clientRowid == 559)
                {
                    DataSet Firstamericandt = new DataSet();
                    Firstamericandt = tpc.GetTempCandidateChecksDT(PersonalRowid);
                    if (Firstamericandt.Tables[1].Rows.Count > 0)
                    {
                        FirstamSubCheckID = Convert.ToString(Firstamericandt.Tables[1].Rows[0]["SubCheckFamilyID"]);
                        FirstamCheckFamily = Convert.ToString(Firstamericandt.Tables[1].Rows[0]["CheckFamilyRowID"]);
                    }
                    if (FirstamSubCheckID == "739")
                    {
                        ViewBag.FirstAmericanCheckID = FirstamSubCheckID;
                    }
                }


                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, 0, "Candidate", "Case");
                ViewBag.ErrorMsg = "";

                DataTable dbValue = new DataTable();
                dbValue = bl.Antecedents_client_select_DB_Candidate(cd.PersonalRowID, 0);
                string UANhaveornot = "";
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (key == "578")
                            {
                                UANhaveornot = formCollection[key].ToString();
                            }
                            if (key == "125")
                            {
                                if (UANhaveornot == "Yes")
                                {

                                    if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                    {
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                    }
                                }

                            }
                            else if (key == "579")
                            {
                                if (UANhaveornot == "No")
                                {
                                    if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                    {
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                    }
                                }
                            }
                            else if (d[4].ToString() == "textbox")
                            {
                                if (clientRowid == 559 && FirstamSubCheckID == "739")
                                {
                                    if (key != "124")
                                    {
                                        if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                        {
                                            ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                        }
                                        if (clsCommonMethods.ValidateSQLInjuctionInput(cd.DataEntryValue) == false)
                                        {
                                            ViewBag.ErrorMsg = ViewBag.ErrorMsg + "Please Provide valid Input in " + cd.DisplayName + " field";
                                        }
                                    }
                                }
                                else
                                {
                                    if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                    {
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                    }
                                    if (clsCommonMethods.ValidateSQLInjuctionInput(cd.DataEntryValue) == false)
                                    {
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + "Please Provide valid Input in " + cd.DisplayName + " field";
                                    }
                                }
                                if (key == "19")
                                {
                                    string mobileNo = cd.DataEntryValue;
                                    if (mobileNo.Length > 10)
                                    {
                                        ViewBag.ErrorMsg = "Please enter 10-digit Mobile Number";
                                        TempData["ErrorMessage"] = "Please enter 10-digit Mobile Number";
                                        return RedirectToAction("UpdateCandidateDetails", "Home", new { subCheckId = "", ClientRowId = clientRowid, PersonalRowID = PersonalRowid });

                                    }
                                }
                            }
                            else if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }

                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }

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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = PersonalRowid + "_" + filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
                                    if (clientRowid == 559 && cd.AntecedentRowID == 584)
                                    {
                                        string FolderPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                                        string fileName = cd.DataEntryValue;
                                        string filePath = Path.Combine(Server.MapPath(FolderPath), fileName); // specify the file path
                                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                                        {
                                            // assume 'fileContent' is a byte array containing the file content
                                            byte[] fileContent = Encoding.UTF8.GetBytes(filePath);
                                            fs.Write(fileContent, 0, fileContent.Length);
                                        }
                                        ClientCustomize cc = new ClientCustomize();
                                        cc.FirstAmericanPanUploadPI(PersonalRowid, 136, cd.DataEntryValue, FolderPath);
                                    }
                                    if (clientRowid == 559 && cd.AntecedentRowID == 584)
                                    {
                                        string FolderPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                                        bl.proc_InsertUpdateTempPQVerifiedUploadCaseDocs(cd, FolderPath);
                                    }
                                    else
                                    {
                                        bl.proc_InsertUpdateTempPQVerifiedUploadCaseDocs(cd, "~/Uploads/");
                                    }
                                }
                                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                                if (rowsFiltered.Count() == 0 && postedFile.ContentLength == 0)
                                {
                                    if (clientRowid != 559 && FirstamSubCheckID != "739")
                                    {
                                        marksModel.Add(d[1].ToString(), d[0].ToString());
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + "<br>" + FString[1] + " is a mandatory field";
                                    }
                                }

                            }
                        }

                    }

                    else if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                if (cd.AntecedentRowID != 121 && cd.AntecedentRowID != 8)
                                {
                                    bl.SaveDataEntry_Candidate(cd);
                                }
                                if ((cd.AntecedentRowID == 121 || cd.AntecedentRowID == 5 || cd.AntecedentRowID == 19 || cd.AntecedentRowID == 124 || cd.AntecedentRowID == 122 || cd.AntecedentRowID == 125) && (clientRowid == 559 || clientRowid == 992 || clientRowid == 993 || clientRowid == 994 || clientRowid == 1125 || clientRowid == 1243 || clientRowid == 193))
                                {
                                    ClientCustomize cc = new ClientCustomize();
                                    cc.UpdateCandidateAutoFillDetails(Convert.ToInt32(cd.AntecedentRowID), Convert.ToInt32(cd.PersonalRowID), cd.DataEntryValue, cd.CreatedBy);
                                }
                                //if (cd.AntecedentRowID == 121 && (clientRowid==559 || clientRowid == 993))
                                //{
                                //ClientCustomize cc = new ClientCustomize();
                                //cc.updateCandidateNameInTempDataEntry(Convert.ToInt32(cd.PersonalRowID), cd.DataEntryValue, cd.CreatedBy);
                                //}
                                //else if (cd.AntecedentRowID == 5 && (clientRowid == 559 || clientRowid == 993))
                                //{
                                //    ClientCustomize cc = new ClientCustomize();
                                //    cc.updateDOBInTempDataEntry(Convert.ToInt32(cd.PersonalRowID), cd.DataEntryValue, cd.CreatedBy);
                                //}
                                //else if (cd.AntecedentRowID == 19 && (clientRowid == 559 || clientRowid == 993))
                                //{
                                //    ClientCustomize cc = new ClientCustomize();
                                //    cc.updateMobileNoInTempDataEntry(Convert.ToInt32(cd.PersonalRowID), cd.DataEntryValue, cd.CreatedBy);
                                //}
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
                                bl.SaveDataEntry_Candidate(cd);
                                if (cd.clientRowId == 576)
                                {
                                    if (cd.AntecedentRowID == 703)
                                    {
                                        if (cd.DataEntryValue.Trim() == "Post Joiner")
                                        {
                                            bl.NaggaroJoiningType(Convert.ToInt32(cd.PersonalRowID), cd.clientRowId, cd.DataEntryValue.Trim());


                                        }
                                        if (cd.DataEntryValue.Trim() == "Pre Joiner")
                                        {
                                            bl.NaggaroJoiningType(Convert.ToInt32(cd.PersonalRowID), cd.clientRowId, cd.DataEntryValue.Trim());


                                        }
                                    }
                                }
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

                        bl.SaveDataEntry_Candidate(cd);
                    }

                    else
                    {
                        DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        //marksModel.Add(d[1].ToString(), d[0].ToString());
                    }
                }
                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, 0, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, 0, "Pending", "Info");
                }
                DataSet ChecksAndDocStatus = new DataSet();
                ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(Convert.ToInt32(cd.PersonalRowID), 0);
                if (ChecksAndDocStatus != null)
                {
                    ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CaseInfoStatus"].ToString());
                }
                CreateMenuitemforDataEntryOperator(cd.PersonalRowID);
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            //ViewBag.Success = "Information Updated Successfully";
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";

                DataSet d = new DataSet();
                if (clientRowid == 559)
                {
                    d = tpc.AutoCheckInitiation_proc(PersonalRowid);
                }
                else
                {
                    d = tpc.GetTempCandidateChecksDT(PersonalRowid);
                }
                string CheckFamilyRowID = "";
                string subcheckrowids = "";
                if (d.Tables[1].Rows.Count > 0)
                {
                    subcheckrowids = Convert.ToString(d.Tables[1].Rows[0]["SubCheckFamilyID"]);
                    CheckFamilyRowID = Convert.ToString(d.Tables[1].Rows[0]["CheckFamilyRowID"]);
                }

                return RedirectToAction(getCheckFormName(CheckFamilyRowID, subcheckrowids), new { subCheckId = clsCommonMethods.PEncrypt(subcheckrowids), ClientRowId = clsCommonMethods.PEncrypt(clientRowid.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowid.ToString()) });
            }
            else
                return View(marksModel);
        }
        #endregion

        #region******* Upload Address Information *******
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddAddressInfo(bool? NotApplicable = false, string subCheckId = "", string ClientRowId = "", string PersonalRowID = "", string pass = "", string msg = "", string isAstraErr = "")
        {
            ViewBag.Digital = "false";
            if (clsCommonMethods.PDecrypt(msg) != "")
            {
                if (isAstraErr == "1")
                    ViewBag.AstraErrorPopup = clsCommonMethods.PDecrypt(msg);
                else
                    ViewBag.Success = clsCommonMethods.PDecrypt(msg);
            }
            ViewBag.NotApplicable = NotApplicable ?? false;
            ClientCustomize cc = new ClientCustomize();
            int clientRowId, SubCheckId, PersonalRowId;
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            Session["Check"] = pass;
            if (IsNumeric(subCheckId))
            {
                SubCheckId = Convert.ToInt32(subCheckId);
            }
            else
            {
                SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            }

            if (IsNumeric(ClientRowId))
            {
                clientRowId = Convert.ToInt32(ClientRowId);
            }
            else
            {
                clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            }
            DataTable AddressName = cc.GetAddressModeDetails(clientRowId);
            if (AddressName.Rows.Count > 0)
            {
                for (int i = 0; i < AddressName.Rows.Count; i++)
                {
                    string AddressMode = Convert.ToString(AddressName.Rows[i]["AddressMode"]);
                    if (AddressMode.ToLower().Trim() == "digital" || AddressMode.ToLower().Trim() == "digital automatically")
                    {
                        ViewBag.Digital = "true";
                        break;
                    }
                }
            }
            if (IsNumeric(PersonalRowID))
            {
                PersonalRowId = Convert.ToInt32(PersonalRowID);
            }
            else
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
            }

            if (clientRowId == 559)
            {
                if (clsCommonMethods.PDecrypt(msg) == "")
                {
                    ViewBag.Information = "Please first fill the information, Click on Save, Upload the document, Click on Submit & then click on Next Button";
                }
            }


            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            DataTable dt = new DataTable();

            DataTable dbValue = new DataTable();
            if (pass == "Copy" && SubCheckId != 2)
            {
                Session["Pass"] = "Copy";
                dt = bl.Antecedents_client_select_form(clientRowId, 2, "Candidate", "Check");
                dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, 2);
            }
            else
            {
                Session["Pass"] = "";
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);
            }
            ViewBag.getviewbagerrormsg = "";
            DataTable dtcheckstatus = bl.getTempPQCandidateChecksDetail(PersonalRowId, SubCheckId);
            string status = "1";
            if (dtcheckstatus.Rows.Count > 0)
            {
                status = dtcheckstatus.Rows[0]["Status"].ToString();

            }


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                    {
                        ViewBag.getviewbagerrormsg = "Data blank";
                    }
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {

                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                        if (d[1].ToString().Contains("34"))
                        {
                            DataTable dtcountry = cc.GetCountryName(rowsFiltered[0][1].ToString());
                            if (dtcountry.Rows.Count > 0)
                            {
                                ViewBag.countryid = dtcountry.Rows[0]["CountryRowID"].ToString();
                            }
                            else
                            {
                                ViewBag.countryid = "";
                            }

                        }
                        else if (d[1].ToString().Contains("44"))
                        {
                            DataTable dtstate = cc.GetStateName(rowsFiltered[0][1].ToString());
                            if (dtstate.Rows.Count > 0)
                            {
                                ViewBag.stateId = dtstate.Rows[0]["StateRowID"].ToString();
                            }
                            else
                            {
                                ViewBag.stateId = "";
                            }
                            //ViewBag.stateId = rowsFiltered[0][1].ToString();
                        }
                        else if (d[1].ToString().Contains("32"))
                        {
                            DataTable dtCity = cc.GetCityName(rowsFiltered[0][1].ToString());
                            if (dtCity.Rows.Count > 0)
                            {
                                ViewBag.Cityid = dtCity.Rows[0]["DistrictRowID"].ToString();
                            }
                            else
                            {
                                ViewBag.Cityid = "";
                            }
                            //ViewBag.Cityid = rowsFiltered[0][1].ToString();
                        }





                        if (d[1].ToString().Contains("34"))
                        {
                            ViewBag.CountryName = rowsFiltered[0][1].ToString();
                        }
                        else if (d[1].ToString().Contains("44"))
                        {
                            ViewBag.StateName = rowsFiltered[0][1].ToString();
                        }
                        else if (d[1].ToString().Contains("32"))
                        {
                            ViewBag.CityName = rowsFiltered[0][1].ToString();
                        }

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

                        if (d[1].ToString().Contains("34"))
                        {
                            ViewBag.CountryName = "";
                            ViewBag.countryid = "";
                        }
                        else if (d[1].ToString().Contains("44"))
                        {
                            ViewBag.StateName = "";
                            ViewBag.stateId = "";
                        }
                        else if (d[1].ToString().Contains("32"))
                        {
                            ViewBag.CityName = "";
                            ViewBag.Cityid = "";
                        }



                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }

            short suncheckid = short.Parse(SubCheckId.ToString());
            expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);

            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            CreateMenuitemforDataEntryOperator(PersonalRowId);

            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            ViewBag.clientno = clientRowId;

            //International client
            DataTable Candidatedocvisibilty = new DataTable();
            Candidatedocvisibilty = cc.getCandidateDocVisibilty(SubCheckId);
            if (Candidatedocvisibilty.Rows[0]["documentDetailsForVerification"].ToString() == "1")
            {
                ViewBag.Candidatedocvisibilty = "true";
            }
            else
            {
                ViewBag.Candidatedocvisibilty = "false";
                bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            }
            DataSet ChecksAndDocStatus = new DataSet();
            ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(PersonalRowId, SubCheckId);
            if (ChecksAndDocStatus != null && ChecksAndDocStatus.Tables.Count > 0 && ChecksAndDocStatus.Tables[0].Rows.Count > 0)
            {
                ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckInfoStatus"]);
                ViewBag.CheckDocStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckDocStatus"]);
            }
            else
            {
                // Handle the case where there are no results
                ViewBag.CheckInfoStatus = "";
                ViewBag.CheckDocStatus = "";
            }
            ClientCustomize blnew = new ClientCustomize();
            DataTable dtpackage = blnew.GetPackageNameByCandidate(PersonalRowId);
            if (dtpackage.Rows.Count > 0)
            {
                ViewBag.packagename = dtpackage.Rows[0][0].ToString();
            }
            return View(marksModel);
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var customers = repoDDL.GetEmploymentDDL();
            return Json(customers);
        }


        [HttpPost]
        public JsonResult getEmployerlist(string Prefix)
        {
            var result = repoCompany.getEmployerlist(Prefix);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getEmployerlistDE(string Prefix)
        {
            var result = repoCompany.getEmployerlistDE(Prefix);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddAddressInfo(FormCollection formCollection, bool? NotApplicable)
        {
            ViewBag.Digital = "false";
            ClientCustomize cc = new ClientCustomize();
            dynamic expando = new ExpandoObject();
            CandidateDEModel cd = new CandidateDEModel();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = 0;
            int SubCheckId = 0;
            int clientRowId = 0;
            string SubCheckid = formCollection["SubCheckId"].ToString();
            string clientRowid = formCollection["clientRowId"].ToString();
            if (IsNumeric(SubCheckid))
            {
                SubCheckId = Convert.ToInt32(SubCheckid);
            }
            else
            {
                SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));
            }

            if (IsNumeric(clientRowid))
            {
                clientRowId = Convert.ToInt32(clientRowid);
            }
            else
            {
                clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));
            }

            if (IsNumeric(PersonalRowid))
            {
                PersonalRowId = Convert.ToInt32(PersonalRowid);
            }
            else
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));
            }
            DataTable AddressName = cc.GetAddressModeDetails(clientRowId);
            if (AddressName.Rows.Count > 0)
            {
                for (int i = 0; i < AddressName.Rows.Count; i++)
                {
                    string AddressMode = Convert.ToString(AddressName.Rows[i]["AddressMode"]);
                    if (AddressMode.ToLower().Trim() == "digital" || AddressMode.ToLower().Trim() == "digital automatically")
                    {
                        ViewBag.Digital = "true";
                        break;
                    }
                }
            }
            if (clientRowId == 559)
            {

                foreach (var item in formCollection.AllKeys)
                {
                    if (item == "GeoMapping")
                    {
                        if (formCollection["GeoMapping"] != null && !string.IsNullOrEmpty(formCollection["GeoMapping"].ToString()))
                        {
                            string GeoMappingLatLong = formCollection["GeoMapping"].ToString();
                            cd.PersonalRowID = PersonalRowId;
                            cd.SubCheckRowID = SubCheckId;
                            cd.DisplayName = "latitude and longitude";
                            cd.AntecedentRowID = 504;
                            cd.DataEntryValue = GeoMappingLatLong;
                            bl.SaveDataEntry_Candidate(cd);
                            ViewBag.Success = "Geo mapping Completed Successfully";
                            return RedirectToAction("AddAddressInfo", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Please select your address through geo mapping";
                            return RedirectToAction("AddAddressInfo", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                        }
                    }
                }
            }
            string Countryid = "";
            string Stateid = "";
            string Cityid = "";
            if (clientRowId != 1425 && clientRowId != 1428 && clientRowId != 1395 && clientRowId != 1414 && clientRowId != 1415 && clientRowId != 1416 && clientRowId != 1429 && clientRowId != 1514)
            {
                Countryid = formCollection["34"].ToString().Replace(",,", "");
                Stateid = formCollection["44"].ToString().Replace(",,", "");
                Cityid = formCollection["32"].ToString().Replace(",,", "");
            }

            string CountryName = "";
            string StateName = "";
            string CityName = "";
            ViewBag.ErrorMsg = "";
            if (clientRowId != 1425 && clientRowId != 1428 && clientRowId != 1395 && clientRowId != 1414 && clientRowId != 1415 && clientRowId != 1416 && clientRowId != 1429 && clientRowId != 1514)
            {
                DataTable DtCountry = new DataTable();
                if (Countryid != "")
                {
                    DtCountry = bl.GetCountryName(Convert.ToInt32(Countryid));
                }
                else
                {
                    if (NotApplicable == true)
                    {
                        ViewBag.countryid = "Not Applicable";
                    }
                    else
                    {
                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + "please select Country.,";
                    }

                }
                DataTable DtState = new DataTable();
                if (Stateid != "")
                {
                    DtState = bl.GetStateName(Convert.ToInt32(Stateid));
                }
                else
                {
                    if (NotApplicable == true)
                    {
                        ViewBag.stateId = "Not Applicable";
                    }
                    else
                    {
                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + "please select State.,";
                    }
                }
                DataTable DtCity = new DataTable();
                if (Cityid != "")
                {
                    DtCity = bl.GetCityName(Convert.ToInt32(Cityid));
                }
                else
                {
                    if (NotApplicable == true)
                    {
                        ViewBag.Cityid = "Not Applicable";
                    }
                    else
                    {
                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + "please select City.,";
                    }
                }

                if (DtCountry.Rows.Count > 0)
                {
                    CountryName = DtCountry.Rows[0]["CountryName"].ToString();
                }

                if (DtState.Rows.Count > 0)
                {
                    StateName = DtState.Rows[0]["StateName"].ToString();
                }

                if (DtCity.Rows.Count > 0)
                {
                    CityName = DtCity.Rows[0]["DistrictName"].ToString();
                }
            }



            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);


            var marksModel = expando as IDictionary<string, object>;
            try
            {
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }

                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }

                            Regex regex = new Regex(@"^[a-zA-Z0-9\s\-_(),.'@/\\]*$");
                            if (!regex.IsMatch(cd.DataEntryValue))
                            {
                                ViewBag.ErrorMsg = "Special characters are not allowed";
                                TempData["ErrorMessage"] = "Special characters are not allowed";
                                return RedirectToAction("AddAddressInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                            }
                            if (key == "131" || key == "132")
                            {
                                string mobileNo = cd.DataEntryValue;
                                if (mobileNo.Length > 10)
                                {
                                    ViewBag.ErrorMsg = "Please enter 10-digit Mobile Number";
                                    TempData["ErrorMessage"] = "Please enter 10-digit Mobile Number";
                                    return RedirectToAction("AddAddressInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                                }
                            }


                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }
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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);

                                    }
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
                                if (key == "504")
                                {
                                    string let = "";
                                    string lng = "";
                                    string letlong = "";
                                    var letlng = formCollection["504"].ToString().Split(',');
                                    if (letlng.Length == 2)
                                    {
                                        let = letlng[0];
                                        lng = letlng[1];
                                        if (letlng[0].Substring(0, 1) == "(")
                                        {
                                            let = letlng[0].Substring(1, letlng[0].Length - 1);
                                        }
                                        if (letlng[1].Substring(letlng[1].Length - 1, 1) == ")")
                                        {
                                            lng = letlng[1].Substring(0, letlng[1].Length - 1);
                                        }
                                        letlong = let + "," + lng;
                                    }

                                    cd.DataEntryValue = letlong;
                                }
                                else
                                {
                                    cd.DataEntryValue = formCollection[key].ToString();
                                }

                                cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                bl.SaveDataEntry_Candidate(cd);
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
                                //cd.DataEntryValue = formCollection[key].ToString();
                                if (cd.AntecedentRowID == 34)
                                {
                                    cd.DataEntryValue = CountryName;
                                    ViewBag.CountryName = CountryName;
                                }
                                else if (cd.AntecedentRowID == 44)
                                {
                                    cd.DataEntryValue = StateName;
                                    ViewBag.StateName = StateName;
                                }
                                else if (cd.AntecedentRowID == 32)
                                {
                                    cd.DataEntryValue = CityName;
                                    ViewBag.CityName = CityName;
                                }
                                else
                                {
                                    cd.DataEntryValue = formCollection[key].ToString();
                                }
                                bl.SaveDataEntry_Candidate(cd);
                                if (d[1].ToString().Contains("34"))
                                {
                                    DataTable dtcountry = cc.GetCountryName(CountryName);
                                    if (dtcountry.Rows.Count > 0)
                                    {
                                        ViewBag.countryid = dtcountry.Rows[0]["CountryRowID"].ToString();
                                    }
                                    else if (Countryid == "Not Applicable")
                                    {
                                        ViewBag.countryid = "Not Applicable";
                                    }
                                    else
                                    {
                                        ViewBag.countryid = "";
                                    }
                                    //ViewBag.countryid = rowsFiltered[0][1].ToString();

                                }
                                else if (d[1].ToString().Contains("44"))
                                {
                                    DataTable dtstate = cc.GetStateName(StateName);
                                    if (dtstate.Rows.Count > 0)
                                    {
                                        ViewBag.stateId = dtstate.Rows[0]["StateRowID"].ToString();
                                    }
                                    else if (Stateid == "Not Applicable")
                                    {
                                        ViewBag.stateId = "Not Applicable";
                                    }
                                    else
                                    {
                                        ViewBag.stateId = "";
                                    }
                                    //ViewBag.stateId = rowsFiltered[0][1].ToString();
                                }
                                else if (d[1].ToString().Contains("32"))
                                {
                                    DataTable dtCity = cc.GetCityName(CityName);
                                    if (dtCity.Rows.Count > 0)
                                    {
                                        ViewBag.Cityid = dtCity.Rows[0]["DistrictRowID"].ToString();
                                    }
                                    else if (Cityid == "Not Applicable")
                                    {
                                        ViewBag.Cityid = "Not Applicable";
                                    }
                                    else
                                    {
                                        ViewBag.Cityid = "";
                                    }
                                    //ViewBag.Cityid = rowsFiltered[0][1].ToString();
                                }
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

                        bl.SaveDataEntry_Candidate(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                ///////////////////////////////////////////
                bool isNotApplicable = NotApplicable ?? false;
                ViewBag.NotApplicable = isNotApplicable;
                if (isNotApplicable == true)
                {
                    bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Completed", "Completed");
                    var tblcheck = bl.GetPQCandidateChecksOrderBy(PersonalRowId, SubCheckId, clientRowId);
                    if (tblcheck.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblcheck.Rows.Count; i++)
                        {
                            int SubCheckFamilyRowId = Convert.ToInt32(tblcheck.Rows[i]["SubCheckFamilyID"]);
                            bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckFamilyRowId, "Completed", "Completed");
                        }
                    }
                    ViewBag.Success = "Information Updated Successfully";
                    return RedirectToAction("AddAddressInfo", new { NotApplicable = isNotApplicable, msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                }
                else
                {
                    bl.UpdateNACheckDocumentStatusNew(PersonalRowId, SubCheckId, "Pending", "Pending");
                    var tblcheck = bl.GetPQCandidateChecksOrderBy(PersonalRowId, SubCheckId, clientRowId);
                    if (tblcheck.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblcheck.Rows.Count; i++)
                        {
                            int SubCheckFamilyRowId = Convert.ToInt32(tblcheck.Rows[i]["SubCheckFamilyID"]);
                            bl.UpdateNACheckDocumentStatusNew(PersonalRowId, SubCheckFamilyRowId, "Pending", "Pending");
                        }
                    }
                }
                ///////////////////////////////////////////
                DataSet ChecksAndDocStatus = new DataSet();
                ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(PersonalRowId, SubCheckId);
                if (ChecksAndDocStatus != null && ChecksAndDocStatus.Tables.Count > 0 && ChecksAndDocStatus.Tables[0].Rows.Count > 0)
                {
                    ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckInfoStatus"]);
                    ViewBag.CheckDocStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckDocStatus"]);
                }
                else
                {
                    // Handle the case where there are no results
                    ViewBag.CheckInfoStatus = "";
                    ViewBag.CheckDocStatus = "";
                }
                CreateMenuitemforDataEntryOperator(cd.PersonalRowID);
                expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(Convert.ToInt16(cd.SubCheckRowID), PersonalRowId);

            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckid = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            ViewBag.clientno = clientRowId;
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));

            if (ViewBag.ErrorMsg == "")
            {
                if (clientRowId == 559)
                {
                    ViewBag.Success = "Information Updated Successfully.Please now proceed with geo mapping";
                }
                else
                {
                    ViewBag.Success = "Information Updated Successfully";
                }
                return RedirectToAction("AddAddressInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowId });
            }
            else
                return View(marksModel);
        }
        [HttpGet]
        public JsonResult SentCandidatelink(string PersonalRowid = "", string SubCheckId = "", string Address = "", string MobileNo = "")
        {
            string name = "";
            string clientRowId = "";
            string address_type = "";
            string mobile = "";
            string email = "";
            string address = "";
            string reference_number = "";
            string Personalid = "";
            string SubCheckRowId = "";
            string resp = "";
            dynamic expando = new ExpandoObject();
            CandidateDEModel cd = new CandidateDEModel();
            API ccapi = new API();
            try
            {
                Personalid = clsCommonMethods.PDecrypt(PersonalRowid);
                SubCheckRowId = clsCommonMethods.PDecrypt(SubCheckId);
                DataTable dt = ccapi.GettblDataEntryTempDetails(Convert.ToInt32(Personalid), Convert.ToInt32(SubCheckRowId));
                reference_number = "TRF-" + Personalid;
                if (dt.Rows.Count > 0)
                {
                    clientRowId = Convert.ToString(dt.Rows[0]["ClientRowID"]);
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        int AntecedentRowID = Convert.ToInt32(dtRow["AntecedentRowID"]);
                        if (AntecedentRowID == 19)
                        {
                            mobile = Convert.ToString(dtRow["DataEntryValue"]);
                        }
                        else if (AntecedentRowID == 121)
                        {
                            name = Convert.ToString(dtRow["DataEntryValue"]);
                        }
                        else if (AntecedentRowID == 27)
                        {
                            address = Convert.ToString(dtRow["DataEntryValue"]);
                        }
                        else if (AntecedentRowID == 8)
                        {
                            email = Convert.ToString(dtRow["DataEntryValue"]);
                        }
                        if (mobile == "")
                        {
                            if (AntecedentRowID == 131)
                            {
                                mobile = Convert.ToString(dtRow["DataEntryValue"]);
                            }
                        }
                        if (Convert.ToString(dtRow["SubCheckName"]) != "")
                        {
                            address_type = Convert.ToString(dtRow["SubCheckName"]);
                        }
                    }
                    if (name == "")
                    {
                        name = Convert.ToString(dt.Rows[0]["CandidateFullName"]);
                    }
                    if (mobile == "")
                    {
                        mobile = MobileNo;
                    }
                    if (address == "")
                    {
                        address = Address;
                    }
                    if (address_type == "")
                    {
                        address_type = "Address Check Verification";
                    }
                    if (mobile != "")
                    {
                        resp = API.Current_Addreass_Veri_API(name, address_type, mobile, email, address, reference_number);
                        if (resp.Contains("success"))
                        {

                            dynamic data = JsonConvert.DeserializeObject<dynamic>(resp);
                            string url = data.url;
                            ccapi.InsertAPI_AuditDetail(Personalid, Convert.ToInt32(clientRowId), "Success", address_type, "apiResponse: " + resp + " ", "apiResponse", "Walsons API");
                            ViewBag.Success = "success," + url; //"Link sent successfully on you mobile no please check click and completed you verification.";

                        }
                        else
                        {
                            ccapi.InsertAPI_AuditDetail(Personalid, Convert.ToInt32(clientRowId), "Failed", address_type, "apiResponse: " + resp + " ", "apiResponse", "Walsons API");
                            ViewBag.Success = "Failed , " + resp;
                        }
                    }
                    else
                    {
                        ViewBag.Success = "Please enter mobile number,";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Success = "Failed," + ex.Message.ToString();
                ccapi.InsertAPI_AuditDetail(Personalid, Convert.ToInt32(clientRowId), "Failed", address_type, "Exception and apiResponse: " + resp + ex.Message.ToString(), "apiResponse", "Walsons API");
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return Json(ViewBag.Success, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region******* Upload Employment Information *******

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddEmploymentInfo(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "", string msg = "", string isAstraErr = "")
        {
            if (clsCommonMethods.PDecrypt(msg) != "")
            {
                if (isAstraErr == "1")
                    ViewBag.AstraErrorPopup = clsCommonMethods.PDecrypt(msg);
                else
                    ViewBag.Success = clsCommonMethods.PDecrypt(msg);
            }
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            int SubCheckId = 0;
            int clientRowId = 0;
            int PersonalRowId = 0;

            if (IsNumeric(subCheckId))
            {
                SubCheckId = Convert.ToInt32(subCheckId);
            }
            else
            {
                SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            }

            if (IsNumeric(ClientRowId))
            {
                clientRowId = Convert.ToInt32(ClientRowId);
            }
            else
            {
                clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            }

            if (IsNumeric(PersonalRowID))
            {
                PersonalRowId = Convert.ToInt32(PersonalRowID);
            }
            else
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
            }

            if (clientRowId == 559)
            {
                if (clsCommonMethods.PDecrypt(msg) == "")
                {
                    ViewBag.Information = "Please first fill the information, Click on Save, Upload the document, Click on Submit & then click on Next Button";
                }
            }
            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            ViewBag.OrgId = bl.Antecedents_ResearchID(PersonalRowId, SubCheckId, "Candidate");

            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
            DataTable dtcheckstatus = bl.getTempPQCandidateChecksDetail(PersonalRowId, SubCheckId);
            string status = "1";
            if (dtcheckstatus.Rows.Count > 0)
            {
                status = dtcheckstatus.Rows[0]["Status"].ToString();
                if (status == "1")
                {
                    ViewBag.NACheckStatus = false;
                }
                else
                {
                    ViewBag.NACheckStatus = true;
                }
            }
            ViewBag.getviewbagerrormsg = "";
            string Currentlyworkingforthecompany = "";
            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);

            foreach (DataRow d in dt.Rows)
            {

                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (rowsFiltered[0][0].ToString() == "144")
                    {
                        Currentlyworkingforthecompany = Convert.ToString(rowsFiltered[0][1]);
                    }
                    if (rowsFiltered[0][0].ToString() == "89" && Currentlyworkingforthecompany == "No")
                    {
                        if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                        {
                            ViewBag.getviewbagerrormsg = "Data blank";
                        }
                    }
                    else if (rowsFiltered[0][0].ToString() == "580" && Currentlyworkingforthecompany == "No")
                    {
                        if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                        {
                            ViewBag.getviewbagerrormsg = "";
                        }
                    }
                    else if (rowsFiltered[0][0].ToString() == "89" && Currentlyworkingforthecompany == "Yes")
                    {
                        if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                        {
                            ViewBag.getviewbagerrormsg = "Data blank";
                        }
                    }
                    else if (rowsFiltered[0][0].ToString() == "580" && Currentlyworkingforthecompany == "Yes")
                    {
                        if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                        {
                            ViewBag.getviewbagerrormsg = "Data blank";
                        }
                    }
                    else if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                    {
                        if (rowsFiltered[0][0].ToString() != "93")
                        {
                            ViewBag.getviewbagerrormsg = "Data blank";
                        }
                    }
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                        if (d[0].ToString() == "92")
                        {
                            ViewBag.CompanyName = rowsFiltered[0][1].ToString();
                        }

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

            short suncheckid = short.Parse(SubCheckId.ToString());
            expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            //ViewBag.GetDocumentListCandidate = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            //if (ViewBag.GetDocumentListCandidate.Count > 0)
            //{
            //    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            //}
            //else
            //{
            //    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
            //}
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            ViewBag.clientno = clientRowId;
            //International client
            DataTable Candidatedocvisibilty = new DataTable();
            ClientCustomize cc = new ClientCustomize();
            Candidatedocvisibilty = cc.getCandidateDocVisibilty(SubCheckId);
            if (Candidatedocvisibilty.Rows[0]["documentDetailsForVerification"].ToString() == "1")
            {
                ViewBag.Candidatedocvisibilty = "true";
            }
            else
            {
                ViewBag.Candidatedocvisibilty = "false";
                bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            }

            if (ViewBag.CompanyName == null)
            {
                ViewBag.CompanyName = "";
            }
            DataSet ChecksAndDocStatus = new DataSet();
            ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(PersonalRowId, SubCheckId);
            if (ChecksAndDocStatus != null && ChecksAndDocStatus.Tables.Count > 0 && ChecksAndDocStatus.Tables[0].Rows.Count > 0)
            {
                ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckInfoStatus"]);
                ViewBag.CheckDocStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckDocStatus"]);
            }
            else
            {
                // Handle the case where there are no results
                ViewBag.CheckInfoStatus = "";
                ViewBag.CheckDocStatus = "";
            }
            CreateMenuitemforDataEntryOperator(PersonalRowId);
            ClientCustomize blnew = new ClientCustomize();
            DataTable dtpackage = blnew.GetPackageNameByCandidate(PersonalRowId);
            if (dtpackage.Rows.Count > 0)
            {
                ViewBag.packagename = dtpackage.Rows[0][0].ToString();
            }
            return View(expando);
        }


        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddEmploymentInfo(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = 0;
            int SubCheckId = 0;
            int clientRowId = 0;
            // int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            // int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            string clientRowid = formCollection["clientRowId"].ToString();
            // int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            if (IsNumeric(SubCheckid))
            {
                SubCheckId = Convert.ToInt32(SubCheckid);
            }
            else
            {
                SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));
            }

            if (IsNumeric(clientRowid))
            {
                clientRowId = Convert.ToInt32(clientRowid);
            }
            else
            {
                clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));
            }

            if (IsNumeric(PersonalRowid))
            {
                PersonalRowId = Convert.ToInt32(PersonalRowid);
            }
            else
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));
            }

            if (formCollection["Notraised"] != null)
            {
                bool NA = Convert.ToBoolean(formCollection["Notraised"].Split(',')[0]);
                if (NA == true)
                {
                    bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Completed", "Completed");
                    var tblcheck = bl.GetPQCandidateChecksOrderBy(PersonalRowId, SubCheckId, clientRowId);
                    if (tblcheck.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblcheck.Rows.Count; i++)
                        {
                            int SubCheckFamilyRowId = Convert.ToInt32(tblcheck.Rows[i]["SubCheckFamilyID"]);
                            bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckFamilyRowId, "Completed", "Completed");
                        }
                    }
                    ViewBag.Success = "Information Updated Successfully";
                    ViewBag.NACheckStatus = true;
                    var GetNextCheckDT = bl.GetNextCheckName(PersonalRowId, SubCheckId);
                    if (GetNextCheckDT.Rows.Count > 0)
                    {
                        return RedirectToAction(getCheckFormName(GetNextCheckDT.Rows[0]["CheckFamilyRowID"].ToString()), "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(GetNextCheckDT.Rows[0]["SubCheckFamilyID"].ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                    }
                    else
                    {
                        return RedirectToAction("Digital_LOA", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });

                    }

                }
                else
                {
                    bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Pending", "Pending");

                    var tblcheck = bl.GetPQCandidateChecksOrderBy(PersonalRowId, SubCheckId, clientRowId);
                    if (tblcheck.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblcheck.Rows.Count; i++)
                        {
                            int SubCheckFamilyRowId = Convert.ToInt32(tblcheck.Rows[i]["SubCheckFamilyID"]);
                            bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckFamilyRowId, "Pending", "Pending");
                        }
                    }
                    ViewBag.NACheckStatus = false;
                }
            }

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            Int32 OrganizationId = 0;
            try
            {
                OrganizationId = Convert.ToInt32(formCollection["OrganizationId"].ToString());
            }
            catch { };
            saveEmpidInTempCandidateTable(PersonalRowId, SubCheckId, OrganizationId);

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());


                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                string Currentlyworkingforthecompany = "";
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (key == "144")
                            {
                                Currentlyworkingforthecompany = formCollection[key].ToString();
                            }
                            if (key == "580")
                            {
                                if (Currentlyworkingforthecompany == "Yes")
                                {

                                    if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                    {
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                    }
                                }

                            }
                            else if (key == "89")
                            {
                                if (Currentlyworkingforthecompany == "No")
                                {
                                    if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                    {
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                    }
                                }
                            }
                            else if (d[4].ToString() == "textbox")
                            {

                                if (FString[0] == "93") //other remarks 
                                {
                                    if (OrganizationId == 0 && cd.DataEntryValue == "")
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + " Organization name or Other Employer must be filled";
                                }
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }

                                Regex regex = new Regex(@"^[a-zA-Z0-9\s\-_(),.'@/\\]*$");
                                if (!regex.IsMatch(cd.DataEntryValue))
                                {
                                    ViewBag.ErrorMsg = "Special characters are not allowed";
                                    TempData["ErrorMessage"] = "Special characters are not allowed";
                                    return RedirectToAction("AddEmploymentInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                                }
                                if (key == "147" || key == "150")
                                {
                                    string mobileNo = cd.DataEntryValue;
                                    if (mobileNo.Length > 10)
                                    {
                                        ViewBag.ErrorMsg = "Please enter 10-digit Mobile Number";
                                        TempData["ErrorMessage"] = "Please enter 10-digit Mobile Number";
                                        return RedirectToAction("AddEmploymentInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                                    }
                                }
                            }
                            else if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }
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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                        if (FString[0] == "92")//OrganizationName
                        {
                            cd.DataEntryValue = formCollection["OrganizationName"].ToString();
                            cd.DisplayName = FString[1];
                            marksModel.Add(d[1].ToString(), cd.DataEntryValue);
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            bl.SaveDataEntry_Candidate(cd);
                        }
                        else if (FString[0] == "82") //Address of the company
                        {
                            if (cd.clientRowId == 1415 || cd.clientRowId == 1416)
                            {
                                //cd.DataEntryValue = formCollection["82"].ToString();
                                cd.DataEntryValue = formCollection["Addressofthecompany"].ToString();
                                cd.DisplayName = FString[1];
                                marksModel.Add(d[1].ToString(), cd.DataEntryValue);
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                bl.SaveDataEntry_Candidate(cd);
                            }
                            else if (cd.clientRowId != 1395)
                            {
                                cd.DataEntryValue = formCollection["Addressofthecompany"].ToString();
                                cd.DisplayName = FString[1];
                                marksModel.Add(d[1].ToString(), cd.DataEntryValue);
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                bl.SaveDataEntry_Candidate(cd);
                            }

                        }
                        else
                        {
                            foreach (string key in formCollection.AllKeys)
                            {
                                if (key == d[0].ToString())
                                {
                                    marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                    cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = formCollection[key].ToString();
                                    cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                    bl.SaveDataEntry_Candidate(cd);
                                }
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
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }
                if (clientRowId == 559)
                {
                    var FutureAndPastdateDiff = bl.GetEMPFutureAndPastdateDiff(Convert.ToInt32(cd.PersonalRowID), cd.SubCheckRowID);
                    if (FutureAndPastdateDiff.Rows.Count > 0)
                    {
                        int DATE_DIFF = Convert.ToInt32(FutureAndPastdateDiff.Rows[0]["DATE_DIFF"]);
                        int SubCheckId1 = Convert.ToInt32(FutureAndPastdateDiff.Rows[0]["SubCheckId1"]);
                        int SubCheckId2 = Convert.ToInt32(FutureAndPastdateDiff.Rows[0]["SubCheckId2"]);
                        if (DATE_DIFF > 0)
                        {
                            ViewBag.ErrorMsg = "There should not be contradiction between future and previous tenure date";
                        }
                        else if (DATE_DIFF < -31)
                        {
                            ViewBag.ErrorMsg = "Provided tenure is not in continuation to previous updated address as there is gap in between the submitted dates.";
                        }
                    }
                }
                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                DataSet ChecksAndDocStatus = new DataSet();
                ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(PersonalRowId, SubCheckId);
                if (ChecksAndDocStatus != null && ChecksAndDocStatus.Tables.Count > 0 && ChecksAndDocStatus.Tables[0].Rows.Count > 0)
                {
                    ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckInfoStatus"]);
                    ViewBag.CheckDocStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckDocStatus"]);
                }
                else
                {
                    // Handle the case where there are no results
                    ViewBag.CheckInfoStatus = "";
                    ViewBag.CheckDocStatus = "";
                }
                CreateMenuitemforDataEntryOperator(cd.PersonalRowID);
                expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(Convert.ToInt16(cd.SubCheckRowID), PersonalRowId);

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
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            ViewBag.clientno = clientRowId;
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return RedirectToAction("AddEmploymentInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowId });
            }
            else
                return View(marksModel);
        }

        #endregion

        #region******* Upload Education Information *******

        [HttpGet]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult AddEducationInfo(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "", string msg = "", string isAstraErr = "")
        {
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            if (msg != "")
            {
                if (isAstraErr == "1")
                {
                    ViewBag.AstraErrorPopup = clsCommonMethods.PDecrypt(msg);
                }
                else
                {
                    ViewBag.Success = clsCommonMethods.PDecrypt(msg);
                }
            }
            else
            {
                if (clientRowId == 559)
                {
                    ViewBag.Information = "Please first fill the information, Click on Save, Upload the document, Click on Submit & then click on Next Button";
                }
            }

            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            ViewBag.EduID = bl.Antecedents_ResearchID(PersonalRowId, SubCheckId, "Candidate");
            ViewBag.DecryptSubcheckId = SubCheckId;
            ViewBag.getviewbagerrormsg = "";
            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
            //====================================
            DataTable dtcheckstatus = bl.getTempPQCandidateChecksDetail(PersonalRowId, SubCheckId);
            string status = "1";
            if (dtcheckstatus.Rows.Count > 0)
            {
                status = dtcheckstatus.Rows[0]["Status"].ToString();
                if (status == "1")
                {
                    ViewBag.NACheckStatus = false;
                }
                else
                {
                    ViewBag.NACheckStatus = true;
                }
            }


            //===================================
            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);

            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                    {
                        if (rowsFiltered[0][0].ToString() != "77")
                        {
                            ViewBag.getviewbagerrormsg = "Data blank";
                        }
                    }

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                        if (d[1].ToString().Contains("72"))
                        {
                            ViewBag.degreeName = rowsFiltered[0][1].ToString().Trim();
                        }
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
                        if (d[1].ToString().Contains("72"))
                        {
                            ViewBag.degreeName = "";

                        }
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }

            short suncheckid = short.Parse(SubCheckId.ToString());
            expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            //International client
            DataTable Candidatedocvisibilty = new DataTable();
            ClientCustomize cc = new ClientCustomize();
            Candidatedocvisibilty = cc.getCandidateDocVisibilty(SubCheckId);
            if (Candidatedocvisibilty.Rows[0]["documentDetailsForVerification"].ToString() == "1")
            {
                ViewBag.Candidatedocvisibilty = "true";
            }
            else
            {
                ViewBag.Candidatedocvisibilty = "false";
                bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            }
            DataSet ChecksAndDocStatus = new DataSet();
            ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(PersonalRowId, SubCheckId);
            if (ChecksAndDocStatus != null && ChecksAndDocStatus.Tables.Count > 0 && ChecksAndDocStatus.Tables[0].Rows.Count > 0)
            {
                ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckInfoStatus"]);
                ViewBag.CheckDocStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckDocStatus"]);
            }
            else
            {
                // Handle the case where there are no results
                ViewBag.CheckInfoStatus = "";
                ViewBag.CheckDocStatus = "";
            }
            CreateMenuitemforDataEntryOperator(PersonalRowId);
            ClientCustomize blnew = new ClientCustomize();
            DataTable dtpackage = blnew.GetPackageNameByCandidate(PersonalRowId);
            if (dtpackage.Rows.Count > 0)
            {
                ViewBag.packagename = dtpackage.Rows[0][0].ToString();
            }
            return View(expando);
        }


        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult AddEducationInfo(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));
            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            string clientRowid = formCollection["clientRowId"].ToString();
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));

            ViewBag.DecryptSubcheckId = SubCheckId;
            string Degreeid = formCollection["72"].ToString();

            string DegreeName = "";
            DataTable DtDegree = new DataTable();

            if (DtDegree.Rows.Count > 0)
            {
                DegreeName = DtDegree.Rows[0]["DegreeType"].ToString();
            }

            //================================================
            if (formCollection["Notraised"] != null)
            {
                bool NA = Convert.ToBoolean(formCollection["Notraised"].Split(',')[0]);
                if (NA == true)
                {
                    bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Completed", "Completed");
                    var tblcheck = bl.GetPQCandidateChecksOrderBy(PersonalRowId, SubCheckId, clientRowId);
                    if (tblcheck.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblcheck.Rows.Count; i++)
                        {
                            int SubCheckFamilyRowId = Convert.ToInt32(tblcheck.Rows[i]["SubCheckFamilyID"]);
                            bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckFamilyRowId, "Completed", "Completed");
                        }
                    }
                    ViewBag.Success = "Information Updated Successfully";
                    ViewBag.NACheckStatus = true;
                    return RedirectToAction("AddEmploymentInfo", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                }
                else
                {
                    bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Pending", "Pending");

                    var tblcheck = bl.GetPQCandidateChecksOrderBy(PersonalRowId, SubCheckId, clientRowId);
                    if (tblcheck.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblcheck.Rows.Count; i++)
                        {
                            int SubCheckFamilyRowId = Convert.ToInt32(tblcheck.Rows[i]["SubCheckFamilyID"]);
                            bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckFamilyRowId, "Pending", "Pending");
                        }
                    }
                    ViewBag.NACheckStatus = false;
                }
            }


            //================================================
            Int32 CollegeId = 0;
            try
            {
                string[] A = formCollection["CollegeId"].ToString().Split(',');
                if (!string.IsNullOrEmpty(A[0]))
                {
                    CollegeId = Convert.ToInt32(A[0].ToString());
                }

            }
            catch (Exception ex)
            {

            }
            if (CollegeId == 0)
            {
                string CollegeName = formCollection["CollegeName"].ToString();
                DataTable dtCollegeId = new DataTable();
                dtCollegeId = bl.getCollegelistNameByCollegeID(CollegeName);
                if (dtCollegeId.Rows.Count > 0)
                {
                    CollegeId = Convert.ToInt32(dtCollegeId.Rows[0]["CollegeId"].ToString());
                }
            }
            saveEmpidInTempCandidateTable(PersonalRowId, SubCheckId, CollegeId);

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[0] == "77") //other remarks 
                                {
                                    if (CollegeId == 0 && cd.DataEntryValue == "")
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + " College/Institute name or Other Institute must be filled";
                                }
                                else
if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }

                            Regex regex = new Regex(@"^[a-zA-Z0-9\s\-_(),.'@/&\\]*$");
                            if (!regex.IsMatch(cd.DataEntryValue))
                            {
                                ViewBag.ErrorMsg = "Special characters are not allowed";
                                TempData["ErrorMessage"] = "Special characters are not allowed";
                                return RedirectToAction("AddEducationInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                            }
                        }
                    }
                }

                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }

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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                        if (FString[0] == "69")       //"College/Institute/School Name"
                        {
                            cd.DataEntryValue = formCollection["CollegeName"].ToString();
                            cd.DisplayName = FString[1];
                            marksModel.Add(d[1].ToString(), cd.DataEntryValue);
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            bl.SaveDataEntry_Candidate(cd);
                        }
                        else if (FString[0] == "70")  //"School Address"
                        {
                            cd.DataEntryValue = formCollection["CollegeAddress"].ToString();
                            cd.DisplayName = FString[1];
                            marksModel.Add(d[1].ToString(), cd.DataEntryValue);
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            bl.SaveDataEntry_Candidate(cd);
                        }
                        else
                        {
                            foreach (string key in formCollection.AllKeys)
                            {
                                if (key == d[0].ToString())
                                {
                                    marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                    cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = formCollection[key].ToString();
                                    cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                    bl.SaveDataEntry_Candidate(cd);
                                }
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
                                //Hfdegreeid




                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                if (cd.AntecedentRowID == 72)
                                {
                                    cd.DataEntryValue = DegreeName;

                                    ViewBag.degreeName = DegreeName;

                                }
                                else
                                {
                                    cd.DataEntryValue = formCollection[key].ToString();
                                }
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }
                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                //====================================
                DataTable dtcheckstatus = bl.getTempPQCandidateChecksDetail(PersonalRowId, SubCheckId);
                string status = "1";
                if (dtcheckstatus.Rows.Count > 0)
                {
                    status = dtcheckstatus.Rows[0]["Status"].ToString();
                    if (status == "1")
                    {
                        ViewBag.NACheckStatus = false;
                    }
                    else
                    {
                        ViewBag.NACheckStatus = true;
                    }
                }


                //===================================
                CreateMenuitemforDataEntryOperator(cd.PersonalRowID);
                expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(Convert.ToInt16(cd.SubCheckRowID), PersonalRowId);
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
            //ViewBag.degreeid = formCollection["Hfdegreeid"].ToString();
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            DataSet ChecksAndDocStatus = new DataSet();
            ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(PersonalRowId, SubCheckId);
            if (ChecksAndDocStatus != null && ChecksAndDocStatus.Tables.Count > 0 && ChecksAndDocStatus.Tables[0].Rows.Count > 0)
            {
                ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckInfoStatus"]);
                ViewBag.CheckDocStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckDocStatus"]);
            }
            else
            {
                // Handle the case where there are no results
                ViewBag.CheckInfoStatus = "";
                ViewBag.CheckDocStatus = "";
            }
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return RedirectToAction("AddEducationInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowId });
            }
            else
                return View(marksModel);
        }


        // [HttpPost]
        public ActionResult UpdateFormFill(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "", string msg = "", string CheckFamilyId = "", string ClientRowIdss = "", string PersonalRowIDss = "", string subCheckIdss = "", Int64 id = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            ViewBag.ErrorMsg = ViewBag.ErrorMsg;
            ViewBag.getviewbagerrormsg = ViewBag.getviewbagerrormsg;

            int clientRowId, SubCheckId, PersonalRowId;
            //int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            if (ClientRowIdss == "" || ClientRowIdss == null)
            {
                clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            }
            else
            {
                clientRowId = Convert.ToInt32(PersonalRowIDss);
            }
            if (PersonalRowIDss == "" || PersonalRowIDss == null)
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
            }
            else
            {
                PersonalRowId = Convert.ToInt32(subCheckIdss);
            }
            if (subCheckIdss == "" || subCheckIdss == null)
            {
                SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            }
            else
            {
                SubCheckId = Convert.ToInt32(ClientRowIdss);
            }


            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            ViewBag.EduID = bl.Antecedents_ResearchID(PersonalRowId, SubCheckId, "Candidate");

            short suncheckid = short.Parse(SubCheckId.ToString());

            ///for Ford moters              
            if (clientRowId == 753)
            {
                if (SubCheckId == 799)
                {
                    GetFaceMatchAndUploadArtifact(Convert.ToInt32(PersonalRowId), SubCheckId, clientRowId);
                }
            }


            expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            ViewBag.GetDocumentListCandidate = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            bool astraBlock = false;
            if (clientRowId == 957 && CheckFamilyId == "11")
            {
                bool hasDegree = false, hasMarksheet = false;
                foreach (var docItem in ViewBag.GetDocumentListCandidate)
                {
                    string dType = Convert.ToString(docItem.DocumentType);
                    if (dType == "Degree_Certificate") hasDegree = true;
                    if (dType == "Final_Marksheet") hasMarksheet = true;
                }
                if (!hasDegree || !hasMarksheet)
                {
                    astraBlock = true;
                    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
                    List<string> miss = new List<string>();
                    if (!hasMarksheet) miss.Add("Final Marksheet");
                    if (!hasDegree) miss.Add("Degree Certificate");
                    TempData["AstraError"] = "✗ Please upload: " + string.Join(", ", miss);
                    //TempData["AstraError"] = "Please upload: " + string.Join(", ", miss);
                }
            }

            bool daimlerBlock = false;
            if (clientRowId == 1540 && CheckFamilyId == "12" && SubCheckId == 68)
            {
                bool hasAppointment = false;
                foreach (var docItem in ViewBag.GetDocumentListCandidate)
                {
                    string dType = Convert.ToString(docItem.DocumentType);
                    if (dType == "Appointment_Letter") hasAppointment = true;
                }
                if (!hasAppointment)
                {
                    daimlerBlock = true;
                    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
                    TempData["AstraError"] = "✗ Please upload: Appointment Letter";
                }
            }
            bool daimlerEduBlock = false;
            if (clientRowId == 1540 && CheckFamilyId == "11")
            {
                bool hasGrade = false, hasDegree = false;
                foreach (var docItem in ViewBag.GetDocumentListCandidate)
                {
                    string dType = Convert.ToString(docItem.DocumentType);
                    if (dType == "Grade_Sheet") hasGrade = true;
                    if (dType == "Degree_Certificate") hasDegree = true;
                }
                if (!hasGrade || !hasDegree)
                {
                    daimlerEduBlock = true;
                    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
                    List<string> miss = new List<string>();
                    if (!hasGrade) miss.Add("Grade Sheet");
                    if (!hasDegree) miss.Add("Degree Certificate");
                    TempData["AstraError"] = "✗ Please upload: " + string.Join(", ", miss);
                }
            }

            if (clientRowId == 1540 && CheckFamilyId == "12" && SubCheckId >= 69 && SubCheckId <= 79)
            {
                bool hasExperience = false;
                foreach (var docItem in ViewBag.GetDocumentListCandidate)
                {
                    string dType = Convert.ToString(docItem.DocumentType);
                    if (dType == "Experience_Letter") hasExperience = true;
                }
                if (!hasExperience)
                {
                    daimlerBlock = true;
                    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
                    TempData["AstraError"] = "✗ Please upload: Experience Letter";
                }
            }
            bool daimlerAddrBlock = false;
            if (clientRowId == 1540 && CheckFamilyId == "2")
            {
                if (ViewBag.GetDocumentListCandidate.Count == 0)
                {
                    daimlerAddrBlock = true;
                    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
                    TempData["AstraError"] = "✗ One document is mandatory";
                }
            }

            if (daimlerAddrBlock)
            {
                GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
                ViewBag.PersonalRowid = PersonalRowID;
                ViewBag.SubCheckId = subCheckId;
                ViewBag.clientRowId = ClientRowId;
                CreateMenuitemforDataEntryOperator(PersonalRowId);
                return RedirectToAction("AddAddressInfo", "Home", new
                {
                    msg = clsCommonMethods.PEncrypt(Convert.ToString(TempData["AstraError"])),
                    subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()),
                    ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()),
                    PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()),
                    isAstraErr = "1"
                });
            }

            if (daimlerBlock)
            {
                GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
                ViewBag.PersonalRowid = PersonalRowID;
                ViewBag.SubCheckId = subCheckId;
                ViewBag.clientRowId = ClientRowId;
                CreateMenuitemforDataEntryOperator(PersonalRowId);
                return RedirectToAction("AddEmploymentInfo", "Home", new
                {
                    msg = clsCommonMethods.PEncrypt(Convert.ToString(TempData["AstraError"])),
                    subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()),
                    ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()),
                    PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()),
                    isAstraErr = "1"
                });
            }

            if (daimlerEduBlock)
            {
                GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
                ViewBag.PersonalRowid = PersonalRowID;
                ViewBag.SubCheckId = subCheckId;
                ViewBag.clientRowId = ClientRowId;
                CreateMenuitemforDataEntryOperator(PersonalRowId);
                return RedirectToAction("AddEducationInfo", "Home", new
                {
                    msg = clsCommonMethods.PEncrypt(Convert.ToString(TempData["AstraError"])),
                    subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()),
                    ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()),
                    PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()),
                    isAstraErr = "1"
                });

            }

            if (astraBlock)
            {
                GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
                ViewBag.PersonalRowid = PersonalRowID;
                ViewBag.SubCheckId = subCheckId;
                ViewBag.clientRowId = ClientRowId;
                CreateMenuitemforDataEntryOperator(PersonalRowId);
                return RedirectToAction("AddEducationInfo", "Home", new
                {
                    msg = clsCommonMethods.PEncrypt(Convert.ToString(TempData["AstraError"])),
                    subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()),
                    ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()),
                    PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()),
                    isAstraErr = "1"
                });
                //return RedirectToAction("AddEducationInfo", "Home", new
                //{
                //    msg = clsCommonMethods.PEncrypt(Convert.ToString(TempData["AstraError"])),
                //    subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()),
                //    ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()),
                //    PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString())
                //});
            }
            else if (ViewBag.GetDocumentListCandidate.Count > 0)
            {
                bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
                msg = "Information uploded Succesfully";
            }
            else
            {
                string[] validIds = { "25", "26", "27", "30", "34",
                    "35", "36", "303", "307", "655", "656", "657",
                    "658", "659", "660", "896", "897", "898", "899", "900",
                "4", "5", "6", "7", "8", "9", "10", "11", "12", "689", "690", "691", "692" };


                if (clientRowId == 559)
                {
                    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
                }
                else if (validIds.Contains(SubCheckId.ToString()))
                {
                    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
                    msg = "Please fill the details";
                }

            }
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            CreateMenuitemforDataEntryOperator(PersonalRowId);

            DataSet d = new DataSet();
            if (clientRowId == 559)
            {
                d = tpc.AutoCheckInitiation_proc(PersonalRowId);

            }
            else
            {
                d = tpc.GetTempCandidateChecksDT(PersonalRowId);
            }
            string CheckFamilyRowID = "";
            string subcheckrowids = "";
            if (d.Tables[1].Rows.Count > 0)
            {
                var match = false;

                for (int i = 0; i < d.Tables[1].Rows.Count; i++)
                {
                    if (match == true)
                    {
                        subcheckrowids = Convert.ToString(d.Tables[1].Rows[i]["SubCheckFamilyID"]);
                        CheckFamilyRowID = Convert.ToString(d.Tables[1].Rows[i]["CheckFamilyRowID"]);
                        break;
                    }
                    if (clsCommonMethods.PDecrypt(subCheckId) == Convert.ToString(d.Tables[1].Rows[i]["SubCheckFamilyID"]))
                    {
                        match = true;
                    }

                }
            }
            return RedirectToAction(getCheckFormName(CheckFamilyRowID, subcheckrowids), "Home", new { subCheckId = clsCommonMethods.PEncrypt(subcheckrowids), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });

        }

        public string getCheckFormName(string CheckFamilyRowID,string SubCheckId="")
        {
            string trans = "";

            if (CheckFamilyRowID == "11")
            {
                trans = "AddEducationInfo";
            }
            else if (CheckFamilyRowID == "2")
            {
                trans = "AddAddressInfo";
            }
            else if (CheckFamilyRowID == "5")
            {
                trans = "AddCriminalInfo";
            }
            else if (CheckFamilyRowID == "12")
            {
                trans = "AddEmploymentInfo";
            }
            else if (CheckFamilyRowID == "26")
            {
                trans = "AddNationalIdentityInfo";
            }
            else if (CheckFamilyRowID == "30")
            {
                trans = "AddReferenceInfo";
            }
            else if (CheckFamilyRowID == "40")
            {
                if(SubCheckId=="890")
                {
                    trans = "AddlivenessInfo";
                }
                else
                {
                    trans = "AddSpecialCheckInfo";
                }
            }
            else
            {
                trans = "Digital_LOA";
            }
            return trans;

        }
        #endregion

        #region******* Upload Criminal Information *******

        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult AddCriminalInfo(bool? NotApplicable = false, string ClientRowId = "", string PersonalRowID = "", string subCheckId = "", string ErrorMsg = "", string msg = "")
        {
            if (clsCommonMethods.PDecrypt(msg) != "")
            {
                ViewBag.Success = clsCommonMethods.PDecrypt(msg);
            }

            ViewBag.NotApplicable = NotApplicable ?? false;
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
            if (clientRowId == 559)
            {
                if (clsCommonMethods.PDecrypt(msg) == "")
                {
                    ViewBag.Information = "Please First Fill The Information Click On Submit Then Click On Next Button";
                }
            }
            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            ViewBag.getviewbagerrormsg = "";
            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
            if (dt.Rows.Count > 0)
            {
                DataTable dbValue = new DataTable();
                dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);
                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                        {
                            ViewBag.getviewbagerrormsg = "Data blank";
                        }
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
            }

            short suncheckid = short.Parse(SubCheckId.ToString());
            expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            //ViewBag.GetDocumentListCandidate = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            //if (ViewBag.GetDocumentListCandidate.Count > 0)
            //{
            //    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            //}
            //else
            //{
            //    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
            //}

            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            CreateMenuitemforDataEntryOperator(PersonalRowId);


            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;

            //International client
            DataTable Candidatedocvisibilty = new DataTable();
            ClientCustomize cc = new ClientCustomize();
            Candidatedocvisibilty = cc.getCandidateDocVisibilty(SubCheckId);
            if (Candidatedocvisibilty.Rows[0]["documentDetailsForVerification"].ToString() == "1")
            {
                ViewBag.Candidatedocvisibilty = "true";
            }
            else
            {
                ViewBag.Candidatedocvisibilty = "false";
                bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            }
            DataSet ChecksAndDocStatus = new DataSet();
            ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(PersonalRowId, SubCheckId);
            if (ChecksAndDocStatus != null && ChecksAndDocStatus.Tables.Count > 0 && ChecksAndDocStatus.Tables[0].Rows.Count > 0)
            {
                ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckInfoStatus"]);
                ViewBag.CheckDocStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckDocStatus"]);
            }
            else
            {
                // Handle the case where there are no results
                ViewBag.CheckInfoStatus = "";
                ViewBag.CheckDocStatus = "";
            }
            ClientCustomize blnew = new ClientCustomize();
            DataTable dtpackage = blnew.GetPackageNameByCandidate(PersonalRowId);
            if (dtpackage.Rows.Count > 0)
            {
                ViewBag.packagename = dtpackage.Rows[0][0].ToString();
            }
            return View(expando);
        }

        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult AddCriminalInfo(FormCollection formCollection, bool? NotApplicable)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));
            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            string clientRowid = formCollection["clientRowId"].ToString();
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }

                                Regex regex = new Regex(@"^[a-zA-Z0-9\s\-_(),.'@/\\]*$");
                                if (!regex.IsMatch(cd.DataEntryValue))
                                {
                                    ViewBag.ErrorMsg = "Special characters are not allowed";
                                    TempData["ErrorMessage"] = "Special characters are not allowed";
                                    return RedirectToAction("AddCriminalInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }
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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                                cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                bl.SaveDataEntry_Candidate(cd);
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
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }
                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                ///////////////////////////////////////////
                DataSet ChecksAndDocStatus = new DataSet();
                ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(PersonalRowId, SubCheckId);
                if (ChecksAndDocStatus != null && ChecksAndDocStatus.Tables.Count > 0 && ChecksAndDocStatus.Tables[0].Rows.Count > 0)
                {
                    ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckInfoStatus"]);
                    ViewBag.CheckDocStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckDocStatus"]);
                }
                else
                {
                    // Handle the case where there are no results
                    ViewBag.CheckInfoStatus = "";
                    ViewBag.CheckDocStatus = "";
                }

                bool isNotApplicable = NotApplicable ?? false;
                ViewBag.NotApplicable = isNotApplicable;
                if (isNotApplicable == true)
                {
                    bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Completed", "Completed");
                    var tblcheck = bl.GetPQCandidateChecksOrderBy(PersonalRowId, SubCheckId, clientRowId);
                    if (tblcheck.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblcheck.Rows.Count; i++)
                        {
                            int SubCheckFamilyRowId = Convert.ToInt32(tblcheck.Rows[i]["SubCheckFamilyID"]);
                            bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckFamilyRowId, "Completed", "Completed");
                        }
                    }
                    ViewBag.Success = "Information Updated Successfully";
                    return RedirectToAction("AddCriminalInfo", new { NotApplicable = isNotApplicable, msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                }
                else
                {
                    bl.UpdateNACheckDocumentStatusNew(PersonalRowId, SubCheckId, "Pending", "Pending");
                    var tblcheck = bl.GetPQCandidateChecksOrderBy(PersonalRowId, SubCheckId, clientRowId);
                    if (tblcheck.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblcheck.Rows.Count; i++)
                        {
                            int SubCheckFamilyRowId = Convert.ToInt32(tblcheck.Rows[i]["SubCheckFamilyID"]);
                            bl.UpdateNACheckDocumentStatusNew(PersonalRowId, SubCheckFamilyRowId, "Pending", "Pending");
                        }
                    }
                }
                ///////////////////////////////////////////

                CreateMenuitemforDataEntryOperator(cd.PersonalRowID);
                expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(Convert.ToInt16(cd.SubCheckRowID), PersonalRowId);
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
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            //ViewBag.Success = "Information Updated Successfully";
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return RedirectToAction("AddCriminalInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowId });
            }
            else
                return View(marksModel);
        }
        #endregion

        #region******* Upload National Identity Information *******

        [HttpGet]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult AddNationalIdentityInfo(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "", string msg = "")
        {
            if (clsCommonMethods.PDecrypt(msg) != "")
            {
                ViewBag.Success = clsCommonMethods.PDecrypt(msg);
            }

            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));

            if (clientRowId == 559)
            {
                if (clsCommonMethods.PDecrypt(msg) == "")
                {
                    ViewBag.Information = "Please First Fill The Information Click On Submit Then Click On Next Button";
                }
            }

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

            ViewBag.getviewbagerrormsg = "";
            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);


            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                    {
                        ViewBag.getviewbagerrormsg = "Data blank";
                    }
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
                    //marksModel.Add("Name_none", studentName);
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
            short suncheckid = short.Parse(SubCheckId.ToString());


            expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            //ViewBag.GetDocumentListCandidate = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            //if (ViewBag.GetDocumentListCandidate.Count > 0)
            //{
            //    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            //}
            //else
            //{
            //    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
            //}
            //SubCheckId
            Session["SubCheckRowID"] = SubCheckId;
            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            CreateMenuitemforDataEntryOperator(PersonalRowId);
            //International client
            DataTable Candidatedocvisibilty = new DataTable();
            ClientCustomize cc = new ClientCustomize();
            Candidatedocvisibilty = cc.getCandidateDocVisibilty(SubCheckId);
            if (Candidatedocvisibilty.Rows[0]["documentDetailsForVerification"].ToString() == "1")
            {
                ViewBag.Candidatedocvisibilty = "true";
            }
            else
            {
                ViewBag.Candidatedocvisibilty = "false";
                bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            }
            DataSet ChecksAndDocStatus = new DataSet();
            ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(PersonalRowId, SubCheckId);
            if (ChecksAndDocStatus != null && ChecksAndDocStatus.Tables.Count > 0 && ChecksAndDocStatus.Tables[0].Rows.Count > 0)
            {
                ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckInfoStatus"]);
                ViewBag.CheckDocStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckDocStatus"]);
            }
            else
            {
                // Handle the case where there are no results
                ViewBag.CheckInfoStatus = "";
                ViewBag.CheckDocStatus = "";
            }
            ClientCustomize blnew = new ClientCustomize();
            DataTable dtpackage = blnew.GetPackageNameByCandidate(PersonalRowId);
            if (dtpackage.Rows.Count > 0)
            {
                ViewBag.packagename = dtpackage.Rows[0][0].ToString();
            }
            return View(expando);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult AddNationalIdentityInfo(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            string clientRowid = formCollection["clientRowId"].ToString();
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                ViewBag.ErrorMsg = "";

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                                Regex regex = new Regex(@"^[a-zA-Z0-9\s\-_(),.'@/\\]*$");
                                if (!regex.IsMatch(cd.DataEntryValue))
                                {
                                    ViewBag.ErrorMsg = "Special characters are not allowed";
                                    TempData["ErrorMessage"] = "Special characters are not allowed";
                                    return RedirectToAction("AddNationalIdentityInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }
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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                                cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                bl.SaveDataEntry_Candidate(cd);
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
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                } 



                expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(Convert.ToInt16(cd.SubCheckRowID), PersonalRowId);
                CreateMenuitemforDataEntryOperator(cd.PersonalRowID);
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
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            //ViewBag.Success = "Information Updated Successfully";
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                //return View(marksModel, new {  subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowId });
                return RedirectToAction("AddNationalIdentityInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowId });
            }
            else
                return View(marksModel);
        }

        #endregion

        #region******* Upload Reference Information *******


        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult AddReferenceInfo(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));

            if (clientRowId == 559)
            {
                ViewBag.Information = "Please First Fill The Information Click On Submit Then Click On Next Button";
            }

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);

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
                    //marksModel.Add("Name_none", studentName);                    
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

            short suncheckid = short.Parse(SubCheckId.ToString());
            expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            ViewBag.GetDocumentListCandidate = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            if (ViewBag.GetDocumentListCandidate.Count > 0)
            {
                bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            }
            else
            {
                bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
            }

            //International client
            DataTable Candidatedocvisibilty = new DataTable();
            ClientCustomize cc = new ClientCustomize();
            Candidatedocvisibilty = cc.getCandidateDocVisibilty(SubCheckId);
            if (Candidatedocvisibilty.Rows[0]["documentDetailsForVerification"].ToString() == "1")
            {
                ViewBag.Candidatedocvisibilty = "true";
            }
            else
            {
                ViewBag.Candidatedocvisibilty = "false";
                bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            }
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            DataSet ChecksAndDocStatus = new DataSet();
            ChecksAndDocStatus = bl.ChekcsInfoAndCheckDocInfoStatus(PersonalRowId, SubCheckId);
            if (ChecksAndDocStatus != null && ChecksAndDocStatus.Tables.Count > 0 && ChecksAndDocStatus.Tables[0].Rows.Count > 0)
            {
                ViewBag.CheckInfoStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckInfoStatus"]);
                ViewBag.CheckDocStatus = Convert.ToString(ChecksAndDocStatus.Tables[0].Rows[0]["CheckDocStatus"]);
            }
            else
            {
                // Handle the case where there are no results
                ViewBag.CheckInfoStatus = "";
                ViewBag.CheckDocStatus = "";
            }
            CreateMenuitemforDataEntryOperator(PersonalRowId);
            ClientCustomize blnew = new ClientCustomize();
            DataTable dtpackage = blnew.GetPackageNameByCandidate(PersonalRowId);
            if (dtpackage.Rows.Count > 0)
            {
                ViewBag.packagename = dtpackage.Rows[0][0].ToString();
            }
            return View(expando);
        }


        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddReferenceInfo(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = 0;
            if (PersonalRowid != "")
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));
            }
            //int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            string clientRowid = formCollection["clientRowId"].ToString();
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }

                            Regex regex = new Regex(@"^[a-zA-Z0-9\s\-_(),.'@/\\]*$");
                            if (!regex.IsMatch(cd.DataEntryValue))
                            {
                                ViewBag.ErrorMsg = "Special characters are not allowed";
                                TempData["ErrorMessage"] = "Special characters are not allowed";
                                return RedirectToAction("AddReferenceInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                            }

                            if (key == "177")
                            {
                                string mobileNo = cd.DataEntryValue;
                                if (mobileNo.Length > 10)
                                {
                                    ViewBag.ErrorMsg = "Please enter 10-digit Mobile Number";
                                    TempData["ErrorMessage"] = "Please enter 10-digit Mobile Number";
                                    return RedirectToAction("AddReferenceInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                                }
                            }

                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }

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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        // postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                                cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                bl.SaveDataEntry_Candidate(cd);
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
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                CreateMenuitemforDataEntryOperator(cd.PersonalRowID);
                expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(Convert.ToInt16(cd.SubCheckRowID), PersonalRowId);
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
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return View(marksModel);
            }
            else
                return View(marksModel);
        }
        #endregion

        #region ******* Add Special Check Information By Candidate *******

        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult AddSpecialCheckInfo(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "", string msg = "", bool? NotApplicable = false)
        {
            ViewBag.NotApplicable = NotApplicable ?? false;
            if (clsCommonMethods.PDecrypt(msg) != "")
            {
                ViewBag.Success = clsCommonMethods.PDecrypt(msg);
            }
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));

            if (clientRowId == 559)
            {
                if (clsCommonMethods.PDecrypt(msg) == "")
                {
                    ViewBag.Information = "Please First Fill The Information Click On Submit Then Click On Next Button";
                }
            }

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

            ViewBag.getviewbagerrormsg = "";
            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);

            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {

                    if (Convert.ToString(rowsFiltered[0][1]) == "" && d[5].ToString() == "mandatory")
                    {
                        ViewBag.getviewbagerrormsg = "Data blank";
                    }
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

            short suncheckid = short.Parse(SubCheckId.ToString());
            expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            //ViewBag.GetDocumentListCandidate = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(suncheckid, PersonalRowId);
            //if (ViewBag.GetDocumentListCandidate.Count > 0)
            //{
            //    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            //}
            //else
            //{
            //    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Pending", "Docs");
            //}


            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            CreateMenuitemforDataEntryOperator(PersonalRowId);

            //International client
            DataTable Candidatedocvisibilty = new DataTable();
            ClientCustomize cc = new ClientCustomize();
            Candidatedocvisibilty = cc.getCandidateDocVisibilty(SubCheckId);
            if (Candidatedocvisibilty.Rows[0]["documentDetailsForVerification"].ToString() == "1")
            {
                if (SubCheckId == 213 && clientRowId == 992 || clientRowId == 993 || clientRowId == 994 || clientRowId == 1125 || clientRowId == 1243 || clientRowId == 1450 || SubCheckId == 38 && clientRowId == 1481)
                {
                    ViewBag.Candidatedocvisibilty = "false";
                    bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
                }
                else
                {
                    ViewBag.Candidatedocvisibilty = "true";
                }
            }
            else
            {
                ViewBag.Candidatedocvisibilty = "false";
                bl.updateCheckStatus_Candidate_temp(PersonalRowId, SubCheckId, "Completed", "Docs");
            }



            ClientCustomize blnew = new ClientCustomize();
            DataTable dtpackage = blnew.GetPackageNameByCandidate(PersonalRowId);
            if (dtpackage.Rows.Count > 0)
            {
                ViewBag.packagename = dtpackage.Rows[0][0].ToString();
            }
            return View(expando);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddSpecialCheckInfo(FormCollection formCollection, bool? NotApplicable)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));

            string clientRowid = formCollection["clientRowId"].ToString();
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                ViewBag.ErrorMsg = "";


                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                                Regex regex = new Regex(@"^[a-zA-Z0-9\s\-_(),.'@/\\]*$");
                                if (!regex.IsMatch(cd.DataEntryValue))
                                {
                                    ViewBag.ErrorMsg = "Special characters are not allowed";
                                    TempData["ErrorMessage"] = "Special characters are not allowed";
                                    return RedirectToAction("AddSpecialCheckInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                                }
                                if (key == "550")
                                {
                                    string mobileNo = cd.DataEntryValue;
                                    if (mobileNo.Length > 10)
                                    {
                                        ViewBag.ErrorMsg = "Please enter 10-digit Mobile Number";
                                        TempData["ErrorMessage"] = "Please enter 10-digit Mobile Number";
                                        return RedirectToAction("AddSpecialCheckInfo", "Home", new { subCheckId = SubCheckid, ClientRowId = clientRowid, PersonalRowID = PersonalRowid });
                                    }
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }
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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                                cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                bl.SaveDataEntry_Candidate(cd);
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
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                CreateMenuitemforDataEntryOperator(cd.PersonalRowID);
                expando.document = repoPQVerifiedUploadDoc.GetTempCheckDocumentUploadedListUpdated(Convert.ToInt16(cd.SubCheckRowID), PersonalRowId);
                ///////////////////////////////////////////
                bool isNotApplicable = NotApplicable ?? false;
                ViewBag.NotApplicable = isNotApplicable;
                if (isNotApplicable == true)
                {
                    bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Completed", "Completed");
                    var tblcheck = bl.GetPQCandidateChecksOrderBy(PersonalRowId, SubCheckId, clientRowId);
                    if (tblcheck.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblcheck.Rows.Count; i++)
                        {
                            int SubCheckFamilyRowId = Convert.ToInt32(tblcheck.Rows[i]["SubCheckFamilyID"]);
                            bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckFamilyRowId, "Completed", "Completed");
                        }
                    }
                    ViewBag.Success = "Information Updated Successfully";
                    return RedirectToAction("AddSpecialCheckInfo", new { NotApplicable = isNotApplicable, msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = clsCommonMethods.PEncrypt(SubCheckId.ToString()), ClientRowId = clsCommonMethods.PEncrypt(clientRowId.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(PersonalRowId.ToString()) });
                }
                else
                {
                    bl.UpdateNACheckDocumentStatusNew(PersonalRowId, SubCheckId, "Pending", "Pending");
                    var tblcheck = bl.GetPQCandidateChecksOrderBy(PersonalRowId, SubCheckId, clientRowId);
                    if (tblcheck.Rows.Count > 0)
                    {
                        for (int i = 0; i < tblcheck.Rows.Count; i++)
                        {
                            int SubCheckFamilyRowId = Convert.ToInt32(tblcheck.Rows[i]["SubCheckFamilyID"]);
                            bl.UpdateNACheckDocumentStatusNew(PersonalRowId, SubCheckFamilyRowId, "Pending", "Pending");
                        }
                    }
                }
                ///////////////////////////////////////////
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
            GetDocumentTypeListBySubCheckID(Convert.ToInt16(SubCheckId));
            //ViewBag.Success = "Information Updated Successfully";
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return RedirectToAction("AddSpecialCheckInfo", "Home", new { msg = clsCommonMethods.PEncrypt(ViewBag.Success), subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowId });
            }
            else
                return View(marksModel);
        }
        #endregion

        #region NonAction/Json

        public string PdfImage(string DocFile)
        {
            string ImageName = string.Empty;
            string DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();

            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.Serial = "80027423579"; //License for server 
                                      //f.Serial = "10273316474"; //License for development machine   
            f.OpenPdf(Server.MapPath(DocPath + DocFile));

            if (f.PageCount > 0)
            {
                string jpgFileName = DocFile.Replace(".pdf", "");
               
                for (int i = 1; i <= f.PageCount; i++)
                {
                    ImageName += jpgFileName + i + ".jpg,";
                }
                if (!string.IsNullOrEmpty(ImageName) && ImageName.Length > 0)
                {
                    ImageName = ImageName.Substring(0, ImageName.Length - 1);

                    //int width = 644; // Width in Px
                    //int height = 911; // Height in Px
                    f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    //f.ImageOptions.Resize(new System.Drawing.Size { Width = width, Height = height }, false);
                    f.ImageOptions.Dpi = 400;
                    f.ToImage(Server.MapPath(DocPath), jpgFileName);
                }
            }
            return ImageName;
        }

        [NonAction]
        private void GetFaceMatchAndUploadArtifact(int Pid, int Sid, int Cid)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataBaseConnections Dc = new DataBaseConnections();
                var DocList = repoPQVerifiedUploadDoc.GetTempCheckUploadDocList(Pid, Convert.ToInt16(Sid));
                var TempDt = Dc.GetTempPqPersonalData(Convert.ToInt32(Pid));
                var CaseDocList = repoPQVerifiedUploadDoc.GetTempCheckUploadCaseDocList(Pid);
                string Path = Server.MapPath(ConfigurationManager.AppSettings["provideddocuments"].ToString());             
                string fileName1 = "";
                string fileName2 = "";
                string CandidateIDImage = "";
                foreach (var item in CaseDocList)
                {
                    if (item.DocumentType != "Verification Artifact")
                    {
                        if (item.DocumentType == "Upload Photo")
                        {
                            fileName1 = item.FileName;
                            if (fileName1.Contains(".pdf"))
                            {
                                string file_Name = PdfImage(fileName1);
                                fileName1 = file_Name.Split(',')[0].ToString();
                                CandidateDEModel cd = new CandidateDEModel();
                                cd.AntecedentRowID = 0;
                                cd.PersonalRowID = Pid;
                                cd.DisplayName = "Upload Photo";
                                cd.DataEntryValue = fileName1;
                                bl.proc_InsertUpdateTempPQVerifiedUploadCaseDocs(cd, Path);
                            }

                        }
                        else if (item.DocumentType == "Upload Interview Screenshot")
                        {
                            fileName2 = item.FileName;
                            if (fileName2.Contains(".pdf"))
                            {
                                string file_Name = PdfImage(fileName2);
                                fileName2 = file_Name.Split(',')[0].ToString();
                                CandidateDEModel cd = new CandidateDEModel();
                                cd.AntecedentRowID = 0;
                                cd.PersonalRowID = Pid;
                                cd.DisplayName = "Upload Interview Screenshot";
                                cd.DataEntryValue = fileName2;
                                bl.proc_InsertUpdateTempPQVerifiedUploadCaseDocs(cd, Path);
                            }
                        }
                    }
                                                         
                }
                foreach (var item in DocList)
                {
                    CandidateIDImage = item.FileName;
                    if (CandidateIDImage != "")
                    {
                        if(CandidateIDImage.Contains(".pdf"))
                        {
                            string file_Name = PdfImage(CandidateIDImage);
                            CandidateIDImage = file_Name.Split(',')[0].ToString();
                            SavePQVerifiedUploadDocFileName(Pid, 799, "Provided", item.DocumentType, "provideddocumentsAPI", CandidateIDImage, "");
                        }

                        break;
                    }
                }
                if (fileName2 != "" && fileName1 != "")
                {
                    InstantCheckRepository InstantcheckObj = new InstantCheckRepository();

                    var Result1 = InstantcheckObj.GetFatchMatchNew(Path + fileName1, Path + CandidateIDImage, "60", "", Pid.ToString());
                    FaceMatchRoot FatchMatchModel = JsonConvert.DeserializeObject<FaceMatchRoot>(Result1);
                    if (FatchMatchModel !=null)
                    {
                        if (FatchMatchModel.statuscode == 200)
                        {                             
                            var FaceMatchId = cc.InsertIntoFaceMatchForTemp(FatchMatchModel, Pid, fileName1, CandidateIDImage, "Image Provided By Client", "ID Provided By Candidate", Convert.ToInt32(Pid), Result1,"");
                                                       
                            var FaceMatchDt = cc.GetFaceMatch(Convert.ToInt32(FaceMatchId));
                            string companyRefNo = FaceMatchDt.Rows[0]["companyRefNo"].ToString();
                            string body = string.Empty;
                            using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/FaceMatchForTemp.html")))
                            {
                                body = reader.ReadToEnd();
                            }
                            body = body.Replace("{CandidateName}", companyRefNo);
                            body = body.Replace("{Match}", FatchMatchModel.data.match);
                            body = body.Replace("{message}", FatchMatchModel.data.status);
                            body = body.Replace("{Image1Name}", fileName1);
                            body = body.Replace("{Image2Name}", CandidateIDImage);
                            body = body.Replace("{Image1Remark}", "Image Provided By Client");
                            body = body.Replace("{Image2Remark}", "ID Provided By Candidate");
                            body = body.Replace("{verifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));

                            SaveTempPQVerifiedUploadDoc(Pid, Convert.ToInt16(Sid), "Verified", "Verification Artifact", "provideddocuments", body, "Face Match Verified");

                        }
                    }
                    var Result2 = InstantcheckObj.GetFatchMatchNew(Path + fileName2, Path + CandidateIDImage, "60", "", Pid.ToString());
                    FaceMatchRoot FatchMatchModel2 = JsonConvert.DeserializeObject<FaceMatchRoot>(Result2);
                    if (FatchMatchModel2 != null)
                    {
                        if (FatchMatchModel2.statuscode == 200)
                        {
                            var FaceMatchId = cc.InsertIntoFaceMatchForTemp(FatchMatchModel2, Pid, fileName2, CandidateIDImage, "Image Provided By Client", "ID Provided By Candidate", Pid, "", Result2);

                            var FaceMatchDt = cc.GetFaceMatch(Convert.ToInt32(FaceMatchId));
                            string companyRefNo = FaceMatchDt.Rows[0]["companyRefNo"].ToString();
                            string body = string.Empty;
                            using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/FaceMatchForTemp.html")))
                            {
                                body = reader.ReadToEnd();
                            }
                            body = body.Replace("{CandidateName}", companyRefNo);
                            body = body.Replace("{Match}", FatchMatchModel2.data.match);
                            body = body.Replace("{message}", FatchMatchModel2.data.status);
                            body = body.Replace("{Image1Name}", fileName2);
                            body = body.Replace("{Image2Name}", CandidateIDImage);
                            body = body.Replace("{Image1Remark}", "Screenshot Provided By Client");
                            body = body.Replace("{Image2Remark}", "ID Provided By Candidate");
                            body = body.Replace("{verifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));

                            SaveTempPQVerifiedUploadDoc(Pid, Convert.ToInt16(Sid), "Verified", "Verification Artifact", "provideddocuments", body, "Face Match Verified");

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), "FaceMatch " + Ex.Message, Ex.StackTrace);
            }
        }
     
        [NonAction]
        private void saveEmpidInTempCandidateTable(int pid, int sid, int researchid)
        {
            tpc.SaveResearchData(pid, sid, researchid);
        }


        [NonAction]
        private void MoveLOADocUploadedByCandidate(long PersonalRowId)
        {
            try
            {
                string existFile = repoPQPersonal.GetPQPersonalCaseDocUploadedByCandidate(PersonalRowId);
                if (!string.IsNullOrEmpty(existFile))
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["CandidateSignAgreeDoc"].ToString();
                    string FldMovePathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string path = Server.MapPath(FldPathDoc + existFile);
                    FileInfo file = new FileInfo(path);

                    if (file.Exists)//check file exsit or not
                    {
                        file.MoveTo(Server.MapPath(FldMovePathDoc + existFile));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        private void CreateMenuitemforDataEntryOperator_old(int id = 0)
        {
            string inEncript = clsCommonMethods.PEncrypt(id.ToString());
            if (id > 0)
            {

                var CandidateChecks = tpc.GetTempCandidateChecks(id);
                string NavInputDataMenuList;
                NavInputDataMenuList = "<li><a href='Documents\\CandidateDocuments\\CandidateManual\\CandidateAgreement.pdf' target='_blank' style='background-color:#3175AF;padding:5px;'><span class='glyphicon glyphicon-download-alt'></span><b style='color:#ffffff'> Download LOA </b></a></li>";
                NavInputDataMenuList = "<li class='bold' style='background-color:#3175AF;padding:5px;'>Guidelines for uploading the documents are below </li>";
                NavInputDataMenuList += "<li style='background-color:#dedddd;padding:1.5%;font-weight: bold' class='list-group-item'><a href='" + Url.Action("Index", "Home") + "'><span class='glyphicon glyphicon-user'></span> Instructions </a></li>";

                NavInputDataMenuList += "<li style='background-color:#dedddd;padding:1.5%;font-weight: bold' class='list-group-item'><a href='" + Url.Action("FAQ", "Home") + "'><span class='glyphicon glyphicon-user'></span> FAQ </a></li>";

                NavInputDataMenuList += "<li ><a href='" + Url.Action("UserManual", "Home") + "' target='_blank' style='padding:5px;background-color:#dedddd;'><span class='glyphicon glyphicon-download-alt '></span> User Manual </a></li>";

                NavInputDataMenuList += "<li><a href='" + Url.Action("DownloadLOA", "Home") + "' target='_blank' style='padding:5px;background-color:#dedddd;'><span class='glyphicon glyphicon-download-alt'></span> Letter of Authorization (LOA) </a></li>";

                NavInputDataMenuList += "<li><a href='" + Url.Action("UpdateCandidateDetails", "Home", new { id = inEncript }) + "'><span class='glyphicon glyphicon-user'></span>  Personal Info </a></li>";

                if (CandidateChecks != null && CandidateChecks.Count() > 0)
                {
                    foreach (var item in CandidateChecks)
                    {
                        string enPersonalRowID = clsCommonMethods.PEncrypt(item.PersonalRowID.ToString());
                        string enClientRowID = clsCommonMethods.PEncrypt(item.ClientRowID.ToString());
                        string enCheckFamilyRowID = clsCommonMethods.PEncrypt(item.CheckFamilyRowID.ToString());
                        string enSubCheckRowID = clsCommonMethods.PEncrypt(item.SubCheckRowID.ToString());

                        string urlValue = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(item.CheckFamilyRowID.ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, enCheckFamilyRowID, enSubCheckRowID));
                        if (item.CheckInfoStatus.ToString() == "Completed" && item.CheckDocStatus.ToString() == "Completed")
                            NavInputDataMenuList += "<li style='background-color:#36d6d4;padding:1.5%' class='list-group-item'><a href='" + urlValue + "'><span class='glyphicon glyphicon-unchecked'></span> " + item.SubCheckName + "</a></li>";
                        else
                            NavInputDataMenuList += "<li><a href='" + urlValue + "'><span class='glyphicon glyphicon-unchecked'></span> " + item.SubCheckName.ToString() + "</a></li>";

                    }
                }

                NavInputDataMenuList += "<li class='slide'><a class='side-menu__item final_submit' href='" + Url.Action("FinalSubmit", "Home", new { id = inEncript }) + "'><b id='btnFinalSub'; class='submit-btn'>Final Submission</b></a></li>";

                Session["NavCandidateInputDataMenuList"] = NavInputDataMenuList;
            }
        }

        private void CreateMenuitemforDataEntryOperator(Int64 id = 0)
        {
            try
            {
                string inEncript = clsCommonMethods.PEncrypt(id.ToString());
                if (id > 0)
                {
                    string refno = "";
                    DataSet d = new DataSet();
                    if (Session["ClientRowID"].ToString() == "679")
                    {
                        DataTable dt = tpc.GetNewCandidateDetails(id);
                        if (dt.Rows.Count > 0)
                        {
                            refno = dt.Rows[0]["CompanyRefNo"].ToString();
                        }
                        if (!string.IsNullOrEmpty(refno))
                        {
                            d = tpc.getTempPQCandidateChecksDT_Fractal(id);
                        }
                        else
                        {
                            d = tpc.GetTempCandidateChecksDT(id);
                        }

                    }
                    else if (Session["ClientRowID"].ToString() == "559")
                    {
                        d = tpc.AutoCheckInitiation_proc(id);
                    }
                    else
                    {
                        d = tpc.GetTempCandidateChecksDT(id);
                    }



                    string NavInputDataMenuList = string.Empty;

                    if (Convert.ToString(Session["ClientRowID"]) != "1606")
                    {
                        if (Convert.ToString(d.Tables[0].Rows[0]["CaseinfoStatus"]) == "Completed")
                            NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + Url.Action("UpdateCandidateDetails", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>Personal Information</span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></a></li>";
                        else
                            NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("UpdateCandidateDetails", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>Personal Information</span></a></li>";
                    }

                    int AarryCount = d.Tables[1].Rows.Count;
                    string[] nxtprv = new string[AarryCount];
                    if (d.Tables[1].Rows.Count > 0)
                    {
                        for (int J = 0; J < d.Tables[1].Rows.Count; J++)
                        {
                            string NextSubCheckRowID = "End";
                            string PreviousSubCheckRowID = "Start";
                            string NextCheckFamilyRowID = "";
                            string PreviousFamilyRowID = "";
                            string enPersonalRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["PersonalRowID"].ToString());
                            string enClientRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["ClientRowID"].ToString());
                            string enCheckFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString());
                            string enSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString());
                            string urlValuePrevious = "Start";
                            string urlValueNext = "End";

                            if (J < d.Tables[1].Rows.Count - 1)
                            {
                                NextSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J + 1]["SubCheckFamilyID"].ToString());
                                NextCheckFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J + 1]["CheckFamilyRowID"].ToString());
                                if (Convert.ToInt16(d.Tables[1].Rows[J + 1]["SubCheckFamilyID"].ToString()) == 890)
                                {
                                    urlValuePrevious = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J + 1]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J + 1]["SubCheckFamilyID"].ToString())) + "?Pid={0}&Sid={1}", enPersonalRowID, NextSubCheckRowID));

                                }
                                else
                                {
                                    urlValueNext = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J + 1]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J + 1]["SubCheckFamilyID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, NextCheckFamilyRowID, NextSubCheckRowID));
                                }
                            }
                            if (J > 0)
                            {
                                PreviousSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J - 1]["SubCheckFamilyID"].ToString());
                                PreviousFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J - 1]["CheckFamilyRowID"].ToString());
                                if (Convert.ToInt16(d.Tables[1].Rows[J - 1]["SubCheckFamilyID"].ToString()) == 890)
                                {
                                    urlValuePrevious = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J - 1]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J - 1]["SubCheckFamilyID"].ToString())) + "?Pid={0}&Sid={1}", enPersonalRowID, PreviousSubCheckRowID));

                                }
                                else
                                {
                                    urlValuePrevious = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J - 1]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J - 1]["SubCheckFamilyID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, PreviousFamilyRowID, PreviousSubCheckRowID));
                                }
                            }

                            if (d.Tables[1].Rows.Count == 1)
                            {
                                NextSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString());
                                NextCheckFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString());
                                if (Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString()) == 890)
                                {
                                    urlValueNext = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString())) + "?Pid={0}&Sid={1}", enPersonalRowID, NextSubCheckRowID));

                                }
                                else
                                {
                                    urlValueNext = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, NextCheckFamilyRowID, NextSubCheckRowID));

                                }

                            }
                            string urlValue = "";
                            if (Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString()) == 890)
                            {
                                urlValue = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString())) + "?Pid={0}&Sid={1}", enPersonalRowID, enSubCheckRowID));

                            }
                            else
                            {
                                urlValue = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, enCheckFamilyRowID, enSubCheckRowID));

                            }
                            nxtprv[J] = urlValueNext + "|" + urlValuePrevious + "|" + PreviousSubCheckRowID + "|" + NextSubCheckRowID + "|" + enSubCheckRowID;

                            if (Session["ClientRowID"].ToString() == "559")
                            {
                                if (d.Tables[1].Rows[J]["CheckInfoStatus"].ToString() == "Completed" && d.Tables[1].Rows[J]["CheckDocStatus"].ToString() == "Completed")
                                    NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + urlValue + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>" + d.Tables[1].Rows[J]["CheckName"] + " </span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></a></li>";
                                else
                                    NavInputDataMenuList += "<li class='slide'><p class='side-menu__item' href=''><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>" + d.Tables[1].Rows[J]["CheckName"].ToString() + " </span></p></li>";
                            }
                            else
                            {
                                if (d.Tables[1].Rows[J]["CheckInfoStatus"].ToString() == "Completed" && d.Tables[1].Rows[J]["CheckDocStatus"].ToString() == "Completed")
                                    NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + urlValue + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>" + d.Tables[1].Rows[J]["CheckNameInCandidatePortal"] + " </span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></a></li>";
                                else
                                    NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + urlValue + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>" + d.Tables[1].Rows[J]["CheckNameInCandidatePortal"].ToString() + " </span></a></li>";
                            }
                        }
                    }

                    if (Session["ClientRowID"].ToString() == "559")
                    {
                        if (Convert.ToString(d.Tables[0].Rows[0]["LOA_Name"]) != "")
                            NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + Url.Action("Digital_LOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>Digital (LOA)</span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></a></li>";
                        else
                            NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("Digital_LOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>Digital (LOA)</span></a></li>";
                    }
                    else
                    {
                        if (refno == "")
                        {
                            if (Session["ClientRowID"].ToString() != "267")
                            {
                                if (Convert.ToString(d.Tables[0].Rows[0]["LOA_Name"]) != "")
                                    NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + Url.Action("Digital_LOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>Digital (LOA)</span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></p></li>";
                                else
                                    NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("Digital_LOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>Digital (LOA)</span></p></li>";
                            }
                        }
                    }
                    //NavInputDataMenuList += "<li class='slide'><a class='side-menu__item final_submit' href='" + Url.Action("FinalSubmit", "Home", new { id = inEncript }) + "'><b id='btnFinalSub'; class='submit-btn'>Final Submission</b></a></li>";

                    if (Session["ClientRowID"].ToString() == "267")
                    {

                        //NavInputDataMenuList += "<li><a href='" + Url.Action("eSigns", "Home", new { AuthType = "1" }) + "'><span class='glyphicon glyphicon-user'></span>  eSign By OTP </a></li>";
                        NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("eSigns", "Home", new { AuthType = "1" }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>eSign via AAdhar</span></p></li>";
                        NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("eSigns", "Home", new { AuthType = "2" }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>eSign By Bio Matric</span></p></li>";
                        if (refno == "")
                        {
                            if (Convert.ToString(d.Tables[0].Rows[0]["LOA_Name"]) != "")
                                NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + Url.Action("Digital_ESignLOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>Digital eSign (LOA)</span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></p></li>";
                            else
                                NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("Digital_ESignLOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>Digital eSign (LOA)</span></p></li>";
                        }
                    }

                    Session["NavCandidateInputDataMenuList"] = NavInputDataMenuList;
                    Session["NextPreviousButtonsURL"] = nxtprv;
                }
            }
            catch (Exception ee)
            {

            }
        }

        private void CreateMenuitemforDataEntrySubmit(Int64 id = 0)
        {
            try
            {
                string inEncript = clsCommonMethods.PEncrypt(id.ToString());
                if (id > 0)
                {

                    DataSet d = new DataSet();
                    if (Session["ClientRowID"].ToString() == "679")
                    {
                        string refno = "";
                        DataTable dt = tpc.GetNewCandidateDetails(id);
                        if (dt.Rows.Count > 0)
                        {
                            refno = dt.Rows[0]["CompanyRefNo"].ToString();
                        }
                        if (!string.IsNullOrEmpty(refno))
                        {
                            d = tpc.getTempPQCandidateChecksDT_Fractal(id);
                        }
                        else
                        {
                            d = tpc.GetTempCandidateChecksDT(id);
                        }

                    }
                    else
                    {
                        d = tpc.GetTempCandidateChecksDT(id);
                    }



                    string NavInputDataMenuListsubmit = string.Empty;

                    NavInputDataMenuListsubmit += "<li><a class='final_submit' id='btnFinalSub' href='" + Url.Action("FinalSubmit", "Home", new { id = inEncript }) + "'><span>Final Submission</span><i class='fa fa-check-circle tx-16' aria-hidden='true'></i></a></li>";

                    Session["NavCandidateInputDataMenuListsubmit"] = NavInputDataMenuListsubmit;
                }
            }
            catch (Exception ee)
            {

            }
        }

        [NonAction]
        private string GetPageNameByCheckId(short Id = 0, int Sid = 0)
        {
            string pageName = string.Empty;
            if (Id == 2)
            {
                pageName = "AddAddressInfo";
            }
            if (Id == 5)
            {
                pageName = "AddCriminalInfo";
            }
            if (Id == 11)
            {
                pageName = "AddEducationInfo";
            }
            if (Id == 12)
            {
                pageName = "AddEmploymentInfo";
            }
            if (Id == 26)
            {
                pageName = "AddNationalIdentityInfo";
            }
            if (Id == 30)
            {
                pageName = "AddReferenceInfo";
            }
            if (Id == 40)
            {
                if (Sid == 890)
                {
                    pageName = "AddlivenessInfo";
                }
                else
                {
                    pageName = "AddSpecialCheckInfo";
                }
            }
            return pageName;
        }

        [NonAction]
        private string GetPageNameByCheckId_AddChack(short Id = 0)
        {
            string pageName = string.Empty;
            if (Id == 2)
            {
                pageName = "AddAddress_Info";
            }
            if (Id == 5)
            {
                pageName = "AddCriminal_Info";
            }
            if (Id == 11)
            {
                pageName = "AddEducation_Info";
            }
            if (Id == 12)
            {
                pageName = "AddEmployment_Info";
            }
            if (Id == 26)
            {
                pageName = "AddNationalIdentity_Info";
            }
            if (Id == 30)
            {
                pageName = "AddReference_Info";
            }
            if (Id == 40)
            {
                pageName = "AddSpecialCheck_Info";
            }
            return pageName;
        }

        [NonAction]
        private DateTime GetDeliveryDate(DateTime oDate, byte TAT, PQClientCodeHolidaysViewModel client)
        {
            DateTime ddate = oDate;
            DateTime fdlDate = oDate;
            ddate = ddate.AddDays(TAT);
            int intDate = 0;
            do
            {
                intDate = getWeekendAndHoliDays(fdlDate, ddate, client);
                fdlDate = ddate;
                ddate = ddate.AddDays(intDate);
            }
            while (intDate != 0);
            return ddate;
        }

        [NonAction]
        private int getWeekendAndHoliDays(DateTime oDate, DateTime dDate, PQClientCodeHolidaysViewModel clientModel)
        {
            int intDate = 0;
            //1-Client or 0-Default
            if (clientModel.MapHolidays == 1)
            {
                var clientHoli = repoPQClientHoliday.GetClientHolidays(clientModel.ClientRowID).Where(ch => ch.HolidayDate.Value.Date >= oDate && ch.HolidayDate.Value <= dDate);
                intDate = clientHoli.Count();
                dDate = dDate.AddDays(intDate);

            }
            //DaysCalculation ==1-Working Days or 0-Calendar Days
            if (clientModel.DaysCalculation == 1)
            {
                // Weekend Days count
                int weekEndCount = 0;
                TimeSpan diff = dDate - oDate;
                int days = diff.Days;
                for (var i = 1; i <= days; i++)
                {
                    var testDate = oDate.AddDays(i);
                    if (testDate.DayOfWeek == DayOfWeek.Saturday || testDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        weekEndCount += 1;
                    }
                }
                dDate = dDate.AddDays(weekEndCount);
                intDate += weekEndCount;
            }
            return intDate;
        }


        void BindCountry()
        {
            try
            {
                ViewBag.Countries = repoDDL.GetCountriesForDDL();
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public void BindCityByStateId(short StateRowId = 0, short teamMemerRowId = 0)
        {
            try
            {
                if (StateRowId != 0)
                {
                    ViewBag.CityList = repoDDL.GetCityForDDL(StateRowId, teamMemerRowId);
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public void GetAddressProofDocs()
        {
            try
            {
                List<SelectListItem> objData = new List<SelectListItem>
                { new SelectListItem{Text="Ration Card", Value="Ration Card"},
                    new SelectListItem{Text="Telephone Bill", Value="Telephone Bill"},
                    new SelectListItem{Text="Rent Agreement", Value="Rent Agreement"},
                    new SelectListItem{Text="Passport", Value="Passport"},
                    new SelectListItem{Text="Electricity Bill", Value="Electricity Bill"},
                    new SelectListItem{Text="Gas Supply Bill", Value="Gas Supply Bill"},
                    new SelectListItem{Text="Water Bill", Value="Water Bill"},
                    new SelectListItem{Text="Others", Value="Others"}};
                ViewBag.AddressProofDocs = objData;
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        void GetCollegeList()
        {
            try
            {
                //List<CollegeDDLListViewModel> obj = new List<CollegeDDLListViewModel>();
                var obj = repoDDL.GetCollegeDDL().ToList();
                //obj.Add(new CollegeDDLListViewModel { CollegeRowID = 0, CollegeName = "Others" });
                ViewBag.CollegeList = obj;// repoDDL.GetCollegeDDL();
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        void GetDegreeList()
        {
            try
            {
                ViewBag.DegreeList = repoDDL.GetDegreeDDL();
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        void GetEmploymentList()
        {
            try
            {
                ViewBag.EmploymentList = repoDDL.GetEmploymentDDL();
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindClientList()
        {
            try
            {
                ViewBag.ClientList = repoDDL.GetClientListWithAbbAndLocationForDDL(); ;
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindCaseTypeList()
        {
            try
            {
                IEnumerable<SelectListItem> CaseTypeList = new List<SelectListItem> {
                new SelectListItem { Text = "Normal", Value = "Normal" },
                new SelectListItem { Text = "Priority", Value = "Priority" },
            };
                ViewBag.CaseTypeList = CaseTypeList;
            }
            catch (Exception Ex)
            {
                // ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindTitleList()
        {
            try
            {
                IEnumerable<SelectListItem> TitleList = new List<SelectListItem>
            {
            new SelectListItem { Text = "Mr.", Value = "Mr.", Selected=false },
            new SelectListItem { Text = "Ms.", Value = "Ms.", Selected=false },
            new SelectListItem { Text = "Mrs.", Value = "Mrs.", Selected=false },
            new SelectListItem { Text = "Mohd.", Value = "Mohd.", Selected=false },
            //new SelectListItem { Text = "Kahtun.", Value = "Kahtun.", Selected=false },
            };
                ViewBag.TitleList = TitleList;
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindMaritalStatusList()
        {
            try
            {
                IEnumerable<SelectListItem> MaritalStatusList = new List<SelectListItem>
            {
            new SelectListItem { Text = "N/A", Value = "N/A", Selected=false },
            new SelectListItem { Text = "Married", Value = "Married", Selected=false },
            new SelectListItem { Text = "Single", Value = "Single", Selected=false },
            //new SelectListItem { Text = "Unmarried", Value = "Unmarried", Selected=false },
            };
                ViewBag.MaritalStatusList = MaritalStatusList;
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindCandidateTypeList()
        {
            try
            {
                IEnumerable<SelectListItem> CandidateTypeList = new List<SelectListItem>
            {
            new SelectListItem { Text = "Fresher", Value = "Fresher", Selected=false },
            new SelectListItem { Text = "Experienced", Value = "Experienced", Selected=false },
            };
                ViewBag.CandidateTypeList = CandidateTypeList;
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindCatagoryList()
        {
            try
            {
                IEnumerable<SelectListItem> CatagoryList = new List<SelectListItem>
            {
            new SelectListItem { Text = "Pre Joining", Value = "Pre Joining", Selected=false },
            new SelectListItem { Text = "Post Joining", Value = "Post Joining", Selected=false },
            };
                ViewBag.CatagoryList = CatagoryList;
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        private void GetAllDocUploadedCheckList(int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    var CandidateChecks = repoPQCandidateCheck.GetTempCandidateChecks(id);
                    List<SubCheckFamilyForDDLViewModel> objData = new List<SubCheckFamilyForDDLViewModel>();
                    if (CandidateChecks != null && CandidateChecks.Count() > 0)
                    {
                        foreach (var item in CandidateChecks)
                        {
                            SubCheckFamilyForDDLViewModel data = new SubCheckFamilyForDDLViewModel();
                            if (item.ClientPackageRowID > 0)
                            {
                                var chkData = repoClientPackage.GetPackageDetailsById(item.ClientPackageRowID);
                                if (repoPQVerifiedUploadDoc.IsExistsTempChecksInUploadDocList(item.PersonalRowID, chkData.SubCheckRowID))
                                {
                                    data.SubCheckName = chkData.SubCheckFamillyName;
                                    data.SubCheckRowID = chkData.SubCheckRowID;
                                    objData.Add(data);
                                }
                            }
                            else if (item.ClientCheckRowID > 0)
                            {
                                var chkData = repoClientCheck.GetChecksDetailsById(item.ClientCheckRowID);
                                if (repoPQVerifiedUploadDoc.IsExistsTempChecksInUploadDocList(item.PersonalRowID, chkData.SubCheckRowID))
                                {
                                    data.SubCheckName = chkData.SubCheckFamillyName;
                                    data.SubCheckRowID = chkData.SubCheckRowID;
                                    objData.Add(data);
                                }
                            }
                        }
                    }
                    ViewBag.CheckList = objData.ToList();
                }
            }
            catch (Exception Ex)
            {
                // ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        private void GetAllCheckListforDocUpload(int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    var CandidateChecks = repoPQCandidateCheck.GetTempCandidateChecks(id);
                    List<SubCheckFamilyForDDLViewModel> objData = new List<SubCheckFamilyForDDLViewModel>();
                    if (CandidateChecks != null && CandidateChecks.Count() > 0)
                    {
                        foreach (var item in CandidateChecks)
                        {
                            SubCheckFamilyForDDLViewModel data = new SubCheckFamilyForDDLViewModel();
                            if (item.ClientPackageRowID > 0)
                            {
                                var chkData = repoClientPackage.GetPackageDetailsById(item.ClientPackageRowID);
                                if (!repoPQVerifiedUploadDoc.IsExistsTempChecksInUploadDocList(item.PersonalRowID, chkData.SubCheckRowID))
                                {
                                    data.SubCheckName = chkData.SubCheckFamillyName;
                                    data.SubCheckRowID = chkData.SubCheckRowID;
                                    objData.Add(data);
                                }
                            }
                            else if (item.ClientCheckRowID > 0)
                            {
                                var chkData = repoClientCheck.GetChecksDetailsById(item.ClientCheckRowID);
                                if (!repoPQVerifiedUploadDoc.IsExistsTempChecksInUploadDocList(item.PersonalRowID, chkData.SubCheckRowID))
                                {
                                    data.SubCheckName = chkData.SubCheckFamillyName;
                                    data.SubCheckRowID = chkData.SubCheckRowID;
                                    objData.Add(data);
                                }
                            }
                        }
                    }
                    ViewBag.CheckList = objData.ToList();
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public void SavePQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", HttpPostedFileBase UploadDoc = null, string remarks = "")
        {
            try
            {

                AddTempPQVerifiedUploadDocViewModel DocModel = new AddTempPQVerifiedUploadDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.SubCheckRowID = SubCheckRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //  Provided / Verified
                DocModel.DocumentType = DocumentType; //Name of the selected docs
                string FldPathDoc = string.Empty;
                string FldPathDoc2 = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();
                string proImg = string.Empty;
                string ext = Path.GetExtension(UploadDoc.FileName).ToLower();
                proImg = (SubCheckRowID.ToString() + DocumentType + Session["CadidateUserName"].ToString()).Replace(" ", "_").Replace("-", "_") + DateTime.Now.ToString("ssfff") + ext;
                if (UploadDoc != null && UploadDoc.ContentLength > 0)
                {
                    using (var br = new BinaryReader(UploadDoc.InputStream))
                    {
                        // Ensure the stream position is at the beginning in case someone touched it earlier
                        if (UploadDoc.InputStream.CanSeek)
                            UploadDoc.InputStream.Position = 0;
                        var bytes = br.ReadBytes(UploadDoc.ContentLength); // read exact lengt                    
                        string base64String = Convert.ToBase64String(bytes);
                        string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(proImg, FldPathDoc, base64String);
                        if (res2.Contains("Success"))
                        {

                            //UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                            DocModel.FileName = Path.GetFileName(proImg);
                            DocModel.FilePath = FldPathDoc2;
                            DocModel.UploadDate = DateTime.Now;
                            DocModel.StatusDocxConvert = 0;
                            DocModel.StatusPDF2DOCXConvert = 0;
                            DocModel.Remarks = remarks;
                            DocModel.PDF_FileName = "";
                            DocModel.Status = 1;
                            repoPQVerifiedUploadDoc.AddTempPQVerifiedUploadDoc(DocModel);
                            repoPQVerifiedUploadDoc.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public void SavePQVerifiedUploadDocFileName(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", string UploadDoc = "", string remarks = "")
        {
            try
            {

                AddTempPQVerifiedUploadDocViewModel DocModel = new AddTempPQVerifiedUploadDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.SubCheckRowID = SubCheckRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //  Provided / Verified
                DocModel.DocumentType = DocumentType; //Name of the selected docs
                string FldPathDoc = string.Empty;
                string FldPathDoc2 = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();
                if (UploadDoc != "" )
                {
                    DocModel.FileName = UploadDoc;
                    DocModel.FilePath = FldPathDoc2;
                    DocModel.UploadDate = DateTime.Now;
                    DocModel.StatusDocxConvert = 0;
                    DocModel.StatusPDF2DOCXConvert = 0;
                    DocModel.Remarks = remarks;
                    DocModel.PDF_FileName = "";
                    DocModel.Status = 1;
                    repoPQVerifiedUploadDoc.AddTempPQVerifiedUploadDoc(DocModel);
                    repoPQVerifiedUploadDoc.SaveChanges();


                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public void SavePQVerifiedUploadDocwithoutEntry(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", byte[] fileBytes = null, string remarks = "", string filenames = "")
        {
            try
            {
                AddTempPQVerifiedUploadDocViewModel DocModel = new AddTempPQVerifiedUploadDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.SubCheckRowID = SubCheckRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; // Provided / Verified
                DocModel.DocumentType = DocumentType; // Name of the selected docs

                string FldPathDoc = string.Empty;
                string FldPathDoc2 = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();

                string proImg = string.Empty;

                string ext = Path.GetExtension(filenames).ToLower();
                proImg = filenames;

                if (fileBytes != null && fileBytes.Length > 0)
                {

                    string base64String = Convert.ToBase64String(fileBytes);

                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(proImg, FldPathDoc, base64String);
                    if (res2.Contains("Success"))
                    {

                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public void GetDocumentTypeListBySubCheckID(short id = 0)
        {
            try
            {
                short CapgeminiID = 0;
                if (Session["CapgeminiClientId"] != null)
                {
                    CapgeminiID = (short)Session["CapgeminiClientId"];
                }

                string clientId = "";
                if (id > 0)
                {

                    if (Session["ClientRowId"] != null)
                    {
                        clientId = Session["ClientRowId"].ToString();
                    }

                    short chkId = repoSubCheckFamily.GetSubCheckFamilyForUpdateById(id).CheckFamilyRowID;
                    string pageName = string.Empty;
                    if (chkId == 2)
                    {
                        if (clientId == "1425" || clientId == "1514")
                        {
                            //pageName = "AddAddressInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Driver's License", Value="Driver's_License"},
                            new SelectListItem{Text="Utility Bill", Value="Utility_Bill"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1429")
                        {
                            //pageName = "AddAddressInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Address proof", Value="Address_proof"},
                            new SelectListItem{Text="Utility Bill", Value="Utility_Bill"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "559")
                        {

                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="Rent Agreement", Value="rent_Agreement"},
                            new SelectListItem{Text="Electricity Bill", Value="electricity_Bill"},
                            new SelectListItem{Text="Telephone Bill", Value="telephone_Bill"},
                            new SelectListItem{Text="Aadhar Card", Value="aadhar_Card"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if(clientId == "576")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Voter ID", Value="voter_ID"},
                            new SelectListItem{Text="DL", Value="D_L"},
                            new SelectListItem{Text="Ration Card", Value="Ration_Card"},
                            new SelectListItem{Text="Telephone Bill", Value="Telephone_Bill"},
                            new SelectListItem{Text="Rent Agreement", Value="Rent_Agreement"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="Electricity Bill", Value="Electricity_Bill"},
                            new SelectListItem{Text="Gas Supply Bill", Value="Gas_Supply_Bill"},
                            new SelectListItem{Text="Water Bill", Value="Water_Bill"},
                            new SelectListItem{Text="AAdhar Card", Value="AAdhar Card"},
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else
                        {
                            //pageName = "AddAddressInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Voter ID", Value="voter_ID"},
                            new SelectListItem{Text="DL", Value="D_L"},
                            new SelectListItem{Text="Ration Card", Value="Ration_Card"},
                            new SelectListItem{Text="Telephone Bill", Value="Telephone_Bill"},
                            new SelectListItem{Text="Rent Agreement", Value="Rent_Agreement"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="Electricity Bill", Value="Electricity_Bill"},
                            new SelectListItem{Text="Gas Supply Bill", Value="Gas_Supply_Bill"},
                            new SelectListItem{Text="Water Bill", Value="Water_Bill"},
                            new SelectListItem{Text="AAdhar Card", Value="AAdhar Card"},
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                            ViewBag.DocumentList = objData;
                        }

                    }
                    if (chkId == 5)
                    {
                        if (clientId == "1331" && id == 514)
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Primary ID 1", Value="Primary_ID_1"},
                            new SelectListItem{Text="Primary ID 2", Value="Primary_ID_2"},
                            new SelectListItem{Text="Secondary ID 1", Value="Secondary_ID_1"},
                            new SelectListItem{Text="Secondary ID 2", Value="Secondary_ID_2"}

                        };
                            ViewBag.DocumentList = objData;
                        }
                        //Burgeon IT Services Pvt Ltd.
                        else if ((clientId == "1331" || clientId == "1514") && id == 716)
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="AFP Form", Value="AFP_Form"},
                            new SelectListItem{Text="Concerned Form", Value="Concerned_Form"},
                            new SelectListItem{Text="100 CheckPointList Form", Value="100_CheckPointList_Form"},
                            new SelectListItem{Text="Primary ID 1", Value="Primary_ID_1"},
                            new SelectListItem{Text="Primary ID 2", Value="Primary_ID_2"},
                            new SelectListItem{Text="Secondary ID 1", Value="Secondary_ID_1"},
                            new SelectListItem{Text="Secondary ID 2", Value="Secondary_ID_2"}
                        };
                            ViewBag.DocumentList = objData;
                        }

                        //Genpact - Australia-Eup
                        else if (clientId == "1428" && (id == 733 || id == 734 || id == 773))
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="National ID Proof", Value="National_ID _Proof"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1429" || clientId == "1395" || clientId == "1414" || clientId == "1415" || clientId == "1416")
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="National ID Proof", Value="National_ID _Proof"}
                        };
                            ViewBag.DocumentList = objData;
                        }

                        //Genpact - Australia-Eup
                        else if (clientId == "1425" && id == 764)
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="AFP Form", Value="AFP_Form"},
                            new SelectListItem{Text="Consent Form", Value="Consent_Form"},
                            new SelectListItem{Text="100 Point Document 1", Value="100_Point_Document_1"},
                            new SelectListItem{Text="100 Point Document 2", Value="100_Point_Document_2"},
                            new SelectListItem{Text="100 Point Document 3", Value="100_Point_Document_3"},
                            new SelectListItem{Text="100 Point Document 4", Value="100_Point_Document_4"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1514")
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="AFP Form", Value="AFP_Form"},
                            new SelectListItem{Text="Consent Form", Value="Consent_Form"},
                            new SelectListItem{Text="100 Point Document 1", Value="100_Point_Document_1"},
                            new SelectListItem{Text="100 Point Document 2", Value="100_Point_Document_2"},
                            new SelectListItem{Text="100 Point Document 3", Value="100_Point_Document_3"},
                            new SelectListItem{Text="100 Point Document 4", Value="100_Point_Document_4"}
                        };
                            ViewBag.DocumentList = objData;
                        }

                        else if (clientId == "1425" && (id == 23 || id == 24))
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="National ID Proof", Value="National_ID_Proof"}
                        };
                            ViewBag.DocumentList = objData;
                        }

                        else if (clientId == "1449")
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="National ID Proof", Value="National_ID_Proof"}
                        };
                            ViewBag.DocumentList = objData;
                        }

                        else if (clientId == "1071" && (id == 24))
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Address Proof", Value="Address_Proof"},
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1071" && (id == 523))
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Address Proof", Value="Address_Proof"},
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1071" && (id == 1275))
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Address Proof", Value="Address_Proof"},
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "576")
                        {
                            //pageName = "AddCriminalInfo";
                            //    List<SelectListItem> objData = new List<SelectListItem>
                            //{
                            //    new SelectListItem{Text="Identity Proof", Value="Identity_Proof"},
                            //    new SelectListItem{Text="Address Proof", Value="Address_Proof"},
                            //    new SelectListItem{Text="Authorized Letter", Value="Authorized_Letter"},
                            //    new SelectListItem{Text="Other Proof", Value="Other_Proof"}
                            //};
                            //    ViewBag.DocumentList = objData;
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Voter ID", Value="voter_ID"},
                            new SelectListItem{Text="DL", Value="D_L"},
                            new SelectListItem{Text="Ration Card", Value="Ration_Card"},
                            new SelectListItem{Text="Telephone Bill", Value="Telephone_Bill"},
                            new SelectListItem{Text="Rent Agreement", Value="Rent_Agreement"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="Electricity Bill", Value="Electricity_Bill"},
                            new SelectListItem{Text="Gas Supply Bill", Value="Gas_Supply_Bill"},
                            new SelectListItem{Text="Water Bill", Value="Water_Bill"},
                            new SelectListItem{Text="AAdhar Card", Value="AAdhar Card"},
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "709" && (id == 29))
                        {
                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Full Page Aadhar (Complete)", Value="Full_Page_Aadhar_Complete"},
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else
                        {


                            //pageName = "AddCriminalInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Identity Proof", Value="Identity_Proof"},
                            new SelectListItem{Text="Address Proof", Value="Address_Proof"},
                            new SelectListItem{Text="Authorized Letter", Value="Authorized_Letter"},
                            new SelectListItem{Text="Other Proof", Value="Others"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                    }
                    if (chkId == 11)
                    {
                        if (clientId == "957")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                            {
                                new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                                new SelectListItem{Text="Final Marksheet", Value="Final_Marksheet"},
                                new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                                new SelectListItem{Text="Other Proof", Value="Other_Proof"},
                            };
                            ViewBag.DocumentList = objData;
                            ViewBag.IsAstra957 = true;
                        }
                        else if (clientId == "635")
                        {
                            //pageName = "AddEducationInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                            new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                            new SelectListItem{Text="Grade Sheet", Value="Grade_Sheet"},
                            new SelectListItem{Text="Other Proof", Value="Other_Proof"},
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (CapgeminiID == 787)
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                            new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                            new SelectListItem{Text="Grade Sheet", Value="Grade_Sheet"},
                            new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="My Hire", Value="MyHire"},
                            new SelectListItem{Text="Other Proof", Value="Other_Proof"},
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1425" && id == 282)
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Professional Certificate", Value="Professional_Certificate"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1425" || clientId == "1429" | clientId == "1514")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Transcript", Value="Transcripts"},
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                        };
                            ViewBag.DocumentList = objData;
                        }

                        else if (clientId == "1449")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Academic Transcript", Value="Academic_Transcript"},
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                        };
                            ViewBag.DocumentList = objData;
                        }

                        else if (clientId == "559")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                            {
                                new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                                new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                                new SelectListItem{Text="Grade Sheet/Marksheet", Value="Grade_Sheet_Marksheet"},
                                new SelectListItem{Text="Other Proof", Value="Other_Proof"},
                            };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1071")
                        {
                            //pageName = "AddEducationInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="10th Certificates", Value="10th _ertificates"},
                            new SelectListItem{Text="12th Certificates", Value="12th _ertificates"},
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                            new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                            new SelectListItem{Text="Grade Sheet", Value="Grade_Sheet"},
                            //new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="Other Proof", Value="Other_Proof"},
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "576")
                        {
                            //pageName = "AddEducationInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                            new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                            new SelectListItem{Text="Grade Sheet", Value="Grade_Sheet"},
                            //new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="Other Proof", Value="Others"},
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else
                        {
                            //pageName = "AddEducationInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                            new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                            new SelectListItem{Text="Grade Sheet", Value="Grade_Sheet"},
                            //new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="Other Proof", Value="Other_Proof"},
                        };
                            ViewBag.DocumentList = objData;
                            if (clientId == "1540") ViewBag.IsDaimlerEdu = true;

                        }
                    }
                    if (chkId == 12)
                    {
                        if (id == 68)
                        {
                            if (clientId == "1540")
                            {
                                List<SelectListItem> objData = new List<SelectListItem>
                                {
                                    new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                                    new SelectListItem{Text="Experience Letter", Value="Experience_Letter"},
                                    new SelectListItem{Text="Relieving Letter", Value="Relieving_Letter"},
                                    new SelectListItem{Text="Others", Value="Others"},
                                };
                                ViewBag.DocumentList = objData;
                                ViewBag.IsDaimlerCurrentEmp = true;
                            }

                            else if (clientId == "635")
                            {
                                //pageName = "AddEmploymentInfo";
                                List<SelectListItem> objData = new List<SelectListItem>
                            {
                            new SelectListItem{Text="Experience Letter ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter ", Value="Relieving_Letter"},

                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                                ViewBag.DocumentList = objData;
                            }
                            //Fractal Analytics Private Limited

                            else if (clientId == "679")
                            {
                                //pageName = "AddEmploymentInfo";
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter ", Value="Experience_Letter"},
                            new SelectListItem{Text="Resignation acceptance Letter", Value="Resignationacceptance_letter"},

                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                                ViewBag.DocumentList = objData;
                            }
                            else if (CapgeminiID == 787)
                            {
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter ", Value="Relieving_Letter"},
                            new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Payslip", Value="Payslip"},
                            new SelectListItem{Text="My Hire", Value="MyHire"},
                            new SelectListItem{Text="Others", Value="Others"},

                        };
                                ViewBag.DocumentList = objData;
                            }

                            else if (clientId == "1425" || clientId == "1514")
                            {
                                List<SelectListItem> objData = new List<SelectListItem>
                                {
                           new SelectListItem{Text="Employment documents", Value="Employment documents"},
                            new SelectListItem{Text="Relieving letter", Value="Relieving letter"}
                        };
                                ViewBag.DocumentList = objData;
                            }
                            else if (clientId == "1449")
                            {
                                List<SelectListItem> objData = new List<SelectListItem>
                                {
                           new SelectListItem{Text="Experience Letter", Value="Experience_Letter"},
                            new SelectListItem{Text="Service Certificate", Value="Service_Certificate"},
                            new SelectListItem{Text="Other", Value="Other"}
                        };
                                ViewBag.DocumentList = objData;
                            }
                            else if (clientId == "1395" || clientId == "1414" || clientId == "1415" || clientId == "1416")
                            {
                                List<SelectListItem> objData = new List<SelectListItem>
                                {
                           new SelectListItem{Text="Any Available Employment Document", Value="Any_Available_Employment_Document"},
                        };
                                ViewBag.DocumentList = objData;
                            }
                            else if (clientId == "559")
                            {
                                //pageName = "AddEmploymentInfo";
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter * ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter * ", Value="Relieving_Letter"},
                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Payslip", Value="Payslip"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                                ViewBag.DocumentList = objData;
                            }
                            else
                            {
                                //pageName = "AddEmploymentInfo";
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter ", Value="Relieving_Letter"},
                            new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Payslip", Value="Payslip"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                                ViewBag.DocumentList = objData;

                            }
                        }
                        else
                        {
                            // For TATA Elxsi
                            if (clientId == "635")
                            {
                                //pageName = "AddEmploymentInfo";
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter * ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter * ", Value="Relieving_Letter"},

                                new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                                ViewBag.DocumentList = objData;
                            }

                            else if (CapgeminiID == 787)// capgemini
                            {
                                //pageName = "AddEmploymentInfo";
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter * ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter * ", Value="Relieving_Letter"},
                            new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Payslip", Value="Payslip"},
                                  new SelectListItem{Text="My Hire", Value="MyHire"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                                ViewBag.DocumentList = objData;
                            }
                            else if (clientId == "1425" || clientId == "1395" || clientId == "1414" || clientId == "1415" || clientId == "1416" || clientId == "1449" || clientId == "1514")
                            {
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                           new SelectListItem{Text="Experience Letter", Value="Experience_Letter"},
                            new SelectListItem{Text="Service Certificate", Value="Service_Certificate"},
                            new SelectListItem{Text="Other", Value="Other"}
                        };
                                ViewBag.DocumentList = objData;
                            }
                            else if (clientId == "559")
                            {
                                //pageName = "AddEmploymentInfo";
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter * ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter * ", Value="Relieving_Letter"},
                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Payslip", Value="Payslip"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                                ViewBag.DocumentList = objData;
                            }
                            else if (clientId == "679" && id == 69)
                            {
                                //pageName = "AddEmploymentInfo";
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter * ", Value="Experience_Letter"},
                           // new SelectListItem{Text="Relieving Letter * ", Value="Relieving_Letter"},
                           // new SelectListItem{Text="LOA", Value="LOA"},
                           // new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Payslip *", Value="Payslip"},
                            //new SelectListItem{Text="Others", Value="Others"},
                        };
                                ViewBag.DocumentList = objData;
                            }
                            else if (clientId == "576")
                            {
                                //pageName = "AddEmploymentInfo";
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter * ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter * ", Value="Relieving_Letter"},
                            new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Payslip", Value="Payslip"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                                ViewBag.DocumentList = objData;
                            }
                            else
                            {
                                //pageName = "AddEmploymentInfo";
                                List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter * ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter * ", Value="Relieving_Letter"},
                            new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Payslip", Value="Payslip"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                                ViewBag.DocumentList = objData;
                            }
                        }

                    }
                    if (chkId == 26)
                    {
                        if (clientId == "1355" && id == 136)//Oblivious Software Limited (Ireland)
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="National ID", Value="National_ID"},
                        };
                            ViewBag.DocumentList = objData;
                        }

                        else if (clientId == "644")//SMART CHIP CLIENT
                        {
                            //pageName = "AddNationalIdentityInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="PAN Card", Value="PANCard"},
                        };
                            ViewBag.DocumentList = objData;
                        }

                        else if (clientId == "1331" || clientId == "1514")//Burgeon IT Services Pvt Ltd.
                        {
                            //pageName = "AddNationalIdentityInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Primary ID 1", Value="Primary_ID_1"},
                            new SelectListItem{Text="Primary ID 2", Value="Primary_ID_2"},
                            new SelectListItem{Text="Secondary ID 1", Value="Secondary_ID_1"},
                            new SelectListItem{Text="Secondary ID 2", Value="Secondary_ID_2"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1425" && id == 763)//Genpact Australia
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="100 Point Document 1", Value="100_Point_Document_1"},
                            new SelectListItem{Text="100 Point Document 2", Value="100_Point_Document_2"},
                            new SelectListItem{Text="100 Point Document 3", Value="100_Point_Document_3"},
                            new SelectListItem{Text="100 Point Document 4", Value="100_Point_Document_4"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if ((clientId == "1425" || clientId == "1428" || clientId == "1429" || clientId == "1514") && id == 136)//Genpact Australia-Eup
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="National ID Proof", Value="National_ID_Proof"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1395" || clientId == "1414" || clientId == "1415" || clientId == "1416")//Chagrebee
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="National ID", Value="National_ID"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if ((clientId == "1425" || clientId == "1428") && id == 134)//Genpact Australia-Eup
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Copy of Passport", Value="Copy_of_Passport"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1425" && id == 267)//Genpact Australia
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Copy of Passport", Value="Copy_of_Passport"},
                            new SelectListItem{Text="Copy of Visa document", Value="Copy_of_Visa_document"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "75")
                        {
                            //pageName = "AddNationalIdentityInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="PAN Card", Value="PANCard"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                             new SelectListItem{Text="Driving License", Value="DrivingLicense"},
                            new SelectListItem{Text="VoterID", Value="VoterID"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1450")
                        {
                            //pageName = "AddNationalIdentityInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Driving License", Value="DrivingLicense"},
                            new SelectListItem{Text="PAN Card", Value="PANCard"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="VoterID", Value="VoterID"},
                            new SelectListItem{Text="OtherProof", Value="OtherProof"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "684")
                        {
                            //pageName = "AddNationalIdentityInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                            {
                                new SelectListItem{Text="Driving License", Value="DrivingLicense"},
                                new SelectListItem{Text="PAN Card", Value="PANCard"},
                                new SelectListItem{Text="Passport", Value="Passport"},
                                new SelectListItem{Text="VoterID", Value="VoterID"}
                            };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "576")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Aadhaar Card", Value="AAdhar Card"},
                            new SelectListItem{Text="Driving License", Value="DrivingLicense"},
                            new SelectListItem{Text="PAN Card", Value="PANCard"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="VoterID", Value="voter_ID"},
                            new SelectListItem{Text="OtherProof", Value="Others"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else
                        {
                            //pageName = "AddNationalIdentityInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Aadhaar Card", Value="AadhaarCard"},
                            new SelectListItem{Text="Driving License", Value="DrivingLicense"},
                            new SelectListItem{Text="PAN Card", Value="PANCard"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="VoterID", Value="VoterID"},
                            new SelectListItem{Text="OtherProof", Value="OtherProof"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                    }
                    if (chkId == 30)
                    {
                        if (clientId == "576")
                        {
                            //pageName = "AddReferenceInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else
                        {
                            //pageName = "AddReferenceInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                            ViewBag.DocumentList = objData;
                        }

                    }
                    if (chkId == 40)
                    {
                        if ((clientId == "1355" || clientId == "1348") && id == 201)//Oblivious Software Limited (Ireland) and Oblv Software India Private Limited
                        {
                            //pageName = "AddNationalIdentityInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="National ID 1", Value="National_ID_1"},
                            new SelectListItem{Text="National ID 2", Value="National_ID_2"}


                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1331")//Burgeon IT Services Pvt Ltd.
                        {
                            //pageName = "AddNationalIdentityInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Primary ID 1", Value="Primary_ID_1"},
                            new SelectListItem{Text="Primary ID 2", Value="Primary_ID_2"},
                            new SelectListItem{Text="Secondary ID 1", Value="Secondary_ID_1"},
                            new SelectListItem{Text="Secondary ID 2", Value="Secondary_ID_2"}

                        };
                            ViewBag.DocumentList = objData;
                        }

                        else if (clientId == "1429" && id == 774)//Burgeon IT Services Pvt Ltd.
                        {
                            //pageName = "AddNationalIdentityInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="National ID", Value="National_ID"},
                            new SelectListItem{Text="LOA (as attached in the mail)", Value="LOA_(as_attached_in_the_mail)"}
                        };
                            ViewBag.DocumentList = objData;
                        }

                        else if (clientId == "1093" && id == 416)//Walsons
                        {
                            List<SelectListItem> selectListItems = new List<SelectListItem>
                            {
                                new SelectListItem{Text="UAN Passbook", Value="UAN_Passbook"},
                                new SelectListItem{Text="Others", Value="Others"}
                            };
                            ViewBag.DocumentList = selectListItems;
                        }

                        else if (clientId == "1093" && id == 304)//Walsons
                        {
                            List<SelectListItem> selectListItems = new List<SelectListItem>
                            {
                                new SelectListItem{Text="CV / Resume",Value="CV"},
                                new SelectListItem{Text="Others", Value="Others"}
                            };
                            ViewBag.DocumentList = selectListItems;
                        }

                        else if (clientId == "576" && id == 38)//Nagaroo cv validation
                        {
                            //pageName = "AddSpecialCheckInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Resume / CV", Value="Resume"},
                            new SelectListItem{Text="ID", Value="ID"}
                        };
                            ViewBag.DocumentList = objData;
                        }

                        else if (clientId == "1425" && id == 765)//Nagaroo cv validation
                        {
                            //pageName = "AddSpecialCheckInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Resume", Value="Resume"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "193" && id == 416) // Acuity's 
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Salary Slip", Value="Salary_Slip"},
                            new SelectListItem{Text="EPFO Passbook", Value="EPFO_Passbook"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "1089" && id == 416)
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Pan Card", Value="Pan_Card"},
                             new SelectListItem{Text="Adhaar", Value="Adhaar_"},
                              new SelectListItem{Text="UAN document", Value="UAN_document"},
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else if (clientId == "576")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Special Document", Value="SpecialDoc"},
                            new SelectListItem{Text="Pan Card", Value="PANCard"},
                            new SelectListItem{Text="Adhaar", Value="AAdhar Card"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                        else
                        {
                            //pageName = "AddSpecialCheckInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Special Document", Value="SpecialDoc"},
                            new SelectListItem{Text="Pan Card", Value="Pan_Card"},
                            new SelectListItem{Text="Adhaar", Value="Adhaar"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                            ViewBag.DocumentList = objData;
                        }
                    }

                    //return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Updated 19-01-2018 rahul
        //[HttpGet]
        //[SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public JsonResult DeleteCandidateUploadedTempDocs(int id = 0, short SubCheckRowID = 0, int PersonalRowid = 0)
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    string FileName = repoPQVerifiedUploadDoc.DeleteUploadedTempDocsByID(id, SubCheckRowID);

                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["provideddocumentsAPI"].ToString();
                    string base64 = FileSaveDownloadAPI.DocReportdeleteDynamic(FileName, FldPathDoc);
                    if (base64.Contains("successfully"))
                    {
                        result = true;
                    }
                    //string path = Server.MapPath(FldPathDoc + FileName);
                    //FileInfo file = new FileInfo(path);
                    //if (file.Exists)//check file exsit or not
                    //{
                    //    file.Delete();
                    //    result = true;
                    //}
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // Updated 29-01-2018
        [NonAction]
        void BindNationalIDProofList()
        {
            try
            {
                IEnumerable<SelectListItem> NationalIDProofList = new List<SelectListItem>
            {
            new SelectListItem { Text = "Aadhaar Card", Value = "Aadhaar Card", Selected=false },
            new SelectListItem { Text = "Driving License", Value = "Driving License", Selected=false },
            new SelectListItem { Text = "Passport", Value = "Passport", Selected=false },
            new SelectListItem { Text = "Pan Card", Value = "Pan Card", Selected=false },
            new SelectListItem { Text = "Voter ID Card", Value = "Voter ID Card", Selected=false },
            };
                ViewBag.NationalIDProofList = NationalIDProofList;
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        // Updated 01-02-2018

        public ActionResult FinalSubmit(string id = "")
        {
            try
            {
                long ids = Convert.ToInt64(clsCommonMethods.PDecrypt(id));
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                if (Session["ClientRowID"].ToString() == "679")
                {
                    string refno = "";
                    DataTable dts = tpc.GetNewCandidateDetails(ids);
                    if (dts.Rows.Count > 0)
                    {
                        refno = dts.Rows[0]["CompanyRefNo"].ToString();
                    }
                    if (!string.IsNullOrEmpty(refno))
                    {
                        dt = tpc.GetCandidateChecksCompletenessStatus_Fractal(ids);
                    }
                    else
                    {
                        dt = tpc.GetCandidateFormCompletenessStatus(ids);
                    }


                }
                else if (Session["ClientRowID"].ToString() == "559")
                {
                    dt = tpc.AutoCandidateChecksCompletenessStatus(ids);
                }
                else
                {
                    dt = tpc.GetCandidateFormCompletenessStatus(ids);
                }


                DataTable Ndt = new DataTable();
                Ndt = tpc.GetNewCandidateDetails(Convert.ToInt64(ids));
                DataTable dt2 = new DataTable();
                dt2 = tpc.GetCandidateFormDigitalStatus(Convert.ToInt64(ids));
                string loa = "";
                if (dt2.Rows.Count > 0)
                {
                    loa = dt2.Rows[0]["Loa"].ToString();
                }
                if (loa == "")
                {
                    TempData["AlertMsg"] = "Please Fill  Singnature.";
                    return RedirectToAction("Index");
                }


                if (Session["ClientRowID"].ToString() == "1680")
                {
                    string ResNa = tpc.checkAllSubcheckNotApplicable(Convert.ToInt32(ids));
                    if(ResNa == "AllChecksNA")
                    {
                        TempData["AlertMsg"] = "Form Can't be Submitted , Because All checks are Not Applicable";
                        return RedirectToAction("Index");
                    }

                }

                string MissingInformation = "";
                string MissingDocs = "";

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["InoStatus"].ToString() == "Pending")
                    {
                        MissingInformation = " " + dr["InformationType"].ToString();
                        break;
                    }
                    if (dr["DocStatus"].ToString() == "Pending")
                    {
                        MissingDocs = dr["InformationType"].ToString();
                        break;
                    }
                }

                if (MissingDocs != "")
                {
                    TempData["AlertMsg"] = "Request you to upload " + MissingDocs + "document to move further";
                }
                else if (MissingInformation != "")
                {
                    TempData["AlertMsg"] = "Request you to complete " + MissingInformation + " check to move further";
                }
                else
                {
                    TempData["AlertMsg"] = "Form Uploaded Successfully. ! Thanks for providing the complete details";
                    TempData["formfillmsg"] = TempData["AlertMsg"];
                }

                Int16 ClientRowID = Convert.ToInt16(Session["ClientRowID"]);


                if (MissingInformation == "" && MissingDocs == "")
                {
                    int createdBy = int.Parse(Session["CandidateUserRowID"].ToString());
                    if (Convert.ToByte(Session["HRApproval"]) == 0)
                    {

                        string result = "";
                        if (Session["ClientRowID"].ToString() == "679")
                        {
                            string refno = "";
                            DataTable dts = tpc.GetNewCandidateDetails(ids);
                            if (dts.Rows.Count > 0)
                            {
                                refno = dts.Rows[0]["CompanyRefNo"].ToString();
                            }
                            if (!string.IsNullOrEmpty(refno))
                            {
                                result = tpc.ApproveOrFinalSubmitByCandidate_Fractal(ids, createdBy);
                            }
                            else
                            {
                                result = tpc.ApproveOrFinalSubmitByCandidate(ids, createdBy);

                            }

                        }
                        else
                        {
                            result = tpc.ApproveOrFinalSubmitByCandidate(ids, createdBy);

                            if (result == "Success")
                            {
                                if (ClientRowID == 753)
                                {
                                    UpdateFacematchCheckStatus(Convert.ToInt32(ids), ClientRowID);

                                }
                            }

                        }
                        if (result == "Success")
                        {
                            SendMail(ids, ClientRowID);
                            try
                            {
                                MessageStatus objMessage = new MessageStatus();
                                WhatsAppAPI objs = new WhatsAppAPI();
                                objMessage = objs.SendWhatsAppMessage("+91" + Ndt.Rows[0]["MobileNo"].ToString(), "submission_from_candidate_", Ndt.Rows[0]["CandidateFullName"].ToString(), "media_template", "", "");
                                cc.sp_AddAuditWhatsAppAPI(Convert.ToInt32(ids), objMessage.message, objMessage.status, "Submission From Candidate");
                            }
                            catch (Exception Ex)
                            {
                                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                            }
                        }
                        if (result == "Success")
                        {
                            MoveLOADocUploadedByCandidate(ids);
                            if (ClientRowID == 1749)
                            {
                                new API().UpdateAPICheckStatus(ClientRowID.ToString(), Convert.ToInt32(ids), 0, "ALL_DOCUMENTS_UPLOADED");
                            }
                            return RedirectToAction("CandidateFeedback", "Account", new { area = "", ClientRowID = ClientRowID.ToString(), CandidateName = Ndt.Rows[0]["CandidateFullName"].ToString().Trim(), EmailID = Ndt.Rows[0]["EmailID"].ToString().Trim() });

                        }

                    }
                    if (Convert.ToByte(Session["HRApproval"]) == 1)
                    {

                        if (cc.GetIslivenessCheckExists(Convert.ToInt32(ids)))
                        {
                            string result = "";
                            result = tpc.ApproveOrFinalSubmitByCandidate(ids, createdBy);

                            if (result == "Success")
                            {
                                updateCandidatelivenessCheckStatus(Convert.ToInt32(ids));
                                SendMail(ids, ClientRowID);
                                try
                                {
                                    MessageStatus objMessage = new MessageStatus();
                                    WhatsAppAPI objs = new WhatsAppAPI();
                                    objMessage = objs.SendWhatsAppMessage("+91" + Ndt.Rows[0]["MobileNo"].ToString(), "submission_from_candidate_", Ndt.Rows[0]["CandidateFullName"].ToString(), "media_template", "", "");
                                    cc.sp_AddAuditWhatsAppAPI(Convert.ToInt32(ids), objMessage.message, objMessage.status, "Submission From Candidate");
                                }
                                catch
                                {
                                }
                                MoveLOADocUploadedByCandidate(ids);
                                if (ClientRowID == 1749)
                                {
                                    new API().UpdateAPICheckStatus(ClientRowID.ToString(), Convert.ToInt32(ids), 0, "ALL_DOCUMENTS_UPLOADED");
                                }
                                return RedirectToAction("LogOutCandidate", "Account", new { area = "" });
                            }

                        }
                        else
                        {
                            DataTable dt1 = bl.getCaseStatusWithCandidate(ids);

                            if (dt1.Rows[0]["CaseStatus"].ToString() == "1")
                            {
                                tpc.ApproveOrFinalSubmitByCandidateToHR(ids);
                                try
                                {
                                    if (ClientRowID == 1428 || ClientRowID == 1449)
                                    {
                                        SendMailInterNationalClient(ids, ClientRowID);
                                    }
                                    else
                                    {
                                        SendMail(ids, ClientRowID, "ClientAprrovalPending");
                                    }
                                }
                                catch { }
                                try
                                {
                                    MessageStatus objMessage = new MessageStatus();
                                    WhatsAppAPI objs = new WhatsAppAPI();
                                    objMessage = objs.SendWhatsAppMessage("+91" + Ndt.Rows[0]["MobileNo"].ToString(), "submission_from_candidate_", Ndt.Rows[0]["CandidateFullName"].ToString(), "media_template", "", "");
                                    // ClientCustomize cc = new ClientCustomize();
                                    cc.sp_AddAuditWhatsAppAPI(Convert.ToInt32(ids), objMessage.message, objMessage.status, "Submission From Candidate");
                                }
                                catch(Exception Ex)
                                {
                                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

                                }
                                bl.UpdateCaseStatus(ids, 2, 0);
                                if (ClientRowID == 1749)
                                {
                                    new API().UpdateAPICheckStatus(ClientRowID.ToString(), Convert.ToInt32(ids), 0, "ALL_DOCUMENTS_UPLOADED");
                                }
                                return RedirectToAction("CandidateFeedback", "Account", new { area = "", ClientRowID = ClientRowID.ToString(), CandidateName = Ndt.Rows[0]["CandidateFullName"].ToString().Trim(), EmailID = Ndt.Rows[0]["EmailID"].ToString().Trim() });
                            }

                            bl.UpdateCaseStatus(ids, 2, 0);//userid 0 means candidate and case status 2 means- showing HR to Approve
                            if (ClientRowID == 1749)
                            {
                                new API().UpdateAPICheckStatus(ClientRowID.ToString(), Convert.ToInt32(ids), 0, "ALL_DOCUMENTS_UPLOADED");
                            }
                            return RedirectToAction("CandidateFeedback", "Account", new { area = "", ClientRowID = ClientRowID.ToString(), CandidateName = Ndt.Rows[0]["CandidateFullName"].ToString().Trim(), EmailID = Ndt.Rows[0]["EmailID"].ToString().Trim() });

                        }


                    }

                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return RedirectToAction("Index");

            #endregion
        }
        public void UpdateFacematchCheckStatus(int Pid, int Cid)
        {
            DataBaseConnections Dc = new DataBaseConnections();
            ClientCustomize cc = new ClientCustomize();

            var TempDt = Dc.GetTempPqPersonalData(Convert.ToInt32(Pid));
            if (TempDt.Rows.Count > 0)
            {
                string CompanyRefNo = Convert.ToString(TempDt.Rows[0]["CompanyRefNo"]);
                int PersonalRowID = Convert.ToInt32(TempDt.Rows[0]["PersonalRowID"]);

                var CheckDT = cc.GetDispositionPQCandidateChecks(PersonalRowID, 799);
                if (CheckDT.Rows.Count > 0)
                {
                    var FaceMatchDT = cc.Sp_getPaysPrintVerifyFaceMatch(Pid.ToString());
                    if (FaceMatchDT.Rows.Count > 0)
                    {
                        string Match1 = "";
                        string Match2 = "";
                        for (int i = 0; i < FaceMatchDT.Rows.Count; i++)
                        {
                            if (Match1 == "")
                            {
                                Match1 = Convert.ToString(FaceMatchDT.Rows[i]["statusData"]);
                            }
                            else if (Match2 == "")
                            {
                                Match2 = Convert.ToString(FaceMatchDT.Rows[i]["statusData"]);
                            }

                        }
                        if (Match2 != "")
                        {
                            if (Match2 == "SUCCESS" && Match1 == "SUCCESS")
                            {
                                var rst = cc.UpdateCandidateChecksReportComments(PersonalRowID, 799);
                                cc.UpdatePQCandidateChecksColorCode(0, 6, "Green", PersonalRowID, 799, "Verified");
                                cc.UpdateCheckStatus(PersonalRowID, 799, 1, 36, "Check", 153);
                                var dt = cc.Antecedents_client_select_form(753, 799, "Operation", "Check");

                                if (dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        int Antecedents = Convert.ToInt32(dt.Rows[j]["AntecedentsRowID"]);
                                      cc.InsertUpdateFaceMatchCheckDetails(PersonalRowID, 799, Antecedents, 753, "Pass");
                                    }
                                }
                            }
                            else
                            {                             
                                cc.UpdatePQCandidateChecksColorCode(248, 1, "Red", PersonalRowID, 799, "Liveness failed");
                                cc.UpdateCheckStatus(PersonalRowID, 799, 1, 36, "Check", 153);
                                var dt = cc.Antecedents_client_select_form(753, 799, "Operation", "Check");
                                if (dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        int Antecedents = Convert.ToInt32(dt.Rows[j]["AntecedentsRowID"]);
                                        cc.InsertUpdateFaceMatchCheckDetails(PersonalRowID, 799, Antecedents, 753, "Fail");
                                    }
                                }
                            }
                        }

                    }
                }

            }
        }

        public void updateCandidatelivenessCheckStatus(int ids)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataBaseConnections DC = new DataBaseConnections();
                var Pdt = DC.GetPersonalDetailsForPersistantClient(Convert.ToInt32(ids));
                if (Pdt.Rows.Count > 0)
                {
                    int PersonalRowId = Convert.ToInt32(Pdt.Rows[0]["PersonalRowID"]);
                    string LJsonString = Convert.ToString(Pdt.Rows[0]["JsonString"]);
                    if (LJsonString != "")
                    {
                        LivenessRoot model = JsonConvert.DeserializeObject<LivenessRoot>(LJsonString);
                        double faceMatchScore = model.result.apiResponse.result.faceMatchScore;
                        double livenessScore = model.result.apiResponse.result.livenessScore;
                        bool isFaceMatch = model.result.apiResponse.result.isFaceMatch;
                        if (faceMatchScore > 60)
                        {
                            if (livenessScore > 60)
                            {
                                var rst = cc.UpdateCandidateChecksReportComments(PersonalRowId, 890);
                                cc.UpdatePQCandidateChecksColorCode(0, 6, "Green", PersonalRowId, 890, "Verified");
                                cc.UpdateCheckStatus(PersonalRowId, 890, 1, 36, "Check", 153);
                                DC.UpdatelivenessChecktbldataEntryData(PersonalRowId, 890, "Positive");
                            }
                            else
                            {
                                var rst = cc.UpdateCandidateChecksReportComments(PersonalRowId, 890);
                                cc.UpdatePQCandidateChecksColorCode(248, 1, "Red", PersonalRowId, 890, "Liveness failed");
                                cc.UpdateCheckStatus(PersonalRowId, 890, 1, 36, "Check", 153);
                                DC.UpdatelivenessChecktbldataEntryData(PersonalRowId, 890, "Negative");
                            }
                        }
                        else
                        {
                            var rst = cc.UpdateCandidateChecksReportComments(PersonalRowId, 890);
                            cc.UpdatePQCandidateChecksColorCode(248, 1, "Red", PersonalRowId, 890, "Face Match failed");
                            cc.UpdateCheckStatus(PersonalRowId, 890, 1, 36, "Check", 153);
                            DC.UpdatelivenessChecktbldataEntryData(PersonalRowId, 890, "Negative");
                        }
                    }

                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

            }
        }
        private void SendMail(long personalRowId, short ClientRowID, string action = "")
        {

            #region Send Mail To Candidate
            string PersonalIds = personalRowId.ToString();
            if (!string.IsNullOrEmpty(PersonalIds))
            {

                string[] PersonalID = PersonalIds.Split(',');
                string TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CandidateAgreement.docx";
                string fileName = Path.GetFileNameWithoutExtension("CandidateAgreement.docx");

                string MailSignature = string.Empty;
                string From = string.Empty;
                string DisplayName = string.Empty;
                string Cc = "";
                clsSendMail mail;

                var cSmtp = repoPQClient.GetPQClientSendMailFromById(ClientRowID);
                if (cSmtp != null && cSmtp.MailSendBy > 0)
                {
                    mail = new clsSendMail(cSmtp.SMTPServer, cSmtp.SMTPPort, cSmtp.SMTPUserName, clsCommonMethods.PasswordDecrypt(cSmtp.SMTPPassword), Convert.ToBoolean(cSmtp.EnableSsl));

                    From = cSmtp.SMTPUserName;
                    DisplayName = cSmtp.ClientName.Trim();
                    MailSignature = string.Concat("<b>", cSmtp.ClientName.Trim(), "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", cSmtp.SMTPUserName, "'>", cSmtp.SMTPUserName, "</a>");
                }
                else
                {
                    mail = new clsSendMail();
                    var signature = mail.GetCompanyMailSignature();

                    if (signature != null)
                    {
                        MailSignature = string.Concat("For Securitas India. <br /><br />");
                        MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(),
                             "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", "bgv.helpdesk@securitas-india.com", "'>", "bgv.helpdesk@securitas-india.com", "</a>");
                    }
                    From = signature.GeneralEmail;
                    DisplayName = signature.CompanyName.Trim();
                }

                foreach (var item in PersonalID)
                {
                    int pID = Convert.ToInt32(item);
                    var personalData = repoPQPersonal.GetTempPQPersonalDetailsForSendMailById(pID);
                    DataTable dtAccess = bl.GetClientAccessType(ClientRowID);
                    string spocEmail = "";
                    string cSpocEmail = "";
                    DataTable dtEmail = new DataTable();
                    int ClientAcctessType = 0;
                    var loginDetail = repoCandidate.GetCandidateLoginDetailsForMailSendById(pID);
                    if (dtAccess.Rows.Count > 0)
                    {
                        ClientAcctessType = Convert.ToInt32(dtAccess.Rows[0]["ClientAccessType"].ToString());
                        if (ClientAcctessType == 3)
                        {
                            DataTable DtAm = new DataTable();
                            DtAm = bl.userwise_ReportingEmailAccess(personalData.ClientUserRowID);
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
                            dtEmail = bl.GetEmailStructureForAlert(ClientRowID, personalData.ClientUserRowID, "Insuff", personalData.PersonalRowID);
                            if (dtEmail.Rows.Count > 0)
                            {
                                spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                                cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                                if (ClientRowID == 1481)
                                {
                                    Cc = spocEmail + "," + cSpocEmail + "," + loginDetail.AlternateEmailID;
                                }
                                else
                                {
                                    Cc = spocEmail + "," + cSpocEmail;
                                }
                            }
                        }
                    }
                    if (loginDetail != null)
                    {
                        string body = string.Empty;
                        string MailSubject = "";
                        ClientCustomize cc = new ClientCustomize();
                        var templateDB = cc.getAlertMailtemplate(ClientRowID, "Final Submit", 0, 1, "candidateFinalEmailer");
                        if (templateDB.Rows.Count > 0)
                        {
                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                        }
                        //using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/candidateFinalEmailer.html")))
                        //{
                        //    body = reader.ReadToEnd();
                        //}

                        body = body.Replace("{CandidateName}", loginDetail.CandidateName);
                        body = body.Replace("{ClientRefId}", personalData.ClientRefID);
                        body = body.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                        body = body.Replace("{MailSignature}", MailSignature);

                        //string Subject = loginDetail.CandidateName.Replace("\n", "") + "  Background Verification";
                        string Subject = string.Empty;
                        if (ClientRowID == 957)     //AstraZeneca India Private Limited
                        {
                            Subject = "BGV submitted : " + loginDetail.CandidateName.Replace("\n", "") + " - " + personalData.ClientRefID;
                        }
                        else
                        {
                            Subject = loginDetail.CandidateName.Replace("\n", "") + "  Background Verification";
                        }
                        string To = loginDetail.CandidateEmail;
                        string ToMail = To.ToLower();
                        String Ccmail = Cc.ToLower();
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

                        Ccmail = Ccmail.Replace(",", ";");
                        arToMail = Ccmail.Split(';');
                        Ccmail = "";
                        foreach (string a in arToMail)
                        {
                            if (!Ccmail.Contains(a))
                            {
                                Ccmail = Ccmail + a.Trim() + ",";
                            }
                        }
                        if (Ccmail.Length > 1)
                            Ccmail = Ccmail.Substring(0, Ccmail.Length - 1);
                        bool flag = false;
                        if (ClientAcctessType == 3)
                        {
                            string ccc = "";
                            string Tos = "";
                            Tos = spocEmail;
                            ccc = cSpocEmail;

                            if (action == "ClientAprrovalPending")
                            {
                                ClientCustomize ccc1 = new ClientCustomize();
                                ccc1.InsertUpdateFollowUpEmails(Convert.ToInt32(personalRowId), 0, "Case Approval Pending", 2, Subject, body, Tos, ccc, "", null, Convert.ToInt32(Session["TeamMemberRowID"]), "Add_Update");

                            }
                            flag = mail.SendMail(DisplayName, From, Tos, ccc, Subject, body);
                        }

                        else
                        {
                            if (ClientRowID == 257)
                            {
                                string bcc = "";
                                string ccc = "";
                                ccc = spocEmail;
                                bcc = cSpocEmail;
                                if (action == "ClientAprrovalPending")
                                {
                                    ClientCustomize ccc1 = new ClientCustomize();
                                    ccc1.InsertUpdateFollowUpEmails(Convert.ToInt32(personalRowId), 0, "Case Approval Pending", 2, Subject, body, ToMail, ccc, bcc, null, Convert.ToInt32(Session["TeamMemberRowID"]), "Add_Update");

                                }
                                flag = mail.SendMail(DisplayName, From, ToMail, ccc, bcc, Subject, body);
                            }
                            else
                            {
                                if (ClientRowID == 992 || ClientRowID == 993 || ClientRowID == 994 || ClientRowID == 1125 || ClientRowID == 1243)
                                {
                                    DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                    var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                    if (recuiteremail != "")
                                    {
                                        Ccmail += "," + recuiteremail;
                                    }

                                }
                                if (action == "ClientAprrovalPending")
                                {
                                    ClientCustomize ccc1 = new ClientCustomize();
                                    ccc1.InsertUpdateFollowUpEmails(Convert.ToInt32(personalRowId), 0, "Case Approval Pending", 2, Subject, body, ToMail, Ccmail, "", null, Convert.ToInt32(Session["TeamMemberRowID"]), "Add_Update");

                                }
                                flag = mail.SendMail(DisplayName, From, ToMail, Ccmail, Subject, body);
                            }
                        }

                        /* bool flag = mail.SendMail(DisplayName, From, ToMail, Ccmail, Subject, body)*/;
                        if (flag)
                        {

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
                }
            }
            #endregion
        }

        private void SendMailInterNationalClient(long personalRowId, short ClientRowID)
        {
            #region Send Mail To Candidate
            string PersonalIds = personalRowId.ToString();
            Session["PersonalIds"] = personalRowId;
            if (!string.IsNullOrEmpty(PersonalIds))
            {
                string[] PersonalID = PersonalIds.Split(',');
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();
                ds = cc.InterNationalCandidateLOA(personalRowId);
                DataTable dt = ds.Tables[0];
                DataRow row = dt.Rows[0];
                string loaValue = row["DataEntryValue"].ToString();

                string TrmFldPathDoc = ConfigurationManager.AppSettings["ClientUploadedDocuments_New"].ToString() + loaValue;
                string fileName = Path.GetFileNameWithoutExtension(loaValue);

                MemoryStream file = new MemoryStream(PDFGenerate("This is pdf file", Server.MapPath("CandidateDetail.pdf")).ToArray());
                file.Seek(0, SeekOrigin.Begin);

                Attachment attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                Attachment attachment2 = new Attachment(file, "CandidateDetail.pdf", "application/pdf");

                string MailSignature = string.Empty;
                string From = string.Empty;
                string DisplayName = string.Empty;
                string Cc = "";
                clsSendMail mail;

                var cSmtp = repoPQClient.GetPQClientSendMailFromById(ClientRowID);
                if (cSmtp != null && cSmtp.MailSendBy > 0)
                {
                    mail = new clsSendMail(cSmtp.SMTPServer, cSmtp.SMTPPort, cSmtp.SMTPUserName, clsCommonMethods.PasswordDecrypt(cSmtp.SMTPPassword), Convert.ToBoolean(cSmtp.EnableSsl));

                    From = cSmtp.SMTPUserName;
                    DisplayName = cSmtp.ClientName.Trim();
                    MailSignature = string.Concat("<b>", cSmtp.ClientName.Trim(), "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", cSmtp.SMTPUserName, "'>", cSmtp.SMTPUserName, "</a>");
                }
                else
                {
                    mail = new clsSendMail();
                    var signature = mail.GetCompanyMailSignature();

                    if (signature != null)
                    {
                        MailSignature = string.Concat("For Securitas India. <br /><br />");
                        MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(),
                             "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", "bgv.helpdesk@securitas-india.com", "'>", "bgv.helpdesk@securitas-india.com", "</a>");
                    }
                    From = signature.GeneralEmail;
                    DisplayName = signature.CompanyName.Trim();
                }

                foreach (var item in PersonalID)
                {
                    int pID = Convert.ToInt32(item);
                    var personalData = repoPQPersonal.GetTempPQPersonalDetailsForSendMailById(pID);
                    DataTable dtAccess = bl.GetClientAccessType(ClientRowID);
                    string spocEmail = "";
                    string cSpocEmail = "";
                    DataTable dtEmail = new DataTable();
                    int ClientAcctessType = 0;
                    if (dtAccess.Rows.Count > 0)
                    {
                        ClientAcctessType = Convert.ToInt32(dtAccess.Rows[0]["ClientAccessType"].ToString());
                        if (ClientAcctessType == 3)
                        {
                            DataTable DtAm = new DataTable();
                            DtAm = bl.userwise_ReportingEmailAccess(personalData.ClientUserRowID);
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
                            dtEmail = bl.GetEmailStructureForAlert(ClientRowID, personalData.ClientUserRowID, "Insuff", personalData.PersonalRowID);
                            if (dtEmail.Rows.Count > 0)
                            {
                                spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                                cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                                Cc = spocEmail + "," + cSpocEmail;
                            }
                        }
                    }
                    var loginDetail = repoCandidate.GetCandidateLoginDetailsForMailSendById(pID);
                    if (loginDetail != null)
                    {
                        string body = string.Empty;
                        string MailSubject = "";
                        var templateDB = cc.getAlertMailtemplate(ClientRowID, "Final Submit", 0, 1, "candidateFinalEmailer");
                        if (templateDB.Rows.Count > 0)
                        {
                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                        }
                        body = body.Replace("{CandidateName}", loginDetail.CandidateName);
                        body = body.Replace("{ClientRefId}", personalData.ClientRefID);
                        body = body.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                        body = body.Replace("{MailSignature}", MailSignature);

                        string Subject = loginDetail.CandidateName.Replace("\n", "") + "  Background Verification";
                        string To = loginDetail.CandidateEmail;
                        string ToMail = To.ToLower();
                        String Ccmail = Cc.ToLower();
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

                        Ccmail = Ccmail.Replace(",", ";");
                        arToMail = Ccmail.Split(';');
                        Ccmail = "";
                        foreach (string a in arToMail)
                        {
                            if (!Ccmail.Contains(a))
                            {
                                Ccmail = Ccmail + a.Trim() + ",";
                            }
                        }
                        if (Ccmail.Length > 1)
                            Ccmail = Ccmail.Substring(0, Ccmail.Length - 1);
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
                            flag = mail.SendMailInternational(DisplayName, From, ToMail, Ccmail, Subject, body, attachment, attachment2);
                        }
                        if (flag)
                        {

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
                }
            }
            #endregion
        }

        private MemoryStream PDFGenerate(string message, string ImagePath)
        {
            MemoryStream output = new MemoryStream();
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 50, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, output);
            Candidate_InformationViewModel model = new Candidate_InformationViewModel();
            //PersonalRowID = clsCommonMethods.PDecrypt(PersonalRowID);
            ClientCustomize cc = new ClientCustomize();
            DataTable dt3 = new DataTable();
            string PersonalRowID = Session["PersonalIds"].ToString();
            if (PersonalRowID != "")
            {
                dt3 = cc.TotalWipcandidate_informationForApprovalTemp(Convert.ToInt32(PersonalRowID));
            }
            //Document pdfDoc = new Document(PageSize.A4, 25, 25, 50, 15);
            //PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            if (dt3.Rows.Count > 0)
            {
                model.ApprovalList = new List<candidate_informationForApprovalViewModel>();
                model.CheckName = dt3.Rows[0]["CheckName"].ToString();
                model.CheckStatus = dt3.Rows[0]["CheckStatus"].ToString();
                if (dt3.Rows.Count > 0)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = cc.Totalwipcandidate_informationApprovaltemp(Convert.ToInt32(PersonalRowID));
                    if (dt1.Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        if (PersonalRowID != "")
                        {
                            dt = cc.GetTotalWipDetailTemp(Convert.ToInt32(PersonalRowID));
                        }
                        PdfPTable table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 20f;
                        table.SpacingAfter = 30f;

                        string CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy");
                        PdfPCell cell = new PdfPCell(new Phrase("Case Initiation Date:- " + CreatedDate + "", FontFactory.GetFont("Times New Roman ,serif", 12)));
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.Border = 0;
                        table.AddCell(cell);
                        pdfDoc.Add(table);

                        table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 10f;
                        table.SpacingAfter = 10f;
                        string CandTitle = dt.Rows[0]["CandTitle"].ToString();
                        string LOAName = dt.Rows[0]["LOA_Name"].ToString();
                        cell = new PdfPCell(new Phrase("Information provided by " + CandTitle + " " + LOAName + " for Background Verification", FontFactory.GetFont("Times New Roman ,serif", 14, Element.ALIGN_MIDDLE)));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 0;
                        table.AddCell(cell);
                        pdfDoc.Add(table);

                        table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 10f;
                        table.SpacingAfter = 10f;
                        string ClientName = dt.Rows[0]["ClientName"].ToString();
                        cell = new PdfPCell(new Phrase("Case Initiated by:-" + ClientName + "", FontFactory.GetFont("Times New Roman ,serif", 14, Element.ALIGN_LEFT)));
                        cell.Border = 0;
                        table.AddCell(cell);
                        pdfDoc.Add(table);



                        table = new PdfPTable(3);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 20f;
                        table.SpacingAfter = 30f;
                        cell = new PdfPCell(new Phrase("Personal Info", FontFactory.GetFont("Times New Roman ,serif", 16, BaseColor.WHITE)));
                        cell.Colspan = 3;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = new BaseColor(129, 211, 201);
                        table.AddCell(cell);
                        for (int k = 0; k < dt1.Rows.Count; k++)
                        {
                            string Label = dt1.Rows[k]["Label"].ToString();
                            string Information = dt1.Rows[k]["Information"].ToString();
                            table.AddCell(Label);
                            cell = new PdfPCell(new Phrase(Information));
                            cell.Colspan = 2;
                            table.AddCell(cell);
                        }
                        pdfDoc.Add(table);
                    }
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        int subChheckRowId = Convert.ToInt32(dt3.Rows[i]["SubCheckRowid"]);
                        string CheckName = model.CheckName = dt3.Rows[i]["CheckName"].ToString();

                        DataTable dt2 = new DataTable();
                        dt2 = cc.TotalWipcandidate_informationForApproval_Checktemp(Convert.ToInt32(PersonalRowID), subChheckRowId);
                        if (dt2.Rows.Count > 0)
                        {
                            PdfPTable table = new PdfPTable(5);
                            table.WidthPercentage = 100;
                            table.HorizontalAlignment = 0;
                            table.SpacingBefore = 20f;
                            table.SpacingAfter = 30f;
                            PdfPCell cell = new PdfPCell();
                            cell.Colspan = 5;
                            cell.Border = 0;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(CheckName, FontFactory.GetFont("Times New Roman ,serif", 16, BaseColor.WHITE)));
                            cell.Colspan = 5;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = new BaseColor(129, 211, 201);
                            table.AddCell(cell);
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                string Label = dt2.Rows[j]["Label"].ToString();
                                string Information = dt2.Rows[j]["Information"].ToString();
                                cell = new PdfPCell(new Phrase(Label));
                                cell.Colspan = 2;
                                table.AddCell(cell);
                                cell = new PdfPCell(new Phrase(Information));
                                cell.Colspan = 3;
                                table.AddCell(cell);
                            }
                            pdfDoc.Add(table);
                        }

                    }

                }
            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            output.Position = 0;
            return output;
        }

        public void DownloadLOA()
        {
            //return Redirect("~/Documents/CandidateDocuments/CandidateManual/CandidateAgreement.pdf");
            int ClientRowID = Convert.ToInt32(Session["ClientRowID"]);
            string FileName = "";
            if (ClientRowID == 635)
            {
                FileName = "CandidateAgreementTataElxsi.pdf";
            }
            else if (ClientRowID == 576)
            {
                FileName = "CandidateAgreementNagarrow.pdf";
            }
            else
            {
                FileName = "CandidateAgreement.pdf";
            }

            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateManual"].ToString();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=CandidateAgreement.pdf");
            Response.TransmitFile(Server.MapPath(FldPathDoc) + FileName);
            Response.End();
        }

        [ValidateInput(false)]
        public void UserManual()
        {
            string FileName = "CandidateManual.pdf";
            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateManual"].ToString();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=CandidateManual.pdf");
            Response.TransmitFile(Server.MapPath(FldPathDoc) + FileName);
            Response.End();
            //return Redirect("~/Documents/CandidateDocuments/CandidateManual/CandidateManual.pdf");
        }

        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult FAQ()
        {
            return View();
        }
        [AuthenticateUser]
        [ValidateSessionFilterCandidate]
        public ActionResult CandidateFOA()
        {
            DataTable dt = bl.GetAgreementdata(Convert.ToInt64(Session["TempPersonalRowID"]));
            List<string> docTable = new List<string>();

            //string[]  docTable=new string[7];

            foreach (DataRow d in dt.Rows)
            {
                docTable.Add(d["CheckFamilyRowID"].ToString());
            }
            ViewBag.CheckFamily = docTable;
            return View();
        }

        public ActionResult Signature()
        {
            return View();
        }


        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult Digital_LOA(string CanName = "")
        {
            long personalrowid = Convert.ToInt64(Session["TempPersonalRowID"]);
            DataSet d = tpc.GetTempCandidateChecksDT(personalrowid);
            ViewBag.CanName = d.Tables[0].Rows[0]["LOA_Name"].ToString();
            ViewBag.CaseStatus = d.Tables[0].Rows[0]["CaseinfoStatus"].ToString();
            ViewBag.ApplicationNo = d.Tables[0].Rows[0]["ClientRefID"].ToString();
            ViewBag.LoaImageB64 = d.Tables[0].Rows[0]["LOA_Signature"].ToString();
            ViewBag.PersonalRowId = clsCommonMethods.PEncrypt(personalrowid.ToString());
            if (Convert.ToString(d.Tables[0].Rows[0]["LOA_Signature"]) != "")
            {
                string CanLOASignature = d.Tables[0].Rows[0]["LOA_Signature"].ToString();
                ViewBag.LoaImage = LoadImage(CanLOASignature);
                ViewBag.LoaImageB64 = CanLOASignature;

            }
            else
            {
                ViewBag.CanLOASignature = "";
            }
            return View();
        }



        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [ValidateSessionFilterCandidate]
        public ActionResult Digital_LOA(FormCollection formCollection, HttpPostedFileBase file, HttpPostedFileBase Excelfile, string digitalcheck = "")
        {
            if (digitalcheck != "")
            {
                ViewBag.digitalcheck = "Checkedbox";
            }
            ClientCustomize cc = new ClientCustomize();
            string newimage = formCollection["hfSign"].ToString();
            string Name = formCollection["Name"].ToString();
            string CTStamp = formCollection["CTStamp"].ToString();
            string[] base64 = newimage.Split(',');

            if (Name == "")
            {
                ViewBag.ErrorMsg = "Please fill Name.";
                return View("Digital_LOA", new { CanName = Name });
            }
            if (newimage == "")
            {
                ViewBag.ErrorMsg = "Please fill signature.";
                @ViewBag.CanName = Name;
                return View("Digital_LOA", new { CanName = Name });
            }

            if (base64.Length == 1)
            {
                ViewBag.ErrorMsg = "Please fill signature.";
                return View("Digital_LOA", new { CanName = Name });
            }
            string Base64Image = "";
            if (base64.Length > 1)
            {
                Base64Image = base64[1].ToString();

            }
            //string hostName = System.Net.Dns.GetHostName();
            //string myIP = System.Net.Dns.GetHostByName(hostName).AddressList[0].ToString();

            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];

            string fileSavePath = "", filesavepathexcel = "", filenameexcel = "";
            string fileSavePath2 = "";
            long personalrowid = Convert.ToInt64(Session["TempPersonalRowID"]);
            if (file != null && file.ContentLength > 0 && (System.IO.Path.GetExtension(file.FileName).ToLower() == ".jpg" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".png" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".jpeg" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".pdf"))
            {

                string FldPathDoc = string.Empty;
                FldPathDoc = ConfigurationManager.AppSettings["provideddocumentsAPI".Trim().ToLower()].ToString();
                string FldPathDoc2 = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                string SecuId = Convert.ToString(Session["SecritadRefId"]);
                string ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                string filename = personalrowid + "_" + "LOA_Pic";
                fileSavePath = FldPathDoc + filename + ext;
                fileSavePath2 = FldPathDoc2 + filename + ext;
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                string base64S = Convert.ToBase64String(fileBytes);
                string res = FileSaveDownloadAPI.DocReportUploadDynamic(filename + ext, FldPathDoc, base64S);
                //file.SaveAs(fileSavePath);
            }

            if (Excelfile != null && Excelfile.ContentLength > 0)
            {


                string fldpathdoc1 = string.Empty;
                fldpathdoc1 = ConfigurationManager.AppSettings["provideddocumentsAPI".Trim().ToLower()].ToString();
                string SecuId = Convert.ToString(Session["secritadrefid"]);
                string extfile = Path.GetExtension(Excelfile.FileName);
                //DateTime dateTime = DateTime.UtcNow.Date;
                //string vv = DateTime.Now.ToString("M-d-yyyy");
                // filenameexcel = personalrowid + "_onboarding_" + vv + ext;
                filenameexcel = personalrowid + "_" + "LOA_Excel";
                filesavepathexcel = fldpathdoc1 + filenameexcel + extfile;
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    Excelfile.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                string base64Str = Convert.ToBase64String(fileBytes);
                string resp = FileSaveDownloadAPI.DocReportUploadDynamic(filenameexcel + filenameexcel, fldpathdoc1, base64Str);
                //Excelfile.SaveAs(filesavepathexcel);
            }

            int PersonalRowid = Convert.ToInt32(Session["TempPersonalRowID"]);
            cc.SaveDigitalSignature(personalrowid, Base64Image, Name, fileSavePath2, filesavepathexcel, filenameexcel, CTStamp, ipaddress);
            string CanLOASignature = "";
            Document pdfDoc = new Document(PageSize.A4, 40, 40, 80, 25);
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();

            //Table
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            //Cell          
            // Define your hexadecimal color code
            string hexColor = "#031F30"; // Example hex color
            // Convert hex to RGB
            int r = Convert.ToInt32(hexColor.Substring(1, 2), 16);
            int g = Convert.ToInt32(hexColor.Substring(3, 2), 16);
            int b = Convert.ToInt32(hexColor.Substring(5, 2), 16);
            // Create a BaseColor instance

            BaseColor customColor = new BaseColor(r, g, b);
            PdfPCell cell = new PdfPCell();
            if (Session["ClientRowID"].ToString() == "684")
            {
                cell = new PdfPCell(new Phrase("NFORMATION RELEASE AUTHORIZATION", FontFactory.GetFont("Securitas Pro", 14, iTextSharp.text.Font.BOLD, customColor)));
            }
            else if (Session["ClientRowID"].ToString() == "957")
            {
                Paragraph headPara = new Paragraph();
                headPara.Add(new Chunk("DECLARATION & AUTHORIZATION\n", FontFactory.GetFont("Securitas Pro", 14, iTextSharp.text.Font.BOLD, customColor)));
                headPara.Add(new Chunk("For Aadhaar-Based Identity Verification & Background Screening", FontFactory.GetFont("Securitas Pro", 9, iTextSharp.text.Font.NORMAL, customColor)));
                headPara.Alignment = Element.ALIGN_CENTER;
                cell = new PdfPCell(headPara);
            }
            else
            {
                cell = new PdfPCell(new Phrase("DECLARATION & AUTHORIZATION", FontFactory.GetFont("Securitas Pro", 14, iTextSharp.text.Font.BOLD, customColor)));
            }
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(172, 194, 221);
            table.AddCell(cell);
            if (Session["ClientRowID"].ToString() == "957")
            {
                string ApplicationNo = formCollection["ApplicationNo"] != null ? formCollection["ApplicationNo"].ToString() : "";

                cell = new PdfPCell(new Phrase("To Whom It May Concern,\n\nI, " + Name + ", hereby declare that all information provided by me in connection with my application and background verification is true, accurate, and complete to the best of my knowledge. I understand that any false, misleading, or incomplete information may lead to termination of employment or withdrawal of offer at any stage.\n\nAuthorization for Verification\nI authorize the Company, its subsidiaries/affiliates, and its authorized Background Verification (BGV) partner/vendor to:\n\n- Collect, verify, validate, and process my personal information, including Aadhaar details, for identity verification and background screening.\n- Access and authenticate my Aadhaar through UIDAI-approved methods strictly for verification purposes.\n- Obtain records, reports, or confirmations from previous employers, educational institutions, or other relevant organizations.\n- Share and receive necessary information with authorized parties involved in the verification process.\n\nConsent & Data Usage\nI confirm that:\n\n- My Aadhaar details are being provided voluntarily and will be used only for identity authentication and verification.\n- My personal data will be handled with appropriate security safeguards, including restricted access, secure storage, and compliance with applicable laws and UIDAI guidelines.\n- My information will be retained only for the minimum duration necessary and securely deleted thereafter unless required by law.\n- I have the right to request correction of my personal information.\n\nDeclarations\nI further acknowledge that:\n\n- No fee, gift, or payment is required to be made to any individual during the verification process.\n- During address verification, I will not share photographs or documents with any verifier via unofficial channels (e.g., WhatsApp, email, etc.).\n- Withdrawal of consent may impact the verification process and my employment eligibility.\n\nRelease of Liability\nI hereby release from liability all individuals, organizations, and entities involved in collecting, verifying, or sharing information for the purpose of my background verification.\n\nFinal Consent\nBy signing below, I confirm that I have read, understood, and agreed to the above terms and voluntarily provide my consent.\n\nCandidate ID / Application No.: " + ApplicationNo + "\n", FontFactory.GetFont("Securitas Pro", 8, customColor)));
            }
            else if (Session["ClientRowID"].ToString() == "1428")
            {
                cell = new PdfPCell(new Phrase("Collect and process my personal information as required to process background verification, including collection and processing of sensitive personal information.\n\nShare my personal information with, and obtain information from, the following third parties as necessary to carry out the requested services: police, courts, suppliers of court and police information for criminal record checks, employers, employment agencies, educational institutions, credit and consumer reporting agencies, government agencies and other public bodies, regulatory or professional bodies, references you provide, other organizations with which I have been associated, and other publicly available print or online sources. I understand that these third parties may be located in countries or regions other than my country of residence, including places where I have previously lived, studied or worked.\n\nWhere necessary, act as my agent to share or obtain your personal information when carrying out the services.\n\nThe data may cross geographical boundaries for the purpose of background verification.For more information, please refer to our privacy policy.\n\n", FontFactory.GetFont("Securitas Pro", 10, customColor)));
            }
            else if (Session["ClientRowID"].ToString() == "1481")
            {
                cell = new PdfPCell(new Phrase("I certify that the statements made in this application are valid and complete to the best of my knowledge. I understand that false or misleading information may result in termination of the agreement.\n\nIf upon investigations, any of this information is found to be incomplete or inaccurate, I understand that agreement will be subject to dismissal at any time during my employment.\n\nI hereby authorize CtrlS Datacenters Limited and/or any of its subsidiaries or affiliates and any persons or organizations acting on its behalf, to verify the information presented on this application form and to procure an investigative report or consumer report for that purpose.\n\nI hereby grant authority for the bearer of this letter to access or be provided with full details of my previous records. In addition, please provide any other pertinent information requested by the individual presenting this authority.\n\nI hereby release from liability all persons or entities requesting or supplying such information.\n\nI am aware that no Money / fee / postage / gift to be paid or given in goodwill to any one during the process of verification and I have informed my family members as well regarding this.\n\nI am aware that during the Address verification, no photographs or documents to be shared with verifier through any medium (Whats App, Email, Etc.)\n\nI have read, understand, and by my signature consent to these statements.\n\nPlease Sign In given below box. And click On Save Button to give your Consent.\n", FontFactory.GetFont("Securitas Pro", 10, customColor)));
            }
            else if (Session["ClientRowID"].ToString() == "576")
            {
                cell = new PdfPCell(new Phrase("I certify that the statements made in this application are valid and complete to the best of my knowledge. I understand that false or misleading information may result in termination of the agreement.\n\nIf upon investigations, any of this information is found to be incomplete or inaccurate, I understand that agreement will be subject to dismissal at any time during my employment.\n\nI hereby authorize the Company and / or any of its subsidiaries or affiliates and any persons or organizations acting on its behalf, to verify the information presented on this application form and to procure an investigative report or consumer report for that purpose. \n\nI hereby grant authority for the bearer of this letter to access or be provided with full details of my previous records.In addition, please provide any other pertinent information requested by the individual presenting this authority. \n\nI hereby release from liability all persons or entities requesting or supplying such information.\n\nI am aware that no Money / fee / postage / gift to be paid or given in goodwill to any one during the process of verification and I have informed my family members as well regarding this. \n\nI am aware that during the Address verification, no photographs or documents to be shared with verifier through any medium (Whats App, Email, Etc.)\n\nNagarro acts as the data fiduciary and engages appointed vendors to conduct background verification checks. By signing this authorization letter, you agree and acknowledge that your personal data will be collected, processed, and stored in compliance with applicable data protection laws. Detailed information about how your personal data is handled can be found at: Applicant Privacy Notice-EN.pdf\n\nI have read, understand, and by my signature consent to these statements.\n\nPlease Sign In given below box. And click On Save Button to give your Consent.\n", FontFactory.GetFont("Securitas Pro", 10, customColor)));
            }
            else if (Session["ClientRowID"].ToString() == "684")
            {
                cell = new PdfPCell(new Phrase("To secure employment with ALLEGIS SERVICES (INDIA) PRIVATE LIMITED (hereinafter referred to as “Company”), I certify that the statements made in this application are valid and complete to the best of my knowledge.I understand that by sharing any false or misleading information, I may be disqualified from my employment, or it may result in termination of my employment with the Company, whereby the Company may also recover any remuneration paid to me.\n\nIf upon investigations, any of this information is found to be incomplete or inaccurate, I understand that agreement will be subject to dismissal at any time during my employment.\n\nI hereby authorize the Company and / or any of its subsidiaries or affiliates and any persons or organizations acting on its behalf, to verify the information presented on this application form and to procure an investigative report or consumer report for that purpose. \n\nI hereby grant authority for the bearer of this letter to access or be provided with full details of my previous records.In addition, please provide any other pertinent information requested by the individual presenting this authority. \n\nI hereby release from liability all persons or entities requesting or supplying such information.\n\nI am aware that no Money / fee / postage / gift to be paid or given in goodwill to any one during the process of verification and I have informed my family members as well regarding this. \n\nI am aware that during the Address verification, no photographs or documents to be shared with verifier through any medium (Whats App, Email, Etc.)\n\nNagarro acts as the data fiduciary and engages appointed vendors to conduct background verification checks. By signing this authorization letter, you agree and acknowledge that your personal data will be collected, processed, and stored in compliance with applicable data protection laws. Detailed information about how your personal data is handled can be found at: Applicant Privacy Notice-EN.pdf\n\nI have read, understand, and by my signature consent to these statements.\n\nPlease Sign In given below box. And click On Save Button to give your Consent.\n", FontFactory.GetFont("Securitas Pro", 10, customColor)));
            }
            else
            {
                cell = new PdfPCell(new Phrase("I certify that the statements made in this application are valid and complete to the best of my knowledge. I understand that false or misleading information may result in termination of the agreement.\n\nIf upon investigations, any of this information is found to be incomplete or inaccurate, I understand that agreement will be subject to dismissal at any time during my employment.\n\nI hereby authorize the Company and / or any of its subsidiaries or affiliates and any persons or organizations acting on its behalf, to verify the information presented on this application form and to procure an investigative report or consumer report for that purpose. \n\nI hereby grant authority for the bearer of this letter to access or be provided with full details of my previous records.In addition, please provide any other pertinent information requested by the individual presenting this authority. \n\nI hereby release from liability all persons or entities requesting or supplying such information.\n\nI am aware that no Money / fee / postage / gift to be paid or given in goodwill to any one during the process of verification and I have informed my family members as well regarding this. \n\nI am aware that during the Address verification, no photographs or documents to be shared with verifier through any medium (Whats App, Email, Etc.)\n\nI have read, understand, and by my signature consent to these statements.\n\nPlease Sign In given below box. And click On Save Button to give your Consent.\n", FontFactory.GetFont("Securitas Pro", 10, customColor)));
            }
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            if (Session["ClientRowID"].ToString() == "957")
                cell.Padding = 15f;
            else
                cell.Padding = 80f;
            cell.Colspan = 4;
            table.AddCell(cell);

            cell = new PdfPCell();
            DataTable dt = new DataTable();
            dt = cc.GetExportLoa(PersonalRowid);
            if (dt.Rows.Count > 0)
            {
                ViewBag.LOA_Signature = dt.Rows[0]["LOA_Signature"].ToString();
                if (Convert.ToString(dt.Rows[0]["LOA_Signature"]) != "")
                {
                    CanLOASignature = dt.Rows[0]["LOA_Signature"].ToString();
                    ViewBag.LoaImage = LoadImage(CanLOASignature);
                    ViewBag.LoaImageB64 = CanLOASignature;
                }
                else
                {
                    ViewBag.CanLOASignature = "";
                }
            }
            LoadImages(CanLOASignature, PersonalRowid);
            string path = ConfigurationManager.AppSettings["DocumentsAPI"].ToString();
            string Strbase64 = FileSaveDownloadAPI.DocAndReportDownloadDynamic(PersonalRowid + ".png", path);
            byte[] imgBytes = Convert.FromBase64String(Strbase64);
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgBytes); img.ScaleAbsolute(200, 100);
            img.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            cell.AddElement(img);
            cell.Colspan = 4;
            table.AddCell(cell);

            cell = new PdfPCell();
            table.AddCell("Name:-");
            table.AddCell(ViewBag.LOA_Name = dt.Rows[0]["LOA_Name"].ToString());
            table.AddCell("Date:-");
            table.AddCell(ViewBag.CreatedDate = dt.Rows[0]["CreatedDate"].ToString());
            table.AddCell(cell);

            pdfDoc.Add(table);


            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            byte[] bytes = memoryStream.ToArray();
            string ProcFileName = "Letter_Of_Authorization" + PersonalRowid + ".pdf";
            string ProcfilePath = ConfigurationManager.AppSettings["provideddocumentsAPI".Trim().ToLower()].ToString();
            string ProcfilePath2 = ConfigurationManager.AppSettings["provideddocuments".Trim().ToLower()].ToString();
            string base64String = Convert.ToBase64String(bytes);
            string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(ProcFileName, ProcfilePath, base64String);
            //string ProcfilePath = "~/Documents/CandidateDocuments/ProvidedDocuments/";
            //string filePath = Server.MapPath("~/Documents/CandidateDocuments/ProvidedDocuments/Letter_Of_Authorization" + PersonalRowid + ".pdf");
            //System.IO.File.WriteAllBytes(filePath, bytes);
            memoryStream.Close();
            cc.UpdateExportLoa(Convert.ToInt32(PersonalRowid), ProcfilePath2 + "Letter_Of_Authorization" + PersonalRowid + ".pdf");
            cc.proc_InsertUpdateDigitalLOAInTempPQVerifiedUploadCaseDocs(Convert.ToInt32(personalrowid), ProcFileName, ProcfilePath2, "Provided", "Letter Of Authorization");
            ViewBag.LoaImageB64 = Base64Image;
            @ViewBag.CanName = Name;
            CreateMenuitemforDataEntryOperator(personalrowid);
            //return View("Digital_LOA", new { CanName = Name });
            return RedirectToAction("Digital_LOA", "Home", new { CanName = Name });
        }

        public Image LoadImage(string Base64Image, int personalrowid = 0)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            byte[] bytes = Convert.FromBase64String(Base64Image);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }

        public void LoadImages(string Base64Image, int personalrowid = 0)
        {
            byte[] imageBytes = Convert.FromBase64String(Base64Image);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            string path = ConfigurationManager.AppSettings["DocumentsAPI"].ToString();
            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(personalrowid + ".png", path, Base64Image);
            //image.Save(Server.MapPath("~/Documents/" + personalrowid + ".png"), System.Drawing.Imaging.ImageFormat.Png);

        }
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult CandidateMedia()
        {
            return View();
        }

        [NonAction]
        void BindCandTitleList()
        {
            try
            {
                IEnumerable<SelectListItem> Department = new List<SelectListItem> {
                new SelectListItem { Text = "Mr.", Value = "Mr." },
                new SelectListItem { Text = "Ms.", Value = "Ms." },
                new SelectListItem { Text = "Mrs.", Value = "Mrs." },
                new SelectListItem { Text = "M/S", Value = "M/S" },
            };
                ViewBag.DepartmentList = Department;
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpGet]
        public JsonResult getlatlong(string add = "")
        {
            string apikey = System.Configuration.ConfigurationManager.AppSettings["GoogleMAPAPIKey"].ToString();
            var locationService = new GoogleMaps.LocationServices.GoogleLocationService(apikey);
            var point = locationService.GetLatLongFromAddress(add);
            return Json(new { pass = point }, JsonRequestBehavior.AllowGet);
        }

        #region Add Chacks Actions
        #region ****** Update Personal Details ******
        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult UpdateCandidatePersonalDetails(string ClientRowId = "", string PersonalRowID = "")
        {

            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            int clientRowId = Convert.ToInt32(Session["ClientRowID"].ToString());
            int PersonalRowId = Convert.ToInt32(Session["TempPersonalRowID"].ToString());

            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, 0, "Candidate", "Case");
            if (clientRowId == 1255)// Infinx services pvt ltd  client UAN column remove for fresher package
            {
                DataTable dtpac = new DataTable();
                dtpac = bl.PackageNameClientWise(PersonalRowId);
                if (dtpac.Rows.Count > 0)
                {
                    string packname = dtpac.Rows[0]["PackageOrCheckDetails"].ToString();
                    if (packname.Contains("INFINX_Freshers (CRCP)"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["AntecedentsRowID"].ToString() == "125")
                                dr.Delete();
                        }
                        dt.AcceptChanges();
                    }
                }
            }

            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, 0);


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
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }

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

            ViewBag.PersonalRowid = PersonalRowId.ToString();
            ViewBag.clientRowId = clientRowId.ToString();
            return View(marksModel);
        }

        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UpdateCandidatePersonalDetails(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());
            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowid;

                cd.clientRowId = clientRowid;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, 0, "Candidate", "Case");
                ViewBag.ErrorMsg = "";

                DataTable dbValue = new DataTable();
                dbValue = bl.Antecedents_client_select_DB_Candidate(cd.PersonalRowID, 0);

                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                                if (clsCommonMethods.ValidateSQLInjuctionInput(cd.DataEntryValue) == false)
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "Please Provide valid Input in " + cd.DisplayName + " field";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }

                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }

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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = PersonalRowid + "_" + filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
                                }
                                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                                if (rowsFiltered.Count() == 0 && postedFile.ContentLength == 0)
                                {
                                    marksModel.Add(d[1].ToString(), d[0].ToString());
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "<br>" + FString[1] + " is a mandatory field";
                                }
                            }
                        }
                    }

                    else if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.SaveDataEntry_Candidate(cd);
                            }
                        }
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }

                    else
                    {
                        DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                }
                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, 0, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, 0, "Pending", "Info");
                }
                CreateMenuitemforCandidate(cd.PersonalRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return View(marksModel);
            }
            else
                return View(marksModel);
        }
        #endregion



        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult Personal_Info()
        {
            var model = new PQPersonalInfoViewModel();
            long id = Convert.ToInt32(Session["TempPersonalRowID"]);
            int pid = Convert.ToInt32(Session["TempPersonalRowID"]);
            string inEncript = clsCommonMethods.PEncrypt(id.ToString());
            model = repoPQPersonal.GetTempPersonalInfoByPersonalRowID(pid);
            model.ClientRowID = Convert.ToInt32(Session["ClientRowID"].ToString());
            model.PersonalRowID = pid;
            model.LOA_TimeStamp = DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss tt");
            model.EncPersonalID = inEncript;
            if (!string.IsNullOrEmpty(model.hfSign))
            {
                ViewBag.LoaImage = LoadImage(model.hfSign);
                ViewBag.LoaImageB64 = model.hfSign;

            }
            else
            {
                ViewBag.CanLOASignature = "";
                ViewBag.LoaImageB64 = "";
            }
            ////BindCandTitleList();
            ViewBag.Message = 0;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Personal_Info(PQPersonalInfoViewModel objs, HttpPostedFileBase file)
        {
            ClientCustomize cc = new ClientCustomize();
            string newimage = objs.hfSign;
            string Name = objs.LOA_Name;
            string CTStamp = objs.LOA_TimeStamp;
            string[] base64 = newimage.Split(',');
            string Base64Image = "";
            if (base64.Length > 1)
            {
                Base64Image = base64[1].ToString();
            }

            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            string fileSavePath = "";
            string fileSavePath2 = "";
            long personalrowid = Convert.ToInt64(Session["TempPersonalRowID"]);
            if (file != null && file.ContentLength > 0 && System.IO.Path.GetExtension(file.FileName).ToLower() == ".jpg")
            {

                string FldPathDoc = string.Empty;
                FldPathDoc = ConfigurationManager.AppSettings["provideddocumentsAPI".Trim().ToLower()].ToString();
                string FldPathDoc2 = ConfigurationManager.AppSettings["provideddocuments".Trim().ToLower()].ToString();

                string SecuId = Convert.ToString(Session["SecritadRefId"]);
                string ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                string filename = personalrowid + "_" + "LOA_Pic";
                fileSavePath = FldPathDoc + filename + ext;
                fileSavePath2 = FldPathDoc2 + filename + ext;
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                string base64String = Convert.ToBase64String(fileBytes);
                string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename + ext, FldPathDoc, base64String);
                //file.SaveAs(fileSavePath);
            }

            int PersonalRowid = Convert.ToInt32(Session["TempPersonalRowID"]);
            cc.SaveDigitalSignature_PersonalNLoa(personalrowid, Base64Image, Name, fileSavePath2, CTStamp, ipaddress);
            repoPQPersonal.UpdateTempPQPersonalInfo(objs);
            //// BindCandTitleList();
            var model = repoPQPersonal.GetTempPersonalInfoByPersonalRowID(PersonalRowid);
            if (!string.IsNullOrEmpty(model.hfSign))
            {
                ViewBag.LoaImage = LoadImage(model.hfSign);
                ViewBag.LoaImageB64 = model.hfSign;
                ViewBag.Message = 1;
            }
            else
            {
                ViewBag.CanLOASignature = "";
                ViewBag.LoaImageB64 = "";
                ViewBag.Message = 0;
            }
            return View(model);
        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddAddress_Info(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "")
        {

            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            int SubCheckId = 0;
            int clientRowId = 0;
            int PersonalRowId = 0;

            if (IsNumeric(subCheckId))
            {
                SubCheckId = Convert.ToInt32(subCheckId);
            }
            else
            {
                SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            }

            if (IsNumeric(ClientRowId))
            {
                clientRowId = Convert.ToInt32(ClientRowId);
            }
            else
            {
                clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            }

            if (IsNumeric(PersonalRowID))
            {
                PersonalRowId = Convert.ToInt32(PersonalRowID);
            }
            else
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
            }


            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);

            //if(clientRowId==1188 && SubCheckId== 13)
            //{
            DataTable dtcheckstatus = bl.getTempPQCandidateChecksDetail(PersonalRowId, SubCheckId);
            string status = "1";
            if (dtcheckstatus.Rows.Count > 0)
            {
                status = dtcheckstatus.Rows[0]["Status"].ToString();
                if (status == "1")
                {
                    ViewBag.NACheckStatus = false;
                }
                else
                {
                    ViewBag.NACheckStatus = true;
                }
            }
            //}


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
                        ClientCustomize cc = new ClientCustomize();
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                        if (d[1].ToString().Contains("34"))
                        {
                            DataTable dtcountry = cc.GetCountryName(rowsFiltered[0][1].ToString());
                            if (dtcountry.Rows.Count > 0)
                            {
                                ViewBag.countryid = dtcountry.Rows[0]["CountryRowID"].ToString();
                            }
                            else
                            {
                                ViewBag.countryid = "";
                            }
                            //ViewBag.countryid = rowsFiltered[0][1].ToString();

                        }
                        else if (d[1].ToString().Contains("44"))
                        {
                            DataTable dtstate = cc.GetStateName(rowsFiltered[0][1].ToString());
                            if (dtstate.Rows.Count > 0)
                            {
                                ViewBag.stateId = dtstate.Rows[0]["StateRowID"].ToString();
                            }
                            else
                            {
                                ViewBag.stateId = "";
                            }
                            //ViewBag.stateId = rowsFiltered[0][1].ToString();
                        }
                        else if (d[1].ToString().Contains("32"))
                        {
                            DataTable dtCity = cc.GetCityName(rowsFiltered[0][1].ToString());
                            if (dtCity.Rows.Count > 0)
                            {
                                ViewBag.Cityid = dtCity.Rows[0]["DistrictRowID"].ToString();
                            }
                            else
                            {
                                ViewBag.Cityid = "";
                            }
                            //ViewBag.Cityid = rowsFiltered[0][1].ToString();
                        }





                        if (d[1].ToString().Contains("34"))
                        {
                            ViewBag.CountryName = rowsFiltered[0][1].ToString();
                        }
                        else if (d[1].ToString().Contains("44"))
                        {
                            ViewBag.StateName = rowsFiltered[0][1].ToString();
                        }
                        else if (d[1].ToString().Contains("32"))
                        {
                            ViewBag.CityName = rowsFiltered[0][1].ToString();
                        }

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

                        if (d[1].ToString().Contains("34"))
                        {
                            ViewBag.CountryName = "";
                            ViewBag.countryid = "";
                        }
                        else if (d[1].ToString().Contains("44"))
                        {
                            ViewBag.StateName = "";
                            ViewBag.stateId = "";
                        }
                        else if (d[1].ToString().Contains("32"))
                        {
                            ViewBag.CityName = "";
                            ViewBag.Cityid = "";
                        }



                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }


            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            ViewBag.clientno = clientRowId;
            CreateMenuitemforCandidate(PersonalRowId);
            //GetEmploymentList();
            return View(marksModel);
        }
        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddAddress_Info(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = 0;
            int SubCheckId = 0;
            int clientRowId = 0;
            string SubCheckid = formCollection["SubCheckId"].ToString();
            string clientRowid = formCollection["clientRowId"].ToString();
            if (IsNumeric(SubCheckid))
            {
                SubCheckId = Convert.ToInt32(SubCheckid);
            }
            else
            {
                SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));
            }

            if (IsNumeric(clientRowid))
            {
                clientRowId = Convert.ToInt32(clientRowid);
            }
            else
            {
                clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));
            }

            if (IsNumeric(PersonalRowid))
            {
                PersonalRowId = Convert.ToInt32(PersonalRowid);
            }
            else
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));
            }

            bool NA = Convert.ToBoolean(formCollection["Notraised"].Split(',')[0]);
            if (NA == true)
            {
                bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Completed", "Completed");
                ViewBag.Success = "Information Updated Successfully";
                ViewBag.NACheckStatus = true;
                return RedirectToAction("AddAddressInfo", new { subCheckId = SubCheckId, ClientRowId = clientRowId, PersonalRowID = PersonalRowId });
            }
            else
            {
                bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Pending", "Pending");
                ViewBag.NACheckStatus = false;
            }
            string Countryid = formCollection["34"].ToString().Replace(",,", "");
            string Stateid = formCollection["44"].ToString().Replace(",,", "");
            string Cityid = formCollection["32"].ToString().Replace(",,", "");

            string CountryName = "";
            string StateName = "";
            string CityName = "";
            DataTable DtCountry = new DataTable();
            if (Countryid != "")
            {
                DtCountry = bl.GetCountryName(Convert.ToInt32(Countryid));
            }
            DataTable DtState = new DataTable();
            if (Stateid != "")
            {
                DtState = bl.GetStateName(Convert.ToInt32(Stateid));
            }
            DataTable DtCity = new DataTable();
            if (Cityid != "")
            {
                DtCity = bl.GetCityName(Convert.ToInt32(Cityid));
            }

            if (DtCountry.Rows.Count > 0)
            {
                CountryName = DtCountry.Rows[0]["CountryName"].ToString();
            }

            if (DtState.Rows.Count > 0)
            {
                StateName = DtState.Rows[0]["StateName"].ToString();
            }

            if (DtCity.Rows.Count > 0)
            {
                CityName = DtCity.Rows[0]["DistrictName"].ToString();
            }




            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            ViewBag.ErrorMsg = "";
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }

                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }

                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }

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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                                if (key == "504")
                                {
                                    string let = "";
                                    string lng = "";
                                    string letlong = "";
                                    var letlng = formCollection["504"].ToString().Split(',');
                                    if (letlng.Length == 2)
                                    {
                                        let = letlng[0];
                                        lng = letlng[1];
                                        if (letlng[0].Substring(0, 1) == "(")
                                        {
                                            let = letlng[0].Substring(1, letlng[0].Length - 1);
                                        }
                                        if (letlng[1].Substring(letlng[1].Length - 1, 1) == ")")
                                        {
                                            lng = letlng[1].Substring(0, letlng[1].Length - 1);
                                        }
                                        letlong = let + "," + lng;
                                    }

                                    cd.DataEntryValue = letlong;
                                }
                                else
                                {
                                    cd.DataEntryValue = formCollection[key].ToString();
                                }

                                cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                bl.SaveDataEntry_Candidate(cd);
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
                                ClientCustomize cc = new ClientCustomize();


                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                //cd.DataEntryValue = formCollection[key].ToString();
                                if (cd.AntecedentRowID == 34)
                                {
                                    cd.DataEntryValue = CountryName;
                                    ViewBag.CountryName = CountryName;
                                }
                                else if (cd.AntecedentRowID == 44)
                                {
                                    cd.DataEntryValue = StateName;
                                    ViewBag.StateName = StateName;
                                }
                                else if (cd.AntecedentRowID == 32)
                                {
                                    cd.DataEntryValue = CityName;
                                    ViewBag.CityName = CityName;
                                }
                                else
                                {
                                    cd.DataEntryValue = formCollection[key].ToString();
                                }
                                bl.SaveDataEntry_Candidate(cd);
                                if (d[1].ToString().Contains("34"))
                                {
                                    DataTable dtcountry = cc.GetCountryName(CountryName);
                                    if (dtcountry.Rows.Count > 0)
                                    {
                                        ViewBag.countryid = dtcountry.Rows[0]["CountryRowID"].ToString();
                                    }
                                    else
                                    {
                                        ViewBag.countryid = "";
                                    }
                                    //ViewBag.countryid = rowsFiltered[0][1].ToString();

                                }
                                else if (d[1].ToString().Contains("44"))
                                {
                                    DataTable dtstate = cc.GetStateName(StateName);
                                    if (dtstate.Rows.Count > 0)
                                    {
                                        ViewBag.stateId = dtstate.Rows[0]["StateRowID"].ToString();
                                    }
                                    else
                                    {
                                        ViewBag.stateId = "";
                                    }
                                    //ViewBag.stateId = rowsFiltered[0][1].ToString();
                                }
                                else if (d[1].ToString().Contains("32"))
                                {
                                    DataTable dtCity = cc.GetCityName(CityName);
                                    if (dtCity.Rows.Count > 0)
                                    {
                                        ViewBag.Cityid = dtCity.Rows[0]["DistrictRowID"].ToString();
                                    }
                                    else
                                    {
                                        ViewBag.Cityid = "";
                                    }
                                    //ViewBag.Cityid = rowsFiltered[0][1].ToString();
                                }
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

                        bl.SaveDataEntry_Candidate(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }
                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                CreateMenuitemforCandidate(cd.PersonalRowID);

            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckid = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            ViewBag.clientno = clientRowId;
            //ViewBag.Success = "Information Updated Successfully";           

            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return View(marksModel);
                //return RedirectToAction("AddEmploymentInfo", new { subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowid });
            }
            else
                return View(marksModel);
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddEmployment_Info(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            int SubCheckId = 0;
            int clientRowId = 0;
            int PersonalRowId = 0;

            if (IsNumeric(subCheckId))
            {
                SubCheckId = Convert.ToInt32(subCheckId);
            }
            else
            {
                SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            }

            if (IsNumeric(ClientRowId))
            {
                clientRowId = Convert.ToInt32(ClientRowId);
            }
            else
            {
                clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            }

            if (IsNumeric(PersonalRowID))
            {
                PersonalRowId = Convert.ToInt32(PersonalRowID);
            }
            else
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
            }


            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            ViewBag.OrgId = bl.Antecedents_ResearchID(PersonalRowId, SubCheckId, "Candidate");

            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
            DataTable dtcheckstatus = bl.getTempPQCandidateChecksDetail(PersonalRowId, SubCheckId);
            string status = "1";
            if (dtcheckstatus.Rows.Count > 0)
            {
                status = dtcheckstatus.Rows[0]["Status"].ToString();
                if (status == "1")
                {
                    ViewBag.NACheckStatus = false;
                }
                else
                {
                    ViewBag.NACheckStatus = true;
                }
            }
            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);

            foreach (DataRow d in dt.Rows)
            {

                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                        if (d[0].ToString() == "92")
                        {
                            ViewBag.CompanyName = rowsFiltered[0][1].ToString();
                        }

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

            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            ViewBag.clientno = clientRowId;
            if (ViewBag.CompanyName == null)
            {
                ViewBag.CompanyName = "";
            }
            CreateMenuitemforCandidate(PersonalRowId);
            return View(marksModel);
        }

        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddEmployment_Info(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = 0;
            int SubCheckId = 0;
            int clientRowId = 0;
            // int PersonalRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            // int SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(SubCheckid));

            string clientRowid = formCollection["clientRowId"].ToString();
            // int clientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(clientRowid));

            if (IsNumeric(SubCheckid))
            {
                SubCheckId = Convert.ToInt32(SubCheckid);
            }
            else
            {
                SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));
            }

            if (IsNumeric(clientRowid))
            {
                clientRowId = Convert.ToInt32(clientRowid);
            }
            else
            {
                clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));
            }

            if (IsNumeric(PersonalRowid))
            {
                PersonalRowId = Convert.ToInt32(PersonalRowid);
            }
            else
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));
            }

            //if (clientRowId==902)
            //{
            //string NA= formCollection["Notraised"];
            bool NA = Convert.ToBoolean(formCollection["Notraised"].Split(',')[0]);
            if (NA == true)
            {
                bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Completed", "Completed");
                ViewBag.Success = "Information Updated Successfully";
                ViewBag.NACheckStatus = true;
                return RedirectToAction("AddEmploymentInfo", new { subCheckId = SubCheckId, ClientRowId = clientRowId, PersonalRowID = PersonalRowId });
            }
            else
            {
                bl.UpdateNACheckDocumentStatus(PersonalRowId, SubCheckId, "Pending", "Pending");
                ViewBag.NACheckStatus = false;
            }
            //}
            //else
            //{
            //    ViewBag.NACheckStatus = false;
            //}
            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            Int32 OrganizationId = 0;
            try
            {
                OrganizationId = Convert.ToInt32(formCollection["OrganizationId"].ToString());
            }
            catch { };
            saveEmpidInTempCandidateTable(PersonalRowId, SubCheckId, OrganizationId);

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());


                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[0] == "93") //other remarks 
                                {
                                    if (OrganizationId == 0 && cd.DataEntryValue == "")
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + " Organization name or Other Employer must be filled";
                                }
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }
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
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                        if (FString[0] == "92")//OrganizationName
                        {
                            cd.DataEntryValue = formCollection["OrganizationName"].ToString();
                            cd.DisplayName = FString[1];
                            marksModel.Add(d[1].ToString(), cd.DataEntryValue);
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            bl.SaveDataEntry_Candidate(cd);
                        }
                        else if (FString[0] == "82") //Address of the company
                        {
                            cd.DataEntryValue = formCollection["Addressofthecompany"].ToString();
                            cd.DisplayName = FString[1];
                            marksModel.Add(d[1].ToString(), cd.DataEntryValue);
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            bl.SaveDataEntry_Candidate(cd);
                        }
                        else
                        {
                            foreach (string key in formCollection.AllKeys)
                            {
                                if (key == d[0].ToString())
                                {
                                    marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                    cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = formCollection[key].ToString();
                                    cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                    bl.SaveDataEntry_Candidate(cd);
                                }
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
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }
                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                CreateMenuitemforCandidate(cd.PersonalRowID);

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
            ViewBag.clientno = clientRowId;
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return View(marksModel);
                //return RedirectToAction("AddEmploymentInfo", new { subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowid });
            }
            else
                return View(marksModel);
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddEducation_Info(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            ViewBag.EduID = bl.Antecedents_ResearchID(PersonalRowId, SubCheckId, "Candidate");

            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);

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
                        if (d[1].ToString().Contains("72"))
                        {
                            ViewBag.degreeName = rowsFiltered[0][1].ToString().Trim();
                        }
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
                        if (d[1].ToString().Contains("72"))
                        {
                            ViewBag.degreeName = "";

                        }
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }

            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            CreateMenuitemforCandidate(PersonalRowId);
            return View(marksModel);
        }

        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddEducation_Info(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));
            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            string clientRowid = formCollection["clientRowId"].ToString();
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));

            string Degreeid = formCollection["72"].ToString();

            string DegreeName = "";
            DataTable DtDegree = new DataTable();
            if (Degreeid != "")
            {
                DtDegree = bl.GetDegreeName(Convert.ToInt32(Degreeid));
            }
            //DataTable DtDegree = bl.GetDegreeName(Convert.ToInt32(Degreeid));
            if (DtDegree.Rows.Count > 0)
            {
                DegreeName = DtDegree.Rows[0]["DegreeType"].ToString();
            }
            Int32 CollegeId = 0;
            try
            {
                CollegeId = Convert.ToInt32(formCollection["CollegeId"].ToString().Replace(",", ""));
            }
            catch { }
            saveEmpidInTempCandidateTable(PersonalRowId, SubCheckId, CollegeId);

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[0] == "77") //other remarks 
                                {
                                    if (CollegeId == 0 && cd.DataEntryValue == "")
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + " College/Institute name or Other Institute must be filled";
                                }
                                else
if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                        }
                    }
                }

                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }

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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                        if (FString[0] == "69")       //"College/Institute/School Name"
                        {
                            cd.DataEntryValue = formCollection["CollegeName"].ToString();
                            cd.DisplayName = FString[1];
                            marksModel.Add(d[1].ToString(), cd.DataEntryValue);
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            bl.SaveDataEntry_Candidate(cd);
                        }
                        else if (FString[0] == "70")  //"School Address"
                        {
                            cd.DataEntryValue = formCollection["CollegeAddress"].ToString();
                            cd.DisplayName = FString[1];
                            marksModel.Add(d[1].ToString(), cd.DataEntryValue);
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            bl.SaveDataEntry_Candidate(cd);
                        }
                        else
                        {
                            foreach (string key in formCollection.AllKeys)
                            {
                                if (key == d[0].ToString())
                                {
                                    marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                    cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = formCollection[key].ToString();
                                    cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                    bl.SaveDataEntry_Candidate(cd);
                                }
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
                                //Hfdegreeid




                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                if (cd.AntecedentRowID == 72)
                                {
                                    cd.DataEntryValue = DegreeName;

                                    ViewBag.degreeName = DegreeName;

                                }
                                else
                                {
                                    cd.DataEntryValue = formCollection[key].ToString();
                                }
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }
                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                CreateMenuitemforCandidate(cd.PersonalRowID);
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
            ViewBag.degreeid = formCollection["Hfdegreeid"].ToString();
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return View(marksModel);
                //return RedirectToAction("AddEmploymentInfo", new { subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowid });
            }
            else
                return View(marksModel);
        }

        #region******* Upload Criminal Information *******

        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult AddCriminal_Info(string ClientRowId = "", string PersonalRowID = "", string subCheckId = "", string ErrorMsg = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
            if (dt.Rows.Count > 0)
            {
                DataTable dbValue = new DataTable();
                dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);

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
            }
            CreateMenuitemforCandidate(PersonalRowId);



            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;

            return View(marksModel);
        }

        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddCriminal_Info(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));
            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            string clientRowid = formCollection["clientRowId"].ToString();
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }
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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                                cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                bl.SaveDataEntry_Candidate(cd);
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
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }
                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                CreateMenuitemforCandidate(cd.PersonalRowID);

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
            //ViewBag.Success = "Information Updated Successfully";
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return View(marksModel);
                //return RedirectToAction("AddEmploymentInfo", new { subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowid });
            }
            else
                return View(marksModel);
        }
        #endregion

        #region******* Upload National Identity Information *******

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddNationalIdentity_Info(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);
            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);


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
                    //marksModel.Add("Name_none", studentName);
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

            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            CreateMenuitemforCandidate(PersonalRowId);
            return View(marksModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddNationalIdentity_Info(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            string clientRowid = formCollection["clientRowId"].ToString();
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                ViewBag.ErrorMsg = "";

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }
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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                                cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                bl.SaveDataEntry_Candidate(cd);
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
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                CreateMenuitemforCandidate(cd.PersonalRowID);
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
            //ViewBag.Success = "Information Updated Successfully";
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return View(marksModel);
            }
            else
                return View(marksModel);
        }

        #endregion

        #region******* Upload Reference Information *******


        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult AddReference_Info(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);

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
                    //marksModel.Add("Name_none", studentName);                    
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

            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            CreateMenuitemforCandidate(PersonalRowId);

            return View(marksModel);
        }


        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddReference_Info(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = 0;
            if (PersonalRowid != "")
            {
                PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));
            }
            //int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            string clientRowid = formCollection["clientRowId"].ToString();
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }

                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }

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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                                cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                bl.SaveDataEntry_Candidate(cd);
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
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                CreateMenuitemforCandidate(cd.PersonalRowID);
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
            //    ViewBag.Success = "Information Updated Successfully";
            //    return View(marksModel);
            //}
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return View(marksModel);
            }
            else
                return View(marksModel);
        }
        #endregion

        #region ******* Add Special Check Information By Candidate *******

        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        public ActionResult AddSpecialCheck_Info(string subCheckId = "", string ClientRowId = "", string PersonalRowID = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(subCheckId));
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            DataTable dt = new DataTable();
            dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");

            DataTable dbValue = new DataTable();
            dbValue = bl.Antecedents_client_select_DB_Candidate(PersonalRowId, SubCheckId);

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
                    //marksModel.Add("Name_none", studentName);
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

            ViewBag.PersonalRowid = PersonalRowID;
            ViewBag.SubCheckId = subCheckId;
            ViewBag.clientRowId = ClientRowId;
            CreateMenuitemforCandidate(PersonalRowId);
            return View(marksModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddSpecialCheck_Info(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            string PersonalRowid = formCollection["PersonalRowid"].ToString();
            int PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowid));

            string SubCheckid = formCollection["SubCheckId"].ToString();
            int SubCheckId = Convert.ToInt32(clsCommonMethods.PDecrypt(SubCheckid));

            string clientRowid = formCollection["clientRowId"].ToString();
            int clientRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(clientRowid));

            ViewBag.Title = bl.getCheckNameNCandidateNameForCheckTitle(PersonalRowId, SubCheckId);

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowId;
                cd.SubCheckRowID = SubCheckId;
                cd.clientRowId = clientRowId;
                cd.CreatedBy = int.Parse(Session["CandidateUserRowID"].ToString());

                ViewBag.ErrorMsg = "";


                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowId, SubCheckId, "Candidate", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');

                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + cd.DisplayName + " is a mandatory field.,";
                                }
                            }
                        }
                    }
                }
                if (ViewBag.ErrorMsg != "")
                {
                    string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                    ErrorMsg = ErrorMsg.Substring(0, ErrorMsg.Length - 1);
                    ViewBag.ErrorMsg = ErrorMsg;
                }
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
                                //string path = Server.MapPath("~/Uploads/");
                                string path = ConfigurationManager.AppSettings["UploadsAPI"].ToString();
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    byte[] fileBytes;
                                    using (var ms = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(ms);
                                        fileBytes = ms.ToArray();
                                    }
                                    string base64String = Convert.ToBase64String(fileBytes);
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(filename, path, base64String);
                                    if (res2.Contains("Success"))
                                    {
                                        //postedFile.SaveAs(path + Path.GetFileName(filename));
                                        cd.AntecedentRowID = Int64.Parse(filenm);
                                        cd.DisplayName = FString[1];
                                        cd.DataEntryValue = filename;
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
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
                                cd.DataEntryValue = clsCommonMethods.ReplaceSQLInjuctionInputs(cd.DataEntryValue);
                                bl.SaveDataEntry_Candidate(cd);
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
                                bl.SaveDataEntry_Candidate(cd);
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

                        bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                if (ViewBag.ErrorMsg == "")
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Completed", "Info");
                }
                else
                {
                    bl.updateCheckStatus_Candidate_temp(cd.PersonalRowID, cd.SubCheckRowID, "Pending", "Info");
                }
                CreateMenuitemforCandidate(cd.PersonalRowID);
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
            //ViewBag.Success = "Information Updated Successfully";
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                return View(marksModel);
                //return RedirectToAction("AddEmploymentInfo", new { subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, PersonalRowID = ViewBag.PersonalRowid });
            }
            else
                return View(marksModel);
        }
        #endregion
        private void CreateMenuitemforCandidate(Int64 id = 0)
        {
            try
            {
                string inEncript = clsCommonMethods.PEncrypt(id.ToString());
                if (id > 0)
                {

                    DataSet d = new DataSet();
                    if (Session["ClientRowID"].ToString() == "679")
                    {
                        string refno = "";
                        DataTable dt = tpc.GetNewCandidateDetails(id);
                        if (dt.Rows.Count > 0)
                        {
                            refno = dt.Rows[0]["CompanyRefNo"].ToString();
                        }
                        d = tpc.getTempPQCandidateChecksDT_Fractal(id);

                    }

                    string NavInputDataMenuList = string.Empty;
                    NavInputDataMenuList = "<li class='bold' style='background-color:#3175AF;padding:5px;'>Guidelines for uploading the documents are below </li>";
                    NavInputDataMenuList += "<li style='background-color:#dedddd;padding:1.5%;font-weight: bold' class='list-group-item'><a href='" + Url.Action("ReIndex", "Home") + "'><span class='glyphicon glyphicon-user'></span> Instructions </a></li>";

                    NavInputDataMenuList += "<li ><a href='" + Url.Action("UserManual", "Home") + "' target='_blank' style='padding:5px;background-color:#dedddd;'><span class='glyphicon glyphicon-download-alt '></span> User Manual </a></li>";


                    if (Convert.ToString(d.Tables[0].Rows[0]["CaseinfoStatus"]) == "Completed")
                        NavInputDataMenuList += "<li  style='background-color:#36d6d4;padding:1.5%' class='list-group-item'><a href='" + Url.Action("UpdateCandidateDetails", "Home", new { id = inEncript }) + "'><span class='glyphicon glyphicon-user'></span>Personal Info </a></li>";
                    else
                        NavInputDataMenuList += "<li><a href='" + Url.Action("UpdateCandidatePersonalDetails", "Home", new { id = inEncript }) + "'><span class='glyphicon glyphicon-user'></span>  Personal Info </a></li>";

                    int AarryCount = d.Tables[1].Rows.Count;
                    string[] nxtprv = new string[AarryCount];
                    if (d.Tables[1].Rows.Count > 0)
                    {
                        for (int J = 0; J < d.Tables[1].Rows.Count; J++)
                        {
                            string NextSubCheckRowID = "End";
                            string PreviousSubCheckRowID = "Start";
                            string NextCheckFamilyRowID = "";
                            string PreviousFamilyRowID = "";
                            string enPersonalRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["PersonalRowID"].ToString());
                            string enClientRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["ClientRowID"].ToString());
                            string enCheckFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString());
                            string enSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString());
                            string urlValuePrevious = "Start";
                            string urlValueNext = "End";

                            if (J < d.Tables[1].Rows.Count - 1)
                            {
                                NextSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J + 1]["SubCheckFamilyID"].ToString());
                                NextCheckFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J + 1]["CheckFamilyRowID"].ToString());
                                urlValueNext = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId_AddChack(Convert.ToInt16(d.Tables[1].Rows[J + 1]["CheckFamilyRowID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, NextCheckFamilyRowID, NextSubCheckRowID));
                            }
                            if (J > 0)
                            {
                                PreviousSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J - 1]["SubCheckFamilyID"].ToString());
                                PreviousFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J - 1]["CheckFamilyRowID"].ToString());
                                urlValuePrevious = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId_AddChack(Convert.ToInt16(d.Tables[1].Rows[J - 1]["CheckFamilyRowID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, PreviousFamilyRowID, PreviousSubCheckRowID));
                            }

                            string urlValue = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId_AddChack(Convert.ToInt16(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, enCheckFamilyRowID, enSubCheckRowID));
                            nxtprv[J] = urlValueNext + "|" + urlValuePrevious + "|" + PreviousSubCheckRowID + "|" + NextSubCheckRowID + "|" + enSubCheckRowID;

                            if (d.Tables[1].Rows[J]["CheckInfoStatus"].ToString() == "Completed" && d.Tables[1].Rows[J]["CheckDocStatus"].ToString() == "Completed")
                                NavInputDataMenuList += "<li style='background-color:#36d6d4;padding:1.5%' class='list-group-item'><a href='" + urlValue + "'><span class='glyphicon glyphicon-unchecked'></span> " + d.Tables[1].Rows[J]["SubCheckName"] + " </a></li>";
                            else
                                NavInputDataMenuList += "<li><a href='" + urlValue + "'><span class='glyphicon glyphicon-unchecked'></span> " + d.Tables[1].Rows[J]["SubCheckName"].ToString() + " </a></li>";

                        }
                    }

                    NavInputDataMenuList += "<li><a href='" + Url.Action("FinalSubmit_Addcheck", "Home", new { id = inEncript }) + "'><b id='btnFinalSub'; class='btn btn-primary btn-full'>Final Submission</b><i class='fa fa-check-circle text-white' aria-hidden='true'></i></a></li>";

                    Session["NavCandidateInputDataMenuList"] = NavInputDataMenuList;
                    Session["NextPreviousButtonsURL"] = nxtprv;
                }
            }
            catch (Exception ee)
            {

            }
        }
        #endregion

        public ActionResult FinalSubmit_Addcheck(string id = "")
        {
            try
            {
                long ids = Convert.ToInt64(clsCommonMethods.PDecrypt(id));
                DataTable dt = new DataTable();
                DataTable Ndt = tpc.GetNewCandidateDetails(ids);
                if (Session["ClientRowID"].ToString() == "679")
                {
                    string refno = "";

                    if (Ndt.Rows.Count > 0)
                    {
                        refno = Ndt.Rows[0]["CompanyRefNo"].ToString();
                    }
                    if (!string.IsNullOrEmpty(refno))
                    {
                        dt = tpc.GetCandidateChecksCompletenessStatus_Fractal(ids);
                    }

                }

                string MissingInformation = "";
                string MissingDocs = "";

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["InoStatus"].ToString() == "Pending")
                    {
                        MissingInformation = " " + dr["InformationType"].ToString();
                        break;
                    }
                    if (dr["DocStatus"].ToString() == "Pending")
                    {
                        MissingDocs = dr["InformationType"].ToString();
                        break;
                    }
                }


                if (MissingDocs != "")
                {
                    TempData["AlertMsg"] = "<div class='alert alert-danger' style='padding:5px'><strong>Attention ! </strong>Request you to upload <b> " + MissingDocs + " </b> document to move further.</div> ";
                }
                else if (MissingInformation != "")
                {
                    TempData["AlertMsg"] = "<div class='alert alert-danger' style='padding:5px'><strong>Attention ! </strong>Request you to complete <b> " + MissingInformation + " </b> check to move further.</div> ";
                }
                else
                    TempData["AlertMsg"] = "<div class='alert alert-success' style='padding:5px'><strong>Form Uploaded Successfully. ! </strong>Thanks for providing the complete details</div> ";

                Int16 ClientRowID = Convert.ToInt16(Session["ClientRowID"]);

                if (MissingInformation == "" && MissingDocs == "")
                {
                    int createdBy = int.Parse(Session["CandidateUserRowID"].ToString());

                    string result = "";
                    if (Session["ClientRowID"].ToString() == "679")
                    {
                        string refno = "";
                        if (Ndt.Rows.Count > 0)
                        {
                            refno = Ndt.Rows[0]["CompanyRefNo"].ToString();
                        }
                        if (!string.IsNullOrEmpty(refno))
                        {
                            result = tpc.ApproveOrFinalSubmitByCandidate_Addcheck(ids, createdBy);
                        }

                    }

                    if (result == "Success")
                    {
                        SendMail(ids, ClientRowID);
                        try
                        {
                            MessageStatus objMessage = new MessageStatus();
                            WhatsAppAPI objs = new WhatsAppAPI();
                            objMessage = objs.SendWhatsAppMessage("+91" + Ndt.Rows[0]["MobileNo"].ToString(), "submission_from_candidate_", Ndt.Rows[0]["CandidateFullName"].ToString(), "media_template", "", "");
                            ClientCustomize cc = new ClientCustomize();
                            cc.sp_AddAuditWhatsAppAPI(Convert.ToInt32(ids), objMessage.message, objMessage.status, "Submission From Candidate");
                        }
                        catch
                        {
                        }
                    }
                    if (result == "Success")
                    {
                        return RedirectToAction("CandidateLogOut", "Account", new { area = "" });
                    }

                }

            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return RedirectToAction("ReIndex");


        }

        public ActionResult Preview()
        {
            long personalrowid = Convert.ToInt64(Session["TempPersonalRowID"]);
            ClientCustomize cc = new ClientCustomize();
            DataSet ds = new DataSet();

            ds = cc.PreviewData(personalrowid);

            List<TempCandidateDetails> tempdata = new List<TempCandidateDetails>();
            tempdata = (from DataRow dr in ds.Tables[0].Rows
                        select new TempCandidateDetails()
                        {
                            PersonalRowID = Convert.ToString(dr["PersonalRowID"]),
                            DisplayName = Convert.ToString(dr["DisplayName"]),
                            DataEntryValue = Convert.ToString(dr["DataEntryValue"]),
                            SubCheckRowID = Convert.ToString(dr["SubCheckRowID"]),
                            SubCheckName = Convert.ToString(dr["SubCheckName"]),
                        }).ToList();

            return View(tempdata);
        }
        [HttpGet]
        public ActionResult AddFaceMatchInfo(string Pid)
        {
            CandidateFaceMatchViewModel model = new CandidateFaceMatchViewModel();
            try
            {
                model.PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(Pid));
                model.SubCheckRowId = 712;

            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        //[HttpPost]
        //public ActionResult AddFaceMatchInfo(FormCollection Frm)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception Ex)
        //    {
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //    return View();
        //}
        [HttpPost]
        public ActionResult SaveSelfie(string base64Image, int Pid, int Sid)
        {
            CandidateFaceMatchViewModel model = new CandidateFaceMatchViewModel();
            try
            {
                if (base64Image != null && base64Image.Length > 0)
                {
                    string filePath = Server.MapPath(ConfigurationManager.AppSettings["provideddocuments".Trim()].ToString());

                    if (string.IsNullOrEmpty(base64Image))
                        return Json(new { success = false, message = "No image received" });

                    // ✅ Remove prefix
                    base64Image = base64Image.Replace("data:image/jpeg;base64,", "")
                                             .Replace("data:image/png;base64,", "");

                    // ✅ Convert base64 → bytes
                    byte[] imageBytes = Convert.FromBase64String(base64Image);

                    // ✅ Create unique JPG filename
                    string fileName = "selfie_" + DateTime.Now.Ticks + ".jpg";
                    string selfie_Path = Path.Combine(filePath, fileName);

                    // ✅ Convert byte[] → Image safely
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    using (Image img = Image.FromStream(ms))
                    {
                        // ✅ Create new bitmap to avoid GDI+ errors
                        using (Bitmap bmp = new Bitmap(img.Width, img.Height))
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.Clear(Color.White); // JPG background
                            g.DrawImage(img, 0, 0, img.Width, img.Height);

                            // ✅ Save safely (fixes GDI+ error)
                            bmp.Save(selfie_Path, ImageFormat.Jpeg);
                        }
                    }

                    ClientCustomize CC = new ClientCustomize();
                    string ImagePic = "";
                    var tblVerifiedDocs = CC.GetPQVerifiedUploadDocs(Pid, Sid);
                    if (tblVerifiedDocs.Rows.Count > 0)
                    {
                        ImagePic = Convert.ToString(tblVerifiedDocs.Rows[0]["FileName"]);
                    }
                    InstantCheckRepository InstantcheckObj = new InstantCheckRepository();

                    string ImagePath = filePath + ImagePic;

                    var FatchMatchModel = InstantcheckObj.GetFatchMatch(selfie_Path, ImagePath, "60", "", Pid.ToString());
                    SavePQVerifiedUploadDocSelfie(Pid, Convert.ToInt16(Sid), "Provided", "CandidateSelfie", "provideddocuments", "Candidate Selfie", fileName);

                    string FaceMatchPass = "Fail";
                    if (FatchMatchModel.statuscode == 200)
                    {
                        model.Face_Match = FatchMatchModel.data.match;
                        if (FatchMatchModel.data.status.ToUpper() == "SUCCESS")
                        {
                            FaceMatchPass = "Match";
                        }
                        int clientRowid = Convert.ToInt32(Session["ClientRowID"]);
                        string matchdata = FatchMatchModel.data.match.Replace("%", "").Trim();
                        var FaceMatchId = CC.InsertIntoFaceMatch(FatchMatchModel, Pid, ImagePic, fileName, "Candidate Photo", "Candidate Selfie", Convert.ToInt32(Pid));

                        var dt = CC.Antecedents_client_select_form(clientRowid, Sid, "Operation", "Check");
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                int rst = CC.InsertUpdateFaceMatchCheckDetails(Pid, Sid, Antecedents, clientRowid, FaceMatchPass);
                            }
                        }
                        var FaceMatchDt = CC.GetFaceMatch(Convert.ToInt32(FaceMatchId));
                        string companyRefNo = FaceMatchDt.Rows[0]["companyRefNo"].ToString();
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/FaceMatch.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{CompanyRefNo}", companyRefNo);
                        body = body.Replace("{Match}", FatchMatchModel.data.match);
                        body = body.Replace("{message}", FatchMatchModel.data.status);
                        body = body.Replace("{Image1Name}", ImagePic);
                        body = body.Replace("{Image2Name}", fileName);
                        body = body.Replace("{Image1Remark}", "Candidate Photo");
                        body = body.Replace("{Image2Remark}", "Candidate Selfie");
                        body = body.Replace("{verifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));

                        SavePQVerifiedUploadUANDoc(Pid, Convert.ToInt16(Sid), "Verified", "Face Match Report", "provideddocuments", body, "Face Match Verified", 0, companyRefNo);
                        CC.UpdateTmMemberID(Pid, Sid);
                        if (Convert.ToDouble(matchdata) > 60)
                        {
                            var rst = CC.UpdateCandidateChecksReportComments(Pid, Convert.ToInt16(Sid));
                            CC.UpdateCheckStatus(Pid, Sid, 1, 36, "Check", Convert.ToInt32(245));
                            CC.UpdatePQCandidateChecksColorCode(905, 6, "Green", Pid, Sid, "Verified");
                        }
                        else
                        {
                            CC.UpdateCheckStatus(Pid, Sid, 1, 36, "Check", Convert.ToInt32(245));
                            CC.UpdatePQCandidateChecksColorCode(905, 6, "Red", Pid, Sid, "Face not Match");

                        }
                        clsCommonMethods.AddCheckActionHistoryByCandidate(Pid, Convert.ToInt16(Sid), "Clear Candidate", "Verification recieve throught integration", clientRowid);

                    }
                    model.FaceMatchStatus = FaceMatchPass;

                    return Json(new { success = model });
                }
            }
            catch (Exception Ex)
            {
            }
            return Json(new { success = false });
        }

        public void SavePQVerifiedUploadDocSelfie(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", string remarks = "", string fileName = "")
        {
            try
            {
                PQAddVerifiedUploadDocViewModel DocModel = new PQAddVerifiedUploadDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.SubCheckRowID = SubCheckRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //  Provided / Verified
                DocModel.DocumentType = DocumentType; //Name of the selected docs
                string FldPathDoc = string.Empty;
                FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();
                string proImg = string.Empty;
                proImg = fileName;//(SubCheckRowID.ToString() + DocumentType + Session["CadidateUserName"].ToString()).Replace(" ", "_").Replace("-", "_") + DateTime.Now.ToString("ssfff") + ext;

                //UploadCandidateDoc(proImg, UploadDoc, "document");
                //UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));

                DocModel.FileName = Path.GetFileName(proImg);
                DocModel.FilePath = FldPathDoc;
                DocModel.UploadDate = DateTime.Now;
                DocModel.StatusDocxConvert = 0;
                DocModel.StatusPDF2DOCXConvert = 0;
                DocModel.Remarks = remarks;
                DocModel.PDF_FileName = "";
                DocModel.Status = 1;
                var rst = repoPQVerifiedUploadDoc.AddPQVerifiedUploadDocNew(DocModel);
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        public void SavePQVerifiedUploadUANDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", string UploadDoc = "", string remarks = "", byte NADoc = 0, string secRefId = "")
        {
            try
            {
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                string FileName = (secRefId + "_" + SubCheckRowID.ToString() + "_" + DocumentType + "_" + Timestamp.ToString()).Replace(" ", "_").Replace("-", "_");
                if (FileName.Contains("24/7CPL_"))
                {
                    FileName = FileName.Replace("/", "-");
                }
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
                    proImg = FileName + ".pdf";

                    string existFile = repoPQVerifiedUploadDoc.ExistFileByPersonalIdSubCheckIdDocTypeNew(PersonalRowID, SubCheckRowID, DocumentType, proImg);
                    if (!string.IsNullOrEmpty(existFile))
                    {
                        string path = Server.MapPath(FldPathDoc + existFile);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)//check file exsit or not
                        {
                            file.Delete();
                        }
                    }
                    string IronKey = System.Configuration.ConfigurationManager.AppSettings["IronKey"].ToString();
                    var Renderer = new IronPdf.ChromePdfRenderer();
                    IronPdf.License.LicenseKey = IronKey;
                    var pdf = Renderer.RenderHtmlAsPdf(UploadDoc);

                    pdf.SaveAs(Server.MapPath(FldPathDoc + proImg)); // Saves our PdfDocument object as a PDF

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
                var rst = repoPQVerifiedUploadDoc.AddPQVerifiedUploadDocNew(DocModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetCaptchaValue()
        {
            Random generator = new Random();
            String captchaVal = generator.Next(1000, 9999).ToString("D4");
            return captchaVal;
        }
        public ActionResult DigilockerlinkGeneration(int Pid, int Sid)
        {
            string link = "";
            try
            {
                PerfiosAPI aPI = new PerfiosAPI();
                var APIPath = Server.MapPath("~\\APILogs\\Logfiles");
                //string Redirect_uri = "http://localhost:54586/Account/DigiRedirect_uri";
                string Redirect_uri = "https://bgv.securitas-india.com/Account/DigiRedirect_uri";
                var model = aPI.GetPerfiosDigilockerAuthenticate(Pid.ToString(), Redirect_uri, APIPath);
                if(model!= null)
                {
                    if (model.statusCode == 101)
                    {
                        link=model.result.link;
                    }
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            if (link != "")
            {
                return Redirect(link);
            }
            else
            {
                return RedirectToAction("AddlivenessInfo", "Home", new { area = "Candidate", Pid = clsCommonMethods.PEncrypt(Pid.ToString()), Sid = clsCommonMethods.PEncrypt(Sid.ToString()), msg = "link not generate" });
            }
        }
        [HttpGet]
        public ActionResult AddlivenessInfo(string Pid = "", string Sid = "", string msg = "")
        {
            CandidateFaceMatchViewModel model = new CandidateFaceMatchViewModel();
            DataBaseConnections DC = new DataBaseConnections();
            try
            {
                model.PersonalRowId = Convert.ToInt32(clsCommonMethods.PDecrypt(Pid));
                int ClientRowID = 0;
                ClientCustomize Dc = new ClientCustomize();
                var Dt = Dc.GetPersonalInfoByTempPersonalRowID(Convert.ToInt32(model.PersonalRowId));
                if (Dt.Rows.Count > 0)
                {
                    model.MobileNo = Convert.ToString(Dt.Rows[0]["MobileNo"]);
                    model.CandidateName = Convert.ToString(Dt.Rows[0]["CandidateFullName"]);
                    ClientRowID = Convert.ToInt32(Dt.Rows[0]["ClientRowID"]);
                    Session["CompanyRefNo"] = Convert.ToString(Dt.Rows[0]["CompanyRefNo"]);
                }
                model.SubCheckRowId = 890;
                model.RendomOTP = GetCaptchaValue();
                CreateMenuitemforDataEntryOperator(model.PersonalRowId);
                model.LivenessWithAdharStatus = DC.GetClientMaster(ClientRowID);
                if (msg != "")
                {
                    ViewBag.SuccessMsg = msg;
                }

            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        public bool GetPerfiosAadhaar(int PersonalRowId, int SubCheckRowId, string CandidateName, string aadhaarNo)
        {
            bool resurlt = false;
            Aadhaar_verificationRoot Obj = new Aadhaar_verificationRoot();
            try
            {
                var ErrorPath = Server.MapPath("~\\ErrorLogs\\Logfiles");
                var APIPath = Server.MapPath("~\\APILogs\\Logfiles");
                ClientCustomize CC = new ClientCustomize();
                PerfiosAPI perfiosAPI = new PerfiosAPI();
                Obj = perfiosAPI.GetPerfiosAadhaar(CandidateName, PersonalRowId.ToString(), aadhaarNo, ErrorPath, APIPath);

                int teamMemerRowId = Convert.ToInt16(Session["CandidateUserRowID"]);
                if (Obj.statuscode == "101")
                {
                    int AutoId = CC.InsertUpdatePerfiosAadhaar_verification(Obj, PersonalRowId, aadhaarNo, teamMemerRowId, SubCheckRowId);

                    if (AutoId > 0)
                    {
                        var tbl = CC.GetPerfiosAadhaar_verification(PersonalRowId, AutoId);
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/PerfiosAadhaarCard.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        if (tbl.Rows.Count > 0)
                        {
                            if (tbl.Rows.Count > 0)
                            {
                                body = body.Replace("{name}", CandidateName);
                                body = body.Replace("{aadhaar_number}", aadhaarNo);
                                body = body.Replace("{age_range}", Convert.ToString(tbl.Rows[0]["ageBand"]));
                                body = body.Replace("{state}", Convert.ToString(tbl.Rows[0]["state"]));
                                body = body.Replace("{gender}", Convert.ToString(tbl.Rows[0]["gender"]));
                                string isIssued = "N";
                                if (Convert.ToInt32(tbl.Rows[0]["isIssued"]) == 1)
                                {
                                    isIssued = "Y";
                                }
                                body = body.Replace("{last_digits}", Convert.ToString(tbl.Rows[0]["mobile"]));
                                body = body.Replace("{is_mobile}", isIssued);
                                body = body.Replace("{verifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));
                                SaveTempPQVerifiedUploadDoc(PersonalRowId, Convert.ToInt16(SubCheckRowId), "Verified", "Aadhaar Verified Report", "provideddocuments", body, "Face Match Verified");
                                resurlt = true;
                            }
                        }
                    }
                }
                else
                {
                    resurlt = false;
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

            }
            return resurlt;
        }
        public bool GetPaysPrintAadhaar(int PersonalRowid, int SubCheckid, string IDNumber, string CandidateName)
        {
            bool status = false;
            AddUANCheckDetailsByAPI AddApiObj = new AddUANCheckDetailsByAPI();
            ClientCustomize cc = new ClientCustomize();

            try
            {
                string AutoId = AddApiObj.AddAadhaarNumberDetails(PersonalRowid, SubCheckid, IDNumber, CandidateName, Convert.ToInt32(Session["CandidateUserRowID"]));

                if (AutoId != "")
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/TempAadhaarCard.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    DataTable tbl = new DataTable();

                    tbl = cc.PaySprintVerifyAadhaarNumber(Convert.ToInt32(AutoId), IDNumber, PersonalRowid, SubCheckid);
                    if (tbl.Rows.Count > 0)
                    {
                        body = body.Replace("{name}", CandidateName);
                        body = body.Replace("{aadhaar_number}", Convert.ToString(tbl.Rows[0]["aadhaar_number"]));
                        body = body.Replace("{age_range}", Convert.ToString(tbl.Rows[0]["age_range"]));
                        body = body.Replace("{state}", Convert.ToString(tbl.Rows[0]["state"]));
                        body = body.Replace("{gender}", Convert.ToString(tbl.Rows[0]["gender"]));
                        body = body.Replace("{last_digits}", Convert.ToString(tbl.Rows[0]["last_digits"]));
                        body = body.Replace("{is_mobile}", Convert.ToString(tbl.Rows[0]["is_mobile"]));
                        body = body.Replace("{remarks}", Convert.ToString(tbl.Rows[0]["remarks"]));
                        body = body.Replace("{less_info}", Convert.ToString(tbl.Rows[0]["less_info"]));
                        body = body.Replace("{verifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));
                        SaveTempPQVerifiedUploadDoc(PersonalRowid, Convert.ToInt16(SubCheckid), "Verified", "Aadhaar Verified artifact", "provideddocuments", body, "Aadhaar Verified form API");
                        status = true;
                    }

                }
                return status;
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

                return status;
            }

        }


        [HttpPost]
        public ActionResult SaveVideo(HttpPostedFileBase videoFile, HttpPostedFileBase IdImage, string Pid, string name, string otp)
        {
            try
            {
                DataBaseConnections Dc = new DataBaseConnections();
                ClientCustomize bl = new ClientCustomize(); bool AadhaarStatus = false;
                if (videoFile == null || videoFile.ContentLength == 0)
                    return Json(new { success = false, message = "No video received" });

                if (IdImage == null || IdImage.ContentLength == 0)
                    return Json(new { success = false, message = "No ID Document received" });

                var VideoFileName = SaveMP4Video(videoFile, Pid, name);
                string uploadFolder = Server.MapPath("~/Documents/CandidateVideos");

                string imageFolder = Server.MapPath(ConfigurationManager.AppSettings["provideddocuments"]);
                string ext = Path.GetExtension(IdImage.FileName);

                string ImageName = $"890_{Pid}_IdDocuments_{Guid.NewGuid()}{ext}";
                BAL.OCR.AadhaarOCRDetails Aadhaardata = new BAL.OCR.AadhaarOCRDetails();
                int LivenessWithAdharStatus = Dc.GetClientMaster(Convert.ToInt32(Session["ClientRowID"]));
                if (LivenessWithAdharStatus == 1)
                {
                    var tempDT = Dc.GetTempPQVerifiedUploadDocs(Convert.ToInt32(Pid), 890, "Candidate Image", 1, 15, "Desc");
                    if (tempDT.Rows.Count > 0)
                    {
                        ImageName = Convert.ToString(tempDT.Rows[0]["FileName"]);
                    }
                    else
                    {
                        ImageName = "";
                    }
                }
                else
                {
                    if (Convert.ToString(Session["ClientRowID"]) == "267")
                    {
                        if (ext != ".pdf")
                        {
                            return Json(new { success = false, message = "Please upload your Aadhaar ID documents in PDF format." });
                        }
                        SaveTempPQVerifiedUploadDoc(Convert.ToInt32(Pid), 890, "Provided", "Id Documents", "provideddocuments", IdImage, "ID Documents", ImageName);
                        BAL.OCR.PDFOCR OcrObj = new BAL.OCR.PDFOCR();
                        Aadhaardata = OcrObj.AddhaarOcr(IdImage);
                        if (Convert.ToString(Aadhaardata.AadhaarNo) is null)
                        {
                            return Json(new { success = false, message = "Aadhaar not verified, Please upload your Aadhaar ID documents in PDF format." });
                        }
                        else
                        {
                            AadhaarStatus = GetPaysPrintAadhaar(Convert.ToInt32(Pid), 890, Aadhaardata.AadhaarNo, Aadhaardata.Name);
                            if (!AadhaarStatus)
                            {
                                return Json(new { success = false, message = "Aadhaar not verified, Please upload your Aadhaar ID documents in PDF format." });
                            }
                        }
                    }
                    else
                    {
                        SaveTempPQVerifiedUploadDoc(Convert.ToInt32(Pid), 890, "Provided", "Id Image", "provideddocuments", IdImage, "Liveness image", ImageName);
                    }
                }

                string videoPath = Path.Combine(uploadFolder, VideoFileName);
                var videoStatus = ProcessVideoLiveness(videoPath, otp?.Replace(" ", ""), Path.Combine(imageFolder, ImageName), Pid);

                string statusValue = videoStatus["status"]?.ToString();
                if (string.IsNullOrEmpty(statusValue))
                    return Json(new { success = false, message = "Invalid response from liveness API" });

                JObject statusobj = JObject.Parse(statusValue);
                string submitResponse = videoStatus["submit"]?.ToString();
                string apiExecutionStatus = (string)statusobj["result"]?["apiExecutionStatus"];



                if (apiExecutionStatus == "CALLBACK_SUCCESS")
                {
                    int responseStatusCode = (int)statusobj["result"]["apiResponse"]["statusCode"];
                    if (responseStatusCode == 101)
                    {
                        Dc.InsertUpdateLivenessApiResponse(Convert.ToInt32(Pid), 890, statusValue, VideoFileName, submitResponse, "Complete");
                        bl.updateCheckStatus_Candidate_temp(Convert.ToInt32(Pid), 890, "Completed", "Info");
                        bl.updateCheckStatus_Candidate_temp(Convert.ToInt32(Pid), 890, "Completed", "Docs");
                        if (Convert.ToString(Session["ClientRowID"]) == "1606")
                        {
                            bl.updateCheckStatus_Candidate_temp(Convert.ToInt32(Pid), 0, "Completed", "Info");
                        }

                        var TempDt = Dc.GetTempPqPersonalData(Convert.ToInt32(Pid));
                        if (TempDt.Rows.Count > 0)
                        {
                            string PersonalRowId = Convert.ToString(TempDt.Rows[0]["PersonalRowID"]);
                            string CompanyRefNo = Convert.ToString(TempDt.Rows[0]["CompanyRefNo"]);
                            if (PersonalRowId != "")
                            {
                                UpdateLivenessCheckStatus(Convert.ToInt32(PersonalRowId), CompanyRefNo, submitResponse);
                            }
                        }

                        return Json(new { success = true, message = "Video received with Liveness Success" });
                    }
                    else
                    {
                        string errormsg = (string)statusobj["result"]["apiResponse"]["error"];
                        string errorDescription = (string)statusobj["result"]["apiResponse"]["errorDescription"];
                        string ErrorMsg = $"{errormsg} {errorDescription}. Please read the instructions and re-upload the video.";
                        Dc.InsertUpdateLivenessApiResponse(Convert.ToInt32(Pid), 890, statusValue, VideoFileName, submitResponse, "Error Found");
                        return Json(new { success = false, message = ErrorMsg });
                    }
                }
                else
                {
                    Dc.InsertUpdateLivenessApiResponse(Convert.ToInt32(Pid), 890, statusValue, VideoFileName, submitResponse, "Error Found");
                    string errormsg = (string)statusobj["result"]["apiResponse"]["error"];
                    string errorDescription = (string)statusobj["result"]["apiResponse"]["errorDescription"];
                    string ErrorMsg = $"{errormsg} {errorDescription}. Please read the instructions and re-upload the video.";
                    return Json(new { success = false, message = ErrorMsg });
                }
            }
            catch (Exception ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~/ErrorLogs/Logfiles"), ex.ToString(), ex.StackTrace);
                return Json(new { success = false, message = "An error occurred while processing the video." });
            }
        }

        public void UpdateLivenessCheckStatus(int P_id, string CompanyRefNo,string JsonString)
        {
            try
            {
                DataBaseConnections Dc = new DataBaseConnections();
                ClientCustomize bl = new ClientCustomize();
                if (CompanyRefNo != "")
                {
                   
                    LivenessRoot model = JsonConvert.DeserializeObject<LivenessRoot>(JsonString);
                    double faceMatchScore = model.result.apiResponse.result.faceMatchScore;
                    double livenessScore = model.result.apiResponse.result.livenessScore;
                    bool isFaceMatch = model.result.apiResponse.result.isFaceMatch;
                    if (faceMatchScore > 60)
                    {
                        if (livenessScore > 60)
                        {
                            var rst = bl.UpdateCandidateChecksReportComments(P_id, 890);
                            bl.UpdatePQCandidateChecksColorCode(0, 6, "Green", P_id, 890, "Verified");
                            bl.UpdateCheckStatus(P_id, 890, 1, 37, "Check", 152);
                            Dc.UpdatelivenessChecktbldataEntryData(P_id, 890, "Positive");
                        }
                        else
                        {
                            var rst = bl.UpdateCandidateChecksReportComments(P_id, 890);
                            bl.UpdatePQCandidateChecksColorCode(248, 1, "Red", P_id, 890, "Liveness failed");
                            bl.UpdateCheckStatus(P_id, 890, 1, 37, "Check", 152);
                            Dc.UpdatelivenessChecktbldataEntryData(P_id, 890, "Negative");
                        }
                    }
                    else
                    {
                        var rst = bl.UpdateCandidateChecksReportComments(P_id, 890);
                        bl.UpdatePQCandidateChecksColorCode(248, 1, "Red", P_id, 890, "Face Match failed");
                        bl.UpdateCheckStatus(P_id, 890, 1, 37, "Check", 152);
                        Dc.UpdatelivenessChecktbldataEntryData(P_id, 890, "Negative");
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            } 
        }
        [HttpPost]
        public ActionResult SaveVideoWithDigilocker(HttpPostedFileBase videoFile, string Pid, string name, string otp)
        {
            try
            {
                DataBaseConnections Dc = new DataBaseConnections();
                ClientCustomize bl = new ClientCustomize();
                bool AadhaarStatus = false;

                if (videoFile == null || videoFile.ContentLength == 0)
                    return Json(new { success = false, message = "No video received" });

                var VideoFileName = SaveMP4Video(videoFile, Pid, name);
                string uploadFolder = Server.MapPath("~/Documents/CandidateVideos");

                string imageFolder = Server.MapPath(ConfigurationManager.AppSettings["provideddocuments"]);

                string ImageName = "";
                BAL.OCR.AadhaarOCRDetails Aadhaardata = new BAL.OCR.AadhaarOCRDetails();
                int LivenessWithAdharStatus = Dc.GetClientMaster(Convert.ToInt32(Session["ClientRowID"]));
                if (LivenessWithAdharStatus == 1)
                {
                    var tempDT = Dc.GetTempPQVerifiedUploadDocs(Convert.ToInt32(Pid), 890, "Candidate Image", 1, 15, "Desc");
                    if (tempDT.Rows.Count > 0)
                    {
                        ImageName = Convert.ToString(tempDT.Rows[0]["FileName"]);
                    }
                    else
                    {
                        ImageName = "";
                    }
                }
                else
                {
                    return Json(new { success = false, message = "The Liveness with Aadhaar has not been configured. Kindly coordinate with the software administrator to resolve this issue" });
                }
                string videoPath = Path.Combine(uploadFolder, VideoFileName);
                var videoStatus = ProcessVideoLiveness(videoPath, otp?.Replace(" ", ""), Path.Combine(imageFolder, ImageName), Pid);

                string statusValue = videoStatus["status"]?.ToString();
                if (string.IsNullOrEmpty(statusValue))
                    return Json(new { success = false, message = "Invalid response from liveness API" });

                JObject statusobj = JObject.Parse(statusValue);
                string submitResponse = videoStatus["submit"]?.ToString();
                string apiExecutionStatus = (string)statusobj["result"]?["apiExecutionStatus"];



                if (apiExecutionStatus == "CALLBACK_SUCCESS")
                {
                    int responseStatusCode = (int)statusobj["result"]["apiResponse"]["statusCode"];
                    if (responseStatusCode == 101)
                    {
                        Dc.InsertUpdateLivenessApiResponse(Convert.ToInt32(Pid), 890, statusValue, VideoFileName, submitResponse, "Complete");
                        bl.updateCheckStatus_Candidate_temp(Convert.ToInt32(Pid), 890, "Completed", "Info");
                        bl.updateCheckStatus_Candidate_temp(Convert.ToInt32(Pid), 890, "Completed", "Docs");
                        if (Convert.ToString(Session["ClientRowID"]) == "1606")
                        {
                            bl.updateCheckStatus_Candidate_temp(Convert.ToInt32(Pid), 0, "Completed", "Info");
                        }
                        return Json(new { success = true, message = "Video received with Liveness Success" });
                    }
                    else
                    {
                        string errormsg = (string)statusobj["result"]["apiResponse"]["error"];
                        string errorDescription = (string)statusobj["result"]["apiResponse"]["errorDescription"];
                        string ErrorMsg = $"{errormsg} {errorDescription}. Please read the instructions and re-upload the video.";
                        Dc.InsertUpdateLivenessApiResponse(Convert.ToInt32(Pid), 890, statusValue, VideoFileName, submitResponse, "Error Found");
                        return Json(new { success = false, message = ErrorMsg });
                    }
                }
                else
                {
                    Dc.InsertUpdateLivenessApiResponse(Convert.ToInt32(Pid), 890, statusValue, VideoFileName, submitResponse, "Error Found");
                    return Json(new { success = false, message = "Video liveness failed. Please read the instructions and re-upload the video." });
                }
            }
            catch (Exception ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~/ErrorLogs/Logfiles"), ex.ToString(), ex.StackTrace);
                return Json(new { success = false, message = "An error occurred while processing the video." });
            }
        }



        public string SaveImageFile(string base64Image, string Pid, string name)
        {
            try
            {
                if (base64Image != null && base64Image.Length > 0)
                {
                    string filePath = Server.MapPath(ConfigurationManager.AppSettings["provideddocuments".Trim()].ToString());

                    if (string.IsNullOrEmpty(base64Image))
                        return "";

                    base64Image = base64Image.Replace("data:image/jpeg;base64,", "")
                                             .Replace("data:image/png;base64,", "");

                    byte[] imageBytes = Convert.FromBase64String(base64Image);

                    string fileName = (name + "_" + Pid + "_selfie_" + DateTime.Now.Ticks + ".jpg").Trim().Replace(" ", "");
                    string selfie_Path = Path.Combine(filePath, fileName);

                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    using (Image img = Image.FromStream(ms))
                    {
                        using (Bitmap bmp = new Bitmap(img.Width, img.Height))
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.Clear(Color.White); // JPG background
                            g.DrawImage(img, 0, 0, img.Width, img.Height);

                            bmp.Save(selfie_Path, ImageFormat.Jpeg);
                        }
                    }
                    return fileName;
                }
            }
            catch (Exception Ex)
            {

            }
            return "";
        }

        public string SaveMP4Video(HttpPostedFileBase videoFile, string Pid, string name)
        {
            try
            {
                if (videoFile == null || videoFile.ContentLength == 0)
                    throw new Exception("No video file uploaded");

                string uploadFolder = Server.MapPath("~/Documents/CandidateVideos");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                // Create a temporary WEBM file path
                string tempWebmPath = Path.Combine(uploadFolder, Guid.NewGuid().ToString() + ".webm");

                // Save temporary WEBM file
                videoFile.SaveAs(tempWebmPath);

                // Create MP4 output path
                string mp4FileName = (name + "_" + Pid + "_" + Guid.NewGuid().ToString() + ".mp4").Replace(" ", "");
                string mp4Path = Path.Combine(uploadFolder, mp4FileName);

                // Convert to MP4
                ConvertWebmToMp4(tempWebmPath, mp4Path);

                // Delete the temporary WEBM file
                if (System.IO.File.Exists(tempWebmPath))
                    System.IO.File.Delete(tempWebmPath);

                return mp4FileName;
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

            }
            return "";

        }
        public void ConvertWebmToMp4(string inputPath, string outputPath)
        {
            try
            {
                string ffmpegPath = "D:\\wwwroot2\\Emailers\\ffmpeg.exe";
                //string ffmpegPath = "D:\\GitProjects\\BGV-APP-NEW-5.5\\WebAppBGV\\Emailers\\ffmpeg.exe";

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ffmpegPath,
                        Arguments = $"-i \"{inputPath}\" -c:v libx264 -c:a aac \"{outputPath}\"",
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.OutputDataReceived += (s, e) => { /* ignore or log */ };
                process.ErrorDataReceived += (s, e) => { /* ignore or log */ };

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit(); // now it will NOT hang
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        private readonly string apiKey = "D766V5omtmx8l28p3xea";

        public JObject ProcessVideoLiveness(string videoPath, string otp, string imagePath = "", string Pid = "")
        {
            // Step 1: Submit video-liveness
            JObject submitResponse = SubmitVideoLivenessInternal(videoPath, imagePath, otp, Pid);

            // Extract requestId
            string requestId = submitResponse["requestId"]?.ToString();

            if (string.IsNullOrEmpty(requestId))
            {
                return new JObject
                {
                    ["error"] = "requestId not found in response",
                    ["submitResponse"] = submitResponse
                };
            }

            // Step 2: Check status
            JObject statusResponse = GetVideoLivenessStatusInternal(requestId, Pid);
            string status = statusResponse["apiExecutionStatus"]?.ToString();
            string apiExecutionStatus = (string)statusResponse["result"]["apiExecutionStatus"];
            if (apiExecutionStatus != "CALLBACK_SUCCESS")
            {
                Thread.Sleep(9000);
                statusResponse = GetVideoLivenessStatusInternal(requestId, Pid);
                apiExecutionStatus = (string)statusResponse["result"]["apiExecutionStatus"];
                if (apiExecutionStatus != "CALLBACK_SUCCESS")
                {
                    Thread.Sleep(4000);
                    statusResponse = GetVideoLivenessStatusInternal(requestId, Pid);

                }
            }

            // Final combined response
            JObject final = new JObject
            {
                ["submit"] = submitResponse,
                ["status"] = statusResponse
            };

            return final;
        }


        // ---------------------------------------------------------
        // INTERNAL METHOD: Step 1 - Submit video-liveness
        // ---------------------------------------------------------
        private JObject SubmitVideoLivenessInternal(string videoPath, string imagePath, string otp, string Pid)
        {
            string apiUrl = "https://api.karza.in/kyc/v3/prod/video-liveness";
            DataBaseConnections DC = new DataBaseConnections();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-karza-key", apiKey);

                using (var form = new MultipartFormDataContent())
                {
                    // Add video
                    if (System.IO.File.Exists(videoPath))
                    {
                        var videoStream = new FileStream(videoPath, FileMode.Open, FileAccess.Read);
                        var videoContent = new StreamContent(videoStream);
                        videoContent.Headers.ContentType =
                            new System.Net.Http.Headers.MediaTypeHeaderValue("video/webm");

                        form.Add(videoContent, "video", Path.GetFileName(videoPath));
                    }

                    // Add image
                    if (System.IO.File.Exists(imagePath))
                    {
                        var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                        var imageContent = new StreamContent(imageStream);
                        imageContent.Headers.ContentType =
                            new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                        form.Add(imageContent, "image", Path.GetFileName(imagePath));
                    }

                    // Other fields
                    form.Add(new StringContent(otp), "otp");
                    form.Add(new StringContent("https://bgv.securitas-india.com"), "callbackUrl");
                    //form.Add(new StringContent(""), "clientData");

                    // Synchronous call
                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                    {
                        Content = form
                    };

                    //HttpResponseMessage response = client.Send(request);
                    var response = client.SendAsync(request).Result;

                    string result = response.Content.ReadAsStringAsync().Result;
                    DC.AddLivenessAPIlogs(Convert.ToInt32(Pid), 890, result, apiUrl);

                    return JObject.Parse(result);
                }
            }
        }

        // ---------------------------------------------------------
        // INTERNAL METHOD: Step 2 - Check video-liveness-status
        // ---------------------------------------------------------
        private JObject GetVideoLivenessStatusInternal(string requestId,string Pid)
        {
            string apiUrl = "https://api.karza.in/v3/video-liveness-status";
            DataBaseConnections DC = new DataBaseConnections();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-karza-key", apiKey);

                var payload = new
                {
                    requestId = requestId
                };

                string json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = content
                };

                //HttpResponseMessage response = client.Send(request);
                var response = client.SendAsync(request).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                DC.AddLivenessAPIlogs(Convert.ToInt32(Pid),890, result, apiUrl);
                return JObject.Parse(result);
            }
        }

        public string SaveTempPQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "",string UploadDoc="",  string remarks = "")
        {
            string Filename = "";
            try
            {
                AddTempPQVerifiedUploadDocViewModel DocModel = new AddTempPQVerifiedUploadDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.SubCheckRowID = SubCheckRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //  Provided / Verified
                DocModel.DocumentType = DocumentType; //Name of the selected docs
                string FldPathDoc = string.Empty;
                FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();
                string proImg = string.Empty;
                proImg = (SubCheckRowID.ToString() + DocumentType).Replace(" ", "_").Replace("-", "_") + DateTime.Now.ToString("ssfff") + ".pdf";

                string IronKey = System.Configuration.ConfigurationManager.AppSettings["IronKey"].ToString();
                var Renderer = new IronPdf.ChromePdfRenderer();
                IronPdf.License.LicenseKey = IronKey;
                var pdf = Renderer.RenderHtmlAsPdf(UploadDoc);

                pdf.SaveAs(Server.MapPath(FldPathDoc + proImg)); // Saves our PdfDocument object as a PDF

                DocModel.FileName = Path.GetFileName(proImg);

                Filename = proImg;
                DocModel.FileName = proImg;
                DocModel.FilePath = FldPathDoc;
                DocModel.UploadDate = DateTime.Now;
                DocModel.StatusDocxConvert = 0;
                DocModel.StatusPDF2DOCXConvert = 0;
                DocModel.Remarks = remarks;
                DocModel.PDF_FileName = "";
                DocModel.Status = 1;
                repoPQVerifiedUploadDoc.AddTempPQVerifiedUploadDoc(DocModel);
                repoPQVerifiedUploadDoc.SaveChanges();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return Filename;
        }

        public void SaveTempPQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", HttpPostedFileBase UploadDoc = null, string remarks = "", string Filename = "")
        {
            try
            {
                AddTempPQVerifiedUploadDocViewModel DocModel = new AddTempPQVerifiedUploadDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.SubCheckRowID = SubCheckRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //  Provided / Verified
                DocModel.DocumentType = DocumentType; //Name of the selected docs
                string FldPathDoc = string.Empty;
                FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();
                string proImg = string.Empty;
                string ext = Path.GetExtension(UploadDoc.FileName).ToLower();
                proImg = Filename;//(SubCheckRowID.ToString() + DocumentType + Session["CadidateUserName"].ToString()).Replace(" ", "_").Replace("-", "_") + DateTime.Now.ToString("ssfff") + ext;


                UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));

                DocModel.FileName = Path.GetFileName(proImg);
                DocModel.FilePath = FldPathDoc;
                DocModel.UploadDate = DateTime.Now;
                DocModel.StatusDocxConvert = 0;
                DocModel.StatusPDF2DOCXConvert = 0;
                DocModel.Remarks = remarks;
                DocModel.PDF_FileName = "";
                DocModel.Status = 1;
                repoPQVerifiedUploadDoc.AddTempPQVerifiedUploadDoc(DocModel);
                repoPQVerifiedUploadDoc.SaveChanges();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        [HttpPost]
        [SessionExpireFilterAttributeCandidate]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        public ActionResult eSigns(string SignerName, string AadhaarLast4, string Location, string Reason,
                                 float? XCoordinate = null, float? YCoordinate = null,
                                 float? Width = null, float? Height = null)
        {
            try
            {
                float x = XCoordinate ?? 72f;
                float y = YCoordinate ?? 432f;
                float width = Width ?? 144f;
                float height = Height ?? 72f;




                string pdfDir = Server.MapPath("~/eSign/eDocuments");
                if (!Directory.Exists(pdfDir))
                {
                    Directory.CreateDirectory(pdfDir);
                }

                string uniqueId = Guid.NewGuid().ToString();
                string displayPdfPath = Path.Combine(pdfDir, "display_" + Session["TempPersonalRowID"].ToString() + ".pdf");
                string tempPdfPath = Path.Combine(pdfDir, "temp_" + Session["TempPersonalRowID"].ToString() + ".pdf");
                string signedPdf = Path.Combine(pdfDir, "signed_" + Session["TempPersonalRowID"].ToString() + ".pdf");

                //string displayPdfPath = Path.Combine(pdfDir, "display.pdf");
                //string tempPdfPath = Path.Combine(pdfDir, "temp.pdf");
                //string signedPdf = Path.Combine(pdfDir, "signed.pdf");

                string SignatureFieldName = "Signature1";


                using (var fs = new FileStream(displayPdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var doc = new Document(PageSize.A5);
                    var writer = PdfWriter.GetInstance(doc, fs);

                    doc.Open();
                    doc.Add(new Paragraph("Aadhaar eSign Test Document"));
                    doc.Close();
                }

                //byte[] fileBytes;
                //using (var ms = new MemoryStream())
                //{
                //    file.InputStream.CopyTo(ms);
                //    fileBytes = ms.ToArray();
                //}
                //string base64S = Convert.ToBase64String(fileBytes);

                byte[] displayBytes = System.IO.File.ReadAllBytes(displayPdfPath);
                byte[] tempPdfBytes;
                string pdfHash;


                using (var reader = new PdfReader(displayBytes))
                {
                    using (var msOut = new MemoryStream())
                    {
                        PdfStamper stamper = PdfStamper.CreateSignature(reader, msOut, '\0', null, true);
                        PdfSignatureAppearance appearance = stamper.SignatureAppearance;

                        Rectangle rect = new Rectangle(x, y, x + width, y + height);
                        appearance.SetVisibleSignature(rect, 1, SignatureFieldName);

                        DateTime signTime = DateTime.Now.AddSeconds(10);

                        appearance.Location = Location ?? "";
                        appearance.Reason = Reason ?? "";
                        appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
                        appearance.SignDate = signTime;
                        appearance.Acro6Layers = false;

                        appearance.Layer2Text =
                            "Digitally Signed by: " + (SignerName ?? "Unknown") + "\n" +
                            "Date: " + signTime.ToString("dd-MM-yyyy HH:mm:ss") + "\n" +
                            "Location: " + (Location ?? "") + "\n" +
                            "Reason: " + (Reason ?? "");

                        PdfSignature dic = new PdfSignature(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
                        dic.Reason = appearance.Reason;
                        dic.Location = appearance.Location;
                        dic.Contact = "";
                        dic.Date = new PdfDate(signTime);

                        appearance.CryptoDictionary = dic;

                        int estimatedSize = 100000;

                        PdfDictionary dic2 = new PdfDictionary();
                        dic2.Put(PdfName.FILTER, PdfName.ADOBE_PPKLITE);
                        dic2.Put(PdfName.SUBFILTER, PdfName.ADBE_PKCS7_DETACHED);

                        IExternalSignatureContainer external = new ExternalBlankSignatureContainer(dic2);
                        MakeSignature.SignExternalContainer(appearance, external, estimatedSize);

                        using (Stream range = appearance.GetRangeStream())
                        using (var sha = SHA256.Create())
                        {
                            byte[] hashBytes = sha.ComputeHash(range);
                            pdfHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                        }

                        tempPdfBytes = msOut.ToArray();
                    }
                }

                System.IO.File.WriteAllBytes(tempPdfPath, tempPdfBytes);


                Session["GeneratedPDF"] = Convert.ToBase64String(tempPdfBytes);
                Session["GeneratedHash"] = pdfHash;


                EsignXmlSigner esignXmlSigner = new EsignXmlSigner();
                string xml = esignXmlSigner.GenerateEsignXml(pdfHash, Session["CandidateUserRowID"].ToString(), Session["AuthenticationToken"].ToString(), Session["KAuthType"].ToString());

                Session["XMLRequest"] = xml;
                TempData["GeneratedXML"] = xml;
                TempData["PdfHash"] = pdfHash;



                System.Xml.XmlDocument docs = new System.Xml.XmlDocument();
                docs.LoadXml(xml);

                System.Xml.XmlElement root = docs.DocumentElement;
                string txn = root.GetAttribute("txn");
                string[] txnId = txn.Split('_');
                string authToken = txnId[0].ToString();
                Session["AuthenticationToken"] = authToken;



                xml = Session["XMLRequest"] as string;

                if (string.IsNullOrEmpty(xml))
                    return Content("Generate XML first.");


                // Get PFX path (MVC4 style)
                string pfxPath = Server.MapPath("~/eSign/roots/certs/DocumentSigner.pfx");
                string password = "ABC12";

                // Sign XML
                string signedXml = EsignXmlSigner.SignEsignXml(xml, pfxPath, password);

                // Store in Session
                Session["SignedXML"] = signedXml;

                // Store in TempData
                TempData["SignedXML"] = signedXml;
                ViewBag.SignedXML = signedXml;


                
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View();
        }

        [HttpPost]
        public ActionResult SendEsignRequest()
        {
            // Get Signed XML from Session
            string signedXml = Session["SignedXML"] as string;
            string pp = Session["AuthenticationToken"].ToString();

            if (string.IsNullOrEmpty(signedXml))
                return Content("Sign XML first.");

            string apiUrl = "https://demo.esign.digital/esign/2.1/signdoc/";
            //sessionId = " + HttpContext.Session.Id
            string html = string.Format(@"
        <html>
            <body onload='document.forms[0].submit();'>
                <form method='post' action='{0}'>
                    <textarea name='esigndoc' style='display:none'>{1}</textarea>
                </form>
            </body>
        </html>",
                apiUrl,
                System.Web.HttpUtility.HtmlEncode(signedXml)
            );

            return Content(html, "text/html");
        }

        [HttpGet]
        [SessionExpireFilterAttributeCandidate]
        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult eSigns(string AuthType = null)
        {
            try
            {
                long tempperid = Convert.ToInt64(Session["TempPersonalRowID"].ToString());

                CreateMenuitemforDataEntryOperator(tempperid);
                CreateMenuitemforDataEntrySubmit(tempperid);
                Session["KAuthType"] = AuthType;
               

            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View();

        }

        public ActionResult DownloadSignedPdf(string tempPresonalId)
        {
            byte[] fileBytes = new byte[0];
            try
            {
                //string final = Server.MapPath("~/eSign/eDocuments/signed_" + Session["TempPersonalRowID"].ToString() + ".pdf");
                string final = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();

                final = Server.MapPath(final) + "signed_" + Session["TempPersonalRowID"].ToString() + ".pdf";


                if (!System.IO.File.Exists(final))
                {
                    return Content("Signed PDF not found.");
                }

                fileBytes = System.IO.File.ReadAllBytes(final);


            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return File(fileBytes, "application/pdf", "SignedDocument.pdf");
        }

        [ValidateInput(false)]
        [ValidateSessionFilterCandidate]
        public ActionResult Digital_ESignLOA(string CanName = "")
        {
            long personalrowid = Convert.ToInt64(Session["TempPersonalRowID"]);
            DataSet d = tpc.GetTempCandidateChecksDT(personalrowid);
            ViewBag.CanName = d.Tables[0].Rows[0]["LOA_Name"].ToString();
            ViewBag.CaseStatus = d.Tables[0].Rows[0]["CaseinfoStatus"].ToString();
            string final = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();

            final = Server.MapPath(final) + "signed_" + Session["TempPersonalRowID"].ToString() + ".pdf";


            if (!System.IO.File.Exists(final))
            {
                ViewBag.LoaImageB64 = d.Tables[0].Rows[0]["LOA_Signature"].ToString();
            }


            ViewBag.PersonalRowId = clsCommonMethods.PEncrypt(personalrowid.ToString());
            if (Convert.ToString(d.Tables[0].Rows[0]["LOA_Signature"]) != "")
            {
                string CanLOASignature = d.Tables[0].Rows[0]["LOA_Signature"].ToString();
                ViewBag.LoaImage = LoadImage(CanLOASignature);
                ViewBag.LoaImageB64 = CanLOASignature;

            }
            else
            {
                ViewBag.CanLOASignature = "";
            }
            return View();
        }



        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [ValidateSessionFilterCandidate]
        public ActionResult Digital_ESignLOA(FormCollection formCollection, HttpPostedFileBase file, HttpPostedFileBase Excelfile, string digitalcheck = "")
        {
            if (digitalcheck != "")
            {
                ViewBag.digitalcheck = "Checkedbox";
            }
            ClientCustomize cc = new ClientCustomize();
            string newimage = formCollection["hfSign"].ToString();
            string Name = formCollection["Name"].ToString();
            string CTStamp = formCollection["CTStamp"].ToString();
            string[] base64 = newimage.Split(',');
            string FldPathDocs = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();

            FldPathDocs = Server.MapPath(FldPathDocs) + "signed_" + Session["TempPersonalRowID"].ToString() + ".pdf";
            string Base64Image = "";
            //if (!System.IO.File.Exists(FldPathDocs))
            //    //if(newimage!="")
            //{

            if (Name == "")
            {
                ViewBag.ErrorMsg = "Please fill Name.";
                return View("Digital_LOA", new { CanName = Name });
            }
            if (newimage == "")
            {
                ViewBag.ErrorMsg = "Please fill signature.";
                @ViewBag.CanName = Name;
                return View("Digital_LOA", new { CanName = Name });
            }

            if (base64.Length == 0)
            {
                ViewBag.ErrorMsg = "Please fill signature.";
                return View("Digital_LOA", new { CanName = Name });
            }

            if (base64.Length == 1)
            {
                Base64Image = base64[0].ToString();

            }

            if (base64.Length > 1)
            {
                Base64Image = base64[1].ToString();

            }
            //}


            //string hostName = System.Net.Dns.GetHostName();
            //string myIP = System.Net.Dns.GetHostByName(hostName).AddressList[0].ToString();

            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];

            string fileSavePath = "", filesavepathexcel = "", filenameexcel = "";
            string fileSavePath2 = "";
            long personalrowid = Convert.ToInt64(Session["TempPersonalRowID"]);
            if (file != null && file.ContentLength > 0 && (System.IO.Path.GetExtension(file.FileName).ToLower() == ".jpg" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".png" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".jpeg" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".pdf"))
            {

                string FldPathDoc = string.Empty;
                FldPathDoc = ConfigurationManager.AppSettings["provideddocumentsAPI".Trim().ToLower()].ToString();
                string FldPathDoc2 = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                string SecuId = Convert.ToString(Session["SecritadRefId"]);
                string ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                string filename = personalrowid + "_" + "LOA_Pic";
                fileSavePath = FldPathDoc + filename + ext;
                fileSavePath2 = FldPathDoc2 + filename + ext;
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                string base64S = Convert.ToBase64String(fileBytes);
                string res = FileSaveDownloadAPI.DocReportUploadDynamic(filename + ext, FldPathDoc, base64S);
                //file.SaveAs(fileSavePath);
            }

            if (Excelfile != null && Excelfile.ContentLength > 0)
            {


                string fldpathdoc1 = string.Empty;
                fldpathdoc1 = ConfigurationManager.AppSettings["provideddocumentsAPI".Trim().ToLower()].ToString();
                string SecuId = Convert.ToString(Session["secritadrefid"]);
                string extfile = Path.GetExtension(Excelfile.FileName);
                //DateTime dateTime = DateTime.UtcNow.Date;
                //string vv = DateTime.Now.ToString("M-d-yyyy");
                // filenameexcel = personalrowid + "_onboarding_" + vv + ext;
                filenameexcel = personalrowid + "_" + "LOA_Excel";
                filesavepathexcel = fldpathdoc1 + filenameexcel + extfile;
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    Excelfile.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                string base64Str = Convert.ToBase64String(fileBytes);
                string resp = FileSaveDownloadAPI.DocReportUploadDynamic(filenameexcel + filenameexcel, fldpathdoc1, base64Str);
                //Excelfile.SaveAs(filesavepathexcel);
            }

            int PersonalRowid = Convert.ToInt32(Session["TempPersonalRowID"]);
            cc.SaveDigitalSignature(personalrowid, Base64Image, Name, fileSavePath2, filesavepathexcel, filenameexcel, CTStamp, ipaddress);
            string CanLOASignature = "";
            Document pdfDoc = new Document(PageSize.A4, 40, 40, 80, 25);
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();

            //Table
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            //Cell          
            // Define your hexadecimal color code
            string hexColor = "#031F30"; // Example hex color
            // Convert hex to RGB
            int r = Convert.ToInt32(hexColor.Substring(1, 2), 16);
            int g = Convert.ToInt32(hexColor.Substring(3, 2), 16);
            int b = Convert.ToInt32(hexColor.Substring(5, 2), 16);
            // Create a BaseColor instance

            BaseColor customColor = new BaseColor(r, g, b);
            PdfPCell cell = new PdfPCell();
            if (Session["ClientRowID"].ToString() == "684")
            {
                cell = new PdfPCell(new Phrase("NFORMATION RELEASE AUTHORIZATION", FontFactory.GetFont("Securitas Pro", 14, iTextSharp.text.Font.BOLD, customColor)));
            }
            else
            {
                cell = new PdfPCell(new Phrase("DECLARATION & AUTHORIZATION", FontFactory.GetFont("Securitas Pro", 14, iTextSharp.text.Font.BOLD, customColor)));
            }
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(172, 194, 221);
            table.AddCell(cell);
            if (Session["ClientRowID"].ToString() == "1428")
            {
                cell = new PdfPCell(new Phrase("Collect and process my personal information as required to process background verification, including collection and processing of sensitive personal information.\n\nShare my personal information with, and obtain information from, the following third parties as necessary to carry out the requested services: police, courts, suppliers of court and police information for criminal record checks, employers, employment agencies, educational institutions, credit and consumer reporting agencies, government agencies and other public bodies, regulatory or professional bodies, references you provide, other organizations with which I have been associated, and other publicly available print or online sources. I understand that these third parties may be located in countries or regions other than my country of residence, including places where I have previously lived, studied or worked.\n\nWhere necessary, act as my agent to share or obtain your personal information when carrying out the services.\n\nThe data may cross geographical boundaries for the purpose of background verification.For more information, please refer to our privacy policy.\n\n", FontFactory.GetFont("Securitas Pro", 10, customColor)));
            }
            else if (Session["ClientRowID"].ToString() == "1481")
            {
                cell = new PdfPCell(new Phrase("I certify that the statements made in this application are valid and complete to the best of my knowledge. I understand that false or misleading information may result in termination of the agreement.\n\nIf upon investigations, any of this information is found to be incomplete or inaccurate, I understand that agreement will be subject to dismissal at any time during my employment.\n\nI hereby authorize CtrlS Datacenters Limited and/or any of its subsidiaries or affiliates and any persons or organizations acting on its behalf, to verify the information presented on this application form and to procure an investigative report or consumer report for that purpose.\n\nI hereby grant authority for the bearer of this letter to access or be provided with full details of my previous records. In addition, please provide any other pertinent information requested by the individual presenting this authority.\n\nI hereby release from liability all persons or entities requesting or supplying such information.\n\nI am aware that no Money / fee / postage / gift to be paid or given in goodwill to any one during the process of verification and I have informed my family members as well regarding this.\n\nI am aware that during the Address verification, no photographs or documents to be shared with verifier through any medium (Whats App, Email, Etc.)\n\nI have read, understand, and by my signature consent to these statements.\n\nPlease Sign In given below box. And click On Save Button to give your Consent.\n", FontFactory.GetFont("Securitas Pro", 10, customColor)));
            }
            else if (Session["ClientRowID"].ToString() == "576")
            {
                cell = new PdfPCell(new Phrase("I certify that the statements made in this application are valid and complete to the best of my knowledge. I understand that false or misleading information may result in termination of the agreement.\n\nIf upon investigations, any of this information is found to be incomplete or inaccurate, I understand that agreement will be subject to dismissal at any time during my employment.\n\nI hereby authorize the Company and / or any of its subsidiaries or affiliates and any persons or organizations acting on its behalf, to verify the information presented on this application form and to procure an investigative report or consumer report for that purpose. \n\nI hereby grant authority for the bearer of this letter to access or be provided with full details of my previous records.In addition, please provide any other pertinent information requested by the individual presenting this authority. \n\nI hereby release from liability all persons or entities requesting or supplying such information.\n\nI am aware that no Money / fee / postage / gift to be paid or given in goodwill to any one during the process of verification and I have informed my family members as well regarding this. \n\nI am aware that during the Address verification, no photographs or documents to be shared with verifier through any medium (Whats App, Email, Etc.)\n\nNagarro acts as the data fiduciary and engages appointed vendors to conduct background verification checks. By signing this authorization letter, you agree and acknowledge that your personal data will be collected, processed, and stored in compliance with applicable data protection laws. Detailed information about how your personal data is handled can be found at: Applicant Privacy Notice-EN.pdf\n\nI have read, understand, and by my signature consent to these statements.\n\nPlease Sign In given below box. And click On Save Button to give your Consent.\n", FontFactory.GetFont("Securitas Pro", 10, customColor)));
            }
            else if (Session["ClientRowID"].ToString() == "684")
            {
                cell = new PdfPCell(new Phrase("To secure employment with ALLEGIS SERVICES (INDIA) PRIVATE LIMITED (hereinafter referred to as “Company”), I certify that the statements made in this application are valid and complete to the best of my knowledge.I understand that by sharing any false or misleading information, I may be disqualified from my employment, or it may result in termination of my employment with the Company, whereby the Company may also recover any remuneration paid to me.\n\nIf upon investigations, any of this information is found to be incomplete or inaccurate, I understand that agreement will be subject to dismissal at any time during my employment.\n\nI hereby authorize the Company and / or any of its subsidiaries or affiliates and any persons or organizations acting on its behalf, to verify the information presented on this application form and to procure an investigative report or consumer report for that purpose. \n\nI hereby grant authority for the bearer of this letter to access or be provided with full details of my previous records.In addition, please provide any other pertinent information requested by the individual presenting this authority. \n\nI hereby release from liability all persons or entities requesting or supplying such information.\n\nI am aware that no Money / fee / postage / gift to be paid or given in goodwill to any one during the process of verification and I have informed my family members as well regarding this. \n\nI am aware that during the Address verification, no photographs or documents to be shared with verifier through any medium (Whats App, Email, Etc.)\n\nNagarro acts as the data fiduciary and engages appointed vendors to conduct background verification checks. By signing this authorization letter, you agree and acknowledge that your personal data will be collected, processed, and stored in compliance with applicable data protection laws. Detailed information about how your personal data is handled can be found at: Applicant Privacy Notice-EN.pdf\n\nI have read, understand, and by my signature consent to these statements.\n\nPlease Sign In given below box. And click On Save Button to give your Consent.\n", FontFactory.GetFont("Securitas Pro", 10, customColor)));
            }
            else
            {
                cell = new PdfPCell(new Phrase("I certify that the statements made in this application are valid and complete to the best of my knowledge. I understand that false or misleading information may result in termination of the agreement.\n\nIf upon investigations, any of this information is found to be incomplete or inaccurate, I understand that agreement will be subject to dismissal at any time during my employment.\n\nI hereby authorize the Company and / or any of its subsidiaries or affiliates and any persons or organizations acting on its behalf, to verify the information presented on this application form and to procure an investigative report or consumer report for that purpose. \n\nI hereby grant authority for the bearer of this letter to access or be provided with full details of my previous records.In addition, please provide any other pertinent information requested by the individual presenting this authority. \n\nI hereby release from liability all persons or entities requesting or supplying such information.\n\nI am aware that no Money / fee / postage / gift to be paid or given in goodwill to any one during the process of verification and I have informed my family members as well regarding this. \n\nI am aware that during the Address verification, no photographs or documents to be shared with verifier through any medium (Whats App, Email, Etc.)\n\nI have read, understand, and by my signature consent to these statements.\n\nPlease Sign In given below box. And click On Save Button to give your Consent.\n", FontFactory.GetFont("Securitas Pro", 10, customColor)));
            }
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Padding = 80f;
            cell.Colspan = 4;
            table.AddCell(cell);

            cell = new PdfPCell();
            DataTable dt = new DataTable();
            dt = cc.GetExportLoa(PersonalRowid);
            if (dt.Rows.Count > 0)
            {
                ViewBag.LOA_Signature = dt.Rows[0]["LOA_Signature"].ToString();
                if (Convert.ToString(dt.Rows[0]["LOA_Signature"]) != "")
                {
                    CanLOASignature = dt.Rows[0]["LOA_Signature"].ToString();
                    ViewBag.LoaImage = LoadImage(CanLOASignature);
                    ViewBag.LoaImageB64 = CanLOASignature;
                }
                else
                {
                    ViewBag.CanLOASignature = "";
                }
            }
            //if (!System.IO.File.Exists(FldPathDocs))
            //{
            //    LoadImages(CanLOASignature, PersonalRowid);
            //}
            //else
            //{
            LoadImages(Base64Image, PersonalRowid);

            //}
            string path = ConfigurationManager.AppSettings["DocumentsAPI"].ToString();
            string Strbase64 = FileSaveDownloadAPI.DocAndReportDownloadDynamic(PersonalRowid + ".png", path);
            byte[] imgBytes = Convert.FromBase64String(Strbase64);
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgBytes); img.ScaleAbsolute(200, 100);
            img.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            cell.AddElement(img);
            cell.Colspan = 4;
            table.AddCell(cell);

            cell = new PdfPCell();
            table.AddCell("Name:-");
            table.AddCell(ViewBag.LOA_Name = dt.Rows[0]["LOA_Name"].ToString());
            table.AddCell("Date:-");
            table.AddCell(ViewBag.CreatedDate = dt.Rows[0]["CreatedDate"].ToString());
            table.AddCell(cell);

            pdfDoc.Add(table);


            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            byte[] bytes = memoryStream.ToArray();
            string ProcFileName = "Letter_Of_Authorization" + PersonalRowid + ".pdf";
            string ProcfilePath = ConfigurationManager.AppSettings["provideddocumentsAPI".Trim().ToLower()].ToString();
            string ProcfilePath2 = ConfigurationManager.AppSettings["provideddocuments".Trim().ToLower()].ToString();
            string base64String = Convert.ToBase64String(bytes);
            string res2 = FileSaveDownloadAPI.DocReportUploadDynamic(ProcFileName, ProcfilePath, base64String);
            //string ProcfilePath = "~/Documents/CandidateDocuments/ProvidedDocuments/";
            //string filePath = Server.MapPath("~/Documents/CandidateDocuments/ProvidedDocuments/Letter_Of_Authorization" + PersonalRowid + ".pdf");
            //System.IO.File.WriteAllBytes(filePath, bytes);
            memoryStream.Close();
            cc.UpdateExportLoa(Convert.ToInt32(PersonalRowid), ProcfilePath2 + "Letter_Of_Authorization" + PersonalRowid + ".pdf");
            cc.proc_InsertUpdateDigitalLOAInTempPQVerifiedUploadCaseDocs(Convert.ToInt32(personalrowid), ProcFileName, ProcfilePath2, "Provided", "Letter Of Authorization");
            ViewBag.LoaImageB64 = Base64Image;
            @ViewBag.CanName = Name;
            CreateMenuitemforDataEntryOperator(personalrowid);
            //return View("Digital_LOA", new { CanName = Name });
            return RedirectToAction("Digital_ESignLOA", "Home", new { CanName = Name });
        }

        public bool ValidateMaliciousFile(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return false;



            // Content Type Check
            string[] allowedContentTypes =
            {
        "application/pdf",
        "image/jpeg",
        "image/png",
        "image/gif",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "text/plain",
        "application/zip",
        "application/x-zip-compressed"
    };

            if (!allowedContentTypes.Contains(file.ContentType.ToLower()))
                return false;

            return true;
        }



    }

}
