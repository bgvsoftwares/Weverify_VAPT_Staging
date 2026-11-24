using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels.CandidateViewModel
{
    public class PQCandidateLoginViewModel
    {
        [Display(Name ="ID")]
        [ScaffoldColumn(false)]
        public int CandidateUserRowID { get; set; }

       
        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }
       
        [Display(Name = "Clien tName")]
        public string ClientName { get; set; }

       
        [Display(Name = "Candidate Name")]
        public string CandidateName { get; set; }

       
        [Display(Name = "Candidate Email")]
        public string CandidateEmail { get; set; }

        [Display(Name = "Candidate EmailCC")]
        public string CandidateEmailCC { get; set; }

       
        [Display(Name = "UserID ")]
        public string UserID { get; set; }

       
        [Display(Name = "Password ")]
        public string UPass { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Time")]
        public DateTime? CreatedTime { get; set; }

        [Display(Name = "Modified Time")]
        public DateTime? ModifiedTime { get; set; }

        [Display(Name = "Blocked Date")]
        public DateTime? BlockedDate { get; set; }

        [Display(Name = "UnBlocked By")]
        public short UnBlockedBy { get; set; }

        [Display(Name = "UnBlocked Date")]
        public DateTime? UnBlockedDate { get; set; }

        [Display(Name = "UserType ")]
        public byte UserType { get; set; }
        public string UserTypeYesNo { get; set; }

        [Display(Name = "Status")]
        public byte Status { get; set; }

        [Display(Name = "EmployeeID")]
        public string EmployeeID { get; set; }

        [Display(Name = "SentMail Status")]
        public byte SentMailStatus { get; set; }
        public string SentMailStatusYesNo { get; set; }          

        [Display(Name = "SentMail Date")]
        public DateTime? SentMailDate { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Cantact No")]
        public string CantactNo { get; set; }

        [Display(Name = "Alternatvie No")]
        public string AlternatvieNo { get; set; }

        [Display(Name = "Created UserId")]
        public short CreatedUserId { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "YTR Applicable")]
        public byte YTRApplicable { get; set; }
        public string YTRApplicableYesNo { get; set; }
    }

    public class AddPQCandidateLoginViewModel
    {
        [ScaffoldColumn(false)]
        [Display(Name = "ID")]
        public int CandidateUserRowID { get; set; }

        [Required]
        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }
        
        [Required]
        [Display(Name = "Candidate Name :")]
        public string CandidateName { get; set; }

        [Display(Name = "Employee ID :")]
        public string EmployeeID { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}", ErrorMessage = "Please enter correct email")]
        [Display(Name = "Candidate Email :")]
        public string CandidateEmail { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[7-9]{1}.*[0-9]{9}$", ErrorMessage = "Please enter valid Mobile Number")]
        [Display(Name = "Contact No :")]
        public string CantactNo { get; set; }

        [Required]
        [Display(Name = "User Id :")]
        public string UserID { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.{6,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&_]).*$", ErrorMessage = "Please enter password between 6 to 50 character and password must contain at least one one Lower case letter, one Upper case letter, one Digit and one Special (!@#$%^&_) Characters.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long. and must start with an alphabet.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password :")]
        public string UPass { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Created By :")]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Created Time :")]
        public DateTime? CreatedTime { get; set; }

        [Required]
        [Display(Name = "YTR Applicable :")]
        public byte YTRApplicable { get; set; }

        [Required]
        [Display(Name = "User Type :")]
        public byte UserType { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Status :")]
        public byte Status { get; set; }

        [Required]
        [Display(Name = "Created User Id :")]
        public short CreatedUserId { get; set; }

        public int TempPersonalRowID { get; set; }

        public int PersonalRowID { get; set; }
    }
    public class AddPQCandidateLoginViewModelInfo
    {
        [ScaffoldColumn(false)]
        [Display(Name = "ID")]
        public int CandidateUserRowID { get; set; }

        [Required]
        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }

        [Required]
        [Display(Name = "Candidate Name :")]
        public string CandidateName { get; set; }

        [Display(Name = "Employee ID :")]
        public string EmployeeID { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}", ErrorMessage = "Please enter correct email")]
        [Display(Name = "Candidate Email :")]
        public string CandidateEmail { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[7-9]{1}.*[0-9]{9}$", ErrorMessage = "Please enter valid Mobile Number")]
        [Display(Name = "Contact No :")]
        public string CantactNo { get; set; }

        [Required]
        [Display(Name = "User Id :")]
        public string UserID { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.{6,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&_]).*$", ErrorMessage = "Please enter password between 6 to 50 character and password must contain at least one one Lower case letter, one Upper case letter, one Digit and one Special (!@#$%^&_) Characters.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long. and must start with an alphabet.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password :")]
        public string UPass { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Created By :")]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Created Time :")]
        public DateTime? CreatedTime { get; set; }

        [Required]
        [Display(Name = "YTR Applicable :")]
        public byte YTRApplicable { get; set; }

        [Required]
        [Display(Name = "User Type :")]
        public byte UserType { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Status :")]
        public byte Status { get; set; }

        [Required]
        [Display(Name = "Created User Id :")]
        public short CreatedUserId { get; set; }

        public int TempPersonalRowID { get; set; }
        public int PersonalRowID { get; set; }
        public int CheckFamilyRowID { get; set; }
        public int SubCheckRowID { get; set; }
    }

    public class UpdatePQCandidateLoginViewModel
    {
        [Required]
        [Display(Name = "ID")]     
        public int CandidateUserRowID { get; set; }

        public short ClientRowID { get; set; }

        [Display(Name = "Name :")]
        [AllowHtml]
        public string CandidateName { get; set; }

        [Display(Name = "Employee ID :")]
        public string EmployeeID { get; set; }

        [Display(Name = "Email :")]
        [AllowHtml]
        public string CandidateEmail { get; set; }

        [Display(Name = "Contact No :")]
        [AllowHtml]
        public string CantactNo { get; set; }

        [Display(Name = "User Id :")]
        public string UserID { get; set; }

        [Required]
        //[RegularExpression(@"^.*(?=.{14,50})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&_]).*$", ErrorMessage = "Please enter password between 14 to 50 character and password must contain at least one one Lower case letter, one Upper case letter, one Digit and one Special (!@#$%^&_) Characters.")]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long. and must start with an alphabet.", MinimumLength = 14)]
        [DataType(DataType.Password)]
        [Display(Name = "Password :")]
        public string UPass { get; set; }

        [Display(Name = "YTR Applicable :")]
        public byte YTRApplicable { get; set; }

       [Display(Name = "User Type :")]
        public byte UserType { get; set; }
        
        [Display(Name = "Created User Id :")]
        public short CreatedUserId { get; set; }

        [Display(Name = "Modified Time :")]
        public DateTime? ModifiedTime { get; set; }
    }

    public class PQCandidateDDLViewModel
    {
        [Display(Name = "Client Name")]
        public int ClientRowID { get; set; }
        public string ClientName { get; set; }
    }

    public class PQCandidateUserListPagedModel
    {
        public IEnumerable<PQCandidateLoginListViewModel> CandidateUsers { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class PQCandidateLoginListViewModel
    {
        [Display(Name = "ID")]
        public int CandidateUserRowID { get; set; }
                
        public short ClientRowID { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        
        [Display(Name = "Candidate Name")]
        public string CandidateName { get; set; }
        
        [Display(Name = "Candidate Email")]
        public string CandidateEmail { get; set; }

        [Display(Name = "UserID ")]
        public string UserID { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? ModifiedTime { get; set; }

        [Display(Name = "Status")]
        public byte Status { get; set; }

        [Display(Name = "EmployeeID")]
        public string EmployeeID { get; set; }

        [Display(Name = "Email Sent")]
        public int SentMailStatus { get; set; }

        [Display(Name = "Sent Date")]
        public DateTime? SentMailDate { get; set; }

    }

    public class PQCandidateLoginForSendMailViewModel
    {
        [Display(Name = "ID")]
        public int CandidateUserRowID { get; set; }

        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }

        [Display(Name = "Name :")]
        public string CandidateName { get; set; }

        [Display(Name = "Employee ID :")]
        public string EmployeeID { get; set; }

        [Display(Name = "Email :")]
        public string CandidateEmail { get; set; }

        [Display(Name = "Contact No :")]
        public string CantactNo { get; set; }

        [Display(Name = "User Id :")]
        public string UserID { get; set; }

        [Display(Name = "Password :")]
        public string UPass { get; set; }

        [Display(Name = "YTR Applicable :")]
        public byte YTRApplicable { get; set; }

        [Display(Name = "User Type :")]
        public byte UserType { get; set; }

        [Display(Name = "Created User Id :")]
        public short CreatedUserId { get; set; }

        [Display(Name = "Modified Time :")]
        public DateTime? ModifiedTime { get; set; }

        [Display(Name = "Email Sent")]
        public int SentMailStatus { get; set; }

        [Display(Name = "Sent Date")]
        public DateTime? SentMailDate { get; set; }
        public int PersonalId { get; set; }
        public string SMSText { get; set; }
        public string AlternateEmailID { get; set; }

    }

    public class UpdatePQCandidateLoginForSendMailViewModel
    {
        [Display(Name = "ID")]
        public int CandidateUserRowID { get; set; }

        [Display(Name = "Email Sent")]
        public byte SentMailStatus { get; set; }

        [Display(Name = "Sent Date")]
        public DateTime? SentMailDate { get; set; }
    }
    public class CandidateInsuffLoginViewModel
    {
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Display(Name = "Candidate Name")]
        public string CandidateName { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        public string pwd { get; set; }
        public string cpid { get; set; }

    }

}
