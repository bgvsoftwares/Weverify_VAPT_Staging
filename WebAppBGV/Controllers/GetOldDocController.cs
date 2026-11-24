using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using BAL;
using BAL.HRRepository;
using DAL;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebAppBGV.Security;
using static BAL.ModelLTI;
using BAL.Models;

namespace WebAppBGV.Controllers
{
    [CustomAuthorize]
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
                                        if (base64values == "")
                                        {
                                            if (PathDoc == "2023")
                                            {
                                                PathDoc = "2024";
                                            }
                                            if (PathDoc == "2024")
                                            {
                                                PathDoc = "2023";
                                            }
                                            base64values = GetOldDocAPI.ReportDownloadDynamic(Convert.ToString(dr["FileName"]).ToString(), PathDoc);
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
                                    if (base64values == "")
                                    {
                                        if (PathDoc == "2023")
                                        {
                                            PathDoc = "2024";
                                        }
                                        if (PathDoc == "2024")
                                        {
                                            PathDoc = "2023";
                                        }
                                        base64values = GetOldDocAPI.ReportDownloadDynamic(Convert.ToString(dr["FileName"]).ToString(), PathDoc);
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
        #region Get Client Pushed Details
        [ValidateInput(false)]
        public ActionResult CheckCaseDetails()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
        #region Case details LTI************************
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CheckCaseDetails(FormCollection formCollection)
        {
            CheckCaseDetails ObjCheckCaseDetails = new CheckCaseDetails();
            try
            {
                string CompanyRefNo = (formCollection["CompanyRefNo"].ToString()).Trim();
                //string RequesttypeID = (formCollection["RequesttypeID"].ToString()).Trim();
                int ClientRowID = 0;
                if (CompanyRefNo != "")
                {
                    DataTable dts = new AccountRepository().GetPersonalRowId(CompanyRefNo);
                    CandidateList candidate = new CandidateList();
                    if (dts.Rows.Count > 0)
                    {
                        candidate.RequestID = "";
                        candidate.PSNO = "";
                        candidate.CandidateID = "";
                        if (dts.Rows[0]["ClientRefID"].ToString() != "")
                        {
                            candidate.RequestID = dts.Rows[0]["ClientRefID"].ToString();
                        }
                        if (dts.Rows[0]["EmployeeID"].ToString() != "")
                        {
                            candidate.CandidateID = dts.Rows[0]["EmployeeID"].ToString();
                        }
                        if (dts.Rows[0]["CandidateCode"].ToString() != "")
                        {
                            candidate.PSNO = dts.Rows[0]["CandidateCode"].ToString();
                        }
                        if (dts.Rows[0]["ClientRowID"].ToString() != "")
                        {
                            ClientRowID = Convert.ToInt32(dts.Rows[0]["ClientRowID"].ToString());
                        }
                    }
                    else
                    {
                        ViewBag.Error = "CompanyRefNo not found !!!";
                        return View(ObjCheckCaseDetails);
                    }
                    //AccountRepositoryBGV.CaseListDetails(candidate.RequestID, 990);
                    ObjCheckCaseDetails.ShowallCheckCaseDetails = FileSaveDownloadAPI.CaseListDetails(candidate.RequestID, ClientRowID);
                    
                }
                else
                {
                    ViewBag.Error = "Please fill mandatory fields";
                }
            }
            catch (Exception ex)
            {

            }

            return View(ObjCheckCaseDetails);
        }
        #endregion *****************************************
        #endregion
        #region ******************************** Doc Download LTI Meet ************************************
        [ValidateInput(false)]
        public ActionResult IndexDoc()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult IndexDoc(FormCollection formCollection)
        {
            //string username = "Walson@ltimindtree.com";
            //string password = "WalsonLTIMBGV@1234#";
            //ModelLTI.Token token = GetAccessToken(username, password);
            ViewBag.Error = "";
            ViewBag.Error2 = "";
            int clientRowId = 990;
            string base64 = "";
            try
            {
                string CompanyRefNo = (formCollection["CompanyRefNo"].ToString()).Trim();
                if (CompanyRefNo.Contains("LTI") && CompanyRefNo != "")
                {
                    base64 = FileSaveDownloadAPI.GetCandidateDetail_Documents2(CompanyRefNo, clientRowId);
                    if (base64.Contains("LTIMindtree Documents"))
                    {
                        ViewBag.Error = base64;
                    }
                    else
                    {
                        string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Documents/LTIDoc/") + CompanyRefNo + "/";
                        ViewBag.Error = "Your File Download Successfully";
                        ViewBag.Error2 = CompanyRefNo;
                        Thread.Sleep(5000);
                        string[] sDirectoryInfo = Directory.GetFiles(filePath);
                        //string[] sDirectoryInfo = Directory.GetFiles("~/documents/", ReferenceId + "*.*");
                        string[] finStr = new string[sDirectoryInfo.Length];
                        for (int i = 0; i < sDirectoryInfo.Length; i++)
                        {
                            finStr[i] = sDirectoryInfo[i].Replace(filePath, "");
                        }
                        string result = Newtonsoft.Json.JsonConvert.SerializeObject(finStr);
                        string str = result.Replace("[", "");
                        str = str.Replace("]", "");
                        str = str.Replace("\"", "");
                        string[] str1 = str.Split(',');
                        ViewBag.Docs = str1;
                    }
                }
                else
                {
                    ViewBag.Error = "Please enter the Company Ref id";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = base64;
            }
            return View();
        }
        #endregion ***************************** Doc Download LTI Mee**************************************
    }
}