using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.PartnerViewModels
{
    public class PQAddressVerPartenerViewModel
    {
        public int AddressRowID { get; set; }

        public int VendorLoginRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }

        [ScaffoldColumn(false)]
        public int SubCheckRowId { get; set; }

        [MaxLength(100)]
        public string SubCheckName { get; set; }

        [MaxLength(100)]
        public string AV_Cand_Name { get; set; }

        [MaxLength(100)]
        public string AV_Add { get; set; }

        [MaxLength(100)]
        public string AV_AddLine1 { get; set; }

        [MaxLength(100)]
        public string AV_AddLine2 { get; set; }

        [MaxLength(100)]
        public string AV_Buldng_Street_Name { get; set; }

        [MaxLength(100)]
        public string AV_State { get; set; }

        [MaxLength(100)]
        public string AV_District { get; set; }

        [MaxLength(100)]
        public string AV_City { get; set; }

        [MaxLength(100)]
        public string AV_Landmark { get; set; }

        [MaxLength(20)]
        public string CheckStatus { get; set; }

        public int AV_Pincode { get; set; }
        public DateTime? InternalOutDate { get; set; }

        // public DateTime? CreatedDate { get; set; }
        // public bool IsSynced { get; set; }           //Used for Web to Mobile Sync
        // public int SyncedBy { get; set; }            //Used for Web to Mobile Sync
        //  public DateTime? SyncedOn { get; set; }      //Used for Web to Mobile Sync

        //[MaxLength(10)]
        //public string IsAddressChanged { get; set; }//Yes/No

        //[MaxLength(200)]
        //public string Address { get; set; }

        //[MaxLength(200)]
        //public string DurationOfStay { get; set; }

        //[MaxLength(100)]
        //public string AccomodationType { get; set; }

        //[MaxLength(100)]
        //public string RespName { get; set; }

        //[MaxLength(100)]
        //public string RespIdDetails { get; set; }

        //[MaxLength(50)]
        //public string RespRelation { get; set; }

        //[MaxLength(50)]
        //public string RespContactDetails { get; set; }//Drop Down

        //[MaxLength(50)]
        //public string NeighbourNameAndNos { get; set; }

        //[MaxLength(200)]
        //public string AnyOtherComments { get; set; }

        //[MaxLength(20)]
        //public string PtrStatus { get; set; } //Verified/Refused/Shifted/Un Traceable

        //[MaxLength(100)]
        //public string UpdatedBy { get; set; }

        //public DateTime? UpdatedDate { get; set; }

        //[MaxLength(200)]
        //public string Longitude { get; set; }

        //[MaxLength(200)]
        //public string Latitude { get; set; }
    }

    public class AddPQAddressVerPartenerViewModel
    {
        public int AddressRowID { get; set; }

        public int VendorLoginRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }

        [ScaffoldColumn(false)]
        public int SubCheckRowId { get; set; }

        [MaxLength(100)]
        public string SubCheckName { get; set; }

        [MaxLength(100)]
        public string AV_Cand_Name { get; set; }

        [MaxLength(100)]
        public string AV_Add { get; set; }

        [MaxLength(100)]
        public string AV_AddLine1 { get; set; }

        [MaxLength(100)]
        public string AV_AddLine2 { get; set; }

        [MaxLength(100)]
        public string AV_Buldng_Street_Name { get; set; }

        [MaxLength(100)]
        public string AV_State { get; set; }

        [MaxLength(100)]
        public string AV_District { get; set; }

        [MaxLength(100)]
        public string AV_City { get; set; }
        
        public int AV_Pincode { get; set; }

        [MaxLength(100)]
        public string AV_Landmark { get; set; }

        public DateTime? InternalOutDate { get; set; }

        [MaxLength(20)]
        public string CheckStatus { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class VerificationPartenerListPagedModel
    {
        public IEnumerable<AddressVerPartenerUserListViewModel> PartenerUserChecks { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class AddressVerPartenerUserListViewModel
    {
        public int PQAddressVarPtrRowId { get; set; }
        public int AddressRowID { get; set; }
        public int VendorLoginRowID { get; set; }
        public int PersonalRowID { get; set; }
        public int SubCheckRowId { get; set; }
        public string SubCheckName { get; set; }
        public string AV_Cand_Name { get; set; }
        public string AV_Add { get; set; }
        public string AV_AddLine1 { get; set; }
        public string AV_AddLine2 { get; set; }
        public string AV_Buldng_Street_Name { get; set; }
        public string AV_State { get; set; }
        public string AV_District { get; set; }
        public string AV_City { get; set; }
        public int AV_Pincode { get; set; }
        public string AV_Landmark { get; set; }
        public string CheckStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsSynced { get; set; }
    }

    public class UpdateAddressVerPartnerViewModel
    {
        [Required]
        public int PQAddressVarPtrRowId { get; set; }

        [Required]
        public int AddressRowID { get; set; }

        [Required]
        public int VendorLoginRowID { get; set; }

        [Required]
        public int PersonalRowID { get; set; }

        [Required]
        public int SubCheckRowId { get; set; }

        [Display(Name = "Is Address Changed : ")]
        public string IsAddressChanged { get; set; }

        [Display(Name = "Address : ")]
        public string Address { get; set; }

        [Display(Name = "Landmark : ")]
        public string Landmark { get; set; }

        [Display(Name = "Duration of Stay : ")]
        public string DurationOfStay { get; set; }

        [Display(Name = "Accomodation Type : ")]
        public string AccomodationType { get; set; }

        [Display(Name = "Respondent Name : ")]
        public string RespName { get; set; }

        [Display(Name = "Respondent ID Details : ")]
        public string RespIdDetails { get; set; }

        [Display(Name = "Respondent Relationship  : ")]
        public string RespRelation { get; set; }

        [Display(Name = "Respondent contact details : ")]
        public string RespContactDetails { get; set; }

        [Display(Name = "Neighbour Name And Nos : ")]
        public string NeighbourNameAndNos { get; set; }

        [Display(Name = "Any Other Comments :")]
        public string AnyOtherComments { get; set; }

        [Display(Name = "Partner Status : ")]
        public string PtrStatus { get; set; }
        
        [Required]
        [Display(Name = "Check Status : ")]
        public string CheckStatus { get; set; }

        [Display(Name = "Partner Check Status :")]
        public string MobileCheckStatus { get; set; }

        [ScaffoldColumn(false)]
        public int UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Date of Verification :")]
        [ScaffoldColumn(false)]
        public DateTime? DateOfVerification { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        public bool IsSynced { get; set; }
        [Display(Name = "Resp Id Type : ")]
        public string RespIdType { get; set; }

        public string SecurityConfirm { get; set; }
        public string SocietyBoard { get; set; }
        public string Photo_Taken { get; set; }
        public string AgencyExecutive_Name { get; set; }
        public int clientRowid { get; set; }

        //// For TCS IT /////////////////
       
        public string NatureofLocation { get; set; }
        public string ModeofVerification { get; set; }
        public string AddressProximity { get; set; }
        public string SelfieOfRespObt { get; set; }
        public string RespIDProof { get; set; }
        public string HousePhotoOfResp { get; set; }
        public string AddProofOfTheResp { get; set; }

    }


}
