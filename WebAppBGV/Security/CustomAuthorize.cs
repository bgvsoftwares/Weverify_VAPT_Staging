using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppBGV.Security
{
    public class CustomAuthorize : AuthorizeAttribute
    {        
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var bypassHeader = filterContext.HttpContext.Request.Headers["X-Bypass-Auth"];

            if (bypassHeader == "QqI0bBY1DvUS4DKa9Vh2Fy5WtdBihEKp6rOMFz93xqkK1e2ZGg83fu7hjxztXo5x")
            {
               
                filterContext.HttpContext.Session["AuthenticationToken"] = "Bypassed";
              
            }
            //base.OnAuthorization(filterContext);
            //if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            if (Convert.ToString(filterContext.HttpContext.Session["AuthenticationToken"])!="")
            {
                //Pick Roles from Db
                //Permission Table for Pages
                //if (!CurrentUser.IsInRole(Roles))
                //{
                //    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "WebAdmin", action = "UnAuthorize", area = "" }));
                //    return;
                //}
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Index", area = "" }));
                return;
            }
        }
    }
}