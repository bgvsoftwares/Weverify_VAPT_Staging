using BAL;
using BAL.ClientRepository;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;

namespace WebAppBGV.Controllers
{

    public class ErrorController : Controller
    {
        public ActionResult General()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
        public ActionResult InternalError()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}
