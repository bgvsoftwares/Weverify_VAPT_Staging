using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ViewModels.ProvidedInfoViewModel
{
    public class PQVerifiedUploadDocViewModel
    {
        public Int64 VerifiedUploadRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        public string DocumentUploadFrom { get; set; }   //Provided / Verified
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public DateTime? UploadDate { get; set; }
        public byte StatusDocxConvert { get; set; }      //Default - 0 otherwise 1
        public byte StatusPDF2DOCXConvert { get; set; }  //Default - 0 otherwise 1
        public string PDF_FileName { get; set; }
        public byte Status { get; set; }
        public int checkstatusId { get; set; }
        public string GeoCo { get; set; }
        public int ClientRowId { get; set; }
        public int CheckStatusId { get; set; }
        public int CheckFamilyRowID { get; set; }
    }
    public class VerifiedResearchDoc
    {
        public int PersonalRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string DocumentUploadFrom { get; set; }  // Verified
        public string DocumentType { get; set; }
        public string IsExist { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime? UploadDate { get; set; }
        public int ClientRowId { get; set; }
        public int CheckStatusId { get; set; }
    }

    public class UploadedDocViewModel
    {
        public Int64 VerifiedUploadRowID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }      
    }

    public class PQAddVerifiedUploadDocViewModel
    {
        [Display(Name ="ID")]
        public Int64 VerifiedUploadRowID { get; set; }

        public int PersonalRowID { get; set; }
        public short CheckFamilyRowID { get; set; }
        public string ClientRowId { get; set; }
        public int clientId { get; set; }
        [Required]
        [Display(Name = "Check Name : ")]
        public short SubCheckRowID { get; set; }
        public short Chec { get; set; }
        [Display(Name = "Document Upload From : ")]
        public string DocumentUploadFrom { get; set; }   //Provided / Verified
         [Display(Name = "Document Type : ")]
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string CheckWithFileName { get; set; }
        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase UploadDoc { get; set; }
        
        [Display(Name = "Description : ")]
        [AllowHtml]
        public string Remarks { get; set; }
        [Display(Name = "Others Name: ")]
        public string OtherDocName { get; set; }

        public DateTime? UploadDate { get; set; }
        public byte StatusDocxConvert { get; set; }      //Default - 0 otherwise 1
        public byte StatusPDF2DOCXConvert { get; set; }  //Default - 0 otherwise 1
        public byte RptArtiFacts { get; set; }
        public string PDF_FileName { get; set; }

        [Display(Name ="Document not applicable : ")]
        public byte NADoc { get; set; }

        [NotMapped]
        public bool NABool
        {
            get { return NADoc > 0; }
            set { NADoc = value ? (byte)1 : (byte)0; }
        }

        [ScaffoldColumn(false)]
        public byte Status { get; set; }

        public byte InsuffDoc { get; set; }
    }

    public class PQVerifiedUploadDocListPagedModel
    {
        public IEnumerable<PQVerifiedUploadDocViewModel> UploadedDocInfo { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class UploadedDocForRWQCTMViewModel
    {
        public Int64 VerifiedUploadRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        public string DocumentUploadFrom { get; set; }   //Provided / Verified
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public DateTime? UploadDate { get; set; }
        public byte RptArtiFacts { get; set; }
        public byte RptArtiFactOrder { get; set; }        
    }

    public class UploadedDocListForRWQCTMPagedModel
    {
        public int PersonalRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        public string AllDocIds { get; set; }
        public string AllUnCheckIds { get; set; }
        public int ClientRowId { get; set; }
        public int CheckStatusid { get; set; }
        public IEnumerable<UploadedDocForRWQCTMViewModel> UploadedDocForRQQCTM { get; set; }
    }

    public class UploadedCaseDocForRWQCTMViewModel
    {
        public Int64 CaseDocRowID { get; set; }
        public int PersonalRowID { get; set; }
        public string DocumentUploadFrom { get; set; }   //Provided / Verified
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public DateTime? UploadDate { get; set; }
        public byte RptArtiFacts { get; set; }
        public byte RptDisplayOrder { get; set; }
    }

    public class UploadedCaseDocListForRWQCTMPagedModel
    {
        public int PersonalRowID { get; set; }
        public string AllDocIds { get; set; }
        public IEnumerable<UploadedCaseDocForRWQCTMViewModel> UploadedCaseDocForRQQCTM { get; set; }
    }

    public class MapUploadedDocRWQCTMModel
    {
        public Int64 VerifiedUploadRowID { get; set; }
        public byte RptArtiFacts { get; set; }
        public byte RptArtiFactOrder { get; set; }
        public string NewDocType { get; set; }
        public string DocType { get; set; }
        public string DocFrom { get; set; }
        public string DocPath { get; set; }
        public string DocName { get; set; }
        public int ClientRowId { get; set; }
    }

    public class VerifiedUploadDocViewModel
    {
        [ScaffoldColumn(false)]      
        public Int64 VerifiedUploadRowID { get; set; }

        [ScaffoldColumn(false)]
        [Required]
        public int PersonalRowID { get; set; }

        [ScaffoldColumn(false)]
        [Required]     
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public string CheckStatus { get; set; }

        [ScaffoldColumn(false)]
        public string DocumentUploadFrom { get; set; }   //Provided / Verified

        [Display(Name = "Document Type :")]
        [Required]
        public string DocumentType { get; set; } // Varification Document

        [ScaffoldColumn(false)]
        public string OtherDocumentType { get; set; }

        [ScaffoldColumn(false)]
        public string FileName { get; set; }

        [ScaffoldColumn(false)]
        public string FilePath { get; set; }

        [Display(Name ="Select Document :")]
        [Required]
        [ValidateLargeFileAttribute]
        public HttpPostedFileBase UploadDoc { get; set; }


        [Display(Name = "Description : ")]
        [Required]
        [AllowHtml]
        public string Remarks { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UploadDate { get; set; }

        [ScaffoldColumn(false)]
        public byte StatusDocxConvert { get; set; }      //Default - 0 otherwise 1

        [ScaffoldColumn(false)]
        public byte StatusPDF2DOCXConvert { get; set; }  //Default - 0 otherwise 1

        [ScaffoldColumn(false)]
        public string PDF_FileName { get; set; }

        [ScaffoldColumn(false)]
        public byte Status { get; set; }
        public int ClientRowId { get; set; }
        public int SpecialCheckId { get; set; }
    }
    public class VerifiedUploadDocPTRViewModel
    {
        [ScaffoldColumn(false)]
        public Int64 VerifiedUploadRowID { get; set; }

        [ScaffoldColumn(false)]
        [Required]
        public int PersonalRowID { get; set; }

        [ScaffoldColumn(false)]
        [Required]
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public string CheckStatus { get; set; }

        [ScaffoldColumn(false)]
        public string DocumentUploadFrom { get; set; }   //Provided / Verified

        [Display(Name = "Document Type :")]
        [Required]
        public string DocumentType { get; set; } // Varification Document

        [ScaffoldColumn(false)]
        public string FileName { get; set; }

        [ScaffoldColumn(false)]
        public string FilePath { get; set; }
            
        
        [Display(Name = "Description : ")]
        [Required]
        [AllowHtml]
        public string Remarks { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UploadDate { get; set; }

        [ScaffoldColumn(false)]
        public byte StatusDocxConvert { get; set; }      //Default - 0 otherwise 1

        [ScaffoldColumn(false)]
        public byte StatusPDF2DOCXConvert { get; set; }  //Default - 0 otherwise 1

        [ScaffoldColumn(false)]
        public string PDF_FileName { get; set; }

        [ScaffoldColumn(false)]
        public byte Status { get; set; }
        public int ClientRowId { get; set; }
        public int SpecialCheckId { get; set; }
   
        [Display(Name = "Select Document :")]
        [Required]
        [ValidateLargeFileAttribute]
        public HttpPostedFileBase[] UploadDoc1 { get; set; }
        [Required]  
        [ValidateLargeFileAttribute]
        public HttpPostedFileBase UploadDoc { get; set; }
    }

    public class VerifiedUploadAddressDocViewModel
    {
        [Required]
        public int PersonalRowID { get; set; }
        public int ClientRowID { get; set; }
        
        [Required]
        public short SubCheckRowID { get; set; }
        public string DocumentUploadFrom { get; set; }        
        public string DocumentType { get; set; }
        
        public string FileName { get; set; }
        public string FilePath { get; set; }

        [ValidateFileWithNullCheckAttributeInPartner]
        [Display(Name = "Select Document For Signature :")]
        //public HttpPostedFileBase UploadSignatureDoc { get; set; }
        public HttpPostedFileBase[] UploadSignatureDoc { get; set; }

        [ValidateFileWithNullCheckAttributeInPartner]
        [Display(Name = "Select Document For IDProof :")]
        public HttpPostedFileBase[] UploadIDProofDoc { get; set; }
        
        [ValidateFileWithNullCheckAttributeInPartner]
        [Display(Name = "Select Document For House photo :")]
        public HttpPostedFileBase[] UploadHousephotoDoc { get; set; }
        
        [ValidateFileWithNullCheckAttributeInPartner]
        [Display(Name = "Select Document For Landmark Photo :")]
        public HttpPostedFileBase[] UploadLandmarkPhotoDoc { get; set; }

        [ValidateFileWithNullCheckAttributeInPartner]
        [Display(Name = "Select Document For Location :")]
        public HttpPostedFileBase[] UploadLocation { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public DateTime? UploadDate { get; set; }
        public string CheckStatus { get; set; }        
        public byte Status { get; set; }

        [ValidateFileWithNullCheckAttributeInPartner]
        [Display(Name = "Select Document :")]
        public HttpPostedFileBase[] UploadAddressDoc { get; set; }

        [ScaffoldColumn(false)]
        public string OtherDocumentType { get; set; }
    }

    public class AddTempPQVerifiedUploadDocViewModel
    {
        [Display(Name = "ID")]
        public Int64 VerifiedUploadRowID { get; set; }

        public int PersonalRowID { get; set; }

        [Required]
        [Display(Name = "Select Check : ")]
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }

        [Display(Name = "Document Upload From : ")]
        public string DocumentUploadFrom { get; set; }   //Provided / Verified

        [Display(Name = "Document Type : ")]
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string GeoCo { get; set; }
        public short CheckFamilyRowID { get; set; }
        public short ClientRowID { get; set; }

        // [Required]
        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase UploadDoc { get; set; }

        //public IEnumerable<HttpPostedFileBase> UploadDoc { get; set; }

        [Display(Name = "Description : ")]
        [AllowHtml]
        public string Remarks { get; set; }

        public DateTime? UploadDate { get; set; }
        public byte StatusDocxConvert { get; set; }      //Default - 0 otherwise 1
        public byte StatusPDF2DOCXConvert { get; set; }  //Default - 0 otherwise 1
        public string PDF_FileName { get; set; }
        public string Division { get; set; }
        public string AadharNo { get; set; }
        public string DOB { get; set; }

        [ScaffoldColumn(false)]
        public byte Status { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase EducationProof { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase PanCard { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase AAdharCard { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase BankAccountProof { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase Employmentproof { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase DOBproof { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase Passport { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase Passportsizephoto { get; set; }
    }
    
    public class TempPQVerifiedUploadDocListPagedModel
    {
        public IEnumerable<PQVerifiedUploadDocViewModel> TempUploadedDocInfo { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    /// <summary>
    /// Updated 18-01-2014
    /// </summary>
    public class TempPQVerifiedUploadDocListPagedModelUpdated
    {
        public Int64 VerifiedUploadRowID { get; set; }
        
        public IEnumerable<PQVerifiedUploadDocViewModelUpdated> TempUploadedDocInfoUpdated { get; set; }
    }
    public class PQVerifiedUploadDocViewModelUpdated
    {
        public Int64 VerifiedUploadRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        public string DocumentUploadFrom { get; set; }   //Provided / Verified
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public DateTime? UploadDate { get; set; }
        public byte StatusDocxConvert { get; set; }      //Default - 0 otherwise 1
        public byte StatusPDF2DOCXConvert { get; set; }  //Default - 0 otherwise 1
        public string PDF_FileName { get; set; }
        public byte Status { get; set; }
    }

    public class MISEmployment
    {
        public string ClientCode1 { get; set; }
        public string CandidateName { get; set; }
        public string companyRefNo { get; set; }
        public string CheckFamilyName { get; set; }
        public string SubCheckName { get; set; }
        public string UniqueComponentID { get; set; }
        public string CurrentCheckStatus { get; set; }
        public string DETL { get; set; }
        public string DETMAllocatedTo { get; set; }
        public string DETMAllocatedDate { get; set; }
        public string DEQCAllocatedTo { get; set; }
        public string DETMQCAllocatedDate { get; set; }
        public string ManagerAllocatedTo { get; set; }
        public string MgrAllocatedDate { get; set; }
        public string TLAllocatedTo { get; set; }
        public string TLAllocatedDate { get; set; }
        public string TMAllocatedTo { get; set; }
        public string TMAllocatedDate { get; set; }
        public string VendorName { get; set; }
        public string PTRMgrAllocatedDate { get; set; }
        public string QCAllocatedTo { get; set; }
        public string TMQCAllocatedDate { get; set; }
        public string ReportManagerAllocatedTo { get; set; }
        public string RWQCManagerDate { get; set; }
        public string RWTLAllocatedTo { get; set; }
        public string RWQCTeamLeaderDate { get; set; }
        public string RWTMAllocatedTo { get; set; }
        public string RWQCTeamMemberDate { get; set; }
        public string outDate { get; set; }
        public string InternalOutDate { get; set; }
        public string InsuffRaisedDate { get; set; }
        public string InsuffClearDate { get; set; }
        public string InsuffRaisedRemarks { get; set; }
        public string ReverificationOpenDate { get; set; }
        public string ReverificationCloseDate { get; set; }
        public string CompanyName { get; set; }

    }


    public class ExportAddressFeedback
    {
        public string CandidateName { get; set; }
        public string MobileNo { get; set; }
        public string Comment { get; set; }
        public string Rating { get; set; }
        public string CreateDate { get; set; }

    }

    public class OfflineInstantCheckDocsViewModel
    {      
        public string CompanyRefNo { get; set; }
        public string IdNumber { get; set; }       
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int UploadBy { get; set; }
    }

    public class IsfileUploadsViewModel
    {
        public string CheckUniqueId { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
        
    }


    public class RiskMatrixModel
    {
        public long PersonalRowId { get; set; }
        public long ClientRowId { get; set; }

        [AllowHtml]
        public string Recommendations1 { get; set; }

        public List<RiskMatrixItem> Items { get; set; }

        public List<RiskMatrixConsolidatedItem> ConsolodatedItems { get; set; }

        public RiskMatrixModel()
        {
            Items = new List<RiskMatrixItem>();

            ConsolodatedItems = new List<RiskMatrixConsolidatedItem>();
        }
    }

    public class RiskMatrixConsolidatedItem
    {
        public long PersonalRowID { get; set; }

        public long ClientRowID { get; set; }

        public string Level { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }
    }

    public class RiskMatrixItem
    {
        public string RiskFactor { get; set; }

        public int Likelihood { get; set; } 
        public int Impact { get; set; }    

        public int RiskScore { get; set; }

        public string Category { get; set; }        
    }


    public class RiskMatrixViewModel
    {
        public long PersonalRowID { get; set; }
        public long ClientRowID { get; set; }
        public string RiskFactor { get; set; }
        public int Likelihood { get; set; }
        public int Impact { get; set; }
        public int RiskScore { get; set; }
        public string Category { get; set; }
        public string RiskType { get; set; }
    }

    public class UANCheckToBGVFormCheckStatus
    {
        public string VerifiedFieldName { get; set; }
        public string Status { get; set; }
        public string VerifiedDate { get; set; }

    }


}
