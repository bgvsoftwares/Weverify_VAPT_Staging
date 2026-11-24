using BAL;
using BAL.CandidateRepository;
using BAL.ClientRepository;
using BAL.HRRepository;
using BAL.OCR;
using BAL.Perfios;
using BAL.ProvidedInfoRepository;
using DAL;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using ViewModels;
using ViewModels.CandidateViewModel;
using ViewModels.ClientViewModel;
using ViewModels.DigiTap;
using ViewModels.HRViewModels;
using ViewModels.PartnerViewModels;
using ViewModels.Perfios;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;
using WebAppBGV.CommonMethods;
using WebAppBGV.Models;
using static BAL.WhatsAppAPI;
using static GoogleMaps.LocationServices.Directions;
using static WebAppBGV.CommonMethods.EmailSmartping;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using rest = RestSharp;
namespace WebAppBGV.Controllers
{
    public class AccountController : Controller
    {
        IAccountRepository repoAccount;
        IDropDownMasterRepository repoDDL;
        IPQClientRepository repoPQClient;
        IPQPersonalRepository repoPQPersonal;
        IPQCandidateLoginRepository repoCandidate;
        ISubCheckFamilyRepository repoSubCheck;
        ISubCheckFamilyRepository repoSubCheckFamily;
        IClientDashboardRepository repoDashboardClient;
        IPQVerifiedUploadDocRepository repoPQVerifiedUploadDoc;
        string Secret_Key_hcaptcha = ConfigurationManager.AppSettings["Secret_Key_hcaptcha"].ToString();
        string hcaptchaURL = ConfigurationManager.AppSettings["hcaptchaURL"].ToString();
        public AccountController(IAccountRepository _repoAccount,
             IDropDownMasterRepository _repoDDL,
            IClientDashboardRepository _repoDashboardClient,
            IPQPersonalRepository _repoPQPersonal,
            IPQCandidateLoginRepository _repoCandidate,
            IPQClientRepository _repoPQClient,
            ISubCheckFamilyRepository _repoSubCheck,
            ISubCheckFamilyRepository _repoSubCheckFamily,
IPQVerifiedUploadDocRepository _repoPQVerifiedUploadDoc)
        {
            repoAccount = _repoAccount;
            repoDDL = _repoDDL;
            repoPQClient = _repoPQClient;
            repoPQPersonal = _repoPQPersonal;
            repoDashboardClient = _repoDashboardClient;
            repoCandidate = _repoCandidate;
            repoSubCheck = _repoSubCheck;
            repoSubCheckFamily = _repoSubCheckFamily;
            repoPQVerifiedUploadDoc = _repoPQVerifiedUploadDoc;

            /* Start VAPT point for Restrict Web.Config file don't download */
            string query = System.Web.HttpContext.Current.Request.Url.Query;
            string queryUpperCase = string.Empty;
            if (query.Count() > 0)
            {
                queryUpperCase = query.ToUpper();
                if (queryUpperCase.Contains("WEB.CONFIG") || queryUpperCase.Contains("CONFIG") || queryUpperCase.Contains("MACHINE.CONFIG"))
                {
                    Response.Cookies["ASP.NET_SessionId"].Secure = false;
                    Session.RemoveAll();
                    Session.Clear();
                    Session.Abandon();
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddSeconds(-30);
                    Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                    Response.Redirect("~/Account/Index");
                }
            } 
            /* End VAPT point for Restrict Web.Config file don't download */
        }

        #region *********************** Company Login ********************* 



        [AllowAnonymous]
        public ActionResult GetImgCaptcha()
        {
            string captchaText = GenerateRandomText(6);
            Session["Captcha"] = clsCommonMethods.PEncrypt(captchaText);
            using (var bitmap = new Bitmap(200, 60))
            using (var g = Graphics.FromImage(bitmap))
            {
                Random rand = new Random();
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // Background
                g.Clear(GetRandomLightColor(rand));
                // Draw noise lines
                for (int i = 0; i < 3; i++)
                {
                    Pen pen = new Pen(GetRandomDarkColor(rand), 1);
                    int x1 = rand.Next(bitmap.Width);
                    int y1 = rand.Next(bitmap.Height);
                    int x2 = rand.Next(bitmap.Width);
                    int y2 = rand.Next(bitmap.Height);
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
                // Fonts
                string[] fonts = { "Arial", "Verdana", "Comic Sans MS", "Tahoma", "Times New Roman" };
                // Draw CAPTCHA characters with slight rotation
                for (int i = 0; i < captchaText.Length; i++)
                {
                    string chr = captchaText[i].ToString();
                    Font font = new Font(fonts[rand.Next(fonts.Length)], rand.Next(20, 30), FontStyle.Bold);
                    Brush brush = new SolidBrush(GetRandomDarkColor(rand));
                    float angle = rand.Next(0, 0);
                    // Save graphics state
                    g.TranslateTransform(30 * i + 10, 5);
                    g.RotateTransform(angle);
                    g.DrawString(chr, font, brush, 0, 0);
                    g.ResetTransform(); // Reset
                }
                // Add random noise dots
                for (int i = 0; i < 100; i++)
                {
                    int x = rand.Next(bitmap.Width);
                    int y = rand.Next(bitmap.Height);
                    bitmap.SetPixel(x, y, GetRandomDarkColor(rand));
                }
                // Convert image to byte array
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return File(stream.ToArray(), "image/png");
                }
            }
        }
        private Color GetRandomDarkColor(Random rand)
        {
            return Color.FromArgb(rand.Next(0, 150), rand.Next(0, 150), rand.Next(0, 150));
        }

        private Color GetRandomLightColor(Random rand)
        {
            return Color.FromArgb(rand.Next(180, 255), rand.Next(180, 255), rand.Next(180, 255));
        }
        private string GenerateRandomText(int length = 5)
        {
            const string chars = "ASDFGHJKLPOIUYTREWQZXCVBNM123456789GHJKLMNPQR";
            var rng = new RNGCryptoServiceProvider();
            var data = new byte[length];
            rng.GetBytes(data);
            var sb = new StringBuilder(length);
            foreach (var b in data)
                sb.Append(chars[b % chars.Length]);
            return sb.ToString();
        }
        #region SSO Login with clients
        [HttpGet]
        public ActionResult Loginsso()
        {
            var ClientId = "";
            if (Session["ClientRowID"] != null)
            {
                ClientId = Convert.ToString(Session["ClientRowID"]);
                ClientCustomize CC = new ClientCustomize();
                int validUser = CC.GetValidUsers(Convert.ToInt32(ClientId));
            }

            Session["AuthenticationToken"] = "";
            Response.Cookies.Clear();
            Session["Counter"] = 0;
            UpdateLogOutDetails();
            string tenantId = ConfigurationManager.AppSettings["TenantId"].ToString();
            string redirectUri = ConfigurationManager.AppSettings["Redirect_uri"].ToString();
            string logoutUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/logout?post_logout_redirect_uri={Uri.EscapeDataString(redirectUri)}";
            return View();
        }
        [HttpPost]
        public ActionResult Loginsso(string id)
        {
            string loginUrl = "";
            try
            {
                string tenantId = ConfigurationManager.AppSettings["TenantId"].ToString();
                string clientId = ConfigurationManager.AppSettings["Client_id"].ToString();
                string redirectUri = ConfigurationManager.AppSettings["Redirect_uri"].ToString();
                string scope = "openid profile email";
                loginUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize" +
                                  $"?client_id={clientId}&response_type=code&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_mode=query&scope={Uri.EscapeDataString(scope)}";
            }
            catch (Exception ex)
            {

            }
            return Redirect(loginUrl);
        }
        [HttpGet]
        public ActionResult SsoLogindetail(string code, string session_state)
        {
            StringBuilder StrBuilderObj = new StringBuilder();
            string Client_id = ConfigurationManager.AppSettings["Client_id"].ToString();
            string Client_secret = ConfigurationManager.AppSettings["Client_secret"].ToString();
            string Redirect_uri = ConfigurationManager.AppSettings["Redirect_uri"].ToString();
            string tenantId = ConfigurationManager.AppSettings["TenantId"].ToString();
            StrBuilderObj.Append(Environment.NewLine);
            StrBuilderObj.Append("SsoLogindetail:Response:code:-" + code.ToString() + "session_state:-" + session_state);
            string Token = API.Microsoft_Azure_ssoAPI(Client_id, Client_secret, tenantId, code, Redirect_uri);
            StrBuilderObj.Append(Environment.NewLine);
            StrBuilderObj.Append("Token = API.Microsoft_Azure_ssoAPI:TokenToString " + Token.ToString());
            if (Token != "")
            {
                var resdata = API.Jwttoken(Token);
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("var resdata = API.Jwttoken(Token) " + resdata.Name + " " + resdata.UniqueName);
                if (resdata.Name != "" && resdata.UniqueName != "")
                {
                    //resdata.UniqueName = "pranay.arukala@ctrls.in";
                    try
                    {
                        var loggedUser = repoAccount.GetClientNGRUserLoggedDetails_API_SSO(resdata.UniqueName);
                        if (loggedUser.ClientRowID == 0)
                        {
                            StrBuilderObj.Append(Environment.NewLine);
                            StrBuilderObj.Append("Response:TokenToString" + resdata.Name + " " + resdata.UniqueName);
                            TempData["Message"] = "User Id not exits! " + resdata.UniqueName.ToString();
                            clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), resdata.UniqueName, "", "Client");
                            return RedirectToActionPermanent("Loginsso", "Account");

                        }
                        string UserName = loggedUser.UserID;
                        string loginTime = new AccountRepository().GetLastLoginDetails(resdata.UniqueName);

                        Session["LastLoginTime"] = loginTime;

                        if (loggedUser.ClientName != null && loggedUser.UserID == UserName)
                        {
                            Session["ClientAccessType"] = loggedUser.ClientAccessType;
                            Session["ClientName"] = loggedUser.ClientName + " (" + loggedUser.ClientSubGroupName + ")";
                            //string hostName = System.Net.Dns.GetHostName();
                            //string myIP = System.Net.Dns.GetHostByName(hostName).AddressList[0].ToString();
                            string localIP = GetIPAddress();
                            string ipaddress;
                            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                            if (ipaddress == "" || ipaddress == null)
                                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
                            Session["ClientUserRowID"] = loggedUser.ClientUserRowID;
                            Session["ClientRowID"] = loggedUser.ClientRowID;
                            Session["UserName"] = UserName;
                            Session["ClientNameMail"] = loggedUser.ClientName;
                            String guid = Convert.ToString(Guid.NewGuid());
                            Session["AuthenticationToken"] = guid;
                            Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));
                            Session["SessionToken"] = guid;
                            Session["Valid"] = 1;
                            if (Convert.ToInt32(Session["Valid"]) == 1)
                            {
                                DateTime dateTime = DateTime.Today;
                                TimeSpan TimeSinceCreation = new TimeSpan();
                                if (loggedUser.ModifiedTime != null)
                                {
                                    TimeSinceCreation = dateTime.Subtract(loggedUser.ModifiedTime.Value);
                                }
                                else
                                {
                                    TimeSinceCreation = dateTime.Subtract(loggedUser.CreatedTime.Value);
                                }
                            }
                            AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                            loggedInDetails.LoginName = loggedUser.ClientName + "(" + loggedUser.ClientSubGroupName + ")";
                            loggedInDetails.LoginType = loggedUser.LocationName;
                            loggedInDetails.LoginId = UserName;
                            loggedInDetails.LoginBy = "Client";
                            string hostNameMachine = Dns.GetHostName();
                            int len = Dns.GetHostByName(hostNameMachine).AddressList.Length;
                            string myIPAddr = "";
                            for (int i = 0; i <= len - 1; i++)
                            {
                                myIPAddr = Dns.GetHostByName(hostNameMachine).AddressList[i].ToString();
                            }
                            loggedInDetails.IPDetails = myIPAddr;
                            loggedInDetails.IPDetails = Request.UserHostAddress;
                            loggedInDetails.LoginDate = DateTime.Now;
                            loggedInDetails.LoginStatus = 1;
                            loggedInDetails.Deviceidentifiers = Request.UserAgent;
                            loggedInDetails.BrowserName = Request.Browser.Browser.ToString();
                            Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);
                            //Updated by harsh 01-09-2020
                            Session["Validation"] = true;
                            Session["OTPcheck"] = true;
                        }
                        else
                        {
                            //var activWebUsers = repoAccount.GetAllActiveClientUsers(UserName);
                            //if (!(activWebUsers.CRPUserName.Trim() == UserName))
                            //{
                            //    Session["Counter"] = Convert.ToInt32(Session["Counter"]) + 1;
                            //}
                            if (!(Convert.ToInt32(Session["Counter"]) >= 3))
                            {
                                TempData["Message"] = "User Id not exits ! " + resdata.UniqueName.ToString();
                                clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), resdata.UniqueName, "", "Client");
                                return RedirectToActionPermanent("Loginsso", "Account");
                            }
                            //else
                            //{
                            //    Session["Counter"] = 0;
                            //    UpdateClientUserBlockStatusViewModel userBlocked = new UpdateClientUserBlockStatusViewModel();
                            //    userBlocked.UserID = UserName;
                            //    userBlocked.BlockedDate = DateTime.Now;
                            //    userBlocked.Status = 3;
                            //    repoAccount.BlockedClientUser(userBlocked);
                            //    repoAccount.SaveChanges();
                            //    ViewBag.ErrorMsg = "User has been Blocked, it will automatically unlocked after 30 minutes or contact to Administrator!";
                            //    Session.Abandon();
                            //    return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                            //}
                        }
                        Session["UserLogin"] = "Client User";
                        FormsAuthentication.SetAuthCookie(UserName, false);

                        SessionGenerateForSessionFixation();

                        /* Start ConCurrent Session implementation */
                        ClientCustomize objClientCustomize = new ClientCustomize();
                        int updateClientLogin = objClientCustomize.UpdateUserSessionToken(Convert.ToString(Session["ClientUserRowID"]), Convert.ToString(Session["SessionToken"]));
                        /* End ConCurrent Session implementation] */

                        if (loggedUser.CreatedBy == "Company-Management")
                        {
                            //Step 8: Redirect to trigger a new session                                        
                            return RedirectToAction("DashboardClientNewForSessionFixation");
                        }
                        else if (loggedUser.isMFA == 1)
                        {
                            //Step 8: Redirect to trigger a new session                                        
                            return RedirectToAction("ClientEmailVerification2FAForSessionFixation");
                        }
                        else
                        {
                            //Step 8: Redirect to trigger a new session                                        
                            return RedirectToAction("DashboardClientForSessionFixation");
                        }


                    }
                    catch (Exception ex)
                    {
                        StrBuilderObj.Append(Environment.NewLine);
                        StrBuilderObj.Append("Response:TokenToString" + ex.Message.ToString());
                        API.WriteLog(StrBuilderObj.ToString(), "", "", "LTIMindTree", "var resdata = API.Jwttoken(Token)", "" + "-" + "" + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                        TempData["Message"] = ex.Message.ToString() + resdata.UniqueName.ToString();
                        clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), resdata.UniqueName, "", "Client");
                        return RedirectToActionPermanent("Loginsso", "Account");

                    }
                }
                else
                {
                    TempData["Message"] = "User Not exits !!!";
                    return RedirectToActionPermanent("Loginsso", "Account");
                }
            }
            else
            {
                TempData["Message"] = "Token not access !!!";
                return RedirectToActionPermanent("Loginsso", "Account");
            }
        }
        public bool ValidateCandidateName(string ProvidedName, string VerifiedName)
        {
            bool MatchStatus = false;
            try
            {
                if (ProvidedName.Trim().ToUpper() == VerifiedName.Trim().ToUpper())
                {
                    MatchStatus = true;
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return MatchStatus;
        }

        public bool ValidateCandidateDOB(string ProvidedDOB, string VerifiedDOB)
        {
            bool MatchStatus = false;
            try
            {
                DateTime dob = DateTime.ParseExact(VerifiedDOB, "yyyy-MM-dd", null);
                string formattedDob = dob.ToString("dd-MM-yyyy");
                if (ProvidedDOB.Trim().ToUpper() == formattedDob.Trim().ToUpper())
                {
                    MatchStatus = true;
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return MatchStatus;
        }
        #endregion
        
        // GET: Account
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Index(string ErM = "")
        {
           CompanyLoginViewModel model = new CompanyLoginViewModel();
            if (ErM != "")
            {
                if (ErM != "")
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(ErM);
                }
            }

            if (LoginDetail.CRPUserName != null)
            {
                AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                loggedInDetails.LoginName = LoginDetail.TeamMemberName;
                loggedInDetails.LoginType = "Company";
                loggedInDetails.LoginBy = "";
                loggedInDetails.IPDetails = Request.UserHostAddress;
                loggedInDetails.LoginStatus = 0;
                loggedInDetails.LogoutTime = DateTime.Now.Date;
                loggedInDetails.LoginDetailRowID = LoginDetail.LoginDetailRowID;
                //UpdateLogOutDetails(LoginDetail.LoginDetailRowID);
                LoginDetail.CRPUserName = null;
                LoginDetail.LoginDetailRowID = 0;
                Session.Abandon();
                return RedirectToAction("Index");
            }
            else
            {
                Session["AuthenticationToken"] = "";
                Response.Cookies.Clear();
                Session["Counter"] = 0;
                Session["UserLogin"] = "!Internal User";
            }

            return View(model);

        }


        [HttpGet]
        public JsonResult Encrl(string pass)
        {
            var result = clsCommonMethods.PasswordEncrypt(pass);
            return Json(new { pass = result }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public static string base64Decode(string sData) //Decode    
        {
            try
            {
                var encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecodeByte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                string result = new String(decodedChar);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
            }
        }
        public static string DecryptString(string cipherText)
        {
            string key = "1234567890123456";
            string iv = "1234567890123456";

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(cipherBytes))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CompanyLoginViewModel model)
        {
            try
            {
                //byte[] bytes = Convert.FromBase64String(model.CRPPassWord.Trim());
                model.CRPPassWord = DecryptString(model.CRPPassWord.Trim());//Encoding.UTF8.GetString(bytes);

                var uname = model.CRPUserName;

                if (ModelState.IsValid)
                {
                    if (clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) != model.CaptchaVal)
                    {
                        ModelState.AddModelError("", "Captcha validation failed.");
                        string msg = clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) + "=" + model.CaptchaVal;
                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), msg, "Index");
                        return RedirectToAction("Index", "Account", new { ErM = clsCommonMethods.PEncrypt("Captcha validation failed") });
                    }

                    string pword = model.CRPPassWord;
                    if (string.Compare(uname, "", false) == 0 && string.Compare(pword, "", false) == 0)
                    {
                        ModelState.AddModelError("", "Enter User Name and Password");
                    }
                    else if (string.Compare(uname, "", false) == 0 && string.Compare(pword, "", false) != 0)
                    {
                        ModelState.AddModelError("", "Enter User Name");
                    }
                    else if (!(string.Compare(uname, "", false) != 0 && string.Compare(pword, "", false) == 0))
                    {
                        DateTime today = DateTime.Today;
                        string str = model.CRPUserName.Trim().ToLower();
                        string str1 = pword.Trim().ToLower();
                        string[] strArrays1 = str.Split(' ');
                        string[] strArrays2 = str1.Split(' ');
                        string str2 = strArrays1[0];
                        string str3 = strArrays2[0];

                        if (!(string.Compare(str2, "select", false) == 0 || string.Compare(str2, "exec", false) == 0 || string.Compare(str2, "execute", false) == 0 || string.Compare(str2, "drop", false) == 0 || string.Compare(str2, "truncate", false) == 0 || string.Compare(str2, "insert", false) == 0 || string.Compare(str2, "update", false) == 0 || string.Compare(str2, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                          )
                        {
                            if (!(string.Compare(str3, "select", false) == 0 || string.Compare(str3, "exec", false) == 0 || string.Compare(str3, "execute", false) == 0 || string.Compare(str3, "drop", false) == 0 || string.Compare(str3, "truncate", false) == 0 || string.Compare(str3, "insert", false) == 0 || string.Compare(str3, "update", false) == 0 || string.Compare(str3, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                               )
                            {
                                string UserName = model.CRPUserName.Trim();
                                string UserPassword = clsCommonMethods.PasswordEncrypt(pword.Trim());
                                CompanyUserLoggedViewModel loggedUser = new CompanyUserLoggedViewModel();
                                try
                                {
                                    loggedUser = repoAccount.GetWebUserSignInDetails(UserName, UserPassword);
                                    if (loggedUser.Message == "Incorrect UserName")
                                    {
                                        ViewBag.ErrorMsg = "Invalid User Name !";
                                        ClientCustomize cn = new ClientCustomize();
                                        cn.Insert_LoginLog("User Login", "Invalid User Name ! " + UserName, UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());
                                        return RedirectToAction("Index", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                    else if (loggedUser.Message == "Incorrect password")
                                    {
                                        ViewBag.ErrorMsg = "Invalid Password !";
                                        ClientCustomize cn = new ClientCustomize();
                                        cn.Insert_LoginLog("User Login", "Invalid Password ! ", UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());
                                        return RedirectToAction("Index", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                    else
                                    {
                                        string loginTime = new AccountRepository().GetLastLoginDetails(UserName);
                                        Session["LastLoginTime"] = loginTime;
                                    }
                                }
                                catch (Exception exception1)
                                {
                                    ViewBag.ErrorMsg = "Login failed Please try Again";
                                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), exception1.Message, exception1.StackTrace);
                                }

                                ClientCustomize cc = new ClientCustomize();
                                if (loggedUser != null && UserName == loggedUser.CRPUserName)
                                {
                                    Session["CRPUserName"] = UserName;
                                    Session["WebUserRowid"] = loggedUser.WebUserRowID;
                                    Session["TeamMemberRowID"] = loggedUser.TeamMemberRoWID;
                                    Session["TeamMemberName"] = loggedUser.TeamMemberName;
                                    Session["TeamMemberEmail"] = loggedUser.TeamMemberEmail;
                                    //Session["LocationCode"] = loggedUser.LocationRowID;
                                    Session["ClientUType"] = "Company";
                                    Session["Valid"] = 1;

                                    ///  Add security Filter implementation

                                    String guid = Convert.ToString(Guid.NewGuid());
                                    //Storing new Guid in Session
                                    Session["AuthenticationToken"] = guid;
                                    //Adding Cookie in Browser
                                    Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));

                                    /* Start ConCurrent Session implementation */
                                    Session["SessionTokenSpok"] = guid;
                                    /* End ConCurrent Session implementation] */

                                    AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                                    loggedInDetails.LoginName = loggedUser.TeamMemberName;
                                    loggedInDetails.LoginType = "Company";
                                    loggedInDetails.LoginId = UserName;
                                    loggedInDetails.LoginBy = "";
                                    loggedInDetails.IPDetails = Request.UserHostAddress;
                                    loggedInDetails.LoginDate = DateTime.Now;
                                    loggedInDetails.LoginStatus = 1;


                                    LoginDetail.CRPUserName = UserName;
                                    LoginDetail.WebUserRowID = loggedUser.WebUserRowID;
                                    LoginDetail.TeamMemberRoWID = loggedUser.TeamMemberRoWID;
                                    LoginDetail.TeamMemberName = loggedUser.TeamMemberName;
                                    LoginDetail.TeamMemberEmail = loggedUser.TeamMemberEmail;
                                    LoginDetail.Deviceidentifiers = Request.UserAgent.ToString();
                                    LoginDetail.BrowserName = Request.Browser.Browser.ToString();

                                    Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);

                                    if (loggedUser.ExpiryDays >= 1)
                                    {
                                        ViewBag.ErrorMsg = "Your Password has Expired. Please Reset ! ";
                                        ClientCustomize cn = new ClientCustomize();
                                        cn.Insert_LoginLog("User Login", "Your Password has Expired. Please Reset ! ", UserName, "UserName: " + UserName + " Password: IP Address :" + GetIP_Address());
                                        return RedirectToAction("ForgetCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(loggedUser.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }


                                }
                                else
                                {
                                    var activWebUsers = repoAccount.GetAllActiveWebUsers(UserName);

                                    if (activWebUsers.CRPUserName.Trim().ToLower() == UserName.ToLower() && activWebUsers.CRPPassWord.Trim() != UserPassword)
                                    {

                                        Session["Counter"] = Convert.ToInt32(Session["Counter"]) + 1;
                                    }

                                    if (!(Convert.ToInt32(Session["Counter"]) >= 3))
                                    {
                                        ViewBag.ErrorMsg = "Login Failed.Try Again!";
                                        clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), UserName, UserPassword, "Company");

                                        return RedirectToAction("Index", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                    else
                                    {
                                        Session["Counter"] = 0;
                                        UpdateCompanyUserBlockStatusViewModel userBlocked = new UpdateCompanyUserBlockStatusViewModel();
                                        userBlocked.CRPUserName = UserName;
                                        userBlocked.BlockedDate = DateTime.Now;
                                        userBlocked.Status = 3;

                                        repoAccount.BlockedCompanyUser(userBlocked);
                                        repoAccount.SaveChanges();

                                        ViewBag.ErrorMsg = "User has been Blocked, Please contact to Administrator!";
                                        Session.Abandon();
                                        ClientCustomize cn = new ClientCustomize();
                                        cn.Insert_LoginLog("User Login", "User has been Blocked, Please contact to Administrator! ", UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());
                                        return RedirectToAction("Index", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                        // return View();
                                    }

                                }

                                Session["UserLogin"] = "Internal User";
                                FormsAuthentication.SetAuthCookie(UserName, false);

                                SessionGenerateForSessionFixation();

                                /* Start ConCurrent Session implementation */
                                ClientCustomize objClientCustomize = new ClientCustomize();
                                int updateClientLogin = objClientCustomize.UpdateUserSessionTokenForSpokLogin(Convert.ToString(Session["WebUserRowid"]), Convert.ToString(Session["SessionTokenSpok"]));
                                /* End ConCurrent Session implementation] */


                                return RedirectToAction("IndexLoginSelectDesignationForSessionFixation");
                                //return RedirectToAction("SelectDesignation", "Home");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter Password");
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
                ViewBag.ErrorMsg = "Userid password not correct";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                ClientCustomize cn = new ClientCustomize();
                cn.Insert_LoginLog("User Login", Ex.Message, model.CRPUserName.Trim(), "UserName: " + model.CRPUserName.Trim() + " Password:   IP Address :" + GetIP_Address());
                return RedirectToAction("Index", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }

            return RedirectToAction("Index", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult SignIn(string ErM = "")
        {
            CompanySignInViewModel model = new CompanySignInViewModel();
            if (ErM != "")
            {
                if (clsCommonMethods.PDecrypt(ErM) != "")
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(ErM);
                }
            }

            Session.Abandon();
            Session["AuthenticationToken"] = "";
            Response.Cookies.Clear();
            Session["Counter"] = 0;
            Session["UserLogin"] = "!Internal User";

            return View(model);

        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(CompanySignInViewModel model)
        {
            try
            {
                //byte[] bytes = Convert.FromBase64String(model.CRPPassWord.Trim());
                model.CRPPassWord = DecryptString(model.CRPPassWord.Trim());//Encoding.UTF8.GetString(bytes);


                var uname = model.CRPUserName;
                string pword = model.CRPPassWord.Trim();


                if (ModelState.IsValid)
                {
                    if (clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) != model.CaptchaVal)
                    {
                        ModelState.AddModelError("", "Captcha validation failed.");
                        string msg = clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) + "=" + model.CaptchaVal;
                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), msg, "SignIn");
                        return RedirectToAction("SignIn", "Account", new { ErM = clsCommonMethods.PEncrypt("Captcha validation failed") });
                    }
                    if (string.Compare(uname, "", false) == 0 && string.Compare(pword, "", false) == 0)
                    {
                        ModelState.AddModelError("", "Enter User Name and Password");
                    }
                    else if (string.Compare(uname, "", false) == 0 && string.Compare(pword, "", false) != 0)
                    {
                        ModelState.AddModelError("", "Enter User Name");
                    }
                    else if (!(string.Compare(uname, "", false) != 0 && string.Compare(pword, "", false) == 0))
                    {
                        DateTime today = DateTime.Today;
                        string str = model.CRPUserName.Trim().ToLower();
                        string str1 = pword.Trim().ToLower();
                        string[] strArrays1 = str.Split(' ');
                        string[] strArrays2 = str1.Split(' ');
                        string str2 = strArrays1[0];
                        string str3 = strArrays2[0];

                        if (!(string.Compare(str2, "select", false) == 0 || string.Compare(str2, "exec", false) == 0 || string.Compare(str2, "execute", false) == 0 || string.Compare(str2, "drop", false) == 0 || string.Compare(str2, "truncate", false) == 0 || string.Compare(str2, "insert", false) == 0 || string.Compare(str2, "update", false) == 0 || string.Compare(str2, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                          )
                        {
                            if (!(string.Compare(str3, "select", false) == 0 || string.Compare(str3, "exec", false) == 0 || string.Compare(str3, "execute", false) == 0 || string.Compare(str3, "drop", false) == 0 || string.Compare(str3, "truncate", false) == 0 || string.Compare(str3, "insert", false) == 0 || string.Compare(str3, "update", false) == 0 || string.Compare(str3, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                               )
                            {
                                string UserName = model.CRPUserName.Trim();

                                string encPassword = clsCommonMethods.PasswordEncrypt(pword.Trim());

                                CompanyUserLoggedViewModel loggedUser = new CompanyUserLoggedViewModel();
                                try
                                {
                                    loggedUser = repoAccount.GetWebUserSignInDetails(UserName, encPassword);
                                    string loginTime = new AccountRepository().GetLastLoginDetails(UserName);
                                    Session["LastLoginTime"] = loginTime;
                                }
                                catch (Exception exception1)
                                {
                                    ViewBag.ErrorMsg = "Login failed Please try Again";
                                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), exception1.Message, exception1.StackTrace);
                                }

                                ClientCustomize cc = new ClientCustomize();
                                if (loggedUser != null && UserName == loggedUser.CRPUserName)
                                {
                                    Session["CRPUserName"] = UserName;
                                    Session["WebUserRowid"] = loggedUser.WebUserRowID;
                                    Session["TeamMemberRowID"] = loggedUser.TeamMemberRoWID;
                                    Session["TeamMemberName"] = loggedUser.TeamMemberName;
                                    Session["TeamMemberEmail"] = loggedUser.TeamMemberEmail;
                                    //Session["LocationCode"] = loggedUser.LocationRowID;
                                    Session["ClientUType"] = "Company";
                                    Session["Valid"] = 1;
                                    ///  Add security Filter implementation
                                    String guid = Convert.ToString(Guid.NewGuid());
                                    //Storing new Guid in Session
                                    Session["AuthenticationToken"] = guid;
                                    //Adding Cookie in Browser
                                    Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));

                                    /* Start ConCurrent Session implementation */
                                    Session["SessionTokenSpok"] = guid;
                                    /* End ConCurrent Session implementation] */

                                    AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                                    loggedInDetails.LoginName = loggedUser.TeamMemberName;
                                    loggedInDetails.LoginType = "Company";
                                    loggedInDetails.LoginId = UserName;
                                    loggedInDetails.LoginBy = "";
                                    loggedInDetails.IPDetails = Request.UserHostAddress;
                                    loggedInDetails.LoginDate = DateTime.Now;
                                    loggedInDetails.LoginStatus = 1;


                                    LoginDetail.CRPUserName = UserName;
                                    LoginDetail.WebUserRowID = loggedUser.WebUserRowID;
                                    LoginDetail.TeamMemberRoWID = loggedUser.TeamMemberRoWID;
                                    LoginDetail.TeamMemberName = loggedUser.TeamMemberName;
                                    LoginDetail.TeamMemberEmail = loggedUser.TeamMemberEmail;
                                    LoginDetail.Deviceidentifiers = Request.UserAgent.ToString();
                                    LoginDetail.BrowserName = Request.Browser.Browser.ToString();

                                    Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);

                                    if (loggedUser.ExpiryDays >= 1)
                                    {
                                        ViewBag.ErrorMsg = "Your Password has Expired. Please Reset ! ";
                                        return RedirectToAction("ForgetCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(loggedUser.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }


                                }
                                else
                                {
                                    var activWebUsers = repoAccount.GetAllActiveWebUsers(UserName);

                                    if (activWebUsers.CRPUserName.Trim().ToLower() == UserName.ToLower() && activWebUsers.CRPPassWord.Trim() != encPassword)
                                    {

                                        Session["Counter"] = Convert.ToInt32(Session["Counter"]) + 1;
                                    }

                                    if (!(Convert.ToInt32(Session["Counter"]) >= 3))
                                    {
                                        ViewBag.ErrorMsg = "Login Failed.Try Again!";
                                        clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), UserName, encPassword, "Company");

                                        return RedirectToAction("SignIn", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                    else
                                    {
                                        Session["Counter"] = 0;
                                        UpdateCompanyUserBlockStatusViewModel userBlocked = new UpdateCompanyUserBlockStatusViewModel();
                                        userBlocked.CRPUserName = UserName;
                                        userBlocked.BlockedDate = DateTime.Now;
                                        userBlocked.Status = 3;

                                        repoAccount.BlockedCompanyUser(userBlocked);
                                        repoAccount.SaveChanges();

                                        ViewBag.ErrorMsg = "User has been Blocked, Please contact to Administrator!";
                                        Session.Abandon();
                                        return RedirectToAction("SignIn", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                        // return View();
                                    }

                                }

                                Session["UserLogin"] = "Internal User";
                                FormsAuthentication.SetAuthCookie(UserName, false);

                                SessionGenerateForSessionFixation();

                                /* Start ConCurrent Session implementation */
                                ClientCustomize objClientCustomize = new ClientCustomize();
                                int updateClientLogin = objClientCustomize.UpdateUserSessionTokenForSpokLogin(Convert.ToString(Session["WebUserRowid"]), Convert.ToString(Session["SessionTokenSpok"]));
                                /* End ConCurrent Session implementation] */

                                return RedirectToAction("SignInLoginSelectDesignationForSessionFixation");
                                //return RedirectToAction("SelectDesignation", "Home");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter Password");
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
                ViewBag.ErrorMsg = "Userid password not correct";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return RedirectToAction("SignIn", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }

            return RedirectToAction("SignIn", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
        }

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                Session["AuthenticationToken"] = "";
                Response.Cookies.Clear();
                Session["Counter"] = 0;
                fnSaveLogOutDetails();
                UpdateLogOutDetails();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "you have logged out successfully";
                // ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult TimeoutRedirect()
        {
            return View();
        }

        #endregion

        #region *********************** Client Login *********************

        [HttpGet]
        public ActionResult otpdetail(string msg = "")
        {
            ViewBag.errormsg = msg;
            return View();
        }

        // GET: Account
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ClientLogin(string ErrorMsg = "")
        {
            Session["AuthenticationToken"] = "";
            Response.Cookies.Clear();
            Session["UserLogin"] = "!Client User";
            if (ErrorMsg != "")
            {
                if (clsCommonMethods.PDecrypt(ErrorMsg) == "")
                {
                    Session["Counter"] = 0;
                }
                else
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(ErrorMsg);
                }
            }

            ClientLoginViewModel model = new ClientLoginViewModel();

            return View(model);
        }

        public string GetCaptcha()
        {
            Random generator = new Random();
            String captchaVal = generator.Next(0, 9999).ToString("D4");
            Session["CaptchaVal"] = ""; //captchaVal;
            ViewBag.valCaptcha = captchaVal;
            return captchaVal;
        }

        public string EncryptionCheckAES()
        {
            ClientCustomize cc = new ClientCustomize();
            string sessionId = Session["sessionId"].ToString();
            DataTable dt = cc.GetPQUserLoginEncryptionKey(sessionId);
            var EncrptSalt = dt.Rows[0]["salt"].ToString();
            TempData["EncrptSalt"] = EncrptSalt;
            ViewBag.EncData = EncrptSalt;
            return string.Empty;
        }


        private string GetIPAddress()
        {
            StringBuilder sb = new StringBuilder();
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            sb.Append("The Local Machine Host Name: " + strHostName);
            sb.AppendLine();
            IPHostEntry ipHostEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] address = ipHostEntry.AddressList;
            sb.Append("The Local IP Address: " + address[1].ToString());
            sb.AppendLine();
            return address[1].ToString();
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


        private string GetClientIPAddress(bool Lan = false)
        {
            string userIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(userIPAddress))
                userIPAddress = Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(userIPAddress))
                userIPAddress = Request.UserHostAddress;

            if (string.IsNullOrEmpty(userIPAddress) || userIPAddress.Trim() == "::1")
            {
                Lan = true;
                userIPAddress = string.Empty;
            }

            if (Lan)
            {
                if (string.IsNullOrEmpty(userIPAddress))
                {
                    string strHostName = Dns.GetHostName();

                    IPHostEntry iPHostEntries = Dns.GetHostEntry(strHostName);

                    IPAddress[] arrIpaddress = iPHostEntries.AddressList;

                    try
                    {
                        foreach (IPAddress iPAddress in arrIpaddress)
                        {
                            if (iPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                userIPAddress = iPAddress.ToString();
                            }
                        }

                        if (string.IsNullOrEmpty(userIPAddress))
                            userIPAddress = arrIpaddress[arrIpaddress.Length - 1].ToString();
                    }
                    catch
                    {
                        try
                        {
                            userIPAddress = arrIpaddress[0].ToString();
                        }
                        catch
                        {
                            try
                            {
                                arrIpaddress = Dns.GetHostAddresses(strHostName);
                                userIPAddress = arrIpaddress[0].ToString();
                            }
                            catch
                            {
                                userIPAddress = "127.0.0.1";
                            }

                        }
                    }
                }
            }

            return userIPAddress;

        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ClientLogin(ClientLoginViewModel model)
        {
            try
            {
                // byte[] bytes = Convert.FromBase64String(model.CRPPassWord.Trim());
                //model.CRPPassWord = Encoding.UTF8.GetString(bytes);

                model.CRPPassWord = DecryptString(model.CRPPassWord.Trim());

                var uname = model.CRPUserName;
                var pword = model.CRPPassWord;
                if (ModelState.IsValid)
                {

                    if (clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) != model.CaptchaVal)
                    {
                        ModelState.AddModelError("", "Captcha validation failed.");
                        string msg = clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) + "=" + model.CaptchaVal;
                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), msg, "ClientLogin");
                        return RedirectToAction("ClientLogin", "Account", new { ErM = clsCommonMethods.PEncrypt("Captcha validation failed") });
                    }

                    if (string.Compare(uname, "", false) == 0 && string.Compare(pword, "", false) == 0)
                    {
                        ModelState.AddModelError("", "Enter User Name and Password");
                    }
                    else if (string.Compare(uname, "", false) == 0 && string.Compare(pword, "", false) != 0)
                    {
                        ModelState.AddModelError("", "Enter User Name");
                    }
                    else if (!(string.Compare(uname, "", false) != 0 && string.Compare(pword, "", false) == 0))
                    {
                        DateTime today = DateTime.Today;
                        string str = uname.Trim().ToLower();
                        string str1 = pword.Trim();
                        string[] strArrays1 = str.Split(' ');
                        string[] strArrays2 = str1.Split(' ');
                        string str2 = strArrays1[0];
                        string str3 = strArrays2[0];
                        if (!(string.Compare(str2, "select", false) == 0 || string.Compare(str2, "exec", false) == 0 || string.Compare(str2, "execute", false) == 0 || string.Compare(str2, "drop", false) == 0 || string.Compare(str2, "truncate", false) == 0 || string.Compare(str2, "insert", false) == 0 || string.Compare(str2, "update", false) == 0 || string.Compare(str2, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0))
                        {
                            if (!(string.Compare(str3, "select", false) == 0 || string.Compare(str3, "exec", false) == 0 || string.Compare(str3, "execute", false) == 0 || string.Compare(str3, "drop", false) == 0 || string.Compare(str3, "truncate", false) == 0 || string.Compare(str3, "insert", false) == 0 || string.Compare(str3, "update", false) == 0 || string.Compare(str3, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0))
                            {
                                string UserName = uname.Trim();
                                string UserPassword = clsCommonMethods.PasswordEncrypt(pword.Trim());
                                ClientUserLoggedViewModel loggedUser = new ClientUserLoggedViewModel();
                                try
                                {
                                    loggedUser = repoAccount.GetClientNGRUserLoggedDetails(UserName, UserPassword);
                                    string loginTime = new AccountRepository().GetLastLoginDetails(UserName);
                                    Session["LastLoginTime"] = loginTime;
                                    if (loggedUser.Message == "Incorrect UserName")
                                    {
                                        ViewBag.ErrorMsg = "Invalid User Name !";
                                        return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                    else if (loggedUser.Message == "Incorrect password")
                                    {
                                        ViewBag.ErrorMsg = "Invalid Password !";
                                        return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                    else
                                    {
                                        if (loggedUser.ClientRowID == 656 || loggedUser.ClientRowID == 657 || loggedUser.ClientRowID == 660)
                                        {
                                            ClientCustomize cc = new ClientCustomize();
                                            DataTable DtAllow = cc.GetClientAllowIPDetail(loggedUser.ClientRowID);
                                            if (DtAllow.Rows.Count > 0)
                                            {
                                                Session["ClientAccessType"] = loggedUser.ClientAccessType;
                                                Session["ClientName"] = loggedUser.ClientName + " (" + loggedUser.ClientSubGroupName + ")";
                                                //string hostName = System.Net.Dns.GetHostName();
                                                //string myIP = System.Net.Dns.GetHostByName(hostName).AddressList[0].ToString();
                                                string localIP = GetIPAddress();
                                                string ipaddress;
                                                ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                                                if (ipaddress == "" || ipaddress == null)
                                                    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
                                                string allowIp = DtAllow.Rows[0]["AllowIpDetail"].ToString();
                                                string[] balance = allowIp.Split(',');
                                                //bool targetStringInArray = balance.Contains(Request.UserHostAddress);
                                                bool targetStringInArray = (new List<String>(balance)).Contains(ipaddress);
                                                if (targetStringInArray == false)
                                                {
                                                    ViewBag.ErrorMsg = "Ip address (" + ipaddress + ") list not match for this client, please contact with Admin!";
                                                    return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception exception1)
                                {
                                    ViewBag.ErrorMsg = "Userid password not correct";
                                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), exception1.Message, exception1.StackTrace);
                                    return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                }
                                if (loggedUser.ClientName != null && loggedUser.UserID == UserName)
                                {
                                    Session["ClientUserRowID"] = loggedUser.ClientUserRowID;
                                    Session["ClientRowID"] = loggedUser.ClientRowID;
                                    Session["UserName"] = UserName;
                                    Session["ClientNameMail"] = loggedUser.ClientName;
                                    //Session["Password"] = encPassword;
                                    if (loggedUser.ClientRowID == 656 || loggedUser.ClientRowID == 657 || loggedUser.ClientRowID == 660 || loggedUser.ClientRowID == 658 || loggedUser.ClientRowID == 659)
                                    {
                                        Session["Valid"] = 0;
                                        Session["ClientAccessType"] = loggedUser.ClientAccessType;
                                        var ClientSubGroupName = loggedUser.ClientSubGroupName;
                                        if (ClientSubGroupName.Length > 1)
                                        {
                                            Session["ClientName"] = loggedUser.ClientName + " (" + loggedUser.ClientSubGroupName + ")";
                                        }
                                        else
                                        {
                                            Session["ClientName"] = loggedUser.ClientName;
                                        }
                                        Session["ClientNameMail"] = loggedUser.ClientName;
                                    }
                                    if (loggedUser.ClientRowID != 656 && loggedUser.ClientRowID != 657 && loggedUser.ClientRowID != 660 && loggedUser.ClientRowID != 658 && loggedUser.ClientRowID != 659)
                                    {
                                        Session["UserName"] = UserName;
                                        Session["ClientUserRowID"] = loggedUser.ClientUserRowID;
                                        Session["ClientNameMail"] = loggedUser.ClientName;
                                        Session["ClientRowID"] = loggedUser.ClientRowID;
                                        var ClientSubGroupName = loggedUser.ClientSubGroupName;
                                        if (ClientSubGroupName.Length > 1)
                                        {
                                            Session["ClientName"] = loggedUser.ClientName + " (" + loggedUser.ClientSubGroupName + ")";
                                        }
                                        else
                                        {
                                            Session["ClientName"] = loggedUser.ClientName;
                                        }
                                        Session["ClientSubGroupName"] = loggedUser.ClientSubGroupName;
                                        Session["ClientCode1"] = loggedUser.ClientCode1;
                                        Session["LocationName"] = loggedUser.LocationName;
                                        Session["HRApproval"] = loggedUser.HRApprovalRequired;
                                        Session["Abbreviation"] = loggedUser.Abbreviation;
                                        Session["CodeGeneration"] = loggedUser.CodeGeneration;
                                        Session["CSpocName"] = loggedUser.CSpocName;
                                        Session["CSpocEmailID"] = loggedUser.CSpocEmailID;
                                        Session["CreatedBy"] = loggedUser.CreatedBy;
                                        Session["ClientBranch"] = loggedUser.ClientBranch;
                                        Session["ClientUType"] = "Client";
                                        Session["Valid"] = 1;
                                        Session["UType"] = loggedUser.UType;

                                        Session["ClientAccessType"] = loggedUser.ClientAccessType;
                                        if (loggedUser.ExpiryDays >= 1)
                                        {
                                            ViewBag.ErrorMsg = "Your Password has Expired. Please Reset ! ";
                                            return RedirectToAction("ForgetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(loggedUser.ClientUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                        }
                                    }
                                    ///  Add security Filter implementation
                                    String guid = Convert.ToString(Guid.NewGuid());
                                    //Storing new Guid in Session
                                    Session["AuthenticationToken"] = guid;
                                    //Adding Cookie in Browser
                                    Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));
                                    Session["SessionTokenClient"] = guid;
                                    if (Convert.ToInt32(Session["Valid"]) == 1)
                                    {
                                        DateTime dateTime = DateTime.Today;
                                        TimeSpan TimeSinceCreation = new TimeSpan();
                                        if (loggedUser.ModifiedTime != null)
                                        {
                                            TimeSinceCreation = dateTime.Subtract(loggedUser.ModifiedTime.Value);
                                        }
                                        else
                                        {
                                            TimeSinceCreation = dateTime.Subtract(loggedUser.CreatedTime.Value);
                                        }
                                    }
                                    AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                                    loggedInDetails.LoginName = loggedUser.ClientName + "(" + loggedUser.ClientSubGroupName + ")";
                                    loggedInDetails.LoginType = loggedUser.LocationName;
                                    loggedInDetails.LoginId = UserName;
                                    loggedInDetails.LoginBy = "Client";
                                    string hostNameMachine = Dns.GetHostName();
                                    int len = Dns.GetHostByName(hostNameMachine).AddressList.Length;
                                    string myIPAddr = "";
                                    for (int i = 0; i <= len - 1; i++)
                                    {
                                        myIPAddr = Dns.GetHostByName(hostNameMachine).AddressList[i].ToString();
                                    }
                                    loggedInDetails.IPDetails = myIPAddr;
                                    loggedInDetails.IPDetails = Request.UserHostAddress;
                                    loggedInDetails.LoginDate = DateTime.Now;
                                    loggedInDetails.LoginStatus = 1;
                                    loggedInDetails.Deviceidentifiers = Request.UserAgent;
                                    loggedInDetails.BrowserName = Request.Browser.Browser.ToString();
                                    ClientCustomize clientCustomize = new ClientCustomize();
                                    DataTable dtpassword = clientCustomize.ValidateUSerInitialLogin("client", loggedUser.ClientUserRowID);
                                    string res = "0";
                                    if (dtpassword.Rows.Count > 0)
                                    {
                                        res = dtpassword.Rows[0]["res"].ToString();
                                   
                                    }

                                    if(res == "1")
                                    {
                                        ViewBag.ErrorMsg = "First-time login detected. Please update your password to secure your account.";
                                        return RedirectToAction("ForgetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(loggedUser.ClientUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                   
                                    Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);
                                    //Updated by harsh 01-09-2020
                                    Session["Validation"] = true;
                                    Session["OTPcheck"] = true;
                                }
                                else
                                {
                                    var activWebUsers = repoAccount.GetAllActiveClientUsers(UserName);
                                    if (!(activWebUsers.CRPUserName.Trim() == UserName) || activWebUsers.CRPPassWord.Trim() != UserPassword)
                                    {
                                        Session["Counter"] = Convert.ToInt32(Session["Counter"]) + 1;
                                    }
                                    if (!(Convert.ToInt32(Session["Counter"]) >= 3))
                                    {
                                        ViewBag.ErrorMsg = "User Id Password not correct.Try Again!";
                                        clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), UserName, UserPassword, "Client");
                                        return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                    else
                                    {
                                        Session["Counter"] = 0;
                                        UpdateClientUserBlockStatusViewModel userBlocked = new UpdateClientUserBlockStatusViewModel();
                                        userBlocked.UserID = UserName;
                                        userBlocked.BlockedDate = DateTime.Now;
                                        userBlocked.Status = 3;
                                        repoAccount.BlockedClientUser(userBlocked);
                                        repoAccount.SaveChanges();
                                        ViewBag.ErrorMsg = "User has been Blocked, it will automatically unlocked after 30 minutes or contact to Administrator!";
                                        Session.Abandon();
                                        return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                }
                                Session["UserLogin"] = "Client User";
                                FormsAuthentication.SetAuthCookie(UserName, false);

                                SessionGenerateForSessionFixation();

                                /* Start ConCurrent Session implementation */
                                ClientCustomize objClientCustomize = new ClientCustomize();
                                int updateClientLogin = objClientCustomize.UpdateUserSessionToken(Convert.ToString(Session["ClientUserRowID"]), Convert.ToString(Session["SessionTokenClient"]));
                                /* End ConCurrent Session implementation] */

                                if (loggedUser.CreatedBy == "Company-Management")
                                {
                                    //Step 8: Redirect to trigger a new session                                        
                                    return RedirectToAction("DashboardClientNewForSessionFixation");
                                }
                                else if (loggedUser.isMFA == 1)
                                {
                                    //Step 8: Redirect to trigger a new session                                        
                                    return RedirectToAction("ClientEmailVerification2FAForSessionFixation");
                                }
                                else
                                {
                                    //Step 8: Redirect to trigger a new session                                        
                                    return RedirectToAction("DashboardClientForSessionFixation");
                                }
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter Password");
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
                ViewBag.ErrorMsg = "Userid password not correct";// Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }
            //clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), "716", "");
            return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ClientNGR(string ErrorMsg = "")
        {
            Session["AuthenticationToken"] = "";
            Response.Cookies.Clear();
            Session["UserLogin"] = "!Client User";
            if (ErrorMsg != "")
            {
                if (clsCommonMethods.PDecrypt(ErrorMsg) != "")
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(ErrorMsg);
                    Session["Counter"] = 0;
                }
            }
            ClientNGRViewModel model = new ClientNGRViewModel();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> ClientNGR(ClientNGRViewModel model)
        {
            try
            {
                //byte[] bytes = Convert.FromBase64String(model.CRPPassWord.Trim());
                //model.CRPPassWord = Encoding.UTF8.GetString(bytes);
                model.CRPPassWord = DecryptString(model.CRPPassWord.Trim());


                var captcha = model.CaptchaVal;
                var uname = model.CRPUserName;
                string pword = model.CRPPassWord;

                if (ModelState.IsValid)
                {
                    if (clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) != model.CaptchaVal)
                    {
                        ModelState.AddModelError("", "Captcha validation failed.");
                        string msg = clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) + "=" + model.CaptchaVal;
                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), msg, "ClientNGR");
                        return RedirectToAction("ClientNGR", "Account", new { ErM = clsCommonMethods.PEncrypt("Captcha validation failed") });
                    }


                    if (string.Compare(uname, "", false) == 0 && string.Compare(pword, "", false) == 0)
                    {
                        ModelState.AddModelError("", "Enter User Name and Password");
                    }
                    else if (string.Compare(uname, "", false) == 0 && string.Compare(pword, "", false) != 0)
                    {
                        ModelState.AddModelError("", "Enter User Name");
                    }
                    else if (!(string.Compare(uname, "", false) != 0 && string.Compare(pword, "", false) == 0))
                    {
                        DateTime today = DateTime.Today;
                        string str = uname.Trim().ToLower();
                        string str1 = pword.Trim();
                        string[] strArrays1 = str.Split(' ');
                        string[] strArrays2 = str1.Split(' ');
                        string str2 = strArrays1[0];
                        string str3 = strArrays2[0];

                        if (!(string.Compare(str2, "select", false) == 0 || string.Compare(str2, "exec", false) == 0 || string.Compare(str2, "execute", false) == 0 || string.Compare(str2, "drop", false) == 0 || string.Compare(str2, "truncate", false) == 0 || string.Compare(str2, "insert", false) == 0 || string.Compare(str2, "update", false) == 0 || string.Compare(str2, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                          )
                        {
                            if (!(string.Compare(str3, "select", false) == 0 || string.Compare(str3, "exec", false) == 0 || string.Compare(str3, "execute", false) == 0 || string.Compare(str3, "drop", false) == 0 || string.Compare(str3, "truncate", false) == 0 || string.Compare(str3, "insert", false) == 0 || string.Compare(str3, "update", false) == 0 || string.Compare(str3, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                               )
                            {
                                string UserName = uname.Trim();
                                string UserPassword = clsCommonMethods.PasswordEncrypt(pword.Trim());
                                ClientUserLoggedViewModel loggedUser = new ClientUserLoggedViewModel();
                                try
                                {
                                    loggedUser = repoAccount.GetClientNGRUserLoggedDetails(UserName, UserPassword);
                                    if (loggedUser.Message == "Incorrect UserName")
                                    {
                                        ViewBag.ErrorMsg = "Invalid User Name !";
                                        ClientCustomize cn = new ClientCustomize();
                                        cn.Insert_LoginLog("client Login", "Invalid User Name ! " + UserName, UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());
                                        return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                    else if (loggedUser.Message == "Incorrect password")
                                    {
                                        ViewBag.ErrorMsg = "Invalid Password !";

                                        ClientCustomize cn = new ClientCustomize();
                                        cn.Insert_LoginLog("client Login", "Invalid Password ! ", UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());
                                        return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                    }
                                    else
                                    {
                                        string loginTime = new AccountRepository().GetLastLoginDetails(UserName);
                                        Session["LastLoginTime"] = loginTime;
                                    }
                                }
                                catch (Exception exception1)
                                {

                                    ViewBag.ErrorMsg = "Userid password not correct";
                                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), exception1.Message, exception1.StackTrace);
                                    return RedirectToAction("ClientNGR", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                }
                                if (loggedUser.ValidUser == 1)
                                {
                                    if (loggedUser.ClientName != null && loggedUser.UserID == UserName)
                                    {
                                        Session["ClientUserRowID"] = loggedUser.ClientUserRowID;
                                        Session["ClientRowID"] = loggedUser.ClientRowID;
                                        Session["UserName"] = UserName;
                                        Session["ClientNameMail"] = loggedUser.ClientName;
                                        Session["UserName"] = UserName;
                                        Session["ClientUserRowID"] = loggedUser.ClientUserRowID;
                                        Session["ClientNameMail"] = loggedUser.ClientName;
                                        Session["ClientRowID"] = loggedUser.ClientRowID;
                                        var ClientSubGroupName = loggedUser.ClientSubGroupName;
                                        if (ClientSubGroupName.Length > 1)
                                        {
                                            Session["ClientName"] = loggedUser.ClientName + " (" + loggedUser.ClientSubGroupName + ")";
                                        }
                                        else
                                        {
                                            Session["ClientName"] = loggedUser.ClientName;
                                        }
                                        Session["ClientSubGroupName"] = loggedUser.ClientSubGroupName;
                                        Session["LocationName"] = loggedUser.LocationName;
                                        Session["HRApproval"] = loggedUser.HRApprovalRequired;
                                        Session["Abbreviation"] = loggedUser.Abbreviation;
                                        Session["CodeGeneration"] = loggedUser.CodeGeneration;
                                        Session["CSpocName"] = loggedUser.CSpocName;
                                        Session["CSpocEmailID"] = loggedUser.CSpocEmailID;
                                        Session["CreatedBy"] = loggedUser.CreatedBy;
                                        Session["ClientBranch"] = loggedUser.ClientBranch;
                                        Session["ClientUType"] = "Client";
                                        Session["UType"] = loggedUser.UType;
                                        if (loggedUser.ClientRowID == 656 || loggedUser.ClientRowID == 657 || loggedUser.ClientRowID == 660 || loggedUser.ClientRowID == 658 || loggedUser.ClientRowID == 659)
                                        {
                                            Session["Valid"] = 0;
                                        }
                                        else
                                        {
                                            Session["Valid"] = 1;
                                        }

                                        if (loggedUser.ExpiryDays >= 1)
                                        {
                                            ViewBag.ErrorMsg = "Your Password has Expired. Please Reset ! ";
                                            ClientCustomize cn = new ClientCustomize();
                                            cn.Insert_LoginLog("client Login", "Your Password has Expired. Please Reset ! ", UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());
                                            return RedirectToAction("ForgetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(loggedUser.ClientUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });

                                        }

                                        Session["ClientAccessType"] = loggedUser.ClientAccessType;
                                        String guid = Convert.ToString(Guid.NewGuid());
                                        //Storing new Guid in Session
                                        Session["AuthenticationToken"] = guid;
                                        //Adding Cookie in Browser
                                        Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));

                                        Session["SessionTokenClient"] = guid;

                                        if (Convert.ToInt32(Session["Valid"]) == 1)
                                        {
                                            DateTime dateTime = DateTime.Today;
                                            TimeSpan TimeSinceCreation = new TimeSpan();

                                            if (loggedUser.ModifiedTime != null)
                                            {
                                                TimeSinceCreation = dateTime.Subtract(loggedUser.ModifiedTime.Value);
                                            }
                                            else
                                            {
                                                TimeSinceCreation = dateTime.Subtract(loggedUser.CreatedTime.Value);
                                            }
                                        }
                                        AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                                        loggedInDetails.LoginName = loggedUser.ClientName + "(" + loggedUser.ClientSubGroupName + ")";
                                        loggedInDetails.LoginType = loggedUser.LocationName;
                                        loggedInDetails.LoginId = UserName;
                                        loggedInDetails.LoginBy = "Client";
                                        loggedInDetails.IPDetails = Request.UserHostAddress;
                                        loggedInDetails.LoginDate = DateTime.Now;
                                        loggedInDetails.LoginStatus = 1;
                                        loggedInDetails.Deviceidentifiers = Request.UserAgent;
                                        loggedInDetails.BrowserName = Request.Browser.Browser.ToString();
                                        ClientCustomize clientCustomize = new ClientCustomize();
                                        DataTable dtpassword = clientCustomize.ValidateUSerInitialLogin("client", loggedUser.ClientUserRowID);
                                        string res = "0";
                                        if (dtpassword.Rows.Count > 0)
                                        {
                                            res = dtpassword.Rows[0]["res"].ToString();

                                        }

                                        if (res == "1")
                                        {
                                            ViewBag.ErrorMsg = "First-time login detected. Please update your password to secure your account.";
                                            return RedirectToAction("ForgetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(loggedUser.ClientUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                        }
                                        Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);
                                        Session["Validation"] = true;
                                        Session["OTPcheck"] = true;

                                    }
                                    else
                                    {
                                        var activWebUsers = repoAccount.GetAllActiveClientUsers(UserName);
                                        if (!(activWebUsers.CRPUserName.Trim() == UserName) || activWebUsers.CRPPassWord.Trim() != UserPassword)
                                        {
                                            Session["Counter"] = Convert.ToInt32(Session["Counter"]) + 1;
                                        }

                                        if (!(Convert.ToInt32(Session["Counter"]) >= 3))
                                        {
                                            ViewBag.ErrorMsg = "User Id Password not correct.Try Again!";
                                            clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), UserName, UserPassword, "Client");
                                            return RedirectToAction("ClientNGR", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });

                                        }
                                        else
                                        {
                                            Session["Counter"] = 0;
                                            return RedirectToAction("ClientNGR", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                        }
                                    }
                                    Session["UserLogin"] = "Client User";
                                    FormsAuthentication.SetAuthCookie(UserName, false);

                                    SessionGenerateForSessionFixation();

                                    /* Start ConCurrent Session implementation */
                                    ClientCustomize objClientCustomize = new ClientCustomize();
                                    int updateClientLogin = objClientCustomize.UpdateUserSessionToken(Convert.ToString(Session["ClientUserRowID"]), Convert.ToString(Session["SessionTokenClient"]));
                                    /* End ConCurrent Session implementation] */

                                    if (loggedUser.CreatedBy == "Company-Management")
                                    {
                                        //Step 8: Redirect to trigger a new session                                        
                                        return RedirectToAction("DashboardClientNewForSessionFixation");

                                    }
                                    //else if (loggedUser.ClientRowID == 656 || loggedUser.ClientRowID == 657 || loggedUser.ClientRowID == 660 || loggedUser.ClientRowID == 658 || loggedUser.ClientRowID == 659)
                                    //{
                                    //    return RedirectToAction("ClientEmailVerification2FA", "Account");
                                    //}
                                    //else if (loggedUser.ClientRowID == 890)
                                    //{
                                    //    return RedirectToAction("ClientEmailVerification2FA", "Account");
                                    //}
                                    else if (loggedUser.isMFA == 1)
                                    {
                                        //Step 8: Redirect to trigger a new session                                        
                                        return RedirectToAction("ClientEmailVerification2FAForSessionFixation");
                                    }
                                    else
                                    {
                                        //Step 8: Redirect to trigger a new session                                        
                                        return RedirectToAction("DashboardClientForSessionFixation");
                                    }
                                }
                                else
                                {
                                    return RedirectToAction("ClientLogin");
                                }
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter Password");
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
                ViewBag.ErrorMsg = "User Id Password is not correct";// Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                ClientCustomize cn = new ClientCustomize();
                cn.Insert_LoginLog("client Login", Ex.Message, model.CRPUserName.Trim(), "UserName: " + model.CRPUserName.Trim() + " Password:   IP Address :" + GetIP_Address());
                return RedirectToAction("ClientNGR", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }
            return RedirectToAction("ClientNGR", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
        }


        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ForgetCredentials(string Wrid = "", string msg = "")
        {
            ForgetCredentialsViewModel model = new ForgetCredentialsViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                model.WebUserRowID = Wrid;
                var userDetailsDB = cc.getwebuserDetails(Convert.ToInt32(clsCommonMethods.PDecrypt(Wrid)));
                if (userDetailsDB.Rows.Count > 0)
                {
                    model.CRPUserName = userDetailsDB.Rows[0]["CRPUserName"].ToString();
                }
                else
                {
                    return RedirectToAction("Index", "Account", new { });
                }


                if (model.WebUserRowID == "")
                {
                    return RedirectToAction("Index", "Account", new { });
                }
                if (msg != "")
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
                    TempData["ErrorMsg"] = clsCommonMethods.PDecrypt(msg);
                }


            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.ToString();
                throw;
            }

            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ForgetCredentials(ForgetCredentialsViewModel model)
        {
            try
            {
                ClientCustomize CCC = new ClientCustomize();

                if (model.CRPConfirmNewPwd == model.CRPNewPwd)
                {
                    model.WebUserRowID = clsCommonMethods.PDecrypt(model.WebUserRowID);
                    model.CRPConfirmNewPwd = clsCommonMethods.PasswordEncrypt(model.CRPConfirmNewPwd);
                    var GetLastPass = CCC.GetLastPasswords(model.WebUserRowID, model.CRPConfirmNewPwd, "WebUsers");
                    if (GetLastPass.Rows.Count > 0)
                    {
                        ViewBag.ErrorMsg = "Please Don't try last five passwords !";
                        return RedirectToAction("ForgetCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                    }
                    if (Convert.ToString(model.CRPConfirmNewPwd) != "")
                    {
                        if (clsCommonMethods.PasswordDecrypt(model.CRPConfirmNewPwd).Length < 14)
                        {
                            ViewBag.ErrorMsg = "Password Should be 14 Character !";
                            return RedirectToAction("ForgetCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                        }
                        else
                        {
                            var userIpAddress = GetClientIPAddress();
                            int results = CCC.UpdateUserCredentials(model.WebUserRowID, model.CRPConfirmNewPwd, userIpAddress);
                            if (results == 1)
                            {
                                ViewBag.SuccessMsg = "Password update successfully";
                                return RedirectToAction("Index", "Account", new { ErM = clsCommonMethods.PEncrypt(ViewBag.SuccessMsg) });
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "Password not update please try again !";
                                return RedirectToAction("ForgetCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                            }
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Password not update please try again !";
                        return RedirectToAction("ForgetCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });

                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Password not match";
                    return RedirectToAction("ForgetCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });

                }


            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.ToString();
                return RedirectToAction("ForgetCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt("Error Founds.!") });
                throw;
            }

        }


        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ForgetClientCredentials(string Wrid = "", string msg = "")
        {
            ForgetCredentialsViewModel model = new ForgetCredentialsViewModel();
            ClientCustomize CC = new ClientCustomize();
            try
            {
                var DBDetails = CC.GetPQClientLoginsDetails(Convert.ToInt32(clsCommonMethods.PDecrypt(Wrid)));
                if (DBDetails.Rows.Count > 0)
                {
                    model.WebUserRowID = Wrid;
                    model.CRPUserName = DBDetails.Rows[0]["UserID"].ToString();
                }
                if (model.WebUserRowID == "")
                {
                    return RedirectToAction("ClientLogin", "Account", new { });
                }
                if (msg != "")
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
                    TempData["ErrorMsg"] = clsCommonMethods.PDecrypt(msg);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.ToString();
                throw;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ForgetClientCredentials(ForgetCredentialsViewModel model)
        {
            try
            {
                ClientCustomize CCC = new ClientCustomize();
                if (model.CRPNewPwd == model.CRPConfirmNewPwd)
                {
                    model.CRPConfirmNewPwd = clsCommonMethods.PasswordEncrypt(model.CRPConfirmNewPwd);
                    model.WebUserRowID = clsCommonMethods.PDecrypt(model.WebUserRowID);
                    var GetLastPass = CCC.GetLastPasswords(model.WebUserRowID, model.CRPConfirmNewPwd, "PQClientLogins");
                    if (GetLastPass.Rows.Count > 0)
                    {
                        ViewBag.ErrorMsg = "Please Don't try last five passwords !";
                        return RedirectToAction("ForgetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), oldP = model.CRPOldPwd1, uId = clsCommonMethods.PEncrypt(model.CRPUserName.ToString()), cRid = clsCommonMethods.PEncrypt(model.ClientRowID), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                    }
                    if (Convert.ToString(model.CRPConfirmNewPwd) != "")
                    {
                        if (clsCommonMethods.PasswordDecrypt(model.CRPConfirmNewPwd).Length < 14)
                        {
                            ViewBag.ErrorMsg = "Password Should be 14 Character !";
                            return RedirectToAction("ForgetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                        }
                        else
                        {
                            var userIpAddress = GetClientIPAddress();
                            int results = CCC.UpdateClientUserCredentials(model.WebUserRowID, model.CRPConfirmNewPwd, userIpAddress);
                            if (results == 1)
                            {
                                ViewBag.SuccessMsg = "Password update successfully";
                                return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.SuccessMsg) });
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "Password not update please try again !";
                                return RedirectToAction("ForgetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                            }
                        }

                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Password not update please try again !";
                        return RedirectToAction("ForgetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                    }

                }
                else
                {
                    ViewBag.ErrorMsg = "Password not Match !";
                    return RedirectToAction("ForgetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Error Founds !";
                return RedirectToAction("ForgetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });

            }
        }


        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ClientEmailVerification2FA()
        {
            try
            {
                ClientCustomize CCC = new ClientCustomize();
                short clientRowID = Convert.ToInt16(Session["ClientRowID"]);
                int validUser = CCC.GetValidUsers(Convert.ToInt32(clientRowID));
                Session["NCounter"] = 0;
                if (Session["OTPcheck"].ToString().ToUpper() == "FALSE")
                {
                    ViewBag.EmailOTP = null;
                    ViewBag.OTPcheck = false;
                    ViewBag.ErrorMsg = "OTP Expired!! Please Click on Resend";
                    return View();
                }
                int clientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);

                if (Helpers.RateLimit.IsLimitExceeded($"ClientEmailVerification2FA{clientUserRowID}", 2, 5))
                {
                    ViewBag.ErrorMsg = "You are allow 2 attempt with 5 min";

                    // CaptchaModel obj = new CaptchaModel();

                    //model.CaptchaCodeText = "";
                    return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                }
                var check = Session["Validation"].ToString();
                if (string.IsNullOrWhiteSpace(check))
                {
                    if (validUser == 0)
                    {
                        return RedirectToAction("ClientLogin", "Account");
                    }

                    return RedirectToAction("ClientNGR", "Account");
                }
                if (check.ToUpper() == "TRUE")
                {

                    
                    //string userName = Session["UserName"].ToString();
                    Random generator = new Random();
                    String mailOTP = generator.Next(0, 999999).ToString("D6");
                    TempData["EmailOTP"] = mailOTP;
                    ViewBag.EmailOTP = mailOTP;
                    ViewBag.OTPcheck = true;
                    ClientCustomize cc = new ClientCustomize();
                    DataTable dt = cc.GetClientUserEmailId(clientRowID, clientUserRowID);
                    var user_EmailId = dt.Rows[0]["UserEmailID"].ToString();
                    if (string.IsNullOrWhiteSpace(user_EmailId))
                    {
                        ViewBag.ErrorMsg = "User Email ID not Found!";
                        if (validUser == 0)
                        {
                            return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                        }

                        return RedirectToAction("ClientNGR", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                    }

                    
                    string to = user_EmailId.ToString();
                    string display = "BGV Securitas";
                    string subj = "OTP For successfull login";
                    string from = "bgv.request@securitas-india.com";
                    string msg = "Your one time OTP for Login is = " + "<span style='color:red;'>" +
                    mailOTP + "</span>" + "</br>" + ".Only valid for 2 min.";
                    clsSendMail clsMail = new clsSendMail();
                    var val = clsMail.SendMail(display, from, to, subj, msg);
                    return View();
                    // return RedirectToAction("DashboardClient", "Home", new { area = "ClientArea" });
                }
                if (validUser == 0)
                {
                    return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                }
                return RedirectToAction("ClientNGR", "Account");
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return RedirectToAction("ClientLogin", "Account");
            }

        }

        [HttpPost]
        public ActionResult ClientEmailVerification2FA(ClientOTPViewmodel otp)
        {
            ClientCustomize CCC = new ClientCustomize();
            short clientRowID = Convert.ToInt16(Session["ClientRowID"]);
            int validUser = CCC.GetValidUsers(Convert.ToInt32(clientRowID));

            var otps = otp.MailOTP;
            var EncData = "000011";

            var checks = Session["Validation"].ToString();
            if (string.IsNullOrWhiteSpace(checks))
            {
                ViewBag.ErrorMsg = "Not Verify Account";
                if (validUser == 0)
                {
                    return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                }
                return RedirectToAction("ClientNGR", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }
            var mailOtp = TempData["EmailOTP"].ToString();
            if (otps == mailOtp)
            {
                Session["Counter"] = 0;
                Session["Valid"] = 1;
                return RedirectToAction("DashboardClient", "Home", new { area = "ClientArea" });
            }
            else
            {
                ViewBag.ErrorMsg = "Wrong OTP!!";
                ViewBag.EmailOTP = mailOtp;
                TempData["EmailOTP"] = mailOtp;

                Session["NCounter"] = Convert.ToInt32(Session["NCounter"]) + 1;

                if (Convert.ToInt32(Session["NCounter"]) >= 3)
                {
                    Session["Counter"] = 0;
                    UpdateClientUserBlockStatusViewModel userBlocked = new UpdateClientUserBlockStatusViewModel();
                    string userId = Session["UserName"].ToString();
                    userBlocked.UserID = userId;
                    userBlocked.BlockedDate = DateTime.Now;
                    userBlocked.Status = 3;
                    repoAccount.BlockedClientUser(userBlocked);
                    repoAccount.SaveChanges();

                    ViewBag.ErrorMsg = "User has been Blocked, please contact to Administrator!";
                    Session.Abandon();
                    if (validUser == 0)
                    {
                        return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                    }
                    return RedirectToAction("ClientNGR", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                }


                return View(/*new { ErrorMsg = ViewBag.ErrorMsg }*/);
            }


        }



        [HttpGet]
        public ActionResult ExpireOtpFun()
        {
            try
            {
                var check = Session["Validation"].ToString();
                if (string.IsNullOrWhiteSpace(check))
                {
                    return RedirectToAction("ClientNGR", "Account");
                }
                if (check.ToUpper() == "TRUE")
                {
                    Session["OTPcheck"] = false;
                }
                return RedirectToAction("ClientEmailVerification2FA");
            }
            catch (Exception)
            {
                return RedirectToAction("ClientNGR", "Account");
            }

        }


        [HttpGet]
        public ActionResult ResendOtpFunc()
        {
            try
            {
                var check = Session["Validation"].ToString();
                if (string.IsNullOrWhiteSpace(check))
                {
                    return RedirectToAction("ClientNGR", "Account");
                }
                if (check.ToUpper() == "TRUE")
                {
                    Session["OTPcheck"] = true;
                }
                return RedirectToAction("ClientEmailVerification2FA", "Account");
            }
            catch (Exception)
            {
                return RedirectToAction("ClientNGR", "Account");
            }

        }


        [HttpGet]
        public ActionResult LogOutClient()
        {
            try
            {
                var ClientId = "";
                ClientId = Convert.ToString(Session["ClientRowID"]);
                ClientCustomize CC = new ClientCustomize();
                int validUser = CC.GetValidUsers(Convert.ToInt32(ClientId));
                Session["AuthenticationToken"] = "";
                Response.Cookies.Clear();
                Session["Counter"] = 0;
                UpdateLogOutDetails();
                if (validUser == 0)
                {
                    return RedirectToAction("ClientLogin");
                }


            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "you have logged out successfully";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return RedirectToAction("ClientLogin");

        }

        [HttpGet]
        public ActionResult ClientTimeoutRedirect()
        {
            return View();
        }
        #endregion

        #region *********************** Candidate Login *********************

        // GET: Account
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult CandidateLogin(string ErrorMsg = "")
        {
            Session["AuthenticationToken"] = "";
            Session["Counter"] = 0;
            Session["UserLogin"] = "!Candidate User";
            Response.Cookies.Clear();
            if (ErrorMsg != "")
            {
                if (clsCommonMethods.PDecrypt(ErrorMsg) != "")
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(ErrorMsg);
                }
            }
            CandidateLoginViewModel model = new CandidateLoginViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CandidateLogin(CandidateLoginViewModel model)
        {
            try
            {
                //byte[] bytes = Convert.FromBase64String(model.CRPPassWord.Trim());
                // model.CRPPassWord = Encoding.UTF8.GetString(bytes);
                model.CRPPassWord = DecryptString(model.CRPPassWord.Trim());

                var captcha = model.CaptchaVal;
                var uname = model.CRPUserName;


                if (ModelState.IsValid)
                {
                    if (clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) != model.CaptchaVal)
                    {
                        ModelState.AddModelError("", "Captcha validation failed.");
                        string msg = clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) + "=" + model.CaptchaVal;
                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), msg, "CandidateLogin");

                        return RedirectToAction("CandidateLogin", "Account", new { ErM = clsCommonMethods.PEncrypt("Captcha validation failed") });
                    }
                    var pword = model.CRPPassWord;
                    if (string.Compare(model.CRPUserName, "", false) == 0 && string.Compare(pword, "", false) == 0)
                    {
                        ModelState.AddModelError("", "Enter User Name and Password");
                    }
                    else if (string.Compare(model.CRPUserName, "", false) == 0 && string.Compare(pword, "", false) != 0)
                    {
                        ModelState.AddModelError("", "Enter User Name");
                    }
                    else if (!(string.Compare(model.CRPUserName, "", false) != 0 && string.Compare(pword, "", false) == 0))
                    {
                        DateTime today = DateTime.Today;
                        string str = model.CRPUserName.Trim().ToLower();
                        string str1 = pword.Trim();
                        string[] strArrays1 = str.Split(' ');
                        string[] strArrays2 = str1.Split(' ');
                        string str2 = strArrays1[0];
                        string str3 = strArrays2[0];

                        if (!(string.Compare(str2, "select", false) == 0 || string.Compare(str2, "exec", false) == 0 || string.Compare(str2, "execute", false) == 0 || string.Compare(str2, "drop", false) == 0 || string.Compare(str2, "truncate", false) == 0 || string.Compare(str2, "insert", false) == 0 || string.Compare(str2, "update", false) == 0 || string.Compare(str2, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                          )
                        {
                            if (!(string.Compare(str3, "select", false) == 0 || string.Compare(str3, "exec", false) == 0 || string.Compare(str3, "execute", false) == 0 || string.Compare(str3, "drop", false) == 0 || string.Compare(str3, "truncate", false) == 0 || string.Compare(str3, "insert", false) == 0 || string.Compare(str3, "update", false) == 0 || string.Compare(str3, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                               )
                            {
                                string UserName = model.CRPUserName.Trim();
                                string UserPassword = pword.Trim();

                                //string pdcry = clsCommonMethods.PasswordDecrypt(str1);
                                string encPassword = clsCommonMethods.PasswordEncrypt(str1);

                                CandidateUserLoggedViewModel loggedUser = new CandidateUserLoggedViewModel();
                                int CandidateLinkLapseDays = 365;


                                try
                                {
                                    //if (IsValidEmail(UserName))
                                    if (UserName != "")
                                    {
                                        //loggedUser = repoAccount.GetInsuffCandidateUserLoggedDetails(UserName, encPassword);
                                        loggedUser = repoAccount.GetCandidateUserLoggedDetails(UserName, encPassword);
                                        if (loggedUser.Status == 3)
                                        {
                                            ViewBag.ErrorMsg = "User is blocked. Please contact to admin!";
                                            ClientCustomize cn = new ClientCustomize();
                                            cn.Insert_LoginLog("Candidate Login", "User is blocked. Please contact to admin! " + UserName, UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());

                                            return RedirectToAction("CandidateLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                        }

                                        if (loggedUser.Status == 0)
                                        {
                                            ViewBag.ErrorMsg = "ID is deactivated. Please contact to admin!";
                                            ClientCustomize cn = new ClientCustomize();
                                            cn.Insert_LoginLog("Candidate Login", "ID is deactivated. Please contact to admin! " + UserName, UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());

                                            return RedirectToAction("CandidateLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                        }

                                        if (loggedUser.Status == 2)
                                        {
                                            ViewBag.ErrorMsg = "Your form has submitted succesfuly. Now you are not able to login. Please conntact to your HR for further querry!";
                                            ClientCustomize cn = new ClientCustomize();
                                            cn.Insert_LoginLog("Candidate Login", "Your form has submitted succesfuly. Now you are not able to login. Please conntact to your HR for further querry! " + UserName, UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());

                                            return RedirectToAction("CandidateLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                        }
                                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), "Account controller UserName : " + UserName, encPassword);
                                        ClientCustomize cc = new ClientCustomize();
                                        DataTable dtAggree = cc.GetDataAgreementsignCandidate(loggedUser.TempPersonalRowID);
                                        if (dtAggree.Rows.Count > 0)
                                        {
                                            loggedUser.ClientUserrowid = Convert.ToInt32(dtAggree.Rows[0]["ClientUserRowID"]);
                                            loggedUser.Agreementsign = Convert.ToBoolean(dtAggree.Rows[0]["Agreementsign"]);
                                            CandidateLinkLapseDays = Convert.ToInt16(dtAggree.Rows[0]["CandidateLinkLapseDays"]);
                                            Session["Agreementsign"] = loggedUser.Agreementsign;
                                        }
                                    }
                                    else
                                    {
                                        loggedUser = repoAccount.GetCandidateUserLoggedDetails(UserName, encPassword);
                                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), "Account controller UserName : " + UserName, encPassword);
                                        ClientCustomize cc = new ClientCustomize();
                                        DataTable dtAggree = cc.GetDataAgreementsignCandidate(loggedUser.TempPersonalRowID);
                                        if (dtAggree.Rows.Count > 0)
                                        {
                                            loggedUser.ClientUserrowid = Convert.ToInt32(dtAggree.Rows[0]["ClientUserRowID"]);
                                            loggedUser.Agreementsign = Convert.ToBoolean(dtAggree.Rows[0]["Agreementsign"]);
                                            CandidateLinkLapseDays = Convert.ToInt16(dtAggree.Rows[0]["CandidateLinkLapseDays"]);
                                            Session["Agreementsign"] = loggedUser.Agreementsign;
                                        }

                                    }
                                }
                                catch (Exception exception1)
                                {
                                    ViewBag.ErrorMsg = "Userid password not correct";
                                    ClientCustomize cn = new ClientCustomize();
                                    cn.Insert_LoginLog("candidate Login", exception1.Message, model.CRPUserName.Trim(), "UserName: " + model.CRPUserName.Trim() + " Password:   IP Address :" + GetIP_Address());


                                    //ViewBag.ErrorMsg = exception1.Message;
                                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), exception1.Message, exception1.StackTrace);
                                }

                                if (loggedUser == null)
                                {

                                    this.Session.Abandon();
                                    //ViewBag.ErrorMsg = "Invalid Usename / Password";
                                    ViewBag.ErrorMsg = "Login Failed.Try Again!";
                                    clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), UserName, UserPassword, "Candidate");
                                    return RedirectToAction("CandidateLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                }
                                else
                                {
                                    ClientCustomize cc = new ClientCustomize();
                                    DataTable dtClientName = cc.GetClientName(loggedUser.ClientRowID);
                                    if (dtClientName.Rows.Count > 0)
                                    {
                                        Session["ClientName"] = dtClientName.Rows[0]["ClientName"].ToString();
                                    }
                                    else
                                    {
                                        Session["ClientName"] = "";
                                    }
                                    DataTable dtCandidateNameTemp = cc.GetCandidateNameTemp(loggedUser.TempPersonalRowID);
                                    if (dtCandidateNameTemp.Rows.Count > 0)
                                    {
                                        Session["CandidateNameForHeader"] = dtCandidateNameTemp.Rows[0]["CandidateFullName"].ToString();
                                    }
                                    else
                                    {
                                        Session["CandidateNameForHeader"] = "";
                                    }
                                    Session["CadidateUserName"] = UserName;
                                    Session["CandidateUserRowID"] = loggedUser.CandidateUserRowID;
                                    Session["ClientRowID"] = loggedUser.ClientRowID;
                                    Session["ClientUserrowid"] = loggedUser.ClientUserrowid;
                                    Session["HRApproval"] = loggedUser.HRApprovalRequired;
                                    Session["TempPersonalRowID"] = loggedUser.TempPersonalRowID;
                                    Session["LoginCandidateName"] = loggedUser.CandidateName;
                                    Session["CandidateEmail"] = loggedUser.CandidateEmail;
                                    Session["YTRApplicable"] = loggedUser.YTRApplicable;
                                    Session["CreatedBy"] = loggedUser.CreatedBy;
                                    Session["ClientUType"] = "Candidate";
                                    Session["Valid"] = 1;
                                    Session["LoginType"] = "";
                                    Session["Agreementsign"] = loggedUser.Agreementsign;

                                    ///  Add security Filter implementation

                                    String guid = Convert.ToString(Guid.NewGuid());
                                    //Storing new Guid in Session
                                    Session["AuthenticationToken"] = guid;
                                    //Adding Cookie in Browser
                                    Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));

                                    /* Start ConCurrent Session implementation */
                                    Session["SessionTokenCandidate"] = guid;
                                    /* End ConCurrent Session implementation */

                                    if (Convert.ToInt32(Session["Valid"]) == 1)
                                    {
                                        DateTime dateTime = DateTime.Today;
                                        TimeSpan TimeSinceCreation = new TimeSpan();

                                        if (loggedUser.ModifiedTime != null)
                                        {
                                            TimeSinceCreation = dateTime.Subtract(loggedUser.ModifiedTime.Value);
                                        }
                                        else
                                        {
                                            TimeSinceCreation = dateTime.Subtract(loggedUser.CreatedTime.Value);
                                        }
                                    }

                                    AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                                    loggedInDetails.LoginName = loggedUser.CandidateName;
                                    loggedInDetails.LoginType = loggedUser.CreatedBy;
                                    loggedInDetails.LoginId = UserName;
                                    loggedInDetails.LoginBy = "Candidate";
                                    string hostNameMachine = Dns.GetHostName();
                                    int len = Dns.GetHostByName(hostNameMachine).AddressList.Length;
                                    string myIPAddr = "";
                                    for (int i = 0; i <= len - 1; i++)
                                    {
                                        myIPAddr = Dns.GetHostByName(hostNameMachine).AddressList[i].ToString();
                                    }
                                    loggedInDetails.IPDetails = myIPAddr;
                                    loggedInDetails.LoginDate = DateTime.Now;
                                    loggedInDetails.LoginStatus = 1;
                                    loggedInDetails.Deviceidentifiers = Request.UserAgent;

                                    loggedInDetails.BrowserName = Request.Browser.Browser.ToString();


                                    Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);
                                }
                                Session["UserLogin"] = "Candidate User";

                                FormsAuthentication.SetAuthCookie(UserName, false);

                                SessionGenerateForSessionFixation();

                                /* Start ConCurrent Session implementation */
                                ClientCustomize objClientCustomize = new ClientCustomize();
                                int updateClientLogin = objClientCustomize.UpdateUserSessionTokenForCandidate(Convert.ToString(Session["CandidateUserRowID"]), Convert.ToString(Session["SessionTokenCandidate"]));
                                /* End ConCurrent Session implementation] */

                                if (IsValidEmail(UserName))
                                {
                                    Session["CInsufficiency"] = "YES";
                                    //return RedirectToAction("Index", "CandidateInsuff", new { area = "Candidate" });
                                    return RedirectToAction("IndexInsuffCandidateLoginForSessionFixation");
                                }
                                if (loggedUser.ClientRowID == 1015)
                                {
                                    //return RedirectToAction("PersonalInfo", "Home", new { area = "Candidate" });
                                    return RedirectToAction("PersonalInfoCandidateLoginForSessionFixation");
                                }
                                // For TATA Elxsi
                                if (loggedUser.Agreementsign == true || loggedUser.ClientRowID == 635)
                                {
                                    //return RedirectToAction("Index", "Home", new { area = "Candidate" });
                                    return RedirectToAction("IndexHomeCandidateLoginForSessionFixation");
                                }
                                else
                                {
                                    if (loggedUser.ClientRowID == 1747)
                                    {
                                        return RedirectToAction("CandidateIngramCandidateLoginForSessionFixation");
                                    }
                                    else
                                    {
                                        //return RedirectToAction("CandidateFOA", "Home", new { area = "Candidate" });
                                        return RedirectToAction("CandidateFOACandidateLoginForSessionFixation");
                                        //return RedirectToAction("Index", "Home", new { area = "Candidate" });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter Password");
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
                ViewBag.ErrorMsg = "User Id Password is not correct";

                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                ClientCustomize cn = new ClientCustomize();
                cn.Insert_LoginLog("candidate Login", Ex.Message, model.CRPUserName.Trim(), "UserName: " + model.CRPUserName.Trim() + " Password:   IP Address :" + GetIP_Address());

                return RedirectToAction("CandidateLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }

            return View();
        }



        public bool CandidateLoginDirectLiveness(CandidateLoginViewModel model)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(model.CRPPassWord.Trim());
                model.CRPPassWord = Encoding.UTF8.GetString(bytes);

                var captcha = model.CaptchaVal;
                var uname = model.CRPUserName;


                if (ModelState.IsValid)
                {
                    //if (clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) != model.CaptchaVal)
                    //{
                    //    ModelState.AddModelError("", "Captcha validation failed.");
                    //    string msg = clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) + "=" + model.CaptchaVal;
                    //    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), msg, "CandidateLogin");

                    //    return RedirectToAction("CandidateLogin", "Account", new { ErM = clsCommonMethods.PEncrypt("Captcha validation failed") });
                    //}
                    var pword = model.CRPPassWord;
                    if (string.Compare(model.CRPUserName, "", false) == 0 && string.Compare(pword, "", false) == 0)
                    {
                        ModelState.AddModelError("", "Enter User Name and Password");
                    }
                    else if (string.Compare(model.CRPUserName, "", false) == 0 && string.Compare(pword, "", false) != 0)
                    {
                        ModelState.AddModelError("", "Enter User Name");
                    }
                    else if (!(string.Compare(model.CRPUserName, "", false) != 0 && string.Compare(pword, "", false) == 0))
                    {
                        DateTime today = DateTime.Today;
                        string str = model.CRPUserName.Trim().ToLower();
                        string str1 = pword.Trim();
                        string[] strArrays1 = str.Split(' ');
                        string[] strArrays2 = str1.Split(' ');
                        string str2 = strArrays1[0];
                        string str3 = strArrays2[0];

                        if (!(string.Compare(str2, "select", false) == 0 || string.Compare(str2, "exec", false) == 0 || string.Compare(str2, "execute", false) == 0 || string.Compare(str2, "drop", false) == 0 || string.Compare(str2, "truncate", false) == 0 || string.Compare(str2, "insert", false) == 0 || string.Compare(str2, "update", false) == 0 || string.Compare(str2, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                          )
                        {
                            if (!(string.Compare(str3, "select", false) == 0 || string.Compare(str3, "exec", false) == 0 || string.Compare(str3, "execute", false) == 0 || string.Compare(str3, "drop", false) == 0 || string.Compare(str3, "truncate", false) == 0 || string.Compare(str3, "insert", false) == 0 || string.Compare(str3, "update", false) == 0 || string.Compare(str3, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                               )
                            {
                                string UserName = model.CRPUserName.Trim();
                                string UserPassword = pword.Trim();

                                //string pdcry = clsCommonMethods.PasswordDecrypt(str1);
                                string encPassword = clsCommonMethods.PasswordEncrypt(str1);

                                CandidateUserLoggedViewModel loggedUser = new CandidateUserLoggedViewModel();
                                int CandidateLinkLapseDays = 365;


                                try
                                {
                                    //if (IsValidEmail(UserName))
                                    if (UserName != "")
                                    {
                                        //loggedUser = repoAccount.GetInsuffCandidateUserLoggedDetails(UserName, encPassword);
                                        loggedUser = repoAccount.GetCandidateUserLoggedDetails(UserName, encPassword);
                                        if (loggedUser.Status == 3)
                                        {
                                            ViewBag.ErrorMsg = "User is blocked. Please contact to admin!";
                                            ClientCustomize cn = new ClientCustomize();
                                            //cn.Insert_LoginLog("Candidate Login", "User is blocked. Please contact to admin! " + UserName, UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());

                                            return false;
                                        }

                                        if (loggedUser.Status == 0)
                                        {
                                            ViewBag.ErrorMsg = "ID is deactivated. Please contact to admin!";
                                            ClientCustomize cn = new ClientCustomize();
                                            //cn.Insert_LoginLog("Candidate Login", "ID is deactivated. Please contact to admin! " + UserName, UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());

                                            return false;
                                        }

                                        if (loggedUser.Status == 2)
                                        {
                                            ViewBag.ErrorMsg = "Your form has submitted succesfuly. Now you are not able to login. Please conntact to your HR for further querry!";
                                            ClientCustomize cn = new ClientCustomize();
                                            //cn.Insert_LoginLog("Candidate Login", "Your form has submitted succesfuly. Now you are not able to login. Please conntact to your HR for further querry! " + UserName, UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());

                                            return false;
                                        }
                                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), "Account controller UserName : " + UserName, encPassword);
                                        ClientCustomize cc = new ClientCustomize();
                                        DataTable dtAggree = cc.GetDataAgreementsignCandidate(loggedUser.TempPersonalRowID);
                                        if (dtAggree.Rows.Count > 0)
                                        {
                                            loggedUser.ClientUserrowid = Convert.ToInt32(dtAggree.Rows[0]["ClientUserRowID"]);
                                            loggedUser.Agreementsign = Convert.ToBoolean(dtAggree.Rows[0]["Agreementsign"]);
                                            CandidateLinkLapseDays = Convert.ToInt16(dtAggree.Rows[0]["CandidateLinkLapseDays"]);
                                            Session["Agreementsign"] = loggedUser.Agreementsign;
                                        }
                                    }
                                    else
                                    {
                                        loggedUser = repoAccount.GetCandidateUserLoggedDetails(UserName, encPassword);
                                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), "Account controller UserName : " + UserName, encPassword);
                                        ClientCustomize cc = new ClientCustomize();
                                        DataTable dtAggree = cc.GetDataAgreementsignCandidate(loggedUser.TempPersonalRowID);
                                        if (dtAggree.Rows.Count > 0)
                                        {
                                            loggedUser.ClientUserrowid = Convert.ToInt32(dtAggree.Rows[0]["ClientUserRowID"]);
                                            loggedUser.Agreementsign = Convert.ToBoolean(dtAggree.Rows[0]["Agreementsign"]);
                                            CandidateLinkLapseDays = Convert.ToInt16(dtAggree.Rows[0]["CandidateLinkLapseDays"]);
                                            Session["Agreementsign"] = loggedUser.Agreementsign;
                                        }

                                    }
                                }
                                catch (Exception exception1)
                                {
                                    ViewBag.ErrorMsg = "Userid password not correct";
                                    ClientCustomize cn = new ClientCustomize();
                                    //cn.Insert_LoginLog("candidate Login", exception1.Message, model.CRPUserName.Trim(), "UserName: " + model.CRPUserName.Trim() + " Password:   IP Address :" + GetIP_Address());


                                    //ViewBag.ErrorMsg = exception1.Message;
                                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), exception1.Message, exception1.StackTrace);
                                }

                                if (loggedUser == null)
                                {

                                    this.Session.Abandon();
                                    //ViewBag.ErrorMsg = "Invalid Usename / Password";
                                    ViewBag.ErrorMsg = "Login Failed.Try Again!";
                                    //clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), UserName, UserPassword, "Candidate");
                                    return false;
                                }
                                else
                                {
                                    ClientCustomize cc = new ClientCustomize();
                                    DataTable dtClientName = cc.GetClientName(loggedUser.ClientRowID);
                                    if (dtClientName.Rows.Count > 0)
                                    {
                                        Session["ClientName"] = dtClientName.Rows[0]["ClientName"].ToString();
                                    }
                                    else
                                    {
                                        Session["ClientName"] = "";
                                    }
                                    DataTable dtCandidateNameTemp = cc.GetCandidateNameTemp(loggedUser.TempPersonalRowID);
                                    if (dtCandidateNameTemp.Rows.Count > 0)
                                    {
                                        Session["CandidateNameForHeader"] = dtCandidateNameTemp.Rows[0]["CandidateFullName"].ToString();
                                    }
                                    else
                                    {
                                        Session["CandidateNameForHeader"] = "";
                                    }
                                    Session["CadidateUserName"] = UserName;
                                    Session["CandidateUserRowID"] = loggedUser.CandidateUserRowID;
                                    Session["ClientRowID"] = loggedUser.ClientRowID;
                                    Session["ClientUserrowid"] = loggedUser.ClientUserrowid;
                                    Session["HRApproval"] = loggedUser.HRApprovalRequired;
                                    Session["TempPersonalRowID"] = loggedUser.TempPersonalRowID;
                                    Session["LoginCandidateName"] = loggedUser.CandidateName;
                                    Session["CandidateEmail"] = loggedUser.CandidateEmail;
                                    Session["YTRApplicable"] = loggedUser.YTRApplicable;
                                    Session["CreatedBy"] = loggedUser.CreatedBy;
                                    Session["ClientUType"] = "Candidate";
                                    Session["Valid"] = 1;
                                    Session["LoginType"] = "";
                                    Session["Agreementsign"] = loggedUser.Agreementsign;

                                    ///  Add security Filter implementation

                                    String guid = Convert.ToString(Guid.NewGuid());
                                    //Storing new Guid in Session
                                    Session["AuthenticationToken"] = guid;
                                    //Adding Cookie in Browser
                                    Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));

                                    /* Start ConCurrent Session implementation */
                                    Session["SessionTokenCandidate"] = guid;
                                    /* End ConCurrent Session implementation */

                                    if (Convert.ToInt32(Session["Valid"]) == 1)
                                    {
                                        DateTime dateTime = DateTime.Today;
                                        TimeSpan TimeSinceCreation = new TimeSpan();

                                        if (loggedUser.ModifiedTime != null)
                                        {
                                            TimeSinceCreation = dateTime.Subtract(loggedUser.ModifiedTime.Value);
                                        }
                                        else
                                        {
                                            TimeSinceCreation = dateTime.Subtract(loggedUser.CreatedTime.Value);
                                        }
                                    }

                                    AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                                    loggedInDetails.LoginName = loggedUser.CandidateName;
                                    loggedInDetails.LoginType = loggedUser.CreatedBy;
                                    loggedInDetails.LoginId = UserName;
                                    loggedInDetails.LoginBy = "Candidate";
                                    string hostNameMachine = Dns.GetHostName();
                                    int len = Dns.GetHostByName(hostNameMachine).AddressList.Length;
                                    string myIPAddr = "";
                                    for (int i = 0; i <= len - 1; i++)
                                    {
                                        myIPAddr = Dns.GetHostByName(hostNameMachine).AddressList[i].ToString();
                                    }
                                    loggedInDetails.IPDetails = myIPAddr;
                                    loggedInDetails.LoginDate = DateTime.Now;
                                    loggedInDetails.LoginStatus = 1;
                                    loggedInDetails.Deviceidentifiers = Request.UserAgent;

                                    loggedInDetails.BrowserName = Request.Browser.Browser.ToString();


                                    Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);
                                }
                                Session["UserLogin"] = "Candidate User";

                                FormsAuthentication.SetAuthCookie(UserName, false);

                                // SessionGenerateForSessionFixation();
                                //SessionGenerateForSessionFixationNew();


                                /* Start ConCurrent Session implementation */
                                ClientCustomize objClientCustomize = new ClientCustomize();
                                int updateClientLogin = objClientCustomize.UpdateUserSessionTokenForCandidate(Convert.ToString(Session["CandidateUserRowID"]), Convert.ToString(Session["SessionTokenCandidate"]));
                                /* End ConCurrent Session implementation] */


                                if (IsValidEmail(UserName))
                                {
                                    Session["CInsufficiency"] = "YES";
                                    //return RedirectToAction("Index", "CandidateInsuff", new { area = "Candidate" });
                                    return false;
                                }
                                if (loggedUser.ClientRowID == 1015)
                                {
                                    //return RedirectToAction("PersonalInfo", "Home", new { area = "Candidate" });
                                    return false;
                                }
                                // For TATA Elxsi
                                if (loggedUser.Agreementsign == true || loggedUser.ClientRowID == 635)
                                {
                                    //return RedirectToAction("Index", "Home", new { area = "Candidate" });
                                    return false;
                                }
                                else
                                {
                                    if (loggedUser.ClientRowID == 1747)
                                    {
                                        //return RedirectToAction("CandidateIngramCandidateLoginForSessionFixation");
                                    }
                                    else
                                    {
                                        //return RedirectToAction("CandidateFOA", "Home", new { area = "Candidate" });
                                        bool a = CandidateFOACandidateLoginForSessionFixationNew();
                                        if (a == true)
                                        {
                                            return true;

                                        }
                                        else
                                        {
                                            return false;
                                        }
                                        //return RedirectToAction("Index", "Home", new { area = "Candidate" });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter Password");
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
                ViewBag.ErrorMsg = "User Id Password is not correct";

                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                ClientCustomize cn = new ClientCustomize();

            }

            return false;
        }


        public bool CandidateFOACandidateLoginForSessionFixationNew()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["CadidateUserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return true;
        }


        [HttpGet]
        [ValidateInput(false)]
        public ActionResult CLogin(string ErrorMsg = "")
        {
            Session["AuthenticationToken"] = "";
            Session["Counter"] = 0;
            Session["UserLogin"] = "!Candidate User";
            Response.Cookies.Clear();
            if (ErrorMsg != "")
            {
                if (clsCommonMethods.PDecrypt(ErrorMsg) != "")
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(ErrorMsg);
                }
            }
            CandidateLoginViewModel model = new CandidateLoginViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CLogin(CandidateLoginViewModel model)
        {
            try
            {
                model.CRPPassWord = AESEncrytDecry.DecryptStringAES(model.CRPPassWord.Trim());
                var captcha = model.CaptchaVal;
                var uname = model.CRPUserName;


                var pword = model.CRPPassWord;
                if (ModelState.IsValid)
                {
                    if (clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) != model.CaptchaVal)
                    {
                        ModelState.AddModelError("", "Captcha validation failed.");
                        string msg = clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) + "=" + model.CaptchaVal;
                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), msg, "CLogin");
                        return RedirectToAction("CLogin", "Account", new { ErM = clsCommonMethods.PEncrypt("Captcha validation failed") });
                    }
                    if (string.Compare(model.CRPUserName, "", false) == 0 && string.Compare(pword, "", false) == 0)
                    {
                        ModelState.AddModelError("", "Enter User Name and Password");
                    }
                    else if (string.Compare(model.CRPUserName, "", false) == 0 && string.Compare(pword, "", false) != 0)
                    {
                        ModelState.AddModelError("", "Enter User Name");
                    }
                    else if (!(string.Compare(model.CRPUserName, "", false) != 0 && string.Compare(pword, "", false) == 0))
                    {
                        DateTime today = DateTime.Today;
                        string str = model.CRPUserName.Trim().ToLower();
                        string str1 = pword.Trim();
                        string[] strArrays1 = str.Split(' ');
                        string[] strArrays2 = str1.Split(' ');
                        string str2 = strArrays1[0];
                        string str3 = strArrays2[0];

                        if (!(string.Compare(str2, "select", false) == 0 || string.Compare(str2, "exec", false) == 0 || string.Compare(str2, "execute", false) == 0 || string.Compare(str2, "drop", false) == 0 || string.Compare(str2, "truncate", false) == 0 || string.Compare(str2, "insert", false) == 0 || string.Compare(str2, "update", false) == 0 || string.Compare(str2, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0))
                        {
                            if (!(string.Compare(str3, "select", false) == 0 || string.Compare(str3, "exec", false) == 0 || string.Compare(str3, "execute", false) == 0 || string.Compare(str3, "drop", false) == 0 || string.Compare(str3, "truncate", false) == 0 || string.Compare(str3, "insert", false) == 0 || string.Compare(str3, "update", false) == 0 || string.Compare(str3, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0))
                            {
                                string UserName = model.CRPUserName.Trim();
                                string UserPassword = pword.Trim();
                                string encPassword = clsCommonMethods.PasswordEncrypt(str1);
                                CandidateUserLoggedViewModel loggedUser = new CandidateUserLoggedViewModel();
                                int CandidateLinkLapseDays = 365;
                                try
                                {
                                    if (UserName != "")
                                    {
                                        loggedUser = repoAccount.GetCandidateUserLoggedDetails(UserName, encPassword);
                                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), "Account controller UserName : " + UserName, encPassword);
                                        ClientCustomize cc = new ClientCustomize();
                                        DataTable dtAggree = cc.GetDataAgreementsignCandidate(loggedUser.TempPersonalRowID);
                                        if (dtAggree.Rows.Count > 0)
                                        {
                                            loggedUser.Agreementsign = Convert.ToBoolean(dtAggree.Rows[0]["Agreementsign"]);
                                            CandidateLinkLapseDays = Convert.ToInt16(dtAggree.Rows[0]["CandidateLinkLapseDays"]);
                                            Session["Agreementsign"] = loggedUser.Agreementsign;
                                        }
                                    }
                                    else
                                    {
                                        loggedUser = repoAccount.GetCandidateUserLoggedDetails(UserName, encPassword);
                                        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), "Account controller UserName : " + UserName, encPassword);
                                        ClientCustomize cc = new ClientCustomize();
                                        DataTable dtAggree = cc.GetDataAgreementsignCandidate(loggedUser.TempPersonalRowID);
                                        if (dtAggree.Rows.Count > 0)
                                        {
                                            loggedUser.Agreementsign = Convert.ToBoolean(dtAggree.Rows[0]["Agreementsign"]);
                                            CandidateLinkLapseDays = Convert.ToInt16(dtAggree.Rows[0]["CandidateLinkLapseDays"]);
                                            Session["Agreementsign"] = loggedUser.Agreementsign;
                                        }
                                    }
                                }
                                catch (Exception exception1)
                                {
                                    ViewBag.ErrorMsg = "Userid password not correct";
                                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), exception1.Message, exception1.StackTrace);
                                }

                                if (loggedUser == null)
                                {
                                    this.Session.Abandon();
                                    ViewBag.ErrorMsg = "Login Failed.Try Again!";
                                    clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), UserName, UserPassword, "Candidate");
                                    return RedirectToAction("CLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                                }
                                else
                                {
                                    Session["CadidateUserName"] = UserName;
                                    Session["CandidateUserRowID"] = loggedUser.CandidateUserRowID;
                                    Session["ClientRowID"] = loggedUser.ClientRowID;
                                    Session["HRApproval"] = loggedUser.HRApprovalRequired;
                                    Session["TempPersonalRowID"] = loggedUser.TempPersonalRowID;
                                    Session["LoginCandidateName"] = loggedUser.CandidateName;
                                    Session["CandidateEmail"] = loggedUser.CandidateEmail;
                                    Session["YTRApplicable"] = loggedUser.YTRApplicable;
                                    Session["CreatedBy"] = loggedUser.CreatedBy;
                                    Session["ClientUType"] = "Candidate";
                                    Session["Valid"] = 1;
                                    Session["LoginType"] = "Add";
                                    Session["Agreementsign"] = loggedUser.Agreementsign;

                                    ///  Add security Filter implementation

                                    String guid = Convert.ToString(Guid.NewGuid());
                                    //Storing new Guid in Session
                                    Session["AuthenticationToken"] = guid;
                                    //Adding Cookie in Browser
                                    Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));

                                    /* Start ConCurrent Session implementation */
                                    Session["SessionTokenCandidate"] = guid;
                                    /* End ConCurrent Session implementation */

                                    if (Convert.ToInt32(Session["Valid"]) == 1)
                                    {
                                        DateTime dateTime = DateTime.Today;
                                        TimeSpan TimeSinceCreation = new TimeSpan();

                                        if (loggedUser.ModifiedTime != null)
                                        {
                                            TimeSinceCreation = dateTime.Subtract(loggedUser.ModifiedTime.Value);
                                        }
                                        else
                                        {
                                            TimeSinceCreation = dateTime.Subtract(loggedUser.CreatedTime.Value);
                                        }


                                    }

                                    AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                                    loggedInDetails.LoginName = loggedUser.CandidateName;
                                    loggedInDetails.LoginType = loggedUser.CreatedBy;
                                    loggedInDetails.LoginId = UserName;
                                    loggedInDetails.LoginBy = "Candidate";
                                    loggedInDetails.IPDetails = Request.UserHostAddress;
                                    loggedInDetails.LoginDate = DateTime.Now;
                                    loggedInDetails.LoginStatus = 1;
                                    loggedInDetails.Deviceidentifiers = Request.UserAgent;

                                    loggedInDetails.BrowserName = Request.Browser.Browser.ToString();

                                    Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);
                                }
                                Session["UserLogin"] = "Candidate User";

                                FormsAuthentication.SetAuthCookie(UserName, false);

                                SessionGenerateForSessionFixation();

                                /* Start ConCurrent Session implementation */
                                ClientCustomize objClientCustomize = new ClientCustomize();
                                int updateClientLogin = objClientCustomize.UpdateUserSessionTokenForCandidate(Convert.ToString(Session["CandidateUserRowID"]), Convert.ToString(Session["SessionTokenCandidate"]));
                                /* End ConCurrent Session implementation] */

                                return RedirectToAction("CLoginReIndexLoginForSessionFixation");
                                //return RedirectToAction("ReIndex", "Home", new { area = "Candidate" });
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter Password");
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
                ViewBag.ErrorMsg = "User Id Password is not correct";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return RedirectToAction("CLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }

            return View();
        }




        [HttpGet]
        public ActionResult LogOutCandidate()
        {
            try
            {
                Session["AuthenticationToken"] = "";
                Response.Cookies.Clear();
                Session["Counter"] = 0;
                //UpdateLogOutDetails();
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "you have logged out successfully";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return RedirectToAction("CandidateLogin");
        }

        [HttpGet]
        public ActionResult CandidateLogOut()
        {
            try
            {
                Session["AuthenticationToken"] = "";
                Response.Cookies.Clear();
                Session["Counter"] = 0;
                //UpdateLogOutDetails();
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "you have logged out successfully";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return RedirectToAction("CLogin");
        }

        [HttpGet]
        public ActionResult CandidateTimeoutRedirect()
        {
            return View();
        }

        #endregion

        //Common Method for testing

        [NonAction]
        private void UpdateLogOutDetails()
        {
            try
            {
                DictionaryEntry dictionaryEntry = new DictionaryEntry();
                DictionaryEntry dictionaryEntry1;

                UpdateUserLogOutDetailViewModel model = new UpdateUserLogOutDetailViewModel();
                model.LoginDetailRowID = Convert.ToInt32(Session["LoginDetailRowID"]);
                model.LogoutTime = DateTime.Now;

                FormsAuthentication.SignOut();

                this.Session.Clear();
                this.Session.Abandon();

                // Clear authentication cookie.
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                cookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie);

                IDictionaryEnumerator enumerator = HttpContext.Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (current != null)
                    {
                        dictionaryEntry1 = (DictionaryEntry)current;
                    }
                    else
                    {
                        dictionaryEntry1 = dictionaryEntry;
                    }
                    DictionaryEntry dictionaryEntry2 = dictionaryEntry1;
                    HttpContext.Cache.Remove(dictionaryEntry2.Key.ToString());
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoServerCaching();
                Response.Cache.SetNoStore();
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                throw Ex;
            }
        }

        [NonAction]
        private void UpdateLogOutDetails(int LoginDetailRowID)
        {
            try
            {
                DictionaryEntry dictionaryEntry = new DictionaryEntry();
                DictionaryEntry dictionaryEntry1;

                UpdateUserLogOutDetailViewModel model = new UpdateUserLogOutDetailViewModel();
                model.LoginDetailRowID = LoginDetailRowID;
                model.LogoutTime = DateTime.Now;

                repoAccount.UpdateUserLogOutDetails(model);

                FormsAuthentication.SignOut();

                this.Session.Clear();
                this.Session.Abandon();

                // Clear authentication cookie.
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                cookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie);

                IDictionaryEnumerator enumerator = HttpContext.Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (current != null)
                    {
                        dictionaryEntry1 = (DictionaryEntry)current;
                    }
                    else
                    {
                        dictionaryEntry1 = dictionaryEntry;
                    }
                    DictionaryEntry dictionaryEntry2 = dictionaryEntry1;
                    HttpContext.Cache.Remove(dictionaryEntry2.Key.ToString());
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoServerCaching();
                Response.Cache.SetNoStore();
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                throw Ex;
            }
        }

        #region *********************** Vendor Login *********************

        // GET: Account
        [HttpGet]
        public ActionResult VendorLogin()
        {
            Session["AuthenticationToken"] = "";
            Response.Cookies.Clear();
            Session["Counter"] = 0;
            Session["UserLogin"] = "!Partner User";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VendorLogin(VendorLoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string aa = model.CRPPassWord;
                    if (string.Compare(model.CRPUserName, "", false) == 0 && string.Compare(aa, "", false) == 0)
                    {
                        ModelState.AddModelError("", "Enter User Name and Password");
                    }
                    else if (string.Compare(model.CRPUserName, "", false) == 0 && string.Compare(aa, "", false) != 0)
                    {
                        ModelState.AddModelError("", "Enter User Name");
                    }
                    else if (!(string.Compare(model.CRPUserName, "", false) != 0 && string.Compare(aa, "", false) == 0))
                    {
                        DateTime today = DateTime.Today;
                        string str = model.CRPUserName.Trim().ToLower();
                        string str1 = aa.Trim();
                        string[] strArrays1 = str.Split(' ');
                        string[] strArrays2 = str1.Split(' ');
                        string str2 = strArrays1[0];
                        string str3 = strArrays2[0];

                        if (!(string.Compare(str2, "select", false) == 0 || string.Compare(str2, "exec", false) == 0 || string.Compare(str2, "execute", false) == 0 || string.Compare(str2, "drop", false) == 0 || string.Compare(str2, "truncate", false) == 0 || string.Compare(str2, "insert", false) == 0 || string.Compare(str2, "update", false) == 0 || string.Compare(str2, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                          )
                        {
                            if (!(string.Compare(str3, "select", false) == 0 || string.Compare(str3, "exec", false) == 0 || string.Compare(str3, "execute", false) == 0 || string.Compare(str3, "drop", false) == 0 || string.Compare(str3, "truncate", false) == 0 || string.Compare(str3, "insert", false) == 0 || string.Compare(str3, "update", false) == 0 || string.Compare(str3, "delete", false) == 0 || string.Compare(str2, "union", false) == 0 || string.Compare(str2, "create", false) == 0)
                               )
                            {
                                string UserName = model.CRPUserName.Trim();
                                string UserPassword = str1;
                                string encPassword = clsCommonMethods.PasswordEncrypt(str1);

                                VendorUserLoggedViewModel loggedUser = new VendorUserLoggedViewModel();
                                try
                                {
                                    loggedUser = repoAccount.GetVendorUserLoggedDetails(UserName, encPassword);
                                    string loginTime = new AccountRepository().GetLastLoginDetails(UserName);
                                    Session["LastLoginTime"] = loginTime;
                                }
                                catch (Exception exception1)
                                {
                                    //ViewBag.ErrorMsg = exception1.Message;
                                    ViewBag.ErrorMsg = "Userid password not correct";
                                    ClientCustomize cn = new ClientCustomize();
                                    cn.Insert_LoginLog("Vendor Login", exception1.Message, model.CRPUserName.Trim(), "UserName: " + model.CRPUserName.Trim() + " Password:   IP Address :" + GetIP_Address());


                                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), exception1.Message, exception1.StackTrace);
                                }

                                if (loggedUser == null)
                                {
                                    var activWebUsers = repoAccount.GetAllActiveVendorUsers();
                                    foreach (var webUser in activWebUsers)
                                    {
                                        if (!(webUser.CRPUserName.Trim() == UserName) || !(webUser.CRPPassWord.Trim() != encPassword))
                                        {
                                            continue;
                                        }

                                        Session["Counter"] = Convert.ToInt32(Session["Counter"]) + 1;
                                        break;
                                    }

                                    if (!(Convert.ToInt32(Session["Counter"]) >= 3))
                                    {
                                        ViewBag.ErrorMsg = "Login Failed.Try Again!";
                                        clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), UserName, UserPassword, "Vendor");
                                        return View();
                                    }
                                    else
                                    {
                                        Session["Counter"] = 0;
                                        UpdateVendorUserBlockStatusViewModel userBlocked = new UpdateVendorUserBlockStatusViewModel();
                                        userBlocked.UserID = UserName;
                                        userBlocked.BlockedDate = DateTime.Now;
                                        userBlocked.Status = 3;

                                        repoAccount.BlockedVendorUser(userBlocked);
                                        repoAccount.SaveChanges();

                                        ViewBag.ErrorMsg = "User has been Blocked, Please contact to Administrator!";
                                        Session.Abandon();

                                        ClientCustomize cn = new ClientCustomize();
                                        cn.Insert_LoginLog("Vendor Login", "User has been Blocked, Please contact to Administrator! ", UserName, "UserName: " + UserName + " Password:   IP Address :" + GetIP_Address());

                                        return View();
                                    }
                                }
                                else
                                {
                                    Session["UserName"] = UserName;
                                    Session["VendorLoginRowID"] = loggedUser.VendorLoginRowID;
                                    Session["VendorRowID"] = loggedUser.VendorRowID;
                                    Session["VendorName"] = loggedUser.VendorName;
                                    Session["VendorEmaiID"] = loggedUser.VendorEmaiID;
                                    Session["ContactPerson"] = loggedUser.ContactPerson;
                                    Session["MobileNo"] = loggedUser.MobileNo;
                                    Session["UserType"] = loggedUser.UserType;
                                    Session["CreatedBy"] = loggedUser.CreatedBy;
                                    Session["Valid"] = 1;

                                    ///  Add security Filter implementation

                                    String guid = Convert.ToString(Guid.NewGuid());
                                    //Storing new Guid in Session
                                    Session["AuthenticationToken"] = guid;
                                    //Adding Cookie in Browser
                                    Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));

                                    Session["SessionTokenVendor"] = guid;

                                    if (Convert.ToInt32(Session["Valid"]) == 1)
                                    {
                                        DateTime dateTime = DateTime.Today;
                                        TimeSpan TimeSinceCreation = new TimeSpan();

                                        if (loggedUser.ModifiedTime != null)
                                        {
                                            TimeSinceCreation = dateTime.Subtract(loggedUser.ModifiedTime.Value);
                                        }
                                        else
                                        {
                                            TimeSinceCreation = dateTime.Subtract(loggedUser.CreatedTime.Value);
                                        }

                                        if (TimeSinceCreation.Days < 0)
                                        {
                                            clsCommonMethods.UserLog(Server.MapPath("~\\ErrorLogs\\Unauthorized_UserLog"), UserName, UserPassword, "Vendor");
                                            ViewBag.ErrorMsg = "Invalid login. Please Try again!";
                                            Session.Abandon();
                                            return View();
                                        }
                                    }

                                    AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                                    loggedInDetails.LoginName = loggedUser.VendorName;
                                    if (loggedUser.UserType == 0)
                                    {
                                        loggedInDetails.LoginType = "Vendor";
                                    }
                                    else
                                    {
                                        loggedInDetails.LoginType = "Vendor's User";
                                    }

                                    loggedInDetails.LoginId = UserName;
                                    loggedInDetails.LoginBy = "Vendor";
                                    loggedInDetails.IPDetails = Request.UserHostAddress;
                                    loggedInDetails.LoginDate = DateTime.Now;
                                    loggedInDetails.LoginStatus = 1;
                                    loggedInDetails.Deviceidentifiers = Request.UserAgent;
                                    loggedInDetails.BrowserName = Request.Browser.Browser.ToString();


                                    Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);
                                }
                                Session["UserLogin"] = "Partner User";
                                FormsAuthentication.SetAuthCookie(UserName, false);

                                SessionGenerateForSessionFixation();
                                /* Start ConCurrent Session implementation */
                                ClientCustomize objClientCustomize = new ClientCustomize();
                                int updateClientLogin = objClientCustomize.UpdateUserSessionTokenForVendorLogin(Convert.ToString(Session["VendorRowID"]), Convert.ToString(Session["SessionTokenVendor"]));
                                /* End ConCurrent Session implementation] */
                                return RedirectToAction("VendorLoginForSessionFixation");

                                //return RedirectToAction("Index", "Home", new { area = "Partner" });
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter Password");
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
                ViewBag.ErrorMsg = "Userid password not correct";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                ClientCustomize cn = new ClientCustomize();
                cn.Insert_LoginLog("Vendor Login", Ex.Message, model.CRPUserName.Trim(), "UserName: " + model.CRPUserName.Trim() + " Password:   IP Address :" + GetIP_Address());

                return RedirectToAction("VendorLogin", "Account", new { area = "Partner" });
            }

            return View();
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult LogOutVendor()
        {
            try
            {
                Session["AuthenticationToken"] = "";
                Response.Cookies.Clear();
                Session["Counter"] = 0;
                //UpdateLogOutDetails();
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "you have logged out successfully";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return RedirectToAction("VendorLogin");
        }

        [HttpGet]
        public ActionResult VendorTimeoutRedirect()
        {
            return View();
        }

        #endregion


        #region NonAction

        [NonAction]
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

        #endregion

        [NonAction]
        public FileResult DownloadMobApp()
        {
            byte[] fileBytes = new byte[] { };
            string fileName = "Securitas-BGV.apk";
            try
            {
                fileBytes = System.IO.File.ReadAllBytes(@"D:\MobileApp\Securitas-BGV.apk");


            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpGet]
        public ActionResult ApplicationError()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ErrorFound()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ExtendSession()
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
            var data = new { IsSuccess = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult closeSession()
        {
            Session.Abandon();
            var data = new { IsSuccess = true };
            return Json(data, JsonRequestBehavior.AllowGet);


        }

        public ActionResult CandidateMedia(string Key = "")
        {
            try
            {
                string a = clsCommonMethods.PasswordDecrypt(Key);
                string[] Keys = a.Split('_');
                ClientCustomize cc = new ClientCustomize();
                DataTable dts = cc.GetMailAuthenticateDetail(Keys[0], Keys[1], Keys[2]);
                if (dts.Rows.Count == 0)
                {
                    return RedirectToAction("Index", "Account");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Account");
            }

            return View();
        }


        [HttpGet]
        public ActionResult AppInfo()
        {
            return View();
        }

        #region Insuff Candidate Information
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult InsuffCandidateLogin(string cpid = "")
        {
            initializeSession();
            ////var strEnccid = clsCommonMethods.PEncrypt("2;35506");
            ClientCustomize cc = new ClientCustomize();
            var objs = new CandidateInsuffLoginViewModel();
            if (!string.IsNullOrEmpty(cpid))
            {
                string[] strCPID = clsCommonMethods.PDecrypt(cpid).Split(';');
                if (strCPID.Length > 1)
                {
                    var tbl = cc.GetClientCandidateName(Convert.ToInt32(strCPID[0]), Convert.ToInt64(strCPID[1]));
                    if (tbl.Rows.Count > 0)
                    {
                        objs.ClientName = Convert.ToString(tbl.Rows[0]["ClientName"]);
                        objs.CandidateName = Convert.ToString(tbl.Rows[0]["CandidateName"]);
                    }
                }

            }

            objs.cpid = cpid;

            return View(objs);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsuffCandidateLogin(CandidateInsuffLoginViewModel objs)
        {

            ClientCustomize cc = new ClientCustomize();
            DataTable tbl = new DataTable("tbl");
            string cpid = objs.cpid;
            if (!string.IsNullOrEmpty(cpid))
            {
                string[] strCPID = clsCommonMethods.PDecrypt(cpid).Split(';');
                if (strCPID.Length > 1)
                {
                    string pwd = clsCommonMethods.PasswordEncrypt(objs.pwd);
                    tbl = cc.GetPQCandidateInsuffLoginsInfo(Convert.ToInt64(strCPID[1]), objs.UserName, pwd);
                }
                if (tbl.Rows.Count > 0)
                {

                    Session["ClientRowId"] = Convert.ToInt32(tbl.Rows[0]["ClientRowID"]);
                    Session["PersonalRowID"] = Convert.ToInt32(tbl.Rows[0]["PersonalRowID"]);
                    Session["CheckFamilyRowID"] = Convert.ToInt32(tbl.Rows[0]["CheckFamilyRowID"]);
                    Session["SubCheckRowID"] = Convert.ToInt32(tbl.Rows[0]["SubCheckRowID"]);
                    Session["UserName"] = Convert.ToString(tbl.Rows[0]["CandidateName"]);

                    ///  Add security Filter implementation

                    String guid = Convert.ToString(Guid.NewGuid());
                    //Storing new Guid in Session
                    Session["AuthenticationToken"] = guid;
                    //Adding Cookie in Browser
                    Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));

                    return RedirectToAction("GetInsufficiencyCheckInfoByCandidate", "ClientInsuff", new { area = "ClientArea" });

                }
            }


            return View(objs);
        }
        private void initializeSession()
        {
            Session["ClientUserRowID"] = 0;
            Session["ClientRowId"] = 0;
            Session["PersonRowID"] = 0;
            Session["CheckFamilyRowID"] = 0;
            Session["SubCheckRowID"] = 0;
            Session["UserName"] = "";
            Session["ClientName"] = "";
            Session["AuthenticationToken"] = "";
            Session["encryptedSession"] = "";

        }

        #endregion

        public ActionResult GetReportInterimLink(string ImageName, string cid)
        {
            try
            {
                string str = clsCommonMethods.PDecrypt(cid);
                string imgname = (clsCommonMethods.PDecrypt(ImageName));
                string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
                byte[] fileBytes = null;
                ClientCustomize cc = new ClientCustomize();
                DataTable ddt = cc.getAutoExistCompRefNo(imgname);
                if (ddt.Rows.Count > 0)
                {
                    imgname = ddt.Rows[0]["ReportFileName"].ToString();
                    FldPathDoc = Server.MapPath(FldPathDoc) + imgname;
                }
                else
                {
                    FldPathDoc = FldPathDoc.Replace("wwwroot2", "wwwroot");
                    FldPathDoc = Server.MapPath(FldPathDoc) + imgname + ".pdf";
                }

                fileBytes = System.IO.File.ReadAllBytes(FldPathDoc);
                if (fileBytes != null)
                {
                    return File(fileBytes, MediaTypeNames.Application.Octet, repoDashboardClient.downloadReportFileName(imgname) + ".pdf"); ;

                }
                else
                {
                    return RedirectToAction("Index", "ReportDownload");
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return RedirectToAction("Index", "ReportDownload");
            }

        }
        public ActionResult GetCRCReportLink(string pID, string key)
        {

            try
            {
                string keyId = clsCommonMethods.PDecrypt(key);
                string personalRowID = (clsCommonMethods.PDecrypt(pID));
                string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CRCReport"].ToString();
                FldPathDoc = Server.MapPath(FldPathDoc) + personalRowID + ".pdf";
                byte[] fileBytes = null;

                fileBytes = System.IO.File.ReadAllBytes(FldPathDoc);
                if (fileBytes != null)
                {
                    return File(fileBytes, MediaTypeNames.Application.Octet, repoDashboardClient.downloadReportFileName(personalRowID) + ".pdf");
                }
                else
                {
                    return RedirectToAction("Index", "ReportDownload");
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return RedirectToAction("Index", "ReportDownload");
            }

        }


        public JsonResult GetClientLoginDetails()
        {
            List<CompanyLoginDetails> objData = new List<CompanyLoginDetails>();

            DataTable dt = new DataTable();
            dt = DAL.DAS.ExecuteDataTable(DAL.DAS.ConnectionString, CommandType.StoredProcedure, "GetClientLoginDetails"
            );
            if (dt.Rows.Count > 0)
            {
                objData = (from DataRow dr in dt.Rows
                           select new CompanyLoginDetails()
                           {
                               WebUserRowID = Convert.ToInt16(dr["ClientUserRowID"]),
                               crpUN = Convert.ToString(dr["UserID"]),
                               crpPWD = clsCommonMethods.PasswordDecrypt(Convert.ToString(dr["UPass"]))//Convert.ToString(dr["UPass"])

                           }).ToList();
            }

            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        private const string SP_PARAM_RETURN_VALUE = "@ReturnValue";
        public JsonResult UpdateClientLoginDetails(int id, string passSHA)
        {
            int intReturnValue = 0;
            System.Data.SqlClient.SqlParameter objReturnValueParam = DAL.DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAL.DAS.ExecuteNonQuery(DAL.DAS.ConnectionString, CommandType.StoredProcedure, "UpdateClientLoginDetails",
            DAL.DAS.CreateParameter("@ClientUserRowID", SqlDbType.BigInt, id, ParameterDirection.Input, 50, false),
            DAL.DAS.CreateParameter("@UPass", SqlDbType.VarChar, passSHA, ParameterDirection.Input, 1000, false),
            objReturnValueParam
            );
            intReturnValue = (int)objReturnValueParam.Value;

            return Json(intReturnValue, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetWebUsersLoginDetails()
        {
            List<CompanyLoginDetails> objData = new List<CompanyLoginDetails>();

            DataTable dt = new DataTable();
            dt = DAL.DAS.ExecuteDataTable(DAL.DAS.ConnectionString, CommandType.StoredProcedure, "GetWebUsersLoginDetails"
            );
            if (dt.Rows.Count > 0)
            {
                objData = (from DataRow dr in dt.Rows
                           select new CompanyLoginDetails()
                           {
                               WebUserRowID = Convert.ToInt16(dr["WebUserRowID"]),
                               crpUN = Convert.ToString(dr["CRPUserName"]),
                               crpPWD = clsCommonMethods.PasswordDecrypt(Convert.ToString(dr["CRPPassWord"]))// Convert.ToString(dr["CRPPassWord"])

                           }).ToList();
            }

            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MessageAction()
        {
            return View();
        }
        public JsonResult UpdateWebUsersLoginDetails(int id, string passSHA)
        {
            int intReturnValue = 0;
            System.Data.SqlClient.SqlParameter objReturnValueParam = DAL.DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAL.DAS.ExecuteNonQuery(DAL.DAS.ConnectionString, CommandType.StoredProcedure, "UpdateWebUserLoginDetails",
            DAL.DAS.CreateParameter("@WebUserRowID", SqlDbType.BigInt, id, ParameterDirection.Input, 50, false),
            DAL.DAS.CreateParameter("@UPass", SqlDbType.VarChar, passSHA, ParameterDirection.Input, 1000, false),

            objReturnValueParam
            );
            intReturnValue = (int)objReturnValueParam.Value;

            return Json(intReturnValue, JsonRequestBehavior.AllowGet);
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
                           new SelectListItem{Text="AAdhar Card", Value="AAdhar Card"},
                            new SelectListItem{Text="Others", Value="Others"}
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
                //ViewBag.ErrorMsg = Ex.Message;
                //clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace)
                throw Ex;
            }
        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult InsufficiencyClearByClient(string id = "", string ClientRowId = "", string ChFamilRowId = "", string subCheckId = "", string PersonalRowID = "", string CompanyRefNo = "", string InfRemarks = "", string actionFrom = "")
        {
            InsuffCheckClearViewModel model = new InsuffCheckClearViewModel();
            try
            {
                ViewBag.CheckStatus = false;
                ClientCustomize cc = new ClientCustomize();
                short Did = 0;
                Did = Convert.ToInt16(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(ClientRowId)));
                int Id = id != "" ? Convert.ToInt32(clsCommonMethods.PDecrypt(id)) : 0;//Convert.ToInt32(clsCommonMethods.PDecrypt(id));
                short CheckFamilyId = Convert.ToInt16(clsCommonMethods.PDecrypt(ChFamilRowId));
                short SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId));
                string actionFromRequest = "";
                int perId = 0;
                if (PersonalRowID.Length == 6)
                {
                    perId = Convert.ToInt32(PersonalRowID);
                }
                else
                {
                    perId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
                }

                string RefNo = (clsCommonMethods.PDecrypt(CompanyRefNo));
                string IRemark = (clsCommonMethods.PDecrypt(InfRemarks));
                actionFromRequest = clsCommonMethods.PDecrypt(actionFrom);
                Session["ClientName"] = "";
                DataTable DTCheckStatus = cc.GetCandidateCheckStatus(perId, SubCheckId);
                if (DTCheckStatus.Rows.Count > 0)
                {
                    Session["ClientName"] = DTCheckStatus.Rows[0]["ClientName"].ToString();
                }
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
                    model.actionFrom = actionFromRequest;
                    if (SubCheckId > 0) { model.SubCheckName = repoSubCheck.GetSubCheckFamilyForUpdateById(SubCheckId).SubCheckName; }
                }
                if (DTCheckStatus.Rows.Count > 0)
                {
                    ViewBag.CheckStatus = true;
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult InsufficiencyClearByClient(InsuffCheckClearViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        //{
        //    try
        //    {
        //        ViewBag.ErrorMsg = "";
        //        if (ModelState.IsValid)
        //        {
        //            if (UploadDoc == null)
        //            {
        //                ViewBag.ErrorMsg = "Invalid Document!";
        //                return RedirectToAction("InsufficiencyClearByClient", "Account", new { ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()), ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()), subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString()) , actionFrom = clsCommonMethods.PEncrypt(model.actionFrom.ToString()) });
        //        }
        //            ClientCustomize cc = new ClientCustomize();
        //            foreach (var item in UploadDoc)
        //            {
        //                string errorMessage = GatZipFileMessage(item, model.PersonalRowID.ToString(), model.ChFamilRowId.ToString());
        //                if (errorMessage != "")
        //                {
        //                    ViewBag.ErrorMsg = errorMessage;
        //                }
        //            }
        //            if (ViewBag.ErrorMsg == "")
        //            {
        //                if (model.PersonalRowID > 0 && ((!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null) || (!string.IsNullOrEmpty(model.Remarks))))
        //                {
        //                    if (!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
        //                    {
        //                        int count = 0;
        //                        string[] DocumentTypes = UploadHid.Split(',');
        //                        foreach (var item in UploadDoc)
        //                        {
        //                            model.FileName = item.FileName.ToString();
        //                            model.DocumentType = DocumentTypes[count];
        //                            if (model.DocumentType != null && model.FileName != null && item.ContentLength > 0)
        //                            {
        //                                SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "Provided", model.DocumentType, "provideddocuments", item, model.Remarks, 0, model.SecuritasID);
        //                            }
        //                            else
        //                            {
        //                                ViewBag.ErrorMsg = "Invalid Document!";
        //                                break;
        //                            }
        //                            count++;
        //                        }

        //                        if (string.IsNullOrEmpty(model.Remarks))
        //                        {
        //                            model.Remarks = "Insuff Document Uploaded By Client";
        //                        }
        //                    }
        //                }
        //                #region Save LogTransaction Details

        //                int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
        //                if (j == 11)
        //                {
        //                    //43 for L1 insuff
        //                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "Client", model.ClientRowID);//45 means L1 Insuff
        //                    cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.Remarks, "L1-Client", model.ClientRowID);
        //                    cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
        //                    cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
        //                    ////SendAlertMassageForInsuffClear(model.PersonalRowID, model.SubCheckRowID, model.Remarks,"L1 Insuff");
        //                    new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
        //                }
        //                else if (j == 22)
        //                {
        //                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 56, "Client", model.ClientRowID);// 56 means L2 insuff
        //                    cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 56, model.Remarks, "L2-Client", model.ClientRowID);
        //                    cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
        //                    cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
        //                    //SendAlertMassageForInsuffClearMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks, "L2 Insuff");
        //                    new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
        //                }
        //                clsCommonMethods.AddPQLogTrasactionByClient(model.PersonalRowID, "InsufficiencyClearByClient", "Insuff Clear By Client", model.DocumentType, "Insert");
        //                #endregion

        //                #region Case/Check Action History Save 
        //                if (model.actionFrom == "InsuffBulkJob") // to be passed this "InsuffBulkJob" in variable actionFrom from insuff mail in case bulk jobs only
        //                {
        //                    clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Candidate", model.Remarks);
        //                }
        //                else
        //                {
        //                    clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Client", model.Remarks);
        //                }
        //                #endregion

        //                int PID = model.PersonalRowID;
        //                TempData["SuccessMsg"] = "Record Added Successfully!";
        //                ModelState.Clear();
        //                return RedirectToAction("MessageAction", "Account");
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
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //    return View(model);
        //}

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsufficiencyClearByClient(InsuffCheckClearViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                ViewBag.ErrorMsg = "";
                if (ModelState.IsValid)
                {
                    //if (UploadDoc == null)
                    //{
                    //    ViewBag.ErrorMsg = "Invalid Document!";
                    //    return RedirectToAction("InsufficiencyClearByClient", "Account", new { ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()), ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()), subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString()) });
                    //}
                    ClientCustomize cc = new ClientCustomize();
                    if (UploadDoc != null)
                    {
                        foreach (var item in UploadDoc)
                        {
                            if (item != null && item.ContentLength > 0)
                            {
                                string errorMessage = GatZipFileMessage(item, model.PersonalRowID.ToString(), model.ChFamilRowId.ToString());
                                if (!string.IsNullOrEmpty(errorMessage))
                                {
                                    ViewBag.ErrorMsg = errorMessage;
                                }
                            }
                        }
                    }

                    if (ViewBag.ErrorMsg == "")
                    {

                        if (model.PersonalRowID > 0 && ((!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null) || !string.IsNullOrEmpty(model.Remarks)))
                        {
                            ////if (!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
                            //{
                            //	int count = 0;
                            //                         string[] DocumentTypes = UploadHid.Split(',');
                            //                         foreach (var item in UploadDoc)
                            //                         {
                            //                             model.FileName = item.FileName.ToString();
                            //                             model.DocumentType = DocumentTypes[count];
                            //                             if (model.DocumentType != null && model.FileName != null && item.ContentLength > 0)
                            //                             {
                            //                                 SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "Provided", model.DocumentType, "provideddocuments", item, model.Remarks, 0, model.SecuritasID);
                            //                             }
                            //                             else
                            //                             {
                            //                                 ViewBag.ErrorMsg = "Invalid Document!";
                            //                                 break;
                            //                             }
                            //                             count++;
                            //                         }

                            //                         if (string.IsNullOrEmpty(model.Remarks))
                            //                         {
                            //                             model.Remarks = "Insuff Document Uploaded By Client";
                            //                         }
                            //                     }
                            //                 }


                            {
                                if (!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
                                {
                                    int count = 0;
                                    string[] DocumentTypes = UploadHid.Split(',');
                                    foreach (var item in UploadDoc)
                                    {
                                        if (item != null && item.ContentLength > 0)
                                        {
                                            model.FileName = item.FileName;
                                            model.DocumentType = DocumentTypes[count];
                                            SavePQVerifiedUploadDoc(model.PersonalRowID, model.SubCheckRowID, "Provided", model.DocumentType,
                                                                    "provideddocuments", item, model.Remarks, 0, model.SecuritasID);
                                        }
                                        count++;
                                    }

                                    if (string.IsNullOrEmpty(model.Remarks))
                                    {
                                        model.Remarks = "Insuff Document Uploaded By Client";
                                    }
                                }
                            }
                            #region Save LogTransaction Details

                            int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                            if (j == 11)
                            {
                                //43 for L1 insuff
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "Client", model.ClientRowID);//45 means L1 Insuff
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.Remarks, "L1-Client", model.ClientRowID);
                                cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                                cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                                ////SendAlertMassageForInsuffClear(model.PersonalRowID, model.SubCheckRowID, model.Remarks,"L1 Insuff");
                                new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                            }
                            else if (j == 22)
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 56, "Client", model.ClientRowID);// 56 means L2 insuff
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 56, model.Remarks, "L2-Client", model.ClientRowID);
                                cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                                cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                                //SendAlertMassageForInsuffClearMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks, "L2 Insuff");
                                new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                            }
                            clsCommonMethods.AddPQLogTrasactionByClient(model.PersonalRowID, "InsufficiencyClearByClient", "Insuff Clear By Client", model.DocumentType, "Insert");
                            #endregion

                            #region Case/Check Action History Save 
                            if (model.actionFrom == "InsuffBulkJob") // to be passed this "InsuffBulkJob" in variable actionFrom from insuff mail in case bulk jobs only
                            {
                                clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Candidate", model.Remarks);
                            }
                            else
                            {
                                clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Client", model.Remarks);
                            }
                            #endregion

                            int PID = model.PersonalRowID;
                            TempData["SuccessMsg"] = "Record Added Successfully!";
                            ModelState.Clear();
                            return RedirectToAction("InsufficiencyClearByClient", "Account",
                                new
                                {
                                    ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()),
                                    ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()),
                                    subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()),
                                    PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString())
                                });

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
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult InsuffClear(string id = "", string CRId = "", string ChRId = "", string sCId = "", string PRID = "", string CRNo = "", string InfR = "")
        {
            InsuffCheckClearViewModel model = new InsuffCheckClearViewModel();
            try
            {
                ViewBag.CheckStatus = false;
                ClientCustomize cc = new ClientCustomize();
                short Did = 0;
                Did = Convert.ToInt16(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(CRId)));
                int Id = id != "" ? Convert.ToInt32(clsCommonMethods.PDecrypt(id)) : 0;//Convert.ToInt32(clsCommonMethods.PDecrypt(id));
                short CheckFamilyId = Convert.ToInt16(clsCommonMethods.PDecrypt(ChRId));
                short SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(sCId));
                int perId = Convert.ToInt32(clsCommonMethods.PDecrypt(PRID));
                string RefNo = (clsCommonMethods.PDecrypt(CRNo));
                string IRemark = (clsCommonMethods.PDecrypt(InfR));
                Session["ClientName"] = "";
                DataTable DTCheckStatus = cc.GetCandidateCheckStatus(perId, SubCheckId);
                if (DTCheckStatus.Rows.Count > 0)
                {
                    Session["ClientName"] = DTCheckStatus.Rows[0]["ClientName"].ToString();
                }
                if (model != null)
                {
                    DataTable dt = cc.GetInsuffRemarks(perId, SubCheckId);
                    if (dt.Rows.Count > 0)
                    {
                        InfR = dt.Rows[0]["Remarks"].ToString();
                    }
                    model.CheckRowID = Id;
                    model.ClientRowID = Did;
                    model.ChFamilRowId = CheckFamilyId;
                    model.PersonalRowID = perId;
                    model.SubCheckRowID = SubCheckId;
                    model.SecuritasID = RefNo;
                    model.INFReason = InfR;
                    model.SubCheckName = "";
                    if (SubCheckId > 0) { model.SubCheckName = repoSubCheck.GetSubCheckFamilyForUpdateById(SubCheckId).SubCheckName; }
                }
                if (DTCheckStatus.Rows.Count > 0)
                {
                    ViewBag.CheckStatus = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsuffClear(InsuffCheckClearViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                ViewBag.ErrorMsg = "";
                if (ModelState.IsValid)
                {
                    if (UploadDoc == null)
                    {
                        ViewBag.ErrorMsg = "Invalid Document!";
                        return RedirectToAction("InsuffClear", "Account", new { ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()), ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()), subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString()) });
                    }
                    ClientCustomize cc = new ClientCustomize();
                    foreach (var item in UploadDoc)
                    {
                        string errorMessage = GatZipFileMessage(item, model.PersonalRowID.ToString(), model.ChFamilRowId.ToString());
                        if (errorMessage != "")
                        {
                            ViewBag.ErrorMsg = errorMessage;
                        }
                    }
                    if (ViewBag.ErrorMsg == "")
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

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasactionByClient(model.PersonalRowID, "InsuffClear", "Insuff Clear By Client", model.DocumentType, "Insert");
                            int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                            if (j == 11)
                            {
                                //43 for L1 insuff
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "Client", model.ClientRowID);//45 means L1 Insuff
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.Remarks, "L1-Client", model.ClientRowID);
                                cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                                cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                                new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                            }
                            else if (j == 22)
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 56, "Client", model.ClientRowID);// 56 means L2 insuff
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 56, model.Remarks, "L2-Client", model.ClientRowID);
                                cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                                cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                                new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                            }
                            #endregion

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Client", model.Remarks);
                            #endregion

                            int PID = model.PersonalRowID;
                            TempData["SuccessMsg"] = "Record Added Successfully!";
                            ModelState.Clear();
                            return RedirectToAction("MessageAction", "Account");


                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Please select at least one document or mention remarks!";
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

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Dldrpt(string id, string Crid, string msg = "")
        {
            ReportDownload model = new ReportDownload();
            try
            {
                if (clsCommonMethods.PDecrypt(msg) != "")
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
                }
                model.ClientRowId = Crid;
                model.reportName = id;
                var ClientRowid = Convert.ToInt32(clsCommonMethods.PDecrypt(model.ClientRowId));
                if (ClientRowid == 113 || ClientRowid == 1578)
                {
                    string imgname = (clsCommonMethods.PDecrypt(model.reportName));
                    string reportFileName = imgname + ".pdf";
                    ClientCustomize cc = new ClientCustomize();
                    DataTable ddt = cc.getAutoExistCompRefNo(imgname);
                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
                    string FldPathDoc_old = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc_Old"].ToString();
                    byte[] fileBytes = null;
                    if (ddt.Rows.Count > 0)
                    {
                        reportFileName = ddt.Rows[0]["ReportFileName"].ToString();
                        FldPathDoc = Server.MapPath(FldPathDoc) + reportFileName;
                    }
                    else
                    {
                        FldPathDoc = FldPathDoc_old + reportFileName;
                    }
                    DataTable reportSecurity = new DataTable();
                    reportSecurity = cc.ReportSeurityBYdatabase(ClientRowid, imgname);
                    if (reportSecurity.Rows.Count > 0)
                    {
                        if (System.IO.File.Exists(FldPathDoc))
                        {
                            var data = System.IO.File.ReadAllBytes(FldPathDoc);

                            using (MemoryStream input = new MemoryStream(data))
                            {
                                using (MemoryStream output = new MemoryStream())
                                {
                                    string pwd = reportSecurity.Rows[0]["Reportvalidation"].ToString();
                                    try
                                    {
                                        PdfReader reader = new PdfReader(input);
                                        PdfEncryptor.Encrypt(reader, output, true, pwd, pwd, PdfWriter.ALLOW_SCREENREADERS);
                                        data = output.ToArray();
                                        Response.ContentType = "application/pdf";
                                        Response.AddHeader("content-disposition", "attachment;filename=" + repoDashboardClient.downloadReportFileName(imgname) + ".pdf");
                                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                        Response.BinaryWrite(data);
                                        Response.End();
                                    }

                                    catch
                                    {
                                        if (System.IO.File.Exists(FldPathDoc))
                                        {
                                            fileBytes = System.IO.File.ReadAllBytes(FldPathDoc);
                                        }
                                        if (fileBytes != null)
                                        {
                                            return File(fileBytes, MediaTypeNames.Application.Octet, repoDashboardClient.downloadReportFileName(imgname) + ".pdf");
                                        }
                                        else
                                        {
                                            return RedirectToAction("Dldrpt", "Account", new { rid = model.reportName, Crid = model.ClientRowId, msg = clsCommonMethods.PEncrypt("Report Not Found !") });
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(FldPathDoc))
                        {
                            fileBytes = System.IO.File.ReadAllBytes(FldPathDoc);
                        }
                        if (fileBytes != null)
                        {
                            return File(fileBytes, MediaTypeNames.Application.Octet, repoDashboardClient.downloadReportFileName(imgname) + ".pdf");
                        }
                        else
                        {
                            return RedirectToAction("Dldrpt", "Account", new { id = model.reportName, Crid = model.ClientRowId, msg = clsCommonMethods.PEncrypt("Report Not Found !") });
                        }
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

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Dldportalrpt(string id, string Crid, string msg = "", string Crefile = "")
        {
            ReportDownload model = new ReportDownload();
            try
            {
                if (clsCommonMethods.PDecrypt(msg) != "")
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
                }
                model.ClientRowId = Crid;
                model.reportName = Crefile;
                model.CompanyRefNo = id;
                var ClientRowid = Convert.ToInt32(clsCommonMethods.PDecrypt(model.ClientRowId));
                var CompanyRefNo = clsCommonMethods.PDecrypt(model.CompanyRefNo);
                var reportName = clsCommonMethods.PDecrypt(model.reportName);
                if (ClientRowid == 113 || ClientRowid == 1578)
                {
                    string reportFileName = (clsCommonMethods.PDecrypt(model.reportName));
                    ClientCustomize cc = new ClientCustomize();
                    DataTable ddt = cc.getAutoExistCompRefNo(CompanyRefNo);
                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
                    string FldPathDoc_old = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc_Old"].ToString();
                    byte[] fileBytes = null;
                    if (ddt.Rows.Count > 0)
                    {

                        var rows = ddt.AsEnumerable()
                                          .Where(r => string.Equals(
                                              Convert.ToString(r["ReportFileName"]).Trim(),
                                              reportName?.Trim(),
                                              StringComparison.OrdinalIgnoreCase));

                        var match = rows.FirstOrDefault();
                        if (match != null)
                        {
                            model.reportType = Convert.ToString(match["ReportType"]);
                        }

                        FldPathDoc = Server.MapPath(FldPathDoc) + reportFileName;
                    }
                    else
                    {
                        FldPathDoc = FldPathDoc_old + reportFileName;
                    }
                    DataTable reportSecurity = new DataTable();
                    reportSecurity = cc.ReportSeurityBYdatabase(ClientRowid, CompanyRefNo);
                    if (reportSecurity.Rows.Count > 0)
                    {
                        if (System.IO.File.Exists(FldPathDoc))
                        {
                            var data = System.IO.File.ReadAllBytes(FldPathDoc);

                            using (MemoryStream input = new MemoryStream(data))
                            {
                                using (MemoryStream output = new MemoryStream())
                                {
                                    string pwd = reportSecurity.Rows[0]["Reportvalidation"].ToString();
                                    try
                                    {
                                        PdfReader reader = new PdfReader(input);
                                        PdfEncryptor.Encrypt(reader, output, true, pwd, pwd, PdfWriter.ALLOW_SCREENREADERS);
                                        data = output.ToArray();
                                        Response.ContentType = "application/pdf";
                                        Response.AddHeader("content-disposition", "attachment;filename=" + repoDashboardClient.downloadReportFileName(CompanyRefNo) + ".pdf");
                                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                        Response.BinaryWrite(data);
                                        Response.End();
                                    }

                                    catch
                                    {
                                        if (System.IO.File.Exists(FldPathDoc))
                                        {
                                            fileBytes = System.IO.File.ReadAllBytes(FldPathDoc);
                                        }
                                        if (fileBytes != null)
                                        {
                                            return File(fileBytes, MediaTypeNames.Application.Octet, repoDashboardClient.downloadReportFileName(CompanyRefNo) + ".pdf");
                                        }
                                        else
                                        {
                                            return RedirectToAction("Dldportalrpt", "Account", new { rid = model.reportName, Crid = model.ClientRowId, Crefile = model.CompanyRefNo, msg = clsCommonMethods.PEncrypt("Report Not Found !") });
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(FldPathDoc))
                        {
                            fileBytes = System.IO.File.ReadAllBytes(FldPathDoc);
                            
                        }
                        string extension = Path.GetExtension(FldPathDoc).ToLower();
                        if (extension == ".docx")
                        {
                            if (fileBytes != null)
                            {
                                return File(fileBytes, MediaTypeNames.Application.Octet, repoDashboardClient.downloadReportFileName(CompanyRefNo, model.reportType) + ".docx");
                            }
                            else
                            {
                                return RedirectToAction("Dldportalrpt", "Account", new { id = model.reportName, Crid = model.ClientRowId, Crefile = model.CompanyRefNo, msg = clsCommonMethods.PEncrypt("Report Not Found !") });
                            }
                        }
                        else
                        {
                            if (fileBytes != null)
                            {
                                return File(fileBytes, MediaTypeNames.Application.Octet, repoDashboardClient.downloadReportFileName(CompanyRefNo, model.reportType) + ".pdf");
                            }
                            else
                            {
                                return RedirectToAction("Dldportalrpt", "Account", new { id = model.reportName, Crid = model.ClientRowId, Crefile = model.CompanyRefNo, msg = clsCommonMethods.PEncrypt("Report Not Found !") });
                            }
                        }
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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Dldrpt(ReportDownload rptDownload)
        {
            try
            {
                ClientCustomize CCC = new ClientCustomize();
                var ClientRowid = Convert.ToInt32(clsCommonMethods.PDecrypt(rptDownload.ClientRowId));
                var tbldata = CCC.GetClientRPTKey(ClientRowid, rptDownload.reportId);
                if (tbldata.Rows.Count > 0)
                {
                    string imgname = (clsCommonMethods.PDecrypt(rptDownload.reportName));
                    string reportFileName = imgname + ".pdf";
                    ClientCustomize cc = new ClientCustomize();
                    DataTable ddt = cc.getAutoExistCompRefNo(imgname);
                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
                    string FldPathDoc_old = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc_Old"].ToString();
                    byte[] fileBytes = null;
                    if (ddt.Rows.Count > 0)
                    {
                        reportFileName = ddt.Rows[0]["ReportFileName"].ToString();
                        FldPathDoc = Server.MapPath(FldPathDoc) + reportFileName;
                    }
                    else
                    {
                        FldPathDoc = FldPathDoc_old + reportFileName;
                    }
                    DataTable reportSecurity = new DataTable();
                    reportSecurity = cc.ReportSeurityBYdatabase(ClientRowid, imgname);
                    if (reportSecurity.Rows.Count > 0)
                    {
                        if (System.IO.File.Exists(FldPathDoc))
                        {
                            var data = System.IO.File.ReadAllBytes(FldPathDoc);

                            using (MemoryStream input = new MemoryStream(data))
                            {
                                using (MemoryStream output = new MemoryStream())
                                {
                                    string pwd = reportSecurity.Rows[0]["Reportvalidation"].ToString();
                                    try
                                    {
                                        PdfReader reader = new PdfReader(input);
                                        PdfEncryptor.Encrypt(reader, output, true, pwd, pwd, PdfWriter.ALLOW_SCREENREADERS);
                                        data = output.ToArray();
                                        Response.ContentType = "application/pdf";
                                        Response.AddHeader("content-disposition", "attachment;filename=" + repoDashboardClient.downloadReportFileName(imgname) + ".pdf");
                                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                        Response.BinaryWrite(data);
                                        Response.End();
                                    }

                                    catch
                                    {
                                        if (System.IO.File.Exists(FldPathDoc))
                                        {
                                            fileBytes = System.IO.File.ReadAllBytes(FldPathDoc);
                                        }
                                        if (fileBytes != null)
                                        {
                                            return File(fileBytes, MediaTypeNames.Application.Octet, repoDashboardClient.downloadReportFileName(imgname) + ".pdf");
                                        }
                                        else
                                        {
                                            return RedirectToAction("Dldrpt", "Account", new { rid = rptDownload.reportName, Crid = rptDownload.ClientRowId, msg = clsCommonMethods.PEncrypt("Report Not Found !") });
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(FldPathDoc))
                        {
                            fileBytes = System.IO.File.ReadAllBytes(FldPathDoc);
                        }
                        if (fileBytes != null)
                        {
                            return File(fileBytes, MediaTypeNames.Application.Octet, repoDashboardClient.downloadReportFileName(imgname) + ".pdf");
                        }
                        else
                        {
                            return RedirectToAction("Dldrpt", "Account", new { id = rptDownload.reportName, Crid = rptDownload.ClientRowId, msg = clsCommonMethods.PEncrypt("Report Not Found !") });
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Dldrpt", "Account", new { id = rptDownload.reportName, Crid = rptDownload.ClientRowId, msg = clsCommonMethods.PEncrypt("Please Enter Valid Key !") });
                }
            }
            catch (Exception ex)
            {
                var strmsg = "Error found !";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), ex.Message, ex.StackTrace);
                return RedirectToAction("Dldrpt", "Account", new { id = rptDownload.reportName, Crid = rptDownload.ClientRowId, msg = clsCommonMethods.PEncrypt(strmsg) });
            }
            return RedirectToAction("Dldrpt", "Account", new { id = rptDownload.reportName, Crid = rptDownload.ClientRowId });
        }


        private void SavePQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", HttpPostedFileBase UploadDoc = null, string remarks = "", byte NADoc = 0, string secRefId = "")
        {
            try
            {
                //var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
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
                    proImg = (PersonalRowID.ToString() + "_" + SubCheckRowID.ToString() + " " + DocumentType + "_" + Guid.NewGuid()).Replace(" ", "_").Replace("-", "_") + "_" + Timestamp.ToString() + ext;
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

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult C_YTR(string id = "", string srId = "", string cfrID = "")
        {
            PQEmploymentYTRClearViewModel model = new PQEmploymentYTRClearViewModel();
            int Did = 0;
            if (id != "")
            {
                Did = Convert.ToInt32(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(id)));
            }
            short SubCheckId = Convert.ToInt16(clsCommonMethods.PDecrypt(srId));
            int CheckFamilyId = Convert.ToInt32(clsCommonMethods.PDecrypt(cfrID));
            try
            {
                if (Did > 0)
                {
                    ClientCustomize cc = new ClientCustomize();
                    DataTable dt = cc.GetCheckDetails(Did);
                    if (dt.Rows.Count > 0)
                    {
                        model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["Personalrowid"].ToString());
                        model.UniqueComponentID = Convert.ToString(dt.Rows[0]["CheckUniqueID"].ToString());
                        model.SecuritasRefID = Convert.ToString(dt.Rows[0]["CompanyRefNo"].ToString());
                        model.ClientRowId = Convert.ToInt32(dt.Rows[0]["ClientRowId"].ToString());
                        model.CheckStatusID = Convert.ToInt32(dt.Rows[0]["CheckStatusID"].ToString());
                        model.CandCheckRowID = Convert.ToInt32(dt.Rows[0]["CandCheckRowID"].ToString());
                    }
                    model.EmploymentRowID = Did;
                    model.CheckFamilyRowID = CheckFamilyId;
                    model.SubCheckRowID = SubCheckId;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult C_YTR(PQEmploymentYTRClearViewModel model)
        {
            try
            {
                if (model.EmploymentRowID > 0)
                {
                    model.EmploymentRowID = model.EmploymentRowID;
                    model.CheckStatus = "WIP-Pending";
                    model.ModifiedBy = 0;
                    model.ModifiedDate = DateTime.Now;
                    short ClientRowID = Convert.ToInt16(model.ClientRowId);
                    if (model.CheckFamilyRowID == 12)
                    {
                        if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                        {
                            SaveYTRUploadDocByClient(model.PersonalRowID, model.SubCheckRowID, "Provided", "YTRClear", "provideddocuments", model.UploadDoc, "", 0, model.SecuritasRefID);

                        }
                    }
                    #region Save LogTransaction Details
                    clsCommonMethods.AddPQLogTrasactionByCandidate(model.PersonalRowID, "C_YTR", "Clear YTR By Candidate", model.UniqueComponentID, "Clear YTR By Candidate", model.ClientRowId);
                    #endregion

                    //Check Action History Save 
                    clsCommonMethods.AddCheckActionHistoryByCandidate(model.PersonalRowID, model.SubCheckRowID, "Clear YTR By Candidate", model.Remarks, model.ClientRowId);

                    ClientCustomize cc = new ClientCustomize();
                    cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 69, model.Remarks, "YTRClear", ClientRowID);
                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 0, 69, "Client", ClientRowID);
                    cc.UpdateEmpIdYTRClear(model.PersonalRowID, model.EmployeeID, ClientRowID);
                    cc.UpdateYTRDetails_tbl(model.PersonalRowID, model.SubCheckRowID, 0, 1);
                    if (ClientRowID == 7 || ClientRowID == 296 || ClientRowID == 305 || ClientRowID == 306)
                    {
                        try
                        {

                            SendAlertMassageForYTRClearlMail(model.PersonalRowID, model.SubCheckRowID, ClientRowID, "YTR Clear", 0, 1);
                        }
                        catch
                        {

                        }
                    }
                    TempData["SuccessMsg"] = "YTR Clear Successfully!";
                    ModelState.Clear();
                    return RedirectToAction("C_YTR", "Account", new { id = clsCommonMethods.PEncrypt(model.CandCheckRowID.ToString()), srId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), cfrID = clsCommonMethods.PEncrypt(model.CheckFamilyRowID.ToString()) });

                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                // ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        public void SaveYTRUploadDocByClient(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", HttpPostedFileBase UploadDoc = null, string remarks = "", byte NADoc = 0, string securitasId = "")
        {
            try
            {
                string secRefId = securitasId;
                string FileName = (PersonalRowID + "_" + SubCheckRowID.ToString() + " " + DocumentType).Replace(" ", "_").Replace("-", "_");
                PQAddVerifiedUploadDocViewModel DocModel = new PQAddVerifiedUploadDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.SubCheckRowID = SubCheckRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //Provided/Verified
                DocModel.DocumentType = DocumentType;            //Name of the selected docs
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
                DocModel.RptArtiFacts = 0;
                repoPQVerifiedUploadDoc.AddPQVerifiedUploadDoc(DocModel);
                repoPQVerifiedUploadDoc.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        bool SendAlertMassageForYTRClearlMail(int PersonalRowID, short SubCheckRowID, int clientrowid, string AlertType = "", int PackageId = 0, int MailType = 0)
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
            ds = cc.getAlertMailingDataForInsuff(PersonalRowID, SubCheckRowID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string body = string.Empty;
                string MailSubject = "";
                var clientid = Convert.ToInt32(ds.Tables[0].Rows[0]["ClientRowId"]);
                var templateDB = cc.getAlertMailtemplate(clientid, AlertType, PackageId, MailType, "YTRClearMail");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }
                string CandidateName = Convert.ToString(ds.Tables[0].Rows[0]["CandidateName"]);
                string CompanyRefNo = Convert.ToString(ds.Tables[0].Rows[0]["CompanyRefNo"]);
                string SubCheckName = Convert.ToString(ds.Tables[0].Rows[0]["SubCheckName"]);
                string TeamEmailID = Convert.ToString(ds.Tables[0].Rows[0]["OperationEmailID"]);
                string SpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["SpocEmailID"]);
                string ClientSpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["ClientSpocEmailID"]);
                int ClientUserRowId = Convert.ToInt32(ds.Tables[0].Rows[0]["ClientUserRowId"]);
                string spocEmail = "";
                string cSpocEmail = "";
                DataTable dtEmail = new DataTable();
                dtEmail = cc.GetEmailStructureForAlert(clientrowid, ClientUserRowId, "Insuff", PersonalRowID);
                if (dtEmail.Rows.Count > 0)
                {
                    spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                    cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                }

                body = body.Replace("{FCandidateName}", CandidateName);
                body = body.Replace("{FCheckName}", SubCheckName);
                body = body.Replace("{FRefNo}", CompanyRefNo);
                body = body.Replace("{MailSignature}", MailSignature);
                string Subject = "YTR Cleared for " + CompanyRefNo + " " + CandidateName + "  " + SubCheckName;
                string To = TeamEmailID + "," + spocEmail + "," + cSpocEmail;
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
                if (clientid == 257)
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


        public string GatZipFileMessage(HttpPostedFileBase postedFile, string clientRowid, string filenm)
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
            if (postedFile != null)
            {
                if (postedFile.ContentLength > 0)
                {
                    string[] AllowedExtension = new string[] { ".ZIP", ".zip" };
                    var bytes = System.IO.File.ReadAllBytes(TempPath + Path.GetFileName(filename));
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
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ResetClientCredentials(string Wrid = "", string uId = "", string oldP = "", string cRid = "", string msg = "")
        {
            ForgetCredentialsViewModel model = new ForgetCredentialsViewModel();
            try
            {
                model.WebUserRowID = clsCommonMethods.PEncrypt(Session["ClientUserRowID"].ToString());
                model.CRPUserName = Session["ClientName"].ToString();
                model.ClientRowID = clsCommonMethods.PEncrypt(Session["ClientRowID"].ToString());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.ToString();
                throw;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ResetClientCredentials(ForgetCredentialsViewModel model)
        {

            try
            {
                ClientCustomize CCC = new ClientCustomize();
                model.CRPNewPwd = clsCommonMethods.PasswordEncrypt(model.CRPNewPwd);
                model.CRPConfirmNewPwd = clsCommonMethods.PasswordEncrypt(model.CRPConfirmNewPwd);
                model.WebUserRowID = clsCommonMethods.PDecrypt(model.WebUserRowID);
                model.ClientRowID = clsCommonMethods.PDecrypt(model.ClientRowID);
                var GetLastPass = CCC.GetLastPasswords(model.WebUserRowID, model.CRPNewPwd, "PQClientLogins");
                if (GetLastPass.Rows.Count > 0)
                {
                    ViewBag.ErrorMsg = "Please Don't try last five passwords !";
                    return RedirectToAction("ResetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString()), /*oldP = model.CRPOldPwd1,*/ uId = clsCommonMethods.PEncrypt(model.CRPUserName.ToString()), cRid = clsCommonMethods.PEncrypt(model.ClientRowID), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                }

                if (Convert.ToString(model.CRPConfirmNewPwd) != "")
                {
                    int results = CCC.ResetClientUserCredentials(model.WebUserRowID, model.CRPConfirmNewPwd, "");
                    if (results == 1)
                    {
                        ViewBag.SuccessMsg = "Password update successfully";
                        return RedirectToAction("ClientLogin", "Account", new { ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.SuccessMsg) });

                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Password not update please try again !";
                        return RedirectToAction("ResetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString())/*, oldP = model.CRPOldPwd1*/, uId = clsCommonMethods.PEncrypt(model.CRPUserName.ToString()), cRid = clsCommonMethods.PEncrypt(model.ClientRowID), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Password not update please try again !";
                    return RedirectToAction("ResetClientCredentials", "Account", new { Wrid = clsCommonMethods.PEncrypt(model.WebUserRowID.ToString())/*, oldP = model.CRPOldPwd1*/, uId = clsCommonMethods.PEncrypt(model.CRPUserName.ToString()), cRid = clsCommonMethods.PEncrypt(model.ClientRowID), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Error Founds !";
                return RedirectToAction("ResetClientCredentials", "Account", new { Wrid = model.WebUserRowID.ToString(), /*oldP = model.CRPOldPwd1.ToString(),*/ uId = clsCommonMethods.PEncrypt(model.CRPUserName.ToString()), cRid = clsCommonMethods.PEncrypt(model.ClientRowID), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });

            }
        }

        byte[] GetFileForPurg(string filePath)
        {
            if (filePath != "")
            {


                string fullName = Server.MapPath("~" + filePath);

                byte[] fileBytes = GetFile(fullName);
                return fileBytes;
                //return File(
                //    fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
            }
            else
            {
                ViewBag.Message = "File Not Uploaded";
                return null;
            }

        }

        byte[] GetFile(string s)
        {
            FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        public ActionResult ApprovePurgData(string ID, string ApprovalPerson)
        {
            try
            {

                clsSendMail mail = new clsSendMail();
                bool flag = false;
                string DisplayName = "Purge Data Information";
                string From = "bgv.request@securitas-india.com";
                string Subject = "Purging Mail";
                ClientCustomize cc = new ClientCustomize();
                int parameter = Convert.ToInt32(clsCommonMethods.PDecrypt(ID));
                string Person = clsCommonMethods.PDecrypt(ApprovalPerson);
                DataTable dataTable = new DataTable();
                dataTable = cc.GetPurgRowDetails(parameter);
                string OriginalFileName = Convert.ToString(dataTable.Rows[0]["OriginalFileName"]);
                string SPOCName = cc.GetUserNameForPurg(Convert.ToInt32(dataTable.Rows[0]["CreatedBy"]));
                string DateFrom = Convert.ToString(dataTable.Rows[0]["DateFrom"]);
                string DateTo = Convert.ToString(dataTable.Rows[0]["DateTo"]);
                string FilePath = Convert.ToString(dataTable.Rows[0]["FilePath"]);
                byte[] inputstream = GetFileForPurg(FilePath);
                MemoryStream memoryStream = new MemoryStream(inputstream);
                var attachment = new System.Net.Mail.Attachment(memoryStream, OriginalFileName);

                DataTable dt = new DataTable();
                dt = cc.GetClientNameForPurg(Convert.ToInt32(dataTable.Rows[0]["ClientRowID"]));

                string ClientName = Convert.ToString(dt.Rows[0]["ClientName"]);
                string Address = Convert.ToString(dt.Rows[0]["Address"]);
                string Location = Convert.ToString(dt.Rows[0]["LocationName"]);
                //if (Person == "ITHead")
                //{
                //    if (Convert.ToInt32(dataTable.Rows[0]["ApprovalByITHead"]) == 0)
                //    {
                //        cc.UpdatePurgStatus(parameter);
                //        string DocumentEmail = Convert.ToString(dataTable.Rows[0]["DocumentEmail"]);
                //        string ToMail = DocumentEmail;

                //        #region Send Mail For Report/Doc Dump
                //        string DocDump = clsCommonMethods.PEncrypt("DocDump");
                //        string bodyDocDump = "<div><p>Dear Team,</p><p>&nbsp;&nbsp;&nbsp;&nbsp; <strong>Greetings</strong> of the day ! </p><p> &nbsp;&nbsp;&nbsp;&nbsp; Data Purging Request By " + "<strong>" + SPOCName + "</strong>" + "</p> <p>&nbsp;&nbsp;&nbsp;&nbsp; Please, find purging data request detail below:</p></div></br>"
                //    + "<table border='1' cellpadding='0' cellspacing='0' width = '100%' class='table table-bordered'>" +
                //    "<tr>" +
                //    "<th> Client Name</th>" +
                //    "<th> Address</th>" +
                //    "<th> Location </th>" +
                //    "<th>Purg Date From</th>" +
                //    "<th>Purg Date to</th>" +
                //    "<th>Size</th>" +
                //    "<th>Doc Path</th>" +
                //    "</tr>" +
                //    "<tr>" +
                //    "<td>" + ClientName + "</td>" +
                //    "<td>" + Address + "</td>" +
                //    "<td>" + Location + "</td>" +
                //    " <td>" + DateFrom + "</td>" +
                //    "<td>" + DateTo + "</td>" +
                //    "<td> </td>" +
                //    "<td> </td>" +
                //    "</tr> " +
                //    "</table>" +
                //    "<div><br>" +
                //    "<a href='https://bgv.securitas-india.com/Account/ApprovePurgData?ID=" + ID + "&ApprovalPerson=" + DocDump + "'>Click me for approval.</a></div>"
                //    + "<br><br><br><br><br><div><strong>Regards</strong></div><div>BGVHelpdesk</div>";
                //        flag = mail.SendMailPurg(DisplayName, From, ToMail, Subject, bodyDocDump, attachment);
                //        if (flag == true)
                //        {
                //            string User = "DocDump";
                //            cc.InsertMailTimeForPurg(User, parameter);
                //        }
                //        #endregion

                //        return View("PurgSucessPage");
                //    }
                //    else
                //        return Content("already approved!! ");

                //}
                if (Person == "OPTMHead")
                {
                    if (Convert.ToInt32(dataTable.Rows[0]["ApprovalByOPTMHead"]) == 0)
                    {
                        cc.UpdatePurgStatusByOPTM(parameter);
                        string DocumentEmail = Convert.ToString(dataTable.Rows[0]["DocumentEmail"]);
                        string ToMail = DocumentEmail;

                        #region Send Mail For Report/Doc Dump
                        string DocDump = clsCommonMethods.PEncrypt("DocDump");
                        string bodyDocDump = "<div><p>Dear Team,</p><p>&nbsp;&nbsp;&nbsp;&nbsp; <strong>Greetings</strong> of the day ! </p><p> &nbsp;&nbsp;&nbsp;&nbsp; Data Purging Request By " + "<strong>" + SPOCName + "</strong>" + "</p> <p>&nbsp;&nbsp;&nbsp;&nbsp; Please, find purging data request detail below:</p></div></br>"
                    + "<table border='1' cellpadding='0' cellspacing='0' width = '100%' class='table table-bordered'>" +
                    "<tr>" +
                    "<th> Client Name</th>" +
                    "<th> Address</th>" +
                    "<th> Location </th>" +
                    "<th>Purg Date From</th>" +
                    "<th>Purg Date to</th>" +
                    "<th>Size</th>" +
                    "<th>Doc Path</th>" +
                    "</tr>" +
                    "<tr>" +
                    "<td>" + ClientName + "</td>" +
                    "<td>" + Address + "</td>" +
                    "<td>" + Location + "</td>" +
                    " <td>" + DateFrom + "</td>" +
                    "<td>" + DateTo + "</td>" +
                    "<td> </td>" +
                    "<td> </td>" +
                    "</tr> " +
                    "</table>" +
                    "<div><br>" +
                    "<a href='https://bgv.securitas-india.com/Account/ApprovePurgData?ID=" + ID + "&ApprovalPerson=" + DocDump + "'>Click me for approval.</a></div>"
                    + "<br><br><br><br><br><div><strong>Regards</strong></div><div>BGVHelpdesk</div>";
                        flag = mail.SendMailPurg(DisplayName, From, ToMail, Subject, bodyDocDump, attachment);
                        if (flag == true)
                        {
                            string User = "DocDump";
                            cc.InsertMailTimeForPurg(User, parameter);
                        }
                        #endregion

                        return View("PurgSucessPage");
                    }
                    else
                        return Content("already approved!! ");
                }

                if (Person == "DocDump")
                {
                    if (Convert.ToInt32(dataTable.Rows[0]["DocDump"]) == 0)
                    {
                        cc.UpdatePurgStatusByDocDump(parameter);
                        string CertificateEmailEnfra = Convert.ToString(dataTable.Rows[0]["CertificateEmailEnfra"]);
                        string Ccmail = Convert.ToString(dataTable.Rows[0]["SPOCEmailId"]) + "," + Convert.ToString(dataTable.Rows[0]["ReportDumpEmail"]); ;
                        string ToMail = CertificateEmailEnfra;
                        #region Send Mail to infra
                        string bodyInfra = "<div><p>Dear Team,</p><p>&nbsp;&nbsp;&nbsp;&nbsp; <strong>Greetings</strong> of the day ! </p><p> &nbsp;&nbsp;&nbsp;&nbsp; Data Purging Request By " + "<strong>" + SPOCName + "</strong>" + "</p> <p>&nbsp;&nbsp;&nbsp;&nbsp; Please, find purging data request detail below:</p></div></br>"
                    + "<table border='1' cellpadding='0' cellspacing='0' width = '100%' class='table table-bordered'>" +
                    "<tr>" +
                    "<th> Client Name</th>" +
                    "<th> Address</th>" +
                    "<th> Location </th>" +
                    "<th>Purg Date From</th>" +
                    "<th>Purg Date to</th>" +
                    "<th>Size</th>" +
                    "<th>Doc Path</th>" +
                    "</tr>" +
                    "<tr>" +
                    "<td>" + ClientName + "</td>" +
                    "<td>" + Address + "</td>" +
                    "<td>" + Location + "</td>" +
                    " <td>" + DateFrom + "</td>" +
                    "<td>" + DateTo + "</td>" +
                    "<td> </td>" +
                    "<td> </td>" +
                    "</tr> " +
                    "</table>"
                    + "<br><br><br><br><br><div><strong>Regards</strong></div><div>BGVHelpdesk</div>";

                        flag = mail.SendMailPurg(DisplayName, From, ToMail, Ccmail, Subject, bodyInfra, attachment);
                        if (flag == true)
                        {
                            string User = "Infra";
                            cc.InsertMailTimeForPurg(User, parameter);
                        }

                        #endregion

                        return View("PurgSucessPage");
                    }
                    else
                        return Content("already approved!! ");
                }
                else
                    return View("Index");

            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Content(message);
            }
        }
        /////////////////////////////////////////////////////////

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult GetFeedback(string id = "", string msg = "")
        {
            try
            {
                //string decryptno = clsCommonMethods.PEncrypt(link);
                string mobileno = clsCommonMethods.PDecrypt(id);
                Session["mobileno"] = mobileno;
                if (mobileno.Length >= 8)
                {
                    if (msg != "")
                    {
                        ViewBag.ErrorMsg = msg;
                        return View();
                    }
                    return View();

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Content(message);
            }
            return null;
        }




        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetFeedback(FormCollection frm)
        {
            string mobileno = Convert.ToString(Session["mobileno"]);
            int ratingId = Convert.ToInt32(frm["rating"]);
            string comment1 = Convert.ToString(frm["comment"]);
            if (string.IsNullOrEmpty(mobileno))
            {
                return View();
            }
            ClientCustomize cc = new ClientCustomize();
            cc.submitFeedback(mobileno, Convert.ToInt16(ratingId), comment1);
            return RedirectToAction("GetFeedback", "Account", new { id = clsCommonMethods.PEncrypt(mobileno), msg = "Thanks for your feedback" });
        }

        [HttpGet]
        public JsonResult GetAadhaarOTP(string id)
        {
            aadhaar_with_otp_objectRoot objData = new aadhaar_with_otp_objectRoot();
            InstantCheckRepository obj = new InstantCheckRepository();
            aadhaar_with_otp_objectRoot GetOTP = new aadhaar_with_otp_objectRoot();
            var otp = "";
            if (id != "")
            {
                GetOTP = obj.Getaadhaar_sendotpLive(id);
                otp = GetOTP.data.client_id;
                Session["client_id"] = GetOTP.data.client_id;
            }
            else
            {
                otp = "";
            }


            return Json(otp, JsonRequestBehavior.AllowGet);
        }
        private static Random random = new Random();

        public static int RandomIntFromInterval(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        public static string GenerateCodeVerifier(int length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
            StringBuilder codeVerifier = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(characters.Length);
                codeVerifier.Append(characters[randomIndex]);
            }
            return codeVerifier.ToString();
        }

        public static async Task<byte[]> Sha256(string plain)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(plain);
                return await Task.FromResult(sha256.ComputeHash(data));
            }
        }

        public static string Base64UrlEncode(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);
            return base64.Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        public static async Task<string> GenerateCodeChallenge(string codeVerifier)
        {
            byte[] hashedBuffer = await Sha256(codeVerifier);
            return Base64UrlEncode(hashedBuffer);
        }
        public string sendDigiLockerLink(string UniqueComponentID)
        {
            try
            {
                int randomByte = RandomIntFromInterval(32, 96);
                string codeVerifier = GenerateCodeVerifier(randomByte);
                var codeChallenge = GenerateCodeChallenge(codeVerifier);

                string NSSOauthorizeURL = "https://digilocker.meripehchaan.gov.in/public/oauth2/1/authorize?response_type=code&client_id=EY2460D287&state=" + UniqueComponentID + "&redirect_uri=";
                NSSOauthorizeURL = NSSOauthorizeURL + "https://bgv.securitas-india.com/Account/DigiLockerRedirect_uri";
                NSSOauthorizeURL = NSSOauthorizeURL + "&code_challenge=" + codeChallenge.Result;
                NSSOauthorizeURL = NSSOauthorizeURL + "&code_challenge_method=S256&scope=files.issueddocs+files.uploadeddocs&pla=Y";


                int rst = ObjAPI.InsertDigiLockerUserDetails(UniqueComponentID, NSSOauthorizeURL, codeVerifier, 0);
                ViewBag.msg = "Link sent successfully !";


            }
            catch (Exception Ex)
            {

                throw;
            }
            return "";
        }

        [HttpGet]
        public ActionResult VerifyAadhaar(string id)
        {
            ClientCustomize cc = new ClientCustomize();
            verifyAadhaar model = new verifyAadhaar();
            var instantCheckId = clsCommonMethods.PDecrypt(id);
            var testid = clsCommonMethods.PEncrypt("1");
            InstantCheckRepository obj = new InstantCheckRepository();
            model.InstantId = Convert.ToInt32(instantCheckId.ToString());
            var tbl = cc.GetInstantMultiCheckSolution(Convert.ToInt32(instantCheckId), "", "", "");
            if (tbl.Rows.Count > 0)
            {
                model.PersonalRowId = Convert.ToInt32(tbl.Rows[0]["PersonalRowId"]);
                model.MobileNo = Convert.ToString(tbl.Rows[0]["MobileNo"]);
                if (Convert.ToString(tbl.Rows[0]["AadhaarId"]) != "")
                {
                    model.AadhaarStatus = Convert.ToInt32(tbl.Rows[0]["AadhaarId"]);
                }
                else
                {
                    model.AadhaarStatus = 0;
                }
            }
            ViewBag.AadhaarStatus = 0;
            if (model.AadhaarStatus != 0)
            {
                ViewBag.AadhaarStatus = 1;
            }
            return View(model);
        }
        public ActionResult VerifyAadhaar(FormCollection frm)
        {
            var OTP = Convert.ToString(frm["otp"]);
            var Client_Id = Convert.ToString(frm["client_id"]);
            int ClientRowId = 267;
            verifyAadhaar model = new verifyAadhaar();
            model.PersonalRowId = 0;
            model.MobileNo = Convert.ToString(frm["MobileNo"]);
            model.InstantId = Convert.ToInt32(frm["InstantId"]);
            model.AadhaarNo = Convert.ToString(frm["AadhaarNo"]);
            InstantCheckRepository obj = new InstantCheckRepository();
            ClientCustomize cc = new ClientCustomize();
            var Aadhaardetails = obj.Getaadhaar_verify_otpLive(OTP, Client_Id);
            if (Aadhaardetails.data != null)
            {
                var AadharId = cc.Insert_API_PaysPrintAadhaar_verifyRequest(Aadhaardetails, ClientRowId, model.PersonalRowId);
                var fileName = Aadhaardetails.data.aadhaar_number + "_" + Convert.ToString(model.InstantId) + "_" + Convert.ToString(model.PersonalRowId);
                string filePathSave = System.Configuration.ConfigurationManager.AppSettings["AadhaarDocumentsPath"].ToString() + fileName;
                var filetype = clsCommonMethods.GetFileExtension(Aadhaardetails.data.profile_image);
                if (Convert.ToString(filetype) == "")
                {
                    filetype = ".jpg";
                }
                cc.Sp_UpdateStatus_InstantMultiCheckSolution(model.InstantId, AadharId, "Aadhaar", fileName + filetype);
                System.IO.File.WriteAllBytes(filePathSave + filetype, Convert.FromBase64String(Aadhaardetails.data.profile_image));
                ViewBag.AadhaarStatus = 1;
            }
            else
            {
                ViewBag.AadhaarStatus = 0;
                ViewBag.errormsg = "OTP not Valid Please try Again !.";
            }

            return View(model);
        }
        #region Digilocker API
        DigilockerAPI ObjAPI = new DigilockerAPI();
        StringBuilder StrBuilderObj = new StringBuilder();
        // GET: DigiLocker
        [ValidateInput(false)]
        public ActionResult DigiLockerRedirect_uri(string code, string State)
        {
            string PersonalRowId = "";
            ViewBag.msg = "";
            try
            {
                string filePath = System.Configuration.ConfigurationManager.AppSettings["DigiLockerDocument"].ToString();

                var codeVerifierDT = ObjAPI.Sp_GetDigiLockerUserDetails(State);
                if (codeVerifierDT.Rows.Count > 0)
                {
                    string codeVerifier = Convert.ToString(codeVerifierDT.Rows[0]["codeVerifier"]);
                    PersonalRowId = Convert.ToString(codeVerifierDT.Rows[0]["PersonalRowId"]);

                    var access_token = ObjAPI.GetAccessTokenAsync(code, codeVerifier, PersonalRowId);
                    var documentslst = ObjAPI.GetDocumentsIssuedList(access_token.access_token, PersonalRowId);
                    foreach (var item in documentslst.items)
                    {
                        var Doc = ObjAPI.GetfilefromURI(access_token.access_token, item.uri, filePath, PersonalRowId);
                    }
                    var res = ObjAPI.RevokeToken(access_token.access_token);
                    ViewBag.msg = "Documents Received";
                }
                else
                {
                    var codeDT = ObjAPI.GetDigiLockerUserDetails(State);
                    if (codeDT.Rows.Count > 0)
                    {
                        string codeVerifier = Convert.ToString(codeDT.Rows[0]["codeVerifier"]);
                        var access_token = ObjAPI.GetAccessTokenAsync(code, codeVerifier, State);
                        var documentslst = ObjAPI.GetDocumentsIssuedList(access_token.access_token, State);
                        foreach (var item in documentslst.items)
                        {
                            var Doc = ObjAPI.GetfilefromURI(access_token.access_token, item.uri, filePath, PersonalRowId, State);
                        }
                        var res = ObjAPI.RevokeToken(access_token.access_token);
                        ViewBag.msg = "Documents Received";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = "";
                string msg = ex.Message.ToString();
                StrBuilderObj.Append("----------------------------API DigiLockerRedirect_uri-----------------");
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("Response:Exception" + msg);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "DigiLocker", "DigiLockerRedirect_uri", PersonalRowId + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));


            }

            return View();
        }

        #endregion
        #region Profices API
        [HttpGet]
        public ActionResult EPFPassbookAuthenticationAPIs(string CompanyRefNo)
        {
            ViewBag.Finalmsg = "";
            CompanyRefNo = clsCommonMethods.PDecrypt(CompanyRefNo);
            ViewBag.CompanyRefNo = CompanyRefNo;
            ViewBag.msg = "";
            try
            {

            }
            catch (Exception ex)
            {
                ViewBag.msg = "";
                string msg = ex.Message.ToString();
                StrBuilderObj.Append("----------------------------API EPFPassbookAuthenticationAPIs-----------------");
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("Response:Exception" + msg);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "PerfiosAPI", "EPFPassbookAuthenticationAPIs", CompanyRefNo + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));


            }

            return View();
        }
        public JsonResult GetOtp(string UANNo, string CompanyRefNo)
        {
            GetOtpmodel Objdt = new GetOtpmodel();
            try
            {
                ViewBag.Finalmsg = "";
                Objdt = ObjAPI.GetOtpDetails(UANNo, CompanyRefNo);
                ViewBag.request_id = Objdt.request_id;
                Session["request_id"] = Objdt.request_id;
                if (Objdt.result.message != "" && Objdt.result.message != null)
                {
                }
                else
                {
                    Objdt.result.message = "Error";
                }
            }
            catch (Exception ex)
            {
                Objdt.result.message = "Error";
            }
            return Json(Objdt, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EPFPassbookAuthenticationAPIs(string CompanyRefNo, FormCollection formCollection)
        {
            string PersonalRowId = "";
            ViewBag.msg = "";
            try
            {
                string OTP = Convert.ToString((formCollection["OTP"].ToString()));
                ViewBag.OTP = OTP;
                string request_id = Session["request_id"].ToString();
                var codeVerifierDT = ObjAPI.GetPersonalandClientDetails(CompanyRefNo);
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();
                FilePath = Server.MapPath(FilePath);
                //if (codeVerifierDT.Rows.Count > 0)
                //{
                var Doc = ObjAPI.GetUanDetails(request_id, OTP, CompanyRefNo, FilePath);
                if (Doc.result.overall_pf_balance == null && Doc.statuscode == null)
                {
                    ViewBag.Finalmsg = "Please review your details and try again.";
                }
                else
                {
                    ViewBag.Finalmsg = "Submission successful!. We will proceed with the next steps. For queries, contact your CTS recruiter.";
                }

                //}
            }
            catch (Exception ex)
            {
                ViewBag.Finalmsg = "Please review your details and try again.";
                ViewBag.msg = "";
                string msg = ex.Message.ToString();
                StrBuilderObj.Append("----------------------------API EPFPassbookAuthenticationAPIs-----------------");
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("Response:Exception" + msg);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "PerfiosAPI", "EPFPassbookAuthenticationAPIs", PersonalRowId + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));


            }

            return View();
        }
        #endregion

        [HttpGet]
        public ActionResult CandidateFeedback(string ClientRowID = "", string CandidateName = "", string EmailID = "")
        {
            ViewBag.ClientRowID = ClientRowID;
            ViewBag.CandidateName = CandidateName;
            ViewBag.EmailID = EmailID;
            return View();
        }

        [HttpPost]
        public ActionResult CandidateFeedback(FormCollection frm)
        {
            ClientCustomize cc = new ClientCustomize();
            int ratingstar = 0;
            string remarks = "";
            string EmailID = "";
            string CandidateName = "";
            string Action = "";
            int ClientRowID = 0;
            if (Convert.ToInt32(frm["ratingstar"]) > 0)
            {
                ratingstar = Convert.ToInt32(frm["ratingstar"]);
            }
            if (Convert.ToString(frm["opinion"]) != "")
            {
                remarks = Convert.ToString(frm["opinion"]);
            }
            if (Convert.ToString(frm["EmailID"]) != "")
            {
                EmailID = Convert.ToString(frm["EmailID"]).Trim();
            }
            if (Convert.ToString(frm["EmailID"]) != "")
            {
                CandidateName = Convert.ToString(frm["CandidateName"]).Trim();
            }
            if (Convert.ToString(frm["Action"]) != "")
            {
                Action = Convert.ToString(frm["Action"]);
            }
            if (Convert.ToInt32(frm["ClientRowID"]) > 0)
            {
                ClientRowID = Convert.ToInt32(frm["ClientRowID"]);
            }

            if (Action == "Skip")
            {
                remarks = "Feedback Not Provided";
            }
            cc.CandidatePortalFeedback_sp(EmailID, ratingstar, remarks, CandidateName, ClientRowID);

            return RedirectToAction("CandidateLogin", "Account", new { area = "" });
            //return View();
        }
        #region Liveliness API
        [HttpGet]
        public ActionResult Initiateliveliness()
        {
            return View();
        }
        API obj = new API();
        [HttpPost]
        public ActionResult Initiateliveliness(FormCollection frm)
        {
            string RequestId = "";
            try
            {
                string fullName = Convert.ToString(frm["fullName"]);
                string aadhaarNumber = Convert.ToString(frm["aadhaarNumber"]);
                string candTitle = "";
                string Name = Convert.ToString(frm["fullName"]);
                string MiddleName = "";
                string LastName = "";
                string CandidateFullName = Convert.ToString(frm["fullName"]);
                string FatherName = "";
                string Gender = "";
                string EmailID = Convert.ToString(frm["EmailID"]);
                string MobileNo = Convert.ToString(frm["MobileNo"]);
                string ClientRowID = "990";
                string ClientRefID = "";
                string PackageName = "";
                string dob = "";
                int personalRowID = 0;
                int ClientPackageRowID = 22906;
                int clientcheckRowID = 0;
                if (fullName == "")
                {
                    ViewBag.fullName = "Please enter the full name";
                    return View();

                }
                if (aadhaarNumber == "")
                {
                    ViewBag.aadhaarNumber = "Please enter the aadhaar number";
                    return View();

                }
                if (EmailID == "")
                {
                    ViewBag.EmailID = "Please enter the email id";
                    return View();

                }
                if (MobileNo == "")
                {
                    ViewBag.EmailID = "Please enter the mobile no";
                    return View();

                }
                //if (Convert.ToInt64(fullName.Length) < 1)
                //{
                //    ViewBag.fullName = "Please enter the fullName";
                //    return View();

                //}
                if (Convert.ToInt64(aadhaarNumber.Length) < 11)
                {
                    ViewBag.aadhaarNumber = "Invalid Aadhaar number. Must be 12 digits.";
                    return View();

                }
                if (EmailID.Contains("@"))
                {

                }
                else
                {
                    ViewBag.EmailID = "Invalid email address.";
                    return View();

                }
                if (Convert.ToInt32(MobileNo.Length) < 0)
                {
                    ViewBag.EmailID = "Invalid mobile number. Must be 10 digits";
                    return View();

                }
                else
                {
                    personalRowID = obj.insertCaseRegistrationPqpersonal(candTitle, Name, MiddleName, LastName, CandidateFullName, FatherName, Gender, EmailID, MobileNo, ClientRowID, ClientRefID, PackageName, dob, aadhaarNumber);
                    obj.CandidateCheckMoveToPQCandidate(Convert.ToInt32(ClientRowID), personalRowID, ClientPackageRowID, clientcheckRowID);
                    string EncryptpersonalRowID = clsCommonMethods1.PEncrypt(Convert.ToString(personalRowID));
                    string redirectUrl = "https://bgv.securitas-india.com/Account/lvhcfnshgd?Id=" + EncryptpersonalRowID;
                    //string redirectUrl = "http://localhost:54586/Account/lvhcfnshgd?Id=" + EncryptpersonalRowID;
                    int intReturnValue = API.insertLivelinessStatusUserDetails(personalRowID, redirectUrl, RequestId, 0, 0, "insert");
                    ViewBag.Error2 = "Successfully generated a case initiation link...";
                    //======================================
                    //var ds = obj.Get_LivelinessStatusUserDetails(Convert.ToInt32(personalRowID), 0, "select2");
                    //if (ds.Rows.Count > 0)
                    //{
                    //    aadhaarNumber = Convert.ToString(ds.Rows[0]["AadhaarCardNo"]);
                    //    fullName = Convert.ToString(ds.Rows[0]["CandidateFullName"]);
                    //}
                    //Resdata = API.InitiateLiveliness_API(fullName, aadhaarNumber, redirectUrl, personalRowID);
                    //if (Resdata.Contains("RedirectUrl"))
                    //{
                    //    string[] strdate = Resdata.Split('|');
                    //    Url = strdate[1].ToString();
                    //    return Redirect(Url);

                    //}
                    //else
                    //{
                    //    ViewBag.Error = Resdata;
                    //}
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [HttpGet]
        public ActionResult lvhcfnshgd(string Id)
        {
            string Resdata = "";
            string Url = "";
            string aadhaarNumber = "";
            string fullName = "";
            string DecryptpersonalRowID = clsCommonMethods1.PDecrypt(Id);
            int personalRowID = Convert.ToInt32(DecryptpersonalRowID);
            try
            {
                string redirectUrl = "http://localhost:54586/Account/livelinessMessage?Id=" + personalRowID;
                //string redirectUrl = "https://bgv.securitas-india.com/Account/livelinessMessage?Id=" + personalRowID;
                var ds = obj.Get_LivelinessStatusUserDetails(Convert.ToInt32(personalRowID), 0, "select2");
                if (ds.Rows.Count > 0)
                {
                    aadhaarNumber = Convert.ToString(ds.Rows[0]["AadhaarCardNo"]);
                    fullName = Convert.ToString(ds.Rows[0]["CandidateFullName"]);
                    Resdata = API.InitiateLiveliness_API(fullName, aadhaarNumber, redirectUrl, personalRowID);
                    if (Resdata.Contains("RedirectUrl"))
                    {
                        string[] strdate = Resdata.Split('|');
                        Url = strdate[1].ToString();
                        return Redirect(Url);

                    }
                    else
                    {
                        ViewBag.Error = Resdata;
                    }
                }
                else
                {
                    ViewBag.Error = "Record not found !!!";
                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [HttpGet]
        public ActionResult livelinessMessage(string Id)
        {
            string RequestId = "";
            string Resdata = "";
            var ds = obj.Get_LivelinessStatusUserDetails(Convert.ToInt32(Id), 0, "select");
            if (ds.Rows.Count > 0)
            {
                RequestId = Convert.ToString(ds.Rows[0]["RequestId"]);
                Resdata = API.CheckLivelinessStatus_API(Id, RequestId);
                if (Resdata.Contains("completed") || Resdata.Contains("Completed"))
                {
                    ViewBag.Error = null;

                }
                else
                {
                    ViewBag.Error = Resdata;
                }

            }
            else
            {
                ViewBag.Error = "Record not found !!!";
            }

            return View();
        }
        #endregion
        #region 76 server GetDocumentDownloadAPI
        [HttpGet]
        public ActionResult GetDocumentDownloadAPI()
        {
            return View();
        }
        public ActionResult ResendLinkMessage(string msg = "")
        {
            if (msg != "")
            {
                if (clsCommonMethods.PDecrypt(msg) == "Mail Sent Successfully, Please check your inbox !")
                {
                    ViewBag.Success = clsCommonMethods.PDecrypt(msg);
                }
                else
                {
                    ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
                }
            }
            return View();
        }

        public ActionResult ResendLink(string PersonalIds)
        {
            ClientCustomize cc = new ClientCustomize();
            //TempPQPersonalInfoListPagedModel model = new TempPQPersonalInfoListPagedModel();
            PQPersonalSearchPagedModel SearchModel = new PQPersonalSearchPagedModel();
            try
            {
                short ClientRowID = 0;
                int ClientUserRowID = 0;
                string ClientNameMail = "";
                DataTable ClientIDDT = new DataTable();
                ClientIDDT = cc.GetClientRowId(Convert.ToInt32(PersonalIds));
                if (ClientIDDT.Rows.Count > 0)
                {
                    ClientRowID = Convert.ToInt16(ClientIDDT.Rows[0]["ClientRowID"]);
                    ClientUserRowID = Convert.ToInt32(ClientIDDT.Rows[0]["ClientUserRowID"]);
                    ClientNameMail = ClientIDDT.Rows[0]["ClientName"].ToString();
                }
                int pageSize = clsGlobalCommon.PageSize;
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
                                "</b><br/><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", "bgv.helpdesk@securitas-india.com", "'>", "bgv.helpdesk@securitas-india.com", "</a>");
                            }
                            else if (ClientRowID == 249)
                            {
                                MailSignature = string.Concat("For Securitas India. <br /><br />");
                                MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
                                "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", ShowSpocId1[0], "'>", ShowSpocId1[0], "</a>",
                                "</b><br /><b>Contact No:</b><br />  <b>Satya :</b>+91 8377022632 <br />  <b>Pradeep Devrani :</b> +91 8373907723<br /><b>Himanshu Pathak :</b> +91 7291974515 ");
                            }
                            else if (ClientRowID == 277)
                            {
                                MailSignature = string.Concat("For Securitas India. <br /><br />");
                                MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(), "</b><br />" + mobileno,
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

                            //Candidate Template Details
                            body = body.Replace("{document}", docString.ToString());
                            body = body.Replace("{CandidateName}", loginDetail.CandidateName);
                            body = body.Replace("#CompanyName", ClientNameMail);
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

                            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                            alternateView.LinkedResources.Add(Triangleres);
                            alternateView.LinkedResources.Add(Securitassec);
                            alternateView.LinkedResources.Add(Walsonssec);
                            alternateView.LinkedResources.Add(BGVdottedImg);
                            alternateView.LinkedResources.Add(RedbulllogoImg);
                            alternateView.LinkedResources.Add(JadeGlobalLogoIMG);

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
                                    if (ClientRowID == 503)
                                    {
                                        flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                    }
                                    else
                                    {
                                        if (ClientRowID == 257) //Biz 2 Credit Info Services Pvt. Ltd
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                        }
                                        else if (ClientRowID == 249)//sutherlandglobal 
                                        {

                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                        }
                                        // For TATA Elxsi
                                        else if (ClientRowID == 635)
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                        }
                                        else if (ClientRowID == 957)
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                        }
                                        else if (ClientRowID == 1312)
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body);
                                            flagspocandclient = mail.CandidateLoginSendMailToSpocAndClient(DisplayName, From, ToMail, Ccmail, Subject, bodySpocAndClient, null, null);
                                        }
                                        else if (ClientRowID == 778)
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                        }
                                        else if (ClientRowID == 1005 || ClientRowID == 2)
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);

                                        }
                                        else if (ClientRowID == 889)
                                        {
                                            flag = mail.SendMailPurgWithoutCC(DisplayName, From, ToMail, Subject, body, attachment);
                                        }
                                        else if (ClientRowID == 974)
                                        {
                                            flag = mail.SendMailAdidas(DisplayName, From, ToMail, Subject, body);
                                        }
                                        else if ((ClientRowID == 1496 || MailClientRowid == 2 || MailClientRowid == 193) && (MailTemplateName == "CandidateLoginDetailsTemplate" || MailTemplateNameSpoc == "CandidateLoginDetailsTemplate"))
                                        {
                                            flag = mail.CandidateLoginSendMail(DisplayName, From, ToMail, Subject, body, attachment, alternateView);
                                        }
                                        else if (ClientRowID == 1481)
                                        {
                                            flag = mail.SendMailToCandidateForCtrls(DisplayName, From, ToMail, Subject, body, attachment, attachment2);
                                        }
                                        else
                                        {
                                            flag = mail.SendMail(DisplayName, From, ToMail, Subject, body, attachment);
                                        }
                                    }
                                }
                            }
                            TempData["ErrorMsg"] = "Out of " + totRec.ToString() + " Mail sent : " + countSent.ToString() + " failed : " + countFailed.ToString();
                            ViewBag.ErrorMsg = "Mail Sent Successfully, Please check your inbox !";
                        }
                        else
                        {
                            TempData["ErrorMsg"] = "Candidate login details not found!";
                        }
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "There Is aan issue while sending mail";
                }
                #endregion
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return RedirectToAction("ResendLinkMessage", new { msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
        }

        [HttpPost]
        public ActionResult GetDocumentDownloadAPI(FormCollection frm)
        {
            try
            {
                string CompanyRefNo = Convert.ToString(frm["CompanyRefNo"]);
                string QType = Convert.ToString(frm["RequesttypeID"]);
                if (CompanyRefNo != "" && QType == "Download Document")
                {
                    if (CompanyRefNo.Contains(","))
                    {
                        string[] multiArray = CompanyRefNo.Split(new Char[] { ',', ':' });
                        foreach (var item in multiArray)
                        {
                            if (item != "")
                            {
                                var res = API.DownloadCandidateDocReport(item, "document");
                                ViewBag.Error += res;
                            }
                        }
                    }
                    else
                    {
                        var res = API.DownloadCandidateDocReport(CompanyRefNo, "document");
                        ViewBag.Error = res;
                    }
                }
                else if (CompanyRefNo != "" && QType == "Download Report")
                {
                    if (CompanyRefNo.Contains(","))
                    {
                        string[] multiArray = CompanyRefNo.Split(new Char[] { ',', ':' });
                        foreach (var item in multiArray)
                        {
                            var res = API.DownloadCandidateDocReport(item, "report");
                            ViewBag.Error += res;
                        }
                    }
                    else
                    {
                        var res = API.DownloadCandidateDocReport(CompanyRefNo, "report");
                        ViewBag.Error = res;
                    }
                }
                else
                {
                    ViewBag.Error = "Record not found !!!";
                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }
        #endregion

        /* Start Session Fixation or generate new ASP.NET_SessionID after Successfull login */

        public ActionResult DashboardClientForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Step 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["UserName"].ToString(), false);

                // Step 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("DashboardClient", "Home", new { area = "ClientArea" });
        }
        public ActionResult ClientEmailVerification2FAForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Step 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["UserName"].ToString(), false);

                // Step 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("ClientEmailVerification2FA", "Account");
        }
        public ActionResult DashboardClientNewForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["UserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("DashboardClientNew", "Home", new { area = "ClientArea" });
        }
        public void SessionGenerateForSessionFixation()
        {
            /*Start New Session generate for Session Fixation 21June2025*/
            var context = System.Web.HttpContext.Current;
            if (context != null || context.Session != null)
            {
                var session = context.Session;
                // Step 1: Save existing session data in oldSessionItems Dictionary
                var oldSessionItems = new Dictionary<string, object>();
                foreach (string key in session.Keys)
                {
                    oldSessionItems[key] = session[key];
                }

                // Step : Clear the current session
                Session.Clear();
                Session.Abandon();

                // Step 3: Remove existing session cookie from the response
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    var sessionCookie = new HttpCookie("ASP.NET_SessionId", "")
                    {
                        Expires = DateTime.Now.AddDays(-1),
                        HttpOnly = true
                    };
                    Response.Cookies.Add(sessionCookie);
                }
                // Step 4: Access session to create a new session and new session ID
                var newSession = context.Session;
                // Step 5: Restore old session data to new session if needed
                foreach (var kvp in oldSessionItems)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Step 6: Serialize to from Dictionary to JSON
                string jsonData = JsonConvert.SerializeObject(oldSessionItems);

                // Step 7: Store jsonData in cookie for traveling data to Redirect on new action
                var cookie = new HttpCookie("TempSessionData", jsonData)
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(5)
                };
                Response.Cookies.Add(cookie);
            }
            /* End Session Fixation or generate new ASP.NET_SessionID after Successfull login */
        }


        public void SessionGenerateForSessionFixationNew()
        {
            /*Start New Session generate for Session Fixation 21June2025*/
            var context = System.Web.HttpContext.Current;
            if (context != null || context.Session != null)
            {
                var session = context.Session;
                // Step 1: Save existing session data in oldSessionItems Dictionary
                var oldSessionItems = new Dictionary<string, object>();
                foreach (string key in session.Keys)
                {
                    oldSessionItems[key] = session[key];
                }

                // Step : Clear the current session
                Session.Clear();
                Session.Abandon();

                // Step 3: Remove existing session cookie from the response
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    var sessionCookie = new HttpCookie("ASP.NET_SessionId", "")
                    {
                        Expires = DateTime.Now.AddDays(-1),
                        HttpOnly = true
                    };
                    Response.Cookies.Add(sessionCookie);
                }
                // Step 4: Access session to create a new session and new session ID
                var newSession = context.Session;
                // Step 5: Restore old session data to new session if needed
                foreach (var kvp in oldSessionItems)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Step 6: Serialize to from Dictionary to JSON
                string jsonData = JsonConvert.SerializeObject(oldSessionItems);

                // Step 7: Store jsonData in cookie for traveling data to Redirect on new action
                var cookie = new HttpCookie("TempSessionData", jsonData)
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(5)
                };
                Response.Cookies.Add(cookie);
            }
            /* End Session Fixation or generate new ASP.NET_SessionID after Successfull login */
        }


        /* Start Session Fixation or generte new ASP.NET_SessionId after Successfull login of Candidate Login*/
        public ActionResult IndexInsuffCandidateLoginForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["CadidateUserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("Index", "CandidateInsuff", new { area = "Candidate" });
        }
        public ActionResult PersonalInfoCandidateLoginForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["CadidateUserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("PersonalInfo", "Home", new { area = "Candidate" });
        }
        public ActionResult IndexHomeCandidateLoginForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["CadidateUserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("Index", "Home", new { area = "Candidate" });
        }
        public ActionResult CandidateFOACandidateLoginForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["CadidateUserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("CandidateFOA", "Home", new { area = "Candidate" });
        }

        public ActionResult CandidateIngramCandidateLoginForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["CadidateUserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("Index", "Home", new { area = "Candidate" });

        }
        /* End Session Fixation or generte new ASP.NET_SessionId after Successfull login of Candidate Login*/

        /* Start Session Fixation or generte new ASP.NET_SessionId after Successfull login of C Login*/
        public ActionResult CLoginReIndexLoginForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["CadidateUserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("ReIndex", "Home", new { area = "Candidate" });
        }
        /* End Session Fixation or generte new ASP.NET_SessionId after Successfull login of Candidate Login*/

        /* Start Session Fixation or generte new ASP.NET_SessionId after Successfull login of Vendor Login*/
        public ActionResult VendorLoginForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["UserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("Index", "Home", new { area = "Partner" });
        }
        /* End Session Fixation or generte new ASP.NET_SessionId after Successfull login of Vendor Login*/

        /* Start Session Fixation or generte new ASP.NET_SessionId after Successfull login of Vendor Login*/
        public ActionResult IndexLoginSelectDesignationForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["CRPUserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            ClientCustomize cmnew = new ClientCustomize();
            int desgId = cmnew.GetDesignationID(Convert.ToInt16(Session["TeamMemberRowID"]));
            if (desgId == 79)
            {
                int teamdepartmentrowid = cmnew.GetTeamDepartmentRowId(Convert.ToInt16(Session["TeamMemberRowID"]));

                string EncTeamDepartmentRowID = clsCommonMethods.PEncrypt(teamdepartmentrowid.ToString());
                return RedirectToAction("RedirectToDashboard", "Home", new { @teamdeptid = EncTeamDepartmentRowID });
            }
            return RedirectToAction("SelectDesignation", "Home");
        }
        /* End Session Fixation or generte new ASP.NET_SessionId after Successfull login of Vendor Login*/

        /* Start Session Fixation or generte new ASP.NET_SessionId after Successfull login of SignIn Login*/
        public ActionResult SignInLoginSelectDesignationForSessionFixation()
        {
            var cookieTempSessionData = Request.Cookies["TempSessionData"];
            if (cookieTempSessionData != null && !string.IsNullOrEmpty(cookieTempSessionData.Value))
            {
                // Step1: Deserialize TempSessionData
                var sessionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieTempSessionData.Value);

                var context = System.Web.HttpContext.Current;
                var newSession = context.Session;
                //Step 2:  Restore old session data to new session if needed
                foreach (var kvp in sessionData)
                {
                    newSession[kvp.Key] = kvp.Value;
                }
                // Stpe 3: Set authentication cookie (optional)
                FormsAuthentication.SetAuthCookie(Session["CRPUserName"].ToString(), false);

                // Stpe 4: Remove the TempSessionData temp cookie
                var deleteCookie = new HttpCookie("TempSessionData")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(deleteCookie);
            }
            return RedirectToAction("SelectDesignation", "Home");
        }



        /* End Session Fixation or generte new ASP.NET_SessionId after Successfull login of SignIn Login*/
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

        [HttpGet]
        public ActionResult LoginToSendOnboardingMailToClient(string msg = "")
        {
            if (msg != "")
            {
                ViewBag.ErorrMsg = msg;
            }
            return View();
        }

        [HttpPost]
        public ActionResult LoginToSendOnboardingMailToClient(string UserOTP = "", string CheckOTPbyInput = "", string PassInputEmailID = "")
        {
            if (UserOTP != "")
            {
                string CheckIngOTP = "";
                CheckIngOTP = CheckOTPbyInput;
                if (CheckIngOTP == UserOTP)
                {
                    return RedirectToAction("ClientOnboardingTemplateDetails", "Account", new { UserEmailID = clsCommonMethods.PEncrypt(PassInputEmailID) });
                }
                else
                {
                    ViewBag.ErrorMsg = "invalid OTP!";
                }
            }
            else
            {
                ViewBag.ErrorMsg = "invalid OTP!";
                return RedirectToAction("LoginToSendOnboardingMailToClient", "Account", new { msg = ViewBag.ErrorMsg });

            }
            return RedirectToAction("LoginToSendOnboardingMailToClient", "Account", new { msg = ViewBag.ErrorMsg });
        }


        public JsonResult LoginToSendOnboardingMailToClientOTP(string UserEmailID = "")
        {
            try
            {
                if (UserEmailID.Contains("@securitas-india.com") || UserEmailID.Contains("@securitas-india.in"))
                {
                    ClientCustomize CCC = new ClientCustomize();
                    Random generator = new Random();
                    String mailOTP = generator.Next(0, 999999).ToString("D6");
                    TempData["EmailOTP"] = mailOTP;
                    ViewBag.EmailOTP = mailOTP;
                    Session["UserLoginOTPcheck"] = mailOTP;
                    ViewBag.OTPcheck = true;
                    ClientCustomize cc = new ClientCustomize();
                    var user_EmailId = UserEmailID;
                    string to = user_EmailId.ToString();
                    string display = "BGV Securitas";
                    string subj = "OTP For successfull login";
                    string from = "bgv.request@securitas-india.com";
                    string msg = "Your one time OTP for Login is = " + "<span style='color:red;'>" +
                    mailOTP + "</span>" + "</br>" + ".Only valid for 2 min.";
                    clsSendMail clsMail = new clsSendMail();
                    var val = clsMail.SendMail(display, from, to, subj, msg);
                    return Json(new { msg = mailOTP }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.ErrorMsg = "Please enter a valid company email address(securitas-india.com or securitas-india.in).";
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return Json(new { msg = ViewBag.ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = ViewBag.ErrorMsg }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult ClientOnboardingTemplateDetails(string UserEmailID, string ClientRowID = "", string msg = "")
        {
            if (msg != "")
            {
                ViewBag.ErrorMsg = msg;
            }
            ClientOnboadringviewModel model = new ClientOnboadringviewModel();
            ClientCustomize cc = new ClientCustomize();
            BindClientList();
            int clientid = 0;
            string useridloginby = "";
            if (UserEmailID != "" && UserEmailID != null)
            {
                useridloginby = clsCommonMethods.PDecrypt(UserEmailID);
            }
            if (useridloginby != "")
            {
                model.LoginByMailID = useridloginby;
                if (ClientRowID != "")
                {
                    clientid = Convert.ToInt32(ClientRowID);
                }
                if (clientid != 0)
                {
                    string txtFolderPath = ConfigurationManager.AppSettings["ClientmailImages"].ToString();
                    ViewBag.bgpath = "/Images/ClientOnboardingImg/" + "NewBGD1.png";

                    DataTable dt = new DataTable();
                    dt = cc.ClientOnboardingMailTemplateDetails(Convert.ToInt32(clientid));
                    if (dt.Rows.Count > 0)
                    {
                        model.ClientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                        model.PrimarySPOC = dt.Rows[0]["PrimarySPOC"].ToString();
                        model.PrimarySPOCName = dt.Rows[0]["PrimarySPOCName"].ToString();
                        model.PrimarySPOCDesignation = dt.Rows[0]["PrimarySPOCDesignation"].ToString();
                        model.FirstLevelEscaltion = dt.Rows[0]["FirstLevelEscaltion"].ToString();
                        model.FirstLevelEscaltionName = dt.Rows[0]["FirstLevelEscaltionName"].ToString();
                        model.FirstLevelEscaltionDesignation = dt.Rows[0]["FirstLevelEscaltionDesignation"].ToString();
                        model.SecondLevelEscaltion = dt.Rows[0]["SecondLevelEscaltion"].ToString();
                        model.SecondLevelEscaltionName = dt.Rows[0]["SecondLevelEscaltionName"].ToString();
                        model.SecondLevelEscaltionDesignation = dt.Rows[0]["SecondLevelEscaltionDesignation"].ToString();
                        model.ThirdLevelEscaltion = dt.Rows[0]["ThirdLevelEscaltion"].ToString();
                        model.ThirdLevelEscaltionName = dt.Rows[0]["ThirdLevelEscaltionName"].ToString();
                        model.ThirdLevelEscaltionDesignation = dt.Rows[0]["ThirdLevelEscaltionDesignation"].ToString();
                        model.ClientRelationshipConnect = dt.Rows[0]["ClientRelationshipConnect"].ToString();
                        model.ClientRelationshipConnectName = dt.Rows[0]["ClientRelationshipConnectName"].ToString();
                        model.ClientRelationshipConnectDesignation = dt.Rows[0]["ClientRelationshipConnectDesignation"].ToString();
                        model.SalesConnect = dt.Rows[0]["SalesConnect"].ToString();
                        model.SalesConnectName = dt.Rows[0]["SalesConnectName"].ToString();
                        model.SalesConnectDesignation = dt.Rows[0]["SalesConnectDesignation"].ToString();
                        model.Status = Convert.ToInt32(dt.Rows[0]["Status"]);
                        model.PrimarySPOCMobileNo = dt.Rows[0]["PrimarySPOCMobileNo"].ToString();
                        model.FirstLevelEscaltionMobileNo = dt.Rows[0]["FirstLevelEscaltionMobileNo"].ToString();
                        model.SecondLevelEscaltionMobileNo = dt.Rows[0]["SecondLevelEscaltionMobileNo"].ToString();
                        model.ThirdLevelEscaltionMobileNo = dt.Rows[0]["ThirdLevelEscaltionMobileNo"].ToString();
                        model.ClientRelationshipConnectMobileNo = dt.Rows[0]["ClientRelationshipConnectMobileNo"].ToString();
                        model.SalesConnectMobileNo = dt.Rows[0]["SalesConnectMobileNo"].ToString();
                        model.MailSendStatus = Convert.ToInt32(dt.Rows[0]["MailSendStatus"]);
                        model.ClientMailToSend = dt.Rows[0]["ClientMailToSend"].ToString();
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Client Spoc details not found!";
                    }
                }
                else
                {
                    ViewBag.ErorrMsg = "Select Client";
                }
            }
            else
            {
                return RedirectToAction("LoginToSendOnboardingMailToClient", "Account", new { msg = ViewBag.ErrorMsg });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult ClientOnboardingTemplateDetails(FormCollection formCollection)
        {
            bool Result = false;
            ClientOnboadringviewModel model = new ClientOnboadringviewModel();
            ClientCustomize cc = new ClientCustomize();
            model.ClientRowID = Convert.ToInt32(formCollection["ClientRowID"]);
            model.LoginByMailID = formCollection["UserEmailID"].ToString();
            model.ClientMailToSend = formCollection["ToEmailID"].ToString();
            //model.MailSendFrom = formCollection["FromEmailID"].ToString();
            string email = formCollection["FromEmailID"].ToString();
            if (model.ClientRowID != 0)
            {
                if (email.Contains("@securitas-india.com") || email.Contains("@securitas-india.in"))
                {
                    model.MailSendFrom = email;
                    Result = ClientOnboardingMailTemplates(Convert.ToInt32(model.ClientRowID), model.ClientMailToSend, model.MailSendFrom);
                    if (Result == true)
                    {
                        cc.sp_UpdateClientMailSendingSpocDetailsStatus(Convert.ToInt32(model.ClientRowID), model.MailSendFrom);
                        ViewBag.ErrorMsg = "Mail sending Succesfully!";
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Mail sending Failed!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Please enter a valid company email address (@securitas-india.com or @securitas-india.in).";
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Please Select Client!";
            }
            return RedirectToAction("ClientOnboardingTemplateDetails", "Account", new { UserEmailID = model.LoginByMailID, ClientRowID = model.ClientRowID, msg = ViewBag.ErrorMsg });
        }

        public bool ClientOnboardingMailTemplates(int id, string ClientMailToSend = "", string MailSendFrom = "")
        {
            bool result = false;
            int ClientRowID = id;
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                //dt = cc.GetClientRowIdByClientAbbRowID(id);
                //if (dt.Rows.Count > 0)
                //{
                //    ClientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                //}
                string PrimarySPOC = "";
                string PrimarySPOCName = "";
                string PrimarySPOCDesignation = "";
                string FirstLevelEscaltion = "";
                string FirstLevelEscaltionName = "";
                string FirstLevelEscaltionDesignation = "";
                string SecondLevelEscaltion = "";
                string SecondLevelEscaltionName = "";
                string SecondLevelEscaltionDesignation = "";
                string ThirdLevelEscaltion = "";
                string ThirdLevelEscaltionName = "";
                string ThirdLevelEscaltionDesignation = "";
                string ClientRelationshipConnect = "";
                string ClientRelationshipConnectName = "";
                string ClientRelationshipConnectDesignation = "";
                string SalesConnect = "";
                string SalesConnectName = "";
                string SalesConnectDesignation = "";
                string PrimarySPOCMobileNo = "";
                string FirstLevelEscaltionMobileNo = "";
                string SecondLevelEscaltionMobileNo = "";
                string ThirdLevelEscaltionMobileNo = "";
                string ClientRelationshipConnectMobileNo = "";
                string SalesConnectMobileNo = "";

                DataTable spocdetails = new DataTable();
                spocdetails = cc.ClientOnboardingMailTemplateDetails(ClientRowID);
                //SendClientLoginInfoViewModel clientLoginInfo = repoLogin.GetClientLoginInfoforSendMail(ClientRowID);
                if (spocdetails != null)
                {
                    if (spocdetails.Rows.Count > 0)
                    {
                        //ClientMailToSend = spocdetails.Rows[0]["ClientMailToSend"].ToString();
                        PrimarySPOC = spocdetails.Rows[0]["PrimarySPOC"].ToString();
                        PrimarySPOCName = spocdetails.Rows[0]["PrimarySPOCName"].ToString();
                        PrimarySPOCDesignation = spocdetails.Rows[0]["PrimarySPOCDesignation"].ToString();
                        FirstLevelEscaltion = spocdetails.Rows[0]["FirstLevelEscaltion"].ToString();
                        FirstLevelEscaltionName = spocdetails.Rows[0]["FirstLevelEscaltionName"].ToString();
                        FirstLevelEscaltionDesignation = spocdetails.Rows[0]["FirstLevelEscaltionDesignation"].ToString();
                        SecondLevelEscaltion = spocdetails.Rows[0]["SecondLevelEscaltion"].ToString();
                        SecondLevelEscaltionName = spocdetails.Rows[0]["SecondLevelEscaltionName"].ToString();
                        SecondLevelEscaltionDesignation = spocdetails.Rows[0]["SecondLevelEscaltionDesignation"].ToString();
                        ThirdLevelEscaltion = spocdetails.Rows[0]["ThirdLevelEscaltion"].ToString();
                        ThirdLevelEscaltionName = spocdetails.Rows[0]["ThirdLevelEscaltionName"].ToString();
                        ThirdLevelEscaltionDesignation = spocdetails.Rows[0]["ThirdLevelEscaltionDesignation"].ToString();
                        ClientRelationshipConnect = spocdetails.Rows[0]["ClientRelationshipConnect"].ToString();
                        ClientRelationshipConnectName = spocdetails.Rows[0]["ClientRelationshipConnectName"].ToString();
                        ClientRelationshipConnectDesignation = spocdetails.Rows[0]["ClientRelationshipConnectDesignation"].ToString();
                        SalesConnect = spocdetails.Rows[0]["SalesConnect"].ToString();
                        SalesConnectName = spocdetails.Rows[0]["SalesConnectName"].ToString();
                        SalesConnectDesignation = spocdetails.Rows[0]["SalesConnectDesignation"].ToString();
                        PrimarySPOCMobileNo = spocdetails.Rows[0]["PrimarySPOCMobileNo"].ToString();
                        FirstLevelEscaltionMobileNo = spocdetails.Rows[0]["FirstLevelEscaltionMobileNo"].ToString();
                        SecondLevelEscaltionMobileNo = spocdetails.Rows[0]["SecondLevelEscaltionMobileNo"].ToString();
                        ThirdLevelEscaltionMobileNo = spocdetails.Rows[0]["ThirdLevelEscaltionMobileNo"].ToString();
                        ClientRelationshipConnectMobileNo = spocdetails.Rows[0]["ClientRelationshipConnectMobileNo"].ToString();
                        SalesConnectMobileNo = spocdetails.Rows[0]["SalesConnectMobileNo"].ToString();
                    }
                    string body = string.Empty;
                    string MailSubject = "";
                    var templateDB = cc.getAlertMailtemplate(ClientRowID, "ClientOnboardingMail", 0, 1, "ClientOnboardingMail");
                    if (templateDB.Rows.Count > 0)
                    {
                        body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                        MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                    }

                    //string txtFolderPath = ConfigurationManager.AppSettings["ClientmailImages"].ToString();

                    //LinkedResource bgImage = new LinkedResource(Server.MapPath("~/Images/ClientOnboardingImg/NewBGD1.png"), MediaTypeNames.Image.Jpeg);
                    //bgImage.ContentId = "DomesticBG1";

                    //LinkedResource D1Img = new LinkedResource(Server.MapPath(txtFolderPath) + "D1.png", MediaTypeNames.Image.Jpeg);
                    //D1Img.ContentId = Guid.NewGuid().ToString();
                    //string D1 = @"<img src='cid:" + D1Img.ContentId + @"'width:100%;'/>";

                    //LinkedResource D2Img = new LinkedResource(Server.MapPath(txtFolderPath) + "D2.png", MediaTypeNames.Image.Jpeg);
                    //D2Img.ContentId = Guid.NewGuid().ToString();
                    //string D2 = @"<img src='cid:" + D2Img.ContentId + @"'width:100%;'/>";

                    //LinkedResource D3Img = new LinkedResource(Server.MapPath(txtFolderPath) + "D3.png", MediaTypeNames.Image.Jpeg);
                    //D3Img.ContentId = Guid.NewGuid().ToString();
                    //string D3 = @"<img src='cid:" + D3Img.ContentId + @"'width:100%;'/>";

                    //LinkedResource ClickbtnImg = new LinkedResource(Server.MapPath(txtFolderPath) + "Clickbtn.png", MediaTypeNames.Image.Jpeg);
                    //ClickbtnImg.ContentId = Guid.NewGuid().ToString();
                    //string Clickbtn = @"<img src='cid:" + ClickbtnImg.ContentId + @"width:100%;'/>";

                    //LinkedResource D4Img = new LinkedResource(Server.MapPath(txtFolderPath) + "D4.png", MediaTypeNames.Image.Jpeg);
                    //D4Img.ContentId = Guid.NewGuid().ToString();
                    //string D4 = @"<img src='cid:" + D4Img.ContentId + @"'width:95%;'/>";

                    //LinkedResource D5Img = new LinkedResource(Server.MapPath(txtFolderPath) + "D5.png", MediaTypeNames.Image.Jpeg);
                    //D5Img.ContentId = Guid.NewGuid().ToString();
                    //string D5 = @"<img src='cid:" + D5Img.ContentId + @"'width='100%'/>";

                    //LinkedResource WalsonslogoImg = new LinkedResource(Server.MapPath(txtFolderPath) + "Walsonslogo.png", MediaTypeNames.Image.Jpeg);
                    //WalsonslogoImg.ContentId = Guid.NewGuid().ToString();
                    //string Walsonslogo = @"<img src='cid:" + WalsonslogoImg.ContentId + @"'width='150' 'style='width:150px;'/>";

                    //LinkedResource DFooterImg = new LinkedResource(Server.MapPath(txtFolderPath) + "DFooterImg.png", MediaTypeNames.Image.Jpeg);
                    //DFooterImg.ContentId = Guid.NewGuid().ToString();
                    //string DFooter = @"<img src='cid:" + DFooterImg.ContentId + @"' width='400' 'style='width:400px;'/>";

                    string txtFolderPath = ConfigurationManager.AppSettings["ClientmailImages"].ToString();
                    LinkedResource D1Img = new LinkedResource(Server.MapPath(txtFolderPath) + "D1.png", MediaTypeNames.Image.Jpeg);
                    D1Img.ContentId = Guid.NewGuid().ToString();
                    string D1 = @"<img src='cid:" + D1Img.ContentId + @"'width='100%''style='display:block;''/>";

                    LinkedResource D2Img = new LinkedResource(Server.MapPath(txtFolderPath) + "D2.png", MediaTypeNames.Image.Jpeg);
                    D2Img.ContentId = Guid.NewGuid().ToString();
                    string D2 = @"<img src='cid:" + D2Img.ContentId + @"'width='100%''style='display:block;'/>";

                    LinkedResource D3Img = new LinkedResource(Server.MapPath(txtFolderPath) + "D3.png", MediaTypeNames.Image.Jpeg);
                    D3Img.ContentId = Guid.NewGuid().ToString();
                    string D3 = @"<img src='cid:" + D3Img.ContentId + @"'width='100%''style='display:block;'/>";

                    LinkedResource D4Img = new LinkedResource(Server.MapPath(txtFolderPath) + "D4.png", MediaTypeNames.Image.Jpeg);
                    D4Img.ContentId = Guid.NewGuid().ToString();
                    string D4 = @"<img src='cid:" + D4Img.ContentId + @"'width='100%''style='display:block;'/>";

                    LinkedResource D5Img = new LinkedResource(Server.MapPath(txtFolderPath) + "D225.png", MediaTypeNames.Image.Jpeg);
                    D5Img.ContentId = Guid.NewGuid().ToString();
                    string D5 = @"<img src='cid:" + D5Img.ContentId + @"'width='100%'/>";

                    LinkedResource WalsonslogoImg = new LinkedResource(Server.MapPath(txtFolderPath) + "Walsonslogo(1).png", MediaTypeNames.Image.Jpeg);
                    WalsonslogoImg.ContentId = Guid.NewGuid().ToString();
                    string Walsonslogo = @"<img src='cid:" + WalsonslogoImg.ContentId + @"'width='25%'/>";

                    LinkedResource DFooterImg = new LinkedResource(Server.MapPath(txtFolderPath) + "DFooterImg.png", MediaTypeNames.Image.Jpeg);
                    DFooterImg.ContentId = Guid.NewGuid().ToString();
                    string DFooter = @"<img src='cid:" + DFooterImg.ContentId + @"' width='45%'/>";

                    body = body.Replace("{PrimarySPOC}", PrimarySPOC);
                    body = body.Replace("{PrimarySPOCName}", PrimarySPOCName);
                    body = body.Replace("{PrimarySPOCDesignation}", PrimarySPOCDesignation);
                    body = body.Replace("{FirstLevelEscaltion}", FirstLevelEscaltion);
                    body = body.Replace("{FirstLevelEscaltionName}", FirstLevelEscaltionName);
                    body = body.Replace("{FirstLevelEscaltionDesignation}", FirstLevelEscaltionDesignation);
                    body = body.Replace("{SecondLevelEscaltion}", SecondLevelEscaltion);
                    body = body.Replace("{SecondLevelEscaltionName}", SecondLevelEscaltionName);
                    body = body.Replace("{SecondLevelEscaltionDesignation}", SecondLevelEscaltionDesignation);
                    body = body.Replace("{ThirdLevelEscaltion}", ThirdLevelEscaltion);
                    body = body.Replace("{ThirdLevelEscaltionName}", ThirdLevelEscaltionName);
                    body = body.Replace("{ThirdLevelEscaltionDesignation}", ThirdLevelEscaltionDesignation);
                    body = body.Replace("{ClientRelationshipConnect}", ClientRelationshipConnect);
                    body = body.Replace("{ClientRelationshipConnectName}", ClientRelationshipConnectName);
                    body = body.Replace("{ClientRelationshipConnectDesignation}", ClientRelationshipConnectDesignation);
                    body = body.Replace("{SalesConnect}", SalesConnect);
                    body = body.Replace("{SalesConnectName}", SalesConnectName);
                    body = body.Replace("{SalesConnectDesignation}", SalesConnectDesignation);
                    body = body.Replace("{PrimarySPOCMobileNo}", PrimarySPOCMobileNo);
                    body = body.Replace("{FirstLevelEscaltionMobileNo}", FirstLevelEscaltionMobileNo);
                    body = body.Replace("{SecondLevelEscaltionMobileNo}", SecondLevelEscaltionMobileNo);
                    body = body.Replace("{ThirdLevelEscaltionMobileNo}", ThirdLevelEscaltionMobileNo);
                    body = body.Replace("{ClientRelationshipConnectMobileNo}", ClientRelationshipConnectMobileNo);
                    body = body.Replace("{SalesConnectMobileNo}", SalesConnectMobileNo);
                    //body = body.Replace("{DomesticBG1}", BGD1);
                    // body = body.Replace("{DomesticBG1}", "cid:DomesticBG1");
                    body = body.Replace("{Domestic1}", D1);
                    body = body.Replace("{Domestic2}", D2);
                    body = body.Replace("{Domestic3}", D3);
                    //body = body.Replace("{Clickbtn}", Clickbtn);
                    body = body.Replace("{Domestic4}", D4);
                    body = body.Replace("{Domestic5}", D5);
                    body = body.Replace("{Walsonslogos}", Walsonslogo);
                    body = body.Replace("{DFooterImges}", DFooter);

                    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                    //alternateView.LinkedResources.Add(bgImage);
                    //alternateView.LinkedResources.Add(BGD1Img);
                    alternateView.LinkedResources.Add(D1Img);
                    alternateView.LinkedResources.Add(D2Img);
                    alternateView.LinkedResources.Add(D3Img);
                    //alternateView.LinkedResources.Add(ClickbtnImg);
                    alternateView.LinkedResources.Add(D4Img);
                    alternateView.LinkedResources.Add(D5Img);
                    alternateView.LinkedResources.Add(WalsonslogoImg);
                    alternateView.LinkedResources.Add(DFooterImg);

                    clsSendMail mail = new clsSendMail();
                    var signature = mail.GetCompanyMailSignature();
                    string Subject = "Welcome To Securitas";
                    string To = ClientMailToSend;
                    //string Cc = "";
                    string From = MailSendFrom;
                    string DisplayName = signature.CompanyName;
                    string MailSignature = string.Concat("For Securitas India. <br /><br />");
                    result = mail.ClientOnBoardingSendMail(DisplayName, From, To, Subject, body, alternateView);
                }
                else
                {
                    ViewBag.ErrorMsg = "Client Spoc details not found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return result;
        }

        [NonAction]
        private void fnSaveLogOutDetails()
        {
            AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
            try
            {
                loggedInDetails.LoginName = Session["TeamMemberName"].ToString();
                loggedInDetails.LoginType = "Company";
                loggedInDetails.LoginId = Session["CRPUserName"].ToString();
                loggedInDetails.LoginBy = "";
                loggedInDetails.IPDetails = Request.UserHostAddress;
                loggedInDetails.LoginDate = DateTime.Now;
                loggedInDetails.LoginStatus = 1;
                loggedInDetails.LogoutTime = DateTime.Now;

                Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);
                ClientCustomize cn = new ClientCustomize();
                cn.Insert_LoginLog("User LogOut", "LogOut ! " + loggedInDetails.LoginId, loggedInDetails.LoginId, "UserName: " + loggedInDetails.LoginName + " IP Address :" + GetIP_Address());

            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                throw Ex;
            }
        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult FaceSignIn(string PId = "", string type = "Liveness", string Msg = "")
        {
            FaceCandidateLoginViewModel model = new FaceCandidateLoginViewModel();
            try
            {
                if (PId != "")
                {
                    //string link = "FaceSignIn?Pid=" + Convert.ToString(clsCommonMethods.PEncrypt("195413")) + "&type=Liveness";
                    model.PersonalRowId = Convert.ToString(clsCommonMethods.PDecrypt(PId));
                    model.loginType = type;
                }
                else
                {
                    model.PersonalRowId = "";
                    model.loginType = type;
                    ViewBag.ErrorMsg = "Please click right link";
                }
                Session["AuthenticationToken"] = "";
                Response.Cookies.Clear();
                Session["Counter"] = 0;
                Session["UserLogin"] = "!Candidate User";
                ViewBag.ErrorMsg = Msg;
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult FaceSignIn(FaceCandidateLoginViewModel model)
        {
            try
            {
                if (clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) != model.CaptchaVal)
                {
                    ModelState.AddModelError("", "Captcha validation failed.");
                    string msg = clsCommonMethods.PDecrypt(Convert.ToString(Session["Captcha"])) + "=" + model.CaptchaVal;
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), msg, "Index");
                    return RedirectToAction("FaceSignIn", "Account", new { PId = clsCommonMethods.PEncrypt(model.PersonalRowId), type = "Liveness", msg = clsCommonMethods.PEncrypt("Captcha validation failed") });

                }
                //else if (Convert.ToString(Session["OTP"]) != Convert.ToString(model.MobileOTP))
                //{1`
                //    return RedirectToAction("FaceSignIn", "Account", new { PId = clsCommonMethods.PEncrypt(model.PersonalRowId), type = "Liveness", msg = "Worng OTP! Please Try Again !" });
                //}
                DataBaseConnections Dc = new DataBaseConnections();
                //var Dt = Dc.GetPQPersonalWithMobileNo(Convert.ToInt32(model.PersonalRowId), model.MobileNo);
                var Dt = Dc.GetPQPersonalWithEmailId(Convert.ToInt32(model.PersonalRowId), model.MobileNo);
                if (Dt.Rows.Count > 0)
                {
                    Session["CadidateUserName"] = Convert.ToString(Dt.Rows[0]["UserID"]); ;
                    Session["CandidateUserRowID"] = Convert.ToString(Dt.Rows[0]["CandidateUserRowID"]);
                    Session["HRApproval"] = Convert.ToString(Dt.Rows[0]["HRApprovalRequired"]);
                    Session["LoginCandidateName"] = Convert.ToString(Dt.Rows[0]["CandidateFullName"]);
                    Session["CandidateEmail"] = Convert.ToString(Dt.Rows[0]["CandidateEmail"]);
                    Session["YTRApplicable"] = Convert.ToInt32(Dt.Rows[0]["YTRApplicable"]);
                    Session["CreatedBy"] = Convert.ToString(Dt.Rows[0]["CreatedBy"]);
                    Session["ClientUType"] = "Candidate";
                    Session["Valid"] = 1;
                    Session["LoginType"] = "";
                    Session["Agreementsign"] = Convert.ToString(Dt.Rows[0]["Agreementsign"]);

                    Session["PersonalRowId"] = Convert.ToString(Dt.Rows[0]["PersonalRowId"]);
                    Session["TempPersonalRowID"] = Convert.ToString(Dt.Rows[0]["PersonalRowId"]);
                    Session["MobileNo"] = Convert.ToString(Dt.Rows[0]["MobileNo"]);
                    Session["ClientRowID"] = Convert.ToString(Dt.Rows[0]["ClientRowId"]);
                    Session["UserName"] = Convert.ToString(Dt.Rows[0]["CandidateFullName"]);
                    Session["ClientNameMail"] = Convert.ToString(Dt.Rows[0]["EmailId"]);
                    Session["ClientAccessType"] = "1";
                    Session["ClientName"] = Convert.ToString(Dt.Rows[0]["ClientName"]);

                    string localIP = GetIPAddress();
                    string ipaddress;
                    ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (ipaddress == "" || ipaddress == null)
                        ipaddress = Request.ServerVariables["REMOTE_ADDR"];
                    Session["ClientUserRowID"] = 0;

                    String guid = Convert.ToString(Guid.NewGuid());
                    Session["AuthenticationToken"] = guid;
                    Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));
                    Session["SessionToken"] = guid;
                    Session["Valid"] = 1;
                    if (Convert.ToInt32(Session["Valid"]) == 1)
                    {
                        DateTime dateTime = DateTime.Today;
                        TimeSpan TimeSinceCreation = new TimeSpan();
                        if (Convert.ToDateTime(Dt.Rows[0]["CreatedDate"]) != null)
                        {
                            TimeSinceCreation = dateTime.Subtract(Convert.ToDateTime(Dt.Rows[0]["CreatedDate"]));
                        }
                        else
                        {
                            TimeSinceCreation = dateTime.Subtract(Convert.ToDateTime(Dt.Rows[0]["CreatedDate"]));
                        }
                    }
                    AddUserLoggedInDetailViewModel loggedInDetails = new AddUserLoggedInDetailViewModel();
                    loggedInDetails.LoginName = Convert.ToString(Dt.Rows[0]["CandidateFullName"]);
                    loggedInDetails.LoginType = Convert.ToString(Dt.Rows[0]["MobileNo"]);
                    loggedInDetails.LoginId = Convert.ToString(Dt.Rows[0]["CreatedBy"]);
                    loggedInDetails.LoginBy = "Candidate";
                    string hostNameMachine = Dns.GetHostName();
                    int len = Dns.GetHostByName(hostNameMachine).AddressList.Length;
                    string myIPAddr = "";
                    for (int i = 0; i <= len - 1; i++)
                    {
                        myIPAddr = Dns.GetHostByName(hostNameMachine).AddressList[i].ToString();
                    }
                    loggedInDetails.IPDetails = myIPAddr;
                    loggedInDetails.LoginDate = DateTime.Now;
                    loggedInDetails.LoginStatus = 1;
                    loggedInDetails.Deviceidentifiers = Request.UserAgent;

                    loggedInDetails.BrowserName = Request.Browser.Browser.ToString();

                    Session["LoginDetailRowID"] = repoAccount.AddUserLoggedInDetails(loggedInDetails);
                    Session["UserLogin"] = "Candidate User";
                    if (model.loginType == "Liveness")
                    {
                        return RedirectToAction("AddlivenessInfo", "Home", new { area = "Candidate", Pid = clsCommonMethods.PEncrypt(Convert.ToString(Dt.Rows[0]["PersonalRowId"])) });
                    }
                    else
                    {
                        return RedirectToAction("AddFaceMatchInfo", "Home", new { area = "Candidate", Pid = clsCommonMethods.PEncrypt(Convert.ToString(Dt.Rows[0]["PersonalRowId"])) });
                    }

                }
                else
                {
                    Session["AuthenticationToken"] = "";
                    Response.Cookies.Clear();
                    Session["Counter"] = 0;
                    Session["UserLogin"] = "!Candidate User";
                }

            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return RedirectToAction("FaceSignIn", "Account", new { PId = clsCommonMethods.PEncrypt(model.PersonalRowId), type = "Liveness", msg = "Worng OTP! Please Try Again !" });

        }



        public string GetCaptchaValue()
        {
            Random generator = new Random();
            String captchaVal = generator.Next(100000, 999999).ToString("D6");
            return captchaVal;
        }
        public ActionResult OTP(string MobileNo,string Pid="")
        {
            //var OTP = GetCaptchaValue();
            //Session["MobileNo"] = MobileNo;
            //Session["OTP"] = OTP;
            //try
            //{
            //    //WhatsAppAPI obj = new WhatsAppAPI();
            //    //var objMessage = obj.SendWhatsAppMessage1("+91" + MobileNo, "verification_temp", OTP);

            //    sendmailWithOtp(MobileNo);
            //}
            //catch (Exception ex)
            //{
            //}
            //return Json(new { success = OTP });

            string EmailId = MobileNo;
            var OTP = "";//;GetCaptchaValue();
            DataBaseConnections Dc = new DataBaseConnections();
            var Dt = Dc.GetPQPersonalWithEmailId(Convert.ToInt32(Pid), EmailId);

            Session["MobileNo"] = Dt.Rows[0]["MobileNo"];
            try
            {               
                OTP = sendmailWithOtp(EmailId);
                Session["OTP"] = OTP;
            }
            catch (Exception ex)
            {
            }
            return Json(new { success = OTP });
        }

        public string sendmailWithOtp(string EmailId)
        {
            Random generator = new Random();
            String mailOTP = generator.Next(0, 999999).ToString("D6");
            try
            {
                //int clientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                //string userName = Session["UserName"].ToString();
                
                TempData["EmailOTP"] = mailOTP;
                ViewBag.EmailOTP = mailOTP;
                ViewBag.OTPcheck = true;
                ClientCustomize cc = new ClientCustomize();
                //DataTable dt = cc.GetClientUserEmailId(clientRowID, clientUserRowID);
                var user_EmailId = EmailId;
                
                string to = user_EmailId.ToString();
                string display = "BGV Securitas";
                string subj = "OTP For successfull login";
                string from = "bgv.request@securitas-india.com";
                string msg = "Your one time OTP for Login is = " + "<span style='color:red;'>" +
                mailOTP + "</span>" + "</br>" + ".Only valid for 2 min.";
                clsSendMail clsMail = new clsSendMail();
                var val = clsMail.SendMail(display, from, to, subj, msg);
                return mailOTP;
            }
            catch (Exception Ex)
            {
                return "";
            }           
        }

        public void sendLivenessLink()
        {
            try
            {
                DataBaseConnections DC = new DataBaseConnections();
                var Dt = DC.fractalLivenessCase();
                if (Dt.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        int PersonalRowId = Convert.ToInt32(Dt.Rows[i]["PersonalRowId"]);
                        string MobileNo = Convert.ToString(Dt.Rows[i]["MobileNo"]);                        
                        DataBaseConnections CC = new DataBaseConnections();
                        var tblDt = CC.GetPQPersonalWithMobileNo(PersonalRowId, MobileNo);
                        if (tblDt.Rows.Count > 0)
                        {
                            string link = "https://bgv.securitas-india.com/Account/FaceSignIn?Pid=" + Convert.ToString(clsCommonMethods.PEncrypt(PersonalRowId.ToString())) + "&type=Liveness";
                            SendAlertMassageForLiveness(link, tblDt);
                        }
                    }
                }
            }
            catch (Exception EX)
            {

            }
        }
        bool SendAlertMassageForLiveness(string LivenessLink, DataTable dt)
        {
            bool result = false;
            try
            {
                string MailSignature = string.Empty;
                string From = string.Empty;
                string body = string.Empty;
                string body1 = string.Empty;
                string MailSubject = "";
                string DisplayName = string.Empty;
                string Cc = string.Empty;
                clsSendMail mail;
                mail = new clsSendMail();
                string EmailID = "";
                string CandidateFullName = "";
                string ClientName = "";
                short ClientRowID = 0;
                int ClientUserRowID = 0;
                string spocEmail = "";
                string cSpocEmail = "";
                string mobileno = "";
                if (dt.Rows.Count > 0)
                {
                    ClientName = Convert.ToString(dt.Rows[0]["ClientName"]);
                    EmailID = Convert.ToString(dt.Rows[0]["EmailID"]);
                    CandidateFullName = Convert.ToString(dt.Rows[0]["CandidateFullName"]);
                    ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"]);
                }

                From = "bgv.request@securitas-india.com";
                DisplayName = "Securitas India";

                var personalData = "";
                if (personalData != null)
                {

                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/ReSendLinkforLiveness.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    string Subject = "Liveness Background Verification";
                    StringBuilder SignatureString = new StringBuilder();
                    ClientCustomize cc = new ClientCustomize();
                    DataTable MailDT = new DataTable();
                    MailDT = cc.GetSpoceNameAndEmailForMailSignature(Convert.ToInt32(ClientRowID));
                    if (MailDT.Rows.Count > 0)
                    {
                        foreach (DataRow dr in MailDT.Rows)
                        {
                            SignatureString.Append("<table style='width:100%;'><tr><td style='color:#031F30;font-weight:bold;'></td><td style='width: 92%;color:#FC273F;text-align:justify;'>Prateek.vashishtha@securitas-India.com,priyanjali.daga@securitas-india.com</td></tr></table>");
                        }
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


                    body = body.Replace("{CandidateName}", CandidateFullName);
                    body = body.Replace("{ClientName}", ClientName);
                    body = body.Replace("{LivenessLink}", LivenessLink);
                    body = body.Replace("{triangle}", triangle);
                    body = body.Replace("{securitas}", securitas);
                    body = body.Replace("{walsons}", walsons);
                    body = body.Replace("{BGVdotted}", BGVdotted);
                    body = body.Replace("{MailSignature}", SignatureString.ToString());

                    //add employeeid in muthoot insuff mail
                    string lblempid = string.Empty;
                    string empid = string.Empty;
                    string To = EmailID;

                    string ToMail = To.ToLower();
                    string ccmail = "onboarding@fractal.ai;priyanjali.daga@securitas-india.com;prateek.vashishtha@securitas-india.com";
                    var flag = mail.SendMail(DisplayName, From, ToMail, ccmail, Subject, body);
                    result = flag;
                }
                else
                {
                    TempData["ErrorMsg"] = "Candidate login details not found!";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        [HttpPost]
        public JsonResult ConvertHtmlStringToPdf(string jsonString = "")
        {
            htmlToPdf htmlToPdf = new htmlToPdf();
            try
            {
                String IronKey = System.Configuration.ConfigurationManager.AppSettings["IronKey"].ToString();
                var Renderer = new IronPdf.ChromePdfRenderer();
                IronPdf.License.LicenseKey = IronKey;
                // Render HTML to PDF
                var pdf = Renderer.RenderHtmlAsPdf(jsonString);
                // Convert to Base64 string (no SaveAs)
                htmlToPdf.base64Pdf = Convert.ToBase64String(pdf.BinaryData);

                // Return the Base64 string
                return Json(htmlToPdf, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return Json(htmlToPdf, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult OCRBGCForm(HttpPostedFileBase file, string RefNo)
        {
            ocrAPIStatus statusObj = new ocrAPIStatus();
            try
            {
                string filename = RefNo + "BGVForm.pdf";
                string StartupPath = Server.MapPath(ConfigurationManager.AppSettings["provideddocuments"].ToString());
                string FinalFileName = StartupPath + Path.GetFileName(filename);
                file.SaveAs(FinalFileName);
                PDFOCR OcrObj = new PDFOCR();
                int Status = OcrObj.GetDetailsfromPDF_OCR(FinalFileName, RefNo, filename);
                if (Status == 1)
                {
                    statusObj.StatusCode = "200";
                    statusObj.Remarks = "Success";
                }
                else
                {
                    statusObj.StatusCode = "201";
                    statusObj.Remarks = "Failed";
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                statusObj.StatusCode = "400";
                statusObj.Remarks = "Error found";
                return Json(statusObj, JsonRequestBehavior.AllowGet);
            }

            return Json(statusObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DigiRedirect_uri(string name, string dob, string gender, string eaadhaar, string state, string requestId)
        {
            string PersonalRowId = state;
            ViewBag.msg = "";
            try
            {
                PerfiosAPI aPI = new PerfiosAPI();
                ClientCustomize Dc = new ClientCustomize();
                DataBaseConnections DC = new DataBaseConnections();
                var Dt = Dc.GetPersonalInfoByTempPersonalRowID(Convert.ToInt32(state));
                if (Dt.Rows.Count > 0)
                {
                    int ClientRowID = Convert.ToInt32(Dt.Rows[0]["ClientRowID"]);
                }
                if (state != "")
                {
                    var APIPath = Server.MapPath("~\\APILogs\\Logfiles");
                    var model = aPI.GetPerfiosdigilockerdocuments(state, requestId, APIPath);
                    if (model != null)
                    {
                        if (model.statusCode == 101)
                        {
                            foreach (var item in model.result)
                            {
                                Thread.Sleep(9000);
                                var docDownload = aPI.GetPerfiosdigilockerdownload(state, requestId, item.uri, APIPath);
                                if (docDownload != null)
                                {
                                    if (docDownload.statusCode == 101)
                                    {
                                        string DocumentName = item.uri + "." + docDownload.result[0].parsedFile.data.issuedTo.photo.format;
                                        string res = "";
                                        StringBuilder StrBuilderObj = new StringBuilder();
                                        StrBuilderObj.Clear();
                                        try
                                        {
                                            string base64File = docDownload.result[0].parsedFile.data.issuedTo.photo.content;
                                            string url = ConfigurationManager.AppSettings["DocumentBaseURL"].ToString() + "Get-Base64-AsFile-Document/";
                                            string jsonData = "{\"DocumentType\":\"document\",\"DocumentName\":\"" + DocumentName + "\",\"DocumentValue\":\"" + base64File + "\"}";
                                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                            request.Method = "POST";
                                            request.ContentType = "application/json"; // Change to "application/x-www-form-urlencoded" if needed
                                            request.Accept = "application/json";
                                            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                                            {
                                                streamWriter.Write(jsonData);
                                                streamWriter.Flush();
                                            }
                                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                            {
                                                res = reader.ReadToEnd();
                                            }
                                            if (res == "Success")
                                            {
                                                var issuedTo = docDownload?.result?[0]?.parsedFile?.data?.issuedTo;
                                                string uid = Convert.ToString(issuedTo.uid);
                                                DC.InsertUpdateLivenessApiResponse(Convert.ToInt32(state), 890, "", "", "", "", uid);
                                                SaveTempPQVerifiedUploadDoc(Convert.ToInt32(state), 890, "Provided", "Candidate Image", "provideddocuments", "Candidate Image from Aadhaar", DocumentName);
                                                SaveAadhaarArtifact(docDownload, Convert.ToInt32(state), 890, DocumentName);
                                            }

                                        }
                                        catch (Exception Ex)
                                        {
                                            return RedirectToAction("RedirectAddlivenessInfoAsync", "Account", new { TempPid = state, msg = "Error found" });

                                        }
                                        return RedirectToAction("RedirectAddlivenessInfoAsync", "Account", new { TempPid = state, msg = "Aadhaar Verification Successfully Complete" });
                                    }
                                    else
                                    {
                                        return RedirectToAction("RedirectAddlivenessInfoAsync", "Account", new { TempPid = state, msg = "Aadhaar Not Download" });
                                    }
                                }
                                else
                                {
                                    return RedirectToAction("RedirectAddlivenessInfoAsync", "Account", new { TempPid = state, msg = "Aadhaar Not Download" });
                                }

                            }
                        }
                        else
                        {
                            return RedirectToAction("RedirectAddlivenessInfoAsync", "Account", new { TempPid = state, msg = "Documents not download" });
                        }
                    }
                    else
                    {
                        return RedirectToAction("RedirectAddlivenessInfoAsync", "Account", new { TempPid = state, msg = "Document not found please try again" });
                    }
                }
                else
                {
                    return RedirectToAction("RedirectAddlivenessInfoAsync", "Account", new { TempPid = state, msg = "State Id not found Please try again" });

                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = "";
                string msg = ex.Message.ToString();
                StrBuilderObj.Append("----------------------------API DigiLockerRedirect_uri-----------------");
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("Response:Exception" + msg);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "DigiLocker", "DigiRedirect_uri", PersonalRowId + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                return RedirectToAction("RedirectAddlivenessInfoAsync", "Account", new { TempPid = state, msg = "Aadhaar Not Download" });
            }

            return View();
        }
        public void SaveTempPQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", string remarks = "", string Filename = "")
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
                DocModel.FileName = Filename;
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
        [HttpGet]
        public ActionResult RedirectAddlivenessInfoAsync(string TempPid, string msg)
        {
            ClientCustomize ccn = new ClientCustomize();
            bool res = false;
            DataTable dtt = ccn.GetCandidateLoginDetails(Convert.ToInt32(TempPid));
            if (dtt.Rows.Count > 0)
            {
                string username = dtt.Rows[0]["UserID"].ToString();
                string password = dtt.Rows[0]["UPass"].ToString();
                CandidateLoginViewModel model = new CandidateLoginViewModel();
                string crpPWD = clsCommonMethods.PasswordDecrypt(password);
                string txtpassword = crpPWD.Trim();
                byte[] textBytes = Encoding.UTF8.GetBytes(txtpassword);
                string encrypted = Convert.ToBase64String(textBytes);
                model.CRPUserName = username;
                model.CRPPassWord = encrypted;
                res = CandidateLoginDirectLiveness(model);
                if (res == true)
                {
                    string Pid = Session["TempPersonalRowID"].ToString();
                    string sid = "890";
                    return RedirectToAction("AddlivenessInfo", "Home", new { area = "Candidate", Pid = clsCommonMethods.PEncrypt(Pid), Sid = sid, msg = msg });
                }
            }
            return RedirectToAction("CandidateLogin", "Account", new { area = "" });
        }

        public bool SaveAadhaarArtifact(digilockerDownloadRootobject model, int Pid, int Sid,string ImageName)
        {
            bool status = false;
            try
            {
                // Extract Aadhaar details safely
                var issuedTo = model?.result?[0]?.parsedFile?.data?.issuedTo;
                if (issuedTo == null) return false;

                string name = Convert.ToString(issuedTo.name);
                string dob = Convert.ToString(issuedTo.dob);
                string gender = Convert.ToString(issuedTo.gender);
                string uid = Convert.ToString(issuedTo.uid);

                var address = issuedTo.address;
                string district = Convert.ToString(address?.district);
                string vtc = Convert.ToString(address?.vtc);
                string pin = Convert.ToString(address?.pin);
                string state = Convert.ToString(address?.state);
                string country = Convert.ToString(address?.country);

                string Address = $"{vtc}, {district}, {state}, {country}, {pin}";

                // Load HTML template
                string body;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/DigilockerAdhar.html")))
                {
                    body = reader.ReadToEnd();
                }

                // Replace placeholders
                body = body.Replace("{Name}", name)
                           .Replace("{DOB}", dob)
                           .Replace("{Adhar_Number}", uid)
                           .Replace("{Address}", Address)
                           .Replace("{Gender}", gender)
                           .Replace("{AdharImage}", ImageName);

                // Save PDF artifact
                SaveTemp_PQVerifiedUploadDoc(Pid, (short)Sid, "Verified", "Aadhaar Artifact", "provideddocuments", "Candidate Aadhaar Details", body);

                status = true; // mark success
            }
            catch (Exception ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), ex.Message, ex.StackTrace);
            }
            return status;
        }


        public void SaveTemp_PQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", string remarks = "", string UploadDoc = "")
        {
            try
            {
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
                string FileName = (PersonalRowID.ToString() + " " + DocumentType)
                                  .Replace(" ", "_")
                                  .Replace("-", "_") + "_" + Timestamp;

                if (FileName.Contains("24/7CPL_"))
                {
                    FileName = FileName.Replace("/", "-");
                }

                string proImg = FileName + ".pdf";

                string FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()];
                string IronKey = ConfigurationManager.AppSettings["IronKey"];

                IronPdf.License.LicenseKey = IronKey;
                var Renderer = new IronPdf.ChromePdfRenderer();
                var pdf = Renderer.RenderHtmlAsPdf(UploadDoc);

                pdf.SaveAs(Server.MapPath(FldPathDoc + proImg));

                AddTempPQVerifiedUploadDocViewModel DocModel = new AddTempPQVerifiedUploadDocViewModel
                {
                    PersonalRowID = PersonalRowID,
                    SubCheckRowID = SubCheckRowID,
                    DocumentUploadFrom = DocumentUploadFrom,
                    DocumentType = DocumentType,
                    FileName = Path.GetFileName(proImg),
                    FilePath = FldPathDoc,
                    UploadDate = DateTime.Now,
                    StatusDocxConvert = 0,
                    StatusPDF2DOCXConvert = 0,
                    Remarks = remarks,
                    PDF_FileName = "",
                    Status = 1
                };

                repoPQVerifiedUploadDoc.AddTempPQVerifiedUploadDoc(DocModel);
                repoPQVerifiedUploadDoc.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), ex.Message, ex.StackTrace);
            }
        }
    }

}
