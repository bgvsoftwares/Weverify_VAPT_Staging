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

namespace WebAppBGV.Controllers
{
    public class ReportDownloadController : Controller
    {
        IClientDashboardRepository repoDashboardClient;
        public ReportDownloadController(IClientDashboardRepository _repoDashboardClient)
        {
            repoDashboardClient = _repoDashboardClient;
        }
        // GET: ReportDownload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetReportInterimLink(string ImageName, string cid)
        {

            try
            {
                string str = clsCommonMethods.PDecrypt(cid);
                string imgname = (clsCommonMethods.PDecrypt(ImageName));
                string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
                FldPathDoc = Server.MapPath(FldPathDoc) + imgname + ".pdf";
                byte[] fileBytes = null;
                ClientCustomize cc = new ClientCustomize();
                DataTable ddt = cc.getAutoExistCompRefNo(imgname);
                if (ddt.Rows.Count == 0)
                    FldPathDoc = FldPathDoc.Replace("wwwroot2", "wwwroot");

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

        [HttpGet]
        public ActionResult GetSpecialLink(string Refid = "")
        {
            if (Refid != "")
            {
                ClientCustomize cc = new ClientCustomize();
                Refid = clsCommonMethods.PDecrypt(Refid);
                DataTable dt = cc.GetReferenceDetailforYTR(Refid);
                if (dt.Rows.Count > 0)
                {
                    string clientrowid = dt.Rows[0]["ClientRowID"].ToString();
                    if (clientrowid == "860")
                    {
                        return View();
                    }


                }

            }
            return RedirectToAction("ClientTimeoutRedirect", "Account", new { area = "" });
        }

    }
}