using BAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ViewModels;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;

namespace WebAppBGV.Controllers
{
    public class BillingController : Controller
    {
        IDropDownMasterRepository repoDDL;
        IBillingCycleRepository billingCycleRepository;
        public BillingController(IDropDownMasterRepository _repoDDL,IBillingCycleRepository _billingCycleRepository)
        {
            repoDDL = _repoDDL;
            billingCycleRepository = _billingCycleRepository;
        }

        // GET: Billing
        public ActionResult AnnexureGeneration(ClientInfoListPagedModel model)
        {
            //int pageSize = 1000;
            //List<ClientviewModel> model = new List<ClientviewModel>();
            try
            {
                short teamMemerRowId = Convert.ToInt16(Session["TeamMemberRowID"]);
                ViewBag.mySkills= billingCycleRepository.GetClientName();
                //model.ClientNameList = billingCycleRepository.GetClientName();
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

        [NonAction]
        private void BindAllClient()
        {
            ViewBag.AllClientList = repoDDL.GetAllClient();
        }

        [NonAction]
        private void clientList()
        {
            ViewBag.clientList = billingCycleRepository.getClientList();
        }


        [HttpGet]
        [ValidateInput(false)]
        public ActionResult MonthelyProjection(string ClientIds, ClientInfoListPagedModel model, string cds)
        {
            int TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
            ViewBag.mySkills = billingCycleRepository.GetClientName();
            model.MonthelyProjectionList = billingCycleRepository.MonthelyProjection(ClientIds);
            model.PageSize = 200000;
            model.TotalRecords = model.MonthelyProjectionList.Count();
            if(cds == "Proceed")
            {
                ClientCustomize cc = new ClientCustomize();
                cc.BulkCaseProceed(ClientIds, TeamMemberRowID.ToString());


            }
           return View("AnnexureGeneration",model);           
            
        }

        [HttpPost]
        public void UpdateBillingCaseDetails(int BillingCaseDetailsID, string  billedValue ,string  Remark, string  status, ClientInfoListPagedModel model)
        {
            ClientCustomize cc = new ClientCustomize();
            cc.UpdateBillingCaseDetails(BillingCaseDetailsID, billedValue, Remark, status);
            
        }



        [ValidateInput(false)]
        public ActionResult BillingProcess(ClientInfoListPagedModel model, string ClientIds,  string cds, string BillingAnnexureNo,string BilledStatus)
        {
            ClientCustomize cc = new ClientCustomize();
            ViewBag.ClientList = billingCycleRepository.GetBillingCaseDetailClients();
            if (cds == "Get Detail")
            {
                if(BilledStatus== "Pending")
                {
                    BilledStatus = "Annexure Generated";
                }
                if(BillingAnnexureNo=="")
                {
                    BillingAnnexureNo = null;
                }
                model.GetBillingCaseProcessDetail = billingCycleRepository.GetBillingCaseProcessGetDetail(ClientIds, BillingAnnexureNo, BilledStatus);
                model.PageSize = 200000;
                model.TotalRecords = model.GetBillingCaseProcessDetail.Count();
            }
            else if (cds == "Freez to export")
            {
                if (BillingAnnexureNo != "")
                { 
                int TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
                cc.UpdateBillingSendAnnexure(ClientIds, TeamMemberRowID.ToString(), BillingAnnexureNo);
            }
                else
                {
                    ViewBag.ErrorMsg = "Please select Annexure No.";
                }
                model.GetBillingCaseProcessDetail = billingCycleRepository.GetBillingCaseProcessDetail(ClientIds);
                model.PageSize = 200000;
                model.TotalRecords = model.GetBillingCaseProcessDetail.Count();
            }

            else if (cds == "Annexure Approved")
            {
                if (BillingAnnexureNo != "")
                {
                    int TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
                    cc.UpdateBillingApproveAnnexure(ClientIds, TeamMemberRowID.ToString(), BillingAnnexureNo);
                }
                else
                {
                    ViewBag.ErrorMsg = "Please select Annexure No.";
                }
                model.GetBillingCaseProcessDetail = billingCycleRepository.GetBillingCaseProcessDetail(ClientIds);
                model.PageSize = 200000;
                model.TotalRecords = model.GetBillingCaseProcessDetail.Count();
            }

            else if (cds == "Export")
            {
                DataTable dt = new DataTable();
                dt=cc.GetBillingSendAnnexureExport(ClientIds);
                ExportToExcel(dt,"BillingSendAnnexure");
                model.GetBillingCaseProcessDetail = billingCycleRepository.GetBillingCaseProcessDetail(ClientIds);
                model.PageSize = 200000;
                model.TotalRecords = model.GetBillingCaseProcessDetail.Count();
            }
                
            return View(model);
        }


        public void ExportToExcel(DataTable dt, string strFileName)
        {
            if (dt.Rows.Count > 0)
            {
                Response.Clear();
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();
                dgGrid.GridLines = GridLines.None;
                dgGrid.BorderStyle = BorderStyle.None;


                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName + ".xls");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
                Response.Write(tw.ToString());
                Response.End();
            }
        }


        public void GetVendorDDL()
        {
            try
            {
                ViewBag.VendorList = repoDDL.GetBillingVendorList();
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult VendorBulkUpload()
        {
            AddVendorBulkUpload model = new AddVendorBulkUpload();
            try
            {
                GetVendorDDL();
                string FileName = "VendorBulkUpload.xlsx";
                string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["BillingVendorUpload"].ToString();
                model.Template = Url.Content(FldPathDoc + FileName);
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
        public ActionResult VendorBulkUpload(AddVendorBulkUpload model)
        {
            try
            {
                GetVendorDDL();
                if (model.VendorRowId > 0 )
                {
                    ClientCustomize cc = new ClientCustomize();
                    int fileNo = cc.VendorBillingBulkUploadCount(model.VendorRowId);                    
                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        fileNo++;
                        string Extension = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                        string FileName = (model.VendorRowId + "_" + fileNo.ToString()).Replace(" ", "_").Replace("-", "_") + Extension;
                        string FolderPath = ConfigurationManager.AppSettings["BillingVendorUpload"];
                        // HttpPostedFileBase
                        string FilePath = Server.MapPath(FolderPath + FileName);
                        model.UploadDoc.SaveAs(Server.MapPath(FolderPath + FileName));
                        cc.InsertVendorBillingBulkUpload(model.VendorRowId, FileName);
                        ImportExcelAndSaveCandidateByBulkUpload(FileName,FilePath, Extension, "Yes", model);
                        return RedirectToAction("VendorProceedConfirm", "Billing", new { fileName = FileName });
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Please Select Excel Document!";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid VendorRowId!";
                }
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult VendorProceedConfirm(string fileName)
        {
            int TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
            ClientInfoListPagedModel model = new ClientInfoListPagedModel();
            model.AddVendorBulkUpload = billingCycleRepository.GetBillingVendorListToProceed(fileName);
            model.PageSize = 200000;
            model.TotalRecords = model.AddVendorBulkUpload.Count();
            model.ErrorRecord = model.AddVendorBulkUpload.Where(T => T.ErrorStatus == "Error").Count();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult VendorProceedConfirm(string fileName, string cds)
        {
            ClientCustomize cc = new ClientCustomize();
            int TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
            ClientInfoListPagedModel model = new ClientInfoListPagedModel();
            model.AddVendorBulkUpload = billingCycleRepository.GetBillingVendorListToProceed(fileName);
            model.PageSize = 200000;
            model.TotalRecords = model.AddVendorBulkUpload.Count();
            model.ErrorRecord = model.AddVendorBulkUpload.Where(T => T.ErrorStatus == "Error").Count();
            if(cds == "Proceed")
            {
                cc.insertBillingVendorUploadDetail(cds, fileName);
                ViewBag.SuccessMsg =  "record added successfully!";
            }
            else
            {
                cc.insertBillingVendorUploadDetail("Cancel", fileName);
                ViewBag.SuccessMsg = "record cancel successfully!";
            }
            return View(model);
            //return RedirectToAction("VendorProceedConfirm", new { fileName = fileName });
        }


        private void ImportExcelAndSaveCandidateByBulkUpload(string fileName, string FilePath, string Extension, string isHDR, AddVendorBulkUpload model) //isHDR - means Include header Yes or No
        {
            ClientCustomize cc = new ClientCustomize();

            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand Excel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();

            Excel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            Excel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = Excel;
            oda.Fill(dt);
            connExcel.Close();

            model.CreatedBy = Convert.ToInt32(Session["TeamMemberRowID"]);

            //Insert into database CandidatesTempBulk table
            if (dt.Rows.Count > 0)
            {
                int i = 0;

                int countSuccess = 0;
                int countDuplicate = 0;
                int countInvalid = 0;
                string Email = string.Empty;
                string MobileNo = string.Empty;

                for ( i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        model.SrNo = dt.Rows[i]["SrNo"].ToString();
                        model.CompanyRefNo = dt.Rows[i]["CompanyRefNo"].ToString();
                        model.ClientName = dt.Rows[i]["Client Name"].ToString();
                        model.CandidateName = dt.Rows[i]["CandidateName"].ToString();
                        model.AddressType = dt.Rows[i]["Address Type"].ToString();
                        model.Location = dt.Rows[i]["Location"].ToString();
                        model.State = dt.Rows[i]["State"].ToString();
                        model.Allocationdate = dt.Rows[i]["Allocation date"].ToString();
                        model.Assignto = dt.Rows[i]["Assign to"].ToString();
                        model.CloserDate = dt.Rows[i]["Closer Date"].ToString();
                        model.Status = dt.Rows[i]["Status"].ToString();
                        model.Ageing = dt.Rows[i]["Ageing"].ToString();
                        model.Panalty = dt.Rows[i]["Panalty"].ToString();
                        model.BilledAmount = dt.Rows[i]["Billed Amount"].ToString();
                        model.BillAmount = dt.Rows[i]["Bill Amount"].ToString();
                        model.BillMonth = dt.Rows[i]["Bill Month"].ToString();
                        cc.insertTempBillingVendorDetail(model.ClientName, model.CompanyRefNo, model.CandidateName, model.AddressType, model.Location, model.State, model.Allocationdate, model.Assignto, model.CloserDate, model.State, model.Ageing, model.BillAmount, model.Panalty, model.BilledAmount, model.BillMonth, model.CreatedBy, model.VendorRowId,null,fileName);
                    }
                    catch (Exception Ex)
                    {
                        cc.insertTempBillingVendorDetailError(model.ClientName, "", model.CandidateName, "", model.Location, model.State, model.Allocationdate, model.Assignto, model.CloserDate, model.State, model.Ageing, model.Panalty,  model.BillMonth, model.CreatedBy, model.VendorRowId, Ex.ToString(), fileName, i+1);

                    }
                }


            }
            else
            {
                // ViewBag.ErrorMsg = "No vendor found!";
            }

        }
        [ValidateInput(false)]
        public ActionResult InvoiceGenerated(string msg)
        {
            ClientInfoListPagedModel model = new ClientInfoListPagedModel();
            model.BillingInvoiceGenerated = new BillingInvoiceGenerated();

            clientList();
            if(msg== "Success")
            {
                ViewBag.SuccessMsg = "Record save successfully";
            }            
            return View(model);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InvoiceGenerated(ClientInfoListPagedModel model)
        {
            int TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
            ClientCustomize cc = new ClientCustomize();
            cc.insertBillingInvoiceGenerated(model.BillingInvoiceGenerated.Month, model.BillingInvoiceGenerated.Year, model.BillingInvoiceGenerated.ClientRowId,
               model.BillingInvoiceGenerated.AnnexureNo, model.BillingInvoiceGenerated.InvoiceNo, model.BillingInvoiceGenerated.InvoiceAmount, model.BillingInvoiceGenerated.GST,
               model.BillingInvoiceGenerated.TotalAmount, TeamMemberRowID, model.BillingInvoiceGenerated.GSTAmount, model.BillingInvoiceGenerated.TDSAmount
                );
            clientList();
            return RedirectToAction("InvoiceGenerated", "Billing", new { msg = "Success" });        

        }

        [ValidateInput(false)]
        public ActionResult InvoiceUpdation(ClientInfoListPagedModel model, string ClientIds, string cds, string BillingAnnexureNo,string Month,string ddlInvoiceNo)
        {
            ClientCustomize cc = new ClientCustomize();
            ViewBag.ClientList = billingCycleRepository.GetBillingCaseDetailClients();      

            if (cds == "Get Detail")
            {
                ViewBag.ClientMonthList = billingCycleRepository.ClientBillingMonthList(ClientIds);
                ViewBag.BillingInvoiceNo = billingCycleRepository.GetBillingInvoiceGenerated(Convert.ToInt32(ClientIds), Convert.ToInt32(BillingAnnexureNo), Month);
                              
                model.GetBillingCaseApproveDetail = billingCycleRepository.GetBillingCaseApproveDetail(ClientIds, BillingAnnexureNo, Month);
                model.PageSize = 200000;
                model.TotalRecords = model.GetBillingCaseApproveDetail.Count();
                model.ClientRowId = Convert.ToInt32(ClientIds);
            }
            else if (cds == "Invoice Update")
            {
                DataTable dt=cc.GetBillingInvoiceNo(Convert.ToInt32(ClientIds), Convert.ToInt32(BillingAnnexureNo), Month);
                int TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
                cc.UpdateBillingCaseInvoice(Convert.ToInt32(ClientIds),  BillingAnnexureNo, Convert.ToInt32(ddlInvoiceNo), Month);
                ViewBag.SuccessMsg = "Invoice update successfully";

            }
               

                return View(model);
        }

        public ActionResult BillingPaymentDetail(ClientInfoListPagedModel model, string cds,string msg)
        {
            if(msg== "Success")
            {
                clientList();
                ViewBag.SuccessMsg = "Record save successfully";
                List<BillingInvoiceModel> obj = new List<BillingInvoiceModel>();
                ViewBag.BillingClientInvoiceNo = obj;
                return View();
            }
            ClientCustomize cc = new ClientCustomize();
            if (cds == "Submit")                
            {
                int TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);                
                cc.insertBilling_PaymentDetail(model.BillingPaymentDetail.Month, model.BillingPaymentDetail.Year, model.BillingPaymentDetail.ClientRowId,
                 model.BillingPaymentDetail.InvoiceNo, model.BillingPaymentDetail.InvoiceAmount, model.BillingPaymentDetail.GSTAmount,
                   model.BillingPaymentDetail.TDSAmount, model.BillingPaymentDetail.TotalAmount, TeamMemberRowID.ToString()
                    );
                clientList();

                ViewBag.SuccessMsg = "Record save successfully";

                return RedirectToAction("BillingPaymentDetail", "Billing", new { msg = "Success" });
            }
            else
            {            
            if (model.BillingPaymentDetail != null)
            {
                ViewBag.BillingClientInvoiceNo = billingCycleRepository.GetClientBillingInvoice(Convert.ToInt32(model.BillingPaymentDetail.ClientRowId), Convert.ToInt32(model.BillingPaymentDetail.Year), model.BillingPaymentDetail.Month);
                if (!string.IsNullOrEmpty (model.BillingPaymentDetail.InvoiceNo))
                {
                    DataTable dt = cc.GetInvoiceAmount(model.BillingPaymentDetail.InvoiceNo);
                    model.BillingPaymentDetail.GSTAmount = dt.Rows[0]["gstamount"].ToString();
                    model.BillingPaymentDetail.InvoiceAmount = dt.Rows[0]["InvoiceAmount"].ToString();
                        ModelState.Clear();
                        // insertBilling_PaymentDetail
                    }

            }
            else
            {
                List<BillingInvoiceModel> obj = new List<BillingInvoiceModel>();
                ViewBag.BillingClientInvoiceNo = obj;
            }
            clientList();
        }
            return View(model);
        }


        [HttpGet]
        [ValidateInput(false)]
        public ActionResult BillingUploadDetail()
        {
            BillingUploadDetail model = new BillingUploadDetail();
            try
            {
                GetVendorDDL();
                string FileName = "VendorBulkUpload.xlsx";
                string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["BillingVendorUpload"].ToString();
                model.Template = Url.Content(FldPathDoc + FileName);
            }
            catch (Exception Ex)
            {
                ViewBag.ErrorMsg = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult BillingUploadDetail(BillingUploadDetail model)
        {
            try
            {
                //GetVendorDDL();
                string TeamMemberRowID =Session["TeamMemberRowID"].ToString();

                ClientCustomize cc = new ClientCustomize();
                    int fileNo = cc.InvoiceBillingBulkUploadCount();
                    if (model.UploadDoc != null && model.UploadDoc.ContentLength > 0)
                    {
                        fileNo++;
                        string Extension = Path.GetExtension(model.UploadDoc.FileName).ToLower();
                        string FileName =  "Invoice"+fileNo.ToString().Replace(" ", "_").Replace("-", "_") + Extension;
                        string FolderPath = ConfigurationManager.AppSettings["BillingVendorUpload"];
                        // HttpPostedFileBase
                        string FilePath = Server.MapPath(FolderPath + FileName);
                        model.UploadDoc.SaveAs(Server.MapPath(FolderPath + FileName));
                        cc.InsertBillingInvoiceBulkUpload( FileName, TeamMemberRowID);
                        ImportExcelInvoiceByBulkUpload(FileName, FilePath, Extension, "Yes", model);
                        return RedirectToAction("InvoiceProceedConfirm", "Billing", new { fileName = FileName });
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
        [ValidateInput(false)]
        public ActionResult InvoiceProceedConfirm(string fileName)
        {
            int TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
            ClientInfoListPagedModel model = new ClientInfoListPagedModel();
            model.BillingUploadDetail = billingCycleRepository.GetBillingInvoiceListToProceed(fileName);
            model.PageSize = 200000;
            model.TotalRecords = model.BillingUploadDetail.Count();
            model.ErrorRecord = model.BillingUploadDetail.Where(T => T.ErrorStatus == "Error").Count();
            return View(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InvoiceProceedConfirm(string fileName, string cds)
        {
            ClientCustomize cc = new ClientCustomize();
            int TeamMemberRowID = Convert.ToInt16(Session["TeamMemberRowID"]);
            ClientInfoListPagedModel model = new ClientInfoListPagedModel();
            model.BillingUploadDetail = billingCycleRepository.GetBillingInvoiceListToProceed(fileName);
            model.PageSize = 200000;
            model.TotalRecords = model.BillingUploadDetail.Count();
            model.ErrorRecord = model.BillingUploadDetail.Where(T => T.ErrorStatus == "Error").Count();
            if (cds == "Proceed")
            {
                cc.insertBillingInvoiceUploadDetail(cds, fileName);
                ViewBag.SuccessMsg = "record added successfully!";
            }
            else
            {
                cc.insertBillingInvoiceUploadDetail("Cancel", fileName);
                ViewBag.SuccessMsg = "record cancel successfully!";
            }
            return View(model);         
        }

        private void ImportExcelInvoiceByBulkUpload(string fileName, string FilePath, string Extension, string isHDR, BillingUploadDetail model) //isHDR - means Include header Yes or No
        {
            ClientCustomize cc = new ClientCustomize();

            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand Excel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();

            Excel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            Excel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = Excel;
            oda.Fill(dt);
            connExcel.Close();

            model.CreatedBy = Session["TeamMemberRowID"].ToString();

            //Insert into database CandidatesTempBulk table
            if (dt.Rows.Count > 0)
            {
                int i = 0;

                int countSuccess = 0;
                int countDuplicate = 0;
                int countInvalid = 0;
                string Email = string.Empty;
                string MobileNo = string.Empty;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        model.SrNo = dt.Rows[i]["S No"].ToString();
                        model.AdditionCost = dt.Rows[i]["Additional Cost"].ToString();
                        model.AnnexureValue = dt.Rows[i]["Annexure Value"].ToString();
                        model.Approvalstatus = dt.Rows[i]["Approval Status"].ToString();
                        model.ClientName = dt.Rows[i]["Client Name"].ToString();
                        model.CompanyRefNo = dt.Rows[i]["Reference No"].ToString();
                        //model.CreatedBy = dt.Rows[i]["Location"].ToString();
                        model.FinalAnnexureAmount = dt.Rows[i]["Final annexure Amount"].ToString();
                        model.InvoiceDate = dt.Rows[i]["Billing Date/Invoice Date"].ToString();
                        model.InvoiceNo = dt.Rows[i]["Invoice Number"].ToString();
                        model.Month = dt.Rows[i]["Billing Month"].ToString();
                        model.PenaltyAmount = dt.Rows[i]["Penalty amount"].ToString();
                        model.Status = dt.Rows[i]["Status"].ToString();
                        model.Year = "";

                        


                        cc.insertTempBillingUploadetail(model.Month, model.Year, model.ClientName, model.CompanyRefNo, model.AnnexureValue, model.AdditionCost, model.FinalAnnexureAmount
                            , model.Approvalstatus, model.Status, model.InvoiceNo, model.InvoiceDate, model.PenaltyAmount, model.CreatedBy,null,fileName,i+1);
                    }
                    catch (Exception Ex)
                    {
                        cc.insertTempBillingUploadetailError(model.ClientName,  model.CompanyRefNo, Ex.ToString(), fileName, i + 1,model.CreatedBy);

                    }
                }
             

            }
            else
            {
                // ViewBag.ErrorMsg = "No vendor found!";
            }

        }
    }
}