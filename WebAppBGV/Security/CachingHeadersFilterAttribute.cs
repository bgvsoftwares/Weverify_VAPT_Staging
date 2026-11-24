using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppBGV.Security
{
    public class CachingHeadersFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.AppendCacheExtension("no-store, must-revalidate");
            response.AppendHeader("Pragma", "no-cache");
            response.AppendHeader("Expires", "0");
            base.OnResultExecuted(filterContext);
        }
    }
}