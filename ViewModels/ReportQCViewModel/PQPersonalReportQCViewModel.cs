using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
   public class PQPersonalReportQCViewModel
    {
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        public string IsSeniorCase { get; set; }

        [Display(Name = "Client Ref ID : ")]
        public string ClientRefID { get; set; }

        public string CRPUserName { get; set; }
        public string CompanyRefNo { get; set; }
         public string CandidateCode { get; set; } 
        public string CandTitle { get; set; }
        public string TMFirstName { get; set; }


        [Required]
        [Display(Name = "Name : ")]
        public string Name { get; set; }

        [Display(Name = "Middle Name : ")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        public string LastName { get; set; }

        [Display(Name = "Details Entered By : ")]
        public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate


        [Display(Name = "Create By : ")]
        public short CreatedBy { get; set; }

        [Display(Name = "Create Date : ")]
        public DateTime? CreatedDate { get; set; }
        
        public string CaseStatus { get; set; }
        public string CreatedBySource { get; set; }
        public string FinalStatus { get; set; }
        public string FinalColor { get; set; }
        public string RWQCName { get; set; }
        public byte InterimReport { get; set; }

        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        public byte Status { get; set; }

        [Display(Name = "OtherDetails : ")]
        public string OtherDetails { get; set; }

        [Display(Name = "OtherDetails1 : ")]
        public string OtherDetails1 { get; set; }

        [Display(Name = "OtherDetails2 : ")]
        public string OtherDetails2 { get; set; }

        public bool IsReportUploaded { get; set; }
        public string ReportUploadedName { get; set; }
        public DateTime? ReportUploadedDate { get; set; }
        public string Interim_Allocation { get; set; }
        public string AllocatedTmem { get; set; }
        public string ReportPart { get; set; }
        public string SpocName { get; set; }
        public string ReportType { get; set; }
        public string Caseageing { get; set; }
        public string SubCheckName { get; set; }
        public int SubCheckRowid { get; set; }
        public string AllocationTime { get; set; }
        public string TeamMemberName { get; set; }
    }

    public class PQPersonalReportQCListPagedModel
    {
        public IEnumerable<PQPersonalReportQCViewModel> PQPersonalForReportQC { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class RWOwnAllocationCases
    {
        public IEnumerable<RWOwnAllocationCase> RWOwnAllocationCase { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
    public class RWOwnAllocationCase
    {
        public int PersonalRowId { get; set; }
        public string CandidateName { get; set; }
        public string CompanyRefNo { get; set; }

        public string clientRowid { get; set; }

        public DateTime CreatedOn { get; set; }
        public string Aging { get; set; }
        public string clientName { get; set; }
        public string clientRefId { get; set; }
        public string InternalOutDate {get; set;}



    }
    public class RwqcTrackerUserData
    {
        public IEnumerable<RwqcTrackerUserDatas> RwqcTrackerUserDatas { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
 
    public class RwqcTrackerUserDatas
    {
        public string  Id { get; set; }
        public string ClientName { get; set; }
        public string RWQC_Teammember { get; set; }
        public string Allocation_Percentage { get; set; }
        public string Total_Allocation { get; set; }
        public string Total_Active_Allocation { get; set; }
        public string Enable { get; set; }

        public string RwteamMember { get; set; }
    }

    public class ReportQCAllocationViewModel
    {
        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }

        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        
        public short RWQCTeamMemberBy { get; set; }
        public DateTime? RWQCTeamMemberDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Check Status : ")]
        public string CheckStatus { get; set; }

        public int SubCheckRowID { get; set; }

    }
    public class PageRaddarViewModels
    {
        public int PersonalRowID { get; set; }
        public string CandidateName { get; set; }
        public string dob { get; set; }
        public string fatherName { get; set; }
        public short ClientRowID { get; set; }
        public string TLAllocatedTo { get; set; }
        public string ClientRefID { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateCode { get; set; }
        public short CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string OrderDate { get; set; }
        public string DetailsEnterdBy { get; set; }
        public string ClientPackageName { get; set; }
        public string ActiveStatus { get; set; }
        public string ReportUploadedName { get; set; }
        public bool IsReportUploaded { get; set; }
        public string EmailID { get; set; }
        public string CaseStatus { get; set; }
        public string MobileNo { get; set; }
        public string EmployeeID { get; set; }
    }
    public class PageRaddarSearchListPagedModel
    {
        public IEnumerable<PageRaddarViewModels> lstPageRaddarViewModels { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string TicketId { get; set; }
    }


    public class RWRejectionTrackerViewModels
    {
        public string SPOC { get; set; }
        public string RWAssignedDate { get; set; }
        public string Client { get; set; }
        public string SecuritasID {get; set;}
        public short ClientRowID { get; set; }
        public string CandidateName { get; set; }
        public string RWEmployeecode { get; set; }
        public string RWName { get; set; }
        public string CheckName { get; set; }
        public string RWRejectionDate { get; set; }
        public string Rejectionreason { get; set; }
        public string Team { get; set; }
    }
    public class RWRejectionTrackerListPagedModel
    {
        public IEnumerable<RWRejectionTrackerViewModels> lstRWRejectionTrackerViewModels { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string TicketId { get; set; }
    }

    public class ReportWritingProductivityViewModels
    {
        public string SPOC { get; set; }
        public string RWAssignedDate { get; set; }
        public string Client { get; set; }
        public string SecuritasID { get; set; }
        public short ClientRowID { get; set; }
        public string CandidateName { get; set; }
        public string RWEmployeecode { get; set; }
        public string RWName { get; set; }
        public string NoofChecks { get; set; }
        public string RWDoneDate { get; set; }
        public string ReportStatus { get; set; }
        public string Colorcode { get; set; }
    }
    public class ReportWritingProductivityListPagedModel
    {
        public IEnumerable<ReportWritingProductivityViewModels> lstReportWritingProductivityViewModels { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string TicketId { get; set; }
    }

    public class UpdateEmployementTierStatusViewModel
    {
        public int PersonalRowID { get; set; }
        public int ClientRowID { get; set; }       
        public int SubCheckRowID { get; set; }
        public int SubCheckName { get; set; }
        public int CheckRowID { get; set; }
        [Display(Name = "Additional Comments : ")]
        public string AdditionalComments { get; set; }
        public string AdditionalCommentsStatus { get; set; }
        [Display(Name = "Type of Document : ")]
        public string TypeofDocument { get; set; }
        public string TypeofDocumentStatus { get; set; }
        [Display(Name = "Candidate Name : ")]
        public string CandidateName { get; set; }       
        [Display(Name = "Company Name : ")]
        public string CompanyName { get; set; }
        public string CompanyNameStatus { get; set; }
        [Display(Name = "PF No./TDS Certificate Number : ")]
        public string PFNo_TDSCertificateNumber { get; set; }
        public string PFNo_TDSCertificateNumberStatus { get; set; }
        [Display(Name = "PAN number of Company : ")]
        public string PANnumberofCompany { get; set; }
        public string PANnumberofCompanyStatus { get; set; }
        [Display(Name = "TAN number of Company : ")]
        public string TANnumberofCompany { get; set; }
        public string TANnumberofCompanyStatus { get; set; }
        [Display(Name = "PAN number of Employee : ")]
        public string PANnumberofEmployee { get; set; }
        public string PANnumberofEmployeeStatus { get; set; }
        [Display(Name = "Assessment Year : ")]
        public string AssessmentYear { get; set; }
        public string AssessmentYearStatus { get; set; }
        [Display(Name = "Undertaking : ")]
        public string Undertaking { get; set; }
        public string UndertakingStatus { get; set; }
        [Display(Name = "Sealand Signature : ")]
        public string SealandSignature { get; set; }
        public string SealandSignatureStatus { get; set; }


    }
}
