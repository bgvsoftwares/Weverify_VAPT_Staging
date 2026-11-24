using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace ViewModels.ClientViewModel
{
   public class PQClientLoginViewModel
    {
        public int ClientUserRowID { get; set; }
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string ClientBranch { get; set; }
        public string UserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public byte Status { get; set; }
        public byte SentMailStatus { get; set; }
        public DateTime? SentMailDate { get; set; }
        public bool ReportVisible { get; set; }
    }
    public class PQClientLoginViewModelNew
    {
        public int ClientUserRowID { get; set; }
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string ClientBranch { get; set; }
        public string UserID { get; set; }
        public string UPass { get; set; }
        public string UserEmail { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedTime { get; set; }
        public string ModifiedTime { get; set; }
        public byte Status { get; set; }
        public byte SentMailStatus { get; set; }
        public string SentMailDate { get; set; }
        public bool ReportVisible { get; set; }
    }
    public class ClientLoginDetailsForMailViewModel
    {
        public int ClientUserRowID { get; set; }
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string UserID { get; set; }
        public string UPass { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public DateTime? BlockedDate { get; set; }
        public short UnBlockedBy { get; set; }
        public DateTime? UnBlockedDate { get; set; }
        public string UType { get; set; }
        public string UserType { get; set; }
        public byte Status { get; set; }
        public byte SentMailStatus { get; set; }
        public string SentMailStatusYesNo { get; set; }
        public DateTime? SentMailDate { get; set; }
        public string Remarks { get; set; }
        public short CreatedUserId { get; set; }
    }
    public class AddPQClientUserLoginViewModel
    {
        [Display(Name = "ID")]
        [ScaffoldColumn(false)]
        public int ClientUserRowID { get; set; }

        [Required]
        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [Display(Name = "Branch Office")]
        public string ClientBranch { get; set; }

        [Required]
        [Display(Name = "User ID :")]
        public string UserID { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.{14,50})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&_]).*$", ErrorMessage = "Please enter password between 14 to 50 character and password must contain at least one one Lower case letter, one Upper case letter, one Digit and one Special (!@#$%^&_) Characters.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long. and must start with an alphabet.", MinimumLength = 14)]
        [DataType(DataType.Password)]
        [Display(Name = "Password :")]
        public string UPass { get; set; }

        public string EncPWD { get; set; }

        [Display(Name = "User Email :")]
        public string UEmail { get; set; }

        [Display(Name = "Created By :")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Time :")]
        public DateTime? CreatedTime { get; set; }

        [Display(Name = "Modified Time :")]
        public DateTime? ModifiedTime { get; set; }

        [Display(Name = "Blocked Date :")]
        public DateTime? BlockedDate { get; set; }

        [Display(Name = "UnBlocked By :")]
        public short UnBlockedBy { get; set; }

        [Display(Name = "UnBlocked Date :")]
        public DateTime? UnBlockedDate { get; set; }

        [Display(Name = "User Type :")]
        public string UType { get; set; }

        [Display(Name = "User Type :")]
        public string UserType { get; set; }

        [Display(Name = "Status :")]
        [ScaffoldColumn(false)]
        public byte Status { get; set; }

        [Display(Name = "Sent Mail Status :")]
        public byte SentMailStatus { get; set; }

        [Display(Name = "Sent Mail Date :")]
        public DateTime? SentMailDate { get; set; }

        [Display(Name = "Remarks :")]
        public string Remarks { get; set; }

        [Display(Name = "Created User Id :")]
        public short CreatedUserId { get; set; }

        public string CapImage { get; set; }
        [Required(ErrorMessage = "Captcha is required.")]
        [System.ComponentModel.DataAnnotations.Compare("CapImageText",
            ErrorMessage = "Captcha code Invalid")]
        public string CaptchaCodeText { get; set; }
        public string CapImageText { get; set; }


        [Display(Name = "Report Visibility Interim :")]
        public bool ReportVisibilityInterim { get; set; }
        [Display(Name = "Report Visibility Final :")]
        public bool ReportVisibilityFinal { get; set; }
        [Display(Name = "Case Initiation Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string CaseInitiationMail { get; set; }
        [Display(Name = "Insuff Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendInsuffEmail { get; set; }
        [Display(Name = "Funds Approved Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendFundsApprovedMail { get; set; }
        [Display(Name = "Report Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendReportEmail { get; set; }
        [Display(Name = "Send YTR Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendYTRMail { get; set; }
        [Display(Name = "Red Report Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendRedReportEmail { get; set; }
        [Display(Name = "Candidate Include In Insuff Mail :")]
        public bool CandidateIncludeInInsuffMail { get; set; }
    }
    public class AddPQClientLoginViewModel
    {
        [Display(Name ="ID")]
        [ScaffoldColumn(false)]
        public int ClientUserRowID { get; set; }

        [Required]
        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }     
        public string ClientName { get; set; }

        [Display(Name ="Branch Office")]
        public string ClientBranch { get; set; }

        [Required]
        [Display(Name = "User ID :")]
        public string UserID { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.{14,50})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&_]).*$", ErrorMessage = "Please enter password between 14 to 50 character and password must contain at least one one Lower case letter, one Upper case letter, one Digit and one Special (!@#$%^&_) Characters.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long. and must start with an alphabet.", MinimumLength = 14)]
        [DataType(DataType.Password)]
        [Display(Name = "Password :")]
        public string UPass { get; set; }

        public string EncPWD { get; set; }

        [Display(Name = "Created By :")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Time :")]
        public DateTime? CreatedTime { get; set; }

        [Display(Name = "Modified Time :")]
        public DateTime? ModifiedTime { get; set; }

        [Display(Name = "Blocked Date :")]
        public DateTime? BlockedDate { get; set; }

        [Display(Name = "UnBlocked By :")]
        public short UnBlockedBy { get; set; }

        [Display(Name = "UnBlocked Date :")]
        public DateTime? UnBlockedDate { get; set; }

        [Display(Name = "User Type :")]
        public string UType { get; set; }

        [Display(Name = "User Type :")]
        public string UserType { get; set; }

        [Display(Name = "Status :")]
        [ScaffoldColumn(false)]
        public byte Status { get; set; }


        [Display(Name = "Sent Mail Status :")]
        public byte SentMailStatus { get; set; }

        [Display(Name = "Sent Mail Date :")]
        public DateTime? SentMailDate { get; set; }

        [Display(Name = "Remarks :")]
        public string Remarks { get; set; }

        [Display(Name = "Created User Id :")]
        public short CreatedUserId { get; set; }

        public string CapImage { get; set; }
        [Required(ErrorMessage = "Captcha is required.")]
        [System.ComponentModel.DataAnnotations.Compare("CapImageText",
            ErrorMessage = "Captcha code Invalid")]
        public string CaptchaCodeText { get; set; }
        public string CapImageText { get; set; }

        [Required]
        [Display(Name = "Report Visibility Interim :")]
        public bool ReportVisibilityInterim { get; set; }
        [Required]
        [Display(Name = "Report Visibility Final :")]
        public bool ReportVisibilityFinal { get; set; }
        [Required]
        [Display(Name = "Case Initiation Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string CaseInitiationMail { get; set; }
        [Required]
        [Display(Name = "Insuff Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendInsuffEmail { get; set; }
        [Required]
        [Display(Name = "Funds Approved Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendFundsApprovedMail { get; set; }
        [Required]
        [Display(Name = "Report Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendReportEmail { get; set; }
        [Required]
        [Display(Name = "Send YTR Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendYTRMail { get; set; }
        [Required]
        [Display(Name = "Red Report Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendRedReportEmail { get; set; }
        [Required]
        [Display(Name = "Candidate Include In Insuff Mail :")]
        public bool CandidateIncludeInInsuffMail { get; set; }
    }

    public class UpdatePQClientLoginViewModel
    {
        [Display(Name = "ID")]
        [ScaffoldColumn(false)]
        public int ClientUserRowID { get; set; }

        [Required]
        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [Display(Name = "User Name :")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "User ID :")]
        public string UserID { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{14,50}$", ErrorMessage = "Please enter password between 14 to 50 character and password must contain at least one one Lower case letter, one Upper case letter, one Digit and one Special (!@#$%^&_) Characters.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long. and must start with an alphabet.", MinimumLength = 14)]
        [DataType(DataType.Password)]
        [Display(Name = "Password :")]
        public string UPass { get; set; }
        public string EncPWD { get; set; }

        [Display(Name = "Created By :")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Time :")]
        public DateTime? CreatedTime { get; set; }

        [Display(Name = "Modified Time :")]
        public DateTime? ModifiedTime { get; set; }

        [Display(Name = "Blocked Date :")]
        public DateTime? BlockedDate { get; set; }

        [Display(Name = "UnBlocked By :")]
        public short UnBlockedBy { get; set; }

        [Display(Name = "UnBlocked Date :")]
        public DateTime? UnBlockedDate { get; set; }

        [Display(Name = "User Type :")]
        public string UType { get; set; }

        [Display(Name = "User Type :")]
        public string UserType { get; set; }

        [Display(Name = "Status :")]
        [ScaffoldColumn(false)]
        public byte Status { get; set; }

        [Display(Name = "Sent Mail Status :")]
        public byte SentMailStatus { get; set; }

        [Display(Name = "Sent Mail Date :")]
        public DateTime? SentMailDate { get; set; }

        [Display(Name = "Remarks :")]
        public string Remarks { get; set; }

        [Display(Name = "Created User Id :")]
        public short CreatedUserId { get; set; }

        [Display(Name = "Report Visibility Interim :")]
        public bool ReportVisibilityInterim { get; set; }
        [Display(Name = "Report Visibility Final :")]
        public bool ReportVisibilityFinal { get; set; }
        [Display(Name = "Case Initiation Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})(\s*,\s*[a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string CaseInitiationMail { get; set; }
        [Display(Name = "Insuff Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendInsuffEmail { get; set; }
        [Display(Name = "Funds Approved Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendFundsApprovedMail { get; set; }
        [Display(Name = "Report Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendReportEmail { get; set; }
        [Display(Name = "Send YTR Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendYTRMail { get; set; }
        [Display(Name = "Red Report Mail :")]
        [RegularExpression(@"^([a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*,\s*[a-zA-Z0-9._%+#-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$", ErrorMessage = "Please enter correct email")]
        public string SendRedReportEmail { get; set; }
        [Display(Name = "Candidate Include In Insuff Mail :")]
        public bool CandidateIncludeInInsuffMail { get; set; }

    }

    public class UpdateClientSendMailStatusViewModel
    {
        public int ClientUserRowID { get; set; }
        public byte SentMailStatus { get; set; }
        public DateTime? SentMailDate { get; set; }
    }

    public class SendClientLoginInfoViewModel
    {
        public string ClientSpocName { get; set; }
        public string ClientSpocEmail { get; set; }
        public string UserID { get; set; }
        public string UPass { get; set; }
        public int ClientRowID { get; set; }
    }

    public class ClientDDLViewModel
    {
        [Display(Name = "Client Name")]
        public int ClientRowID { get; set; }
        public string ClientName { get; set; }
    }
    public class BusinessUnitView
    {        
        public int BURowId { get; set; }
        public string BUName { get; set; }
    }
    
    public class ClientDDLViewModelEnc
    {
        [Display(Name = "Client Name")]
        public String ClientRowID { get; set; }
        public string ClientName { get; set; }
    }


    public class DocumentDDLViewModel
    {
        [Display(Name = "Client Name")]
        public string DocumentType { get; set; }
       
    }

    public class ClientDesignationsDDLViewModel
    {
        [Display(Name = "Designation Name")]
        public short DesignationRowID { get; set; }

        public string DesignationName { get; set; }
    }

    public class PQClientUserListPagedModel
    {
        public IEnumerable<PQClientLoginViewModel> ClientUsers { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
    public class PQClientUserListPagedModelNew
    {
        public IEnumerable<PQClientLoginViewModelNew> ClientUsers { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public PQPersonalSearchPagedModel ItemSearch { get; set; }
    }
    public class UnBlockedClientUsersViewModel
    {
        public short ClientUserRowID { get; set; }

        public short UnBlockedBy { get; set; }

        public DateTime? UnBlockedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Status")]
        public byte Status { get; set; }
    }

    public class UnBlockedPartnerUsersViewModel
    {
        public short PartnerUsersRowID { get; set; }

        public short UnBlockedBy { get; set; }

        public DateTime? UnBlockedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Status")]
        public byte Status { get; set; }
    }

    public class ExportClientLoginViewModel
    {
        public int ClientUserRowID { get; set; }       
        public string ClientName { get; set; }       
        public string UserID { get; set; }
        public string CreatedBy { get; set; }       
        public string SentMailStatus { get; set; }
        public DateTime? SentMailDate { get; set; }
    }

    public class DuplicateCandidateTicket
    {       
        public string TicketNo { get; set; }
    }

    public class ClientLatestCasesInitiatedToCandidate
    {
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Lastname { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string PersonalRowId { get; set; }
        public string CreationDate { get; set; }
    }
}
