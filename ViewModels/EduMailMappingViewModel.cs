using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class EduMailMappingViewModel
    {
        [Display(Name ="ID")]
        public int EduMailMappingRowID { get; set; }

        public short UniversityRowID { get; set; }

        [Display(Name = "University Name :")]
        public string UniversityName { get; set; }

        public int CollegeRowID { get; set; }

        [Display(Name = "College Name :")]
        public string CollegeName { get; set; }

        [Display(Name = "EMail Type :")]
        public string EMailType { get; set; }

        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}", ErrorMessage = "Please enter correct email")]
        [Display(Name = "Email ID :")]
        public string EMailID { get; set; }

        [ScaffoldColumn(false)]
        public byte? Status { get; set; }
        
    }

    public class AddEduMailMappingViewModel
    {
        [ScaffoldColumn(false)]
        [Display(Name = "ID")]
        public int EduMailMappingRowID { get; set; }

        [Required]
        [Display(Name = "University Name :")]
        public short UniversityRowID { get; set; }

        [Required]
        [Display(Name = "College Name :")]
        public int CollegeRowID { get; set; }

        [Required]
        [Display(Name = "EMail Type :")]
        public string EMailType { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*[,;]\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})*$",
         ErrorMessage = "Please enter valid email addresses")]
        [Display(Name = "Email ID :")]
        public string EMailID { get; set; }

        [ScaffoldColumn(false)]
        public byte? Status { get; set; }
    }

    public class UpdateEduMailMappingViewModel
    {
        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "ID")]
        public int EduMailMappingRowID { get; set; }

        [Required]
        [Display(Name = "University Name :")]
        public short UniversityRowID { get; set; }

        [Required]
        [Display(Name = "College Name :")]
        public int CollegeRowID { get; set; }

        [Required]
        [Display(Name = "EMail Type :")]
        public string EMailType { get; set; }

        [Required]
        [RegularExpression( @"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})(\s*[,;]\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})*$", 
         ErrorMessage = "Please enter valid email addresses")]
        [Display(Name = "Email ID :")]
        public string EMailID { get; set; }
        
        [ScaffoldColumn(false)]
        public byte? Status { get; set; }
    }

    public class EduMailMappingExportViewModel
    {
        public int EduMailMappingRowID { get; set; }
        public string UniversityName { get; set; }
        public string CollegeName { get; set; }
        public string EMailType { get; set; }
        public string EMailID { get; set; }
        public string Status { get; set; }

    }

    public class EduMailMappingListPagedModel
    {
        public IEnumerable<EduMailMappingViewModel> EduMailMapping { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int PageNo { get; set; }
    }

}
