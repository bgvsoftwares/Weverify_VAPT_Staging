using BAL;
using BAL.ClientRepository;
using BAL.HRRepository;
using BAL.ProvidedInfoRepository;
using BAL.VerificationRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ClientViewModel;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;
using System.Data;
using System.Net.Mime;
using System.Web.UI.WebControls;
using BAL.ReportQCRepository;
using ViewModels.ReportQCViewModel;
using ViewModels;
using System.Text;
using ClosedXML.Excel;
using ViewModels.TempViewModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Image = System.Drawing.Image;
using System.IO.Compression;
using WebAppBGV.Areas.ClientArea.Models;
using DAL;
using System.Text.RegularExpressions;
using System.Dynamic;
using DocumentFormat.OpenXml.EMMA;
using System.Data.Entity.Core.Metadata.Edm;
using System.Windows.Interop;
using System.Data.OleDb;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Diagnostics;
using System.Net;
using System.Threading;
using ViewModels.CandidateViewModel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using ViewModels.refinitiv;
using System.Data.SqlClient;
using System.Web.Helpers;
using Unity.Utility;
using Org.BouncyCastle.Asn1.X509.SigI;
using ViewModels.HRViewModels;
using iTextSharp.text.pdf.security;
using System.Net.Mail;
using System.Xml;


namespace WebAppBGV.Controllers
{
    [CustomAuthorize]
    public class SalesController : Controller
    {
        #region ******  Repository Objects ******
        ClientCustomize cc;
        IClientServicingRepository repoDashboard;
        IDropDownMasterRepository repoDDL;
        IPQVerifiedUploadDocRepository repoPQVerifiedUploadDoc;
        IClientCheckRepository repoClientCheck;
        IClientSubGroupRepository repoCliSubGroup;
        IVerificationInfoRepository repoVerificationInfo;
        IPQPersonalRepository repoPQPersonal;
        IAddressInfoRepository repoAddressInfo;
        IEmploymentInfoRepository repoEmploymentInfo;
        IEducationInfoRepository repoEducationInfo;
        IPQClientHolidayRepository repoPQClientHoliday;
        IPQClientCandiBulkUploadRepository repoPQClientCandiBulkUpload;
        IAbbreviationRepository repoAbbreviation;
        IPQClientRepository repoPQClient;
        ITeamMemberRepository repoTeamMember;
        IClientPackageRepository repoClientPackage;
        ICriminalInfoRepository repoCriminalInfo;
        INationalIdentityInfoRepository repoNationalIdentInfo;
        IReferenceInfoRepository repoReferenceInfo;
        ISubCheckFamilyRepository repoSubCheck;
        ISpecialCheckInfoRepository repoSplCheckInfo;
        IReportQCRepository repoReportQC;
        IClientDashboardRepository repoDashboardClient;
        IClientLoginRepository repoLogin;
        IPQVerifiedUploadCaseDocRepository repoPQVerifiedUploadCaseDoc;
        IPQCandidateCheckRepository repoPQCandidateCheckRepository;

        public SalesController(
           IClientServicingRepository _repoDashboard
            , IDropDownMasterRepository _repoDDL
            , IPQVerifiedUploadDocRepository _repoPQVerifiedUploadDoc
            , IVerificationInfoRepository _repoVerificationInfo
            , IPQPersonalRepository _repoPQPersonal
            , ITeamMemberRepository _repoTeamMember
            , IClientCheckRepository _repoClientCheck
            , IAddressInfoRepository _repoAddressInfo
            , IPQClientHolidayRepository _repoPQClientHoliday
            , IEmploymentInfoRepository _repoEmploymentInfo
            , IEducationInfoRepository _repoEducationInfo
            , ICriminalInfoRepository _repoCriminalInfo
                , IClientSubGroupRepository _repoCliSubGroup
            , INationalIdentityInfoRepository _repoNationalIdentInfo
            , IReferenceInfoRepository _repoReferenceInfo
              , IClientPackageRepository _repoClientPackage
            , ISubCheckFamilyRepository _repoSubCheck
            , ISpecialCheckInfoRepository _repoSplCheckInfo
            , IReportQCRepository _repoReportQC
             , IPQClientCandiBulkUploadRepository _repoPQClientCandiBulkUpload
            , IPQClientRepository _repoPQClient
            , IAbbreviationRepository _repoAbbreviation
             , IClientDashboardRepository _repoDashboardClient
            , IClientLoginRepository _repoLogin
            , IPQVerifiedUploadCaseDocRepository _repoPQVerifiedUploadCaseDoc
            , IPQCandidateCheckRepository _repoPQCandidateCheckRepository
            )
        {
            repoDashboard = _repoDashboard;
            repoCliSubGroup = _repoCliSubGroup;
            repoClientCheck = _repoClientCheck;
            repoDDL = _repoDDL;
            repoPQVerifiedUploadDoc = _repoPQVerifiedUploadDoc;
            repoVerificationInfo = _repoVerificationInfo;
            repoPQPersonal = _repoPQPersonal;
            repoAddressInfo = _repoAddressInfo;
            repoClientPackage = _repoClientPackage;
            repoTeamMember = _repoTeamMember;
            repoEmploymentInfo = _repoEmploymentInfo;
            repoEducationInfo = _repoEducationInfo;
            repoCriminalInfo = _repoCriminalInfo;
            repoPQClientHoliday = _repoPQClientHoliday;
            repoNationalIdentInfo = _repoNationalIdentInfo;
            repoReferenceInfo = _repoReferenceInfo;
            repoPQClientCandiBulkUpload = _repoPQClientCandiBulkUpload;
            repoAbbreviation = _repoAbbreviation;
            repoPQClient = _repoPQClient;
            repoSubCheck = _repoSubCheck;
            repoSplCheckInfo = _repoSplCheckInfo;
            repoReportQC = _repoReportQC;
            repoDashboardClient = _repoDashboardClient;
            repoLogin = _repoLogin;
            repoPQVerifiedUploadCaseDoc = _repoPQVerifiedUploadCaseDoc;
            repoPQCandidateCheckRepository = _repoPQCandidateCheckRepository;
        }

        #endregion

        // GET: ClientServicing
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult Index()
        {
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                //BindClientListByLogInName(SpocEmail);
                BindClientListBaseOnLoginScop(teamMemerRowId);
                //Department Manager
                //bool flagMgr = false;
                if (Designation == "Manager-Sales")
                {
                    //flagMgr = true;
                    CreateNavigationforInsuffCaseForSpoc(0);
                    ViewBag.Dashboard = "Sales Dashboard!";
                }

                //if (flagMgr)
                //{
                //    ViewBag.ManagerDashboard = repoDashboard.GetManagerClientServicingDashBoardInfo(teamMemerRowId, "Manager-Add");
                //}
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View();
        }

       
    
       
     
        [NonAction]
        private DateTime GetDeliveryDate(DateTime oDate, byte TAT, PQClientCodeHolidaysViewModel client)
        {
            try
            {
                DateTime ddate = oDate;
                DateTime fdlDate = oDate;
                ddate = ddate.AddDays(TAT);
                int intDate = 0;
                do
                {
                    intDate = getWeekendAndHoliDays(fdlDate, ddate, client);
                    fdlDate = ddate;
                    ddate = ddate.AddDays(intDate);
                }
                while (intDate != 0);
                return ddate;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private string GenrateCandidateCode(short id = 0)
        {
            try
            {
                var ClientDetails = repoPQClient.GetPQClientForCadiCodeGenById(id);
                int CodeGeneration = ClientDetails.CodeGeneration;
                string CandidateCode = string.Empty;
                if (CodeGeneration > 0)
                {
                    string str1 = ClientDetails.ClientAbbName;
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

                }
                else
                {
                    CandidateCode = "";
                }
                return CandidateCode;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [NonAction]
        private int getWeekendAndHoliDays(DateTime oDate, DateTime dDate, PQClientCodeHolidaysViewModel clientModel)
        {
            try
            {
                int intDate = 0;
                //1-Client or 0-Default
                if (clientModel.MapHolidays == 1)
                {
                    var clientHoli = repoPQClientHoliday.GetClientHolidays(clientModel.ClientRowID).Where(ch => ch.HolidayDate.Value.Date >= oDate && ch.HolidayDate.Value <= dDate);
                    //int offDayCount = 0;
                    //TimeSpan diff = dDate - oDate;
                    //int days = diff.Days;
                    //for (var i = 1; i <= days; i++)
                    //{
                    //    var testDate = oDate.AddDays(i);
                    //    var currDate = clientHoli.Where(cd => cd.HolidayDate.Value.Date == testDate.Date).Select(c => c.HolidayDate).ToList();
                    //    if (currDate.Count() != 0)
                    //    {
                    //        offDayCount += 1;
                    //    }
                    //}

                    intDate = clientHoli.Count();
                    dDate = dDate.AddDays(intDate);

                }
                //DaysCalculation ==1-Working Days or 0-Calendar Days
                if (clientModel.DaysCalculation == 1)
                {
                    // Weekend Days count
                    int weekEndCount = 0;
                    TimeSpan diff = dDate - oDate;
                    int days = diff.Days;
                    for (var i = 1; i <= days; i++)
                    {
                        var testDate = oDate.AddDays(i);
                        if (testDate.DayOfWeek == DayOfWeek.Saturday || testDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            weekEndCount += 1;
                        }
                    }
                    dDate = dDate.AddDays(weekEndCount);
                    intDate += weekEndCount;
                }
                return intDate;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetCompanyRefNo(short id = 0, byte bckDays = 0)
        {
            try
            {
                string CRefNo = string.Empty;
                short ClientAbbRowID = repoPQClient.GetPQClientForUpdateById(id).ClientAbbRowID;
                string str1 = repoAbbreviation.GetAbbreviationForUpdateById(ClientAbbRowID).ClientAbbreviation;
                int noOfEntr = repoPQPersonal.GetAllCandidatesById(id);
                string empty = string.Empty;
                int NoOfCases = 0, OldValue = 0;
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

                string[] strArrays = new string[] { str1, "-", DateTime.Now.AddDays(-bckDays).ToString("dd-MM-yyyy"), "-", empty };
                CRefNo = string.Concat(strArrays);
                return CRefNo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [NonAction]
        private void CreateNavigationforInsuffCaseForSpoc(int ClientRowId)
        {

            int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
            string CreatedBy = Convert.ToString(Session["CreatedBy"]);
            string spocEmail = Session["TeamMemberEmail"].ToString();
            var AllChecks = repoPQPersonal.GetAllInsuffChecksCountForCSSpoc1(ClientRowId, spocEmail);
            string NavTeamMemberDE = string.Empty;
            NavTeamMemberDE += "<div class='col-md-1 col-sm-4 col-xs-6 tile_stats_count'><a href='" + Url.Action("ManageAddressInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'> <div class='tile-stats1'><div class='count_top'><b>Address</b></div><div class='count'> " + AllChecks.AddressCount + " </div></div></a></div>";
            NavTeamMemberDE += "<div class='col-md-1 col-sm-4 col-xs-6 tile_stats_count'><a href='" + Url.Action("ManageEmploymentInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'><div class='tile-stats1'><div class='count_top'><b>Employment</b></div><div class='count'> " + AllChecks.EmploymentCount + "</div></div></a></div>";
            NavTeamMemberDE += "<div class='col-md-1 col-sm-4 col-xs-6 tile_stats_count'><a href='" + Url.Action("ManageEducationInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'><div class='tile-stats1'><div class='count_top'><b>Education</b></div><div class='count'>" + AllChecks.EducationCount + "</div></div></a></div>";
            NavTeamMemberDE += "<div class='col-md-1 col-sm-4 col-xs-6 tile_stats_count'><a href='" + Url.Action("ManageCriminalInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'><div class='tile-stats1'><div class='count_top'><b>Criminal</b></div><div class='count'>" + AllChecks.CriminalCount + "</div></div></a></div>";
            NavTeamMemberDE += "<div class='col-md-1 col-sm-4 col-xs-6 tile_stats_count'><a href='" + Url.Action("ManageNationalIdentityInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'><div class='tile-stats1'><div class='count_top'><b>National Identity</b></div><div class='count'>" + AllChecks.NatIdentityCount + "</div></div></a></div>";
            NavTeamMemberDE += "<div class='col-md-1 col-sm-4 col-xs-6 tile_stats_count'><a href='" + Url.Action("ManageReferenceInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'><div class='tile-stats1'><div class='count_top'><b>Reference</b></div><div class='count'>" + AllChecks.ReferenceCount + "</div></div></a></div>";
            NavTeamMemberDE += "<div class='col-md-1 col-sm-4 col-xs-6 tile_stats_count'><a href='" + Url.Action("ManageSpecialCheckInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'><div class='tile-stats1'><div class='count_top'><b>Special Check</b></div><div class='count'>" + AllChecks.SpecialcheckCount + "</div></div></a></div>";
            Session["NavLeftInsuffMenuForSpoc"] = NavTeamMemberDE;
        }

        [NonAction]
        private void CreateNavigationforInsuffCaseForSpoc_MultipleClient(DataTable ClientRowId)
        {

            int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
            string CreatedBy = Convert.ToString(Session["CreatedBy"]);
            string spocEmail = Session["TeamMemberEmail"].ToString();
            var AllChecks = repoPQPersonal.GetAllInsuffChecksCountForCSSpoc1_MultipleClient(ClientRowId, spocEmail);
            string NavTeamMemberDE = string.Empty;
            NavTeamMemberDE += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-51px;'><span class='count_top' style='margin-right:38px'><b>Address</b></span><div class='count' style='margin-right:41px'><a href='" + Url.Action("ManageAddressInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'> " + AllChecks.AddressCount + "</a></div></div>";
            NavTeamMemberDE += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>Employment</b></span><div class='count' style='margin-right:41px'><a href='" + Url.Action("ManageEmploymentInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'> " + AllChecks.EmploymentCount + " </a></div></div>";
            NavTeamMemberDE += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>Education</b></span><div class='count' style='margin-right:41px'><a href='" + Url.Action("ManageEducationInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'> " + AllChecks.EducationCount + " </a></div></div>";
            NavTeamMemberDE += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>Criminal</b></span><div class='count' style='margin-right:41px'><a href='" + Url.Action("ManageCriminalInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'> " + AllChecks.CriminalCount + "</a></div></div>";
            NavTeamMemberDE += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>National Identity</b></span><div class='count' style='margin-right:41px'><a href='" + Url.Action("ManageNationalIdentityInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'> " + AllChecks.NatIdentityCount + "</a></div></div>";
            NavTeamMemberDE += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>Reference</b></span><div class='count' style='margin-right:41px'><a href='" + Url.Action("ManageReferenceInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'> " + AllChecks.ReferenceCount + "</a></div></div>";
            NavTeamMemberDE += "<div class='col-md-2 col-sm-4 col-xs-6 tile_stats_count' style='margin-left:-30px;'><span class='count_top' style='margin-right:38px'><b>Special Check</b></span><div class='count' style='margin-right:41px'><a href='" + Url.Action("ManageSpecialCheckInsuffForCSSpoc", "ClientServicing", new { ClientRowID = clsCommonMethods.PEncrypt(ClientRowId.ToString()) }) + "'> " + AllChecks.SpecialcheckCount + "</a></div></div>";
            Session["NavLeftInsuffMenuForSpoc"] = NavTeamMemberDE;
        }

        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ExportCaseStatus(string ClientRowId, string dtf, string dtt)
        {
            short ClientRowID = 0;
            try
            {
                var tblRowID = GetClientRowID(ClientRowId);
                //ClientRowID = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowId));

                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();
                if (ClientRowID == 317 || ClientRowID == 327)
                {
                    //ds = cc.WiproCasewiseCheckColorCodeAndStatus_CS(ClientRowID, teamMemerRowId);
                    ds = cc.WiproCasewiseCheckColorCodeAndStatus_MultipleClient(tblRowID, teamMemerRowId, dtf, dtt);
                    if (ds.Tables.Count > 0)
                    {
                        var dt = new DataTable("Details");
                        dt = ds.Tables[0];
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
                            Response.AddHeader("content-disposition", "attachment;filename=PendingWithCandidate.xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                    }

                }
                else
                {
                    //ds = cc.CasewiseCheckColorCodeAndStatus_CS(ClientRowID, teamMemerRowId);
                    ds = cc.CasewiseCheckColorCodeAndStatus_MultipleClient(tblRowID, teamMemerRowId, dtf, dtt);
                    if (ds.Tables.Count > 0)
                    {
                        var dt = new DataTable("Details");
                        dt = ds.Tables[0];
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
                            Response.AddHeader("content-disposition", "attachment;filename=PendingWithCandidate.xlsx");

                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                    }

                }
                return null;
            }

            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return RedirectToAction("Index", "ClientServicing");
            }
        }

        private DataTable GetClientRowID(string clientRowId)
        {
            DataTable tbl = new DataTable();
            if (!tbl.Columns.Contains("ClientRowID"))
            {
                tbl.Columns.Add("ClientRowID", typeof(int));
            }
            string[] strClient = clientRowId.Split(',');

            for (int i = 0; i < strClient.Length; i++)
            {
                DataRow dr = tbl.NewRow();
                if (strClient[i] != "")
                {
                    dr["ClientRowID"] = Convert.ToInt16(clsCommonMethods.PDecrypt(strClient[i]));
                }
                else
                {
                    dr["ClientRowID"] = Convert.ToInt16(0);
                }

                tbl.Rows.Add(dr);
            }
            return tbl;
        }
        private DataTable GetDashClientRowID(string clientRowId)
        {
            DataTable tbl = new DataTable();
            if (!tbl.Columns.Contains("ClientRowID"))
            {
                tbl.Columns.Add("ClientRowID", typeof(int));
            }
            string[] strClient = clientRowId.Split(',');

            for (int i = 0; i < strClient.Length; i++)
            {
                DataRow dr = tbl.NewRow();
                if (strClient[i] != "0")
                {
                    dr["ClientRowID"] = Convert.ToInt16(clsCommonMethods.PDecrypt(strClient[i]));
                }
                else
                {
                    dr["ClientRowID"] = Convert.ToInt16(0);
                }

                tbl.Rows.Add(dr);
            }
            return tbl;
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult RefreshDashboardDetails(string ClientRowID = "")
        {
            string Department = Session["Department"].ToString();
            try
            {
                DataTable tblRowID = new DataTable();
                short ClientRowId = 0;
                string[] strClient = ClientRowID.Split(',');

                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);

                string SpocEmail = Session["TeamMemberEmail"].ToString();

                bool flagMgr = false;
                if (Department == "Sales Department")
                {
                    flagMgr = true;
                    ViewBag.Dashboard = "Sales Dashboard!";
                }
                if (Department == "Pending Case")
                {
                    flagMgr = true;
                    ViewBag.Dashboard = "Insuff YTR FundsPending Dashboard!";
                }

                if (flagMgr)
                {
                    if (strClient.Length > 1)
                    {
                        tblRowID = GetDashClientRowID(ClientRowID);
                        ViewBag.ClientID = Convert.ToInt32(tblRowID.Rows[0]["ClientRowID"]);
                        ViewBag.ManagerDashboard = repoDashboard.GetManagerClientServicingDashBoardInfo_MultipleClient(teamMemerRowId, tblRowID, SpocEmail);
                        CreateNavigationforInsuffCaseForSpoc_MultipleClient(tblRowID);

                    }
                    else
                    {
                        ClientRowID = strClient[0];
                        if (ClientRowID != "")
                        {
                            ClientRowId = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowID));
                        }
                        else
                        {
                            ClientRowId = 0;
                        }

                        ViewBag.ClientID = ClientRowId;
                        ViewBag.ManagerDashboard = repoDashboard.GetManagerClientServicingDashBoardInfo(teamMemerRowId, ClientRowId, SpocEmail, Department);
                        CreateNavigationforInsuffCaseForSpoc(ClientRowId);
                    }


                }


            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            if (Department == "Pending Case")
            {
                return PartialView("_PendingCaseDashboard");
            }
            else
                return PartialView("_SalesDashboard");
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ManageSubGroup(int page = 1, string sort = "SubGroupName", string sortDir = "asc", string Search = "")
        {
            ClientSubGroupListPagedModel model = new ClientSubGroupListPagedModel();
            try
            {
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                int pageSize = clsGlobalCommon.PageSize;
                model = repoCliSubGroup.GetClientSubGroupsNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search , teamMemerRowId);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
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
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public JsonResult DeleteSubGroup(short id)
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    repoCliSubGroup.DeleteClientSubGroup(id);
                    int Res = repoCliSubGroup.SaveChanges();
                    if (Res > 0)
                    {
                        result = true;
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public void ExportSubGroup()
        {
            try
            {
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                var data = repoCliSubGroup.GetClientSubGroupForExportNew(teamMemerRowId);
                ExportToExcelNew(data, "SubGroup");
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult AddSubGroup()
        {
            try
            {
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                BindClientAbbListbyId(teamMemerRowId); 

            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult AddSubGroup(AddClientSubGroupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.SubGroupName != null && model.SubGroupName.Trim().Length > 0)
                    {
                        if (!repoCliSubGroup.IsClientSubGroupExist(model.SubGroupName.Trim(), model.ClientAbbRowID))
                        {
                            int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                            repoCliSubGroup.AddClientSubGroupNew(model , teamMemerRowId);

                            int Res = repoCliSubGroup.SaveChanges();
                            if (Res > 0)
                            {
                                TempData["SuccessMsg"] = "Record Added Successfully!";
                                ModelState.Clear();
                                return RedirectToAction("ManageSubGroup");
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Record Already Exist!";
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", " Sub Group could not be blank!");
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
            int teamMemerRowIds = Convert.ToInt32(Session["TeamMemberRowID"]);
            BindClientAbbListbyId(teamMemerRowIds);
            return View();
        }


        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ManagePQClient(int page = 1, string sort = "CreatedDate", string sortDir = "desc", string Search = "")
        {
            PQClientListPagedModel model = new PQClientListPagedModel();
            try
            {
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                int pageSize = clsGlobalCommon.PageSize;
                model = repoPQClient.GetPQClientNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search , teamMemerRowId);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }

                Session["ClientNameForContract"] = null;
                Session["PricingType"] = null;
                Session["MapAntecedent"] = null;
                Session["MapDisposition"] = null;
                Session["MapSeverity"] = null;
                Session["MailSendBy"] = null;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult AddPQClient()
        {
            int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
            BindCountry();
            BindClientAbbListbyId(teamMemerRowId);
            // BindClientSpocList();
            BindPrimaryClientSpocList();
            BindSecondaryClientSpocList();
            BindYesNoList();
            BindReportSentByList();
            BindCandidateReminderMailGapList();
            BindNoOfReminderList();
            BindRequiredColorCodeReviewList();
            BindPricingTypeList();
            BindSeverityTypeList();
            BindBranchOfficeList();
            BindBillingCycleList();
            return View();
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult AddPQClient(AddPQClientViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize CCObj = new ClientCustomize();
                    short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);

                    if (model.ClientAbbRowID > 0 && model.ClientSubgroupID > 0)
                    {
                        bool SpocEmailID = false;
                        if (!string.IsNullOrEmpty(model.SpocEmailID))
                        {
                            var SpocEmailIds = model.SpocEmailID.Split(',');
                            foreach (string email in SpocEmailIds)
                            {
                                if (!clsValidationMethods.IsValidEmail(email))
                                {
                                    SpocEmailID = true;
                                }
                            }
                        }
                        if (!SpocEmailID)
                        {
                            string strClientAbbName = repoPQClient.GetClientAbbNameNew(model.ClientAbbRowID);
                            string SubGroupName = repoPQClient.GetSubGroupNameNew(model.ClientSubgroupID);
                            string strClientCode = strClientAbbName + "-" + SubGroupName + "-" + model.DistrictRowID.ToString() + "-" + model.LocationRowID.ToString();

                            model.ClientCode = strClientCode;
                            model.ClientCode1 = strClientAbbName;
                            model.Status = 1;
                            model.CreatedDate = DateTime.Now;
                            model.SLAUploaded = 0;
                            model.AgreementType = 1;

                            if (!repoPQClient.IsPQClientData(model.ClientCode, model.ClientSubgroupID))
                            {
                                model.SpocName = string.Concat(model.SpocName, ",", teamMemerRowId.ToString());
                                //repoPQClient.AddPQClient(model);
                                //int Res = repoPQClient.SaveChanges();
                                int Res = CCObj.AddPQClient(model, teamMemerRowId);
                                if (Res == 0)
                                {
                                    //repoPQClient.AddClientSevirityDetails(model.ClientCode, model.ClientSubgroupID);

                                    if (model.MapHolidays == 0)
                                    {
                                        repoPQClient.AddClientDefaultHolidays(model.ClientCode, model.ClientSubgroupID);
                                    }
                                    CCObj.sp_ClientMailSendingSpocDetails(model);
                                    //ClientOnboardingMailTemplates(Convert.ToInt32(model.ClientAbbRowID));
                                    TempData["SuccessMsg"] = "Record Added Successfully!";
                                    ModelState.Clear();
                                    return RedirectToAction("ManagePQClient");
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = "There is some problem to save record. Please try again later!";
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "Client Already Exists.Choose Another one!";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Please enter correct email!";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "All * fields are mandatory!");
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
            BindCountry();
            // BindClientAbbList();
            int teamMemerRowIds = Convert.ToInt32(Session["TeamMemberRowID"]);
            BindClientAbbListbyId(teamMemerRowIds);
            //BindClientSpocList();
            BindPrimaryClientSpocList();
            BindSecondaryClientSpocList();
            BindYesNoList();
            BindCandidateReminderMailGapList();
            BindNoOfReminderList();
            BindRequiredColorCodeReviewList();
            BindReportSentByList();
            BindPricingTypeList();
            BindSeverityTypeList();
            BindBranchOfficeList();
            BindBillingCycleList();
            return View(model);
        }


        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public void ExportClient()
        {
            try
            {
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                var data = repoPQClient.GetClientForExportNew(teamMemerRowId);
                ExportToExcelNew(data, "MasterClient");
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        public void ExportToExcelNew<T>(IEnumerable<T> list, string strFileName)
        {
            try
            {
                // Step 1 - get the data from database
                var data = list.ToList();

                // instantiate the GridView control from System.Web.UI.WebControls namespace
                // set the data source
                GridView gridview = new GridView();
                gridview.DataSource = data;
                gridview.DataBind();

                // Clear all the content from the current response
                Response.ClearContent();
                Response.Buffer = true;

                // set the header
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                clsExportDataToExcel.WriteHtmlTable(data, Response.Output);

                Response.Flush();
                Response.End();
            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult EditSubGroup(short id = 0)
        {
            UpdateClientSubGroupViewModel model = new UpdateClientSubGroupViewModel();
            try
            {
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                BindClient();
                model = repoCliSubGroup.GetClientSubGroupForUpdateById(id);

            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View(model);
        }
        [NonAction]
        public void BindClient()
        {
            try
            {
                ViewBag.ClientSubGroup = repoDDL.GetClientSubGroupDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult EditSubGroup(UpdateClientSubGroupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.SubGroupName != null && model.SubGroupName.Trim().Length > 0 && model.ClientSubGroupID > 0)
                    {
                        if (!repoCliSubGroup.IsClientSubGroupExist(model.SubGroupName.Trim(), model.ClientAbbRowID))
                        {
                            repoCliSubGroup.UpdateClientSubGroup(model);
                            int Res = repoCliSubGroup.SaveChanges();
                            if (Res > 0)
                            {
                                TempData["SuccessMsg"] = "Record Edited Successfully!";
                                ModelState.Clear();
                                return RedirectToAction("ManageSubGroup");
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "There is no change!";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Record Already Exist!";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sub Group Name could not be blank!");
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
            BindClient();
            return View();
        }


        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult SpecialInstruction(int id = 0)
        {
            PQClientSpecialInstructionViewModel model = new PQClientSpecialInstructionViewModel();
            try
            {
                GetCheckFamilyForIns();
                BindHCDepartmentList();
                BindPriorityList();
                model = repoPQClient.GetPQClientInstructionForUpdateById(id);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            TempData["Message"] = "";
            return View(model);
        }

        [HttpPost]
        public JsonResult BindSpecialInstructionToDelete(string sid)
        {
            if (string.IsNullOrEmpty(sid))
            {
                sid = "0";
            }
            try
            {
                repoPQClient.GetSpecialInstructionForDeleteById(Convert.ToInt64(sid));
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                repoPQClient.GetSpecialInstructionForDeleteById(Convert.ToInt64(sid));
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult BindSpecialInstructionToUpdate(string sid)
        {
            if (string.IsNullOrEmpty(sid))
            {
                sid = "0";
            }
            var result = repoPQClient.GetSpecialInstructionForUpdateById(Convert.ToInt64(sid));
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SpecialInstruction(PQClientSpecialInstructionViewModel objs)
        {
            PQClientSpecialInstructionViewModel model = new PQClientSpecialInstructionViewModel();
            try
            {
                TempData["Message"] = "";
                GetCheckFamilyForIns();
                BindHCDepartmentList();
                BindPriorityList();
                if (objs.btnSubmit != "Search")
                {
                    objs.CreatedBy = Convert.ToInt64(Session["TeamMemberRowID"]);
                    //repoPQClient.AddPQClientSpecialInstruction(objs);
                    if (objs.btnSubmit == "Submit")
                    {
                        ClientCustomize cc = new ClientCustomize();
                        var Res = cc.GetExistSpecialInstruction(objs.ClientRowID, objs.CheckFamilyRowID, objs.Department);
                        if (Res.Rows.Count > 0)
                        {
                            ViewBag.ErrorMsg = "Special Instruction Exist.";
                        }
                        else
                        {
                            if (objs.CheckFamilyRowID == 1)
                            {
                                var checkFamlieslist = repoDDL.GetCheckFamilyForDDL().ToList();
                                var idArray = checkFamlieslist.Select(x => x.CheckFamilyRowID).ToArray();
                                for (int i = 0; i < idArray.Length; i++)
                                {
                                    objs.CheckFamilyRowID = idArray[i];
                                    repoPQClient.AddPQClientSpecialInstruction(objs);

                                }
                                TempData["Message"] = "Record Saved Successfully.";
                            }
                            else
                            {
                                repoPQClient.AddPQClientSpecialInstruction(objs);
                                TempData["Message"] = "Record Saved Successfully.";
                            }
                            //repoPQClient.AddPQClientSpecialInstruction(objs);
                            //TempData["Message"] = "Record Saved Successfully.";
                        }
                    }
                    if (objs.btnSubmit == "Update")
                    {

                        if (objs.CheckFamilyRowID == 1)
                        {
                            var checkFamlieslist = repoDDL.GetCheckFamilyForDDL().ToList();
                            var idArray = checkFamlieslist.Select(x => x.CheckFamilyRowID).ToArray();
                            for (int i = 0; i < idArray.Length; i++)
                            {
                                objs.CheckFamilyRowID = idArray[i];
                                repoPQClient.AddPQClientSpecialInstruction(objs);


                            }
                            TempData["Message"] = "Record Updated Successfully.";
                        }
                        else
                        {
                            repoPQClient.AddPQClientSpecialInstruction(objs);
                            TempData["Message"] = "Record Updated Successfully.";
                        }
                        //repoPQClient.AddPQClientSpecialInstruction(objs);
                        //TempData["Message"] = "Record Updated Successfully.";
                    }
                }

                if (objs.CheckFamilyRowID == 1)
                {
                    model = repoPQClient.GetPQClientInstructionForUpdateById(objs.ClientRowID);
                }
                else
                {
                    //model = repoPQClient.GetPQClientInstructionForUpdateById(objs.ClientRowID, objs.Department, objs.CheckFamilyRowID);
                    model = repoPQClient.GetPQClientInstructionForUpdateById(objs.ClientRowID);
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                TempData["Message"] = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ClientServicingDashboardInfo(int page = 1, bool fetchDataCheckbox = false, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", byte tatdays = 0)
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                short clientid = 0;
                if (cr != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                }
                int pageSize = 20;
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                Session["clientid"] = clientid;
                Session["CaseStatus"] = CaseStatus;
                Session["clientid"] = clientid;
                string Designation = Session["Designation"].ToString();
                model = repoDashboard.GetClientServicingDashBoardInfo1(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid, fetchDataCheckbox);

                //IBy this Code e are getting clientrowid on searching for showing LOA for Genpac-EU and Uniques.
                if (Search != "")
                {
                    var clients = model.DashboardClientCandidates;
                    foreach (var client in clients)
                    {
                        // Access the ClientRowID property
                        ViewBag.ClientRowIDForLOA = clsCommonMethods.PEncrypt(client.ClientRowID.ToString());
                    }
                }

                Session["CaseStatus"] = CaseStatus;
                Session["cr"] = clientid;
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                BindClientListByLogInName(SpocEmail);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult ToggleSeniorProfileCase(int personalRowID, bool isSenior)
        {
            try
            {
                ClientCustomize CC = new ClientCustomize();
                string teamMemerRowId = Session["TeamMemberRowID"].ToString();
                CC.AddRemoveSeniorProfileCase(personalRowID.ToString(), teamMemerRowId, isSenior);
                return Json(new { success = true, message = "Status updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [ValidateInput(false)]
        public ActionResult InternationalLoA(int id = 0, int ClientRowID = 0, string CaseStatus = "")
        {
            ClientCustomize cc = new ClientCustomize();
            DataTable dt = new DataTable();

            dt = cc.International_LoA(id);
            string FileName = "";
            if (dt.Rows.Count > 0)
            {
                FileName = Convert.ToString(dt.Rows[0]["InterLOA"]);
            }
            string path = Server.MapPath("~/Uploads/") + FileName;
            string clientrowid = "1428";
            // Check if the file exists
            if (System.IO.File.Exists(path))
            {
                // Download the file
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                Response.TransmitFile(path);
                Response.End();
            }
            else
            {
                // Handle the case when the file does not exist
                Response.Write("File not found.");
            }
            return RedirectToAction("ClientServicingDashboardInfo", "ClientServicing", new { cr = clsCommonMethods.PEncrypt(clientrowid), CaseStatus = CaseStatus });
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ClientServicingDashboardCaseInfo(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", byte tatdays = 0, string Aging = "")
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                short clientid = 0;
                if (cr != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                }

                int pageSize = 20;
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);

                if (Aging == "Yes")
                {
                    model = repoDashboard.GetClientServicingDashBoardInfo_Aging(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid);
                }
                else
                {
                    model = repoDashboard.GetClientServicingDashBoardInfo1(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid);
                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                BindClientListByLogInName(SpocEmail);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }


        #region ******** Manage FundPending By Spoc ********

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageFundPendingChecksForSpoc(int page = 1, string sort = "", string sortDir = "asc", string Search = "", short ClientRowID = 0)
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            try
            {
                int pageSize = 20;
                short teamMemberId = Convert.ToInt16(Session["TeamMemberRowID"]);
                model = repoPQVerifiedUploadDoc.GetFundPendingCheckListForSpoc(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, SpocEmail, ClientRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientListByLogInName(SpocEmail);
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult FundPendingApproveBySpoc(int PersonalId = 0, int VerId = 0, short chkFmlyId = 0, short SubchkFmlyId = 0)
        {
            FundPendingApprovalViewModel model = new FundPendingApprovalViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                DataTable dt;
                dt = cc.GetsFundsPendingAmount(PersonalId, SubchkFmlyId);
                if (dt.Rows.Count > 0)
                {
                    model.Amount = Convert.ToInt32(dt.Rows[0]["Amount"].ToString());
                    model.Remarks = dt.Rows[0]["Remarks"].ToString();
                    model.ExpectedTat = dt.Rows[0]["ExpectedTAT"].ToString();
                }
                if (PersonalId > 0 && VerId > 0 && chkFmlyId > 0 && SubchkFmlyId > 0)
                {
                    model.PersonalRowID = PersonalId;
                    model.VerificationRowID = VerId;
                    model.CheckFamilyRowID = chkFmlyId;
                    model.SubCheckRowID = SubchkFmlyId;
                }
                else
                {
                    if (PersonalId > 0 && SubchkFmlyId > 0)
                    {
                        model.PersonalRowID = PersonalId;
                        model.SubCheckRowID = SubchkFmlyId;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult FundPendingApproveBySpoc(FundPendingApprovalViewModel model, HttpPostedFileBase ProofDocument
)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();

                if (ModelState.IsValid)
                {
                    if (model.PersonalRowID > 0 && model.VerificationRowID > 0 && model.CheckFamilyRowID > 0 && model.SubCheckRowID > 0)
                    {
                        model.CheckStatus = "WIP-Pending";
                        model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                        model.ModifiedDate = DateTime.Now;
                        repoVerificationInfo.FundPendingChecksUpdateByDepartmentTL(model);

                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                        #endregion

                        ModelState.Clear();
                        ViewBag.SuccessMsg = "Record Updated Successfully!";

                        //TempData["SuccessMsg"] = "Record Updated Successfully!";
                        //RedirectToAction("ManageFundPendingChecksForDeprtTL");
                    }
                    else
                    {
                        if (model.PersonalRowID > 0 && model.SubCheckRowID > 0)
                        {

                            if (model.FundsStatus == "Approve")
                            {

                                if (ProofDocument != null && ProofDocument.ContentLength > 0)
                                {

                                    VerifiedUploadDocViewModel modell = new VerifiedUploadDocViewModel();
                                    modell.DocumentType = "Payment Proof";
                                    modell.PersonalRowID = model.PersonalRowID;
                                    modell.Remarks = "Payment Proof";
                                    modell.SubCheckRowID = model.SubCheckRowID;
                                    modell.UploadDoc = ProofDocument;
                                    modell.CheckStatus = model.CheckStatus;
                                    UplodEducationVerDocsN(modell, "FundPendingProofDocs");


                                }

                                int spocId = Convert.ToInt16(Session["TeamMemberRowID"]);
                                // 61 check status for funds request closed by client
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 61, model.Remarks, "Spoc", spocId);
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 61, "Spoc", spocId);
                                cc.InsertFundsAmount(model.PersonalRowID, model.SubCheckRowID, model.Amount, model.Remarks, Session["TeamMemberRowID"].ToString(), Session["TeamMemberRowID"].ToString(), model.FundsStatus, model.ExpectedTat);
                                repoPQCandidateCheckRepository.UpdateFundDateandRemark(model.PersonalRowID, model.SubCheckRowID, "Approve", model.Remarks);
                                #region Check Action History Save                           
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Fund Approve ", model.Remarks);
                                #endregion
                                //SendAlertMassageForFundApproval(model.PersonalRowID, model.SubCheckRowID,model.Remarks);
                                SendAlertMassageForFundApprovalMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks, "APPROVED", "Funds Approval", 0, 1);
                                ModelState.Clear();
                                ViewBag.SuccessMsg = "Record Updated Successfully!";
                            }
                            else
                            {
                                int spocId = Convert.ToInt16(Session["TeamMemberRowID"]);
                                // 61 check status for funds request closed by client
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 61, model.Remarks, "Spoc", spocId);
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 61, "Spoc", spocId);
                                cc.InsertFundsAmount(model.PersonalRowID, model.SubCheckRowID, model.Amount, model.Remarks, Session["TeamMemberRowID"].ToString(), Session["TeamMemberRowID"].ToString(), model.FundsStatus, model.ExpectedTat);
                                repoPQCandidateCheckRepository.UpdateFundDateandRemark(model.PersonalRowID, model.SubCheckRowID, "Reject", model.Remarks);
                                #region Check Action History Save                           
                                clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Fund Reject ", model.Remarks);
                                #endregion
                                //SendAlertMassageForFundApproval(model.PersonalRowID, model.SubCheckRowID,model.Remarks);
                                SendAlertMassageForFundApprovalMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks, "REJECTED", "Funds Rejected", 0, 1);
                                ModelState.Clear();
                                ViewBag.SuccessMsg = "Record Updated Successfully!";
                            }
                        }
                        else
                            ViewBag.ErrorMsg = "All fields are mandatory!";
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


        public bool UplodEducationVerDocsN(VerifiedUploadDocViewModel model, string Action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    int inputSize = 0;
                    ClientCustomize CC = new ClientCustomize();
                    inputSize = CC.GetControlInputs("ArtifactPageSize");

                    if (model.DocumentType == "Others if any")
                    {
                        model.DocumentType = model.OtherDocumentType?.Trim(); // Use the custom value
                    }

                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        int pageCount = GetNoOfPageInPDFFile(model.UploadDoc);
                        if (pageCount <= inputSize)
                        {
                            if (model.DocumentType == null)
                            {
                                model.DocumentType = "Varification Document";
                            }
                            model.DocumentUploadFrom = "Verified";
                            model.StatusDocxConvert = 0;
                            model.StatusPDF2DOCXConvert = 0;
                            model.UploadDate = DateTime.Now;
                            model.Status = 1;
                            string errorMessage = GatZipFileMessage(model.UploadDoc, model.PersonalRowID.ToString(), "UplodEducationVerDocs");
                            if (errorMessage != "")
                            {
                                ViewBag.ErrorMsg += errorMessage;
                                //return View(model);
                            }
                            else
                            {
                                string ext = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                                CC.UpdateImageArtifact(model.PersonalRowID, model.SubCheckRowID);
                                proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + model.SubCheckRowID + "_" + model.UploadDate.Value.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                                model.UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                                model.FileName = proImg;
                                repoPQVerifiedUploadDoc.VerifiedUploadDoc(model);
                                int Res = repoPQVerifiedUploadDoc.SaveChanges();
                                if (Res > 0)
                                {
                                    //================================
                                    string fileName = proImg; /*Path.GetFileName(model.UploadDoc.FileName);*/
                                    string path = Path.Combine(Server.MapPath("~/Documents/CandidateDocuments/FundPendingProofDocs/"), fileName);
                                    var directoryPath = Path.GetDirectoryName(path);
                                    if (!Directory.Exists(directoryPath))
                                    {
                                        Directory.CreateDirectory(directoryPath);
                                    }
                                    model.UploadDoc.SaveAs(path);


                                    ClientCustomize cc1 = new ClientCustomize();
                                    cc1.InsertProofDocuments(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, fileName, "~/Documents/CandidateDocuments/FundPendingProofDocs/");
                                    //========================================================

                                    #region Check Action History Save 
                                    var checkfamilyDB = cc1.GetCheckFamilyname(Convert.ToInt32(model.SubCheckRowID));

                                    string CheckFamilyName = Convert.ToString(checkfamilyDB.Rows[0]["CheckFamilyName"]);
                                    clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, CheckFamilyName + " information Upload Doc " /*"Education Verification information Upload Doc "*/);
                                    #endregion
                                    ModelState.Clear();
                                    ViewBag.SuccessMsg = "Record Added Successfully!";
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = "There is some problem to save record. Please try again later!";
                                }
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Page Size Greater Then Authorized Page Size!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "please select upload doc!";
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
            return true;
        }

        #endregion
        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UpdateCheckWiseInfoBySpoc(int id, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0, string msg = "", int checkfamilyrowid = 0)
        {




            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;

            if (checkfamilyrowid == 30)
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);
                if (msg != "")
                {
                    ViewBag.ErrorMsg = msg;
                }
                short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
                int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
                int dptid = GetDepartmentID(tmid);
                ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

                dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
                DataTable dbValue = new DataTable();

                dbValue = cc.GetReportValue(PersonalRowID, subCheckId);

                DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
                DataTable DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
                int SeverityRowID = 0;
                string ColorCode = "";
                string SeverityGrid = "";
                string ReportComment = "";
                if (dts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                    {
                        SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                    {
                        ColorCode = dts.Rows[0]["ColorName"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                    {
                        SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                    }

                    if (DtActionRemark.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }


                    ViewBag.SeverityRowID = SeverityRowID;
                    ViewBag.ColorCode = ColorCode;
                    ViewBag.SeverityGrid = SeverityGrid;
                    ViewBag.ReportComment = ReportComment.ToString();


                }
                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                    }
                    else
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
                }
                ViewBag.PersonalRowid = PersonalRowID.ToString();
                ViewBag.SubCheckId = subCheckId.ToString();
                ViewBag.clientRowId = ClientRowId.ToString();
                ViewBag.CheckStatusid = CheckStatusId.ToString();
                ViewBag.ReportComment = ReportComment.ToString();
                BindDocumentList(PersonalRowID, subCheckId);
                GetCaseDocumentList(PersonalRowID);
                BindMailList(PersonalRowID, subCheckId);
            }
            if (checkfamilyrowid == 2)
            {
                ClientCustomize cc = new ClientCustomize();
                ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

                short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
                int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
                int dptid = GetDepartmentID(tmid);
                ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

                DataTable dt = new DataTable();
                dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
                DataTable dbValue = new DataTable();
                dbValue = cc.GetReportValue(PersonalRowID, subCheckId);

                DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
                DataTable DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
                int SeverityRowID = 0;
                string ColorCode = "";
                string SeverityGrid = "";
                string ReportComment = "";
                if (dts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                    {
                        SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                    {
                        ColorCode = dts.Rows[0]["ColorName"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                    {
                        SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                    }

                    if (DtActionRemark.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }

                    ViewBag.SeverityRowID = SeverityRowID;
                    ViewBag.ColorCode = ColorCode;
                    ViewBag.SeverityGrid = SeverityGrid;
                }
                else
                {
                    ViewBag.SeverityRowID = 0;
                    ViewBag.ColorCode = null;
                    ViewBag.SeverityGrid = null;
                }
                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                            if (d[0].ToString() == "34")
                            {
                                string a = rowsFiltered[0][1].ToString();
                                string[] nm = rowsFiltered[0][1].ToString().Split('_');
                                string CountryName = "";
                                if (nm[0] == "")
                                {
                                    CountryName = nm[1].ToString();
                                }
                                else
                                {
                                    CountryName = nm[0].ToString();
                                }
                                if (CountryName != "")
                                {
                                    ViewBag.CountryName = CountryName;
                                }
                                else
                                {
                                    ViewBag.CountryName = null;
                                }

                                DataTable dtcountry = cc.GetCountryName(CountryName);
                                if (dtcountry.Rows.Count > 0)
                                {
                                    ViewBag.countryid = dtcountry.Rows[0]["CountryRowID"].ToString();
                                    ViewBag.CountryName = dtcountry.Rows[0]["CountryName"].ToString();
                                }
                                else
                                {
                                    ViewBag.countryid = "";
                                    ViewBag.CountryName = "";
                                }
                            }
                            else if (d[1].ToString().Contains("44"))
                            {

                                string a = rowsFiltered[0][1].ToString();
                                string[] nm = rowsFiltered[0][1].ToString().Split('_');
                                string StateName = "";
                                if (nm[0] == "")
                                {
                                    StateName = nm[1].ToString();
                                }
                                else
                                {
                                    StateName = nm[0].ToString();
                                }
                                if (StateName != "")
                                {
                                    ViewBag.StateName = StateName;
                                }
                                else
                                {
                                    ViewBag.StateName = null;
                                }

                                DataTable dtstate = cc.GetStateName(StateName);
                                if (dtstate.Rows.Count > 0)
                                {
                                    ViewBag.stateId = dtstate.Rows[0]["StateRowID"].ToString();
                                    ViewBag.StateName = dtstate.Rows[0]["StateName"].ToString();
                                }
                                else
                                {
                                    ViewBag.stateId = "";
                                    ViewBag.StateName = "";
                                }
                            }
                            else if (d[1].ToString().Contains("32"))
                            {

                                string a = rowsFiltered[0][1].ToString();
                                string[] nm = rowsFiltered[0][1].ToString().Split('_');
                                string CityName = "";
                                if (nm[0] == "")
                                {
                                    CityName = nm[1].ToString();
                                }
                                else
                                {
                                    CityName = nm[0].ToString();
                                }
                                if (CityName != "")
                                {
                                    ViewBag.CityName = CityName;
                                }
                                else
                                {
                                    ViewBag.CityName = null;
                                }
                                DataTable dtCity = cc.GetCityName(CityName);
                                if (dtCity.Rows.Count > 0)
                                {
                                    ViewBag.Cityid = dtCity.Rows[0]["DistrictRowID"].ToString();
                                    ViewBag.CityName = dtCity.Rows[0]["DistrictName"].ToString();
                                }
                                else
                                {
                                    ViewBag.Cityid = "";
                                    ViewBag.CityName = "";
                                }
                            }

                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                    }
                    else
                    {

                        if (d[4].ToString() == "textbox")
                        {
                            marksModel.Add(d[1].ToString(), "");
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                            if (d[1].ToString().Contains("34"))
                            {
                                ViewBag.CountryName = "";
                                ViewBag.countryid = "";
                            }
                            else if (d[1].ToString().Contains("44"))
                            {
                                ViewBag.StateName = "";
                                ViewBag.stateId = "";
                            }
                            else if (d[1].ToString().Contains("32"))
                            {
                                ViewBag.CityName = "";
                                ViewBag.Cityid = "";
                            }
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                        }
                        else
                            marksModel.Add(d[1].ToString(), d[0].ToString());
                    }
                }
                ViewBag.PersonalRowid = PersonalRowID.ToString();
                ViewBag.SubCheckId = subCheckId.ToString();
                ViewBag.clientRowId = ClientRowId.ToString();
                ViewBag.CheckStatusid = CheckStatusId.ToString();
                ViewBag.ReportComment = ReportComment.ToString();
                if (ViewBag.CountryName == null)
                {
                    ViewBag.CountryName = "";
                    ViewBag.countryid = "";
                }

                if (ViewBag.StateName == null)
                {
                    ViewBag.StateName = "";
                    ViewBag.stateId = "";
                }

                if (ViewBag.CityName == null)
                {
                    ViewBag.CityName = "";
                    ViewBag.Cityid = "";
                }
                BindDocumentList(PersonalRowID, subCheckId);
                GetCaseDocumentList(PersonalRowID);
                BindMailList(PersonalRowID, subCheckId);

            }
            if (checkfamilyrowid == 11)
            {
                ClientCustomize cc = new ClientCustomize();
                ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);


                short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
                int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
                int dptid = GetDepartmentID(tmid);
                ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

                DataTable dt = new DataTable();
                dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
                DataTable dbValue = new DataTable();
                dbValue = cc.GetReportValue(PersonalRowID, subCheckId);

                DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
                DataTable DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
                int SeverityRowID = 0;
                string ColorCode = "";
                string SeverityGrid = "";
                string ReportComment = "";
                if (msg != "")
                {
                    ViewBag.ErrorMsg = msg;
                }
                if (dts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                    {
                        SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                    {
                        ColorCode = dts.Rows[0]["ColorName"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                    {
                        SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                    }

                    if (DtActionRemark.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }
                    ViewBag.SeverityRowID = SeverityRowID;
                    ViewBag.ColorCode = ColorCode;
                    ViewBag.SeverityGrid = SeverityGrid;


                }
                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());

                            if (d[1].ToString().Contains("72"))
                            {
                                string a = rowsFiltered[0][1].ToString();
                                string[] nm = rowsFiltered[0][1].ToString().Split('_');
                                string degreename = "";
                                if (nm[1].ToString() != "")
                                {
                                    degreename = nm[1].ToString();
                                }
                                else
                                {
                                    degreename = nm[0].ToString();
                                    nm[1] = nm[0].ToString();
                                }

                                if (degreename != "")
                                {
                                    ViewBag.degreeName = degreename.Trim();
                                }
                                else
                                {
                                    ViewBag.degreeName = "";
                                }

                            }
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                    }
                    else
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
                }
                ViewBag.PersonalRowid = PersonalRowID.ToString();
                ViewBag.SubCheckId = subCheckId.ToString();
                ViewBag.clientRowId = ClientRowId.ToString();
                ViewBag.CheckStatusid = CheckStatusId.ToString();
                ViewBag.ReportComment = ReportComment.ToString();
                BindDocumentList(PersonalRowID, subCheckId);
                GetCaseDocumentList(PersonalRowID);
                BindMailList(PersonalRowID, subCheckId);

                if (ViewBag.degreeName == null)
                {
                    ViewBag.degreeName = "";

                }
            }
            if (checkfamilyrowid == 12)
            {
                ClientCustomize cc = new ClientCustomize();
                ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

                short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
                int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
                int dptid = GetDepartmentID(tmid);

                if (ChFamilRowId == 12 && ClientRowId == 607)
                {
                    DataTable dataTable = cc.GetConxDOJ(PersonalRowID);
                    ViewBag.DOJ = dataTable.Rows[0]["DateofJoining"].ToString();
                    ViewBag.ConxRow = ClientRowId;

                }
                ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

                DataTable dt = new DataTable();
                dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
                DataTable dbValue = new DataTable();
                if ((ClientRowId == 317 || ClientRowId == 327) && (CheckStatusId == 37))
                {
                    dbValue = cc.GetReportValueWipro(PersonalRowID, subCheckId);
                }
                else
                {
                    dbValue = cc.GetReportValue(PersonalRowID, subCheckId);
                    if (ClientRowId == 787)
                    {
                        DataRow dr = dbValue.Select("AntecedentRowID=82").FirstOrDefault();
                        if (dr != null)
                        {
                            DataTable dtt = cc.getAutoEmpDetail(PersonalRowID, subCheckId);
                            dr[1] = dtt.Rows[0]["CompanyAddress"].ToString();
                        }
                    }
                }
                DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
                DataTable DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
                int SeverityRowID = 0;
                string ColorCode = "";
                string SeverityGrid = "";
                string ReportComment = "";
                if (msg != "")
                {
                    ViewBag.ErrorMsg = msg;
                }
                if (dts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                    {
                        SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                    {
                        ColorCode = dts.Rows[0]["ColorName"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                    {
                        SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                    }
                    if (DtActionRemark.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }


                    ViewBag.SeverityRowID = SeverityRowID;
                    ViewBag.ColorCode = ColorCode;
                    ViewBag.SeverityGrid = SeverityGrid;

                }
                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel.Add(d[1].ToString(), d[0].ToString().ToString());
                    }
                    else
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
                }
                ViewBag.PersonalRowid = PersonalRowID.ToString();
                ViewBag.SubCheckId = subCheckId.ToString();
                ViewBag.clientRowId = ClientRowId.ToString();
                ViewBag.CheckStatusid = CheckStatusId.ToString();
                ViewBag.ReportComment = ReportComment.ToString();
                ViewBag.PersonalBind = repoReportQC.GetPersonalDetailsForTM(PersonalRowID, subCheckId);
                BindDocumentList(PersonalRowID, subCheckId);
                GetCaseDocumentList(PersonalRowID);
                BindMailList(PersonalRowID, subCheckId);

            }
            if (checkfamilyrowid == 40)
            {
                ClientCustomize cc = new ClientCustomize();
                ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

                short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
                int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
                int dptid = GetDepartmentID(tmid);
                ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

                DataTable dt = new DataTable();
                dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
                DataTable dbValue = new DataTable();
                dbValue = cc.GetReportValue(PersonalRowID, subCheckId);

                DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
                DataTable DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");

                int SeverityRowID = 0;
                string ColorCode = "";
                string SeverityGrid = "";
                string ReportComment = "";
                if (msg != "")
                {
                    ViewBag.ErrorMsg = msg;
                }
                if (dts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                    {
                        SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                    {
                        ColorCode = dts.Rows[0]["ColorName"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                    {
                        SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                    }
                    if (DtActionRemark.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }
                }
                ViewBag.SeverityRowID = SeverityRowID;
                ViewBag.ColorCode = ColorCode;
                ViewBag.SeverityGrid = SeverityGrid;
                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                    }
                    else
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
                }
                ViewBag.PersonalRowid = PersonalRowID.ToString();
                ViewBag.SubCheckId = subCheckId.ToString();
                ViewBag.clientRowId = ClientRowId.ToString();
                ViewBag.CheckStatusid = CheckStatusId.ToString();
                ViewBag.ReportComment = ReportComment.ToString();
                BindDocumentList(PersonalRowID, subCheckId);
                GetCaseDocumentList(PersonalRowID);
                BindMailList(PersonalRowID, subCheckId);

            }
            if (checkfamilyrowid == 5)
            {
                ClientCustomize cc = new ClientCustomize();
                ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

                short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
                int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
                int dptid = GetDepartmentID(tmid);
                ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

                DataTable dt = new DataTable();
                dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
                DataTable dbValue = new DataTable();
                if ((ClientRowId == 317 || ClientRowId == 327) && (CheckStatusId == 37))
                {
                    dbValue = cc.GetReportValueWipro(PersonalRowID, subCheckId);
                }
                else
                {
                    dbValue = cc.GetReportValue(PersonalRowID, subCheckId);
                }
                DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
                DataTable DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
                int SeverityRowID = 0;
                string ColorCode = "";
                string SeverityGrid = "";
                string ReportComment = "";
                if (msg != "")
                {
                    ViewBag.ErrorMsg = msg;
                }
                if (dts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                    {
                        SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                    {
                        ColorCode = dts.Rows[0]["ColorName"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                    {
                        SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                    }

                    if (DtActionRemark.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }
                    ViewBag.SeverityRowID = SeverityRowID;
                    ViewBag.ColorCode = ColorCode;
                    ViewBag.SeverityGrid = SeverityGrid;
                    ViewBag.ReportComment = ReportComment;
                }
                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                    }
                    else
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
                }
                ViewBag.PersonalRowid = PersonalRowID.ToString();
                ViewBag.SubCheckId = subCheckId.ToString();
                ViewBag.clientRowId = ClientRowId.ToString();
                ViewBag.CheckStatusid = CheckStatusId.ToString();
                BindDocumentList(PersonalRowID, subCheckId);
                GetCaseDocumentList(PersonalRowID);
                BindMailList(PersonalRowID, subCheckId);

            }
            if (checkfamilyrowid == 26)
            {
                ClientCustomize cc = new ClientCustomize();
                ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

                short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
                int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
                int dptid = GetDepartmentID(tmid);
                ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

                DataTable dt = new DataTable();
                dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
                DataTable dbValue = new DataTable();
                dbValue = cc.GetReportValue(PersonalRowID, subCheckId);
                DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
                DataTable DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
                int SeverityRowID = 0;
                string ColorCode = "";
                string SeverityGrid = "";
                string ReportComment = "";
                if (msg != "")
                {
                    ViewBag.ErrorMsg = msg;
                }
                if (dts.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                    {
                        SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                    {
                        ColorCode = dts.Rows[0]["ColorName"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                    {
                        SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                    }

                    if (DtActionRemark.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }

                    ViewBag.SeverityRowID = SeverityRowID;
                    ViewBag.ColorCode = ColorCode;
                    ViewBag.SeverityGrid = SeverityGrid;
                    ViewBag.ReportComment = ReportComment;

                }
                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                    }
                    else
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
                }
                ViewBag.PersonalRowid = PersonalRowID.ToString();
                ViewBag.SubCheckId = subCheckId.ToString();
                ViewBag.clientRowId = ClientRowId.ToString();
                ViewBag.CheckStatusid = CheckStatusId.ToString();
                ViewBag.ReportComment = ReportComment.ToString();
                BindDocumentList(PersonalRowID, subCheckId);
                GetCaseDocumentList(PersonalRowID);
                BindMailList(PersonalRowID, subCheckId);
            }

            ViewBag.Checkfamid = checkfamilyrowid.ToString();
            return View(marksModel);

        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCheckWiseInfoBySpoc(FormCollection formCollection, string action = "")
        {
            int checkfamid = Convert.ToInt32(formCollection["Checkfamid"].ToString());

            if (checkfamid == 30)
            {
                dynamic expando = new ExpandoObject();
                int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());

                short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());

                short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
                int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

                string CheckStatus = action;
                string ColorName = formCollection["ColorsName"].ToString();
                string Remarks = formCollection["Remarks"].ToString();
                string PriorityType = formCollection["PriorityType"].ToString();

                try
                {
                    if (ModelState.IsValid)
                    {
                        ClientCustomize cc = new ClientCustomize();
                        if (clientRowid > 0 && SubCheckid > 0)
                        {
                            int checkstatusid = 0;
                            DataTable Dtstatusid = new DataTable();
                            Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                            if (Dtstatusid.Rows.Count > 0)
                            {
                                checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                            }
                            if (CheckStatus == "Reject")
                            {
                                //cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                //cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //#region Save LogTransaction Details  Case/Check Action History Save
                                //clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateReferenceVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");

                                //clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                //#endregion

                                //ModelState.Clear();
                            }
                            else
                            {
                                var marksModel = expando as IDictionary<string, object>;

                                OperationViewModel cd = new OperationViewModel();
                                cd.PersonalRowID = PersonalRowid;
                                cd.SubCheckRowID = SubCheckid;
                                cd.clientRowId = clientRowid;
                                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                                cd.moduleName = "ReportQC";


                                ClientCustomize bl = new ClientCustomize();
                                DataTable dt = new DataTable();
                                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                                foreach (DataRow d in dt.Rows)
                                {
                                    string[] FString = d[1].ToString().Split('_');

                                    if (d[4].ToString() == "fileUpload")
                                    {
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
                                            }
                                        }
                                    }
                                    else if (d[4].ToString() == "dropdown")
                                    {

                                        foreach (string key in formCollection.AllKeys)
                                        {
                                            if (key == d[0].ToString())
                                            {
                                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
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
                                            }
                                        }
                                        if (chkselect == 0)
                                        {
                                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                        }
                                    }
                                    else
                                        marksModel.Add(d[1].ToString(), d[0].ToString());

                                }
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 20, Remarks, "client clear Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                                cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #region Save LogTransaction Details     Case/Check Action History Save 
                                clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCheckWiseInfoBySpoc", CheckStatus, "", "Update");

                                clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                #endregion
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Invlid Employment id and model!";
                        }
                    }
                }
                catch (Exception Ex)
                {
                    ViewBag.ErrorMsg = "Invalid parameter value";
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                }
                ViewBag.PersonalRowid = PersonalRowid;
                ViewBag.SubCheckId = SubCheckid;
                ViewBag.clientRowId = clientRowid;
                return RedirectToAction("ManageClientReviewClearDashboardInfo", new { CaseStatus = "SpocReview", cr = clsCommonMethods.PEncrypt(clientRowid.ToString()), tmr = Session["Teammemberrowid"].ToString() });

            }
            if (checkfamid == 2)
            {
                dynamic expando = new ExpandoObject();
                int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
                short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
                short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
                int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

                string CheckStatus = action;
                string ColorName = formCollection["ColorsName"].ToString();
                string Remarks = formCollection["Remarks"].ToString();
                string PriorityType = formCollection["PriorityType"].ToString();
                try
                {
                    if (ModelState.IsValid)
                    {
                        ClientCustomize cc = new ClientCustomize();
                        if (clientRowid > 0 && SubCheckid > 0)
                        {
                            int checkstatusid = 0;
                            DataTable Dtstatusid = new DataTable();
                            Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                            if (Dtstatusid.Rows.Count > 0)
                            {
                                checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                            }
                            if (CheckStatus == "Reject")
                            {
                                //cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                //cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //#region Save LogTransaction Details
                                //clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateAddressVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");
                                //#endregion

                                //#region Case/Check Action History Save
                                //clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                //#endregion

                                //ModelState.Clear();
                            }
                            else
                            {
                                var marksModel = expando as IDictionary<string, object>;

                                OperationViewModel cd = new OperationViewModel();
                                cd.PersonalRowID = PersonalRowid;
                                cd.SubCheckRowID = SubCheckid;
                                cd.clientRowId = clientRowid;
                                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                                cd.moduleName = "Client Servicing";

                                ClientCustomize bl = new ClientCustomize();
                                DataTable dt = new DataTable();
                                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
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
                                            }
                                        }
                                    }
                                    else if (d[4].ToString() == "dropdown")
                                    {
                                        if (clientRowid == 317 || clientRowid == 327)
                                        {
                                            foreach (string key in formCollection.AllKeys)
                                            {
                                                if (key == d[0].ToString())
                                                {
                                                    marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                                }
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
                                            }
                                        }
                                        if (chkselect == 0)
                                        {
                                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                        }
                                    }
                                    else
                                        marksModel.Add(d[1].ToString(), d[0].ToString());

                                }
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 20, Remarks, "client clear Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                                cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #region Save LogTransaction Details Case/Check Action History Save
                                clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCheckWiseInfoBySpoc", CheckStatus, "", "Update");

                                clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                #endregion
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Invlid Employment id and model!";
                        }
                    }
                }
                catch (Exception Ex)
                {
                    ViewBag.ErrorMsg = "Invalid parameter value";
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                }
                ViewBag.PersonalRowid = PersonalRowid;
                ViewBag.SubCheckId = SubCheckid;
                ViewBag.clientRowId = clientRowid;
                return RedirectToAction("ManageClientReviewClearDashboardInfo", new { CaseStatus = "SpocReview", cr = clsCommonMethods.PEncrypt(clientRowid.ToString()), tmr = Session["Teammemberrowid"].ToString() });
            }
            if (checkfamid == 11)
            {
                dynamic expando = new ExpandoObject();
                int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());

                short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());

                short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
                int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

                string CheckStatus = action;
                string ColorName = formCollection["ColorsName"].ToString();
                string Remarks = formCollection["Remarks"].ToString();
                string PriorityType = formCollection["PriorityType"].ToString();
                try
                {
                    if (ModelState.IsValid)
                    {
                        ClientCustomize cc = new ClientCustomize();
                        if (clientRowid > 0 && SubCheckid > 0)
                        {
                            int checkstatusid = 0;
                            DataTable Dtstatusid = new DataTable();
                            Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                            if (Dtstatusid.Rows.Count > 0)
                            {
                                checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                            }
                            if (CheckStatus == "Reject")
                            {
                                //cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                //cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //#region Save LogTransaction Details  Case/Check Action History Save
                                //clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEducationVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");

                                //clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                //#endregion

                                //ModelState.Clear();

                            }
                            else
                            {
                                var marksModel = expando as IDictionary<string, object>;

                                OperationViewModel cd = new OperationViewModel();
                                cd.PersonalRowID = PersonalRowid;
                                cd.SubCheckRowID = SubCheckid;
                                cd.clientRowId = clientRowid;
                                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                                cd.moduleName = "ClientServicing";


                                ClientCustomize bl = new ClientCustomize();
                                DataTable dt = new DataTable();
                                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                                foreach (DataRow d in dt.Rows)
                                {
                                    string[] FString = d[1].ToString().Split('_');

                                    if (d[4].ToString() == "fileUpload")
                                    {

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
                                            }
                                        }
                                    }

                                    else if (d[4].ToString() == "dropdown")
                                    {
                                        if (clientRowid == 787 || clientRowid == 317 || clientRowid == 327)
                                        {
                                            foreach (string key in formCollection.AllKeys)
                                            {
                                                if (key == d[0].ToString())
                                                {
                                                    marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                                }
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

                                            }
                                        }
                                        if (chkselect == 0)
                                        {
                                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                        }
                                    }
                                    else
                                        marksModel.Add(d[1].ToString(), d[0].ToString());

                                }

                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 20, Remarks, "client clear Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                // cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                                cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCheckWiseInfoBySpoc", CheckStatus, "", "Update");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                #endregion

                            }
                            //  cc.UpdateReportComments(PersonalRowid, SubCheckid, Remarks);
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Invlid Employment id and model!";
                        }
                    }

                }
                catch (Exception Ex)
                {
                    ViewBag.ErrorMsg = "Invalid parameter value";
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                }
                ViewBag.PersonalRowid = PersonalRowid;
                ViewBag.SubCheckId = SubCheckid;
                ViewBag.clientRowId = clientRowid;

                return RedirectToAction("ManageClientReviewClearDashboardInfo", new { CaseStatus = "SpocReview", cr = clsCommonMethods.PEncrypt(clientRowid.ToString()), tmr = Session["Teammemberrowid"].ToString() });
            }
            if (checkfamid == 12)
            {
                dynamic expando = new ExpandoObject();
                int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
                short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
                short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
                int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

                string CheckStatus = action;
                string ColorName = formCollection["ColorsName"].ToString();
                string Remarks = formCollection["Remarks"].ToString();
                string PriorityType = formCollection["PriorityType"].ToString();
                ClientCustomize cc = new ClientCustomize();

                try
                {
                    if (ModelState.IsValid)
                    {

                        if (clientRowid > 0 && SubCheckid > 0)
                        {
                            int checkstatusid = 0;
                            DataTable Dtstatusid = new DataTable();
                            Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                            if (Dtstatusid.Rows.Count > 0)
                            {
                                checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                            }
                            if (CheckStatus == "Reject")
                            {
                                //cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");
                                //cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //#region Save LogTransaction Details  Case/Check Action History Save
                                //clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");

                                //clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                //#endregion

                                //ModelState.Clear();
                                //return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { PersonalRowID = PersonalRowid, dispFor = "Dashboard" });

                            }
                            else
                            {
                                var marksModel = expando as IDictionary<string, object>;

                                OperationViewModel cd = new OperationViewModel();
                                cd.PersonalRowID = PersonalRowid;
                                cd.SubCheckRowID = SubCheckid;
                                cd.clientRowId = clientRowid;
                                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                                cd.moduleName = "ClientServicing";

                                ClientCustomize bl = new ClientCustomize();
                                DataTable dt = new DataTable();
                                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                                foreach (DataRow d in dt.Rows)
                                {
                                    string[] FString = d[1].ToString().Split('_');

                                    if (d[4].ToString() == "fileUpload")
                                    {
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
                                            }
                                        }
                                    }
                                    else if (d[4].ToString() == "dropdown")
                                    {
                                        if (clientRowid == 787 || clientRowid == 317)
                                        {
                                            foreach (string key in formCollection.AllKeys)
                                            {
                                                if (key == d[0].ToString())
                                                {
                                                    marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                                }
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
                                            }
                                        }
                                        if (chkselect == 0)
                                        {
                                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                        }
                                    }
                                    else
                                        marksModel.Add(d[1].ToString(), d[0].ToString());

                                }

                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 20, Remarks, "client clear Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                                cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #region Save LogTransaction Details   Case/Check Action History Save 
                                clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCheckWiseInfoBySpoc", CheckStatus, "", "Update");

                                clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                #endregion

                                ModelState.Clear();

                                return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { ClientRowID = clientRowid, PersonalRowID = PersonalRowid, dispFor = "Dashboard" });
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Invlid Employment id and model!";
                        }
                    }
                }
                catch (Exception Ex)
                {
                    ViewBag.ErrorMsg = "Invalid parameter value";
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                }
                ViewBag.PersonalRowid = PersonalRowid;
                ViewBag.SubCheckId = SubCheckid;
                ViewBag.clientRowId = clientRowid;

                return RedirectToAction("ManageClientReviewClearDashboardInfo", new { CaseStatus = "SpocReview", cr = clsCommonMethods.PEncrypt(clientRowid.ToString()), tmr = Session["Teammemberrowid"].ToString() });
            }
            if (checkfamid == 26)
            {
                dynamic expando = new ExpandoObject();
                int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());

                short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());

                short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
                int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

                string CheckStatus = action;
                string ColorName = formCollection["ColorsName"].ToString();
                string Remarks = formCollection["Remarks"].ToString();
                string PriorityType = formCollection["PriorityType"].ToString();

                try
                {
                    if (ModelState.IsValid)
                    {
                        ClientCustomize cc = new ClientCustomize();
                        if (clientRowid > 0 && SubCheckid > 0)
                        {
                            int checkstatusid = 0;
                            DataTable Dtstatusid = new DataTable();
                            Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                            if (Dtstatusid.Rows.Count > 0)
                            {
                                checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                            }
                            if (CheckStatus == "Reject")
                            {
                                //cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                //cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //#region Save LogTransaction Details  Case/Check Action History Save
                                //clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateNationalCheckVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");

                                //clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                //#endregion

                                //ModelState.Clear();
                            }
                            else
                            {
                                var marksModel = expando as IDictionary<string, object>;

                                OperationViewModel cd = new OperationViewModel();
                                cd.PersonalRowID = PersonalRowid;
                                cd.SubCheckRowID = SubCheckid;
                                cd.clientRowId = clientRowid;
                                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                                cd.moduleName = "ClientServicing";

                                ClientCustomize bl = new ClientCustomize();
                                DataTable dt = new DataTable();
                                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                                foreach (DataRow d in dt.Rows)
                                {
                                    string[] FString = d[1].ToString().Split('_');

                                    if (d[4].ToString() == "fileUpload")
                                    {
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
                                            }
                                        }
                                    }

                                    else if (d[4].ToString() == "dropdown")
                                    {
                                        if (clientRowid == 317 || clientRowid == 327)
                                        {
                                            foreach (string key in formCollection.AllKeys)
                                            {
                                                if (key == d[0].ToString())
                                                {
                                                    marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                                }
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
                                            }
                                        }
                                        if (chkselect == 0)
                                        {
                                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());

                                        }
                                    }

                                    else
                                        marksModel.Add(d[1].ToString(), d[0].ToString());

                                }
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 20, Remarks, "client clear Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                                cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #region Save LogTransaction Details Case/Check Action History Save
                                clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCheckWiseInfoBySpoc", CheckStatus, "", "Update");

                                clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                #endregion
                            }

                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Invlid Employment id and model!";
                        }
                    }
                }
                catch (Exception Ex)
                {
                    ViewBag.ErrorMsg = "Invalid parameter value";
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                }
                ViewBag.PersonalRowid = PersonalRowid;
                ViewBag.SubCheckId = SubCheckid;
                ViewBag.clientRowId = clientRowid;

                return RedirectToAction("ManageClientReviewClearDashboardInfo", new { CaseStatus = "SpocReview", cr = clsCommonMethods.PEncrypt(clientRowid.ToString()), tmr = Session["Teammemberrowid"].ToString() });
            }
            if (checkfamid == 40)
            {
                dynamic expando = new ExpandoObject();
                int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
                short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
                short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
                int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

                string CheckStatus = action;
                string ColorName = formCollection["ColorsName"].ToString();
                string Remarks = formCollection["Remarks"].ToString();
                string PriorityType = formCollection["PriorityType"].ToString();

                try
                {
                    if (ModelState.IsValid)
                    {
                        ClientCustomize cc = new ClientCustomize();
                        if (clientRowid > 0 && SubCheckid > 0)
                        {
                            int checkstatusid = 0;
                            DataTable Dtstatusid = new DataTable();
                            Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                            if (Dtstatusid.Rows.Count > 0)
                            {
                                checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                            }
                            if (CheckStatus == "Reject")
                            {
                                //cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                //cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //#region Save LogTransaction Details
                                //clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateSpecialCheckVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");
                                //#endregion

                                //#region Case/Check Action History Save
                                //clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                //#endregion

                                //ModelState.Clear();

                            }
                            else
                            {
                                var marksModel = expando as IDictionary<string, object>;

                                OperationViewModel cd = new OperationViewModel();
                                cd.PersonalRowID = PersonalRowid;
                                cd.SubCheckRowID = SubCheckid;
                                cd.clientRowId = clientRowid;
                                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                                cd.moduleName = "ClientServicing";

                                ClientCustomize bl = new ClientCustomize();
                                DataTable dt = new DataTable();
                                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                                foreach (DataRow d in dt.Rows)
                                {
                                    string[] FString = d[1].ToString().Split('_');

                                    if (d[4].ToString() == "fileUpload")
                                    {
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
                                            }
                                        }
                                    }

                                    else if (d[4].ToString() == "dropdown")
                                    {
                                        if (clientRowid == 317 || clientRowid == 327)
                                        {
                                            foreach (string key in formCollection.AllKeys)
                                            {
                                                if (key == d[0].ToString())
                                                {
                                                    marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                                }
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
                                            }
                                        }
                                        if (chkselect == 0)
                                        {
                                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                        }
                                    }
                                    else
                                        marksModel.Add(d[1].ToString(), d[0].ToString());

                                }
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 20, Remarks, "client clear Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                                cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #region Save LogTransaction Details  Case/Check Action History Save
                                clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCheckWiseInfoBySpoc", CheckStatus, "", "Update");

                                clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                #endregion
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Invlid Employment id and model!";
                        }
                    }
                }
                catch (Exception Ex)
                {
                    ViewBag.ErrorMsg = "Invalid parameter value";
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                }
                ViewBag.PersonalRowid = PersonalRowid;
                ViewBag.SubCheckId = SubCheckid;
                ViewBag.clientRowId = clientRowid;

                return RedirectToAction("ManageClientReviewClearDashboardInfo", new { CaseStatus = "SpocReview", cr = clsCommonMethods.PEncrypt(clientRowid.ToString()), tmr = Session["Teammemberrowid"].ToString() });
            }
            if (checkfamid == 5)
            {
                dynamic expando = new ExpandoObject();
                int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
                short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
                short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
                int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());
                string Remarks = formCollection["Remarks"].ToString();
                string PriorityType = formCollection["PriorityType"].ToString();
                string CheckStatus = action;
                string ColorName = formCollection["ColorsName"].ToString();
                try
                {
                    if (ModelState.IsValid)
                    {
                        ClientCustomize cc = new ClientCustomize();
                        if (clientRowid > 0 && SubCheckid > 0)
                        {
                            int checkstatusid = 0;
                            DataTable Dtstatusid = new DataTable();
                            Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                            if (Dtstatusid.Rows.Count > 0)
                            {
                                checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                            }
                            if (CheckStatus == "Reject")
                            {
                                //cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                                //cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //#region Save LogTransaction Details Case/Check Action History Save
                                //clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCriminalVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");

                                //clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                //#endregion

                                //ModelState.Clear();

                            }
                            else
                            {
                                var marksModel = expando as IDictionary<string, object>;

                                OperationViewModel cd = new OperationViewModel();
                                cd.PersonalRowID = PersonalRowid;
                                cd.SubCheckRowID = SubCheckid;
                                cd.clientRowId = clientRowid;
                                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                                cd.moduleName = "ReportQC";

                                ClientCustomize bl = new ClientCustomize();
                                DataTable dt = new DataTable();
                                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                                foreach (DataRow d in dt.Rows)
                                {
                                    string[] FString = d[1].ToString().Split('_');

                                    if (d[4].ToString() == "fileUpload")
                                    {
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
                                            }
                                        }
                                    }
                                    else if (d[4].ToString() == "dropdown")
                                    {

                                        foreach (string key in formCollection.AllKeys)
                                        {
                                            if (key == d[0].ToString())
                                            {
                                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
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
                                            }
                                        }
                                        if (chkselect == 0)
                                        {
                                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                        }
                                    }
                                    else
                                        marksModel.Add(d[1].ToString(), d[0].ToString());

                                }
                                cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 20, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 20, Remarks, "client clear Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                //  cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                                cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                #region Save LogTransaction Details Case/Check Action History Save
                                clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCheckWiseInfoBySpoc", CheckStatus, "", "Update");

                                clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);

                                #endregion
                                ViewBag.ErrorMsg = "Record Updated Successfully!.";
                            }
                        }
                        else


                        {
                            ViewBag.ErrorMsg = "Invlid Employment id and model!";
                        }
                    }
                }
                catch (Exception Ex)
                {
                    ViewBag.ErrorMsg = "Invalid parameter value";
                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                }
                ViewBag.PersonalRowid = PersonalRowid;
                ViewBag.SubCheckId = SubCheckid;
                ViewBag.clientRowId = clientRowid;

                return RedirectToAction("ManageClientReviewClearDashboardInfo", new { CaseStatus = "SpocReview", cr = clsCommonMethods.PEncrypt(clientRowid.ToString()), tmr = Session["Teammemberrowid"].ToString() });
            }

            return View();



        }
        #region ****** Insufficiency Management By Client Servicing (Spoc) ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult InsuffMgmtForClientServicing()
        {
            CreateNavigationforInsuffCaseForClientServicing();
            return View();
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult InsufficiencyCheckClearByCSSpoc(int id = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0, int PersonalRowID = 0, string CompanyRefNo = "", string InfRemarks = "")
        {
            InsuffCheckClearViewModel model = new InsuffCheckClearViewModel();
            if (model != null)
            {
                model.CheckRowID = id;
                model.ClientRowID = ClientRowId;
                model.ChFamilRowId = ChFamilRowId;
                model.PersonalRowID = PersonalRowID;
                model.SubCheckRowID = subCheckId;
                model.SecuritasID = CompanyRefNo;
                model.INFReason = InfRemarks;
                model.SubCheckName = repoSubCheck.GetSubCheckFamilyForUpdateById(subCheckId).SubCheckName;
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult InsufficiencyCheckClearByCSSpoc(InsuffCheckClearViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                    ClientCustomize cc = new ClientCustomize();
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
                                model.Remarks = "Insuff Document Uploaded By Spoc";
                            }
                        }
                        //repoPQPersonal.UpdateInsuffCheckByPeraonalIdAndSubCheckID(model.ChFamilRowId, model.CheckRowID, model.Remarks);

                        #region Save LogTransaction Details
                        clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "InsufficiencyCheckClearByClient", "Insuff Clear By Client Spoc", model.DocumentType, "Insert");
                        #endregion

                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            //43 for L1 insuff
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "Client", teamMemerRowId);//45 means L1 Insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.Remarks, "L1-Spoc Client", teamMemerRowId);
                            cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                            new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-InsuffClear");
                        }
                        else if (j == 22)
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 56, "Client", teamMemerRowId);// 56 means L2 insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 56, model.Remarks, "L2-Spoc Client", teamMemerRowId);
                            cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                            SendAlertMassageForInsuffClearMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks, "L2 Insuff", "Insuff", 0, 1);
                            new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L2-InsuffClear");
                        }

                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Client Spoc", model.Remarks);
                        #endregion

                        int PID = model.PersonalRowID;
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Please select at least one document or mention remarks!";
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
                if (model.SubCheckRowID == 990)
                {
                    ClientCustomize cc = new ClientCustomize();
                    API objApi = new API();
                    string ClientRefID = "";
                    DataTable dtcompany = cc.GetPqpersonalInfo(model.PersonalRowID, model.ClientRowID);
                    if (dtcompany.Rows.Count > 0)
                    {
                        ClientRefID = dtcompany.Rows[0]["ClientRefID"].ToString();
                    }
                    string StrData = "PersonalRowId:" + model.PersonalRowID + " ,SubCheckRowID:" + model.SubCheckRowID + " ,ClientCheckName:" + model.SubCheckName;
                    objApi.InsertAPI_AuditDetail_LTI(ClientRefID, model.ClientRowID, "Failed", "IN PROGRESS", "RequestData:" + StrData, Convert.ToString(Ex.Message), "API", Convert.ToInt32(model.SubCheckRowID));
                }

                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        #region ****** Address Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageAddressInsuffForCSSpoc(int page = 1, string sort = "", string sortDir = "asc", string Search = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string ClientRowID = "")
        {
            PQAddressInsuffListPagedModel model = new PQAddressInsuffListPagedModel();
            try
            {
                short clientid = 0;
                if (ClientRowID != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowID));
                }
                int pageSize = 20;
                ViewBag.ClientRowID = clientid;
                string spocEmail = Session["TeamMemberEmail"].ToString();
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "Address Verification";
                model = repoAddressInfo.GetAllAddressInsuffListForCSSpoc(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, clientid, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, spocEmail, 0, ClientUserRowID);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        #endregion

        #region ****** Employment Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageEmploymentInsuffForCSSpoc(int page = 1, string sort = "", string sortDir = "asc", string Search = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string ClientRowID = "")
        {
            PQEmploymentInsuffListPagedModel model = new PQEmploymentInsuffListPagedModel();
            try
            {
                short clientid = 0;
                if (ClientRowID != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowID));
                }
                int pageSize = 20;
                ViewBag.ClientRowID = clientid;
                string spocEmail = Session["TeamMemberEmail"].ToString();
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "Employment Verification";
                model = repoEmploymentInfo.GetAllEmploymentInsuffListForCSSpoc(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, clientid, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, spocEmail, 0, ClientUserRowID);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        #endregion

        #region ****** Education Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageEducationInsuffForCSSpoc(int page = 1, string sort = "", string sortDir = "asc", string Search = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string ClientRowID = "")
        {
            PQEducationInsuffListPagedModel model = new PQEducationInsuffListPagedModel();
            try
            {
                short clientid = 0;
                if (ClientRowID != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowID));
                }
                int pageSize = 20;
                ViewBag.ClientRowID = clientid;
                string spocEmail = Session["TeamMemberEmail"].ToString();
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "Education Verification";
                model = repoEducationInfo.GetAllEducationInsuffListForCSSpoc(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, clientid, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, spocEmail, 0, ClientUserRowID);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        #endregion

        #region ****** Criminal Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageCriminalInsuffForCSSpoc(int page = 1, string sort = "", string sortDir = "asc", string Search = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string ClientRowID = "")
        {
            PQCriminalInsuffListPagedModel model = new PQCriminalInsuffListPagedModel();
            try
            {
                short clientid = 0;
                if (ClientRowID != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowID));
                }
                int pageSize = 20;
                ViewBag.ClientRowID = clientid;
                string spocEmail = Session["TeamMemberEmail"].ToString();
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "Criminal Record Verification";
                model = repoCriminalInfo.GetAllCriminalInsuffListForCSSpoc(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, clientid, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, spocEmail, 0, ClientUserRowID);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        #endregion

        #region ****** NationalIdentity Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageNationalIdentityInsuffForCSSpoc(int page = 1, string sort = "", string sortDir = "asc", string Search = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string ClientRowID = "")
        {
            PQNationalIdentityInsuffListPagedModel model = new PQNationalIdentityInsuffListPagedModel();
            try
            {
                short clientid = 0;
                if (ClientRowID != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowID));
                }

                int pageSize = 20;
                ViewBag.ClientRowID = clientid;
                string spocEmail = Session["TeamMemberEmail"].ToString();
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "National Identity Check";
                model = repoNationalIdentInfo.GetAllNationalIdentityInsuffListForCSSpoc(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, clientid, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, spocEmail, 0, ClientUserRowID);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        #endregion

        #region ****** Reference Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageReferenceInsuffForCSSpoc(int page = 1, string sort = "", string sortDir = "asc", string Search = "", string RecievingDate = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string ClientRowID = "")
        {
            PQReferenceInsuffListPagedModel model = new PQReferenceInsuffListPagedModel();
            try
            {
                short clientid = 0;
                if (ClientRowID != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowID));
                }
                int pageSize = 20;
                ViewBag.ClientRowID = clientid;
                string spocEmail = Session["TeamMemberEmail"].ToString();
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "Reference Verification";
                model = repoReferenceInfo.GetAllReferenceInsuffListForCSSpoc(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, clientid, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, spocEmail, 0, ClientUserRowID);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        #endregion
        #region ****** SpecialCheck Insuff Cases ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageSpecialCheckInsuffForCSSpoc(int page = 1, string sort = "", string sortDir = "asc", string Search = "", string RecievingDate = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string ClientRowID = "")
        {
            PQSpecialCheckInsuffListPagedModel model = new PQSpecialCheckInsuffListPagedModel();
            try
            {
                short clientid = 0;
                if (ClientRowID != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(ClientRowID));
                }
                int pageSize = 20;
                ViewBag.ClientRowID = clientid;
                string spocEmail = Session["TeamMemberEmail"].ToString();
                int ClientUserRowID = Convert.ToInt32(Session["ClientUserRowID"]);
                string CreatedBy = Convert.ToString(Session["CreatedBy"]);
                string subCheckType = "Special Checks";
                model = repoSplCheckInfo.GetAllSpecialCheckInsuffListForCSSpoc(page, pageSize, sort.ToLower(), sortDir.ToLower(), subCheckType, Search, clientid, RecievingToDate, RecievingFromDate, CompletedToDate, CompletedFromDate, 0, spocEmail, 0, ClientUserRowID);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        #endregion
        #endregion

        #region ***** NonAction *****

        [NonAction]
        void BindClientListByLogInName(string SpocEmail = "")
        {
            try
            {
                if (SpocEmail == "Trimbak.deshpande@securitas-india.com")
                {
                    ViewBag.ClientList = repoDDL.GetClientListForDDLByClientSpoc(SpocEmail);
                }
                else
                {
                    List<ClientDDLViewModelEnc> list = new List<ClientDDLViewModelEnc>();
                    list = repoDDL.GetClientListForDDLByClientSpocEmailIDEnc(SpocEmail).ToList();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        list[i].ClientRowID = clsCommonMethods.PEncrypt(list[i].ClientRowID.ToString());

                    }
                    ViewBag.ClientList = list;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [NonAction]
        void BindClientListByLogInNameWithOutEncrypt(string SpocEmail = "")
        {
            try
            {
                if (SpocEmail == "Trimbak.deshpande@securitas-india.com")
                {
                    ViewBag.ClientList = repoDDL.GetClientListForDDLByClientSpoc(SpocEmail);
                }
                else
                {
                    List<ClientDDLViewModelEnc> list = new List<ClientDDLViewModelEnc>();
                    list = repoDDL.GetClientListForDDLByClientSpocEmailIDEnc(SpocEmail).ToList();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        list[i].ClientRowID = list[i].ClientRowID.ToString();
                    }
                    ViewBag.ClientList = list;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [NonAction]
        private void CreateNavigationforInsuffCaseForClientServicing()
        {
            string spocEmail = Session["TeamMemberEmail"].ToString();
            var AllChecks = repoPQPersonal.GetAllInsuffChecksCountForCSSpoc(spocEmail);
            string NavTeamMemberDE = string.Empty;
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageAddressInsuffForCSSpoc", "ClientServicing") + "'> Address (" + AllChecks.AddressCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageEmploymentInsuffForCSSpoc", "ClientServicing") + "'> Employment (" + AllChecks.EmploymentCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageEducationInsuffForCSSpoc", "ClientServicing") + "'> Education (" + AllChecks.EducationCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageCriminalInsuffForCSSpoc", "ClientServicing") + "'> Criminal (" + AllChecks.CriminalCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageNationalIdentityInsuffForCSSpoc", "ClientServicing") + "'> NationalIdentity (" + AllChecks.NatIdentityCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageReferenceInsuffForCSSpoc", "ClientServicing") + "'> Reference (" + AllChecks.ReferenceCount + ") </a></li>";
            NavTeamMemberDE += "<li><a href='" + Url.Action("ManageSpecialCheckInsuffForCSSpoc", "ClientServicing") + "'> Special Check (" + AllChecks.SpecialcheckCount + ") </a></li>";

            Session["NavLeftInsuffMenuForCS"] = NavTeamMemberDE;
        }

        private void SavePQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", HttpPostedFileBase UploadDoc = null, string remarks = "", byte NADoc = 0, string secRefId = "")
        {
            try
            {
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
                    proImg = (secRefId + "_" + SubCheckRowID.ToString() + " " + DocumentType + "_" + Timestamp.ToString()).Replace(" ", "_").Replace("-", "_") + ext;
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
                //repoPQVerifiedUploadDoc.AddPQVerifiedUploadDoc(DocModel);
                repoPQVerifiedUploadDoc.AddPQVerifiedUploadDocNew(DocModel);
                //repoPQVerifiedUploadDoc.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        [SessionExpireFilterAttribute]
        [HttpGet]
        [ValidateInput(false)]
        public PartialViewResult ClientCheckStatus(int page = 1, string CompanyRefNo = "")
        {
            ClientServiceListViewModel model = new ClientServiceListViewModel();

            try
            {
                int pageSize = clsGlobalCommon.PageSize;
                if (!string.IsNullOrEmpty(CompanyRefNo))
                {
                    model = repoDashboard.getClientCheckDetails(page, pageSize, CompanyRefNo);
                }
                else
                {
                    TempData["ErrorMsg"] = "invlid id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return PartialView("~/Views/Shared/ClientCheckStatus.cshtml", model);
        }
        [ValidateInput(false)]
        public ActionResult UploadCandidateDoc(string CompanyRefNo = "", short SubCheckRowID = 0)
        {
            TempPQCheckdataListViewModel model = new TempPQCheckdataListViewModel();
            try
            {
                //string CompanyRefNoDB = (clsCommonMethods.PDecrypt(CompanyRefNo));
                int tempperid = 0;
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();
                DataSet dscase = new DataSet();
                List<TempPQCheckdataViewModel> lmd = new List<TempPQCheckdataViewModel>();
                int ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                if (ClientRowID == 791 || ClientRowID == 898)
                {
                    DataTable dt1 = cc.GetCaseDocCandidate(CompanyRefNo);
                    if (dt1.Rows.Count > 0)
                    {
                        model.caseDocumentName = dt1.Rows[0]["FileName"].ToString();
                        model.caseDocumentPath = "";
                    }
                }
                else
                {
                    DataTable dt1 = cc.GetCaseDocCandidateAll(CompanyRefNo);
                    if (dt1.Rows.Count > 0)
                    {
                        model.caseDocumentName = dt1.Rows[0]["FileName"].ToString();
                        model.caseDocumentPath = "";
                    }
                }
                DataTable dt = cc.getCandidateCaseDocumentForLOA(CompanyRefNo);
                if (dt.Rows.Count > 0)
                {
                    model.caseDocumentName = dt.Rows[0]["DocumentName"].ToString();
                    model.caseDocumentPath = dt.Rows[0]["DocumentPath"].ToString();
                    tempperid = Convert.ToInt32(dt.Rows[0]["TempPersonalRowID"].ToString());
                    model.Pid = tempperid;

                }

                ds = cc.GetResume(CompanyRefNo);
                List<TempPQCheckdata> lm = new List<TempPQCheckdata>();
                if (ds.Tables.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToString(dr["ClientID"]) == "1324" || Convert.ToString(dr["ClientID"]) == "890")
                        {
                            lm.Add(new TempPQCheckdata
                            {

                                caseResumeDocumentName = (dr["DocumentName"]).ToString(),
                                caseResumeDocumentPath = "/Uploads/" + (dr["DocumentName"]),
                                Pid2 = Convert.ToInt32((dr["personalrow"])),


                            });
                        }
                    }
                    model.CaseDetails = lm;

                }


                //if (CompanyRefNo > 0)
                //{
                ds = cc.getTempCheckDetailsInCaseData(CompanyRefNo);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lmd.Add(new TempPQCheckdataViewModel
                    {

                        CheckName = (dr["CheckName"]).ToString(),
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        DocumentType = (dr["DocumentType"]).ToString(),
                        FileName = (dr["FileName"]).ToString(),


                    });
                }
                //}

                model.CheckDetails = lmd;

            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View(model);
        }
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult GetCandidatedoc(string FileName)
        {
            string fileName = (clsCommonMethods.PDecrypt(FileName));
            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();
            byte[] fileBytes = null;
            if (System.IO.File.Exists(Server.MapPath(FldPathDoc) + fileName))
            {
                fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FldPathDoc) + fileName);
            }
            if (fileBytes != null)
                return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
            else
            {
                return RedirectToAction("GetClientCasesDashBoardInfo", new { CaseStatus = "ReportReceived" });
            }

        }
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult GetReport(string ImageName, string CaseStatus)
        {
            DataTable ddt = new ClientCustomize().getAutoExistCompRefNo(ImageName);
            string ReportFileName = "";
            if (ddt.Rows.Count > 0)
            {
                ReportFileName = ddt.Rows[0]["ReportFileName"].ToString();
            }
            else
            {
                ReportFileName = ImageName + ".pdf";
            }
            string Designation = Session["CRPUserName"].ToString();
            if (Designation == "InvestigationMgr")
            {
                if (!string.IsNullOrEmpty(ReportFileName))
                {
                    ReportFileName = ReportFileName;
                }
                else
                {
                    ReportFileName = ImageName + ".pdf";

                }

            }
            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
            byte[] fileBytes = null;

            if (ReportFileName.Contains("24/7CPL"))
            {
                ReportFileName = ReportFileName.Replace("24/7CPL", "24-7CPL");
            }
            if (System.IO.File.Exists(Server.MapPath(FldPathDoc) + ReportFileName))
            {
                fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FldPathDoc) + ReportFileName);
            }

            if (fileBytes != null)
            {
                //return File(fileBytes, MediaTypeNames.Application.Octet, ImageName);
                ReportFileName = repoDashboardClient.downloadReportFileName(ImageName);
                return File(fileBytes, MediaTypeNames.Application.Octet, ReportFileName + ".pdf");
            }
            else
            {
                if (CaseStatus == "YTR")
                {
                    CaseStatus = "YTR Hold";
                }
                return RedirectToAction("ClientServicingDashboardInfo", new { CaseStatus = CaseStatus });
            }
        }

        public ActionResult DownloadTodayUploadReport(string ReportType)
        {
            try
            {
                string ZIPFileName = ReportType + "_Report.Zip";
                List<ClientBulkDownlode> f = new List<ClientBulkDownlode>();
                ClientCustomize CC = new ClientCustomize();
                DataTable ReportDt = CC.GetUploadReportDetailsViaReportType(ReportType);
                string path = ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
                if (ReportDt.Rows.Count > 0)
                {
                    for (int i = 0; i < ReportDt.Rows.Count; i++)
                    {
                        string ReportFileName = repoDashboardClient.downloadReportFileName(Convert.ToString(ReportDt.Rows[i]["CompanyRefNo"]), ReportType);
                        ClientBulkDownlode tt = new ClientBulkDownlode();
                        string fname = Server.MapPath(path + ReportDt.Rows[i]["ReportFileName"]);
                        FileInfo fi1 = new FileInfo(fname);
                        tt.fileDetails = fi1;
                        tt.FileDownloadName = ReportFileName + ".pdf";
                        f.Add(tt);

                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                        {
                            foreach (ClientBulkDownlode fo in f)
                            {
                                try
                                {
                                    ziparchive.CreateEntryFromFile(fo.fileDetails.FullName, fo.FileDownloadName);
                                }
                                catch (Exception Ex)
                                {
                                    ViewBag.ErrorMsg = "Invalid paramerter value";
                                    clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                                }
                            }
                        }
                        return File(memoryStream.ToArray(), "application/zip", ZIPFileName);
                    }
                }

            }
            catch (Exception Ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View();
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public void E2eTrackerForCS_clientwise(string ClientRowId, string dtf, string dtt)
        {
            try
            {
                //string ClientRowID = "";
                //if (ClientRowId != "0")
                //{
                //    ClientRowID = clsCommonMethods.PDecrypt(ClientRowId);
                //}
                var tblRowID = GetCommaSeparateClientRowID(ClientRowId);
                ClientCustomize cc = new ClientCustomize();
                string teamMemerRowId = Session["TeamMemberRowID"].ToString();
                string department = "CS";
                //if (ClientRowId == "0")
                //{
                //    ClientRowID = "";
                //}
                DataTable DT = new DataTable();
                //DT = cc.e2eTrackerForCS_clientwise(ClientRowID, teamMemerRowId, department);
                DT = cc.e2eTrackerForCS_clientwise_MultipleClient(tblRowID, teamMemerRowId, department, dtf, dtt);
                ExportToExcel(DT, "E2ETrackerforCS");
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        public void DownloadTemplate()
        {
            int ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
            string FileName = "";
            FileName = "Updated Details.xlsx";
            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["clientCanidatetemplate"].ToString();
            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Updated Details.xlsx");
            Response.TransmitFile(Server.MapPath(FldPathDoc) + FileName);
            Response.End();
        }


        private string GetCommaSeparateClientRowID(string clientRowId)
        {
            string strCommaSeparate = "";
            if (clientRowId == "")
            {
                clientRowId = "0";
            }

            string[] strClient = clientRowId.Split(',');

            for (int i = 0; i < strClient.Length; i++)
            {

                if (strClient[i].Length > 0)
                {
                    if (strClient[i] != "0")
                    {
                        strCommaSeparate += Convert.ToString(clsCommonMethods.PDecrypt(strClient[i])) + ",";
                    }
                    else
                    {
                        strCommaSeparate += Convert.ToString(0) + ",";
                    }

                }

            }
            var strResult = strCommaSeparate.Substring(0, strCommaSeparate.LastIndexOf(","));
            return strResult;
        }

        //public void ExportToExcel(DataTable dt, string strFileName)
        //{
        //    if (dt.Rows.Count > 0)
        //    {

        //        System.IO.StringWriter tw = new System.IO.StringWriter();
        //        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        //        DataGrid dgGrid = new DataGrid();
        //        dgGrid.DataSource = dt;
        //        dgGrid.DataBind();

        //        //Get the HTML for the control.
        //        dgGrid.RenderControl(hw);
        //        //Write the HTML back to the browser.
        //        //Response.ContentType = application/vnd.ms-excel;
        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName + ".xls");


        //        Response.Write(tw.ToString());
        //        Response.End();
        //    }
        //}

        public void ExportToExcel(DataTable dt, string strFileName)
        {
            if (dt.Rows.Count > 0)
            {
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                // Render the DataGrid control
                dgGrid.RenderControl(hw);

                // Prepare UTF-8 encoded bytes with meta tag
                string htmlWithMeta = @"<html><head>
<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
</head><body>" + tw.ToString() + "</body></html>";

                byte[] utfBytes = System.Text.Encoding.UTF8.GetBytes(htmlWithMeta);

                // Clear the response and set headers
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "utf-8";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName + ".xls");

                // Write UTF-8 bytes directly
                Response.BinaryWrite(utfBytes);
                Response.End();
            }
        }

        public void CandidateCasePdfLists(int id, int dpi)
        {

            string DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
            var CandidatePdfList = repoReportQC.GetCandidateCasePdfListForReport(id);
            foreach (var item in CandidatePdfList)
            {
                SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
                f.Serial = "80027423579"; //License for server 
                //f.Serial = "10273316474"; //License for development machine
                f.OpenPdf(Server.MapPath(DocPath + item.DocFile));
                if (f.PageCount > 0)
                {
                    string jpgFileName = item.DocFile.Replace(".pdf", "");
                    string ImageName = string.Empty;
                    for (int i = 1; i <= f.PageCount; i++)
                    {
                        ImageName += jpgFileName + i + ".jpg,";
                    }
                    if (!string.IsNullOrEmpty(ImageName) && ImageName.Length > 0)
                    {
                        ImageName = ImageName.Substring(0, ImageName.Length - 1);
                        //bool res = repoReportQC.UpdateCaseImageName(item.CaseDocRowID, ImageName);
                        //if (res)
                        //{
                        //int width = 644; // Width in Px
                        //int height = 800; // Height in Px
                        f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        //f.ImageOptions.Resize(new System.Drawing.Size { Width = width, Height = height }, false);
                        f.ImageOptions.Dpi = dpi;
                        f.ToImage(Server.MapPath(DocPath), jpgFileName);
                        //}
                    }
                }
            }
        }
        public void CandidatePdfLists(int id, int dpi)
        {
            string DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
            var CandidatePdfList = repoReportQC.GetCandidatePdfListForConversion(id);
            foreach (var item in CandidatePdfList)
            {
                SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
                f.Serial = "80027423579"; //License for server 
                //f.Serial = "10273316474"; //License for development machine
                f.OpenPdf(Server.MapPath(DocPath + item.DocFile));

                if (f.PageCount > 0)
                {
                    string jpgFileName = item.DocFile.Replace(".pdf", "");
                    string ImageName = string.Empty;
                    for (int i = 1; i <= f.PageCount; i++)
                    {
                        ImageName += jpgFileName + i + ".jpg,";
                    }
                    if (!string.IsNullOrEmpty(ImageName) && ImageName.Length > 0)
                    {
                        ImageName = ImageName.Substring(0, ImageName.Length - 1);
                        bool res = repoReportQC.UpdateImageName(item.VerifiedUploadRowID, ImageName);
                        if (res)
                        {
                            //int width = 644; // Width in Px
                            //int height = 911; // Height in Px
                            f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                            //f.ImageOptions.Resize(new System.Drawing.Size { Width = width, Height = height }, false);
                            f.ImageOptions.Dpi = dpi;
                            f.ToImage(Server.MapPath(DocPath), jpgFileName);
                        }
                        //cc.updateReportPDFToImagePages(item.VerifiedUploadRowID, ImageName);
                    }
                }
            }
        }

        public void PdfImage(int id = 0)
        {
            string DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
            var CandidatePdfList = repoReportQC.GetCandidatePdfListForConversion(id);
            foreach (var item in CandidatePdfList)
            {
                SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
                f.Serial = "80027423579"; //License for server 
                //f.Serial = "10273316474"; //License for development machine   
                f.OpenPdf(Server.MapPath(DocPath + item.DocFile));

                if (f.PageCount > 0)
                {
                    string jpgFileName = item.DocFile.Replace(".pdf", "");
                    string ImageName = string.Empty;
                    for (int i = 1; i <= f.PageCount; i++)
                    {
                        ImageName += jpgFileName + i + ".jpg,";
                    }
                    if (!string.IsNullOrEmpty(ImageName) && ImageName.Length > 0)
                    {
                        ImageName = ImageName.Substring(0, ImageName.Length - 1);
                        bool res = repoReportQC.UpdateImageName(item.VerifiedUploadRowID, ImageName);
                        if (res)
                        {
                            //int width = 644; // Width in Px
                            //int height = 911; // Height in Px
                            f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                            //f.ImageOptions.Resize(new System.Drawing.Size { Width = width, Height = height }, false);
                            f.ImageOptions.Dpi = 400;
                            f.ToImage(Server.MapPath(DocPath), jpgFileName);
                        }
                        //cc.updateReportPDFToImagePages(item.VerifiedUploadRowID, ImageName);
                    }
                }
            }
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ReportInterim(int id = 0, string Report_Type = "", string ReportStatus = "", string ReportPart = "", string CaseStatus = "")
        {
            InterimReportGenerateViewModel model = new InterimReportGenerateViewModel();
            try
            {
                Session["id"] = id;
                Session["Report_Type"] = Report_Type;
                string BA = "";

                DataTable DT = new DataTable();
                cc = new ClientCustomize();

                model = repoReportQC.GenerateCandidateInterimReportByReportQCTM(id);
                if (model.ClientRowID == 762)
                {
                    model.TekInfo = new GetDynamicReport();
                    DataTable dtTek = cc.GetTekSystemReportInfo(model.PersonalRowID);
                    if (dtTek.Rows.Count > 0)
                    {
                        model.TekInfo.FatherName = dtTek.Rows[0]["FatherName"].ToString();
                        model.TekInfo.VerifiedBy = dtTek.Rows[0]["VerifiedBy"].ToString();
                        model.TekInfo.VerifiedOn = dtTek.Rows[0]["VerifiedOn"].ToString();
                        model.TekInfo.UniqueId = dtTek.Rows[0]["UniqueId"].ToString();
                        model.TekInfo.ReportComment = dtTek.Rows[0]["ReportComment"].ToString();
                        model.TekInfo.CreatedBy = dtTek.Rows[0]["CreatedBy"].ToString();
                        model.TekInfo.DOB = dtTek.Rows[0]["DOB"].ToString();
                    }


                }
                //get resolution for client 
                int resolutionDPI = cc.getResolutionForReport(model.ClientRowID);
                CandidatePdfLists(id, resolutionDPI, model.ClientRowID);
                //CandidatePdfListsNew(id);
                // CandidateCasePdfListsNew(id);
                CandidateCasePdfLists(id, resolutionDPI);
                Session["SecritadRefId"] = model.CompanyRefNo;
                model.CandidateCaseDocList = repoReportQC.GetCandidateCaseDocListForReport(id);
                model.ReportDate = DateTime.Now;
                model.Report_Type = Report_Type;
                model.DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                model.DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                if (model.ClientRowID == 762)
                {
                    model.CandidateDocList = repoReportQC.GetCandidateDocListForReportArtifactWithApprove(id);
                    var list = model.CandidateDocList.Where(T => T.SubCheckRowID == 205).ToList();
                    foreach (var a in list)
                    {
                        if (a.DocumentType.ToUpper() == "OFAC REPORT")
                        {
                            a.ArtifactOrder = 1;
                        }
                        else if (a.DocumentType.ToUpper() == "ID  REPORT" || a.DocumentType.Contains("ID") || a.DocumentType.Contains("Id"))
                        {
                            a.ArtifactOrder = 4;
                        }
                        else if (a.DocumentType.ToUpper() == "OFAC KEYWORD")
                        {
                            a.ArtifactOrder = 2;
                        }
                        else if (a.DocumentType.Contains("TGS EXPORT") || a.DocumentType.Contains("TGS Export"))
                        {
                            a.ArtifactOrder = 3;
                        }
                    }
                    model.CandidateDocList = list.OrderBy(T => T.ArtifactOrder);

                }
                else
                {
                    model.CandidateDocList = repoReportQC.GetCandidateDocListForReport(id);
                }

                model.ReportPart = ReportPart;


                DT = cc.GetColorCode(model.PersonalRowID, model.ClientRowID);
                if (DT.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DT.Rows[0]["colorName"].ToString()))
                    {
                        model.FinalColor = DT.Rows[0]["colorName"].ToString();
                        model.FinalColorCode = DT.Rows[0]["colorCode"].ToString();
                    }
                    else
                    {
                        model.FinalColorCode = "#fff";
                    }
                }
                if (model.ClientRowID == 3)
                {

                    model.PersonalChecksBriefDetails = repoReportQC.GetCandidateChecksBriefDetails_TCSByPersonalId(id);

                    DataTable dtpenalty = cc.GetPenaltydat(id);
                    if (dtpenalty.Rows.Count > 0)
                    {
                        model.PenaltyAmount = Convert.ToDecimal(dtpenalty.Rows[0]["PenaltyAmount"].ToString());
                        model.LocationTCS = dtpenalty.Rows[0]["Location"].ToString();
                    }
                    model.TCS_BPO_Cal_Sheet = repoReportQC.GetTCS_calSheet(id);
                    model.TCS_BaseAmount = model.TCS_BPO_Cal_Sheet.Sum(T => T.Charge);
                    model.TCS_Total_Amount = model.TCS_BaseAmount + Convert.ToDouble(model.PenaltyAmount);



                    //var BriefDetailslist = model.PersonalChecksBriefDetails.Where(t => t.SubCheckDetail != "Not Applicable").ToList();
                    //BriefDetailslist.Remove(BriefDetailslist.First(t => t.SubCheckDetail == "Not Applicable"));
                    //model.PersonalChecksBriefDetails = BriefDetailslist;

                    // Address Verification
                    if (!string.IsNullOrEmpty(model.EmploymentId))
                    {
                        string loc = model.EmploymentId.Substring(0, 2);
                        if (loc == "HD")
                        {
                            model.CalsheetLocation = "Hyderabad";
                        }
                        else if (loc == "NM")
                        {
                            model.CalsheetLocation = "Nagpur";
                        }
                        else if (loc == "DG")
                        {
                            model.CalsheetLocation = "Delhi";
                        }
                        else if (loc == "MN")
                        {
                            model.CalsheetLocation = "Mumbai";
                        }
                        else if (loc == "BT")
                        {
                            model.CalsheetLocation = "Bangalore";
                        }
                        else if (loc == "CS")
                        {
                            model.CalsheetLocation = "Chennai";
                        }
                        else if (loc == "PS")
                        {
                            model.CalsheetLocation = "Pune";
                        }
                        else if (loc == "NI")
                        {
                            model.CalsheetLocation = "Nashik";
                        }
                        else if (loc == "XD")
                        {
                            model.CalsheetLocation = "Delhi";
                        }
                        else if (loc == "XM")
                        {
                            model.CalsheetLocation = "Mumbai";
                        }
                        else if (loc == "XC")
                        {
                            model.CalsheetLocation = "Chennai";
                        }
                        else if (loc == "XH")
                        {
                            model.CalsheetLocation = "Hyderabad";
                        }


                    }
                    else
                    {
                        model.CalsheetLocation = "";
                    }
                }
                if (model.ClientRowID == 268)
                {

                    string[] location = model.ClientRefID.Split('/');
                    if (location != null)
                    {
                        BA = location[0];
                        if (location.Length > 2)
                        {
                            model.ApplicationId = location[1];
                        }
                        if (location.Length == 3)
                        {
                            model.Location = location[2];
                        }

                        // model.VerticalLocation = BA;

                        if (BA.Trim() == "BA")
                        {
                            model.VerticalLocation = "Business Analyst";
                        }
                        else if (BA == "Direct Trainee")
                        {
                            model.VerticalLocation = "Direct Trainee";
                        }
                        else if (BA == "Experience Professional")
                        {
                            model.VerticalLocation = "Experience Professional";
                        }
                        else if (BA == "CampusTrainee")
                        {
                            model.VerticalLocation = "CampusTrainee";
                        }



                        if (BA == "BA")
                        {
                            model.PersonalChecksBriefDetails = repoReportQC.GetCandidateChecksBriefDetails_TCS_IT_ByPersonalId(id);
                        }
                        else if (BA == "CampusTrainee")
                        {
                            List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();
                            lmd = repoReportQC.GetCandidateChecksBriefDetails_TCS_IT_ByPersonalId(id).ToList();



                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "Candidate hired from TCS accredited Institute/College/University Hence Education check not part of scope",
                                CheckFName = "EDUCATION CHECK",
                                subCheckId = 56,
                                SubCheckFName = "EDUCATION QUALIFICATION",
                                ColorName = "NA"
                            });

                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "Candidate hired as Trainee Hence Employment Check not part of scope",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 68,
                                SubCheckFName = "CURRENT / IMMEDIATE PAST EMPLOYMENT",
                                ColorName = "NA"
                            });



                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "-",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 69,
                                SubCheckFName = "PREVIOUS EMPLOYMENT I",
                                ColorName = "-"
                            });

                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "-",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 70,
                                SubCheckFName = "PREVIOUS EMPLOYMENT II",
                                ColorName = "-"
                            });

                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "-",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 71,
                                SubCheckFName = "PREVIOUS EMPLOYMENT III",
                                ColorName = "-"
                            });

                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "-",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 72,
                                SubCheckFName = "PREVIOUS EMPLOYMENT IV",
                                ColorName = "-"
                            });


                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "-",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 73,
                                SubCheckFName = "PREVIOUS EMPLOYMENT V",
                                ColorName = "-"
                            });
                            model.PersonalChecksBriefDetails = lmd;


                            //model.PersonalChecksBriefDetails

                        }

                        else if (BA == "Direct Trainee")
                        {
                            List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();
                            lmd = repoReportQC.GetCandidateChecksBriefDetails_TCS_IT_ByPersonalId(id).ToList();



                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "Candidate hired as Trainee Hence Employment Check not part of scope",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 68,
                                SubCheckFName = "CURRENT / IMMEDIATE PAST EMPLOYMENT",
                                ColorName = "NA"
                            });



                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "-",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 69,
                                SubCheckFName = "PREVIOUS EMPLOYMENT I",
                                ColorName = "-"
                            });

                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "-",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 70,
                                SubCheckFName = "PREVIOUS EMPLOYMENT II",
                                ColorName = "-"
                            });

                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "-",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 71,
                                SubCheckFName = "PREVIOUS EMPLOYMENT III",
                                ColorName = "-"
                            });

                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "-",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 72,
                                SubCheckFName = "PREVIOUS EMPLOYMENT IV",
                                ColorName = "-"
                            });


                            lmd.Add(new PersonalCheckBriefDetailsViewModel
                            {
                                SubCheckDetail = "-",
                                CheckFName = "EMPLOYMENT CHECK",
                                subCheckId = 73,
                                SubCheckFName = "PREVIOUS EMPLOYMENT V",
                                ColorName = "-"
                            });

                            model.PersonalChecksBriefDetails = lmd;

                        }
                        else
                        {
                            model.PersonalChecksBriefDetails = repoReportQC.GetCandidateChecksBriefDetails_TCS_IT_ByPersonalId(id);

                        }



                        //var BriefDetailslist = model.PersonalChecksBriefDetails.Where(t => t.SubCheckDetail != "Not Applicable").ToList();
                        //BriefDetailslist.Remove(BriefDetailslist.First(t => t.SubCheckDetail == "Not Applicable"));
                        //model.PersonalChecksBriefDetails = BriefDetailslist;
                    }



                }
                else if (model.ClientRowID == 317 || model.ClientRowID == 327)
                {
                    model.WiproChecklist = repoReportQC.GetCheckSOW(id);
                    DataTable dtpenalty = cc.GetPenaltydat(id);
                    if (dtpenalty.Rows.Count > 0)
                    {
                        model.PenaltyAmount = Convert.ToDecimal(dtpenalty.Rows[0]["PenaltyAmount"].ToString());
                        model.LocationTCS = dtpenalty.Rows[0]["Location"].ToString();
                        model.SOW = dtpenalty.Rows[0]["SOW"].ToString();
                    }
                    if (ReportStatus == "BGV2")
                    {
                        model.PersonalChecksBriefDetails = repoReportQC.GetCandidateChecksBriefDetails_wipro_BGV2_ByPersonalId(id);
                    }
                    else
                    {
                        model.PersonalChecksBriefDetails = repoReportQC.GetCandidateChecksBriefDetails_wipro_ByPersonalId1(id);
                    }
                }
                else if (model.ClientRowID == 8 || model.ClientRowID == 8)
                {
                    model.PersonalChecksBriefDetails = repoReportQC.GetCandidateChecksBriefDetails_CventByPersonalId(id);
                }
                else if (model.ClientRowID == 446 || model.ClientRowID == 446)
                {
                    model.PersonalChecksBriefDetails = repoReportQC.GetCandidateChecksBriefDetailsByPersonalIdTechM(id);
                }
                else
                {
                    model.PersonalChecksBriefDetails = repoReportQC.GetCandidateChecksBriefDetailsByPersonalId1(id);
                }
                if (model.ClientRowID == 80)
                {
                    List<PersonalCheckBriefDetailsViewModel> oo = new List<PersonalCheckBriefDetailsViewModel>();
                    oo = model.PersonalChecksBriefDetails.Where(T => T.subCheckId == 200 || T.subCheckId == 201 || T.subCheckId == 202 || T.subCheckId == 227).ToList();
                    foreach (var x in oo)
                    {
                        if (x.ColorName == "Green")
                        {
                            oo.ForEach(p =>
                            {
                                p.SubCheckDetail = "No Record Found";
                            });

                        }
                        else
                        {
                            oo.ForEach(p =>
                            {
                                p.SubCheckDetail = "Record Found";
                            });
                        }
                    }



                }


                //model.AddressCheckList = repoAddressVer.GetAddressInfoForTMQC(id);

                #region ***** All Checks *****

                var AddressProvidedList = repoReportQC.GetProvidedAddressForReportQC(id);
                var AddressVerifiedList = repoReportQC.GetVerifiedAddressForReportQC(id);
                model.AntecedentList = repoReportQC.GetAntecedentVerList(model.ClientRowID);

                var EmploymentProvidedList = repoReportQC.GetProvidedEmploymentForReportQC(id);
                var EmploymentVerifiedList = repoReportQC.GetVerifiedEmploymentForReportQC(id);

                var EducationProvidedList = repoReportQC.GetProvidedEducationForReportQC(id);
                var EducationVerifiedList = repoReportQC.GetVerifiedEducationForReportQC(id);

                var CriminalProvidedList = repoReportQC.GetProvidedCriminalForReportQC(id);
                var CriminalVerifiedList = repoReportQC.GetVerifiedCriminalForReportQC(id);

                var CriminalProvidedListTCS = repoReportQC.GetProvidedCriminalForReportQCTCS(id);
                var CriminalVerifiedListTCS = repoReportQC.GetVerifiedCriminalForReportQCTCS(id);

                var CriminalProvidedListTCSCourt = repoReportQC.GetProvidedCriminalForReportQCTCSCourt(id);
                var CriminalVerifiedListTCSCourt = repoReportQC.GetVerifiedCriminalForReportQCTCSCourt(id);

                var ReferenceProvidedList = repoReportQC.GetProvidedReferenceForReportQC(id);
                var ReferenceVerifiedList = repoReportQC.GetVerifiedReferenceForReportQC(id);

                var NationalIdentityProvidedList = repoReportQC.GetProvidedNationalIdentityForReportQC(id);
                var NationalIdentityVerifiedList = repoReportQC.GetVerifiedNationalIdentityForReportQC(id);

                var SpecialCheckProvidedListDurgTechM = repoReportQC.GetProvidedSpecialCheckForReportQCDurgTechM(id);
                var SpecialCheckVerifiedListDurgTechM = repoReportQC.GetVerifiedSpecialCheckForReportQCDurgTechM(id);

                var SpecialCheckProvidedListTechM = repoReportQC.GetProvidedSpecialCheckForReportQCTechM(id);
                var SpecialCheckVerifiedListTechM = repoReportQC.GetVerifiedSpecialCheckForReportQCTechM(id);

                var SpecialCheckProvidedList = repoReportQC.GetProvidedSpecialCheckForReportQC(id);
                var SpecialCheckVerifiedList = repoReportQC.GetVerifiedSpecialCheckForReportQC(id);
                if (model.ClientRowID == 3)
                {
                    model.colorCodeOrderinglist = repoReportQC.getCheckColorCodeOrderingTCS(id);

                }
                else if (model.ClientRowID == 446)
                {
                    model.colorCodeOrderinglist = repoReportQC.getCheckColorCodeOrderingTechM(id);

                }

                else if (model.ClientRowID == 268)
                {
                    model.colorCodeOrderinglist = repoReportQC.getCheckColorCodeOrderingTCS_IT(id);

                }

                else
                {
                    model.colorCodeOrderinglist = repoReportQC.getCheckColorCodeOrdering(id);

                }

                AddresssReportViewModel AddressList = new AddresssReportViewModel();
                AddresssReportViewModel AddressVerList = new AddresssReportViewModel();

                EmploymentReportViewModel EmployeeList = new EmploymentReportViewModel();
                EmploymentReportViewModel EmployeeVerList = new EmploymentReportViewModel();

                EducationReportViewModel EducationList = new EducationReportViewModel();
                EducationReportViewModel EducationVerList = new EducationReportViewModel();

                CriminalReportViewModel CriminalList = new CriminalReportViewModel();
                CriminalReportViewModel CriminalVerList = new CriminalReportViewModel();

                CriminalReportViewModel CriminalListTCS = new CriminalReportViewModel();
                CriminalReportViewModel CriminalVerListTCS = new CriminalReportViewModel();

                CriminalReportViewModel CriminalListTCSCourt = new CriminalReportViewModel();
                CriminalReportViewModel CriminalVerListTCSCourt = new CriminalReportViewModel();


                ReferenceReportViewModel ReferenceList = new ReferenceReportViewModel();
                ReferenceReportViewModel ReferenceVerList = new ReferenceReportViewModel();

                NationalIdentityReportViewModel NationalList = new NationalIdentityReportViewModel();
                NationalIdentityReportViewModel NationalVerList = new NationalIdentityReportViewModel();

                SpecialCheckReportViewModel SpecialList = new SpecialCheckReportViewModel();
                SpecialCheckReportViewModel SpecialVerList = new SpecialCheckReportViewModel();

                SpecialCheckReportViewModel SpecialListDurgTechM = new SpecialCheckReportViewModel();
                SpecialCheckReportViewModel SpecialVerListDurgTechM = new SpecialCheckReportViewModel();
                SpecialCheckReportViewModel SpecialListTechM = new SpecialCheckReportViewModel();
                SpecialCheckReportViewModel SpecialVerListTechM = new SpecialCheckReportViewModel();



                foreach (var x in model.colorCodeOrderinglist)
                {
                    AddressList = AddressProvidedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    AddressVerList = AddressVerifiedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    EmployeeList = EmploymentProvidedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    EmployeeVerList = EmploymentVerifiedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    EducationList = EducationProvidedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    EducationVerList = EducationVerifiedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);

                    CriminalList = CriminalProvidedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    CriminalVerList = CriminalVerifiedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    CriminalListTCS = CriminalProvidedListTCS.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    CriminalVerListTCS = CriminalVerifiedListTCS.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);

                    CriminalListTCSCourt = CriminalProvidedListTCSCourt.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    CriminalVerListTCSCourt = CriminalVerifiedListTCSCourt.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);

                    ReferenceList = ReferenceProvidedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    ReferenceVerList = ReferenceVerifiedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);

                    NationalList = NationalIdentityProvidedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    NationalVerList = NationalIdentityVerifiedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);

                    SpecialList = SpecialCheckProvidedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    SpecialVerList = SpecialCheckVerifiedList.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);


                    SpecialListDurgTechM = SpecialCheckProvidedListDurgTechM.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    SpecialVerListDurgTechM = SpecialCheckVerifiedListDurgTechM.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);

                    SpecialListTechM = SpecialCheckProvidedListTechM.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    SpecialVerListTechM = SpecialCheckVerifiedListTechM.FirstOrDefault(d => d.SubCheckRowID == x.SubCheckFamilyID);
                    if (AddressList != null)
                    {
                        if (model.ClientRowID == 268)
                        {
                            if (AddressList != null && AddressList.SubCheckRowID != 0)
                            {
                                if (AddressList.SubCheckRowID == 2)
                                {
                                    AddressList.SubCheckName = "ADDRESS VERIFICATION (Present Address)";
                                }
                                if (AddressList.SubCheckRowID == 13)
                                {
                                    AddressList.SubCheckName = "ADDRESS VERIFICATION (Permanent Address)";
                                }

                            }
                        }
                        AddressList.CheckStatus = x.checkstatus;
                        AddressList.checkOrdering = x.subcheckSequence;
                    }
                    if (AddressVerList != null)
                    {
                        AddressVerList.CheckStatus = x.checkstatus;
                        AddressVerList.checkOrdering = x.subcheckSequence;
                        AddressVerList.ColorCode = x.ColorCode;
                        AddressVerList.ColorName = x.Color;
                        if (model.ClientRowID == 268)
                        {
                            var docList = model.CandidateDocList.Where(t => t.SubCheckRowID == AddressVerList.SubCheckRowID && t.DocumentType == "Signature").Select(T => T.SubCheckRowID).FirstOrDefault().ToString();
                            if (docList != "")
                            {
                                AddressVerList.ATA_Photograph = "Yes";
                            }
                        }

                        if (!string.IsNullOrEmpty(x.reportComment))
                            AddressVerList.ReportComment = x.reportComment;
                    }

                    if (EmployeeList != null)
                    {
                        if (model.ClientRowID == 268)
                        {
                            if (EmployeeList != null && EmployeeList.SubCheckRowID != 0)
                            {
                                if (EmployeeList.SubCheckRowID == 68)
                                {
                                    EmployeeList.SubCheckName = "EMPLOYMENT VERIFICATION - CURRENT  EMPLOYMENT";
                                }
                                else if (EmployeeList.SubCheckRowID == 69)
                                {
                                    EmployeeList.SubCheckName = "EMPLOYMENT VERIFICATION - PREVIOUS  EMPLOYMENT-I";
                                }
                                else if (EmployeeList.SubCheckRowID == 70)
                                {
                                    EmployeeList.SubCheckName = "EMPLOYMENT VERIFICATION - PREVIOUS  EMPLOYMENT-II";
                                }
                                else if (EmployeeList.SubCheckRowID == 71)
                                {
                                    EmployeeList.SubCheckName = "EMPLOYMENT VERIFICATION - PREVIOUS  EMPLOYMENT-III";
                                }
                                else if (EmployeeList.SubCheckRowID == 72)
                                {
                                    EmployeeList.SubCheckName = "EMPLOYMENT VERIFICATION - PREVIOUS  EMPLOYMENT-IV";
                                }
                                else if (EmployeeList.SubCheckRowID == 73)
                                {
                                    EmployeeList.SubCheckName = "EMPLOYMENT VERIFICATION - PREVIOUS  EMPLOYMENT-V";
                                }


                            }
                        }


                        EmployeeList.CheckStatus = x.checkstatus;
                        EmployeeList.checkOrdering = x.subcheckSequence;
                    }
                    if (EmployeeVerList != null)
                    {
                        EmployeeVerList.CheckStatus = x.checkstatus;
                        EmployeeVerList.checkOrdering = x.subcheckSequence;
                        EmployeeVerList.ColorCode = x.ColorCode;
                        EmployeeVerList.ColorName = x.Color;
                        if (!string.IsNullOrEmpty(x.reportComment))
                            EmployeeVerList.ReportComment = x.reportComment;
                    }


                    if (EducationList != null)
                    {
                        if (model.ClientRowID == 268)
                        {
                            if (EducationList.SubCheckRowID == 56)
                            {
                                EducationList.SubCheckName = "EDUCATION CHECK";
                            }
                        }
                        EducationList.CheckStatus = x.checkstatus;
                        EducationList.checkOrdering = x.subcheckSequence;
                    }
                    if (EducationVerList != null)
                    {
                        EducationVerList.CheckStatus = x.checkstatus;
                        EducationVerList.checkOrdering = x.subcheckSequence;
                        EducationVerList.ColorCode = x.ColorCode;
                        EducationVerList.ColorName = x.Color;
                        if (!string.IsNullOrEmpty(x.reportComment))
                            EducationVerList.ReportComment = x.reportComment;
                    }

                    if (NationalList != null)
                    {
                        if (model.ClientRowID == 3)
                        {
                            if (NationalList != null && NationalList.SubCheckRowID != 0)
                            {
                                if (NationalList.SubCheckRowID == 136)
                                {
                                    NationalList.SubCheckName = "ID Verification";

                                }


                            }
                        }
                    }


                    if (CriminalList != null)
                    {

                        if (model.ClientRowID == 268)
                        {
                            if (CriminalList != null && CriminalList.SubCheckRowID != 0)
                            {
                                if (CriminalList.SubCheckRowID == 24)
                                {
                                    CriminalList.SubCheckName = "POLICE VERIFICATION (Present Address)";
                                }
                                if (CriminalList.SubCheckRowID == 197)
                                {
                                    CriminalList.SubCheckName = "POLICE VERIFICATION (At the address of longest stay in the last 7 yrs.)";
                                }
                                if (CriminalList.SubCheckRowID == 210)
                                {
                                    CriminalList.SubCheckName = "POLICE VERIFICATION (Present Address)";
                                }
                            }
                        }
                        CriminalList.CheckStatus = x.checkstatus;
                        CriminalList.checkOrdering = x.subcheckSequence;

                    }
                    if (CriminalVerList != null)
                    {
                        CriminalVerList.CheckStatus = x.checkstatus;
                        CriminalVerList.checkOrdering = x.subcheckSequence;
                        CriminalVerList.ColorCode = x.ColorCode;
                        CriminalVerList.ColorName = x.Color;
                        if (!string.IsNullOrEmpty(x.reportComment))
                            CriminalVerList.ReportComment = x.reportComment;
                    }
                    if (CriminalListTCS != null)
                    {

                        CriminalListTCS.CheckStatus = x.checkstatus;
                        CriminalListTCS.checkOrdering = x.subcheckSequence;

                    }
                    if (CriminalVerListTCS != null)
                    {
                        CriminalVerListTCS.CheckStatus = x.checkstatus;
                        CriminalVerListTCS.checkOrdering = x.subcheckSequence;
                        CriminalVerListTCS.ColorCode = x.ColorCode;
                        CriminalVerListTCS.ColorName = x.Color;
                        if (!string.IsNullOrEmpty(x.reportComment))
                            CriminalVerListTCS.ReportComment = x.reportComment;
                    }

                    if (CriminalListTCSCourt != null)
                    {

                        CriminalListTCSCourt.CheckStatus = x.checkstatus;
                        CriminalListTCSCourt.checkOrdering = x.subcheckSequence;

                    }
                    if (CriminalVerListTCSCourt != null)
                    {
                        CriminalVerListTCSCourt.CheckStatus = x.checkstatus;
                        CriminalVerListTCSCourt.checkOrdering = x.subcheckSequence;
                        CriminalVerListTCSCourt.ColorCode = x.ColorCode;
                        CriminalVerListTCSCourt.ColorName = x.Color;
                        if (!string.IsNullOrEmpty(x.reportComment))
                            CriminalVerListTCSCourt.ReportComment = x.reportComment;
                    }

                    if (ReferenceList != null)
                    {
                        ReferenceList.CheckStatus = x.checkstatus;
                        ReferenceList.checkOrdering = x.subcheckSequence;
                    }
                    if (ReferenceVerList != null)
                    {
                        ReferenceVerList.CheckStatus = x.checkstatus;
                        ReferenceVerList.checkOrdering = x.subcheckSequence;
                        ReferenceVerList.ColorCode = x.ColorCode;
                        ReferenceVerList.ColorName = x.Color;
                        if (!string.IsNullOrEmpty(x.reportComment))
                            ReferenceVerList.ReportComment = x.reportComment;
                    }


                    if (NationalList != null)
                    {
                        NationalList.CheckStatus = x.checkstatus;
                        NationalList.checkOrdering = x.subcheckSequence;
                    }
                    if (NationalVerList != null)
                    {
                        NationalVerList.CheckStatus = x.checkstatus;
                        NationalVerList.checkOrdering = x.subcheckSequence;
                        NationalVerList.ColorCode = x.ColorCode;
                        NationalVerList.ColorName = x.Color;
                        if (!string.IsNullOrEmpty(x.reportComment))
                            NationalVerList.ReportComment = x.reportComment;
                    }


                    if (SpecialList != null)
                    {
                        SpecialList.CheckStatus = x.checkstatus;
                        SpecialList.checkOrdering = x.subcheckSequence;
                    }
                    if (SpecialVerList != null)
                    {
                        SpecialVerList.CheckStatus = x.checkstatus;
                        SpecialVerList.checkOrdering = x.subcheckSequence;
                        SpecialVerList.ColorCode = x.ColorCode;
                        SpecialVerList.ColorName = x.Color;
                        if (!string.IsNullOrEmpty(x.reportComment))
                            SpecialVerList.ReportComment = x.reportComment;
                    }
                    if (SpecialListDurgTechM != null)
                    {
                        SpecialListDurgTechM.CheckStatus = x.checkstatus;
                        SpecialListDurgTechM.checkOrdering = x.subcheckSequence;
                    }
                    if (SpecialVerListDurgTechM != null)
                    {
                        SpecialVerListDurgTechM.CheckStatus = x.checkstatus;
                        SpecialVerListDurgTechM.checkOrdering = x.subcheckSequence;
                        SpecialVerListDurgTechM.ColorCode = x.ColorCode;
                        SpecialVerListDurgTechM.ColorName = x.Color;
                        if (!string.IsNullOrEmpty(x.reportComment))
                            SpecialVerListDurgTechM.ReportComment = x.reportComment;
                    }
                    if (SpecialListTechM != null)
                    {
                        SpecialListTechM.CheckStatus = x.checkstatus;
                        SpecialListTechM.checkOrdering = x.subcheckSequence;
                    }
                    if (SpecialVerListTechM != null)
                    {
                        SpecialVerListTechM.CheckStatus = x.checkstatus;
                        SpecialVerListTechM.checkOrdering = x.subcheckSequence;
                        SpecialVerListTechM.ColorCode = x.ColorCode;
                        SpecialVerList.ColorName = x.Color;
                        if (!string.IsNullOrEmpty(x.reportComment))
                            SpecialVerListTechM.ReportComment = x.reportComment;
                    }

                    if (model.ClientRowID == 3)
                    {










                        if (CriminalListTCSCourt != null && CriminalVerListTCSCourt != null)
                        {

                            if (CriminalListTCSCourt.SubCheckRowID != 0 && CriminalVerListTCSCourt.CriminalRowID != 0)
                            {
                                DataTable dtcri = cc.getcheckcloserdate(CriminalListTCSCourt.PersonalRowID, CriminalListTCSCourt.SubCheckRowID);
                                if (dtcri.Rows.Count > 0)
                                {
                                    model.Checkclosedate = Convert.ToDateTime(dtcri.Rows[0]["updatedDate"]);
                                }
                            }
                        }

                    }

                }
                //MsgList.Where(x => !SentList.Any(y => y.MsgID == x.MsgID))

                model.AddressProvidedList = AddressProvidedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.AddressVerifiedList = AddressVerifiedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                model.EmploymentProvidedList = EmploymentProvidedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.EmploymentVerifiedList = EmploymentVerifiedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                model.EducationProvidedList = EducationProvidedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.EducationVerifiedList = EducationVerifiedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                model.CriminalProvidedList = CriminalProvidedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.CriminalVerifiedList = CriminalVerifiedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                model.CriminalProvidedListTCS = CriminalProvidedListTCS.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.CriminalVerifiedListTCS = CriminalVerifiedListTCS.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                model.CriminalProvidedListTCSCourt = CriminalProvidedListTCSCourt.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.CriminalVerifiedListTCSCourt = CriminalVerifiedListTCSCourt.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                model.ReferenceProvidedList = ReferenceProvidedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.ReferenceVerifiedList = ReferenceVerifiedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                model.NationalIdentityProvidedList = NationalIdentityProvidedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.NationalIdentityVerifiedList = NationalIdentityVerifiedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                model.SpecialCheckProvidedList = SpecialCheckProvidedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.SpecialCheckVerifiedList = SpecialCheckVerifiedList.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                model.SpecialCheckProvidedListDurgTechM = SpecialCheckProvidedListDurgTechM.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.SpecialCheckVerifiedListDurgTechM = SpecialCheckVerifiedListDurgTechM.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();

                model.SpecialCheckProvidedListTechM = SpecialCheckProvidedListTechM.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                model.SpecialCheckVerifiedListTechM = SpecialCheckVerifiedListTechM.Where(T => model.colorCodeOrderinglist.Any(k => k.SubCheckFamilyID == T.SubCheckRowID)).OrderBy(T => T.checkOrdering).ToList();
                //if (model.ClientRowID == 446)
                //{
                //    DataTable dtreportType = cc.GetReportType(id);
                //    if (dtreportType.Rows.Count > 0)
                //    {
                //        model.ReportTypeBYSystum = dtreportType.Rows[0]["ReportType"].ToString();             
                //    }


                //}

                if (model.ClientRowID == 8)
                {
                    DataTable dtpackage = cc.GetPakegename(model.PersonalRowID);
                    model.Location = dtpackage.Rows[0]["packageName"].ToString();
                    //}
                    //else
                    //{
                    //    model.Location = "Fresher";
                    //}

                }

                if (model.ClientRowID == 3)
                {
                    if (model.EmploymentVerifiedList.Count() > 0)
                    {
                        model.Location = "Experience";
                    }
                    else
                    {
                        model.Location = "Fresher";
                    }

                }
                BindColorCode(model.ClientRowID);

                if (model.ClientRowID == 317 || model.ClientRowID == 327)
                {
                    if (ReportStatus == "BGV2")
                    {
                        model.AddressProvidedList = null;
                        model.AddressVerifiedList = null;
                        model.CriminalProvidedList = null;
                        model.CriminalVerifiedList = null;
                        model.SpecialCheckProvidedList = null;
                        model.SpecialCheckVerifiedList = null;
                        model.ReferenceProvidedList = null;
                        model.ReferenceVerifiedList = null;
                        model.NationalIdentityProvidedList = null;
                        model.NationalIdentityVerifiedList = null;
                        model.EducationProvidedList = null;
                        model.EducationVerifiedList = null;
                        var EMPProvidedList = model.EmploymentProvidedList.Where(t => t.SubCheckRowID == 68).ToList();
                        var EMPProvidedVList = model.EmploymentVerifiedList.Where(t => t.SubCheckRowID == 68).ToList();
                        model.EmploymentProvidedList = null;
                        model.EmploymentVerifiedList = null;
                        model.EmploymentProvidedList = EMPProvidedList;
                        model.EmploymentVerifiedList = EMPProvidedVList;
                    }
                }
                if (model.ClientRowID == 268)
                {

                    var AddProvidedList = model.AddressProvidedList.Where(t => t.CheckStatus != "Not Applicable").ToList();
                    model.AddressProvidedList = null;
                    model.AddressProvidedList = AddProvidedList;

                    var EMPProvidedList = model.EmploymentProvidedList.Where(t => t.CheckStatus != "Not Applicable").ToList();
                    model.EmploymentProvidedList = null;
                    model.EmploymentProvidedList = EMPProvidedList;


                    var CRIProvidedList = model.CriminalProvidedList.Where(t => t.CheckStatus != "Not Applicable").ToList();
                    model.CriminalProvidedList = null;
                    model.CriminalProvidedList = CRIProvidedList;

                }

                #endregion
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            Session["Type"] = "";
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ReportMain(int PersonalRowID = 0, int ClientRowId = 0, string Report_Type = "", string ReportStatus = "", string ReportPart = "", string CaseStatus = "")
        {

            ReportViewModel model = new ReportViewModel();
            ClientCustomize cc = new ClientCustomize();

            model = repoReportQC.GetReportPersonalInfo(PersonalRowID);
            if (model.ClientRowID == 762)
            {
                model.TekInfo = new GetDynamicReport();
                DataTable dtTek = cc.GetTekSystemReportInfo(model.PersonalRowID);
                if (dtTek.Rows.Count > 0)
                {
                    model.TekInfo.FatherName = dtTek.Rows[0]["FatherName"].ToString();
                    model.TekInfo.VerifiedBy = dtTek.Rows[0]["VerifiedBy"].ToString();
                    model.TekInfo.VerifiedOn = dtTek.Rows[0]["VerifiedOn"].ToString();
                    model.TekInfo.UniqueId = dtTek.Rows[0]["UniqueId"].ToString();
                    model.TekInfo.ReportComment = dtTek.Rows[0]["ReportComment"].ToString();
                    model.TekInfo.CreatedBy = dtTek.Rows[0]["CreatedBy"].ToString();
                    model.TekInfo.DOB = dtTek.Rows[0]["DOB"].ToString();
                }


            }

            int resolutionDPI = cc.getResolutionForReport(model.ClientRowID);
            CandidatePdfLists(PersonalRowID, resolutionDPI, model.ClientRowID);
            model.ReportDate = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MM-yyyy"));
            //CandidatePdfListsNew(id);
            // CandidateCasePdfListsNew(id);
            CandidateCasePdfLists(PersonalRowID, resolutionDPI);
            Session["SecritadRefId"] = model.CompanyRefNo;
            model.CandidateCaseDocList = repoReportQC.GetCandidateCaseDocListForReport(PersonalRowID);
            model.CandidateDocList = repoReportQC.GetCandidateDocListForReport(PersonalRowID);
            BindColorCode(model.ClientRowID);
            model.DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
            //model = repoReportQC.GenerateCandidateInterimReportByReportQCTM(PersonalRowID);
            if (model.ClientRowID == 762)
            {
                model.ReportPart = "OFAC";
                model.CandidateDocList = repoReportQC.GetCandidateDocListForReportArtifactWithApprove(PersonalRowID);
                var list = model.CandidateDocList.Where(T => T.SubCheckRowID == 205).ToList();
                foreach (var a in list)
                {
                    if (a.DocumentType.ToUpper() == "OFAC REPORT")
                    {
                        a.ArtifactOrder = 1;
                    }
                    else if (a.DocumentType.ToUpper() == "ID  REPORT" || a.DocumentType.Contains("ID") || a.DocumentType.Contains("Id"))
                    {
                        a.ArtifactOrder = 4;
                    }
                    else if (a.DocumentType.ToUpper() == "OFAC KEYWORD")
                    {
                        a.ArtifactOrder = 2;
                    }
                    else if (a.DocumentType.Contains("TGS EXPORT REPORT") || a.DocumentType.Contains("TGS Export"))
                    {
                        a.ArtifactOrder = 3;
                    }
                }
                //model.CandidateDocList = list.Where(T => T.DocumentType == "OFAC Keyword" || T.DocumentType == "TGS Export Report" || T.DocumentType == "ID Report" || T.DocumentType == "OFAC Report").OrderBy(T => T.ArtifactOrder);

                model.CandidateDocList = list.Where(T => T.DocumentType == "OFAC Keyword" || T.DocumentType == "TGS Export Report" || T.DocumentType == "ID Report" || T.DocumentType == "OFAC Report").OrderBy(T => T.ArtifactOrder);
            }
            else
            {
                model.CandidateDocList = repoReportQC.GetCandidateDocListForReport(PersonalRowID);
            }
            model.Report_Type = Report_Type;

            DataTable dtcol = new DataTable();
            if (model.ClientRowID == 332)
            {
                dtcol = cc.FinalReportColorChola(model.PersonalRowID, model.ClientRowID);
                if (dtcol.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dtcol.Rows[0]["colorName"].ToString()))
                    {
                        model.FinalColor = dtcol.Rows[0]["colorName"].ToString();
                        model.FinalColorCode = dtcol.Rows[0]["colorCode"].ToString();
                    }
                    else
                    {
                        model.FinalColorCode = "#fff";
                    }

                }
            }
            else
            {
                dtcol = cc.GetColorCode(model.PersonalRowID, model.ClientRowID);
                if (dtcol.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dtcol.Rows[0]["colorName"].ToString()))
                    {
                        model.FinalColor = dtcol.Rows[0]["colorName"].ToString();
                        model.FinalColorCode = dtcol.Rows[0]["colorCode"].ToString();
                    }
                    else
                    {
                        model.FinalColorCode = "#fff";
                    }

                }
            }


            #region  Personal Info 
            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, 0, "Report", "Case");
            DataTable dbValue = new DataTable();
            dbValue = cc.GetReportPersonalValue(PersonalRowID);




            //foreach (DataRow d in dt.Rows)
            //{
            //    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
            //    if (rowsFiltered.Length > 0)
            //    {

            //        model.PersonalInfoModel.Add(new PersonalInfo

            //        {
            //            AntecedentRowID = rowsFiltered[0][0].ToString(),
            //            DisplayNameDE = rowsFiltered[0][1].ToString(),
            //            DisplayNameRep = rowsFiltered[0][2].ToString(),
            //            DataEntryValue = rowsFiltered[0][3].ToString(),
            //            OpsValue = rowsFiltered[0][4].ToString(),
            //            PersonalRowID = Convert.ToInt32(rowsFiltered[0][5].ToString()),


            //        });



            //    }

            //}

            model.PersonalChecksBriefDetails = repoReportQC.PersonalChecksBriefDetails_ReportSpoc(PersonalRowID);
            if (model.ClientRowID == 762 && model.ReportPart == "OFAC")
            {
                var list = model.PersonalChecksBriefDetails.Where(T => T.subCheckId == 205);
                model.PersonalChecksBriefDetails = list;
            }

            #endregion

            //DataTable dtCheckDetail = new DataTable();
            //dtCheckDetail = cc.GetReportCheckDetails(PersonalRowID);


            #region Address List

            DataTable dbAddressValue = new DataTable();
            dbAddressValue = cc.GetReportAllValue1(PersonalRowID, 2);
            if (dbAddressValue.Rows.Count > 0)
            {
                foreach (DataRow dr in dbAddressValue.Rows)
                {
                    DataTable dtAddressAnti = new DataTable();
                    dtAddressAnti = cc.Antecedents_client_select_form(ClientRowId, Convert.ToInt32(dr["SubcheckRowID"].ToString()), "Report", "Check");


                    DataRow[] rowsFiltered = dtAddressAnti.Select("AntecedentsRowID=" + dr["AntecedentRowID"].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        model.AddressInfoModel.Add(new PersonalInfo
                        {
                            AntecedentRowID = dr["AntecedentRowID"].ToString(),
                            //DisplayNameDE = dr["DisplayNameOps"].ToString(),
                            DisplayNameRep = dr["DisplayNameRep"].ToString(),
                            DataEntryValue = dr["DataEntryValue"].ToString(),
                            OpsValue = dr["OpsValue"].ToString(),
                            //PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            SubcheckRowID = Convert.ToInt32(dr["SubcheckRowID"].ToString()),
                            SubCheckName = dr["SubCheckName"].ToString()

                        });

                        DataTable DtColor = cc.GetDispositionPQCandidateChecks(PersonalRowID, Convert.ToInt32(dr["SubcheckRowID"].ToString()));
                        if (DtColor.Rows.Count > 0)
                        {
                            if (DtColor.Rows[0]["Status"].ToString() != "1")
                            {
                                int subcheckid = Convert.ToInt32(DtColor.Rows[0]["SubCheckFamilyID"].ToString());
                                model.AddressInfoModel.RemoveAll(T => T.SubcheckRowID == subcheckid);
                            }
                            else
                            {
                                //if (DtColor.Rows[0]["checkstatusid"].ToString() == "36")
                                //{
                                model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorCode = DtColor.Rows[0]["ColorCode"].ToString();
                                model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = DtColor.Rows[0]["ColorName"].ToString();
                                // }
                                model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().ReportComment = DtColor.Rows[0]["Disposition"].ToString();
                                //model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().modeofverification = "Physical";
                                // ReportComment
                            }

                        }





                    }





                }
                //}
            }

            #endregion


            #region Employment List List

            DataTable dbEmploymentValue = new DataTable();
            dbEmploymentValue = cc.GetReportAllValue1(PersonalRowID, 12);
            if (dbEmploymentValue.Rows.Count > 0)
            {
                foreach (DataRow dr in dbEmploymentValue.Rows)
                {
                    DataTable dtEmpAnti = new DataTable();
                    dtEmpAnti = cc.Antecedents_client_select_form(ClientRowId, Convert.ToInt32(dr["SubcheckRowID"].ToString()), "Report", "Check");


                    DataRow[] rowsFiltered = dtEmpAnti.Select("AntecedentsRowID=" + dr["AntecedentRowID"].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        model.EmploymentInfoModel.Add(new PersonalInfo
                        {
                            AntecedentRowID = dr["AntecedentRowID"].ToString(),
                            //DisplayNameDE = dr["DisplayNameOps"].ToString(),
                            DisplayNameRep = dr["DisplayNameRep"].ToString(),
                            DataEntryValue = dr["DataEntryValue"].ToString(),
                            OpsValue = dr["OpsValue"].ToString(),
                            ////PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            SubcheckRowID = Convert.ToInt32(dr["SubcheckRowID"].ToString()),
                            SubCheckName = dr["SubCheckName"].ToString()

                        });
                        DataTable DtColor = cc.GetDispositionPQCandidateChecks(PersonalRowID, Convert.ToInt32(dr["SubcheckRowID"].ToString()));
                        //if (DtColor.Rows[0]["checkstatusid"].ToString() == "36")
                        //{
                        model.EmploymentInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorCode = DtColor.Rows[0]["ColorCode"].ToString();
                        model.EmploymentInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = DtColor.Rows[0]["ColorName"].ToString();
                        //}
                        model.EmploymentInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().ReportComment = DtColor.Rows[0]["Disposition"].ToString();

                    }




                }
                //}
            }

            #endregion


            #region Education List 

            DataTable dbEducationtValue = new DataTable();
            dbEducationtValue = cc.GetReportAllValue1(PersonalRowID, 11);
            if (dbEducationtValue.Rows.Count > 0)
            {
                foreach (DataRow dr in dbEducationtValue.Rows)
                {
                    DataTable dtEducationAnti = new DataTable();
                    dtEducationAnti = cc.Antecedents_client_select_form(ClientRowId, Convert.ToInt32(dr["SubcheckRowID"].ToString()), "Report", "Check");


                    DataRow[] rowsFiltered = dtEducationAnti.Select("AntecedentsRowID=" + dr["AntecedentRowID"].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        model.EducationInfoModel.Add(new PersonalInfo
                        {
                            AntecedentRowID = dr["AntecedentRowID"].ToString(),
                            ////DisplayNameDE = dr["DisplayNameOps"].ToString(),
                            DisplayNameRep = dr["DisplayNameRep"].ToString(),
                            DataEntryValue = dr["DataEntryValue"].ToString(),
                            OpsValue = dr["OpsValue"].ToString(),
                            //PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            SubcheckRowID = Convert.ToInt32(dr["SubcheckRowID"].ToString()),
                            SubCheckName = dr["SubCheckName"].ToString()

                        });
                        DataTable DtColor = cc.GetDispositionPQCandidateChecks(PersonalRowID, Convert.ToInt32(dr["SubcheckRowID"].ToString()));
                        //if (DtColor.Rows[0]["checkstatusid"].ToString() == "36")
                        //{
                        model.EducationInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorCode = DtColor.Rows[0]["ColorCode"].ToString();
                        model.EducationInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = DtColor.Rows[0]["ColorName"].ToString();
                        // }
                        model.EducationInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().ReportComment = DtColor.Rows[0]["Disposition"].ToString();

                    }




                }
                //}
            }

            #endregion


            #region National List 

            DataTable dbNationalValue = new DataTable();
            dbNationalValue = cc.GetReportAllValue1(PersonalRowID, 26);
            if (dbNationalValue.Rows.Count > 0)
            {
                foreach (DataRow dr in dbNationalValue.Rows)
                {
                    DataTable dtNationalAnti = new DataTable();
                    dtNationalAnti = cc.Antecedents_client_select_form(ClientRowId, Convert.ToInt32(dr["SubcheckRowID"].ToString()), "Report", "Check");

                    if (dtNationalAnti.Rows.Count > 0)
                    {
                        DataRow[] rowsFiltered = dtNationalAnti.Select("AntecedentsRowID=" + dr["AntecedentRowID"].ToString());
                        if (rowsFiltered.Length > 0)
                        {
                            model.NationalInfoModel.Add(new PersonalInfo
                            {
                                AntecedentRowID = dr["AntecedentRowID"].ToString(),
                                ////DisplayNameDE = dr["DisplayNameOps"].ToString(),
                                DisplayNameRep = dr["DisplayNameRep"].ToString(),
                                DataEntryValue = dr["DataEntryValue"].ToString(),
                                OpsValue = dr["OpsValue"].ToString(),
                                //PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                                SubcheckRowID = Convert.ToInt32(dr["SubcheckRowID"].ToString()),
                                SubCheckName = dr["SubCheckName"].ToString()

                            });
                            DataTable DtColor = cc.GetDispositionPQCandidateChecks(PersonalRowID, Convert.ToInt32(dr["SubcheckRowID"].ToString()));
                            //if (DtColor.Rows[0]["checkstatusid"].ToString() == "36")
                            //{
                            model.NationalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorCode = DtColor.Rows[0]["ColorCode"].ToString();
                            model.NationalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = DtColor.Rows[0]["ColorName"].ToString();
                            //}
                            model.NationalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().ReportComment = DtColor.Rows[0]["Disposition"].ToString();

                        }

                        model.NationalInfoModel.RemoveAll(T => T.AntecedentRowID == "99");
                    }





                }
                //}
            }

            #endregion


            #region Reference List 

            DataTable dbReferenceValue = new DataTable();
            dbReferenceValue = cc.GetReportAllValue1(PersonalRowID, 30);
            if (dbReferenceValue.Rows.Count > 0)
            {
                foreach (DataRow dr in dbReferenceValue.Rows)
                {
                    DataTable dtReferenceAnti = new DataTable();
                    dtReferenceAnti = cc.Antecedents_client_select_form(ClientRowId, Convert.ToInt32(dr["SubcheckRowID"].ToString()), "Report", "Check");


                    DataRow[] rowsFiltered = dtReferenceAnti.Select("AntecedentsRowID=" + dr["AntecedentRowID"].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        model.ReferenceInfoModel.Add(new PersonalInfo
                        {
                            AntecedentRowID = dr["AntecedentRowID"].ToString(),
                            //DisplayNameDE = dr["DisplayNameOps"].ToString(),
                            DisplayNameRep = dr["DisplayNameRep"].ToString(),
                            DataEntryValue = dr["DataEntryValue"].ToString(),
                            OpsValue = dr["OpsValue"].ToString(),
                            //PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            SubcheckRowID = Convert.ToInt32(dr["SubcheckRowID"].ToString()),
                            SubCheckName = dr["SubCheckName"].ToString()

                        });
                        DataTable DtColor = cc.GetDispositionPQCandidateChecks(PersonalRowID, Convert.ToInt32(dr["SubcheckRowID"].ToString()));
                        //if (DtColor.Rows[0]["checkstatusid"].ToString() == "36")
                        //{
                        model.ReferenceInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorCode = DtColor.Rows[0]["ColorCode"].ToString();
                        model.ReferenceInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = DtColor.Rows[0]["ColorName"].ToString();
                        //}
                        model.ReferenceInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().ReportComment = DtColor.Rows[0]["Disposition"].ToString();


                    }




                }
                //}
            }

            #endregion

            #region Criminal List 

            DataTable dbCriminalValue = new DataTable();
            dbCriminalValue = cc.GetReportAllValue1(PersonalRowID, 5);
            if (dbCriminalValue.Rows.Count > 0)
            {
                foreach (DataRow dr in dbCriminalValue.Rows)
                {
                    DataTable dtCriminalAnti = new DataTable();
                    dtCriminalAnti = cc.Antecedents_client_select_form(ClientRowId, Convert.ToInt32(dr["SubcheckRowID"].ToString()), "Report", "Check");


                    DataRow[] rowsFiltered = dtCriminalAnti.Select("AntecedentsRowID=" + dr["AntecedentRowID"].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        model.CriminalInfoModel.Add(new PersonalInfo
                        {
                            AntecedentRowID = dr["AntecedentRowID"].ToString(),
                            //DisplayNameDE = dr["DisplayNameOps"].ToString(),
                            DisplayNameRep = dr["DisplayNameRep"].ToString(),
                            DataEntryValue = dr["DataEntryValue"].ToString(),
                            OpsValue = dr["OpsValue"].ToString(),
                            //PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            SubcheckRowID = Convert.ToInt32(dr["SubcheckRowID"].ToString()),
                            SubCheckName = dr["SubCheckName"].ToString()

                        });
                        DataTable DtColor = cc.GetDispositionPQCandidateChecks(PersonalRowID, Convert.ToInt32(dr["SubcheckRowID"].ToString()));
                        if (DtColor.Rows.Count > 0)
                        {
                            if (DtColor.Rows[0]["Status"].ToString() != "1")
                            {
                                int subcheckid = Convert.ToInt32(DtColor.Rows[0]["SubCheckFamilyID"].ToString());
                                model.CriminalInfoModel.RemoveAll(T => T.SubcheckRowID == subcheckid);
                            }
                            else
                            {
                                //if (DtColor.Rows[0]["checkstatusid"].ToString() == "36")
                                //{
                                model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorCode = DtColor.Rows[0]["ColorCode"].ToString();
                                model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = DtColor.Rows[0]["ColorName"].ToString();
                                //}
                                model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().ReportComment = DtColor.Rows[0]["Disposition"].ToString();
                                // ReportComment
                            }

                        }




                    }




                }
                //}
            }

            #endregion

            #region Special List 

            DataTable dbSpecialValue = new DataTable();
            dbSpecialValue = cc.GetReportAllValue1(PersonalRowID, 40);
            if (dbSpecialValue.Rows.Count > 0)
            {
                foreach (DataRow dr in dbSpecialValue.Rows)
                {
                    DataTable dtSpecialAnti = new DataTable();
                    dtSpecialAnti = cc.Antecedents_client_select_form(ClientRowId, Convert.ToInt32(dr["SubcheckRowID"].ToString()), "Report", "Check");


                    DataRow[] rowsFiltered = dtSpecialAnti.Select("AntecedentsRowID=" + dr["AntecedentRowID"].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        model.SpecialInfoModel.Add(new PersonalInfo
                        {
                            AntecedentRowID = dr["AntecedentRowID"].ToString(),
                            //DisplayNameDE = dr["DisplayNameOps"].ToString(),
                            DisplayNameRep = dr["DisplayNameRep"].ToString(),
                            DataEntryValue = dr["DataEntryValue"].ToString(),
                            OpsValue = dr["OpsValue"].ToString(),
                            //PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            SubcheckRowID = Convert.ToInt32(dr["SubcheckRowID"].ToString()),
                            SubCheckName = dr["SubCheckName"].ToString()

                        });
                        DataTable DtColor = cc.GetDispositionPQCandidateChecks(PersonalRowID, Convert.ToInt32(dr["SubcheckRowID"].ToString()));
                        if (DtColor.Rows[0]["checkstatusid"].ToString() == "37")
                        {
                            model.SpecialInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorCode = DtColor.Rows[0]["ColorCode"].ToString();
                            model.SpecialInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = DtColor.Rows[0]["ColorName"].ToString();
                        }

                        model.SpecialInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().ReportComment = DtColor.Rows[0]["Disposition"].ToString();


                    }
                    model.SpecialInfoModel.RemoveAll(T => T.AntecedentRowID == "116" || T.AntecedentRowID == "117");



                }
                //}
            }

            #endregion

            Session["Type"] = "";
            return View(model);

        }
        [NonAction]
        void BindColorCode(int ClientRowId)
        {
            try
            {
                ViewBag.Color = repoReportQC.getcolorcodeByClient(ClientRowId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public void CandidatePdfLists(int id, int dpi, int clientid)
        {
            string DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();
            var CandidatePdfList = Enumerable.Empty<CandidateAllDocumentListViewModel>();
            if (clientid == 762)
            {
                CandidatePdfList = repoReportQC.GetCandidatePdfListForConversionWithOutApprove(id);
            }
            else
            {
                CandidatePdfList = repoReportQC.GetCandidatePdfListForConversion_spoc(id);
            }
            foreach (var item in CandidatePdfList)
            {
                SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
                f.Serial = "80027423579"; //License for server 
                //f.Serial = "10273316474"; //License for development machine
                f.OpenPdf(Server.MapPath(DocPath + item.DocFile));

                if (f.PageCount > 0)
                {
                    string jpgFileName = item.DocFile.Replace(".pdf", "");
                    string ImageName = string.Empty;
                    for (int i = 1; i <= f.PageCount; i++)
                    {
                        ImageName += jpgFileName + i + ".jpg,";
                    }
                    if (!string.IsNullOrEmpty(ImageName) && ImageName.Length > 0)
                    {
                        ImageName = ImageName.Substring(0, ImageName.Length - 1);
                        if (clientid == 762)
                        {
                            bool res = repoReportQC.UpdateImageName(item.VerifiedUploadRowID, ImageName);
                            if (res)
                            {
                                //int width = 644; // Width in Px
                                //int height = 911; // Height in Px
                                f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                                //f.ImageOptions.Resize(new System.Drawing.Size { Width = width, Height = height }, false);
                                f.ImageOptions.Dpi = dpi;
                                f.ToImage(Server.MapPath(DocPath), jpgFileName);
                            }
                            //cc.updateReportPDFToImagePages(item.VerifiedUploadRowID, ImageName);

                        }
                        else
                        {
                            //bool res = repoReportQC.UpdateImageName(item.VerifiedUploadRowID, ImageName);
                            // if (res)
                            //{
                            //int width = 644; // Width in Px
                            //int height = 911; // Height in Px
                            f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                            //f.ImageOptions.Resize(new System.Drawing.Size { Width = width, Height = height }, false);
                            f.ImageOptions.Dpi = dpi;
                            f.ToImage(Server.MapPath(DocPath), jpgFileName);
                            cc = new ClientCustomize();
                            cc.updateReportPDFToImagePages(item.VerifiedUploadRowID, ImageName);
                        }
                    }
                }
            }
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult CaseTransaction()
        {
            detailTran d = new detailTran();
            return View(d);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CaseTransaction(detailTran model)
        {
            DataTable dt = new DataTable();
            ClientCustomize cc = new ClientCustomize();
            dt = cc.GetCandidateChecksTranDetails(model.companyRefNo);

            List<detailTran_List> lmd = new List<detailTran_List>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new detailTran_List
                    {
                        subCheckName = (dr["subCheckName"].ToString()),
                        oldStatus = (dr["oldStatus"].ToString()),
                        newStatus = (dr["newStatus"].ToString()),
                        checkStatus = (dr["checkStatus"].ToString()),
                        CreationDate = (dr["CreationDate"].ToString()),
                        doneBy = (dr["doneBy"].ToString())
                    });
                }
                int pageNo = 1;
                int pageSize = 200;
                model.TotalRecords = dt.Rows.Count;
                model.PageSize = 200;
                model.checkTranDetails = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult DeleteCase(FormCollection form, string Search = "", string Remarks = " ", string SaveWork = "", String CheckDel = "", int pageNo = 1, int pageSize = 100)
        {
            try
            {
                int clientRowID = 0;
                string ddlCheckStatus = form["ddlCheckStatus"];
                string ColorName = form["ddlColor"];
                ClientServiceListViewModel model = new ClientServiceListViewModel();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                if (Search == "")
                {
                    BindSubchecknameDelete(0, 0);
                    BindMasterCheckStatus();
                }

                else
                {

                    // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                    DataTable dt = new DataTable();
                    ClientCustomize cc = new ClientCustomize();
                    dt = cc.GetCandidateChecks(Search);
                    List<ClientDashboardCheckSummaryDetailViewModel> lmd = new List<ClientDashboardCheckSummaryDetailViewModel>();
                    if (dt.Rows.Count > 0)
                    {
                        clientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            lmd.Add(new ClientDashboardCheckSummaryDetailViewModel
                            {
                                SecuritasID = (dr["CompanyRefNo"].ToString()),
                                CheckName = (dr["SubCheckName"].ToString()),
                                CheckStatus = dr["CheckStatus"].ToString(),
                                CheckStatusID = (dr["CheckStatusID"].ToString()),
                                OrderDate = (dr["orderDate"].ToString()),
                                OutDate = (dr["outDate"].ToString()),
                                CreatedDate = (dr["CreationDate"].ToString()),
                                ColorName = dr["ColorName"].ToString(),
                                ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"]),
                                Name = (dr["Name"].ToString()),

                                PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),

                            });

                        }
                        model.TotalRecords = dt.Rows.Count;
                        BindSubchecknameDelete(Convert.ToInt32(dt.Rows[0]["ClientRowID"]), Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                        BindMasterCheckStatus();

                    }






                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.ClientServiceChecks = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                    if ((SaveWork != "" && SaveWork == "DeleteCase"))
                    {
                        if (Remarks != "")
                        {
                            int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                            dt = cc.DeleteChecks(Search, "CaseDelete", teamMemerRowId, Remarks);
                            TempData["SuccessMsg"] = "Case Deleted Successfully!";
                            if (clientRowID == 249)
                            {
                                DataTable dt1 = cc.GetPersonalRowId(Search);
                                string ClientRefID = dt1.Rows[0]["ClientRefID"].ToString();
                                APIResponse apiResponse = new API().SutherlandStatusAPI("", ClientRefID, Search, "Stop BGV", "Stopped");
                                new API().InsertAPI_AuditDetail(ClientRefID, 249, apiResponse.status, "Delete Case", "CompanyRefNo:" + Search + ",Color:Stop BGV, Status:Stopped", apiResponse.statusCode + ":" + apiResponse.apiResponseData, "API");
                            }
                            Search = ""; Remarks = "";
                            return RedirectToAction("DeleteCase", "ClientServicing", new { Search = "" });
                        }
                        else
                        {

                            TempData["SuccessMsg"] = "Please fill Remarks!";


                        }

                    }

                    if ((SaveWork != "" && SaveWork == "StopCase"))
                    {
                        if (Remarks != "")
                        {
                            int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                            dt = cc.StopChecks(Search, "CaseStop", teamMemerRowId, Remarks);
                            TempData["SuccessMsg"] = "Case Stop Successfully!";
                            Search = ""; Remarks = "";
                            return RedirectToAction("DeleteCase", "ClientServicing", new { Search = "" });
                        }
                        else
                        {

                            TempData["SuccessMsg"] = "Please fill Remarks!";


                        }

                    }


                    if (ColorName != null && ColorName != "0")//this is use for check remove

                    {
                        if (ddlCheckStatus == "102") //   delete
                        {
                            if (Remarks != "")
                            {
                                int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);

                                cc.PQCandidateChecksDeleteCheck(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(Request.Form["ddlColor"]), 1, 0, "helpdesk", Convert.ToInt32(Session["TeamMemberRowID"]), 0, Remarks);
                                cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), strDDLValue, 2, Remarks, "Support", Convert.ToInt32(Session["TeamMemberRowID"]));
                                TempData["SuccessMsg"] = "Check Remove Successfully!";
                                return RedirectToAction("DeleteCase", "ClientServicing", new { Search = Search });
                            }

                            else
                            {
                                TempData["SuccessMsg"] = "Please fill the Remarks";
                            }
                        }
                        if (ddlCheckStatus == "100") // stop
                        {
                            if (Remarks != "")
                            {
                                int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);

                                cc.AllStopCheck(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(Request.Form["ddlColor"]), "helpdesk", Convert.ToInt32(Session["TeamMemberRowID"]), Remarks);

                                cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), strDDLValue, 3, Remarks, "Support", Convert.ToInt32(Session["TeamMemberRowID"]));
                                TempData["SuccessMsg"] = "Check Remove Successfully!";
                                return RedirectToAction("DeleteCase", "ClientServicing", new { Search = Search });
                            }

                            else
                            {
                                TempData["SuccessMsg"] = "Please fill the Remarks";
                            }
                        }
                        else
                        {
                            TempData["SuccessMsg"] = "Please Select  Delete action";
                        }

                    }

                    if (ColorName != null && ColorName != "0" && ddlCheckStatus != "102" && ddlCheckStatus != "0" && Remarks != "") //this is use for check move
                    {
                        if (Remarks != "")
                        {

                            int MoveCheckId = Convert.ToInt32(ddlCheckStatus);


                            int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);

                            cc.PQCandidateChecksDeleteCheck(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(Request.Form["ddlColor"]), 1, MoveCheckId, "helpdesk", Convert.ToInt32(Session["TeamMemberRowID"]), 0, Remarks);
                            cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), strDDLValue, MoveCheckId, Remarks, "Support", Convert.ToInt32(Session["TeamMemberRowID"]));
                            TempData["SuccessMsg"] = "Check Move Successfully!";
                            return RedirectToAction("DeleteCase", "ClientServicing", new { Search = Search });
                        }
                        else
                        {
                            TempData["SuccessMsg"] = "Please fill all Details and move check!";

                        }

                    }




                }

                return View(model);



            }
            catch (Exception ex)
            {
                TempData["SuccessMsg"] = ex.Message;
                throw;
            }
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ManageCase(FormCollection form, string Search = "", string Remarks = " ", string SaveWork = "", string CheckActionButton = "", string createdby = "", string SelectedChecksHiddenField = "", int pageNo = 1, int pageSize = 100)
        {
            try
            {
                int clientRowID = 0;
                string ddlCheckStatus = form["ddlCheckStatus"];
                string ColorName = form["ddlColor"];
                int CandCheckRowID = 0;
                ClientServiceListViewModel model = new ClientServiceListViewModel();

                if (pageNo < 0) { pageNo = 1; }

                if (Search == "")
                {
                    BindSubchecknameDelete(0, 0);
                    BindMasterCheckStatus();
                }
                else
                {
                    DataTable dt = new DataTable();
                    ClientCustomize cc = new ClientCustomize();
                    dt = cc.GetCandidateinactivechecks(Search);
                    List<ClientDashboardCheckSummaryDetailViewModel> lmd = new List<ClientDashboardCheckSummaryDetailViewModel>();

                    if (dt.Rows.Count > 0)
                    {
                        clientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new ClientDashboardCheckSummaryDetailViewModel
                            {
                                SecuritasID = dr["CompanyRefNo"].ToString(),
                                CheckName = dr["SubCheckName"].ToString(),
                                CheckStatus = dr["CheckStatus"].ToString(),
                                CheckStatusID = dr["CheckStatusID"].ToString(),
                                OrderDate = dr["orderDate"].ToString(),
                                OutDate = dr["outDate"].ToString(),
                                CreatedDate = dr["CreationDate"].ToString(),
                                ColorName = dr["ColorName"].ToString(),
                                ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"]),
                                CandCheckRowID = Convert.ToInt32(dr["CandCheckRowID"]),


                                Name = dr["Name"].ToString(),
                                PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            });
                        }
                        model.TotalRecords = dt.Rows.Count;
                        BindSubchecknameDelete(clientRowID, Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                        BindMasterCheckStatus();
                    }
                    else
                    {
                        model.TotalRecords = 0;
                    }

                    model.PageSize = pageSize;
                    model.ClientServiceChecks = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);


                    if (SaveWork == "CASEACTIVE" && Remarks != "")
                    {
                        int teamMemberRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                        cc.CaseActivee(Search, Remarks);
                        TempData["SuccessMsg"] = "Case Active Successfully!";
                        return RedirectToAction("ManageCase", "ClientServicing", new { Search = "" });
                    }
                    else if (SaveWork == "CASEACTIVE" && Remarks == "")
                    {
                        TempData["SuccessMsg"] = "Please fill Remarks!";
                    }

                    // Handle Check Active - Check Action Button
                    if (!string.IsNullOrEmpty(CheckActionButton))
                    {
                        string[] selectedCheckIds = SelectedChecksHiddenField.Split(',');

                        foreach (string checkId in selectedCheckIds)
                        {
                            int checkID = Convert.ToInt32(checkId);
                            int teamMemberRowId = Convert.ToInt32(Session["TeamMemberRowID"]);

                            // Activate each selected check

                            cc.caseactivelog(Search, checkID, createdby, Remarks);
                        }

                        TempData["SuccessMsg"] = "Selected checks activated successfully!";
                        return RedirectToAction("ManageCase", "ClientServicing", new { Search = Search });
                    }

                    else if (ColorName != null && ColorName != "0" && Remarks == "")
                    {
                        TempData["SuccessMsg"] = "Please fill Remarks!";
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["SuccessMsg"] = ex.Message;
                throw;
            }
        }











        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UpdateCaseStatus(FormCollection form, string Search = "", string Remarks = " ", string SaveWork = "", String CheckDel = "", int pageNo = 1, int pageSize = 100)
        {
            try
            {
                int clientRowID = 0;
                //  string ddlCheckStatus = form["ddlCheckStatus"];
                string ddlcaseStatus = form["ddlcaseStatus"];
                string ColorName = form["ddlColor"];
                ClientServiceListViewModel model = new ClientServiceListViewModel();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                if (Search == "")
                {
                    //BindSubchecknameDelete(0, 0);
                    // BindMasterCaseStatus();
                }

                else
                {

                    // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                    DataTable dt = new DataTable();
                    ClientCustomize cc = new ClientCustomize();
                    dt = cc.GetCandidateChecks(Search);
                    List<ClientDashboardCheckSummaryDetailViewModel> lmd = new List<ClientDashboardCheckSummaryDetailViewModel>();
                    if (dt.Rows.Count > 0)
                    {
                        clientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            lmd.Add(new ClientDashboardCheckSummaryDetailViewModel
                            {
                                SecuritasID = (dr["CompanyRefNo"].ToString()),
                                CheckName = (dr["SubCheckName"].ToString()),
                                CheckStatus = dr["CheckStatus"].ToString(),
                                CheckStatusID = (dr["CheckStatusID"].ToString()),
                                OrderDate = (dr["orderDate"].ToString()),
                                OutDate = (dr["outDate"].ToString()),
                                CreatedDate = (dr["CreationDate"].ToString()),
                                ColorName = dr["ColorName"].ToString(),
                                ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"]),
                                Name = (dr["Name"].ToString()),
                                casestatusid = (dr["CaseStatus"].ToString()),
                                PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),

                            });

                        }
                        model.TotalRecords = dt.Rows.Count;
                        BindSubchecknameDelete(Convert.ToInt32(dt.Rows[0]["ClientRowID"]), Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                        BindMasterCaseStatus();

                    }






                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.ClientServiceChecks = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);





                    if (ColorName != null && ColorName != "0" && ddlcaseStatus != "101" && ddlcaseStatus != "0" && Remarks != "") //this is use for check move
                    {
                        if (Remarks != "")
                        {

                            int MoveCheckId = Convert.ToInt32(ddlcaseStatus);


                            int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);
                            string LocalIP = GetIPAddress();
                            cc.UpdateCaseStatus(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), MoveCheckId, Convert.ToInt32(Session["TeamMemberRowID"]));
                            cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), strDDLValue, MoveCheckId, Remarks, "Support : " + LocalIP, Convert.ToInt32(Session["TeamMemberRowID"]));
                            TempData["SuccessMsg"] = "Case Move Successfully!";
                            return RedirectToAction("DeleteCase", "ClientServicing", new { Search = Search });
                        }
                        else
                        {
                            TempData["SuccessMsg"] = "Please fill all Details and move check!";

                        }

                    }




                }

                return View(model);



            }
            catch (Exception ex)
            {
                TempData["SuccessMsg"] = ex.Message;
                throw;
            }
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



        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult CaseProcess(FormCollection form, string Search = "", string Remarks = " ", string SaveWork = "", String CheckDel = "", int pageNo = 1, int pageSize = 100)
        {
            try
            {

                string Department = Session["Department"].ToString();
                string ddlCheckStatus = form["ddlCheckStatus"];
                string ColorName = form["ddlColor"];
                ClientServiceListViewModel model = new ClientServiceListViewModel();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                if (Search == "")
                {
                    BindSubchecknameDelete(0, 0);
                    BindMasterCheckStatusForSpoc(Department);
                }

                else
                {

                    // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                    DataTable dt = new DataTable();
                    ClientCustomize cc = new ClientCustomize();
                    dt = cc.GetCandidateChecks(Search);

                    List<ClientDashboardCheckSummaryDetailViewModel> lmd = new List<ClientDashboardCheckSummaryDetailViewModel>();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            lmd.Add(new ClientDashboardCheckSummaryDetailViewModel
                            {
                                SecuritasID = (dr["CompanyRefNo"].ToString()),
                                CheckName = (dr["SubCheckName"].ToString()),
                                CheckStatus = dr["CheckStatus"].ToString(),
                                CheckStatusID = (dr["CheckStatusID"].ToString()),
                                OrderDate = (dr["orderDate"].ToString()),
                                OutDate = (dr["outDate"].ToString()),
                                CreatedDate = (dr["CreationDate"].ToString()),
                                ColorName = dr["ColorName"].ToString(),
                                ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"]),
                                Name = (dr["Name"].ToString()),

                                PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),

                            });

                        }
                        model.TotalRecords = dt.Rows.Count;
                        BindSubchecknameDelete(Convert.ToInt32(dt.Rows[0]["ClientRowID"]), Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                        BindMasterCheckStatusForSpoc(Department);

                    }






                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.ClientServiceChecks = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);







                    if (ColorName != null && ColorName != "0" && ddlCheckStatus != "0" && Remarks != "") //this is use for check move
                    {
                        if (Remarks != "")
                        {
                            DataTable dtNotapplicable = cc.CheckNotApplicable(Search, Convert.ToInt32(Request.Form["ddlColor"]));
                            if (dtNotapplicable.Rows.Count > 0 && Convert.ToInt32(ddlCheckStatus) != 8)
                            {
                                TempData["SuccessMsg"] = "Not applicable check move only data entry TM!";
                            }
                            else
                            {
                                int MoveCheckId = Convert.ToInt32(ddlCheckStatus);


                                int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);

                                cc.PQCandidateChecksDeleteCheck(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(Request.Form["ddlColor"]), 1, MoveCheckId, "helpdesk", Convert.ToInt32(Session["TeamMemberRowID"]), 0, Remarks);
                                cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), strDDLValue, MoveCheckId, Remarks, "Support", Convert.ToInt32(Session["TeamMemberRowID"]));
                                //Remarks = "Re verification -" + Remarks;
                                #region Check Action History Save                           
                                clsCommonMethods.AddCheckActionHistory(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt16(strDDLValue), "Check Movement", Remarks);
                                #endregion
                                //cc.UpdateVertable(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), MoveCheckId);
                                TempData["SuccessMsg"] = "Check Move Successfully!";
                                if (ddlCheckStatus != "36")
                                {
                                    alertMailToManagerOnCheckReopen(Search, ddlCheckStatus, strDDLValue, Remarks);
                                }
                                return RedirectToAction("CaseProcess", "ClientServicing", new { Search = Search });

                            }






                        }


                    }
                    else
                    {
                        TempData["SuccessMsg"] = "Please Select check details!";
                    }



                }

                return View(model);



            }
            catch (Exception)
            {
                throw;
            }
        }

        private void alertMailToManagerOnCheckReopen(string Comp_RefNo, string ddlCheckStatus, int subCheckRowId, string Remarks)
        {
            ClientCustomize cc = new ClientCustomize();
            string MailSignature = string.Empty;
            clsSendMail mail = new clsSendMail();
            DataTable dt = cc.GetDepartmentManager(subCheckRowId);
            var signature = mail.GetCompanyMailSignature();
            string Email = "";
            string subCheckName = "";
            if (dt.Rows.Count > 0)
            {
                Email = dt.Rows[0]["EmailId"].ToString();
                subCheckName = dt.Rows[0]["subCheckName"].ToString();
                string body = string.Empty;
                string MailSubject = "";
                var templateDB = cc.getAlertMailtemplate(0, "Check Reopen", 0, 1, "CheckReopenMail");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }
                //using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/CheckReopenMail.html")))
                //{
                //    body = reader.ReadToEnd();
                //}

                body = body.Replace("{FCheckName}", subCheckName);
                body = body.Replace("{FPRemarks}", Remarks);
                body = body.Replace("{FRefNo}", Comp_RefNo);
                body = body.Replace("{MailSignature}", MailSignature);
                string Subject = "Check ReOpen " + subCheckName + "  " + Comp_RefNo;

                string msg = "<b>" + subCheckName + "</b> Reopen By Spoc with Ref No: <b>" + Comp_RefNo + "</b>";
                if (ddlCheckStatus == "8")
                {
                    mail.SendMail("", "bgv.request@securitas-india.com", "bgv.initiation@securitas-india.com", Subject, body);
                }
                else
                {
                    mail.SendMail("", "bgv.request@securitas-india.com", Email, "bgv.initiation@securitas-india.com", Subject, body);

                }
            }

        }

        void BindSubchecknameDelete(int ClientRowId, int PersonalRowID)
        {
            try
            {

                ViewBag.chek = repoPQPersonal.getSubcheckdataforDelete(ClientRowId, PersonalRowID);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [ValidateInput(false)]
        public ActionResult RejectCase(string Search = "", int pageNo = 1, int pageSize = 100, string SaveWork = "", int id = 0, int sub = 0)
        {

            try
            {
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                //  cc.getCurrentCheckStatus(personalRowID, subCheckRowID);

                return View();
            }

            catch (Exception)
            {
                throw;
            }
        }

        void BindMasterCheckStatus()
        {
            try
            {

                ViewBag.CheckId = repoPQPersonal.GetMastercheckId();

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        void BindMasterCaseStatus()
        {
            try
            {

                ViewBag.CaseId = repoPQPersonal.GetMasterCaseId();

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        #region ****** Employment Add doj and employementid  ******

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult AddEmpCodeAndDOJByCSSpoc(int page = 1, string sort = "", string sortDir = "asc", int PersonalRowID = 0, string EmployeeID = "", string DateofJoining = "", string Update = "", string ClientRowId = "")
        {
            ClientCustomize cc = new ClientCustomize();
            DataTable dt = new DataTable();
            ViewEmpData model = new ViewEmpData();

            dt = cc.CheckdataEmp(PersonalRowID);
            if (dt.Rows.Count > 0)
            {
                model.EmployeeID = dt.Rows[0]["EmployeeID"].ToString();
                if (dt.Rows[0]["DateofJoining"].ToString() != "")
                {
                    model.DateofJoining = Convert.ToDateTime(dt.Rows[0]["DateofJoining"].ToString());
                }

                model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                model.ClientRowID = ClientRowId;
                model.EntityName = dt.Rows[0]["EntityName"].ToString();
                model.location = dt.Rows[0]["locationName"].ToString();
                model.PanCard = dt.Rows[0]["PanCardNo"].ToString();
            }

            return View(model);
        }

        #endregion
        [SessionExpireFilterAttribute]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult AddEmpCodeAndDOJByCSSpoc(ViewEmpData model)
        {
            ClientCustomize cc = new ClientCustomize();
            DataTable dt = new DataTable();
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        if (string.IsNullOrEmpty(model.DateofJoining.ToString()))
                        {
                            model.DateofJoining = null;
                        }

                        cc.UpdateEmpIdDOJNew(model.EmployeeID, model.DateofJoining, model.PersonalRowID, teamMemerRowId, model.ClientRefID, model.EntityName, model.location, model.PanCard, model.ProcessName, model.LineOfBusiness);

                        ViewBag.SuccessMsg = "Record Updated Successfully!";
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

        bool SendAlertMassageForInsuffClearMail(int PersonalRowID, short SubCheckRowID, string remarks, string insuffType, string AlertType = "", int PackageId = 0, int MailType = 0)
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
                var clientId = Convert.ToInt32(ds.Tables[0].Rows[0]["ClientRowId"]);
                var templateDB = cc.getAlertMailtemplate(clientId, AlertType, PackageId, MailType, "InsuffClear");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }
                //using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/InsuffClear.html")))
                //{
                //    body = reader.ReadToEnd();
                //}
                string CandidateName = Convert.ToString(ds.Tables[0].Rows[0]["CandidateName"]);
                string CompanyRefNo = Convert.ToString(ds.Tables[0].Rows[0]["CompanyRefNo"]);
                string SubCheckName = Convert.ToString(ds.Tables[0].Rows[0]["SubCheckName"]);
                string TeamEmailID = Convert.ToString(ds.Tables[0].Rows[0]["OperationEmailID"]);
                string SpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["SpocEmailID"]);
                string ToMailId = Convert.ToString(ds.Tables[0].Rows[0]["SendInsuffEmail"]);
                //string SpocEmailID = ""; // Convert.ToString(ds.Tables[0].Rows[0]["SpocEmailID"]);
                string ClientSpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["ClientSpocEmailID"]);


                //var dtEmail = cc.GetEmailIdForAlert(clientId, 0, "Insuff");
                //if (dtEmail.Rows.Count > 0)
                //{
                //    SpocEmailID = Convert.ToString(dtEmail.Rows[0]["SpocEmailID"]);
                //}
                body = body.Replace("{FCandidateName}", CandidateName);
                body = body.Replace("{FCheckName}", SubCheckName);
                body = body.Replace("{FPRemarks}", remarks);
                body = body.Replace("{FRefNo}", CompanyRefNo);

                body = body.Replace("{MailSignature}", MailSignature);

                string Subject = "Insuff Cleared for " + CompanyRefNo + " " + CandidateName + "  " + SubCheckName;

                string To = TeamEmailID + "," + SpocEmailID;
                string MailCC = TeamEmailID + "," + SpocEmailID;
                if (clientId == 50)
                {
                    To = ToMailId;
                }

                string ToMail = To.ToLower();
                bool flag;
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

                MailCC = MailCC.ToLower();
                MailCC = MailCC.Replace(",", ";");
                string[] arCCMail = MailCC.Split(';');
                MailCC = "";
                foreach (string a in arCCMail)
                {
                    if (!MailCC.Contains(a))
                    {
                        MailCC = MailCC + a.Trim() + ",";
                    }
                }
                if (MailCC.Length > 1)
                    MailCC = MailCC.Substring(0, MailCC.Length - 1);



                if (clientId == 50)
                {
                    flag = mail.SendMail(DisplayName, From, ToMail, MailCC, Subject, body);
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

        void BindMasterCheckStatusForSpoc(string Department)
        {
            try
            {

                ViewBag.CheckId = repoPQPersonal.GetMastercheckIdForSpoc(Department);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ManageFundApprovelforSpoc(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", byte tatdays = 0)
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                short clientid = 0;
                if (cr != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                }

                int pageSize = 20;
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                //ClientRowID = Convert.ToInt16(Request.QueryString["cr"].ToString());
                //if (!string.IsNullOrEmpty(CaseStatus) && (CaseStatus == "Pendingwithcandidate" || CaseStatus == "PendingwithHR"))
                //{
                //    model = repoDashboard.GetClientServicingDashBoardInfo1(page, pageSize, Search, teamMemerRowId, CaseStatus, ClientRowID);
                //}
                //else
                //{                 

                model = repoDashboard.GetClientServicingDashBoardInfoFundApprovespoc(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid);
                //}
                Session["CaseStatus"] = CaseStatus;
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                BindClientListByLogInName(SpocEmail);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult FundApproveBySpoc(int PersonalId = 0, int VerId = 0, short chkFmlyId = 0, short SubchkFmlyId = 0)
        {
            FundPendingApprovalViewModel model = new FundPendingApprovalViewModel();
            try
            {
                if (PersonalId > 0 && VerId > 0 && chkFmlyId > 0 && SubchkFmlyId > 0)
                {
                    model.PersonalRowID = PersonalId;
                    model.VerificationRowID = VerId;
                    model.CheckFamilyRowID = chkFmlyId;
                    model.SubCheckRowID = SubchkFmlyId;
                }
                else
                {
                    if (PersonalId > 0 && SubchkFmlyId > 0)
                    {
                        model.PersonalRowID = PersonalId;
                        model.SubCheckRowID = SubchkFmlyId;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult FundApproveBySpoc(FundPendingApprovalViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.PersonalRowID > 0 && model.VerificationRowID > 0 && model.CheckFamilyRowID > 0 && model.SubCheckRowID > 0)
                    {
                        short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                        model.CheckStatus = "WIP-Pending";
                        model.ModifiedBy = ClientRowID;
                        model.ModifiedDate = DateTime.Now;
                        repoVerificationInfo.FundPendingChecksUpdateByDepartmentTL(model);

                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                        #endregion

                        cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 61, model.Remarks, "Client", Convert.ToInt16(Session["ClientRowID"]));
                        cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 61, "Client", Convert.ToInt16(Session["ClientRowID"]));
                        ModelState.Clear();
                        ViewBag.SuccessMsg = "Record Updated Successfully!";

                        //TempData["SuccessMsg"] = "Record Updated Successfully!";
                        //RedirectToAction("ManageFundPendingChecksForDeprtTL");
                    }
                    else
                    {
                        if (model.PersonalRowID > 0 && model.SubCheckRowID > 0)
                        {

                            // 61 check status for funds request closed by client
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 61, model.Remarks, "Client", Convert.ToInt16(Session["ClientRowID"]));
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 61, "Client", Convert.ToInt16(Session["ClientRowID"]));
                            #region Check Action History Save                           
                            clsCommonMethods.AddCheckActionHistoryByClient(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, model.Remarks);
                            #endregion
                            //SendAlertMassageForFundApproval(model.PersonalRowID, model.SubCheckRowID,model.Remarks);
                            //SendAlertMassageForFundApprovalMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks);
                            ModelState.Clear();
                            ViewBag.SuccessMsg = "Record Updated Successfully!";
                        }
                        else
                            ViewBag.ErrorMsg = "All fields are mandatory!";
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
        bool SendAlertMassageForFundApprovalMail(int PersonalRowID, short SubCheckRowID, string remarks, string fundStatus = "", string AlertType = "", int PackageId = 0, int MailType = 0)
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
                var clientId = Convert.ToInt32(ds.Tables[0].Rows[0]["ClientRowId"]);
                string MailSubject = "";
                var templateDB = cc.getAlertMailtemplate(clientId, AlertType, PackageId, MailType, "fundApproval");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }
                //using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/fundApproval.html")))
                //{
                //    body = reader.ReadToEnd();
                //}
                string CandidateName = Convert.ToString(ds.Tables[0].Rows[0]["CandidateName"]);
                string CompanyRefNo = Convert.ToString(ds.Tables[0].Rows[0]["CompanyRefNo"]);
                string SubCheckName = Convert.ToString(ds.Tables[0].Rows[0]["SubCheckName"]);
                string TeamEmailID = Convert.ToString(ds.Tables[0].Rows[0]["OperationEmailID"]);
                // string SpocEmailID = "";//Convert.ToString(ds.Tables[0].Rows[0]["SpocEmailID"]);
                string SpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["SpocEmailID"]);
                string ToMailId = Convert.ToString(ds.Tables[0].Rows[0]["SendFundsApprovedMail"]);
                string ClientSpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["ClientSpocEmailID"]);


                //var dtEmail = cc.GetEmailIdForAlert(clientId, 0, "Funds Approval");
                //if (dtEmail.Rows.Count > 0)
                //{
                //    SpocEmailID = Convert.ToString(dtEmail.Rows[0]["SpocEmailID"]);
                //}

                body = body.Replace("{FCandidateName}", CandidateName);
                body = body.Replace("{FCheckName}", SubCheckName);
                body = body.Replace("{FPRemarks}", remarks);
                body = body.Replace("{FRefNo}", CompanyRefNo);

                body = body.Replace("{MailSignature}", MailSignature);

                string Subject = "";
                if (fundStatus == "APPROVED")
                {
                    Subject = "Funds Approved " + CompanyRefNo + " " + CandidateName + "  " + SubCheckName;
                }
                else
                {
                    Subject = "Funds Rejected " + CompanyRefNo + " " + CandidateName + "  " + SubCheckName;
                }
                string To = TeamEmailID + "," + SpocEmailID;
                string MailCC = TeamEmailID + "," + SpocEmailID;
                if (clientId == 50)
                {
                    To = ToMailId;
                }
                string ToMail = To.ToLower();
                bool flag;
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

                MailCC = MailCC.ToLower();
                MailCC = MailCC.Replace(",", ";");
                string[] arCCMail = MailCC.Split(';');
                MailCC = "";
                foreach (string a in arCCMail)
                {
                    if (!MailCC.Contains(a))
                    {
                        MailCC = MailCC + a.Trim() + ",";
                    }
                }
                if (MailCC.Length > 1)
                    MailCC = MailCC.Substring(0, MailCC.Length - 1);



                if (clientId == 50)
                {
                    flag = mail.SendMail(DisplayName, From, ToMail, MailCC, Subject, body);
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

        public void CheckwiseMIS_clientwise(string ClientRowId, string dtf, string dtt)
        {
            //int ClientRowID = 0;
            //if (ClientRowId != "")
            //{
            //     ClientRowID = Convert.ToInt32(clsCommonMethods.PDecrypt(ClientRowId));
            //}

            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            var strCommaSeparate = GetCommaSeparateClientRowID(ClientRowId);
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();

                //ds = cc.CheckWiseMIS_CS(ClientRowID, teamMemerRowId);
                ds = cc.CheckWiseMIS_CS_MultipleClient(strCommaSeparate, teamMemerRowId, dtf, dtt);
                if (ds.Tables.Count > 0)
                {
                    var dt = new DataTable("Details");
                    dt = ds.Tables[0];
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
                        Response.AddHeader("content-disposition", "attachment;filename=Check_MIS.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [ValidateInput(false)]
        public ActionResult InterimRequest(string Search = "")
        {

            ClientCustomize cc = new ClientCustomize();
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                int pageSize = 20;
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                if (Search != "")
                {
                    DataTable dt = cc.InterimRequest(Search);
                    if (dt.Rows.Count > 0)
                    {
                        TempData["ErrorMsg"] = "Already  interim request Open!";
                    }
                    //if(dt.Rows.Count == 0)
                    //{
                    //    TempData["ErrorMsg"] = "Interim request is not genrate!";

                    //}
                    else
                    {
                        DataTable dts = cc.GetReferenceDetailforYTR(Search);
                        if (dts.Rows.Count > 0)
                        {
                            int clientRowId = 0;
                            clientRowId = Convert.ToInt32(dts.Rows[0]["ClientRowID"].ToString());
                            if (clientRowId == 22 || clientRowId == 331 || clientRowId == 332 || clientRowId == 333 || clientRowId == 587)
                            {
                                TempData["ErrorMsg"] = "Interim Request not Generated because same request add-on RWQC !";
                            }
                            else
                            {
                                cc.Insert_InterimRequest(Search);
                                TempData["ErrorMsg"] = "Genrate interim request successfully !";
                                cc.AutoAllocateInterimReportToRWTM(Search);
                            }


                        }
                        else
                        {
                            TempData["ErrorMsg"] = "No record found !";

                        }

                    }



                }

            }
            catch (Exception)
            {

                throw;
            }

            return View(model);
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult InvestigationGetRWQCTeamMemberDashBoardInfo(int page = 1, string sort = "", string sortDir = "desc", string Search = "", short cr = 0, string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", byte tatdays = 0)
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                int pageSize = 20;
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                //ClientRowID = Convert.ToInt16(Request.QueryString["cr"].ToString());
                //if (!string.IsNullOrEmpty(CaseStatus) && (CaseStatus == "Pendingwithcandidate" || CaseStatus == "PendingwithHR"))
                //{
                //    model = repoDashboard.GetClientServicingDashBoardInfo1(page, pageSize, Search, teamMemerRowId, CaseStatus, ClientRowID);
                //}
                //else
                //{                 

                model = repoDashboard.GetClientServicingDashBoardInfoInvestigation(page, pageSize, Search, teamMemerRowId, CaseStatus, cr);
                //}

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                BindClientListByLogInName(SpocEmail);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult InvestigationUploadReportPDFFileBYRWTM(int id = 0, string Report_Type = "", string ReportStatus = "")
        {
            InterimReportGenerateViewModel model = new InterimReportGenerateViewModel();
            try
            {
                Session["id"] = id;
                Session["Report_Type"] = Report_Type;
                string BA = "";
                DataTable DT = new DataTable();
                cc = new ClientCustomize();

                model = repoReportQC.GenerateCandidateInterimReportByReportQCTM(id);
                Session["ClientRowID"] = model.ClientRowID;
                //get resolution for client 
                int resolutionDPI = cc.getResolutionForReport(model.ClientRowID);
                CandidatePdfLists(id, resolutionDPI, model.ClientRowID);
                //CandidatePdfListsNew(id);
                // CandidateCasePdfListsNew(id);
                CandidateCasePdfLists(id, resolutionDPI);
                Session["SecritadRefId"] = model.CompanyRefNo;
                model.CandidateCaseDocList = repoReportQC.GetCandidateCaseDocListForReport(id);
                model.ReportDate = DateTime.Now;
                model.Report_Type = Report_Type;
                model.DocPath = ConfigurationManager.AppSettings["provideddocuments"].ToString();

                model.CandidateDocList = repoReportQC.GetCandidateDocListForReport(id);
                DT = cc.GetColorCode(model.PersonalRowID, model.ClientRowID);
                if (DT.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DT.Rows[0]["colorName"].ToString()))
                    {
                        model.FinalColor = DT.Rows[0]["colorName"].ToString();
                        model.FinalColorCode = DT.Rows[0]["colorCode"].ToString();
                    }
                    else
                    {
                        model.FinalColorCode = "#fff";
                    }
                }
                model.colorCodeOrderinglist = repoReportQC.getCheckColorCodeOrdering(id);
            }
            catch { }

            return View();
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UploadReportPDFFileBYRWTM(HttpPostedFileBase file, FormCollection form)
        {
            try
            {
                string rep_status = form["ddlReportType"];
                string ColorName = form["ddlColor"];
                int userId = Convert.ToInt32(Session["TeamMemberRowID"].ToString());
                ClientCustomize cc = new ClientCustomize();
                if (rep_status != "--Select--")
                {
                    if (file != null && file.ContentLength > 0 && (System.IO.Path.GetExtension(file.FileName).ToLower() == ".pdf" ||
                        System.IO.Path.GetExtension(file.FileName).ToLower() == ".zipx" ||
                        System.IO.Path.GetExtension(file.FileName).ToLower() == ".rar" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".zip") && rep_status != "--Select--")
                    {

                        string FldPathDoc = string.Empty;
                        FldPathDoc = ConfigurationManager.AppSettings["CandidateReportdoc".Trim().ToLower()].ToString();

                        string SecuId = Convert.ToString(Session["SecritadRefId"]);
                        string ImgName = string.Empty;
                        string ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                        ImgName = SecuId + ext;
                        string fileSavePath = Server.MapPath(FldPathDoc + ImgName);
                        file.SaveAs(fileSavePath);

                        //bool k = ReadPDF(SecuId, fileSavePath, SecuId);
                        //if (k)
                        //{
                        repoReportQC.UpdatePQPersonalUploadReportDoc(SecuId, ImgName, rep_status, rep_status, ColorName, userId);
                        // SendAlertMassageForReportSubmit(SecuId, rep_status, ColorName);
                        //cc.UpdateCaseStatus(0, 30, Convert.ToInt32(Session["WebUserRowid"].ToString()), SecuId);
                        //ClientCustomize cc = new ClientCustomize();
                        //cc.UpdateCaseStatus(personalmodel.PersonalRowID,29, teamMemberId);
                        TempData["SuccessMsg"] = "Record Update Successfully!";
                        //}
                        //else
                        //{
                        //    System.IO.File.Delete(fileSavePath);
                        //    TempData["SuccessMsg"] = "Report does not match with the candidate name!. pls try with correct file";
                        //}
                    }
                    else
                    {

                        TempData["ErrorMsg"] = "Please upload file having extension .pdf only.!";
                    }
                }
                else
                {
                    TempData["ErrorMsg"] = "Please select report type!";
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return RedirectToAction("InvestigationGetRWQCTeamMemberDashBoardInfo");
        }

        [NonAction]
        public bool ReadPDF(string pdfFileName, string fileP, string dbRefno)
        {
            bool retVal = false;
            StringBuilder text = new StringBuilder();

            try
            {
                iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileP);
                iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                string currentText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, 2, strategy);
                currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                pdfReader.Close();
                if (pdfFileName.Contains("TechM"))
                {
                    dbRefno = dbRefno.Replace("TechM", "");
                }


                if (pdfFileName.Contains("IPSoft"))
                {
                    pdfFileName = pdfFileName.Replace("IPSoft", "");
                    dbRefno = dbRefno.Replace("IPSoft", "");
                }
                if (currentText.Contains(dbRefno))
                    retVal = true;
            }
            catch
            {
                if (pdfFileName == dbRefno)
                    retVal = true;
                else
                    retVal = false;

                if (dbRefno.Contains("TCS"))
                    retVal = true;


            }
            return retVal;
        }


        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ManageColorCodeReviewforSpoc(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", byte tatdays = 0, string msg = "")
        {
            if (msg != "")
            {
                ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
            }
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                short clientid = 0;
                if (cr != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                }

                int pageSize = 20;
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                Session["CaseStatus"] = CaseStatus.ToString();
                model = repoDashboard.GetClientServicingDashBoardInfoColorcodeReview(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid);

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                BindClientListByLogInName(SpocEmail);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult ManageColorCodeReviewforSpocExportExcel()
        {
            ClientCustomize cc = new ClientCustomize();
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            DataTable dt = new DataTable();
            dt = cc.ColorCodeExporttoExcelSpoc(0, "ColorCodeReview", teamMemerRowId, "");
            ExportToExcel(dt, "ColorCodeReview");
            return RedirectToAction("ManageColorCodeReviewforSpoc", new { CaseStatus = "ColorCodeReview", /*cr = clsCommonMethods.PEncrypt(item.ClientRowID.ToString()),*/ tmr = Session["Teammemberrowid"].ToString() });
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ColorCodeReviewApproveBySpoc(int PersonalId = 0, int VerId = 0, short chkFmlyId = 0, short SubchkFmlyId = 0)
        {
            FundPendingApprovalViewModel model = new FundPendingApprovalViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                DataTable dt;
                dt = cc.GetscolorCodereview(PersonalId);
                if (dt.Rows.Count > 0)
                {

                    model.Remarks = dt.Rows[0]["Remarks"].ToString();
                    model.PersonalRowID = PersonalId;

                }


            }
            catch (Exception)
            {

                throw;
            }
            return View(model);
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult BulkColorCodeReviewApproveBySpoc(string hdnSelected)
        //{
        //    int ClientRowID = 0;
        //    try
        //    {

        //        string CompanyRefNo = "";
        //        string ColorCode = "";
        //        string ReportType = "";
        //        ClientCustomize cc = new ClientCustomize();
        //        if (hdnSelected != "")
        //        {
        //            string[] Personalrowid = hdnSelected.Split(',');
        //            foreach (string id in Personalrowid)
        //            {
        //                int personalid = Convert.ToInt32(id);
        //                if (personalid > 0)
        //                {
        //                    int spocId = Convert.ToInt16(Session["TeamMemberRowID"]);
        //                    //model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
        //                    //model.ModifiedDate = DateTime.Now;
        //                    // 30 check status report close
        //                    cc.Process_Remarks_Insert(personalid, 0, 30, "Approve", "Spoc", spocId);

        //                    #region Save LogTransaction Details
        //                    clsCommonMethods.AddPQLogTrasaction(personalid, "Color Code Approved", "Approve", "", "Color Code Approved");
        //                    #endregion

        //                    #region Case/Check Action History Save 
        //                    clsCommonMethods.AddCaseActionHistory(personalid, "Color Code Approved", "Approve");
        //                    #endregion
        //                    cc.InsertColorCodeReview(personalid, 107, "", "", 0, spocId, "Approve");
        //                    DataTable dt;
        //                    dt = cc.GetColorcodedateformail(personalid);
        //                    if (dt.Rows.Count > 0)
        //                    {

        //                        CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();
        //                        ColorCode = dt.Rows[0]["ColorCode"].ToString();
        //                        ReportType = dt.Rows[0]["ReportType"].ToString();
        //                        ClientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"].ToString());
        //                        SendAlertMassageForReportSubmit(CompanyRefNo, ReportType, ColorCode, "Report", 0, 1);
        //                    }


        //                    #region UpdateReportInAPI
        //                    string imgname = (clsCommonMethods.PEncrypt(CompanyRefNo));
        //                    string PEncryptedClientRowID = (clsCommonMethods.PEncrypt(ClientRowID.ToString()));
        //                    new API().UpdateReportInAPI(personalid, ClientRowID.ToString(), ReportType, ColorCode, imgname, PEncryptedClientRowID, "Approve", ReportType);
        //                    #endregion

        //                    ModelState.Clear();
        //                    ViewBag.ErrorMsg = "Color Code Approved successufly";
        //                    return RedirectToAction("ManageColorCodeReviewforSpoc", "ClientServicing", new { CaseStatus = "ColorCodeReview", cr = clsCommonMethods.PEncrypt(ClientRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.ErrorMsg = "Please Checked the checkbox!";
        //            return RedirectToAction("ManageColorCodeReviewforSpoc", "ClientServicing", new { CaseStatus = "ColorCodeReview", cr = clsCommonMethods.PEncrypt(ClientRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.ErrorMsg = Ex.Message;
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //    return RedirectToAction("ManageColorCodeReviewforSpoc", "ClientServicing", new { CaseStatus = "ColorCodeReview", cr = clsCommonMethods.PEncrypt(ClientRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });

        //}

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BulkColorCodeReviewApproveBySpoc(string hdnSelected)
        {
            int ClientRowID = 0;
            try
            {

                string CompanyRefNo = "";
                string ColorCode = "";
                string ReportType = "";
                ClientCustomize cc = new ClientCustomize();
                if (hdnSelected != "")
                {
                    string[] Personalrowid = hdnSelected.Split(',');
                    int spocId = Convert.ToInt16(Session["TeamMemberRowID"]);
                    int approvedCount = 0;

                    foreach (string id in Personalrowid)
                    {
                        int personalid = Convert.ToInt32(id);
                        if (personalid > 0)
                        {
                            cc.Process_Remarks_Insert(personalid, 0, 30, "Approve", "Spoc", spocId);



                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(personalid, "Color Code Approved", "Approve", "", "Color Code Approved");
                            #endregion

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCaseActionHistory(personalid, "Color Code Approved", "Approve");
                            #endregion
                            cc.InsertColorCodeReview(personalid, 107, "", "", 0, spocId, "Approve");
                            DataTable dt = cc.GetColorcodedateformail(personalid);
                            if (dt.Rows.Count > 0)
                            {
                                CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();
                                ColorCode = dt.Rows[0]["ColorCode"].ToString();
                                ReportType = dt.Rows[0]["ReportType"].ToString();
                                ClientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"].ToString());
                                SendAlertMassageForReportSubmit(CompanyRefNo, ReportType, ColorCode, "Report", 0, 1);




                                #region UpdateReportInAPI
                                string imgname = (clsCommonMethods.PEncrypt(CompanyRefNo));
                                string PEncryptedClientRowID = (clsCommonMethods.PEncrypt(ClientRowID.ToString()));
                                new API().UpdateReportInAPI(personalid, ClientRowID.ToString(), ReportType, ColorCode, imgname, PEncryptedClientRowID, "Approve", ReportType);
                                #endregion
                            }

                            approvedCount++;

                            //ModelState.Clear();
                            //ViewBag.ErrorMsg = "Color Code Approved successufly";
                            ////return RedirectToAction("ManageColorCodeReviewforSpoc", "ClientServicing", new { CaseStatus = "ColorCodeReview", cr = clsCommonMethods.PEncrypt(ClientRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                        }
                    }

                    ViewBag.ErrorMsg = $"{approvedCount} cases approved successfully";

                }

                else
                {
                    ViewBag.ErrorMsg = "Please Checked the checkbox!";
                    return RedirectToAction("ManageColorCodeReviewforSpoc", "ClientServicing", new { CaseStatus = "ColorCodeReview", cr = clsCommonMethods.PEncrypt(ClientRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return RedirectToAction("ManageColorCodeReviewforSpoc", "ClientServicing", new { CaseStatus = "ColorCodeReview", cr = clsCommonMethods.PEncrypt(ClientRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });

        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ColorCodeReviewApproveBySpoc(FundPendingApprovalViewModel model)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();

                if (ModelState.IsValid)
                {
                    if (model.PersonalRowID > 0 && model.FundsStatus == "Reject")
                    {
                        string clientrowid = "";
                        string reporttype = "";
                        string Action = "";
                        int spocId = Convert.ToInt16(Session["TeamMemberRowID"]);
                        ClientCustomize cc1 = new ClientCustomize();
                        DataTable dttt1 = cc1.GetReportTypeAndActionQcPending(model.PersonalRowID);
                        if (dttt1.Rows.Count > 0)
                        {
                            clientrowid = dttt1.Rows[0]["ClientRowID"].ToString();
                            reporttype = dttt1.Rows[0]["ReportType"].ToString();
                            Action = dttt1.Rows[0]["ActionQcPending"].ToString();

                        }

                        if ((clientrowid == "268" || clientrowid == "3") && Action == "QcPendingApproved")
                        {

                            //int spocId = Convert.ToInt16(Session["TeamMemberRowID"]);
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 108, model.Remarks, "Spoc", spocId);
                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "Color Code Reject", "Reject", "", "Color Code Reject");
                            #endregion
                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCaseActionHistory(model.PersonalRowID, "QcPendingColorCodeRejected", "QcPendingColorCodeRejected");
                            #endregion
                            cc.updateTCSDBReport(model.PersonalRowID, "QcPendingColorCodeRejected");

                        }

                        else
                        {
                            model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                            model.ModifiedDate = DateTime.Now;
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 108, model.Remarks, "Spoc", spocId);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "Color Code Reject", "Reject", "", "Color Code Reject");
                            #endregion

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCaseActionHistory(model.PersonalRowID, "Color Code Reject", model.Remarks);
                            #endregion

                            cc.InsertColorCodeReview(model.PersonalRowID, 108, "", "", 0, spocId, model.Remarks);


                        }


                        ModelState.Clear();
                        ViewBag.SuccessMsg = "Record Updated Successfully!";


                    }
                    else
                    {
                        if (model.PersonalRowID > 0 && model.FundsStatus == "Approve")
                        {


                            string clientrowid = "";
                            string reporttype = "";
                            string Action = "";
                            cc.InsertReportEmailSentDetails(model.PersonalRowID, "Email Function", "Approve");
                            int spocId = Convert.ToInt16(Session["TeamMemberRowID"]);
                            ClientCustomize cc1 = new ClientCustomize();
                            DataTable dttt = cc1.GetReportTypeAndActionQcPending(model.PersonalRowID);
                            if (dttt.Rows.Count > 0)
                            {
                                clientrowid = dttt.Rows[0]["ClientRowID"].ToString();
                                reporttype = dttt.Rows[0]["ReportType"].ToString();
                                Action = dttt.Rows[0]["ActionQcPending"].ToString();

                            }

                            if ((clientrowid == "268" || clientrowid == "3") && Action == "QcPendingApproved")
                            {
                                //int spocId = Convert.ToInt16(Session["TeamMemberRowID"]);
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 107, model.Remarks, "Spoc", spocId);
                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "Color Code Reject", "Reject", "", "Color Code Reject");
                                #endregion
                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCaseActionHistory(model.PersonalRowID, "QcPendingColorCodeApproved", "QcPendingColorCodeApproved");
                                #endregion
                                cc.updateTCSDBReport(model.PersonalRowID, "QcPendingColorCodeApproved");

                            }
                            else
                            {
                                model.ModifiedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                                model.ModifiedDate = DateTime.Now;
                                int ClientRowID = 0;
                                // 30 check status report close
                                cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 30, model.Remarks, "Spoc", spocId);

                                #region Save LogTransaction Details
                                clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "Color Code Approved", "Approve", "", "Color Code Approved");
                                #endregion

                                #region Case/Check Action History Save 
                                clsCommonMethods.AddCaseActionHistory(model.PersonalRowID, "Color Code Approved", model.Remarks);
                                #endregion
                                cc.InsertColorCodeReview(model.PersonalRowID, 107, "", "", 0, spocId, model.Remarks);
                                DataTable dt;
                                dt = cc.GetColorcodedateformail(model.PersonalRowID);
                                if (dt.Rows.Count > 0)
                                {

                                    model.CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();
                                    model.ColorCode = dt.Rows[0]["ColorCode"].ToString();
                                    model.ReportType = dt.Rows[0]["ReportType"].ToString();
                                    ClientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"].ToString());
                                    SendAlertMassageForReportSubmit(model.CompanyRefNo, model.ReportType, model.ColorCode, "Report", 0, 1);
                                }


                                #region UpdateReportInAPI
                                string imgname = (clsCommonMethods.PEncrypt(model.CompanyRefNo));
                                string PEncryptedClientRowID = (clsCommonMethods.PEncrypt(ClientRowID.ToString()));
                                new API().UpdateReportInAPI(model.PersonalRowID, ClientRowID.ToString(), model.ReportType, model.ColorCode, imgname, PEncryptedClientRowID, model.Remarks, model.ReportType);
                                #endregion
                            }




                            ModelState.Clear();
                            ViewBag.SuccessMsg = "Record Updated Successfully!";
                        }
                        else
                            ViewBag.ErrorMsg = "All fields are mandatory!";
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
                cc.InsertReportEmailSentDetails(model.PersonalRowID, "Email Function", Ex.Message);
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        bool SendAlertMassageForReportSubmit(string companyRefNo, string reportType, string colorCode, string AlertType = "", int PackageId = 0, int MailType = 0)
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
                    " | Direct Tel: ", signature.PhoneNo, " | Fax: ", signature.FaxNo, "</b><br /><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", signature.GeneralEmail, "'>", "bgv.reports@securitas-india.com", "</a>");
            }

            From = signature.GeneralEmail;
            DisplayName = signature.CompanyName.Trim();

            ClientCustomize cc = new ClientCustomize();
            DataSet ds = new DataSet();
            ds = cc.getAlertMailingForReportSubmit(companyRefNo);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string body = string.Empty;
                int clientrowid = Convert.ToInt32(ds.Tables[0].Rows[0]["clientrowid"]);
                string MailSubject = "";
                var templateDB = cc.getAlertMailtemplate(clientrowid, AlertType, PackageId, MailType, "ReportSubmitTemplate");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }
                string CandidateName = Convert.ToString(ds.Tables[0].Rows[0]["CandidateName"]);
                string CompanyRefNo = Convert.ToString(ds.Tables[0].Rows[0]["CompanyRefNo"]);
                string SpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["SpocEmailID"]);
                string ClientSpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["ClientSpocEmailID"]);
                string EmployeeID = Convert.ToString(ds.Tables[0].Rows[0]["EmployeeID"]);
                int personalRowid = Convert.ToInt32(ds.Tables[0].Rows[0]["personalrowid"]);
                int ReportTeammemberid = Convert.ToInt32(ds.Tables[0].Rows[0]["ReportTeammemberid"]);
                string TNLID = Convert.ToString(ds.Tables[0].Rows[0]["TNLID"]);
                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(personalRowid);
                string spocEmail = "";
                string cSpocEmail = "";
                string toMailId = "";
                DataTable dtEmail = new DataTable();
                dtEmail = cc.GetEmailStructureForAlert(clientrowid, CandidateData.ClientUserRowId, "Report", personalRowid);
                var dtEmailNew = cc.GetEmailIdForAlert(clientrowid, CandidateData.ClientUserRowId, "Report");
                if (dtEmail.Rows.Count > 0)
                {
                    spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                    cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                    if (clientrowid == 503)
                    {
                        cSpocEmail = cSpocEmail.Replace("ankit.sagar@uniqlo.in", "");
                    }
                    if (clientrowid == 649)
                    {
                        cSpocEmail = cSpocEmail + "," + "Prakriti.Bansal@ageasfederal.com";
                    }
                }
                if (dtEmailNew.Rows.Count > 0)
                {
                    toMailId = dtEmailNew.Rows[0]["SendReportEmail"].ToString();
                }
                body = body.Replace("{FCandidateName}", CandidateName);
                body = body.Replace("{FReportType}", reportType);
                body = body.Replace("{FReportColor}", colorCode);
                body = body.Replace("{FRefNo}", CompanyRefNo);
                body = body.Replace("{FEmpcode}", EmployeeID);
                body = body.Replace("{FTNLID}", TNLID);
                body = body.Replace("{MailSignature}", MailSignature);
                if (clientrowid == 249)
                {
                    string Subject = reportType + " " + colorCode + " Report uploaded for " + CandidateName + "  " + CompanyRefNo + " Emp code " + EmployeeID;
                    string To = spocEmail.Replace(";", ",") + "," + cSpocEmail.Replace(";", ",");
                    if (To.Substring(To.Length - 1) == ",")
                        To = To.Substring(0, To.Length - 1);

                    bool flag = mail.SendMail_Report(DisplayName, From, To, Subject, body, personalRowid);
                    if (flag)
                    {
                        result = true;
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Mail sending failed!";
                        cc.InsertReportEmailSentDetails(personalRowid, "Email Function", "Mail sending failed!" + body);
                    }
                }
                else if (clientrowid == 50)
                {
                    string Subject = "";
                    Subject = reportType + " " + colorCode + " Report uploaded for " + CandidateName + "  " + CompanyRefNo;
                    string MailCC = spocEmail.Replace(";", ",") + "," + cSpocEmail.Replace(";", ",");

                    string ToMail = toMailId.ToLower();
                    bool flag;
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

                    MailCC = MailCC.ToLower();
                    MailCC = MailCC.Replace(",", ";");
                    string[] arCCMail = MailCC.Split(';');
                    MailCC = "";
                    foreach (string a in arCCMail)
                    {
                        if (!MailCC.Contains(a))
                        {
                            MailCC = MailCC + a.Trim() + ",";
                        }
                    }
                    if (MailCC.Length > 1)
                        MailCC = MailCC.Substring(0, MailCC.Length - 1);


                    flag = mail.SendMail(DisplayName, From, ToMail, MailCC, Subject, body, personalRowid);
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
                    string Subject = "";
                    if (clientrowid == 649 || clientrowid == 580)
                    {
                        if (EmployeeID != "")
                        {
                            Subject = reportType + " " + colorCode + " Report uploaded for " + CandidateName + "  " + CompanyRefNo + " for Employeeid " + EmployeeID;
                        }
                        else
                        {
                            Subject = reportType + " " + colorCode + " Report uploaded for " + CandidateName + "  " + CompanyRefNo;
                        }
                    }
                    else
                    {
                        Subject = reportType + " " + colorCode + " Report uploaded for " + CandidateName + "  " + CompanyRefNo;
                    }
                    string To = spocEmail.Replace(";", ",") + "," + cSpocEmail.Replace(";", ",");
                    if (To.Substring(To.Length - 1) == ",")
                        To = To.Substring(0, To.Length - 1);
                    bool flag = false;
                    if (CandidateData.ClientRowID == 257)
                    {
                        string bcc = "";
                        string Tos = "";
                        Tos = spocEmail;
                        bcc = cSpocEmail;
                        flag = mail.SendMail(DisplayName, From, Tos, "", bcc, Subject, body);
                    }
                    if (CandidateData.ClientRowID == 497)
                    {

                        string Ccs = "";
                        string Tos = "";
                        Tos = spocEmail /*+ "," + "rblonboarding@tmirpo.com" + "," + "madhuram@tminetwork.com" + "," + "rakeshy@tminetwork.com" + "," + "pallavis@tminetwork.com" + "," + "Sushil.Lokhande@rblbank.com"*/;
                        Ccs = cSpocEmail;
                        flag = mail.SendMail(DisplayName, From, Tos, Ccs, Subject, body, personalRowid);
                    }
                    else
                    {
                        flag = mail.SendMail_Report(DisplayName, From, To, Subject, body, personalRowid);
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

            }
            else
            {
                TempData["ErrorMsg"] = "Candidate Check details not found!";
            }
            return result;
        }


        [NonAction]
        public string GatZipFileMessage(HttpPostedFileBase postedFile, string clientRowid, string filenm)
        {
            string NowTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            GetMimeTypeTrue MimeType = new GetMimeTypeTrue();
            string ErrorMessage = "";
            string StartupPath = Server.MapPath("~/Uploads/TempFile/");
            StartupPath += clientRowid + "_" + filenm + "_" + NowTime;
            string TempPath = Server.MapPath("~/Uploads/");
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
                                if (!Directory.Exists(StartupPath + "/"))
                                {
                                    Directory.CreateDirectory(StartupPath);
                                }
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


        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult Reverification()
        {
            ViewBag.SearchPress = 0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Reverification(CheckAddAndReverification model, FormCollection form, string Command)
        {
            cc = new ClientCustomize();
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string[] strSubcheckRowId = null;

            if (model.SecuritasID != null && Command == "Search")
            {

                DataTable dt = (cc.getcasestatusid(model.SecuritasID));


                if (dt.Rows[0][0].ToString() == "6")
                {
                    return View();
                }
                else
                {

                    if (!string.IsNullOrEmpty(model.SecuritasID))
                    {
                        model.ClientRowId = BindCheckList(model.SecuritasID).ToString();
                        BindSubcheckname(model.SecuritasID);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "please fill Company Ref No.";
                    }

                }

                ViewBag.SearchPress = 1;
            }
            if (Command == "Submit")
            {
                foreach (HttpPostedFileBase file in model.UploadDoc)
                {
                    string errorMessage = GatZipFileMessage(file, model.PersonalRowID.ToString(), model.ClientRowId.ToString());
                    if (errorMessage != "")
                    {
                        ViewBag.ErrorMsg += errorMessage;
                        return View(model);
                    }
                }
                ViewBag.SearchPress = 1;
            }


            if (string.IsNullOrEmpty(model.SecuritasID))
            {
                ViewBag.ErrorMsg = "please fill Company Ref No.";
            }
            if (model.PersonalRowID != 0 && Command == "Submit")
            {
                if (string.IsNullOrEmpty(model.Remark))
                {
                    ViewBag.ErrorMsg = "Please fill Remark";
                }

                else if (string.IsNullOrEmpty(model.Remark) && model.Actions == "Add Check")
                {
                    ViewBag.ErrorMsg = "Please fill Remark";
                }
                else if (model.UploadDoc[0] == null && model.Actions == "Add Check")
                {
                    ViewBag.ErrorMsg = "Please select doc";
                }

                if (model.Actions == "ReVerification")
                {

                    strSubcheckRowId = model.rSubCheckRowIDMultiple.Split(',');
                }
                else if (model.Actions == "Add Check")
                {

                    strSubcheckRowId = model.aSubCheckRowIDMultiple.Split(',');
                }
                string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                string proImg = string.Empty;
                int i = 1;
                if (model.UploadDoc[0] != null)
                {
                    foreach (HttpPostedFileBase file in model.UploadDoc) // loop for adding add from dataset to list<modeldata>
                    {
                        AddTempPQVerifiedUploadDocViewModel mm = new AddTempPQVerifiedUploadDocViewModel();

                        if (model.Actions == "Add Check")
                        {
                            mm.DocumentType = "AddCheck";
                            mm.FileName = file.FileName;
                            mm.DocumentUploadFrom = "AddCheck";
                            mm.Status = 1;
                            mm.UploadDate = DateTime.Now;
                            mm.PersonalRowID = model.PersonalRowID;
                            //mm.SubCheckRowID = Convert.ToInt16(model.sub);
                            mm.Remarks = model.Remark;

                        }
                        else
                        {
                            mm.DocumentType = "ReVerification";
                            mm.FileName = file.FileName;
                            mm.DocumentUploadFrom = "ReVerification";
                            mm.Status = 1;
                            mm.UploadDate = DateTime.Now;
                            mm.PersonalRowID = model.PersonalRowID;
                            //mm.SubCheckRowID = Convert.ToInt16(model.sub);
                            mm.Remarks = model.Remark;

                        }

                        string ext = Path.GetExtension(file.FileName).ToLower();
                        for (int j = 0; j < strSubcheckRowId.Length; j++)
                        {
                            proImg = (Convert.ToInt32(strSubcheckRowId[j]) + "_" + model.PersonalRowID + "_" + mm.UploadDate.Value.ToString("ddmmyyyyHH:mm:ss:f") + "_" + mm.DocumentType + "" + i).Replace("'", "_").Replace("&", "_").Replace(":", "_").Replace("-", "_").Replace(" ", "_") + ext;
                            model.FileName = Path.GetFileName(proImg);
                            model.FilePath = FldPathDoc;
                            cc.insertPartnerdoc(model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]), model.FileName, model.FilePath, mm.DocumentType);
                            file.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        }

                        ++i;
                    }
                }

                if (model.Actions == "ReVerification" && strSubcheckRowId.Length >= 1 && model.Remark != null)
                {
                    for (int j = 0; j < strSubcheckRowId.Length; j++)
                    {
                        cc.SpocReverificationProcess(model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]), "ReVerification", teamMemerRowId, model.ClientRowId, model.Remark, model.BillingStatus, model.approveStatus);
                        //===========================================
                        var SubCheckDT = cc.GetCheckDetailsnew(Convert.ToInt32(model.PersonalRowID), Convert.ToInt32(strSubcheckRowId[j]));
                        if (SubCheckDT.Rows.Count > 0)
                        {
                            var tmid = SubCheckDT.Rows[0]["TMAllocatedTo"].ToString();
                            if (tmid != null && tmid != "0" && tmid != "")
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]), 1, 20, "Check", teamMemerRowId);
                            }
                            else
                            {
                                cc.UpdateCheckStatus(model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]), 1, 16, "Check", teamMemerRowId);
                            }

                        }
                        //================================================

                        if (model.ClientRowId == "249" || model.ClientRowId == "277" || model.ClientRowId == "990")
                        {
                            new API().UpdateAPICheckStatus(model.ClientRowId, model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]), "Reverification");
                        }
                        cc.Process_Remarks_Insert(model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]), 20, model.Remark, "Reverification by spoc", teamMemerRowId);
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, Convert.ToInt16(strSubcheckRowId[j]), "Reverification by spoc", model.Remark);
                    }
                    cc.CheckMovementProcessCompletionDate(model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[0]), "ReVerificationRaiseDate");
                    ViewBag.SuccessMsg = "Re-Verification check successfully!";

                }
                else if (model.Actions == "Add Check" && strSubcheckRowId.Length >= 1 && model.UploadDoc[0] != null && model.Remark != null)
                {
                    for (int j = 0; j < strSubcheckRowId.Length; j++)
                    {

                        int status = cc.GetcheckExists(model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]));
                        if (status == 1)
                        {
                            ViewBag.SuccessMsg = "Check Already Exist! please check status";

                        }
                        else
                        {
                            cc.SpocReverificationProcess(model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]), "Add Check", teamMemerRowId, model.ClientRowId, model.Remark, model.BillingStatus, model.approveStatus);
                            if (model.ClientRowId == "990")
                            {
                                if (model.Actions == "Add Check")
                                {
                                    cc.InsertClientCheckMapDetails(model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]));
                                }
                            }

                            if (model.ClientRowId == "249" || model.ClientRowId == "277" || model.ClientRowId == "990")
                            {
                                new API().UpdateAPICheckStatus(model.ClientRowId, model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]), "Spoc-AddCheck");
                            }
                            cc.Process_Remarks_Insert(model.PersonalRowID, Convert.ToInt32(strSubcheckRowId[j]), 8, model.Remark, "Add Check", teamMemerRowId);
                            //Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, Convert.ToInt16(strSubcheckRowId[j]), "Add Check By Spoc", model.Remark);
                            ViewBag.SuccessMsg = "Add check successfully!";
                        }
                    }

                }



            }

            return View(model);
        }

        [NonAction]
        int BindCheckList(string RefNo = "")
        {
            int clientRowId = 0;
            try
            {
                var list = repoDDL.BindChecksDetails(RefNo);
                if (list.Count() >= 1)
                {
                    ViewBag.CheckList = list;
                    var a = list.ToList();
                    clientRowId = a[0].ClientRowID;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return clientRowId;
        }

        void BindSubcheckname(string RefNo)
        {
            try
            {

                ViewBag.chek = repoPQPersonal.GetSubSetchecks(RefNo);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpGet]
        public JsonResult BillableStatus(string Billable, string companyRefNo)
        {
            ClientCustomize cc = new ClientCustomize();
            DataTable dt = cc.getBillingStatus(companyRefNo);
            string msg = "";
            string PersonalRowID = "";
            string ClientRowID = "";
            //var list = repoDashboardClient.costingGraph_client(ClientRowID, Year);
            if (dt.Rows.Count > 0)
            {

                string status = dt.Rows[0]["ReOpenBilling"].ToString();
                PersonalRowID = dt.Rows[0]["PersonalRowID"].ToString();
                ClientRowID = dt.Rows[0]["ClientRowID"].ToString();

                if (status == "Billable")
                {
                    if (Billable == "Not Billable")
                    {
                        msg = "Client billing status is " + status + " Are you sure to proceed";
                    }

                }



            }



            return Json(new { billingstatus = msg, PersonalRowId = PersonalRowID, ClientRowId = ClientRowID }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddChecks(CheckAddAndReverification model)
        {
            //if (Search == "")
            //{
            //    BindSubchecknameDelete(0, 0);
            //    BindMasterCheckStatusForSpoc(Department);
            //}
            if (model.SecuritasID != null)
            {
                model.ClientRowId = BindCheckList(model.SecuritasID).ToString();
                BindSubcheckname(model.SecuritasID);
            }

            return View(model);
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult BillingApproval()
        {
            int page = 1;
            int pageSize = clsGlobalCommon.PageSize;
            ClientServiceListViewModel model = new ClientServiceListViewModel();
            model = repoDashboard.GetReverificationApproval(page, pageSize);
            return View(model);


            ////if (Search == "")
            ////{
            ////    BindSubchecknameDelete(0, 0);
            ////    BindMasterCheckStatusForSpoc(Department);
            ////}
            //if (model.SecuritasID != null)
            //{
            //    model.clientRowId = BindCheckList(model.SecuritasID);
            //    BindSubcheckname(model.SecuritasID);
            //}

            //return View();
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult CheckApprovalAction(int PersonalRowID = 0, int SubCheckid = 0, string ClientRowId = "", string Remark = "", string subcheckName = "", string BillingStatus = "")
        {
            ClientServiceListViewModel model = new ClientServiceListViewModel(); model.ReverificationApp = new ReverificationApproval();
            model.ReverificationApp.PersonalRowID = PersonalRowID;
            model.ReverificationApp.SubCheckid = SubCheckid;
            model.ReverificationApp.ClientRowId = ClientRowId;
            model.ReverificationApp.Remark = Remark;
            model.ReverificationApp.SubCheckName = subcheckName;
            model.ReverificationApp.billingStatus = BillingStatus;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CheckApprovalAction(ClientServiceListViewModel model)
        {
            int teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            ClientCustomize cc = new ClientCustomize();
            if (model.ReverificationApp.approvalAction == "Approve")
            {
                //cc.UpdateCheckStatus()
                cc.SpocReverProcessAfterApproval(model.ReverificationApp.PersonalRowID, model.ReverificationApp.SubCheckid, teamMemerRowId, model.ReverificationApp.ClientRowId, model.ReverificationApp.Remark);
            }
            else
            {
                cc.UpdateCheckStatus(model.ReverificationApp.PersonalRowID, model.ReverificationApp.SubCheckid, 1, 105, "", teamMemerRowId);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpGet]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UpdateClientEmailId(int pid = 0)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable DT = new DataTable();
                DT = cc.GetClientLoginDetails(pid);
                if (DT.Rows.Count > 0)
                {
                    ViewBag.UpdateClientEmailId = Convert.ToString(DT.Rows[0]["UserEmailID"]);
                    ViewBag.ClientUserRowID = Convert.ToString(DT.Rows[0]["ClientUserRowID"]);
                }
                else
                {
                    ViewBag.UpdateClientEmailId = "";
                    ViewBag.ClientUserRowID = 0;
                }


            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return PartialView("_UpdateClientEmailId");
        }


        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public JsonResult UpdateEmailId(int id = 0, string email = "")
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    ClientCustomize cc = new ClientCustomize();
                    cc.UpdateEmailId(id, email);
                    result = true;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ClientManagment(string Search = "")
        {
            int teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            ClientServiceListViewModel model = new ClientServiceListViewModel();

            try
            {
                int page = 1;
                int pageSize = clsGlobalCommon.PageSize;
                if (Search != "")
                {
                    model = repoDashboard.GetClientInfo(page, pageSize, Search);

                }

                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);

        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public JsonResult ClientUserActive(string checkeds = "", short id = 0)
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    if (checkeds.Trim().ToLower() == "true")
                    {
                        checkeds = "1";
                    }
                    if (checkeds.Trim().ToLower() == "false")
                    {
                        checkeds = "3";
                    }
                    ClientCustomize cc = new ClientCustomize();
                    cc.UpdateClientUser(id, checkeds);

                    result = true;

                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public JsonResult ClientReportCheckActive(string checkeds = "", int id = 0, int scheckid = 0)
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    if (checkeds.Trim().ToLower() == "true")
                    {
                        checkeds = "1";
                    }
                    if (checkeds.Trim().ToLower() == "false")
                    {
                        checkeds = "0";
                    }
                    ClientCustomize cc = new ClientCustomize();
                    cc.UpdateClientReportCheck(id, checkeds, scheckid);

                    result = true;

                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public JsonResult ClientUserReportVisible(string checkeds = "", short id = 0)
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    if (checkeds.Trim().ToLower() == "true")
                    {
                        checkeds = "1";
                    }
                    if (checkeds.Trim().ToLower() == "false")
                    {
                        checkeds = "0";
                    }
                    ClientCustomize cc = new ClientCustomize();
                    cc.UpdateFinalReportVisible(id, checkeds);

                    result = true;

                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public JsonResult ClientUserIntrimReportVisible(string checkeds = "", short id = 0)
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    if (checkeds.Trim().ToLower() == "true")
                    {
                        checkeds = "1";
                    }
                    if (checkeds.Trim().ToLower() == "false")
                    {
                        checkeds = "0";
                    }
                    ClientCustomize cc = new ClientCustomize();
                    cc.UpdateInterimReportVisible(id, checkeds);

                    result = true;

                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidateInput(false)]
        public void ExportChecksReport(string CaseStatus, int ClientRowID = 0, string CompletedFromDate = "", string CompletedToDate = "")
        {
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);

            string Designation = Session["Designation"].ToString();
            DataTable dt = new DataTable();

            try
            {
                ClientCustomize cc = new ClientCustomize();

                dt = cc.getClientServicing_Dashboard_Details(ClientRowID, CaseStatus, teamMemerRowId);



                ExportToExcel(dt, "Report");
            }


            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [ValidateInput(false)]

        public ActionResult CommonMISTracker()
        {

            BindClientList();

            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CommonMISTracker(int ClientRowID)
        {

            ClientCustomize cc = new ClientCustomize();
            BindClientList();
            try
            {
                DataTable dt = new DataTable();
                if (ClientRowID == 599)
                {
                    dt = cc.MIS_tracker_icici();
                    DataView view = new System.Data.DataView(dt);
                    DataTable dtv = view.ToTable("Details", false);

                    using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(dtv);
                        ws.Row(1).InsertRowsAbove(1);
                        ws.Tables.FirstOrDefault().ShowAutoFilter = false;

                        ws.Range("K1:z1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("K1:z1").Merge();
                        ws.Cell(1, 11).Value = "PARTNER";
                        ws.Cell(1, 11).Style.Font.FontColor = XLColor.Red;
                        ws.Cell(1, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("AA1:AH1").Style.Fill.BackgroundColor = XLColor.DarkGray;
                        ws.Range("AA1:AH1").Merge();
                        ws.Cell(1, 27).Value = "ICICI BANK LTD";
                        ws.Cell(1, 27).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("AI1:AW1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("AI1:AW1").Merge();
                        ws.Cell(1, 35).Value = "PARTNER";
                        ws.Cell(1, 35).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(1, 35).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("AX1:BE1").Style.Fill.BackgroundColor = XLColor.DarkGray;
                        ws.Range("AX1:BE1").Merge();
                        ws.Cell(1, 50).Value = "ICICI BANK LTD";
                        ws.Cell(1, 50).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("BF1:BT1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("BF1:BT1").Merge();
                        ws.Cell(1, 58).Value = "PARTNER";
                        ws.Cell(1, 58).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(1, 58).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("BU1:CB1").Style.Fill.BackgroundColor = XLColor.DarkGray;
                        ws.Range("BU1:CB1").Merge();
                        ws.Cell(1, 73).Value = "ICICI BANK LTD";
                        ws.Cell(1, 73).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("CC1:CQ1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("CC1:CQ1").Merge();
                        ws.Cell(1, 81).Value = "PARTNER";
                        ws.Cell(1, 81).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(1, 81).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("CR1:CY1").Style.Fill.BackgroundColor = XLColor.DarkGray;
                        ws.Range("CR1:CY1").Merge();
                        ws.Cell(1, 96).Value = "ICICI BANK LTD";
                        ws.Cell(1, 96).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("CZ1:DN1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("CZ1:DN1").Merge();
                        ws.Cell(1, 104).Value = "PARTNER";
                        ws.Cell(1, 104).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(1, 104).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("DO1:DV1").Style.Fill.BackgroundColor = XLColor.DarkGray;
                        ws.Range("DO1:DV1").Merge();
                        ws.Cell(1, 119).Value = "ICICI BANK LTD";
                        ws.Cell(1, 119).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("DW1:EK1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("DW1:EK1").Merge();
                        ws.Cell(1, 127).Value = "PARTNER";
                        ws.Cell(1, 127).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(1, 127).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("EL1:ES1").Style.Fill.BackgroundColor = XLColor.DarkGray;
                        ws.Range("EL1:ES1").Merge();
                        ws.Cell(1, 142).Value = "ICICI BANK LTD";
                        ws.Cell(1, 142).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("ET1:FH1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("ET1:FH1").Merge();
                        ws.Cell(1, 150).Value = "PARTNER";
                        ws.Cell(1, 150).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(1, 150).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("FI1:FP1").Style.Fill.BackgroundColor = XLColor.DarkGray;
                        ws.Range("FI1:FP1").Merge();
                        ws.Cell(1, 165).Value = "ICICI BANK LTD";
                        ws.Cell(1, 165).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("FQ1:GE1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("FQ1:GE1").Merge();
                        ws.Cell(1, 173).Value = "PARTNER";
                        ws.Cell(1, 173).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(1, 173).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("GF1:GM1").Style.Fill.BackgroundColor = XLColor.DarkGray;
                        ws.Range("GF1:GM1").Merge();
                        ws.Cell(1, 188).Value = "ICICI BANK LTD";
                        ws.Cell(1, 188).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("GN1:GS1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("GN1:GS1").Merge();
                        ws.Cell(1, 196).Value = "PARTNER";
                        ws.Cell(1, 196).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(1, 196).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("GT1:GV1").Style.Fill.BackgroundColor = XLColor.DarkGray;
                        ws.Range("GT1:GV1").Merge();
                        ws.Cell(1, 202).Value = "ICICI BANK LTD";
                        ws.Cell(1, 202).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Range("Q2:Z2").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("Q2:Z2").Style.Font.FontColor = XLColor.Red;

                        ws.Range("AA2:AH2").Style.Fill.BackgroundColor = XLColor.DarkGray;

                        ws.Range("AS2:AW2").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("AS2:AW2").Style.Font.FontColor = XLColor.CoolBlack;

                        ws.Range("AX2:BE2").Style.Fill.BackgroundColor = XLColor.DarkGray;

                        ws.Range("BP2:BT2").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("BP2:BT2").Style.Font.FontColor = XLColor.CoolBlack;

                        ws.Range("BU2:CB2").Style.Fill.BackgroundColor = XLColor.DarkGray;

                        ws.Range("CM2:CQ2").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("CM2:CQ2").Style.Font.FontColor = XLColor.CoolBlack;

                        ws.Range("CR2:CY2").Style.Fill.BackgroundColor = XLColor.DarkGray;

                        ws.Range("DJ2:DN2").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("DJ2:DN2").Style.Font.FontColor = XLColor.CoolBlack;

                        ws.Range("DO2:DV2").Style.Fill.BackgroundColor = XLColor.DarkGray;

                        ws.Range("EG2:EK2").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("EG2:EK2").Style.Font.FontColor = XLColor.CoolBlack;

                        ws.Range("EL2:ES2").Style.Fill.BackgroundColor = XLColor.DarkGray;

                        ws.Range("FD2:FH2").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("FD2:FH2").Style.Font.FontColor = XLColor.CoolBlack;

                        ws.Range("FI2:FP2").Style.Fill.BackgroundColor = XLColor.DarkGray;

                        ws.Range("GA2:GE2").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("GA2:GE2").Style.Font.FontColor = XLColor.CoolBlack;

                        ws.Range("GF2:GM2").Style.Fill.BackgroundColor = XLColor.DarkGray;

                        ws.Range("GN2:GP2").Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws.Range("GN2:GP2").Style.Font.FontColor = XLColor.CoolBlack;

                        ws.Range("GT2:GV2").Style.Fill.BackgroundColor = XLColor.DarkGray;

                        //wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=CommonMISTracker.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }

                    }
                }
                else
                {
                    dt = cc.MIS_Client_common(ClientRowID);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["NonGreenRemarks"] = clsCommonMethods.HTMLToText(Convert.ToString(dt.Rows[i]["NonGreenRemarks"])); ;
                        }
                    }
                    DataView view = new System.Data.DataView(dt);
                    DataTable dtv = view.ToTable("Details", false);

                    using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(dtv);
                        ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                        wb.Style.Font.Bold = true;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=CommonMISTracker.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }

                BindClientList();

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View();
        }

        [NonAction]
        void BindClientListEncl()
        {
            try
            {
                List<ClientDDLViewModelEnc> list = new List<ClientDDLViewModelEnc>();

                list = repoDDL.GetActiveClientListEnc().ToList();
                list.Insert(0, new ClientDDLViewModelEnc { ClientRowID = "0", ClientName = "-- Select Client -- " });


                //list.Add(new ClientDDLViewModelEnc { ClientRowID = "0", ClientName = "--Select--" });

                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].ClientRowID = clsCommonMethods.PEncrypt(list[i].ClientRowID.ToString());
                }
                ViewBag.ClientList = list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        [NonAction]
        void BindClientList()
        {
            try
            {
                ViewBag.ClientList = repoDDL.GetActiveClientList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateDetails(string ErrMessage = "")
        {
            BindClientList();
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateDetails(HttpPostedFileBase file, FormCollection formCollection)
        {
            try
            {
                if (file != null)
                {
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {
                        ViewBag.ErrorMsg = "Please upload a valid Excel file (.xls or .xlsx).";
                        return View();
                    }
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Emailers"), fileName);
                    file.SaveAs(filePath);

                    int result = UpdateDetaisbyExcel(filePath);
                    if (result <= 0)
                    {
                        ViewBag.ErrorMsg = "Error updating data.";
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        return View();
                    }

                    ViewBag.ErrorMsg = "Candidate details updated successfully.";
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Please select an Excel file to upload.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Error: " + ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), ex.Message, ex.StackTrace);
            }

            return View();
        }

        public int UpdateDetaisbyExcel(string filePath)
        {
            try
            {

                string fileExtension = Path.GetExtension(filePath).ToLower();
                string conStr = "";
                switch (fileExtension)
                {
                    case ".xls":
                        conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx":
                        conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                    default:
                        throw new InvalidOperationException("Invalid file format. Only .xls and .xlsx are allowed.");
                }

                conStr = string.Format(conStr, filePath, "YES");

                using (OleDbConnection connExcel = new OleDbConnection(conStr))
                {
                    connExcel.Open();
                    DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    connExcel.Close();

                    OleDbCommand cmdExcel = new OleDbCommand($"SELECT * FROM [{sheetName}]", connExcel);
                    OleDbDataAdapter oda = new OleDbDataAdapter(cmdExcel);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    connExcel.Close();

                    int updatedCount = 0;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string CompanyRefNo = row["Vendor Reference ID"].ToString().Trim();
                            string clientRefID = row["Applicant ID"].ToString().Trim();
                            string candidateType = row["Type of Employee"].ToString().Trim();
                            string businessName = row["Business Unit"].ToString().Trim();
                            string placeOfJoining = row["Joining Location"].ToString().Trim();
                            string designation = row["Grade"].ToString().Trim();
                            string category = row["Joining Status"].ToString().Trim();
                            string employeeID = row["Employee ID- Joined Cases"].ToString().Trim();

                            ClientCustomize cc = new ClientCustomize();
                            cc.UpdateDetailsPQPersonalByExcell(CompanyRefNo, clientRefID, candidateType, businessName, placeOfJoining, designation, category, employeeID);
                            updatedCount++;
                        }
                    }

                    return updatedCount;
                }
            }
            catch (Exception ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), ex.Message, ex.StackTrace);
                return 0;
            }
        }




        [HttpGet]
        public void RejectionCheckwiseMIS(string ClientRowId, string dtf, string dtt)
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();
                var tblRowID = GetClientRowID(ClientRowId);
                //ds = cc.MIS_Rejection_Checkwise(ClientRowId);
                ds = cc.MIS_Rejection_Checkwise_MultipleClient(tblRowID, dtf, dtt);
                if (ds.Tables.Count > 0)
                {
                    var dt = new DataTable("Details");
                    dt = ds.Tables[0];
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
                        Response.AddHeader("content-disposition", "attachment;filename=PendingWithCandidate.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpGet]
        public void BankingClientMIS()
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();

                //ds = cc.MIS_Rejection_Checkwise(ClientRowId);
                ds = cc.MIS_LTIMindtree_Trackar();
                if (ds.Tables.Count > 0)
                {
                    var dt = new DataTable("Details");
                    dt = ds.Tables[0];
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
                        Response.AddHeader("content-disposition", "attachment;filename=PendingWithCandidate.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        public void Exportfundsapproval()
        {
            DataTable dt = new DataTable();

            int teamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
            string CaseStatus = (Session["CaseStatus"]).ToString();
            int clientid = Convert.ToInt32(Session["clientid"]);

            ClientCustomize cc = new ClientCustomize();
            dt = cc.ExportExcel_FundPendingBuucket(clientid, CaseStatus, teamMemberRowID);
            ExportToExcel(dt, "funds_approval");
        }

        public void ReminderMailTataElxsi()
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();

                ds = cc.mailAlertCandidate_Reminder_tataElsxi();
                if (ds.Tables.Count > 0)
                {
                    var dt = new DataTable("Details");
                    dt = ds.Tables[0];
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
                        Response.AddHeader("content-disposition", "attachment;filename=TataReminderMails.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpGet]
        [SessionExpireFilter]
        [ValidateInput(false)]
        public ActionResult EmploymentYTRChecksClearByClient(string id = "", string SubCheckRowID = "", string CheckFamilyRowID = "", string PersonalRowid = "")
        {
            PQEmploymentYTRClearViewModel model = new PQEmploymentYTRClearViewModel();
            int Did = 0;
            int PersonalRowID = 0;
            if (id != "")
            {
                Did = Convert.ToInt32(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(id)));

            }

            if (PersonalRowid != "")
            {
                PersonalRowID = Convert.ToInt32(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(PersonalRowid)));

            }
            string SubCheckId = (clsCommonMethods.PDecrypt(SubCheckRowID));
            int CheckFamilyId = Convert.ToInt32(clsCommonMethods.PDecrypt(CheckFamilyRowID));
            var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowID);
            int ClientRowID = CandidateData.ClientRowID;
            ClientCustomize CC1 = new ClientCustomize();
            DataTable dtx = new DataTable();
            dtx = CC1.GetClientLWDRequired(ClientRowID);
            bool dolisrequired = true;
            if (dtx.Rows.Count > 0)
            {
                dolisrequired = Convert.ToBoolean(dtx.Rows[0]["DOLIsRequired"]);
                if (dolisrequired)
                {
                    dolisrequired = true;
                }
                else
                {
                    dolisrequired = false;

                }
            }
            try
            {
                if (Did > 0)
                {
                    model.EmploymentRowID = Did;
                    model.CheckFamilyRowID = CheckFamilyId;
                    model.SubCheckRowID = Convert.ToInt16(SubCheckId);
                    model.PersonalRowID = PersonalRowID;
                    model.ClientRowId = CandidateData.ClientRowID;
                    model.DOLisRequired = dolisrequired;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [HttpGet]
        [SessionExpireFilter]
        [ValidateInput(false)]
        public ActionResult EducationResultAwaitedChecksClearByClient(string id = "", string SubCheckRowID = "", string CheckFamilyRowID = "", string PersonalRowid = "")
        {
            PQEmploymentYTRClearViewModel model = new PQEmploymentYTRClearViewModel();
            int Did = 0;
            int PersonalRowID = 0;
            if (id != "")
            {
                Did = Convert.ToInt32(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(id)));

            }

            if (PersonalRowid != "")
            {
                PersonalRowID = Convert.ToInt32(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(PersonalRowid)));

            }
            string SubCheckId = (clsCommonMethods.PDecrypt(SubCheckRowID));
            int CheckFamilyId = Convert.ToInt32(clsCommonMethods.PDecrypt(CheckFamilyRowID));
            var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowID);
            int ClientRowID = CandidateData.ClientRowID;
            ClientCustomize CC1 = new ClientCustomize();
            //DataTable dtx = new DataTable();
            //dtx = CC1.GetClientLWDRequired(ClientRowID);
            //bool dolisrequired = true;
            //if (dtx.Rows.Count > 0)
            //{
            //    dolisrequired = Convert.ToBoolean(dtx.Rows[0]["DOLIsRequired"]);
            //    if (dolisrequired)
            //    {
            //        dolisrequired = true;
            //    }
            //    else
            //    {
            //        dolisrequired = false;

            //    }
            //}
            try
            {
                if (Did > 0)
                {
                    model.EmploymentRowID = Did;
                    model.CheckFamilyRowID = CheckFamilyId;
                    model.SubCheckRowID = Convert.ToInt16(SubCheckId);
                    model.PersonalRowID = PersonalRowID;
                    model.ClientRowId = CandidateData.ClientRowID;
                    //model.DOLisRequired = dolisrequired;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [HttpPost]
        [SessionExpireFilter]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EducationResultAwaitedChecksClearByClient(PQEmploymentYTRClearViewModel model)
        {
            try
            {
                if (model.EmploymentRowID > 0)
                {
                    model.EmploymentRowID = model.EmploymentRowID;
                    model.CheckStatus = "WIP-Pending";
                    model.ModifiedBy = 0;
                    model.ModifiedDate = DateTime.Now;
                    short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    if (model.CheckFamilyRowID == 11)
                    {
                        if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                        {
                            SaveYTRUploadDocByClient(model.PersonalRowID, model.SubCheckRowID, "Provided", "ResultAwaitedClear", "provideddocuments", model.UploadDoc, "", 0, model.SecuritasRefID);

                        }
                    }

                    #region Save LogTransaction Details
                    clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "EducationResultAwaitedChecksClearByClient", model.CheckStatus, model.UniqueComponentID, "Clear Result Awaited By Spoc");
                    #endregion

                    //Check Action History Save 
                    clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Result Awaited cleared By Spoc", model.Remarks);

                    ClientCustomize cc = new ClientCustomize();
                    cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 114, model.Remarks, "Result Awaited cleared by Spoc", ClientRowID);
                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 0, 114, "Client", ClientRowID);
                    cc.UpdateEmpIdYTRClear(model.PersonalRowID, model.EmployeeID, ClientRowID);
                    //if (model.DOL != null)
                    //{
                    //    cc.InsertDataTblDataEntry(model.PersonalRowID, model.SubCheckRowID, (model.DOL).ToString());
                    //}
                    //else
                    //{
                    //    cc.InsertDataTblDataEntry(model.PersonalRowID, model.SubCheckRowID, "Till Date");
                    //}

                    //cc.UpdateYTRDetails_tbl(model.PersonalRowID, model.SubCheckRowID, 0, 1);
                    //if (ClientRowID == 7 || ClientRowID == 296 || ClientRowID == 305 || ClientRowID == 306)
                    //{
                    //    SendAlertMassageForYTRClearlMail(model.PersonalRowID, model.SubCheckRowID, ClientRowID, "YTR Clear", 0, 1);
                    //}


                    //repoPQCandidateCheckRepository.UpdateYTR_RaisedOrCleared_Date(model.PersonalRowID, model.SubCheckRowID, "Cleared");


                    //===========================================
                    TempData["SuccessMsg"] = "Result Awaited Clear Successfully!";
                    ModelState.Clear();
                    return View(model);
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

        [HttpPost]
        [SessionExpireFilter]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EmploymentYTRChecksClearByClient(PQEmploymentYTRClearViewModel model)
        {
            try
            {
                if (model.EmploymentRowID > 0)
                {
                    model.EmploymentRowID = model.EmploymentRowID;
                    model.CheckStatus = "WIP-Pending";
                    model.ModifiedBy = 0;
                    model.ModifiedDate = DateTime.Now;
                    short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);
                    if (model.CheckFamilyRowID == 12)
                    {
                        if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                        {
                            SaveYTRUploadDocByClient(model.PersonalRowID, model.SubCheckRowID, "Provided", "YTRClear", "provideddocuments", model.UploadDoc, "", 0, model.SecuritasRefID);

                        }
                    }
                    if (model.CheckFamilyRowID == 30)
                    {
                        if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                        {
                            SaveYTRUploadDocByClient(model.PersonalRowID, model.SubCheckRowID, "Provided", "YTRClear", "provideddocuments", model.UploadDoc, "", 0, model.SecuritasRefID);
                        }

                    }
                    #region Save LogTransaction Details
                    clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "EmploymentYTRChecksClearByClient", model.CheckStatus, model.UniqueComponentID, "Clear YTR By Client");
                    #endregion

                    //Check Action History Save 
                    clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Clear YTR By Spoc", model.Remarks);

                    ClientCustomize cc = new ClientCustomize();
                    cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 69, model.Remarks, "YTRClear by Spoc", ClientRowID);
                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 0, 69, "Client", ClientRowID);
                    cc.UpdateEmpIdYTRClear(model.PersonalRowID, model.EmployeeID, ClientRowID);
                    if (model.DOL != null)
                    {
                        cc.InsertDataTblDataEntry(model.PersonalRowID, model.SubCheckRowID, (model.DOL).ToString());
                    }
                    else
                    {
                        cc.InsertDataTblDataEntry(model.PersonalRowID, model.SubCheckRowID, "Till Date");
                    }

                    cc.UpdateYTRDetails_tbl(model.PersonalRowID, model.SubCheckRowID, 0, 1);
                    if (ClientRowID == 7 || ClientRowID == 296 || ClientRowID == 305 || ClientRowID == 306 || ClientRowID == 683)
                    {
                        SendAlertMassageForYTRClearlMail(model.PersonalRowID, model.SubCheckRowID, ClientRowID, "YTR Clear", 0, 1);
                    }


                    repoPQCandidateCheckRepository.UpdateYTR_RaisedOrCleared_Date(model.PersonalRowID, model.SubCheckRowID, "Cleared");


                    //===========================================
                    TempData["SuccessMsg"] = "YTR Clear Successfully!";
                    ModelState.Clear();
                    return View(model);
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


        [HttpPost]
        [SessionExpireFilter]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult YTRChecksClearByClient(PQEmploymentYTRClearViewModel model)
        {
            try
            {
                if (model.EmploymentRowID > 0)
                {
                    ClientCustomize cb = new ClientCustomize();
                    DataTable dto = cb.GetAddress_IDCheckByPid(model.PersonalRowID, model.SubCheckRowID);
                    if (dto.Rows.Count > 0)
                    {
                        for (int i = 0; i < dto.Rows.Count; i++)
                        {
                            model.EmploymentRowID = Convert.ToInt32(dto.Rows[i]["CandCheckRowID"].ToString());
                            model.PersonalRowID = Convert.ToInt32(dto.Rows[i]["PersonalRowID"].ToString());
                            model.SubCheckRowID = Convert.ToInt16(dto.Rows[i]["SubCheckFamilyID"].ToString());


                            //        model.EmploymentRowID = model.EmploymentRowID;
                            model.CheckStatus = "WIP-Pending";
                            model.ModifiedBy = 0;
                            model.ModifiedDate = DateTime.Now;
                            short ClientRowID = Convert.ToInt16(Session["ClientRowID"]);

                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "YTRChecksClearByClient", model.CheckStatus, model.UniqueComponentID, "Clear YTR By Client");
                            #endregion

                            //Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Clear YTR By Spoc", model.Remarks);

                            ClientCustomize cc = new ClientCustomize();
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 69, model.Remarks, "YTRClear by Spoc", ClientRowID);
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 0, 69, "Client", ClientRowID);
                            //cc.UpdateEmpIdYTRClear(model.PersonalRowID, model.EmployeeID, ClientRowID);
                            if (model.DOJ != null)
                            {
                                cc.InsertDataTblDataEntryDOJ(model.PersonalRowID, model.SubCheckRowID, (model.DOJ).ToString());
                            }


                            cc.UpdateYTRDetails_tbl(model.PersonalRowID, model.SubCheckRowID, 0, 1);



                            repoPQCandidateCheckRepository.UpdateYTR_RaisedOrCleared_Date(model.PersonalRowID, model.SubCheckRowID, "Cleared");


                            //===========================================
                            TempData["SuccessMsg"] = "YTR Clear Successfully!";
                            ModelState.Clear();


                        }
                        return RedirectToAction("ClientServicingDashboardInfo", "ClientServicing", new { CaseStatus = "YTR Hold", tmr = Convert.ToInt16(Session["TeamMemberRowID"]), cr = clsCommonMethods.PEncrypt("0") });

                    }

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
            return RedirectToAction("ClientServicingDashboardInfo", "ClientServicing", new { CaseStatus = "YTR Hold", tmr = Convert.ToInt16(Session["TeamMemberRowID"]), cr = clsCommonMethods.PEncrypt("0") });
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

        [HttpGet]
        [SessionExpireFilter]
        public JsonResult EmploymentYTRCheckClear(short id = 0)
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    PQUpdateEmploymentResearcherViewModel eModel = new PQUpdateEmploymentResearcherViewModel();
                    eModel.EmploymentRowID = id;
                    eModel.CheckStatus = "WIP-Pending";
                    eModel.ModifiedBy = 0;
                    eModel.ModifiedDate = DateTime.Now;
                    eModel = repoEmploymentInfo.UpdateEmploymentInfoByResearcher(eModel);

                    #region Save LogTransaction Details
                    clsCommonMethods.AddPQLogTrasactionByClient(eModel.PersonalRowID, "EmploymentYTRCheckClear", eModel.CheckStatus, eModel.UniqueComponentID, "Clear YTR By Client");
                    #endregion

                    //Check Action History Save 
                    clsCommonMethods.AddCheckActionHistoryByClient(eModel.PersonalRowID, eModel.SubCheckRowID, eModel.CheckStatus, "Clear YTR By Client");

                    result = true;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                //ViewBag.ErrorMsg = Ex.Message;
                ViewBag.ErrorMsg = "Invalid paramerter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);
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
                var templateDB = cc.getAlertMailtemplate(clientrowid, AlertType, PackageId, MailType, "YTRClearMail");
                if (templateDB.Rows.Count > 0)
                {
                    body = Convert.ToString(templateDB.Rows[0]["MailTemplate"]);
                    MailSubject = Convert.ToString(templateDB.Rows[0]["MailSubject"]);
                }
                //using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailers/Company/YTRClearMail.html")))
                //{
                //    body = reader.ReadToEnd();
                //}
                string CandidateName = Convert.ToString(ds.Tables[0].Rows[0]["CandidateName"]);
                string CompanyRefNo = Convert.ToString(ds.Tables[0].Rows[0]["CompanyRefNo"]);
                string SubCheckName = Convert.ToString(ds.Tables[0].Rows[0]["SubCheckName"]);
                string TeamEmailID = Convert.ToString(ds.Tables[0].Rows[0]["OperationEmailID"]);
                string SpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["SpocEmailID"]);
                string ClientSpocEmailID = Convert.ToString(ds.Tables[0].Rows[0]["ClientSpocEmailID"]);
                //int clientrowid = Convert.ToInt32(ds.Tables[0].Rows[0]["clientrowid"]);
                var CandidateData = repoPQPersonal.GetPQPersonalInfoForSession(PersonalRowID);
                string spocEmail = "";
                string cSpocEmail = "";
                string toMailId = "";
                DataTable dtEmail = new DataTable();
                dtEmail = cc.GetEmailStructureForAlert(clientrowid, CandidateData.ClientUserRowId, "Insuff", PersonalRowID);
                var dtEmailNew = cc.GetEmailIdForAlert(clientrowid, CandidateData.ClientUserRowId, "Insuff");

                if (dtEmail.Rows.Count > 0)
                {
                    spocEmail = dtEmail.Rows[0]["SpocEmailID"].ToString();
                    cSpocEmail = dtEmail.Rows[0]["CSpocEmailID"].ToString();
                }
                if (dtEmailNew.Rows.Count > 0)
                {
                    toMailId = dtEmailNew.Rows[0]["SendYTRMail"].ToString();
                }

                body = body.Replace("{FCandidateName}", CandidateName);
                body = body.Replace("{FCheckName}", SubCheckName);
                //body = body.Replace("{FPRemarks}", remarks);
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
                if (CandidateData.ClientRowID == 257)
                {
                    string bcc = "";
                    string Tos = "";
                    Tos = spocEmail;
                    bcc = cSpocEmail;
                    flag = mail.SendMail(DisplayName, From, Tos, "", bcc, Subject, body);
                }
                else if (CandidateData.ClientRowID == 50)
                {
                    //-------------------To-----------------
                    ToMail = toMailId.ToLower();
                    ToMail = ToMail.Replace(",", ";");
                    arToMail = ToMail.Split(';');
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

                    //-------------------CCCC-----------------
                    string ccMail = (TeamEmailID + "," + spocEmail + "," + cSpocEmail).ToLower();
                    ccMail = ccMail.Replace(",", ";");
                    string[] arccMail = ccMail.Split(';');
                    ccMail = "";
                    foreach (string a in arccMail)
                    {
                        if (!ccMail.Contains(a))
                        {
                            ccMail = ccMail + a.Trim() + ",";
                        }
                    }
                    if (ccMail.Length > 1)
                        ccMail = ccMail.Substring(0, ccMail.Length - 1);

                    flag = mail.SendMail(DisplayName, From, ToMail, ccMail, Subject, body);
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

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult CaseProcessSpoc(FormCollection form, string Search = "", string Remarks = " ", string SaveWork = "", String CheckDel = "", int pageNo = 1, int pageSize = 100)
        {
            try
            {

                string Department = Session["Department"].ToString();
                string ddlCheckStatus = form["ddlCheckStatus"];
                string ColorName = form["ddlColor"];
                ClientServiceListViewModel model = new ClientServiceListViewModel();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                if (Search == "")
                {
                    BindSubchecknameDelete(0, 0);
                    BindMasterCheckStatusForSpoc(Department);
                }

                else
                {

                    // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                    DataTable dt = new DataTable();
                    ClientCustomize cc = new ClientCustomize();
                    //dt = cc.GetGenpackCase(Search);
                    dt = cc.CaseCheckDetailForSpoc(Search);

                    List<ClientDashboardCheckSummaryDetailViewModel> lmd = new List<ClientDashboardCheckSummaryDetailViewModel>();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            lmd.Add(new ClientDashboardCheckSummaryDetailViewModel
                            {
                                SecuritasID = (dr["CompanyRefNo"].ToString()),
                                CandidateName = (dr["CandidateFullName"].ToString()),
                                CheckName = (dr["SubCheckName"].ToString()),
                                CheckStatus = dr["CheckStatus"].ToString(),
                                CheckStatusID = (dr["CheckStatusID"].ToString()),
                                OrderDate = (dr["orderDate"].ToString()),
                                OutDate = (dr["outDate"].ToString()),
                                CreatedDate = (dr["CreationDate"].ToString()),
                                ColorName = dr["ColorName"].ToString(),
                                ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"]),
                                Name = (dr["Name"].ToString()),

                                PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),

                            });

                        }
                        model.TotalRecords = dt.Rows.Count;
                        BindSubchecknameDelete(Convert.ToInt32(dt.Rows[0]["ClientRowID"]), Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                        BindMasterCheckStatusForSpoc(Department);

                    }






                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.ClientServiceChecks = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);







                    if (ColorName != null && ColorName != "0" && ddlCheckStatus != "0" && Remarks != "") //this is use for check move
                    {
                        if (Remarks != "")
                        {
                            DataTable dtNotapplicable = cc.CheckNotApplicable(Search, Convert.ToInt32(Request.Form["ddlColor"]));
                            if (dtNotapplicable.Rows.Count > 0 && Convert.ToInt32(ddlCheckStatus) != 8)
                            {
                                TempData["SuccessMsg"] = "Not applicable check move only data entry TM!";
                            }
                            else
                            {
                                int MoveCheckId = Convert.ToInt32(ddlCheckStatus);


                                int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);

                                cc.PQCandidateChecksDeleteCheck(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(Request.Form["ddlColor"]), 1, MoveCheckId, "helpdesk", Convert.ToInt32(Session["TeamMemberRowID"]), 0, Remarks);
                                cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), strDDLValue, MoveCheckId, Remarks, "Support", Convert.ToInt32(Session["TeamMemberRowID"]));
                                //Remarks = "Re verification -" + Remarks;
                                #region Check Action History Save                           
                                clsCommonMethods.AddCheckActionHistory(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt16(strDDLValue), "Check Movement", Remarks);
                                #endregion
                                //cc.UpdateVertable(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), MoveCheckId);
                                TempData["SuccessMsg"] = "Check Move Successfully!";
                                if (ddlCheckStatus != "36")
                                {
                                    alertMailToManagerOnCheckReopen(Search, ddlCheckStatus, strDDLValue, Remarks);
                                }
                                return RedirectToAction("CaseProcessSpoc", "ClientServicing", new { Search = Search });

                            }






                        }


                    }
                    else
                    {
                        TempData["SuccessMsg"] = "Please Select check details!";
                    }



                }

                return View(model);



            }
            catch (Exception)
            {
                throw;
            }
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult GetCandidateDetail(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string ClientRowId = "", int ClientUserRowId = 0)
        {
            GetDynamicReport model = new GetDynamicReport();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetCandidateDetail(GetDynamicReport model, FormCollection form, string Command)
        {
            ClientCustomize cc = new ClientCustomize();
            if (model.SecuritasID != null && Command == "Search")
            {
                if (!string.IsNullOrEmpty(model.SecuritasID))
                {
                    DataTable dt = new DataTable();
                    dt = cc.GetOFACCheckDetail(model.SecuritasID);
                    if (dt.Rows.Count > 0)
                    {
                        model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                        model.ClientRowId = 762;
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No OFAC detail found";
                    }


                    //model.ClientRowId = BindCheckList(model.SecuritasID).ToString();
                    //BindSubcheckname(model.SecuritasID);
                }
                else
                {
                    ViewBag.ErrorMsg = "please fill Company Ref No.";
                }
            }

            if (model.SecuritasID != null && Command == "Report")
            {
                if (!string.IsNullOrEmpty(model.SecuritasID))
                {
                    DataTable dt = new DataTable();
                    dt = cc.GetOFACCheckDetail(model.SecuritasID);
                    if (dt.Rows.Count > 0)
                    {
                        model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                        return RedirectToAction("ReportInterim", "ClientServicing", new { id = model.PersonalRowID, ReportPart = "OFAC", Report_Type = "0" });

                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No OFAC detail found";
                    }


                    //model.ClientRowId = BindCheckList(model.SecuritasID).ToString();
                    //BindSubcheckname(model.SecuritasID);
                }
                else
                {
                    ViewBag.ErrorMsg = "please fill Company Ref No.";
                }
            }


            if (string.IsNullOrEmpty(model.SecuritasID))
            {
                ViewBag.ErrorMsg = "please fill Company Ref No.";
            }
            if (Command == "submit")
            {

                if (!string.IsNullOrEmpty(model.SecuritasID))
                {
                    DataTable dt = new DataTable();
                    dt = cc.GetOFACCheckDetail(model.SecuritasID);
                    if (dt.Rows.Count > 0)
                    {
                        model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "No OFAC detail found";
                    }


                    //model.ClientRowId = BindCheckList(model.SecuritasID).ToString();
                    //BindSubcheckname(model.SecuritasID);
                }
                else
                {
                    ViewBag.ErrorMsg = "please fill Company Ref No.";
                }
            }
            if (model.PersonalRowID != 0 && Command == "submit")
            {

                string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                model.UploadDate = DateTime.Now;
                if (model.OFACDoc != null && model.OFACDoc.ContentLength > 0)
                {
                    string proImg = string.Empty;
                    string ext = Path.GetExtension(model.OFACDoc.FileName).ToLower();
                    proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + "205" + "_" + "OFACDoc" + model.UploadDate.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                    model.OFACDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                    model.FileName = proImg;
                    //cc.deleteUploadDoc(model.PersonalRowID, 205, "OFAC Report");
                    cc.insertTekSystemOFACDoc(model.PersonalRowID, 205, model.FileName, model.FilePath, "OFAC Report");
                }

                if (model.IDReportDoc != null && model.IDReportDoc.ContentLength > 0)
                {
                    string proImg = string.Empty;
                    string ext = Path.GetExtension(model.IDReportDoc.FileName).ToLower();
                    proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + "205" + "_" + "IDReportDoc" + model.UploadDate.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                    model.IDReportDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                    model.FileName = proImg;
                    //cc.deleteUploadDoc(model.PersonalRowID, 205, "ID Report");
                    cc.insertTekSystemOFACDoc(model.PersonalRowID, 205, model.FileName, model.FilePath, "ID Report");
                }

                if (model.TGSExportDoc != null && model.TGSExportDoc.ContentLength > 0)
                {
                    string proImg = string.Empty;
                    string ext = Path.GetExtension(model.TGSExportDoc.FileName).ToLower();
                    proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + "205" + "_" + "TGSExportDoc" + model.UploadDate.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                    model.TGSExportDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                    model.FileName = proImg;
                    //cc.deleteUploadDoc(model.PersonalRowID, 205, "TGS Export Report");
                    cc.insertTekSystemOFACDoc(model.PersonalRowID, 205, model.FileName, model.FilePath, "TGS Export Report");
                }

                if (model.OFACKeywordDoc != null && model.OFACKeywordDoc.ContentLength > 0)
                {
                    string proImg = string.Empty;
                    string ext = Path.GetExtension(model.OFACKeywordDoc.FileName).ToLower();
                    proImg = ("Artifact" + "_" + model.PersonalRowID + "_" + "205" + "_" + "OFACKeyword" + model.UploadDate.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                    model.OFACKeywordDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                    model.FileName = proImg;
                    //cc.deleteUploadDoc(model.PersonalRowID, 205, "OFAC Keyword");
                    cc.insertTekSystemOFACDoc(model.PersonalRowID, 205, model.FileName, model.FilePath, "OFAC Keyword");
                }
                cc.InsertTekSystemReportInfo(model.PersonalRowID, model.FatherName, model.VerifiedBy, model.VerifiedOn, model.UniqueId, model.ReportComment, model.CreatedBy, model.DOB);

                ViewBag.SuccessMsg = "Record Update Successfully";

            }

            return View(model);
        }


        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult StopCheckBySpoc(FormCollection form, string Search = "", string Remarks = " ", string SaveWork = "", String CheckDel = "", int pageNo = 1, int pageSize = 100)
        {
            try
            {
                string ddlCheckStatus = form["ddlCheckStatus"];
                string ColorName = form["ddlColor"];
                ClientServiceListViewModel model = new ClientServiceListViewModel();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                if (Search == "")
                {
                    BindSubchecknameDelete(0, 0);
                    GetMasterStopcheckStatusId();
                }
                else
                {
                    DataTable dt = new DataTable();
                    ClientCustomize cc = new ClientCustomize();
                    dt = cc.GetCandidateChecks(Search);

                    List<ClientDashboardCheckSummaryDetailViewModel> lmd = new List<ClientDashboardCheckSummaryDetailViewModel>();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            lmd.Add(new ClientDashboardCheckSummaryDetailViewModel
                            {
                                SecuritasID = (dr["CompanyRefNo"].ToString()),
                                CheckName = (dr["SubCheckName"].ToString()),
                                CheckStatus = dr["CheckStatus"].ToString(),
                                CheckStatusID = (dr["CheckStatusID"].ToString()),
                                OrderDate = (dr["orderDate"].ToString()),
                                OutDate = (dr["outDate"].ToString()),
                                CreatedDate = (dr["CreationDate"].ToString()),
                                ColorName = dr["ColorName"].ToString(),
                                ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"]),
                                Name = (dr["Name"].ToString()),
                                CandidateName = (dr["CandidateFullName"].ToString()),
                                PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),

                            });

                        }

                        DataTable dts = cc.GetSpocClients(Convert.ToInt32(dt.Rows[0]["ClientRowID"]), Session["TeamMemberEmail"].ToString());
                        if (dts.Rows.Count < 1)
                        {
                            TempData["SuccessMsg"] = "Please check reference no associate with spoc"; model.TotalRecords = dt.Rows.Count;
                            BindSubchecknameDelete(Convert.ToInt32(dt.Rows[0]["ClientRowID"]), Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                        }
                        else
                        {
                            model.TotalRecords = dt.Rows.Count;
                            BindSubchecknameDelete(Convert.ToInt32(dt.Rows[0]["ClientRowID"]), Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                            GetMasterStopcheckStatusId();
                        }
                    }
                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.ClientServiceChecks = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                    if (ColorName != null && ColorName != "0")//this is use for check remove
                    {
                        if (ddlCheckStatus == "102")
                        {
                            if (Remarks != "")
                            {
                                int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);

                                cc.PQCandidateChecksDeleteCheck(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(Request.Form["ddlColor"]), 1, 0, "helpdesk", Convert.ToInt32(Session["TeamMemberRowID"]), 0, Remarks);

                                cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), strDDLValue, 2, Remarks, "Support", Convert.ToInt32(Session["TeamMemberRowID"]));
                                TempData["SuccessMsg"] = "Check Remove Successfully!";
                                return RedirectToAction("StopCheckBySpoc", "ClientServicing", new { Search = Search });
                            }

                            else
                            {
                                TempData["SuccessMsg"] = "Please fill the Remarks";
                            }
                        }
                        if (ddlCheckStatus == "100")
                        {
                            if (Remarks != "")
                            {
                                int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);
                                cc.AllStopCheck(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(Request.Form["ddlColor"]), "helpdesk", Convert.ToInt32(Session["TeamMemberRowID"]), Remarks);

                                cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), strDDLValue, 3, Remarks, "Support", Convert.ToInt32(Session["TeamMemberRowID"]));
                                TempData["SuccessMsg"] = "Check Remove Successfully!";
                                return RedirectToAction("StopCheckBySpoc", "ClientServicing", new { Search = Search });
                            }
                            else
                            {
                                TempData["SuccessMsg"] = "Please fill the Remarks";
                            }
                        }
                        else
                        {
                            TempData["SuccessMsg"] = "Please Select  Delete action";
                        }
                    }

                    if (ColorName != null && ColorName != "0" && ddlCheckStatus != "102" && ddlCheckStatus != "0" && Remarks != "") //this is use for check move
                    {
                        if (Remarks != "")
                        {

                            int MoveCheckId = Convert.ToInt32(ddlCheckStatus);
                            int strDDLValue = Convert.ToInt32(Request.Form["ddlColor"]);
                            cc.PQCandidateChecksDeleteCheck(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(Request.Form["ddlColor"]), 1, MoveCheckId, "helpdesk", Convert.ToInt32(Session["TeamMemberRowID"]), 0, Remarks);
                            cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), strDDLValue, MoveCheckId, Remarks, "Support", Convert.ToInt32(Session["TeamMemberRowID"]));
                            TempData["SuccessMsg"] = "Check Move Successfully!";
                            return RedirectToAction("StopCheckBySpoc", "ClientServicing", new { Search = Search });
                        }
                        else
                        {
                            TempData["SuccessMsg"] = "Please fill all Details and move check!";

                        }
                    }
                }
                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        void GetMasterStopcheckStatusId()
        {
            try
            {

                ViewBag.CheckId = repoPQPersonal.GetMasterStopcheckStatusId();

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        public Image LoadImage(string Base64Image, int personalrowid = 0)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            byte[] bytes = Convert.FromBase64String(Base64Image);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
        public void LoadImages(string Base64Image, int personalrowid = 0)
        {
            byte[] imageBytes = Convert.FromBase64String(Base64Image);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            image.Save(Server.MapPath("~/Documents/" + personalrowid + ".png"), System.Drawing.Imaging.ImageFormat.Png);

        }
        public string DigitalLOA(int PersonalRowid)
        {
            ClientCustomize cc = new ClientCustomize();

            string CanLOASignature = "";
            Document pdfDoc = new Document(PageSize.A4, 40, 40, 80, 25);
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();

            //Table
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            //Cell          
            PdfPCell cell = new PdfPCell(new Phrase("DECLARATION  Authorization", FontFactory.GetFont("Times New Roman ,serif", 16, BaseColor.WHITE)));
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(129, 211, 201);
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("I certify that the statements made in this application are valid and complete to the best of my knowledge. I understand that false or misleading information may result in termination of the agreement.\n\nIf upon investigations, any of this information is found to be incomplete or inaccurate, I understand that agreement will be subject to dismissal at any time during my employment.\n\nI hereby authorize the Company and / or any of its subsidiaries or affiliates and any persons or organizations acting on its behalf, to verify the information presented on this application form and to procure an investigative report or consumer report for that purpose. \n\nI hereby grant authority for the bearer of this letter to access or be provided with full details of my previous records.In addition, please provide any other pertinent information requested by the individual presenting this authority. \n\nI hereby release from liability all persons or entities requesting or supplying such information.\n\nI am aware that no Money / fee / postage / gift to be paid or given in goodwill to any one during the process of verification and I have informed my family members as well regarding this. \n\nI am aware that during the Address verification, no photographs or documents to be shared with verifier through any medium (Whats App, Email, Etc.)\n\nI have read, understand, and by my signature consent to these statements.\n\nPlease Sign In given below box. And click On Save Button to give your Consent.\n"));
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Colspan = 4;
            table.AddCell(cell);

            cell = new PdfPCell();
            DataTable dt = new DataTable();
            dt = cc.GetExportLoa(PersonalRowid);
            if (dt.Rows.Count > 0)
            {
                ViewBag.LOA_Signature = dt.Rows[0]["LOA_Signature"].ToString();
                if (Convert.ToString(dt.Rows[0]["LOA_Signature"]) != "")
                {
                    CanLOASignature = dt.Rows[0]["LOA_Signature"].ToString();
                    ViewBag.LoaImage = LoadImage(CanLOASignature);
                    ViewBag.LoaImageB64 = CanLOASignature;
                }
                else
                {
                    ViewBag.CanLOASignature = "";
                }
            }
            LoadImages(CanLOASignature, PersonalRowid);
            string imageURL = Server.MapPath("~/Documents/" + PersonalRowid + ".png");
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Documents/" + PersonalRowid + ".png"));
            img.ScaleAbsolute(200, 100);
            img.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            cell.AddElement(img);
            cell.Colspan = 4;
            table.AddCell(cell);

            cell = new PdfPCell();
            table.AddCell("Name:-");
            table.AddCell(ViewBag.LOA_Name = dt.Rows[0]["LOA_Name"].ToString());
            table.AddCell("Date:-");
            table.AddCell(ViewBag.CreatedDate = dt.Rows[0]["CreatedDate"].ToString());
            table.AddCell(cell);

            pdfDoc.Add(table);
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            byte[] bytes = memoryStream.ToArray();
            string filePath = Server.MapPath("~/Documents/CandidateDocuments/ProvidedDocuments/Letter_Of_Authorization" + PersonalRowid + ".pdf");
            System.IO.File.WriteAllBytes(filePath, bytes);
            memoryStream.Close();
            return filePath;
        }
        [ValidateInput(false)]
        public ActionResult GetCaseDocNew(string ImageName, string ImagePath, int Pid)
        {

            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["ClientUploadedDocuments"].ToString();
            string provideddocuments = System.Configuration.ConfigurationManager.AppSettings["ClientUploadedDocuments_New"].ToString();
            string provideddocuments1 = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();

            string filePath = Server.MapPath("~" + ImagePath);

            byte[] fileBytes = null;

            if (System.IO.File.Exists(filePath))
            {
                fileBytes = System.IO.File.ReadAllBytes(filePath);
            }
            else if (System.IO.File.Exists(Server.MapPath(provideddocuments1) + ImageName))
            {
                fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(provideddocuments1) + ImageName);
            }
            if (Convert.ToInt16(Session["ClientRowID"]) == 791 || Convert.ToInt16(Session["ClientRowID"]) == 898)
            {
                string provideddocumentsCand = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();

                fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(provideddocumentsCand) + ImageName);
            }
            if (System.IO.File.Exists(Server.MapPath(provideddocuments) + ImageName))
            {
                fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(provideddocuments) + ImageName);
            }
            if (fileBytes == null)
            {
                fileBytes = System.IO.File.ReadAllBytes(DigitalLOA(Pid));

            }
            if (fileBytes != null)
                return File(fileBytes, MediaTypeNames.Application.Octet, ImageName);
            else
            {
                return RedirectToAction("Index", new { CaseStatus = "WIP" });
            }

        }
        [ValidateInput(false)]
        public ActionResult GetReportForColorCodeReview(string ImageName, string CaseStatus)
        {
            ImageName = (clsCommonMethods.PDecrypt(ImageName));
            string ReportUploadedName = "";
            DataTable ddt = new ClientCustomize().getUploadedReportCompRefNo(ImageName);
            int ClientRowID = 0;
            string ClientRefID = "";
            string CandidateName = "";
            string EmploymentId = "";
            string EmploymentType = "";

            if (ddt.Rows.Count > 0)
            {
                ClientRowID = Convert.ToInt32(ddt.Rows[0]["ClientRowID"].ToString());
                if (ClientRowID == 268)
                {
                    ClientRefID = ddt.Rows[0]["ClientRefID"].ToString();
                    CandidateName = ddt.Rows[0]["CandidateFullName"].ToString();

                    string[] A = ClientRefID.Split('/');
                    if (A.Count() > 2)
                    {
                        EmploymentType = A[1].ToString();
                        EmploymentId = A[0].ToString();
                    }

                    ReportUploadedName = ImageName + "-" + ddt.Rows.Count + ".pdf";

                    if (EmploymentType == "Direct Trainee")
                    {
                        ImageName = "BGC_FR_" + EmploymentId + "_" + CandidateName.Trim();
                    }
                }
                else
                {
                    ReportUploadedName = ImageName + "-" + ddt.Rows.Count + ".pdf";
                }

            }
            else
            {
                ReportUploadedName = ImageName + ".pdf";
            }
            string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
            byte[] fileBytes = null;

            if (ReportUploadedName.Contains("24/7CPL"))
            {
                ReportUploadedName = ReportUploadedName.Replace("24/7CPL", "24-7CPL");
            }
            if (System.IO.File.Exists(Server.MapPath(FldPathDoc) + ReportUploadedName))
            {
                fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FldPathDoc) + ReportUploadedName);
            }
            if (ddt.Rows.Count > 0)
            {
                ClientRowID = Convert.ToInt32(ddt.Rows[0]["ClientRowID"].ToString());
                if (ClientRowID == 787)
                {
                    if (fileBytes != null)
                    {
                        var ReportFileName = repoDashboardClient.downloadReportFileName(ImageName);
                        return File(fileBytes, MediaTypeNames.Application.Octet, ReportFileName + ".pdf");
                    }
                    else
                    {
                        return RedirectToAction("ManageColorCodeReviewforSpoc", new { CaseStatus = "ColorCodeReview" });
                    }
                }
                else
                {
                    if (fileBytes != null)
                        return File(fileBytes, MediaTypeNames.Application.Octet, ImageName + ".pdf");
                    else
                    {
                        return RedirectToAction("ManageColorCodeReviewforSpoc", new { CaseStatus = "ColorCodeReview" });
                    }
                }
            }

            else
            {
                if (fileBytes != null)
                    return File(fileBytes, MediaTypeNames.Application.Octet, ImageName + ".pdf");
                else
                {
                    return RedirectToAction("ManageColorCodeReviewforSpoc", new { CaseStatus = "ColorCodeReview" });
                }
            }

        }

        #region ********** UnBlockUserClient  **********

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult UnBlockedUserClient(int page = 1, string sort = "CRPUserName", string sortDir = "asc",
            string ClientName = "", string UserName = "")
        {
            PQClientUserListPagedModel model = new PQClientUserListPagedModel();
            try
            {
                BindClientList();
                int pageSize = clsGlobalCommon.PageSize;
                model = repoLogin.GetBlockedClientUser(page, pageSize, sort.ToLower(), sortDir.ToLower(), ClientName, UserName);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public JsonResult UnBlockedClientUsers(short id = 0, string checkeds = "")
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    if (checkeds.Trim().ToLower() == "true")
                    {
                        checkeds = "1";
                    }
                    if (checkeds.Trim().ToLower() == "false")
                    {
                        checkeds = "0";
                    }
                    UnBlockedClientUsersViewModel model = new UnBlockedClientUsersViewModel();
                    model.UnBlockedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                    model.UnBlockedDate = DateTime.Now;
                    model.ClientUserRowID = id;
                    model.Status = Convert.ToByte(checkeds);
                    if (model.UnBlockedBy > 0)
                    {
                        repoLogin.UnBlockedClientUser(model);
                        int Res = repoLogin.SaveChanges();
                        if (Res > 0)
                        {
                            result = true;
                            // Send_Mail_UnblockedPassword_Client
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invalid Id!";
                    }

                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult PersonalInformation(string PersonalRowID)
        {
            Candidate_InformationViewModel model = new Candidate_InformationViewModel();
            PersonalRowID = clsCommonMethods.PDecrypt(PersonalRowID);
            ClientCustomize cc = new ClientCustomize();
            DataTable dt3 = new DataTable();
            if (PersonalRowID != "")
            {
                dt3 = cc.totalwipcandidate_informationForApproval(Convert.ToInt32(PersonalRowID));
            }
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 50, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            if (dt3.Rows.Count > 0)
            {
                model.ApprovalList = new List<candidate_informationForApprovalViewModel>();
                model.CheckName = dt3.Rows[0]["CheckName"].ToString();
                model.CheckStatus = dt3.Rows[0]["CheckStatus"].ToString();
                if (dt3.Rows.Count > 0)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = cc.totalwipcandidate_informationApproval(Convert.ToInt32(PersonalRowID));
                    if (dt1.Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        if (PersonalRowID != "")
                        {
                            dt = cc.GetTotalWipDetails(Convert.ToInt32(PersonalRowID));
                        }
                        PdfPTable table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 20f;
                        table.SpacingAfter = 30f;

                        string CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy");
                        PdfPCell cell = new PdfPCell(new Phrase("Case Initiation Date:- " + CreatedDate + "", FontFactory.GetFont("Times New Roman ,serif", 12)));
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.Border = 0;
                        table.AddCell(cell);
                        pdfDoc.Add(table);

                        table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 10f;
                        table.SpacingAfter = 10f;
                        string CandTitle = dt.Rows[0]["CandTitle"].ToString();
                        string LOAName = dt.Rows[0]["LOA_Name"].ToString();
                        cell = new PdfPCell(new Phrase("Information provided by " + CandTitle + " " + LOAName + " for Background Verification", FontFactory.GetFont("Times New Roman ,serif", 14, Element.ALIGN_MIDDLE)));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 0;
                        table.AddCell(cell);
                        pdfDoc.Add(table);

                        table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 10f;
                        table.SpacingAfter = 10f;
                        string ClientName = dt.Rows[0]["ClientName"].ToString();
                        cell = new PdfPCell(new Phrase("Case Initiated by:-" + ClientName + "", FontFactory.GetFont("Times New Roman ,serif", 14, Element.ALIGN_LEFT)));
                        cell.Border = 0;
                        table.AddCell(cell);
                        pdfDoc.Add(table);



                        table = new PdfPTable(3);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 20f;
                        table.SpacingAfter = 30f;
                        cell = new PdfPCell(new Phrase("Personal Info", FontFactory.GetFont("Times New Roman ,serif", 16, BaseColor.WHITE)));
                        cell.Colspan = 3;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = new BaseColor(129, 211, 201);
                        table.AddCell(cell);
                        for (int k = 0; k < dt1.Rows.Count; k++)
                        {
                            string Label = dt1.Rows[k]["Label"].ToString();
                            string Information = dt1.Rows[k]["Information"].ToString();
                            table.AddCell(Label);
                            cell = new PdfPCell(new Phrase(Information));
                            cell.Colspan = 2;
                            table.AddCell(cell);
                        }
                        pdfDoc.Add(table);
                    }
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        int subChheckRowId = Convert.ToInt32(dt3.Rows[i]["SubCheckRowid"]);
                        string CheckName = model.CheckName = dt3.Rows[i]["CheckName"].ToString();

                        DataTable dt2 = new DataTable();
                        dt2 = cc.totalwipcandidate_informationForApproval_Check(Convert.ToInt32(PersonalRowID), subChheckRowId);
                        if (dt2.Rows.Count > 0)
                        {
                            PdfPTable table = new PdfPTable(5);
                            table.WidthPercentage = 100;
                            table.HorizontalAlignment = 0;
                            table.SpacingBefore = 20f;
                            table.SpacingAfter = 30f;
                            PdfPCell cell = new PdfPCell();
                            cell.Colspan = 5;
                            cell.Border = 0;
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(CheckName, FontFactory.GetFont("Times New Roman ,serif", 16, BaseColor.WHITE)));
                            cell.Colspan = 5;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = new BaseColor(129, 211, 201);
                            table.AddCell(cell);
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                string Label = dt2.Rows[j]["Label"].ToString();
                                string Information = dt2.Rows[j]["Information"].ToString();
                                cell = new PdfPCell(new Phrase(Label));
                                cell.Colspan = 2;
                                table.AddCell(cell);
                                cell = new PdfPCell(new Phrase(Information));
                                cell.Colspan = 3;
                                table.AddCell(cell);
                            }
                            pdfDoc.Add(table);
                        }

                    }

                }
            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=CadidateDetails.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
            return View(model);
        }

        public void MIS_Insuff(string ClientRowId, string dtf, string dtt)
        {


            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);

            string SpocEmail = Session["TeamMemberEmail"].ToString();
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();


                ds = cc.GetInsuffDetailsMIS(SpocEmail);
                if (ds.Tables.Count > 0)
                {
                    var dt = new DataTable("Details");
                    dt = ds.Tables[0];
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
                        Response.AddHeader("content-disposition", "attachment;filename=Insuff-MIS.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [ValidateInput(false)]
        public ActionResult ConcentrixMIS()
        {
            int clientrowid = 607;
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();


                ds = cc.GetConcentrixMIS(clientrowid);


                //ds = cc.GetConcentrixMIS(clientrowidmis);
                if (ds.Tables.Count > 0)
                {
                    var dt1 = new DataTable("Details");
                    dt1 = ds.Tables[0];
                    using (ClosedXML.Excel.XLWorkbook wb1 = new ClosedXML.Excel.XLWorkbook())
                    {
                        var ws1 = wb1.Worksheets.Add(dt1);
                        ws1.Tables.FirstOrDefault().ShowAutoFilter = false;
                        //wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb1.Style.Font.Bold = true;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=concentrix-MIS.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb1.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View();
        }
        [HttpGet]
        [SessionExpireFilter]
        [ValidateInput(false)]
        public ActionResult ReportUploadOnOldPortal(string Msg = "")
        {
            ReportUploadOnOldPortal model = new ReportUploadOnOldPortal();
            model.ClientRowId = 0;
            model.CompanyRefNo = "";
            model.ColorName = "";
            model.ReportStatus = "";
            model.StatusYesNo = true;
            BindColorCodeOld("");
            if (Msg != "")
            {
                ViewBag.ErrorMsg = Msg;
            }
            return View();
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ReportUploadOnOldPortal(ReportUploadOnOldPortal Model, HttpPostedFileBase objReport)
        {
            string rep_status = Convert.ToString(Model.ReportStatus);
            string ColorName = Convert.ToString(Model.ColorName);
            string cid = Model.ClientRowId.ToString();
            string CompanyRefNo = Model.CompanyRefNo;
            bool StatusYesNo = Model.StatusYesNo;

            try
            {
                int userId = Convert.ToInt32(Session["TeamMemberRowID"].ToString());
                var objReport1 = Model.ObjOldReport;
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetPersonalRowIdOLDDB(CompanyRefNo);
                ViewBag.ErrorMsg = "";
                if (objReport1 == null)
                {
                    ViewBag.ErrorMsg = "Please Select Report!";
                }
                if (ColorName == "0" || ColorName == "")
                {
                    ViewBag.ErrorMsg = "Please Select Color Code.!";
                }
                if (rep_status == "-- Select --")
                {
                    ViewBag.ErrorMsg = "Please Select Report Status.!";
                }
                if (ViewBag.ErrorMsg == "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        int PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"]);
                        int ClientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                        int CaseStausID = Convert.ToInt32(dt.Rows[0]["CaseStausID"]);
                        string ReportName = Convert.ToString(dt.Rows[0]["ReportUploadedName"]);
                        string FldPathDoc = string.Empty;
                        FldPathDoc = ConfigurationManager.AppSettings["CandidateReportdoc_Old".Trim().ToLower()].ToString();
                        string fileSavePath = FldPathDoc + ReportName;
                        objReport1.SaveAs(fileSavePath);

                        if (StatusYesNo == true)
                        {
                            string InterimRptSendStatus = "";
                            string FinalRptSendStatus = "";
                            if (rep_status == "Interim")
                            {
                                InterimRptSendStatus = "Completed";
                            }
                            else if (rep_status == "Final")
                            {
                                FinalRptSendStatus = "Completed";
                            }
                            cc.UploadReportDetailsOldDB(PersonalRowID, ReportName, InterimRptSendStatus, FinalRptSendStatus);
                            cc.finalReportDetailOldDB(PersonalRowID, userId, rep_status, ColorName, ReportName, 1);
                        }
                        ViewBag.SuccessMsg = "Report Upload Successfully";
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Please Enter Valid Company Ref No.!";

                    }
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Report Upload failed !.";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return RedirectToAction("ReportUploadOnOldPortal", "ClientServicing", new { Msg = ViewBag.ErrorMsg });

            }
            BindColorCodeOld(CompanyRefNo);

            Model.ClientRowId = 0;
            Model.CompanyRefNo = "";
            Model.ColorName = "";
            Model.ReportStatus = "";
            Model.StatusYesNo = false;
            return View();
        }
        public JsonResult ColorNameList(string CompanyRefNo)
        {

            var ColorList = repoReportQC.getcolorcodeByCompanyRefNo(CompanyRefNo);

            return Json(new SelectList(ColorList.ToArray(), "ColorId", "ColorName"), JsonRequestBehavior.AllowGet);
        }
        [NonAction]
        void BindColorCodeOld(string CompanyRefNo)
        {
            try
            {
                ViewBag.Color = repoReportQC.getcolorcodeByCompanyRefNo(CompanyRefNo);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult StopCheckVerification()
        {
            CheckVerificationViewModel model = new CheckVerificationViewModel();
            model.CompanyRefNo = "";
            model.ClientName = "";
            model.PersonalRowId = 0;
            model.CheckVerificationList = null;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult StopCheckVerification(FormCollection Frm)
        {
            CheckVerificationViewModel model = new CheckVerificationViewModel();
            string CompanyRefNo = Frm["CompanyRefNo"].ToString();
            model = repoVerificationInfo.GetCheckVerificationDeatils(CompanyRefNo);

            return View(model);
        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult UpdateCheckVerification(string Id, string msg = "")
        {
            if (clsCommonMethods.PDecrypt(msg) != "")
            {
                ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(msg);
            }
            StopCheckVerificationView model = new StopCheckVerificationView();
            model = repoVerificationInfo.GetCheckVerificationByCandCheckRowID(Convert.ToInt32(clsCommonMethods.PDecrypt(Id)));
            BindDispositions(Convert.ToInt16(model.CheckFamilyRowId), Convert.ToInt16(model.ClientRowId));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UpdateCheckVerification(FormCollection Frm)
        {
            StopCheckVerificationView model = new StopCheckVerificationView();
            int CandCheckRowID = Convert.ToInt32(Frm["CandCheckRowID"]);
            try
            {
                model.ReportComments = Frm["Remark"].ToString();
                model.DispositionRowID = Convert.ToInt32(Frm["hdnDispositionID"]);
                model.ColorCode = Frm["ColorsName"].ToString();
                model.SeverityRowID = Convert.ToInt32(Frm["hdnSeverity"]);
                model.CandCheckRowId = Convert.ToInt32(Frm["CandCheckRowID"]);
                int userId = Convert.ToInt32(Session["TeamMemberRowID"].ToString());
                ViewBag.ErrorMsg = "";
                if (model.ReportComments == "")
                {
                    ViewBag.ErrorMsg = "Please Fill Report Comments !";
                }
                if (model.DispositionRowID <= 0)
                {
                    ViewBag.ErrorMsg = "Please select Disposition !";
                }
                ClientCustomize CC = new ClientCustomize();
                if (ViewBag.ErrorMsg == "")
                {
                    if (model.CandCheckRowId > 0 && model.DispositionRowID > 0)
                    {
                        CC.UpdatePQCandidateChecksStopStatus(model.DispositionRowID, model.SeverityRowID, model.ColorCode, model.CandCheckRowId, model.ReportComments, userId);
                        ViewBag.SuccessMsg = "Check Status Update Successfully";
                        return RedirectToAction("UpdateCheckVerification", "ClientServicing", new { id = clsCommonMethods.PEncrypt(CandCheckRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.SuccessMsg) });
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Error Founds !";
                        return RedirectToAction("UpdateCheckVerification", "ClientServicing", new { id = clsCommonMethods.PEncrypt(CandCheckRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                    }
                }
                else
                {
                    return RedirectToAction("UpdateCheckVerification", "ClientServicing", new { id = clsCommonMethods.PEncrypt(CandCheckRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Stop Check Verification failed !.";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return RedirectToAction("UpdateCheckVerification", "ClientServicing", new { id = clsCommonMethods.PEncrypt(CandCheckRowID.ToString()), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }

            return View(model);
        }

        void BindDispositions(short ChFamilRowId = 0, short ClientRowId = 0)
        {
            try
            {
                ViewBag.Disposition = repoDDL.DispositionForDDL(ChFamilRowId, ClientRowId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        public ViewResult MIS_MIS()
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string Department = Session["Department"].ToString();
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            BindClientListByLogInNameWithOutEncrypt(SpocEmail);
            return View();
        }

        public ViewResult Generate_MIS(string NM = "", string DateFrom = "", string DateTo = "", string clid = "")
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            try
            {
                DataTable DT = new DataTable();
                ClientCustomize cc = new ClientCustomize();

                if (clid == "null" || clid == "" || clid == "undefined")
                {
                    clid = "0";
                }
                if (teamMemerRowId == 160 && NM == "WIPPOOL")
                {
                    DT = cc.MIS_WIPPOOL_ALL(teamMemerRowId, DateFrom, DateTo, clid);
                }
                else
                {
                    DT = cc.MIS_All_MultipleClientUserWise(NM, DateFrom, DateTo, clid, SpocEmail);
                }

                if (DT.Rows.Count > 0)
                {
                    if (NM == "Capgemini MIS")
                    {
                        ExportToExcel(DT, "Capgemini_MIS");
                    }
                    else
                    {
                        ExportToExcel(DT, "All_MIS");
                    }
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientListByLogInName(SpocEmail);
            return View("~/Views/ClientServicing/MIS_MIS.cshtml");
        }

        // our code starts here
        [HttpGet]
        [SessionExpireFilter]
        [ValidateInput(false)]
        public ActionResult PurgDataDetails()
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string Designation = Session["Designation"].ToString();
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            BindClientListByLogInName(SpocEmail);
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PurgDataDetails(PurgDataDetailsModel model, HttpPostedFileBase file, FormCollection form)
        {

            try
            {

                string OriginalFileName = file.FileName;

                string SpocEmail = Session["TeamMemberEmail"].ToString();
                model.ClientRow = Convert.ToInt32(clsCommonMethods.PDecrypt(form["ClientRowID"].ToString()));
                ClientCustomize cc = new ClientCustomize();
                int TeammemberId = Convert.ToInt32(Session["TeamMemberRowID"]);
                string SPOCName = cc.GetUserNameForPurg(TeammemberId);
                DataTable dt = new DataTable();
                dt = cc.GetClientNameForPurg(model.ClientRow);

                model.ClientName = Convert.ToString(dt.Rows[0]["ClientName"]);
                string Address = Convert.ToString(dt.Rows[0]["Address"]);
                string Location = Convert.ToString(dt.Rows[0]["LocationName"]);
                cc = new ClientCustomize();
                if (file != null)
                {

                    var FileNameOnly = Path.GetFileNameWithoutExtension(file.FileName);
                    var Ext = Path.GetExtension(file.FileName);
                    string CompleteFileName = (model.ClientRow + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + FileNameOnly).Replace("'", "_").Replace(" ", "_").Replace("-", "_").Replace("+", "_").Replace("&", "_") + Ext;
                    model.FilePath = Url.Content(Path.Combine("~/Documents/PurgDocuments", CompleteFileName));
                    cc.InsertToPurg(model.ClientRow, model.DateFrom, model.DateTo, CompleteFileName, model.FilePath, file, TeammemberId, OriginalFileName, model.Remarks);
                    if (file.ContentLength > 0)
                    {
                        string _FileName = CompleteFileName;
                        string _path = Path.Combine(Server.MapPath("~/Documents/PurgDocuments"), _FileName);
                        file.SaveAs(_path);
                    }
                }
                else
                {
                    model.FilePath = "";
                    string CompleteFileName = "";
                    cc.InsertToPurg(model.ClientRow, model.DateFrom, model.DateTo, CompleteFileName, model.FilePath, file, TeammemberId, OriginalFileName, model.Remarks);
                }

                ViewBag.Message = "Successfully!! Submitted";
                DataTable dataTable = new DataTable();
                dataTable = cc.GetLastPurgDataID();
                int res = Convert.ToInt32(dataTable.Rows[0]["Id"]);

                string OperationHeadEmail = Convert.ToString(dataTable.Rows[0]["OperationHeadEmail"]);

                string id = clsCommonMethods.PEncrypt(res.ToString());

                string OPTMHead = clsCommonMethods.PEncrypt("OPTMHead");

                BindClientListByLogInName(SpocEmail);

                //===========================================
                #region Send Mail to Purg
                clsSendMail mail = new clsSendMail();

                var attachment = new System.Net.Mail.Attachment(file.InputStream, file.FileName);
                string DisplayName = "Purge Data Information";
                string From = "bgv.request@securitas-india.com";

                string Subject = "Purging Mail";


                //Mail for OPTM head

                mail = new clsSendMail();
                bool status = false;
                string ToMail1 = OperationHeadEmail;
                string bodyOPTM = "<div><p>Dear Team,</p><p>&nbsp;&nbsp;&nbsp;&nbsp; <strong>Greetings</strong> of the day ! </p><p> &nbsp;&nbsp;&nbsp;&nbsp; Data Purging Request By " + "<strong>" + SPOCName + "</strong>" + "</p> <p>&nbsp;&nbsp;&nbsp;&nbsp; Please, find purging data request detail below:</p></div></br>"
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
                    "<td>" + model.ClientName + "</td>" +
                    "<td>" + Address + "</td>" +
                    "<td>" + Location + "</td>" +
                    " <td>" + model.DateFrom.ToShortDateString() + "</td>" +
                    "<td>" + model.DateTo.ToShortDateString() + "</td>" +
                    "<td> </td>" +
                    "<td> </td>" +
                    "</tr> " +
                    "</table>" +
                    "<br>" +
                    "<a href='https://bgv.securitas-india.com/Account/ApprovePurgData?ID=" + id + "&ApprovalPerson=" + OPTMHead + "'>Click me for approval.</a></div>"
                    + "<br><br><br><br><br><div><strong>Regards</strong></div><div>BGVHelpdesk</div>";

                status = mail.SendMailPurg(DisplayName, From, ToMail1, Subject, bodyOPTM, attachment);
                if (status == true)
                {
                    string User = "OPHead";
                    cc.InsertMailTimeForPurg(User, res);
                }

                //mail to spoc
                mail = new clsSendMail();
                bool statusspoc = false;
                string ToSpoc = SpocEmail;
                string bcc = Convert.ToString(dataTable.Rows[0]["ReportDumpEmail"]);
                string bodySPOC = "<div><p>Dear &nbsp;" + SPOCName + ",</p><p>&nbsp;&nbsp;&nbsp;&nbsp; <strong>Greetings</strong> of the day ! </p><p> &nbsp;&nbsp;&nbsp;&nbsp; Congratulations, Data Purging Request has been received successfully!!   " + "</p> <p>&nbsp;&nbsp;&nbsp;&nbsp; Please, find Purging Data Requested by you below:</p></div></br>"
                    + "<table border='1' cellpadding='0' cellspacing='0' width = '100%' class='table table-bordered'>" +
                    "<tr>" +
                    "<th> Client Name</th>" +
                    "<th> Address</th>" +
                    "<th> Location </th>" +
                    "<th>Purg Date From</th>" +
                    "<th>Purg Date to</th>" +
                    "</tr>" +
                    "<tr>" +
                    "<td>" + model.ClientName + "</td>" +
                    "<td>" + Address + "</td>" +
                    "<td>" + Location + "</td>" +
                    " <td>" + model.DateFrom.ToShortDateString() + "</td>" +
                    "<td>" + model.DateTo.ToShortDateString() + "</td>" +
                    "</tr> " +
                    "</table>" +
                    "<br><br>" +
                    "<div>Please note that the purging request process is expected to take approximately 2 to 3 weeks or longer, depending on the size of the data. Once the process is complete and approvals are finalized, you will be notified. In the meantime, you can monitor the status of your request by using the History link provided in PurgDataDetails Page/Tab</div>" +
                     "<br><br><br><br><br><div><strong>Regards</strong></div><div>BGVHelpdesk</div>";
                statusspoc = mail.SendMailPurgWithoutCC(DisplayName, From, ToSpoc, bcc, Subject, bodySPOC, attachment);
                if (statusspoc == true)
                {
                    string User = "Spoc";
                    cc.InsertMailTimeForPurg(User, res);
                }


                #endregion

                //===========================================
            }
            catch (Exception ex)

            {
                ViewBag.Message = ex.Message;
                //ViewBag.Message = "Form Submitted,Document not uploaded";
                return View();
            }
            finally
            {
                ModelState.Clear();

            }

            return View();
        }

        public ActionResult DownloadFile(string filePath)
        {
            if (filePath != "")
            {


                string fullName = Server.MapPath("~" + filePath);

                byte[] fileBytes = GetFile(fullName);
                return File(
                    fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
            }
            else
            {
                ViewBag.Message = "File Not Uploaded";
                return RedirectToAction("PurgDataDetailsGrid");
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

        [ValidateInput(false)]
        public ActionResult PurgDataDetailsGrid()
        {
            try
            {


                string SpocEmail = Session["TeamMemberEmail"].ToString();
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetPurgData(SpocEmail);
                List<PurgDataDetailsModel> list = new List<PurgDataDetailsModel>();
                foreach (DataRow dataRow in dt.Rows)
                {
                    list.Add(
                        new PurgDataDetailsModel
                        {
                            ClientRow = Convert.ToInt32(dataRow["ClientRowID"]),
                            DateFrom = Convert.ToDateTime(dataRow["PurgDateFrom"]),
                            DateTo = Convert.ToDateTime(dataRow["PurgDateTo"]),
                            ApprovalByOperationHead = Convert.ToBoolean(dataRow["ApprovalByOPTMHead"]),
                            ApprovalByIT = Convert.ToBoolean(dataRow["ApprovalByITHead"]),
                            ReportDump = Convert.ToBoolean(dataRow["ReportDump"]),
                            CheckDocDump = Convert.ToBoolean(dataRow["DocDump"]),
                            FileName = Convert.ToString(dataRow["FileName"]),
                            FilePath = Convert.ToString(dataRow["FilePath"]),
                            ClientName = Convert.ToString(dataRow["ClientName"])
                        });
                }


                return View(list);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Content(message);
            }
        }
        [ValidateInput(false)]
        public ActionResult PurgeDetails()
        {
            try
            {
                DateTime dt1 = new DateTime();
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetPurgeDetails();
                List<PurgDataDetailsModel> list = new List<PurgDataDetailsModel>();
                foreach (DataRow dataRow in dt.Rows)
                {
                    list.Add(
                        new PurgDataDetailsModel
                        {
                            ClientRow = Convert.ToInt32(dataRow["ClientRowID"]),
                            DateFrom = Convert.ToDateTime(dataRow["PurgDateFrom"]),
                            DateTo = Convert.ToDateTime(dataRow["PurgDateTo"]),
                            ApprovalByOperationHead = Convert.ToBoolean(dataRow["ApprovalByOPTMHead"]),
                            ApprovalByIT = Convert.ToBoolean(dataRow["ApprovalByITHead"]),
                            ReportDump = Convert.ToBoolean(dataRow["ReportDump"]),
                            CheckDocDump = Convert.ToBoolean(dataRow["DocDump"]),
                            FileName = Convert.ToString(dataRow["FileName"]),
                            FilePath = Convert.ToString(dataRow["FilePath"]),
                            ClientName = Convert.ToString(dataRow["ClientName"]),
                            CreatedDate = Convert.ToString(dataRow["CreatedDate"]) == " " ? "" : Convert.ToString(dataRow["CreatedDate"]),
                            RequestBy = Convert.ToString(dataRow["RequestedBy"]) == " " ? "" : Convert.ToString(dataRow["RequestedBy"]),
                            Remarks = Convert.ToString(dataRow["Remarks"]) == " " ? "" : Convert.ToString(dataRow["Remarks"])
                        });
                }


                return View(list);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Content(message);
            }
        }

        //our code ends here//


        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ChangeClientPassword(short BindID = 0, string ErrorMsg = "")
        {
            if (ErrorMsg != "")
            {
                ViewBag.ErrorMsg = clsCommonMethods.PDecrypt(ErrorMsg);
            }
            ChangeClientPassword model = new ChangeClientPassword();
            if (BindID != 0)
            {

                model.Value = BindID;
            }
            ClientCustomize cc = new ClientCustomize();
            DataTable dt = new DataTable();
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            List<ChangeClientPassword> list = new List<ChangeClientPassword>();
            dt = cc.GetClientInfoForChangePassword(SpocEmail);
            foreach (DataRow dataRow in dt.Rows)
            {
                list.Add(
                    new ChangeClientPassword
                    {
                        ClientName = Convert.ToString(dataRow["ClientName"]),
                        ClientRowID = Convert.ToInt32(dataRow["ClientRowID"])
                    });
            }
            ViewBag.ClientList = list;
            //BindClientListByLogInName(SpocEmail);
            GetClient(BindID);
            return View(model);
        }

        public JsonResult GetClient(short id = 0)
        {

            List<ChangeClientPassword> list = new List<ChangeClientPassword>();
            try
            {
                Session["selectedid"] = id;
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = cc.GetClientDetails(id);
                foreach (DataRow dataRow in dt.Rows)
                {
                    list.Add(
                        new ChangeClientPassword
                        {
                            LoginID = Convert.ToString(dataRow["UserID"]),
                            Password = clsCommonMethods.PasswordDecrypt(Convert.ToString(dataRow["UPass"])),
                            Status = Convert.ToString(dataRow["Status"]),
                            ID = clsCommonMethods.PEncrypt(Convert.ToString(dataRow["ClientUserRowID"])),
                            ModifiedDate = Convert.ToString(dataRow["ModifiedDate"])
                            //ID = Convert.ToInt32(dataRow["ClientUserRowID"])
                        });
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClientIndex()
        {
            return View();
        }
        [HttpGet]
        [ValidateInput(false)]

        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ManageAbbreviation(int page = 1, string sort = "ClientName", string sortDir = "asc", string Search = "")
        {
            AbbreviationListPagedModel model = new AbbreviationListPagedModel();
            try
            {
                int TeamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                int pageSize = clsGlobalCommon.PageSize;
                model = repoAbbreviation.GetAbbreviationsNew(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search , TeamMemerRowId);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        public void ExportAbbreviation()
        {
            try
            {
                int TeamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                var exportData = repoAbbreviation.GetAbbreviationForExportsNew(TeamMemerRowId);

                WebGrid grid = new WebGrid(source: exportData, canPage: false, canSort: false);
                string gridData = grid.GetHtml(
                    columns: grid.Columns(
                            grid.Column(header: "No.", format: item => item.WebGrid.Rows.IndexOf(item) + 1 + Math.Round(Convert.ToDouble(grid.TotalRowCount / grid.PageCount) / grid.RowsPerPage) * grid.RowsPerPage * grid.PageIndex),
                            grid.Column(columnName: "ClientName", header: "Client"),
                            grid.Column(columnName: "ClientAbbreviation", header: "Abbreviation")
                            )
                        ).ToString();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=Abbreviations" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
                Response.ContentType = "application/excel";
                Response.Write(gridData);
                Response.End();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult AddAbbreviation()
        {
            return View();
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        [ValidateInput(false)]
        public ActionResult AddAbbreviation(AddAbbreviationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.ClientAbbreviation != null && model.ClientName != null && model.ClientAbbreviation.Trim().Length >= 3 && model.ClientName.Trim().Length > 0)
                    {
                        if (!repoAbbreviation.IsAbbreviationExist(model.ClientName.Trim()))
                        {
                            int TeamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                            repoAbbreviation.AddAbbreviationNew(model , TeamMemerRowId);
                            int Res = repoAbbreviation.SaveChanges();
                            if (Res > 0)
                            {
                                TempData["SuccessMsg"] = "Record Added Successfully!";
                                ModelState.Clear();
                                return RedirectToAction("ManageAbbreviation");
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "There is some problem to save record. Please try again later!";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Record Already Exist!";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Client Name could not be blank!");
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
            return View();
        }
        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult EditPQClient(short id = 0)
        {
            UpdatePQClientViewModel model = new UpdatePQClientViewModel();
            try
            {
                BindCountry();
                BindClientAbbList();
                //BindClientSpocList();
                BindPrimaryClientSpocList();
                BindSecondaryClientSpocList();
                BindYesNoList();
                BindCandidateReminderMailGapList();
                BindNoOfReminderList();
                BindRequiredColorCodeReviewList();
                BindReportSentByList();
                BindPricingTypeList();
                BindSeverityTypeList();
                BindBranchOfficeList();
                BindBillingCycleList();
                model = repoPQClient.GetPQClientForUpdateByIdNew(id);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult EditPQClient(UpdatePQClientViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.ClientRowID > 0)
                    {
                        short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);

                        ClientCustomize cc = new ClientCustomize();
                        cc.InsertSpocDeatailLogTable(Convert.ToInt32(model.ClientRowID), model.SpocName, model.SecondarySpocName, model.CSpocEmailID, model.CSpocName, model.CSpocEmailID, Convert.ToInt32(teamMemerRowId));
                        var History = cc.GetauditClientHistory_sp(model.ClientRowID, teamMemerRowId);
                        model.SpocName = string.Concat(model.SpocName, ",", teamMemerRowId.ToString());
                        int Res = cc.UpdatePQClient(model, teamMemerRowId);
                        //int Res = repoPQClient.SaveChanges();
                        if (Res == 0)
                        {

                            TempData["SuccessMsg"] = "Record Edited Successfully!";
                            ModelState.Clear();
                            return RedirectToAction("ManagePQClient");
                        }
                        else
                        {
                            //model = repoPQClient.GetPQClientForUpdateById(model.ClientRowID);
                            model = repoPQClient.GetPQClientForUpdateByIdNew(model.ClientRowID);

                            ViewBag.ErrorMsg = "There is no change!";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Record Already Exist!");
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
            BindCountry();
            BindClientAbbList();
            //BindClientSpocList();
            BindPrimaryClientSpocList();
            BindSecondaryClientSpocList();
            BindYesNoList();
            BindCandidateReminderMailGapList();
            BindNoOfReminderList();
            BindRequiredColorCodeReviewList();
            BindReportSentByList();
            BindPricingTypeList();
            BindSeverityTypeList();
            BindBranchOfficeList();
            BindBillingCycleList();
            return View(model);
        }

        [NonAction]
        void BindBillingCycleList()
        {
            try
            {
                //ViewBag.BillingCycleList = repoDDL.GetBillingCycleForDDL();
                ViewBag.BillingCycleList = repoDDL.GetBillingCycleForDDLNew();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        void BindSeverityTypeList()
        {
            try
            {
                IEnumerable<SelectListItem> SeverityTypeList = new List<SelectListItem> {
                new SelectListItem { Text = "Default", Value = "0" },
                new SelectListItem { Text = "Client", Value = "1" },
            };
                ViewBag.SeverityTypeList = SeverityTypeList;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [NonAction]
        void BindBranchOfficeList()
        {
            try
            {
                //ViewBag.BranchOfficeList = repoDDL.GetBranchOfficeForDDL();
                ViewBag.BranchOfficeList = repoDDL.GetBranchOfficeForDDLNew();


            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindReportSentByList()
        {
            try
            {
                IEnumerable<SelectListItem> ReportSentBy = new List<SelectListItem> {
                new SelectListItem { Text = "Mail", Value = "1" },
                new SelectListItem { Text = "Physical", Value = "2" },
                new SelectListItem { Text = "Both", Value = "3" },
            };
                ViewBag.ReportSentByList = ReportSentBy;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindPricingTypeList()
        {
            try
            {
                IEnumerable<SelectListItem> PricingType = new List<SelectListItem> {
                new SelectListItem { Text = "Package", Value = "1" },
                new SelectListItem { Text = "Check", Value = "2" },
                new SelectListItem { Text = "Both", Value = "3" },
            };
                ViewBag.PricingTypeList = PricingType;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindNoOfReminderList()
        {
            try
            {
                IEnumerable<SelectListItem> NoOfReminderList = new List<SelectListItem> {
                new SelectListItem { Text = "1", Value = "1" },
                new SelectListItem { Text = "2", Value = "2" },
                new SelectListItem { Text = "3", Value = "3" },
                new SelectListItem { Text = "4", Value = "4" },
                new SelectListItem { Text = "5", Value = "5" },
                new SelectListItem { Text = "6", Value = "6" },
                new SelectListItem { Text = "7", Value = "7" },
                new SelectListItem { Text = "8", Value = "8" },
                new SelectListItem { Text = "9", Value = "9" },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "11", Value = "11" },
                new SelectListItem { Text = "12", Value = "12" },
                new SelectListItem { Text = "13", Value = "13" },
                new SelectListItem { Text = "14", Value = "14" },
                new SelectListItem { Text = "15", Value = "15" },
                new SelectListItem { Text = "16", Value = "16" },
                new SelectListItem { Text = "17", Value = "17" },
                new SelectListItem { Text = "18", Value = "18" },
                new SelectListItem { Text = "19", Value = "19" },
                new SelectListItem { Text = "20", Value = "20" },
                new SelectListItem { Text = "21", Value = "21" },
                new SelectListItem { Text = "22", Value = "22" },
                new SelectListItem { Text = "23", Value = "23" },
                new SelectListItem { Text = "24", Value = "24" },
                new SelectListItem { Text = "25", Value = "25" },
            };
                ViewBag.NoOfReminderList = NoOfReminderList;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [NonAction]
        void BindCandidateReminderMailGapList()
        {
            try
            {
                IEnumerable<SelectListItem> CandidateReminderMailGapList = new List<SelectListItem> {
                new SelectListItem { Text = "1 Day", Value = "1" },
                new SelectListItem { Text = "2 Day`s", Value = "2" },
                new SelectListItem { Text = "3 Day`s", Value = "3" },
                new SelectListItem { Text = "4 Day`s", Value = "4" },
                new SelectListItem { Text = "5 Day`s", Value = "5" },
            };
                ViewBag.CandidateReminderMailGapList = CandidateReminderMailGapList;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [NonAction]
        void BindRequiredColorCodeReviewList()
        {
            try
            {
                IEnumerable<SelectListItem> RequiredColorCodeReviewList = new List<SelectListItem> {
                new SelectListItem { Text = "All", Value = "1" },
                new SelectListItem { Text = "NA", Value = "2" },
                new SelectListItem { Text = "None Green", Value = "3" },
            };
                ViewBag.RequiredColorCodeReviewList = RequiredColorCodeReviewList;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [NonAction]
        void BindCountry()
        {
            try
            {
                ViewBag.Countries = repoDDL.GetCountriesForDDL();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [NonAction]
        void BindYesNoList()
        {
            try
            {
                IEnumerable<SelectListItem> YesNoList = new List<SelectListItem> {
                new SelectListItem { Text = "Yes", Value = "1" },
                new SelectListItem { Text = "No", Value = "0" },
            };
                ViewBag.YesNoList = YesNoList;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult EditAbbreviation(short id = 0)
        {
            UpdateAbbreviationViewModel model = new UpdateAbbreviationViewModel();
            try
            {
                model = repoAbbreviation.GetAbbreviationForUpdateById(id);
            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult EditAbbreviation(UpdateAbbreviationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.ClientAbbreviation != null && model.ClientName != null && model.ClientAbbreviation.Trim().Length >= 3 && model.ClientName.Trim().Length > 0)
                    {
                        if (!repoAbbreviation.IsAbbreviationExist(model.ClientName, model.ClientAbbreviation, model.ClientAbbRowID
                            ))
                        {
                            repoAbbreviation.UpdateAbbreviation(model);
                            int Res = repoAbbreviation.SaveChanges();
                            if (Res > 0)
                            {
                                TempData["SuccessMsg"] = "Record Edited Successfully!";
                                ModelState.Clear();
                                return RedirectToAction("ManageAbbreviation");
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "There is no change!";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Record Already Exist!";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Client Name could not be blank!");
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
            return View();
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public JsonResult DeleteAbbreviation(byte id)
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    repoAbbreviation.DeleteAbbreviation(id);
                    int Res = repoAbbreviation.SaveChanges();
                    if (Res > 0)
                    {
                        result = true;
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid Id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult ChangePasswordClient(string LoginID, string CUserID /*int ClientUserID*/)
        {
            UpdateClientPassword model = new UpdateClientPassword();
            try
            {
                int ClientUserID = Convert.ToInt32(clsCommonMethods.PDecrypt(CUserID));
                ViewBag.UserName = LoginID;
                TempData["UserName"] = LoginID;
                TempData["ID"] = ClientUserID;
                model.UserName = LoginID;
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
        public ActionResult ChangePasswordClient(UpdateClientPassword model)
        {
            try
            {
                short SessionId = (short)Session["selectedid"];
                model.UserName = TempData["UserName"].ToString();
                model.WebUserRowID = int.Parse(TempData["ID"].ToString());
                ClientCustomize cc = new ClientCustomize();
                model.Password = clsCommonMethods.PasswordEncrypt(model.Password);
                model.ConfirmPassword = clsCommonMethods.PasswordEncrypt(model.ConfirmPassword);
                model.TeamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                if (model.EncPWD != null)
                {
                    int results = cc.ChangeClientUserCredentials(model.TeamMemerRowId, model.WebUserRowID, model.ConfirmPassword, model.EncPWD);

                    if (results == 1)
                    {
                        ViewBag.SuccessMsg = "Password update successfully";
                        return RedirectToAction("ChangeClientPassword", "ClientServicing", new { BindID = SessionId, ErrorMsg = clsCommonMethods.PEncrypt(ViewBag.SuccessMsg) });

                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Password not update please try again !";
                        return RedirectToAction("ChangePasswordClient", "ClientServicing", new { LoginID = model.UserName, CUserID = clsCommonMethods.PEncrypt(Convert.ToString(model.WebUserRowID)), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Password not update please try again !";
                    return RedirectToAction("ChangePasswordClient", "ClientServicing", new { LoginID = model.UserName, CUserID = clsCommonMethods.PEncrypt(Convert.ToString(model.WebUserRowID)), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Error Founds !";
                return RedirectToAction("ChangePasswordClient", "ClientServicing", new { LoginID = model.UserName, CUserID = clsCommonMethods.PEncrypt(Convert.ToString(model.WebUserRowID)), msg = clsCommonMethods.PEncrypt(ViewBag.ErrorMsg) });
            }

        }

        public ViewResult MIS_MIS2()
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string Department = Session["Department"].ToString();
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            BindClientListByLogInNameWithOutEncrypt(SpocEmail);
            return View();
        }

        public ViewResult Generate_MIS2(string NM = "", string DateFrom = "", string DateTo = "", string clid = "")
        {
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            try
            {
                DataTable DT = new DataTable();
                ClientCustomize cc = new ClientCustomize();

                if (clid == "null")
                {
                    clid = "0";
                }
                DT = cc.MIS_All_MultipleClientUserWise(NM, DateFrom, DateTo, clid, SpocEmail);
                if (DT.Rows.Count > 0)
                {
                    ExportToExcel(DT, "Single_MIS");
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientListByLogInName(SpocEmail);
            return View("~/Views/ClientServicing/MIS_MIS2.cshtml");
        }
        [ValidateInput(false)]
        public ActionResult FirstAmericanActiveUser()
        {
            int clientrowid = 559;
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();


                ds = cc.FirstAmericanActiveUser(clientrowid);


                //ds = cc.GetConcentrixMIS(clientrowidmis);
                if (ds.Tables.Count > 0)
                {
                    var dt1 = new DataTable("Details");
                    dt1 = ds.Tables[0];
                    using (ClosedXML.Excel.XLWorkbook wb1 = new ClosedXML.Excel.XLWorkbook())
                    {
                        var ws1 = wb1.Worksheets.Add(dt1);
                        ws1.Tables.FirstOrDefault().ShowAutoFilter = false;
                        //wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb1.Style.Font.Bold = true;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=First American Active User.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb1.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View();
        }

        public ViewResult ClientDataMis()
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            string Department = Session["Department"].ToString();
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            BindClientListByLogInNameWithOutEncrypt(SpocEmail);
            return View();
        }

        public ViewResult Generate_MIS3(string DateFrom = "", string DateTo = "", string NM = "")
        {
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            try
            {
                DataTable DT = new DataTable();
                ClientCustomize cc = new ClientCustomize();

                if (DateTo == "")
                {
                    DateTo = null;
                }

                DT = cc.ClientMIS(NM, DateFrom, DateTo);
                if (DT.Rows.Count > 0)
                {
                    ExportToExcel(DT, "ClientData");
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientListByLogInName(SpocEmail);
            return View("~/Views/ClientServicing/clientDataMIs.cshtml");
        }




        [ValidateInput(false)]
        public ActionResult clientloginData()
        {
            int ClientUserRowID = Convert.ToInt16(Session["ClientUserRowID"]);
            int ClientRowID = Convert.ToInt32(Session["ClientRowID"]);

            //BindClientListByLogInName(ClientRowID, ClientUserRowID);
            // BindClientList();


            return View();

        }

        public ViewResult Generate_MIS4(string DateFrom = "", string DateTo = "", string NM = "")
        {
            string SpocEmail = Session["TeamMemberEmail"].ToString();
            try
            {
                DataTable DT = new DataTable();
                ClientCustomize cc = new ClientCustomize();

                if (DateTo == "")
                {
                    DateTo = null;
                }

                DT = cc.ClientMIS(NM, DateFrom, DateTo);
                if (DT.Rows.Count > 0)
                {
                    ExportToExcel(DT, "ClientData");
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientListByLogInName(SpocEmail);
            return View("~/Views/ClientServicing/clientloginData.cshtml");
        }
        [ValidateInput(false)]
        public ActionResult CheckStatusDetails()
        {

            try
            {

                string SpocEmail = Session["TeamMemberEmail"].ToString();
                BindClientListByLogInNameWithOutEncrypt(SpocEmail);





            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View();
        }
        [ValidateInput(false)]
        public ActionResult CheckStatusDetailsCasewise(string clid = "")
        {
            if (clid != "")
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();
                int clientrowid = Convert.ToInt32(Session["ClientRowID"]);

                ds = cc.CheckStatusDetailsCasewise(clid);
                if (ds.Tables.Count > 0)
                {
                    var dt = new DataTable("Details");

                    dt = ds.Tables[0];
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
                        Response.AddHeader("content-disposition", "attachment;filename=CheckStatusDetailsCase-wise.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }

            return RedirectToAction("CheckStatusDetails");
        }



        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult CaseActive()
        {
            detailTran d = new detailTran();
            return View(d);
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult CaseActive(detailTran model)
        //{
        //    if (model.companyRefNo != null)
        //    {
        //        DataTable dt = new DataTable();
        //        ClientCustomize cc = new ClientCustomize();
        //        cc.CaseActive(model.companyRefNo);

        //        string test = "Case Active has been successfully !!";
        //        ViewBag.ErrorMsg = test;


        //        ModelState.Clear();
        //        return View("CaseActive");
        //    }
        //    else
        //    {
        //        ViewBag.ErrorMsg = "Please Input CompanyRefNo!";
        //        return View("CaseActive");
        //    }
        //}

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult InsertToTechSupportClientDetails(string ErrMessage = "")
        {
            if (ErrMessage != "")
            {
                ViewBag.ErrMsg = ErrMessage;
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertToTechSupportClientDetails(TechSupportDetails model)
        {
            try
            {


                ClientCustomize cc = new ClientCustomize();
                cc.InsertClientDocumentInsert(model.DocumentType, model.ClientName, model.CheckFamily, model.SubCheckFamily, model.CandidateDocumentVisibility);
                ViewBag.ErrMsg = "Inserted";
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = "Error";
            }
            finally
            {
                ModelState.Clear();
            }
            return View();
        }

        [NonAction]
        private void CreateMenuitemforInsuffClearSpoc(int id = 0, int ClientRowId = 0)
        {
            try
            {
                if (id > 0)
                {
                    ClientCustomize cc = new ClientCustomize();
                    DataSet ds = new DataSet();
                    ds = cc.InsuffCase_CheckDetails(id);
                    string NavInputDataMenuList = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string urlValue = Url.Content(string.Format("~/ClientServicing/InsufficiencyClearBySpoc/?ClientRowId={0}&ChFamilRowId={1}&subCheckId={2}&PersonalRowID={3}", clsCommonMethods.PEncrypt(Convert.ToString(ds.Tables[0].Rows[i]["ClientRowId"])), clsCommonMethods.PEncrypt(Convert.ToString(ds.Tables[0].Rows[i]["CheckFamilyRowID"])), clsCommonMethods.PEncrypt(Convert.ToString(ds.Tables[0].Rows[i]["SubCheckFamilyID"])), clsCommonMethods.PEncrypt(Convert.ToString(id))));
                        NavInputDataMenuList += "<li class='insuff_li mt-2 ps-1'><a class='insuff_DataMenuList' href='" + urlValue + "'> " + Convert.ToString(ds.Tables[0].Rows[i]["SubCheckName"]) + " </a></li>";
                    }
                    Session["NavInputDataMenuList"] = NavInputDataMenuList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult InsufficiencyClearBySpoc(string id = "", string ClientRowId = "", string ChFamilRowId = "", string subCheckId = "", string PersonalRowID = "", string CompanyRefNo = "", string InfRemarks = "")
        {
            InsuffCheckClearViewModel model = new InsuffCheckClearViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                int perId = Convert.ToInt32(clsCommonMethods.PDecrypt(PersonalRowID));
                DataSet ds = new DataSet();
                ds = cc.InsuffCase_CheckDetails(perId);
                short CheckFamilyId = (short)ds.Tables[0].Rows[0]["CheckFamilyRowID"];
                short SubCheckId = (short)ds.Tables[0].Rows[0]["SubCheckFamilyID"];
                short Did = 0;
                Did = Convert.ToInt16(HttpUtility.UrlDecode(clsCommonMethods.PDecrypt(ClientRowId)));
                int Id = id != "" ? Convert.ToInt32(clsCommonMethods.PDecrypt(id)) : 0;//Convert.ToInt32(clsCommonMethods.PDecrypt(id));
                CheckFamilyId = (Convert.ToInt16(clsCommonMethods.PDecrypt(ChFamilRowId)) > 0) ? Convert.ToInt16(clsCommonMethods.PDecrypt(ChFamilRowId)) : CheckFamilyId;
                SubCheckId = (Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId)) > 0) ? Convert.ToInt16(clsCommonMethods.PDecrypt(subCheckId)) : SubCheckId;

                string RefNo = (clsCommonMethods.PDecrypt(CompanyRefNo));
                //string IRemark = (clsCommonMethods.PDecrypt(InfRemarks));
                CreateMenuitemforInsuffClearSpoc(perId, Did);



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
                    if (SubCheckId > 0) { model.SubCheckName = repoSubCheck.GetSubCheckFamilyForUpdateById(SubCheckId).SubCheckName; }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult InsufficiencyClearBySpoc(InsuffCheckClearViewModel model, string UploadHid, IEnumerable<HttpPostedFileBase> UploadDoc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (model.PersonalRowID > 0 && ((!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null) || (!string.IsNullOrEmpty(model.Remarks))))
                    {
                        int i = cc.SaveandUpdateInsuffRemark(model.PersonalRowID, model.SubCheckRowID, model.Remarks, DateTime.UtcNow.ToString(), "");
                        if (!string.IsNullOrEmpty(UploadHid) && UploadDoc != null && UploadDoc.FirstOrDefault() != null)
                        {
                            int count = 0, cnt = 1;
                            string[] DocumentTypesWithFile = model.CheckWithFile.Split(',');
                            foreach (var item in UploadDoc)
                            {
                                string[] checkWithFile = DocumentTypesWithFile[count].Split('|');
                                model.DocumentType = checkWithFile[0];
                                model.FileName = checkWithFile[1];
                                cnt = Convert.ToInt32(checkWithFile[2]);
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
                        clsCommonMethods.AddPQLogTrasaction(model.PersonalRowID, "InsufficiencyClearByClient", "Insuff Clear By Client", model.DocumentType, "Insert");
                        int j = cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                        if (j == 11)
                        {
                            //43 for L1 insuff
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 45, "Spoc", model.ClientRowID);//45 means L1 Insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 45, model.Remarks, "L1-Spoc", model.ClientRowID);
                            cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                            cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                            new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                        }
                        else if (j == 22)
                        {
                            cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 56, "Spoc", model.ClientRowID);// 56 means L2 insuff
                            cc.Process_Remarks_Insert(model.PersonalRowID, model.SubCheckRowID, 56, model.Remarks, "L2-Spoc", model.ClientRowID);
                            cc.UpdateEmpIdInsuffclear(model.PersonalRowID, model.ClientRowID, model.EmployeeID, model.ClientRowID);
                            cc.UpdatePQCandidateChecksDates(model.PersonalRowID, model.SubCheckRowID, "InsuffClear");
                            SendAlertMassageForInsuffClearMail(model.PersonalRowID, model.SubCheckRowID, model.Remarks, "L2 Insuff", "Insuff", 0, 1);
                            new API().UpdateAPICheckStatus(model.ClientRowID.ToString().Trim(), model.PersonalRowID, model.SubCheckRowID, "L1-Insuff Clear");
                        }
                        #endregion

                        #region Case/Check Action History Save 
                        clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, "Insuff Clear By Spoc", model.Remarks);
                        #endregion

                        int PID = model.PersonalRowID;
                        TempData["SuccessMsg"] = "Record Added Successfully!";
                        ModelState.Clear();
                        var CandidateChecks = repoDashboard.GetInsuffCheck(PID);
                        if (CandidateChecks != null && CandidateChecks.Count() > 0)
                        {
                            return RedirectToAction("InsufficiencyClearBySpoc", "ClientServicing", new { ClientRowId = clsCommonMethods.PEncrypt(model.ClientRowID.ToString()), ChFamilRowId = clsCommonMethods.PEncrypt(model.ChFamilRowId.ToString()), subCheckId = clsCommonMethods.PEncrypt(model.SubCheckRowID.ToString()), PersonalRowID = clsCommonMethods.PEncrypt(model.PersonalRowID.ToString()) });
                        }
                        else
                        {
                            return RedirectToAction("ClientServicingDashboardInfo", "ClientServicing", new { CaseStatus = "Insufficient", tmr = Convert.ToInt16(Session["TeamMemberRowID"]), cr = clsCommonMethods.PEncrypt("0") });
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Please select at least one document or mention remarks!";
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
                if (model.SubCheckRowID == 990)
                {
                    ClientCustomize cc = new ClientCustomize();
                    API objApi = new API();
                    string ClientRefID = "";
                    DataTable dtcompany = cc.GetPqpersonalInfo(model.PersonalRowID, model.ClientRowID);
                    if (dtcompany.Rows.Count > 0)
                    {
                        ClientRefID = dtcompany.Rows[0]["ClientRefID"].ToString();
                    }
                    string StrData = "PersonalRowId:" + model.PersonalRowID + " ,SubCheckRowID:" + model.SubCheckRowID + " ,ClientCheckName:" + model.SubCheckName;
                    objApi.InsertAPI_AuditDetail_LTI(ClientRefID, model.ClientRowID, "Failed", "IN PROGRESS", "RequestData:" + StrData, Convert.ToString(Ex.Message), "API", Convert.ToInt32(model.SubCheckRowID));
                }

                ViewBag.ErrorMsg = Ex.Message;
                TempData["SuccessMsg"] = "Record Added Successfully!" + ViewBag.ErrorMsg;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult ManageSpocReviewDashboardInfo(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", string cr = "", string action = "", string test = "")
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                int CheckStatusId = 110;
                short clientid = 0;
                if (cr != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                }

                if (test == "ExportToExcel")
                {
                    DataTable dt = new DataTable();
                    ClientCustomize cc = new ClientCustomize();
                    int[] TeamID = { 524, 882, 1040, 1041, 1042, 1043, 926 };
                    if (Array.Find(TeamID, element => element == Convert.ToInt32(teamMemerRowId)) > 0)
                    {
                        dt = cc.getSpocReview_Dashboard_DetailsAPI(clientid, "110,103", teamMemerRowId);
                    }
                    else
                    {
                        dt = cc.getSpocReview_Dashboard_Details(clientid, CheckStatusId, teamMemerRowId);
                    }

                    DataTable filteredDt = new DataTable();
                    filteredDt.Columns.Add("Client Name");
                    filteredDt.Columns.Add("SecuritasID");
                    filteredDt.Columns.Add("Candidate Name");
                    filteredDt.Columns.Add("Candidate Code");
                    filteredDt.Columns.Add("ClientRefID");
                    filteredDt.Columns.Add("CreatedDate");
                    filteredDt.Columns.Add("Unique Check Id");
                    filteredDt.Columns.Add("SubCheckName");
                    filteredDt.Columns.Add("Company / University Name");
                    filteredDt.Columns.Add("Suggestion Remarks");
                    filteredDt.Columns.Add("Team Member Name");
                    filteredDt.Columns.Add("Suggestion Raised Date");
                    filteredDt.Columns.Add("Ageing");
                    filteredDt.Columns.Add("Spoc Name");
                    filteredDt.Columns.Add("Spoc Review Raised Date");

                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow newRow = filteredDt.NewRow();
                        newRow["Client Name"] = row["ClientName"];
                        newRow["SecuritasID"] = row["SecuritasID"];
                        newRow["Candidate Name"] = row["CandidateName"];
                        newRow["Candidate Code"] = row["CandidateCode"];
                        newRow["ClientRefID"] = row["ClientRefID"];
                        newRow["CreatedDate"] = row["CreatedDate"];
                        newRow["Unique Check Id"] = row["CheckUniqueID"];
                        newRow["SubCheckName"] = row["SubCheckName"];
                        newRow["Company / University Name"] = row["Company/UniversityName"];
                        newRow["Suggestion Remarks"] = row["SuggestionRemarks"];
                        newRow["Team Member Name"] = row["Team Member Name"];
                        newRow["Suggestion Raised Date"] = row["SuggestionRaisedDate"];
                        newRow["Ageing"] = row["Ageing"];
                        newRow["Spoc Name"] = row["SpocName"];
                        newRow["Spoc Review Raised Date"] = row["SpocReviewRaisedDate"];
                        filteredDt.Rows.Add(newRow);
                    }
                    ExportToExcel(filteredDt, "SpocReview");
                }
                else
                {

                    int pageSize = 20;
                    string SpocEmail = Session["TeamMemberEmail"].ToString();
                    string Designation = Session["Designation"].ToString();
                    int[] TeamID = { 524, 882, 1040, 1041, 1042, 1043, 926 };
                    if (Array.Find(TeamID, element => element == Convert.ToInt32(teamMemerRowId)) > 0)
                    {
                        model = repoDashboard.GetSpocReviewDashBoardInfoAPI(page, pageSize, Search, teamMemerRowId, "110,103", clientid);
                    }
                    else
                    {
                        model = repoDashboard.GetSpocReviewDashBoardInfo(page, pageSize, Search, teamMemerRowId, 110, clientid);
                    }
                    if (action != "Search")
                    {
                        var onjlist = model.DashboardClientCandidates;
                    }

                    Session["CaseStatus"] = CaseStatus;
                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                    BindClientListByLogInName(SpocEmail);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        private List<SpecialInstructionVerViewModel> GetSpecialInstructionsRWQC(short ClientRowId, short ChFamilRowId, int designationRowID)
        {
            ClientCustomize cc = new ClientCustomize();
            List<SpecialInstructionVerViewModel> model = new List<SpecialInstructionVerViewModel>();
            var tbl = cc.GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, designationRowID);
            model = (from DataRow dr in tbl.Rows
                     select new SpecialInstructionVerViewModel()
                     {
                         Instructions = Convert.ToString(dr["SpecialInstruction"]),
                         Department = Convert.ToString(dr["DepartmentName"]),
                         Priority = Convert.ToInt32(dr["Priority"])
                     }).ToList();
            return model;
        }
        private int GetDepartmentID(int desigid)
        {
            ClientCustomize cc = new ClientCustomize();
            return cc.GetDesignationID(desigid); ;
        }
        [NonAction]
        void BindDocumentList(int PersonalRowID = 0, short subCheckId = 0)
        {
            try
            {
                ViewBag.GetDocumentList = repoDDL.PQAddressVerGetDocumentList(PersonalRowID, subCheckId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        public void GetCaseDocumentList(int personalRowID = 0)
        {
            try
            {
                ViewBag.CaseDocList = repoPQVerifiedUploadCaseDoc.GetCaseDocumentList(personalRowID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [NonAction]
        void BindMailList(int PersonalRowID = 0, short subCheckId = 0)
        {
            try
            {
                ViewBag.GetMailList = repoVerificationInfo.GetAutoUploadMails(PersonalRowID, subCheckId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UpdateAddressVerInfoBySpoc(int id, int CheckStatusId = 0, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            ClientCustomize cc = new ClientCustomize();
            ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

            short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
            int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
            int dptid = GetDepartmentID(tmid);
            ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
            DataTable dbValue = new DataTable();
            dbValue = cc.GetReportValue(PersonalRowID, subCheckId);

            DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
            DataTable DtActionRemark = new DataTable();
            if (CheckStatusId == 103)
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Client Review");
            }
            else
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
            }
            int SeverityRowID = 0;
            string ColorCode = "";
            string SeverityGrid = "";
            string ReportComment = "";
            if (dts.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                {
                    SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                {
                    ColorCode = dts.Rows[0]["ColorName"].ToString();
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                {
                    SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                }

                if (DtActionRemark.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                    {
                        if (CheckStatusId == 103)
                        {
                            foreach (DataRow row in DtActionRemark.Rows)
                            {
                                ReportComment += Convert.ToString(row["Remarks"]) + "<br/>";
                            }
                        }
                        else
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }
                }

                ViewBag.SeverityRowID = SeverityRowID;
                ViewBag.ColorCode = ColorCode;
                ViewBag.SeverityGrid = SeverityGrid;
            }
            else
            {
                ViewBag.SeverityRowID = 0;
                ViewBag.ColorCode = null;
                ViewBag.SeverityGrid = null;
            }
            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                        if (d[0].ToString() == "34")
                        {
                            string a = rowsFiltered[0][1].ToString();
                            string[] nm = rowsFiltered[0][1].ToString().Split('_');
                            string CountryName = "";
                            if (nm[0] == "")
                            {
                                CountryName = nm[1].ToString();
                            }
                            else
                            {
                                CountryName = nm[0].ToString();
                            }
                            if (CountryName != "")
                            {
                                ViewBag.CountryName = CountryName;
                            }
                            else
                            {
                                ViewBag.CountryName = null;
                            }

                            DataTable dtcountry = cc.GetCountryName(CountryName);
                            if (dtcountry.Rows.Count > 0)
                            {
                                ViewBag.countryid = dtcountry.Rows[0]["CountryRowID"].ToString();
                                ViewBag.CountryName = dtcountry.Rows[0]["CountryName"].ToString();
                            }
                            else
                            {
                                ViewBag.countryid = "";
                                ViewBag.CountryName = "";
                            }
                        }
                        else if (d[1].ToString().Contains("44"))
                        {

                            string a = rowsFiltered[0][1].ToString();
                            string[] nm = rowsFiltered[0][1].ToString().Split('_');
                            string StateName = "";
                            if (nm[0] == "")
                            {
                                StateName = nm[1].ToString();
                            }
                            else
                            {
                                StateName = nm[0].ToString();
                            }
                            if (StateName != "")
                            {
                                ViewBag.StateName = StateName;
                            }
                            else
                            {
                                ViewBag.StateName = null;
                            }

                            DataTable dtstate = cc.GetStateName(StateName);
                            if (dtstate.Rows.Count > 0)
                            {
                                ViewBag.stateId = dtstate.Rows[0]["StateRowID"].ToString();
                                ViewBag.StateName = dtstate.Rows[0]["StateName"].ToString();
                            }
                            else
                            {
                                ViewBag.stateId = "";
                                ViewBag.StateName = "";
                            }
                        }
                        else if (d[1].ToString().Contains("32"))
                        {

                            string a = rowsFiltered[0][1].ToString();
                            string[] nm = rowsFiltered[0][1].ToString().Split('_');
                            string CityName = "";
                            if (nm[0] == "")
                            {
                                CityName = nm[1].ToString();
                            }
                            else
                            {
                                CityName = nm[0].ToString();
                            }
                            if (CityName != "")
                            {
                                ViewBag.CityName = CityName;
                            }
                            else
                            {
                                ViewBag.CityName = null;
                            }
                            DataTable dtCity = cc.GetCityName(CityName);
                            if (dtCity.Rows.Count > 0)
                            {
                                ViewBag.Cityid = dtCity.Rows[0]["DistrictRowID"].ToString();
                                ViewBag.CityName = dtCity.Rows[0]["DistrictName"].ToString();
                            }
                            else
                            {
                                ViewBag.Cityid = "";
                                ViewBag.CityName = "";
                            }
                        }

                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
                {

                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                        if (d[1].ToString().Contains("34"))
                        {
                            ViewBag.CountryName = "";
                            ViewBag.countryid = "";
                        }
                        else if (d[1].ToString().Contains("44"))
                        {
                            ViewBag.StateName = "";
                            ViewBag.stateId = "";
                        }
                        else if (d[1].ToString().Contains("32"))
                        {
                            ViewBag.CityName = "";
                            ViewBag.Cityid = "";
                        }
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }
            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            ViewBag.ReportComment = ReportComment.ToString();
            if (ViewBag.CountryName == null)
            {
                ViewBag.CountryName = "";
                ViewBag.countryid = "";
            }

            if (ViewBag.StateName == null)
            {
                ViewBag.StateName = "";
                ViewBag.stateId = "";
            }

            if (ViewBag.CityName == null)
            {
                ViewBag.CityName = "";
                ViewBag.Cityid = "";
            }
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);

            return View(marksModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAddressVerInfoBySpoc(FormCollection formCollection, string action = "")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();
            string Remarks = formCollection["Remarks"].ToString();
            string APIRemarks = "";
            if (Remarks != "")
            {
                APIRemarks = Regex.Replace(Remarks, "<.*?>", string.Empty);
            }
            string PriorityType = formCollection["PriorityType"].ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        #region LTI Client Review
                        if (CheckStatus == "Approve" || CheckStatus == "Submit")
                        {
                            if (clientRowid == 990)
                            {
                                if (CheckStatusId == 103)
                                {
                                    string DocumentType = "Client Review By API";
                                    string ReportUploadedName = "";
                                    DataTable ddt = new DataTable();
                                    ddt = cc.getClientReviewdocumet(PersonalRowid, SubCheckid, DocumentType);
                                    if (ddt.Rows.Count > 0)
                                    {
                                        ReportUploadedName = Convert.ToString(ddt.Rows[0]["FileName"]);
                                    }
                                    string Res = API.LTIClientReviewStatus(PersonalRowid, clientRowid, SubCheckid, ReportUploadedName, APIRemarks);
                                    if (Res == "OK")
                                    {

                                        cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 118, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Approved");
                                        cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 118, Remarks, "Client Review Approved", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                        #region Save LogTransaction Details  Case/Check Action History Save
                                        clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentVerInfoBySpoc", CheckStatus, "Client Review Approved", "Updated");

                                        clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                        #endregion

                                        ModelState.Clear();
                                        return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { PersonalRowID = PersonalRowid, dispFor = "Dashboard" });
                                    }
                                }
                            }

                        }
                        #endregion
                        if (CheckStatus == "Reject")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateAddressVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");
                            #endregion

                            #region Case/Check Action History Save
                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion

                            ModelState.Clear();
                        }
                        else
                        {
                            var marksModel = expando as IDictionary<string, object>;

                            OperationViewModel cd = new OperationViewModel();
                            cd.PersonalRowID = PersonalRowid;
                            cd.SubCheckRowID = SubCheckid;
                            cd.clientRowId = clientRowid;
                            cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                            cd.moduleName = "Client Servicing";

                            ClientCustomize bl = new ClientCustomize();
                            DataTable dt = new DataTable();
                            dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
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
                                        }
                                    }
                                }
                                else if (d[4].ToString() == "dropdown")
                                {
                                    if (clientRowid == 317 || clientRowid == 327)
                                    {
                                        foreach (string key in formCollection.AllKeys)
                                        {
                                            if (key == d[0].ToString())
                                            {
                                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                            }
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
                                        }
                                    }
                                    if (chkselect == 0)
                                    {
                                        marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                    }
                                }
                                else
                                    marksModel.Add(d[1].ToString(), d[0].ToString());

                            }
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 111, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 111, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                            cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details Case/Check Action History Save
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateAddressVerInfoBySpoc", CheckStatus, "", "Update");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;
            return RedirectToAction("UpdateAddressVerInfoBySpoc", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });

        }

        [HttpGet]
        public ActionResult REALTIMELTmindTRACKER()
        {
            return View();
        }
        [HttpPost]
        public PartialViewResult REALTIMELTmindTRACKER(string CompletedFromDate = "", string CompletedToDate = "")
        {
            int ClientRowId = 990;
            try
            {
                DataTable DT = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                DT = cc.MIS_realtimeTracker_Ltimindtree(CompletedFromDate, CompletedToDate, ClientRowId.ToString());
                ExportToExcel(DT, "MISLTITrackerforCS");
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            //return View(model);
            return PartialView("~/Views/ClientServicing/REALTIMELTmindTRACKER.cshtml");
        }




        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UpdateCriminalVerInfoBySpoc(int id, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0, string msg = "")
        {

            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            ClientCustomize cc = new ClientCustomize();
            ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

            short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
            int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
            int dptid = GetDepartmentID(tmid);
            ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
            DataTable dbValue = new DataTable();
            if ((ClientRowId == 317 || ClientRowId == 327) && (CheckStatusId == 37))
            {
                dbValue = cc.GetReportValueWipro(PersonalRowID, subCheckId);
            }
            else
            {
                dbValue = cc.GetReportValue(PersonalRowID, subCheckId);
            }
            DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
            DataTable DtActionRemark = new DataTable();
            if (CheckStatusId == 103)
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Client Review");
            }
            else
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
            }
            int SeverityRowID = 0;
            string ColorCode = "";
            string SeverityGrid = "";
            string ReportComment = "";
            if (msg != "")
            {
                ViewBag.ErrorMsg = msg;
            }
            if (dts.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                {
                    SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                {
                    ColorCode = dts.Rows[0]["ColorName"].ToString();
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                {
                    SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                }

                if (DtActionRemark.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                    {
                        if (CheckStatusId == 103)
                        {
                            foreach (DataRow row in DtActionRemark.Rows)
                            {
                                ReportComment += Convert.ToString(row["Remarks"]) + "<br/>";
                            }
                        }
                        else
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }
                }
                ViewBag.SeverityRowID = SeverityRowID;
                ViewBag.ColorCode = ColorCode;
                ViewBag.SeverityGrid = SeverityGrid;
                ViewBag.ReportComment = ReportComment;
            }
            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
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
            }
            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);
            return View(marksModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCriminalVerInfoBySpoc(FormCollection formCollection, string action = "")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());
            string Remarks = formCollection["Remarks"].ToString();
            string APIRemarks = "";
            if (Remarks != "")
            {
                APIRemarks = Regex.Replace(Remarks, "<.*?>", string.Empty);
            }
            string PriorityType = formCollection["PriorityType"].ToString();
            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        #region LTI Client Review
                        if (CheckStatus == "Approved" || CheckStatus == "Submit")
                        {
                            if (clientRowid == 990)
                            {
                                if (CheckStatusId == 103)
                                {
                                    string DocumentType = "Client Review By API";
                                    string ReportUploadedName = "";
                                    DataTable ddt = new DataTable();
                                    ddt = cc.getClientReviewdocumet(PersonalRowid, SubCheckid, DocumentType);
                                    if (ddt.Rows.Count > 0)
                                    {
                                        ReportUploadedName = Convert.ToString(ddt.Rows[0]["FileName"]);
                                    }
                                    string Res = API.LTIClientReviewStatus(PersonalRowid, clientRowid, SubCheckid, ReportUploadedName, APIRemarks);
                                    if (Res == "OK")
                                    {

                                        cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 118, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Approved");
                                        cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 118, Remarks, "Client Review Approved", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                        #region Save LogTransaction Details  Case/Check Action History Save
                                        clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentVerInfoBySpoc", CheckStatus, "Client Review Approved", "Updated");

                                        clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                        #endregion

                                        ModelState.Clear();
                                        return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { PersonalRowID = PersonalRowid, dispFor = "Dashboard" });
                                    }
                                }
                            }

                        }
                        #endregion
                        if (CheckStatus == "Reject")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details Case/Check Action History Save
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCriminalVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion

                            ModelState.Clear();

                        }
                        else
                        {
                            var marksModel = expando as IDictionary<string, object>;

                            OperationViewModel cd = new OperationViewModel();
                            cd.PersonalRowID = PersonalRowid;
                            cd.SubCheckRowID = SubCheckid;
                            cd.clientRowId = clientRowid;
                            cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                            cd.moduleName = "ReportQC";

                            ClientCustomize bl = new ClientCustomize();
                            DataTable dt = new DataTable();
                            dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                            foreach (DataRow d in dt.Rows)
                            {
                                string[] FString = d[1].ToString().Split('_');

                                if (d[4].ToString() == "fileUpload")
                                {
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
                                        }
                                    }
                                }
                                else if (d[4].ToString() == "dropdown")
                                {

                                    foreach (string key in formCollection.AllKeys)
                                    {
                                        if (key == d[0].ToString())
                                        {
                                            marksModel.Add(d[1].ToString(), formCollection[key].ToString());
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
                                        }
                                    }
                                    if (chkselect == 0)
                                    {
                                        marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                    }
                                }
                                else
                                    marksModel.Add(d[1].ToString(), d[0].ToString());

                            }
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 111, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 111, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                            cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details Case/Check Action History Save
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateCriminalVerInfoBySpoc", CheckStatus, "", "Update");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);

                            #endregion
                            ViewBag.ErrorMsg = "Record Updated Successfully!.";
                        }
                    }
                    else


                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;

            return RedirectToAction("UpdateCriminalVerInfoBySpoc", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId, msg = ViewBag.ErrorMsg });

        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UpdateNationalCheckVerInfoBySpoc(int id, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0, string msg = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            ClientCustomize cc = new ClientCustomize();
            ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

            short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
            int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
            int dptid = GetDepartmentID(tmid);
            ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
            DataTable dbValue = new DataTable();
            dbValue = cc.GetReportValue(PersonalRowID, subCheckId);
            DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
            DataTable DtActionRemark = new DataTable();
            if (CheckStatusId == 103)
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Client Review");
            }
            else
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
            }
            int SeverityRowID = 0;
            string ColorCode = "";
            string SeverityGrid = "";
            string ReportComment = "";
            if (msg != "")
            {
                ViewBag.ErrorMsg = msg;
            }
            if (dts.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                {
                    SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                {
                    ColorCode = dts.Rows[0]["ColorName"].ToString();
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                {
                    SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                }

                if (DtActionRemark.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                    {
                        if (CheckStatusId == 103)
                        {
                            foreach (DataRow row in DtActionRemark.Rows)
                            {
                                ReportComment += Convert.ToString(row["Remarks"]) + "<br/>";
                            }
                        }
                        else
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }
                }

                ViewBag.SeverityRowID = SeverityRowID;
                ViewBag.ColorCode = ColorCode;
                ViewBag.SeverityGrid = SeverityGrid;
                ViewBag.ReportComment = ReportComment;

            }
            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
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
            }
            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            ViewBag.ReportComment = ReportComment.ToString();
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);
            return View(marksModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateNationalCheckVerInfoBySpoc(FormCollection formCollection, string action = "")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());

            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());

            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();
            string Remarks = formCollection["Remarks"].ToString();
            string APIRemarks = "";
            if (Remarks != "")
            {
                APIRemarks = Regex.Replace(Remarks, "<.*?>", string.Empty);
            }
            string PriorityType = formCollection["PriorityType"].ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        #region LTI Client Review
                        if (CheckStatus == "Approved" || CheckStatus == "Submit")
                        {
                            if (clientRowid == 990)
                            {
                                if (CheckStatusId == 103)
                                {
                                    string DocumentType = "Client Review By API";
                                    string ReportUploadedName = "";
                                    DataTable ddt = new DataTable();
                                    ddt = cc.getClientReviewdocumet(PersonalRowid, SubCheckid, DocumentType);
                                    if (ddt.Rows.Count > 0)
                                    {
                                        ReportUploadedName = Convert.ToString(ddt.Rows[0]["FileName"]);
                                    }
                                    string Res = API.LTIClientReviewStatus(PersonalRowid, clientRowid, SubCheckid, ReportUploadedName, APIRemarks);
                                    if (Res == "OK")
                                    {

                                        cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 118, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Approved");
                                        cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 118, Remarks, "Client Review Approved", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                        #region Save LogTransaction Details  Case/Check Action History Save
                                        clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentVerInfoBySpoc", CheckStatus, "Client Review Approved", "Updated");

                                        clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                        #endregion

                                        ModelState.Clear();
                                        return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { PersonalRowID = PersonalRowid, dispFor = "Dashboard" });
                                    }
                                }
                            }

                        }
                        #endregion
                        if (CheckStatus == "Reject")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details  Case/Check Action History Save
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateNationalCheckVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion

                            ModelState.Clear();
                        }
                        else
                        {
                            var marksModel = expando as IDictionary<string, object>;

                            OperationViewModel cd = new OperationViewModel();
                            cd.PersonalRowID = PersonalRowid;
                            cd.SubCheckRowID = SubCheckid;
                            cd.clientRowId = clientRowid;
                            cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                            cd.moduleName = "ClientServicing";

                            ClientCustomize bl = new ClientCustomize();
                            DataTable dt = new DataTable();
                            dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                            foreach (DataRow d in dt.Rows)
                            {
                                string[] FString = d[1].ToString().Split('_');

                                if (d[4].ToString() == "fileUpload")
                                {
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
                                        }
                                    }
                                }

                                else if (d[4].ToString() == "dropdown")
                                {
                                    if (clientRowid == 317 || clientRowid == 327)
                                    {
                                        foreach (string key in formCollection.AllKeys)
                                        {
                                            if (key == d[0].ToString())
                                            {
                                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                            }
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
                                        }
                                    }
                                    if (chkselect == 0)
                                    {
                                        marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());

                                    }
                                }

                                else
                                    marksModel.Add(d[1].ToString(), d[0].ToString());

                            }
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 111, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 111, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                            cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details Case/Check Action History Save
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateNationalCheckVerInfoBySpoc", CheckStatus, "", "Update");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion
                        }

                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;

            return RedirectToAction("UpdateNationalCheckVerInfoBySpoc", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UpdateSpecialCheckVerInfoBySpoc(int id, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0, string msg = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            ClientCustomize cc = new ClientCustomize();
            ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

            short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
            int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
            int dptid = GetDepartmentID(tmid);
            ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
            DataTable dbValue = new DataTable();
            dbValue = cc.GetReportValue(PersonalRowID, subCheckId);

            DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
            DataTable DtActionRemark = new DataTable();
            if (CheckStatusId == 103)
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Client Review");
            }
            else
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
            }

            int SeverityRowID = 0;
            string ColorCode = "";
            string SeverityGrid = "";
            string ReportComment = "";
            if (msg != "")
            {
                ViewBag.ErrorMsg = msg;
            }
            if (dts.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                {
                    SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                {
                    ColorCode = dts.Rows[0]["ColorName"].ToString();
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                {
                    SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                }
                if (DtActionRemark.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                    {
                        if (CheckStatusId == 103)
                        {
                            foreach (DataRow row in DtActionRemark.Rows)
                            {
                                ReportComment += Convert.ToString(row["Remarks"]) + "<br/>";
                            }
                        }
                        else
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }
                }
            }
            ViewBag.SeverityRowID = SeverityRowID;
            ViewBag.ColorCode = ColorCode;
            ViewBag.SeverityGrid = SeverityGrid;
            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
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
            }
            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            ViewBag.ReportComment = ReportComment.ToString();
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);

            return View(marksModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSpecialCheckVerInfoBySpoc(FormCollection formCollection, string action = "")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();
            string Remarks = formCollection["Remarks"].ToString();
            string APIRemarks = "";
            if (Remarks != "")
            {
                APIRemarks = Regex.Replace(Remarks, "<.*?>", string.Empty);
            }
            string PriorityType = formCollection["PriorityType"].ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        #region LTI Client Review
                        if (CheckStatus == "Approved" || CheckStatus == "Submit")
                        {
                            if (clientRowid == 990)
                            {
                                if (CheckStatusId == 103)
                                {
                                    string DocumentType = "Client Review By API";
                                    string ReportUploadedName = "";
                                    DataTable ddt = new DataTable();
                                    ddt = cc.getClientReviewdocumet(PersonalRowid, SubCheckid, DocumentType);
                                    if (ddt.Rows.Count > 0)
                                    {
                                        ReportUploadedName = Convert.ToString(ddt.Rows[0]["FileName"]);
                                    }
                                    string Res = API.LTIClientReviewStatus(PersonalRowid, clientRowid, SubCheckid, ReportUploadedName, APIRemarks);
                                    if (Res == "OK")
                                    {

                                        cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 118, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Approved");
                                        cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 118, Remarks, "Client Review Approved", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                        #region Save LogTransaction Details  Case/Check Action History Save
                                        clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentVerInfoBySpoc", CheckStatus, "Client Review Approved", "Updated");

                                        clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                        #endregion

                                        ModelState.Clear();
                                        return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { PersonalRowID = PersonalRowid, dispFor = "Dashboard" });
                                    }
                                }
                            }

                        }
                        #endregion
                        if (CheckStatus == "Reject")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateSpecialCheckVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");
                            #endregion

                            #region Case/Check Action History Save
                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion

                            ModelState.Clear();

                        }
                        else
                        {
                            var marksModel = expando as IDictionary<string, object>;

                            OperationViewModel cd = new OperationViewModel();
                            cd.PersonalRowID = PersonalRowid;
                            cd.SubCheckRowID = SubCheckid;
                            cd.clientRowId = clientRowid;
                            cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                            cd.moduleName = "ClientServicing";

                            ClientCustomize bl = new ClientCustomize();
                            DataTable dt = new DataTable();
                            dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                            foreach (DataRow d in dt.Rows)
                            {
                                string[] FString = d[1].ToString().Split('_');

                                if (d[4].ToString() == "fileUpload")
                                {
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
                                        }
                                    }
                                }

                                else if (d[4].ToString() == "dropdown")
                                {
                                    if (clientRowid == 317 || clientRowid == 327)
                                    {
                                        foreach (string key in formCollection.AllKeys)
                                        {
                                            if (key == d[0].ToString())
                                            {
                                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                            }
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
                                        }
                                    }
                                    if (chkselect == 0)
                                    {
                                        marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                    }
                                }
                                else
                                    marksModel.Add(d[1].ToString(), d[0].ToString());

                            }
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 111, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 111, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                            cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details  Case/Check Action History Save
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateSpecialCheckVerInfoBySpoc", CheckStatus, "", "Update");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;

            return RedirectToAction("UpdateSpecialCheckVerInfoBySpoc", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }
        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UpdateEmploymentVerInfoBySpoc(int id, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0, string msg = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            ClientCustomize cc = new ClientCustomize();
            ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

            short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
            int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
            int dptid = GetDepartmentID(tmid);

            if (ChFamilRowId == 12 && ClientRowId == 607)
            {
                DataTable dataTable = cc.GetConxDOJ(PersonalRowID);
                ViewBag.DOJ = dataTable.Rows[0]["DateofJoining"].ToString();
                ViewBag.ConxRow = ClientRowId;

            }
            ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
            DataTable dbValue = new DataTable();
            if ((ClientRowId == 317 || ClientRowId == 327) && (CheckStatusId == 37))
            {
                dbValue = cc.GetReportValueWipro(PersonalRowID, subCheckId);
            }
            else
            {
                dbValue = cc.GetReportValue(PersonalRowID, subCheckId);
                if (ClientRowId == 787)
                {
                    DataRow dr = dbValue.Select("AntecedentRowID=82").FirstOrDefault();
                    if (dr != null)
                    {
                        DataTable dtt = cc.getAutoEmpDetail(PersonalRowID, subCheckId);
                        dr[1] = dtt.Rows[0]["CompanyAddress"].ToString();
                    }
                }
            }
            DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
            DataTable DtActionRemark = new DataTable();
            if (CheckStatusId == 103)
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Client Review");
            }
            else
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
            }
            int SeverityRowID = 0;
            string ColorCode = "";
            string SeverityGrid = "";
            string ReportComment = "";
            if (msg != "")
            {
                ViewBag.ErrorMsg = msg;
            }
            if (dts.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                {
                    SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                {
                    ColorCode = dts.Rows[0]["ColorName"].ToString();
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                {
                    SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                }
                if (DtActionRemark.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                    {
                        if (CheckStatusId == 103)
                        {
                            foreach (DataRow row in DtActionRemark.Rows)
                            {
                                ReportComment += Convert.ToString(row["Remarks"]) + "<br/>";
                            }
                        }
                        else
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }
                }


                ViewBag.SeverityRowID = SeverityRowID;
                ViewBag.ColorCode = ColorCode;
                ViewBag.SeverityGrid = SeverityGrid;

            }
            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());
                }
                else
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
            }
            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            ViewBag.ReportComment = ReportComment.ToString();
            ViewBag.PersonalBind = repoReportQC.GetPersonalDetailsForTM(PersonalRowID, subCheckId);
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);
            return View(marksModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEmploymentVerInfoBySpoc(FormCollection formCollection, string action = "")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());
            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();
            string Remarks = formCollection["Remarks"].ToString();
            string APIRemarks = "";
            if (Remarks != "")
            {
                APIRemarks = Regex.Replace(Remarks, "<.*?>", string.Empty);
            }
            string PriorityType = formCollection["PriorityType"].ToString();
            ClientCustomize cc = new ClientCustomize();

            try
            {
                if (ModelState.IsValid)
                {

                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        #region LTI Client Review
                        if (CheckStatus == "Approved" || CheckStatus == "Submit")
                        {
                            if (clientRowid == 990)
                            {
                                if (CheckStatusId == 103)
                                {
                                    string DocumentType = "Client Review By API";
                                    string ReportUploadedName = "";
                                    DataTable ddt = new DataTable();
                                    ddt = cc.getClientReviewdocumet(PersonalRowid, SubCheckid, DocumentType);
                                    if (ddt.Rows.Count > 0)
                                    {
                                        ReportUploadedName = Convert.ToString(ddt.Rows[0]["FileName"]);
                                    }
                                    string Res = API.LTIClientReviewStatus(PersonalRowid, clientRowid, SubCheckid, ReportUploadedName, APIRemarks);
                                    if (Res == "OK")
                                    {

                                        cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 118, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Approved");
                                        cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 118, Remarks, "Client Review Approved", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                        #region Save LogTransaction Details  Case/Check Action History Save
                                        clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentVerInfoBySpoc", CheckStatus, "Client Review Approved", "Updated");

                                        clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                        #endregion

                                        ModelState.Clear();
                                        return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { PersonalRowID = PersonalRowid, dispFor = "Dashboard" });
                                    }
                                }
                            }

                        }
                        #endregion
                        if (CheckStatus == "Reject")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details  Case/Check Action History Save
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion

                            ModelState.Clear();
                            return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { PersonalRowID = PersonalRowid, dispFor = "Dashboard" });

                        }
                        else
                        {
                            var marksModel = expando as IDictionary<string, object>;

                            OperationViewModel cd = new OperationViewModel();
                            cd.PersonalRowID = PersonalRowid;
                            cd.SubCheckRowID = SubCheckid;
                            cd.clientRowId = clientRowid;
                            cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                            cd.moduleName = "ClientServicing";

                            ClientCustomize bl = new ClientCustomize();
                            DataTable dt = new DataTable();
                            dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                            foreach (DataRow d in dt.Rows)
                            {
                                string[] FString = d[1].ToString().Split('_');

                                if (d[4].ToString() == "fileUpload")
                                {
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
                                        }
                                    }
                                }
                                else if (d[4].ToString() == "dropdown")
                                {
                                    if (clientRowid == 787 || clientRowid == 317)
                                    {
                                        foreach (string key in formCollection.AllKeys)
                                        {
                                            if (key == d[0].ToString())
                                            {
                                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                            }
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
                                        }
                                    }
                                    if (chkselect == 0)
                                    {
                                        marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                    }
                                }
                                else
                                    marksModel.Add(d[1].ToString(), d[0].ToString());

                            }

                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 111, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 111, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                            cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details   Case/Check Action History Save 
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentVerInfoBySpoc", CheckStatus, "", "Update");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion

                            ModelState.Clear();

                            return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { ClientRowID = clientRowid, PersonalRowID = PersonalRowid, dispFor = "Dashboard" });
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;

            return RedirectToAction("UpdateEmploymentVerInfoBySpoc", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UpdateEducationVerInfoBySpoc(int id, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0, string msg = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            ClientCustomize cc = new ClientCustomize();
            ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);


            short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
            int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
            int dptid = GetDepartmentID(tmid);
            ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
            DataTable dbValue = new DataTable();
            dbValue = cc.GetReportValue(PersonalRowID, subCheckId);

            DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
            DataTable DtActionRemark = new DataTable();
            if (CheckStatusId == 103)
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Client Review");
            }
            else
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
            }
            int SeverityRowID = 0;
            string ColorCode = "";
            string SeverityGrid = "";
            string ReportComment = "";
            if (msg != "")
            {
                ViewBag.ErrorMsg = msg;
            }
            if (dts.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                {
                    SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                {
                    ColorCode = dts.Rows[0]["ColorName"].ToString();
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                {
                    SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                }

                if (DtActionRemark.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                    {
                        if (CheckStatusId == 103)
                        {
                            foreach (DataRow row in DtActionRemark.Rows)
                            {
                                ReportComment += Convert.ToString(row["Remarks"]) + "<br/>";
                            }
                        }
                        else
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }
                }
                ViewBag.SeverityRowID = SeverityRowID;
                ViewBag.ColorCode = ColorCode;
                ViewBag.SeverityGrid = SeverityGrid;


            }
            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());

                        if (d[1].ToString().Contains("72"))
                        {
                            string a = rowsFiltered[0][1].ToString();
                            string[] nm = rowsFiltered[0][1].ToString().Split('_');
                            string degreename = "";
                            if (nm[1].ToString() != "")
                            {
                                degreename = nm[1].ToString();
                            }
                            else
                            {
                                degreename = nm[0].ToString();
                                nm[1] = nm[0].ToString();
                            }

                            if (degreename != "")
                            {
                                ViewBag.degreeName = degreename.Trim();
                            }
                            else
                            {
                                ViewBag.degreeName = "";
                            }

                        }
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
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
            }
            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            ViewBag.ReportComment = ReportComment.ToString();
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);

            if (ViewBag.degreeName == null)
            {
                ViewBag.degreeName = "";

            }
            return View(marksModel);
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEducationVerInfoBySpoc(FormCollection formCollection, string action = "")
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());

            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());

            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();
            string Remarks = formCollection["Remarks"].ToString();
            string APIRemarks = "";
            if (Remarks != "")
            {
                APIRemarks = Regex.Replace(Remarks, "<.*?>", string.Empty);
            }
            string PriorityType = formCollection["PriorityType"].ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        #region LTI Client Review
                        if (CheckStatus == "Approved" || CheckStatus == "Submit")
                        {
                            if (clientRowid == 990)
                            {
                                if (CheckStatusId == 103)
                                {
                                    string DocumentType = "Client Review By API";
                                    string ReportUploadedName = "";
                                    DataTable ddt = new DataTable();
                                    ddt = cc.getClientReviewdocumet(PersonalRowid, SubCheckid, DocumentType);
                                    if (ddt.Rows.Count > 0)
                                    {
                                        ReportUploadedName = Convert.ToString(ddt.Rows[0]["FileName"]);
                                    }
                                    string Res = API.LTIClientReviewStatus(PersonalRowid, clientRowid, SubCheckid, ReportUploadedName, APIRemarks);
                                    if (Res == "OK")
                                    {

                                        cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 118, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Approved");
                                        cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 118, Remarks, "Client Review Approved", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                        #region Save LogTransaction Details  Case/Check Action History Save
                                        clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentVerInfoBySpoc", CheckStatus, "Client Review Approved", "Updated");

                                        clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                        #endregion

                                        ModelState.Clear();
                                        return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { PersonalRowID = PersonalRowid, dispFor = "Dashboard" });
                                    }
                                }
                            }

                        }
                        #endregion
                        if (CheckStatus == "Reject")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details  Case/Check Action History Save
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEducationVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion

                            ModelState.Clear();

                        }
                        else
                        {
                            var marksModel = expando as IDictionary<string, object>;

                            OperationViewModel cd = new OperationViewModel();
                            cd.PersonalRowID = PersonalRowid;
                            cd.SubCheckRowID = SubCheckid;
                            cd.clientRowId = clientRowid;
                            cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                            cd.moduleName = "ClientServicing";


                            ClientCustomize bl = new ClientCustomize();
                            DataTable dt = new DataTable();
                            dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                            foreach (DataRow d in dt.Rows)
                            {
                                string[] FString = d[1].ToString().Split('_');

                                if (d[4].ToString() == "fileUpload")
                                {

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
                                        }
                                    }
                                }

                                else if (d[4].ToString() == "dropdown")
                                {
                                    if (clientRowid == 787 || clientRowid == 317 || clientRowid == 327)
                                    {
                                        foreach (string key in formCollection.AllKeys)
                                        {
                                            if (key == d[0].ToString())
                                            {
                                                marksModel.Add(d[1].ToString(), formCollection[key].ToString());
                                            }
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

                                        }
                                    }
                                    if (chkselect == 0)
                                    {
                                        marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                    }
                                }
                                else
                                    marksModel.Add(d[1].ToString(), d[0].ToString());

                            }

                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 111, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 111, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                            cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEducationVerInfoBySpoc", CheckStatus, "", "Update");
                            #endregion

                            #region Case/Check Action History Save 
                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion

                        }
                        cc.UpdateReportComments(PersonalRowid, SubCheckid, Remarks);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;

            return RedirectToAction("UpdateEducationVerInfoBySpoc", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UpdateReferenceVerInfoBySpoc(int id, int CheckStatusId = 0, bool Aging = false, int PersonalRowID = 0, short subCheckId = 0, short ClientRowId = 0, string msg = "")
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            ClientCustomize cc = new ClientCustomize();
            DataTable dt = new DataTable();
            ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);
            if (msg != "")
            {
                ViewBag.ErrorMsg = msg;
            }
            short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
            int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
            int dptid = GetDepartmentID(tmid);
            ViewBag.SpecialInstruction = GetSpecialInstructionsRWQC(ClientRowId, ChFamilRowId, dptid);

            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Report", "Check");
            DataTable dbValue = new DataTable();

            dbValue = cc.GetReportValue(PersonalRowID, subCheckId);

            DataTable dts = cc.GetDispositionPQCandidateChecks(PersonalRowID, subCheckId);
            DataTable DtActionRemark = new DataTable();
            if (CheckStatusId == 103)
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Client Review");
            }
            else
            {
                DtActionRemark = cc.GetCheckActionRemarks(PersonalRowID, subCheckId, "Spoc Review");
            }
            int SeverityRowID = 0;
            string ColorCode = "";
            string SeverityGrid = "";
            string ReportComment = "";
            if (dts.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityRowID"].ToString()))
                {
                    SeverityRowID = Convert.ToInt32(dts.Rows[0]["SeverityRowID"].ToString());
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["ColorName"].ToString()))
                {
                    ColorCode = dts.Rows[0]["ColorName"].ToString();
                }

                if (!string.IsNullOrEmpty(dts.Rows[0]["SeverityGrid"].ToString()))
                {
                    SeverityGrid = dts.Rows[0]["SeverityGrid"].ToString();
                }

                if (DtActionRemark.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DtActionRemark.Rows[0]["Remarks"].ToString()))
                    {
                        if (CheckStatusId == 103)
                        {
                            foreach (DataRow row in DtActionRemark.Rows)
                            {
                                ReportComment += Convert.ToString(row["Remarks"]) + "<br/>";
                            }
                        }
                        else
                        {
                            ReportComment = Convert.ToString(DtActionRemark.Rows[0]["Remarks"]);
                        }
                    }
                }


                ViewBag.SeverityRowID = SeverityRowID;
                ViewBag.ColorCode = ColorCode;
                ViewBag.SeverityGrid = SeverityGrid;
                ViewBag.ReportComment = ReportComment.ToString();


            }
            foreach (DataRow d in dt.Rows)
            {
                DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                if (rowsFiltered.Length > 0)
                {
                    if (d[4].ToString() == "textbox")
                    {
                        marksModel.Add(d[1].ToString(), rowsFiltered[0][1]);
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "checkbox")
                    {
                        marksModel.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                    }
                    else if (d[4].ToString() == "fileUpload")
                        marksModel.Add(d[1].ToString(), d[0].ToString().ToString());

                }
                else
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
            }
            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            ViewBag.CheckStatusid = CheckStatusId.ToString();
            ViewBag.ReportComment = ReportComment.ToString();
            BindDocumentList(PersonalRowID, subCheckId);
            GetCaseDocumentList(PersonalRowID);
            BindMailList(PersonalRowID, subCheckId);

            return View(marksModel);

        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateReferenceVerInfoBySpoc(FormCollection formCollection, string action = "")
        {

            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());

            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());

            short clientRowid = Convert.ToInt16(formCollection["clientRowId"].ToString());
            int CheckStatusId = Convert.ToInt32(formCollection["CheckStatusid"].ToString());

            string CheckStatus = action;
            string ColorName = formCollection["ColorsName"].ToString();
            string Remarks = formCollection["Remarks"].ToString();
            string APIRemarks = "";
            if (Remarks != "")
            {
                APIRemarks = Regex.Replace(Remarks, "<.*?>", string.Empty);
            }
            string PriorityType = formCollection["PriorityType"].ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    ClientCustomize cc = new ClientCustomize();
                    if (clientRowid > 0 && SubCheckid > 0)
                    {
                        int checkstatusid = 0;
                        DataTable Dtstatusid = new DataTable();
                        Dtstatusid = cc.getCheckStatus(PersonalRowid, SubCheckid);
                        if (Dtstatusid.Rows.Count > 0)
                        {
                            checkstatusid = Convert.ToInt32(Dtstatusid.Rows[0]["CheckStatusID"]);
                        }
                        #region LTI Client Review
                        if (CheckStatus == "Approved" || CheckStatus == "Submit")
                        {
                            if (clientRowid == 990)
                            {
                                if (CheckStatusId == 103)
                                {
                                    string DocumentType = "Client Review By API";
                                    string ReportUploadedName = "";
                                    DataTable ddt = new DataTable();
                                    ddt = cc.getClientReviewdocumet(PersonalRowid, SubCheckid, DocumentType);
                                    if (ddt.Rows.Count > 0)
                                    {
                                        ReportUploadedName = Convert.ToString(ddt.Rows[0]["FileName"]);
                                    }
                                    string Res = API.LTIClientReviewStatus(PersonalRowid, clientRowid, SubCheckid, ReportUploadedName, APIRemarks);
                                    if (Res == "OK")
                                    {

                                        cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 118, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Approved");
                                        cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 118, Remarks, "Client Review Approved", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                                        #region Save LogTransaction Details  Case/Check Action History Save
                                        clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateEmploymentVerInfoBySpoc", CheckStatus, "Client Review Approved", "Updated");

                                        clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                                        #endregion

                                        ModelState.Clear();
                                        return RedirectToAction("ManageSpocReviewDashboardInfo", "ClientServicing", new { PersonalRowID = PersonalRowid, dispFor = "Dashboard" });
                                    }
                                }
                            }

                        }
                        #endregion
                        if (CheckStatus == "Reject")
                        {
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 21, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()), "Report Reject");                            //repoEmploymentVer.UpdateEmploymentByTMQC(model);
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 21, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details  Case/Check Action History Save
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateReferenceVerInfoBySpoc", CheckStatus, "Spoc Team Member", "Updated");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion

                            ModelState.Clear();
                        }
                        else
                        {
                            var marksModel = expando as IDictionary<string, object>;

                            OperationViewModel cd = new OperationViewModel();
                            cd.PersonalRowID = PersonalRowid;
                            cd.SubCheckRowID = SubCheckid;
                            cd.clientRowId = clientRowid;
                            cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                            cd.moduleName = "ReportQC";


                            ClientCustomize bl = new ClientCustomize();
                            DataTable dt = new DataTable();
                            dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Report", "Check");
                            foreach (DataRow d in dt.Rows)
                            {
                                string[] FString = d[1].ToString().Split('_');

                                if (d[4].ToString() == "fileUpload")
                                {
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
                                        }
                                    }
                                }
                                else if (d[4].ToString() == "dropdown")
                                {

                                    foreach (string key in formCollection.AllKeys)
                                    {
                                        if (key == d[0].ToString())
                                        {
                                            marksModel.Add(d[1].ToString(), formCollection[key].ToString());
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
                                        }
                                    }
                                    if (chkselect == 0)
                                    {
                                        marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                                    }
                                }
                                else
                                    marksModel.Add(d[1].ToString(), d[0].ToString());

                            }
                            cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 111, "Check", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.Process_Remarks_Insert(PersonalRowid, SubCheckid, 111, Remarks, "Spoc Review", Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            cc.CheckMovementProcessCompletionDate(Convert.ToInt32(PersonalRowid), Convert.ToInt32(SubCheckid), "SpocReviewCleared");
                            cc.InsertSpocActionRemark(PersonalRowid, SubCheckid, PriorityType, Remarks, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));
                            #region Save LogTransaction Details     Case/Check Action History Save 
                            clsCommonMethods.AddPQLogTrasaction(PersonalRowid, "UpdateReferenceVerInfoBySpoc", CheckStatus, "", "Update");

                            clsCommonMethods.AddCheckActionHistory(PersonalRowid, SubCheckid, CheckStatus, Remarks);
                            #endregion
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Invlid Employment id and model!";
                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = PersonalRowid;
            ViewBag.SubCheckId = SubCheckid;
            ViewBag.clientRowId = clientRowid;
            return RedirectToAction("UpdateReferenceVerInfoBySpoc", new { PersonalRowID = ViewBag.PersonalRowid, subCheckId = ViewBag.SubCheckId, ClientRowId = ViewBag.clientRowId });

        }
        void GetSubchecksForSpocRemark(int PersonalRowID)
        {
            try
            {

                ViewBag.chek = repoPQPersonal.GetSubchecksForSpocRemark(PersonalRowID);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddSpocActionRemark()
        {
            ViewBag.CompanyRefNo = "";

            return View();
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddSpocActionRemark(FormCollection frm, string action = "")
        {
            try
            {
                string CompanyRefNo = Convert.ToString(frm["CompanyRefNo"]);
                string SubCheckRowId = Convert.ToString(frm["SubCheckRowId"]);
                string PriorityType = Convert.ToString(frm["PriorityType"]);
                string SpocRemark = Convert.ToString(frm["SpocRemark"]);
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetPersonalRowId(CompanyRefNo);
                if (action != "Search")
                {
                    if (Convert.ToString(SpocRemark) != "")
                    {
                        if (CompanyRefNo != "")
                        {
                            if (SubCheckRowId == "")
                            {
                                SubCheckRowId = "0";
                            }
                            cc.InsertSpocActionRemark(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(SubCheckRowId), PriorityType, SpocRemark, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));

                            TempData["SuccessMsg"] = "Records Save Successfully !.";
                        }
                        else
                        {
                            GetSubchecksForSpocRemark(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                            ViewBag.CompanyRefNo = CompanyRefNo;
                            TempData["SuccessMsg"] = "Please fill CompanyRefNo";
                        }

                    }
                    else
                    {
                        GetSubchecksForSpocRemark(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                        ViewBag.CompanyRefNo = CompanyRefNo;
                        TempData["SuccessMsg"] = "Please fill the Remarks";
                    }

                }
                else
                {
                    GetSubchecksForSpocRemark(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                    ViewBag.CompanyRefNo = CompanyRefNo;
                }


            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddSpocActionRemarkNew(string CompanyRefNo = "", string msg = "")
        {
            BindSubchecknameDelete(0, 0);
            GetMasterStopcheckStatusId();
            ViewBag.CompanyRefNo = CompanyRefNo;

            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddSpocActionRemarkNew(FormCollection frm, HttpPostedFileBase UploadDoc, string action = "")
        {
            try
            {
                string CompanyRefNo = Convert.ToString(frm["CompanyRefNo"]);
                string ActionType = Convert.ToString(frm["ActionType"]);
                string SubCheckRowId = Convert.ToString(frm["SubCheckRowId"]);
                string PriorityType = Convert.ToString(frm["PriorityType"]);
                string SpocRemark = Convert.ToString(frm["SpocRemark"]);

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetPersonalRowId(CompanyRefNo);
                DataTable dt1 = cc.getcasestatusid(CompanyRefNo);
                var caseStatusId = dt1.Rows[0][0].ToString();
                if (caseStatusId != "30")
                {
                    if (action != "Search")
                    {
                        if (Convert.ToString(SpocRemark) != "")
                        {
                            if (CompanyRefNo != "")
                            {
                                if (SubCheckRowId == "")
                                {
                                    SubCheckRowId = "0";
                                }
                                if (UploadDoc != null)
                                {
                                    string errorMessage = GatZipFileMessage(UploadDoc, Convert.ToInt32(dt.Rows[0]["PersonalRowID"]).ToString(), SubCheckRowId.ToString());
                                    if (errorMessage != "")
                                    {
                                        return RedirectToAction("AddSpocActionRemark", new { CompanyRefNo = CompanyRefNo, Msg = errorMessage });
                                    }
                                }
                                cc.InsertSpocActionRemark(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(SubCheckRowId), PriorityType, SpocRemark, Convert.ToInt32(Session["TeamMemberRowID"].ToString()));


                                if (UploadDoc != null)
                                {
                                    SavePQVerifiedUploadDoc(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt16(SubCheckRowId), "Provided", "NA", "provideddocuments", UploadDoc, SpocRemark, 0);

                                }
                                // SavePQVerifiedUploadDoc(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt16(SubCheckRowId), "Provided", "NA", "provideddocuments", UploadDoc, SpocRemark, 0);
                                if (ActionType == "Reopen")
                                {
                                    int spocId = Convert.ToInt16(Session["TeamMemberRowID"]);
                                    cc.UpdateCheckStatus(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(SubCheckRowId), 1, 20, "Spoc", spocId);
                                    cc.Process_Remarks_Insert(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]), Convert.ToInt32(SubCheckRowId), 20, SpocRemark, "Spoc", spocId);
                                }
                                BindSubchecknameDelete(0, 0);
                                GetMasterStopcheckStatusId();
                                ViewBag.CompanyRefNo = CompanyRefNo;
                                TempData["SuccessMsg"] = "Records Save Successfully !.";

                            }
                            else
                            {
                                GetSubchecksForSpocRemarkNew(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                                ViewBag.CompanyRefNo = CompanyRefNo;
                                TempData["SuccessMsg"] = "Please fill CompanyRefNo";
                            }

                        }
                        else
                        {
                            GetSubchecksForSpocRemarkNew(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                            ViewBag.CompanyRefNo = CompanyRefNo;
                            TempData["SuccessMsg"] = "Please fill the Remarks";
                        }

                    }
                    else
                    {
                        GetSubchecksForSpocRemarkNew(Convert.ToInt32(dt.Rows[0]["PersonalRowID"]));
                        ViewBag.CompanyRefNo = CompanyRefNo;
                    }

                }
                else
                {
                    BindSubchecknameDelete(0, 0);
                    GetMasterStopcheckStatusId();
                    ViewBag.CompanyRefNo = CompanyRefNo;
                }


            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        void GetSubchecksForSpocRemarkNew(int PersonalRowID)
        {
            try
            {

                ViewBag.chek = repoPQPersonal.GetSubchecksForSpocRemark(PersonalRowID);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        [SessionExpireFilterAttribute]
        [HttpGet]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UplodVerificationDocs(short scid = 0, int pe = 0, string cstatus = "")
        {
            VerifiedUploadDocViewModel model = new VerifiedUploadDocViewModel();
            try
            {
                if (scid > 0 && pe > 0)
                {
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Verification id!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [SessionExpireFilterAttribute]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult UplodVerificationDocs(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    ClientCustomize CC = new ClientCustomize();
                    int inputSize = CC.GetControlInputs("ArtifactPageSize");
                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        int pageCount = GetNoOfPageInPDFFile(model.UploadDoc);
                        if (pageCount <= inputSize)
                        {
                            if (model.DocumentType == null)
                            {
                                model.DocumentType = "Document Provided By Spoc";
                            }
                            model.DocumentUploadFrom = "Provided";
                            model.StatusDocxConvert = 0;
                            model.StatusPDF2DOCXConvert = 0;
                            model.UploadDate = DateTime.Now;
                            model.Status = 1;
                            string errorMessage = GatZipFileMessage(model.UploadDoc, model.PersonalRowID.ToString(), "UplodVerificationDocs");
                            if (errorMessage != "")
                            {
                                ViewBag.ErrorMsg += errorMessage;
                            }
                            else
                            {
                                string ext = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                                proImg = ("Document_Provided_By_Spoc" + "_" + model.PersonalRowID + "_" + model.SubCheckRowID + "_" + model.UploadDate.Value.ToString("ddMMyyyyHHmmss")).Replace("'", "_").Replace(" ", "_").Replace("-", "_") + ext;
                                model.UploadDoc.SaveAs(Server.MapPath(FldPathDoc + proImg));
                                model.FileName = proImg;
                                model.FilePath = FldPathDoc;
                                repoPQVerifiedUploadDoc.VerifiedUploadDoc(model);
                                int Res = repoPQVerifiedUploadDoc.SaveChanges();
                                if (Res > 0)
                                {
                                    #region Check Action History Save                           
                                    clsCommonMethods.AddCheckActionHistory(model.PersonalRowID, model.SubCheckRowID, model.CheckStatus, "Spoc Verification information Upload Doc ");
                                    #endregion
                                    ModelState.Clear();
                                    TempData["SuccessMsg"] = "Record Added Successfully!";
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = "There is some problem to save record. Please try again later!";
                                }
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Page Size Greater Then Authorized Page Size!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "please select upload doc!";
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
        [NonAction]
        public int GetNoOfPageInPDFFile(HttpPostedFileBase file)
        {
            int numberOfPages = 1;
            return numberOfPages;
        }

        public void ExportPendingcandidateSpoc()
        {
            DataTable dt = new DataTable();

            int teamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
            string CaseStatus = (Session["CaseStatus"]).ToString();
            int clientid = Convert.ToInt32(Session["clientid"]);
            string Designation = Session["Designation"].ToString();

            ClientCustomize cc = new ClientCustomize();
            if (Designation == "Insuff-YTR-FundsPending")
            {
                dt = cc.getPendingCase_Dashboard_DetailsForFundExportToExcel(clientid, CaseStatus, teamMemberRowID);
            }
            else
            {
                dt = cc.getClientServicing_Dashboard_Details(clientid, CaseStatus, teamMemberRowID);
            }
            ExportToExcel(dt, "PendingWithCandiate_Excel");
        }


        public void ExportPendingcandidateSpocSeniorprofile()
        {
            DataTable dt = new DataTable();

            int teamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
            string CaseStatus = (Session["CaseStatus"]).ToString();
            int clientid = Convert.ToInt32(Session["clientid"]);

            ClientCustomize cc = new ClientCustomize();
            dt = cc.getClientServicing_Dashboard_DetailsNew1(clientid, CaseStatus, teamMemberRowID);
            ExportToExcel(dt, "PendingWithCandiate_Excel_seniorProfileCase");
        }

        public void MIS_Excel()
        {


            int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);

            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();


                ds = cc.allPendingcheck(teamMemerRowId);
                if (ds.Tables.Count > 0)
                {
                    var dt = new DataTable("Details");
                    dt = ds.Tables[0];
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
                        Response.AddHeader("content-disposition", "attachment;filename=AllPendingcheck.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        public void MIS_Excell(DateTime FromDate, DateTime ToDate)
        {


            int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);

            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();


                ds = cc.USP_AllPending_CompletedcheckByDate(teamMemerRowId, FromDate, ToDate);

                if (ds.Tables.Count > 0)
                {
                    var dt = new DataTable("Details");
                    dt = ds.Tables[0];
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
                        Response.AddHeader("content-disposition", "attachment;filename=AllPendingcheck.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }



        ////////////////////////////////////


        public void RGBSIPLCheckwiseTracker()
        {
            DataTable dt;
            ClientCustomize cc = new ClientCustomize();

            dt = new DataTable();
            dt = cc.RGBSIPLCheckwiseTracker();
            ExportToExcel(dt, "RGBSIPL_Checkwise_Tracker");
        }

        [HttpGet]
        public ActionResult Completed_check_MIS()
        {
            return View();
        }
        [HttpPost]
        public void Completed_check_MIS(string FromDate, string ToDate)
        {


            int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);

            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataSet ds = new DataSet();
                DateTime from = Convert.ToDateTime(FromDate);
                DateTime To = Convert.ToDateTime(ToDate);

                ds = cc.USP_AllPending_CompletedcheckByDate(teamMemerRowId, from, To);

                if (ds.Tables.Count > 0)
                {
                    var dt = new DataTable("Details");
                    dt = ds.Tables[0];
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
                        Response.AddHeader("content-disposition", "attachment;filename=Completedcheck.xlsx");

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [HttpGet]
        public ActionResult HoldForScan()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult HoldForScan(string CompanyRefNo = "")
        {
            string final = "',";
            final += CompanyRefNo.Replace(" ", ",");
            final += ",'";

            try
            {
                ClientCustomize cc = new ClientCustomize();
                cc.HoldForScan(final);
                return RedirectToAction("HoldForScan", "ClientServicing");
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                return View("Error");
            }
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public void InterimReportBulkDownload()
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.interimReportBulkDownload();

                if (dt != null)
                {
                    using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(dt, "Details");
                        wb.Style.Font.Bold = true;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=InterimReportBulkDownload.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public void FinalReportBulkDownload()
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.FinalReportBulkDownload();

                if (dt != null)
                {
                    using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(dt, "Details");
                        wb.Style.Font.Bold = true;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=FinalReportBulkDownload.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        [HttpGet]
        public ActionResult EmployeeCodeBulkUpdate(string ErrMessage = "")
        {
            BindClientList();
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EmployeeCodeBulkUpdate(HttpPostedFileBase file, FormCollection formCollection)
        {
            try
            {
                if (file != null)
                {
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {
                        ViewBag.ErrorMsg = "Please upload a valid Excel file (.xls or .xlsx).";
                        return View();
                    }
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Emailers"), fileName);
                    file.SaveAs(filePath);

                    int result = UploadEmployeeCodeBulkUpdateExcel(filePath);
                    if (result <= 0)
                    {
                        ViewBag.ErrorMsg = "Error updating data.";
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        return View();
                    }

                    ViewBag.ErrorMsg = "successfully.";
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Please select an Excel file to upload.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Error: " + ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), ex.Message, ex.StackTrace);
            }

            return View();
        }

        public int UploadEmployeeCodeBulkUpdateExcel(string filePath)
        {
            try
            {

                string fileExtension = Path.GetExtension(filePath).ToLower();
                string conStr = "";
                switch (fileExtension)
                {
                    case ".xls":
                        conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx":
                        conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                    default:
                        throw new InvalidOperationException("Invalid file format. Only .xls and .xlsx are allowed.");
                }

                conStr = string.Format(conStr, filePath, "YES");

                using (OleDbConnection connExcel = new OleDbConnection(conStr))
                {
                    connExcel.Open();
                    DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    connExcel.Close();

                    OleDbCommand cmdExcel = new OleDbCommand($"SELECT * FROM [{sheetName}]", connExcel);
                    OleDbDataAdapter oda = new OleDbDataAdapter(cmdExcel);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    connExcel.Close();

                    int updatedCount = 0;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string clientname = row["CompanyRefNo"].ToString().Trim();
                            string EmployerName = row["EmployeeID"].ToString().Trim();



                            ClientCustomize cc = new ClientCustomize();
                            cc.EmployeeCodeBulkUpdate(clientname, EmployerName);
                            updatedCount++;
                        }
                    }

                    return updatedCount;
                }
            }
            catch (Exception ex)
            {
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), ex.Message, ex.StackTrace);
                return 0;
            }
        }


        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public async Task<ActionResult> DownloadEmployeeCodeBulkUpdateExcel()

        {

            var filePath = Path.Combine(Server.MapPath("~/Uploads/"), "Empcodeupdate.xlsx");


            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);


            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Empcodeupdate.xlsx");


        }

        #region Sent status excle
        [ValidateInput(false)]
        public ActionResult LTIExcelStatus()
        {
            ViewBag.Title = "Home Page";
            //if (Session["UserName"] == null)
            //{
            //    return RedirectToAction("Login");
            //}
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LTIExcelStatus(FormCollection formCollection, HttpPostedFileBase DigitalVer)
        {
            StringBuilder StrBuilderObj = new StringBuilder();
            StrBuilderObj.Append("-------------------------------Statrded  here-----------------------------------");
            StrBuilderObj.Append(Environment.NewLine);
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
            string path = ConfigurationManager.AppSettings["LTI_DocumentPATH"].ToString();
            StrBuilderObj.Append("-------------------------------DocumentPATH here-----------------------------------");
            StrBuilderObj.Append(Environment.NewLine);
            string ext = Path.GetExtension(DigitalVer.FileName).ToLower();
            string filename = "LITCheckdetails_" + Timestamp + ext;
            string filePath = path + filename;
            //filePath = Server.MapPath(path + filename);
            DigitalVer.SaveAs(filePath);
            ViewBag.Error = "";
            ViewBag.Error2 = "";
            int clientRowId = 990;
            string base64 = "";
            try
            {
                StrBuilderObj.Append("-------------------------------Payload data Function Name :LTIExcelStatus-----------------------------------");
                StrBuilderObj.Append(Environment.NewLine);
                string conStr = "";
                switch (ext)
                {
                    case ".xls": //Excel 97-03
                        conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07
                        conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }
                StrBuilderObj.Append("-------------------------------Excel07ConString  here-----------------------------------");
                StrBuilderObj.Append(Environment.NewLine);
                conStr = String.Format(conStr, filePath, "Yes");
                StrBuilderObj.Append("-------------------------------conStr  here-----------------------------------" + conStr);
                StrBuilderObj.Append(Environment.NewLine);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                StrBuilderObj.Append("-------------------------------connExcel  here-----------------------------------" + connExcel);
                StrBuilderObj.Append(Environment.NewLine);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                StrBuilderObj.Append("-------------------------------oda  here-----------------------------------" + oda);
                StrBuilderObj.Append(Environment.NewLine);
                DataTable dt = new DataTable();
                StrBuilderObj.Append("-------------------------------dt  here-----------------------------------" + dt);
                StrBuilderObj.Append(Environment.NewLine);
                cmdExcel.Connection = connExcel;
                StrBuilderObj.Append("-------------------------------connExcel  here-----------------------------------" + connExcel);
                StrBuilderObj.Append(Environment.NewLine);

                //Get the name of First Sheet
                connExcel.Open();
                StrBuilderObj.Append("-------------------------------connExcel.Open  here-----------------------------------" + dt);
                StrBuilderObj.Append(Environment.NewLine);
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                StrBuilderObj.Append("-------------------------------SheetName  here-----------------------------------" + SheetName);
                StrBuilderObj.Append(Environment.NewLine);
                connExcel.Close();
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                StrBuilderObj.Append("-------------------------------SELECT * From  here-----------------------------------");
                StrBuilderObj.Append(Environment.NewLine);

                oda.Fill(dt);
                connExcel.Close();
                if (dt.Rows.Count > 0)
                {
                    StrBuilderObj.Append("-------------------------------Load dt here-----------------------------------");
                    StrBuilderObj.Append(Environment.NewLine);
                    string StatusType = "1";
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        string CompanyRefNo = CompanyRefNo = dtRow["BGV_Vendor_Order_ID"].ToString();
                        string Check_Name = dtRow["Check_Name"].ToString();
                        DataTable dts = new AccountRepository().GetCheckmappingdetails(Check_Name);
                        if (dts.Rows.Count > 0)
                        {
                            string subcheckrowid = dts.Rows[0]["SubCheckRowID"].ToString();
                            if (CompanyRefNo.Contains("LTI") && CompanyRefNo != "" && subcheckrowid != "" && StatusType != "")
                            {
                                if (subcheckrowid != "" && StatusType == "1")
                                {
                                    string result = API.PostClearstatus(CompanyRefNo, Convert.ToInt32(subcheckrowid), StatusType, 0);
                                    if (result.ToUpper() == "OK")
                                    {
                                        ViewBag.Error = "Status Successfully Sent";
                                    }
                                    else
                                    {
                                        ViewBag.Error = "Status Unsuccessfully Sent";
                                    }
                                }

                                else
                                {
                                    ViewBag.Error = "Please fill mandatory fields";
                                }
                            }
                            else
                            {
                                ViewBag.Error = "Please fill mandatory fields";
                            }
                        }
                        Thread.Sleep(1000);

                    }


                }
                else
                {
                    ViewBag.Error = "Please fill mandatory fields";
                }
            }
            catch (Exception ex)
            {
                StrBuilderObj.Append("Exception:-" + Convert.ToString(ex.Message));
                StrBuilderObj.Append(Environment.NewLine);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "LTIExcel", "LTIExceldata", "923874823" + "-" + "Exceldata");
                ViewBag.Error = "Please check excel data.";
            }
            return View();
        }

        #endregion



        [HttpGet]
        public ActionResult GetCandidateCredential()
        {
            CandidateUserLoggedViewModel LoginModel = new CandidateUserLoggedViewModel();
            ViewBag.LogInCredential = "";
            return View(LoginModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCandidateCredential(FormCollection Frm)
        {
            CandidateUserLoggedViewModel LoginModel = new CandidateUserLoggedViewModel();
            try
            {
                string EmailId = Convert.ToString(Frm["EmailId"]);
                if (EmailId != "")
                {
                    ClientCustomize CC = new ClientCustomize();
                    var CredentialDT = CC.getCandidateCredentialByEmailId(EmailId);
                    if (CredentialDT.Rows.Count > 0)
                    {
                        LoginModel.CandidateFullName = Convert.ToString(CredentialDT.Rows[0]["CandidateName"]);
                        LoginModel.UserID = Convert.ToString(CredentialDT.Rows[0]["UserID"]);
                        LoginModel.CandidateEmail = EmailId;
                        LoginModel.UPass = clsCommonMethods.PasswordDecrypt(Convert.ToString(CredentialDT.Rows[0]["UPass"]));

                        ViewBag.LogInCredential = "True";
                    }
                    else
                    {
                        ViewBag.LogInCredential = "";
                        ViewBag.ErrorMsg = "Records Not Found";
                    }

                }
                else
                {
                    ViewBag.LogInCredential = "";
                    ViewBag.ErrorMsg = "Please Enter Email Id";
                }
            }
            catch (Exception ex)
            {
                ViewBag.LogInCredential = "";
                ViewBag.ErrorMsg = "Error Found";

            }
            return View(LoginModel);
        }


        #region User Active Details
        [ValidateInput(false)]
        public ActionResult UserActiveDetails()
        {
            int teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
            ViewBag.Title = "Home Page";
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UserActiveDetails(FormCollection frm)
        {
            try
            {

                int teamMemerId = Convert.ToInt32(Session["TeamMemberRowID"]);
                string UserID = Convert.ToString(frm["UserId"]);
                string Remarks = Convert.ToString(frm["Remarks"]);
                int SStatus = 0;
                string ddlActionType = Convert.ToString(frm["ddlActionType"]);
                if (ddlActionType == "-- Select request type -- " || ddlActionType == "")
                {

                }
                else
                {
                    SStatus = Convert.ToInt32(frm["ddlActionType"]);
                }
                if (UserID != "" && Remarks != "" && SStatus != 0 && UserID != null && Remarks != null)
                {
                    ClientCustomize cc = new ClientCustomize();
                    cc.SP_UserActiveDetails(teamMemerId, SStatus, UserID.Trim(), Remarks);
                    ViewBag.Succe = "user activation successful..";
                }
                else
                {
                    ViewBag.Error = "Please fill mandatory fields";
                }

            }

            catch (Exception ex)
            {
            }
            return View();
        }

        #endregion

        [ValidateInput(false)]
        public ActionResult GetDownloadPdf(int pid)
        {
            ReportViewModel model = new ReportViewModel();
            ClientCustomize cc = new ClientCustomize();

            model = repoReportQC.GetReportPersonalInfo(pid);

            if (model.ClientRowID == 268)
            {
                string[] A = model.EmploymentId.Split('/');
                if (A.Count() > 2)
                {
                    model.EmploymentType = A[1].ToString();
                    model.Location = A[2].ToString();
                    model.EmploymentId = A[0].ToString();
                }
            }

            if (model.ClientRowID == 3)
            {
                string[] A = model.EmploymentId.Split('/');
                if (A.Count() > 2)
                {
                    model.EmploymentType = A[2].ToString();
                    model.Location = A[3].ToString();
                    model.EmploymentId = A[0].ToString();
                }
            }

            DataTable dtcol = new DataTable();
            dtcol = cc.GetColorCode(model.PersonalRowID, model.ClientRowID);
            if (dtcol.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dtcol.Rows[0]["colorName"].ToString()))
                {
                    model.FinalColor = dtcol.Rows[0]["colorName"].ToString();
                    model.FinalColorCode = dtcol.Rows[0]["colorCode"].ToString();
                }
                else
                {
                    model.FinalColorCode = "#fff";
                    model.FinalColor = "";
                }

            }

            #region  Personal Info 

            if (model.ClientRowID == 268 || model.ClientRowID == 3)
            {
                model.PersonalChecksBriefDetails = repoReportQC.GetCandidateChecksBriefDetailsByPersonalIdTCS(pid);
            }

            if (model.ClientRowID == 268)
            {
                DataTable dts = cc.GetReportUploadDate(pid);
                if (dts.Rows.Count > 0)
                {
                    model.PendingCheck = Convert.ToInt32(dts.Rows[0]["pending"].ToString());
                    string FinalReportDate = dts.Rows[0]["FinalUpload"].ToString();
                    string InterimReportDate = dts.Rows[0]["InterimUpload"].ToString();


                    if (FinalReportDate == null || FinalReportDate == "")
                    {
                        model.TCSReportDate = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MM-yyyy"));
                    }
                    else
                    {

                        model.TCSReportDate = Convert.ToDateTime(dts.Rows[0]["FinalUpload"]);
                    }

                    if (InterimReportDate == null || InterimReportDate == "")
                    {
                        model.TCSIntrimReportDate = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MM-yyyy"));
                    }
                    else
                    {
                        model.TCSIntrimReportDate = Convert.ToDateTime(dts.Rows[0]["InterimUpload"]); ;
                    }

                }
            }
            #endregion

            #region Address List

            DataTable dbAddressValue = new DataTable();
            //dbAddressValue = cc.GetReportAllValues(PersonalRowID, 2);

            dbAddressValue = cc.GetReportAllValues(pid, 2);
            if (dbAddressValue.Rows.Count > 0)
            {
                foreach (DataRow dr in dbAddressValue.Rows)
                {
                    DataTable dtAddressAnti = new DataTable();
                    dtAddressAnti = cc.Antecedents_client_select_form(model.ClientRowID, Convert.ToInt32(dr["SubcheckRowID"].ToString()), "Report", "Check");


                    DataRow[] rowsFiltered = dtAddressAnti.Select("AntecedentsRowID=" + dr["AntecedentRowID"].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        model.AddressInfoModel.Add(new PersonalInfo
                        {
                            AntecedentRowID = dr["AntecedentRowID"].ToString(),
                            //DisplayNameDE = dr["DisplayNameOps"].ToString(),
                            DisplayNameRep = dr["DisplayNameRep"].ToString(),
                            DataEntryValue = dr["DataEntryValue"].ToString(),
                            OpsValue = dr["OpsValue"].ToString(),
                            //PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            SubcheckRowID = Convert.ToInt32(dr["SubcheckRowID"].ToString()),
                            SubCheckName = dr["SubCheckName"].ToString(),
                            comment = dr["RepValue"].ToString(),
                            CheckType = dr["CheckType"].ToString(),
                            CreationDate = Convert.ToDateTime(dr["CreationDate"].ToString()),
                            Status = Convert.ToInt32(dr["Status"].ToString()),

                        });

                        DataTable DtColor = cc.GetDispositionPQCandidateChecks(pid, Convert.ToInt32(dr["SubcheckRowID"].ToString()));
                        if (DtColor.Rows.Count > 0)
                        {
                            if (DtColor.Rows[0]["Status"].ToString() != "1")
                            {
                                int subcheckid = Convert.ToInt32(DtColor.Rows[0]["SubCheckFamilyID"].ToString());
                                //if (model.ClientRowID == 532)
                                //{
                                if (DtColor.Rows[0]["Status"].ToString() == "4")
                                {
                                    model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "Not Applicable";
                                }
                                //}
                                if (DtColor.Rows[0]["Status"].ToString() == "0")
                                {
                                    model.AddressInfoModel.RemoveAll(T => T.SubcheckRowID == subcheckid);
                                }
                                //else
                                //{
                                //    model.EmploymentInfoModel.RemoveAll(T => T.SubcheckRowID == subcheckid);
                                //}


                            }
                            else
                            {
                                model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckStatusId = DtColor.Rows[0]["CheckStatusid"].ToString();

                                if (DtColor.Rows[0]["checkstatusid"].ToString() == "37")
                                {
                                    model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorCode = DtColor.Rows[0]["ColorCode"].ToString();
                                    model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = DtColor.Rows[0]["ColorName"].ToString();
                                }
                                else if (DtColor.Rows[0]["CheckStatus"].ToString().Contains("YTR"))
                                {
                                    model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "YTR on Hold";
                                }
                                else if (DtColor.Rows[0]["CheckStatus"].ToString() == "Not Applicable")
                                {
                                    model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "Not Applicable";
                                }
                                else if (DtColor.Rows[0]["CheckClientStatus"].ToString().ToLower().Contains("wip"))
                                {
                                    model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "WIP";
                                }

                                else if (DtColor.Rows[0]["CheckClientStatus"].ToString().ToLower().Contains("insuff"))
                                {
                                    model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "Insuff";
                                }


                                model.AddressInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().ReportComment = DtColor.Rows[0]["ReportComments"].ToString();
                                // ReportComment
                            }

                        }
                    }
                }
            }

            #region Criminal List 

            DataTable dbCriminalValue = new DataTable();

            dbCriminalValue = cc.GetReportAllValues(pid, 5);
            if (dbCriminalValue.Rows.Count > 0)
            {
                foreach (DataRow dr in dbCriminalValue.Rows)
                {
                    DataTable dtCriminalAnti = new DataTable();
                    dtCriminalAnti = cc.Antecedents_client_select_form(model.ClientRowID, Convert.ToInt32(dr["SubcheckRowID"].ToString()), "Report", "Check");


                    DataRow[] rowsFiltered = dtCriminalAnti.Select("AntecedentsRowID=" + dr["AntecedentRowID"].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        model.CriminalInfoModel.Add(new PersonalInfo
                        {
                            AntecedentRowID = dr["AntecedentRowID"].ToString(),
                            //DisplayNameDE = dr["DisplayNameOps"].ToString(),
                            DisplayNameRep = dr["DisplayNameRep"].ToString(),
                            DataEntryValue = dr["DataEntryValue"].ToString(),
                            OpsValue = dr["OpsValue"].ToString(),
                            //PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            SubcheckRowID = Convert.ToInt32(dr["SubcheckRowID"].ToString()),
                            SubCheckName = dr["SubCheckName"].ToString(),
                            comment = dr["RepValue"].ToString(),
                            CheckType = dr["CheckType"].ToString(),
                            CreationDate = Convert.ToDateTime(dr["CreationDate"].ToString()),
                            Status = Convert.ToInt32(dr["Status"].ToString()),
                            CheckFamilyRowid = Convert.ToInt32(dr["CheckFamilyRowID"].ToString()),

                        });
                        DataTable DtColor = cc.GetDispositionPQCandidateChecks(pid, Convert.ToInt32(dr["SubcheckRowID"].ToString()));
                        if (DtColor.Rows.Count > 0)
                        {
                            if (DtColor.Rows[0]["Status"].ToString() != "1")
                            {
                                int subcheckid = Convert.ToInt32(DtColor.Rows[0]["SubCheckFamilyID"].ToString());
                                //if(model.ClientRowID==532)
                                //{
                                if (DtColor.Rows[0]["Status"].ToString() == "4")
                                {
                                    model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "Not Applicable";
                                }
                                //}
                                if (DtColor.Rows[0]["Status"].ToString() == "0")
                                {
                                    model.CriminalInfoModel.RemoveAll(T => T.SubcheckRowID == subcheckid);
                                }


                            }
                            else
                            {
                                model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckStatusId = DtColor.Rows[0]["CheckStatusid"].ToString();

                                if (DtColor.Rows[0]["checkstatusid"].ToString() == "37")
                                {
                                    model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorCode = DtColor.Rows[0]["ColorCode"].ToString();
                                    model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = DtColor.Rows[0]["ColorName"].ToString();
                                }
                                else if (DtColor.Rows[0]["CheckStatus"].ToString().Contains("YTR"))
                                {
                                    model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "YTR on Hold";
                                }
                                else if (DtColor.Rows[0]["CheckStatus"].ToString() == "Not Applicable")
                                {
                                    model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "Not Applicable";
                                }
                                else if (DtColor.Rows[0]["CheckClientStatus"].ToString().ToLower().Contains("wip"))
                                {
                                    model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "WIP";
                                }
                                else if (DtColor.Rows[0]["CheckClientStatus"].ToString().ToLower() == "funds pending")
                                {
                                    model.EducationInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "Funds pending";
                                }

                                else if (DtColor.Rows[0]["CheckClientStatus"].ToString().ToLower().Contains("insuff"))
                                {
                                    model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().CheckColorName = "Insuff";
                                }


                                model.CriminalInfoModel.Where(T => T.SubcheckRowID == Convert.ToInt32(dr["SubcheckRowID"].ToString())).FirstOrDefault().ReportComment = DtColor.Rows[0]["ReportComments"].ToString();
                                // ReportComment
                            }

                        }
                    }
                }
                //}
            }

            #endregion
            #endregion

            if (model.ClientRowID == 268)
            {
                if (model.AddressInfoModel.Count() > 0)
                {
                    var current = model.AddressInfoModel.Where(T => T.SubcheckRowID == 2 && T.AntecedentRowID == "239").ToList();
                    var permanant = model.AddressInfoModel.Where(T => T.SubcheckRowID == 13 && T.AntecedentRowID == "239").ToList();
                    if (current.Count > 0 && permanant.Count > 0)
                    {
                        if (string.Equals(current.FirstOrDefault().DataEntryValue, permanant.FirstOrDefault().DataEntryValue) &&
                        (current.FirstOrDefault().DataEntryValue != null || permanant.FirstOrDefault().DataEntryValue != null))
                        {
                            model.Address_currentPermanantSame = current.FirstOrDefault().DataEntryValue;
                        }
                        else
                        {
                            model.Address_currentPermanantSame = "No";
                        }
                    }
                    else
                    {
                        model.Address_currentPermanantSame = "No";
                    }
                }

                if (model.CriminalInfoModel.Count() > 0)
                {
                    var current = model.CriminalInfoModel.Where(T => T.SubcheckRowID == 24 && T.AntecedentRowID == "377").ToList();
                    var permanant = model.CriminalInfoModel.Where(T => (T.SubcheckRowID == 28) && T.AntecedentRowID == "377").ToList();
                    if (current.Count > 0 && permanant.Count > 0)
                    {
                        if (string.Equals(current.FirstOrDefault().DataEntryValue, permanant.FirstOrDefault().DataEntryValue) &&
                        (current.FirstOrDefault().DataEntryValue != null || permanant.FirstOrDefault().DataEntryValue != null))
                        {
                            model.Criminal_currentPermanantSame = current.FirstOrDefault().DataEntryValue;
                        }
                        else
                        {
                            model.Criminal_currentPermanantSame = "No";
                        }
                    }
                    else
                    {
                        model.Criminal_currentPermanantSame = "No";
                    }
                }
            }

            return new Rotativa.ViewAsPdf("TCSSummaryReportPdf", model)
            {
                FileName = model.CompanyRefNo + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(10, 10, 10, 10),
                CustomSwitches = "--enable-local-file-access"
            };
        }

        /* Start Over all case count */

        // GET: ClientServicing
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        //[ValidateSessionFilterSpokLoginInternal]
        public ActionResult InsuffYTRFundsPending()
        {
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                string Designation = Session["Designation"].ToString();
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                BindClientListByLogInName(SpocEmail); // TOODO
                //Department Manager
                //bool flagMgr = false;
                if (Designation == "Insuff-YTR-FundsPending")
                {
                    //flagMgr = true;
                    CreateNavigationforInsuffCaseForSpoc(0);
                    ViewBag.Dashboard = "Pending Cases Dashboard!";
                }

                //if (flagMgr)
                //{
                //    ViewBag.ManagerDashboard = repoDashboard.GetManagerClientServicingDashBoardInfo(teamMemerRowId, "Manager-Add");
                //}
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View();
        }



        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult PendingCaseDashboardInfo(int page = 1, bool fetchDataCheckbox = false, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", byte tatdays = 0)
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                short clientid = 0;
                if (cr != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                }
                int pageSize = 20;
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                Session["CaseStatus"] = CaseStatus;
                Session["clientid"] = clientid;
                string Designation = Session["Designation"].ToString();
                string Department = Session["Department"].ToString();
                model = repoDashboard.GetPendingCaseDashBoardInfo1(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid, fetchDataCheckbox);

                //IBy this Code e are getting clientrowid on searching for showing LOA for Genpac-EU and Uniques.
                if (Search != "")
                {
                    var clients = model.DashboardClientCandidates;
                    foreach (var client in clients)
                    {
                        // Access the ClientRowID property
                        ViewBag.ClientRowIDForLOA = clsCommonMethods.PEncrypt(client.ClientRowID.ToString());
                    }
                }

                Session["CaseStatus"] = CaseStatus;
                Session["cr"] = clientid;
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                BindClientListByLogInName(SpocEmail);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult PendingCaseManageFundApprovelforSpoc(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", byte tatdays = 0)
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                short clientid = 0;
                if (cr != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                }

                int pageSize = 20;
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                //ClientRowID = Convert.ToInt16(Request.QueryString["cr"].ToString());
                //if (!string.IsNullOrEmpty(CaseStatus) && (CaseStatus == "Pendingwithcandidate" || CaseStatus == "PendingwithHR"))
                //{
                //    model = repoDashboard.GetClientServicingDashBoardInfo1(page, pageSize, Search, teamMemerRowId, CaseStatus, ClientRowID);
                //}
                //else
                //{                 

                model = repoDashboard.GetPendingCaseDashBoardInfoFundApprovespoc(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid);
                //}
                Session["CaseStatus"] = CaseStatus;
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                BindClientListByLogInName(SpocEmail);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        /* End Over all case count */


        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult MISDeptWiseCount(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string FromDate = "", string ToDate = "", byte tatdays = 0)
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                var fDate = (dynamic)null;
                var tDate = (dynamic)null;
                var difDay = (dynamic)null;
                if (!string.IsNullOrEmpty(FromDate))
                {
                    fDate = DateTime.Parse(FromDate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    tDate = DateTime.Parse(ToDate);
                }
                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    difDay = (tDate - fDate).Days;
                }
                if (difDay > 30)
                {
                    ViewBag.ErrorMsg = "The difference between the dates should be not greater than 30 days. ";
                }
                else
                {
                    short clientid = 0;
                    if (cr != "")
                    {
                        clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                    }
                    int pageSize = 20;
                    string SpocEmail = Session["TeamMemberEmail"].ToString();
                    short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                    model = repoDashboard.GetMISDeptWiseCount(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid, FromDate, ToDate);
                    Session["CaseStatus"] = CaseStatus;
                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }
                    // BindClientListByLogInName(SpocEmail);
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
        public void ExportMISDeptWiseCount(int ClientId, string CaseStatus, int teamMemberRowID, string search = "", string FromDate = "", string ToDate = "")
        {

            DataTable dt = new DataTable();
            try
            {
                var fDate = (dynamic)null;
                var tDate = (dynamic)null;
                var difDay = (dynamic)null;
                if (!string.IsNullOrEmpty(FromDate))
                {
                    fDate = DateTime.Parse(FromDate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    tDate = DateTime.Parse(ToDate);
                }
                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    difDay = (tDate - fDate).Days;
                }
                if (difDay > 30)
                {
                    ViewBag.ErrorMsg = "The difference between the dates should be not greater than 30 days. ";
                }
                else
                {
                    ClientCustomize cc = new ClientCustomize();
                    dt = cc.GetMISDeptWiseCount(ClientId, CaseStatus, teamMemberRowID, search, FromDate, ToDate);
                    ExportToExcel(dt, "MISDeptWiseCountReport");
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        #region Add Special Instruction
        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult SpecialInstructioncsc(int id = 0)
        {
            PQClientSpecialInstructionViewModel model = new PQClientSpecialInstructionViewModel();
            try
            {
                string TeamMemberRowID = Convert.ToString(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(Convert.ToInt16(TeamMemberRowID));
                GetCheckFamilyForIns();
                BindHCDepartmentList();
                BindPriorityList();
                model = repoPQClient.GetPQClientInstructionForUpdateById(id);

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            TempData["Message"] = "";
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SpecialInstructioncsc(PQClientSpecialInstructionViewModel objs)
        {
            PQClientSpecialInstructionViewModel model = new PQClientSpecialInstructionViewModel();
            try
            {
                string TeamMemberRowID = Convert.ToString(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(Convert.ToInt16(TeamMemberRowID));
                TempData["Message"] = "";
                GetCheckFamilyForIns();
                BindHCDepartmentList();
                BindPriorityList();
                if (objs.btnSubmit != "Search")
                {
                    objs.CreatedBy = Convert.ToInt64(Session["TeamMemberRowID"]);
                    //repoPQClient.AddPQClientSpecialInstruction(objs);
                    if (objs.btnSubmit == "Submit")
                    {
                        ClientCustomize cc = new ClientCustomize();
                        var Res = cc.GetExistSpecialInstruction(objs.ClientRowID, objs.CheckFamilyRowID, objs.Department);
                        if (Res.Rows.Count > 0)
                        {
                            ViewBag.ErrorMsg = "Special Instruction Exist.";
                        }
                        else
                        {
                            if (objs.CheckFamilyRowID == 1)
                            {
                                var checkFamlieslist = repoDDL.GetCheckFamilyForDDL().ToList();
                                var idArray = checkFamlieslist.Select(x => x.CheckFamilyRowID).ToArray();
                                for (int i = 0; i < idArray.Length; i++)
                                {
                                    objs.CheckFamilyRowID = idArray[i];
                                    repoPQClient.AddPQClientSpecialInstruction(objs);

                                }
                                TempData["Message"] = "Record Saved Successfully.";
                            }
                            else
                            {
                                repoPQClient.AddPQClientSpecialInstruction(objs);
                                TempData["Message"] = "Record Saved Successfully.";
                            }
                            //repoPQClient.AddPQClientSpecialInstruction(objs);
                            //TempData["Message"] = "Record Saved Successfully.";
                        }
                    }
                    if (objs.btnSubmit == "Update")
                    {

                        if (objs.CheckFamilyRowID == 1)
                        {
                            var checkFamlieslist = repoDDL.GetCheckFamilyForDDL().ToList();
                            var idArray = checkFamlieslist.Select(x => x.CheckFamilyRowID).ToArray();
                            for (int i = 0; i < idArray.Length; i++)
                            {
                                objs.CheckFamilyRowID = idArray[i];
                                repoPQClient.AddPQClientSpecialInstruction(objs);


                            }
                            TempData["Message"] = "Record Updated Successfully.";
                        }
                        else
                        {
                            repoPQClient.AddPQClientSpecialInstruction(objs);
                            TempData["Message"] = "Record Updated Successfully.";
                        }
                        //repoPQClient.AddPQClientSpecialInstruction(objs);
                        //TempData["Message"] = "Record Updated Successfully.";
                    }
                }

                if (objs.CheckFamilyRowID == 1)
                {
                    model = repoPQClient.GetPQClientInstructionForUpdateById(objs.ClientRowID);
                }
                else
                {
                    //model = repoPQClient.GetPQClientInstructionForUpdateById(objs.ClientRowID, objs.Department, objs.CheckFamilyRowID);
                    model = repoPQClient.GetPQClientInstructionForUpdateById(objs.ClientRowID);
                }



            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                TempData["Message"] = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

      
        //
        
        private void GetCheckFamilyForIns()
        {
            CheckFamilyDDLViewModel objs = new CheckFamilyDDLViewModel();
            objs.CheckFamilyRowID = 0;
            objs.CheckFamilyName = "--Select--";
            var lst = repoDDL.GetCheckFamilyForDDL().ToList();
            lst.Insert(0, objs);
            CheckFamilyDDLViewModel objse = new CheckFamilyDDLViewModel();
            objse.CheckFamilyRowID = 1;
            objse.CheckFamilyName = "--Select ALL--";
            lst.Insert(1, objse);
            ViewBag.CheckFamiliesForInstruction = lst;



        }
        [NonAction]
        void BindHCDepartmentList()
        {
            try
            {
                IEnumerable<SelectListItem> Department = new List<SelectListItem> {
                new SelectListItem { Text = "--Select--", Value = "0" },
                new SelectListItem { Text = "Data Entry", Value = "8" },
                new SelectListItem { Text = "Scan", Value = "2" },
                new SelectListItem { Text = "QC Report", Value = "17" },
                new SelectListItem { Text = "Address", Value = "1" },
                new SelectListItem { Text = "Education", Value = "9" },
                new SelectListItem { Text = "Employment", Value = "10" },
                new SelectListItem { Text = "Reference", Value = "16" },
                new SelectListItem { Text = "Criminal", Value = "7" },
                new SelectListItem { Text = "National Idendity", Value = "13" },
                new SelectListItem { Text = "Special Check", Value = "19" },
            };
                ViewBag.DepartmentList = Department;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [NonAction]
        void BindPriorityList()
        {
            try
            {
                IEnumerable<SelectListItem> Priority = new List<SelectListItem> {
                new SelectListItem { Text = "--Select--", Value = "0" },
                new SelectListItem { Text = "High", Value = "1" },
                new SelectListItem { Text = "Medium", Value = "2" },
                new SelectListItem { Text = "Low", Value = "3" },
                new SelectListItem { Text = "Normal", Value = "4" },
            };
                ViewBag.PriorityList = Priority;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        #endregion
        #region Add Client Spoc Details 
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddClientDetails(int ClientRowID = 0)
        {
            List<GetClietDetails> liClitdata = new List<GetClietDetails>();
            try
            {
                short TeamMemberRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(TeamMemberRowId);
                string AType = "";
                int pageSize = clsGlobalCommon.PageSize;
                ClientCustomize cc = new ClientCustomize();
                if (ClientRowID > 0)
                {
                    //DataTable dt = cc.Get_Client_Deatils(TeamMemberRowId, "list");
                    DataTable dt = cc.Get_Client_Deatils(ClientRowID, "one");
                    liClitdata = (from DataRow dr in dt.Rows
                                  select new GetClietDetails()
                                  {
                                      ClientRowID = Convert.ToInt32(dr["ClientRowID"]),
                                      ClientName = Convert.ToString(dr["ClientName"]),
                                      ClientCode = Convert.ToString(dr["ClientCode1"]),
                                      Address = Convert.ToString(dr["Address"]),
                                  }).ToList();
                    if (liClitdata == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
                    }

                    return RedirectToAction("AddSpocDetails", new { ClientId = ClientRowID });

                }

                Session["ClientNameForContract"] = null;
                Session["PricingType"] = null;
                Session["MapAntecedent"] = null;
                Session["MapDisposition"] = null;
                Session["MapSeverity"] = null;
                Session["MailSendBy"] = null;

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(liClitdata);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddClientDetails(FormCollection form)
        {
            List<GetClietDetails> liClitdata = new List<GetClietDetails>();
            int Id = 0;
            if (Convert.ToInt32(form["ClientRowId"]) != 0)
            {
                Id = Convert.ToInt32(form["ClientRowId"]);
            }
            try
            {
                short TeamMemberRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                BindClientListBaseOnLogin(TeamMemberRowId);
                string AType = "";
                if (Id != 0)
                {
                    AType = "one";
                }
                else
                {
                    AType = "list";
                    Id = Convert.ToInt32(Session["TeamMemberRowID"]);
                }
                int pageSize = clsGlobalCommon.PageSize;
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.Get_Client_Deatils(Id, AType);
                liClitdata = (from DataRow dr in dt.Rows
                              select new GetClietDetails()
                              {
                                  ClientRowID = Convert.ToInt32(dr["ClientRowID"]),
                                  ClientName = Convert.ToString(dr["ClientName"]),
                                  ClientCode = Convert.ToString(dr["ClientCode1"]),
                                  Address = Convert.ToString(dr["Address"]),
                              }).ToList();
                if (liClitdata == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }

                Session["ClientNameForContract"] = null;
                Session["PricingType"] = null;
                Session["MapAntecedent"] = null;
                Session["MapDisposition"] = null;
                Session["MapSeverity"] = null;
                Session["MailSendBy"] = null;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(liClitdata);
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddSpocDetails(short ClientId = 0, string CaseStatus = "")
        {

            UpdatePQClientViewModel model = new UpdatePQClientViewModel();
            DataTable dt = new DataTable();
            try
            {
                BindClientAbbList();
                BindPrimaryClientSpocList();
                BindSecondaryClientSpocList();
                ClientCustomize cc = new ClientCustomize();
                model = repoPQClient.GetPQClientForUpdateByIdNew(ClientId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult AddSpocDetails(UpdatePQClientViewModel model)
        {
            try
            {

                if (model.ClientRowID > 0)
                {
                    short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);


                    ClientCustomize cc = new ClientCustomize();
                    //var History = cc.GetauditClientHistory_sp(model.ClientRowID, teamMemerRowId);
                    cc.InsertSpocDeatailLogTable(Convert.ToInt32(model.ClientRowID), model.SpocName, model.SecondarySpocName, model.CSpocEmailID, model.CSpocName, model.CSpocEmailID, Convert.ToInt32(teamMemerRowId));
                    cc.deleteClientSpoceDetails(Convert.ToInt32(model.ClientRowID));
                    int Res = cc.InsertClientSpoceDetails(model, teamMemerRowId);
                    if (Res == 0)
                    {
                        TempData["SuccessMsg"] = "Record Edited Successfully!";
                        //ModelState.Clear();
                        BindClientAbbList();
                        BindPrimaryClientSpocList();
                        BindSecondaryClientSpocList();
                        cc.InsertSpocDeatailLogTable(Convert.ToInt32(model.ClientRowID), model.SpocName, model.SecondarySpocName, model.CSpocEmailID, model.CSpocName, model.CSpocEmailID, Convert.ToInt32(teamMemerRowId));
                        return RedirectToAction("AddSpocDetails", new { ClientId = model.ClientRowID });
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "There is no change!";
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Record Already Exist!");
                }
                BindClientAbbList();
                BindPrimaryClientSpocList();
                BindSecondaryClientSpocList();

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            BindClientAbbList();
            BindPrimaryClientSpocList();
            BindSecondaryClientSpocList();
            cc.InsertSpocDeatailLogTable(0, "error", "error", "error", "error", "error", Convert.ToInt32(Session["TeamMemberRowID"]));
            return View(model);
        }

        [NonAction]
        public void BindClientAbbList()
        {
            try
            {
                ViewBag.ClientAbbList = repoDDL.GetClientSubGroupDDL();
            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        [NonAction]
        public void BindClientAbbListbyId(int tmid)
        {
            try
            {
                int teamMemerRowId = Convert.ToInt32(Session["TeamMemberRowID"]);
                ViewBag.ClientAbbList = repoDDL.GetClientSubGroupDDLbyId(teamMemerRowId);
            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        [NonAction]
        void BindPrimaryClientSpocList()
        {
            try
            {
                ViewBag.PrimaryClientSpocList = repoDDL.GetPrimarySpocForClientForChkBoxList();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [NonAction]
        void BindSecondaryClientSpocList()
        {
            try
            {

                ViewBag.SecondaryClientSpocList = repoDDL.GetSecondarySpocForClientForChkBoxList();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        #endregion
        #region Update Email ID
        [HttpGet]
        public ActionResult GetEmailDetails(int Id = 0)
        {
            string TeamMemberRowID = Convert.ToString(Session["TeamMemberRowID"]);
            ClientCustomize cc = new ClientCustomize();
            BindClientListBaseOnLogin(Convert.ToInt16(TeamMemberRowID));
            return View();
        }
        [HttpPost]
        public ActionResult GetEmailDetails(FormCollection form)
        {
            List<UserEmailDetails> liuser = new List<UserEmailDetails>();
            int id = Convert.ToInt32(form["ClientRowID"]);
            string TeamMemberRowID = Convert.ToString(Session["TeamMemberRowID"]);
            BindClientListBaseOnLogin(Convert.ToInt16(TeamMemberRowID));
            if (id != 0)
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetClientLoginSettings(id, "list");
                liuser = (from DataRow dr in dt.Rows
                          select new UserEmailDetails()
                          {
                              ClientName = Convert.ToString(dr["ClientName"]),
                              ClientUserRowID = Convert.ToInt32(dr["ClientUserRowID"]),
                              UserID = Convert.ToString(dr["UserID"]),
                              //UserEmailID = Convert.ToString(dr["UserEmailID"]),
                              //CaseInitiationMail = Convert.ToString(dr["CaseInitiationMail"]),
                              //SendInsuffEmail = Convert.ToString(dr["SendInsuffEmail"]),
                              //SendRedReportEmail = Convert.ToString(dr["SendRedReportEmail"]),
                              //SendFundsApprovedMail = Convert.ToString(dr["SendFundsApprovedMail"]),
                              //SendYTRMail = Convert.ToString(dr["SendYTRMail"]),
                              //SendReportEmail = Convert.ToString(dr["SendReportEmail"])
                          }).ToList();
            }
            return View(liuser);
        }

        [HttpGet]
        public ActionResult UpdateEmailDetails(int Id = 0)
        {
            UserEmailDetails user = new UserEmailDetails();
            //int id = Convert.ToInt32(form["ClientRowID"]);
            string TeamMemberRowID = Convert.ToString(Session["TeamMemberRowID"]);
            BindClientListBaseOnLogin(Convert.ToInt16(TeamMemberRowID));
            if (Id != 0)
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetClientLoginSettings(Id, "one");
                if (dt.Rows.Count > 0)
                {
                    user.ClientName = Convert.ToString(dt.Rows[0]["ClientName"]);
                    user.ClientUserRowID = Convert.ToInt32(dt.Rows[0]["ClientUserRowID"]);
                    user.UserID = Convert.ToString(dt.Rows[0]["UserID"]);
                    user.UserEmailID = Convert.ToString(dt.Rows[0]["UserEmailID"]);
                    user.CaseInitiationMail = Convert.ToString(dt.Rows[0]["CaseInitiationMail"]);
                    user.SendInsuffEmail = Convert.ToString(dt.Rows[0]["SendInsuffEmail"]);
                    user.SendRedReportEmail = Convert.ToString(dt.Rows[0]["SendRedReportEmail"]);
                    user.SendFundsApprovedMail = Convert.ToString(dt.Rows[0]["SendFundsApprovedMail"]);
                    user.SendYTRMail = Convert.ToString(dt.Rows[0]["SendYTRMail"]);
                    user.SendReportEmail = Convert.ToString(dt.Rows[0]["SendReportEmail"]);
                }
                // ViewBag.Userdate = user;
            }
            return View(user);
        }


        [HttpPost]
        public ActionResult UpdateEmailDetails(UserEmailDetails model, FormCollection form)
        {
            UserEmailDetails user = new UserEmailDetails();
            int TeamMemberRowID = Convert.ToInt32(Session["TeamMemberRowID"]);
            // int id = Convert.ToInt32(form["ClientRowID"]);
            model.Teammemberid = TeamMemberRowID;
            ClientCustomize cc = new ClientCustomize();
            int res = cc.UpdateUserEmailDetails(model);
            if (res == 1)
            {

                ViewBag.msg = "Update successfully";
            }
            else
            {
                ViewBag.msg = "Error";

            }
            DataTable dt = cc.GetClientLoginSettings(model.ClientUserRowID, "one");
            if (dt.Rows.Count > 0)
            {
                user.ClientUserRowID = Convert.ToInt32(dt.Rows[0]["ClientUserRowID"]);
                user.UserID = Convert.ToString(dt.Rows[0]["UserID"]);
                user.UserEmailID = Convert.ToString(dt.Rows[0]["UserEmailID"]);
                user.CaseInitiationMail = Convert.ToString(dt.Rows[0]["CaseInitiationMail"]);
                user.SendInsuffEmail = Convert.ToString(dt.Rows[0]["SendInsuffEmail"]);
                user.SendRedReportEmail = Convert.ToString(dt.Rows[0]["SendRedReportEmail"]);
                user.SendFundsApprovedMail = Convert.ToString(dt.Rows[0]["SendFundsApprovedMail"]);
                user.SendYTRMail = Convert.ToString(dt.Rows[0]["SendYTRMail"]);
                user.SendReportEmail = Convert.ToString(dt.Rows[0]["SendReportEmail"]);
            }
            //ViewBag.Userdate = user;
            return View(user);
        }


        [NonAction]
        void BindClientListBaseOnLogin(short TeamMemberRowId = 0)
        {
            try
            {
                List<ClientDDLViewModel> liobj = new List<ClientDDLViewModel>();
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.Get_Client_Deatils(TeamMemberRowId, "list");
                liobj = (from DataRow dr in dt.Rows
                         select new ClientDDLViewModel()
                         {
                             ClientRowID = Convert.ToInt32(dr["ClientRowID"]),
                             ClientName = Convert.ToString(dr["ClientName"]),
                         }).ToList();
                ViewBag.ClientList = liobj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        [NonAction]
        void BindClientListBaseOnLoginScop(short TeamMemberRowId = 0)
        {
            try
            {
                List<ClientDDLViewModelEnc> liobj = new List<ClientDDLViewModelEnc>();
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.Get_Client_Deatils(TeamMemberRowId, "list");
                liobj = (from DataRow dr in dt.Rows
                         select new ClientDDLViewModelEnc()
                         {
                             ClientRowID = clsCommonMethods.PEncrypt(dr["ClientRowID"].ToString()),
                             ClientName = Convert.ToString(dr["ClientName"]),
                         }).ToList();
                ViewBag.ClientList = liobj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult AnnexureInvoicing(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string FromDate = "", string ToDate = "", string ClientName = "", string BilingType = "", byte tatdays = 0)
        {
            BindClientList();
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                var fDate = (dynamic)null;
                var tDate = (dynamic)null;
                var difDay = (dynamic)null;
                if (!string.IsNullOrEmpty(FromDate))
                {
                    fDate = DateTime.Parse(FromDate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    tDate = DateTime.Parse(ToDate);
                }
                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    difDay = (tDate - fDate).Days;
                }
                if (difDay > 30)
                {
                    ViewBag.ErrorMsg = "The difference between the dates should be not greater than 30 days. ";
                }
                else
                {
                    short clientid = 0;
                    if (cr != "")
                    {
                        clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                    }
                    int pageSize = 20;
                    string SpocEmail = Session["TeamMemberEmail"].ToString();
                    short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                    model = repoDashboard.GetAnnexureInvoicing(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid, FromDate, ToDate, ClientName, BilingType);
                    Session["CaseStatus"] = CaseStatus;
                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
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
        public void ExportAnnexureInvoicing(int ClientId, string CaseStatus, int teamMemberRowID, string search = "", string FromDate = "", string ToDate = "", string ClientName = "", string BilingType = "")
        {
            DataTable dt = new DataTable();
            try
            {
                var fDate = (dynamic)null;
                var tDate = (dynamic)null;
                var difDay = (dynamic)null;
                if (!string.IsNullOrEmpty(FromDate))
                {
                    fDate = DateTime.Parse(FromDate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    tDate = DateTime.Parse(ToDate);
                }
                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    difDay = (tDate - fDate).Days;
                }
                if (difDay > 30)
                {
                    ViewBag.ErrorMsg = "The difference between the dates should be not greater than 30 days. ";
                }
                else
                {
                    ClientCustomize cc = new ClientCustomize();
                    dt = cc.GetAnnexureInvoicing(ClientId, CaseStatus, teamMemberRowID, search, FromDate, ToDate, ClientName, BilingType);
                    ExportToExcel(dt, "AnnexureInvoicingReport");
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AnnexureInvoicing(ClientCandidateViewModel model)
        {
            try
            {
                BindClientList();
                DashboardClientCandidateListViewModel objDashboardClientCandidateListViewModel = new DashboardClientCandidateListViewModel();
                string filePathDoc = ConfigurationManager.AppSettings["AnnexureInvoicingDocuments"].ToString();
                model.AnnexureUploadOptionFilePath = filePathDoc;
                string proImg = string.Empty;

                model = repoDashboard.SaveUpdateAnnexureInvoicing(model);
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                return RedirectToAction("AnnexureInvoicing", "ClientServicing");

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AnnexureInvoicingUploadDetail(ClientCandidateViewModel model)
        {
            try
            {
                string TeamMemberRowID = Session["TeamMemberRowID"].ToString();
                ClientCustomize cc = new ClientCustomize();
                int fileNo = cc.AnnexureInvoiceUploadCount();

                if (model.UploadDoc1 != null && model.UploadDoc1.ContentLength > 0)
                {
                    fileNo++;
                    string Extension = Path.GetExtension(model.UploadDoc1.FileName).ToLower();
                    string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.UploadDoc1.FileName).ToLower();
                    string FileName = FileNameWithoutExtension.ToLower() + '_' + DateTime.Now.ToString("yyyyMMddhhmmss") + Extension;
                    string FolderPath = ConfigurationManager.AppSettings["AnnexureInvoicingDocuments"];
                    string FilePath = Server.MapPath(FolderPath + FileName);
                    model.UploadDoc1.SaveAs(Server.MapPath(FolderPath + FileName));
                    cc.InsertUpdateAnnexureInvoiceUploadfile(model.ClientRowID, FileName, model.AnnexureInvoicingId, model.AnnexureInvoicingType);
                    return RedirectToAction("AnnexureInvoicing", "ClientServicing", new { fileName = FileName });
                }
                else
                {
                    ViewBag.ErrorMsg = "Please Select Excel Document!";
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
        public ActionResult AnnexureInvoicingUploadDetail(int AnnexureInvoicingId, int ClientRowID, string AnnexureInvoicingType)
        {
            ClientCandidateViewModel model = new ClientCandidateViewModel();
            try
            {
                string FileName = "AnnexureInvoicingDocumentsBulkUpload.xlsx";
                string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["AnnexureInvoicingDocuments"].ToString();
                model.Template = Url.Content(FldPathDoc + FileName);
                model.AnnexureInvoicingId = Convert.ToInt16(AnnexureInvoicingId);
                model.ClientRowID = Convert.ToInt16(ClientRowID);
                model.AnnexureInvoicingType = AnnexureInvoicingType;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult EditAnnexureInvoicing(int AnnexureInvoicingId = 0)
        {
            ClientCandidateViewModel model = new ClientCandidateViewModel();
            try
            {
                model = repoDashboard.GetAnnexureInvoiceDetailsBasedonId(AnnexureInvoicingId);
            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult EditAnnexureInvoicing(ClientCandidateViewModel model)
        {
            try
            {
                if (model != null && model.AnnexureInvoicingId > 0)
                {
                    model = repoDashboard.EditAnnexureInvoiceDetailsBasedonId(model);
                    ViewBag.ErrorMsg = "Record Edited Successfully!";
                    return RedirectToAction("AnnexureInvoicing");
                }
            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View();
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public JsonResult DeleteAnnexureInvoicing(int id)
        {
            bool result = false;

            try
            {
                string deleteResult = repoDashboard.deleteAnnexureInvoiceDetailsBasedonId(id);
                result = true;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public JsonResult SendMailAnnexureInvoicing(int id, int ReminderSendOption)
        {
            bool result = false;
            try
            {
                ClientCustomize cc = new ClientCustomize();
                clsSendMail mail = new clsSendMail();
                DataTable dt = new DataTable();
                int InvoicingId = 0;
                int ClientRowID = 2;
                string SpocName = string.Empty;
                string BillingType = string.Empty;
                string Month = string.Empty;
                string ClientName = string.Empty;
                string InvoiceNo = string.Empty;
                string InvoiceDate = string.Empty;
                string InvoiceAmount = string.Empty;
                string Aging = string.Empty;
                string InvoiceSharedDate = string.Empty;
                string Invoicecreationdate = string.Empty;
                string Amount = string.Empty;
                string PaymentReceivedDate = string.Empty;
                string AcknowledgementDate = string.Empty;
                string Followup1 = string.Empty;
                string Followup2 = string.Empty;
                string Followup3 = string.Empty;
                string Remarks = string.Empty;
                string PaymentExpecteddate = string.Empty;
                string Paymentstatus = string.Empty;
                string ClientEmailId = string.Empty;
                string bodySpocAndClient = string.Empty;
                string MailTemplateNameSpoc = string.Empty;
                string SpocAndClientMailSubject = string.Empty;
                string resultDetails = string.Empty;
                string MailSignature = string.Empty;
                string DisplayName = string.Empty;
                //Attachment attachment;
                Attachment attachmentAnnexureUploadFileName;
                Attachment attachmentAnnexureApprovedUploadFileName;
                Attachment attachmentInvoiceUploadFileName;
                List<Attachment> attachmentList = new List<Attachment>();
                string AnnexureInvoicingDocuments = string.Empty;
                string AnnexurApprovedDocuments = string.Empty;
                string InvoiceDocuments = string.Empty;


                if (id > 0)
                {
                    dt = cc.SendMailAnnexureInvoiceDetailsBasedonId(id, ReminderSendOption);
                    ClientEmailId = Convert.ToString(dt.Rows[0]["ClientEmailId"]);
                    var templateDBSpocAndClient = cc.getMailtemplateforAnnexureInvoicing(ClientRowID, "AnnexureInvoicing", 0, 1, "AnnexureInvoicing");
                    var signature = mail.GetCompanyMailSignature();
                    MailSignature = string.Concat(MailSignature, "<b>", signature.CompanyName.Trim(),
                                    "</b><br/><b>E-mail id </b>: <a style='color:#df012a;' href='mailto:", "bgv.helpdesk@securitas-india.com", "'>", "bgv.helpdesk@securitas-india.com", "</a>");

                    string pathOfImageContent = ConfigurationManager.AppSettings["mailimages"].ToString();

                    LinkedResource LRsecuritaslogo = new LinkedResource(Server.MapPath(pathOfImageContent) + "Securitas-logo.png", MediaTypeNames.Image.Jpeg);
                    LRsecuritaslogo.ContentId = Guid.NewGuid().ToString();
                    string securitaslogo = @"<img src='cid:" + LRsecuritaslogo.ContentId + @"' width='25%'/>";

                    if (templateDBSpocAndClient.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["AnnexureUploadOptionFileName"])))
                        {
                            AnnexureInvoicingDocuments = ConfigurationManager.AppSettings["AnnexureInvoicingDocuments"].ToString() + Convert.ToString(dt.Rows[0]["AnnexureUploadOptionFileName"]);
                            attachmentAnnexureUploadFileName = new Attachment(Server.MapPath(AnnexureInvoicingDocuments));
                            attachmentList.Add(attachmentAnnexureUploadFileName);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["AnnexureApprovedUploadOption"])))
                        {
                            AnnexurApprovedDocuments = ConfigurationManager.AppSettings["AnnexureInvoicingDocuments"].ToString() + Convert.ToString(dt.Rows[0]["AnnexureApprovedUploadOption"]);
                            attachmentAnnexureApprovedUploadFileName = new Attachment(Server.MapPath(AnnexurApprovedDocuments));
                            attachmentList.Add(attachmentAnnexureApprovedUploadFileName);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["InvoiceUploadOptionFile"])))
                        {
                            InvoiceDocuments = ConfigurationManager.AppSettings["AnnexureInvoicingDocuments"].ToString() + Convert.ToString(dt.Rows[0]["InvoiceUploadOptionFile"]);
                            attachmentInvoiceUploadFileName = new Attachment(Server.MapPath(InvoiceDocuments));
                            attachmentList.Add(attachmentInvoiceUploadFileName);
                        }
                        bodySpocAndClient = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailTemplate"]);
                        bodySpocAndClient = bodySpocAndClient.Replace("{SpocName}", Convert.ToString(dt.Rows[0]["SpocName"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{Month}", Convert.ToString(dt.Rows[0]["Month"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{ClientName}", Convert.ToString(dt.Rows[0]["ClientName"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{InvoiceNo}", Convert.ToString(dt.Rows[0]["InvoiceNo"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{InvoiceDate}", Convert.ToString(dt.Rows[0]["InvoiceDate"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{InvoiceAmount}", Convert.ToString(dt.Rows[0]["InvoiceAmount"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{Aging}", Convert.ToString(dt.Rows[0]["Aging"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{InvoiceSharedDate}", Convert.ToString(dt.Rows[0]["InvoiceSharedDate"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{Invoicecreationdate}", Convert.ToString(dt.Rows[0]["Invoicecreationdate"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{Amount}", Convert.ToString(dt.Rows[0]["Amount"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{PaymentReceivedDate}", Convert.ToString(dt.Rows[0]["PaymentReceivedDate"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{AcknowledgementDate}", Convert.ToString(dt.Rows[0]["AcknowledgementDate"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{Followup1}", Convert.ToString(dt.Rows[0]["Followup1"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{Followup2}", Convert.ToString(dt.Rows[0]["Followup2"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{Followup3}", Convert.ToString(dt.Rows[0]["Followup3"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{Remarks}", Convert.ToString(dt.Rows[0]["Remarks"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{PaymentExpecteddate}", Convert.ToString(dt.Rows[0]["PaymentExpecteddate"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{Paymentstatus}", Convert.ToString(dt.Rows[0]["Paymentstatus"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{ClientEmailId}", Convert.ToString(dt.Rows[0]["ClientEmailId"]));
                        bodySpocAndClient = bodySpocAndClient.Replace("{MailSignature}", MailSignature);
                        bodySpocAndClient = bodySpocAndClient.Replace("{securitas-logo}", securitaslogo);
                        bodySpocAndClient = bodySpocAndClient.Replace("{ReminderSentMailCount}", Convert.ToString(dt.Rows[0]["ReminderSentMailCount"]));
                        AlternateView alternateView = AlternateView.CreateAlternateViewFromString(bodySpocAndClient, null, MediaTypeNames.Text.Html);
                        alternateView.LinkedResources.Add(LRsecuritaslogo);

                        MailTemplateNameSpoc = Convert.ToString(templateDBSpocAndClient.Rows[0]["TemplateName"]);
                        SpocAndClientMailSubject = Convert.ToString(templateDBSpocAndClient.Rows[0]["MailSubject"]);

                        result = mail.AnnexureInvoicingSendMail("Annexure and Invoicing", "bgv.alert@securitas-india.com", ClientEmailId, ClientEmailId, ClientEmailId, SpocAndClientMailSubject, bodySpocAndClient, attachmentList, alternateView);
                        if (result == true)
                        {
                            resultDetails = "Mail has been Sent successfully!!!";
                        }
                        else
                        {
                            resultDetails = "Mail don't Sent!!";
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }


        #region ********** CheckTag Info/History (SA) **********

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ClientServicingCheckTagInfo(int page = 1, bool fetchDataCheckbox = false, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", byte tatdays = 0, string CandCheckRowIDs = "")
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);

            try
            {
                if (!string.IsNullOrEmpty(CandCheckRowIDs))
                {
                    ClientCustomize cc = new ClientCustomize();

                    int[] PerIDs = CandCheckRowIDs.Split(',').Select(str => int.Parse(str)).ToArray();

                    foreach (var item in PerIDs)
                    {
                        cc.InsertCheckTagHistory(item, teamMemerRowId);
                    }
                }

                short clientid = 0;
                if (cr != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                }
                int pageSize = 20;
                string SpocEmail = Session["TeamMemberEmail"].ToString();

                Session["clientid"] = clientid;
                Session["CaseStatus"] = CaseStatus;
                string Designation = Session["Designation"].ToString();
                model = repoDashboard.GetClientServicingCheckTagInfo(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid, fetchDataCheckbox);

                Session["cr"] = clientid;
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                BindClientListByLogInName(SpocEmail);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult ClientServicingCheckTagHistory(int page = 1, bool fetchDataCheckbox = false, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", byte tatdays = 0)
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                short clientid = 0;
                if (cr != "")
                {
                    clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                }
                int pageSize = 20;
                string SpocEmail = Session["TeamMemberEmail"].ToString();
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                Session["clientid"] = clientid;
                Session["CaseStatus"] = CaseStatus;
                string Designation = Session["Designation"].ToString();
                model = repoDashboard.GetClientServicingCheckTagHistory(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid, fetchDataCheckbox);

                Session["cr"] = clientid;
                if (model == null)
                {
                    ViewBag.ErrorMsg = "No item found!";
                }
                BindClientListByLogInName(SpocEmail);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpGet]
        [SessionExpireFilterAttribute]
        public void ClientServicingCheckTagExport(string strExportType = "")
        {
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                var data = repoDashboard.GetCheckTagForExport(strExportType, teamMemerRowId);
                ExportToExcelCheckTagByList(data, strExportType + "Report" + DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        public void ExportToExcelCheckTagByList<T>(IEnumerable<T> list, string strFileName)
        {
            short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);

            //get the data from database
            var data = list.ToList();

            string columnHeader1 = "ClientName";
            string columnHeader2 = "SecuritasID";
            string columnHeader3 = "";
            if ((teamMemerRowId == 160) || (teamMemerRowId == 492) || (teamMemerRowId == 638) || (teamMemerRowId == 703) || (teamMemerRowId == 1060) || (teamMemerRowId == 965) || (teamMemerRowId == 981) || (teamMemerRowId == 1031))
            {
                columnHeader3 = "CandidateID";
            }
            else
            {
                columnHeader3 = "ApplicantID";
            }

            string columnHeader4 = "CandidateName";
            string columnHeader5 = "CheckName";
            string columnHeader6 = "CheckID";
            string columnHeader7 = "ColorCode";
            string columnHeader8 = "CreatedDate";
            string columnHeader9 = "VerificationDate";
            string columnHeader10 = "CompletionDate";
            string columnHeader11 = "CheckTagBy";
            string columnHeader12 = "CheckTagDate";

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            Response.Write("<table border='1'>");
            Response.Write("<tr>");
            Response.Write("<th>" + columnHeader1 + "</th>");
            Response.Write("<th>" + columnHeader2 + "</th>");
            Response.Write("<th>" + columnHeader3 + "</th>");
            Response.Write("<th>" + columnHeader4 + "</th>");
            Response.Write("<th>" + columnHeader5 + "</th>");
            Response.Write("<th>" + columnHeader6 + "</th>");
            Response.Write("<th>" + columnHeader7 + "</th>");
            Response.Write("<th>" + columnHeader8 + "</th>");
            Response.Write("<th>" + columnHeader9 + "</th>");
            Response.Write("<th>" + columnHeader10 + "</th>");
            Response.Write("<th>" + columnHeader11 + "</th>");
            Response.Write("<th>" + columnHeader12 + "</th>");
            Response.Write("</tr>");

            foreach (var item in data)
            {
                Response.Write("<tr>");
                Response.Write("<td>" + item.GetType().GetProperty("ClientName").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("SecuritasID").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("EmployeeID").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("CandidateName").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("CheckName").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("CheckID").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("ColorCode").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("CreatedDate").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("VerificationDate").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("CompletionDate").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("CheckTagBy").GetValue(item, null) + "</td>");
                Response.Write("<td>" + item.GetType().GetProperty("CheckTagDate").GetValue(item, null) + "</td>");
                Response.Write("</tr>");
            }

            Response.Write("</table>");
            Response.Flush();
            Response.End();
        }




        public void ExportToExcelByList<T>(IEnumerable<T> list, string strFileName)
        {
            // Step 1 - get the data from database
            var data = list.ToList();

            // instantiate the GridView control from System.Web.UI.WebControls namespace
            // set the data source
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();

            // Clear all the content from the current response
            Response.ClearContent();
            Response.Buffer = true;

            // set the header
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            clsExportDataToExcel.WriteHtmlTable(data, Response.Output);

            Response.Flush();
            Response.End();
        }

        #endregion

        [SessionExpireFilterAttribute]
        [ValidateInput(false)]
        public ActionResult MISDataEntryQCAgeing(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string cr = "", string CaseStatus = "", string FromDate = "", string ToDate = "", byte tatdays = 0, string typeOfOption = "")
        {
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            try
            {
                var fDate = (dynamic)null;
                var tDate = (dynamic)null;
                var difDay = (dynamic)null;
                if (!string.IsNullOrEmpty(FromDate))
                {
                    fDate = DateTime.Parse(FromDate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    tDate = DateTime.Parse(ToDate);
                }
                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    difDay = (tDate - fDate).Days;
                }
                if (difDay > 30)
                {
                    ViewBag.ErrorMsg = "The difference between the dates should be not greater than 30 days. ";
                }
                else
                {
                    short clientid = 0;
                    if (cr != "")
                    {
                        clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
                    }
                    int pageSize = 20;
                    string SpocEmail = Session["TeamMemberEmail"].ToString();
                    short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                    string typeOfOp = typeOfOption;
                    Session["typeOfOption"] = typeOfOption;
                    model = repoDashboard.GetDataEntryAging(page, pageSize, Search, teamMemerRowId, CaseStatus, clientid, FromDate, ToDate, typeOfOp);
                    Session["CaseStatus"] = CaseStatus;
                    if (model == null)
                    {
                        ViewBag.ErrorMsg = "No item found!";
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
        public void ExportMISDataEntryQCAgeing(int ClientId, string CaseStatus, int teamMemberRowID, string search = "", string FromDate = "", string ToDate = "", string TypeOfOption = "")
        {

            DataTable dt = new DataTable();
            try
            {
                var fDate = (dynamic)null;
                var tDate = (dynamic)null;
                var difDay = (dynamic)null;
                if (!string.IsNullOrEmpty(FromDate))
                {
                    fDate = DateTime.Parse(FromDate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    tDate = DateTime.Parse(ToDate);
                }
                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    difDay = (tDate - fDate).Days;
                }
                if (difDay > 30)
                {
                    ViewBag.ErrorMsg = "The difference between the dates should be not greater than 30 days. ";
                }
                else
                {
                    ClientCustomize cc = new ClientCustomize();
                    dt = cc.GetDataEntryAging(ClientId, CaseStatus, teamMemberRowID, search, FromDate, ToDate, TypeOfOption);
                    ExportToExcel(dt, "AgeingReport");
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        [ValidateInput(false)]
        public ActionResult RecentCommonMISTracker()
        {

            BindClientList();

            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult RecentCommonMISTracker(int ClientRowID)
        {

            ClientCustomize cc = new ClientCustomize();
            BindClientList();
            try
            {
                DataTable dt = new DataTable();

                dt = cc.Recent_MIS_Client_common(ClientRowID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["NonGreenRemarks"] = clsCommonMethods.HTMLToText(Convert.ToString(dt.Rows[i]["NonGreenRemarks"])); ;
                    }
                }
                DataView view = new System.Data.DataView(dt);
                DataTable dtv = view.ToTable("Details", false);

                using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                {
                    var ws = wb.Worksheets.Add(dtv);
                    ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                    wb.Style.Font.Bold = true;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=CommonMISTracker.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }


                BindClientList();

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View();
        }

        [ValidateInput(false)]
        public ActionResult GetDownloadLTIPdf(int pid, string Report_Type = "")
        {
            ReportViewModel model = new ReportViewModel();
            ClientCustomize cc = new ClientCustomize();

            model = repoReportQC.GetReportPersonalInfo(pid);

            model.Report_Type = Report_Type;
            model.ReportDate = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MM-yyyy"));

            if (model.ClientRowID == 990)
            {
                DataTable dttechColor = new DataTable();
                dttechColor = cc.GetColorCodeTECHM(model.PersonalRowID, model.ClientRowID);
                if (dttechColor.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dttechColor.Rows[0]["colorName"].ToString()))
                    {
                        string closeall = dttechColor.Rows[0]["CloseAll"].ToString();
                        string YTRConfirm = dttechColor.Rows[0]["YTRConfirm"].ToString();
                        string CYTR = dttechColor.Rows[0]["CYTR"].ToString();
                        string YTRClearDate = dttechColor.Rows[0]["YTRClearDate"].ToString();
                        string InsuffClearDate = dttechColor.Rows[0]["InsuffClearDate"].ToString();

                        if (closeall == "1")
                        {
                            model.FinalColor = dttechColor.Rows[0]["colorName"].ToString();
                            model.FinalColorCode = dttechColor.Rows[0]["colorCode"].ToString();
                        }
                        else if (CYTR == "" && YTRConfirm == "1")
                        {
                            if (dttechColor.Rows[0]["colorName"].ToString().ToLower() == "amber")
                            {
                                model.FinalColor = "IR CEP Amber";
                                model.FinalColorCode = dttechColor.Rows[0]["colorCode"].ToString();
                            }
                            else if (dttechColor.Rows[0]["colorName"].ToString().ToLower() == "red")
                            {
                                model.FinalColor = "IR CEP Red";
                                model.FinalColorCode = dttechColor.Rows[0]["colorCode"].ToString();
                            }
                            else if (dttechColor.Rows[0]["colorName"].ToString().ToLower() == "green")
                            {
                                model.FinalColor = "IR CEP";
                                //model.FinalColorCode = dttechColor.Rows[0]["colorCode"].ToString();
                            }

                        }
                        else if (CYTR == "0" && YTRConfirm == "1")
                        {
                            if (dttechColor.Rows[0]["colorName"].ToString().ToLower() == "amber")
                            {
                                model.FinalColor = "IR CEP Amber";
                                model.FinalColorCode = dttechColor.Rows[0]["colorCode"].ToString();
                            }
                            else if (dttechColor.Rows[0]["colorName"].ToString().ToLower() == "red")
                            {
                                model.FinalColor = "IR CEP Red";
                                model.FinalColorCode = dttechColor.Rows[0]["colorCode"].ToString();
                            }
                            else if (dttechColor.Rows[0]["colorName"].ToString().ToLower() == "green")
                            {
                                model.FinalColor = "IR CEP";
                                //model.FinalColorCode = dttechColor.Rows[0]["colorCode"].ToString();
                            }
                        }
                        else if (closeall == "0" && YTRConfirm == "0")
                        {
                            if (dttechColor.Rows[0]["colorName"].ToString().ToLower() == "green")
                            {
                                model.FinalColor = "IR WIP";
                                //model.FinalColorCode = dttechColor.Rows[0]["colorCode"].ToString();
                            }
                            else if (dttechColor.Rows[0]["colorName"].ToString().ToLower() == "amber")
                            {
                                model.FinalColor = "IR WIP Amber";
                                model.FinalColorCode = dttechColor.Rows[0]["colorCode"].ToString();
                            }
                            else if (dttechColor.Rows[0]["colorName"].ToString().ToLower() == "red")
                            {
                                model.FinalColor = "IR WIP Red";
                                model.FinalColorCode = dttechColor.Rows[0]["colorCode"].ToString();
                            }

                        }

                    }
                    else
                    {
                        model.FinalColorCode = "#fff";
                    }
                }

            }

            #region  Personal Info 
            if (model.ClientRowID == 990)
            {
                model.PersonalChecksBriefDetails = repoReportQC.PersonalChecksBriefDetails_ReportNew_techM(pid);
            }

            #endregion

            if ((model.ClientRowID == 990) && Report_Type.ToLower() == "1")
            {
                model = repoReportQC.GetUANLTIDetail(model);
            }


            return new Rotativa.ViewAsPdf("LTISummaryReportPdf", model)
            {
                FileName = model.CompanyRefNo + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(10, 10, 10, 10),
                CustomSwitches = "--enable-local-file-access"
            };
        }
        // #region Client Review LTI
        //[SessionExpireFilterAttribute]
        //[ValidateInput(false)]
        //[ValidateSessionFilterSpokLoginInternal]
        //public ActionResult ManageClientReviewApprovedDashboardInfo(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", string cr = "", string action = "", string test = "")
        //{
        //    DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
        //    try
        //    {
        //        short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
        //        int CheckStatusId = 118;
        //        short clientid = 0;
        //        if (cr != "")
        //        {
        //            clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
        //        }

        //        if (test == "ExportToExcel")
        //        {
        //            DataTable dt = new DataTable();
        //            ClientCustomize cc = new ClientCustomize();
        //            dt = cc.getSpocReview_Dashboard_Details(clientid, CheckStatusId, teamMemerRowId);

        //            DataTable filteredDt = new DataTable();
        //            filteredDt.Columns.Add("Client Name");
        //            filteredDt.Columns.Add("SecuritasID");
        //            filteredDt.Columns.Add("Candidate Name");
        //            filteredDt.Columns.Add("Candidate Code");
        //            filteredDt.Columns.Add("ClientRefID");
        //            filteredDt.Columns.Add("CreatedDate");
        //            filteredDt.Columns.Add("Unique Check Id");
        //            filteredDt.Columns.Add("SubCheckName");
        //            filteredDt.Columns.Add("Company / University Name");
        //            filteredDt.Columns.Add("Suggestion Remarks");
        //            filteredDt.Columns.Add("Team Member Name");
        //            filteredDt.Columns.Add("Suggestion Raised Date");
        //            filteredDt.Columns.Add("Ageing");
        //            filteredDt.Columns.Add("Spoc Name");
        //            filteredDt.Columns.Add("Spoc Review Raised Date");

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                DataRow newRow = filteredDt.NewRow();
        //                newRow["Client Name"] = row["ClientName"];
        //                newRow["SecuritasID"] = row["SecuritasID"];
        //                newRow["Candidate Name"] = row["CandidateName"];
        //                newRow["Candidate Code"] = row["CandidateCode"];
        //                newRow["ClientRefID"] = row["ClientRefID"];
        //                newRow["CreatedDate"] = row["CreatedDate"];
        //                newRow["Unique Check Id"] = row["CheckUniqueID"];
        //                newRow["SubCheckName"] = row["SubCheckName"];
        //                newRow["Company / University Name"] = row["Company/UniversityName"];
        //                newRow["Suggestion Remarks"] = row["SuggestionRemarks"];
        //                newRow["Team Member Name"] = row["Team Member Name"];
        //                newRow["Suggestion Raised Date"] = row["SuggestionRaisedDate"];
        //                newRow["Ageing"] = row["Ageing"];
        //                newRow["Spoc Name"] = row["SpocName"];
        //                newRow["Spoc Review Raised Date"] = row["SpocReviewRaisedDate"];
        //                filteredDt.Rows.Add(newRow);
        //            }
        //            ExportToExcel(filteredDt, "SpocReview");
        //        }
        //        else
        //        {

        //            int pageSize = 20;
        //            string SpocEmail = Session["TeamMemberEmail"].ToString();
        //            string Designation = Session["Designation"].ToString();
        //            model = repoDashboard.GetSpocReviewDashBoardInfoAPI(page, pageSize, Search, teamMemerRowId, "118", clientid);
        //            if (action != "Search")
        //            {
        //                var onjlist = model.DashboardClientCandidates;
        //            }

        //            Session["CaseStatus"] = CaseStatus;
        //            if (model == null)
        //            {
        //                ViewBag.ErrorMsg = "No item found!";
        //            }
        //            BindClientListByLogInName(SpocEmail);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.ErrorMsg = Ex.Message;
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //    return View(model);
        //}
        //public ActionResult ManageClientReviewClearDashboardInfo(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string CaseStatus = "", string RecievingDate = "", string CompletedDate = "", string cr = "", string action = "", string test = "")
        //{
        //    DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
        //    try
        //    {
        //        short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
        //        int CheckStatusId = 119;
        //        short clientid = 0;
        //        if (cr != "")
        //        {
        //            clientid = Convert.ToInt16(clsCommonMethods.PDecrypt(cr));
        //        }

        //        if (test == "ExportToExcel")
        //        {
        //            DataTable dt = new DataTable();
        //            ClientCustomize cc = new ClientCustomize();
        //            dt = cc.getSpocReview_Dashboard_Details(clientid, CheckStatusId, teamMemerRowId);

        //            DataTable filteredDt = new DataTable();
        //            filteredDt.Columns.Add("Client Name");
        //            filteredDt.Columns.Add("SecuritasID");
        //            filteredDt.Columns.Add("Candidate Name");
        //            filteredDt.Columns.Add("Candidate Code");
        //            filteredDt.Columns.Add("ClientRefID");
        //            filteredDt.Columns.Add("CreatedDate");
        //            filteredDt.Columns.Add("Unique Check Id");
        //            filteredDt.Columns.Add("SubCheckName");
        //            filteredDt.Columns.Add("Company / University Name");
        //            filteredDt.Columns.Add("Suggestion Remarks");
        //            filteredDt.Columns.Add("Team Member Name");
        //            filteredDt.Columns.Add("Suggestion Raised Date");
        //            filteredDt.Columns.Add("Ageing");
        //            filteredDt.Columns.Add("Spoc Name");
        //            filteredDt.Columns.Add("Spoc Review Raised Date");

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                DataRow newRow = filteredDt.NewRow();
        //                newRow["Client Name"] = row["ClientName"];
        //                newRow["SecuritasID"] = row["SecuritasID"];
        //                newRow["Candidate Name"] = row["CandidateName"];
        //                newRow["Candidate Code"] = row["CandidateCode"];
        //                newRow["ClientRefID"] = row["ClientRefID"];
        //                newRow["CreatedDate"] = row["CreatedDate"];
        //                newRow["Unique Check Id"] = row["CheckUniqueID"];
        //                newRow["SubCheckName"] = row["SubCheckName"];
        //                newRow["Company / University Name"] = row["Company/UniversityName"];
        //                newRow["Suggestion Remarks"] = row["SuggestionRemarks"];
        //                newRow["Team Member Name"] = row["Team Member Name"];
        //                newRow["Suggestion Raised Date"] = row["SuggestionRaisedDate"];
        //                newRow["Ageing"] = row["Ageing"];
        //                newRow["Spoc Name"] = row["SpocName"];
        //                newRow["Spoc Review Raised Date"] = row["SpocReviewRaisedDate"];
        //                filteredDt.Rows.Add(newRow);
        //            }
        //            ExportToExcel(filteredDt, "SpocReview");
        //        }
        //        else
        //        {

        //            int pageSize = 20;
        //            string SpocEmail = Session["TeamMemberEmail"].ToString();
        //            string Designation = Session["Designation"].ToString();
        //            model = repoDashboard.GetSpocReviewDashBoardInfoAPI(page, pageSize, Search, teamMemerRowId, "119", clientid);
        //            if (action != "Search")
        //            {
        //                var onjlist = model.DashboardClientCandidates;
        //            }

        //            Session["CaseStatus"] = CaseStatus;
        //            if (model == null)
        //            {
        //                ViewBag.ErrorMsg = "No item found!";
        //            }
        //            BindClientListByLogInName(SpocEmail);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.ErrorMsg = Ex.Message;
        //        clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
        //    }
        //    return View(model);
        //}

        //#endregion

    }
}