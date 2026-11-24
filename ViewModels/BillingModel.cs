using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace ViewModels
{  

        public class ClientInfoListPagedModel
        {
            public IEnumerable<ClientviewModel> ClientNameList { get; set; }
        public IEnumerable<MonthelyProjection> MonthelyProjectionList { get; set; }
        public IEnumerable<GetBillingCaseProcessDetail> GetBillingCaseProcessDetail { get; set; }
        public IEnumerable<AddVendorBulkUpload> AddVendorBulkUpload { get; set; }
        public BillingInvoiceGenerated BillingInvoiceGenerated { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string AnnuxureStatus { get; set; }
        public int ErrorRecord { get; set; }
        public IEnumerable<GetBillingCaseProcessDetail> GetBillingCaseApproveDetail { get; set; }
        public IEnumerable<BillingMonthModel> ClientBillingMonthList { get; set; }
        public IEnumerable<BillingInvoiceModel> BillingInvoiceModelList { get; set; }

        public IEnumerable<BillingInvoiceModel> BillingClientInvoiceList { get; set; }
        public int ClientRowId { get; set; }

        public BillingInvoiceGenerated BillingPaymentDetail { get; set; }


        public IEnumerable<BillingUploadDetail> BillingUploadDetail { get; set; }
        //public BillingInvoiceModel BillingInvoiceModelList { get; set; }
        //public ClientviewModel ClientviewModel { get; set; }
        //public MonthelyProjection MonthelyProjection { get; set; }

    }

        public class ClientviewModel
        {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        
    }


    public class BillingMonthModel
    {
        public string MonthId { get; set; }
        public string MonthName { get; set; }

    }

    public class BillingInvoiceModel
    {
        public string InvoiceId { get; set; }
        public string InvoiceNo { get; set; }

    }


    public class MonthelyProjection
    {
        public string ClientName { get; set; }
        public string CandidateName { get; set; }
        public string CompanyRefNo { get; set; }
        public string CheckOrPackage { get; set; }
        public string InitianDate { get; set; }
        public string CaseStatus { get; set; }
        public string ReportSendDate { get; set; }
        public string ReportColor { get; set; }
        public string AddCheck { get; set; }
        public string TotalChecks { get; set; }
        public string NACheck { get; set; }
        public string StopCheck { get; set; }
        public string DeleteCheck { get; set; }
        public string SystemPrice { get; set; }
        public string BillingType { get; set; }
        public string BilledValue { get; set; }
        public string Incentive { get; set; }
        public string Penalities { get; set; }
        public string ModiefiedBilledprice { get; set; }
        public string Reason { get; set; }
        [AllowHtml]
        public string Remark { get; set; }
        public string Status { get; set; }
    }

    public class GetBillingCaseProcessDetail
    {
        public int  BillingCaseDetailsID {get;set;}
        public int  PersonalRowID { get; set; }
        public string  BillingType { get; set; }
        public string BillingStatus { get; set; }
        public string TotalNoOfChecks { get; set; }
        public string CompletedChecks { get; set; }
        public string PendingChecks { get; set; }
        public string NAChecks { get; set; }
        public string TotalPrice { get; set; }
        public string AdditionalCost { get; set; }
        public string FinalAmount { get; set; }
        public string Panelty { get; set; }   
        public string PaneltyRemarks { get; set; }
        public string InvoiceDetailsID { get; set; }
        public string InvoiceCaseAmount { get; set; }
        public string ManuallyModifiedStatus { get; set; }
        public string ManuallyModifiedRemarks { get; set; }
        public string ClientName { get; set; }
        public string CompanyRefNo { get; set; }
        public string BillingCaseStatus { get; set; }
        public string AnnexureNo { get; set; }
        public string CaseStatus { get; set; }
        public string StandardPrice { get; set; }
        public string ExtraCost { get; set; }
        public string Incentive { get; set; }

    }

    public class AddVendorBulkUpload
    {
        public string SrNo { get; set; }
        public string ClientName { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandidateName { get; set; }
        public string AddressType { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public string Allocationdate { get; set; }
        public string Assignto { get; set; }
        public string CloserDate { get; set; }
        public string Status { get; set; }
        public string Ageing { get; set; }
        public string BillAmount { get; set; }
        public string Panalty { get; set; }
        public string BilledAmount { get; set; }
        public string BillMonth { get; set; }
        public int CreatedBy { get; set; }
        public int VendorRowId { get; set; }
        public string Template { get; set; }
        public string ErrorStatus { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase UploadDoc { get; set; }
    }

    public class BillingInvoiceGenerated
    {   
        public int InvoiceId { get; set; }
        public string   Month { get; set; }
        public string Year { get; set; }
        public string ClientRowId { get; set; }
        public string AnnexureNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceAmount { get; set; }
        public string GST { get; set; }
        public string TotalAmount { get; set; }
        public string GSTAmount { get; set; }
        public string TDSAmount { get; set; }


        
    }

    public class BillingUploadDetail
    {
        public string SrNo { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string ClientRowId { get; set; }
        public string CompanyRefNo { get; set; }
        public string AnnexureValue { get; set; }
        public string AdditionCost { get; set; }
        public string FinalAnnexureAmount { get; set; }
        public string Approvalstatus { get; set; }
        public string Status { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PenaltyAmount { get; set; }
        public string CreatedBy { get; set; }
        public string ClientName { get; set; }        
        public string Template { get; set; }
        public string ErrorStatus { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase UploadDoc { get; set; }
    }


}
