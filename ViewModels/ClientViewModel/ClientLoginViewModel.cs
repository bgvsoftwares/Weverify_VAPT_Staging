using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ViewModels.ClientViewModel
{
    public class ClientLoginViewModel
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
        [AllowHtml]
        public string CaptchaVal { get; set; }
        public string encPwd { get; set; }
        public string GenerateCaptchaVal { get; set; }
    }

    public class ClientNGRViewModel
    {
        [Required]
        [AllowHtml]
        [Display(Name = "User Name")]
        public string CRPUserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string CRPPassWord { get; set; }
        public string CaptchaVal { get; set; }
        public string encPwd { get; set; }
        public string GenerateCaptchaVal { get; set; }        
    }
    public class ClientUserLoggedViewModel
    {
        public int ClientUserRowID { get; set; }
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string ClientCode1 { get; set; }
        public string ClientBranch { get; set; }
        public string ClientSubGroupName { get; set; }
        public string LocationName { get; set; }
        public byte HRApprovalRequired { get; set; }
        public string Abbreviation { get; set; }
        public byte CodeGeneration { get; set; }
        public string UserID { get; set; }
        public string UPass { get; set; }
        public string CSpocName { get; set; }
        public string CSpocEmailID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public byte Status { get; set; }
        public int ClientAccessType { get; set; }      
        public int ExpiryDays { get; set; }
        public int ValidUser { get; set; }
        public int isMFA { get; set; }
        public int UType { get; set; }
        public string Message { get; set; }
    }
    
    public class UpdateClientUserBlockStatusViewModel
    {
        public int ClientUserRowID { get; set; }
        public string UserID { get; set; }
        public DateTime? BlockedDate { get; set; }
        public byte Status { get; set; }
    }
    public class HCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTs { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

}
