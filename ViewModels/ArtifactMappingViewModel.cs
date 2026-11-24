using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
   public class ArtifactMappingViewModel
    {
        [Display(Name = "ID")]
        public int ArtifactMappingRowID { get; set; }

        [Display(Name = "ReportCheckMapping")]
        public int ReportCheckMappingRowID { get; set; }

        [Display(Name = "Client Name :")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [Display(Name = "Report Type :")]
        public string ReportType { get; set; }
                
        [Display(Name = "Color Code :")]
        public int ClientSeverityRowId { get; set; }
        public string ColorCode { get; set; }

        [Display(Name = "Check Family :")]
        public short CheckFamilyRowID { get; set; }
        public string CheckFamilyName { get; set; }

        [Display(Name = "Document Type :")]
        public int DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        
        [Display(Name = "Document Seq No.")]
        public short DocumentSeqNo { get; set; }
                
        [Display(Name = "Status")]
        public byte? Status { get; set; }
    }

    public class AddArtifactMappingViewModel
    {
        [Display(Name = "ID")]
        public int ArtifactMappingRowID { get; set; }

        [Display(Name = "ReportCheckMapping")]
        public int ReportCheckMappingRowID { get; set; }

        public short ClientRowID { get; set; }

        [Required(ErrorMessage = "Please select Color Code")]
        [Display(Name = "Color Code :")]
        public int ClientSeverityRowId { get; set; }
                
        public string ColorCode { get; set; }

        [Display(Name = "Check Family :")]
        public short CheckFamilyRowID { get; set; }

        [Display(Name = "Document Type :")]
        public int DocumentTypeId { get; set; }

        public string DocumentType { get; set; }

        [Display(Name = "Document Seq No.")]
        public short DocumentSeqNo { get; set; }
                                        
        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        public byte? Status { get; set; }
    }
        
    public class ArtifactMappingListPagedModel
    {
        public IEnumerable<ArtifactMappingViewModel> ArtifactMappings { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    

}
