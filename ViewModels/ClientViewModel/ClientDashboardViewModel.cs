using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProvidedInfoViewModel;
//using System.Web.Mvc;

namespace ViewModels.ClientViewModel
{
    public class ClientDashboardViewModel
    {
        public int totReportReceived { get; set; }
        public int totReportReceivedInterim { get; set; }
        public int ResultAwaited { get; set; }
        public int totWIP { get; set; }
        public int totPendingwithcandidate { get; set; }
        public int totCaseApprovalpending { get; set; }
        public int totInsuffWIP { get; set; }
        public int totYTRWIP { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }
        public int totPriority { get; set; }
        public int InterimCRC { get; set; }
        public int totYTRHold { get; set; }
        public int totYTRHoldOr10Days { get; set; }
        public int totInsufficiency { get; set; }
        public int totInsuffOr5Day { get; set; }
        public int totFundApproval { get; set; }
        public int totEscalated { get; set; }
        public int totInterim { get; set; }
        public int totFinal { get; set; }
        public int totOtherthanGreen { get; set; }
        public int NonGreenCheck { get; set; }


        public int GallagherSecondInterims { get; set; }

        public int CRCReports { get; set; }
        public int PCCReports { get; set; }
        public int AutoGallagherInterims { get; set; }

        public int AutoGallagherInterims1 { get; set; }

        public int CasesAllReports { get; set; }

        public int ClientReviewBucket { get; set; }
        public List<ChartDataPoint> ChartData { get; set; }

        public List<ClientDashboardCheckFamilyViewModel> CheckFamilies { get; set; }
    }

    public class ClientCandidateViewModel
    {
        public int PersonalRowID { get; set; }
        public string CandidateName { get; set; }

        public short ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string IsSeniorCase { get; set; }

        public string CompanyRefNo { get; set; }
        public string CandidateCode { get; set; }

        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }

        public DateTime ?RWQC2Submissiondate { get; set; }
        public DateTime ? RWQCTMCompletedDate { get; set; }
        public string RWQCTMName { get; set; }

        public string RWTMName { get; set; }

        public DateTime? RWTMDate { get; set; }

        public string CaseStatus { get; set; }
        public int CheckStatusId { get; set; }
        

        public string FinalStatus { get; set; }
        public string FinalColor { get; set; }
        public byte InterimReport { get; set; }

        public bool IsReportUploaded { get; set; }
        public string ReportUploadedName { get; set; }
        public DateTime? ReportUploadedDate { get; set; }
        public string ClientRefID { get; set; }
        public bool ReportVisibality { get; set; }
        public int ClientStatus { get; set; }
        public string casedocument { get; set; }
        public string SubCheckName { get; set; }
        public int SubCheckRowID { get; set; }
        public string Report_Type { get; set; }
        public string Package { get; set; }
        public string Emailid { get; set; }
        public string Entity { get; set; }
        public string MobileNo { get; set; }
        public int EmploymentRowId { get; set; }
        public int CandCheckRowID { get; set; }

        public string CaseType { get; set; }
        public int CheckFamilyRowID { get; set; }
        public string ReportActionRemark { get; set; }
        public string DetailsInsuffRemarks { get; set; }
        public string UAN { get; set; }
        public string insuffCreatedate { get; set; }
        public string Remarks { get; set; }
        public bool ReportVisibilityInterim { get; set; }
        public string Location { get; set; }
        public string CaseStatusId { get; set; }
        public string ColorCode { get; set; }

        public string CheckStatusName { get; set; }
        public string DeptNameWIP { get; set; }
        public string DeptNameInsuff { get; set; }
        public string DeptNameYTR { get; set; }
        public string DeptNameFundsPending { get; set; }
        public string DeptNameClosed { get; set; }
        public string CurrentCaseStatus { get; set; }
        public string CandidateEmail { get; set; }
        public string EmployerInstitutionNameChecksName { get; set; }
        public string AgeingFor { get; set; }
        public string CheckUniqueID { get; set; } 

        /* Start Invoicing */

        public string BillingType { get; set; }

        [Required(ErrorMessage = "Billing Wise is required.")]
        public string BillingWise { get; set; }
        public string AnnexureUploadOptionFileName { get; set; }
        public string AnnexureUploadOptionFilePath { get; set; }

        [Required(ErrorMessage = "AnnexureSent Date is required")]
        public string AnnexureSentDate { get; set; }

        [Range(0, 999999999, ErrorMessage = "Amount must be a whole number up to 9 digits.")]
        [RegularExpression(@"^\d{1,9}$", ErrorMessage = "Only whole numbers up to 9 digits are allowed.")]
        public double AnnexureAmount { get; set; }
        public Boolean ReminderSendOption { get; set; }
        public string AnnexureApprovedUploadOption { get; set; }

        [Required(ErrorMessage = "AnnexureApproved Date is required")]
        public string AnnexureApprovedDate { get; set; }
        //public DateTime? OutDate { get; set; }
        [Range(0, 999999999, ErrorMessage = "Amount must be a whole number up to 9 digits.")]
        [RegularExpression(@"^\d{1,9}$", ErrorMessage = "Only whole numbers up to 9 digits are allowed.")]
        public double AnnexureApprovedAmount { get; set; }

        [Required(ErrorMessage = "Invoice No is required.")]
        public string InvoiceNo { get; set; }
        public string InvoiceUploadOptionFile { get; set; }

        [Required(ErrorMessage = "InvoiceSend Date is required")]
        public string InvoiceSendDate { get; set; }

        [Range(0, 999999999, ErrorMessage = "Amount must be a whole number up to 9 digits.")]
        [RegularExpression(@"^\d{1,9}$", ErrorMessage = "Only whole numbers up to 9 digits are allowed.")]
        public double InvoiceAmount { get; set; }

        [Required(ErrorMessage = "PaymentReceived Date is required")]
        public string PaymentReceivedDate { get; set; }

        [Range(0, 999999999, ErrorMessage = "Amount must be a whole number up to 9 digits.")]
        [RegularExpression(@"^\d{1,9}$", ErrorMessage = "Only whole numbers up to 9 digits are allowed.")]
        public double PaymentReceivedAmount { get; set; }
        [Required(ErrorMessage = "Please select a settlement option")]
        public string SettlementOption { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        [Display(Name = "Select Document :")]
        [Required]
        [ValidateLargeFileAttribute]
        [ValidateLargeFileCandidateUploadDocs]
        // public HttpPostedFileBase[] UploadDoc1 { get; set; }
        public HttpPostedFileBase UploadDoc1 { get; set; }
        public string ModifyedBy { get; set; }
        public DateTime? ModifyedDate { get; set; }
        public bool Status { get; set; }
        public string Template { get; set; }
        public int AnnexureInvoicingId { get; set; }
        public string ClientCode { get; set; }
        public string SpocName { get; set; }
        public string AnnexureInvoicingType { get; set; }

        [Required(ErrorMessage = "Acknowledgement Date is required")]
        public string AcknowledgementDate { get; set; }

        [Required(ErrorMessage = "PaymentExpected Date is required")]
        public string PaymentExpectedDate { get; set; }
        /*End Invoicing*/

        public string HRRecruiterName { get; set; }
        public string PackageName { get; set; }

        public string todayPending { get; set; }
        public string NextTwoDays { get; set; }
        public string WithinTAT { get; set; }
        public string OneToTwodaysBT { get; set; }
        public string ThreeToFivedaysBT { get; set; }
        public string TwoPlusDays { get; set; }
        public string ZeroToSixHour { get; set; }
        public string SevenToTwelveHour { get; set; }
        public string ThirteenToEighteenHour { get; set; }
        public string NineteenToTwnetyFourHour { get; set; }
        public string EmployeeID { get; set; }
        public DateTime? VerificationDate { get; set; }
        public string Total { get; set; }
        public string TodayClosureUnit { get; set; }
        public DateTime? CheckTagDate { get; set; }
        public int CandidateActiveStatus { get; set; } = 1;

    }

    public class ExcelClientCandidateViewModel
    {
        public int PersonalRowID { get; set; }
        public string CandidateName { get; set; }

        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        public string CompanyRefNo { get; set; }
        public string CandidateCode { get; set; }

        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }

        public string CaseStatus { get; set; }

        public string FinalStatus { get; set; }
        public string FinalColor { get; set; }
        public byte InterimReport { get; set; }

        public bool IsReportUploaded { get; set; }
        public string ReportUploadedName { get; set; }
        public DateTime? ReportUploadedDate { get; set; }
    }

    public class RollbackFileModel
    {
        public string ClientType { get; set; }
        public string FileName { get; set; }
        public string OriginalPath { get; set; }
        public string FileValue { get; set; }  
    }

    public class PurgeFormSubmitModel
    {
        public int? ClientId { get; set; }
        public List<string> CompanyRefNos { get; set; } = new List<string>();
    }
    public class DashboardClientCandidateListViewModel
    {
        public IEnumerable<ClientCandidateViewModel> DashboardClientCandidates { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public bool ReportVisibality { get; set; }
        public bool ReportVisibilityInterim { get; set; }
        public bool ReportVisibilityFinal { get; set; }
        public PQPersonalSearchPagedModel ItemSearch { get; set; }
        public IEnumerable<AnnexureInvoicing> DashboardAnnexureInvoicing { get; set; }
    }

    #region ***** CS Dashboard View Model *****

    public class DashboardManagerClientServicingViewModel
    {
        public int totWIP { get; set; }
        public int totPendingwithcandidate { get; set; }
        public int totPendingwithHR { get; set; }
        public int ResultAwaited { get; set; }
        public int totNonInitiated { get; set; }
        public int totYTRHold { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }
        public int totPriority { get; set; }
        public int totInsuffWIP { get; set; }

        public int totTodaysDue { get; set; }
        public int totDueinNext3days { get; set; }
        public int totInTAT4to7days { get; set; }
        public int totBT1to2days { get; set; }
        public int totBT3to5days { get; set; }
        public int totBT5to10days { get; set; }
        public int totBT10daysPlus { get; set; }
        public int tot1checkPending { get; set; }
        public int totInsuffPending5Day { get; set; }

        public int totWIPRevenue { get; set; }
        public int totInsufficiency { get; set; }
        public int totWIPYTR { get; set; }
        public int totQCPending { get; set; }
        public int totFundApproval { get; set; }
        public int totApprovalPending { get; set; }
        public int totEscalated { get; set; }
        public int totExprted { get; set; }
        public int totOutTatInPercent { get; set; }

        public short ClientRowID { get; set; }
        public string todayPending { get; set; }
        public string NextTwoDays { get; set; }
        public string WithinTAT { get; set; }
        public string daysBT1_2 { get; set; }
        public string daysBT3_5 { get; set; }
        public string above5daysBT { get; set; }
        public int ColorCodeReview { get; set; }
        public string SpocReview { get; set; }
        public string ClientReviewApproved { get; set; }
        public string ClientReviewClear { get; set; }

        public int totCheckTag { get; set; }
        public int ValidateCheckTag { get; set; }


    }

    #endregion

    public class ClientCandidateCheckTagExportViewModel
    {
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string EmployeeID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public string CheckID { get; set; }
        public string ColorCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? VerificationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string CheckTagBy { get; set; }
        public DateTime? CheckTagDate { get; set; }

    }

    public class ClientDashboardCaseSummaryViewModel
    {
        public int totPendingwithcandidate { get; set; }
        public int totCaseApprovalpending { get; set; }
        public int totWIP { get; set; }
        public int totComplete { get; set; }
        public int totInsuff { get; set; }
        public int totOtherthanGreen { get; set; }
        public List<ClientDashboardCaseSummaryDetailViewModel> DashboardClientCaseDetails { get; set; }
    }
    public class ClientDashboardCaseSummaryDetailViewModel
    {
        public int SNumber { get; set; }
        public int PersonalRowID { get; set; }
        public int ClientRowID { get; set; }
        public string CandidateName { get; set; }

        public string CandidateCode { get; set; }
        public string SecuritasID { get; set; }
        public string DetailsEnteredBy { get; set; }
        public string CaseStatus { get; set; }

        public string OrderDate { get; set; }
        public string CreatedDate { get; set; }
        public string OutDate { get; set; }

        //public short CreatedBy { get; set; }

        //public string FinalStatus { get; set; }
        //public string FinalColor { get; set; }
        //public byte InterimReport { get; set; }

        //public bool IsReportUploaded { get; set; }
        //public string ReportUploadedName { get; set; }
        //public DateTime? ReportUploadedDate { get; set; }
    }

    public class ClientDashboardCheckSummaryViewModel
    {
        public int totWIP { get; set; }
        public int totFundsPending { get; set; }
        public int totComplete { get; set; }
        public int totInsuff { get; set; }
        public int totOtherthanGreen { get; set; }
        
        public IEnumerable<ClientDashboardCheckSummaryDetailViewModel> DashboardClientCheckDetails { get; set; }
    }
    public class ClientDashboardCheckSummaryDetailViewModel
    {
        public int SNumber { get; set; }
        public int PersonalRowID { get; set; }
        public int ClientRowID { get; set; }

        public string CandidateName { get; set; }
        public string CandidateCode { get; set; }
        public string SecuritasID { get; set; }
        public string CheckName { get; set; }
        public string CheckStatus { get; set; }

        public string casestatusid { get; set; }
        public string OrderDate { get; set; }
        public string CreatedDate { get; set; }
        public string OutDate { get; set; }
        public string CheckStatusID { get; set; }
        public string ColorName { get; set; }
        public int SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        public int ClientCheckRowID { get; set; }
        public string Name { get; set; }
        public string DatabaseName { get; set; }
		public int CandCheckRowID { get; set; }
		public int createdby { get; set; }


	}

	public class ClientServiceListViewModel
    {
        public string caseDocumentName { get; set; }
        public string caseDocumentPath { get; set; }
        public IEnumerable<ClientDashboardCheckSummaryDetailViewModel> ClientServiceChecks { get; set; }
        public IEnumerable<FakeUniversity> FakeUniversity { get; set; }
        public IEnumerable<FakeCompanies> FakeCompanies { get; set; }
        public IEnumerable<ReverificationApproval> ReverificationApproval { get; set; }
        public ReverificationApproval ReverificationApp { get; set; }
        public IEnumerable<GetClientDetails> GetClientDetails { get; set; }

        public int PageSize { get; set; }
        public int TotalRecords { get; set; }        
    }


    public class ClientDashboardTrackerMISViewModel
    {
        public string  SecuritasID { get; set; }
        public string ClientName { get; set; }
        public int ClientRowId { get; set; }
        public string CandidateCode { get; set; }
        public string CandidateName { get; set; }
        public string Category { get; set; }
        public string ClientRefID { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string MaritalStatus { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string CandidateType { get; set; }
        public string EmployeeID { get; set; }
        public string Designation { get; set; }
        public string PlaceofJoining { get; set; }
        public DateTime? DateofJoining { get; set; }
        public DateTime? SLADate { get; set; }
        public DateTime? DUEDate { get; set; }
        public string CaseStatus { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public string ReportColor { get; set; }
        public string ColorCodeRemark { get; set; }
        public DateTime? ReportSentDate { get; set; }
        public string Insuffremarks { get; set; }
        public string Entity { get; set; }
        public string EmploymentType { get; set; }
        public string NonGreenRemarks { get; set; }

        public IEnumerable<ClientDashboardTrackerMISViewModel> ClientDashboardTrackerMISViewModels { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string InsuffClearDate { get; set; }
        public string InsuffRaiseDate { get; set; }
        public string FundRequestClearDate { get; set; }
        public string FundRequestRaisedDate { get; set; }
        public string YTRClearDate { get; set; }
        public string ReOpenDate { get; set; }

    }
    public class ClientCheckDetailViewModel
    {

        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int SNumber { get; set; }
        public int PersonalRowID { get; set; }
        public int ClientRowID { get; set; }

        public string CandidateName { get; set; }
        public string CandidateCode { get; set; }
        public string SecuritasID { get; set; }
        public string CheckName { get; set; }
        public string CheckStatus { get; set; }

        public string OrderDate { get; set; }
        public string CreatedDate { get; set; }
        public string OutDate { get; set; }
        public string CheckStatusID { get; set; }
        public string ColorName { get; set; }
        public int SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        public int ClientCheckRowID { get; set; }
        public string Name { get; set; }
    }

    public class FakeUniversity
    {
        public int SrNo { get; set; }
        public string CurrentStatus { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }


    public class FakeCompanies
    {
        public int SrNo { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
    }
    public class DashboardChart
    {
        public string month { get; set; }
        public string TotalCases { get; set; }
        public string ActiveCases { get; set; }
        public string Stopped { get; set; }
        public string TotalPrice { get; set; }
    }

    public class ColorCodeGraph
    {
        public string InitionMonth { get; set; }
        public string WIP { get; set; }
        public string Orange { get; set; }
        public string Green { get; set; }
        public string Red { get; set; }
        public string Nocolor { get; set; }
        public string CheckFamilyName { get; set; }
        public string Completed { get; set; }
        public string Total { get; set; }
        public string Department { get; set; }
        public string STOP { get; set; }





    }

    public class GetDynamicReport
    {
        public string SecuritasID { get; set; }        
        public int PersonalRowID { get; set; }
        public int ClientRowId { get; set; }
        public string FileName { get; set; }
        public string VerifiedBy { get; set; }
        public string VerifiedOn { get; set; }
        public string FilePath { get; set; }  
        public string DocumentType { get; set; }
        public string DocumentUploadFrom { get; set; }
        public string FatherName { get; set; }
        public string UniqueId { get; set; }
        public string ReportComment { get; set; }
        public string DOB { get; set; }
        public string CreatedBy { get; set; }
        public int StatusDocxConvert { get; set; }
        public DateTime UploadDate { get; set; }
        public System.Web.HttpPostedFileBase OFACDoc { get; set; }
        public System.Web.HttpPostedFileBase TGSExportDoc { get; set; }
        public System.Web.HttpPostedFileBase IDReportDoc { get; set; }
        public System.Web.HttpPostedFileBase OFACKeywordDoc { get; set; }
    }

    public class CheckAddAndReverification
    {
        public string SecuritasID { get; set; }
        public string Actions { get; set; }
        public int aSubCheckRowID { get; set; }
        public int rSubCheckRowID { get; set; }
        public string aSubCheckRowIDMultiple { get; set; }
        public string rSubCheckRowIDMultiple { get; set; }
        public string SubCheckName { get; set; }
        public string BillingStatus { get; set; }
        public string Remark { get; set; }
        public string ClientRowId { get; set; }
        public int PersonalRowID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int clientCheckRowID { get; set; }
        public bool approveStatus { get; set; }
        public System.Web.HttpPostedFileBase[] UploadDoc { get; set; }

        public string ProcessId { get; set; }
    }

    public class AgencyReverifyCases
    {
        public int Personalrowid { get; set; }
        public string ResumeID { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string ReverificationDate { get; set; }
        public string CheckDetails { get; set; }
        public string ReverificationRemarks { get; set; }
        public string Documents { get; set; }
    }

    public class ReverificationApproval
        {
        public string CandidateName { get; set; }
        public string SubCheckName { get; set; }
        public int SubCheckid { get; set; }
        public string ClientRowId { get; set; }
        public int PersonalRowID { get; set; }
        public string Remark { get; set; }
        public string SecuritasID { get; set; }
        public string approvalAction { get; set; }
        public string billingStatus { get; set; }

        }

    public class detailTran
    {
        public string companyRefNo { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<detailTran_List> checkTranDetails { get; set; }
        
    }

    public class detailTran_List
    {
        public string subCheckName { get; set; }
        public string oldStatus { get; set; }
        public string companyRefNo { get; set; }
        public string newStatus { get; set; }
        public string checkStatus { get; set; }
        public string CreationDate { get; set; }
        public string doneBy { get; set; }

    }

    public class GetClientDetails
    {
        public int ClientUserRowID { get; set; }
        public string UserID { get; set; }
        public byte ReportVisible { get; set; }
        public byte ReportVisibilityInterim { get; set; }
        public byte ReportVisibilityFinal { get; set; }        
        public int Status { get; set; }
        public int ClientRowID { get; set; }

    }
    public class ReportUploadOnOldPortal
    {
        public int ClientRowId { get; set; }
        public string CompanyRefNo { get; set; }
        public string ReportStatus { get; set; }
        public string ColorName { get; set; }
        public bool StatusYesNo { get; set; }
        public System.Web.HttpPostedFileBase ObjOldReport { get; set; }
    }
    public class TechSupportDetails
    {
        public string DocumentType { get; set; }
        public string ClientName { get; set; }
        public string CheckFamily { get; set; }
        public string SubCheckFamily { get; set; }
        public int CandidateDocumentVisibility { get; set; }
    }

    public class PurgDataDetailsModel
    {
        [Display(Name = "Client Name")]
        public int ClientRow { get; set; }
        [Display(Name = "Purg Date From")]
        public DateTime DateFrom { get; set; }
        [Display(Name = "Purg Date To")]
        public DateTime DateTo { get; set; }
        public string Attachement { get; set; }
        public bool ApprovalByOperationHead { get; set; }
        public bool ApprovalByIT { get; set; }
        public bool ReportDump { get; set; }
        public bool CheckDocDump { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ClientName { get; set; }
        public string CreatedDate { get; set; }
        public string RequestBy { get; set; }
        public string Remarks { get; set; }

        public string Id { get; set; }
    }


    public class ChangeClientPassword
    {
        public int Value { get; set; }
        public string ClientName { get; set; }
        public int ClientRowID { get; set; }
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }

        public string ID { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class UpdateClientPassword
    {
        public int WebUserRowID { get; set; }

        public int TeamMemerRowId { get; set; }

        public string EncPWD { get; set; }

        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.{14,50})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&_]).*$", ErrorMessage = "Please enter password between 14 to 50 character and password must contain at least one one Lower case letter, one Upper case letter, one Digit and one Special (!@#$%^&_) Characters.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long. and must start with an alphabet.", MinimumLength = 14)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.{14,50})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&_]).*$", ErrorMessage = "Please enter password between 14 to 50 character and password must contain at least one one Lower case letter, one Upper case letter, one Digit and one Special (!@#$%^&_) Characters.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long. and must start with an alphabet.", MinimumLength = 14)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }

    public class AnnexureInvoicing
    {
        public string BillingType { get; set; }

        [Required(ErrorMessage = "Billing Wise is required.")]
        public string BillingWise { get; set; }
        public string AnnexureUploadOptionFileName { get; set; }
        public string AnnexureUploadOptionFilePath { get; set; }
        public DateTime? AnnexureSentDate { get; set; }

        [Required(ErrorMessage = "Annexure Amount is required.")]
        [Range(0.01, 10000000.00, ErrorMessage = "Annexure Amount must be between 0.01 and 10000000.00.")]
        public double AnnexureAmount { get; set; }
        public Boolean ReminderSendOption { get; set; }
        public string AnnexureApprovedUploadOption { get; set; }
        public DateTime? AnnexureApprovedDate { get; set; }
        //public DateTime? OutDate { get; set; }
        public double AnnexureApprovedAmount { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceUploadOptionFile { get; set; }
        public DateTime? InvoiceSendDate { get; set; }
        public double InvoiceAmount { get; set; }
        public DateTime? PaymentReceivedDate { get; set; }
        public double PaymentReceivedAmount { get; set; }
        public string SettlementOption { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        [Display(Name = "Select Document :")]
        [Required]
        [ValidateLargeFileAttribute]
        [ValidateLargeFileCandidateUploadDocs]
        // public HttpPostedFileBase[] UploadDoc1 { get; set; }
        public HttpPostedFileBase UploadDoc1 { get; set; }
        public string ModifyedBy { get; set; }
        public DateTime? ModifyedDate { get; set; }
        public bool Status { get; set; }
        public string Template { get; set; }
        public int AnnexureInvoicingId { get; set; }
        public string ClientCode { get; set; }
        public string SpocName { get; set; }
        public string AnnexureInvoicingType { get; set; }
    }

    public class ChartDataPoint
    {
        public string Date { get; set; }
        public int Value { get; set; }
    }
    public class ChartDataPointCheck
    {
        public string CheckFamilyName { get; set; }
        public string CheckFamilyID { get; set; }
    }

    public class ClientDashboardCheckFamilyViewModel
    {
        public List<ChartDataPointCheck> ChartData1 { get; set; }
    }

    public class ClientDashBoardTable
    {
        public string ClientStatus { get; set; }
        public int fiveDays { get; set; }
        public int tenDays { get; set; }
        public int twentyDays { get; set; }
        public int fifteenDays { get; set; }
        public int twentyplusDays { get; set; }

        public int Total { get; set; }

    }

    public class DashBoardMonthCaseData
    {
        public string monthname { get; set; }
        public int mCount { get; set; }
    }

    public class DashBoardColorCodeCount
    {
        public string CColorName { get; set; }
        public int CCount { get; set; }
    }

    public class ClientDashBoardTableModel
    {
        public IEnumerable<ClientDashBoardTable> ClientDashBoardTable { get; set; }
        public IEnumerable<DashBoardMonthCaseData> DashBoardMonthCaseData { get; set; }
        public IEnumerable<DashBoardColorCodeCount> DashBoardColorCodeCount { get; set; }
    }

    public class QCRejectionClient
    {
        public int Personalrowid { get; set; }
        public string ResumeID { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string QCRejectionDate { get; set; }
        public string CheckDetails { get; set; }
        public string QCRejectionRemarks { get; set; }
        public string Documents { get; set; }
    }
}

