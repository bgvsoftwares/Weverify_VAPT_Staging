using BAL;
using BAL.ClientRepository;
using BAL.ProvidedInfoRepository;
using DAL;
using DocumentFormat.OpenXml.EMMA;
using iTextSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Unity.Utility;
using ViewModels;
using ViewModels.ClientViewModel;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.refinitiv;
using ViewModels.TempViewModel;
using WebAppBGV.CommonMethods;
using WebAppBGV.DocumentServices;
using WebAppBGV.DocumentServices.Model;
using WebAppBGV.Security;

namespace WebAppBGV.Areas.ClientArea.Controllers
{
    [CustomAuthorizeClient]
    public class ClientCompanyController : Controller
    {
        #region ****** Repository Object ******
        IClientCheckRepository repoClientCheck;
        IPQCandidateCheckRepository repoPQCandidateCheck;
        IPQPersonalRepository repoPQPersonal;
        IPQClientRepository repoPQClient;
        IClientPackageRepository repoClientPackage;
        IPQClientCompnayUploadDocRepository repoClientCmpUplod;
        IDropDownMasterRepository repoDDL;
        IPQClientCandiBulkUploadRepository repoPQClientCandiBulkUpload;
        IPQClientHolidayRepository repoPQClientHoliday;
        IAbbreviationRepository repoAbbreviation;
        IPQVerifiedUploadDocRepository repoPQVerifiedUploadDoc;
        IClientDashboardRepository repoDashboardClient;
        public readonly DocumentService _documentService;
        ClientCustomize cc = new ClientCustomize();
        public ClientCompanyController(

            IClientCheckRepository _repoClientCheck
            , IPQPersonalRepository _repoPQPersonal
            , IPQClientRepository _repoPQClient
            , IPQCandidateCheckRepository _repoPQCandidateCheck
            , IClientPackageRepository _repoClientPackage
            , IPQClientCompnayUploadDocRepository _repoClientCmpUplod
            ,IDropDownMasterRepository _repoDDL
             , IPQClientCandiBulkUploadRepository _repoPQClientCandiBulkUpload
            , IPQClientHolidayRepository _repoPQClientHoliday
            , IAbbreviationRepository _repoAbbreviation
              , IPQVerifiedUploadDocRepository _repoPQVerifiedUploadDoc
            , IClientDashboardRepository _repoDashboardClient
            , DocumentService documentService
            )
        {
            repoPQPersonal = _repoPQPersonal;
            _documentService = documentService;
            repoClientCheck = _repoClientCheck;
            repoPQClient = _repoPQClient;
            repoPQCandidateCheck = _repoPQCandidateCheck;
            repoClientPackage = _repoClientPackage;
            repoClientCmpUplod = _repoClientCmpUplod;
            repoPQClientCandiBulkUpload = _repoPQClientCandiBulkUpload;
            repoDDL = _repoDDL;
            repoPQClientHoliday = _repoPQClientHoliday;
            repoAbbreviation = _repoAbbreviation;
            repoPQVerifiedUploadDoc = _repoPQVerifiedUploadDoc;
            repoDashboardClient = _repoDashboardClient;
        }
        #endregion

        // GET: ClientArea/ClientCompany
        [SessionExpireFilterAttributeClient]
        public ActionResult Index()
        {
            return View();
        }

        [SessionExpireFilterAttributeClient]
        public ActionResult ProcessFlow()
        {
            return View();
        }

        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]

        public ActionResult ManageClientCompany(FormCollection formCollection,int page = 1, string sort = "", string sortDir = "desc", string Search = "",short id=0)
        {
            ClientByBulkUploadListPagedModel model = new ClientByBulkUploadListPagedModel();
            try
            {
                short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                if (ClientRowID == 1209)
                {
                    BindClientListByLogInName(ClientRowID, ClientUserRowID);
                    ViewBag.clientRowId = ClientRowID.ToString();

                    if(id>0)
                    {
                        ClientRowID = id;
                    }
                }
                int pageSize = 20;
                model = repoClientCmpUplod.GetCompanyListForUpload(page, pageSize, sort, sortDir, Search, ClientRowID);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Error Found !.";
                string ErrorMessage = ViewBag.ErrorMsg ;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return RedirectToAction("ExceptionPage", "Home", new { area = "ClientArea", ClientRowId = "", msg = ErrorMessage });
            }
            return View(model);
        }
        public void CandidateUploadTemplateNew()
        {
            //string FileName = "CandidateUploadTemplate.xlsx";
            int ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
            string FileName = "";

            FileName = "CandidateUploadTemplateschema.xlsx";



            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["clientCanidatetemplate"].ToString();
            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=CandidateUploadTemplate.xlsx");
            Response.TransmitFile(Server.MapPath(FldPathDoc) + FileName);
            Response.End();
            //return Redirect("~/Documents/CandidateDocuments/CandidateManual/ClientManual.pdf");
        }
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public ActionResult AddCasesByBulkUpload()
        {
            ClientByBulkUploadViewModel model = new ClientByBulkUploadViewModel();
            try
            {
                model.ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                int ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                if (model.ClientRowID == 1209)
                {
                    BindClientListByLogInName(model.ClientRowID, ClientUserRowID);
                    ViewBag.clientRowId = model.ClientRowID.ToString();
                }
                // CaptchaModel obj = new CaptchaModel();
                model.CapImage = (RandomString(6));
                model.CapImageText = model.CapImage;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Error Found !.";
                string ErrorMessage = ViewBag.ErrorMsg ;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return RedirectToAction("ExceptionPage", "Home", new { area = "ClientArea", ClientRowId = "", msg = ErrorMessage });

            }
            return View(model);
        }


        //[NonAction]
        //public string GatZipFileMessage(HttpPostedFileBase postedFile, string clientRowid, string filenm)
        //{
        //    string NowTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        //    GetMimeTypeTrue MimeType = new GetMimeTypeTrue();
        //    string ErrorMessage = "";
        //    string StartupPath = Server.MapPath("~/Uploads/TempFile/");
        //    StartupPath += clientRowid + "_" + filenm + "_" + NowTime;
        //    string TempPath = Server.MapPath("~/Uploads/");
        //    if (!Directory.Exists(StartupPath + "/"))
        //    {
        //        Directory.CreateDirectory(StartupPath);
        //    }
        //    ValidateParentFileUploadedByCandidate obj = new ValidateParentFileUploadedByCandidate();
        //    string ext = Path.GetExtension(postedFile.FileName);
        //    string filename = clientRowid + "_" + filenm + "_" + NowTime + ext;
        //    string base64 = string.Empty;
        //    postedFile.InputStream.Position = 0; 
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        postedFile.InputStream.CopyTo(memoryStream); // Convert the byte array to Base64
        //        base64 = Convert.ToBase64String(memoryStream.ToArray()); 
        //    }
        //        _documentService.UploadDynamicCandidateFiles(filename, base64, "D:/wwwroot2/Uploads/");
        //    //postedFile.SaveAs(TempPath + Path.GetFileName(filename));
        //    if (postedFile != null)
        //    {
        //        if (postedFile.ContentLength > 0)
        //        {
        //            string[] AllowedExtension = new string[] { ".ZIP", ".zip" };
        //            var bytes = System.IO.File.ReadAllBytes(TempPath + Path.GetFileName(filename));
        //            if (MimeType.GetMimeType(bytes, TempPath + Path.GetFileName(filename)) == false)
        //            {
        //                ErrorMessage = "Only,.doc,.xls,.pdf,Image and Zip File allowed";
        //            }
        //            else
        //            {
        //                if (!obj.IsValid(TempPath + Path.GetFileName(filename)))
        //                {
        //                    ErrorMessage = obj.ErrorMessage;
        //                }
        //                if (ErrorMessage == "")
        //                {
        //                    if (AllowedExtension.Contains(ext))
        //                    {
        //                        ZipFile.ExtractToDirectory(TempPath + Path.GetFileName(filename), StartupPath);
        //                        using (ZipArchive zipArchive = ZipFile.OpenRead(TempPath + Path.GetFileName(filename)))
        //                        {
        //                            foreach (ZipArchiveEntry item in zipArchive.Entries)
        //                            {
        //                                ValidateChildFileUploadedByCandidate obj1 = new ValidateChildFileUploadedByCandidate();
        //                                string[] Extension = item.Name.ToString().Split('.');
        //                                string filePath = StartupPath + "/" + item.FullName;
        //                                if (Extension.Length == 2 || Extension.Length == 3)
        //                                {
        //                                    bytes = System.IO.File.ReadAllBytes(filePath);
        //                                    if (MimeType.GetMimeType(bytes, filePath) == false)
        //                                    {
        //                                        ErrorMessage = "Only,.doc,.pdf,Image files allowed in Zip File ";
        //                                    }
        //                                    if (!obj1.IsValid(filePath))
        //                                    {
        //                                        ErrorMessage = obj1.ErrorMessage;
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    FileInfo file1 = new FileInfo(TempPath + Path.GetFileName(filename));
        //    if (file1.Exists)
        //    {
        //        file1.Delete();
        //    }
        //    if (Directory.Exists(StartupPath))
        //    {
        //        Directory.Delete(StartupPath, true);
        //    }
        //    return ErrorMessage;
        //}

        [NonAction]
        public async Task<string> GatZipFileMessage(HttpPostedFileBase postedFile, string clientRowid, string filenm)
        {
            string NowTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            GetMimeTypeTrue MimeType = new GetMimeTypeTrue();
            string ErrorMessage = "";
            string StartupPath = Server.MapPath("~/Uploads/TempFile/");
            StartupPath += clientRowid + "_" + filenm + "_" + NowTime;
            string TempPath = Server.MapPath("~/Uploads/");
            if (!Directory.Exists(StartupPath + "/"))
            {
                Directory.CreateDirectory(StartupPath);
            }
            ValidateParentFileUploadedByCandidate obj = new ValidateParentFileUploadedByCandidate();
            string ext = Path.GetExtension(postedFile.FileName);
            string filename = clientRowid + "_" + filenm + "_" + NowTime + ext;
            postedFile.SaveAs(TempPath + Path.GetFileName(filename));
            string base64 = string.Empty;
            postedFile.InputStream.Position = 0;
            using (var memoryStream = new MemoryStream())
            {
                postedFile.InputStream.CopyTo(memoryStream); // Convert the byte array to Base64 
                base64 = Convert.ToBase64String(memoryStream.ToArray());
            }
            _documentService.UploadDynamicCandidateFiles(filename, base64, "D:/wwwroot2/Uploads/");
            if (postedFile != null)
            {
                if (postedFile.ContentLength > 0)
                {
                    string[] AllowedExtension = new string[] { ".ZIP", ".zip" };
                    HttpResponseMessage response = await _documentService.GetDocumentAsync(filename, "D:/wwwroot2/Uploads/");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var objnew = JsonConvert.DeserializeObject<DocumentResponse>(jsonString);

                        string docName = objnew.DocumentName;
                        string base64Data = objnew.DocumentValue;


                        byte[] fileBytes = Convert.FromBase64String(base64Data);
                        string contentType = System.Web.MimeMapping.GetMimeMapping(docName);
                        if (MimeType.GetMimeType(fileBytes, TempPath + Path.GetFileName(filename)) == false)
                        {
                            ErrorMessage = "Only .doc .xls .pdf Image and Zip File allowed";
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
                                                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                                if (MimeType.GetMimeType(bytes, filePath) == false)
                                                {
                                                    ErrorMessage = "(" + item.FullName + ") file is incorrect format. Please provide Currect file";
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
                    //var bytes = System.IO.File.ReadAllBytes(TempPath + Path.GetFileName(filename));


                }
            }




            HttpResponseMessage response12 = await _documentService.GetDocumentAsync(filename, "D:/wwwroot2/Uploads/");


            if (response12.IsSuccessStatusCode)
            {
                var jsonString = await response12.Content.ReadAsStringAsync();
                var obj12 = JsonConvert.DeserializeObject<DocumentResponse>(jsonString);

                string docName = obj12.DocumentName;
                string base64Data = obj12.DocumentValue;
                await _documentService.DeleteDocumentAsync(filename, "D:/wwwroot2/Uploads/");
            }



            FileInfo file1 = new FileInfo(TempPath + Path.GetFileName(filename));
            if (file1.Exists)
            {
                file1.Delete();
            }
            if (Directory.Exists(StartupPath))
            {
                Directory.Delete(StartupPath, true);
            }
            return ErrorMessage;
        }


        [HttpPost]
        [SessionExpireFilterAttributeClient]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public async Task<ActionResult> AddCasesByBulkUpload(ClientByBulkUploadViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Status = 1;
                    if (model.ClientRowID > 0)
                    {
                        #region File Upload

                        string FldPathDoc = ConfigurationManager.AppSettings["ClientAndCompanydoc"].ToString();
                        string errorMessage = await GatZipFileMessage(model.IdentityDoc, model.ClientRowID.ToString(), "IdentityDoc");
                        if (errorMessage != "")
                        {
                            ViewBag.ErrorMsg += errorMessage;
                            model.ClientRowID = Convert.ToInt16(Session["ClientRowID"]);                           
                            model.CapImage = model.CapImageText;
                            model.CapImageText = model.CapImage;                          
                            return View(model);
                        }

                        if (model.IdentityExcel != null && model.IdentityExcel.FileName.ToString() != "" && model.IdentityExcel.ContentLength > 0)
                        {
                            string userId = User.Identity.Name;
                            string ip = Request.UserHostAddress;

                            if (Helpers.RateLimit.IsLimitExceeded($"AddCasesByBulkUpload_{userId}", 30, 5))
                            {
                                ViewBag.ErrorMsg = "User can upload 30 file within 5 min";
                                model.ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                                // CaptchaModel obj = new CaptchaModel();
                                model.CapImage = model.CapImageText;
                                model.CapImageText = model.CapImage;
                                //model.CaptchaCodeText = "";
                                return View(model);
                            }

                            model.ExcelFileName = Path.GetFileName(model.IdentityExcel.FileName).ToLower();
                            string ext = Path.GetExtension(model.ExcelFileName).ToLower();
                            string proImg = (model.ClientRowID).ToString()+"_"+ DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss").Replace(" ", "_").Replace("-", "_") + ext;
                            //model.ExcelFileName = Path.GetFileName(model.IdentityExcel.FileName).ToLower();
                            //model.ExcelFileName = model.ExcelFileName + "" + model.ClientRowID + "-" + DateTime.Now.Date.ToString("dd-MM-yyyy-hh-mm".ToString());
                            int count = model.IdentityExcel.FileName.Split('.').Length - 1;
                            if (count > 1)
                            {
                                ViewBag.ErrorMsg = "please check file name";
                                model.ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                                // CaptchaModel obj = new CaptchaModel();
                                model.CapImage = model.CapImageText;
                                model.CapImageText = model.CapImage;
                                //model.CaptchaCodeText = "";
                                return View(model);

                            }
                            model.ExcelFileName = proImg;

                            //==============================================

                            string base64 = string.Empty;
                            model.IdentityExcel.InputStream.Position = 0;
                            using (var memoryStream = new MemoryStream())
                            {
                                model.IdentityExcel.InputStream.CopyTo(memoryStream); // Convert the byte array to Base64 
                                base64 = Convert.ToBase64String(memoryStream.ToArray());
                            }
                            _documentService.UploadDynamicCandidateFiles(proImg, base64, "D:/wwwroot2/Documents/ClientDocuments/UploadedExcelAndDocsForCompany/");
                            //==============================================

                           // model.IdentityExcel.SaveAs(Server.MapPath(FldPathDoc + proImg));

                            if (model.IdentityDoc != null && model.IdentityDoc.FileName.ToString() != "" && model.IdentityDoc.ContentLength > 0)
                            {
                                model.AttachDocName = Path.GetFileName(model.IdentityDoc.FileName).ToLower();
                                string docext = Path.GetExtension(model.AttachDocName).ToLower();
                                string DocproImg = (model.ClientRowID).ToString() + "_" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss").Replace(" ", "_").Replace("-", "_") + docext;
                                int countdoc = model.IdentityDoc.FileName.Split('.').Length - 1;
                                string fileName = Path.GetFileName(model.IdentityDoc.FileName);

                                string[] parts = fileName.Split('.');

                                if (parts.Length > 2)
                                {
                                    model.ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                                    // CaptchaModel obj = new CaptchaModel();
                                    model.CapImage = model.CapImageText;
                                    model.CapImageText = model.CapImage;
                                    //model.CaptchaCodeText = "";
                                    ViewBag.ErrorMsg = "Files with multiple extensions are not allowed.";
                                    return View(model);
                                }
                                if (countdoc > 1)
                                {
                                    ViewBag.ErrorMsg = "please check file name";
                                    model.ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                                    // CaptchaModel obj = new CaptchaModel();
                                    model.CapImage = model.CapImageText;
                                    model.CapImageText = model.CapImage;
                                    return View(model);

                                }
                                model.AttachDocName = DocproImg;
                                //==============================================

                                string base641 = string.Empty;
                                model.IdentityDoc.InputStream.Position = 0;
                                using (var memoryStream = new MemoryStream())
                                {
                                    model.IdentityDoc.InputStream.CopyTo(memoryStream); // Convert the byte array to Base64 
                                    base641 = Convert.ToBase64String(memoryStream.ToArray());
                                }
                                _documentService.UploadDynamicCandidateFiles(DocproImg, base641, "D:/wwwroot2/Documents/ClientDocuments/UploadedExcelAndDocsForCompany/");
                                //model.IdentityDoc.SaveAs(Server.MapPath(FldPathDoc + DocproImg));

                                if (!string.IsNullOrEmpty(Convert.ToString(Session["ClientBranch"])))
                                {
                                    model.UploadedBy =  Convert.ToString(Session["UserName"]) + "(" + Convert.ToString(Session["ClientBranch"]) +")";
                                }
                                else
                                {
                                    model.UploadedBy = Convert.ToString(Session["UserName"]);
                                }

                                model.UploadedDate = DateTime.Now;

                                repoClientCmpUplod.AddPQClientCompnayUploadDoc(model);
                                int Res= repoClientCmpUplod.SaveChanges();
                                if (Res>0)
                                {                         
                                       TempData["SuccessMsg"] = "Record Added Successfully!";
                                       SendMail(model.ClientRowID, Convert.ToInt32(Session["ClientUserRowID"]));
                                    ModelState.Clear();
                                    return RedirectToAction("ManageClientCompany");
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = "There is some problem to save record. Please try again later!";
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "Upload Document First!";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Upload Excel File First!";
                        }
                        #endregion                        
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invalid ClientID!";
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
        public List<BatchFileList> BindBatchFileList(int ClientRowId)
        {
            List<BatchFileList> lstobj = new List<BatchFileList>();
            var tdlst=cc.GetBatchFileList(ClientRowId);
            if(tdlst.Rows.Count>0)
            {

                lstobj = (from DataRow dr in tdlst.Rows
                          select new BatchFileList()
                          {
                              BatchFileId = Convert.ToString(dr["CreatedBySource"]),
                              BatchFileName = Convert.ToString(dr["CreatedBySource"])
                          }).ToList();

            }

            return lstobj;
        }


        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public ActionResult AddCasesByBulkUploadWithCheckProcess()
        {
            AddPQPersonalBulkImportViewModel model = new AddPQPersonalBulkImportViewModel();
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                model.ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                int ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                string FileName = "CandidateUploadTemplate.xlsx";
                string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["clientCanidatetemplate"].ToString();
                model.Template = Url.Content(FldPathDoc + FileName);
                ViewBag.CaseList = null;
                ViewBag.BatchFileList = BindBatchFileList(model.ClientRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [HttpPost]
        [SessionExpireFilterAttributeClient]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public ActionResult AddCasesByBulkUploadWithCheckProcess(AddPQPersonalBulkImportViewModel model,string Action="")
        {
            try
            {
                List<CandidateCaseDetailsWithSoruce> lstonj = new List<CandidateCaseDetailsWithSoruce>();
                if (Action == "submit")
                {
                    string BatchFileName = "";
                    if (model.ClientRowID > 0 && !string.IsNullOrEmpty(model.CandidateCheckOrPackage) && model.CreatedBySource != "")
                    {
                        int fileNo = repoPQClientCandiBulkUpload.GetNoOfExcelFileByClientID(model.ClientRowID);
                        var ClientDetails = repoPQClient.GetPQClientForCadiCodeGenById(model.ClientRowID);
                        if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                        {
                            fileNo++;
                            string Extension = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                            string FileName = (ClientDetails.ClientName + "_" + fileNo.ToString()).Replace(" ", "_").Replace("-", "_").Replace("/", "_") + Extension;
                            string FolderPath = ConfigurationManager.AppSettings["clientCanidatetemplate"];
                            string FilePath = Server.MapPath(FolderPath + FileName);
                            model.UploadDoc.SaveAs(Server.MapPath(FolderPath + FileName));

                            int res = SaveClientCandidateUploadFileWithChecks(model.ClientRowID, FileName, model.CandidateCheckOrPackage);
                            string TicketNo = model.ClientRowID + "-" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "-" + fileNo;
                            if (res > 0)
                            {
                                BatchFileName = model.CreatedBySource + "_" + DateTime.Now.ToString("yyyy-MMM-dd")+"_"+ TicketNo;
                                ImportExcelAndSaveCandidateByBulkUpload(FilePath, Extension, "Yes", model, TicketNo, BatchFileName);
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Please Select Excel Document!";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.CandidateCheckOrPackage))
                        {
                            ViewBag.ErrorMsg = "Please select Check or Package!";
                        }
                        else if (model.CreatedBySource != "")
                        {
                            ViewBag.ErrorMsg = "Please Enter Batch File Name!";
                        }
                    }
                    if (ViewBag.ErrorMsg == "")
                    {
                        var tbldt = cc.getClientCaseListBySource(model.ClientRowID, BatchFileName);
                        if (tbldt.Rows.Count > 0)
                        {
                            lstonj = (from DataRow dr in tbldt.Rows
                                      select new CandidateCaseDetailsWithSoruce()
                                      {
                                          PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                                          CompanyRefNo = Convert.ToString(dr["CompanyRefNo"]),
                                          CandidateName = Convert.ToString(dr["CandidateFullName"]),
                                          ClientRefNo = Convert.ToString(dr["ClientRefNo"]),
                                          BatchFilename = Convert.ToString(dr["BatchFilename"]),
                                          IsReportUploaded = Convert.ToString(dr["IsReportUploaded"]),
                                          ReportUploadedName = Convert.ToString(dr["ReportUploadedName"]),
                                          ReportUploadedDate = Convert.ToString(dr["ReportUploadedDate"])
                                      }).ToList();
                        }
                        ViewBag.CaseList = lstonj;
                    }
                }
                else if (Action == "Report Download")
                {
                    if (model.BatchFileName != "")
                    {
                        var tbldt = cc.getClientCaseListBySource(model.ClientRowID, model.BatchFileName, 1);
                        if (tbldt.Rows.Count > 0)
                        {
                            List<ClientBulkDownlode> f = new List<ClientBulkDownlode>();
                            string CandidateFullName = "";

                            for (int i = 0; i < tbldt.Rows.Count; i++)
                            {
                                string CompanyRefNo = Convert.ToString(tbldt.Rows[i]["CompanyRefNo"]);
                                string reportFileName = CompanyRefNo + ".pdf";
                            
                                        //reportFileName = ddd.Rows[0]["ReportFileName"].ToString();
                                        CandidateFullName = tbldt.Rows[i]["CandidateFullName"].ToString();
                                    
                                ClientBulkDownlode tt = new ClientBulkDownlode();
                                string fileName = CompanyRefNo;
                                if (!string.IsNullOrEmpty(CompanyRefNo))
                                {
                                    string FldPathDoc = "";
                                    string fname = "";
                                    FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
                                    fname = Server.MapPath(FldPathDoc) + reportFileName;

                                    FileInfo fi1 = new FileInfo(fname);
                                    tt.fileDetails = fi1;
                                    tt.FileDownloadName = repoDashboardClient.downloadReportFileName(CompanyRefNo) + ".pdf";
                                    f.Add(tt);
                                }

                            }

                            using (var memoryStream = new MemoryStream())
                            {
                                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                                {
                                    foreach (ClientBulkDownlode fo in f)
                                    {
                                        try
                                        {
                                            ziparchive.CreateEntryFromFile(fo.fileDetails.FullName, fo.FileDownloadName);
                                        }
                                        catch (Exception Ex)
                                        {
                                            ViewBag.ErrorMsg = "Invalid paramerter value";
                                            clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                                        }
                                    }
                                }
                                return File(memoryStream.ToArray(), "application/zip", model.BatchFileName + ".zip");
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Report Not Found";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Please Enter Batch File Name!";
                    }

                }
                else if (Action == "Bulk Process")
                {
                    if (model.BatchFileName != "")
                    {
                        var tes = BulkFaildWorldCheckCaseProcess(model.ClientRowID, model.BatchFileName);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Please select Batch Name!";
                    }

                }
                else
                {

                    if (model.BatchFileName != "")
                    {

                        var tbldt = cc.getClientCaseListBySource(model.ClientRowID, model.BatchFileName);
                        if (tbldt.Rows.Count > 0)
                        {
                            lstonj = (from DataRow dr in tbldt.Rows
                                      select new CandidateCaseDetailsWithSoruce()
                                      {
                                          PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                                          CompanyRefNo = Convert.ToString(dr["CompanyRefNo"]),
                                          CandidateName = Convert.ToString(dr["CandidateFullName"]),
                                          ClientRefNo = Convert.ToString(dr["ClientRefID"]),
                                          BatchFilename = Convert.ToString(dr["CreatedBySource"]),
                                          IsReportUploaded = Convert.ToString(dr["IsReportUploaded"]),
                                          ReportUploadedName = Convert.ToString(dr["ReportUploadedName"]),
                                          ReportUploadedDate = Convert.ToString(dr["ReportUploadedDate"])
                                      }).ToList();
                        }
                        ViewBag.CaseList = lstonj;
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Please Enter Batch File Name!";
                    }
                }
                ViewBag.BatchFileList = BindBatchFileList(model.ClientRowID);
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        public int BulkFaildWorldCheckCaseProcess(int ClientRowId,string BatchName)
        {           
            int Count = 0;
            try
            {
                ClientCustomize CC = new ClientCustomize();

                var ExecuteSQLQueryDb = CC.GetWorldCheckNotProcessCaseBySource(ClientRowId, BatchName);
                if (ExecuteSQLQueryDb.Rows.Count > 0)
                {
                    for (int i = 0; i < ExecuteSQLQueryDb.Rows.Count; i++)
                    {
                        int personalRowId = Convert.ToInt32(ExecuteSQLQueryDb.Rows[i]["PersonalRowId"]);
                        int SId = Convert.ToInt32(ExecuteSQLQueryDb.Rows[i]["SubCheckRowId"]);
                        var tblCase = CC.GetWorldCheckscreeningRequest(personalRowId,SId);
                        string ResultJson = Convert.ToString(tblCase.Rows[0]["JsonResult"]);
                        string CaseId = Convert.ToString(tblCase.Rows[0]["CaseId"]);
                        refinitivRootobject myDeserializedClass = JsonConvert.DeserializeObject<refinitivRootobject>(ResultJson);
                        string CaseSystemId = myDeserializedClass.caseSystemId;
                        string CompanyRefNo = Convert.ToString(ExecuteSQLQueryDb.Rows[i]["CompanyRefNo"]);
                        string reportId = WorldChecksRequestAsyncReport(CaseId, "NoMatchFound");
                        if (reportId != "")
                        {
                            var ReportStatus = WorldChecksGetReportStatus(reportId);
                            if (ReportStatus != "COMPLETED")
                            {
                                Thread.Sleep(5000);
                                ReportStatus = WorldChecksGetReportStatus(reportId);
                            }
                            if (ReportStatus == "COMPLETED")
                            {
                                string Reportname = WorldChecksDownloadReport(reportId, personalRowId, SId, CaseId, ClientRowId, CompanyRefNo);
                                if (Reportname != "")
                                {
                                    cc.UpdatePQCandidateChecksColorCode(905, 6, "Green", personalRowId, SId);
                                    cc.UpdateCheckStatus(personalRowId, Convert.ToInt16(SId), 1, 37, "Check", Convert.ToInt32(Session["ClientUserRowID"].ToString()));
                                    if (cc.GetPQCandidateCheckList(personalRowId).Rows.Count > 1)
                                    {
                                        cc.UploadReportDetails(personalRowId, CompanyRefNo + ".pdf", "", "Completed");
                                        cc.finalReportDetail(personalRowId, Convert.ToInt32(Session["ClientUserRowID"].ToString()), "Final", "Green", CompanyRefNo + ".pdf", 1);
                                    }
                                    else
                                    {
                                        cc.UploadReportDetails(personalRowId, CompanyRefNo + ".pdf", "", "Completed");
                                        cc.finalReportDetail(personalRowId, Convert.ToInt32(Session["ClientUserRowID"].ToString()), "Final", "Green", CompanyRefNo + ".pdf", 1);
                                    }
                                    Count = Count + 1;

                                }
                               
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                return Count;
            }
            return Count;
        }
        private int SaveClientCandidateUploadFileWithChecks(short ClientRowID = 0, string FileName = "", string CandidateCheckOrPackage = "")
        {
            try
            {
                int res = 0;
                AddPQClientCandiBulkUploadViewModel objModel = new AddPQClientCandiBulkUploadViewModel();
                objModel.ClientRowID = ClientRowID;
                objModel.ExcelFileName = FileName;
                objModel.ImportBy = "Company";
                objModel.Status = 1;
                string[] strChkPkg = CandidateCheckOrPackage.Split(',');
                foreach (var ChkPkg in strChkPkg)
                {
                    string[] strIds = ChkPkg.Split('-');
                    if (!string.IsNullOrEmpty(strIds[0]))
                    {
                        if (strIds[0] == "Check")
                        {
                            var ClientCheckRowID = Convert.ToInt16(strIds[1]);
                            var ChekData = repoClientCheck.GetChecksByClientId(ClientRowID).Where(p => p.ClientCheckRowID == ClientCheckRowID);
                            objModel.ClientPackageRowID = 0;
                            foreach (var item in ChekData)
                            {
                                objModel.ClientCheckRowID = item.ClientCheckRowID;
                                repoPQClientCandiBulkUpload.AddPQClientCandiBulkUpload(objModel);
                                res += repoPQClientCandiBulkUpload.SaveChanges();
                            }
                        }
                        if (strIds[0] == "Package")
                        {
                            string pkgnm = ChkPkg.Replace(strIds[0] + "-", "");
                            var pkgDetails = repoClientPackage.GetPackageDetailsByPackageName(ClientRowID.ToString(), pkgnm);
                            //var pkgDetails = repoClientPackage.GetPackageDetailsByPackageName(strIds[1]).Where(p => p.AntecedentSelected == 1);
                            objModel.ClientCheckRowID = 0;
                            foreach (var item in pkgDetails)
                            {
                                objModel.ClientPackageRowID = item.ClientPackageRowID;
                                repoPQClientCandiBulkUpload.AddPQClientCandiBulkUpload(objModel);
                                res += repoPQClientCandiBulkUpload.SaveChanges();
                            }
                        }
                    }
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ImportExcelAndSaveCandidateByBulkUpload(string FilePath, string Extension, string isHDR, AddPQPersonalBulkImportViewModel model, string TicketNo,string CreatedBySource="",string EntityType= "INDIVIDUAL") //isHDR - means Include header Yes or No
        {
            try
            {                
                ClientCustomize cc = new ClientCustomize();
                string conStr = "";
                switch (Extension)
                {
                    case ".xls": //Excel 97-03
                        conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07
                        conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }
                conStr = String.Format(conStr, FilePath, isHDR);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                DataTable dt = new DataTable();

                cmdExcel.Connection = connExcel;
             
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();             
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();

               
                if (dt.Rows.Count > 0)
                {
                   
                    int countSuccess = 0;
                    int countDuplicate = 0;
                    string Email = string.Empty;
                    string MobileNo = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Email = Convert.ToString(dt.Rows[i]["Email"]);

                        model.EmailID = Email;
                        MobileNo = Convert.ToString(dt.Rows[i]["Mobile"]);
                        if (MobileNo.Length > 10)
                        {
                            if (MobileNo.Length == 12)
                            {
                                model.MobileNo = MobileNo.Remove(0, 2);
                            }
                            else if (MobileNo.Length == 11)
                            {
                                model.MobileNo = MobileNo.Remove(0, 1);
                            }
                        }
                        model.MobileNo = MobileNo;
                        model.Name = dt.Rows[i]["Name"].ToString();
                        model.VoterIDCardNo = dt.Rows[i]["VoterIDCardNo"].ToString();
                        model.DrivingLicenseNo = dt.Rows[i]["DrivingLicenseNo"].ToString();
                        model.AadhaarCardNo = dt.Rows[i]["AadhaarCardNo"].ToString();
                        model.PassportNo = dt.Rows[i]["PassportNo"].ToString();
                        model.PanCardNo = dt.Rows[i]["PanCardNo"].ToString();
                        model.FatherName = dt.Rows[i]["FatherName"].ToString();
                        model.MiddleName = dt.Rows[i]["MiddleName"].ToString();
                        model.LastName = dt.Rows[i]["LastName"].ToString();
                        if (dt.Rows[i]["DOB"].ToString() != "")
                            model.DOB = Convert.ToDateTime(dt.Rows[i]["DOB"].ToString());
                        else
                            model.DOB = null;

                        if (!string.IsNullOrEmpty(model.Name))
                        {
                            
                            model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                            if (!repoPQPersonal.IsCandidateExist(model.ClientRowID, model.VoterIDCardNo, model.DrivingLicenseNo, model.AadhaarCardNo, model.PassportNo, model.PanCardNo))
                            {
                                model.MiddleName = dt.Rows[i]["MiddleName"].ToString();
                                model.LastName = dt.Rows[i]["LastName"].ToString();
                                model.Gender = dt.Rows[i]["Gender"].ToString();
                                if (string.IsNullOrEmpty(model.Gender) || model.Gender.ToLower() == "male")
                                {
                                    model.Gender = "male";
                                    model.CandTitle = "Mr.";
                                }
                                else
                                {
                                    model.CandTitle = "Ms.";
                                }
                                model.EmployeeID = dt.Rows[i]["EmployeID"].ToString();
                                model.ClientRefID = dt.Rows[i]["ClientRefID"].ToString();
                                model.CandidateCode = GenrateCandidateCode(model.ClientRowID);
                                model.CompanyRefNo = GetCompanyRefNo(model.ClientRowID);
                                model.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                                model.CreatedDate = DateTime.Now;
                                model.FromType = "Company Cases";
                                model.DetailsEnteredBy = "Company";
                                model.CaseStatus = "Allocated";
                                model.Status = 1;
                                model.ClientUserName = dt.Rows[i]["ClientUserName"].ToString();                                
                                model.PlaceOfJoining = dt.Rows[i]["PlaceOfJoining"].ToString();
                                model.AlternateNo = dt.Rows[i]["AlternateNo"].ToString();
                                model.ProcessName = dt.Rows[i]["ProcessName"].ToString();
                                model.ProjectId = dt.Rows[i]["ProjectId"].ToString();
                                model.ProjectName = dt.Rows[i]["ProjectName"].ToString();
                                
                                    model.Grade = "";
                               
                                model.UAN = dt.Rows[i]["UAN"].ToString();                             

                                    model.PID = "";
                                    model.SDO_Code = "";
                                    model.COE_Code = "";
                                    model.UID = "";
                                    model.OHR_Taleo_ID = "";
                                    model.Band = "";
                                    model.Designation_Genpact = "";
                                
                                model.AlternateEmailID = dt.Rows[i]["AlternateEmail"].ToString();
                                model.Entity = dt.Rows[i]["Entity"].ToString();
                                model.CandidateType = dt.Rows[i]["CandidateType"].ToString();
                                model.Location = "";

                                model.CreatedBySource = CreatedBySource;
                                PQClientCodeHolidaysViewModel client = repoPQClient.GetPQClientCodeHolidaysById(model.ClientRowID);
                                model.TAT = repoPQPersonal.GetClientTATByCandidateId(model.ClientRowID);
                                model.CaseType = "Normal";

                                // OrderDate And DeliveryDate
                                DateTime oDate = DateTime.Now;
                                model.OrderDate = oDate;
                                model.DeliveryDate = GetDeliveryDate(oDate, model.TAT, client);
                                model.ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                               
                                //// Check Duplicate Name
                                model.CandidateDuplicateList = repoPQPersonal.CheckCandidateduplicateNameFromDETL(model.ClientRowID, model.Name, model.MiddleName, model.LastName, "", model.MobileNo,model.EmployeeID);
                                if (model.CandidateDuplicateList.Count() > 0)
                                {
                                    cc.Insert_DuplicateReferenceDetail(TicketNo, model.Name, model.MiddleName, model.LastName, model.MobileNo, model.CreatedBy.ToString(), model.EmailID, true, model.ClientRowID.ToString(), model.FatherName, CreatedBySource, model.DOB);

                                }
                                else
                                {
                                    cc.Insert_DuplicateReferenceDetail(TicketNo, model.Name, model.MiddleName, model.LastName, model.MobileNo, model.CreatedBy.ToString(), model.EmailID, false, model.ClientRowID.ToString(), model.FatherName, CreatedBySource, model.DOB);
                                }
                                int Res = 0;

                                if (model.CandidateDuplicateList.Count() == 0)
                                {
                                    Res = repoPQPersonal.AddCandidate(model);
                                    model.PersonalRowID = Res;                                   
                                }
                                else
                                {
                                    Res = -1;
                                }
                                if (Res == 0)
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + " Mobile no and email id should not be duplicate";
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = "Reference Generated Successfully";
                                }

                                if (Res > 0)
                                {
                                    #region Save LogTransaction Details 
                                    clsCommonMethods.AddPQLogTrasactionByClient(model.PersonalRowID, "AddCasesByBulkUploadWithCheckProcess", "Candidate Reference Generated", "Input", "Insert");
                                    #endregion

                                    #region Case/Check Action History Save 
                                    clsCommonMethods.AddCaseActionHistoryByClient(model.PersonalRowID, "Allocated-TL", "Securitas Id Generated by Import: " + model.CompanyRefNo);
                                    #endregion

                                    SavePackageOrCheckWithCandidateAsBulkImport(model);
                                    cc.UpdateCaseStatus(model.PersonalRowID, 6, 0);
                                    if(model.ClientRowID==1638)
                                    {
                                        string CandidateName = model.Name + " " + model.MiddleName + " " + model.LastName;
                                        if (EntityType == "INDIVIDUAL")
                                        {
                                            UploadWorldCheckReport(model.PersonalRowID, 201, CandidateName,model.CompanyRefNo, model.Location, model.Gender,Convert.ToString(model.DOB),Convert.ToString(model.ClientRefID));
                                        }
                                        else
                                        {
                                            UploadWorldCheckReport(model.PersonalRowID, 201, CandidateName, model.CompanyRefNo, model.Location, model.Gender, Convert.ToString(model.DOB), Convert.ToString(model.ClientRefID));
                                        }                                           
                                    }                                    
                                    countSuccess++;
                                }
                            }
                            else
                            {
                                cc.Insert_DuplicateReferenceDetail(TicketNo, model.Name, model.MiddleName, model.LastName, model.MobileNo, model.CreatedBy.ToString(), model.EmailID, true, model.ClientRowID.ToString(), model.FatherName, CreatedBySource, model.DOB);
                                countDuplicate++;
                            }
                        }
                    }

                    TempData["SuccessMsg"] = countSuccess.ToString() + " Candidate(s) added successfully!";
                    if (countDuplicate > 0)
                    {
                        TempData["ErrorMsg"] = countDuplicate.ToString() + " Candidate(s) already exist!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "No candidate found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        public string getMonth(string month)
        {
            string stringMonth = "";
            if (month == "01")
            {
                stringMonth = "Jan";
            }
            else if (month == "02")
            {
                stringMonth = "Feb";
            }
            else if (month == "03")
            {
                stringMonth = "Mar";
            }
            else if (month == "04")
            {
                stringMonth = "Apr";
            }
            else if (month == "05")
            {
                stringMonth = "May";
            }
            else if (month == "06")
            {
                stringMonth = "Jun";
            }
            else if (month == "07")
            {
                stringMonth = "Jul";
            }
            else if (month == "08")
            {
                stringMonth = "Aug";
            }
            else if (month == "09")
            {
                stringMonth = "Sep";
            }
            else if (month == "10")
            {
                stringMonth = "Oct";
            }
            else if (month == "11")
            {
                stringMonth = "Nov";
            }
            else if (month == "12")
            {
                stringMonth = "Dec";
            }
            return stringMonth;
        }

        public void UploadWorldCheckReport(int PersonalRowid, int SubCheckid, string CandidateName,string CompanyRefNo,string Country,string Gender,string DOB,string ClientRefID)
        {
          
            try
            {
                try
                {
                    var Checkdt = cc.GetPQCandidateCheckList(PersonalRowid);
                    DataTable dt;
                    int ClientRowID = 0;
                    if (SubCheckid > 0)
                    {
                        ClientRowID = Convert.ToInt32(Checkdt.Rows[0]["ClientRowID"]);
                        string CaseId = "";
                        var Time_stamp = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUnixTimeSeconds();
                        var LocationDt = cc.getWolrdCheckNationalities(Country);
                        string Location = "";
                        if (LocationDt.Rows.Count > 0)
                        {
                            Location = Convert.ToString(LocationDt.Rows[0]["NationalitiesCode"]);
                        }
                        var DOB_ = Convert.ToDateTime(DOB).ToString("dd-MM-yyyy").Split('-');
                        string Month = getMonth(DOB_[1]);
                        string DoB = DOB_[0] + "-" + Month + "-" + DOB_[2];

                        if (Gender is null)
                        {
                            Gender = "";
                        }
                        if (ClientRefID == " " || ClientRefID == "" || ClientRefID == "-" || ClientRefID == "--")
                        {
                            CaseId = CompanyRefNo.Replace(" ", "").Trim() + "_" + Time_stamp.ToString();
                        }
                        else
                        {
                            CaseId = ClientRefID.Replace(" ", "").Trim() + "_" + Time_stamp.ToString();
                        }
                        CaseId = CompanyRefNo.Replace(" ", "").Trim() + "_" + Time_stamp.ToString();
                        var ResultJson = WorldCheckscreeningRequest(CaseId, PersonalRowid, SubCheckid, CandidateName, DoB, Convert.ToString(Gender), Location);

                        if (Convert.ToString(ResultJson) != "")
                        {
                            refinitivRootobject myDeserializedClass = JsonConvert.DeserializeObject<refinitivRootobject>(ResultJson);

                            if (myDeserializedClass.results.Length > 0)
                            {
                                int cnt = 1;
                                foreach (var item in myDeserializedClass.results)
                                {
                                    cc.InsertWorldCheckResultIds(PersonalRowid, SubCheckid, item.resultId, item.referenceId,cnt);
                                    cnt = cnt + 1;
                                }
                            }
                            if (myDeserializedClass.results.Length > 0)
                            {

                                dt = cc.Antecedents_client_select_form(ClientRowID, SubCheckid, "Operation", "Check");
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                        int rst = cc.InsertUpdateWorldChecksDetails(PersonalRowid, SubCheckid, Antecedents, ClientRowID, "Record found");
                                    }
                                }
                                cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 115, "Check", Convert.ToInt32(Session["ClientUserRowID"].ToString()));
                                clsCommonMethods.AddCaseActionHistoryByClient(PersonalRowid, "Allocated-TL", "Report Verifeid By World Check API");
                                cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                            }
                            else
                            {
                                CaseId = myDeserializedClass.caseId;
                                var reportresult = WorldChecksRequestAsyncReport(CaseId, "NoMatchFound");
                                if (reportresult != "")
                                {
                                    dt = cc.Antecedents_client_select_form(ClientRowID, SubCheckid, "Operation", "Check");
                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                            int rst = cc.InsertUpdateWorldChecksDetails(PersonalRowid, SubCheckid, Antecedents, ClientRowID, "No record found");
                                        }
                                    }
                                    var stastus = WorldChecksGetReportStatus(reportresult);
                                    if (stastus == "COMPLETED")
                                    {
                                        var reportname = WorldChecksDownloadReport(reportresult, PersonalRowid, SubCheckid, CaseId, ClientRowID, CaseId);
                                        if (reportname != "")
                                        {                                       
                                            cc.UpdatePQCandidateChecksColorCode(905, 6, "Green", PersonalRowid, SubCheckid, "No Match Found");
                                            cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 37, "Check", Convert.ToInt32(Session["ClientUserRowID"].ToString()));
                                            cc.UploadReportDetails(PersonalRowid, CompanyRefNo + ".pdf", "", "Completed");
                                            cc.finalReportDetail(PersonalRowid, Convert.ToInt32(Session["ClientUserRowID"]), "Final", "Green", CompanyRefNo + ".pdf", 1);
                                            clsCommonMethods.AddCaseActionHistoryByClient(PersonalRowid, "Allocated-TL", "Report Verifeid By World Check API");
                                            cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                        }
                                    }
                                    else
                                    {
                                        cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 115, "Check", Convert.ToInt32(Session["ClientUserRowID"].ToString()));                                   
                                        clsCommonMethods.AddCaseActionHistoryByClient(PersonalRowid, "Allocated-TL", "Report Verifeid By World Check API" );
                                        cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                    }

                                }
                                else
                                {
                                    cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 115, "Check", Convert.ToInt32(Session["ClientUserRowID"].ToString()));
                                    clsCommonMethods.AddCaseActionHistoryByClient(PersonalRowid, "Allocated-TL", "Report Verifeid By World Check API");
                                    cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                }
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

                }

            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

            }
        }
        private static readonly string WorldCheckGroupId = System.Configuration.ConfigurationManager.AppSettings["WorldCheckGroupId"].ToString();
        public string WorldCheckscreeningRequest(string CompanyRefNo, int PersonalRowId, int SubCheckRowId, string CandidateName, string DOB, string Gender, string Location)
        {
            string ResultJson = "";
            try
            {
                DateTime Date = Convert.ToDateTime(DOB).AddHours(13).AddMinutes(30);
                var Timestamp = new DateTimeOffset(Date).ToUnixTimeMilliseconds();

                string CaseId = CompanyRefNo.ToString();
                string Rbody = "{\n    \"caseId\": \"" + CaseId + "\",\n  " +
                    "  \"groupId\": \"" + WorldCheckGroupId + "\",\n  " +
                    "  \"entityType\": \"INDIVIDUAL\",\n   " +
                    " \"providerTypes\": [\n        \"WATCHLIST\"\n    ],\n   " +
                    " \"caseScreeningState\": {\n        \"WATCHLIST\": \"INITIAL\"\n    },\n " +
                    "   \"name\": \"" + CandidateName + "\",\n   " +
                    " \"nameTransposition\": false,\n   " +
                    " \"secondaryFields\": [\n        {\n            \"typeId\": \"SFCT_1\",\n            \"value\": \"" + Gender.ToUpper() + "\"\n        },\n   " +
                    "{\n\"typeId\":\"SFCT_2\", \n \"dateTimeValue\": {\n\"timelinePrecision\": \"ON\",\n \"pointInTimePrecision\": \"DAY\",\n\"utcDateTime\": " + Timestamp + ",\n \"timeZone\": null\n}\n},\n  " +
                    "{\n\"typeId\": \"SFCT_3\",\n\"value\": \"" + Location + "\"\n},\n   " +
                    "{\n\"typeId\": \"SFCT_4\",\n\"value\": \"" + Location + "\"\n} ]\n}";
                var JsonRequest = JObject.Parse(Rbody).ToString();
                string Url = "https://api.risk.lseg.com/screening/v2/cases/screeningRequest";
                refinitivAPI refinitivAPI = new refinitivAPI();
                ResultJson = refinitivAPI.PostRequest(Url, JsonRequest, Server.MapPath("~\\APIErrorLogs\\Logfiles"));
                if (ResultJson != "")
                {
                    cc.insertWorldCheckscreeningRequest(PersonalRowId, SubCheckRowId, ResultJson, CaseId);
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\APIErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

            }


            return ResultJson;
        }

        public string WorldChecksRequestAsyncReport(string Caseid, string ReportStatus)
        {
            try
            {
                string Requestquery = "{\"query\": \"caseId==" + Caseid + "\",\n  " +
                "  \"filter\": \"\",\n    \"reportType\": \"CASE_DOSSIER\",\n  " +
                "  \"sort\": [\n        {\n            \"columnName\": \"caseId\",\n     " +
                "       \"order\": \"ASCENDING\"\n        }\n    ],\n  " +
                "  \"reportName\": \"" + Caseid + "\",\n   " +
                "  \"reportFilter\": \"(includeUnresolvedMatches==true; includePositiveMatches==true; includePossibleMatches==true;includeFalseMatches==true;includeUnspecifiedMatches==true;providerType==WATCHLIST),(includeAudit==true)\",\n  " +
                "  \"customAttributes\": \"note==" + ReportStatus + "\"\n}";

                string ResultJson = "";
                string Url = "https://api.risk.lseg.com/screening/v2/reports";
                refinitivAPI refinitivAPI = new refinitivAPI();
                ResultJson = refinitivAPI.PostRequest(Url, Requestquery, Server.MapPath("~\\APIErrorLogs\\Logfiles"));
                if (ResultJson != "")
                {
                    ReportRoot myDeserializedClass = JsonConvert.DeserializeObject<ReportRoot>(ResultJson);
                    return myDeserializedClass.reportId;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";

            }

        }
        public string WorldChecksGetReportStatus(string reportId)
        {
            string ResultJson = "";
            string progressStatus = "Under progress";
            try
            {
                string Url = "https://api.risk.lseg.com/screening/v2/reports/" + reportId + "/status";
                refinitivAPI refinitivAPI = new refinitivAPI();
                ResultJson = refinitivAPI.GetRequest(Url, "", Server.MapPath("~\\APIErrorLogs\\Logfiles"));


                reportstatusRoot myDeserializedClass = JsonConvert.DeserializeObject<reportstatusRoot>(ResultJson);
                if (myDeserializedClass.progressStatus == "COMPLETED")
                {
                    progressStatus = myDeserializedClass.progressStatus;
                }
                else
                {
                    Thread.Sleep(3000);
                    ResultJson = refinitivAPI.GetRequest(Url, "", Server.MapPath("~\\APIErrorLogs\\Logfiles"));
                    myDeserializedClass = JsonConvert.DeserializeObject<reportstatusRoot>(ResultJson);
                    if (myDeserializedClass.progressStatus == "COMPLETED")
                    {
                        progressStatus = myDeserializedClass.progressStatus;
                    }
                }
            }
            catch (Exception ex)
            {


            }

            return progressStatus;



        }

        public string WorldChecksDownloadReport(string reportId, int PersonalRowId, int SubCheckRowId, string CaseId, int ClientRowId = 0, string CompanyRefNo = "")
        {
            var Reportname = "";
            try
            {
                string Url = "https://api.risk.lseg.com/screening/v2/reports/" + reportId;
                refinitivAPI refinitivAPI = new refinitivAPI();
                var response = refinitivAPI.GetFileRequest(Url, "", Server.MapPath("~\\APIErrorLogs\\Logfiles"));
                var ResultJson = response.Content.ReadAsStringAsync().Result;
                //reportstatusRoot myDeserializedClass = JsonConvert.DeserializeObject<reportstatusRoot>(ResultJson);
                if (response.IsSuccessStatusCode)
                {
                    var srcImgStream = response.Content.ReadAsStreamAsync().Result;

                    byte[] bytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        srcImgStream.CopyTo(memoryStream);
                        bytes = memoryStream.ToArray();
                    }
                    //WoldCheckReports
                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["WoldCheckReports"].ToString();
                    string provideddocuments = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string CandidateReportdoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
                    string Path = Server.MapPath(FldPathDoc + CaseId + ".zip");
                    System.IO.File.WriteAllBytes(Path, bytes);

                    string StartupPath = Server.MapPath(FldPathDoc);
                    string NowTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    StartupPath += CaseId + "_" + NowTime;
                    ZipFile.ExtractToDirectory(Path, StartupPath);
                    using (ZipArchive zipArchive = ZipFile.OpenRead(Path))
                    {
                        foreach (ZipArchiveEntry item in zipArchive.Entries)
                        {
                            string sourcePath = StartupPath + "\\" + item.FullName;
                            string destinationPath = Server.MapPath(provideddocuments + SubCheckRowId.ToString() + "_" + item.Name.Replace(" " , ""));
                            string ReportdestinationPath = Server.MapPath(CandidateReportdoc + CompanyRefNo + ".pdf");
                            try
                            {
                                if (ClientRowId == 1638)
                                {
                                    System.IO.File.Copy(sourcePath, ReportdestinationPath);                                   
                                }

                                System.IO.File.Copy(sourcePath, destinationPath);
                                SaveWorldCheckDocPQVerifiedUploadDoc(PersonalRowId, Convert.ToInt16(SubCheckRowId), "Verified", "World Check Report", "provideddocuments", SubCheckRowId.ToString() + "_" + item.Name.Replace(" ", ""), "World Check Doc", 0);
                                zipArchive.Dispose();
                                Reportname = SubCheckRowId.ToString() + "_" + item.Name;
                                break;
                            }
                            catch
                            {
                            }

                        }
                    }
                    Directory.Delete(StartupPath, recursive: true);
                    if (System.IO.File.Exists(Path))
                    {
                        System.IO.File.Delete(Path);
                    }
                    return Reportname;
                }
                else
                {
                    return Reportname;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }       

        public void SaveWorldCheckDocPQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", string FileName = "", string remarks = "", byte NADoc = 0)
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

                    DocModel.FileName = FileName;
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
                repoPQVerifiedUploadDoc.AddPQVerifiedUploadDoc(DocModel);
                repoPQVerifiedUploadDoc.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }


        void SavePackageOrCheckWithCandidateAsBulkImport(AddPQPersonalBulkImportViewModel model)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                AddPQCandidateCheckViewModel chekModel = new AddPQCandidateCheckViewModel();
                chekModel.ClientRowID = model.ClientRowID;
                chekModel.PersonalRowID = model.PersonalRowID;
                string[] strChkPkg = model.CandidateCheckOrPackage.Split(',');
                foreach (var ChkPkg in strChkPkg)
                {
                    string[] strIds = ChkPkg.Split('-');
                    if (!string.IsNullOrEmpty(strIds[0]))
                    {
                        if (strIds[0] == "Check")
                        {
                            int ClientCheckRowID = Convert.ToInt32(strIds[1]);
                            var ChekData = repoClientCheck.GetChecksByClientId(model.ClientRowID).Where(p => p.ClientCheckRowID == ClientCheckRowID);
                            chekModel.ClientPackageRowID = 0;
                            foreach (var item in ChekData)
                            {
                                cc.CandidateCheckMoveToPQCandidate(chekModel.ClientRowID, chekModel.PersonalRowID, chekModel.ClientPackageRowID, item.ClientCheckRowID);
                                new API().UpdateAPINewCaseInitiateStatus(chekModel.PersonalRowID, chekModel.ClientRowID.ToString(), "AddCase_Bulk");
                            }
                        }
                        if (strIds[0] == "Package")
                        {
                            string pkgnm = ChkPkg.Substring(8);
                            var pkgDetails = repoClientPackage.GetPackageDetailsByPackageName(chekModel.ClientRowID.ToString(), pkgnm);
                            chekModel.ClientCheckRowID = 0;
                            foreach (var item in pkgDetails)
                            {
                                chekModel.ClientPackageRowID = item.ClientPackageRowID;
                                cc.CandidateCheckMoveToPQCandidate(chekModel.ClientRowID, chekModel.PersonalRowID, chekModel.ClientPackageRowID, chekModel.ClientCheckRowID);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [NonAction]
        private DateTime GetDeliveryDate(DateTime oDate, byte TAT, PQClientCodeHolidaysViewModel client)
        {
            try
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
            catch (Exception)
            {

                throw;
            }
        }
        [NonAction]
        private int getWeekendAndHoliDays(DateTime oDate, DateTime dDate, PQClientCodeHolidaysViewModel clientModel)
        {
            try
            {
                int intDate = 0;              
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
            catch (Exception)
            {

                throw;
            }
        }
        public string GetCompanyRefNo(short id = 0, byte bckDays = 0)
        {
            try
            {
                string CRefNo = string.Empty;
                short ClientAbbRowID = repoPQClient.GetPQClientForUpdateById(id).ClientAbbRowID;
                string str1 = repoAbbreviation.GetAbbreviationForUpdateById(ClientAbbRowID).ClientAbbreviation;
                int noOfEntr = repoPQPersonal.GetAllCandidatesById(id);
                string empty = string.Empty;                
                if (noOfEntr >= 10)
                {
                    noOfEntr++;
                    empty = Convert.ToString(noOfEntr);
                }
                else
                {
                    noOfEntr++;
                    empty = string.Concat("0", Convert.ToString(noOfEntr));
                }

                string[] strArrays = new string[] { str1, "-", DateTime.Now.AddDays(-bckDays).ToString("dd-MM-yyyy"), "-", empty };
                CRefNo = string.Concat(strArrays);
                return CRefNo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GenrateCandidateCode(short id = 0)
        {
            try
            {
                var ClientDetails = repoPQClient.GetPQClientForCadiCodeGenById(id);
                int CodeGeneration = ClientDetails.CodeGeneration;
                string CandidateCode = string.Empty;
                if (CodeGeneration > 0)
                {
                    string str1 = ClientDetails.ClientAbbName;
                    int noOfEntr = repoPQPersonal.GetAllCandidateCodeGenerationById(id);
                    string empty = string.Empty;
                    if (noOfEntr >= 10)
                    {
                        noOfEntr++;
                        empty = Convert.ToString(noOfEntr);
                    }
                    else
                    {
                        noOfEntr++;
                        empty = string.Concat("0", Convert.ToString(noOfEntr));
                    }

                    string str2 = DateTime.Now.Year.ToString();
                    string str3 = DateTime.Now.Month.ToString();
                    string str4 = str2 + str3 + DateTime.Now.Day.ToString();
                    string str5 = DateTime.Now.Hour.ToString();
                    string str6 = DateTime.Now.Minute.ToString();
                    string str7 = DateTime.Now.Second.ToString();
                    string str8 = str5 + str6 + str7 + DateTime.Now.Millisecond.ToString();
                    string[] strArrays = new string[] { str1, "-", str4, "-", empty, "-", str8 };
                    CandidateCode = string.Concat(strArrays);

                }
                else
                {
                    CandidateCode = "";
                }
                return CandidateCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SendMail( int ClientRowID, int ClientUserRowID)
        {
            string spocEmail = "";
            string CspocEmail = "";
            string ClientName = "";
            #region Send Mail To Candidate
            //string PersonalIds = personalRowId.ToString();

            ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.SendBulkuplodemail(ClientRowID, ClientUserRowID);
            if (dt.Rows.Count > 0)
            {
                spocEmail = dt.Rows[0]["SpocEmailID"].ToString();
                CspocEmail = dt.Rows[0]["CSpocEmailID"].ToString();
                ClientName = dt.Rows[0]["ClientName"].ToString();




                string MailSignature = string.Empty;
                string From = string.Empty;
                string DisplayName = string.Empty;
                string Cc = "";


                clsSendMail mail;



                mail = new clsSendMail();
                var signature = mail.GetCompanyMailSignature();

                if (signature != null)
                {
                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br /><b>Mobile: ", signature.MobileNo,
                        " | Direct Tel: ", signature.PhoneNo, " | Fax: ", signature.FaxNo, "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", signature.GeneralEmail, "'>", signature.GeneralEmail, "</a>");
                }

                //string cmpSpocEmails = repoPQClient.GetCompanySPOCEmailIdFromPQClientMasterById(ClientRowID);
                //if(!string.IsNullOrEmpty(cmpSpocEmails))
                //Cc = cmpSpocEmails;

                From = signature.GeneralEmail;
                DisplayName = signature.CompanyName.Trim();
                string body = string.Empty;
                string MailSubject = "";
                var templateDB = cc.getAlertMailtemplate(ClientRowID, "Bulk Upload", 0, 1, "BulkUploadMail");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }
                //using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/BulkUploadMail.html")))
                //{
                //    body = reader.ReadToEnd();
                //}

                body = body.Replace("{FClientName}", ClientName);

                body = body.Replace("{MailSignature}", MailSignature);
                string initationName = "bgv.initiation@securitas-india.com";
                string Subject = ClientName + " Bulk Upload";
                string To = spocEmail + "," + CspocEmail+","+ initationName;
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



                bool flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);

                if (flag)
                {

                }
                else
                {
                    TempData["ErrorMsg"] = "Mail sending failed!";
                }
            }
          
            #endregion
        }

        //[ValidateInput(false)]
        //public void AttachDoc(string AttachDocName)
        //{

        //    string query = System.Web.HttpContext.Current.Request.Url.Query;
        //    string queryUpperCase = string.Empty;
        //    if (query.Count() > 0)
        //    {
        //        queryUpperCase = query.ToUpper(); /* Start VAPT point for Restrict Web.Config file don't download */
        //        if (queryUpperCase.Contains("WEB.CONFIG") || queryUpperCase.Contains("CONFIG") || queryUpperCase.Contains(".CONFIG") || queryUpperCase.Contains("MACHINE.CONFIG"))
        //        {
        //            Response.Cookies["ASP.NET_SessionId"].Secure = false;
        //            Session.RemoveAll();
        //            Session.Clear();
        //            Session.Abandon();
        //            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddSeconds(-30);
        //            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
        //            Response.Redirect("~/Account/Index");
        //        } /* End VAPT point for Restrict Web.Config file don't download */
        //        else
        //        {
        //            FileInfo fi = new FileInfo(AttachDocName);
        //            string extn = fi.Extension;
        //            string FileName = AttachDocName;
        //            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["ClientAndCompanydoc"].ToString();
        //            Response.ContentType = "application/" + extn + "";
        //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + AttachDocName + "");
        //            Response.TransmitFile(Server.MapPath(FldPathDoc) + FileName);
        //            Response.End();                  
        //        }
        //    } 
        //    else
        //    {
        //        FileInfo fi = new FileInfo(AttachDocName);
        //        string extn = fi.Extension;
        //        string FileName = AttachDocName;
        //        string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["ClientAndCompanydoc"].ToString();
        //        Response.ContentType = "application/" + extn + "";
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + AttachDocName + "");
        //        Response.TransmitFile(Server.MapPath(FldPathDoc) + FileName);
        //        Response.End();

        //    }
        //}



        [ValidateInput(false)]
        public async Task AttachDoc(string AttachDocName)
        {

            string query = System.Web.HttpContext.Current.Request.Url.Query;
            string queryUpperCase = string.Empty;
            if (query.Count() > 0)
            {
                queryUpperCase = query.ToUpper(); /* Start VAPT point for Restrict Web.Config file don't download */
                if (queryUpperCase.Contains("WEB.CONFIG") || queryUpperCase.Contains("CONFIG") || queryUpperCase.Contains(".CONFIG") || queryUpperCase.Contains("MACHINE.CONFIG"))
                {
                    Response.Cookies["ASP.NET_SessionId"].Secure = false;
                    Session.RemoveAll();
                    Session.Clear();
                    Session.Abandon();
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddSeconds(-30);
                    Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                    Response.Redirect("~/Account/Index");
                } /* End VAPT point for Restrict Web.Config file don't download */
                else
                {
                    FileInfo fi = new FileInfo(AttachDocName);
                    string extn = fi.Extension;
                    string FileName = AttachDocName;
                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["ClientAndCompanydoc"].ToString();
                    //Response.ContentType = "application/" + extn + "";
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + AttachDocName + "");
                    //Response.TransmitFile(Server.MapPath(FldPathDoc) + FileName);
                    //Response.End();

                    HttpResponseMessage response = await _documentService.GetDocumentAsync(FileName, "D:/wwwroot2/Documents/ClientDocuments/UploadedExcelAndDocsForCompany/");
                    if (response.IsSuccessStatusCode)
                    {
                        using (var httpClient = new HttpClient())
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var obj = JsonConvert.DeserializeObject<DocumentResponse>(jsonString);

                            string docName = obj.DocumentName;
                            string base64Data = obj.DocumentValue;


                            var fileBytes = Convert.FromBase64String(base64Data);
                            string contentType = System.Web.MimeMapping.GetMimeMapping(docName);

                            Response.Clear();
                            Response.ContentType = "application/" + extn + "";
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" + AttachDocName + "");
                            Response.BinaryWrite(fileBytes);
                            Response.End();




                        }



                    }



                }
            }
            else
            {
                FileInfo fi = new FileInfo(AttachDocName);
                string extn = fi.Extension;
                string FileName = AttachDocName;
                string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["ClientAndCompanydoc"].ToString();
                //Response.ContentType = "application/" + extn + "";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + AttachDocName + "");
                //Response.TransmitFile(Server.MapPath(FldPathDoc) + FileName);
                //Response.End();

                HttpResponseMessage response = await _documentService.GetDocumentAsync(FileName, "D:/wwwroot2/Documents/ClientDocuments/UploadedExcelAndDocsForCompany/");
                if (response.IsSuccessStatusCode)
                {
                    using (var httpClient = new HttpClient())
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var obj = JsonConvert.DeserializeObject<DocumentResponse>(jsonString);

                        string docName = obj.DocumentName;
                        string base64Data = obj.DocumentValue;


                        var fileBytes = Convert.FromBase64String(base64Data);
                        string contentType = System.Web.MimeMapping.GetMimeMapping(docName);

                        Response.Clear();
                        Response.ContentType = "application/" + extn + "";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + AttachDocName + "");
                        Response.BinaryWrite(fileBytes);
                        Response.End();




                    }



                }


            }
        }


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [NonAction]
        void BindClientListByLogInName(int ClientRowid, int clientUserRowid = 0)
        {
            int ClientAccessType = Convert.ToInt32(Session["ClientAccessType"].ToString());
            try
            {
                if (ClientAccessType == 2)
                {
                    ViewBag.ClientList = repoDDL.GetClientGroupingInfo(ClientRowid);
                }
                else if (ClientAccessType == 3)
                {
                    ViewBag.ClientList = repoDDL.userwise_ReportingAccess(clientUserRowid);
                }
                else
                {
                    ViewBag.ClientList = repoDDL.GetClientGroupingInfo(ClientRowid);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}