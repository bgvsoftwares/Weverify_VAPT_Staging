using BAL;
using BAL.CandidateRepository;
using BAL.ClientRepository;
using BAL.ProvidedInfoRepository;
using DAL;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Interop;
using ViewModels;
using ViewModels.CandidateViewModel;
using ViewModels.ClientViewModel;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;
using WebAppBGV.CommonMethods;
using WebAppBGV.DocumentServices;
using WebAppBGV.DocumentServices.Model;
using WebAppBGV.Security;
using static BAL.WhatsAppAPI;
using static WebAppBGV.CommonMethods.EmailSmartping;
namespace WebAppBGV.Areas.ClientArea.Controllers
{
    [CustomAuthorizeClient]
    public class ClientCandidateController : Controller
    {
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
        IPQClientCandiBulkUploadRepository repoPQClientCandiBulkUpload;
        IPQCandidateLoginRepository repoCandidate;
        IPQVerifiedUploadDocRepository repoPQVerifiedUploadDoc;
        ISpecialCheckInfoRepository repoSplCheckInfo;
        public readonly DocumentService _documentService;
        public ClientCandidateController(
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
            , IPQClientCandiBulkUploadRepository _repoPQClientCandiBulkUpload
            , IPQCandidateLoginRepository _repoCandidate
            , IPQVerifiedUploadDocRepository _repoPQVerifiedUploadDoc
            , ISpecialCheckInfoRepository _repoSplCheckInfo
            , DocumentService documentService
        )
        {
            repoDDL = _repoDDL;
            repoPQClient = _repoPQClient;
            _documentService = documentService;
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
            repoPQClientCandiBulkUpload = _repoPQClientCandiBulkUpload;
            repoCandidate = _repoCandidate;
            repoPQVerifiedUploadDoc = _repoPQVerifiedUploadDoc;
            repoSplCheckInfo = _repoSplCheckInfo;
        }
        #endregion

        // GET: ClientArea/ClientCandidate
        [SessionExpireFilterAttributeClient]
        public ActionResult Index()
        {
            return View();
        }

        #region ****** Manage Candidate ******

        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public ActionResult ManageClientCandidate(FormCollection formCollection, int page = 1, string msg = "", short id = 0)
        {

            if (clsCommonMethods.PDecrypt(msg) != "")
            {
                ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
            }

            string sort = "";
            string sortDir = "asc";
            string Search = "";
            string PersonalIds = "";
            short ClientRowID = 0;
            int ClientUserRowID = 0;
            ClientCustomize cc = new ClientCustomize();
            TempPQPersonalInfoListPagedModel model = new TempPQPersonalInfoListPagedModel();
            try
            {
                ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                BindClientListByLogInName(ClientRowID);
                ViewBag.clientRowId = ClientRowID.ToString();
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);

                //int pageSize = clsGlobalCommon.PageSize;
                int pageSize = 1000;

                #region Send Mail To Candidate
                if (!string.IsNullOrEmpty(PersonalIds))
                {
                    string spocEmail = "";
                    string cSpocEmail = "";
                    string mobileno = "";
                    string[] PersonalID = PersonalIds.Split(',');
                    string TrmFldPathDoc = "";
                    string fileName = "";
                    if (ClientRowID == 576)
                    {
                        TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "LOA.pdf";
                        fileName = Path.GetFileNameWithoutExtension("LOA.pdf");
                    }
                    else
                    {
                        TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CandidateAgreement.pdf";
                        fileName = Path.GetFileNameWithoutExtension("CandidateAgreement.pdf");
                    }

                    Attachment attachment;
                    attachment = new Attachment(Server.MapPath(TrmFldPathDoc));

                    string MailSignature = string.Empty;
                    string From = string.Empty;
                    string DisplayName = string.Empty;
                    string Cc = "";

                    clsSendMail mail;

                    var cSmtp = repoPQClient.GetPQClientSendMailFromById(ClientRowID);
                    DataTable dtEmail = new DataTable();

                    string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);
                    if (ClientAccessType == "3")
                    {
                        dtEmail = cc.userwise_ReportingEmailAccess(ClientUserRowID);
                        if (dtEmail.Rows.Count > 0)
                        {
                            spocEmail = dtEmail.Rows[0]["ClientUserEmailID"].ToString();
                            cSpocEmail = dtEmail.Rows[0]["ReportingTo_EmailID"].ToString();

                        }
                    }
                    else
                    {
                        dtEmail = cc.GetEmailStructureForAlert(ClientRowID, ClientUserRowID, "Case Registration");
                        if (dtEmail.Rows.Count > 0)
                        {
                            spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                            cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                            mobileno = dtEmail.Rows[0]["mobileno"].ToString();

                        }
                    }
                    Cc = cSpocEmail;
                    if (cSmtp != null && cSmtp.MailSendBy > 0)
                    {
                        mail = new clsSendMail(cSmtp.SMTPServer, cSmtp.SMTPPort, cSmtp.SMTPUserName, clsCommonMethods.PasswordDecrypt(cSmtp.SMTPPassword), Convert.ToBoolean(cSmtp.EnableSsl));

                        From = cSmtp.SMTPUserName;
                        DisplayName = cSmtp.ClientName.Trim();
                        Cc = string.Concat(Cc, ",", spocEmail);

                        MailSignature = string.Concat("<b>", cSmtp.ClientName.Trim(), "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", cSmtp.SMTPUserName, "'>", cSmtp.SMTPUserName, "</a>");
                    }
                    else
                    {
                        mail = new clsSendMail();
                        var signature = mail.GetCompanyMailSignature();

                        if (mobileno != "")
                            mobileno = "<b>Mobile: " + mobileno;

                        //string cmpSpocEmails = repoPQClient.GetCompanySPOCEmailIdFromPQClientMasterById(ClientRowID);
                        string ShowSpocId = spocEmail.ToLower();

                        ShowSpocId = ShowSpocId.Replace(",", ";");
                        string[] ShowSpocId1 = spocEmail.Split(';');
                        if (signature != null)
                        {

                            // For TATA Elxsi
                            if (ClientRowID == 635)
                            {
                                MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                //"<b>Mobile: ", signature.MobileNo,
                                "</b><br/><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", "bgv.helpdesk@securitas-india.com", "'>", "bgv.helpdesk@securitas-india.com", "</a>");
                            }
                            else if (ClientRowID == 249)
                            {
                                MailSignature = string.Concat("For Securitas India. <br /><br />");
                                MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                //"<b>Mobile: ", signature.MobileNo,
                                "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                "</b><br /><b>Contact No:</b><br />  <b>Satya :</b>+91 8377022632 <br />  <b>Pradeep Devrani :</b> +91 8373907723<br /><b>Himanshu Pathak :</b> +91 7291974515 ");
                                //Please mention the contact number of Satya - +91 8377022632 / Pradeep Devrani - 91 - 8373907723 / Himanshu Pathak - +91 7291974515
                            }
                            else if (ClientRowID == 277)
                            {
                                MailSignature = string.Concat("For Securitas India. <br /><br />");
                                MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                //"<b>Mobile: ", signature.MobileNo,
                                "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                "</b><br /><b>Contact No </b>:+91-8373907723");
                            }
                            else
                            {
                                MailSignature = string.Concat("For Securitas India. <br /><br />");
                                MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                //"<b>Mobile: ", signature.MobileNo,
                                "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>");
                            }

                        }



                        Cc = string.Concat(Cc, ",", ShowSpocId);

                        From = signature.GeneralEmail;
                        DisplayName = signature.CompanyName.Trim();
                    }
                    //int sum = 0;
                    foreach (var item in PersonalID)
                    {
                        int pID = Convert.ToInt32(item);

                        var personalData = repoPQPersonal.GetTempPQPersonalDetailsForSendMailById(pID);
                        var loginDetail = repoCandidate.GetCandidateLoginDetailsForMailSendById(pID);
                        DataTable d = new DataTable();
                        d = cc.CandidateEmailDocFormat(pID); // it should be subcheck family based on special check, 
                        StringBuilder docString = new StringBuilder();

                        foreach (DataRow dr in d.Rows)
                        {
                            if (dr["CheckFamilyRowID"].ToString() == "12") //address                            
                                docString.Append("<tr><td>Employment </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");

                            if (dr["CheckFamilyRowID"].ToString() == "11") //address                            
                                docString.Append("<tr><td>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");

                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                docString.Append("<tr><td>Address </td><td>Address Proof for Current & Permanent Address </td></tr>");

                            if (dr["CheckFamilyRowID"].ToString() == "26") //National Identity                            
                                docString.Append("<tr><td>National ID Check </td><td>ID Proof </td></tr>");

                            if (dr["CheckFamilyRowID"].ToString() == "40") //Credit check                            
                                docString.Append("<tr><td>Credit Check</td><td>PAN Card Copy</td></tr>");

                        }


                        if (loginDetail != null)
                        {
                            string body = string.Empty;
                            DataTable tblExperiece = cc.CheckCandidateExperienced(personalData.ClientRowID, personalData.PersonalRowID);
                            string MailSubject = "";
                            var templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplate");
                            if (templateDB.Rows.Count > 0)
                            {
                                body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                            }

                            if (ClientRowID == 679)
                            {
                                DataTable tblfractal = cc.CheckCandidateExperiencedByCandidateType(personalData.ClientRowID, personalData.PersonalRowID);


                                if (tblfractal.Rows.Count > 0)
                                {
                                    if (tblfractal.Rows[0][0].ToString() == "Existing Employee")
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployee");
                                    }
                                    else
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                    }
                                }
                                if (templateDB.Rows.Count > 0)
                                {
                                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                }

                            }
                            else if (ClientRowID == 576)
                            {
                                //---------Experienced-----------
                                if (tblExperiece.Rows.Count > 0)
                                {
                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LateralMailNagarro");
                                }
                                else
                                {
                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "NagarroFresherMail");
                                }
                                if (templateDB.Rows.Count > 0)
                                {
                                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                }
                            }
                            else if (ClientRowID == 635) //Tata Elxsi
                            {

                                DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                if (dtpackage.Rows.Count > 0)
                                {
                                    if (dtpackage.Rows[0][0].ToString() == "TEL_Experienced (ID_CRCP_CRCC_PADD_CADD_GDB_2EMP")
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 671, 1, "LoginDetailsSendToCandidateFotTataElxsiExp");
                                    }
                                    else
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresher");
                                    }
                                }
                                if (templateDB.Rows.Count > 0)
                                {
                                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                }

                            }


                            else if (ClientRowID == 647)
                            {

                                DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                if (dtpackage.Rows.Count > 0)
                                {
                                    if (dtpackage.Rows[0][0].ToString() == "NNSCIPL_Experienced (EDU_2EMP_PADD_GDB_CRCP_ID)")
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 737, 1, "LoginDetailsSendToCandidateForNovoNordiskExp");
                                    }
                                    else
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresher");
                                    }
                                }
                                if (templateDB.Rows.Count > 0)
                                {
                                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                }
                            }

                            else if (ClientRowID == 75)
                            {
                                DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                if (dtpackage.Rows.Count > 0)
                                {
                                    if (dtpackage.Rows[0][0].ToString() == "PWC_Package Assurance Campus (Pilot Project) (5EDU_8EMP_CRCC_CRCP_ID_CV_IDB_GDB)")
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 1684, 1, "CandidateLoginDetailsTemplate");
                                    }
                                    else
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplate");
                                    }
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                    }
                                }

                            }

                            if (ClientRowID == 103)
                            {
                                body = body.Replace("{Remarks}", "Please login and fill the details within 3 days");
                            }
                            else
                            {
                                body = body.Replace("{Remarks}", "Please login and fill the details within 2 days");
                            }
                            body = body.Replace("{document}", docString.ToString());
                            body = body.Replace("{CandidateName}", loginDetail.CandidateName);
                            body = body.Replace("{ClientRefId}", personalData.ClientRefID);
                            body = body.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                            body = body.Replace("{UName}", loginDetail.UserID);
                            body = body.Replace("{UPasswd}", clsCommonMethods.PasswordDecrypt(loginDetail.UPass.Trim()));
                            body = body.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());

                            body = body.Replace("{MailSignature}", MailSignature);
                            if (ClientRowID == 2)
                            {
                                string key = loginDetail.CandidateUserRowID + "_" + personalData.ClientRowID + "_" + personalData.PersonalRowID;
                                body = body.Replace("{MediaLink}", ConfigurationManager.AppSettings["CandidateMediaUrl"].ToString() + "?Key=" + clsCommonMethods.PasswordEncrypt(key));

                            }
                            string Subject = "";
                            // For Sutherland Global  249,277
                            if (ClientRowID == 249 || ClientRowID == 277)
                            {
                                Subject = loginDetail.CandidateName + "  Sutherland Background Verification by Securitas";
                                DisplayName = "Sutherland BGV by Securitas";
                            }
                            // For TATA Elxsi
                            else if (ClientRowID == 635)
                            {
                                body = body.Replace("{document}", "Documents Required<br /> Education: Degree Certificate / Final Year Marksheet < br /> " +
                                    "Address: Address Proof < br />   Nationality: ID Proof");
                                Subject = "Background verification";
                            }
                            else
                            {
                                Subject = loginDetail.CandidateName + "  Background Verification";
                            }


                            string To = loginDetail.CandidateEmail;
                            if (ClientRowID == 249 || ClientRowID == 277)
                            {
                                To = To + ";" + personalData.AlternateEmail;
                            }

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
                            DataTable Dt = cc.getCandidateMailstatus(pID.ToString());

                            int countSent = 0, countFailed = 0, totRec = 0;
                            bool flag = false;
                            if (Dt.Rows.Count > 0)
                            {

                                totRec = Dt.Rows.Count;
                                for (int i = 0; i < totRec; i++)
                                {


                                    if (Dt.Rows[0]["SentMailStatus"].ToString() == "0")
                                    {
                                        if (ClientRowID == 503)
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Ccmail, Subject, body);
                                        }
                                        else
                                        {
                                            if (ClientRowID == 257) //Biz 2 Credit Info Services Pvt. Ltd
                                            {
                                                string bcc = "";
                                                string Ccc = "";
                                                Ccc = spocEmail;
                                                bcc = cSpocEmail;
                                                //flag = mail.SendMail(DisplayName, From, Ccc, "", bcc, Subject, body); error 
                                                flag = mail.SendMail(DisplayName, From, ToMail, Ccc, bcc, Subject, body);
                                            }
                                            else if (ClientRowID == 249)//sutherlandglobal 
                                            {
                                                String CcSuthermail = "brij.bhushan@securitas-india.com";

                                                CcSuthermail = CcSuthermail + "," + Ccmail;

                                                flag = mail.SendMail(DisplayName, From, ToMail, CcSuthermail, Subject, body, attachment);
                                            }
                                            // For TATA Elxsi
                                            else if (ClientRowID == 635)
                                            {
                                                flag = mail.SendMail(DisplayName, From, ToMail, Ccmail, Subject, body);
                                            }
                                            else if (ClientRowID == 957)
                                            {
                                                flag = mail.SendMail(DisplayName, From, ToMail, Ccmail, Subject, body);
                                            }
                                            else if (ClientRowID == 957)
                                            {
                                                flag = mail.SendMail(DisplayName, From, ToMail, Ccmail, Subject, body);
                                            }
                                            else if (ClientRowID == 778)
                                            {

                                                String CcHHS = personalData.BusinessName;
                                                if (CcHHS == null)
                                                {
                                                    CcHHS = Ccmail.ToString();
                                                }
                                                else
                                                {
                                                    CcHHS = CcHHS + "," + Ccmail;
                                                }


                                                //CcHHS = CcHHS + "," + Ccmail;

                                                flag = mail.SendMail(DisplayName, From, ToMail, CcHHS, Subject, body, attachment);
                                            }
                                            else
                                            {
                                                flag = mail.SendMail(DisplayName, From, ToMail, Ccmail, Subject, body, attachment);
                                            }



                                        }

                                        if (flag)
                                        {
                                            if (ClientRowID == 249 || ClientRowID == 277) //sutherlandglobal and Sutherland Healthcare Solutions Pvt. Ltd.
                                            {
                                                try
                                                {
                                                    string Message = "";
                                                    Message = "Dear " + personalData.Name + Environment.NewLine + Environment.NewLine;
                                                    Message += "Please complete Sutherland BGV on email sent to your ID or below link." + Environment.NewLine;
                                                    Message += "ID:" + loginDetail.UserID + Environment.NewLine;
                                                    Message += "Pwd:" + clsCommonMethods.PasswordDecrypt(loginDetail.UPass) + Environment.NewLine;
                                                    Message += "Link:" + ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString() + Environment.NewLine;
                                                    Message += "Regards" + Environment.NewLine;
                                                    Message += "Securitas";
                                                    loginDetail.SMSText = Message;
                                                    loginDetail.PersonalId = pID;
                                                    SendSMS(loginDetail);
                                                }
                                                catch
                                                {

                                                }
                                            }


                                            if (ClientRowID == 635)
                                            {
                                                cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, body, ClientRowID, 1);
                                            }
                                            else if (ClientRowID == 636)
                                            {
                                                cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, body, ClientRowID, 1);
                                                cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, body, ClientRowID, 1);


                                            }
                                            else if (ClientRowID == 957)
                                            {
                                                cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, body, ClientRowID, 1);
                                                //cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, body, ClientRowID, 1);


                                            }
                                            else
                                            {
                                                cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, body, ClientRowID, 1);
                                            }

                                            if (ClientRowID == 635)
                                            {
                                                //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                                RemindermailTataElxsi(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                            }

                                            if (ClientRowID == 957)
                                            {
                                                RemindermailAstraZenca(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);

                                            }

                                            if (ClientRowID == 679 || ClientRowID == 725)
                                            {
                                                //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                                SaveReminderMail(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                            }
                                            UpdatePQCandidateLoginForSendMailViewModel loginModel = new UpdatePQCandidateLoginForSendMailViewModel();
                                            loginModel.CandidateUserRowID = loginDetail.CandidateUserRowID;
                                            loginModel.SentMailStatus = 1;
                                            loginModel.SentMailDate = DateTime.Now;
                                            repoCandidate.UpdateCandidateLoginSendMailStatusById(loginModel);
                                            //int Res = repoCandidate.SaveChanges();
                                            //if (Res > 0)
                                            //{
                                            countSent++;
                                            //result = true;

                                            //}
                                        }


                                        else
                                        {
                                            countFailed++;

                                        }
                                    }

                                }

                            }
                            TempData["ErrorMsg"] = "Out of " + totRec.ToString() + " Mail sent : " + countSent.ToString() + " failed : " + countFailed.ToString();

                        }
                        else
                        {
                            TempData["ErrorMsg"] = "Candidate login details not found!";
                        }
                        //sum++;
                        //if (sum >= 5)
                        //{
                        //    break;

                        //}



                    }
                }
                #endregion

                model = repoPQPersonal.GetTempCandidateListByClientBulkUpload(page, pageSize, sort, sortDir, Search, ClientRowID,ClientUserRowID);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                SearchModel.Search = Search;
                model.ItemSearch = SearchModel;
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [ValidateSessionFilter]
        public ActionResult ManageClientCandidate(FormCollection frm, string export = "")
        {
            int page = 1;
            string sort = "";
            string sortDir = "asc";
            string Search = Convert.ToString(frm["Search"]);
            string PersonalIds = frm["PersonalIds"];
            bool loaAdded = false;

            ClientCustomize cc = new ClientCustomize();
            TempPQPersonalInfoListPagedModel model = new TempPQPersonalInfoListPagedModel();
            PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
            try
            {
                short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                int pageSize = clsGlobalCommon.PageSize;
                ViewBag.ErrorMsg = "";
                if (export == "Excel")
                {
                    pageSize = 300;
                    model = repoPQPersonal.GetTempCandidateListByClientBulkUpload(page, pageSize, sort, sortDir, Search, ClientRowID);
                    var value = model.TempPQPersonalInfo;
                    var a = value.Select(i => new { i.Name, i.CandidateCode, i.Entityname, i.PackageName, i.CreatedDate, i.SendMailStatus, });
                    ExportToExcel(a, "ExportSentMail");
                }
                //////////////////////////////
                if (ViewBag.ErrorMsg == "")
                {
                    #region Send Mail To Candidate
                    if (!string.IsNullOrEmpty(PersonalIds))
                    {
                        string spocEmail = "";
                        string cSpocEmail = "";
                        string mobileno = "";
                        string[] PersonalID = PersonalIds.Split(',');
                        string TrmFldPathDoc = "";
                        string SendUserManulToCtrls = "";
                        string CtrlsfileName = "";
                        string fileName = "";
                        if (ClientRowID == 576)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "LOA.pdf";
                            fileName = Path.GetFileNameWithoutExtension("LOA.pdf");
                        }
                        else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Chargebee_LOA.docx";
                            fileName = Path.GetFileNameWithoutExtension("Chargebee_LOA.docx");
                        }
                        else if (ClientRowID == 1425)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "GlobalAuthorization-Australia.docx";
                            fileName = Path.GetFileNameWithoutExtension("GlobalAuthorization-Australia.docx");
                        }
                        else if (ClientRowID == 1428)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Genpact Privacy Notice and Consent Form template_Pre-Employment Screening_EU (Securitas).docx";
                            fileName = Path.GetFileNameWithoutExtension("Genpact Privacy Notice and Consent Form template_Pre-Employment Screening_EU (Securitas).docx");
                        }
                        else if (ClientRowID == 1497)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Capco Privacy Notice and Consent Form template_Pre-Employment Screening(Securitas).docx";
                            fileName = Path.GetFileNameWithoutExtension("Capco Privacy Notice and Consent Form template_Pre-Employment Screening(Securitas).docx");
                        }
                        else if (ClientRowID == 1429)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Global_Authorization-Nagarro.docx";
                            fileName = Path.GetFileNameWithoutExtension("Global_Authorization-Nagarro.docx");
                        }
                        else if (ClientRowID == 1449)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "GlobalAuthorization-Uniqus.docx";
                            fileName = Path.GetFileNameWithoutExtension("GlobalAuthorization-Uniqus.docx");
                        }
                        else if (ClientRowID == 1514)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "SILVERLINK-I.docx";
                            fileName = Path.GetFileNameWithoutExtension("SILVERLINK-I.docx");
                        }
                        else if (ClientRowID == 1481)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CtrlS_Datacenter-LOA.pdf";
                            fileName = Path.GetFileNameWithoutExtension("CtrlS_Datacenter-LOA.pdf");
                            SendUserManulToCtrls = ConfigurationManager.AppSettings["CandidateManual"].ToString() + "CandidateManual.pdf";
                            CtrlsfileName = Path.GetFileNameWithoutExtension("CandidateManual.pdf");
                        }
                        else if (ClientRowID == 1071)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "LOA-gallagher.pdf";
                            fileName = Path.GetFileNameWithoutExtension("LOA-gallagher.pdf");
                        }

                        else if (ClientRowID == 684)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Walsons_Allegis_LOA.pdf";
                            fileName = Path.GetFileNameWithoutExtension("Walsons_Allegis_LOA.pdf");
                        }
                        //else if (ClientRowID == 1747)
                        //{
                        //    if (ClientUserRowID == 5159)
                        //    {
                        //        // LOA for Ingram Micro India Private Limited
                        //        TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Ingram Micro India Private Limited - LOA.pdf";
                        //        fileName = Path.GetFileNameWithoutExtension("Ingram Micro India Private Limited - LOA.pdf");
                        //    }
                        //    else if (ClientUserRowID == 5160)
                        //    {
                        //        // LOA for Ingram MicroPlatform Technology India Private Limited
                        //        TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Ingram Micro Platform Technology India Private Limited - LOA.pdf";
                        //        fileName = Path.GetFileNameWithoutExtension("Ingram Micro Platform Technology India Private Limited - LOA.pdf");
                        //    }

                        //}
                        else if (ClientRowID == 1803)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "EXL LOA Document.pdf";
                            fileName = Path.GetFileNameWithoutExtension("EXL LOA Document.pdf");
                        }
                        else if (ClientRowID == 753)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Ford_LOA.pdf";
                            fileName = Path.GetFileNameWithoutExtension("Ford_LOA.pdf");
                        }
                        else if (ClientRowID != 1747) 
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CandidateAgreement.pdf";
                            fileName = Path.GetFileNameWithoutExtension("CandidateAgreement.pdf");
                        }
                        Attachment attachment;
                        Attachment attachment2;
                        if (ClientRowID == 1481)
                        {
                            attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                            attachment2 = new Attachment(Server.MapPath(SendUserManulToCtrls));
                        }
                        else if (ClientRowID == 1747)
                        {
                            attachment = null;
                            attachment2 = null;
                        }
                        else
                        {
                            attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                            attachment2 = null;
                        }

                        string MailSignature = string.Empty;
                        string From = string.Empty;
                        string DisplayName = string.Empty;
                        string Cc = "";
                        StringBuilder SignatureString = new StringBuilder();
                        clsSendMail mail;

                        var cSmtp = repoPQClient.GetPQClientSendMailFromById(ClientRowID);
                        DataTable dtEmail = new DataTable();

                        string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);
                        if (ClientAccessType == "3")
                        {
                            dtEmail = cc.userwise_ReportingEmailAccess(ClientUserRowID);
                            if (dtEmail.Rows.Count > 0)
                            {
                                spocEmail = dtEmail.Rows[0]["ClientUserEmailID"].ToString();
                                cSpocEmail = dtEmail.Rows[0]["ReportingTo_EmailID"].ToString();

                            }
                        }
                        else
                        {
                            int intPersonalrowid = 0;
                            if (PersonalID.Length > 0)
                            {
                                int.TryParse(PersonalID[0].Trim(), out intPersonalrowid);
                            }                            
                            dtEmail = cc.GetEmailStructureForAlert(ClientRowID, ClientUserRowID, "Case Registration", intPersonalrowid);
                            if (dtEmail.Rows.Count > 0)
                            {
                                spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                                cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                                mobileno = dtEmail.Rows[0]["mobileno"].ToString();

                            }
                        }
                        Cc = cSpocEmail;
                        if (cSmtp != null && cSmtp.MailSendBy > 0)
                        {
                            mail = new clsSendMail(cSmtp.SMTPServer, cSmtp.SMTPPort, cSmtp.SMTPUserName, clsCommonMethods.PasswordDecrypt(cSmtp.SMTPPassword), Convert.ToBoolean(cSmtp.EnableSsl));

                            From = cSmtp.SMTPUserName;
                            DisplayName = cSmtp.ClientName.Trim();
                            Cc = string.Concat(Cc, ",", spocEmail);

                            MailSignature = string.Concat("<b>", cSmtp.ClientName.Trim(), "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", cSmtp.SMTPUserName, "'>", cSmtp.SMTPUserName, "</a>");
                        }
                        else
                        {
                            mail = new clsSendMail();
                            var signature = mail.GetCompanyMailSignature();

                            if (mobileno != "")
                                mobileno = "<b>Mobile: " + mobileno;
                            string ShowSpocId = spocEmail.ToLower();

                            ShowSpocId = ShowSpocId.Replace(",", ";");
                            string[] ShowSpocId1 = spocEmail.Split(';');
                            if (signature != null)
                            {
                                // For TATA Elxsi
                                if (ClientRowID == 635)
                                {
                                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                    //"<b>Mobile: ", signature.MobileNo,
                                    "</b><br/><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", "bgv.helpdesk@securitas-india.com", "'>", "bgv.helpdesk@securitas-india.com", "</a>");
                                }
                                else if (ClientRowID == 249)
                                {
                                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                    //"<b>Mobile: ", signature.MobileNo,
                                    "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                    "</b><br /><b>Contact No:</b><br />  <b>Satya :</b>+91 8377022632 <br />  <b>Pradeep Devrani :</b> +91 8373907723<br /><b>Himanshu Pathak :</b> +91 7291974515 ");
                                    //Please mention the contact number of Satya - +91 8377022632 / Pradeep Devrani - 91 - 8373907723 / Himanshu Pathak - +91 7291974515
                                }
                                else if (ClientRowID == 277)
                                {
                                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                    //"<b>Mobile: ", signature.MobileNo,
                                    "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                    "</b><br /><b>Contact No </b>:+91-8373907723");
                                }
                                else
                                {
                                    MailSignature = string.Empty;
                                    MailSignature = string.Concat(ShowSpocId1[0]);
                                }
                            }

                            Cc = string.Concat(Cc, ",", ShowSpocId);
                            From = signature.GeneralEmail;
                            DisplayName = signature.CompanyName.Trim();
                            DataTable MailDT = new DataTable();
                            MailDT = cc.GetSpoceNameAndEmailForMailSignature(Convert.ToInt32(ClientRowID));
                            if (MailDT.Rows.Count > 0)
                            {
                                foreach (DataRow dr in MailDT.Rows)
                                {
                                    if (ClientRowID == 1693)
                                    {
                                        SignatureString.Append("<table style='width:100%;'><tr><td style='width: 92%;color:#FC273F;'>"+ dr["EmailIDs"].ToString() + "</td></tr></table>");
                                    }
                                    else
                                    {
                                        SignatureString.Append("<table style='width:100%;'><tr><td style='color:#031F30;font-weight:bold;'>E-MailID - </td><td style='width: 92%;color:#FC273F;text-align:justify;'>" + dr["EmailIDs"].ToString() + "</td></tr></table>");
                                    }

                                }
                            }
                        }

                        foreach (var item in PersonalID)
                        {
                            int pID = Convert.ToInt32(item);
                            var personalData = repoPQPersonal.GetTempPQPersonalDetailsForSendMailById(pID);
                            var loginDetail = repoCandidate.GetCandidateLoginDetailsForMailSendById(pID);
                            DataTable d = new DataTable();
                            StringBuilder docString = new StringBuilder();

                            if (ClientRowID == 1425 || ClientRowID == 1428 || ClientRowID == 1222)
                            {
                                d = cc.GetDocumentForVerificationList(pID, ClientRowID);
                                foreach (DataRow dr in d.Rows)
                                {
                                    docString.Append("<tr><td style='padding:3px;'>" + dr["SubCheckName"].ToString() + " </td><td>" + dr["DocumentForVerification"].ToString() + " </td></tr>");
                                }
                            }
                            else if (ClientRowID == 193)
                            {
                                d = cc.GetDocumentForVerificationList(pID, ClientRowID);
                                foreach (DataRow dr in d.Rows)
                                {
                                    docString.Append("<tr><td style='padding:3px;'>" + dr["SubCheckName"].ToString() + " </td><td>" + dr["DocumentForVerification"].ToString() + " </td></tr>");
                                }
                            }
                            else
                            {
                                d = cc.CandidateEmailDocFormat(pID); // it should be subcheck family based on special check, 
                                foreach (DataRow dr in d.Rows)
                                {
                                    if (ClientRowID == 1357)
                                    {
                                        //---- Employment Check Start from here ----//
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Past two Employments </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");
                                    }
                                    else if (ClientRowID == 1425)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Any available document. Note: Two employment details prior to Chargebee.</ td></tr>");
                                    }
                                    else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Experience letter / Service Certificate</td></tr>");
                                    }
                                    else if (ClientRowID == 1428)
                                    {

                                    }
                                    else if (ClientRowID == 1449)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Experience letter, Service Certificate, Other</td></tr>");
                                    }
                                    else if (ClientRowID == 1481)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter</td></tr>");
                                    }
                                    else if (ClientRowID == 636)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Current Company Employment<br> • Experience Letter / Relieving Letter<br> • Salary Slip for last 3 months <br> Previous Employment(only latest 2 employers)<br> • Experience Letter / Relieving Letter </ td></tr>");
                                    }

                                    else if (ClientRowID == 1693)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12")
                                        {
                                            DataTable dt1 = new DataTable();
                                            dt1 = cc.GetSubCheckNameByPackage(pID, 12);

                                            if (dt1.Rows.Count > 0)
                                            {
                                                foreach (DataRow item1 in dt1.Rows)
                                                {
                                                    if (Convert.ToInt32(item1["SubCheckRowID"]) == 68)
                                                    {
                                                        docString.Append("<tr><td style='padding:3px;'>Current Employment Verification</td><td>Appointment, Relieving, Exp letter or salary slips</td></tr>");
                                                    }
                                                    if (Convert.ToInt32(item1["SubCheckRowID"]) == 69)
                                                    {
                                                        docString.Append("<tr><td style='padding:3px;'>Previous Employment Verification 1</td><td>Appointment, Relieving, Exp letter or salary slips</td></tr>");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //Employment
                                    //if (dr["CheckFamilyRowID"].ToString() == "2")
                                    //{
                                    //    docString.Append("<tr><td style='padding:3px;'>Current Address Verification </td><td>•Current Address proof (Aadhar, passport, driving license etic</td></tr>");
                                    //    docString.Append("<tr><td style='padding:3px;'>Permanent Address Verification </td><td>•Permanent Address proof (Aadhar, passport, driving license etic</td></tr>");
                                    //} //Address
                                    //if (dr["CheckFamilyRowID"].ToString() == "5")
                                    //{
                                    //    docString.Append("<tr><td style='padding:3px;'>Criminal Verification through E-Court - Permanent Address Check</td><td>Permanent Address proof (Aadhar, passport, driving license etic)</td></tr>");
                                    //} //CRIMINAL
                                    //if (dr["CheckFamilyRowID"].ToString() == "11")
                                    //{
                                    //    docString.Append("<tr><td style='padding:3px;'>Highest Education</td><td>Final Year marksheet/certificate</td></tr>");
                                    //} //Education
                                    //if (dr["CheckFamilyRowID"].ToString() == "26")
                                    //{
                                    //    docString.Append("<tr><td style='padding:3px;'>PAN No Verification</td><td>•PAN card </td></tr>");
                                    //    docString.Append("<tr><td style='padding:3px;'>CIBIL</td><td>•PAN card</td></tr>");
                                    //} //ID 
                                    else if (ClientRowID == 709)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "5") // Criminal
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Police Verification </td><td>Permanent Address Proof & Passport Size Photo</td></tr>");
                                        }
                                    }
                                    else if (ClientRowID == 1803)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") // Employment
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter</td></tr>");
                                        }

                                        if (dr["CheckFamilyRowID"].ToString() == "11") // Education
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Final Year Marksheet / Degree Certificate</td></tr>");
                                        }

                                        // Add LOA only once
                                        if (!loaAdded)
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>LOA </td><td>Attached EXL LOA should be signed and uploaded in portal</td></tr>");
                                            loaAdded = true;
                                        }
                                    }
                                    else if (ClientRowID == 1804)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") // Employment
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter</td></tr>");
                                        }
                                        if (dr["CheckFamilyRowID"].ToString() == "11") // Education
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Consolidated Mark Sheet / Degree Certificate / Provisional Certificate / Final Year Marksheet</td></tr>");
                                        }
                                        if (dr["CheckFamilyRowID"].ToString() == "2")   //Address
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Address </td><td>Any One Address Proof</td></tr>");
                                        }
                                        if (dr["CheckFamilyRowID"].ToString() == "5")   //Court
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Court Record Check</td><td>Aadhar Card Copy</td></tr>");
                                        }
                                        if (dr["CheckFamilyRowID"].ToString() == "40")  //Global Database Check
                                            docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>PAN Card Copy</td></tr>");
                                    }
                                    else if (ClientRowID == 1834)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment 
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Previous Employment Documents </td><td>Service/Experience Letter, Relieving Letter, Salary Slips, etc., as applicable.</ td></tr>");
                                        }
                                        if (dr["CheckFamilyRowID"].ToString() == "2") //address   
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Address </td><td>Address Proof: Rental Agreement & Receipt, Masked Aadhaar, Electricity/Gas Bill, etc </td></tr>");
                                        }
                                        if (dr["CheckFamilyRowID"].ToString() == "26")// Identity
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Aadhaar Masked, PAN, Driving Licence, UAN Card, etc. </td></tr>");
                                        }
                                    }
                                    else
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");
                                    }
                                    //---- Employment Check End from here ----//

                                    //---- Education Check Start from here ----//
                                    if (ClientRowID == 1355 || ClientRowID == 1348)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Marksheet/Provisional Degree Certificate </td></tr>");
                                    }
                                    else if (ClientRowID == 1318)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");
                                        if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                            docString.Append("<tr><td style='padding:3px;'>Sanad Certification </td><td>Required Bar Council Certificate</td></tr>");
                                    }
                                    else if (ClientRowID == 1425)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Transcript/Degree Certificate</td></tr>");
                                    }
                                    else if (ClientRowID == 1428)
                                    {

                                    }
                                    else if (ClientRowID == 1693)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "11")
                                        {
                                            DataTable dt1 = new DataTable();
                                            dt1 = cc.GetSubCheckNameByPackage(pID, 11);

                                            if (dt1.Rows.Count > 0)
                                            {
                                                foreach (DataRow item1 in dt1.Rows)
                                                {
                                                    if (Convert.ToInt32(item1["SubCheckRowID"]) == 56)
                                                    {
                                                        docString.Append("<tr><td style='padding:3px;'>Highest Education</td><td>Final Year marksheet/certificate</td></tr>");
                                                    }

                                                }
                                            }

                                        } //Education
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");
                                        }
                                        //---- Education Check End from here ----//

                                        //---- Address Check Start from here ----//
                                        if (ClientRowID == 1425)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>Driver's License/Utility Bill</td></tr>");
                                        }
                                        if (ClientRowID == 1395)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>SSN Number</td></tr>");
                                        }
                                        else if (ClientRowID == 1428)
                                        {

                                        }
                                        else if (ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>National ID Proof</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>• Address Proof </td></tr>");
                                        }
                                        else if (ClientRowID == 1693)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2")
                                            {
                                                DataTable dt1 = new DataTable();
                                                dt1 = cc.GetSubCheckNameByPackage(pID, 2);

                                                if (dt1.Rows.Count > 0)
                                                {
                                                    foreach (DataRow item1 in dt1.Rows)
                                                    {
                                                        if (Convert.ToInt32(item1["SubCheckRowID"]) == 2)
                                                        {
                                                            docString.Append("<tr><td style='padding:3px;'>Current Address Verification </td><td> Current Address proof (Aadhar, passport, driving license etic</td></tr>");
                                                        }
                                                        if (Convert.ToInt32(item1["SubCheckRowID"]) == 13)
                                                        {
                                                            docString.Append("<tr><td style='padding:3px;'>Permanent Address Verification </td><td> Permanent Address proof (Aadhar, passport, driving license etic</td></tr>");
                                                        }
                                                    }
                                                }
                                            }
                                        }//Address
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>Address Proof for Current & Permanent Address </td></tr>");
                                        }
                                        //---- Address Check End from here ----//

                                        //---- National Check Start from here ----//
                                        if (ClientRowID == 1355 || ClientRowID == 1348)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td>ID Verification</td><td>Passport preferred</td></tr>");
                                        }
                                        else if (ClientRowID == 1425 || ClientRowID == 1428 || ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td>National ID Check </td><td>National ID Proof</td></tr>");
                                        }
                                        else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td>National ID Check </td><td>Passport / National ID</td></tr>");
                                        }
                                        else if (ClientRowID == 559)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Pan Card</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Photo ID Proof </td></tr>");
                                        }
                                        else if (ClientRowID == 648)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>ID Proof – Pan Card and Aadhar Card</td></tr>");
                                        }
                                        else if (ClientRowID == 1693)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                            {
                                                DataTable dt1 = new DataTable();
                                                dt1 = cc.GetSubCheckNameByPackage(pID, 26);

                                                if (dt1.Rows.Count > 0)
                                                {
                                                    foreach (DataRow item1 in dt1.Rows)
                                                    {
                                                        if (Convert.ToInt32(item1["SubCheckRowID"]) == 133)
                                                        {
                                                            docString.Append("<tr><td style='padding:3px;'>PAN No Verification</td><td> PAN card </td></tr>");
                                                        }
                                                    }
                                                }
                                            }
                                        } //ID 
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>ID Proof </td></tr>");
                                        }
                                        //---- National Check End from here ----//

                                        //---- Special Check Start from here ----//
                                        if (ClientRowID == 487)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "40")
                                                docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>Required Aadhar Card</td></tr>");
                                        }
                                        else if (ClientRowID == 1470)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "40")
                                                docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>NA</td></tr>");
                                        }
                                        else if (ClientRowID == 193)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "40")
                                                docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>NA</td></tr>");
                                        }
                                        else if (ClientRowID == 1693)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "40")
                                            {
                                                DataTable dt1 = new DataTable();
                                                dt1 = cc.GetSubCheckNameByPackage(pID, 40);

                                                if (dt1.Rows.Count > 0)
                                                {
                                                    foreach (DataRow item1 in dt1.Rows)
                                                    {
                                                        if (Convert.ToInt32(item1["SubCheckRowID"]) == 206)
                                                        {
                                                            docString.Append("<tr><td style='padding:3px;'>CIBIL</td><td> PAN card</td></tr>");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //---- Special Check End from here ----//

                                        //---- Criminal Check Start from here ----//
                                        if (ClientRowID == 487 || ClientRowID == 1428 || ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>National ID Proof</td></tr>");
                                        }
                                        else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>Passport / National ID</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>• Address Proof </td></tr>");
                                        }
                                        else if (ClientRowID == 648)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>Require – Address proof, ID Proof and Passport Size Photo</td></tr>");
                                        }
                                        else if (ClientRowID == 1470)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>National ID Proof</td></tr>");
                                        }
                                        else if (ClientRowID == 1693)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                            {
                                                DataTable dt1 = new DataTable();
                                                dt1 = cc.GetSubCheckNameByPackage(pID, 5);

                                                if (dt1.Rows.Count > 0)
                                                {
                                                    foreach (DataRow item1 in dt1.Rows)
                                                    {
                                                        if (Convert.ToInt32(item1["SubCheckRowID"]) == 23)
                                                        {
                                                            docString.Append("<tr><td style='padding:3px;'>Criminal Verification through E-Court - Permanent Address Check</td><td>Permanent Address proof (Aadhar, passport, driving license etic)</td></tr>");
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        //CRIMINAL
                                        //---- Criminal Check End from here ----//
                                    }


                                    if (ClientRowID == 51)
                                    {
                                        docString.Append("<tr><td style='padding:3px;'>Address </td><td>Aadhar Card/Voter ID/Driving License/Passport/Voter ID/Electric Bill/Rent Agreement/Others.</td></tr>");
                                    }
                                }
                            }

                            if (loginDetail != null)
                            {
                                string body = string.Empty;
                                string bodySpocAndClient = string.Empty;
                                string SaveWPassMail = string.Empty;
                                DataTable tblExperiece = cc.CheckCandidateExperienced(personalData.ClientRowID, personalData.PersonalRowID);

                                string MailSubject = "";
                                string SpocAndClientMailSubject = "";
                                string MailTemplateName = "";
                                string MailTemplateNameSpoc = "";
                                int MailClientRowid = 0;
                                var templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplate");
                                if (templateDB.Rows.Count > 0)
                                {
                                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                    MailTemplateName = Convert.ToString(templateDB.Rows[0]["TemplateName"]);
                                    MailClientRowid = Convert.ToInt32(templateDB.Rows[0]["ClientRowId"]);
                                }
                                var templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplateForSpocAndClient");
                                if (templateDBSpocAndClient.Rows.Count > 0)
                                {
                                    bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                    MailTemplateNameSpoc = Convert.ToString(templateDBSpocAndClient.Rows[0]["TemplateName"]);
                                    SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                }

                                if (ClientRowID == 679)
                                {
                                    DataTable tblfractal = cc.CheckCandidateExperiencedByCandidateType(personalData.ClientRowID, personalData.PersonalRowID);
                                    if (tblfractal.Rows.Count > 0)
                                    {
                                        if (tblfractal.Rows[0][0].ToString() == "Existing Employee")
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployee");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployeeForSpocAndClient");
                                        }
                                        else
                                        {
                                            DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                            if (dtpackage.Rows.Count > 0)
                                            {
                                                if (dtpackage.Rows[0][0].ToString().Trim() == "FAPL_IAL2")
                                                {
                                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 2926, 1, "FractelNewJoinertemp");
                                                    templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 2926, 1, "FractelNewJoinertempForSpocAndClient");
                                                }
                                                else
                                                {
                                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                                    templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                                }
                                            }
                                            else
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                            }
                                            // templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                            //  templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                        }
                                    }
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                        if (ClientRowID == 679 || ClientRowID == 2)
                                        {
                                            SaveWPassMail = Convert.ToString(templateDB.Rows[0]["WLDMailTemplate"]);
                                        }
                                    }
                                    if (templateDBSpocAndClient.Rows.Count > 0)
                                    {
                                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                    }
                                }

                                else if (ClientRowID == 576)
                                {
                                    //---------Experienced-----------
                                    if (tblExperiece.Rows.Count > 0)
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LateralMailNagarro");
                                        templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LateralMailNagarroForSpocAndClient");
                                    }
                                    else
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "NagarroFresherMail");
                                        templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "NagarroFresherMailForSpocAndClient");
                                    }
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                    }
                                    if (templateDBSpocAndClient.Rows.Count > 0)
                                    {
                                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                    }
                                }
                                else if (ClientRowID == 635) //Tata Elxsi
                                {

                                    DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                    if (dtpackage.Rows.Count > 0)
                                    {
                                        if (dtpackage.Rows[0][0].ToString() == "TEL_Experienced (ID_CRCP_CRCC_PADD_CADD_GDB_2EMP")
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 671, 1, "LoginDetailsSendToCandidateFotTataElxsiExp");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiExpSpocAndClient");
                                        }
                                        else
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresher");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresherSpocAndClient");
                                        }
                                    }
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                    }
                                    if (templateDBSpocAndClient.Rows.Count > 0)
                                    {
                                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                    }
                                }


                                else if (ClientRowID == 647)
                                {

                                    DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                    if (dtpackage.Rows.Count > 0)
                                    {
                                        if (dtpackage.Rows[0][0].ToString() == "NNSCIPL_Experienced (EDU_2EMP_PADD_GDB_CRCP_ID)")
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 737, 1, "LoginDetailsSendToCandidateForNovoNordiskExp");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskExpForSpocAndClient");
                                        }
                                        else
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresher");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresherForSpocAndClient");
                                        }
                                    }
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                    }
                                    if (templateDBSpocAndClient.Rows.Count > 0)
                                    {
                                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                    }
                                }

                                if (ClientRowID == 103)
                                {
                                    body = body.Replace("{Remarks}", "Please login and fill the details within 3 days");
                                    bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Please login and fill the details within 3 days");
                                }
                                else if (ClientRowID == 1530)
                                {
                                    body = body.Replace("{Remarks}", "Kindly fill the form within 2 working days");
                                    bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Kindly fill the form within 2 working days");
                                }
                                else
                                {
                                    body = body.Replace("{Remarks}", "Please login and fill the details within 2 days");
                                    bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Please login and fill the details within 2 days");
                                }

                                string txtFolderPath = ConfigurationManager.AppSettings["mailimages"].ToString();
                                LinkedResource Triangleres = new LinkedResource(Server.MapPath(txtFolderPath) + "Triangle.png", MediaTypeNames.Image.Jpeg);
                                Triangleres.ContentId = Guid.NewGuid().ToString();
                                string triangle = @"<img src='cid:" + Triangleres.ContentId + @"' width='25%'/>";

                                LinkedResource Securitassec = new LinkedResource(Server.MapPath(txtFolderPath) + "Securitas-logo.png", MediaTypeNames.Image.Jpeg);
                                Securitassec.ContentId = Guid.NewGuid().ToString();
                                string securitas = @"<img src='cid:" + Securitassec.ContentId + @"' width='25%'/>";

                                LinkedResource Walsonssec = new LinkedResource(Server.MapPath(txtFolderPath) + "Walsons-Temp-img.JPG", MediaTypeNames.Image.Jpeg);
                                Walsonssec.ContentId = Guid.NewGuid().ToString();
                                string walsons = @"<img src='cid:" + Walsonssec.ContentId + @"' width='25%'/>";

                                LinkedResource BGVdottedImg = new LinkedResource(Server.MapPath(txtFolderPath) + "BGV-dotted-design.png", MediaTypeNames.Image.Jpeg);
                                BGVdottedImg.ContentId = Guid.NewGuid().ToString();
                                string BGVdotted = @"<img src='cid:" + BGVdottedImg.ContentId + @"' width='100%'/>";

                                LinkedResource RedbulllogoImg = new LinkedResource(Server.MapPath(txtFolderPath) + "Redbullogo.png", MediaTypeNames.Image.Jpeg);
                                RedbulllogoImg.ContentId = Guid.NewGuid().ToString();
                                string redbulllogo = @"<img src='cid:" + RedbulllogoImg.ContentId + @"' width='30%'/>";

                                LinkedResource JadeGlobalLogoIMG = new LinkedResource(Server.MapPath(txtFolderPath) + "JadeGlobalLogo.png", MediaTypeNames.Image.Jpeg);
                                JadeGlobalLogoIMG.ContentId = Guid.NewGuid().ToString();
                                string JadeGlobalLogo = @"<img src='cid:" + JadeGlobalLogoIMG.ContentId + @"' width='30%'/>";

                                LinkedResource SmartChipLogoIMG = new LinkedResource(Server.MapPath(txtFolderPath) + "Picture1.jpg", MediaTypeNames.Image.Jpeg);
                                SmartChipLogoIMG.ContentId = Guid.NewGuid().ToString();
                                string SmartChipLogo = @"<img src='cid:" + SmartChipLogoIMG.ContentId + @"' width='30%'/>";


                                string ClientNameMail = Session["ClientNameMail"].ToString();
                                //Candidate Template Details
                                string UPass = clsCommonMethods.PasswordDecrypt(loginDetail.UPass.Trim());
                                body = body.Replace("{document}", docString.ToString());
                                body = body.Replace("{CandidateName}", loginDetail.CandidateName);
                                body = body.Replace("#CompanyName", ClientNameMail);
                                body = body.Replace("{ClientRefId}", personalData.ClientRefID);
                                body = body.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                body = body.Replace("{UName}", loginDetail.UserID);
                                body = body.Replace("{UPasswd}", UPass);
                                body = body.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                                body = body.Replace("{triangle}", triangle);
                                body = body.Replace("{securitas}", securitas);
                                body = body.Replace("{walsons}", walsons);
                                body = body.Replace("{BGVdotted}", BGVdotted);
                                body = body.Replace("{redbulllogo}", redbulllogo);
                                body = body.Replace("{JadeLogo}", JadeGlobalLogo);
                                body = body.Replace("{SmatLogo}", SmartChipLogo);
                                body = body.Replace("{MailSignature}", SignatureString.ToString());

                                //Spoc And Client Template Details
                                var Resendlinks = "https://bgv.securitas-india.com/account/ResendLink/?PersonalIds=" + pID.ToString();
                                bodySpocAndClient = bodySpocAndClient.Replace("{Resendlink}", Resendlinks);
                                bodySpocAndClient = bodySpocAndClient.Replace("{document}", docString.ToString());
                                bodySpocAndClient = bodySpocAndClient.Replace("{CandidateName}", loginDetail.CandidateName);
                                bodySpocAndClient = bodySpocAndClient.Replace("#CompanyName", ClientNameMail);
                                bodySpocAndClient = bodySpocAndClient.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                bodySpocAndClient = bodySpocAndClient.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                                bodySpocAndClient = bodySpocAndClient.Replace("{triangle}", triangle);
                                bodySpocAndClient = bodySpocAndClient.Replace("{securitas}", securitas);
                                bodySpocAndClient = bodySpocAndClient.Replace("{walsons}", walsons);
                                bodySpocAndClient = bodySpocAndClient.Replace("{BGVdotted}", BGVdotted);
                                bodySpocAndClient = bodySpocAndClient.Replace("{redbulllogo}", redbulllogo);
                                bodySpocAndClient = bodySpocAndClient.Replace("{JadeLogo}", JadeGlobalLogo);
                                bodySpocAndClient = bodySpocAndClient.Replace("{SmatLogo}", SmartChipLogo);
                                bodySpocAndClient = bodySpocAndClient.Replace("{MailSignature}", MailSignature);

                                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                                alternateView.LinkedResources.Add(Triangleres);
                                alternateView.LinkedResources.Add(Securitassec);
                                alternateView.LinkedResources.Add(Walsonssec);
                                alternateView.LinkedResources.Add(BGVdottedImg);
                                alternateView.LinkedResources.Add(RedbulllogoImg);
                                alternateView.LinkedResources.Add(JadeGlobalLogoIMG);
                                alternateView.LinkedResources.Add(SmartChipLogoIMG);

                                AlternateView alternateViewSpocAndClient = AlternateView.CreateAlternateViewFromString(bodySpocAndClient, null, MediaTypeNames.Text.Html);
                                alternateViewSpocAndClient.LinkedResources.Add(Triangleres);
                                alternateViewSpocAndClient.LinkedResources.Add(Securitassec);
                                alternateViewSpocAndClient.LinkedResources.Add(Walsonssec);
                                alternateViewSpocAndClient.LinkedResources.Add(BGVdottedImg);
                                alternateViewSpocAndClient.LinkedResources.Add(RedbulllogoImg);
                                alternateViewSpocAndClient.LinkedResources.Add(JadeGlobalLogoIMG);
                                alternateViewSpocAndClient.LinkedResources.Add(SmartChipLogoIMG);

                                if (ClientRowID == 2)
                                {
                                    string key = loginDetail.CandidateUserRowID + "_" + personalData.ClientRowID + "_" + personalData.PersonalRowID;
                                    body = body.Replace("{MediaLink}", ConfigurationManager.AppSettings["CandidateMediaUrl"].ToString() + "?Key=" + clsCommonMethods.PasswordEncrypt(key));

                                }
                                string Subject = "";
                                // For Sutherland Global  249,277
                                if (ClientRowID == 249 || ClientRowID == 277)
                                {
                                    Subject = loginDetail.CandidateName + "  Sutherland Background Verification by Securitas";
                                    DisplayName = "Sutherland BGV by Securitas";
                                }
                                // For TATA Elxsi
                                else if (ClientRowID == 635)
                                {
                                    body = body.Replace("{document}", "Documents Required<br /> Education: Degree Certificate / Final Year Marksheet < br /> " +
                                        "Address: Address Proof < br />   Nationality: ID Proof");
                                    Subject = "Background verification";
                                }
                                else if (ClientRowID == 957)
                                {
                                    Subject = "BGV Initiated: " + Regex.Replace(loginDetail.CandidateName, @"\t|\n|\r", "") + " - " + personalData.ClientRefID;
                                }
                                else
                                {
                                    Subject = Regex.Replace(loginDetail.CandidateName, @"\t|\n|\r", "") + "  Background Verification";
                                }
                                string To = loginDetail.CandidateEmail;
                                if (ClientRowID == 249 || ClientRowID == 277 || ClientRowID == 193)
                                {
                                    Cc = Cc + ";" + personalData.AlternateEmail;
                                }

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
                                DataTable Dt = cc.getCandidateMailstatus(pID.ToString());

                                int countSent = 0, countFailed = 0, totRec = 0;
                                bool flag = false;
                                bool flagspocandclient = false;
                                if (Dt.Rows.Count > 0)
                                {
                                    try
                                    {
                                        if (Convert.ToString(personalData.MobileNo) != "" && !UPass.Contains("Bad Data"))
                                        {
                                            MessageStatus objMessage = new MessageStatus();
                                            WhatsAppAPI objs = new WhatsAppAPI();
                                            var link = "https://bgv.securitas-india.com/Account/CandidateLogin";
                                            objMessage = objs.SendWhatsAppMessage("+91" + personalData.MobileNo, "case_registration_candidate_invite_mode", loginDetail.CandidateName, "media_template", ClientNameMail, To, link);
                                            cc.sp_AddAuditWhatsAppAPI(personalData.PersonalRowID, objMessage.message, objMessage.status, "Candidate Invite");
                                        }
                                    }
                                    catch
                                    {

                                    }
                                    totRec = Dt.Rows.Count;
                                    for (int i = 0; i < totRec; i++)
                                    {
                                        if (Dt.Rows[0]["SentMailStatus"].ToString() == "0")
                                        {
                                            if (!UPass.ToLower().Contains("bad data") && UPass != "")
                                            {
                                                if (ClientRowID == 503)
                                                {
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                }
                                                else
                                                {
                                                    if (ClientRowID == 257) //Biz 2 Credit Info Services Pvt. Ltd
                                                    {
                                                        string bcc = "";
                                                        string Ccc = "";
                                                        Ccc = spocEmail;
                                                        bcc = cSpocEmail;
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Ccc, bcc, Subject, bodySpocAndClient);
                                                        //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, Ccc, bcc,Subject, bodySpocAndClient, null, null);
                                                    }
                                                    else if (ClientRowID == 249)//sutherlandglobal 
                                                    {
                                                        String CcSuthermail = "brij.bhushan@securitas-india.com";

                                                        CcSuthermail = CcSuthermail + "," + Ccmail;

                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, CcSuthermail, Subject, bodySpocAndClient, attachment, null);
                                                    }
                                                    // For TATA Elxsi
                                                    else if (ClientRowID == 635)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);

                                                    }
                                                    else if (ClientRowID == 957)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                    }
                                                    else if (ClientRowID == 753)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment, "Ford_LOA.pdf");
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClientExl(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, null, "Ford_LOA.pdf");
                                                    }
                                                    else if (ClientRowID == 1312)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                    }
                                                    else if (ClientRowID == 778)
                                                    {

                                                        String CcHHS = personalData.BusinessName;
                                                        if (CcHHS == null)
                                                        {
                                                            CcHHS = Ccmail.ToString();
                                                        }
                                                        else
                                                        {
                                                            CcHHS = CcHHS + "," + Ccmail;
                                                        }
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, CcHHS, Subject, bodySpocAndClient, attachment, null);

                                                    }
                                                    else if (ClientRowID == 1005 || ClientRowID == 2)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);

                                                    }
                                                    else if (ClientRowID == 889)
                                                    {
                                                        flag = mail.SendMailPurgWithoutCC(DisplayName, From, ToMail, Subject, body, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, cSpocEmail, Subject, bodySpocAndClient, attachment, null);
                                                    }
                                                    else if (ClientRowID == 974)
                                                    {
                                                        flag = mail.SendMailAdidas(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.SendMailAdidasForSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, body);
                                                    }
                                                    else if ((ClientRowID == 1496 || MailClientRowid == 2 || MailClientRowid == 193 || MailClientRowid == 1665 || MailClientRowid == 1693) && (MailTemplateName == "CandidateLoginDetailsTemplate" || MailTemplateNameSpoc == "CandidateLoginDetailsTemplateForSpocAndClient"))
                                                    {
                                                        if (!UPass.Contains("Bad Data"))

                                                        {
                                                            if (ClientRowID == 1693 || ClientRowID == 992 || ClientRowID == 1125)
                                                            {
                                                                DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                                                var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                                                if (recuiteremail != "")
                                                                {
                                                                    Ccmail += "," + recuiteremail;
                                                                }
                                                            }
                                                                if (ClientRowID == 1747)
                                                            {
                                                                flag = mail.CandidateLoginSendMailIngram(DisplayName, From, ToMail, Subject, body, attachment, alternateView);
                                                            }
                                                            else
                                                            {
                                                                flag = mail.CandidateLoginSendMail(DisplayName, From, ToMail, Subject, body, attachment, alternateView);
                                                            }
                                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, alternateViewSpocAndClient);

                                                        }

                                                    }
                                                    else if (ClientRowID == 1481)
                                                    {
                                                        DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                                        var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                                        if (recuiteremail != "")
                                                        {
                                                            Ccmail += "," + recuiteremail;
                                                        }
                                                        flag = mail.SendMailToCandidateForCtrls(DisplayName, From, ToMail, Subject, body, attachment, attachment2);
                                                        flagspocandclient = mail.SendMailToCandidateForCtrlsForSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, attachment2);
                                                    }
                                                    else
                                                    {
                                                        //=====================================
                                                        if (ClientRowID == 636 || ClientRowID == 994 || ClientRowID == 993 || ClientRowID == 992 || ClientRowID == 1125 || ClientRowID == 1243  || ClientRowID == 1618 || ClientRowID == 1803)
                                                        {
                                                            DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                                            var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                                            if (recuiteremail != "")
                                                            {
                                                                Ccmail += "," + recuiteremail;
                                                            }
                                                        }
                                                        //===========================================
                                                        if(ClientRowID == 1803)
                                                        {
                                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment , "EXL LOA Document.pdf");
                                                            //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClientWithoutAlternateView(DisplayName, From, Ccmail, "", Subject, bodySpocAndClient, attachment);
                                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClientExl(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, null , "EXL LOA Document.pdf");
                                                        }
                                                        else
                                                        {
                                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                            //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClientWithoutAlternateView(DisplayName, From, Ccmail, "", Subject, bodySpocAndClient, attachment);
                                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, null);
                                                        }

                                                        
                                                    }
                                                }

                                            }


                                            if (flag)
                                            {
                                                if (ClientRowID == 249 || ClientRowID == 277) //sutherlandglobal and Sutherland Healthcare Solutions Pvt. Ltd.
                                                {
                                                    try
                                                    {
                                                        string Message = "";
                                                        Message = "Dear " + personalData.Name + Environment.NewLine + Environment.NewLine;
                                                        Message += "Please complete Sutherland BGV on email sent to your ID or below link." + Environment.NewLine;
                                                        Message += "ID:" + loginDetail.UserID + Environment.NewLine;
                                                        Message += "Pwd:" + clsCommonMethods.PasswordDecrypt(loginDetail.UPass) + Environment.NewLine;
                                                        Message += "Link:" + ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString() + Environment.NewLine;
                                                        Message += "Regards" + Environment.NewLine;
                                                        Message += "Securitas";
                                                        loginDetail.SMSText = Message;
                                                        loginDetail.PersonalId = pID;
                                                        SendSMS(loginDetail);
                                                    }
                                                    catch
                                                    {

                                                    }
                                                }
                                                if (ClientRowID == 635)
                                                {
                                                    cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                }
                                                else if (ClientRowID == 636)
                                                {
                                                    cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                }
                                                else if (ClientRowID == 957)
                                                {
                                                    cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                }
                                                else if (ClientRowID == 1209)
                                                {
                                                    DataTable dt = cc.GetCandidateClient(ToMail, pID);
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        int clientrowidnew = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                                                        cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, clientrowidnew, 1);
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                    }
                                                }
                                                else if (ClientRowID == 1005 || ClientRowID == 2 || ClientRowID == 889)
                                                {
                                                    SaveWPassMail = SaveWPassMail.Replace("Click the below Url, login", "");//.Replace("Candidate Login", "").Replace("Click Reference ID:{ClientRefId}", "").Replace("Login Id: {UName}", "").Replace("Password: {UPasswd}", "");
                                                    SaveWPassMail = SaveWPassMail.Replace("into the website", "");
                                                    SaveWPassMail = SaveWPassMail.Replace("Candidate Login", "");
                                                    SaveWPassMail = SaveWPassMail.Replace("Click Reference ID:{ClientRefId}", "");

                                                    SaveWPassMail = SaveWPassMail.Replace("Password: {XXXXX}", "");

                                                    SaveWPassMail = SaveWPassMail.Replace("Login Id: {XXXXX}", "");

                                                    SaveWPassMail = SaveWPassMail.Replace("{document}", docString.ToString());
                                                    SaveWPassMail = SaveWPassMail.Replace("{CandidateName}", loginDetail.CandidateName);
                                                    SaveWPassMail = SaveWPassMail.Replace("#CompanyName", ClientNameMail);
                                                    SaveWPassMail = SaveWPassMail.Replace("{ClientRefId}", personalData.ClientRefID);
                                                    SaveWPassMail = SaveWPassMail.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                                    SaveWPassMail = SaveWPassMail.Replace("{UName}", "XXXXX");
                                                    SaveWPassMail = SaveWPassMail.Replace("{UPasswd}", "XXXXX");
                                                    SaveWPassMail = SaveWPassMail.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());

                                                    SaveWPassMail = SaveWPassMail.Replace("{MailSignature}", MailSignature);

                                                    cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                }
                                                else
                                                {
                                                    string Message = $"Dear {loginDetail.CandidateName} We kindly request you to upload the required document in the application, using the login credentials sent on your email. Feel free to connect in case of any assistance required on mentioned contact details on email. Thank you Walsons Services Pvt. Ltd.";
                                                    loginDetail.PersonalId = pID;
                                                    loginDetail.SMSText = Message;
                                                    SendSMS(loginDetail);
                                                    cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);

                                                }

                                                if (ClientRowID == 635)
                                                {
                                                    //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                                    RemindermailTataElxsi(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                                }

                                                if (ClientRowID == 957)
                                                {
                                                    RemindermailAstraZenca(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                                }

                                                if (ClientRowID == 679 || ClientRowID == 725)
                                                {
                                                    //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                                    SaveReminderMail(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                                }
                                                UpdatePQCandidateLoginForSendMailViewModel loginModel = new UpdatePQCandidateLoginForSendMailViewModel();
                                                loginModel.CandidateUserRowID = loginDetail.CandidateUserRowID;
                                                loginModel.SentMailStatus = 1;
                                                loginModel.SentMailDate = DateTime.Now;
                                                repoCandidate.UpdateCandidateLoginSendMailStatusById(loginModel);
                                                countSent++;
                                            }
                                            else
                                            {
                                                countFailed++;
                                            }
                                        }

                                    }

                                }
                                TempData["ErrorMsg"] = "Out of " + totRec.ToString() + " Mail sent : " + countSent.ToString() + " failed : " + countFailed.ToString();

                            }
                            else
                            {
                                TempData["ErrorMsg"] = "Candidate login details not found!";
                            }
                        }
                    }
                    #endregion

                    model = repoPQPersonal.GetTempCandidateListByClientBulkUpload(page, pageSize, sort, sortDir, Search, ClientRowID);

                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                //PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();               
                SearchModel.Search = Search;
                model.ItemSearch = SearchModel;
            }
            catch (Exception Ex)
            {
                SearchModel.Search = "";
                model.ItemSearch = SearchModel;
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //return View(model);
            return RedirectToAction("ManageClientCandidate", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });

        }

        [HttpPost]
        public ActionResult ManageClientCandidateDelete(int id)
        {
            int page = 1;
            string sort = "";
            string sortDir = "asc";
            string Search = "";
            ClientCustomize cc = new ClientCustomize();
            TempPQPersonalInfoListPagedModel model = new TempPQPersonalInfoListPagedModel();
            PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
            short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
            int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
            int pageSize = clsGlobalCommon.PageSize;
            // Your deletion logic here
            int res = cc.SpDeleteTeampPQPersonalDetails(ClientRowID, Convert.ToInt32(id));
            if (res != 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Candidate delete unsuccessfully !!!!!" });
            }



        }
        public void SendAutoEmailTocandidateAfterInitiation(string PersonalRowId, string export = "")
        {
            int page = 1;
            string sort = "";
            string sortDir = "asc";
            string Search = null;
            string PersonalIds = PersonalRowId;
            ClientCustomize cc = new ClientCustomize();
            TempPQPersonalInfoListPagedModel model = new TempPQPersonalInfoListPagedModel();
            PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
            try
            {
                short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                int pageSize = clsGlobalCommon.PageSize;
                ViewBag.ErrorMsg = "";
                if (export == "Excel")
                {
                    model = repoPQPersonal.GetTempCandidateListByClientBulkUpload(page, pageSize, sort, sortDir, Search, ClientRowID);
                    var value = model.TempPQPersonalInfo;
                    var a = value.Select(i => new { i.Name, i.CandidateCode, i.Entityname, i.PackageName, i.CreatedDate, i.SendMailStatus, });
                    ExportToExcel(a, "ExportSentMail");
                }
                //////////////////////////////
                if (ViewBag.ErrorMsg == "")
                {
                    #region Send Mail To Candidate
                    if (!string.IsNullOrEmpty(PersonalIds))
                    {
                        string spocEmail = "";
                        string cSpocEmail = "";
                        string mobileno = "";
                        string[] PersonalID = PersonalIds.Split(',');
                        string TrmFldPathDoc = "";
                        string SendUserManulToCtrls = "";
                        string CtrlsfileName = "";
                        string fileName = "";
                        if (ClientRowID == 576)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "LOA.pdf";
                            fileName = Path.GetFileNameWithoutExtension("LOA.pdf");
                        }
                        else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Chargebee_LOA.docx";
                            fileName = Path.GetFileNameWithoutExtension("Chargebee_LOA.docx");
                        }
                        else if (ClientRowID == 1425)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "GlobalAuthorization-Australia.docx";
                            fileName = Path.GetFileNameWithoutExtension("GlobalAuthorization-Australia.docx");
                        }
                        else if (ClientRowID == 1428)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Genpact Privacy Notice and Consent Form template_Pre-Employment Screening_EU (Securitas).docx";
                            fileName = Path.GetFileNameWithoutExtension("Genpact Privacy Notice and Consent Form template_Pre-Employment Screening_EU (Securitas).docx");
                        }
                        else if (ClientRowID == 1497)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Capco Privacy Notice and Consent Form template_Pre-Employment Screening(Securitas).docx";
                            fileName = Path.GetFileNameWithoutExtension("Capco Privacy Notice and Consent Form template_Pre-Employment Screening(Securitas).docx");
                        }
                        else if (ClientRowID == 1429)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Global_Authorization-Nagarro.docx";
                            fileName = Path.GetFileNameWithoutExtension("Global_Authorization-Nagarro.docx");
                        }
                        else if (ClientRowID == 1449)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "GlobalAuthorization-Uniqus.docx";
                            fileName = Path.GetFileNameWithoutExtension("GlobalAuthorization-Uniqus.docx");
                        }
                        else if (ClientRowID == 1514)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "SILVERLINK-I.docx";
                            fileName = Path.GetFileNameWithoutExtension("SILVERLINK-I.docx");
                        }
                        else if (ClientRowID == 1481)
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CtrlS_Datacenter-LOA.pdf";
                            fileName = Path.GetFileNameWithoutExtension("CtrlS_Datacenter-LOA.pdf");
                            SendUserManulToCtrls = ConfigurationManager.AppSettings["CandidateManual"].ToString() + "CandidateManual.pdf";
                            CtrlsfileName = Path.GetFileNameWithoutExtension("CandidateManual.pdf");
                        }
                        else
                        {
                            TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CandidateAgreement.pdf";
                            fileName = Path.GetFileNameWithoutExtension("CandidateAgreement.pdf");
                        }
                        Attachment attachment;
                        Attachment attachment2;
                        if (ClientRowID == 1481)
                        {
                            attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                            attachment2 = new Attachment(Server.MapPath(SendUserManulToCtrls));
                        }
                        else
                        {
                            attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                            attachment2 = null;
                        }

                        string MailSignature = string.Empty;
                        string From = string.Empty;
                        string DisplayName = string.Empty;
                        string Cc = "";

                        clsSendMail mail;

                        var cSmtp = repoPQClient.GetPQClientSendMailFromById(ClientRowID);
                        DataTable dtEmail = new DataTable();

                        string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);
                        if (ClientAccessType == "3")
                        {
                            dtEmail = cc.userwise_ReportingEmailAccess(ClientUserRowID);
                            if (dtEmail.Rows.Count > 0)
                            {
                                spocEmail = dtEmail.Rows[0]["ClientUserEmailID"].ToString();
                                cSpocEmail = dtEmail.Rows[0]["ReportingTo_EmailID"].ToString();

                            }
                        }
                        else
                        {
                            dtEmail = cc.GetEmailStructureForAlert(ClientRowID, ClientUserRowID, "Case Registration");
                            if (dtEmail.Rows.Count > 0)
                            {
                                spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                                cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                                mobileno = dtEmail.Rows[0]["mobileno"].ToString();

                            }
                        }
                        Cc = cSpocEmail;
                        if (cSmtp != null && cSmtp.MailSendBy > 0)
                        {
                            mail = new clsSendMail(cSmtp.SMTPServer, cSmtp.SMTPPort, cSmtp.SMTPUserName, clsCommonMethods.PasswordDecrypt(cSmtp.SMTPPassword), Convert.ToBoolean(cSmtp.EnableSsl));

                            From = cSmtp.SMTPUserName;
                            DisplayName = cSmtp.ClientName.Trim();
                            Cc = string.Concat(Cc, ",", spocEmail);

                            MailSignature = string.Concat("<b>", cSmtp.ClientName.Trim(), "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", cSmtp.SMTPUserName, "'>", cSmtp.SMTPUserName, "</a>");
                        }
                        else
                        {
                            mail = new clsSendMail();
                            var signature = mail.GetCompanyMailSignature();

                            if (mobileno != "")
                                mobileno = "<b>Mobile: " + mobileno;
                            string ShowSpocId = spocEmail.ToLower();

                            ShowSpocId = ShowSpocId.Replace(",", ";");
                            string[] ShowSpocId1 = spocEmail.Split(';');
                            if (signature != null)
                            {
                                // For TATA Elxsi
                                if (ClientRowID == 635)
                                {
                                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                    //"<b>Mobile: ", signature.MobileNo,
                                    "</b><br/><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", "bgv.helpdesk@securitas-india.com", "'>", "bgv.helpdesk@securitas-india.com", "</a>");
                                }
                                else if (ClientRowID == 249)
                                {
                                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                    //"<b>Mobile: ", signature.MobileNo,
                                    "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                    "</b><br /><b>Contact No:</b><br />  <b>Satya :</b>+91 8377022632 <br />  <b>Pradeep Devrani :</b> +91 8373907723<br /><b>Himanshu Pathak :</b> +91 7291974515 ");
                                    //Please mention the contact number of Satya - +91 8377022632 / Pradeep Devrani - 91 - 8373907723 / Himanshu Pathak - +91 7291974515
                                }
                                else if (ClientRowID == 277)
                                {
                                    MailSignature = string.Concat("For Securitas India. <br /><br />");
                                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                    //"<b>Mobile: ", signature.MobileNo,
                                    "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                    "</b><br /><b>Contact No </b>:+91-8373907723");
                                }
                                else
                                {
                                    MailSignature = string.Empty;
                                    MailSignature = string.Concat(ShowSpocId1[0]);
                                }
                            }

                            Cc = string.Concat(Cc, ",", ShowSpocId);
                            From = signature.GeneralEmail;
                            DisplayName = signature.CompanyName.Trim();
                        }

                        foreach (var item in PersonalID)
                        {
                            int pID = Convert.ToInt32(item);
                            var personalData = repoPQPersonal.GetTempPQPersonalDetailsForSendMailById(pID);
                            var loginDetail = repoCandidate.GetCandidateLoginDetailsForMailSendById(pID);
                            DataTable d = new DataTable();
                            StringBuilder docString = new StringBuilder();

                            if (ClientRowID == 1425 || ClientRowID == 1428 || ClientRowID == 1222)
                            {
                                d = cc.GetDocumentForVerificationList(pID, ClientRowID);
                                foreach (DataRow dr in d.Rows)
                                {
                                    docString.Append("<tr><td style='padding:3px;'>" + dr["SubCheckName"].ToString() + " </td><td>" + dr["DocumentForVerification"].ToString() + " </td></tr>");
                                }
                            }
                            else if (ClientRowID == 193)
                            {
                                d = cc.GetDocumentForVerificationList(pID, ClientRowID);
                                foreach (DataRow dr in d.Rows)
                                {
                                    docString.Append("<tr><td style='padding:3px;'>" + dr["SubCheckName"].ToString() + " </td><td>" + dr["DocumentForVerification"].ToString() + " </td></tr>");
                                }
                            }
                            else
                            {
                                d = cc.CandidateEmailDocFormat(pID); // it should be subcheck family based on special check, 
                                foreach (DataRow dr in d.Rows)
                                {
                                    if (ClientRowID == 1357)
                                    {
                                        //---- Employment Check Start from here ----//
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Past two Employments </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");
                                    }
                                    else if (ClientRowID == 1425)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Any available document. Note: Two employment details prior to Chargebee.</ td></tr>");
                                    }
                                    else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Experience letter / Service Certificate</td></tr>");
                                    }
                                    else if (ClientRowID == 1428)
                                    {

                                    }
                                    else if (ClientRowID == 1449)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Experience letter, Service Certificate, Other</td></tr>");
                                    }
                                    else if (ClientRowID == 1481)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter</td></tr>");
                                    }
                                    else if (ClientRowID == 636)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Current Company Employment<br> • Experience Letter / Relieving Letter<br> • Salary Slip for last 3 months <br> Previous Employment(only latest 2 employers)<br> • Experience Letter / Relieving Letter </ td></tr>");
                                    }
                                    else if (ClientRowID == 1804)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") // Employment
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter</td></tr>");
                                        }
                                        if (dr["CheckFamilyRowID"].ToString() == "11") // Education
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Consolidated Mark Sheet / Degree Certificate / Provisional Certificate / Final Year Marksheet</td></tr>");
                                        }
                                        if (dr["CheckFamilyRowID"].ToString() == "2")   //Address
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Address </td><td>Any One Address Proof</td></tr>");
                                        }
                                        if (dr["CheckFamilyRowID"].ToString() == "5")   //Court
                                        {
                                            docString.Append("<tr><td style='padding:3px;'>Court Record Check</td><td>Aadhar Card Copy</td></tr>");
                                        }
                                        if (dr["CheckFamilyRowID"].ToString() == "40")  //Global Database Check
                                            docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>PAN Card Copy</td></tr>");
                                    }
                                    else
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                            docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");
                                    }
                                    //---- Employment Check End from here ----//

                                    //---- Education Check Start from here ----//
                                    if (ClientRowID == 1355 || ClientRowID == 1348)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Marksheet/Provisional Degree Certificate </td></tr>");
                                    }
                                    else if (ClientRowID == 1318)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");
                                        if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                            docString.Append("<tr><td style='padding:3px;'>Sanad Certification </td><td>Required Bar Council Certificate</td></tr>");
                                    }
                                    else if (ClientRowID == 1425)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Transcript/Degree Certificate</td></tr>");
                                    }
                                    else if (ClientRowID == 1428)
                                    {

                                    }
                                    else if (ClientRowID == 1449)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Transcript/ Degree Certificate/ Convocation Certificate</td></tr>");
                                    }
                                    else
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                            docString.Append("<tr><td style='padding:3px;'>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");
                                    }
                                    //---- Education Check End from here ----//

                                    //---- Address Check Start from here ----//
                                    if (ClientRowID == 1425)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                            docString.Append("<tr><td style='padding:3px;'>Address </td><td>Driver's License/Utility Bill</td></tr>");
                                    }
                                    if (ClientRowID == 1395)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                            docString.Append("<tr><td style='padding:3px;'>Address </td><td>SSN Number</td></tr>");
                                    }
                                    else if (ClientRowID == 1428)
                                    {

                                    }
                                    else if (ClientRowID == 1449)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                            docString.Append("<tr><td style='padding:3px;'>Address </td><td>National ID Proof</td></tr>");
                                    }
                                    else if (ClientRowID == 636)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                            docString.Append("<tr><td style='padding:3px;'>Address </td><td>• Address Proof </td></tr>");
                                    }
                                    else
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                            docString.Append("<tr><td style='padding:3px;'>Address </td><td>Address Proof for Current & Permanent Address </td></tr>");
                                    }
                                    //---- Address Check End from here ----//

                                    //---- National Check Start from here ----//
                                    if (ClientRowID == 1355 || ClientRowID == 1348)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "26")
                                            docString.Append("<tr><td>ID Verification</td><td>Passport preferred</td></tr>");
                                    }
                                    else if (ClientRowID == 1425 || ClientRowID == 1428 || ClientRowID == 1449)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "26")
                                            docString.Append("<tr><td>National ID Check </td><td>National ID Proof</td></tr>");
                                    }
                                    else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "26")
                                            docString.Append("<tr><td>National ID Check </td><td>Passport / National ID</td></tr>");
                                    }
                                    else if (ClientRowID == 559)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "26")
                                            docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Pan Card</td></tr>");
                                    }
                                    else if (ClientRowID == 636)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "26")
                                            docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Photo ID Proof </td></tr>");
                                    }
                                    else if (ClientRowID == 648)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "26")
                                            docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>ID Proof – Pan Card and Aadhar Card</td></tr>");
                                    }
                                    else
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "26")
                                            docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>ID Proof </td></tr>");
                                    }
                                    //---- National Check End from here ----//

                                    //---- Special Check Start from here ----//
                                    if (ClientRowID == 487)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "40")
                                            docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>Required Aadhar Card</td></tr>");
                                    }
                                    else if (ClientRowID == 1470)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "40")
                                            docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>NA</td></tr>");
                                    }
                                    else if (ClientRowID == 193)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "40")
                                            docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>NA</td></tr>");
                                    }

                                    //---- Special Check End from here ----//

                                    //---- Criminal Check Start from here ----//
                                    if (ClientRowID == 487 || ClientRowID == 1428 || ClientRowID == 1449)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "5")
                                            docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>National ID Proof</td></tr>");
                                    }
                                    else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "5")
                                            docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>Passport / National ID</td></tr>");
                                    }
                                    else if (ClientRowID == 636)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "5")
                                            docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>• Address Proof </td></tr>");
                                    }
                                    else if (ClientRowID == 648)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "5")
                                            docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>Require – Address proof, ID Proof and Passport Size Photo</td></tr>");
                                    }
                                    else if (ClientRowID == 1470)
                                    {
                                        if (dr["CheckFamilyRowID"].ToString() == "5")
                                            docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>National ID Proof</td></tr>");
                                    }
                                    //---- Criminal Check End from here ----//
                                }
                            }

                            if (loginDetail != null)
                            {
                                string body = string.Empty;
                                string bodySpocAndClient = string.Empty;
                                string SaveWPassMail = string.Empty;
                                DataTable tblExperiece = cc.CheckCandidateExperienced(personalData.ClientRowID, personalData.PersonalRowID);

                                string MailSubject = "";
                                string SpocAndClientMailSubject = "";
                                string MailTemplateName = "";
                                string MailTemplateNameSpoc = "";
                                int MailClientRowid = 0;
                                var templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplate");
                                if (templateDB.Rows.Count > 0)
                                {
                                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                    MailTemplateName = Convert.ToString(templateDB.Rows[0]["TemplateName"]);
                                    MailClientRowid = Convert.ToInt32(templateDB.Rows[0]["ClientRowId"]);
                                }
                                var templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplateForSpocAndClient");
                                if (templateDBSpocAndClient.Rows.Count > 0)
                                {
                                    bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                    MailTemplateNameSpoc = Convert.ToString(templateDBSpocAndClient.Rows[0]["TemplateName"]);
                                    SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                }

                                if (ClientRowID == 679)
                                {
                                    DataTable tblfractal = cc.CheckCandidateExperiencedByCandidateType(personalData.ClientRowID, personalData.PersonalRowID);
                                    if (tblfractal.Rows.Count > 0)
                                    {
                                        if (tblfractal.Rows[0][0].ToString() == "Existing Employee")
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployee");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployeeForSpocAndClient");
                                        }
                                        else
                                        {
                                            DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                            if (dtpackage.Rows.Count > 0)
                                            {
                                                if (dtpackage.Rows[0][0].ToString().Trim() == "FAPL_IAL2")
                                                {
                                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 2926, 1, "FractelNewJoinertemp");
                                                    templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 2926, 1, "FractelNewJoinertempForSpocAndClient");
                                                }
                                                else
                                                {
                                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                                    templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                                }
                                            }
                                            else
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                            }
                                            //templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                            //templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                        }
                                    }
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                        if (ClientRowID == 679 || ClientRowID == 2)
                                        {
                                            SaveWPassMail = Convert.ToString(templateDB.Rows[0]["WLDMailTemplate"]);
                                        }
                                    }
                                    if (templateDBSpocAndClient.Rows.Count > 0)
                                    {
                                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                    }
                                }

                                else if (ClientRowID == 576)
                                {
                                    //---------Experienced-----------
                                    if (tblExperiece.Rows.Count > 0)
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LateralMailNagarro");
                                        templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LateralMailNagarroForSpocAndClient");
                                    }
                                    else
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "NagarroFresherMail");
                                        templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "NagarroFresherMailForSpocAndClient");
                                    }
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                    }
                                    if (templateDBSpocAndClient.Rows.Count > 0)
                                    {
                                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                    }
                                }
                                else if (ClientRowID == 635) //Tata Elxsi
                                {

                                    DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                    if (dtpackage.Rows.Count > 0)
                                    {
                                        if (dtpackage.Rows[0][0].ToString() == "TEL_Experienced (ID_CRCP_CRCC_PADD_CADD_GDB_2EMP")
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 671, 1, "LoginDetailsSendToCandidateFotTataElxsiExp");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiExpSpocAndClient");
                                        }
                                        else
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresher");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresherSpocAndClient");
                                        }
                                    }
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                    }
                                    if (templateDBSpocAndClient.Rows.Count > 0)
                                    {
                                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                    }
                                }


                                else if (ClientRowID == 647)
                                {

                                    DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                    if (dtpackage.Rows.Count > 0)
                                    {
                                        if (dtpackage.Rows[0][0].ToString() == "NNSCIPL_Experienced (EDU_2EMP_PADD_GDB_CRCP_ID)")
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 737, 1, "LoginDetailsSendToCandidateForNovoNordiskExp");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskExpForSpocAndClient");
                                        }
                                        else
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresher");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresherForSpocAndClient");
                                        }
                                    }
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                    }
                                    if (templateDBSpocAndClient.Rows.Count > 0)
                                    {
                                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                    }
                                }

                                if (ClientRowID == 103)
                                {
                                    body = body.Replace("{Remarks}", "Please login and fill the details within 3 days");
                                    bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Please login and fill the details within 3 days");
                                }
                                else if (ClientRowID == 1530)
                                {
                                    body = body.Replace("{Remarks}", "Kindly fill the form within 2 working days");
                                    bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Kindly fill the form within 2 working days");
                                }
                                else
                                {
                                    body = body.Replace("{Remarks}", "Please login and fill the details within 2 days");
                                    bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Please login and fill the details within 2 days");
                                }

                                string txtFolderPath = ConfigurationManager.AppSettings["mailimages"].ToString();
                                LinkedResource Triangleres = new LinkedResource(Server.MapPath(txtFolderPath) + "Triangle.png", MediaTypeNames.Image.Jpeg);
                                Triangleres.ContentId = Guid.NewGuid().ToString();
                                string triangle = @"<img src='cid:" + Triangleres.ContentId + @"' width='25%'/>";

                                LinkedResource Securitassec = new LinkedResource(Server.MapPath(txtFolderPath) + "Securitas-logo.png", MediaTypeNames.Image.Jpeg);
                                Securitassec.ContentId = Guid.NewGuid().ToString();
                                string securitas = @"<img src='cid:" + Securitassec.ContentId + @"' width='25%'/>";

                                LinkedResource Walsonssec = new LinkedResource(Server.MapPath(txtFolderPath) + "Walsons-Temp-img.JPG", MediaTypeNames.Image.Jpeg);
                                Walsonssec.ContentId = Guid.NewGuid().ToString();
                                string walsons = @"<img src='cid:" + Walsonssec.ContentId + @"' width='25%'/>";

                                LinkedResource BGVdottedImg = new LinkedResource(Server.MapPath(txtFolderPath) + "BGV-dotted-design.png", MediaTypeNames.Image.Jpeg);
                                BGVdottedImg.ContentId = Guid.NewGuid().ToString();
                                string BGVdotted = @"<img src='cid:" + BGVdottedImg.ContentId + @"' width='100%'/>";

                                LinkedResource RedbulllogoImg = new LinkedResource(Server.MapPath(txtFolderPath) + "Redbullogo.png", MediaTypeNames.Image.Jpeg);
                                RedbulllogoImg.ContentId = Guid.NewGuid().ToString();
                                string redbulllogo = @"<img src='cid:" + RedbulllogoImg.ContentId + @"' width='30%'/>";

                                LinkedResource JadeGlobalLogoIMG = new LinkedResource(Server.MapPath(txtFolderPath) + "JadeGlobalLogo.png", MediaTypeNames.Image.Jpeg);
                                JadeGlobalLogoIMG.ContentId = Guid.NewGuid().ToString();
                                string JadeGlobalLogo = @"<img src='cid:" + JadeGlobalLogoIMG.ContentId + @"' width='30%'/>";

                                string ClientNameMail = Session["ClientNameMail"].ToString();
                                //Candidate Template Details
                                body = body.Replace("{document}", docString.ToString());
                                body = body.Replace("{CandidateName}", loginDetail.CandidateName);
                                body = body.Replace("#CompanyName", ClientNameMail);
                                body = body.Replace("{ClientRefId}", personalData.ClientRefID);
                                body = body.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                body = body.Replace("{UName}", loginDetail.UserID);
                                body = body.Replace("{UPasswd}", clsCommonMethods.PasswordDecrypt(loginDetail.UPass.Trim()));
                                body = body.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                                body = body.Replace("{triangle}", triangle);
                                body = body.Replace("{securitas}", securitas);
                                body = body.Replace("{walsons}", walsons);
                                body = body.Replace("{BGVdotted}", BGVdotted);
                                body = body.Replace("{redbulllogo}", redbulllogo);
                                body = body.Replace("{JadeLogo}", JadeGlobalLogo);
                                body = body.Replace("{MailSignature}", MailSignature);

                                //Spoc And Client Template Details
                                var Resendlinks = "https://bgv.securitas-india.com/account/ResendLink/?PersonalIds=" + pID.ToString();
                                bodySpocAndClient = bodySpocAndClient.Replace("{Resendlink}", Resendlinks);
                                bodySpocAndClient = bodySpocAndClient.Replace("{document}", docString.ToString());
                                bodySpocAndClient = bodySpocAndClient.Replace("{CandidateName}", loginDetail.CandidateName);
                                bodySpocAndClient = bodySpocAndClient.Replace("#CompanyName", ClientNameMail);
                                bodySpocAndClient = bodySpocAndClient.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                bodySpocAndClient = bodySpocAndClient.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                                bodySpocAndClient = bodySpocAndClient.Replace("{triangle}", triangle);
                                bodySpocAndClient = bodySpocAndClient.Replace("{securitas}", securitas);
                                bodySpocAndClient = bodySpocAndClient.Replace("{walsons}", walsons);
                                bodySpocAndClient = bodySpocAndClient.Replace("{BGVdotted}", BGVdotted);
                                bodySpocAndClient = bodySpocAndClient.Replace("{redbulllogo}", redbulllogo);
                                bodySpocAndClient = bodySpocAndClient.Replace("{JadeLogo}", JadeGlobalLogo);
                                bodySpocAndClient = bodySpocAndClient.Replace("{MailSignature}", MailSignature);

                                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                                alternateView.LinkedResources.Add(Triangleres);
                                alternateView.LinkedResources.Add(Securitassec);
                                alternateView.LinkedResources.Add(Walsonssec);
                                alternateView.LinkedResources.Add(BGVdottedImg);
                                alternateView.LinkedResources.Add(RedbulllogoImg);
                                alternateView.LinkedResources.Add(JadeGlobalLogoIMG);

                                AlternateView alternateViewSpocAndClient = AlternateView.CreateAlternateViewFromString(bodySpocAndClient, null, MediaTypeNames.Text.Html);
                                alternateViewSpocAndClient.LinkedResources.Add(Triangleres);
                                alternateViewSpocAndClient.LinkedResources.Add(Securitassec);
                                alternateViewSpocAndClient.LinkedResources.Add(Walsonssec);
                                alternateViewSpocAndClient.LinkedResources.Add(BGVdottedImg);
                                alternateViewSpocAndClient.LinkedResources.Add(RedbulllogoImg);
                                alternateViewSpocAndClient.LinkedResources.Add(JadeGlobalLogoIMG);

                                if (ClientRowID == 2)
                                {
                                    string key = loginDetail.CandidateUserRowID + "_" + personalData.ClientRowID + "_" + personalData.PersonalRowID;
                                    body = body.Replace("{MediaLink}", ConfigurationManager.AppSettings["CandidateMediaUrl"].ToString() + "?Key=" + clsCommonMethods.PasswordEncrypt(key));

                                }
                                string Subject = "";
                                // For Sutherland Global  249,277
                                if (ClientRowID == 249 || ClientRowID == 277)
                                {
                                    Subject = loginDetail.CandidateName + "  Sutherland Background Verification by Securitas";
                                    DisplayName = "Sutherland BGV by Securitas";
                                }
                                // For TATA Elxsi
                                else if (ClientRowID == 635)
                                {
                                    body = body.Replace("{document}", "Documents Required<br /> Education: Degree Certificate / Final Year Marksheet < br /> " +
                                        "Address: Address Proof < br />   Nationality: ID Proof");
                                    Subject = "Background verification";
                                }
                                else
                                {
                                    Subject = loginDetail.CandidateName + "  Background Verification";
                                }
                                string To = loginDetail.CandidateEmail;
                                if (ClientRowID == 249 || ClientRowID == 277 || ClientRowID == 193)
                                {
                                    Cc = Cc + ";" + personalData.AlternateEmail;
                                }

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
                                DataTable Dt = cc.getCandidateMailstatus(pID.ToString());

                                int countSent = 0, countFailed = 0, totRec = 0;
                                bool flag = false;
                                bool flagspocandclient = false;
                                if (Dt.Rows.Count > 0)
                                {
                                    try
                                    {
                                        if (Convert.ToString(personalData.MobileNo) != "")
                                        {
                                            MessageStatus objMessage = new MessageStatus();
                                            WhatsAppAPI objs = new WhatsAppAPI();
                                            var link = "https://bgv.securitas-india.com/Account/CandidateLogin";
                                            objMessage = objs.SendWhatsAppMessage("+91" + personalData.MobileNo, "case_registration_candidate_invite_mode", loginDetail.CandidateName, "media_template", ClientNameMail, To, link);
                                            cc.sp_AddAuditWhatsAppAPI(personalData.PersonalRowID, objMessage.message, objMessage.status, "Candidate Invite");
                                        }
                                    }
                                    catch
                                    {

                                    }
                                    totRec = Dt.Rows.Count;
                                    for (int i = 0; i < totRec; i++)
                                    {
                                        if (Dt.Rows[0]["SentMailStatus"].ToString() == "0")
                                        {
                                            if (ClientRowID == 503)
                                            {
                                                flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                            }
                                            else
                                            {
                                                if (ClientRowID == 257) //Biz 2 Credit Info Services Pvt. Ltd
                                                {
                                                    string bcc = "";
                                                    string Ccc = "";
                                                    Ccc = spocEmail;
                                                    bcc = cSpocEmail;
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Ccc, bcc, Subject, bodySpocAndClient);
                                                    //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, Ccc, bcc,Subject, bodySpocAndClient, null, null);
                                                }
                                                else if (ClientRowID == 249)//sutherlandglobal 
                                                {
                                                    String CcSuthermail = "brij.bhushan@securitas-india.com";

                                                    CcSuthermail = CcSuthermail + "," + Ccmail;

                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, CcSuthermail, Subject, bodySpocAndClient, attachment, null);
                                                }
                                                // For TATA Elxsi
                                                else if (ClientRowID == 635)
                                                {
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);

                                                }
                                                else if (ClientRowID == 957)
                                                {
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                }
                                                else if (ClientRowID == 1312)
                                                {
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                }
                                                else if (ClientRowID == 778)
                                                {

                                                    String CcHHS = personalData.BusinessName;
                                                    if (CcHHS == null)
                                                    {
                                                        CcHHS = Ccmail.ToString();
                                                    }
                                                    else
                                                    {
                                                        CcHHS = CcHHS + "," + Ccmail;
                                                    }
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, CcHHS, Subject, bodySpocAndClient, attachment, null);

                                                }
                                                else if (ClientRowID == 1005 || ClientRowID == 2)
                                                {
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);

                                                }
                                                else if (ClientRowID == 889)
                                                {
                                                    flag = mail.SendMailPurgWithoutCC(DisplayName, From, ToMail, Subject, body, attachment);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, cSpocEmail, Subject, bodySpocAndClient, attachment, null);
                                                }
                                                else if (ClientRowID == 974)
                                                {
                                                    flag = mail.SendMailAdidas(DisplayName, From, ToMail, Subject, body);
                                                    flagspocandclient = mail.SendMailAdidasForSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, body);
                                                }
                                                else if ((ClientRowID == 1496 || MailClientRowid == 2 || MailClientRowid == 193) && (MailTemplateName == "CandidateLoginDetailsTemplate" || MailTemplateNameSpoc == "CandidateLoginDetailsTemplate"))
                                                {
                                                    flag = mail.CandidateLoginSendMail(DisplayName, From, ToMail, Subject, body, attachment, alternateView);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, alternateViewSpocAndClient);
                                                }
                                                else if (ClientRowID == 1481)
                                                {
                                                    DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                                    var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                                    if (recuiteremail != "")
                                                    {
                                                        Ccmail += "," + recuiteremail;
                                                    }
                                                    flag = mail.SendMailToCandidateForCtrls(DisplayName, From, ToMail, Subject, body, attachment, attachment2);
                                                    flagspocandclient = mail.SendMailToCandidateForCtrlsForSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, attachment2);
                                                }
                                                else
                                                {
                                                    //=====================================
                                                    if (ClientRowID == 636)
                                                    {
                                                        DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                                        var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                                        if (recuiteremail != "")
                                                        {
                                                            Ccmail += "," + recuiteremail;
                                                        }
                                                    }
                                                    //===========================================
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                    //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClientWithoutAlternateView(DisplayName, From, Ccmail, "", Subject, bodySpocAndClient, attachment);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, null);
                                                }
                                            }

                                            if (flag)
                                            {
                                                if (ClientRowID == 249 || ClientRowID == 277) //sutherlandglobal and Sutherland Healthcare Solutions Pvt. Ltd.
                                                {
                                                    try
                                                    {
                                                        string Message = "";
                                                        Message = "Dear " + personalData.Name + Environment.NewLine + Environment.NewLine;
                                                        Message += "Please complete Sutherland BGV on email sent to your ID or below link." + Environment.NewLine;
                                                        Message += "ID:" + loginDetail.UserID + Environment.NewLine;
                                                        Message += "Pwd:" + clsCommonMethods.PasswordDecrypt(loginDetail.UPass) + Environment.NewLine;
                                                        Message += "Link:" + ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString() + Environment.NewLine;
                                                        Message += "Regards" + Environment.NewLine;
                                                        Message += "Securitas";
                                                        loginDetail.SMSText = Message;
                                                        loginDetail.PersonalId = pID;
                                                        SendSMS(loginDetail);
                                                    }
                                                    catch
                                                    {

                                                    }
                                                }
                                                if (ClientRowID == 635)
                                                {
                                                    cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                }
                                                else if (ClientRowID == 636)
                                                {
                                                    cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                }
                                                else if (ClientRowID == 957)
                                                {
                                                    cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                }
                                                else if (ClientRowID == 1209)
                                                {
                                                    DataTable dt = cc.GetCandidateClient(ToMail, pID);
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        int clientrowidnew = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                                                        cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, clientrowidnew, 1);
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                    }
                                                }
                                                else if (ClientRowID == 1005 || ClientRowID == 2 || ClientRowID == 889)
                                                {
                                                    SaveWPassMail = SaveWPassMail.Replace("Click the below Url, login", "");//.Replace("Candidate Login", "").Replace("Click Reference ID:{ClientRefId}", "").Replace("Login Id: {UName}", "").Replace("Password: {UPasswd}", "");
                                                    SaveWPassMail = SaveWPassMail.Replace("into the website", "");
                                                    SaveWPassMail = SaveWPassMail.Replace("Candidate Login", "");
                                                    SaveWPassMail = SaveWPassMail.Replace("Click Reference ID:{ClientRefId}", "");

                                                    SaveWPassMail = SaveWPassMail.Replace("Password: {XXXXX}", "");

                                                    SaveWPassMail = SaveWPassMail.Replace("Login Id: {XXXXX}", "");

                                                    SaveWPassMail = SaveWPassMail.Replace("{document}", docString.ToString());
                                                    SaveWPassMail = SaveWPassMail.Replace("{CandidateName}", loginDetail.CandidateName);
                                                    SaveWPassMail = SaveWPassMail.Replace("#CompanyName", ClientNameMail);
                                                    SaveWPassMail = SaveWPassMail.Replace("{ClientRefId}", personalData.ClientRefID);
                                                    SaveWPassMail = SaveWPassMail.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                                    SaveWPassMail = SaveWPassMail.Replace("{UName}", "XXXXX");
                                                    SaveWPassMail = SaveWPassMail.Replace("{UPasswd}", "XXXXX");
                                                    SaveWPassMail = SaveWPassMail.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());

                                                    SaveWPassMail = SaveWPassMail.Replace("{MailSignature}", MailSignature);

                                                    cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                }
                                                else
                                                {
                                                    string Message = $"Dear {loginDetail.CandidateName} We kindly request you to upload the required document in the application, using the login credentials sent on your email. Feel free to connect in case of any assistance required on mentioned contact details on email. Thank you Walsons Services Pvt. Ltd.";
                                                    loginDetail.PersonalId = pID;
                                                    loginDetail.SMSText = Message;
                                                    SendSMS(loginDetail);
                                                    cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);

                                                }

                                                if (ClientRowID == 635)
                                                {
                                                    //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                                    RemindermailTataElxsi(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                                }

                                                if (ClientRowID == 957)
                                                {
                                                    RemindermailAstraZenca(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);

                                                }

                                                if (ClientRowID == 679 || ClientRowID == 725)
                                                {
                                                    //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                                    SaveReminderMail(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                                }
                                                UpdatePQCandidateLoginForSendMailViewModel loginModel = new UpdatePQCandidateLoginForSendMailViewModel();
                                                loginModel.CandidateUserRowID = loginDetail.CandidateUserRowID;
                                                loginModel.SentMailStatus = 1;
                                                loginModel.SentMailDate = DateTime.Now;
                                                repoCandidate.UpdateCandidateLoginSendMailStatusById(loginModel);
                                                countSent++;
                                            }
                                            else
                                            {
                                                countFailed++;
                                            }
                                        }

                                    }

                                }
                                TempData["ErrorMsg"] = "Out of " + totRec.ToString() + " Mail sent : " + countSent.ToString() + " failed : " + countFailed.ToString();

                            }
                            else
                            {
                                TempData["ErrorMsg"] = "Candidate login details not found!";
                            }
                        }
                    }
                    #endregion

                    model = repoPQPersonal.GetTempCandidateListByClientBulkUpload(page, pageSize, sort, sortDir, Search, ClientRowID);

                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                }
                //PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();               
                SearchModel.Search = Search;
                model.ItemSearch = SearchModel;
            }
            catch (Exception Ex)
            {
                SearchModel.Search = "";
                model.ItemSearch = SearchModel;
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        private void RemindermailTataElxsi(TempPQPersonalForSendMailViewModel personalData, PQCandidateLoginForSendMailViewModel loginDetail, int pID, string ToMail, string Ccmail, string Subject, int ClientRowID, clsSendMail mail, string DisplayName, string From)
        {
            string body = string.Empty;
            string MailSubject = "";
            ClientCustomize cc = new ClientCustomize();
            var templateDB = cc.getAlertMailtemplate(ClientRowID, "Follow UP", 0, 1, "FollowUPSendToCandidateForTataElxsi");
            if (templateDB.Rows.Count > 0)
            {
                body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
            }

            body = body.Replace("{CandidateName}", loginDetail.CandidateName);

            body = body.Replace("{XXXXXX}", loginDetail.UserID);
            body = body.Replace("{XXXXXX}", clsCommonMethods.PasswordDecrypt(loginDetail.UPass.Trim()));
            body = body.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
            cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, body, ClientRowID, 1);
        }


        private void RemindermailAstraZenca(TempPQPersonalForSendMailViewModel personalData, PQCandidateLoginForSendMailViewModel loginDetail, int pID, string ToMail, string Ccmail, string Subject, int ClientRowID, clsSendMail mail, string DisplayName, string From)
        {


            string body = string.Empty;
            ClientCustomize cc = new ClientCustomize();
            string MailSubject = "";
            var templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplateForSpocAndClient");
            if (templateDB.Rows.Count > 0)
            {
                body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
            }
            body = body.Replace("{CandidateName}", loginDetail.CandidateName);
            body = body.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
            cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, body, ClientRowID, 1);
        }


        private void SaveReminderMail(TempPQPersonalForSendMailViewModel personalData, PQCandidateLoginForSendMailViewModel loginDetail, int pID, string ToMail, string Ccmail, string Subject, int ClientRowID, clsSendMail mail, string DisplayName, string From)
        {


            string body = string.Empty;
            ClientCustomize cc = new ClientCustomize();
            string MailSubject = "";
            if (ClientRowID == 679)
            {

                var templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractalAnalyticsReminderMail");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }
                //using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/FractalAnalyticsReminderMail.html")))
                //{

                //    body = reader.ReadToEnd();
                //}
            }

            if (ClientRowID == 725)
            {
                var templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractalAnalyticsAustraliaEmailTemplateReminder");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }
                //    using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/FractalAnalyticsAustraliaEmailTemplateReminder.html")))
                //    {

                //        body = reader.ReadToEnd();
                //    }
            }

            body = body.Replace("{CandidateName}", loginDetail.CandidateName);
            body = body.Replace("{XXXXXX}", loginDetail.UserID);
            body = body.Replace("{XXXXXX}", clsCommonMethods.PasswordDecrypt(loginDetail.UPass.Trim()));
            body = body.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
            cc.saveCandidatemail(pID, "Reminder mail", ToMail, Ccmail, Subject, body, ClientRowID, 1);
        }

        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public ActionResult AddBulkCandidateCaseUpload(short id = 0, string msg = "")
        {
            AddTempPQPersonalForBulkUpViewModel model = new AddTempPQPersonalForBulkUpViewModel();
            try
            {
                model.ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                model.ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                if (model.ClientRowID == 1209)
                {
                    BindClientListByLogInName(model.ClientRowID);
                    ViewBag.clientRowId = model.ClientRowID.ToString();
                    model.ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                }
                if (model.ClientRowID == 858)
                {
                    string FileName = "CandidateUploadTemplatePatra.xlsx";
                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["clientCanidatetemplate"].ToString();
                    model.Template = Url.Content(FldPathDoc + FileName);

                }
                else if (model.ClientRowID == 722)//Genpact
                {
                    string FileName = "CandidateUploadTemplate_Genpact.xlsx";
                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["clientCanidatetemplate"].ToString();
                    model.Template = Url.Content(FldPathDoc + FileName);

                }
                else if (model.ClientRowID == 1057)//CTSIL UAN
                {
                    string FileName = "CandidateUploadTemplate _CTSIL_UAN.xlsx";
                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["clientCanidatetemplate"].ToString();
                    model.Template = Url.Content(FldPathDoc + FileName);

                }
                else
                {
                    string FileName = "CandidateUploadTemplate.xlsx";
                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["clientCanidatetemplate"].ToString();
                    model.Template = Url.Content(FldPathDoc + FileName);
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


        [HttpPost]
        [SessionExpireFilterAttributeClient]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public async Task<ActionResult> AddBulkCandidateCaseUpload(AddTempPQPersonalForBulkUpViewModel model, FormCollection formCollection)
        {
            try
            {

                short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                string strBtn = formCollection["Savebtn"] != null ? formCollection["Savebtn"].ToString() : "Submit";

                if (ClientRowID == 1209)
                {
                    ClientRowID = Convert.ToInt16(formCollection["ClientRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(formCollection["ClientRowID"].ToString());
                    model.ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);

                }
                string errorMessage = await GatZipFileMessageXLS(model.UploadDoc, model.ClientRowID.ToString(), model.ClientUserRowID.ToString());
                if (errorMessage != "")
                {
                    ViewBag.ErrorMsg += errorMessage;
                }
                else
                {
                    if (model.ClientRowID > 0 && model.ClientUserRowID > 0 && !string.IsNullOrEmpty(model.CandidateCheckOrPackage))
                    {
                        int fileNo = repoPQClientCandiBulkUpload.GetNoOfExcelFileByClientID(model.ClientRowID);
                        if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                        {
                            short TeamMemberRowID = repoPQPersonal.GetDETLByClientID(model.ClientRowID, "Team Leader-DE");
                            if (TeamMemberRowID > 0)
                            {
                                model.CreatedBy = TeamMemberRowID; //  Convert.ToInt16(Session["ClientUserRowID"]); DE TL ID InsteadOf ClientUserRowId
                                string FolderPath = string.Empty;
                                string Extension = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                                string FileName = (Path.GetFileNameWithoutExtension(model.UploadDoc.FileName) + "_" + fileNo.ToString()).Replace(" ", "_").Replace("-", "_") + Extension;
                                int count = FileName.Split('.').Length - 1;
                                if (count > 1)
                                {
                                    ViewBag.ErrorMsg = "please check file name";
                                    return View(model);

                                }
                                FolderPath = ConfigurationManager.AppSettings["clientCanidatetemplate"].ToString();
                                string FilePath = Server.MapPath(FolderPath + FileName);
                                string base64 = string.Empty;
                                model.UploadDoc.InputStream.Position = 0;
                                using (var memoryStream = new MemoryStream())
                                {
                                    model.UploadDoc.InputStream.CopyTo(memoryStream); // Convert the byte array to Base64 
                                    base64 = Convert.ToBase64String(memoryStream.ToArray());
                                }
                                _documentService.UploadDynamicCandidateFiles(FileName, base64, "D:/wwwroot2/Documents/ClientDocuments/ClientCandidateUploadedExcel/");
                                //model.UploadDoc.SaveAs(Server.MapPath(FolderPath + FileName));

                                int res = SaveClientCandidateUploadFileWithChecks(model.ClientRowID, FileName, model.CandidateCheckOrPackage);
                                string TicketNo = model.ClientRowID + "-" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "-" + fileNo;
                                if (res > 0)
                                {
                                    ViewBag.ErrorMsg = ViewBag.SuccessFully;
                                    ModelState.Clear();
                                    ImportExcelAndSaveCandidateByBulkUpload(FilePath, Extension, "Yes", model, TicketNo, strBtn);
                                    return RedirectToAction("DuplicateReference", "Home", new { ticid = TicketNo, msg = ViewBag.ErrorMsg });
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Please assign data entry team leader for this client");
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Please Select Excel Document!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invalid ClientID!";
                    }
                }
            }
            catch (Exception Ex)
            {
                // ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public ActionResult AddSingleCandidateCaseUpload(FormCollection formCollection, string msg = "")
        {

            if (msg != "")
            {
                ViewBag.ErrorMsg = msg;
            }
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            BindTitleList();
            int ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
            int ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
            BindClientListByLogInName(ClientRowID);
            BindCandidateTypeList();
            BindBusinessUnitList(ClientUserRowID, 0);

            ClientCustomize cc = new ClientCustomize();
            DataTable dt = new DataTable();
            dt = cc.Antecedents_select_form(ClientRowID, 0, "Client Candidate", "Case");

            dt.DefaultView.RowFilter = "AntecedentsRowID=19";
            var tblMand = dt.DefaultView.ToTable();
            dt.DefaultView.RowFilter = "";
            string strMobile = "";
            if (tblMand.Rows.Count > 0)
            {
                strMobile = Convert.ToString(tblMand.Rows[0]["QLabel"]).Split('_')[3];
            }
            ViewBag.Mandatory = strMobile;
            TempData["Exists"] = 0;


            foreach (DataRow d in dt.Rows)
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

            ViewBag.clientRowId = ClientRowID.ToString();
            if (ClientRowID == 1209)
            {
                BindClientListByLogInName(ClientRowID, ClientUserRowID);
            }

            ViewBag.clientRowId = ClientRowID.ToString();
            ClientCustomize bl = new ClientCustomize();
            ViewBag.LastFiveCases = bl.LastFiveCasesInitiatedToCandidate(ClientRowID, ClientUserRowID);


            return View(marksModel);
        }


        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public ActionResult AddSingleCandidateCaseUploadUAN(FormCollection formCollection, string msg = "")
        {

            if (msg != "")
            {
                ViewBag.ErrorMsg = msg;
            }
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            BindTitleList();
            int ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
            int ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
            BindClientListByLogInName(ClientRowID);
            BindCandidateTypeList();
            BindBusinessUnitList(ClientUserRowID, 0);

            ClientCustomize cc = new ClientCustomize();
            DataTable dt = new DataTable();
            dt = cc.Antecedents_select_form(ClientRowID, 0, "Client Candidate", "Case");

            dt.DefaultView.RowFilter = "AntecedentsRowID=19";
            var tblMand = dt.DefaultView.ToTable();
            dt.DefaultView.RowFilter = "";
            string strMobile = "";
            if (tblMand.Rows.Count > 0)
            {
                strMobile = Convert.ToString(tblMand.Rows[0]["QLabel"]).Split('_')[3];
            }
            ViewBag.Mandatory = strMobile;
            TempData["Exists"] = 0;


            foreach (DataRow d in dt.Rows)
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

            ViewBag.clientRowId = ClientRowID.ToString();
            if (ClientRowID == 1209)
            {
                BindClientListByLogInName(ClientRowID, ClientUserRowID);
            }

            ViewBag.clientRowId = ClientRowID.ToString();
            ClientCustomize bl = new ClientCustomize();
            ViewBag.LastFiveCases = bl.LastFiveCasesInitiatedToCandidate(ClientRowID, ClientUserRowID);


            return View(marksModel);
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
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindClientListByLogInName(int ClientRowid)
        {
            try
            {
                ViewBag.ClientList = repoDDL.GetClientGroupingInfo(ClientRowid);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [NonAction]
        void BindBusinessUnitList(int ClientUserRowid, int BURowId = 0)
        {
            try
            {
                ClientCustomize CC = new ClientCustomize();
                ViewBag.BusinessUnitList = repoDDL.GetBusinessUnitList(ClientUserRowid, BURowId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        public int PepPersonal;
        [HttpPost]
        [SessionExpireFilterAttributeClient]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public async Task<ActionResult> AddSingleCandidateCaseUpload(FormCollection formCollection)
        {

            TempData["Exists"] = 0;
            ViewBag.ErrorMsg = "";
            dynamic expando = new ExpandoObject();

            string strBtn = formCollection["Savebtn"].ToString();
            string clientRowid = formCollection["ClientRowID"].ToString();
            int ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
            BindBusinessUnitList(ClientUserRowID, 0);
            string CandidateCheckOrPackage = formCollection["CandidateCheckOrPackage"].ToString();
            int clientRowId = Convert.ToInt32(clientRowid);

            if (CandidateCheckOrPackage == "")
            {
                ViewBag.ErrorMsg = "Pricing Type is mandatory field please select check or package from below list";
            }


            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                AddTempPQPersonalForBulkUpViewModel model = new AddTempPQPersonalForBulkUpViewModel();

                model.ClientRowID = Convert.ToInt16(clientRowid);
                model.CandidateCheckOrPackage = CandidateCheckOrPackage;
                if (clientRowId == 193)
                {
                    if (formCollection["343"].ToString() != null)
                    {
                        model.AlternateEmailID = formCollection["343"].ToString();
                    }
                }
                if (clientRowId == 1209)
                {
                    model.ClientUserRowID = 0;
                    cd.clientRowId = Convert.ToInt16(formCollection["ClientRowID"].ToString());
                }
                else
                {
                    cd.clientRowId = clientRowId;
                }

                cd.CreatedBy = Convert.ToInt32(Session["CandidateUserRowID"]);

                ClientCustomize bl = new ClientCustomize();

                DataTable dt = new DataTable();

                dt = bl.Antecedents_select_form(cd.clientRowId, 0, "client candidate", "Case");
                dt.DefaultView.RowFilter = "AntecedentsRowID=19";
                var tblMand = dt.DefaultView.ToTable();
                dt.DefaultView.RowFilter = "";
                string strMobile = "";
                if (tblMand.Rows.Count > 0)
                {
                    strMobile = Convert.ToString(tblMand.Rows[0]["QLabel"]).Split('_')[3];
                }
                ViewBag.Mandatory = strMobile;
                string mobileNo = "";//email
                string emailid = ""; //mobile no

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
                                if (key == "19")
                                {
                                    mobileNo = cd.DataEntryValue;
                                    if (clientRowId == 684)
                                    {
                                    }
                                    else
                                    {
                                        if (mobileNo.Length > 10)
                                        {
                                            ViewBag.ErrorMsg = "Please enter 10-digit Mobile Number";
                                        }
                                    }                                    
                                }
                                if (key == "8")
                                    emailid = cd.DataEntryValue;
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {

                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + cd.DisplayName + " is a mandatory field.";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {

                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + cd.DisplayName + " is a mandatory field.";
                                }
                            }
                            if (d[4].ToString() == "fileUpload")
                            {

                                foreach (string filenm in Request.Files)
                                {
                                    if (filenm == d[0].ToString())
                                    {
                                        HttpPostedFileBase postedFile = Request.Files[filenm];
                                        if (postedFile.ContentLength > 0)
                                        {
                                            string errorMessage = await GatZipFileMessage(postedFile, model.ClientRowID.ToString(), filenm);
                                            if (errorMessage != "")
                                            {
                                                ViewBag.ErrorMsg += errorMessage;
                                            }
                                        }
                                        else
                                        {
                                            if (FString[3].Trim().ToLower() == "mandatory")
                                            {
                                                ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + FString[1] + " is a mandatory field.";
                                            }
                                        }
                                    }

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

                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1].Trim();
                                cd.DataEntryValue = formCollection[key].ToString();

                                if (d[0].ToString() == "121") //candidate full Name
                                {
                                    model.CandidateName = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "20") // candidate first name
                                {
                                    model.Name = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "18") // candidate middle name
                                {
                                    model.MiddleName = cd.DataEntryValue;

                                }

                                if (d[0].ToString() == "14") // candidate last name
                                {
                                    model.LastName = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "8") // candidate Email
                                {
                                    model.EmailID = cd.DataEntryValue;

                                }

                                if (d[0].ToString() == "19") // mobile no
                                {
                                    model.MobileNo = cd.DataEntryValue;

                                }

                                if (d[0].ToString() == "4") // client ref id
                                {
                                    model.ClientRefID = cd.DataEntryValue;

                                }

                                if (d[0].ToString() == "16") // location
                                {
                                    //model.pla = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "9") // location
                                {
                                    model.EmployeID = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "7") // location
                                {
                                    model.Designation = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "208") // Entity
                                {
                                    model.Entity = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "6") // DOJ
                                {
                                    if( !string.IsNullOrEmpty(cd.DataEntryValue) )
                                    {
                                        model.DateofJoining = Convert.ToDateTime(cd.DataEntryValue);
                                    }

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

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                if (d[0].ToString() == "24") //Title
                                {
                                    model.CandTitle = cd.DataEntryValue;
                                    if (model.CandTitle == "Mr." || model.CandTitle == "Mohd.")
                                    {
                                        model.Gender = "Male";
                                    }
                                    else
                                    {
                                        model.Gender = "Female";
                                    }
                                }
                                if (d[0].ToString() == "208")
                                {
                                    model.BusinessName = cd.DataEntryValue;

                                }

                                if (d[0].ToString() == "23")
                                {
                                    model.PlaceOfJoining = cd.DataEntryValue;
                                }

                                if (d[0].ToString() == "644")
                                {
                                    model.TCSClientType = cd.DataEntryValue;
                                }
                                if (cd.clientRowId == 685)
                                {
                                    if (d[0].ToString() == "208") // Entity
                                    {
                                        model.Entity = cd.DataEntryValue;

                                    }
                                }



                            }
                        }
                    }

                }
                // ── Client 684: Entity Name not in dt (Client Candidate module), read directly ──
                //if (clientRowId == 684)
                //{
                //    if (formCollection["208"] != null)
                //        model.Entity = formCollection["208"].ToString();

                //    if (formCollection["OfferType"] != null)
                //        model.OfferType = formCollection["OfferType"].ToString();
                //}
                // ── end 684 ──

                model.ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                model.ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                if (model.ClientRowID == 1209)
                {
                    model.ClientUserRowID = 0;
                    model.ClientRowID = Convert.ToInt16(formCollection["ClientRowID"].ToString());
                }
                // client 684 hard guard entity name must never save null
                if (model.ClientRowID == 684 && string.IsNullOrWhiteSpace(model.Entity))
                {
                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + " Entity Name is a mandatory field";
                }

                model.PersonalRowID = 0;
                if (strBtn == "Submit")
                {

                    int existcandidate = 0;
                    int[] ClientRowIds = { 1062, 1091, 1086, 1067, 1067, 1072, 1077, 1266, 1268, 1068, 1127, 1193, 1074, 1155 };
                    if (Array.Find(ClientRowIds, element => element == model.ClientRowID) > 0)
                    {
                        var existcandidateDT = bl.GetExistsTempCandidateDetails(model.ClientRowID, model.CandidateName, model.EmailID, model.MobileNo, Convert.ToDateTime(model.DOB), model.AadhaarCardNo);
                        if (existcandidateDT.Rows.Count > 0)
                        {
                            existcandidate = 1;
                            ViewBag.ErrorMsg = model.CandidateName + " Candidate(s) already exist!";
                        }
                    }
                    if (existcandidate == 0)
                    {

                        if (ViewBag.ErrorMsg == "")
                        {
                            DataTable dts = bl.InsertTempPQPersonals(model);
                            if (Convert.ToString(dts.Rows[0]["CaseType"]) == "New")
                            {
                                model.PersonalRowID = Convert.ToInt32(dts.Rows[0][0].ToString());
                                PepPersonal = Convert.ToInt32(dts.Rows[0][0]);
                                CreateUserIdForCandidate(model);
                                SaveTempPackageOrCheckWithCandidate(model);
                                if (model.ClientRowID == 679)
                                {
                                    try
                                    {
                                        SendmailtoCandidate(model.PersonalRowID.ToString(), model.ClientRowID, model.ClientUserRowID);
                                    }
                                    catch (Exception)
                                    {
                                        bl.AddSentInviteEmailCases(Convert.ToInt32(model.ClientRowID), Convert.ToInt32(model.ClientUserRowID), Convert.ToInt32(model.PersonalRowID));


                                    }
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "Record already exists with,";
                                if (Convert.ToString(dts.Rows[0]["CompanyRefNo"]) != "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + Convert.ToString(dts.Rows[0]["CompanyRefNo"]);
                                }
                                if (Convert.ToString(dts.Rows[0]["Emailid"]) != "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + "Email ID  :" + Convert.ToString(dts.Rows[0]["Emailid"]);
                                }
                                if (Convert.ToString(dts.Rows[0]["MobileNo"]) != "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + "Mobile No : " + Convert.ToString(dts.Rows[0]["MobileNo"]);
                                }

                                string strMsg = ViewBag.ErrorMsg;
                                if (strMsg != "")
                                {
                                    strMsg = strMsg.Substring(0, strMsg.LastIndexOf(','));
                                    ViewBag.ErrorMsg = strMsg;
                                }

                                TempData["Exists"] = 1;
                            }
                        }

                    }
                }
                else if (strBtn == "Force Submit")
                {
                    DataTable dts = bl.InsertForceTempPQPersonals(model);
                    model.PersonalRowID = Convert.ToInt32(dts.Rows[0][0].ToString());
                    CreateUserIdForCandidate(model);
                    SaveTempPackageOrCheckWithCandidate(model);
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
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string base64 = string.Empty;
                                    postedFile.InputStream.Position = 0;
                                    using (var memoryStream = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(memoryStream); // Convert the byte array to Base64 
                                        base64 = Convert.ToBase64String(memoryStream.ToArray());
                                    }
                                    _documentService.UploadDynamicCandidateFiles(filename, base64, "D:/wwwroot2/Uploads/");
                                    if(clientRowId== 753)
                                    {
                                        _documentService.UploadDynamicCandidateFiles(filename, base64, "D:/wwwroot2/Documents/CandidateDocuments/ProvidedDocuments/");
                                    }
                                    // postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.PersonalRowID = model.PersonalRowID;
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    if (ViewBag.ErrorMsg == "")
                                    {
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
                                    bl.proc_InsertUpdateTempPQVerifiedUploadCaseDocs(cd, "~/Uploads/");
                                }
                                else
                                {
                                    marksModel.Add(d[1].ToString(), d[0].ToString());
                                    if (FString[3].Trim().ToLower() == "mandatory")
                                    {
                                        //ViewBag.ErrorMsg = ViewBag.ErrorMsg + "<br>" +   FString[1] + " is a mandatory field";
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + FString[1] + " is a mandatory field.";
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
                                cd.PersonalRowID = model.PersonalRowID;
                                if (ViewBag.ErrorMsg == "")
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

                                if (cd.AntecedentRowID == 380)
                                {
                                    if (clientRowid == "1430")
                                    {
                                        cd.DataEntryValue = bl.GetVarrocBusinessUnit(Convert.ToInt32(formCollection[key]));
                                    }
                                    else
                                    {
                                        cd.DataEntryValue = formCollection[key].ToString();
                                    }
                                }
                                else
                                {
                                    cd.DataEntryValue = formCollection[key].ToString();
                                }
                                cd.PersonalRowID = model.PersonalRowID;
                                if (ViewBag.ErrorMsg == "")
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
                        cd.PersonalRowID = model.PersonalRowID;
                        if (ViewBag.ErrorMsg == "")
                            bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
                if (model.ClientRowID == 679)
                {
                    SendAutoEmailTocandidateAfterInitiation(model.PersonalRowID.ToString(), "");
                }
            }
            catch (Exception Ex)
            {
                string str = Ex.Message;
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg =  " Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientListByLogInName(clientRowId);
            BindCandidateTypeList();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();


            /////////////////////////////////////////////////////////////////////////


            if (clientRowId == 1324 || clientRowId == 1470)
            {
                ClientCustomize cc = new ClientCustomize();
                TempPQPersonalInfoListPagedModel mm = new TempPQPersonalInfoListPagedModel();
                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = Convert.ToString(formCollection["Search"]);

                string PersonalIds = PepPersonal.ToString();

                try
                {
                    short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    //int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    int pageSize = clsGlobalCommon.PageSize;


                    ViewBag.ErrorMsg = "";
                    if (ViewBag.ErrorMsg == "")
                    {
                        #region Send Mail To Candidate
                        if (!string.IsNullOrEmpty(PersonalIds))
                        {
                            string spocEmail = "";
                            string cSpocEmail = "";
                            string mobileno = "";
                            string[] PersonalID = PersonalIds.Split(',');
                            string TrmFldPathDoc = "";
                            string SendUserManulToCtrls = "";
                            string CtrlsfileName = "";
                            string fileName = "";
                            if (ClientRowID == 576)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "LOA.pdf";
                                fileName = Path.GetFileNameWithoutExtension("LOA.pdf");
                            }
                            else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Chargebee_LOA.docx";
                                fileName = Path.GetFileNameWithoutExtension("Chargebee_LOA.docx");
                            }
                            else if (ClientRowID == 1425)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "GlobalAuthorization-Australia.docx";
                                fileName = Path.GetFileNameWithoutExtension("GlobalAuthorization-Australia.docx");
                            }
                            else if (ClientRowID == 1428)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Genpact Privacy Notice and Consent Form template_Pre-Employment Screening_EU (Securitas).docx";
                                fileName = Path.GetFileNameWithoutExtension("Genpact Privacy Notice and Consent Form template_Pre-Employment Screening_EU (Securitas).docx");
                            }
                            else if (ClientRowID == 1497)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Capco Privacy Notice and Consent Form template_Pre-Employment Screening(Securitas).docx";
                                fileName = Path.GetFileNameWithoutExtension("Capco Privacy Notice and Consent Form template_Pre-Employment Screening(Securitas).docx");
                            }
                            else if (ClientRowID == 1429)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Global_Authorization-Nagarro.docx";
                                fileName = Path.GetFileNameWithoutExtension("Global_Authorization-Nagarro.docx");
                            }
                            else if (ClientRowID == 1449)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "GlobalAuthorization-Uniqus.docx";
                                fileName = Path.GetFileNameWithoutExtension("GlobalAuthorization-Uniqus.docx");
                            }
                            else if (ClientRowID == 1514)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "SILVERLINK-I.docx";
                                fileName = Path.GetFileNameWithoutExtension("SILVERLINK-I.docx");
                            }
                            else if (ClientRowID == 1481)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CtrlS_Datacenter-LOA.pdf";
                                fileName = Path.GetFileNameWithoutExtension("CtrlS_Datacenter-LOA.pdf");
                                SendUserManulToCtrls = ConfigurationManager.AppSettings["CandidateManual"].ToString() + "CandidateManual.pdf";
                                CtrlsfileName = Path.GetFileNameWithoutExtension("CandidateManual.pdf");
                            }
                            else
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CandidateAgreement.pdf";
                                fileName = Path.GetFileNameWithoutExtension("CandidateAgreement.pdf");
                            }

                            //Attachment attachment;
                            Attachment attachment;
                            Attachment attachment2;
                            if (ClientRowID == 1481)
                            {
                                attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                                attachment2 = new Attachment(Server.MapPath(SendUserManulToCtrls));
                            }
                            else
                            {
                                attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                                attachment2 = null;
                            }

                            string MailSignature = string.Empty;
                            string From = string.Empty;
                            string DisplayName = string.Empty;
                            string Cc = "";

                            clsSendMail mail;

                            var cSmtp = repoPQClient.GetPQClientSendMailFromById(ClientRowID);
                            DataTable dtEmail = new DataTable();

                            string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);
                            if (ClientAccessType == "3")
                            {
                                dtEmail = cc.userwise_ReportingEmailAccess(ClientUserRowID);
                                if (dtEmail.Rows.Count > 0)
                                {
                                    spocEmail = dtEmail.Rows[0]["ClientUserEmailID"].ToString();
                                    cSpocEmail = dtEmail.Rows[0]["ReportingTo_EmailID"].ToString();

                                }
                            }
                            else
                            {
                                dtEmail = cc.GetEmailStructureForAlert(ClientRowID, ClientUserRowID, "Case Registration");
                                if (dtEmail.Rows.Count > 0)
                                {
                                    spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                                    cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                                    mobileno = dtEmail.Rows[0]["mobileno"].ToString();

                                }
                            }
                            Cc = cSpocEmail;
                            if (cSmtp != null && cSmtp.MailSendBy > 0)
                            {
                                mail = new clsSendMail(cSmtp.SMTPServer, cSmtp.SMTPPort, cSmtp.SMTPUserName, clsCommonMethods.PasswordDecrypt(cSmtp.SMTPPassword), Convert.ToBoolean(cSmtp.EnableSsl));

                                From = cSmtp.SMTPUserName;
                                DisplayName = cSmtp.ClientName.Trim();
                                Cc = string.Concat(Cc, ",", spocEmail);

                                MailSignature = string.Concat("<b>", cSmtp.ClientName.Trim(), "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", cSmtp.SMTPUserName, "'>", cSmtp.SMTPUserName, "</a>");
                            }
                            else
                            {
                                mail = new clsSendMail();
                                var signature = mail.GetCompanyMailSignature();

                                if (mobileno != "")
                                    mobileno = "<b>Mobile: " + mobileno;
                                string ShowSpocId = spocEmail.ToLower();

                                ShowSpocId = ShowSpocId.Replace(",", ";");
                                string[] ShowSpocId1 = spocEmail.Split(';');
                                if (signature != null)
                                {

                                    // For TATA Elxsi
                                    if (ClientRowID == 635)
                                    {
                                        MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                        //"<b>Mobile: ", signature.MobileNo,
                                        "</b><br/><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", "bgv.helpdesk@securitas-india.com", "'>", "bgv.helpdesk@securitas-india.com", "</a>");
                                    }
                                    else if (ClientRowID == 249)
                                    {
                                        MailSignature = string.Concat("For Securitas India. <br /><br />");
                                        MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                        //"<b>Mobile: ", signature.MobileNo,
                                        "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                        "</b><br /><b>Contact No:</b><br />  <b>Satya :</b>+91 8377022632 <br />  <b>Pradeep Devrani :</b> +91 8373907723<br /><b>Himanshu Pathak :</b> +91 7291974515 ");
                                        //Please mention the contact number of Satya - +91 8377022632 / Pradeep Devrani - 91 - 8373907723 / Himanshu Pathak - +91 7291974515
                                    }
                                    else if (ClientRowID == 277)
                                    {
                                        MailSignature = string.Concat("For Securitas India. <br /><br />");
                                        MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                        //"<b>Mobile: ", signature.MobileNo,
                                        "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                        "</b><br /><b>Contact No </b>:+91-8373907723");
                                    }
                                    else
                                    {
                                        MailSignature = string.Empty;
                                        MailSignature = string.Concat(ShowSpocId1[0]);
                                    }

                                }



                                Cc = string.Concat(Cc, ",", ShowSpocId);

                                From = signature.GeneralEmail;
                                DisplayName = signature.CompanyName.Trim();
                            }
                            //int sum = 0;
                            foreach (var item in PersonalID)
                            {
                                int pID = Convert.ToInt32(item);
                                var personalData = repoPQPersonal.GetTempPQPersonalDetailsForSendMailById(pID);
                                var loginDetail = repoCandidate.GetCandidateLoginDetailsForMailSendById(pID);
                                DataTable d = new DataTable();
                                StringBuilder docString = new StringBuilder();

                                if (ClientRowID == 1425 || ClientRowID == 1428 || ClientRowID == 1222)
                                {
                                    d = cc.GetDocumentForVerificationList(pID, ClientRowID);
                                    foreach (DataRow dr in d.Rows)
                                    {
                                        docString.Append("<tr><td style='padding:3px;'>" + dr["SubCheckName"].ToString() + " </td><td>" + dr["DocumentForVerification"].ToString() + " </td></tr>");
                                    }
                                }
                                else if (ClientRowID == 193)
                                {
                                    d = cc.GetDocumentForVerificationList(pID, ClientRowID);
                                    foreach (DataRow dr in d.Rows)
                                    {
                                        docString.Append("<tr><td style='padding:3px;'>" + dr["SubCheckName"].ToString() + " </td><td>" + dr["DocumentForVerification"].ToString() + " </td></tr>");
                                    }
                                }
                                else
                                {
                                    d = cc.CandidateEmailDocFormat(pID); // it should be subcheck family based on special check, 
                                    foreach (DataRow dr in d.Rows)
                                    {
                                        if (ClientRowID == 1357)
                                        {
                                            //---- Employment Check Start from here ----//
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Past two Employments </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");
                                        }
                                        else if (ClientRowID == 1425)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Any available document. Note: Two employment details prior to Chargebee.</ td></tr>");
                                        }
                                        else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Experience letter / Service Certificate</td></tr>");
                                        }
                                        else if (ClientRowID == 1428)
                                        {

                                        }
                                        else if (ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Experience letter, Service Certificate, Other</td></tr>");
                                        }
                                        else if (ClientRowID == 1481)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Current Company Employment<br> • Experience Letter / Relieving Letter<br> • Salary Slip for last 3 months <br> Previous Employment(only latest 2 employers)<br> • Experience Letter / Relieving Letter </ td></tr>");
                                        }
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");
                                        }
                                        //---- Employment Check End from here ----//

                                        //---- Education Check Start from here ----//
                                        if (ClientRowID == 1355 || ClientRowID == 1348)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Marksheet/Provisional Degree Certificate </td></tr>");
                                        }
                                        else if (ClientRowID == 1318)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Sanad Certification </td><td>Required Bar Council Certificate</td></tr>");
                                        }
                                        else if (ClientRowID == 1425)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Transcript/Degree Certificate</td></tr>");
                                        }
                                        else if (ClientRowID == 1428)
                                        {

                                        }
                                        else if (ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Transcript/ Degree Certificate/ Convocation Certificate</td></tr>");
                                        }
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");
                                        }
                                        //---- Education Check End from here ----//

                                        //---- Address Check Start from here ----//
                                        if (ClientRowID == 1425)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>Driver's License/Utility Bill</td></tr>");
                                        }
                                        if (ClientRowID == 1395)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>SSN Number</td></tr>");
                                        }
                                        else if (ClientRowID == 1428)
                                        {

                                        }
                                        else if (ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>National ID Proof</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>• Address Proof </td></tr>");
                                        }
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>Address Proof for Current & Permanent Address </td></tr>");
                                        }
                                        //---- Address Check End from here ----//

                                        //---- National Check Start from here ----//
                                        if (ClientRowID == 1355 || ClientRowID == 1348)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td>ID Verification</td><td>Passport preferred</td></tr>");
                                        }
                                        else if (ClientRowID == 1425 || ClientRowID == 1428 || ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td>National ID Check </td><td>National ID Proof</td></tr>");
                                        }
                                        else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td>National ID Check </td><td>Passport / National ID</td></tr>");
                                        }
                                        else if (ClientRowID == 559)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Pan Card</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Photo ID Proof </td></tr>");
                                        }
                                        else if (ClientRowID == 648)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>ID Proof – Pan Card and Aadhar Card</td></tr>");
                                        }
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>ID Proof </td></tr>");
                                        }
                                        //---- National Check End from here ----//

                                        //---- Special Check Start from here ----//
                                        if (ClientRowID == 487)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "40")
                                                docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>Required Aadhar Card</td></tr>");
                                        }
                                        else if (ClientRowID == 1470)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "40")
                                                docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>NA</td></tr>");
                                        }
                                        else if (ClientRowID == 193)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "40")
                                                docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>NA</td></tr>");
                                        }

                                        //---- Special Check End from here ----//

                                        //---- Criminal Check Start from here ----//
                                        if (ClientRowID == 487 || ClientRowID == 1428 || ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>National ID Proof</td></tr>");
                                        }
                                        else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>Passport / National ID</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>• Address Proof </td></tr>");
                                        }
                                        else if (ClientRowID == 648)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>Require – Address proof, ID Proof and Passport Size Photo</td></tr>");
                                        }
                                        else if (ClientRowID == 1470)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>National ID Proof</td></tr>");
                                        }
                                        //---- Criminal Check End from here ----//
                                    }
                                }

                                if (loginDetail != null)
                                {
                                    string body = string.Empty;
                                    string bodySpocAndClient = string.Empty;
                                    string SaveWPassMail = string.Empty;
                                    DataTable tblExperiece = cc.CheckCandidateExperienced(personalData.ClientRowID, personalData.PersonalRowID);

                                    string MailSubject = "";
                                    string SpocAndClientMailSubject = "";
                                    string MailTemplateName = "";
                                    string MailTemplateNameSpoc = "";
                                    int MailClientRowid = 0;
                                    var templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplate");
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                        MailTemplateName = Convert.ToString(templateDB.Rows[0]["TemplateName"]);
                                        MailClientRowid = Convert.ToInt32(templateDB.Rows[0]["ClientRowId"]);
                                    }
                                    var templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplateForSpocAndClient");
                                    if (templateDBSpocAndClient.Rows.Count > 0)
                                    {
                                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                        MailTemplateNameSpoc = Convert.ToString(templateDBSpocAndClient.Rows[0]["TemplateName"]);
                                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                    }

                                    if (ClientRowID == 679)
                                    {
                                        DataTable tblfractal = cc.CheckCandidateExperiencedByCandidateType(personalData.ClientRowID, personalData.PersonalRowID);
                                        if (tblfractal.Rows.Count > 0)
                                        {
                                            if (tblfractal.Rows[0][0].ToString() == "Existing Employee")
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployee");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployeeForSpocAndClient");
                                            }
                                            else
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                            }
                                        }
                                        if (templateDB.Rows.Count > 0)
                                        {
                                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                            if (ClientRowID == 679 || ClientRowID == 2)
                                            {
                                                SaveWPassMail = Convert.ToString(templateDB.Rows[0]["WLDMailTemplate"]);
                                            }
                                        }
                                        if (templateDBSpocAndClient.Rows.Count > 0)
                                        {
                                            bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                            SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                        }
                                    }

                                    else if (ClientRowID == 576)
                                    {
                                        //---------Experienced-----------
                                        if (tblExperiece.Rows.Count > 0)
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LateralMailNagarro");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LateralMailNagarroForSpocAndClient");
                                        }
                                        else
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "NagarroFresherMail");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "NagarroFresherMailForSpocAndClient");
                                        }
                                        if (templateDB.Rows.Count > 0)
                                        {
                                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                        }
                                        if (templateDBSpocAndClient.Rows.Count > 0)
                                        {
                                            bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                            SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                        }
                                    }
                                    else if (ClientRowID == 635) //Tata Elxsi
                                    {

                                        DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                        if (dtpackage.Rows.Count > 0)
                                        {
                                            if (dtpackage.Rows[0][0].ToString() == "TEL_Experienced (ID_CRCP_CRCC_PADD_CADD_GDB_2EMP")
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 671, 1, "LoginDetailsSendToCandidateFotTataElxsiExp");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiExpSpocAndClient");
                                            }
                                            else
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresher");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresherSpocAndClient");
                                            }
                                        }
                                        if (templateDB.Rows.Count > 0)
                                        {
                                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                        }
                                        if (templateDBSpocAndClient.Rows.Count > 0)
                                        {
                                            bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                            SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                        }
                                    }


                                    else if (ClientRowID == 647)
                                    {

                                        DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                        if (dtpackage.Rows.Count > 0)
                                        {
                                            if (dtpackage.Rows[0][0].ToString() == "NNSCIPL_Experienced (EDU_2EMP_PADD_GDB_CRCP_ID)")
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 737, 1, "LoginDetailsSendToCandidateForNovoNordiskExp");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskExpForSpocAndClient");
                                            }
                                            else
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresher");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresherForSpocAndClient");
                                            }
                                        }
                                        if (templateDB.Rows.Count > 0)
                                        {
                                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                        }
                                        if (templateDBSpocAndClient.Rows.Count > 0)
                                        {
                                            bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                            SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                        }
                                    }

                                    if (ClientRowID == 103)
                                    {
                                        body = body.Replace("{Remarks}", "Please login and fill the details within 3 days");
                                        bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Please login and fill the details within 3 days");
                                    }
                                    else if (ClientRowID == 1530)
                                    {
                                        body = body.Replace("{Remarks}", "Kindly fill the form within 2 working days");
                                        bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Kindly fill the form within 2 working days");
                                    }
                                    else
                                    {
                                        body = body.Replace("{Remarks}", "Please login and fill the details within 2 days");
                                        bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Please login and fill the details within 2 days");
                                    }

                                    string txtFolderPath = ConfigurationManager.AppSettings["mailimages"].ToString();
                                    LinkedResource Triangleres = new LinkedResource(Server.MapPath(txtFolderPath) + "Triangle.png", MediaTypeNames.Image.Jpeg);
                                    Triangleres.ContentId = Guid.NewGuid().ToString();
                                    string triangle = @"<img src='cid:" + Triangleres.ContentId + @"' width='25%'/>";

                                    LinkedResource Securitassec = new LinkedResource(Server.MapPath(txtFolderPath) + "Securitas-logo.png", MediaTypeNames.Image.Jpeg);
                                    Securitassec.ContentId = Guid.NewGuid().ToString();
                                    string securitas = @"<img src='cid:" + Securitassec.ContentId + @"' width='25%'/>";

                                    LinkedResource Walsonssec = new LinkedResource(Server.MapPath(txtFolderPath) + "Walsons-Temp-img.JPG", MediaTypeNames.Image.Jpeg);
                                    Walsonssec.ContentId = Guid.NewGuid().ToString();
                                    string walsons = @"<img src='cid:" + Walsonssec.ContentId + @"' width='25%'/>";

                                    LinkedResource BGVdottedImg = new LinkedResource(Server.MapPath(txtFolderPath) + "BGV-dotted-design.png", MediaTypeNames.Image.Jpeg);
                                    BGVdottedImg.ContentId = Guid.NewGuid().ToString();
                                    string BGVdotted = @"<img src='cid:" + BGVdottedImg.ContentId + @"' width='100%'/>";

                                    LinkedResource RedbulllogoImg = new LinkedResource(Server.MapPath(txtFolderPath) + "Redbullogo.png", MediaTypeNames.Image.Jpeg);
                                    RedbulllogoImg.ContentId = Guid.NewGuid().ToString();
                                    string redbulllogo = @"<img src='cid:" + RedbulllogoImg.ContentId + @"' width='30%'/>";

                                    LinkedResource JadeGlobalLogoIMG = new LinkedResource(Server.MapPath(txtFolderPath) + "JadeGlobalLogo.png", MediaTypeNames.Image.Jpeg);
                                    JadeGlobalLogoIMG.ContentId = Guid.NewGuid().ToString();
                                    string JadeGlobalLogo = @"<img src='cid:" + JadeGlobalLogoIMG.ContentId + @"' width='30%'/>";

                                    string ClientNameMail = Session["ClientNameMail"].ToString();
                                    //Candidate Template Details
                                    body = body.Replace("{document}", docString.ToString());
                                    body = body.Replace("{CandidateName}", loginDetail.CandidateName);
                                    body = body.Replace("#CompanyName", ClientNameMail);
                                    body = body.Replace("{ClientRefId}", personalData.ClientRefID);
                                    body = body.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                    body = body.Replace("{UName}", loginDetail.UserID);
                                    body = body.Replace("{UPasswd}", clsCommonMethods.PasswordDecrypt(loginDetail.UPass.Trim()));
                                    body = body.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                                    body = body.Replace("{triangle}", triangle);
                                    body = body.Replace("{securitas}", securitas);
                                    body = body.Replace("{walsons}", walsons);
                                    body = body.Replace("{BGVdotted}", BGVdotted);
                                    body = body.Replace("{redbulllogo}", redbulllogo);
                                    body = body.Replace("{JadeLogo}", JadeGlobalLogo);
                                    body = body.Replace("{MailSignature}", MailSignature);

                                    //Spoc And Client Template Details
                                    var Resendlinks = "https://bgv.securitas-india.com/account/ResendLink/?PersonalIds=" + pID.ToString();
                                    bodySpocAndClient = bodySpocAndClient.Replace("{Resendlink}", Resendlinks);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{document}", docString.ToString());
                                    bodySpocAndClient = bodySpocAndClient.Replace("{CandidateName}", loginDetail.CandidateName);
                                    bodySpocAndClient = bodySpocAndClient.Replace("#CompanyName", ClientNameMail);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                    bodySpocAndClient = bodySpocAndClient.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                                    bodySpocAndClient = bodySpocAndClient.Replace("{triangle}", triangle);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{securitas}", securitas);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{walsons}", walsons);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{BGVdotted}", BGVdotted);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{redbulllogo}", redbulllogo);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{JadeLogo}", JadeGlobalLogo);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{MailSignature}", MailSignature);

                                    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                                    alternateView.LinkedResources.Add(Triangleres);
                                    alternateView.LinkedResources.Add(Securitassec);
                                    alternateView.LinkedResources.Add(Walsonssec);
                                    alternateView.LinkedResources.Add(BGVdottedImg);
                                    alternateView.LinkedResources.Add(RedbulllogoImg);
                                    alternateView.LinkedResources.Add(JadeGlobalLogoIMG);

                                    AlternateView alternateViewSpocAndClient = AlternateView.CreateAlternateViewFromString(bodySpocAndClient, null, MediaTypeNames.Text.Html);
                                    alternateViewSpocAndClient.LinkedResources.Add(Triangleres);
                                    alternateViewSpocAndClient.LinkedResources.Add(Securitassec);
                                    alternateViewSpocAndClient.LinkedResources.Add(Walsonssec);
                                    alternateViewSpocAndClient.LinkedResources.Add(BGVdottedImg);
                                    alternateViewSpocAndClient.LinkedResources.Add(RedbulllogoImg);
                                    alternateViewSpocAndClient.LinkedResources.Add(JadeGlobalLogoIMG);

                                    if (ClientRowID == 2)
                                    {
                                        string key = loginDetail.CandidateUserRowID + "_" + personalData.ClientRowID + "_" + personalData.PersonalRowID;
                                        body = body.Replace("{MediaLink}", ConfigurationManager.AppSettings["CandidateMediaUrl"].ToString() + "?Key=" + clsCommonMethods.PasswordEncrypt(key));

                                    }
                                    string Subject = "";
                                    // For Sutherland Global  249,277
                                    if (ClientRowID == 249 || ClientRowID == 277)
                                    {
                                        Subject = loginDetail.CandidateName + "  Sutherland Background Verification by Securitas";
                                        DisplayName = "Sutherland BGV by Securitas";
                                    }
                                    // For TATA Elxsi
                                    else if (ClientRowID == 635)
                                    {
                                        body = body.Replace("{document}", "Documents Required<br /> Education: Degree Certificate / Final Year Marksheet < br /> " +
                                            "Address: Address Proof < br />   Nationality: ID Proof");
                                        Subject = "Background verification";
                                    }
                                    else
                                    {
                                        Subject = loginDetail.CandidateName + "  Background Verification";
                                    }
                                    string To = loginDetail.CandidateEmail;
                                    if (ClientRowID == 249 || ClientRowID == 277 || ClientRowID == 193)
                                    {
                                        Cc = Cc + ";" + personalData.AlternateEmail;
                                    }

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
                                    DataTable Dt = cc.getCandidateMailstatus(pID.ToString());

                                    int countSent = 0, countFailed = 0, totRec = 0;
                                    bool flag = false;
                                    bool flagspocandclient = false;
                                    if (Dt.Rows.Count > 0)
                                    {
                                        try
                                        {
                                            if (Convert.ToString(personalData.MobileNo) != "")
                                            {
                                                MessageStatus objMessage = new MessageStatus();
                                                WhatsAppAPI objs = new WhatsAppAPI();
                                                var link = "https://bgv.securitas-india.com/Account/CandidateLogin";
                                                objMessage = objs.SendWhatsAppMessage("+91" + personalData.MobileNo, "case_registration_candidate_invite_mode", loginDetail.CandidateName, "media_template", ClientNameMail, To, link);
                                                cc.sp_AddAuditWhatsAppAPI(personalData.PersonalRowID, objMessage.message, objMessage.status, "Candidate Invite");
                                            }
                                        }
                                        catch
                                        {

                                        }
                                        totRec = Dt.Rows.Count;
                                        for (int i = 0; i < totRec; i++)
                                        {
                                            if (Dt.Rows[0]["SentMailStatus"].ToString() == "0")
                                            {
                                                if (ClientRowID == 503)
                                                {
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                }
                                                else
                                                {
                                                    if (ClientRowID == 257) //Biz 2 Credit Info Services Pvt. Ltd
                                                    {
                                                        string bcc = "";
                                                        string Ccc = "";
                                                        Ccc = spocEmail;
                                                        bcc = cSpocEmail;
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Ccc, bcc, Subject, bodySpocAndClient);
                                                        //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, Ccc, bcc,Subject, bodySpocAndClient, null, null);
                                                    }
                                                    else if (ClientRowID == 249)//sutherlandglobal 
                                                    {
                                                        String CcSuthermail = "brij.bhushan@securitas-india.com";

                                                        CcSuthermail = CcSuthermail + "," + Ccmail;

                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, CcSuthermail, Subject, bodySpocAndClient, attachment, null);
                                                    }
                                                    // For TATA Elxsi
                                                    else if (ClientRowID == 635)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);

                                                    }
                                                    else if (ClientRowID == 957)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                    }
                                                    else if (ClientRowID == 1312)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                    }
                                                    else if (ClientRowID == 778)
                                                    {

                                                        String CcHHS = personalData.BusinessName;
                                                        if (CcHHS == null)
                                                        {
                                                            CcHHS = Ccmail.ToString();
                                                        }
                                                        else
                                                        {
                                                            CcHHS = CcHHS + "," + Ccmail;
                                                        }
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, CcHHS, Subject, bodySpocAndClient, attachment, null);

                                                    }
                                                    else if (ClientRowID == 1005 || ClientRowID == 2)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);

                                                    }
                                                    else if (ClientRowID == 889)
                                                    {
                                                        flag = mail.SendMailPurgWithoutCC(DisplayName, From, ToMail, Subject, body, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, cSpocEmail, Subject, bodySpocAndClient, attachment, null);
                                                    }
                                                    else if (ClientRowID == 974)
                                                    {
                                                        flag = mail.SendMailAdidas(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.SendMailAdidasForSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, body);
                                                    }
                                                    else if ((ClientRowID == 1496 || MailClientRowid == 2 || MailClientRowid == 193) && (MailTemplateName == "CandidateLoginDetailsTemplate" || MailTemplateNameSpoc == "CandidateLoginDetailsTemplate"))
                                                    {
                                                        flag = mail.CandidateLoginSendMail(DisplayName, From, ToMail, Subject, body, attachment, alternateView);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, alternateViewSpocAndClient);
                                                    }
                                                    else if (ClientRowID == 1481)
                                                    {
                                                        DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                                        var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                                        if (recuiteremail != "")
                                                        {
                                                            Ccmail += "," + recuiteremail;
                                                        }
                                                        flag = mail.SendMailToCandidateForCtrls(DisplayName, From, ToMail, Subject, body, attachment, attachment2);
                                                        flagspocandclient = mail.SendMailToCandidateForCtrlsForSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, attachment2);
                                                    }
                                                    else
                                                    {
                                                        //=====================================
                                                        if (ClientRowID == 636)
                                                        {
                                                            DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                                            var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                                            if (recuiteremail != "")
                                                            {
                                                                Ccmail += "," + recuiteremail;
                                                            }
                                                        }
                                                        //===========================================
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                        //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClientWithoutAlternateView(DisplayName, From, Ccmail, "", Subject, bodySpocAndClient, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, null);
                                                    }
                                                }

                                                if (flag)
                                                {
                                                    if (ClientRowID == 249 || ClientRowID == 277) //sutherlandglobal and Sutherland Healthcare Solutions Pvt. Ltd.
                                                    {
                                                        try
                                                        {
                                                            string Message = "";
                                                            Message = "Dear " + personalData.Name + Environment.NewLine + Environment.NewLine;
                                                            Message += "Please complete Sutherland BGV on email sent to your ID or below link." + Environment.NewLine;
                                                            Message += "ID:" + loginDetail.UserID + Environment.NewLine;
                                                            Message += "Pwd:" + clsCommonMethods.PasswordDecrypt(loginDetail.UPass) + Environment.NewLine;
                                                            Message += "Link:" + ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString() + Environment.NewLine;
                                                            Message += "Regards" + Environment.NewLine;
                                                            Message += "Securitas";
                                                            loginDetail.SMSText = Message;
                                                            loginDetail.PersonalId = pID;
                                                            SendSMS(loginDetail);
                                                        }
                                                        catch
                                                        {

                                                        }
                                                    }
                                                    if (ClientRowID == 635)
                                                    {
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    }
                                                    else if (ClientRowID == 636)
                                                    {
                                                        cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                    }
                                                    else if (ClientRowID == 957)
                                                    {
                                                        cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                    }
                                                    else if (ClientRowID == 1209)
                                                    {
                                                        DataTable dt = cc.GetCandidateClient(ToMail, pID);
                                                        if (dt.Rows.Count > 0)
                                                        {
                                                            int clientrowidnew = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                                                            cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, clientrowidnew, 1);
                                                            cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                        }
                                                    }
                                                    else if (ClientRowID == 1005 || ClientRowID == 2 || ClientRowID == 889)
                                                    {
                                                        SaveWPassMail = SaveWPassMail.Replace("Click the below Url, login", "");//.Replace("Candidate Login", "").Replace("Click Reference ID:{ClientRefId}", "").Replace("Login Id: {UName}", "").Replace("Password: {UPasswd}", "");
                                                        SaveWPassMail = SaveWPassMail.Replace("into the website", "");
                                                        SaveWPassMail = SaveWPassMail.Replace("Candidate Login", "");
                                                        SaveWPassMail = SaveWPassMail.Replace("Click Reference ID:{ClientRefId}", "");

                                                        SaveWPassMail = SaveWPassMail.Replace("Password: {XXXXX}", "");

                                                        SaveWPassMail = SaveWPassMail.Replace("Login Id: {XXXXX}", "");

                                                        SaveWPassMail = SaveWPassMail.Replace("{document}", docString.ToString());
                                                        SaveWPassMail = SaveWPassMail.Replace("{CandidateName}", loginDetail.CandidateName);
                                                        SaveWPassMail = SaveWPassMail.Replace("#CompanyName", ClientNameMail);
                                                        SaveWPassMail = SaveWPassMail.Replace("{ClientRefId}", personalData.ClientRefID);
                                                        SaveWPassMail = SaveWPassMail.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                                        SaveWPassMail = SaveWPassMail.Replace("{UName}", "XXXXX");
                                                        SaveWPassMail = SaveWPassMail.Replace("{UPasswd}", "XXXXX");
                                                        SaveWPassMail = SaveWPassMail.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());

                                                        SaveWPassMail = SaveWPassMail.Replace("{MailSignature}", MailSignature);

                                                        cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                    }
                                                    else
                                                    {
                                                        string Message = $"Dear {loginDetail.CandidateName} We kindly request you to upload the required document in the application, using the login credentials sent on your email. Feel free to connect in case of any assistance required on mentioned contact details on email. Thank you Walsons Services Pvt. Ltd.";
                                                        loginDetail.PersonalId = pID;
                                                        loginDetail.SMSText = Message;
                                                        SendSMS(loginDetail);
                                                        cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);

                                                    }

                                                    if (ClientRowID == 635)
                                                    {
                                                        //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                                        RemindermailTataElxsi(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                                    }

                                                    if (ClientRowID == 957)
                                                    {
                                                        RemindermailAstraZenca(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);

                                                    }

                                                    if (ClientRowID == 679 || ClientRowID == 725)
                                                    {
                                                        //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                                        SaveReminderMail(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                                    }
                                                    UpdatePQCandidateLoginForSendMailViewModel loginModel = new UpdatePQCandidateLoginForSendMailViewModel();
                                                    loginModel.CandidateUserRowID = loginDetail.CandidateUserRowID;
                                                    loginModel.SentMailStatus = 1;
                                                    loginModel.SentMailDate = DateTime.Now;
                                                    repoCandidate.UpdateCandidateLoginSendMailStatusById(loginModel);
                                                    countSent++;
                                                }
                                                else
                                                {
                                                    countFailed++;
                                                }
                                            }

                                        }

                                    }
                                    TempData["ErrorMsg"] = "Out of " + totRec.ToString() + " Mail sent : " + countSent.ToString() + " failed : " + countFailed.ToString();

                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Candidate login details not found!";
                                }
                            }
                        }
                        #endregion

                        mm = repoPQPersonal.GetTempCandidateListByClientBulkUpload(page, pageSize, sort, sortDir, Search, ClientRowID);

                        if (mm == null)
                        {
                            ViewBag.ErrorMsg = "No item found!";
                        }
                    }
                    //PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();               
                    SearchModel.Search = Search;
                    mm.ItemSearch = SearchModel;
                }
                catch (Exception Ex)
                {
                    SearchModel.Search = "";
                    mm.ItemSearch = SearchModel;
                    //ViewBag.ErrorMsg = Ex.Message;
                    ViewBag.ErrorMsg = "Invalid parameter value";
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                }
                //return View(model);
                //return RedirectToAction("ManageClientCandidate", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }


            //////////////////////////////////////////////////////////////////////////


            if (ViewBag.ErrorMsg == "")
            {
                if (clientRowId == 1324)
                {
                    TempData["SuccessMsg"] = "Case Registered Successfully";
                }
                else
                {
                    TempData["SuccessMsg"] = "Case Registered Successfully" + "," + "Send mail from “Manage Candidate” section";
                }
                return RedirectToAction("AddSingleCandidateCaseUpload");
            }
            else
            {
                int ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                //int ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                ClientCustomize bl = new ClientCustomize();
                ViewBag.LastFiveCases = bl.LastFiveCasesInitiatedToCandidate(ClientRowID, ClientUserRowID);
                string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                string comm = ErrorMsg.Substring(0, 1);
                if (comm.Contains(","))
                {
                    ViewBag.ErrorMsg = ErrorMsg.Substring(1, ErrorMsg.Length - 1);
                }
                return View(marksModel);
            }

        }

        [HttpPost]
        [SessionExpireFilterAttributeClient]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        //[ValidateSessionFilter]
        public async Task<ActionResult> AddSingleCandidateCaseUploadUAN(FormCollection formCollection)
        {

            TempData["Exists"] = 0;
            ViewBag.ErrorMsg = "";
            dynamic expando = new ExpandoObject();

            string strBtn = formCollection["Savebtn"].ToString();
            string clientRowid = formCollection["ClientRowID"].ToString();
            int ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
            BindBusinessUnitList(ClientUserRowID, 0);
            string CandidateCheckOrPackage = formCollection["CandidateCheckOrPackage"].ToString();
            int clientRowId = Convert.ToInt32(clientRowid);

            if (CandidateCheckOrPackage == "")
            {
                ViewBag.ErrorMsg = "Pricing Type is mandatory field please select check or package from below list";
            }
            string Securid = formCollection["Securid"].ToString();
            if (Securid == "")
            {
                ViewBag.ErrorMsg = "Company Ref No is mandatory field please Enter Company Ref No.";
            }
            ClientCustomize cm = new ClientCustomize();
            string isvalidate = cm.ValidateCompanyRefNoForAcuityUANCheck(Securid , Convert.ToInt32(clientRowid));
            if(isvalidate == "false")
            {
                ViewBag.ErrorMsg = "UAN check Not Completed or Case is not found.";

            }

            var marksModel = expando as IDictionary<string, object>;
            try
            {
                CandidateDEModel cd = new CandidateDEModel();
                AddTempPQPersonalForBulkUpViewModel model = new AddTempPQPersonalForBulkUpViewModel();

                model.ClientRowID = Convert.ToInt16(clientRowid);
                model.CandidateCheckOrPackage = CandidateCheckOrPackage;
                if (clientRowId == 193)
                {
                    if (formCollection["343"].ToString() != null)
                    {
                        model.AlternateEmailID = formCollection["343"].ToString();
                    }
                }
                if (clientRowId == 1209)
                {
                    model.ClientUserRowID = 0;
                    cd.clientRowId = Convert.ToInt16(formCollection["ClientRowID"].ToString());
                }
                else
                {
                    cd.clientRowId = clientRowId;
                }

                cd.CreatedBy = Convert.ToInt32(Session["CandidateUserRowID"]);

                ClientCustomize bl = new ClientCustomize();

                DataTable dt = new DataTable();

                dt = bl.Antecedents_select_form(cd.clientRowId, 0, "client candidate", "Case");
                dt.DefaultView.RowFilter = "AntecedentsRowID=19";
                var tblMand = dt.DefaultView.ToTable();
                dt.DefaultView.RowFilter = "";
                string strMobile = "";
                if (tblMand.Rows.Count > 0)
                {
                    strMobile = Convert.ToString(tblMand.Rows[0]["QLabel"]).Split('_')[3];
                }
                ViewBag.Mandatory = strMobile;
                string mobileNo = "";//email
                string emailid = ""; //mobile no

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
                                if (key == "19")
                                    mobileNo = cd.DataEntryValue;
                                if (key == "8")
                                    emailid = cd.DataEntryValue;
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {

                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + cd.DisplayName + " is a mandatory field.";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {

                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + cd.DisplayName + " is a mandatory field.";
                                }
                            }
                            if (d[4].ToString() == "fileUpload")
                            {

                                foreach (string filenm in Request.Files)
                                {
                                    if (filenm == d[0].ToString())
                                    {
                                        HttpPostedFileBase postedFile = Request.Files[filenm];
                                        if (postedFile.ContentLength > 0)
                                        {
                                            string errorMessage = await GatZipFileMessage(postedFile, model.ClientRowID.ToString(), filenm);
                                            if (errorMessage != "")
                                            {
                                                ViewBag.ErrorMsg += errorMessage;
                                            }
                                        }
                                        else
                                        {
                                            if (FString[3].Trim().ToLower() == "mandatory")
                                            {
                                                ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + FString[1] + " is a mandatory field.";
                                            }
                                        }
                                    }

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

                    if (d[4].ToString() == "textbox")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1].Trim();
                                cd.DataEntryValue = formCollection[key].ToString();

                                if (d[0].ToString() == "121") //candidate full Name
                                {
                                    model.CandidateName = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "20") // candidate first name
                                {
                                    model.Name = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "18") // candidate middle name
                                {
                                    model.MiddleName = cd.DataEntryValue;

                                }

                                if (d[0].ToString() == "14") // candidate last name
                                {
                                    model.LastName = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "8") // candidate Email
                                {
                                    model.EmailID = cd.DataEntryValue;

                                }

                                if (d[0].ToString() == "19") // mobile no
                                {
                                    model.MobileNo = cd.DataEntryValue;

                                }

                                if (d[0].ToString() == "4") // client ref id
                                {
                                    model.ClientRefID = cd.DataEntryValue;

                                }

                                if (d[0].ToString() == "16") // location
                                {
                                    //model.pla = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "9") // location
                                {
                                    model.EmployeID = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "7") // location
                                {
                                    model.Designation = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "208") // Entity
                                {
                                    model.Entity = cd.DataEntryValue;

                                }
                                if (d[0].ToString() == "6") // DOJ
                                {
                                    if (!string.IsNullOrEmpty(cd.DataEntryValue))
                                    {
                                        model.DateofJoining = Convert.ToDateTime(cd.DataEntryValue);
                                    }

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

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                if (d[0].ToString() == "24") //Title
                                {
                                    model.CandTitle = cd.DataEntryValue;
                                    if (model.CandTitle == "Mr." || model.CandTitle == "Mohd.")
                                    {
                                        model.Gender = "Male";
                                    }
                                    else
                                    {
                                        model.Gender = "Female";
                                    }
                                }
                                if (d[0].ToString() == "208")
                                {
                                    model.BusinessName = cd.DataEntryValue;

                                }

                                if (d[0].ToString() == "23")
                                {
                                    model.PlaceOfJoining = cd.DataEntryValue;
                                }

                                if (d[0].ToString() == "644")
                                {
                                    model.TCSClientType = cd.DataEntryValue;
                                }
                                if (cd.clientRowId == 685)
                                {
                                    if (d[0].ToString() == "208") // Entity
                                    {
                                        model.Entity = cd.DataEntryValue;

                                    }
                                }



                            }
                        }
                    }

                }
                // ── Client 684: Entity Name not in dt (Client Candidate module), read directly ──
                //if (clientRowId == 684)
                //{
                //    if (formCollection["208"] != null)
                //        model.Entity = formCollection["208"].ToString();

                //    if (formCollection["OfferType"] != null)
                //        model.OfferType = formCollection["OfferType"].ToString();
                //}
                // ── end 684 ──

                model.ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                model.ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                if (model.ClientRowID == 1209)
                {
                    model.ClientUserRowID = 0;
                    model.ClientRowID = Convert.ToInt16(formCollection["ClientRowID"].ToString());
                }
                // client 684 hard guard entity name must never save null
                if (model.ClientRowID == 684 && string.IsNullOrWhiteSpace(model.Entity))
                {
                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + " Entity Name is a mandatory field";
                }

                model.PersonalRowID = 0;
                if (strBtn == "Submit")
                {

                    int existcandidate = 0;
                    int[] ClientRowIds = { 1062, 1091, 1086, 1067, 1067, 1072, 1077, 1266, 1268, 1068, 1127, 1193, 559, 1074, 1155 };
                    if (Array.Find(ClientRowIds, element => element == model.ClientRowID) > 0)
                    {
                        var existcandidateDT = bl.GetExistsTempCandidateDetails(model.ClientRowID, model.CandidateName, model.EmailID, model.MobileNo, Convert.ToDateTime(model.DOB), model.AadhaarCardNo);
                        if (existcandidateDT.Rows.Count > 0)
                        {
                            existcandidate = 1;
                            ViewBag.ErrorMsg = model.CandidateName + " Candidate(s) already exist!";
                        }
                    }
                    if (existcandidate == 0)
                    {

                        if (ViewBag.ErrorMsg == "")
                        {

                            DataTable dts = bl.InsertTempPQPersonalsUAN(model , Securid);
                            if (Convert.ToString(dts.Rows[0]["CaseType"]) == "New")
                            {
                                model.PersonalRowID = Convert.ToInt32(dts.Rows[0][0].ToString());
                                PepPersonal = Convert.ToInt32(dts.Rows[0][0]);
                                CreateUserIdForCandidate(model);
                                SaveTempPackageOrCheckWithCandidate(model);
                                if (model.ClientRowID == 679)
                                {
                                    try
                                    {
                                        SendmailtoCandidate(model.PersonalRowID.ToString(), model.ClientRowID, model.ClientUserRowID);
                                    }
                                    catch (Exception)
                                    {
                                        bl.AddSentInviteEmailCases(Convert.ToInt32(model.ClientRowID), Convert.ToInt32(model.ClientUserRowID), Convert.ToInt32(model.PersonalRowID));


                                    }
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "Record already exists with,";
                                if (Convert.ToString(dts.Rows[0]["CompanyRefNo"]) != "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + Convert.ToString(dts.Rows[0]["CompanyRefNo"]);
                                }
                                if (Convert.ToString(dts.Rows[0]["Emailid"]) != "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + "Email ID  :" + Convert.ToString(dts.Rows[0]["Emailid"]);
                                }
                                if (Convert.ToString(dts.Rows[0]["MobileNo"]) != "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + "Mobile No : " + Convert.ToString(dts.Rows[0]["MobileNo"]);
                                }

                                string strMsg = ViewBag.ErrorMsg;
                                if (strMsg != "")
                                {
                                    strMsg = strMsg.Substring(0, strMsg.LastIndexOf(','));
                                    ViewBag.ErrorMsg = strMsg;
                                }

                                TempData["Exists"] = 1;
                            }
                        }

                    }
                }
                else if (strBtn == "Force Submit")
                {
                    DataTable dts = bl.InsertTempPQPersonalsUAN(model, Securid);
                    model.PersonalRowID = Convert.ToInt32(dts.Rows[0][0].ToString());
                    CreateUserIdForCandidate(model);
                    SaveTempPackageOrCheckWithCandidate(model);
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
                                    string filename = filenm + "_" + postedFile.FileName;
                                    string base64 = string.Empty;
                                    postedFile.InputStream.Position = 0;
                                    using (var memoryStream = new MemoryStream())
                                    {
                                        postedFile.InputStream.CopyTo(memoryStream); // Convert the byte array to Base64 
                                        base64 = Convert.ToBase64String(memoryStream.ToArray());
                                    }
                                    _documentService.UploadDynamicCandidateFiles(filename, base64, "D:/wwwroot2/Uploads/");
                                    if (clientRowId == 753)
                                    {
                                        _documentService.UploadDynamicCandidateFiles(filename, base64, "D:/wwwroot2/Documents/CandidateDocuments/ProvidedDocuments/");
                                    }
                                    // postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.PersonalRowID = model.PersonalRowID;
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    if (ViewBag.ErrorMsg == "")
                                    {
                                        bl.SaveDataEntry_Candidate(cd);
                                    }
                                    bl.proc_InsertUpdateTempPQVerifiedUploadCaseDocs(cd, "~/Uploads/");
                                }
                                else
                                {
                                    marksModel.Add(d[1].ToString(), d[0].ToString());
                                    if (FString[3].Trim().ToLower() == "mandatory")
                                    {
                                        //ViewBag.ErrorMsg = ViewBag.ErrorMsg + "<br>" +   FString[1] + " is a mandatory field";
                                        ViewBag.ErrorMsg = ViewBag.ErrorMsg + "," + FString[1] + " is a mandatory field.";
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
                                cd.PersonalRowID = model.PersonalRowID;
                                if (ViewBag.ErrorMsg == "")
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

                                if (cd.AntecedentRowID == 380)
                                {
                                    if (clientRowid == "1430")
                                    {
                                        cd.DataEntryValue = bl.GetVarrocBusinessUnit(Convert.ToInt32(formCollection[key]));
                                    }
                                    else
                                    {
                                        cd.DataEntryValue = formCollection[key].ToString();
                                    }
                                }
                                else
                                {
                                    cd.DataEntryValue = formCollection[key].ToString();
                                }
                                cd.PersonalRowID = model.PersonalRowID;
                                if (ViewBag.ErrorMsg == "")
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
                        cd.PersonalRowID = model.PersonalRowID;
                        if (ViewBag.ErrorMsg == "")
                            bl.SaveDataEntry_Candidate(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
                if (model.ClientRowID == 679)
                {
                    SendAutoEmailTocandidateAfterInitiation(model.PersonalRowID.ToString(), "");
                }
            }
            catch (Exception Ex)
            {
                string str = Ex.Message;
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg =  " Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientListByLogInName(clientRowId);
            BindCandidateTypeList();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();


            /////////////////////////////////////////////////////////////////////////


            if (clientRowId == 1324 || clientRowId == 1470)
            {
                ClientCustomize cc = new ClientCustomize();
                TempPQPersonalInfoListPagedModel mm = new TempPQPersonalInfoListPagedModel();
                PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
                int page = 1;
                string sort = "";
                string sortDir = "asc";
                string Search = Convert.ToString(formCollection["Search"]);

                string PersonalIds = PepPersonal.ToString();

                try
                {
                    short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    //int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                    int pageSize = clsGlobalCommon.PageSize;


                    ViewBag.ErrorMsg = "";
                    if (ViewBag.ErrorMsg == "")
                    {
                        #region Send Mail To Candidate
                        if (!string.IsNullOrEmpty(PersonalIds))
                        {
                            string spocEmail = "";
                            string cSpocEmail = "";
                            string mobileno = "";
                            string[] PersonalID = PersonalIds.Split(',');
                            string TrmFldPathDoc = "";
                            string SendUserManulToCtrls = "";
                            string CtrlsfileName = "";
                            string fileName = "";
                            if (ClientRowID == 576)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "LOA.pdf";
                                fileName = Path.GetFileNameWithoutExtension("LOA.pdf");
                            }
                            else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Chargebee_LOA.docx";
                                fileName = Path.GetFileNameWithoutExtension("Chargebee_LOA.docx");
                            }
                            else if (ClientRowID == 1425)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "GlobalAuthorization-Australia.docx";
                                fileName = Path.GetFileNameWithoutExtension("GlobalAuthorization-Australia.docx");
                            }
                            else if (ClientRowID == 1428)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Genpact Privacy Notice and Consent Form template_Pre-Employment Screening_EU (Securitas).docx";
                                fileName = Path.GetFileNameWithoutExtension("Genpact Privacy Notice and Consent Form template_Pre-Employment Screening_EU (Securitas).docx");
                            }
                            else if (ClientRowID == 1497)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Capco Privacy Notice and Consent Form template_Pre-Employment Screening(Securitas).docx";
                                fileName = Path.GetFileNameWithoutExtension("Capco Privacy Notice and Consent Form template_Pre-Employment Screening(Securitas).docx");
                            }
                            else if (ClientRowID == 1429)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Global_Authorization-Nagarro.docx";
                                fileName = Path.GetFileNameWithoutExtension("Global_Authorization-Nagarro.docx");
                            }
                            else if (ClientRowID == 1449)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "GlobalAuthorization-Uniqus.docx";
                                fileName = Path.GetFileNameWithoutExtension("GlobalAuthorization-Uniqus.docx");
                            }
                            else if (ClientRowID == 1514)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "SILVERLINK-I.docx";
                                fileName = Path.GetFileNameWithoutExtension("SILVERLINK-I.docx");
                            }
                            else if (ClientRowID == 1481)
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CtrlS_Datacenter-LOA.pdf";
                                fileName = Path.GetFileNameWithoutExtension("CtrlS_Datacenter-LOA.pdf");
                                SendUserManulToCtrls = ConfigurationManager.AppSettings["CandidateManual"].ToString() + "CandidateManual.pdf";
                                CtrlsfileName = Path.GetFileNameWithoutExtension("CandidateManual.pdf");
                            }
                            else
                            {
                                TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CandidateAgreement.pdf";
                                fileName = Path.GetFileNameWithoutExtension("CandidateAgreement.pdf");
                            }

                            //Attachment attachment;
                            Attachment attachment;
                            Attachment attachment2;
                            if (ClientRowID == 1481)
                            {
                                attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                                attachment2 = new Attachment(Server.MapPath(SendUserManulToCtrls));
                            }
                            else
                            {
                                attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                                attachment2 = null;
                            }

                            string MailSignature = string.Empty;
                            string From = string.Empty;
                            string DisplayName = string.Empty;
                            string Cc = "";

                            clsSendMail mail;

                            var cSmtp = repoPQClient.GetPQClientSendMailFromById(ClientRowID);
                            DataTable dtEmail = new DataTable();

                            string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);
                            if (ClientAccessType == "3")
                            {
                                dtEmail = cc.userwise_ReportingEmailAccess(ClientUserRowID);
                                if (dtEmail.Rows.Count > 0)
                                {
                                    spocEmail = dtEmail.Rows[0]["ClientUserEmailID"].ToString();
                                    cSpocEmail = dtEmail.Rows[0]["ReportingTo_EmailID"].ToString();

                                }
                            }
                            else
                            {
                                dtEmail = cc.GetEmailStructureForAlert(ClientRowID, ClientUserRowID, "Case Registration");
                                if (dtEmail.Rows.Count > 0)
                                {
                                    spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                                    cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                                    mobileno = dtEmail.Rows[0]["mobileno"].ToString();

                                }
                            }
                            Cc = cSpocEmail;
                            if (cSmtp != null && cSmtp.MailSendBy > 0)
                            {
                                mail = new clsSendMail(cSmtp.SMTPServer, cSmtp.SMTPPort, cSmtp.SMTPUserName, clsCommonMethods.PasswordDecrypt(cSmtp.SMTPPassword), Convert.ToBoolean(cSmtp.EnableSsl));

                                From = cSmtp.SMTPUserName;
                                DisplayName = cSmtp.ClientName.Trim();
                                Cc = string.Concat(Cc, ",", spocEmail);

                                MailSignature = string.Concat("<b>", cSmtp.ClientName.Trim(), "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", cSmtp.SMTPUserName, "'>", cSmtp.SMTPUserName, "</a>");
                            }
                            else
                            {
                                mail = new clsSendMail();
                                var signature = mail.GetCompanyMailSignature();

                                if (mobileno != "")
                                    mobileno = "<b>Mobile: " + mobileno;
                                string ShowSpocId = spocEmail.ToLower();

                                ShowSpocId = ShowSpocId.Replace(",", ";");
                                string[] ShowSpocId1 = spocEmail.Split(';');
                                if (signature != null)
                                {

                                    // For TATA Elxsi
                                    if (ClientRowID == 635)
                                    {
                                        MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                        //"<b>Mobile: ", signature.MobileNo,
                                        "</b><br/><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", "bgv.helpdesk@securitas-india.com", "'>", "bgv.helpdesk@securitas-india.com", "</a>");
                                    }
                                    else if (ClientRowID == 249)
                                    {
                                        MailSignature = string.Concat("For Securitas India. <br /><br />");
                                        MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                        //"<b>Mobile: ", signature.MobileNo,
                                        "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                        "</b><br /><b>Contact No:</b><br />  <b>Satya :</b>+91 8377022632 <br />  <b>Pradeep Devrani :</b> +91 8373907723<br /><b>Himanshu Pathak :</b> +91 7291974515 ");
                                        //Please mention the contact number of Satya - +91 8377022632 / Pradeep Devrani - 91 - 8373907723 / Himanshu Pathak - +91 7291974515
                                    }
                                    else if (ClientRowID == 277)
                                    {
                                        MailSignature = string.Concat("For Securitas India. <br /><br />");
                                        MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                        //"<b>Mobile: ", signature.MobileNo,
                                        "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                        "</b><br /><b>Contact No </b>:+91-8373907723");
                                    }
                                    else
                                    {
                                        MailSignature = string.Empty;
                                        MailSignature = string.Concat(ShowSpocId1[0]);
                                    }

                                }



                                Cc = string.Concat(Cc, ",", ShowSpocId);

                                From = signature.GeneralEmail;
                                DisplayName = signature.CompanyName.Trim();
                            }
                            //int sum = 0;
                            foreach (var item in PersonalID)
                            {
                                int pID = Convert.ToInt32(item);
                                var personalData = repoPQPersonal.GetTempPQPersonalDetailsForSendMailById(pID);
                                var loginDetail = repoCandidate.GetCandidateLoginDetailsForMailSendById(pID);
                                DataTable d = new DataTable();
                                StringBuilder docString = new StringBuilder();

                                if (ClientRowID == 1425 || ClientRowID == 1428 || ClientRowID == 1222)
                                {
                                    d = cc.GetDocumentForVerificationList(pID, ClientRowID);
                                    foreach (DataRow dr in d.Rows)
                                    {
                                        docString.Append("<tr><td style='padding:3px;'>" + dr["SubCheckName"].ToString() + " </td><td>" + dr["DocumentForVerification"].ToString() + " </td></tr>");
                                    }
                                }
                                else if (ClientRowID == 193)
                                {
                                    d = cc.GetDocumentForVerificationList(pID, ClientRowID);
                                    foreach (DataRow dr in d.Rows)
                                    {
                                        docString.Append("<tr><td style='padding:3px;'>" + dr["SubCheckName"].ToString() + " </td><td>" + dr["DocumentForVerification"].ToString() + " </td></tr>");
                                    }
                                }
                                else
                                {
                                    d = cc.CandidateEmailDocFormat(pID); // it should be subcheck family based on special check, 
                                    foreach (DataRow dr in d.Rows)
                                    {
                                        if (ClientRowID == 1357)
                                        {
                                            //---- Employment Check Start from here ----//
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Past two Employments </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");
                                        }
                                        else if (ClientRowID == 1425)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Any available document. Note: Two employment details prior to Chargebee.</ td></tr>");
                                        }
                                        else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Experience letter / Service Certificate</td></tr>");
                                        }
                                        else if (ClientRowID == 1428)
                                        {

                                        }
                                        else if (ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Experience letter, Service Certificate, Other</td></tr>");
                                        }
                                        else if (ClientRowID == 1481)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Current Company Employment<br> • Experience Letter / Relieving Letter<br> • Salary Slip for last 3 months <br> Previous Employment(only latest 2 employers)<br> • Experience Letter / Relieving Letter </ td></tr>");
                                        }
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                                docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");
                                        }
                                        //---- Employment Check End from here ----//

                                        //---- Education Check Start from here ----//
                                        if (ClientRowID == 1355 || ClientRowID == 1348)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Marksheet/Provisional Degree Certificate </td></tr>");
                                        }
                                        else if (ClientRowID == 1318)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Sanad Certification </td><td>Required Bar Council Certificate</td></tr>");
                                        }
                                        else if (ClientRowID == 1425)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Transcript/Degree Certificate</td></tr>");
                                        }
                                        else if (ClientRowID == 1428)
                                        {

                                        }
                                        else if (ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Transcript/ Degree Certificate/ Convocation Certificate</td></tr>");
                                        }
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                                docString.Append("<tr><td style='padding:3px;'>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");
                                        }
                                        //---- Education Check End from here ----//

                                        //---- Address Check Start from here ----//
                                        if (ClientRowID == 1425)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>Driver's License/Utility Bill</td></tr>");
                                        }
                                        if (ClientRowID == 1395)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>SSN Number</td></tr>");
                                        }
                                        else if (ClientRowID == 1428)
                                        {

                                        }
                                        else if (ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>National ID Proof</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>• Address Proof </td></tr>");
                                        }
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                                docString.Append("<tr><td style='padding:3px;'>Address </td><td>Address Proof for Current & Permanent Address </td></tr>");
                                        }
                                        //---- Address Check End from here ----//

                                        //---- National Check Start from here ----//
                                        if (ClientRowID == 1355 || ClientRowID == 1348)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td>ID Verification</td><td>Passport preferred</td></tr>");
                                        }
                                        else if (ClientRowID == 1425 || ClientRowID == 1428 || ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td>National ID Check </td><td>National ID Proof</td></tr>");
                                        }
                                        else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td>National ID Check </td><td>Passport / National ID</td></tr>");
                                        }
                                        else if (ClientRowID == 559)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Pan Card</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Photo ID Proof </td></tr>");
                                        }
                                        else if (ClientRowID == 648)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>ID Proof – Pan Card and Aadhar Card</td></tr>");
                                        }
                                        else
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "26")
                                                docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>ID Proof </td></tr>");
                                        }
                                        //---- National Check End from here ----//

                                        //---- Special Check Start from here ----//
                                        if (ClientRowID == 487)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "40")
                                                docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>Required Aadhar Card</td></tr>");
                                        }
                                        else if (ClientRowID == 1470)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "40")
                                                docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>NA</td></tr>");
                                        }
                                        else if (ClientRowID == 193)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "40")
                                                docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>NA</td></tr>");
                                        }

                                        //---- Special Check End from here ----//

                                        //---- Criminal Check Start from here ----//
                                        if (ClientRowID == 487 || ClientRowID == 1428 || ClientRowID == 1449)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>National ID Proof</td></tr>");
                                        }
                                        else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>Passport / National ID</td></tr>");
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>• Address Proof </td></tr>");
                                        }
                                        else if (ClientRowID == 648)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>Require – Address proof, ID Proof and Passport Size Photo</td></tr>");
                                        }
                                        else if (ClientRowID == 1470)
                                        {
                                            if (dr["CheckFamilyRowID"].ToString() == "5")
                                                docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>National ID Proof</td></tr>");
                                        }
                                        //---- Criminal Check End from here ----//
                                    }
                                }

                                if (loginDetail != null)
                                {
                                    string body = string.Empty;
                                    string bodySpocAndClient = string.Empty;
                                    string SaveWPassMail = string.Empty;
                                    DataTable tblExperiece = cc.CheckCandidateExperienced(personalData.ClientRowID, personalData.PersonalRowID);

                                    string MailSubject = "";
                                    string SpocAndClientMailSubject = "";
                                    string MailTemplateName = "";
                                    string MailTemplateNameSpoc = "";
                                    int MailClientRowid = 0;
                                    var templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplate");
                                    if (templateDB.Rows.Count > 0)
                                    {
                                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                        MailTemplateName = Convert.ToString(templateDB.Rows[0]["TemplateName"]);
                                        MailClientRowid = Convert.ToInt32(templateDB.Rows[0]["ClientRowId"]);
                                    }
                                    var templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplateForSpocAndClient");
                                    if (templateDBSpocAndClient.Rows.Count > 0)
                                    {
                                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                        MailTemplateNameSpoc = Convert.ToString(templateDBSpocAndClient.Rows[0]["TemplateName"]);
                                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                    }

                                    if (ClientRowID == 679)
                                    {
                                        DataTable tblfractal = cc.CheckCandidateExperiencedByCandidateType(personalData.ClientRowID, personalData.PersonalRowID);
                                        if (tblfractal.Rows.Count > 0)
                                        {
                                            if (tblfractal.Rows[0][0].ToString() == "Existing Employee")
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployee");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployeeForSpocAndClient");
                                            }
                                            else
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                            }
                                        }
                                        if (templateDB.Rows.Count > 0)
                                        {
                                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                            if (ClientRowID == 679 || ClientRowID == 2)
                                            {
                                                SaveWPassMail = Convert.ToString(templateDB.Rows[0]["WLDMailTemplate"]);
                                            }
                                        }
                                        if (templateDBSpocAndClient.Rows.Count > 0)
                                        {
                                            bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                            SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                        }
                                    }

                                    else if (ClientRowID == 576)
                                    {
                                        //---------Experienced-----------
                                        if (tblExperiece.Rows.Count > 0)
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LateralMailNagarro");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LateralMailNagarroForSpocAndClient");
                                        }
                                        else
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "NagarroFresherMail");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "NagarroFresherMailForSpocAndClient");
                                        }
                                        if (templateDB.Rows.Count > 0)
                                        {
                                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                        }
                                        if (templateDBSpocAndClient.Rows.Count > 0)
                                        {
                                            bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                            SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                        }
                                    }
                                    else if (ClientRowID == 635) //Tata Elxsi
                                    {

                                        DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                        if (dtpackage.Rows.Count > 0)
                                        {
                                            if (dtpackage.Rows[0][0].ToString() == "TEL_Experienced (ID_CRCP_CRCC_PADD_CADD_GDB_2EMP")
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 671, 1, "LoginDetailsSendToCandidateFotTataElxsiExp");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiExpSpocAndClient");
                                            }
                                            else
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresher");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresherSpocAndClient");
                                            }
                                        }
                                        if (templateDB.Rows.Count > 0)
                                        {
                                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                        }
                                        if (templateDBSpocAndClient.Rows.Count > 0)
                                        {
                                            bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                            SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                        }
                                    }


                                    else if (ClientRowID == 647)
                                    {

                                        DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                        if (dtpackage.Rows.Count > 0)
                                        {
                                            if (dtpackage.Rows[0][0].ToString() == "NNSCIPL_Experienced (EDU_2EMP_PADD_GDB_CRCP_ID)")
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 737, 1, "LoginDetailsSendToCandidateForNovoNordiskExp");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskExpForSpocAndClient");
                                            }
                                            else
                                            {
                                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresher");
                                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresherForSpocAndClient");
                                            }
                                        }
                                        if (templateDB.Rows.Count > 0)
                                        {
                                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                        }
                                        if (templateDBSpocAndClient.Rows.Count > 0)
                                        {
                                            bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                            SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                                        }
                                    }

                                    if (ClientRowID == 103)
                                    {
                                        body = body.Replace("{Remarks}", "Please login and fill the details within 3 days");
                                        bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Please login and fill the details within 3 days");
                                    }
                                    else if (ClientRowID == 1530)
                                    {
                                        body = body.Replace("{Remarks}", "Kindly fill the form within 2 working days");
                                        bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Kindly fill the form within 2 working days");
                                    }
                                    else
                                    {
                                        body = body.Replace("{Remarks}", "Please login and fill the details within 2 days");
                                        bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Please login and fill the details within 2 days");
                                    }

                                    string txtFolderPath = ConfigurationManager.AppSettings["mailimages"].ToString();
                                    LinkedResource Triangleres = new LinkedResource(Server.MapPath(txtFolderPath) + "Triangle.png", MediaTypeNames.Image.Jpeg);
                                    Triangleres.ContentId = Guid.NewGuid().ToString();
                                    string triangle = @"<img src='cid:" + Triangleres.ContentId + @"' width='25%'/>";

                                    LinkedResource Securitassec = new LinkedResource(Server.MapPath(txtFolderPath) + "Securitas-logo.png", MediaTypeNames.Image.Jpeg);
                                    Securitassec.ContentId = Guid.NewGuid().ToString();
                                    string securitas = @"<img src='cid:" + Securitassec.ContentId + @"' width='25%'/>";

                                    LinkedResource Walsonssec = new LinkedResource(Server.MapPath(txtFolderPath) + "Walsons-Temp-img.JPG", MediaTypeNames.Image.Jpeg);
                                    Walsonssec.ContentId = Guid.NewGuid().ToString();
                                    string walsons = @"<img src='cid:" + Walsonssec.ContentId + @"' width='25%'/>";

                                    LinkedResource BGVdottedImg = new LinkedResource(Server.MapPath(txtFolderPath) + "BGV-dotted-design.png", MediaTypeNames.Image.Jpeg);
                                    BGVdottedImg.ContentId = Guid.NewGuid().ToString();
                                    string BGVdotted = @"<img src='cid:" + BGVdottedImg.ContentId + @"' width='100%'/>";

                                    LinkedResource RedbulllogoImg = new LinkedResource(Server.MapPath(txtFolderPath) + "Redbullogo.png", MediaTypeNames.Image.Jpeg);
                                    RedbulllogoImg.ContentId = Guid.NewGuid().ToString();
                                    string redbulllogo = @"<img src='cid:" + RedbulllogoImg.ContentId + @"' width='30%'/>";

                                    LinkedResource JadeGlobalLogoIMG = new LinkedResource(Server.MapPath(txtFolderPath) + "JadeGlobalLogo.png", MediaTypeNames.Image.Jpeg);
                                    JadeGlobalLogoIMG.ContentId = Guid.NewGuid().ToString();
                                    string JadeGlobalLogo = @"<img src='cid:" + JadeGlobalLogoIMG.ContentId + @"' width='30%'/>";

                                    string ClientNameMail = Session["ClientNameMail"].ToString();
                                    //Candidate Template Details
                                    body = body.Replace("{document}", docString.ToString());
                                    body = body.Replace("{CandidateName}", loginDetail.CandidateName);
                                    body = body.Replace("#CompanyName", ClientNameMail);
                                    body = body.Replace("{ClientRefId}", personalData.ClientRefID);
                                    body = body.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                    body = body.Replace("{UName}", loginDetail.UserID);
                                    body = body.Replace("{UPasswd}", clsCommonMethods.PasswordDecrypt(loginDetail.UPass.Trim()));
                                    body = body.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                                    body = body.Replace("{triangle}", triangle);
                                    body = body.Replace("{securitas}", securitas);
                                    body = body.Replace("{walsons}", walsons);
                                    body = body.Replace("{BGVdotted}", BGVdotted);
                                    body = body.Replace("{redbulllogo}", redbulllogo);
                                    body = body.Replace("{JadeLogo}", JadeGlobalLogo);
                                    body = body.Replace("{MailSignature}", MailSignature);

                                    //Spoc And Client Template Details
                                    var Resendlinks = "https://bgv.securitas-india.com/account/ResendLink/?PersonalIds=" + pID.ToString();
                                    bodySpocAndClient = bodySpocAndClient.Replace("{Resendlink}", Resendlinks);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{document}", docString.ToString());
                                    bodySpocAndClient = bodySpocAndClient.Replace("{CandidateName}", loginDetail.CandidateName);
                                    bodySpocAndClient = bodySpocAndClient.Replace("#CompanyName", ClientNameMail);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                    bodySpocAndClient = bodySpocAndClient.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                                    bodySpocAndClient = bodySpocAndClient.Replace("{triangle}", triangle);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{securitas}", securitas);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{walsons}", walsons);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{BGVdotted}", BGVdotted);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{redbulllogo}", redbulllogo);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{JadeLogo}", JadeGlobalLogo);
                                    bodySpocAndClient = bodySpocAndClient.Replace("{MailSignature}", MailSignature);

                                    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                                    alternateView.LinkedResources.Add(Triangleres);
                                    alternateView.LinkedResources.Add(Securitassec);
                                    alternateView.LinkedResources.Add(Walsonssec);
                                    alternateView.LinkedResources.Add(BGVdottedImg);
                                    alternateView.LinkedResources.Add(RedbulllogoImg);
                                    alternateView.LinkedResources.Add(JadeGlobalLogoIMG);

                                    AlternateView alternateViewSpocAndClient = AlternateView.CreateAlternateViewFromString(bodySpocAndClient, null, MediaTypeNames.Text.Html);
                                    alternateViewSpocAndClient.LinkedResources.Add(Triangleres);
                                    alternateViewSpocAndClient.LinkedResources.Add(Securitassec);
                                    alternateViewSpocAndClient.LinkedResources.Add(Walsonssec);
                                    alternateViewSpocAndClient.LinkedResources.Add(BGVdottedImg);
                                    alternateViewSpocAndClient.LinkedResources.Add(RedbulllogoImg);
                                    alternateViewSpocAndClient.LinkedResources.Add(JadeGlobalLogoIMG);

                                    if (ClientRowID == 2)
                                    {
                                        string key = loginDetail.CandidateUserRowID + "_" + personalData.ClientRowID + "_" + personalData.PersonalRowID;
                                        body = body.Replace("{MediaLink}", ConfigurationManager.AppSettings["CandidateMediaUrl"].ToString() + "?Key=" + clsCommonMethods.PasswordEncrypt(key));

                                    }
                                    string Subject = "";
                                    // For Sutherland Global  249,277
                                    if (ClientRowID == 249 || ClientRowID == 277)
                                    {
                                        Subject = loginDetail.CandidateName + "  Sutherland Background Verification by Securitas";
                                        DisplayName = "Sutherland BGV by Securitas";
                                    }
                                    // For TATA Elxsi
                                    else if (ClientRowID == 635)
                                    {
                                        body = body.Replace("{document}", "Documents Required<br /> Education: Degree Certificate / Final Year Marksheet < br /> " +
                                            "Address: Address Proof < br />   Nationality: ID Proof");
                                        Subject = "Background verification";
                                    }
                                    else
                                    {
                                        Subject = loginDetail.CandidateName + "  Background Verification";
                                    }
                                    string To = loginDetail.CandidateEmail;
                                    if (ClientRowID == 249 || ClientRowID == 277 || ClientRowID == 193)
                                    {
                                        Cc = Cc + ";" + personalData.AlternateEmail;
                                    }

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
                                    DataTable Dt = cc.getCandidateMailstatus(pID.ToString());

                                    int countSent = 0, countFailed = 0, totRec = 0;
                                    bool flag = false;
                                    bool flagspocandclient = false;
                                    if (Dt.Rows.Count > 0)
                                    {
                                        try
                                        {
                                            if (Convert.ToString(personalData.MobileNo) != "")
                                            {
                                                MessageStatus objMessage = new MessageStatus();
                                                WhatsAppAPI objs = new WhatsAppAPI();
                                                var link = "https://bgv.securitas-india.com/Account/CandidateLogin";
                                                objMessage = objs.SendWhatsAppMessage("+91" + personalData.MobileNo, "case_registration_candidate_invite_mode", loginDetail.CandidateName, "media_template", ClientNameMail, To, link);
                                                cc.sp_AddAuditWhatsAppAPI(personalData.PersonalRowID, objMessage.message, objMessage.status, "Candidate Invite");
                                            }
                                        }
                                        catch
                                        {

                                        }
                                        totRec = Dt.Rows.Count;
                                        for (int i = 0; i < totRec; i++)
                                        {
                                            if (Dt.Rows[0]["SentMailStatus"].ToString() == "0")
                                            {
                                                if (ClientRowID == 503)
                                                {
                                                    flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                    flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                }
                                                else
                                                {
                                                    if (ClientRowID == 257) //Biz 2 Credit Info Services Pvt. Ltd
                                                    {
                                                        string bcc = "";
                                                        string Ccc = "";
                                                        Ccc = spocEmail;
                                                        bcc = cSpocEmail;
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Ccc, bcc, Subject, bodySpocAndClient);
                                                        //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, Ccc, bcc,Subject, bodySpocAndClient, null, null);
                                                    }
                                                    else if (ClientRowID == 249)//sutherlandglobal 
                                                    {
                                                        String CcSuthermail = "brij.bhushan@securitas-india.com";

                                                        CcSuthermail = CcSuthermail + "," + Ccmail;

                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, CcSuthermail, Subject, bodySpocAndClient, attachment, null);
                                                    }
                                                    // For TATA Elxsi
                                                    else if (ClientRowID == 635)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);

                                                    }
                                                    else if (ClientRowID == 957)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                    }
                                                    else if (ClientRowID == 1312)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                                    }
                                                    else if (ClientRowID == 778)
                                                    {

                                                        String CcHHS = personalData.BusinessName;
                                                        if (CcHHS == null)
                                                        {
                                                            CcHHS = Ccmail.ToString();
                                                        }
                                                        else
                                                        {
                                                            CcHHS = CcHHS + "," + Ccmail;
                                                        }
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, CcHHS, Subject, bodySpocAndClient, attachment, null);

                                                    }
                                                    else if (ClientRowID == 1005 || ClientRowID == 2)
                                                    {
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);

                                                    }
                                                    else if (ClientRowID == 889)
                                                    {
                                                        flag = mail.SendMailPurgWithoutCC(DisplayName, From, ToMail, Subject, body, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, cSpocEmail, Subject, bodySpocAndClient, attachment, null);
                                                    }
                                                    else if (ClientRowID == 974)
                                                    {
                                                        flag = mail.SendMailAdidas(DisplayName, From, ToMail, Subject, body);
                                                        flagspocandclient = mail.SendMailAdidasForSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, body);
                                                    }
                                                    else if ((ClientRowID == 1496 || MailClientRowid == 2 || MailClientRowid == 193) && (MailTemplateName == "CandidateLoginDetailsTemplate" || MailTemplateNameSpoc == "CandidateLoginDetailsTemplate"))
                                                    {
                                                        flag = mail.CandidateLoginSendMail(DisplayName, From, ToMail, Subject, body, attachment, alternateView);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, alternateViewSpocAndClient);
                                                    }
                                                    else if (ClientRowID == 1481)
                                                    {
                                                        DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                                        var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                                        if (recuiteremail != "")
                                                        {
                                                            Ccmail += "," + recuiteremail;
                                                        }
                                                        flag = mail.SendMailToCandidateForCtrls(DisplayName, From, ToMail, Subject, body, attachment, attachment2);
                                                        flagspocandclient = mail.SendMailToCandidateForCtrlsForSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, attachment2);
                                                    }
                                                    else
                                                    {
                                                        //=====================================
                                                        if (ClientRowID == 636)
                                                        {
                                                            DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                                            var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                                            if (recuiteremail != "")
                                                            {
                                                                Ccmail += "," + recuiteremail;
                                                            }
                                                        }
                                                        //===========================================
                                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                                        //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClientWithoutAlternateView(DisplayName, From, Ccmail, "", Subject, bodySpocAndClient, attachment);
                                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, null);
                                                    }
                                                }

                                                if (flag)
                                                {
                                                    if (ClientRowID == 249 || ClientRowID == 277) //sutherlandglobal and Sutherland Healthcare Solutions Pvt. Ltd.
                                                    {
                                                        try
                                                        {
                                                            string Message = "";
                                                            Message = "Dear " + personalData.Name + Environment.NewLine + Environment.NewLine;
                                                            Message += "Please complete Sutherland BGV on email sent to your ID or below link." + Environment.NewLine;
                                                            Message += "ID:" + loginDetail.UserID + Environment.NewLine;
                                                            Message += "Pwd:" + clsCommonMethods.PasswordDecrypt(loginDetail.UPass) + Environment.NewLine;
                                                            Message += "Link:" + ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString() + Environment.NewLine;
                                                            Message += "Regards" + Environment.NewLine;
                                                            Message += "Securitas";
                                                            loginDetail.SMSText = Message;
                                                            loginDetail.PersonalId = pID;
                                                            SendSMS(loginDetail);
                                                        }
                                                        catch
                                                        {

                                                        }
                                                    }
                                                    if (ClientRowID == 635)
                                                    {
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                    }
                                                    else if (ClientRowID == 636)
                                                    {
                                                        cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                    }
                                                    else if (ClientRowID == 957)
                                                    {
                                                        cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                    }
                                                    else if (ClientRowID == 1209)
                                                    {
                                                        DataTable dt = cc.GetCandidateClient(ToMail, pID);
                                                        if (dt.Rows.Count > 0)
                                                        {
                                                            int clientrowidnew = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                                                            cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, clientrowidnew, 1);
                                                            cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                        }
                                                    }
                                                    else if (ClientRowID == 1005 || ClientRowID == 2 || ClientRowID == 889)
                                                    {
                                                        SaveWPassMail = SaveWPassMail.Replace("Click the below Url, login", "");//.Replace("Candidate Login", "").Replace("Click Reference ID:{ClientRefId}", "").Replace("Login Id: {UName}", "").Replace("Password: {UPasswd}", "");
                                                        SaveWPassMail = SaveWPassMail.Replace("into the website", "");
                                                        SaveWPassMail = SaveWPassMail.Replace("Candidate Login", "");
                                                        SaveWPassMail = SaveWPassMail.Replace("Click Reference ID:{ClientRefId}", "");

                                                        SaveWPassMail = SaveWPassMail.Replace("Password: {XXXXX}", "");

                                                        SaveWPassMail = SaveWPassMail.Replace("Login Id: {XXXXX}", "");

                                                        SaveWPassMail = SaveWPassMail.Replace("{document}", docString.ToString());
                                                        SaveWPassMail = SaveWPassMail.Replace("{CandidateName}", loginDetail.CandidateName);
                                                        SaveWPassMail = SaveWPassMail.Replace("#CompanyName", ClientNameMail);
                                                        SaveWPassMail = SaveWPassMail.Replace("{ClientRefId}", personalData.ClientRefID);
                                                        SaveWPassMail = SaveWPassMail.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                                        SaveWPassMail = SaveWPassMail.Replace("{UName}", "XXXXX");
                                                        SaveWPassMail = SaveWPassMail.Replace("{UPasswd}", "XXXXX");
                                                        SaveWPassMail = SaveWPassMail.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());

                                                        SaveWPassMail = SaveWPassMail.Replace("{MailSignature}", MailSignature);

                                                        cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                                    }
                                                    else
                                                    {
                                                        string Message = $"Dear {loginDetail.CandidateName} We kindly request you to upload the required document in the application, using the login credentials sent on your email. Feel free to connect in case of any assistance required on mentioned contact details on email. Thank you Walsons Services Pvt. Ltd.";
                                                        loginDetail.PersonalId = pID;
                                                        loginDetail.SMSText = Message;
                                                        SendSMS(loginDetail);
                                                        cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                                        cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);

                                                    }

                                                    if (ClientRowID == 635)
                                                    {
                                                        //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                                        RemindermailTataElxsi(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                                    }

                                                    if (ClientRowID == 957)
                                                    {
                                                        RemindermailAstraZenca(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);

                                                    }

                                                    if (ClientRowID == 679 || ClientRowID == 725)
                                                    {
                                                        //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                                        SaveReminderMail(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                                    }
                                                    UpdatePQCandidateLoginForSendMailViewModel loginModel = new UpdatePQCandidateLoginForSendMailViewModel();
                                                    loginModel.CandidateUserRowID = loginDetail.CandidateUserRowID;
                                                    loginModel.SentMailStatus = 1;
                                                    loginModel.SentMailDate = DateTime.Now;
                                                    repoCandidate.UpdateCandidateLoginSendMailStatusById(loginModel);
                                                    countSent++;
                                                }
                                                else
                                                {
                                                    countFailed++;
                                                }
                                            }

                                        }

                                    }
                                    TempData["ErrorMsg"] = "Out of " + totRec.ToString() + " Mail sent : " + countSent.ToString() + " failed : " + countFailed.ToString();

                                }
                                else
                                {
                                    TempData["ErrorMsg"] = "Candidate login details not found!";
                                }
                            }
                        }
                        #endregion

                        mm = repoPQPersonal.GetTempCandidateListByClientBulkUpload(page, pageSize, sort, sortDir, Search, ClientRowID);

                        if (mm == null)
                        {
                            ViewBag.ErrorMsg = "No item found!";
                        }
                    }
                    //PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();               
                    SearchModel.Search = Search;
                    mm.ItemSearch = SearchModel;
                }
                catch (Exception Ex)
                {
                    SearchModel.Search = "";
                    mm.ItemSearch = SearchModel;
                    //ViewBag.ErrorMsg = Ex.Message;
                    ViewBag.ErrorMsg = "Invalid parameter value";
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                }
                //return View(model);
                //return RedirectToAction("ManageClientCandidate", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }


            //////////////////////////////////////////////////////////////////////////


            if (ViewBag.ErrorMsg == "")
            {
                if (clientRowId == 1324)
                {
                    TempData["SuccessMsg"] = "Case Registered Successfully";
                }
                else
                {
                    TempData["SuccessMsg"] = "Case Registered Successfully" + "," + "Send mail from “Manage Candidate” section";
                }
                return RedirectToAction("AddSingleCandidateCaseUpload");
            }
            else
            {
                int ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                //int ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
                ClientCustomize bl = new ClientCustomize();
                ViewBag.LastFiveCases = bl.LastFiveCasesInitiatedToCandidate(ClientRowID, ClientUserRowID);
                string ErrorMsg = Convert.ToString(ViewBag.ErrorMsg);
                string comm = ErrorMsg.Substring(0, 1);
                if (comm.Contains(","))
                {
                    ViewBag.ErrorMsg = ErrorMsg.Substring(1, ErrorMsg.Length - 1);
                }
                return View(marksModel);
            }

        }

        public JsonResult UploadCandidateCV(long id, HttpPostedFileBase UploadCV)
        {
            try
            {
                if (UploadCV == null || UploadCV.ContentLength == 0)
                    return Json(new { result = false, msg = "Please select CV" });
                string ext = Path.GetExtension(UploadCV.FileName).ToLower();
                string[] allowed = { ".pdf", ".doc", ".docx" };
                if (Array.IndexOf(allowed, ext) < 0 || UploadCV.ContentLength > 5 * 1024 * 1024)
                    return Json(new { result = false, msg = "Invalid file type or size" });
                string filename = id + "_CV_" + Path.GetFileName(UploadCV.FileName);
                string base64;
                UploadCV.InputStream.Position = 0;
                using (var memoryStream = new MemoryStream())
                {
                    UploadCV.InputStream.CopyTo(memoryStream);
                    base64 = Convert.ToBase64String(memoryStream.ToArray());
                }
                _documentService.UploadDynamicCandidateFiles(filename, base64, "D:/wwwroot2/Uploads/");
                ClientCustomize bl = new ClientCustomize();
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = id;
                cd.DisplayName = "CV";
                cd.DataEntryValue = filename;
                bl.proc_InsertUpdateTempPQVerifiedUploadCaseDocs(cd, "~/Uploads/");
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), ex.Message, ex.StackTrace);
                return Json(new { result = false, msg = "Error while uploading CV" });
            }
        }


        public string Validation_HTMLSanitizer(string Search = "")
        {
            try
            {
                var sata = getval(Search);
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

        public string Validation_HTMLSanitizer(string Name = "", string MiddleN = "", string LName = "", string EmailId = "", string Mob = "", string ClientRefId = "")
        {

            try
            {

                var sata = getval(Name);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(MiddleN);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(LName);
                if (sata == "false")
                {
                    return sata;
                }

                sata = getval(EmailId);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(Mob);
                if (sata == "false")
                {
                    return sata;
                }
                sata = getval(ClientRefId);
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

        public class Sql_Key_Val
        {
            public string SQl_Check { get; set; }
            public bool Status { get; set; }
        }


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

        public string getval(string val)
        {
            string st = null;
            if (val != null)
            {
                st = "";
                var vals = val.ToUpper().Trim();
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

        public void SavePQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", string remarks = "", string fileName = "")
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
                //string ext = Path.GetExtension(UploadDoc.FileName).ToLower();
                //proImg = (SubCheckRowID.ToString() + DocumentType + Session["CadidateUserName"].ToString()).Replace(" ", "_").Replace("-", "_") + DateTime.Now.ToString("ssfff") + ext;
                //UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                DocModel.FileName = fileName;
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
                FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();
                string proImg = string.Empty;
                string ext = Path.GetExtension(UploadDoc.FileName).ToLower();
                proImg = (SubCheckRowID.ToString() + DocumentType + DocModel.PersonalRowID).Replace(" ", "_").Replace("-", "_") + DateTime.Now.ToString("ssfff") + ext;
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
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult ChangeCandidatePassword(string pid = "")
        {
            UpdatePQCandidateLoginViewModel model = new UpdatePQCandidateLoginViewModel();
            try
            {
                int Epid = 0;
                if (pid != "")
                {
                    Epid = Convert.ToInt32(clsCommonMethods.PDecrypt(pid));
                }
                BindClientNameSubGrup();
                BindTypeList();
                BindYTRApplicableList();
                model = repoCandidate.GetCandidateLoginForPassUpdateById(Epid);
                string EncryptedPasswd = clsCommonMethods.PasswordDecrypt(model.UPass.Trim());
                model.UPass = EncryptedPasswd;
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

        [HttpPost]
        [SessionExpireFilterAttributeClient]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ChangeCandidatePassword(UpdatePQCandidateLoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.ModifiedTime = DateTime.Now;
                    string EncryptedPasswd = clsCommonMethods.PasswordEncrypt(model.UPass.Trim());
                    model.UPass = EncryptedPasswd;
                    model.UserType = model.UserType;
                    repoCandidate.UpdateCandidatePasswordByID(model);
                    TempData["SuccessMsg"] = "Record Update Successfully!";
                    ModelState.Clear();
                    return RedirectToAction("ManageClientCandidate");
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
                ViewBag.ErrorMsg = "Error Found !.";
                string ErrorMessage = ViewBag.ErrorMsg ;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return RedirectToAction("ExceptionPage", "Home", new { area = "ClientArea", ClientRowId = "", msg = ErrorMessage });
            }
            BindClientNameSubGrup();
            BindYTRApplicableList();
            BindTypeList();
            return View();
        }

        #endregion

        #region ******* NonAction/Json *******

        private void ImportExcelAndSaveCandidateByBulkUpload(string FilePath, string Extension, string isHDR, AddTempPQPersonalForBulkUpViewModel model, string TicketNo, string btnType = "Submit") //isHDR - means Include header Yes or No
        {
            ClientCustomize cc = new ClientCustomize();
            try
            {
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

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();

                //Read Data from First Sheet
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();

                //Insert into database CandidatesTempBulk table
                if (dt.Rows.Count > 0)
                {
                    //string lblMsg = clsCommanMethod.MsgBox("Total Candidates : " + dt.Rows.Count.ToString());
                    int countSuccess = 0;
                    int countDuplicate = 0;
                    int countInvalid = 0;
                    string Email = string.Empty;
                    string MobileNo = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["Name"].ToString()))
                        {


                            Email = dt.Rows[i]["Email"].ToString().Trim();
                            bool flag = IsValidEmail(Email);
                            model.Name = dt.Rows[i]["Name"].ToString();
                            model.VoterIDCardNo = dt.Rows[i]["VoterIDCardNo"].ToString();
                            model.DrivingLicenseNo = dt.Rows[i]["DrivingLicenseNo"].ToString();
                            model.AadhaarCardNo = dt.Rows[i]["AadhaarCardNo"].ToString();
                            model.PassportNo = dt.Rows[i]["PassportNo"].ToString();
                            model.PanCardNo = dt.Rows[i]["PanCardNo"].ToString();

                            model.FatherName = dt.Rows[i]["FatherName"].ToString();
                            model.MiddleName = dt.Rows[i]["MiddleName"].ToString();
                            model.LastName = dt.Rows[i]["LastName"].ToString();
                            model.Gender = dt.Rows[i]["Gender"].ToString();
                            if (model.ClientRowID == 636)
                            {
                                model.RecruiterEmailID = dt.Rows[i]["HRSPOCsemailid "].ToString();
                            }
                            else if (model.ClientRowID == 193)
                            {
                                model.AlternateEmailID = dt.Rows[i]["DLHR&TAEmailID"].ToString();
                            }
                            else
                            {
                                model.AlternateEmailID = dt.Rows[i]["AlternateEmail"].ToString();
                            }


                            if (string.IsNullOrEmpty(model.Gender) || model.Gender.ToLower() == "male")
                            {
                                model.Gender = "male";
                                model.CandTitle = "Mr.";
                            }
                            else
                            {
                                model.CandTitle = "Ms.";
                            }
                            model.CandidateUserID = 0;
                            model.EmployeID = dt.Rows[i]["EmployeID"].ToString();
                            model.ClientRefID = dt.Rows[i]["ClientRefID"].ToString();
                            model.ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                            model.CreatedDate = DateTime.Now;
                            model.CandidateCode = GetCandidateCode(model.ClientRowID);
                            model.FromType = "Client-Cases";
                            model.DetailsEnteredBy = "Candidate";
                            model.OtherDetails = "Pending";
                            model.OtherDetails1 = "Candidate";
                            model.CaseStatus = 1;
                            model.Status = 1;
                            model.CandidateName = model.Name + " " + model.MiddleName + " " + model.LastName;
                            model.CandidateType = dt.Rows[i]["CandidateType"].ToString();
                            model.EmailID = Email;
                            MobileNo = dt.Rows[i]["Mobile"].ToString().Trim().Replace("'", "").Replace(" ", "");
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
                            else
                            {
                                model.MobileNo = MobileNo.Trim();
                            }

                            model.ClientUserName = dt.Rows[i]["ClientUserName"].ToString();
                            if (dt.Rows[i]["DateofJoining"].ToString().Length > 4)
                            {
                                model.DateofJoining = Convert.ToDateTime(dt.Rows[i]["DateofJoining"].ToString());
                            }
                            model.PlaceOfJoining = dt.Rows[i]["PlaceOfJoining"].ToString();
                            model.AlternateNo = dt.Rows[i]["AlternateNo"].ToString();
                            model.ProcessName = dt.Rows[i]["ProcessName"].ToString();
                            model.ProjectId = dt.Rows[i]["ProjectId"].ToString();
                            model.ProjectName = dt.Rows[i]["ProjectName"].ToString();
                            model.Entity = dt.Rows[i]["Entity"].ToString();
                            if (model.ClientRowID == 858 || model.ClientRowID == 722)//Patra client
                            {

                                model.Designation = dt.Rows[i]["Designation"].ToString();
                            }
                            else
                            {
                                model.Designation = "";
                            }
                            if (model.ClientRowID == 1057)//CTSIL UAN
                            {
                                model.Grade = dt.Rows[i]["Grade"].ToString();

                            }
                            else
                            {
                                model.Grade = "";
                            }
                            model.UAN = dt.Rows[i]["UAN"].ToString();
                            if (model.ClientRowID == 722)//Genpact
                            {
                                model.PID = dt.Rows[i]["PID Number"].ToString();
                                model.SDO_Code = dt.Rows[i]["SDO Code"].ToString();
                                model.COE_Code = dt.Rows[i]["COE Code"].ToString();
                                model.UID = dt.Rows[i]["UID Number"].ToString();
                                model.OHR_Taleo_ID = dt.Rows[i]["OHR/Taleo ID"].ToString();
                                model.Band = dt.Rows[i]["Band"].ToString();
                            }
                            else
                            {
                                model.PID = "";
                                model.SDO_Code = "";
                                model.COE_Code = "";
                                model.UID = "";
                                model.OHR_Taleo_ID = "";
                                model.Band = "";
                            }
                            if (model.ClientRowID == 1193)//TCSION iDZ & LISP
                            {

                                model.Location = dt.Rows[i]["Zone"].ToString();
                            }
                            else
                            {
                                model.Location = "";

                            }
                            if (dt.Rows[i]["DOB"].ToString() != "")
                            {
                                model.DOB = Convert.ToDateTime(dt.Rows[i]["DOB"].ToString());
                            }
                            if (flag)
                            {
                                if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.EmailID))
                                {
                                    if (model.ClientRowID == 940 && btnType == "Force Submit")
                                    {
                                        DataTable dtsForce = cc.InsertForceTempPQPersonals(model);
                                        if (dtsForce.Rows.Count > 0)
                                        {
                                            model.PersonalRowID = Convert.ToInt32(dtsForce.Rows[0][0].ToString());
                                        }
                                        if (model.PersonalRowID > 0)
                                        {
                                            SaveTempPackageOrCheckWithCandidate(model);
                                            CreateUserIdForCandidate(model);
                                            countSuccess++;
                                        }
                                    }
                                    else
                                    {
                                        if (!repoPQPersonal.IsCandidateEmailExist(model.EmailID, model.ClientRowID))
                                        {
                                            if (model.ClientRowID == 576)
                                            {
                                                model.BusinessName = dt.Rows[i]["Entity"].ToString();
                                            }
                                            var DuplicateList = cc.CheckCandidateduplicateName(model.ClientRowID, model.Name, model.MiddleName, model.LastName, "", model.MobileNo, model.EmployeID);
                                            //model.CandidateDuplicateList = repoPQPersonal.CheckCandidateduplicateNameFromCandidate(model.ClientRowID, model.Name, model.MiddleName, model.LastName,"",model.MobileNo);
                                            if (DuplicateList.Rows.Count > 0)
                                            {
                                                cc.Insert_DuplicateReferenceDetail(TicketNo, model.Name, model.MiddleName, model.LastName, model.MobileNo, model.ClientUserRowID.ToString(), model.EmailID, true, model.ClientRowID.ToString(), model.FatherName, "Client", null);
                                            }
                                            else
                                            {
                                                cc.Insert_DuplicateReferenceDetail(TicketNo, model.Name, model.MiddleName, model.LastName, model.MobileNo, model.ClientUserRowID.ToString(), model.EmailID, false, model.ClientRowID.ToString(), model.FatherName, "Client", null);
                                            }
                                            if (DuplicateList.Rows.Count == 0)
                                            {
                                                DataTable dts = cc.InsertTempPQPersonals(model);
                                                if (dts.Rows.Count > 0)
                                                {
                                                    model.PersonalRowID = Convert.ToInt32(dts.Rows[0][0].ToString());
                                                }
                                                DataTable dts1 = cc.GetTempPersonalRowIDByNameAndClientRowID(559, model.Name, model.EmailID);
                                                if (dts1.Rows.Count > 0)
                                                {
                                                    model.PersonalRowID = Convert.ToInt32(dts1.Rows[0]["PersonalRowID"].ToString());
                                                }
                                            }
                                            if (model.PersonalRowID > 0)
                                            {
                                                SaveTempPackageOrCheckWithCandidate(model);
                                                CreateUserIdForCandidate(model);

                                                if (model.ClientRowID == 559 )//First American
                                                {
                                                    AddAddressRecordsViewModel addObj = new AddAddressRecordsViewModel();
                                                    addObj.PersonalRowId = model.PersonalRowID;
                                                    addObj.SubCheckRowId = 221;
                                                    addObj.CandidateName = model.CandidateName;
                                                    addObj.Address = Convert.ToString(dt.Rows[i]["AddressLine"]);
                                                    addObj.City = Convert.ToString(dt.Rows[i]["City"]);
                                                    addObj.Country = Convert.ToString(dt.Rows[i]["Country"]);
                                                    addObj.State = Convert.ToString(dt.Rows[i]["State"]);
                                                    addObj.District = Convert.ToString(dt.Rows[i]["District"]);
                                                    addObj.MobileNo = model.MobileNo;
                                                    addObj.Pincode = Convert.ToString(dt.Rows[i]["Pincode"]);
                                                    addObj.ClientRowId = model.ClientRowID;
                                                    addObj.IDNumber = model.PanCardNo;
                                                    if (Convert.ToString(model.DOB) != "")
                                                    {
                                                        addObj.DOB = Convert.ToDateTime(model.DOB).ToString("dd-MMM-yyyy");
                                                    }
                                                    UpdateCandidateAddress(addObj, model.ClientUserRowID);
                                                }
                                                countSuccess++;
                                                if (model.ClientRowID == 249)
                                                {
                                                    new API().InsertAPI_AuditDetail(model.ClientRefID, model.ClientRowID, "Success", "CaseInitiate Test Bulk", "PersonalId:" + model.PersonalRowID, "Case Initiated Successfully", "API");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            cc.Insert_DuplicateReferenceDetail(TicketNo, model.Name, model.MiddleName, model.LastName, model.MobileNo, model.ClientUserRowID.ToString(), model.EmailID, true, model.ClientRowID.ToString(), model.FatherName, "Client", null);
                                            countDuplicate++;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (flag == false)
                                {
                                    ViewBag.SuccessFully = countSuccess.ToString() + " please check Email Id!";
                                }

                            }
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                    if (countSuccess > 0)
                    {
                        if (countDuplicate > 0)
                        {
                            ViewBag.SuccessFully = (countDuplicate + countSuccess).ToString() + " Candidate(s) added successfully!";
                        }
                        else
                        {
                            ViewBag.SuccessFully = countSuccess.ToString() + " Candidate(s) added successfully!";
                        }

                    }
                    else if (countDuplicate > 0)
                    {

                        ViewBag.SuccessFully = countDuplicate.ToString() + " Candidate(s) added successfully!";

                    }

                }
                else
                {
                    ViewBag.ErrorMsg = "No candidate found!";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void UpdateCandidateAddress(AddAddressRecordsViewModel model,int ClientUserRowID)
        {
            try
            {
                DataBaseConnections Dc = new DataBaseConnections();
                ClientCustomize bl = new ClientCustomize();
                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = model.PersonalRowId;
                cd.SubCheckRowID = model.SubCheckRowId;
                cd.clientRowId = model.ClientRowId;
                cd.CreatedBy = ClientUserRowID;
                if (Dc.IsPackageExistsInCase(model.PersonalRowId, model.ClientRowId, "FAIPL_Package Re-Screening (ADD_CRC_OFAC_ID_GDB)"))
                {
                    if (Convert.ToString(model.CandidateName) != "")
                    {
                        cd.AntecedentRowID = 30;
                        cd.DisplayName = "Candidate Name";
                        cd.DataEntryValue = model.CandidateName;
                        bl.SaveDataEntry_Candidate(cd);
                    }
                    if (Convert.ToString(model.Address) != "")
                    {
                        cd.AntecedentRowID = 27;
                        cd.DisplayName = "Address";
                        cd.DataEntryValue = model.Address;
                        bl.SaveDataEntry_Candidate(cd);
                    }
                    if (Convert.ToString(model.City) != "")
                    {
                        cd.AntecedentRowID = 32;
                        cd.DisplayName = "City";
                        cd.DataEntryValue = model.City;
                        bl.SaveDataEntry_Candidate(cd);
                    }
                    if (Convert.ToString(model.District) != "")
                    {
                        cd.AntecedentRowID = 130;
                        cd.DisplayName = "District";
                        cd.DataEntryValue = model.District;
                        bl.SaveDataEntry_Candidate(cd);
                    }
                    if (Convert.ToString(model.State) != "")
                    {
                        cd.AntecedentRowID = 44;
                        cd.DisplayName = "State";
                        cd.DataEntryValue = model.State;
                        bl.SaveDataEntry_Candidate(cd);
                    }
                    if (Convert.ToString(model.Pincode) != "")
                    {
                        cd.AntecedentRowID = 42;
                        cd.DisplayName = "Pincode";
                        cd.DataEntryValue = model.Pincode;
                        bl.SaveDataEntry_Candidate(cd);
                    }
                    if (Convert.ToString(model.MobileNo) != "")
                    {
                        cd.AntecedentRowID = 131;
                        cd.DisplayName = "Mobile Number";
                        cd.DataEntryValue = model.MobileNo;
                        bl.SaveDataEntry_Candidate(cd);
                    }

                    if (Convert.ToString(model.Country) != "")
                    {
                        cd.AntecedentRowID = 34;
                        cd.DisplayName = "Country";
                        cd.DataEntryValue = model.Country;
                        bl.SaveDataEntry_Candidate(cd);
                    }

                    //For Id Check
                    if (Convert.ToString(model.IDNumber) != "")
                    {
                        CandidateDEModel IDObj = new CandidateDEModel();
                        IDObj.PersonalRowID = model.PersonalRowId;
                        IDObj.SubCheckRowID = 136;
                        IDObj.clientRowId = model.ClientRowId;
                        IDObj.CreatedBy = ClientUserRowID;
                        if (Convert.ToString(model.CandidateName) != "")
                        {
                            IDObj.AntecedentRowID = 99;
                            IDObj.DisplayName = "Candidate Name";
                            IDObj.DataEntryValue = model.CandidateName;
                            bl.SaveDataEntry_Candidate(IDObj);
                        }
                        if (Convert.ToString(model.DOB) != "")
                        {
                            IDObj.AntecedentRowID = 100;
                            IDObj.DisplayName = "Date of Birth";
                            IDObj.DataEntryValue = model.DOB;
                            bl.SaveDataEntry_Candidate(IDObj);
                        }
                        if (Convert.ToString(model.IDNumber) != "")
                        {
                            IDObj.AntecedentRowID = 154;
                            IDObj.DisplayName = "Identity Type";
                            IDObj.DataEntryValue = "PAN Card";
                            bl.SaveDataEntry_Candidate(IDObj);

                            IDObj.AntecedentRowID = 104;
                            IDObj.DisplayName = "ID Number";
                            IDObj.DataEntryValue = model.IDNumber;
                            bl.SaveDataEntry_Candidate(IDObj);
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

        }



        private void CreateUserIdForCandidate(AddTempPQPersonalForBulkUpViewModel perModel)
        {
            AddPQCandidateLoginViewModel model = new AddPQCandidateLoginViewModel();
            try
            {

                model.CandidateName = perModel.Name + " " + perModel.MiddleName + " " + perModel.LastName;
                model.ClientRowID = perModel.ClientRowID;
                model.CandidateEmail = perModel.EmailID;
                model.CantactNo = perModel.MobileNo;

                #region Generate UserID And Pass
                string UserID = Session["Abbreviation"].ToString().ToUpper();
                int NumberOfEntiry = repoCandidate.GetAllCandidateUserByClientId(perModel.ClientRowID);
                string empty = string.Empty;

                if (NumberOfEntiry >= 10)
                {
                    NumberOfEntiry++;
                    empty = Convert.ToString(NumberOfEntiry);
                }
                else
                {
                    NumberOfEntiry++;
                    empty = string.Concat("0", Convert.ToString(NumberOfEntiry));
                }
                string[] strArrays = new string[] { UserID, DateTime.Now.ToString("ddMMyy"), empty };
                UserID = string.Concat(strArrays);
                #endregion

                model.UserID = UserID;
                //model.UPass = UserID + "#";
                //string passwrd = clsCryptoRandomNumber.GetRandomAlphaNumeric();
                string passwrd = clsCryptoRandomNumber.GeneratePassword(3, 3, 4);
                passwrd = passwrd + "@" + DateTime.Now.ToString("mmmsss");
                model.UPass = passwrd;
                if (!repoCandidate.IsCandidateLoginExist(model.UserID.Trim(), model.CandidateEmail.Trim()))
                {
                    model.CreatedBy = "Client-Candidate";
                    model.CreatedTime = DateTime.Now;
                    model.Status = 1;

                    string EncryptedPasswd = clsCommonMethods.PasswordEncrypt(model.UPass.Trim());
                    model.UPass = EncryptedPasswd;
                    model.EmployeeID = perModel.EmployeID;
                    model.YTRApplicable = 0;
                    model.UserType = 1;
                    model.CreatedUserId = 0;
                    model.TempPersonalRowID = perModel.PersonalRowID;
                    model.PersonalRowID = 0;
                    ClientCustomize cc = new ClientCustomize();
                    cc.insertpqcandidatelogins(model);
                    cc.Insert_LoginLog("Candidate Login", "Create Candidate credential !", model.UserID, "PersonalRowId: " + model.PersonalRowID +  " IP Address :" + GetIP_Address());

                }
                else
                {
                    TempData["ErrorMsg"] = "Record Already Exist!";
                }
            }
            catch (Exception Ex)
            {
                ClientCustomize cc = new ClientCustomize();
                cc.Insert_LoginLog("CreateUserIdForCandidate", Ex.Message, model.UserID, "UserName: " + model.UserID + " IP Address :" + GetIP_Address());
                throw;
            }
        }
        private string GetIP_Address()
        {
            string myIPAddr = "";
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            myIPAddr += $" Interface: {ni.Name}, IP Address: {ip.Address} ";
                        }
                    }
                }
            }
            return myIPAddr.ToString();
        }
        private void SendmailtoCandidate(string PersonalIds, short ClientRowID, int ClientUserRowID)
        {
            #region Send Mail To Candidate
            if (!string.IsNullOrEmpty(PersonalIds))
            {
                ClientCustomize cc = new ClientCustomize();
                string spocEmail = "";
                string cSpocEmail = "";
                string mobileno = "";
                string[] PersonalID = PersonalIds.Split(',');
                string TrmFldPathDoc = "";
                string SendUserManulToCtrls = "";
                string CtrlsfileName = "";
                string fileName = "";
                if (ClientRowID == 576)
                {
                    TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "LOA.pdf";
                    fileName = Path.GetFileNameWithoutExtension("LOA.pdf");
                }
                else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                {
                    TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Chargebee_LOA.docx";
                    fileName = Path.GetFileNameWithoutExtension("Chargebee_LOA.docx");
                }
                else if (ClientRowID == 1425)
                {
                    TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "GlobalAuthorization-Australia.docx";
                    fileName = Path.GetFileNameWithoutExtension("GlobalAuthorization-Australia.docx");
                }
                else if (ClientRowID == 1428)
                {
                    TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Genpact Privacy Notice and Consent Form template_Pre-Employment Screening_EU (Securitas).docx";
                    fileName = Path.GetFileNameWithoutExtension("Genpact Privacy Notice and Consent Form template_Pre-Employment Screening_EU (Securitas).docx");
                }
                else if (ClientRowID == 1497)
                {
                    TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Capco Privacy Notice and Consent Form template_Pre-Employment Screening(Securitas).docx";
                    fileName = Path.GetFileNameWithoutExtension("Capco Privacy Notice and Consent Form template_Pre-Employment Screening(Securitas).docx");
                }
                else if (ClientRowID == 1429)
                {
                    TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "Global_Authorization-Nagarro.docx";
                    fileName = Path.GetFileNameWithoutExtension("Global_Authorization-Nagarro.docx");
                }
                else if (ClientRowID == 1449)
                {
                    TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "GlobalAuthorization-Uniqus.docx";
                    fileName = Path.GetFileNameWithoutExtension("GlobalAuthorization-Uniqus.docx");
                }
                else if (ClientRowID == 1514)
                {
                    TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "SILVERLINK-I.docx";
                    fileName = Path.GetFileNameWithoutExtension("SILVERLINK-I.docx");
                }
                else if (ClientRowID == 1481)
                {
                    TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CtrlS_Datacenter-LOA.pdf";
                    fileName = Path.GetFileNameWithoutExtension("CtrlS_Datacenter-LOA.pdf");
                    SendUserManulToCtrls = ConfigurationManager.AppSettings["CandidateManual"].ToString() + "CandidateManual.pdf";
                    CtrlsfileName = Path.GetFileNameWithoutExtension("CandidateManual.pdf");
                }
                else
                {
                    TrmFldPathDoc = ConfigurationManager.AppSettings["CandidateAgreeDoc"].ToString() + "CandidateAgreement.pdf";
                    fileName = Path.GetFileNameWithoutExtension("CandidateAgreement.pdf");
                }

                //Attachment attachment;
                Attachment attachment;
                Attachment attachment2;
                if (ClientRowID == 1481)
                {
                    attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                    attachment2 = new Attachment(Server.MapPath(SendUserManulToCtrls));
                }
                else
                {
                    attachment = new Attachment(Server.MapPath(TrmFldPathDoc));
                    attachment2 = null;
                }

                string MailSignature = string.Empty;
                string From = string.Empty;
                string DisplayName = string.Empty;
                string Cc = "";

                clsSendMail mail;

                var cSmtp = repoPQClient.GetPQClientSendMailFromById(ClientRowID);
                DataTable dtEmail = new DataTable();

                string ClientAccessType = Convert.ToString(Session["ClientAccessType"]);
                if (ClientAccessType == "3")
                {
                    dtEmail = cc.userwise_ReportingEmailAccess(ClientUserRowID);
                    if (dtEmail.Rows.Count > 0)
                    {
                        spocEmail = dtEmail.Rows[0]["ClientUserEmailID"].ToString();
                        cSpocEmail = dtEmail.Rows[0]["ReportingTo_EmailID"].ToString();

                    }
                }
                else
                {
                    dtEmail = cc.GetEmailStructureForAlert(ClientRowID, ClientUserRowID, "Case Registration");
                    if (dtEmail.Rows.Count > 0)
                    {
                        spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                        cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                        mobileno = dtEmail.Rows[0]["mobileno"].ToString();

                    }
                }
                Cc = cSpocEmail;
                if (cSmtp != null && cSmtp.MailSendBy > 0)
                {
                    mail = new clsSendMail(cSmtp.SMTPServer, cSmtp.SMTPPort, cSmtp.SMTPUserName, clsCommonMethods.PasswordDecrypt(cSmtp.SMTPPassword), Convert.ToBoolean(cSmtp.EnableSsl));

                    From = cSmtp.SMTPUserName;
                    DisplayName = cSmtp.ClientName.Trim();
                    Cc = string.Concat(Cc, ",", spocEmail);

                    MailSignature = string.Concat("<b>", cSmtp.ClientName.Trim(), "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", cSmtp.SMTPUserName, "'>", cSmtp.SMTPUserName, "</a>");
                }
                else
                {
                    mail = new clsSendMail();
                    var signature = mail.GetCompanyMailSignature();

                    if (mobileno != "")
                        mobileno = "<b>Mobile: " + mobileno;
                    string ShowSpocId = spocEmail.ToLower();

                    ShowSpocId = ShowSpocId.Replace(",", ";");
                    string[] ShowSpocId1 = spocEmail.Split(';');
                    if (signature != null)
                    {

                        // For TATA Elxsi
                        if (ClientRowID == 635)
                        {
                            MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                            //"<b>Mobile: ", signature.MobileNo,
                            "</b><br/><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", "bgv.helpdesk@securitas-india.com", "'>", "bgv.helpdesk@securitas-india.com", "</a>");
                        }
                        else if (ClientRowID == 249)
                        {
                            MailSignature = string.Concat("For Securitas India. <br /><br />");
                            MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                            //"<b>Mobile: ", signature.MobileNo,
                            "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                            "</b><br /><b>Contact No:</b><br />  <b>Satya :</b>+91 8377022632 <br />  <b>Pradeep Devrani :</b> +91 8373907723<br /><b>Himanshu Pathak :</b> +91 7291974515 ");
                            //Please mention the contact number of Satya - +91 8377022632 / Pradeep Devrani - 91 - 8373907723 / Himanshu Pathak - +91 7291974515
                        }
                        else if (ClientRowID == 277)
                        {
                            MailSignature = string.Concat("For Securitas India. <br /><br />");
                            MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                            //"<b>Mobile: ", signature.MobileNo,
                            "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                            "</b><br /><b>Contact No </b>:+91-8373907723");
                        }
                        else
                        {
                            MailSignature = string.Empty;
                            MailSignature = string.Concat(ShowSpocId1[0]);
                        }

                    }



                    Cc = string.Concat(Cc, ",", ShowSpocId);

                    From = signature.GeneralEmail;
                    DisplayName = signature.CompanyName.Trim();
                }
                //int sum = 0;
                foreach (var item in PersonalID)
                {
                    int pID = Convert.ToInt32(item);
                    var personalData = repoPQPersonal.GetTempPQPersonalDetailsForSendMailById(pID);
                    var loginDetail = repoCandidate.GetCandidateLoginDetailsForMailSendById(pID);
                    DataTable d = new DataTable();
                    StringBuilder docString = new StringBuilder();

                    if (ClientRowID == 1425 || ClientRowID == 1428 || ClientRowID == 1222)
                    {
                        d = cc.GetDocumentForVerificationList(pID, ClientRowID);
                        foreach (DataRow dr in d.Rows)
                        {
                            docString.Append("<tr><td style='padding:3px;'>" + dr["SubCheckName"].ToString() + " </td><td>" + dr["DocumentForVerification"].ToString() + " </td></tr>");
                        }
                    }
                    else if (ClientRowID == 193)
                    {
                        d = cc.GetDocumentForVerificationList(pID, ClientRowID);
                        foreach (DataRow dr in d.Rows)
                        {
                            docString.Append("<tr><td style='padding:3px;'>" + dr["SubCheckName"].ToString() + " </td><td>" + dr["DocumentForVerification"].ToString() + " </td></tr>");
                        }
                    }
                    else
                    {
                        d = cc.CandidateEmailDocFormat(pID); // it should be subcheck family based on special check, 
                        foreach (DataRow dr in d.Rows)
                        {
                            if (ClientRowID == 1357)
                            {
                                //---- Employment Check Start from here ----//
                                if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                    docString.Append("<tr><td style='padding:3px;'>Past two Employments </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");
                            }
                            else if (ClientRowID == 1425)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                    docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Any available document. Note: Two employment details prior to Chargebee.</ td></tr>");
                            }
                            else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                    docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Experience letter / Service Certificate</td></tr>");
                            }
                            else if (ClientRowID == 1428)
                            {

                            }
                            else if (ClientRowID == 1449)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                    docString.Append("<tr><td style='padding:3px;'>Employments </td><td>Experience letter, Service Certificate, Other</td></tr>");
                            }
                            else if (ClientRowID == 1481)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                    docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter</td></tr>");
                            }
                            else if (ClientRowID == 636)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                    docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Current Company Employment<br> • Experience Letter / Relieving Letter<br> • Salary Slip for last 3 months <br> Previous Employment(only latest 2 employers)<br> • Experience Letter / Relieving Letter </ td></tr>");
                            }
                            else
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "12") //Employment                            
                                    docString.Append("<tr><td style='padding:3px;'>Employment </td><td>Experience Letter / Relieving Letter / Salary Slip / Offer Letter </td></tr>");
                            }
                            //---- Employment Check End from here ----//

                            //---- Education Check Start from here ----//
                            if (ClientRowID == 1355 || ClientRowID == 1348)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                    docString.Append("<tr><td style='padding:3px;'>Education </td><td>Marksheet/Provisional Degree Certificate </td></tr>");
                            }
                            else if (ClientRowID == 1318)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                    docString.Append("<tr><td style='padding:3px;'>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");
                                if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                    docString.Append("<tr><td style='padding:3px;'>Sanad Certification </td><td>Required Bar Council Certificate</td></tr>");
                            }
                            else if (ClientRowID == 1425)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                    docString.Append("<tr><td style='padding:3px;'>Education </td><td>Transcript/Degree Certificate</td></tr>");
                            }
                            else if (ClientRowID == 1428)
                            {

                            }
                            else if (ClientRowID == 1449)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                    docString.Append("<tr><td style='padding:3px;'>Education </td><td>Transcript/ Degree Certificate/ Convocation Certificate</td></tr>");
                            }
                            else
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "11") //Education                            
                                    docString.Append("<tr><td style='padding:3px;'>Education </td><td>Degree Certificate / Final Year Mark sheet </td></tr>");
                            }
                            //---- Education Check End from here ----//

                            //---- Address Check Start from here ----//
                            if (ClientRowID == 1425)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                    docString.Append("<tr><td style='padding:3px;'>Address </td><td>Driver's License/Utility Bill</td></tr>");
                            }
                            if (ClientRowID == 1395)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                    docString.Append("<tr><td style='padding:3px;'>Address </td><td>SSN Number</td></tr>");
                            }
                            else if (ClientRowID == 1428)
                            {

                            }
                            else if (ClientRowID == 1449)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                    docString.Append("<tr><td style='padding:3px;'>Address </td><td>National ID Proof</td></tr>");
                            }
                            else if (ClientRowID == 636)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                    docString.Append("<tr><td style='padding:3px;'>Address </td><td>• Address Proof </td></tr>");
                            }
                            else
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "2") //address                            
                                    docString.Append("<tr><td style='padding:3px;'>Address </td><td>Address Proof for Current & Permanent Address </td></tr>");
                            }
                            //---- Address Check End from here ----//

                            //---- National Check Start from here ----//
                            if (ClientRowID == 1355 || ClientRowID == 1348)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "26")
                                    docString.Append("<tr><td>ID Verification</td><td>Passport preferred</td></tr>");
                            }
                            else if (ClientRowID == 1425 || ClientRowID == 1428 || ClientRowID == 1449)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "26")
                                    docString.Append("<tr><td>National ID Check </td><td>National ID Proof</td></tr>");
                            }
                            else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "26")
                                    docString.Append("<tr><td>National ID Check </td><td>Passport / National ID</td></tr>");
                            }
                            else if (ClientRowID == 559)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "26")
                                    docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Pan Card</td></tr>");
                            }
                            else if (ClientRowID == 636)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "26")
                                    docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>Photo ID Proof </td></tr>");
                            }
                            else if (ClientRowID == 648)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "26")
                                    docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>ID Proof – Pan Card and Aadhar Card</td></tr>");
                            }
                            else
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "26")
                                    docString.Append("<tr><td style='padding:3px;'>National ID Check </td><td>ID Proof </td></tr>");
                            }
                            //---- National Check End from here ----//

                            //---- Special Check Start from here ----//
                            if (ClientRowID == 487)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "40")
                                    docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>Required Aadhar Card</td></tr>");
                            }
                            else if (ClientRowID == 1470)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "40")
                                    docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>NA</td></tr>");
                            }
                            else if (ClientRowID == 193)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "40")
                                    docString.Append("<tr><td style='padding:3px;'>Global Database Check</td><td>NA</td></tr>");
                            }

                            //---- Special Check End from here ----//

                            //---- Criminal Check Start from here ----//
                            if (ClientRowID == 487 || ClientRowID == 1428 || ClientRowID == 1449)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "5")
                                    docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>National ID Proof</td></tr>");
                            }
                            else if (ClientRowID == 1395 || ClientRowID == 1414 || ClientRowID == 1415 || ClientRowID == 1416)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "5")
                                    docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>Passport / National ID</td></tr>");
                            }
                            else if (ClientRowID == 636)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "5")
                                    docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>• Address Proof </td></tr>");
                            }
                            else if (ClientRowID == 648)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "5")
                                    docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>Require – Address proof, ID Proof and Passport Size Photo</td></tr>");
                            }
                            else if (ClientRowID == 1470)
                            {
                                if (dr["CheckFamilyRowID"].ToString() == "5")
                                    docString.Append("<tr><td style='padding:3px;'>Criminal Check</td><td>National ID Proof</td></tr>");
                            }
                            //---- Criminal Check End from here ----//
                        }
                    }

                    if (loginDetail != null)
                    {
                        string body = string.Empty;
                        string bodySpocAndClient = string.Empty;
                        string SaveWPassMail = string.Empty;
                        DataTable tblExperiece = cc.CheckCandidateExperienced(personalData.ClientRowID, personalData.PersonalRowID);

                        string MailSubject = "";
                        string SpocAndClientMailSubject = "";
                        string MailTemplateName = "";
                        string MailTemplateNameSpoc = "";
                        int MailClientRowid = 0;
                        var templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplate");
                        if (templateDB.Rows.Count > 0)
                        {
                            body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                            MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                            MailTemplateName = Convert.ToString(templateDB.Rows[0]["TemplateName"]);
                            MailClientRowid = Convert.ToInt32(templateDB.Rows[0]["ClientRowId"]);
                        }
                        var templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "CandidateLoginDetailsTemplateForSpocAndClient");
                        if (templateDBSpocAndClient.Rows.Count > 0)
                        {
                            bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                            MailTemplateNameSpoc = Convert.ToString(templateDBSpocAndClient.Rows[0]["TemplateName"]);
                            SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                        }

                        if (ClientRowID == 679)
                        {
                            DataTable tblfractal = cc.CheckCandidateExperiencedByCandidateType(personalData.ClientRowID, personalData.PersonalRowID);
                            if (tblfractal.Rows.Count > 0)
                            {
                                if (tblfractal.Rows[0][0].ToString() == "Existing Employee")
                                {
                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployee");
                                    templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractalExitingEmployeeForSpocAndClient");
                                }
                                else
                                {
                                    //templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                    //templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                    DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                                    if (dtpackage.Rows.Count > 0)
                                    {
                                        if (dtpackage.Rows[0][0].ToString().Trim() == "FAPL_IAL2")
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 2926, 1, "FractelNewJoinertemp");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 2926, 1, "FractelNewJoinertempForSpocAndClient");
                                        }
                                        else
                                        {
                                            templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                            templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                        }
                                    }
                                    else
                                    {
                                        templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertemp");
                                        templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "FractelNewJoinertempForSpocAndClient");
                                    }
                                }
                            }
                            if (templateDB.Rows.Count > 0)
                            {
                                body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                                if (ClientRowID == 679 || ClientRowID == 2)
                                {
                                    SaveWPassMail = Convert.ToString(templateDB.Rows[0]["WLDMailTemplate"]);
                                }
                            }
                            if (templateDBSpocAndClient.Rows.Count > 0)
                            {
                                bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                            }
                        }

                        else if (ClientRowID == 576)
                        {
                            //---------Experienced-----------
                            if (tblExperiece.Rows.Count > 0)
                            {
                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LateralMailNagarro");
                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LateralMailNagarroForSpocAndClient");
                            }
                            else
                            {
                                templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "NagarroFresherMail");
                                templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "NagarroFresherMailForSpocAndClient");
                            }
                            if (templateDB.Rows.Count > 0)
                            {
                                body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                            }
                            if (templateDBSpocAndClient.Rows.Count > 0)
                            {
                                bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                            }
                        }
                        else if (ClientRowID == 635) //Tata Elxsi
                        {

                            DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                            if (dtpackage.Rows.Count > 0)
                            {
                                if (dtpackage.Rows[0][0].ToString() == "TEL_Experienced (ID_CRCP_CRCC_PADD_CADD_GDB_2EMP")
                                {
                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 671, 1, "LoginDetailsSendToCandidateFotTataElxsiExp");
                                    templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiExpSpocAndClient");
                                }
                                else
                                {
                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresher");
                                    templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateFotTataElxsiFresherSpocAndClient");
                                }
                            }
                            if (templateDB.Rows.Count > 0)
                            {
                                body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                            }
                            if (templateDBSpocAndClient.Rows.Count > 0)
                            {
                                bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                            }
                        }


                        else if (ClientRowID == 647)
                        {

                            DataTable dtpackage = cc.GetPackageNameByCandidate(personalData.PersonalRowID);
                            if (dtpackage.Rows.Count > 0)
                            {
                                if (dtpackage.Rows[0][0].ToString() == "NNSCIPL_Experienced (EDU_2EMP_PADD_GDB_CRCP_ID)")
                                {
                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 737, 1, "LoginDetailsSendToCandidateForNovoNordiskExp");
                                    templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskExpForSpocAndClient");
                                }
                                else
                                {
                                    templateDB = cc.getAlertMailtemplate(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresher");
                                    templateDBSpocAndClient = cc.getAlertMailtemplateForSpocAndClient(ClientRowID, "Login Details", 0, 1, "LoginDetailsSendToCandidateForNovoNordiskFresherForSpocAndClient");
                                }
                            }
                            if (templateDB.Rows.Count > 0)
                            {
                                body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                bodySpocAndClient = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                                MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                            }
                            if (templateDBSpocAndClient.Rows.Count > 0)
                            {
                                bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                                SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);
                            }
                        }

                        if (ClientRowID == 103)
                        {
                            body = body.Replace("{Remarks}", "Please login and fill the details within 3 days");
                            bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Please login and fill the details within 3 days");
                        }
                        else if (ClientRowID == 1530)
                        {
                            body = body.Replace("{Remarks}", "Kindly fill the form within 2 working days");
                            bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Kindly fill the form within 2 working days");
                        }
                        else
                        {
                            body = body.Replace("{Remarks}", "Please login and fill the details within 2 days");
                            bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", "Please login and fill the details within 2 days");
                        }

                        string txtFolderPath = ConfigurationManager.AppSettings["mailimages"].ToString();
                        LinkedResource Triangleres = new LinkedResource(Server.MapPath(txtFolderPath) + "Triangle.png", MediaTypeNames.Image.Jpeg);
                        Triangleres.ContentId = Guid.NewGuid().ToString();
                        string triangle = @"<img src='cid:" + Triangleres.ContentId + @"' width='25%'/>";

                        LinkedResource Securitassec = new LinkedResource(Server.MapPath(txtFolderPath) + "Securitas-logo.png", MediaTypeNames.Image.Jpeg);
                        Securitassec.ContentId = Guid.NewGuid().ToString();
                        string securitas = @"<img src='cid:" + Securitassec.ContentId + @"' width='25%'/>";

                        LinkedResource Walsonssec = new LinkedResource(Server.MapPath(txtFolderPath) + "Walsons-Temp-img.JPG", MediaTypeNames.Image.Jpeg);
                        Walsonssec.ContentId = Guid.NewGuid().ToString();
                        string walsons = @"<img src='cid:" + Walsonssec.ContentId + @"' width='25%'/>";

                        LinkedResource BGVdottedImg = new LinkedResource(Server.MapPath(txtFolderPath) + "BGV-dotted-design.png", MediaTypeNames.Image.Jpeg);
                        BGVdottedImg.ContentId = Guid.NewGuid().ToString();
                        string BGVdotted = @"<img src='cid:" + BGVdottedImg.ContentId + @"' width='100%'/>";

                        LinkedResource RedbulllogoImg = new LinkedResource(Server.MapPath(txtFolderPath) + "Redbullogo.png", MediaTypeNames.Image.Jpeg);
                        RedbulllogoImg.ContentId = Guid.NewGuid().ToString();
                        string redbulllogo = @"<img src='cid:" + RedbulllogoImg.ContentId + @"' width='30%'/>";

                        LinkedResource JadeGlobalLogoIMG = new LinkedResource(Server.MapPath(txtFolderPath) + "JadeGlobalLogo.png", MediaTypeNames.Image.Jpeg);
                        JadeGlobalLogoIMG.ContentId = Guid.NewGuid().ToString();
                        string JadeGlobalLogo = @"<img src='cid:" + JadeGlobalLogoIMG.ContentId + @"' width='30%'/>";

                        string ClientNameMail = Session["ClientNameMail"].ToString();
                        //Candidate Template Details
                        body = body.Replace("{document}", docString.ToString());
                        body = body.Replace("{CandidateName}", loginDetail.CandidateName);
                        body = body.Replace("#CompanyName", ClientNameMail);
                        body = body.Replace("{ClientRefId}", personalData.ClientRefID);
                        body = body.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                        body = body.Replace("{UName}", loginDetail.UserID);
                        body = body.Replace("{UPasswd}", clsCommonMethods.PasswordDecrypt(loginDetail.UPass.Trim()));
                        body = body.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                        body = body.Replace("{triangle}", triangle);
                        body = body.Replace("{securitas}", securitas);
                        body = body.Replace("{walsons}", walsons);
                        body = body.Replace("{BGVdotted}", BGVdotted);
                        body = body.Replace("{redbulllogo}", redbulllogo);
                        body = body.Replace("{JadeLogo}", JadeGlobalLogo);
                        body = body.Replace("{MailSignature}", MailSignature);

                        //Spoc And Client Template Details
                        var Resendlinks = "https://bgv.securitas-india.com/account/ResendLink/?PersonalIds=" + pID.ToString();
                        bodySpocAndClient = bodySpocAndClient.Replace("{Resendlink}", Resendlinks);
                        bodySpocAndClient = bodySpocAndClient.Replace("{document}", docString.ToString());
                        bodySpocAndClient = bodySpocAndClient.Replace("{CandidateName}", loginDetail.CandidateName);
                        bodySpocAndClient = bodySpocAndClient.Replace("#CompanyName", ClientNameMail);
                        bodySpocAndClient = bodySpocAndClient.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                        bodySpocAndClient = bodySpocAndClient.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());
                        bodySpocAndClient = bodySpocAndClient.Replace("{triangle}", triangle);
                        bodySpocAndClient = bodySpocAndClient.Replace("{securitas}", securitas);
                        bodySpocAndClient = bodySpocAndClient.Replace("{walsons}", walsons);
                        bodySpocAndClient = bodySpocAndClient.Replace("{BGVdotted}", BGVdotted);
                        bodySpocAndClient = bodySpocAndClient.Replace("{redbulllogo}", redbulllogo);
                        bodySpocAndClient = bodySpocAndClient.Replace("{JadeLogo}", JadeGlobalLogo);
                        bodySpocAndClient = bodySpocAndClient.Replace("{MailSignature}", MailSignature);

                        AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                        alternateView.LinkedResources.Add(Triangleres);
                        alternateView.LinkedResources.Add(Securitassec);
                        alternateView.LinkedResources.Add(Walsonssec);
                        alternateView.LinkedResources.Add(BGVdottedImg);
                        alternateView.LinkedResources.Add(RedbulllogoImg);
                        alternateView.LinkedResources.Add(JadeGlobalLogoIMG);

                        AlternateView alternateViewSpocAndClient = AlternateView.CreateAlternateViewFromString(bodySpocAndClient, null, MediaTypeNames.Text.Html);
                        alternateViewSpocAndClient.LinkedResources.Add(Triangleres);
                        alternateViewSpocAndClient.LinkedResources.Add(Securitassec);
                        alternateViewSpocAndClient.LinkedResources.Add(Walsonssec);
                        alternateViewSpocAndClient.LinkedResources.Add(BGVdottedImg);
                        alternateViewSpocAndClient.LinkedResources.Add(RedbulllogoImg);
                        alternateViewSpocAndClient.LinkedResources.Add(JadeGlobalLogoIMG);

                        if (ClientRowID == 2)
                        {
                            string key = loginDetail.CandidateUserRowID + "_" + personalData.ClientRowID + "_" + personalData.PersonalRowID;
                            body = body.Replace("{MediaLink}", ConfigurationManager.AppSettings["CandidateMediaUrl"].ToString() + "?Key=" + clsCommonMethods.PasswordEncrypt(key));

                        }
                        string Subject = "";
                        // For Sutherland Global  249,277
                        if (ClientRowID == 249 || ClientRowID == 277)
                        {
                            Subject = loginDetail.CandidateName + "  Sutherland Background Verification by Securitas";
                            DisplayName = "Sutherland BGV by Securitas";
                        }
                        // For TATA Elxsi
                        else if (ClientRowID == 635)
                        {
                            body = body.Replace("{document}", "Documents Required<br /> Education: Degree Certificate / Final Year Marksheet < br /> " +
                                "Address: Address Proof < br />   Nationality: ID Proof");
                            Subject = "Background verification";
                        }
                        else
                        {
                            Subject = loginDetail.CandidateName + "  Background Verification";
                        }
                        string To = loginDetail.CandidateEmail;
                        if (ClientRowID == 249 || ClientRowID == 277 || ClientRowID == 193)
                        {
                            Cc = Cc + ";" + personalData.AlternateEmail;
                        }

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
                        DataTable Dt = cc.getCandidateMailstatus(pID.ToString());

                        int countSent = 0, countFailed = 0, totRec = 0;
                        bool flag = false;
                        bool flagspocandclient = false;
                        if (Dt.Rows.Count > 0)
                        {
                            try
                            {
                                if (Convert.ToString(personalData.MobileNo) != "")
                                {
                                    MessageStatus objMessage = new MessageStatus();
                                    WhatsAppAPI objs = new WhatsAppAPI();
                                    var link = "https://bgv.securitas-india.com/Account/CandidateLogin";
                                    objMessage = objs.SendWhatsAppMessage("+91" + personalData.MobileNo, "case_registration_candidate_invite_mode", loginDetail.CandidateName, "media_template", ClientNameMail, To, link);
                                    cc.sp_AddAuditWhatsAppAPI(personalData.PersonalRowID, objMessage.message, objMessage.status, "Candidate Invite");
                                }
                            }
                            catch
                            {

                            }
                            totRec = Dt.Rows.Count;
                            for (int i = 0; i < totRec; i++)
                            {
                                if (Dt.Rows[0]["SentMailStatus"].ToString() == "0")
                                {
                                    if (ClientRowID == 503)
                                    {
                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                        flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                    }
                                    else
                                    {
                                        if (ClientRowID == 257) //Biz 2 Credit Info Services Pvt. Ltd
                                        {
                                            string bcc = "";
                                            string Ccc = "";
                                            Ccc = spocEmail;
                                            bcc = cSpocEmail;
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                            flag = mail.SendMail(DisplayName, From, ToMail, Ccc, bcc, Subject, bodySpocAndClient);
                                            //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, Ccc, bcc,Subject, bodySpocAndClient, null, null);
                                        }
                                        else if (ClientRowID == 249)//sutherlandglobal 
                                        {
                                            String CcSuthermail = "brij.bhushan@securitas-india.com";

                                            CcSuthermail = CcSuthermail + "," + Ccmail;

                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, CcSuthermail, Subject, bodySpocAndClient, attachment, null);
                                        }
                                        // For TATA Elxsi
                                        else if (ClientRowID == 635)
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);

                                        }
                                        else if (ClientRowID == 957)
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                        }
                                        else if (ClientRowID == 1312)
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                        }
                                        else if (ClientRowID == 778)
                                        {

                                            String CcHHS = personalData.BusinessName;
                                            if (CcHHS == null)
                                            {
                                                CcHHS = Ccmail.ToString();
                                            }
                                            else
                                            {
                                                CcHHS = CcHHS + "," + Ccmail;
                                            }
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, CcHHS, Subject, bodySpocAndClient, attachment, null);

                                        }
                                        else if (ClientRowID == 1005 || ClientRowID == 2)
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);

                                        }
                                        else if (ClientRowID == 889)
                                        {
                                            flag = mail.SendMailPurgWithoutCC(DisplayName, From, ToMail, Subject, body, attachment);
                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, cSpocEmail, Subject, bodySpocAndClient, attachment, null);
                                        }
                                        else if (ClientRowID == 974)
                                        {
                                            flag = mail.SendMailAdidas(DisplayName, From, ToMail, Subject, body);
                                            flagspocandclient = mail.SendMailAdidasForSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, body);
                                        }
                                        else if ((ClientRowID == 1496 || MailClientRowid == 2 || MailClientRowid == 193) && (MailTemplateName == "CandidateLoginDetailsTemplate" || MailTemplateNameSpoc == "CandidateLoginDetailsTemplate"))
                                        {
                                            flag = mail.CandidateLoginSendMail(DisplayName, From, ToMail, Subject, body, attachment, alternateView);
                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, alternateViewSpocAndClient);
                                        }
                                        else if (ClientRowID == 1481)
                                        {
                                            DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                            var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                            if (recuiteremail != "")
                                            {
                                                Ccmail += "," + recuiteremail;
                                            }
                                            flag = mail.SendMailToCandidateForCtrls(DisplayName, From, ToMail, Subject, body, attachment, attachment2);
                                            flagspocandclient = mail.SendMailToCandidateForCtrlsForSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, attachment2);
                                        }
                                        else
                                        {
                                            //=====================================
                                            if (ClientRowID == 636)
                                            {
                                                DataTable dto = cc.GetRecuiterEmailIdbyClient1(personalData.PersonalRowID, ClientRowID);
                                                var recuiteremail = dto.Rows[0]["RecuriterEmail"].ToString();
                                                if (recuiteremail != "")
                                                {
                                                    Ccmail += "," + recuiteremail;
                                                }
                                            }
                                            //===========================================
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                            //flagspocandclient = mail.CandidateLoginSendMailToSpocAndClientWithoutAlternateView(DisplayName, From, Ccmail, "", Subject, bodySpocAndClient, attachment);
                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, attachment, null);
                                        }
                                    }

                                    if (flag)
                                    {
                                        if (ClientRowID == 249 || ClientRowID == 277) //sutherlandglobal and Sutherland Healthcare Solutions Pvt. Ltd.
                                        {
                                            try
                                            {
                                                string Message = "";
                                                Message = "Dear " + personalData.Name + Environment.NewLine + Environment.NewLine;
                                                Message += "Please complete Sutherland BGV on email sent to your ID or below link." + Environment.NewLine;
                                                Message += "ID:" + loginDetail.UserID + Environment.NewLine;
                                                Message += "Pwd:" + clsCommonMethods.PasswordDecrypt(loginDetail.UPass) + Environment.NewLine;
                                                Message += "Link:" + ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString() + Environment.NewLine;
                                                Message += "Regards" + Environment.NewLine;
                                                Message += "Securitas";
                                                loginDetail.SMSText = Message;
                                                loginDetail.PersonalId = pID;
                                                SendSMS(loginDetail);
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        if (ClientRowID == 635)
                                        {
                                            cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                            cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                        }
                                        else if (ClientRowID == 636)
                                        {
                                            cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                            cc.saveCandidatemail(pID, "Followup mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                            cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                        }
                                        else if (ClientRowID == 957)
                                        {
                                            cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                            cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                        }
                                        else if (ClientRowID == 1209)
                                        {
                                            DataTable dt = cc.GetCandidateClient(ToMail, pID);
                                            if (dt.Rows.Count > 0)
                                            {
                                                int clientrowidnew = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                                                cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, clientrowidnew, 1);
                                                cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                            }
                                        }
                                        else if (ClientRowID == 1005 || ClientRowID == 2 || ClientRowID == 889)
                                        {
                                            SaveWPassMail = SaveWPassMail.Replace("Click the below Url, login", "");//.Replace("Candidate Login", "").Replace("Click Reference ID:{ClientRefId}", "").Replace("Login Id: {UName}", "").Replace("Password: {UPasswd}", "");
                                            SaveWPassMail = SaveWPassMail.Replace("into the website", "");
                                            SaveWPassMail = SaveWPassMail.Replace("Candidate Login", "");
                                            SaveWPassMail = SaveWPassMail.Replace("Click Reference ID:{ClientRefId}", "");

                                            SaveWPassMail = SaveWPassMail.Replace("Password: {XXXXX}", "");

                                            SaveWPassMail = SaveWPassMail.Replace("Login Id: {XXXXX}", "");

                                            SaveWPassMail = SaveWPassMail.Replace("{document}", docString.ToString());
                                            SaveWPassMail = SaveWPassMail.Replace("{CandidateName}", loginDetail.CandidateName);
                                            SaveWPassMail = SaveWPassMail.Replace("#CompanyName", ClientNameMail);
                                            SaveWPassMail = SaveWPassMail.Replace("{ClientRefId}", personalData.ClientRefID);
                                            SaveWPassMail = SaveWPassMail.Replace("{ExpireDate}", DateTime.Now.AddDays(7).ToString());
                                            SaveWPassMail = SaveWPassMail.Replace("{UName}", "XXXXX");
                                            SaveWPassMail = SaveWPassMail.Replace("{UPasswd}", "XXXXX");
                                            SaveWPassMail = SaveWPassMail.Replace("{CandidateLoginUrl}", ConfigurationManager.AppSettings["CandidateLoginUrl"].ToString());

                                            SaveWPassMail = SaveWPassMail.Replace("{MailSignature}", MailSignature);

                                            cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                            cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);
                                        }
                                        else
                                        {
                                            string Message = $"Dear {loginDetail.CandidateName} We kindly request you to upload the required document in the application, using the login credentials sent on your email. Feel free to connect in case of any assistance required on mentioned contact details on email. Thank you Walsons Services Pvt. Ltd.";
                                            loginDetail.PersonalId = pID;
                                            loginDetail.SMSText = Message;
                                            SendSMS(loginDetail);
                                            cc.saveCandidatemail(pID, "Candidate mail", ToMail, Ccmail, Subject, bodySpocAndClient, ClientRowID, 1);
                                            cc.saveCandidatemail(pID, "Candidate Invite Mail", ToMail, "", Subject, body, ClientRowID, 1);

                                        }

                                        if (ClientRowID == 635)
                                        {
                                            //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                            RemindermailTataElxsi(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                        }

                                        if (ClientRowID == 957)
                                        {
                                            RemindermailAstraZenca(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);

                                        }

                                        if (ClientRowID == 679 || ClientRowID == 725)
                                        {
                                            //flag = mail.SendMail(DisplayName, From, ToMail, Ccmail,  Subject, body);
                                            SaveReminderMail(personalData, loginDetail, pID, ToMail, Ccmail, Subject, ClientRowID, mail, DisplayName, From);
                                        }
                                        UpdatePQCandidateLoginForSendMailViewModel loginModel = new UpdatePQCandidateLoginForSendMailViewModel();
                                        loginModel.CandidateUserRowID = loginDetail.CandidateUserRowID;
                                        loginModel.SentMailStatus = 1;
                                        loginModel.SentMailDate = DateTime.Now;
                                        repoCandidate.UpdateCandidateLoginSendMailStatusById(loginModel);
                                        countSent++;
                                    }
                                    else
                                    {
                                        countFailed++;
                                    }
                                }

                            }

                        }
                        TempData["ErrorMsg"] = "Out of " + totRec.ToString() + " Mail sent : " + countSent.ToString() + " failed : " + countFailed.ToString();

                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Candidate login details not found!";
                    }
                }
            }
            #endregion
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

        private int SaveClientCandidateUploadFileWithChecks(short ClientRowID = 0, string FileName = "", string CandidateCheckOrPackage = "")
        {
            try
            {

                int res = 0;
                AddPQClientCandiBulkUploadViewModel objModel = new AddPQClientCandiBulkUploadViewModel();
                objModel.ClientRowID = ClientRowID;
                objModel.ExcelFileName = FileName;
                objModel.ImportBy = "Client";
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

        //Save Temp Package Or Check for Candidate
        void SaveTempPackageOrCheckWithCandidate(AddTempPQPersonalForBulkUpViewModel model)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                AddTempPQCandidateCheckViewModel chekModel = new AddTempPQCandidateCheckViewModel();
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
                                chekModel.ClientCheckRowID = item.ClientCheckRowID;
                                //repoPQCandidateCheck.AddTempCandidateCheck(chekModel);
                                //repoPQCandidateCheck.SaveChanges();
                                cc.TempCandidateCheckInsert(chekModel, item.CheckFamilyRowID, chekModel.ClientCheckRowID);
                                //cc.CandidateCheckMoveToPQCandidate(chekModel.ClientRowID, chekModel.PersonalRowID, chekModel.ClientCheckRowID, chekModel.ClientPackageRowID);                                
                            }
                        }
                        if (strIds[0] == "Package")
                        {
                            string pkgnm = ChkPkg.Replace(strIds[0] + "-", "");
                            var pkgDetails = repoClientPackage.GetPackageDetailsByPackageName(chekModel.ClientRowID.ToString(), pkgnm);


                            //============================================
                            if (model.ClientRowID == 797 || model.ClientRowID == 791 || model.ClientRowID == 792 || model.ClientRowID == 793 || model.ClientRowID == 794 || model.ClientRowID == 795 || model.ClientRowID == 796)
                            {
                                if (model.Designation != null && model.Designation != "")
                                {
                                    if (model.Entity == "Ambit Finvest Private Limited")
                                    {
                                        if (model.Designation.Replace(" ", "").ToLower() == "seniorsalesmanager" || model.Designation.Replace(" ", "").ToLower() == "salesmanager" || model.Designation.Replace(" ", "").ToLower() == "salesofficer" || model.Designation.Replace(" ", "").ToLower() == "seniorsalesofficer")
                                        {
                                            pkgDetails = pkgDetails.Where(x => x.CheckFamilyRowID != 11).ToList();

                                        }

                                    }

                                }

                            }


                            //===========================================


                            chekModel.ClientCheckRowID = 0;
                            foreach (var item in pkgDetails)
                            {
                                chekModel.ClientPackageRowID = item.ClientPackageRowID;
                                //chekModel.ClientCheckRowID = item.SubCheckRowID;
                                //repoPQCandidateCheck.AddTempCandidateCheck(chekModel);
                                //repoPQCandidateCheck.SaveChanges();
                                cc.TempCandidateCheckInsert(chekModel, item.CheckFamilyRowID, chekModel.ClientCheckRowID);
                                //cc.CandidateCheckMoveToPQCandidate(chekModel.ClientRowID, chekModel.PersonalRowID, chekModel.ClientCheckRowID, chekModel.ClientPackageRowID);
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

        private string GetCandidateCode(short id = 0)
        {
            try
            {

                if (id > 0)
                {
                    string CandidateCode = string.Empty;
                    string str1 = Session["Abbreviation"].ToString();
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
                    return CandidateCode;
                }
                return "";

            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        void BindClientNameSubGrup()
        {
            try
            {
                ViewBag.Client = repoDDL.GetClientListWithAbbAndLocationForDDL();
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
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
        public void BindTypeList()
        {
            try
            {
                IEnumerable<SelectListItem> TypeNameList = new List<SelectListItem>
            {
            new SelectListItem { Text = "Non-Expire ", Value = "1"},
            new SelectListItem { Text = "Expire ", Value = "0"},

            };
                ViewBag.AddTypeList = TypeNameList;
            }
            catch (Exception Ex)
            {

                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        public void BindYTRApplicableList()
        {
            try
            {
                IEnumerable<SelectListItem> YTRApplicableList = new List<SelectListItem>
            {
            new SelectListItem { Text = "Not-Applicable ", Value = "0"},
            new SelectListItem { Text = "Applicable ", Value = "1"},

            };
                ViewBag.AddYTRApplicableList = YTRApplicableList;
            }
            catch (Exception Ex)
            {

                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        #endregion


        // New
        public static string SendSMS(PQCandidateLoginForSendMailViewModel model)
        {
            string responseString = "";
            try
            {

                string username = "20200611";
                string pass = "E0QjcMa9";
                string senderId = "BGVSEC";

                string message = HttpUtility.UrlEncode(model.SMSText);
                string destination = model.CantactNo;
                //Prepare you post parameters  
                StringBuilder sbPostData = new StringBuilder();
                sbPostData.AppendFormat("uname={0}", username);
                sbPostData.AppendFormat("&pass={0}", pass);
                sbPostData.AppendFormat("&msg={0}", message);
                sbPostData.AppendFormat("&send={0}", senderId);
                sbPostData.AppendFormat("&dest={0}", destination);


                //Call Send SMS API  
                string sendSMSUri = "https://prpsms.co.in/API/SendMsg.aspx";
                //Create HTTPWebrequest  
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data  
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method  
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response  
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();

                //Close the response  
                reader.Close();

                response.Close();
                ClientCustomize cc = new ClientCustomize();
                cc.saveCandidatemail(model.PersonalId, "SMS", destination, "", "Candidate SMS", model.SMSText, model.ClientRowID, 1);


            }
            catch
            {
            }
            return responseString;

        }


        public void CandidateUploadTemplate()
        {
            //string FileName = "CandidateUploadTemplate.xlsx";
            int ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
            string FileName = "";
            if (ClientRowID == 576)
            {
                FileName = "CandidateUploadTemplate_Nagarrow.xlsx";
            }
            else if (ClientRowID == 858)
            {
                FileName = "CandidateUploadTemplatePatra.xlsx";
            }
            else if (ClientRowID == 1193)
            {
                FileName = "CandidateUploadTemplateTCSION.xlsx";
            }
            else if (ClientRowID == 722)
            {
                FileName = "CandidateUploadTemplate_Genpact.xlsx";
            }
            else if (ClientRowID == 1057)
            {
                FileName = "CandidateUploadTemplate _CTSIL_UAN.xlsx";
            }
            else if (ClientRowID == 636)
            {
                FileName = "CandidateUploadTemplate_IDEMIA.xlsx";
            }
            else if (ClientRowID == 193)
            {
                FileName = "JadeGlobalCandidateUploadTemplate.xlsx";
            }
            else
            {
                FileName = "CandidateUploadTemplate.xlsx";

            }

            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["clientCanidatetemplate"].ToString();
            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=CandidateUploadTemplate.xlsx");
            Response.TransmitFile(Server.MapPath(FldPathDoc) + FileName);
            Response.End();
            //return Redirect("~/Documents/CandidateDocuments/CandidateManual/ClientManual.pdf");
        }



        [HttpGet]
        [SessionExpireFilterAttributeClient]
        [ValidateInput(false)]
        public ActionResult UploadHindCandidatedoc(string pid = "")
        {
            AddTempPQVerifiedUploadDocViewModel model = new AddTempPQVerifiedUploadDocViewModel();
            int Epid = 0;
            if (pid != "")
            {
                Epid = Convert.ToInt32(clsCommonMethods.PDecrypt(pid));
            }
            try
            {
                model.PersonalRowID = Epid;
                //GetAllCheckListforDocUpload(model.PersonalRowID);
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttributeClient]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> UploadHindCandidatedoc(AddTempPQVerifiedUploadDocViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                ViewBag.ErrorMsg = "";
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    List<CheckDetailModel> objData = GetAllCheckListforHindujaDocUpload(model.PersonalRowID);
                    if (model.AAdharCard != null)
                    {
                        string errorMessage = await GatZipFileMessage(model.UploadDoc, model.PersonalRowID.ToString(), "AAdharCard");
                        if (errorMessage != "")
                        {
                            ViewBag.ErrorMsg += errorMessage;
                        }
                        else
                        {

                            if (model.PersonalRowID > 0 && model.AAdharCard.ContentLength > 0)
                            {
                                TempPQAddressViewModel tempEmp = new TempPQAddressViewModel();
                                tempEmp.CreatedDate = DateTime.Now;
                                tempEmp.CreatedBy = Convert.ToInt32(Session["CandidateUserRowID"]);
                                model.ClientRowID = Convert.ToInt16(Session["ClientRowid"]);
                                PQClientCodeHolidaysViewModel client = repoPQClient.GetPQClientCodeHolidaysById(model.ClientRowID);
                                #region Generate UniqueComponentId                          

                                var checks = objData.Where(T => T.CheckFamilyRowID == 2);
                                if (checks.Count() > 0)
                                {
                                    foreach (var str in checks)
                                    {
                                        tempEmp.CheckFamilyRowID = str.CheckFamilyRowID;
                                        tempEmp.SubCheckRowID = str.SubCheckRowID;
                                        tempEmp.ClientPackageRowID = str.ClientPackageRowID;
                                        tempEmp.Status = 1;
                                        tempEmp.AV_OtherDetails10 = model.AadharNo;
                                        int TotalRecord = repoEmploymentInfo.GetAllTempEmploymentByClientId(model.ClientRowID);
                                        string clCode = client.ClientCode1.Substring(0, 3).ToUpper();
                                        TotalRecord++;
                                        tempEmp.UniqueComponentID = string.Concat(clCode, "-ADD-", DateTime.Now.ToString("yyyyMMdd"), "-", Convert.ToString(TotalRecord));
                                        tempEmp.PersonalRowID = model.PersonalRowID;
                                        tempEmp.ClientRowID = model.ClientRowID;
                                        tempEmp.CheckStatus = "DEC";
                                        DataTable dt = cc.getTempcheckDetailCount(model.PersonalRowID, tempEmp.SubCheckRowID, "Address");
                                        //if (model.SpecialCheckRowId > 0 && Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        if (Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        {
                                            //repoAddressInfo.UpdateTempAddressInformation(tempEmp);
                                            //int Res = repoAddressInfo.SaveChanges();

                                        }
                                        else
                                        {
                                            repoAddressInfo.AddTempAddressInformation(tempEmp);
                                            int Res = repoAddressInfo.SaveChanges();
                                        }
                                        model.Status = 1;
                                        SavePQVerifiedUploadDoc(model.PersonalRowID, tempEmp.SubCheckRowID, "Provided", "AadhaarCard", "provideddocuments", model.AAdharCard, "");

                                    }
                                }
                                #endregion
                                //SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "Provided", "AadhaarCard", "provideddocuments", model.AAdharCard, "");
                            }
                        }
                    }

                    if (model.PanCard != null)
                    {
                        string errorMessage = await GatZipFileMessage(model.UploadDoc, model.PersonalRowID.ToString(), "PanCard");
                        if (errorMessage != "")
                        {
                            ViewBag.ErrorMsg += errorMessage;
                        }
                        else
                        {
                            if (model.PersonalRowID > 0 && model.PanCard.ContentLength > 0)
                            {
                                AddTempSpecialCheckViewModel tempEmp = new AddTempSpecialCheckViewModel();
                                tempEmp.CreatedDate = DateTime.Now;
                                tempEmp.CreatedBy = Convert.ToInt32(Session["CandidateUserRowID"]);
                                model.ClientRowID = Convert.ToInt16(Session["ClientRowid"]);
                                PQClientCodeHolidaysViewModel client = repoPQClient.GetPQClientCodeHolidaysById(model.ClientRowID);
                                #region Generate UniqueComponentId                          

                                var checks = objData.Where(T => T.CheckFamilyRowID == 40 && T.SubCheckRowID == 200);
                                if (checks.Count() > 0)
                                {
                                    foreach (var str in checks)
                                    {
                                        tempEmp.CheckFamilyRowID = str.CheckFamilyRowID;
                                        tempEmp.SubCheckRowID = str.SubCheckRowID;
                                        tempEmp.ClientPackageRowID = str.ClientPackageRowID;
                                        tempEmp.Status = 1;
                                        tempEmp.SC_DOB = Convert.ToDateTime(model.DOB);
                                        int TotalRecord = repoEmploymentInfo.GetAllTempEmploymentByClientId(model.ClientRowID);
                                        string clCode = client.ClientCode1.Substring(0, 3).ToUpper();
                                        TotalRecord++;
                                        tempEmp.UniqueComponentID = string.Concat(clCode, "-SPC-", DateTime.Now.ToString("yyyyMMdd"), "-", Convert.ToString(TotalRecord));
                                        tempEmp.PersonalRowID = model.PersonalRowID;
                                        tempEmp.ClientRowID = model.ClientRowID;
                                        tempEmp.CheckStatus = "DEC";
                                        DataTable dt = cc.getTempcheckDetailCount(model.PersonalRowID, tempEmp.SubCheckRowID, "Special");
                                        //if (model.SpecialCheckRowId > 0 && Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        if (Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        {
                                            //repoSplCheckInfo.UpdateTempSpecialInfoByCandidate(tempEmp);
                                            //int Res = repoSplCheckInfo.SaveChanges();

                                        }
                                        else
                                        {
                                            repoSplCheckInfo.AddTempSpacelCheckInformation(tempEmp);
                                            int Res = repoSplCheckInfo.SaveChanges();
                                        }

                                        model.Status = 1;
                                        SavePQVerifiedUploadDoc(model.PersonalRowID, tempEmp.SubCheckRowID, "Provided", "PANCard", "provideddocuments", model.PanCard, "");

                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    if (model.EducationProof != null)
                    {
                        string errorMessage = await GatZipFileMessage(model.UploadDoc, model.PersonalRowID.ToString(), "EducationProof");
                        if (errorMessage != "")
                        {
                            ViewBag.ErrorMsg += errorMessage;
                        }
                        else
                        {
                            if (model.PersonalRowID > 0 && model.EducationProof.ContentLength > 0)
                            {
                                if (model.PersonalRowID > 0 && model.EducationProof.ContentLength > 0)
                                {
                                    AddTempPQEducationViewModel tempEmp = new AddTempPQEducationViewModel();
                                    tempEmp.CreatedDate = DateTime.Now;
                                    tempEmp.CreatedBy = Convert.ToInt32(Session["CandidateUserRowID"]);

                                    model.ClientRowID = Convert.ToInt16(Session["ClientRowid"]);
                                    PQClientCodeHolidaysViewModel client = repoPQClient.GetPQClientCodeHolidaysById(model.ClientRowID);
                                    #region Generate UniqueComponentId  
                                    model.Status = 1;
                                    var checks = objData.Where(T => T.CheckFamilyRowID == 11);
                                    if (checks.Count() > 0)
                                    {
                                        foreach (var str in checks)
                                        {
                                            tempEmp.CheckFamilyRowID = str.CheckFamilyRowID;
                                            tempEmp.SubCheckRowID = str.SubCheckRowID;
                                            tempEmp.ClientPackageRowID = str.ClientPackageRowID;
                                            tempEmp.Status = 1;
                                            tempEmp.EV_Division_CGPA = model.Division;
                                            int TotalRecord = repoEmploymentInfo.GetAllTempEmploymentByClientId(model.ClientRowID);
                                            string clCode = client.ClientCode1.Substring(0, 3).ToUpper();
                                            TotalRecord++;
                                            tempEmp.UniqueComponentID = string.Concat(clCode, "-EDU-", DateTime.Now.ToString("yyyyMMdd"), "-", Convert.ToString(TotalRecord));
                                            tempEmp.PersonalRowID = model.PersonalRowID;
                                            tempEmp.ClientRowID = model.ClientRowID;
                                            tempEmp.CheckStatus = "DEC";



                                            DataTable dt = cc.getTempcheckDetailCount(model.PersonalRowID, tempEmp.SubCheckRowID, "Education");
                                            //if (model.SpecialCheckRowId > 0 && Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                            if (Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                            {
                                                //repoEducationInfo.UpdateTempEducationInfoByself(tempEmp);
                                                //int Res = repoEducationInfo.SaveChanges();

                                            }
                                            else
                                            {
                                                repoEducationInfo.AddTempEducationInformation(tempEmp);
                                                int Res = repoEducationInfo.SaveChanges();
                                            }
                                            SavePQVerifiedUploadDoc(model.PersonalRowID, tempEmp.SubCheckRowID, "Provided", "Degree_Certificate", "provideddocuments", model.EducationProof, "");

                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }

                    if (model.BankAccountProof != null)
                    {
                        string errorMessage = await GatZipFileMessage(model.UploadDoc, model.PersonalRowID.ToString(), "BankAccountProof");
                        if (errorMessage != "")
                        {
                            ViewBag.ErrorMsg += errorMessage;
                        }
                        else
                        {
                            if (model.PersonalRowID > 0 && model.BankAccountProof.ContentLength > 0)
                            {
                                AddTempSpecialCheckViewModel tempEmp = new AddTempSpecialCheckViewModel();
                                tempEmp.CreatedDate = DateTime.Now;
                                tempEmp.CreatedBy = Convert.ToInt32(Session["CandidateUserRowID"]);
                                model.ClientRowID = Convert.ToInt16(Session["ClientRowid"]);
                                PQClientCodeHolidaysViewModel client = repoPQClient.GetPQClientCodeHolidaysById(model.ClientRowID);
                                #region Generate UniqueComponentId                          

                                var checks = objData.Where(T => T.CheckFamilyRowID == 40 && T.SubCheckRowID == 200);
                                if (checks.Count() > 0)
                                {
                                    foreach (var str in checks)
                                    {
                                        tempEmp.CheckFamilyRowID = str.CheckFamilyRowID;
                                        tempEmp.SubCheckRowID = str.SubCheckRowID;
                                        tempEmp.ClientPackageRowID = str.ClientPackageRowID;
                                        tempEmp.Status = 1;
                                        int TotalRecord = repoEmploymentInfo.GetAllTempEmploymentByClientId(model.ClientRowID);
                                        string clCode = client.ClientCode1.Substring(0, 3).ToUpper();
                                        TotalRecord++;
                                        tempEmp.UniqueComponentID = string.Concat(clCode, "-SPC-", DateTime.Now.ToString("yyyyMMdd"), "-", Convert.ToString(TotalRecord));
                                        tempEmp.PersonalRowID = model.PersonalRowID;
                                        tempEmp.ClientRowID = model.ClientRowID;
                                        tempEmp.CheckStatus = "DEC";



                                        DataTable dt = cc.getTempcheckDetailCount(model.PersonalRowID, tempEmp.SubCheckRowID, "Special");
                                        //if (model.SpecialCheckRowId > 0 && Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        if (Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        {
                                            //repoSplCheckInfo.UpdateTempSpecialInfoByCandidate(tempEmp);
                                            //int Res = repoSplCheckInfo.SaveChanges();

                                        }
                                        else
                                        {
                                            repoSplCheckInfo.AddTempSpacelCheckInformation(tempEmp);
                                            int Res = repoSplCheckInfo.SaveChanges();
                                        }

                                        model.Status = 1;
                                        SavePQVerifiedUploadDoc(model.PersonalRowID, tempEmp.SubCheckRowID, "Provided", "Bank_account_proof", "provideddocuments", model.BankAccountProof, "");

                                    }
                                }
                                #endregion
                                //SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "Provided", "Bank_account_proof", "provideddocuments", model.BankAccountProof, "");
                            }
                        }
                    }
                    if (model.Employmentproof != null)
                    {

                        string errorMessage = await GatZipFileMessage(model.UploadDoc, model.PersonalRowID.ToString(), "Employmentproof");
                        if (errorMessage != "")
                        {
                            ViewBag.ErrorMsg += errorMessage;
                        }
                        else
                        {
                            if (model.PersonalRowID > 0 && model.Employmentproof.ContentLength > 0)
                            {
                                AddTempEmploymentViewModel tempEmp = new AddTempEmploymentViewModel();
                                tempEmp.CreatedDate = DateTime.Now;
                                tempEmp.CreatedBy = Convert.ToInt32(Session["CandidateUserRowID"]);
                                model.ClientRowID = Convert.ToInt16(Session["ClientRowid"]);
                                PQClientCodeHolidaysViewModel client = repoPQClient.GetPQClientCodeHolidaysById(model.ClientRowID);
                                #region Generate UniqueComponentId                          

                                var checks = objData.Where(T => T.CheckFamilyRowID == 12);
                                if (checks.Count() > 0)
                                {
                                    foreach (var str in checks)
                                    {
                                        tempEmp.CheckFamilyRowID = str.CheckFamilyRowID;
                                        tempEmp.SubCheckRowID = str.SubCheckRowID;
                                        tempEmp.ClientPackageRowID = str.ClientPackageRowID;
                                        tempEmp.Status = 1;
                                        int TotalRecord = repoEmploymentInfo.GetAllTempEmploymentByClientId(model.ClientRowID);
                                        string clCode = client.ClientCode1.Substring(0, 3).ToUpper();
                                        TotalRecord++;
                                        tempEmp.UniqueComponentID = string.Concat(clCode, "-EMP-", DateTime.Now.ToString("yyyyMMdd"), "-", Convert.ToString(TotalRecord));
                                        tempEmp.PersonalRowID = model.PersonalRowID;
                                        tempEmp.ClientRowID = model.ClientRowID;
                                        tempEmp.CheckStatus = "DEC";



                                        DataTable dt = cc.getTempcheckDetailCount(model.PersonalRowID, tempEmp.SubCheckRowID, "Employement");
                                        //if (model.SpecialCheckRowId > 0 && Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        if (Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        {
                                            //repoEmploymentInfo.UpdateTempEmploymentInfoByDataEntry(tempEmp);
                                            //int Res = repoEmploymentInfo.SaveChanges();

                                        }
                                        else
                                        {
                                            repoEmploymentInfo.AddTempEmploymentInformation(tempEmp);
                                            int Res = repoEmploymentInfo.SaveChanges();
                                        }


                                        model.Status = 1;
                                        SavePQVerifiedUploadDoc(model.PersonalRowID, tempEmp.SubCheckRowID, "Provided", "Employment_proof", "provideddocuments", model.Employmentproof, "");

                                    }
                                }
                                #endregion
                            }
                        }
                    }

                    if (model.DOBproof != null)
                    {
                        string errorMessage = await GatZipFileMessage(model.UploadDoc, model.PersonalRowID.ToString(), "DOBproof");
                        if (errorMessage != "")
                        {
                            ViewBag.ErrorMsg += errorMessage;
                        }
                        else
                        {
                            if (model.PersonalRowID > 0 && model.DOBproof.ContentLength > 0)
                            {
                                AddTempSpecialCheckViewModel tempEmp = new AddTempSpecialCheckViewModel();
                                tempEmp.CreatedDate = DateTime.Now;
                                tempEmp.CreatedBy = Convert.ToInt32(Session["CandidateUserRowID"]);
                                model.ClientRowID = Convert.ToInt16(Session["ClientRowid"]);
                                PQClientCodeHolidaysViewModel client = repoPQClient.GetPQClientCodeHolidaysById(model.ClientRowID);
                                #region Generate UniqueComponentId                          

                                var checks = objData.Where(T => T.CheckFamilyRowID == 40 && T.SubCheckRowID == 200);
                                if (checks.Count() > 0)
                                {
                                    foreach (var str in checks)
                                    {
                                        tempEmp.CheckFamilyRowID = str.CheckFamilyRowID;
                                        tempEmp.SubCheckRowID = str.SubCheckRowID;
                                        tempEmp.ClientPackageRowID = str.ClientPackageRowID;
                                        tempEmp.Status = 1;
                                        tempEmp.SC_DOB = Convert.ToDateTime(model.DOB);
                                        int TotalRecord = repoEmploymentInfo.GetAllTempEmploymentByClientId(model.ClientRowID);
                                        string clCode = client.ClientCode1.Substring(0, 3).ToUpper();
                                        TotalRecord++;
                                        tempEmp.UniqueComponentID = string.Concat(clCode, "-SPC-", DateTime.Now.ToString("yyyyMMdd"), "-", Convert.ToString(TotalRecord));
                                        tempEmp.PersonalRowID = model.PersonalRowID;
                                        tempEmp.ClientRowID = model.ClientRowID;
                                        tempEmp.CheckStatus = "DEC";



                                        DataTable dt = cc.getTempcheckDetailCount(model.PersonalRowID, tempEmp.SubCheckRowID, "Special");
                                        //if (model.SpecialCheckRowId > 0 && Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        if (Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        {
                                            //repoSplCheckInfo.UpdateTempSpecialInfoByCandidate(tempEmp);
                                            //int Res = repoSplCheckInfo.SaveChanges();

                                        }
                                        else
                                        {
                                            repoSplCheckInfo.AddTempSpacelCheckInformation(tempEmp);
                                            int Res = repoSplCheckInfo.SaveChanges();
                                        }

                                        model.Status = 1;
                                        SavePQVerifiedUploadDoc(model.PersonalRowID, tempEmp.SubCheckRowID, "Provided", "DOB_proof", "provideddocuments", model.DOBproof, "");

                                    }
                                }
                                #endregion
                                //SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "Provided", "DOB_proof", "provideddocuments", model.DOBproof, "");
                            }
                        }
                    }
                    if (model.Passport != null)
                    {
                        string errorMessage = await GatZipFileMessage(model.UploadDoc, model.PersonalRowID.ToString(), "Passport");
                        if (errorMessage != "")
                        {
                            ViewBag.ErrorMsg += errorMessage;
                        }
                        else
                        {
                            if (model.PersonalRowID > 0 && model.Passport.ContentLength > 0)
                            {
                                AddTempSpecialCheckViewModel tempEmp = new AddTempSpecialCheckViewModel();
                                tempEmp.CreatedDate = DateTime.Now;
                                tempEmp.CreatedBy = Convert.ToInt32(Session["CandidateUserRowID"]);
                                model.ClientRowID = Convert.ToInt16(Session["ClientRowid"]);
                                PQClientCodeHolidaysViewModel client = repoPQClient.GetPQClientCodeHolidaysById(model.ClientRowID);
                                #region Generate UniqueComponentId                          

                                var checks = objData.Where(T => T.CheckFamilyRowID == 40 && T.SubCheckRowID == 200);
                                if (checks.Count() > 0)
                                {
                                    foreach (var str in checks)
                                    {
                                        tempEmp.CheckFamilyRowID = str.CheckFamilyRowID;
                                        tempEmp.SubCheckRowID = str.SubCheckRowID;
                                        tempEmp.ClientPackageRowID = str.ClientPackageRowID;
                                        tempEmp.Status = 1;
                                        int TotalRecord = repoEmploymentInfo.GetAllTempEmploymentByClientId(model.ClientRowID);
                                        string clCode = client.ClientCode1.Substring(0, 3).ToUpper();
                                        TotalRecord++;
                                        tempEmp.UniqueComponentID = string.Concat(clCode, "-SPC-", DateTime.Now.ToString("yyyyMMdd"), "-", Convert.ToString(TotalRecord));
                                        tempEmp.PersonalRowID = model.PersonalRowID;
                                        tempEmp.ClientRowID = model.ClientRowID;
                                        tempEmp.CheckStatus = "DEC";

                                        DataTable dt = cc.getTempcheckDetailCount(model.PersonalRowID, tempEmp.SubCheckRowID, "Special");
                                        //if (model.SpecialCheckRowId > 0 && Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        if (Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        {
                                            //repoSplCheckInfo.UpdateTempSpecialInfoByCandidate(tempEmp);
                                            //int Res = repoSplCheckInfo.SaveChanges();

                                        }
                                        else
                                        {
                                            repoSplCheckInfo.AddTempSpacelCheckInformation(tempEmp);
                                            int Res = repoSplCheckInfo.SaveChanges();
                                        }

                                        model.Status = 1;
                                        SavePQVerifiedUploadDoc(model.PersonalRowID, tempEmp.SubCheckRowID, "Provided", "Passport", "provideddocuments", model.Passport, "");

                                    }
                                }
                                #endregion
                                //SavePQVerifiedUploadDoc(model.PersonalRowID, tempEmp.SubCheckRowID, "Provided", "Passport", "provideddocuments", model.Passport, "");


                            }
                        }
                    }
                    if (model.Passportsizephoto != null)
                    {
                        string errorMessage = await GatZipFileMessage(model.UploadDoc, model.PersonalRowID.ToString(), "Passportsizephoto");
                        if (errorMessage != "")
                        {
                            ViewBag.ErrorMsg += errorMessage;
                        }
                        else
                        {
                            if (model.PersonalRowID > 0 && model.Passportsizephoto.ContentLength > 0)
                            {
                                AddTempSpecialCheckViewModel tempEmp = new AddTempSpecialCheckViewModel();
                                tempEmp.CreatedDate = DateTime.Now;
                                tempEmp.CreatedBy = Convert.ToInt32(Session["CandidateUserRowID"]);
                                model.ClientRowID = Convert.ToInt16(Session["ClientRowid"]);
                                PQClientCodeHolidaysViewModel client = repoPQClient.GetPQClientCodeHolidaysById(model.ClientRowID);
                                #region Generate UniqueComponentId                          

                                var checks = objData.Where(T => T.CheckFamilyRowID == 40 && T.SubCheckRowID == 200);
                                if (checks.Count() > 0)
                                {
                                    foreach (var str in checks)
                                    {
                                        tempEmp.CheckFamilyRowID = str.CheckFamilyRowID;
                                        tempEmp.SubCheckRowID = str.SubCheckRowID;
                                        tempEmp.ClientPackageRowID = str.ClientPackageRowID;
                                        tempEmp.Status = 1;
                                        int TotalRecord = repoEmploymentInfo.GetAllTempEmploymentByClientId(model.ClientRowID);
                                        string clCode = client.ClientCode1.Substring(0, 3).ToUpper();
                                        TotalRecord++;
                                        tempEmp.UniqueComponentID = string.Concat(clCode, "-SPC-", DateTime.Now.ToString("yyyyMMdd"), "-", Convert.ToString(TotalRecord));
                                        tempEmp.PersonalRowID = model.PersonalRowID;
                                        tempEmp.ClientRowID = model.ClientRowID;
                                        tempEmp.CheckStatus = "DEC";

                                        DataTable dt = cc.getTempcheckDetailCount(model.PersonalRowID, tempEmp.SubCheckRowID, "Special");
                                        //if (model.SpecialCheckRowId > 0 && Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        if (Convert.ToInt32(dt.Rows[0]["counts"]) > 0)
                                        {
                                            //repoSplCheckInfo.UpdateTempSpecialInfoByCandidate(tempEmp);
                                            //int Res = repoSplCheckInfo.SaveChanges();

                                        }
                                        else
                                        {
                                            repoSplCheckInfo.AddTempSpacelCheckInformation(tempEmp);
                                            int Res = repoSplCheckInfo.SaveChanges();
                                        }

                                        model.Status = 1;
                                        SavePQVerifiedUploadDoc(model.PersonalRowID, tempEmp.SubCheckRowID, "Provided", "Passportsizephoto", "provideddocuments", model.Passportsizephoto, "");

                                    }
                                }
                                #endregion
                                //SavePQVerifiedUploadDoc(model.PersonalRowID, tempEmp.SubCheckRowID, "Provided", "Passport", "provideddocuments", model.Passport, "");

                                //SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "Provided", "Passportsizephoto", "provideddocuments", model.Passportsizephoto, "");
                            }
                        }
                    }
                    if (ViewBag.ErrorMsg == "")
                    {
                        int PID = model.PersonalRowID;
                        ViewBag.SuccessMsg = "Record Added Successfully!";
                        ModelState.Clear();
                        return View(model);
                    }
                    else
                    {
                        GetAllCheckListforDocUpload(model.PersonalRowID);
                        return View(model);
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

        [NonAction]
        private List<CheckDetailModel> GetAllCheckListforHindujaDocUpload(int id = 0)
        {
            List<CheckDetailModel> objData = new List<CheckDetailModel>();
            try
            {

                if (id > 0)
                {
                    var CandidateChecks = repoPQCandidateCheck.GetTempCandidateChecks(id);

                    if (CandidateChecks != null && CandidateChecks.Count() > 0)
                    {
                        foreach (var item in CandidateChecks)
                        {
                            CheckDetailModel data = new CheckDetailModel();
                            if (item.ClientPackageRowID > 0)
                            {
                                var chkData = repoClientPackage.GetPackageDetailsById(item.ClientPackageRowID);
                                data.SubCheckName = chkData.SubCheckFamillyName;
                                data.SubCheckRowID = chkData.SubCheckRowID;
                                data.CheckFamilyRowID = chkData.CheckFamilyRowID;
                                data.ClientPackageRowID = chkData.ClientPackageRowID;
                                objData.Add(data);

                            }
                            else if (item.ClientCheckRowID > 0)
                            {
                                var chkData = repoClientCheck.GetChecksDetailsById(item.ClientCheckRowID);
                                data.SubCheckName = chkData.SubCheckFamillyName;
                                data.SubCheckRowID = chkData.SubCheckRowID;
                                data.CheckFamilyRowID = chkData.CheckFamilyRowID;
                                data.ClientPackageRowID = chkData.ClientCheckRowID;
                                objData.Add(data);

                            }
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
            return objData.ToList();
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
        //    _documentService.UploadDynamicCandidateFiles(filename, base64, "D:/wwwroot2/Uploads/");
        //    //  postedFile.SaveAs(TempPath + Path.GetFileName(filename));
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
        //[NonAction]
        //public string GatZipFileMessageXLS(HttpPostedFileBase postedFile, string clientRowid, string filenm)
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
        //    string ext = Path.GetExtension(postedFile.FileName);
        //    string filename = clientRowid + "_" + filenm + "_" + NowTime + ext;
        //    string base64 = string.Empty;
        //    postedFile.InputStream.Position = 0;
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        postedFile.InputStream.CopyTo(memoryStream); // Convert the byte array to Base64 
        //        base64 = Convert.ToBase64String(memoryStream.ToArray());
        //    }
        //    _documentService.UploadDynamicCandidateFiles(filename, base64, "D:/wwwroot2/Uploads/");
        //      postedFile.SaveAs(TempPath + Path.GetFileName(filename));
        //    if (postedFile != null)
        //    {
        //        if (postedFile.ContentLength > 0)
        //        {
        //            var bytes = System.IO.File.ReadAllBytes(TempPath + Path.GetFileName(filename));
        //            if (MimeType.GetMimeTypeXLS(bytes, TempPath + Path.GetFileName(filename)) == false)
        //            {
        //                ErrorMessage = "Only.xls,.xlsx File allowed";
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
        public async Task<string> GatZipFileMessageXLS(HttpPostedFileBase postedFile, string clientRowid, string filenm)
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
            string ext = Path.GetExtension(postedFile.FileName);
            string filename = clientRowid + "_" + filenm + "_" + NowTime + ext;
            string base64 = string.Empty;
            postedFile.InputStream.Position = 0;
            using (var memoryStream = new MemoryStream())
            {
                postedFile.InputStream.CopyTo(memoryStream); // Convert the byte array to Base64 
                base64 = Convert.ToBase64String(memoryStream.ToArray());
            }
            _documentService.UploadDynamicCandidateFiles(filename, base64, "D:/wwwroot2/Uploads/");
            postedFile.SaveAs(TempPath + Path.GetFileName(filename));
            if (postedFile != null)
            {
                if (postedFile.ContentLength > 0)
                {
                    var bytes = System.IO.File.ReadAllBytes(TempPath + Path.GetFileName(filename));
                    if (MimeType.GetMimeTypeXLS(bytes, TempPath + Path.GetFileName(filename)) == false)
                    {
                        ErrorMessage = "Only.xls,.xlsx File allowed";
                    }
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


        public void ExportToExcel<T>(IEnumerable<T> list, string strFileName, string[] selectedColumns)
        {
            try
            {
                var TempTable = new DataTable(typeof(T).Name);
                PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in props)
                {
                    //TempTable.Columns.Add(prop.Name, prop.PropertyType);
                    TempTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                foreach (var item in list)
                {
                    var values = new object[props.Length];
                    for (var i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                        //row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }

                    TempTable.Rows.Add(values);
                }


                DataTable dt = new DataTable();
                DataView view = new System.Data.DataView(TempTable);
                if (selectedColumns.Length > 0)
                    dt = view.ToTable("Details", false, selectedColumns);
                else
                    dt = TempTable;

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
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception Ex)
            {

                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        public void ExportToExcel<T>(IEnumerable<T> list, string strFileName)
        {
            try
            {
                var TempTable = new DataTable(typeof(T).Name);
                PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in props)
                {
                    //TempTable.Columns.Add(prop.Name, prop.PropertyType);
                    TempTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                foreach (var item in list)
                {
                    var values = new object[props.Length];
                    for (var i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                        //row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }

                    TempTable.Rows.Add(values);
                }


                DataTable dt = new DataTable();
                DataView view = new System.Data.DataView(TempTable);

                dt = TempTable;

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
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception Ex)
            {

                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpPost]
        public JsonResult CheckDuplicate(string MobileNo, string EmailId)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetDuplicate(MobileNo, EmailId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return Json(new
                    {
                        isDuplicate = true,
                        companyRefNo = dt.Rows[0]["CompanyRefNo"].ToString(),
                        name = dt.Rows[0]["CandidateName"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { isDuplicate = false, error = ex.Message });
            }

            return Json(new { isDuplicate = false });
        }
    }

}