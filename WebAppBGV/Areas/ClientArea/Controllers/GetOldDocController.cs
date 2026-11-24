using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BAL;
using BAL.HRRepository;
using WebAppBGV.Security;

namespace WebAppBGV.Areas.ClientArea.Controllers
{
    [CustomAuthorizeClient]
    public class GetOldDocController : Controller
    {
        #region 46 server GetDocumentDownloadAPI
        [HttpGet]
        public ActionResult GetDocReportAPI(string Uname)
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetDocReportAPI(FormCollection frm)
        {
            try
            {
                string reps = "";
                GetOldDocAPI objacc = new GetOldDocAPI();
                string CompanyRefNo = Convert.ToString(frm["CompanyRefNo"]);
                string base64values = "";
                string FileName = "";
                if (CompanyRefNo != "")
                {
                    if (CompanyRefNo.Contains(","))
                    {
                        string[] multiArray = CompanyRefNo.Split(new Char[] { ',', ':' });
                        foreach (var item in multiArray)
                        {
                            string PathDoc = "";

                            string pattern = @"\b(20\d{2})\b";   // finds years like 2000–2099

                            PathDoc = Regex.Match(item, pattern).Value;

                            DataTable dt = objacc.Get_ReportHistories(item);
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    base64values = "";
                                    if (Convert.ToString(dr["FileName"]).ToString() != "" && Convert.ToString(dr["Doctype"]).ToString() != "")
                                    {
                                        base64values = GetOldDocAPI.ReportDownloadDynamic(Convert.ToString(dr["FileName"]).ToString(), PathDoc);
                                        if (base64values.Contains("Error:-"))
                                        {
                                            base64values = "";
                                        }
                                        if (base64values != "")
                                        {
                                            //Upload document on 46 server
                                            reps = GetOldDocAPI.DownloadCandidateDocReport2(Convert.ToString(dr["FileName"]).ToString(), Convert.ToString(dr["Doctype"]), base64values);
                                        }
                                        if (base64values == "")
                                        {
                                            FileName += Convert.ToString(dr["FileName"]).ToString();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ViewBag.Error = "Record not found !!!" + reps;
                            }
                        }
                    }
                    else
                    {
                        DataTable dt = objacc.Get_ReportHistories(CompanyRefNo);
                        if (dt.Rows.Count > 0)
                        {
                            string PathDoc = "";

                            string pattern = @"\b(20\d{2})\b";   // finds years like 2000–2099

                            PathDoc = Regex.Match(CompanyRefNo, pattern).Value;
                            foreach (DataRow dr in dt.Rows)
                            {
                                base64values = "";
                                if (Convert.ToString(dr["FileName"]).ToString() != "" && Convert.ToString(dr["Doctype"]).ToString() != "")
                                {
                                    base64values = GetOldDocAPI.ReportDownloadDynamic(Convert.ToString(dr["FileName"]).ToString(), PathDoc);
                                    if (base64values.Contains("Error:-"))
                                    {
                                        base64values = "";
                                    }
                                    if (base64values != "")
                                    {
                                        //Upload document on 46 server
                                        reps = GetOldDocAPI.DownloadCandidateDocReport2(Convert.ToString(dr["FileName"]).ToString(), Convert.ToString(dr["Doctype"]), base64values);
                                    }
                                    if (base64values == "")
                                    {
                                        FileName += Convert.ToString(dr["FileName"]).ToString();
                                    }
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Record not found !!!";
                        }
                    }
                }
                else
                {
                    ViewBag.Error = "Please fill mandatory fields!!!";
                }
                if (reps != "")
                {
                    if (FileName != "")
                    {
                        ViewBag.Error = reps + " Not found no of Doc File Name:- " + FileName;
                    }
                    else
                    {
                        ViewBag.Error = reps;
                    }
                }
                else
                {
                    ViewBag.Error = "Report not found !!!" + " Not found no of Doc File Name:- " + FileName;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
            }
            return View();
        }
        #endregion
        #region 55 server GetDocumentDownloadAPI Old
        [HttpGet]
        public ActionResult GetOldDocumentDownloadAPI(string Uname)
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetOldDocumentDownloadAPI(FormCollection frm)
        {
            try
            {
                string reps = "";
                string QType = "";
                GetOldDocAPI objacc = new GetOldDocAPI();
                string CompanyRefNo = Convert.ToString(frm["CompanyRefNo"]);
                string base64values = "";
                string datfilestr = "";
                if (CompanyRefNo != "")
                {
                    if (CompanyRefNo.Contains(","))
                    {
                        string[] multiArray = CompanyRefNo.Split(new Char[] { ',', ':' });
                        foreach (var item in multiArray)
                        {
                            if (item.Contains("-"))
                            {
                                string[] multiArr = item.Split(new Char[] { '-' });
                                string strvalues = multiArr[2].ToString();
                                if (strvalues == "2022")
                                {
                                    QType = "document2022";
                                }
                                else if (strvalues == "2023")
                                {
                                    QType = "document2023";
                                }
                                else if (strvalues == "2024")
                                {
                                    QType = "document2024";
                                }
                            }
                            if (QType == "")
                            {
                                string[] multiArr = CompanyRefNo.Split(new Char[] { '-' });
                                string strvalues = multiArr[4].ToString();
                                if (strvalues == "2022")
                                {
                                    QType = "document2022";
                                }
                                else if (strvalues == "2023")
                                {
                                    QType = "document2023";
                                }
                                else if (strvalues == "2024")
                                {
                                    QType = "document2024";
                                }
                            }
                            else
                            {
                                ViewBag.Error = "CompanyRefNo not right format !!!";
                            }

                            if (item != "")
                            {
                                DataTable dt = objacc.Get_DocListOld(item);
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        base64values = "";
                                        if (Convert.ToString(dr["FileName"]).ToString() != "" && Convert.ToString(dr["Doctype"]).ToString() != "")
                                        {
                                            if (QType != "")
                                            {
                                                base64values = GetOldDocAPI.DocumentDownloadOld(Convert.ToString(dr["FileName"]).ToString(), QType);
                                            }
                                            if (base64values.Contains("Error:-") || base64values == "")
                                            {
                                                base64values = GetOldDocAPI.DocumentDownloadDynamic(Convert.ToString(dr["FileName"]).ToString());
                                            }
                                            if (base64values.Contains("Error:-"))
                                            {
                                                base64values = "";
                                                datfilestr += Convert.ToString(dr["FileName"]).ToString();
                                            }
                                            if (base64values != "")
                                            {
                                                //Upload document on 46 server
                                                reps = GetOldDocAPI.DownloadCandidateDocReport2(Convert.ToString(dr["FileName"]).ToString(), Convert.ToString(dr["Doctype"]), base64values);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    ViewBag.Error = "Record not found !!!";
                                }
                            }
                        }
                    }
                    else
                    {
                        string FileName = "";
                        DataTable dt = objacc.Get_DocListOld(CompanyRefNo);
                        if (CompanyRefNo.Contains("-"))
                        {
                            string[] multiArr = CompanyRefNo.Split(new Char[] { '-' });
                            string strvalues = multiArr[3].ToString();
                            if (strvalues == "2022")
                            {
                                QType = "document2022";
                            }
                            else if (strvalues == "2023")
                            {
                                QType = "document2023";
                            }
                            else if (strvalues == "2024")
                            {
                                QType = "document2024";
                            }
                        }
                        if (QType == "")
                        {
                            string[] multiArr = CompanyRefNo.Split(new Char[] { '-' });
                            string strvalues = multiArr[4].ToString();
                            if (strvalues == "2022")
                            {
                                QType = "document2022";
                            }
                            else if (strvalues == "2023")
                            {
                                QType = "document2023";
                            }
                            else if (strvalues == "2024")
                            {
                                QType = "document2024";
                            }
                        }
                        else
                        {
                            ViewBag.Error = "CompanyRefNo not right format !!!";
                        }

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                base64values = "";
                                if (Convert.ToString(dr["FileName"]).ToString() != "" && Convert.ToString(dr["Doctype"]).ToString() != "")
                                {
                                    if (QType != "")
                                    {
                                        base64values = GetOldDocAPI.DocumentDownloadOld(Convert.ToString(dr["FileName"]).ToString(), QType);
                                    }
                                    if (base64values.Contains("Error:-") || base64values == "")
                                    {
                                        base64values = GetOldDocAPI.DocumentDownloadDynamic(Convert.ToString(dr["FileName"]).ToString());
                                    }
                                    if (base64values.Contains("Error:-"))
                                    {
                                        base64values = "";
                                    }
                                    if (base64values != "")
                                    {
                                        //Upload document on 46 server
                                        reps = GetOldDocAPI.DownloadCandidateDocReport2(Convert.ToString(dr["FileName"]).ToString(), Convert.ToString(dr["Doctype"]), base64values);
                                    }
                                    if (base64values == "")
                                    {
                                        datfilestr += Convert.ToString(dr["FileName"]).ToString();
                                    }
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Record not found !!!";
                        }
                    }
                }
                else
                {
                    ViewBag.Error = "Please fill mandatory fields!!!";
                }
                if (reps != "")
                {
                    ViewBag.Error = reps;
                }
                else
                {
                    ViewBag.Error = "Document not found !!!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
            }
            return View();
        }
        #endregion
    }
}