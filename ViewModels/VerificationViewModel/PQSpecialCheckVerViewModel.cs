using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ViewModels.ProvidedInfoViewModel;

namespace ViewModels.VerificationViewModel
{
    public class PQSpecialCheckVerViewModel
    {

        [ScaffoldColumn(false)]
        public int SpecialCheckVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int SpecialCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }

        public string ClientName { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        public int CheckStatusId { get; set; }
        public bool Aging { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
        public string PersonalName { get; set; }

        [ScaffoldColumn(false)]
        public string UniqueComponentID { get; set; }
        public string CandidateName { get; set; }
        public string CompanyReferenceNo { get; set; }
        public string SubReferenceID { get; set; }
        public string MobileNo { get; set; }
        public string AllocatedTMBy { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        [MaxLength(200)]
        public string SC_Cand_Name { get; set; }

        [MaxLength(100)]
        public string SC_Father_Name { get; set; }      // Text Field - Auto Capture
        [MaxLength(100)]
        public string SC_SecuritasID { get; set; }      // Text Field - Auto Capture

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SC_DOB { get; set; }           // date  Text Field - Auto Capture

        [MaxLength(50)]
        public string SC_Mode_Verify { get; set; }          // Dropdown - (Written/Verbal/Online/Physical/Discrete/Research/Observation/public database/Other)

        [MaxLength(100)]
        public string SC_Mode_VerOther { get; set; }        // Text Field - if Mode is other

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SC_Date_Verify { get; set; }       // date  Text Field

        [MaxLength(500)]
        public string SC_AnyOtherComments { get; set; }     // Text Area 


        //Following not show on page. It is for future use only
        [MaxLength(200)]
        public string SC_Others1 { get; set; }       // Text Field

        [MaxLength(200)]
        public string SC_Others2 { get; set; }       // Text Field
        [MaxLength(200)]
        public string SC_Others3 { get; set; }       // Text Field
        [MaxLength(200)]
        public string SC_Others4 { get; set; }       // Text Field
        [MaxLength(200)]
        public string SC_Others5 { get; set; }       // Text Field 

        [MaxLength(50)]
        public string TypeRevert { get; set; }

        [MaxLength(50)]
        [Required]
        public string CheckStatus { get; set; }

        [MaxLength(50)]
        [Required]
        public string Severity { get; set; }

        public string hdSeverity { get; set; }

        [MaxLength(50)]
        [Required]
        public string ColorName { get; set; }

        [MaxLength(3000)]
        public string ReportComments { get; set; }

        public byte Status { get; set; }
       
        public short VerifiedBy { get; set; }
        public string VerifierDesignation { get; set; }
        public string VerifierContactNo { get; set; }
        public string VerifierMobileNo { get; set; }
        public string VerifierEmailId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? VerifiedDate { get; set; }
        public string  ApproveStatus { get; set; }

    }

    public class UpdatePQSpecialCheckVerViewModel
    {

        [ScaffoldColumn(false)]
        public int SpecialCheckVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int SpecialCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }

        public string ClientName { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
        public string PersonalName { get; set; }

        [ScaffoldColumn(false)]
        public string UniqueComponentID { get; set; }
        public string CandidateName { get; set; }
        public string CompanyReferenceNo { get; set; }
        public string SubReferenceID { get; set; }
        public string MobileNo { get; set; }
        public string AllocatedTMBy { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        [MaxLength(200)]
        public string SC_Cand_Name { get; set; }

        [MaxLength(100)]
        public string SC_Father_Name { get; set; }      // Text Field - Auto Capture
        [MaxLength(100)]
        public string SC_SecuritasID { get; set; }      // Text Field - Auto Capture

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SC_DOB { get; set; }           // date  Text Field - Auto Capture

        [MaxLength(50)]
        public string SC_Mode_Verify { get; set; }          // Dropdown - (Written/Verbal/Online/Physical/Discrete/Research/Observation/public database/Other)

        [MaxLength(100)]
        public string SC_Mode_VerOther { get; set; }        // Text Field - if Mode is other

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SC_Date_Verify { get; set; }       // date  Text Field

        [MaxLength(500)]
        public string SC_AnyOtherComments { get; set; }     // Text Area 


        //Following not show on page. It is for future use only
        [MaxLength(200)]
        public string SC_Others1 { get; set; }       // Text Field

        [MaxLength(200)]
        public string SC_Others2 { get; set; }       // Text Field
        [MaxLength(200)]
        public string SC_Others3 { get; set; }       // Text Field
        [MaxLength(200)]
        public string SC_Others4 { get; set; }       // Text Field
        [MaxLength(200)]
        public string SC_Others5 { get; set; }       // Text Field 

        [MaxLength(50)]
        public string TypeRevert { get; set; }

        [MaxLength(50)]
        public string CheckStatus { get; set; }

        [MaxLength(50)]
        public string Severity { get; set; }

        [MaxLength(50)]
        public string ColorName { get; set; }

        [MaxLength(3000)]
        public string ReportComments { get; set; }

        public byte Status { get; set; }

        public short VerifiedBy { get; set; }
        public string VerifierDesignation { get; set; }
        public string VerifierContactNo { get; set; }
        public string VerifierMobileNo { get; set; }
        public string VerifierEmailId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? VerifiedDate { get; set; }
        public int CheckStatusId { get; set; }
        public bool Aging { get; set; }

        #region Criminal Closer fields

        public SpecialCheckVerCloserViewModel CloserViewModel { get; set; }

        #endregion

    }
    public class SpecialCheckVerCloserViewModel
    {
        [ScaffoldColumn(false)]
        public int SpecialCheckVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int SpecialCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }

        public string Disposition { get; set; }
        public string DispositionID { get; set; }
        //public int ClientDispositionRowId { get; set; }
        public string ColorName { get; set; }
        public string Severity { get; set; }
        public string Remarks { get; set; }
        public string CheckStatus { get; set; }// check action and pqaddress update
        public string ActionHistory { get; set; }

        public List<int> CDispositionRowId { get; set; }
    }

    public class PQUpdateSpecialCheckTeamMemberViewModel
    {
        [ScaffoldColumn(false)]
        public int SpecialCheckRowID { get; set; }

        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public short TMAllocatedTo { get; set; }
        public DateTime? TMAllocatedDate { get; set; }

        public string PVMgrCheckStatus { get; set; }
        public short PVMgrAllocatedTo { get; set; }
        public DateTime? PVMgrAllocatedDate { get; set; }

        public string ActionRemark { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Check Status : ")]
        public string CheckStatus { get; set; }
    }

    public class UpdateSpecialCheckInsuffClearViewModel
    {
        [ScaffoldColumn(false)]
        public int SpecialCheckRowID { get; set; }
        public short ClientRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Check Status : ")]
        public string CheckStatus { get; set; }
        public string ReWorkCheckStatus { get; set; }
        public short SubCheckRowID { get; set; }
        public string UniqueComponentID { get; set; }
        public short InfSuffRaiseBy { get; set; }
        public DateTime? InfSuffRaisedDate { get; set; }
        public short InfSuffClearBy { get; set; }
        public DateTime? InfSuffClearDate { get; set; }
        [Display(Name = "Insuff Remarks : ")]
        public string INFRemarks { get; set; }
        
    }

  
}
