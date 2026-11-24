using BAL.HRRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ViewModels.HRViewModels;
using WebAppBGV.CommonMethods;
using BAL;
using System.Net;
using System.Web.SessionState;
//hello test the TFS server 
namespace WebAppBGV
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) == false)
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }

            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            MvcHandler.DisableMvcResponseHeader = true;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            /*Start Handel custom error 22May2025*/

            Exception exception = Server.GetLastError();
            Response.Clear();
            HttpException httpException = exception as HttpException;
            clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), httpException.Message, httpException.StackTrace);
            string message = "";
            if (httpException != null)
            {
                if (httpException.GetHttpCode() == 404)
                {
                    Server.ClearError();
                    message = "path not found";
                    if (Context != null && Context.Handler is IRequiresSessionState && Session != null)
                    {
                        Session["ApplicationError"] = message;
                    }
                    Response.Redirect("/Account/ApplicationError");
                }
                else if (httpException.GetHttpCode() == 500)
                {
                    Server.ClearError();
                    message = "path not found";
                    if (Context != null && Context.Handler is IRequiresSessionState && Session != null)
                    {
                        Session["ApplicationError"] = message;
                    }
                    Response.Redirect("/Account/ApplicationError");
                }
                else
                {
                    Server.ClearError();
                    message = "path not found";
                    if (Context != null && Context.Handler is IRequiresSessionState && Session != null)
                    {
                        Session["ApplicationError"] = message;
                    }
                    Response.Redirect("/Account/ApplicationError");
                }

            }
            else if (exception != null)
            {
                Server.ClearError();
                if (Context != null && Context.Handler is IRequiresSessionState && Session != null)
                {
                    Session["ApplicationError"] = exception.Message;
                }
                Response.Redirect("/Account/ApplicationError");
            }

            /*End Handel custom error 22May2025*/

        }



        protected void Session_Start(object sender, EventArgs e)
        {
            try
            {
                if (Request.IsSecureConnection == true)
                {
                    Response.Cookies["ASP.NET_SessionId"].Secure = true;
                }
            }
            catch { }

            //if (Session.SessionID != "")
            //{
            //    string CookieHeaders = HttpContext.Current.Request.Headers["Cookie"];
            //    if ((null != CookieHeaders) && (CookieHeaders.IndexOf("ASP.NET_SessionId") >= 0))
            //    {
            //        FormsAuthentication.SignOut();
            //        // Clear authentication cookie.
            //        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            //        cookie.Expires = DateTime.Now.AddYears(-1);
            //        Response.Cookies.Add(cookie);
            //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //        Response.Cache.SetNoServerCaching();
            //        Response.Cache.SetNoStore();
            //    }
            //    else
            //    {
            //        // It is a new visitor, session was not created before
            //        string sessionId = System.Web.HttpContext.Current.Session.SessionID;
            //        Session["sessionId"] = sessionId.ToString();
            //    }

            //}
            ////Random generator = new Random();
            ////String r = generator.Next(0, 999999).ToString("D6");
            ////ClientCustomize cc = new ClientCustomize();
            ////cc.insertPQUserLoginEncryptionKey(r, sessionId);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            //cookie.Expires = DateTime.Now.AddYears(-1);
            //this.Session.Clear();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            //if (FormsAuthentication.CookiesSupported == true)
            //{
            //    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            //    if (authCookie != null)
            //    {
            //        try
            //        {
            //            string userName = FormsAuthentication.Decrypt(authCookie.Value).Name;
            //            string roles = string.Empty;

            //            //using (userDbEntities entities = new userDbEntities())
            //            //{
            //            //    User user = entities.Users.SingleOrDefault(u => u.username == username);

            //            //    roles = user.Roles;
            //            //}
            //            //let us extract the roles from our own custom cookie


            //            //Let us set the Pricipal with our user specific details
            //            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
            //              new System.Security.Principal.GenericIdentity(userName, "Forms"), roles.Split(';'));


            //            #region For Customer Authentication

            //            //FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //            //AdminUserViewModel serializeModel = JsonConvert.DeserializeObject<AdminUserViewModel>(authTicket.UserData);

            //            //CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
            //            //newUser.UserId = serializeModel.UserId;
            //            //newUser.UserName = serializeModel.UserName;
            //            //newUser.Name = serializeModel.Name;
            //            //newUser.Roles = serializeModel.Roles;

            //            //HttpContext.Current.User = newUser; 

            //            #endregion

            //        }
            //        catch (Exception Ex)
            //        {
            //            clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            //        }
            //    }
            //}
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //if (Request.IsSecureConnection == true && HttpContext.Current.Request.Url.Scheme == "https")
            //{
            //    Request.Cookies["ASP.NET_SessionID"].Secure = true;
            //    if (Request.Cookies.Count > 0)
            //    {
            //        foreach (string s in Request.Cookies.AllKeys)
            //        {
            //            Request.Cookies[s].Secure = true;
            //        }
            //    }

            //    Response.Cookies["ASP.NET_SessionID"].Secure = true;
            //    if (Response.Cookies.Count > 0)
            //    {
            //        foreach (string s in Response.Cookies.AllKeys)
            //        {
            //            Response.Cookies[s].Secure = true;
            //        }
            //    }
            //}
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");           //Remove Server Header   
            Response.Headers.Remove("X-AspNet-Version"); //Remove X-AspNet-Version Header
            Response.Headers.Remove("X-AspNetMvc-Version"); //04052021
            Response.Headers.Remove("X-Powered-By"); // ASP.Net
        
        //if (HttpContext.Current != null)
        //{
        //    HttpContext.Current.Response.Headers.Remove("Server");
        //}
    }

        //protected void application_AcquireRequestState(object sender, EventArgs e)
        //{
        //    string _sessionIPAddress = string.Empty;
        //    if (HttpContext.Current.Session != null)
        //    {
        //        string _encryptedString = Convert.ToString(Session["encryptedSession"]);
        //        byte[] _encodedAsBytes = System.Convert.FromBase64String(_encryptedString);
        //        string _decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(_encodedAsBytes);
        //        char[] _separator = new char[] { '^' };

        //        if (_decryptedString != string.Empty && _decryptedString != "" && _decryptedString != null)
        //        {
        //            string[] _splitString = _decryptedString.Split(_separator);
        //            if (_splitString.Count() > 0)
        //            {
        //                if (_splitString[2].Count() > 0)
        //                {
        //                    string[] _userBrowserInfo = _splitString[2].Split('~');
        //                    if (_userBrowserInfo.Count() > 0)
        //                    {
        //                        _sessionIPAddress = _userBrowserInfo[1];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    string _currentUserIPAddress;
        //    if (string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
        //    {
        //        _currentUserIPAddress = Request.ServerVariables["REMOTE_ADDR"];
        //    }
        //    else
        //    {
        //        _currentUserIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        //    }
        //    System.Net.IPAddress result;
        //    if (!System.Net.IPAddress.TryParse(_currentUserIPAddress, out result))
        //    {
        //        result = System.Net.IPAddress.None;
        //    }
        //    if (_sessionIPAddress != "" && _sessionIPAddress != string.Empty)
        //    {
        //        if (_sessionIPAddress != _currentUserIPAddress)
        //        {
        //            Session.RemoveAll();
        //            Session.Clear();
        //            Session.Abandon();
        //            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddSeconds(-30);
        //            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
        //           // Response.Redirect("Account");
        //        }
        //    }

        //}

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
            HttpContext.Current.Response.Headers.Set("X-Powered-By", "Hidden");
        }

    }
}
