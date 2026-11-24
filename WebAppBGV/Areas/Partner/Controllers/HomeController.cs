using BAL;
using BAL.PartnerRepository;
using BAL.ProvidedInfoRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.PartnerViewModels;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;
using System.Data;
using System.Web.UI.WebControls;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO.Compression;
using Newtonsoft.Json;
using ViewModels;

namespace WebAppBGV.Areas.Partner.Controllers
{
    [CustomAuthorizeVendor]
    public class HomeController : Controller
    {
        #region ***** Repository Object *****
        IDropDownMasterRepository repoDDL;
        IPQPersonalRepository repoPQPersonal;
        IPartnerVerificationRepository repoPartnerVer;
        IPartnerAddressVerRepository repoAddressPTR;
        IPartnerDashboardRepository repoPtrDashboard;
        IPQVerifiedUploadDocRepository repoPQVerifiedUploadDoc;

        public HomeController(IDropDownMasterRepository _repoDDL
            , IPartnerVerificationRepository _repoPartnerVer
           , IPQPersonalRepository _repoPQPersonal
            , IPartnerAddressVerRepository _repoAddressPTR
            , IPartnerDashboardRepository _repoPtrDashboard
            , IPQVerifiedUploadDocRepository _repoPQVerifiedUploadDoc
            )
        {
            repoDDL = _repoDDL;
            repoPQPersonal = _repoPQPersonal;
            repoPartnerVer = _repoPartnerVer;
            repoAddressPTR = _repoAddressPTR;
            repoPtrDashboard = _repoPtrDashboard;
            repoPQVerifiedUploadDoc = _repoPQVerifiedUploadDoc;
        }
        #endregion
        ClientCustomize cc = new ClientCustomize();

        // GET: Partner/Home
        [SessionExpireFilterAttributeVendor]
        [AuthenticateUser]
        [ValidateSessionFilterVendor]
        public ActionResult Index()
        {
            try
            {
                short VendorRowID = Convert.ToInt16(Session["VendorRowID"]);
                ViewBag.PtrCasesDashboard = repoPtrDashboard.GetPtrManagerDashBoardInfo(VendorRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View();
        }

        #region ******* Manage Dashboard *******

        [SessionExpireFilterAttributeVendor]
        [ValidateInput(false)]
        [ValidateSessionFilterVendor]
        public ActionResult GetPartnerCasesDashBoardInfo(int page = 1, string sort = "", string sortDir = "desc", string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0, string Version = "New")
        {
            DashboardPartnerCandidateListViewModel model = new DashboardPartnerCandidateListViewModel();
            try
            {
                string Statusid = "";
                int pageSize = 20;
                short VendorRowID = Convert.ToInt16(Session["VendorRowID"]);
                if (CaseStatus == "UnAllocated")
                {
                    Statusid = "27";
                }
                if (CaseStatus == "Allocated")
                {
                    Statusid = "62";
                }
                if (CaseStatus == "Revisit")
                {
                    Statusid = "63";
                }
                if (CaseStatus == "Rejected")
                {
                    Statusid = "64";
                }
                if (CaseStatus == "FundApproval")
                {
                    Statusid = "59";
                }
                if (CaseStatus == "Approvalpending")
                {
                    Statusid = "58";
                }
                if (CaseStatus == "Closed")
                {
                    Statusid = "38";
                }
                if (CaseStatus == "OUTTAT")
                {
                    Statusid = "27,62";

                }
                if (Version == "New")
                {
                    model = repoPtrDashboard.GetCandidateListForReportQCManager1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, Statusid, CaseStatus, VendorRowID, ClientRowID, RecievingDate, CompletedDate, tatHours);

                }
                else
                {
                    model = repoPtrDashboard.GetCandidateListForReportQCManager(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, CaseStatus, VendorRowID, ClientRowID, RecievingDate, CompletedDate, tatHours);
                }
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
            BindClientList();
            return View(model);
        }

        [HttpGet]
        [ValidateInput(false)]
        [SessionExpireFilterAttributeVendor]
        public ActionResult UplodAddressCheckDocsAndComplete(short scid = 0, int pe = 0, string cstatus = "", int cid = 0)
        {
            VerifiedUploadAddressDocViewModel model = new VerifiedUploadAddressDocViewModel();
            AddTempPQVerifiedUploadDocViewModel uploadDoc = new AddTempPQVerifiedUploadDocViewModel();
            //ViewBag.GetDocumentList = repoDDL.GetMobileUploaddoc(pe, scid);
            ViewBag.GetDocumentList = repoPartnerVer.GetpartnerUploadDoc(pe, scid);
            try
            {
                if (scid > 0 && pe > 0)
                {
                    // if(ViewBag.GetDocumentList.Count == 0)

                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                    model.ClientRowID = cid;
                    ViewBag.GetDocumentList = repoPartnerVer.GetpartnerUploadDoc(model.PersonalRowID, model.SubCheckRowID);
                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
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
        [SessionExpireFilterAttributeVendor]
        [ValidateAntiForgeryToken]
        public ActionResult UplodAddressCheckDocsAndComplete(VerifiedUploadAddressDocViewModel model, string Upload, string Submit)
        {
            try
            {
                List<AddTempPQVerifiedUploadDocViewModel> lmd = new List<AddTempPQVerifiedUploadDocViewModel>();
                if (!string.IsNullOrEmpty(Upload))
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    int i = 1;
                    foreach (HttpPostedFileBase file in model.UploadAddressDoc) // loop for adding add from dataset to list<modeldata>
                    {
                        if (model.DocumentType == "Others if any")
                        {
                            model.DocumentType =  model.OtherDocumentType?.Trim(); // Use the custom value
                        }
                        AddTempPQVerifiedUploadDocViewModel mm = new AddTempPQVerifiedUploadDocViewModel();

                        mm.DocumentType = model.DocumentType;
                        mm.FileName = file.FileName;
                        mm.DocumentUploadFrom = "Verified";
                        mm.Status = 1;
                        mm.UploadDate = DateTime.Now;
                        mm.PersonalRowID = model.PersonalRowID;
                        mm.SubCheckRowID = model.SubCheckRowID;
                        mm.Remarks = "Uploaded By Partner";

                        model.DocumentType = mm.DocumentType;
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        proImg = (model.SubCheckRowID + "_" + model.PersonalRowID + "_" + mm.UploadDate.Value.ToString("ddmmyyyyHH:mm:ss:f") + "_" + model.DocumentType + "" + i).Replace("'", "_").Replace("&", "_").Replace(":", "_").Replace("-", "_").Replace(" ", "_") + ext;

                        model.FileName = Path.GetFileName(proImg);
                        model.FilePath = FldPathDoc;

                        model.UploadDate = DateTime.Now.Date;
                        model.Status = 1;
                        cc.insertPartnerdoc(model.PersonalRowID, model.SubCheckRowID, model.FileName, model.FilePath, model.DocumentType);
                        file.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistoryByPartner(model.PersonalRowID, model.SubCheckRowID, "Upload Doc by PTR", "Address Verification information Upload Doc ");
                        #endregion                                        

                        ++i;

                    }

                    ViewBag.GetDocumentList = repoPartnerVer.GetpartnerUploadDoc(model.PersonalRowID, model.SubCheckRowID);

                }


                if (ModelState.IsValid && Submit != null)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    model.Remarks = "Uploaded By Partner";
                    ModelState.Clear();
                    ViewBag.SuccessMsg = "Record Added Successfully!";
                }


            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }


        #endregion

        #region ***** Address *******

        [HttpGet]
        [ValidateInput(false)]
        [SessionExpireFilterAttributeVendor]
        public ActionResult AddAddressInfoByPTRTeamMember(int id = 0, int PersonalRowID = 0, int subCheckId = 0, int ClientRowId = 0)
        {

            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Vendor", "Check");
            DataTable dbValue = new DataTable();

            foreach (DataRow d in dt.Rows)
            {


                if (d[4].ToString() == "textbox")
                {
                    if (d[0].ToString() == "30")
                    {
                        DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        if (dtperinfo.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString(), dtperinfo.Rows[0]["CandidateName"]);

                        }
                    }
                    else if (d[0].ToString() == "27")
                    {
                        DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        if (dtperinfo.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString(), dtperinfo.Rows[0]["Address"]);

                        }
                    }
                    else if (d[0].ToString() == "42")
                    {
                        DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        if (dtperinfo.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString(), dtperinfo.Rows[0]["Pincode"]);

                        }
                    }
                    else if (d[0].ToString() == "36")
                    {
                        //DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        //if (dtperinfo.Rows.Count > 0)
                        //{
                        //    marksModel.Add(d[1].ToString(), dtperinfo.Rows[0]["DurationOfStay"]);

                        //}

                        DataTable dataTable = cc.GetOPSValueForDurationOfStay(PersonalRowID, subCheckId);
                        if (dataTable.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString(), dataTable.Rows[0]["DurationOfStay"]);

                        }
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        marksModel.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                    }

                    else if (d[0].ToString() == "40")
                    {
                        DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        if (dtperinfo.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString(), dtperinfo.Rows[0]["Landmark"]);

                        }
                    }
                    else if (d[0].ToString() == "180")
                    {
                        DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        if (dtperinfo.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString(), dtperinfo.Rows[0]["RespondentName"]);

                        }
                    }
                    else if (d[0].ToString() == "181")
                    {
                        DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        if (dtperinfo.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString(), dtperinfo.Rows[0]["RespondentContact"]);

                        }
                    }
                    else if (d[0].ToString() == "182")
                    {
                        DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        if (dtperinfo.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString(), dtperinfo.Rows[0]["RespondentRelationship"]);

                        }
                    }
                    else if (d[0].ToString() == "170")
                    {
                        DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        if (dtperinfo.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString(), dtperinfo.Rows[0]["DateOfVerification"]);

                        }
                    }
                    else if (d[0].ToString() == "135")
                    {
                        DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        if (dtperinfo.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString(), dtperinfo.Rows[0]["AnyOtherComments"]);

                        }
                    }
                    else
                    {
                        marksModel.Add(d[1].ToString(), "");
                    }


                }
                else if (d[4].ToString() == "dropdown")
                {
                    if (d[0].ToString() == "26")
                    {
                        DataTable dtperinfo = cc.GetDEAddressDetail(PersonalRowID, subCheckId);
                        if (dtperinfo.Rows.Count > 0)
                        {
                            marksModel.Add(d[1].ToString() + "_" + dtperinfo.Rows[0]["AccommodationType"].ToString() + "_" + d[3].ToString(), dtperinfo.Rows[0]["AccommodationType"].ToString());

                        }
                    }
                }
                else if (d[4].ToString() == "checkbox")
                {
                    marksModel.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                }
                else
                    marksModel.Add(d[1].ToString(), d[0].ToString());

            }


            ViewBag.PersonalRowid = PersonalRowID.ToString();
            ViewBag.SubCheckId = subCheckId.ToString();
            ViewBag.clientRowId = ClientRowId.ToString();
            //GetEmploymentList();
            return View(marksModel);


        }

        [HttpPost]
        [ValidateInput(false)]
        [SessionExpireFilterAttributeVendor]
        [ValidateAntiForgeryToken]
        public ActionResult AddAddressInfoByPTRTeamMember(FormCollection formCollection)
        {
            dynamic expando = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            int SubCheckid = Convert.ToInt32(formCollection["SubCheckId"].ToString());
            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());

            string checkstatus = formCollection["CheckStatus"].ToString();
            var marksModel = expando as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPTM";

                ClientCustomize bl = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Vendor", "Check");
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
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
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
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                //marksModel.Add(d[1].ToString(), formCollection[key].ToString());                            
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
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
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());

                }

                string CheckStatus = "";
                if (checkstatus == "Re-Visit")
                {

                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 63, "Check", -1);

                }
                else if (checkstatus == "Not-Doable")
                {
                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 72, "Check", -1);

                }
                else if (checkstatus == "Completed")
                {
                    cc.UpdateCheckStatus(PersonalRowid, SubCheckid, 1, 38, "Check", -1);
                    new API().UpdateAPICheckStatus(clientRowid.ToString().Trim(), PersonalRowid, SubCheckid, "CompletedByPartner"); //APICheckStatus

                }


                #region Check Action History Save                           
                clsCommonMethods.AddCheckActionHistoryByPartner(PersonalRowid, Convert.ToInt16(SubCheckid), checkstatus + " by PTR", checkstatus);
                #endregion

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckId = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = formCollection["clientRowId"].ToString();
            return View(marksModel);
        }

        [NonAction]
        void AddressVerList(int id = 0, short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                //Bind AddressVer Details 
                ViewBag.modelBind = repoAddressPTR.GetAddressForPTRTM(id);

                ViewBag.ListAddressVer = repoDDL.GetAntecedentVerList(ClientRowId, ChFamilRowId, subCheckId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        #endregion

        #region ***** Partner User Allocation Checks *******

        [HttpGet]
        [ValidateInput(false)]
        [SessionExpireFilterAttributeVendor]
        [ValidateSessionFilterVendor]
        public ActionResult ManagePartnerAllocatedChecks(int page = 1, string sort = "", string sortDir = "desc", string Search = "", short ClientRowID = 0, short StateRowId = 0, string CheckStatus = "", string Version = "New", string CaseStatus = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            short VendorRowID = Convert.ToInt16(Session["VendorRowID"]);
            try
            {
                string Statusid = "";
                int pageSize = 20;
                if (CaseStatus == "Rejected")
                {
                    Statusid = "64";
                }
                else if (CaseStatus == "UserAllocated")
                {
                    Statusid = "62";
                }

                else if (CaseStatus == "FieldPartnerClosed")
                {
                    Statusid = "66";
                }
                if (Version == "New")
                {
                    model = repoPartnerVer.GetPartnerAllocatedChecksForManager1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, Statusid, CaseStatus, VendorRowID, StateRowId, ClientRowID);
                }
                else
                {
                    model = repoPartnerVer.GetPartnerAllocatedChecksForManager(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, VendorRowID, StateRowId);
                }


            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            GetStatesByVendorIdDDL(VendorRowID);
            GetVendorList(VendorRowID);
            return View(model);
        }

        [HttpGet]
        [ValidateInput(false)]
        [SessionExpireFilterAttributeVendor]
        public ActionResult ManagePartnerReScynChecks(int page = 1, string sort = "", string sortDir = "desc", string Search = "", int VendorUserRowID = 0, string Resync = "")
        {
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            short VendorRowID = Convert.ToInt16(Session["VendorRowID"]);
            try
            {
                int pageSize = 20;
                if (VendorUserRowID > 0 && string.IsNullOrEmpty(Resync))
                {
                    model = repoPartnerVer.GetPartnerAllocatedChecksForReSync(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, VendorRowID, VendorUserRowID);
                }
                if (!string.IsNullOrEmpty(Resync) && VendorUserRowID > 0)
                {
                    bool res = repoPartnerVer.PartnerAllocatedChecksForReSync(VendorUserRowID);
                    if (res)
                    {
                        repoPartnerVer.SaveChanges();
                        ViewBag.ErrorMsg = "Re-Sync successfully!";
                    }
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            GetVendorIdByUserDDL(VendorRowID);
            return View(model);
        }


        [HttpGet]
        [ValidateInput(false)]
        [SessionExpireFilterAttributeVendor]
        [ValidateSessionFilterVendor]
        public ActionResult AllocationToPatenerUser(int page = 1, string sort = "", string sortDir = "desc", string Search = "", short ClientRowID = 0, short StateRowId = 0, string CheckStatus = "", string VerificationIds = "", int VendorLoginRowID = 0, string version = "New", string CaseStatus = "")
        {
            UpdateAddressVerPartnerViewModel model1 = new UpdateAddressVerPartnerViewModel();
            VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
            int partnerid = VendorLoginRowID;
            short VendorRowID = Convert.ToInt16(Session["VendorRowID"]);
            short VendorLoginRowId = Convert.ToInt16(Session["VendorLoginRowID"]);
            string vendorName = Session["VendorName"].ToString();

            try
            {
                string Statusid = "";
                int pageSize = 20;
                if (CaseStatus == "ReAllocation")
                {
                    Statusid = "72";
                }
                else if (CaseStatus == "UnAllocated")
                {
                    Statusid = "27";
                }
                else
                {
                    Statusid = "27";
                }
                if (!string.IsNullOrEmpty(VerificationIds))
                {
                    if (VendorLoginRowID > 0)
                    {
                        if (CaseStatus == "ReAllocation")
                        {
                            repoPartnerVer.PartnerReAllocation(VerificationIds, VendorLoginRowID, VendorRowID);
                        }
                        else
                        {
                            repoPartnerVer.GetAddressInfoForPtrUser(VerificationIds, VendorLoginRowID, VendorRowID, vendorName, partnerid);
                        }


                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Select Vendor User!!";
                    }
                }
                if (version == "New")
                {
                    model = repoPartnerVer.GetPartnerUser1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, VendorRowID, ClientRowID, CheckStatus, VerificationIds, StateRowId, Statusid, CaseStatus);
                }
                else
                {
                    //model = repoPartnerVer.GetPartnerUser(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, VendorRowID, ClientRowID, CheckStatus, VerificationIds, StateRowId);
                    model = repoPartnerVer.GetPartnerUser1(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, VendorRowID, ClientRowID, CheckStatus, VerificationIds, StateRowId, Statusid);

                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            GetStatesByVendorIdDDL(VendorRowID);
            GetVendorIdByUserDDL(VendorRowID);
            return View(model);
        }

        #endregion

        #region ***** Partner User Allocated Checks *******

        [HttpGet]
        [ValidateInput(false)]
        [SessionExpireFilterAttributeVendor]
        public ActionResult ManagePartnerUserAllocatedChecks(int page = 1, string sort = "", string sortDir = "desc", string Search = "")
        {
            VerificationPartenerListPagedModel model = new VerificationPartenerListPagedModel();
            short VendorLoginRowID = Convert.ToInt16(Session["VendorLoginRowID"]);
            try
            {
                int pageSize = 20;

                model = repoPartnerVer.GetPartnerUserAllocatedChecks(page, pageSize, sort.ToLower(), sortDir.ToLower(), Search, VendorLoginRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return View(model);
        }

        [HttpGet]
        public JsonResult CompletedByPRTMgr(short id = 0, int pid = 0, short subid = 0)
        {
            bool result = false;
            try
            {
                if (id > 0)
                {
                    repoAddressPTR.UpdateAddressCheckStatusByPRTMgr(id);

                    #region Save LogTransaction Details
                    clsCommonMethods.AddPQLogTrasactionByPartner(pid, "CompletedByPRTMgr", "Completed", "", "Update By Partner");
                    #endregion

                    #region Case/Check Action History Save 
                    clsCommonMethods.AddCheckActionHistoryByPartner(pid, subid, "Completed", "Completed by Partner");
                    #endregion
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

        #endregion

        #region *******No Action*****

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

        [NonAction]
        void TotalNumber()
        {
            try
            {
                IEnumerable<SelectListItem> Number = new List<SelectListItem>
            {
                new SelectListItem { Text = "1" , Value = "1" },
                new SelectListItem { Text = "2" , Value = "2" },
                new SelectListItem { Text = "3" , Value = "3" },
                new SelectListItem { Text = "4" , Value = "4" },
                new SelectListItem { Text = "5" , Value = "5" },
                new SelectListItem { Text = "6" , Value = "6" },
                new SelectListItem { Text = "7" , Value = "7" },
                new SelectListItem { Text = "8" , Value = "8" },
                new SelectListItem { Text = "9" , Value = "9" },
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
            };
                ViewBag.TotalNumberes = Number;
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }
        [NonAction]
        void CheckStatus()
        {
            try
            {
                IEnumerable<SelectListItem> CheckStatus = new List<SelectListItem>
            {
                new SelectListItem { Text = "Completed" , Value = "Completed" },
                new SelectListItem { Text = "Re-Visit" , Value = "Re-Visit" },
                new SelectListItem { Text = "Not-Doable" , Value = "Not-Doable" },

            };
                ViewBag.CheckStatusLst = CheckStatus;
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

        public void GetStatesByVendorIdDDL(short VendorRowID = 0)
        {
            try
            {
                ViewBag.VendorStatesList = repoDDL.GetStatesByVendorDDL(VendorRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        public void GetVendorIdByUserDDL(short VendorRowID = 0)
        {
            try
            {
                ViewBag.VendorUserList = repoDDL.GetVendorByUserDDL(VendorRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }


        [SessionExpireFilterAttributeVendor]
        [HttpGet]
        public ActionResult BindCheckActionHistory(int PersonalRowID = 0, short subCheckId = 0)
        {
            try
            {
                ViewBag.CheckActionHistories = repoDDL.GetCheckActionHistory(PersonalRowID, subCheckId);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return PartialView("_CheckActionHistories");
        }

        [SessionExpireFilterAttributeVendor]
        [HttpGet]
        public ActionResult CaseActionHistories(int pid = 0)
        {
            try
            {
                ViewBag.CaseActionHistories = repoDDL.GetCaseActionHistories(pid);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return PartialView("_CaseActionHistories");
        }

        #endregion

        public void ExportChecksReport(string CaseStatus)
        {
            int CheckStatusid = 0;
            if (CaseStatus == "UserAllocated")
            {
                CheckStatusid = 62;
            }
            else if (CaseStatus == "FieldPartnerClosed")
            {
                CheckStatusid = 66;
            }
            DataTable dt = new DataTable();
            //string department = "";
            dt = cc.ExportToExcelPartner(CheckStatusid, Convert.ToInt16(Session["VendorRowID"]));
            ExportToExcel(dt, "Partner Allocated");

        }
        public void ExportChecksReportForNonallocated(string CaseStatus)
        {
            CaseStatus = Convert.ToString(Session["CaseStatus"]);
            DataTable dt = new DataTable();
            string department = "";
            dt = cc.ExportToExcelPartner(27, Convert.ToInt16(Session["VendorRowID"]));
            ExportToExcel(dt, "PartnerNonAllocated");

        }
        public void ExportToExcel(DataTable dt, string strFileName)
        {
            if (dt.Rows.Count > 0)
            {

                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName + ".xls");


                Response.Write(tw.ToString());
                Response.End();
            }
        }
        [SessionExpireFilterAttributeVendor]
        public FileResult GetSubNotMatchMail(string ImageName)
        {
            string ToPath = "";
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MobileImageUpload"].ToString()))
            {
                ToPath = System.Configuration.ConfigurationManager.AppSettings["MobileImageUpload"].ToString();
            }

            string ftppath = "";
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ftppathMobiledoc"].ToString()))
            {
                ftppath = ConfigurationManager.AppSettings["ftppathMobiledoc"].ToString();
            }

            byte[] fileBytes = new byte[] { };

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, ImageName);
        }




        public void writeOnImage(string firstText, string secondText, string fromimagePath, string toImagePath, string filename)
        {
            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(fromimagePath + filename);//load the image file
            int hg = bitmap.Height;
            PointF firstLocation = new PointF(100, hg - (hg / 16));
            PointF secondLocation = new PointF(100, hg - (hg / 25));
            if (firstText != "00.000")
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font arialFont = new Font("Arial", 50))
                    {
                        graphics.DrawString(firstText, arialFont, Brushes.Red, firstLocation);
                        graphics.DrawString(secondText, arialFont, Brushes.Red, secondLocation);
                    }
                }
            }
            using (MemoryStream memory = new MemoryStream())
            {

                using (FileStream fs = new FileStream(Server.MapPath("~\\Documents\\CandidateDocuments\\ProvidedDocuments\\") + filename, FileMode.Create, FileAccess.ReadWrite))
                {
                    bitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

        }

        [HttpGet]
        public ActionResult delete(int personalRowId, int subCheckId, string gtr, string doctype, string fileName, int verifiedUploadRowID)
        {
            ClientCustomize cc = new ClientCustomize();
            cc.deleteUploadDoc(personalRowId, subCheckId, doctype, fileName, verifiedUploadRowID);
            return Redirect(gtr);
        }

        [HttpGet]
        public ActionResult MobiledeleteDoc(int personalRowId, int subCheckId, string gtr, string doctype, string fileName)
        {
            ClientCustomize cc = new ClientCustomize();
            cc.deleteMobileUploadDoc(personalRowId, subCheckId, doctype, fileName);
            return Redirect(gtr);
        }

        [SessionExpireFilterAttributeVendor]
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddEducationInfoByPTR(int id = 0, bool Aging = false, string msg = "", int PersonalRowID = 0, short subCheckId = 0, int ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            ClientCustomize cc = new ClientCustomize();

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");
            DataTable Dtstatus = cc.getCheckStatus(PersonalRowID, subCheckId);
            int checkStatusid = 0;
            if (Dtstatus.Rows.Count > 0)
            {
                checkStatusid = Convert.ToInt32(Dtstatus.Rows[0]["CheckStatusID"]);
                ClientRowId = Convert.ToInt32(Dtstatus.Rows[0]["ClientRowID"]);
            }

            DataTable dbValue = new DataTable();
            DataTable dtOpd = new DataTable();
            dtOpd = cc.GetOpsTeamValue(PersonalRowID, subCheckId);
            if (dtOpd.Rows.Count > 0)
            {
                dbValue = cc.GetOperationsValue(PersonalRowID, subCheckId);
            }
            else
            {
                dbValue = cc.GetDataEntryQCValue(PersonalRowID, subCheckId);
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
            ViewBag.checkStatusid = checkStatusid.ToString();
            ViewBag.CheckStatus = "WIP";
            ViewBag.CandCheckRowID = id.ToString();
            CheckStatus();
            ViewBag.ErrorMsg = " ";
            if (ViewBag.degreeName == null)
            {
                ViewBag.degreeName = "";
            }
            if (Session["Mode"] == null)
            {
                Session["Mode"] = "0";
            }
            return View(marksModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddEducationInfoByPTR(FormCollection formCollection, string action = "")
        {
            dynamic expando = new ExpandoObject();
            dynamic expando1 = new ExpandoObject();
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());
            string DegreeName = formCollection["HfdegreeName"].ToString().Replace(",", "");
            string checkstatus = Convert.ToString(formCollection["CheckStatus"]);
            string otherInstituteName = "";
            DataTable Dtstatus = cc.getCheckStatus(PersonalRowid, SubCheckid);
            int checkStatusid = 0;
            if (Dtstatus.Rows.Count > 0)
            {
                checkStatusid = Convert.ToInt32(Dtstatus.Rows[0]["CheckStatusID"]);
                clientRowid = Convert.ToInt32(Dtstatus.Rows[0]["ClientRowID"]);
            }

            DataTable dt = new DataTable();

            ViewBag.ErrorMsg = "";

            var marksModel = expando as IDictionary<string, object>;
            var marksModel1 = expando1 as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPTM";
                ClientCustomize bl = new ClientCustomize();
                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Operation", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');
                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "<br>" + cd.DisplayName + " is a mandatory field";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "<br>" + cd.DisplayName + " is a mandatory field";
                                }

                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "<br>" + cd.DisplayName + " is a mandatory field";
                                }
                            }
                        }
                    }

                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
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
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                if (cd.DisplayName == "Other Institute Details")
                                    otherInstituteName = cd.DataEntryValue;
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                            }
                            if (key == "CollegeName" && d[0].ToString() == "69")
                            {
                                cd.AntecedentRowID = 69;
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
                                marksModel.Add("CollegeName_College / Institute / School Name_textbox_mandatory_", formCollection["CollegeName"].ToString());
                            }
                            if (key == "CollegeAddress" && d[0].ToString() == "70")
                            {
                                cd.AntecedentRowID = 70;
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                marksModel.Add("CollegeAddress_College / University / Institute / School Address_textbox_mandatory_", formCollection["CollegeAddress"].ToString());
                            }
                        }
                    }
                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                if (cd.AntecedentRowID == 72)
                                {
                                    cd.DataEntryValue = DegreeName;
                                    ViewBag.degreeName = DegreeName;
                                }
                                else
                                {
                                    cd.DataEntryValue = formCollection[key].ToString();
                                }
                                bl.tblDataEntry_OperationInsert(cd);

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
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }
                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckId = formCollection["SubCheckId"].ToString();
            ViewBag.clientRowId = clientRowid.ToString();

            ViewBag.checkStatusid = checkStatusid.ToString();
            ViewBag.CheckStatus = "WIP";
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                DataTable dbValue = new DataTable();
                DataTable dtOpd = new DataTable();
                dtOpd = cc.GetOpsTeamValue(PersonalRowid, SubCheckid);
                if (dtOpd.Rows.Count > 0)
                {
                    dbValue = cc.GetOperationsValue(PersonalRowid, SubCheckid);
                }
                else
                {
                    dbValue = cc.GetDataEntryQCValue(PersonalRowid, SubCheckid);
                }

                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel1.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                            if (d[1].ToString().Contains("72"))
                            {
                                string a = rowsFiltered[0][1].ToString();
                                string[] nm = rowsFiltered[0][1].ToString().Split('_');
                                if (nm[1].ToString() == "")
                                {
                                    nm[1] = nm[0].ToString();
                                }
                                string degreename = nm[1].ToString();
                                if (degreename != "")
                                {
                                    ViewBag.degreeName = degreename.Trim();
                                }
                                else
                                {
                                    ViewBag.degreeName = null;
                                }
                            }
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel1.Add(d[1].ToString(), d[0].ToString().ToString());

                    }
                    else
                    {

                        if (d[4].ToString() == "textbox")
                        {
                            marksModel1.Add(d[1].ToString(), "");
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                        }
                        else
                            marksModel1.Add(d[1].ToString(), d[0].ToString());
                    }
                }

                if (checkstatus == "Re-Visit")
                {
                    cc.UpdateCheckStatusPV(PersonalRowid, SubCheckid, 1, 63, "Check", -1);
                }
                else if (checkstatus == "Not-Doable")
                {
                    cc.UpdateCheckStatusPV(PersonalRowid, SubCheckid, 1, 72, "Check", -1);
                }
                else if (checkstatus == "Completed")
                {
                    cc.UpdateCheckStatusPV(PersonalRowid, SubCheckid, 1, 38, "Check", -1);
                    // new API().UpdateAPICheckStatus(clientRowid.ToString().Trim(), PersonalRowid, SubCheckid, "CompletedByPartner"); //APICheckStatus
                }
                CheckStatus();
                return View(marksModel1);
            }
            else
            {
                DataTable dbValue = new DataTable();
                DataTable dtOpd = new DataTable();
                dtOpd = cc.GetOpsTeamValue(PersonalRowid, SubCheckid);
                if (dtOpd.Rows.Count > 0)
                {
                    dbValue = cc.GetOperationsValue(PersonalRowid, SubCheckid);
                }
                else
                {
                    dbValue = cc.GetDataEntryQCValue(PersonalRowid, SubCheckid);
                }

                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel1.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                            if (d[1].ToString().Contains("72"))
                            {
                                string a = rowsFiltered[0][1].ToString();
                                string[] nm = rowsFiltered[0][1].ToString().Split('_');
                                string degreename = nm[1].ToString();
                                if (degreename != "")
                                {
                                    ViewBag.degreeName = degreename.Trim();
                                }
                                else
                                {
                                    ViewBag.degreeName = null;
                                }
                            }
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel1.Add(d[1].ToString(), d[0].ToString().ToString());
                    }
                    else
                    {

                        if (d[4].ToString() == "textbox")
                        {
                            marksModel1.Add(d[1].ToString(), "");
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                        }
                        else
                            marksModel1.Add(d[1].ToString(), d[0].ToString());
                    }
                }
                if (DegreeName == "")
                {
                    ViewBag.degreeName = "";
                }
                CheckStatus();
                return View(marksModel1);
            }
        }

        [NonAction]
        public int GetNoOfPageInPDFFile(HttpPostedFileBase file)
        {
            int numberOfPages = 1;
            return numberOfPages;
        }

        [NonAction]
        public string GatZipFileMessage(HttpPostedFileBase postedFile, string clientRowid, string filenm)
        {
            string NowTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            DAL.GetMimeTypeTrue MimeType = new DAL.GetMimeTypeTrue();
            string ErrorMessage = "";
            string StartupPath = Server.MapPath("~/Uploads/TempFile/");
            StartupPath += clientRowid + "_" + filenm + "_" + NowTime;
            string TempPath = Server.MapPath("~/Uploads/");
            if (!Directory.Exists(StartupPath + "/"))
            {
                Directory.CreateDirectory(StartupPath);
            }
            ViewModels.ValidateParentFileUploadedByCandidate obj = new ViewModels.ValidateParentFileUploadedByCandidate();
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
                        ErrorMessage = "Only,.doc,.xls,.pdf,Image,MP3 and Zip File allowed";
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
                                        ViewModels.ValidateChildFileUploadedByCandidate obj1 = new ViewModels.ValidateChildFileUploadedByCandidate();
                                        string[] Extension = item.Name.ToString().Split('.');
                                        string filePath = StartupPath + "/" + item.FullName;
                                        if (Extension.Length == 2 || Extension.Length == 3)
                                        {
                                            bytes = System.IO.File.ReadAllBytes(filePath);
                                            if (MimeType.GetMimeType(bytes, filePath) == false)
                                            {
                                                ErrorMessage = "Only,.doc,.pdf,Image,MP3 files allowed in Zip File ";
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

        [SessionExpireFilterAttributeVendor]
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddEmploymentInfoByPTR(int PersonalRowID = 0, short subCheckId = 0, int ClientRowId = 0)
        {
            dynamic expando = new ExpandoObject();
            var marksModel = expando as IDictionary<string, object>;
            ViewBag.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(PersonalRowID, subCheckId);
            ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowID, subCheckId);

            ClientCustomize cc = new ClientCustomize();
            short ChFamilRowId = ViewBag.PersonalBind.CheckFamilyRowID;
            int tmid = Convert.ToInt32(Session["TeamMemberRowID"]);
            if (ChFamilRowId == 12 && ClientRowId == 607)
            {
                DataTable dataTable = cc.GetConxDOJ(PersonalRowID);
                ViewBag.DOJ = dataTable.Rows[0]["DateofJoining"].ToString();
                ViewBag.ConxRow = ClientRowId;
            }

            DataTable dt = new DataTable();
            dt = cc.Antecedents_client_select_form(ClientRowId, subCheckId, "Operation", "Check");

            DataTable Dtstatus = cc.getCheckStatus(PersonalRowID, subCheckId);
            int checkStatusid = 0;
            if (Dtstatus.Rows.Count > 0)
            {
                checkStatusid = Convert.ToInt32(Dtstatus.Rows[0]["CheckStatusID"]);
                ClientRowId = Convert.ToInt32(Dtstatus.Rows[0]["ClientRowID"]);
            }

            DataTable dbValue = new DataTable();
            // dbValue = cc.GetReportValue(PersonalRowID, subCheckId);
            DataTable dtOpd = new DataTable();
            dtOpd = cc.GetOpsTeamValue(PersonalRowID, subCheckId);
            if (dtOpd.Rows.Count > 0)
            {
                dbValue = cc.GetOperationsValue(PersonalRowID, subCheckId);
            }
            else
            {
                dbValue = cc.GetDataEntryQCValue(PersonalRowID, subCheckId);
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
            ViewBag.CheckStatus = "WIP";
            ViewBag.checkStatusid = checkStatusid.ToString();
            CheckStatus();
            ViewBag.ErrorMsg = " ";
            if (Session["Mode"] == null)
            {
                Session["Mode"] = "0";
            }
            //GetEmploymentList();
            return View(marksModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmploymentInfoByPTR(FormCollection formCollection, string action = "")
        {
            dynamic expando = new ExpandoObject();
            dynamic expando1 = new ExpandoObject();
            bool IsPhysicalVerification = false;
            int PersonalRowid = Convert.ToInt32(formCollection["PersonalRowid"].ToString());
            short SubCheckid = Convert.ToInt16(formCollection["SubCheckId"].ToString());
            string checkstatus = formCollection["CheckStatus"].ToString();
            string[] Antecedent_Id = { "92", "81", "353", "354", "355", "356", "357", "358", "359", "360" };
            int clientRowid = Convert.ToInt32(formCollection["clientRowId"].ToString());
            ViewBag.UploadedDocInfo = repoPQVerifiedUploadDoc.GetCheckUploadDocList(PersonalRowid, SubCheckid);
            ViewBag.PersonalBind = repoPQPersonal.GetPersonalDetailsForTM(PersonalRowid, SubCheckid);
            ViewBag.ErrorMsg = "";
            // string Designation = Session["Designation"].ToString();
            DataTable Dtstatus = cc.getCheckStatus(PersonalRowid, SubCheckid);
            int checkStatusid = 0;
            if (Dtstatus.Rows.Count > 0)
            {
                checkStatusid = Convert.ToInt32(Dtstatus.Rows[0]["CheckStatusID"]);
                clientRowid = Convert.ToInt32(Dtstatus.Rows[0]["ClientRowID"]);
            }
            DataTable dt = new DataTable();

            var marksModel = expando as IDictionary<string, object>;
            var marksModel1 = expando1 as IDictionary<string, object>;
            try
            {
                OperationViewModel cd = new OperationViewModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckid;
                cd.clientRowId = clientRowid;
                cd.CreatedBy = Convert.ToInt16(Session["TeamMemberRowID"]);
                cd.moduleName = "OPTM";

                ClientCustomize bl = new ClientCustomize();

                dt = bl.Antecedents_client_select_form(clientRowid, SubCheckid, "Operation", "Check");
                foreach (DataRow d in dt.Rows)
                {
                    string[] FString = d[1].ToString().Split('_');
                    foreach (string key in formCollection.AllKeys)
                    {
                        if (key == d[0].ToString())
                        {
                            cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                            cd.DisplayName = FString[1].Trim();
                            cd.DataEntryValue = formCollection[key].ToString();
                            if (d[4].ToString() == "textbox")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "<br>" + cd.DisplayName + " is a mandatory field";
                                }
                            }
                            if (d[4].ToString() == "dropdown")
                            {
                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "--Select--")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "<br>" + cd.DisplayName + " is a mandatory field";
                                }

                                if (FString[3].Trim().ToLower() == "mandatory" && cd.DataEntryValue == "")
                                {
                                    ViewBag.ErrorMsg = ViewBag.ErrorMsg + "<br>" + cd.DisplayName + " is a mandatory field";
                                }
                            }
                        }
                    }
                    if (d[4].ToString() == "fileUpload")
                    {

                        foreach (string filenm in Request.Files)
                        {
                            if (filenm == d[0].ToString())
                            {
                                HttpPostedFileBase postedFile = Request.Files[filenm];
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                if (postedFile.ContentLength > 0)
                                {
                                    string filename = filenm + "_" + postedFile.FileName;
                                    postedFile.SaveAs(path + Path.GetFileName(filename));
                                    cd.AntecedentRowID = Int64.Parse(filenm);
                                    cd.DisplayName = FString[1];
                                    cd.DataEntryValue = filename;
                                    bl.tblDataEntry_OperationInsert(cd);
                                }
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
                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);

                                if (clientRowid == 860)
                                {
                                    try
                                    {
                                        if (Antecedent_Id.Contains(d[0].ToString()))
                                        {
                                            cd.reportcomment = formCollection[key + "_1"].ToString();
                                            bl.UpdateWiproComment(cd);
                                        }

                                    }
                                    catch
                                    { }

                                }
                            }
                        }
                    }

                    else if (d[4].ToString() == "dropdown")
                    {
                        foreach (string key in formCollection.AllKeys)
                        {
                            if (key == d[0].ToString())
                            {
                                marksModel.Add(d[1].ToString() + "_" + formCollection[key].ToString() + "_" + d[3].ToString(), d[0].ToString());

                                cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                                cd.DisplayName = FString[1];
                                cd.DataEntryValue = formCollection[key].ToString();
                                bl.tblDataEntry_OperationInsert(cd);
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
                                cd.DataEntryValue = "TRUE";
                            }
                        }
                        if (chkselect == 0)
                        {
                            marksModel.Add(d[1].ToString() + "_FALSE", d[0].ToString());
                            cd.DataEntryValue = "FALSE";
                        }

                        cd.AntecedentRowID = Int64.Parse(d[0].ToString());
                        cd.DisplayName = FString[1];

                        bl.tblDataEntry_OperationInsert(cd);
                    }

                    else
                        marksModel.Add(d[1].ToString(), d[0].ToString());
                }
            }
            catch (Exception Ex)
            {

                ViewBag.ErrorMsg = "Invalid parameter value";
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            ViewBag.PersonalRowid = formCollection["PersonalRowid"].ToString();
            ViewBag.SubCheckId = formCollection["SubCheckId"].ToString();

            ViewBag.clientRowId = clientRowid.ToString();

            ViewBag.checkStatusid = checkStatusid.ToString();
            ViewBag.CheckStatus = "WIP";
            if (ViewBag.ErrorMsg == "")
            {
                ViewBag.Success = "Information Updated Successfully";
                DataTable dbValue = new DataTable();
                DataTable dtOpd = new DataTable();
                dtOpd = cc.GetOpsTeamValue(PersonalRowid, SubCheckid);
                if (dtOpd.Rows.Count > 0)
                {
                    dbValue = cc.GetOperationsValue(PersonalRowid, SubCheckid);
                }
                else
                {
                    dbValue = cc.GetDataEntryQCValue(PersonalRowid, SubCheckid);
                }
                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel1.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                            if (d[1].ToString().Contains("72"))
                            {
                                string a = rowsFiltered[0][1].ToString();
                                string[] nm = rowsFiltered[0][1].ToString().Split('_');
                                string degreename = nm[1].ToString();
                                if (degreename != "")
                                {
                                    ViewBag.degreeName = degreename.Trim();
                                }
                                else
                                {
                                    ViewBag.degreeName = null;
                                }

                            }
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel1.Add(d[1].ToString(), d[0].ToString().ToString());

                    }
                    else
                    {

                        if (d[4].ToString() == "textbox")
                        {
                            marksModel1.Add(d[1].ToString(), "");
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                        }
                        else
                            marksModel1.Add(d[1].ToString(), d[0].ToString());
                    }
                }

                if (checkstatus == "Re-Visit")
                {
                    cc.UpdateCheckStatusPV(PersonalRowid, SubCheckid, 1, 63, "Check", -1);
                }
                else if (checkstatus == "Not-Doable")
                {
                    cc.UpdateCheckStatusPV(PersonalRowid, SubCheckid, 1, 72, "Check", -1);
                }
                else if (checkstatus == "Completed")
                {
                    cc.UpdateCheckStatusPV(PersonalRowid, SubCheckid, 1, 38, "Check", -1);
                    // new API().UpdateAPICheckStatus(clientRowid.ToString().Trim(), PersonalRowid, SubCheckid, "CompletedByPartner"); //APICheckStatus
                }

                CheckStatus();


                return View(marksModel1);
            }
            else
            {
                DataTable dbValue = new DataTable();
                DataTable dtOpd = new DataTable();
                dtOpd = cc.GetOpsTeamValue(PersonalRowid, SubCheckid);
                if (dtOpd.Rows.Count > 0)
                {
                    dbValue = cc.GetOperationsValue(PersonalRowid, SubCheckid);
                }
                else
                {
                    dbValue = cc.GetDataEntryQCValue(PersonalRowid, SubCheckid);
                }
                foreach (DataRow d in dt.Rows)
                {
                    DataRow[] rowsFiltered = dbValue.Select("AntecedentRowID=" + d[0].ToString());
                    if (rowsFiltered.Length > 0)
                    {
                        if (d[4].ToString() == "textbox")
                        {
                            marksModel1.Add(d[1].ToString(), rowsFiltered[0][1]);
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString() + "_" + d[3].ToString(), rowsFiltered[0][1].ToString());
                            if (d[1].ToString().Contains("72"))
                            {
                                string a = rowsFiltered[0][1].ToString();
                                string[] nm = rowsFiltered[0][1].ToString().Split('_');
                                string degreename = nm[1].ToString();
                                if (degreename != "")
                                {
                                    ViewBag.degreeName = degreename.Trim();
                                }
                                else
                                {
                                    ViewBag.degreeName = null;
                                }
                            }
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + rowsFiltered[0][1].ToString(), rowsFiltered[0][1].ToString());
                        }
                        else if (d[4].ToString() == "fileUpload")
                            marksModel1.Add(d[1].ToString(), d[0].ToString().ToString());

                    }
                    else
                    {

                        if (d[4].ToString() == "textbox")
                        {
                            marksModel1.Add(d[1].ToString(), "");
                        }
                        else if (d[4].ToString() == "dropdown")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + d[2].ToString() + "_" + d[3].ToString(), d[0].ToString());
                        }
                        else if (d[4].ToString() == "checkbox")
                        {
                            marksModel1.Add(d[1].ToString() + "_" + d[2].ToString(), d[0].ToString());
                        }
                        else
                            marksModel1.Add(d[1].ToString(), d[0].ToString());
                    }
                }
                Session["Mode"] = 0;
                CheckStatus();
                return View(marksModel1);
            }
        }


        [HttpGet]
        [SessionExpireFilterAttributeVendor]
        [ValidateInput(false)]
        public ActionResult UplodEmploymentVerDocs(short scid = 0, int pe = 0, string cstatus = "")
        {
            VerifiedUploadDocPTRViewModel model = new VerifiedUploadDocPTRViewModel();
            AddTempPQVerifiedUploadDocViewModel uploadDoc = new AddTempPQVerifiedUploadDocViewModel();

            ViewBag.GetDocumentList = repoPartnerVer.GetpartnerUploadDoc(pe, scid);
            try
            {
                if (scid > 0 && pe > 0)
                {
                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                    ViewBag.GetDocumentList = repoPartnerVer.GetpartnerUploadDoc(model.PersonalRowID, model.SubCheckRowID);



                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
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
        [SessionExpireFilterAttributeVendor]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UplodEmploymentVerDocs(VerifiedUploadDocPTRViewModel model, string Upload, string Submit)
        {
            try
            {
                List<AddTempPQVerifiedUploadDocViewModel> lmd = new List<AddTempPQVerifiedUploadDocViewModel>();
                if (!string.IsNullOrEmpty(Upload))
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    int i = 1;
                    foreach (HttpPostedFileBase file in model.UploadDoc1) // loop for adding add from dataset to list<modeldata>
                    {
                        AddTempPQVerifiedUploadDocViewModel mm = new AddTempPQVerifiedUploadDocViewModel();

                        mm.DocumentType = model.DocumentType;
                        mm.FileName = file.FileName;
                        mm.DocumentUploadFrom = "Verified";
                        mm.Status = 1;
                        mm.UploadDate = DateTime.Now;
                        mm.PersonalRowID = model.PersonalRowID;
                        mm.SubCheckRowID = model.SubCheckRowID;
                        mm.Remarks = "Uploaded By Partner";

                        model.DocumentType = mm.DocumentType;
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        proImg = (model.SubCheckRowID + "_" + model.PersonalRowID + "_" + mm.UploadDate.Value.ToString("ddmmyyyyHH:mm:ss:f") + "_" + model.DocumentType + "" + i).Replace("'", "_").Replace("&", "_").Replace(":", "_").Replace("-", "_").Replace(" ", "_") + ext;

                        model.FileName = Path.GetFileName(proImg);
                        model.FilePath = FldPathDoc;

                        model.UploadDate = DateTime.Now.Date;
                        model.Status = 1;
                        cc.insertPartnerdoc(model.PersonalRowID, model.SubCheckRowID, model.FileName, model.FilePath, model.DocumentType);
                        file.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        #region Check Action History Save                           
                        clsCommonMethods.AddCheckActionHistoryByPartner(model.PersonalRowID, model.SubCheckRowID, "Employment Doc by PTR", "Address Verification information Upload Doc ");
                        #endregion
                        ++i;
                    }

                    ViewBag.GetDocumentList = repoPartnerVer.GetpartnerUploadDoc(model.PersonalRowID, model.SubCheckRowID);

                }
                if (ModelState.IsValid && Submit != null)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    model.Remarks = "Uploaded By Partner";
                    ModelState.Clear();
                    ViewBag.SuccessMsg = "Record Added Successfully!";
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
        [SessionExpireFilterAttributeVendor]
        [ValidateInput(false)]
        public ActionResult UplodEducationVerDocs(short scid = 0, int pe = 0, string cstatus = "")
        {
            VerifiedUploadDocPTRViewModel model = new VerifiedUploadDocPTRViewModel();
            AddTempPQVerifiedUploadDocViewModel uploadDoc = new AddTempPQVerifiedUploadDocViewModel();
            //ViewBag.GetDocumentList = repoDDL.GetMobileUploaddoc(pe, scid);
            ViewBag.GetDocumentList = repoPartnerVer.GetpartnerUploadDoc(pe, scid);
            try
            {
                if (scid > 0 && pe > 0)
                {
                    // if(ViewBag.GetDocumentList.Count == 0)

                    model.SubCheckRowID = scid;
                    model.PersonalRowID = pe;
                    model.CheckStatus = cstatus;
                    ViewBag.GetDocumentList = repoPartnerVer.GetpartnerUploadDoc(model.PersonalRowID, model.SubCheckRowID);



                }
                else
                {
                    ViewBag.ErrorMsg = "Invlid Education id!";
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
        [SessionExpireFilterAttributeVendor]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UplodEducationVerDocs(VerifiedUploadDocPTRViewModel model, string Upload, string Submit)
        {
            try
            {
                List<AddTempPQVerifiedUploadDocViewModel> lmd = new List<AddTempPQVerifiedUploadDocViewModel>();
                if (!string.IsNullOrEmpty(Upload))
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    int i = 1;
                    foreach (HttpPostedFileBase file in model.UploadDoc1) // loop for adding add from dataset to list<modeldata>
                    {
                        AddTempPQVerifiedUploadDocViewModel mm = new AddTempPQVerifiedUploadDocViewModel();

                        mm.DocumentType = model.DocumentType;
                        mm.FileName = file.FileName;
                        mm.DocumentUploadFrom = "Verified";
                        mm.Status = 1;
                        mm.UploadDate = DateTime.Now;
                        mm.PersonalRowID = model.PersonalRowID;
                        mm.SubCheckRowID = model.SubCheckRowID;
                        mm.Remarks = "Uploaded By Partner";

                        model.DocumentType = mm.DocumentType;
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        proImg = (model.SubCheckRowID + "_" + model.PersonalRowID + "_" + mm.UploadDate.Value.ToString("ddmmyyyyHH:mm:ss:f") + "_" + model.DocumentType + "" + i).Replace("'", "_").Replace("&", "_").Replace(":", "_").Replace("-", "_").Replace(" ", "_") + ext;

                        model.FileName = Path.GetFileName(proImg);
                        model.FilePath = FldPathDoc;

                        model.UploadDate = DateTime.Now.Date;
                        model.Status = 1;
                        cc.insertPartnerdoc(model.PersonalRowID, model.SubCheckRowID, model.FileName, model.FilePath, model.DocumentType);
                        file.SaveAs(Server.MapPath(FldPathDoc + proImg));
                        #region Check Action History Save    
                        var checkfamilyDB = cc.GetCheckFamilyname(Convert.ToInt32(model.SubCheckRowID));

                        string CheckFamilyName = Convert.ToString(checkfamilyDB.Rows[0]["CheckFamilyName"]);
                        clsCommonMethods.AddCheckActionHistoryByPartner(model.PersonalRowID, model.SubCheckRowID, "Upload Doc by PTR", CheckFamilyName + " information Upload Doc "/*"Education Verification information Upload Doc "*/);
                        #endregion
                        ++i;
                    }

                    ViewBag.GetDocumentList = repoPartnerVer.GetpartnerUploadDoc(model.PersonalRowID, model.SubCheckRowID);

                }
                if (ModelState.IsValid && Submit != null)
                {
                    string FldPathDoc = ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string proImg = string.Empty;
                    model.Remarks = "Uploaded By Partner";
                    ModelState.Clear();
                    ViewBag.SuccessMsg = "Record Added Successfully!";
                }

            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        public void ExportExeclNonAllocatedReport()
        {
            DataTable dt = new DataTable();
            String CaseStatus = "UnAllocated";
            String Statusid = "27";
            int VendorRowID = Convert.ToInt16(Session["VendorRowID"]);
            dt = cc.getDashboardPartnerDetailNonAllocation(Statusid, VendorRowID, null, null, CaseStatus);
            ExportToExcel(dt, "PartnerNonAllocated");
        }

        public void GetVendorList(short VendorRowID)
        {
            try
            {
                ViewBag.VendorNameList = repoDDL.GetMasterVendor(VendorRowID);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult PartnerCheckMovement(string VendorRowID, string CheckData)
        {

            if (!string.IsNullOrEmpty(VendorRowID) || !string.IsNullOrEmpty(CheckData))
            {
                var checkDataList = JsonConvert.DeserializeObject<List<CheckDataModel>>(CheckData);


                foreach (var checkData in checkDataList)
                {
                    var subCheckRowID = checkData.SubCheckRowID;
                    var personalRowID = checkData.PersonalRowID;
                    var Res = repoDDL.GetcheckUsingSubcheckRowId(VendorRowID, subCheckRowID.ToString(), personalRowID.ToString());

                }

            }



            return RedirectToAction("ManagePartnerAllocatedChecks", "Home", new { CaseStatus = "UserAllocated", Version = "New" });
        }
    }
}