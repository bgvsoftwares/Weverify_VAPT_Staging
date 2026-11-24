using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace ViewModels.VerificationViewModel
{

    public class OperationViewModel
    {
        public Int64 PersonalRowID { get; set; }
        public int SubCheckRowID { get; set; }
        public int clientRowId { get; set; }
        public Int64 AntecedentRowID { get; set; }
        public string DisplayName { get; set; }
        public string DataEntryValue { get; set; }
        public string RepQcValue { get; set; }        
        public int CreatedBy { get; set; }
        public string moduleName { get; set; }
        public string reportcomment { get; set; }
    }
    public class VerificationViewModel
    {
        public DateTime? UserClosureDate { get; set; }
        public bool IsPreviouslyRejected { get; set; }
        public string AutoAllocation { get; set; }
        public string IsSeniorCase { get; set; }
        public string MethodName { get; set; }
        public string checkFackEdu { get; set; }
        public int PQAddressVarPtrRowId { get; set; }

        [ScaffoldColumn(false)]
        public int VerificationRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
        public string PersonalName { get; set; }

        //////////////////////////////////////
        
           public string TLAllocationDate { get; set; }
           public string CreatedDates { get; set; }
           public string completedDate { get; set; }
           public string CompletionDate { get; set; }
           public string CheckcompletedDate { get; set; }
        public string CurrentlyAllocatedTM { get; set; }
        public string AutoAllocatedTM { get; set; }
        public string CollegeeName { get; set; }

        public string UploadDate { get; set; }

        //////////////////////////////////////

        public string CandidateCode { get; set; }

        [ScaffoldColumn(false)]
        public int ClientPackageRowID { get; set; }
        public string ClientPackageName { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }
        public string CheckFamilyName { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }

        [ScaffoldColumn(false)]
        public string UniqueComponentID { get; set; }

        [ScaffoldColumn(false)]
        public string ClientRefID { get; set; }
        public string CompanyRefNo { get; set; }

        public string Severity { get; set; }
        public string ColorName { get; set; }

        public string StateName { get; set; }
        public string DistrictName { get; set; }

        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }       
        public DateTime? InternalOutDate { get; set; }
        public bool IsSynced { get; set; }
        public string CheckStatus { get; set; }
        public string PtrStatus { get; set; }
        public string ReWorkCheckStatus { get; set; }
        public short RWQCManagerBy { get; set; }
        public short RWQCTLBy { get; set; }
        public short RWQCTMBy { get; set; }
        public string RevwRequestBy { get; set; }
        public DateTime? RevwRequestDate { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public byte Status { get; set; }
        // Add new filed 22-07-2017
        public int Ageing { get; set; }
        public string SecuritasSPOC { get; set; }
        public string AllocatedTLName { get; set; }
        public string AllocatedTMName { get; set; }
        public string AllocatedQCName { get; set; }
        public string ActionHistoryName { get; set; }
        public string Location { get; set; }
        public string Partner { get; set; }
        public string ClientRemarks { get; set; }
        public int CheckStatusid { get; set; }
        public bool Aging { get; set; }
        public string Organization_Name { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Address { get; set; }
        public string VendorName { get; set; }
        public string AutoProcess { get; set; }
        public string OfficialEmailId { get; set; }
        public string CandCheckRowID { get; set; }
        public string MailStatus { get; set; }
        public string CaseStatus { get; set; }
        public string PartnerUserName { get; set; }
        public string GEO_Co { get; set; }
        public string CheckType { get; set; }
        public string spocremarks { get; set; }

        public string UniversityName { get; set; }
        public string collegeName { get; set; }
        public string Link { get; set; }
        public string YearOfPassing { get; set; }
        public string Coursename { get; set; }
        public string DurationOfCourse { get; set; }
        public string RollNo { get; set; }
        public string CaseType { get; set; }

        public string UAN { get; set; }
        public short CheckPublishInReport { get; set; }
        public string CandidateAsManager { get; set; }
        public string ReVerificationDate { get; set; }
        public string LastInsuffClearDate { get; set; }
        public string CaseStatusId { get; set; }

        //public DateTime?  TMQCAllocatedDate { get; set; }
        public string SubCheckFamilyId { get; set; }
        public string VendorMobileNo { get; set; }

      
        //public DateTime? TMAllocationDate { get; set; }      
        //public DateTime? CheckCompletionDate { get; set; }
        public string TMCompletionDate { get; set; }

        public string DateTLAllocation { get; set; }
        public string DateTMAllocation { get; set; }
        public string DateInitiation { get; set; }

        public string CheckCompletionDate { get; set; }
        public string TypeOfBusinesses { get; set; }
        public int CheckReadyForUploadReport { get; set; }
        public string ClientMapWithUserIdEdu { get; set; }
        public string ClientTAT { get; set; }
    }

    public class VerificationInfoListPagedModel
    {
        public IEnumerable<VerificationViewModel> PQVerificationInfo { get; set; }
        public IEnumerable<AutoRevertViewModel> AutoRevertViewModeldetail { get; set; }
        public IEnumerable<GroupAutoMailerViewModel> GroupAutoMailerDetail { get; set; }
        public IEnumerable<GroupAutoManualDetailViewModel> GroupAutoManualDetail { get; set; }
        public IEnumerable<EmailAutomationCheckStatus> EmailAutomationCheckStatusMIS { get; set; }
        public IEnumerable<FakeComp> FakeComp { get; set; }



        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
    public class EMPUancheckdata
    {
        public int VerificationId { get; set; }
        public string CandidateName { get; set; }
        public string OrganisationName { get; set; }
        public string EmployeeCode { get; set; }
        public string UANNumber { get; set; }
        public string DOJ { get; set; }
        public string DOL { get; set; }
        public string ModeOfVerification { get; set; }
        public string DateOfVerification { get; set; }

        public string Remarks { get; set; }
        public string Color_Code { get; set; }
    }
    public class AddEmploymentUANInfo
    {
        public string PersonalRowID { get; set; }
        public string SubCheckRowID { get; set; }
        public string ClientRowID { get; set; }
        public string CheckStatusdID { get; set; }

        public string CandidateName { get; set; }
        public string OrganisationName { get; set; }
        public string EmployeeCode { get; set; }
        public string UANNumber { get; set; }
        public string DOJ { get; set; }
        public string DOL { get; set; }
        public string ModeOfVerification { get; set; }
        public string DateOfVerification { get; set; }
        
        public string Remarks { get; set; }
        public string Color_Code { get; set; }
        
    }
    public class Comperative_TigerModel
    {
        public int PersonalRowId { get; set; }
        [Display(Name = "Company Name Resume")]
        public string CompanyName { get; set; }
        [Display(Name = "Start Date Resume")]
        public string StartDate { get; set; }
        [Display(Name = "End Date Resume")]
        public string EndDate { get; set; }
        [Display(Name = "Company Name Onboarding")]
        public string CompanyNameOnboarding { get; set; }
        [Display(Name = "Start Date Onboarding")]
        public string StartDateOnboarding { get; set; }
        [Display(Name = "End Date Onboarding")]
        public string EndDateOnboarding { get; set; }
        [Display(Name = "Company Name UAN")]
        public string CompanyNameUAN { get; set; }
        [Display(Name = "Start Date UAN")]
        public string StartDateUAN { get; set; }
        [Display(Name = "End Date UAN")]
        public string EndDateUAN { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Email Verification")]
        public string EmailVerification { get; set; }
        public IEnumerable<Comperative_Tigerlst> comperative_Tigerlsts { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class Comperative_Tigerlst
    {
        public int ID { get; set; }
        public int PersonalRowId { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        [Display(Name = "Company Name Onboarding")]
        public string CompanyNameOnboarding { get; set; }
        [Display(Name = "Start Date Onboarding")]
        public string StartDateOnboarding { get; set; }
        [Display(Name = "End Date Onboarding")]
        public string EndDateOnboarding { get; set; }
        [Display(Name = "Company Name UAN")]
        public string CompanyNameUAN { get; set; }
        [Display(Name = "Start Date UAN")]
        public string StartDateUAN { get; set; }
        [Display(Name = "End Date UAN")]
        public string EndDateUAN { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Email Verification")]
        public string EmailVerification { get; set; }
        public int PageSize { get; set; }
    }
    public class CandidatePersonalInfoViewModel
    {
        public int Personalrowid { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandidateName { get; set; }
        public string ClientRefNo { get; set; }
    }
    public class CaptureActionHistoryByVerTM
    {
        [ScaffoldColumn(false)]
        public int CheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }        

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }

        [Display(Name ="Check Status : ")]
        public string CheckStatus { get; set; }

        [Required]
        [Display(Name = "Action Comment : ")]
        public string ActionHistory { get; set; }

        
        [Display(Name = "Document Requirement (if any) : ")]
        public string DocumentRequired { get; set; }

        
        [Display(Name = "ExpectedTAT : ")]
        public string ExpectedTAT { get; set; }

        public short InfSuffRaiseBy { get; set; }
        public string Insufftype { get; set; }

        public HttpPostedFileBase ProofDocument { get; set; }   
        public DateTime? InfSuffRaisedDate { get; set; }
        public byte StoppedCheckStatus { get; set; }
        public short StoppedCheckBy { get; set; }
        public DateTime? StoppedCheckDate { get; set; }
        public decimal Amount { get; set; }
        public int checkStatusid { get; set; }
        public string EMailType { get; set; }

    }


    public class Insuff_FundsAllRemarks
    {
        [ScaffoldColumn(false)]
        public int CheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }


        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public int RemarksID { get; set; }
        public int CreatedByID { get; set; }
        public int CheckstatusId { get; set; }



    }

    public class ExportVerificationAddMgrViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID  { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public int Ageing { get; set; }
        public DateTime? TAT { get; set; }
        public string SPOC { get; set; }
        public string AllocatedTL { get; set; }
        public string AllocatedTM { get; set; }
        public string Partner { get; set; }
        public string Location { get; set; }
        public string CheckStatus { get; set; }
    }
    public class AllocationonAddMgrViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public string UniqueComponentId { get; set; }
        public DateTime? TAT { get; set; }
        public DateTime? InternalTAT { get; set; }
        public string CheckStatus { get; set; }
    }
    public class ExportVerificationAddTLViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public int Ageing { get; set; }
        public DateTime? TAT { get; set; }
        public string SPOC { get; set; }        
        public string AllocatedTM { get; set; }
        public string Partner { get; set; }
        public string Location { get; set; }
        public string CheckStatus { get; set; }
    }
    public class ExportVerificationAddTMViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public int Ageing { get; set; }
        public DateTime? TAT { get; set; }
        public string SPOC { get; set; }     
        public string Partner { get; set; }
        public string Location { get; set; }
        public string CheckStatus { get; set; }
    }

    public class ExportVerificationEduMgrViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public int Ageing { get; set; }
        public DateTime? TAT { get; set; }
        public string SPOC { get; set; }
        public string AllocatedTL { get; set; }
        public string AllocatedTM { get; set; }
        public string AllocatedQC { get; set; }        
        public string CheckStatus { get; set; }
    }
    public class ExportVerificationEduTLViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public int Ageing { get; set; }
        public DateTime? TAT { get; set; }
        public string SPOC { get; set; }       
        public string AllocatedTM { get; set; }
        public string AllocatedQC { get; set; }
        public string CheckStatus { get; set; }
    }
    public class ExportVerificationTMMgrViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public int Ageing { get; set; }
        public DateTime? TAT { get; set; }
        public string SPOC { get; set; }        
        public string CheckStatus { get; set; }
    }
    public class ExportVerificationQCMgrViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public int Ageing { get; set; }
        public DateTime? TAT { get; set; }
        public string SPOC { get; set; }
        public string CheckStatus { get; set; }
    }

    public class ExportPVerificationMgrViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public int Ageing { get; set; }
        public DateTime? TAT { get; set; }
        public string SPOC { get; set; }
        public string AllocatedTL { get; set; }
        public string AllocatedTM { get; set; }
        public string AllocatedQC { get; set; }
        public string CheckStatus { get; set; }
    }

    public class FakeComp
    {
        public int SrNo { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
    }

    public class PVExportVerificationMgrViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public int Ageing { get; set; }
        public DateTime? TAT { get; set; }
        public string SPOC { get; set; }
        public string AllocatedTL { get; set; }
        public string AllocatedTM { get; set; }
        public string AllocatedQC { get; set; }
        public string CheckStatus { get; set; }
    }

    public class PVExportMgrViewModel
    {
        public int VerificationRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public string UniqueComponentId { get; set; }
        public DateTime? TAT { get; set; }
        public DateTime? InternalTAT  { get; set; }
        public string Status { get; set; }
    }

    public class RWExportMgrViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }   
        public DateTime? CreatedDate { get; set; }
        public string Final { get; set; }
        public string Status { get; set; }
    }
    public class RWExportViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }
        public string CandidateName { get; set; }
        public string ClientRefID { get; set; }
        public string SecuritasID { get; set; }
        public string Status { get; set; }
    }
    public class TMExportMgrViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public DateTime? CreatedDate { get; set; }       
        public string Status { get; set; }
    }
    public class TMExportViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }      
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Status { get; set; }
    }

    public class TMCaseExportMgrViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }       
        public string CandidateName { get; set; }
        public string ClientRefID  { get; set; }
        public string SecuritasID { get; set; }
        public string Final { get; set; }
        public string Status { get; set; }
    }

    public class ExportVerificationAddressViewModel
    {
        


        public string ClientName { get; set; }
        public string CompanyRefNo { get; set; }
        public string Name { get; set; }
        public string AddressType { get; set; }
        public string address { get; set; }
        public string AV_Pincode { get; set; }
        public string Location { get; set; }
        public string StateName { get; set; }
        public string MobileNo { get; set; }
        public string FatherName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Spoc_name { get; set; }



    }

    public class ExportVerificationSpecialViewModel
    {

        public string ClientName { get; set; }
        public string CompanyRefNo { get; set; }
        public string Name { get; set; }
        public string SpecialType { get; set; }
        public string address { get; set; }
        public string AV_Pincode { get; set; }
        public string Location { get; set; }
        public string StateName { get; set; }
        public string MobileNo { get; set; }
        public string FatherName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Spoc_name { get; set; }


    }


    public class ExportVerificationNationalViewModel
    { 


        public string ClientName { get; set; }
        public string CompanyRefNo { get; set; }
        public string Name { get; set; }
        public string CheckName { get; set; }
        public string Id_Type { get; set; }
        public string Id_No { get; set; }
        public string Spoc_name { get; set; }        


    }
    public class AddPageAutomation
    {
        public string Pagename { get; set; }

        public int VerifiedUploadRowID { get; set; }
        public string DocReqValue { get; set; }
    }
    public class ReminderMail
    {
        public int ClientRowID { get; set; }
        public int SubCheckRowID { get; set; }
        public int PersonalRowID { get; set; }
        public string subject { get; set; }
      
        public string Tomail { get; set; }
       
        public string ccmail { get; set; }
        public string Body { get; set; }
        public string Follow_Body { get; set; }
        public string ReminderStatus { get; set; }
        public int CheckStatusId { get; set; }
        public string UniqueComponentId { get; set; }
        public DateTime Creation_Date { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }       
        public HttpPostedFileBase UploadDoc { get; set; }
        public HttpPostedFileBase[] UploadDocedu { get; set; }
        public string GroupId { get; set; }
        public string CandCheckRowID { get; set; }
        public int AutoId { get; set; }
        public int BounceId { get; set; }
        public int ReminderInterval { get; set; }
        public string CompanyName { get; set; }
    }

    public class AutoRevertViewModel
    {
        public int id { get; set; }
        public int PersonalRowId { get; set; }        
        public string Subject { get; set; }
        public int Status { get; set; }
        public string MailStatus { get; set; }
        public string AttachmentPath { get; set; }
        public string companyRefNo { get; set; }
        public string CheckName { get; set; }
        public string CandCheckId { get; set; }
        public string CreationDate { get; set; }
    }

    public class GroupAutoMailerViewModel
    {
       
        public string GroupId { get; set; }
        public int ChecksCount { get; set; }
        public int CheckStatusId { get; set; }
        public string CheckStatus { get; set; }


    }

    public class AutoEmploymentDetail
    {        
        public int PersonalRowId { get; set; }
        public int SubCheckRowId { get; set; }
        public int ClientRowID { get; set; }
        public string ClientAbbreviation { get; set; }
        public string CompanyName { get; set; }
        //public string Abbreviations { get; set; }
        public int AbbCount { get; set; }
    }
    public class GroupAutoManualDetailViewModel
    {
        public string CandidateName { get; set; }
        public string FatherName { get; set; }
        public string DOB { get; set; }
        public string UniqueComponentID { get; set; }
    }

    public class EmailAutomationCheckStatus
    {
        public string CompanyRefNo              { get; set; }
        public string CheckName                 { get; set; }
        public string Subject                   { get; set; }
        public string CheckStatus               { get; set; }
        //public string MailStatus                { get; set; }
        public string UpdatedOn                 { get; set; }
        public string MainStatus { get; set; }
        public string SubStatus { get; set; }

     


    }
    public class geoModel
    {
        public string address { get; set; }
        public double latitude { get; set; }
        public double Longitude { get; set; }
        public int PersonalRowID { get; set; }
        public int SubCheckRowID { get; set; }

    }

    public class AddressMIS
    {

        public string Todate { get; set; }
        public string Fromdate { get; set; }
        public string Department { get; set; }
    }


    
    public class checkViewModel
    {
        public IEnumerable<CheckSummaryDetailViewModel> ClientServiceChecks { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
    public class CheckSummaryDetailViewModel
    {
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


    public class WiproUANCheckViewModel
    {
        public IEnumerable<WiproUANCheck> WiproUANChecklist { get; set; }
        public WiproUANCheck WiproUANCheck { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class WiproUANCheck
    {
        public string PersonalrowId { get; set; }
        public string Employment { get; set; }
        public string UAN_DOJ { get; set; }
        public string UAN_LWD { get; set; }
        public string HR_DOJ { get; set; }
        public string HR_LWD { get; set; }
        public string IVerify_DOJ { get; set; }
        public string IVerify_LWD { get; set; }
        public string iVerify_Y_N { get; set; }
        public string CompanyName { get; set; }
    }

    public class WiproGapCheckViewModel
    {
        public IEnumerable<WiproGapCheck> WiproGapChecklist { get; set; }
        public WiproGapCheck WiproGapCheck { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class WiproGapCheck
    {
        public string PersonalrowId { get; set; }
        public string GapType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string GapTo { get; set; }
        public string GapFrom { get; set; }
        public string Remarks { get; set; }
    }
}


public class ExportVerificationCriminalViewModel
{
    public int VerificationRowID { get; set; }
    public string ClientName { get; set; }
    public string SecuritasID { get; set; }
    public string CandidateName { get; set; }
    public string CheckName { get; set; }
    public string UniqueComponentId { get; set; }
    public int TAT { get; set; }
    public DateTime? InternalTAT { get; set; }
    public string CheckStatus { get; set; }
    public string DOB { get; set; }
    public string FatherName { get; set; }
    public string Address { get; set; }
    public string Spoc_name { get; set; }
}

public class Datefilter
{
    public int ClientRowId { get; set; }
    public string fromdate { get; set; }
    public string Todate { get; set; }
   
}

public class CheckVerificationViewModel
{
    public int PersonalRowId { get; set; }
    public string CompanyRefNo { get; set; }
    public int ClientRowId { get; set; }
    public string ClientName { get; set; }
    public IEnumerable<CheckVerificationList> CheckVerificationList { get; set; }
}

public class CheckVerificationList
{
    public int PersonalRowId { get; set; }
    public int CandCheckRowId { get; set; }
    public int CheckStatusId { get; set; }
    public string CheckStatusName { get; set; }
    public int SubCheckRowId { get; set; }
    public string CheckName { get; set; }
    public string CandidateFullName { get; set; }
    public string ClientName { get; set; }
   
    public string CompanyRefNo { get; set; }
}


public class StopCheckVerificationView
{
    public int PersonalRowId { get; set; }
    public int ClientRowId { get; set; }
    public int CheckFamilyRowId { get; set; }
    public int CandCheckRowId { get; set; }
    public int CheckStatusId { get; set; }
    public int SubCheckRowId { get; set; }
    public string CheckName { get; set; }
    public string ClientName { get; set; }
    public string CandidateFullName { get; set; }
    public string CompanyRefNo { get; set; }
    public int DispositionRowID { get; set; }
    public int SeverityRowID { get; set; }
    public string ColorCode { get; set; }
    public string CheckCompletionDate { get; set; }
    public string ReportComments { get; set; }

}



public class UANCheckAPIResults
{
    public int AutoId { get; set; }
    public string CompanyRefNo { get; set; }
    public string UANNO { get; set; }
    public string status { get; set; }
    public string message { get; set; }
    public string reference_id { get; set; }
    public DateTime ReportDate { get; set; }

    public string name { get; set; }
    public string guardian_name { get; set; }
    public string establishment_name { get; set; }
    public string member_id { get; set; }
    public string date_of_joining { get; set; }
    public string date_of_exit { get; set; }
}

public class FeedbackExport
{

    public string Todate { get; set; }
    public string Fromdate { get; set; }
    public string Department { get; set; }
}
public class SpocActionRemark
{
    public int PersonalRowID { get; set; }
    public int SubCheckRowId { get; set; }
    public string SpocRemark { get; set; }
    public string PriorityType { get; set; }
}


public class OnlineLinkSearchingPageModel
{
    public IEnumerable<OnlineLinkSearching> onlineLinkSearching { get; set; }   

    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
}

public class OnlineLinkSearching
{
    public int Id { get; set; }
    public string CollageName { get; set; }

    public string UniversityName { get; set; }

    public string CourseName { get; set; }

    public int YearOfPassing { get; set; }

    public string RollNo { get; set; }

    public string ResultScore { get; set; }

    public string Convocation { get; set; }

    public int Year { get; set; }

    public string Link { get; set; }
}
public class GetDigiLockerListDetails
{
    public IEnumerable<GetDigiLockerDetails> DigiLockerDataDetails { get; set; }
    public int TotalRecords { get; set; }
}

public class GetDigiLockerDetails
{
    public string CompanyRefNo { get; set; }
    public string CandidateName { get; set; }
    public string UserName { get; set; }
    public string CandidateStatus { get; set; }
    public string Personalrowid { get; set; }
    public string Createddate { get; set; }
    
    public string CheckUniqueID { get; set; }
    public string TypeOfEducation { get; set; }
    public string UniversityName { get; set; }
    public string CourseName { get; set;}
    public string CheckStatus { get; set; }
    public string RequestRaiseDate { get; set ; }
    public string RequestClosedDate { get; set; }
}
public class GetBounceListDetails
{
    public IEnumerable<GetBounceDetails> GetBounceDetails { get; set; }
    public int TotalRecords { get; set; }
}

public class GetBounceDetails
{
    public string CompanyRefNo { get; set; }
    public string CandidateName { get; set; }
    public string Reason { get; set; }
    public string SendEmail { get; set; }
    public string Fromaddress { get; set; }
    public string CreatedDate { get; set; }
}

public class GetDominosListDetails
{
    public IEnumerable<GetDominosDetails> DominosDetails { get; set; }
    public int TotalRecords { get; set; }
}
public class GetDominosDetails
{
    public string Mobile_No { get; set; }
    public string LinkSendDate { get; set; }
    public string CandidateStatus { get; set; }
    public string Closedate { get; set; }
}
public class GetDigiLockerDocumentlistDetails
{
    public IEnumerable<GetDigiLockerDocumentDetails> GetDigiLockerDocumentlist { get; set; }
    public int TotalRecords { get; set; }

}

public class GetDigiLockerDocumentDetails
{
    public string FileName { get; set; }
    public string CreateDate { get; set; }
    public string Personalrowid { get; set; }
}
public class CompressimageViewModel
{
    [ValidateLargeFileCandidateUploadDocs]
    public HttpPostedFileBase UploadDoc { get; set; }
}








