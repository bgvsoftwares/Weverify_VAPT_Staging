using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ViewModels.HRViewModels
{
    public class CompanyLoginViewModel
    {
        [Required]
        [AllowHtml]
        [Display(Name = "User Name")]
        public string CRPUserName { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.Text)]
        [Display(Name = "Password")]
        public string CRPPassWord { get; set; }
        public string encPwd { get; set; }   
        [AllowHtml]
        public string CaptchaVal { get; set; }
        public string GenerateCaptchaVal { get; set; }
    }
    public class CompanySignInViewModel
    {
        [Required]
        [AllowHtml]
        [Display(Name = "User Name")]
        public string CRPUserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [AllowHtml]
        public string CRPPassWord { get; set; }
        public string encPwd { get; set; }
        [AllowHtml]
        public string CaptchaVal { get; set; }
        public string GenerateCaptchaVal { get; set; }        
    }

    public class ForgetCredentialsViewModel
    {
      
        [Display(Name = "User Name")]
        public string CRPUserName { get; set; }      
        public string WebUserRowID { get; set; }
        public string ClientRowID { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string CRPOldPwd { get; set; }
        public string CRPOldPwd1 { get; set; }
        [Required]
        [AllowHtml]
        [RegularExpression(@"^.*(?=.{14,50})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&_]).*$", ErrorMessage = "Please enter password between 14 to 50 character and password must contain at least one one Lower case letter, one Upper case letter, one Digit and one Special (!@#$%^&_) Characters.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long. and must start with an alphabet.", MinimumLength = 14)]  
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string CRPNewPwd { get; set; }
        [Required]
        [AllowHtml]
        [RegularExpression(@"^.*(?=.{14,50})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&_]).*$", ErrorMessage = "Please enter password between 14 to 50 character and password must contain at least one one Lower case letter, one Upper case letter, one Digit and one Special (!@#$%^&_) Characters.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long. and must start with an alphabet.", MinimumLength = 14)]       
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string CRPConfirmNewPwd { get; set; }
        public string EncPWD { get; set; }
        public string CaptchaVal { get; set; }
    }
   

    public class CompanyUserLoggedViewModel
    {
        public short WebUserRowID { get; set; }
        
        public short TeamMemberRoWID { get; set; }
        public string TeamMemberName { get; set; }
        public string TeamMemberEmail { get; set; }              
        public string CRPUserName { get; set; }                
        public string CRPPassWord { get; set; }
        public string encPassWord { get; set; }
        public byte IsExportedExcel { get; set; }        
        public byte IsExportedWord { get; set; }        
        public byte IsExportedPDF { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte Status { get; set; }
        public int ExpiryDays { get; set; }
        public string Message { get; set; }
    }

    public class UpdateCompanyUserBlockStatusViewModel
    {
        public short WebUserRowID { get; set; }
        public string CRPUserName { get; set; }
        public DateTime? BlockedDate { get; set; }
        public byte Status { get; set; }
    }

    public static class LoginDetail
    {
        
        public static string CRPUserName { get; set; }
        public static int WebUserRowID { get; set; }
        public static int TeamMemberRoWID { get; set; }
        public static string TeamMemberName { get; set; }
        public static string TeamMemberEmail { get; set; }
        public static int LoginDetailRowID { get; set; }
        public static string Deviceidentifiers { get; set; }
        public static string BrowserName { get; set; }

    }

    public class CompanyLoginDetails
    {
        public short WebUserRowID { get; set; }
        public string crpUN { get; set; }
        public string crpPWD { get; set; }
    }
    public class FaceCandidateLoginViewModel
    {
        [Display(Name = "User Name")]
        public string PersonalRowId { get; set; }

        [DataType(DataType.Text)]
        public string MobileNo { get; set; }
        public string MobileOTP { get; set; }
        [AllowHtml]
        public string CaptchaVal { get; set; }
        public string GenerateCaptchaVal { get; set; }
        public string loginType { get; set; }
    }
    public class htmlToPdf
    {
        public string base64Pdf { get; set; }
    }
}
