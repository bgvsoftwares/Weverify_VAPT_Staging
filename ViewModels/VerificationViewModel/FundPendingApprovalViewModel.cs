using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels.VerificationViewModel
{
    public class FundPendingApprovalViewModel
    {
        [Required]
        [ScaffoldColumn(false)]
        public int VerificationRowID { get; set; }

        [Required]
        public int PersonalRowID { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }
        public string CheckFamilyName { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }

        public DateTime ModifiedDate { get; set; }
        public short ModifiedBy { get; set; }

        public string CheckStatus { get; set; }

        [Required]
        [Display(Name ="Remarks")]
        [AllowHtml] 
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
        public string ExpectedTat { get; set; }
        public string FundsStatus { get; set; }
        public string CompanyRefNo { get; set; }
        public string ColorCode { get; set; }
        public string ReportType { get; set; }
    }
}
