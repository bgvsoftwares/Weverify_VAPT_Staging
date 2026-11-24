using BAL;
using BAL.ClientRepository;
using BAL.ProvidedInfoRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using ViewModels.ProvidedInfoViewModel;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;
using System.Data;
using ViewModels.ClientViewModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net.Mime;
using WebAppBGV.DocumentServices;

namespace WebAppBGV.Areas.ClientArea.Controllers
{
    [CustomAuthorizeClient]
    public class ClientInsuffController : Controller
    {
        #region ******  Repository Objects ******
        IDropDownMasterRepository repoDDL;
        IAbbreviationRepository repoAbbreviation;
        IClientLoginRepository repoLogin;
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
        IPQCandidateCheckRepository repoPQCandidateCheck;
        ISubCheckFamilyRepository repoSubCheckFamily;
        public DocumentService documentServices;

        public ClientInsuffController(
              IDropDownMasterRepository _repoDDL
            , IAbbreviationRepository _repoAbbreviation
            , IClientLoginRepository _repoLogin
            , IPQPersonalRepository _repoPQPersonal
            , IAddressInfoRepository _repoAddressInfo
            , IEmploymentInfoRepository _repoEmploymentInfo
            , IEducationInfoRepository _repoEducationInfo
            , ICriminalInfoRepository _repoCriminalInfo
            , INationalIdentityInfoRepository _repoNationalIdentInfo
            , IReferenceInfoRepository _repoReferenceInfo
            , ISubCheckFamilyRepository _repoSubCheck
            , IPQVerifiedUploadDocRepository _repoPQVerifiedUploadDoc
            , ISpecialCheckInfoRepository _repoSplCheckInfo
            , IPQCandidateCheckRepository _repoPQCandidateCheck
                 , ISubCheckFamilyRepository _repoSubCheckFamily
            , DocumentService _documentServices
        )
        {
            repoDDL = _repoDDL;
            repoAbbreviation = _repoAbbreviation;
            repoLogin = _repoLogin;
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
            repoPQCandidateCheck = _repoPQCandidateCheck;
            repoSubCheckFamily = _repoSubCheckFamily;
            documentServices = _documentServices;
        }
        #endregion

        // GET: ClientArea/ClientInsuff
        [SessionExpireFilterAttributeClient]
        public ActionResult Index()
        {
            CreateNavigationforInsuffCaseForClient();
            return View();
        }
        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult InsufficiencyCheckClearByClient(string id = "", string ClientRowId = "", string ChFamilRowId = "", string subCheckId = "", string PersonalRowID = "", string CompanyRefNo = "", string InfRemarks = "")
        {
            InsuffCheckClearViewModel model = new InsuffCheckClearViewModel();
            short Did = 0;
            Did = Convert.ToInt16(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(ClientRowId)));

            int Id = id != "" ? Convert.ToInt32(clsCommonMethods.PDecrypt(id)) : 0;//Convert.ToInt32(clsCommonMethods.PDecrypt(id));
            short CheckFamilyId = Convert.ToInt16(clsCommonMethods.PDecrypt(ChFamilRowId));            
            short SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            int perId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
            string RefNo = (clsCommonMethods.PDecrypt(CompanyRefNo));
            string IRemark = (clsCommonMethods.PDecrypt(InfRemarks));
            //CreateMenuitemforInsuffClear(perId, Did);
            if (model != null)
            {                
                    ClientCustomize cc = new ClientCustomize();
                    DataTable dt = cc.GetInsuffRemark(perId, SubCheckId);
                    if(dt.Rows.Count>0)
                    {
                        InfRemarks = dt.Rows[0]["Remarks"].ToString();
                    }
                    


                model.CheckRowID = Id;
                model.ClientRowID = Did;
                model.ChFamilRowId = CheckFamilyId;
                model.PersonalRowID = perId;
                model.SubCheckRowID = SubCheckId;
                model.SecuritasID = RefNo;
                model.INFReason = InfRemarks;
                model.SubCheckName = repoSubCheck.GetSubCheckFamilyForUpdateById(SubCheckId).SubCheckName;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult InsufficiencyCheckClearByClient(InsuffCheckClearViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.PersonalRowID > 0 && ((!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null) || (!string.IsNullOrEmpty(model.Remarks))))
                    {
                        if (!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
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
                                model.Remarks = "Insuff Document Uploaded By Client";
                            }
                        }
                        //repoPQPersonal.UpdateInsuffCheckByPeraonalIdAndSubCheckID(model.ChFamilRowId, model.CheckRowID, model.Remarks);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasactionByClient(model.PersonalRowID, "InsufficiencyCheckClearByClient", "Insuff Clear By Client", model.DocumentType, "Insert");
                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            //43 for L1 insuff
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "Client", model.ClientRowID);//45 means L1 Insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.Remarks, "L1-Client", model.ClientRowID);
                            cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                            cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                            //SendAlertMassageForInsuffClear(model.PersonalRowID, model.SubCheckRowID, model.Remarks,"L1 Insuff");
                            new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                        }
                        else if (j == 22)
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 56, "Client", model.ClientRowID);// 56 means L2 insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 56, model.Remarks, "L2-Client", model.ClientRowID);
                            cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                            cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                            SendAlertMassageForInsuffClearMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks, "L2 Insuff", "Insuff", 0,1);
                            new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                        }
                        #endregion
                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Client", model.Remarks);
                        #endregion
                        int PID = model.PersonalRowID;
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        var CandidateChecks = repoPQCandidateCheck.GetInfuffCheck(PID);
                        if (CandidateChecks != null && CandidateChecks.Count() > 0)
                        {
                            return RedirectToAction("GetClientCasesDashBoardInsuffInfo", "Home", new { CaseStatus = "Insufficiency", ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()) });
                            // Home/GetClientCasesDashBoardInsuffInfo?CaseStatus=Insufficiency&ClientRowId=gbnyFLhiINU%3D
                        }
                        else
                        {
                            return RedirectToAction("DashboardClient", "Home");
                        }
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
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult InsufficiencyClearByClient(string id = "", string ClientRowId = "", string ChFamilRowId = "", string subCheckId = "", string PersonalRowID = "", string CompanyRefNo = "", string InfRemarks = "")
        {
            InsuffCheckClearViewModel model = new InsuffCheckClearViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {               
                short Did = 0;
                Did = Convert.ToInt16(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(ClientRowId)));
                int Id = id != "" ? Convert.ToInt32(clsCommonMethods.PDecrypt(id)) : 0;//Convert.ToInt32(clsCommonMethods.PDecrypt(id));
                short CheckFamilyId = Convert.ToInt16(clsCommonMethods.PDecrypt(ChFamilRowId));
                short SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
                int perId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
                string RefNo = (clsCommonMethods.PDecrypt(CompanyRefNo));
                //string IRemark = (clsCommonMethods.PDecrypt(InfRemarks));
                CreateMenuitemforInsuffClear(perId, Did);
                string CompanyName = cc.CompanyNameForClientInsuff(perId, SubCheckId);
                ViewBag.CompName = CompanyName;
                if (model != null)
                {
                       
                        DataTable dt = cc.GetInsuffRemarks(perId, SubCheckId);
                        if (dt.Rows.Count > 0)
                        {
                            InfRemarks = dt.Rows[0]["Remarks"].ToString();
                        }

                    model.CheckRowID = Id;
                    model.ClientRowID = Did;
                    model.ChFamilRowId = CheckFamilyId;
                    model.PersonalRowID = perId;
                    model.SubCheckRowID = SubCheckId;
                    model.SecuritasID = RefNo;
                    model.INFReason = InfRemarks;
                    model.SubCheckName = "";
                    model.ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    if (SubCheckId > 0) { model.SubCheckName = repoSubCheck.GetSubCheckFamilyForUpdateById(SubCheckId).SubCheckName; }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult InsufficiencyClearByClient(InsuffCheckClearViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            ClientCustomize cc = new ClientCustomize();
        //            if (model.PersonalRowID > 0 && ((!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null) || (!string.IsNullOrEmpty(model.Remarks))))
        //            {
        //                //=====================================
        //                int i = cc.SaveandUpdateInsuffRemark(model.PersonalRowID, model.SubCheckRowID, model.Remarks, DateTime.UtcNow.ToString(), "");

        //                //===================================
        //                if (!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
        //                {
        //                    int count = 0;
        //                    string[] DocumentTypes = UploadHid.Split(',');
        //                    foreach (var item in UploadDoc)
        //                    {
        //                        model.FileName = item.FileName.ToString();
        //                        model.DocumentType = DocumentTypes[count];
        //                        if (model.DocumentType != null && model.FileName != null && item.ContentLength > 0)
        //                        {
        //                            SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "Provided", model.DocumentType, "provideddocuments", item, model.Remarks, 0, model.SecuritasID);
        //                        }
        //                        else
        //                        {
        //                            ViewBag.ErrorMsg = "Invalid Document!";
        //                            break;
        //                        }
        //                        count++;
        //                    }

        //                    if (string.IsNullOrEmpty(model.Remarks))
        //                    {
        //                        model.Remarks = "Insuff Document Uploaded By Client";
        //                    }
        //                }
        //                //repoPQPersonal.UpdateInsuffCheckByPeraonalIdAndSubCheckID(model.ChFamilRowId, model.CheckRowID, model.Remarks);

        //                #region Save LogTransaction Details
        //                clsCommonMethods.AddPQLogTrasactionByClient(model.PersonalRowID, "InsufficiencyClearByClient", "Insuff Clear By Client", model.DocumentType, "Insert");
        //                int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
        //                if (j == 11)
        //                {
        //                    //43 for L1 insuff
        //                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "Client", model.ClientRowID);//45 means L1 Insuff
        //                    cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.Remarks, "L1-Client", model.ClientRowID);
        //                    cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
        //                    cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
        //                    //SendAlertMassageForInsuffClear(model.PersonalRowID, model.SubCheckRowID, model.Remarks,"L1 Insuff");
        //                    new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
        //                }
        //                else if (j == 22)
        //                {
        //                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 56, "Client", model.ClientRowID);// 56 means L2 insuff
        //                    cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 56, model.Remarks, "L2-Client", model.ClientRowID);
        //                    cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
        //                    cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
        //                    SendAlertMassageForInsuffClearMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks, "L2 Insuff", "Insuff",0,1);
        //                    new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
        //                }
        //                #endregion

        //                #region Case/Check Action History Save 
        //                clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Client", model.Remarks);
        //                #endregion

        //                int PID = model.PersonalRowID;
        //                TempData["SuccessMsg"] = "Record Added Successfully!";
        //                ModelState.Clear();
        //                var CandidateChecks = repoPQCandidateCheck.GetInfuffCheck(PID);
        //                if (CandidateChecks != null && CandidateChecks.Count() > 0)
        //                {
        //                    return RedirectToAction("InsufficiencyClearByClient", "ClientInsuff", new { ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()), ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()), subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString()) });
        //                    // Home/GetClientCasesDashBoardInsuffInfo?CaseStatus=Insufficiency&ClientRowId=gbnyFLhiINU%3D
        //                    //return RedirectToAction("GetClientCasesDashBoardInsuffInfo", "ClientInsuff", new { ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()), ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()), subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString()) });

        //                }
        //                else
        //                {
        //                    return RedirectToAction("DashboardClient", "Home");
        //                    //return RedirectToAction("GetClientCasesDashBoardInsuffInfo", "ClientInsuff", new { ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()), ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()), subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString()) });

        //                }


        //            }
        //            else
        //            {
        //                ViewBag.ErrorMsg = "Please select at least one document or mention remarks!";
        //            }
        //        }
        //        else
        //        {
        //            var query = from state in ModelState.Values
        //                        from error in state.Errors
        //                        select error.ErrorMessage;

        //            var errorList = query.ToList();
        //            string strError = string.Empty;
        //            foreach (string str in errorList)
        //            {
        //                strError += str + "\n";
        //            }
        //            ModelState.AddModelError("", strError);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.ErrorMsg = Ex.Message;
        //        TempData["SuccessMsg"] = "Record Added Successfully!" + ViewBag.ErrorMsg;
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //    return View(model);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult InsufficiencyClearByClient(InsuffCheckClearViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.PersonalRowID > 0 && ((!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null) || (!string.IsNullOrEmpty(model.Remarks))))
                    {
                        //=====================================
                        int i = cc.SaveandUpdateInsuffRemark(model.PersonalRowID, model.SubCheckRowID, model.Remarks, DateTime.UtcNow.ToString(), "");

                        //===================================
                        if (!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
                        {
                            int count = 0;
                            string[] DocumentTypes = UploadHid.Split(',');
                            foreach (var item in UploadDoc)
                            {
                                model.FileName = item.FileName.ToString();
                                model.DocumentType = DocumentTypes[count];
                                if (model.DocumentType != null && model.FileName != null && item.ContentLength > 0)
                                {
                                    SavePQVerifiedUploadDocByAPI(model.PersonalRowID, model.SubCheckRowID, "Provided", model.DocumentType, "provideddocuments", item, model.Remarks, 0, model.SecuritasID);
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
                                model.Remarks = "Insuff Document Uploaded By Client";
                            }
                        }
                        //repoPQPersonal.UpdateInsuffCheckByPeraonalIdAndSubCheckID(model.ChFamilRowId, model.CheckRowID, model.Remarks);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasactionByClient(model.PersonalRowID, "InsufficiencyClearByClient", "Insuff Clear By Client", model.DocumentType, "Insert");
                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            int flagval = 0;
                            DataTable dataAll = cc.GetRecuiterInsuffValidation(model.PersonalRowID, Convert.ToInt32(model.SubCheckRowID), Convert.ToInt32(model.ClientRowID), "GetRecuiterInsuffValidationflag1");
                            if (dataAll.Rows.Count > 0)
                            {
                                flagval = Convert.ToInt32(dataAll.Rows[0]["value"].ToString());
                            }
                            if (flagval == 1 && Convert.ToInt32(model.SubCheckRowID) == 38 && (model.ClientRowID == 317 || model.ClientRowID == 327))
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "Client-RecuiterInsuffClearedClient", model.ClientRowID);//45 means L1 Insuff
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, "Recruiter Insuff - " + model.Remarks, "L1-Client", model.ClientRowID);
                                new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "Recruiter-Insuff Clear");
                            }
                            else
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "Client", model.ClientRowID);//45 means L1 Insuff
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.Remarks, "L1-Client", model.ClientRowID);
                                new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                            }
                                //43 for L1 insuff
                         
                            cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                            cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                            //SendAlertMassageForInsuffClear(model.PersonalRowID, model.SubCheckRowID, model.Remarks,"L1 Insuff");
                           
                        }
                        else if (j == 22)
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 56, "Client", model.ClientRowID);// 56 means L2 insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 56, model.Remarks, "L2-Client", model.ClientRowID);
                            cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                            cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                            SendAlertMassageForInsuffClearMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks, "L2 Insuff", "Insuff", 0, 1);
                            new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                        }
                        #endregion

                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Client", model.Remarks);
                        #endregion

                        int PID = model.PersonalRowID;
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        var CandidateChecks = repoPQCandidateCheck.GetInfuffCheck(PID);
                        if (CandidateChecks != null && CandidateChecks.Count() > 0)
                        {
                            return RedirectToAction("InsufficiencyClearByClient", "ClientInsuff", new { ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()), ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()), subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString()) });
                            // Home/GetClientCasesDashBoardInsuffInfo?CaseStatus=Insufficiency&ClientRowId=gbnyFLhiINU%3D
                            //return RedirectToAction("GetClientCasesDashBoardInsuffInfo", "ClientInsuff", new { ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()), ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()), subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString()) });

                        }
                        else
                        {
                            return RedirectToAction("DashboardClient", "Home");
                            //return RedirectToAction("GetClientCasesDashBoardInsuffInfo", "ClientInsuff", new { ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()), ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()), subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString()) });

                        }


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
                ViewBag.ErrorMsg = "Invalid paramerter value";
                TempData["SuccessMsg"] = "Record Added Successfully!" + ViewBag.ErrorMsg;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [NonAction]
        private void CreateMenuitemforInsuffClear(int id = 0, int ClientRowId = 0)
        {
            try
            {
                if (id > 0)
                {
                    ClientCustomize cc = new ClientCustomize();
                    DataSet ds = new DataSet();
                    ds = cc.InsuffCase_CheckDetails(id);
                    string NavInputDataMenuList = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string urlValue = Url.Content(string.Format("~/ClientArea/ClientInsuff/InsufficiencyClearByClient/?ClientRowId={0}&ChFamilRowId={1}&subCheckId={2}&PersonalRowID={3}", clsCommonMethods.PEncrypt(Convert.ToString(ds.Tables[0].Rows[i]["ClientRowId"])), clsCommonMethods.PEncrypt(Convert.ToString(ds.Tables[0].Rows[i]["CheckFamilyRowID"])), clsCommonMethods.PEncrypt(Convert.ToString(ds.Tables[0].Rows[i]["SubCheckFamilyID"])), clsCommonMethods.PEncrypt(Convert.ToString(id))));
                        NavInputDataMenuList += "<li class='insuff_li mt-2 ps-1'><a class='insuff_DataMenuList' href='" + urlValue + "'> " + Convert.ToString(ds.Tables[0].Rows[i]["SubCheckName"]) + " </a></li>";
                    }
                    Session["NavInputDataMenuList"] = NavInputDataMenuList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult InsuffClientAddresspdf(string  PersonalRowID, string SubCheckRowID)
        {
            ClientInsuffCheckViewModels model = new ClientInsuffCheckViewModels();
            ClientCustomize cc = new ClientCustomize();           
            string SubCheckName = repoSubCheck.GetSubCheckFamilyForUpdateById(Convert.ToInt16(SubCheckRowID)).SubCheckName;
            DataTable dt = new DataTable();
            dt = cc.addressInsuffcheck(Convert.ToInt32(PersonalRowID), Convert.ToInt32(SubCheckRowID));
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 50, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            if (dt.Rows.Count > 0)
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

                        cell = new PdfPCell(new Phrase(SubCheckName, FontFactory.GetFont("Times New Roman ,serif", 16, BaseColor.WHITE)));
                        cell.Colspan = 5;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = new BaseColor(129, 211, 201);
                        table.AddCell(cell);
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            string Label = dt.Rows[j]["Label"].ToString();
                            string Information = dt.Rows[j]["Information"].ToString();                           
                            cell = new PdfPCell(new Phrase(Label));
                            cell.Colspan = 2;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(Information));
                            cell.Colspan = 3;
                            table.AddCell(cell);                           
                        }
                        pdfDoc.Add(table);

            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Info.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
            return View(model);
        }      
       
        bool SendAlertMassageForInsuffClear(int PersonalRowID, short SubCheckRowID, string remarks,string insuffType)
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

            ClientCustomize cc = new ClientCustomize();
            DataSet ds = new DataSet();
            ds = cc.getAlertMailingData(PersonalRowID, SubCheckRowID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string body = string.Empty;
                string MailSubject = "";
                int clientrowid = Convert.ToInt32(ds.Tables[0].Rows[0]["clientrowid"]);
                var templateDB = cc.getAlertMailtemplate(clientrowid, "Insuff", 0, 1, "InsuffClear");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }

                //using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/InsuffClear.html")))
                //{
                //    body = reader.ReadToEnd();
                //}
                string CandidateName = Convert.ToString(ds.Tables[0].Rows[0]["CandidateName"]);
                string CompanyRefNo = Convert.ToString(ds.Tables[0].Rows[0]["CompanyRefNo"]);
                string SubCheckName = Convert.ToString(ds.Tables[0].Rows[0]["SubCheckName"]);
                string TeamEmailID = Convert.ToString(ds.Tables[0].Rows[0]["OperationEmailID"]);
                string SpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["SpocEmailID"]);
                string ClientSpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["ClientSpocEmailID"]);

                body = body.Replace("{FCandidateName}", CandidateName);
                body = body.Replace("{FCheckName}", SubCheckName);
                body = body.Replace("{FPRemarks}", remarks);
                body = body.Replace("{FRefNo}", CompanyRefNo);

                body = body.Replace("{MailSignature}", MailSignature);

                string Subject = "Insuff Cleared for " + CompanyRefNo + " " + CandidateName + "  " + SubCheckName;
                string To = TeamEmailID + "," + SpocEmailID;

             
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
                TempData["ErrorMsg"] = "Candidate Check details not found!";
            }
            return result;
        }

        bool SendAlertMassageForInsuffClearMail(int PersonalRowID, short SubCheckRowID, string remarks, string insuffType ,string AlertType="", int PackageId=0, int MailType =0)
        {
            bool result = false;
            string MailSignature = string.Empty;
            string From = string.Empty;
            string DisplayName = string.Empty;
            string Cc = string.Empty; //Session["CSpocEmailID"].ToString();
            clsSendMail mail;
            mail = new clsSendMail();
            var signature = mail.GetCompanyMailSignature();
            From = signature.GeneralEmail;
            DisplayName = signature.CompanyName.Trim();
            ClientCustomize cc = new ClientCustomize();
            DataSet ds = new DataSet();
            ds = cc.getAlertMailingDataForInsuff(PersonalRowID, SubCheckRowID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string body = string.Empty;
                string MailSubject = string.Empty;

                var templateDB = cc.getAlertMailtemplate(Convert.ToInt32(ds.Tables[0].Rows[0]["ClientRowID"]), AlertType, PackageId, MailType, "InsuffClear");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }
                //using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/InsuffClear.html")))
                //{
                //    body = reader.ReadToEnd();
                //}
                string CandidateName = Convert.ToString(ds.Tables[0].Rows[0]["CandidateName"]);
                string CompanyRefNo = Convert.ToString(ds.Tables[0].Rows[0]["CompanyRefNo"]);
                string SubCheckName = Convert.ToString(ds.Tables[0].Rows[0]["SubCheckName"]);
                string TeamEmailID = Convert.ToString(ds.Tables[0].Rows[0]["OperationEmailID"]);
                string SpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["SpocEmailID"]);
                string ClientSpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["ClientSpocEmailID"]);
                int clientrowid = Convert.ToInt32(ds.Tables[0].Rows[0]["clientrowid"]);
                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowID);
                string spocEmail = "";
                string cSpocEmail = "";
                if (signature != null)
                {
                    if (clientrowid == 249)
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
                DataTable dtEmail = new DataTable();
                dtEmail = cc.GetEmailStructureForAlert(clientrowid, CandidateData.ClientUserRowId, "Insuff", PersonalRowID);
                if (dtEmail.Rows.Count > 0)
                {
                    spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                    cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                }
                body = body.Replace("{FCandidateName}", CandidateName);
                body = body.Replace("{FCheckName}", SubCheckName);
                body = body.Replace("{FPRemarks}", remarks);
                body = body.Replace("{FRefNo}", CompanyRefNo);
                body = body.Replace("{MailSignature}", MailSignature);

                string Subject = "Insuff Cleared for " + CompanyRefNo + " " + CandidateName + "  " + SubCheckName;
                string To = TeamEmailID+"," + spocEmail + "," + cSpocEmail;
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
                if (CandidateData.ClientRowID == 257)
                {                    
                        string bcc = "";
                        string Ccc = "";
                        Ccc = spocEmail;
                        bcc = cSpocEmail;
                       flag = mail.SendMail(DisplayName, From, Ccc, "", bcc, Subject, body);
                    }
                else
                {
                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);

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
                TempData["ErrorMsg"] = "Candidate Check details not found!";
            }
            return result;
        }

        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult GetInsufficiencyCheckInfoByCandidate()
        {
            InsuffCheckInfoCandidateViewModel model = new InsuffCheckInfoCandidateViewModel();
            ClientCustomize cc = new ClientCustomize();
            short Did = Convert.ToInt16(Session["ClientRowId"]);
            int Id = 0;
            string RefNo = "";
            string InfRemarks = "";
            string strSubcheckName = "";
            short CheckFamilyId = Convert.ToInt16(Session["CheckFamilyRowID"]);
            short SubCheckId = Convert.ToInt16(Session["SubCheckRowID"]);
            int perId = Convert.ToInt32(Session["PersonalRowID"]);
            var tbl = cc.GetInsuffRemarksRefNo(perId, SubCheckId);
            if (tbl.Rows.Count > 0)
            {
                InfRemarks = Convert.ToString(tbl.Rows[0]["InsuffRaisedRemarks"]);
                strSubcheckName = Convert.ToString(tbl.Rows[0]["SubcheckName"]);
                Id = Convert.ToInt32(tbl.Rows[0]["CheckRowId"]);
                RefNo = Convert.ToString(tbl.Rows[0]["CompanyRefNo"]);
            }
            if (model != null)
            {
                if (Did == 891)
                {

                    DataTable dt = cc.GetInsuffRemark(perId, SubCheckId);
                    if (dt.Rows.Count > 0)
                    {
                        InfRemarks = dt.Rows[0]["Remarks"].ToString();
                    }

                }

                model.CheckRowID = Id;
                model.ClientRowID = Did;
                model.ChFamilRowId = CheckFamilyId;
                model.PersonalRowID = perId;
                model.SubCheckRowID = SubCheckId;
                model.SecuritasID = RefNo;
                model.INFReason = InfRemarks;
                model.SubCheckName = strSubcheckName;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetInsufficiencyCheckInfoByCandidate(InsuffCheckInfoCandidateViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.PersonalRowID > 0 && ((!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null) || (!string.IsNullOrEmpty(model.Remarks))))
                    {
                        if (!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
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
                        }

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasactionByClient(model.PersonalRowID, "InsufficiencyCheckClearByClient", "Insuff Clear By Candidate", model.DocumentType, "Insert");
                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            //43 for L1 insuff
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "Client", model.ClientRowID);//45 means L1 Insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.Remarks, "L1-Client", model.ClientRowID);
                            cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                            cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                            //SendAlertMassageForInsuffClear(model.PersonalRowID, model.SubCheckRowID, model.Remarks,"L1 Insuff");
                            new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                        }
                        else if (j == 22)
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 56, "Client", model.ClientRowID);// 56 means L2 insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 56, model.Remarks, "L2-Client", model.ClientRowID);
                            cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                            cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                            ////SendAlertMassageForInsuffClearMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks, "L2 Insuff");
                            new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                        }
                        #endregion

                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Client", model.Remarks);
                        #endregion

                        int PID = model.PersonalRowID;
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        var CandidateChecks = repoPQCandidateCheck.GetInfuffCheck(PID);
                        if (CandidateChecks != null && CandidateChecks.Count() > 0)
                        {
                            return RedirectToAction("GetClientCasesDashBoardInsuffInfo", "Home", new { CaseStatus = "Insufficiency", ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()) });
                            // Home/GetClientCasesDashBoardInsuffInfo?CaseStatus=Insufficiency&ClientRowId=gbnyFLhiINU%3D
                        }
                        else
                        {
                            return RedirectToAction("DashboardClient", "Home");
                        }


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
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }



        #region ****** Address Insuff Cases ******

        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult ManageAddressInsuffForClient(string ClientRowId = "")
        {
            PQAddressInsuffListPagedModel model = new PQAddressInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = "";
                string RecievingToDate = "";
                string RecievingFromDate = "";
                string CompletedToDate = "";
                string CompletedFromDate = "";
                int pageSize = 20;
                int allClients = 0;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }
                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);


                if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;
                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;
                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;
                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                }
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subcheckType = "Address Verification";
                model = repoAddressInfo.GetAllAddressInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subcheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);

                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = "";
                SearchModel.RecievingFromDate = "";
                SearchModel.CompletedFromDate = "";
                SearchModel.RecievingToDate = "";
                SearchModel.CompletedToDate = "";
                model.ItemSearch = SearchModel;
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ManageAddressInsuffForClient(FormCollection frm)
        {
            PQAddressInsuffListPagedModel model = new PQAddressInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = frm["Search"];
                string RecievingToDate = frm["RecievingToDate"];
                string RecievingFromDate = frm["RecievingfromDate"];
                string CompletedToDate = frm["CompletedToDate"];
                string CompletedFromDate = frm["CompletedToDate"];
                string ClientRowId = frm["ClientRowID"];
                int pageSize = 20;
                int allClients = 0;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }
                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);

                ViewBag.ErrorMsg = "";
                if (Search != "")
                {
                    if (clsCommonMethods.ValidateSQLInjuctionInput(Search) == false)
                    {
                        ViewBag.ErrorMsg = "Please Provide Valid Input";
                    }
                }
                if (ViewBag.ErrorMsg == "")
                {
                    if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;
                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;
                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;
                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                }
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subcheckType = "Address Verification";
                model = repoAddressInfo.GetAllAddressInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subcheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);
                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();            
                SearchModel.Search = Search;
                SearchModel.RecievingFromDate = RecievingFromDate;
                SearchModel.CompletedFromDate = CompletedFromDate;
                SearchModel.RecievingToDate = RecievingToDate;
                SearchModel.CompletedToDate = CompletedToDate;
                model.ItemSearch = SearchModel;
               
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }

        #endregion

        #region ****** Employment Insuff Cases ******

        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult ManageEmploymentInsuffForClient(string ClientRowId = "")
        {
            PQEmploymentInsuffListPagedModel model = new PQEmploymentInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = "";
                string RecievingToDate = "";
                string RecievingFromDate = "";
                string CompletedToDate = "";
                string CompletedFromDate = "";
                int pageSize = 20;
                
                int allClients = 0;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);


                if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;
                    ViewBag.ClientRowID = ClientRowID;
                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;
                    ViewBag.ClientRowID = ClientRowID;
                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "Employment Verification";
                model = repoEmploymentInfo.GetAllEmploymentInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);

                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = "";
                SearchModel.RecievingFromDate = "";
                SearchModel.CompletedFromDate = "";
                SearchModel.RecievingToDate = "";
                SearchModel.CompletedToDate = "";
                model.ItemSearch = SearchModel;
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ManageEmploymentInsuffForClient(FormCollection frm)
        {
            PQEmploymentInsuffListPagedModel model = new PQEmploymentInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = frm["Search"];
                string RecievingToDate = frm["RecievingToDate"];
                string RecievingFromDate = frm["RecievingfromDate"];
                string CompletedToDate = frm["CompletedToDate"];
                string CompletedFromDate = frm["CompletedFromDate"];
                string ClientRowId = frm["ClientRowID"];
                int pageSize = 20;

                int allClients = 0;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);


                ViewBag.ErrorMsg = "";
                if (Search != "")
                {
                    if (clsCommonMethods.ValidateSQLInjuctionInput(Search) == false)
                    {
                        ViewBag.ErrorMsg = "Please Provide Valid Input";
                    }
                }
                if (ViewBag.ErrorMsg == "")
                {
                    if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;

                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;

                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;

                }

                ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));

                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "Employment Verification";
                model = repoEmploymentInfo.GetAllEmploymentInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);
                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = Search;
                SearchModel.RecievingFromDate = RecievingFromDate;
                SearchModel.CompletedFromDate = CompletedFromDate;
                SearchModel.RecievingToDate = RecievingToDate;
                SearchModel.CompletedToDate = CompletedToDate;
                model.ItemSearch = SearchModel;


            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }
        #endregion

        #region ****** Education Insuff Cases ******

        [HttpGet]
        [SessionExpireFilterAttributeClient]
        public ActionResult ManageEducationInsuffForClient(string ClientRowId = "")
        {
            PQEducationInsuffListPagedModel model = new PQEducationInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = "";
                string RecievingToDate = "";
                string RecievingFromDate = "";
                string CompletedToDate = "";
                string CompletedFromDate = "";
                int pageSize = 20;

                int allClients = 0;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);


                if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;

                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;

                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;

                }

                ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));

                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "Education Verification";
                model = repoEducationInfo.GetAllEducationInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }

                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = "";
                SearchModel.RecievingFromDate = "";
                SearchModel.CompletedFromDate = "";
                SearchModel.RecievingToDate = "";
                SearchModel.CompletedToDate = "";
                model.ItemSearch = SearchModel;

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageEducationInsuffForClient(FormCollection frm)
        {
            PQEducationInsuffListPagedModel model = new PQEducationInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = frm["Search"];
                string RecievingToDate = frm["RecievingToDate"];
                string RecievingFromDate = frm["RecievingfromDate"];
                string CompletedToDate = frm[""];
                string CompletedFromDate = frm["CompletedFromDate"];
                string ClientRowId = frm["CompletedToDate"];


                int pageSize = 20;

                int allClients = 0;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);

                ViewBag.ErrorMsg = "";
                if (Search != "")
                {
                    if (clsCommonMethods.ValidateSQLInjuctionInput(Search) == false)
                    {
                        ViewBag.ErrorMsg = "Please Provide Valid Input";
                    }
                }
                if (ViewBag.ErrorMsg == "")
                {
                    if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;

                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;

                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;

                }

                ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));

                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "Education Verification";
                model = repoEducationInfo.GetAllEducationInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);
                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = Search;
                SearchModel.RecievingFromDate = RecievingFromDate;
                SearchModel.CompletedFromDate = CompletedFromDate;
                SearchModel.RecievingToDate = RecievingToDate;
                SearchModel.CompletedToDate = CompletedToDate;
                model.ItemSearch = SearchModel;
                
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }
        #endregion

        #region ****** Criminal Insuff Cases ******
        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult ManageCriminalInsuffForClient(string ClientRowId = "")
        {
            PQCriminalInsuffListPagedModel model = new PQCriminalInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = "";
                string RecievingToDate = "";
                string RecievingFromDate = "";
                string CompletedToDate = "";
                string CompletedFromDate = "";
                int pageSize = 20;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }

                int allClients = 0;

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);


                if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;
                    
                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;

                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;
                   
                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;

                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;

                }

                ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));

                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subcheckType = "Criminal Record Verification";
                model = repoCriminalInfo.GetAllCriminalInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subcheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }

                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = Search;
                SearchModel.RecievingFromDate = RecievingFromDate;
                SearchModel.CompletedFromDate = CompletedFromDate;
                SearchModel.RecievingToDate = RecievingToDate;
                SearchModel.CompletedToDate = CompletedToDate;
                model.ItemSearch = SearchModel;
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ManageCriminalInsuffForClient(FormCollection frm)
        {
            PQCriminalInsuffListPagedModel model = new PQCriminalInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = frm["Search"];
                string RecievingToDate = frm["RecievingToDate"];
                string RecievingFromDate = frm["RecievingfromDate"];
                string CompletedToDate = frm["CompletedToDate"];
                string CompletedFromDate = frm["CompletedFromDate"];
                string ClientRowId = frm["ClientRowID"];
                int pageSize = 20;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }

                int allClients = 0;

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);

                ViewBag.ErrorMsg = "";
                if (Search != "")
                {
                    if (clsCommonMethods.ValidateSQLInjuctionInput(Search) == false)
                    {
                        ViewBag.ErrorMsg = "Please Provide Valid Input";
                    }
                }
                if (ViewBag.ErrorMsg == "")
                {
                    if (ClientAccessType == "3" && ClientRowID > 0)
                    {
                        ClientUserRowID = ClientRowID;
                        ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                        allClients = 0;

                    }

                    if (ClientAccessType == "3" && ClientRowID == 0)
                    {
                        ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                        ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                        allClients = 1;
                        ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                        ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));
                    }

                    if (ClientAccessType == "2" && ClientRowID > 0)
                    {
                        ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                        allClients = 0;

                    }

                    if (ClientAccessType == "2" && ClientRowID == 0)
                    {
                        ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                        ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                        allClients = 1;

                    }

                    if (ClientAccessType == "1")
                    {
                        ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                        ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                        allClients = 1;

                    }

                    ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                    ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));

                    string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                    string subcheckType = "Criminal Record Verification";
                    model = repoCriminalInfo.GetAllCriminalInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subcheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);
                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = Search;
                SearchModel.RecievingFromDate = RecievingFromDate;
                SearchModel.CompletedFromDate = CompletedFromDate;
                SearchModel.RecievingToDate = RecievingToDate;
                SearchModel.CompletedToDate = CompletedToDate;
                model.ItemSearch = SearchModel;

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }
        #endregion

        #region ****** NationalIdentity Insuff Cases ******
        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult ManageNationalIdentityInsuffForClient(string ClientRowId = "")
        {
            PQNationalIdentityInsuffListPagedModel model = new PQNationalIdentityInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = "";
                string RecievingToDate = "";
                string RecievingFromDate = "";
                string CompletedToDate = "";
                string CompletedFromDate = "";
                int pageSize = 20;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }
                int allClients = 0;

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);


                if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;
                   
                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;
                    
                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                ViewBag.ClientRowID = Convert.ToString(ClientRowID);

                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subcheckType = "National Identity Check";
                model = repoNationalIdentInfo.GetAllNationalIdentityInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subcheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }

                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = "";
                SearchModel.RecievingFromDate = "";
                SearchModel.CompletedFromDate = "";
                SearchModel.RecievingToDate = "";
                SearchModel.CompletedToDate = "";
                model.ItemSearch = SearchModel;
               
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ManageNationalIdentityInsuffForClient(FormCollection frm)
        {
            PQNationalIdentityInsuffListPagedModel model = new PQNationalIdentityInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = frm["Search"];
                string RecievingToDate = frm["RecievingToDate"];
                string RecievingFromDate = frm["RecievingfromDate"];
                string CompletedToDate = frm["CompletedToDate"];
                string CompletedFromDate = frm["CompletedFromDate"];
                string ClientRowId = frm["ClientRowID"];
                int pageSize = 20;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }
                int allClients = 0;

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);

                ViewBag.ErrorMsg = "";
                if (Search != "")
                {
                    if (clsCommonMethods.ValidateSQLInjuctionInput(Search) == false)
                    {
                        ViewBag.ErrorMsg = "Please Provide Valid Input";
                    }
                }
                if (ViewBag.ErrorMsg == "")
                {
                    if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                ViewBag.ClientRowID = Convert.ToString(ClientRowID);

                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subcheckType = "National Identity Check";
                model = repoNationalIdentInfo.GetAllNationalIdentityInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subcheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);
                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = Search;
                SearchModel.RecievingFromDate = RecievingFromDate;
                SearchModel.CompletedFromDate = CompletedFromDate;
                SearchModel.RecievingToDate = RecievingToDate;
                SearchModel.CompletedToDate = CompletedToDate;
                model.ItemSearch = SearchModel;
                
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }
        #endregion

        #region ****** Reference Insuff Cases ******
        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult ManageReferenceInsuffForClient(string ClientRowId = "")
        {
            PQReferenceInsuffListPagedModel model = new PQReferenceInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = "";
                string RecievingToDate = "";
                string RecievingFromDate = "";
                string CompletedToDate = "";
                string CompletedFromDate = "";
                int pageSize = 20;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }


              int allClients = 0;

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);


                if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;
                    
                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;
                    
                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));


                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subcheckType = "Reference Verification";
                model = repoReferenceInfo.GetAllReferenceInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subcheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = "";
                SearchModel.RecievingFromDate = "";
                SearchModel.CompletedFromDate = "";
                SearchModel.RecievingToDate = "";
                SearchModel.CompletedToDate = "";
                model.ItemSearch = SearchModel;
                
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ManageReferenceInsuffForClient(FormCollection frm)
        {
            PQReferenceInsuffListPagedModel model = new PQReferenceInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = frm["Search"];
                string RecievingToDate = frm["RecievingToDate"];
                string RecievingFromDate = frm["RecievingfromDate"];
                string CompletedToDate = frm["CompletedToDate"];
                string CompletedFromDate = frm["CompletedFromDate"];
                string ClientRowId = frm["ClientRowID"];
                int pageSize = 20;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }


                int allClients = 0;

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);

                ViewBag.ErrorMsg = "";
                if (Search != "")
                {
                    if (clsCommonMethods.ValidateSQLInjuctionInput(Search) == false)
                    {
                        ViewBag.ErrorMsg = "Please Provide Valid Input";
                    }
                }
                if (ViewBag.ErrorMsg == "")
                {
                    if (ClientAccessType == "3" && ClientRowID > 0)
                    {
                        ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                ViewBag.ClientRowID = clsCommonMethods.PEncrypt(Convert.ToString(ClientRowID));


                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subcheckType = "Reference Verification";
                model = repoReferenceInfo.GetAllReferenceInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subcheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);
                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = Search;
                SearchModel.RecievingFromDate = RecievingFromDate;
                SearchModel.CompletedFromDate = CompletedFromDate;
                SearchModel.RecievingToDate = RecievingToDate;
                SearchModel.CompletedToDate = CompletedToDate;
                model.ItemSearch = SearchModel;

               
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }
        #endregion

        #region ****** SpecialChack Insuff Cases ******
        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult ManageSpecialChackInsuffForClient(string ClientRowId = "")
        {
            PQSpecialCheckInsuffListPagedModel model = new PQSpecialCheckInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = "";
                string RecievingToDate = "";
                string RecievingFromDate = "";
                string CompletedToDate = "";
                string CompletedFromDate = "";
                int pageSize = 20;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }

                int allClients = 0;

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);


                if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;
                    
                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;
                    
                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                ViewBag.ClientRowID = Convert.ToString(ClientRowID);

                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subcheckType = "Special Checks";
                model = repoSplCheckInfo.GetAllSpecialCheckInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subcheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);

                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = "";
                SearchModel.RecievingFromDate = "";
                SearchModel.CompletedFromDate = "";
                SearchModel.RecievingToDate = "";
                SearchModel.CompletedToDate = "";
                model.ItemSearch = SearchModel;


                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ManageSpecialChackInsuffForClient(FormCollection frm)
        {
            PQSpecialCheckInsuffListPagedModel model = new PQSpecialCheckInsuffListPagedModel();
            try
            {
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = frm["Search"];
                string RecievingToDate = frm["RecievingToDate"];
                string RecievingFromDate = frm["RecievingfromDate"];
                string CompletedToDate = frm["CompletedToDate"];
                string CompletedFromDate = frm["CompletedFromDate"];
                string ClientRowId = frm["ClientRowID"];
                int pageSize = 20;
                short ClientRowID = 0;
                try
                {
                    ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));
                }
                catch { }
                
                int allClients = 0;

                int ClientUserRowID = 0;
                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);


                if (ClientAccessType == "3" && ClientRowID > 0)
                {
                    ClientUserRowID = ClientRowID;
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "3" && ClientRowID == 0)
                {
                    ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "2" && ClientRowID > 0)
                {
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 0;

                }

                if (ClientAccessType == "2" && ClientRowID == 0)
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                if (ClientAccessType == "1")
                {
                    ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    allClients = 1;
                }

                ViewBag.clientUserRowid = Convert.ToString(ClientUserRowID);
                ViewBag.ClientRowID = Convert.ToString(ClientRowID);

                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subcheckType = "Special Checks";
                model = repoSplCheckInfo.GetAllSpecialCheckInsuffListForDETL1(page, pageSize, sort.ToLower(), sortDir.ToLower(), subcheckType, Search, ClientRowID, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, "", 0, ClientUserRowID, allClients);

                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = Search;
                SearchModel.RecievingFromDate = RecievingFromDate;
                SearchModel.CompletedFromDate = CompletedFromDate;
                SearchModel.RecievingToDate = RecievingToDate;
                SearchModel.CompletedToDate = CompletedToDate;
                model.ItemSearch = SearchModel;

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid paramerter value";
            }
            return View(model);
        }


        #endregion

        #region ****** NonAction ******

        [NonAction]
        private void CreateNavigationforInsuffCaseForClient()
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

            string NavTeamMemberDE = string.Empty;
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageAddressInsuffForClient", "ClientInsuff") + "'> Address (" + AllChecks.AddressCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageEmploymentInsuffForClient", "ClientInsuff") + "'> Employment (" + AllChecks.EmploymentCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageEducationInsuffForClient", "ClientInsuff") + "'> Education (" + AllChecks.EducationCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageCriminalInsuffForClient", "ClientInsuff") + "'> Criminal (" + AllChecks.CriminalCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageNationalIdentityInsuffForClient", "ClientInsuff") + "'> NationalIdentity (" + AllChecks.NatIdentityCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageReferenceInsuffForClient", "ClientInsuff") + "'> Reference (" + AllChecks.ReferenceCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageSpecialChackInsuffForClient", "ClientInsuff") + "'> Special Check (" + AllChecks.SpecialcheckCount + ") </a></li>";
            Session["NavLeftInsuffMenuForClient"] = NavTeamMemberDE;
        }

        private void SavePQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", HttpPostedFileBase UploadDoc = null, string remarks = "", byte NADoc = 0, string secRefId = "")
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
                    //proImg = (secRefId + "_" + SubCheckRowID.ToString() + " " + DocumentType).Replace(" ", "_").Replace("-", "_") + ext;
                    proImg = (PersonalRowID + "_" + SubCheckRowID.ToString() + " " + DocumentType).Replace(" ", "_").Replace("-", "_") + ext;
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


        private void SavePQVerifiedUploadDocByAPI(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", HttpPostedFileBase UploadDoc = null, string remarks = "", byte NADoc = 0, string secRefId = "")
        {
            //UploadDynamicCandidateFiles(string DocumentName, string DocumentValue, string Documentpath)
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
                    //proImg = (secRefId + "_" + SubCheckRowID.ToString() + " " + DocumentType).Replace(" ", "_").Replace("-", "_") + ext;
                    proImg = (PersonalRowID + "_" + SubCheckRowID.ToString() + " " + DocumentType).Replace(" ", "_").Replace("-", "_") + ext;
                    // UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                    string base64 = string.Empty;
                    UploadDoc.InputStream.Position = 0;
                    using (var memoryStream = new MemoryStream())
                    {
                        UploadDoc.InputStream.CopyTo(memoryStream); // Convert the byte array to Base64 
                        base64 = Convert.ToBase64String(memoryStream.ToArray());
                    }
                    documentServices.UploadDynamicCandidateFiles(proImg , base64 , "D:/wwwroot2/Documents/CandidateDocuments/ProvidedDocuments/");
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

        public JsonResult GetDocumentTypeListBySubCheckIDForInsuffCandidate(short id = 0)
        {
            try
            {
                string clientId = "";
                if (id > 0)
                {
                    if (Session["ClientRowId"] != null)
                    {
                        clientId = Session["ClientRowId"].ToString();
                    }

                    short chkId = repoSubCheck.GetSubCheckFamilyForUpdateById(id).CheckFamilyRowID;
                    string pageName = string.Empty;
                    if (chkId == 2)
                    {
                        //pageName = "AddAddressInfo";
                        List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Ration Card", Value="Ration_Card"},
                            new SelectListItem{Text="Telephone Bill", Value="Telephone_Bill"},
                            new SelectListItem{Text="Rent Agreement", Value="Rent_Agreement"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="Electricity Bill", Value="Electricity_Bill"},
                            new SelectListItem{Text="Gas Supply Bill", Value="Gas_Supply_Bill"},
                            new SelectListItem{Text="Water Bill", Value="Water_Bill"},
                           new SelectListItem{Text="Others", Value="Others"},
                           new SelectListItem{Text="AAdhar Card", Value="AAdhar Card"}
                        };
                        return Json(objData, JsonRequestBehavior.AllowGet);

                    }
                    if (chkId == 5)
                    {
                        //pageName = "AddCriminalInfo";
                        List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Identity Proof", Value="Identity_Proof"},
                            new SelectListItem{Text="Address Proof", Value="Address_Proof"},
                            new SelectListItem{Text="Authorized Letter", Value="Authorized_Letter"},
                            new SelectListItem{Text="Other Proof", Value="Other_Proof"}
                        };
                        return Json(objData, JsonRequestBehavior.AllowGet);
                    }
                    if (chkId == 11)
                    {
                        // For TATA Elxsi
                        if (clientId == "635")
                        {
                            //pageName = "AddEducationInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                            new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                            new SelectListItem{Text="Grade Sheet", Value="Grade_Sheet"},
                            new SelectListItem{Text="Other Proof", Value="Other_Proof"},
                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //pageName = "AddEducationInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                            new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                            new SelectListItem{Text="Grade Sheet", Value="Grade_Sheet"},
                            new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="Other Proof", Value="Other_Proof"},
                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (chkId == 12 && id == 68)
                    {
                        // For TATA Elxsi
                        if (clientId == "635")
                        {
                            //pageName = "AddEmploymentInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter ", Value="Relieving_Letter"},

                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }
                        //Fractal Analytics Private Limited

                        if (clientId == "679")
                        {
                            //pageName = "AddEmploymentInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter ", Value="Experience_Letter"},
                            new SelectListItem{Text="Resignation acceptance Letter", Value="Resignationacceptance_letter"},

                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
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
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (chkId == 12 && id != 68)
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
                            return Json(objData, JsonRequestBehavior.AllowGet);
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
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (chkId == 26)
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
                        return Json(objData, JsonRequestBehavior.AllowGet);
                    }
                    if (chkId == 30)
                    {
                        //pageName = "AddReferenceInfo";
                        List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                        return Json(objData, JsonRequestBehavior.AllowGet);
                    }
                    if (chkId == 40)
                    {
                        //pageName = "AddSpecialCheckInfo";
                        List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Special Document", Value="SpecialDoc"},
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                        return Json(objData, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                 throw Ex;
            }
        }


        #endregion

        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult ApproveRejectNonGreenCheck(string id = "", string ClientRowId = "", string ChFamilRowId = "", string subCheckId = "", string PersonalRowID = "", string CompanyRefNo = "", string InfRemarks = "",string  colorCode="")
        {
            InsuffCheckClearViewModel model = new InsuffCheckClearViewModel();
            short Did = 0;
            Did = Convert.ToInt16(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(ClientRowId)));

            int Id = id != "" ? Convert.ToInt32(clsCommonMethods.PDecrypt(id)) : 0;//Convert.ToInt32(clsCommonMethods.PDecrypt(id));
            short CheckFamilyId = Convert.ToInt16(clsCommonMethods.PDecrypt(ChFamilRowId));
            short SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
            int perId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
            string RefNo = (clsCommonMethods.PDecrypt(CompanyRefNo));
            string IRemark = (clsCommonMethods.PDecrypt(InfRemarks));
            string colorCodeCheck = (clsCommonMethods.PDecrypt(colorCode));
            //CreateMenuitemforInsuffClear(perId, Did);
            if (model != null)
            {
                ClientCustomize cc = new ClientCustomize();
                //DataTable dt = cc.getCheckColorCode(perId, SubCheckId, Convert.ToInt32(ClientRowId));
                //if (dt.Rows.Count > 0)
                //{
                //    InfRemarks = dt.Rows[0]["Remarks"].ToString();
                //}



                model.CheckRowID = Id;
                model.ClientRowID = Did;
                model.ChFamilRowId = CheckFamilyId;
                model.PersonalRowID = perId;
                model.SubCheckRowID = SubCheckId;
                model.SecuritasID = RefNo;
                model.INFReason = InfRemarks;
                model.SubCheckName = repoSubCheck.GetSubCheckFamilyForUpdateById(SubCheckId).SubCheckName;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ApproveRejectNonGreenCheck(InsuffCheckClearViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc, string action = "")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.Remarks != "" && model.Remarks!=null)
                    {
                        if (model.PersonalRowID > 0 && action == "Approve")
                        {
                            if (!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
                            {
                                int count = 0;
                                string[] DocumentTypes = UploadHid.Split(',');
                                foreach (var item in UploadDoc)
                                {
                                    model.FileName = item.FileName.ToString();
                                    model.DocumentType = DocumentTypes[count];
                                    if (model.DocumentType != null && model.FileName != null && item.ContentLength > 0)
                                    {
                                        SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "clientApprove", model.DocumentType, "provideddocuments", item, model.Remarks, 0, model.SecuritasID);
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
                                    model.Remarks = "Approve NonGreenCheck Document Uploaded By Client";
                                }
                            }
                            cc.UpdateCheckStatusColorCodeNongreen(model.PersonalRowID, model.SubCheckRowID, model.Remarks, action);
                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Non-Greencheck Approve By Client", model.Remarks);
                            #endregion
                            ViewBag.ErrorMsg = "Check has been Approve";
                            return RedirectToAction("GetColorCodeReviewClientCheck", "Home", new { CaseStatus = "NonGreenCheck", ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()) });


                        }
                        else if (model.PersonalRowID > 0 && action == "Reject")
                        {
                            if (model.PersonalRowID > 0 && ((!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null) || (!string.IsNullOrEmpty(model.Remarks))))
                            {
                                if (!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
                                {
                                    int count = 0;
                                    string[] DocumentTypes = UploadHid.Split(',');
                                    foreach (var item in UploadDoc)
                                    {
                                        model.FileName = item.FileName.ToString();
                                        model.DocumentType = DocumentTypes[count];
                                        if (model.DocumentType != null && model.FileName != null && item.ContentLength > 0)
                                        {
                                            SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "ClientReject", model.DocumentType, "provideddocuments", item, model.Remarks, 0, model.SecuritasID);
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
                                        model.Remarks = "Rejected NonGreenCheck Document Uploaded By Client";
                                    }
                                }
                                cc.UpdateCheckStatusColorCodeNongreen(model.PersonalRowID, model.SubCheckRowID, model.Remarks, action);
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 20, "ClientNonGreencheckReject", model.ClientRowID);// 56 means L2 insuff
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 20, model.Remarks, "ClientNonGreencheckReject", model.ClientRowID);
                                

                            }
                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Non-Greencheck rejected By Client", model.Remarks);
                            return RedirectToAction("GetColorCodeReviewClientCheck", "Home", new { CaseStatus = "NonGreenCheck", ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()) });
                            #endregion

                        }



                    }

                    else
                    {
                        ViewBag.ErrorMsg = "Please fill the remarks";

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
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }


        public JsonResult GetDocumentTypeListBySubCheckID(short id = 0)
        {
            try
            {
                string clientId = "";
                if (id > 0)
                {
                    if (Session["ClientRowId"] != null)
                    {
                        clientId = Session["ClientRowId"].ToString();
                    }

                    short chkId = repoSubCheckFamily.GetSubCheckFamilyForUpdateById(id).CheckFamilyRowID;
                    string pageName = string.Empty;
                    if (chkId == 2 && clientId == "1255")
                    {
                        List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="NonGreenCheck-Doc", Value="NonGreenCheck-Doc"}

                        };
                        return Json(objData, JsonRequestBehavior.AllowGet);
                    }

                    else if (chkId == 2 && clientId != "1188")
                    {
                        //pageName = "AddAddressInfo";
                        List<SelectListItem> objData = new List<SelectListItem>
                        {
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
                        return Json(objData, JsonRequestBehavior.AllowGet);

                    }
                    else if (chkId == 2 && clientId == "1188")
                    {
                        //pageName = "AddAddressInfo";
                        List<SelectListItem> objData = new List<SelectListItem>

                        {
                            new SelectListItem{Text="Ration Card", Value="Ration_Card"},
                            new SelectListItem{Text="Telephone Bill", Value="Telephone_Bill"},
                            new SelectListItem{Text="Rent Agreement", Value="Rent_Agreement"},
                            new SelectListItem{Text="Passport", Value="Passport"},
                            new SelectListItem{Text="Electricity Bill", Value="Electricity_Bill"},
                            new SelectListItem{Text="Gas Supply Bill", Value="Gas_Supply_Bill"},
                            new SelectListItem{Text="Water Bill", Value="Water_Bill"},
                           new SelectListItem{Text="AAdhar Card", Value="AAdhar Card"},
                            new SelectListItem{Text="Others(VoterID/Driving licence/Bank Passbook)", Value="Others"}
                        };
                        return Json(objData, JsonRequestBehavior.AllowGet);

                    }
                    if (chkId == 5 && clientId == "1255")
                    {
                        List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="NonGreenCheck-Doc", Value="NonGreenCheck-Doc"}

                        };
                        return Json(objData, JsonRequestBehavior.AllowGet);
                    }
                    else if (chkId == 5)
                    {
                        //pageName = "AddCriminalInfo";
                        List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Identity Proof", Value="Identity_Proof"},
                            new SelectListItem{Text="Address Proof", Value="Address_Proof"},
                            new SelectListItem{Text="Authorized Letter", Value="Authorized_Letter"},
                            new SelectListItem{Text="Other Proof", Value="Other_Proof"}
                        };
                        return Json(objData, JsonRequestBehavior.AllowGet);
                    }
                    if (chkId == 11)
                    {

                        // For TATA Elxsi
                        if (clientId == "635")
                        {
                            //pageName = "AddEducationInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                            new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                            new SelectListItem{Text="Grade Sheet", Value="Grade_Sheet"},
                            new SelectListItem{Text="Other Proof", Value="Other_Proof"},
                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }
                        if (chkId == 11 && clientId == "1255")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="NonGreenCheck-Doc", Value="NonGreenCheck-Doc"}

                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //pageName = "AddEducationInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Degree Certificate", Value="Degree_Certificate"},
                            new SelectListItem{Text="Provisional Certificate", Value="Provisional_Certificate"},
                            new SelectListItem{Text="Grade Sheet", Value="Grade_Sheet"},
                            new SelectListItem{Text="LOA", Value="LOA"},
                            new SelectListItem{Text="Other Proof", Value="Other_Proof"},
                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (chkId == 12 && id == 68)
                    {
                        // For TATA Elxsi
                        if (clientId == "635")
                        {
                            //pageName = "AddEmploymentInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter ", Value="Experience_Letter"},
                            new SelectListItem{Text="Relieving Letter ", Value="Relieving_Letter"},

                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }
                        //Fractal Analytics Private Limited

                        if (clientId == "679")
                        {
                            //pageName = "AddEmploymentInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Experience Letter ", Value="Experience_Letter"},
                            new SelectListItem{Text="Resignation acceptance Letter", Value="Resignationacceptance_letter"},

                            new SelectListItem{Text="Appointment Letter", Value="Appointment_Letter"},
                            new SelectListItem{Text="Others", Value="Others"},
                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }
                        if (clientId == "1255")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="NonGreenCheck-Doc", Value="NonGreenCheck-Doc"}

                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
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
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (chkId == 12 && id != 68)
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
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }
                        if (chkId == 12 && clientId == "1255")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="NonGreenCheck-Doc", Value="NonGreenCheck-Doc"}

                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
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
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (chkId == 26)
                    {
                        if (clientId == "1255")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="NonGreenCheck-Doc", Value="NonGreenCheck-Doc"}

                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
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
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (chkId == 30)
                    {
                        if (clientId == "1255")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="NonGreenCheck-Doc", Value="NonGreenCheck-Doc"}

                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //pageName = "AddReferenceInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (chkId == 40)
                    {
                        if (clientId == "1255")
                        {
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="NonGreenCheck-Doc", Value="NonGreenCheck-Doc"}

                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //pageName = "AddSpecialCheckInfo";
                            List<SelectListItem> objData = new List<SelectListItem>
                        {
                            new SelectListItem{Text="Special Document", Value="SpecialDoc"},
                            new SelectListItem{Text="Others", Value="Others"}
                        };
                            return Json(objData, JsonRequestBehavior.AllowGet);
                        }

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                //clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace)
                throw Ex;
            }
        }

        public ActionResult GetReportForColorCodeReviewClient(string CompanyRefNo, string CaseStatus)
        {
            CompanyRefNo = (clsCommonMethods.PDecrypt(CompanyRefNo));
            string ReportUploadedName = "";
            DataTable ddt = new ClientCustomize().getUploadedReportCompRefNo(CompanyRefNo);
            if (ddt.Rows.Count > 0)
            {
                ReportUploadedName = CompanyRefNo + "-" + ddt.Rows.Count + ".pdf";
            }
            else
            {
                ReportUploadedName = CompanyRefNo + ".pdf";
            }
            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
            byte[] fileBytes = null;

            if (ReportUploadedName.Contains("24/7CPL"))
            {
                ReportUploadedName = ReportUploadedName.Replace("24/7CPL", "24-7CPL");
            }
            if (System.IO.File.Exists(Server.MapPath(FldPathDoc) + ReportUploadedName))
            {
                fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FldPathDoc) + ReportUploadedName);
            }
            if (fileBytes != null)
                return File(fileBytes, MediaTypeNames.Application.Octet, CompanyRefNo + ".pdf");
            else
            {
                return RedirectToAction("GetColorCodeReviewClientCheck", new { CaseStatus = "ColorCodeReview" });
            }
        }
    }
}