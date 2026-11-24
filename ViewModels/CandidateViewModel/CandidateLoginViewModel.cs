using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ViewModels.CandidateViewModel
{
    public class CandidateLoginViewModel
    {
        [Required]
        [AllowHtml] 
        [Display(Name = "User Name")]
        public string CRPUserName { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string CRPPassWord { get; set; }
        [AllowHtml]
        public string CaptchaVal { get; set; }
        public string GenerateCaptchaVal { get; set; }
    }

    public class CandidateUserLoggedViewModel
    {
        public int CandidateUserRowID { get; set; }
        public short ClientRowID { get; set; }
        public byte HRApprovalRequired { get; set; }
        public int TempPersonalRowID { get; set; }
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public string UserID { get; set; }
        public string UPass { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public byte YTRApplicable { get; set; }
        public bool Agreementsign { get; set; }
        public string ClientName { get; set; }
        public string CandidateFullName { get; set; }
        public byte Status { get; set; }
        public int ClientUserrowid { get; set; }
    }

    public class UpdateCandidateLoginExpireStatusViewModel
    {
        public int CandidateUserRowID { get; set; }
        public string UserID { get; set; }
        public byte UserType { get; set; }
        public byte Status { get; set; }
    }
}
