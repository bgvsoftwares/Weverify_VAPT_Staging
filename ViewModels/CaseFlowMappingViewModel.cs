using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
   public class CaseFlowMappingViewModel
    {
        [Display(Name = "ID")]
        public int CaseFlowMappingRowID { get; set; }

        [Display(Name = "ReportCheckMapping")]
        public int ReportCheckMappingRowID { get; set; }

        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [Display(Name = "Report Type :")]
        public string ReportType { get; set; }

        [Display(Name = "Color Code :")]
        public string ColorCode { get; set; }

        [Display(Name = "Move To RWQC")]
        public byte MoveToRWQC { get; set; }

        [Display(Name = "ColorCode Review")]
        public byte ColorCodeReview { get; set; }

        [Display(Name = "Send To Client")]
        public byte SendToClient { get; set; }

        [Display(Name = "Status")]
        public byte? Status { get; set; }
    }

    public class AddCaseFlowMappingViewModel
    {
        [Display(Name = "ID")]
        public int CaseFlowMappingRowID { get; set; }

        [Display(Name = "ReportCheckMapping")]
        public int ReportCheckMappingRowID { get; set; }

        [Display(Name = "Color Code :")]
        public string ColorCode { get; set; }

        [Required(ErrorMessage = "Please select Case Flow")]
        [Display(Name = "Move To RWQC")]
        public byte MoveToRWQC { get; set; }

        [Required(ErrorMessage = "Please select Case Flow")]
        [Display(Name = "ColorCode Review")]
        public byte ColorCodeReview { get; set; }

        [Required(ErrorMessage = "Please select Case Flow")]
        [Display(Name = "Send To Client")]
        public byte SendToClient { get; set; }
                
        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        public byte? Status { get; set; }
    }

    public class UpdateCaseFlowMappingViewModel
    {
        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "ID")]
        public int CaseFlowMappingRowID { get; set; }

        [Required]
        [Display(Name = "ReportCheckMapping")]
        public int ReportCheckMappingRowID { get; set; }

        [Required(ErrorMessage = "Please select Color Code")]
        [Display(Name = "Color Code :")]
        public string ColorCode { get; set; }

        [Required(ErrorMessage = "Please select Case Flow")]
        [Display(Name = "Move To RWQC")]
        public byte MoveToRWQC { get; set; }

        [Required(ErrorMessage = "Please select Case Flow")]
        [Display(Name = "ColorCode Review")]
        public byte ColorCodeReview { get; set; }

        [Required(ErrorMessage = "Please select Case Flow")]
        [Display(Name = "Send To Client")]
        public byte SendToClient { get; set; }
        
        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        public byte? Status { get; set; }
    }

    public class CaseFlowMappingListPagedModel
    {
        public IEnumerable<CaseFlowMappingViewModel> CaseFlowMappings { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

}
