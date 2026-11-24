using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BAL;
using System.Data;
using Org.BouncyCastle.Asn1.Ocsp;

namespace WebAppBGV.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ValidateSessionFilterSpokLoginInternal : ActionFilterAttribute
    {
        ClientCustomize objClientCustomize = new ClientCustomize();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            int CurrentWebUserRowid = Convert.ToInt16(filterContext.HttpContext.Session["WebUserRowid"]);
            DataTable objDataTable = objClientCustomize.GetSessionTokenbySpokWebUserRowID(CurrentWebUserRowid);
            string DBStoredSessionId = string.Empty;
            if (objDataTable.Rows.Count > 0)
                DBStoredSessionId = Convert.ToString(objDataTable.Rows[0]["SessionToken"]);

            var CurrentSessionId = Convert.ToString(filterContext.HttpContext.Session["SessionTokenSpok"]);
            string IsConCurrentSessionFunctionalityStop = System.Configuration.ConfigurationManager.AppSettings["IsConCurrentSessionFunctionalityStop"].ToString();
            if (DBStoredSessionId != CurrentSessionId && IsConCurrentSessionFunctionalityStop == "No")           
            {
                // For round-trip posts, we're forcing a redirect to Home/TimeoutRedirect/, which
                // simply displays a temporary 5 second notification that they have timed out, and
                // will, in turn, redirect to the logon page.

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller", "Account" },
                        { "Action", "TimeoutRedirect" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}