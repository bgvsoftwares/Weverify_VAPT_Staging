using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ReportCheckMappingViewModel
    {
        [Display(Name ="ID")]
        public int ReportCheckMappingRowID { get; set; }

        public short ClientRowID { get; set; }

        [Display(Name = "Client Name :")]
        public string ClientName { get; set; }

        [Display(Name = "Report Type :")]
        public string ReportType { get; set; }

        [Display(Name = "Address Check")]
        public byte AddressChk { get; set; }

        [Display(Name = "Criminal Check")]
        public byte CriminalChk { get; set; }

        [Display(Name = "Education Check")]
        public byte EducationChk { get; set; }

        [Display(Name = "Employment Check")]
        public byte EmploymentChk { get; set; }

        [Display(Name = "Identity Check")]
        public byte IdentityChk { get; set; }

        [Display(Name = "Reference Check")]
        public byte ReferenceChk { get; set; }

        [Display(Name = "Special Check")]
        public byte SpecialChk { get; set; }        

        [ScaffoldColumn(false)]
        public byte? Status { get; set; }
        
    }

    public class AddReportCheckMappingViewModel
    {
        [ScaffoldColumn(false)]
        [Display(Name = "ID")]
        public int ReportCheckMappingRowID { get; set; }

        [Required]
        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }

        [Required]
        [Display(Name = "Report Type :")]
        public string ReportType { get; set; }

        [Required]
        [Display(Name = "Address Check :")]
        public byte AddressChk { get; set; }

        [Required]
        [Display(Name = "Criminal Check :")]
        public byte CriminalChk { get; set; }

        [Required]
        [Display(Name = "Education Check :")]
        public byte EducationChk { get; set; }

        [Required]
        [Display(Name = "Employment Check :")]
        public byte EmploymentChk { get; set; }

        [Required]
        [Display(Name = "Identity Check :")]
        public byte IdentityChk { get; set; }

        [Required]
        [Display(Name = "Reference Check :")]
        public byte ReferenceChk { get; set; }

        [Required]
        [Display(Name = "Special Check :")]
        public byte SpecialChk { get; set; }               

        [ScaffoldColumn(false)]
        public byte? Status { get; set; }
    }

    public class UpdateReportCheckMappingViewModel
    {
        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "ID")]
        public int ReportCheckMappingRowID { get; set; }

        [Required]
        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }

        [Required]
        [Display(Name = "Report Type :")]
        public string ReportType { get; set; }

        [Required]
        [Display(Name = "Address Check :")]
        public byte AddressChk { get; set; }

        [Required]
        [Display(Name = "Criminal Check :")]
        public byte CriminalChk { get; set; }

        [Required]
        [Display(Name = "Education Check :")]
        public byte EducationChk { get; set; }

        [Required]
        [Display(Name = "Employment Check :")]
        public byte EmploymentChk { get; set; }

        [Required]
        [Display(Name = "Identity Check :")]
        public byte IdentityChk { get; set; }

        [Required]
        [Display(Name = "Reference Check :")]
        public byte ReferenceChk { get; set; }

        [Required]
        [Display(Name = "Special Check :")]
        public byte SpecialChk { get; set; }               

        [ScaffoldColumn(false)]
        public byte? Status { get; set; }
    }

    public class ReportCheckMappingExportViewModel
    {
        public int ReportCheckMappingRowID { get; set; }

        public string ClientName { get; set; }

        public string ReportType { get; set; }

        public byte AddressChk { get; set; }

        public byte CriminalChk { get; set; }

        public byte EducationChk { get; set; }

        public byte EmploymentChk { get; set; }

        public byte IdentityChk { get; set; }

        public byte ReferenceChk { get; set; }

        public byte SpecialChk { get; set; }

    }

    public class ReportCheckMappingListPagedModel
    {
        public IEnumerable<ReportCheckMappingViewModel> ReportCheckMapping { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

}
