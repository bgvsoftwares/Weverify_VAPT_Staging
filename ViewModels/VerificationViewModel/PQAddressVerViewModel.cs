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
    public class PQPersonalDetailsVerViewModel
    {

        public int AddressVerRowID { get; set; }

        public int AddressRowID { get; set; }

        public string AddressName { get; set; }

        public short ClientRowID { get; set; }

        public string ClientName { get; set; }

        public int SubCheckRowID { get; set; }
        public short CheckFamilyRowID { get; set; }

        public int PersonalRowID { get; set; }

        public string PersonalName { get; set; }

        public string UniqueComponentID { get; set; }

        public string CandidateName { get; set; }

        public string CompanyReferenceNo { get; set; }

        public string SubReferenceID { get; set; }

        public string MobileNo { get; set; }

        public string AllocatedTMBy { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }

        public string CheckStatus { get; set; }

        public string CheckName { get; set; }

        public string SecuritasId { get; set; }
        public string AccountName { get; set; }
        public string ProcessName { get; set; }
        public string CheckStatusID { get; set; }
        public string EntityName { get; set; }


        public string collegeName { get; set; }

        public string employmentName { get; set; }
        public string PackageCheckDetails { get; set; }
        public string AntecedentRowID { get; set; }
        public string DataEntryRowID { get; set; }
        public string OfficialEmailID { get; set; }
        public string ContactNumber { get; set; }

        public string ModeOfVerification { get; set; }
        public string CreatedBySource { get; set; }
        public string ClientRefId { get; set; }
        public string DOJ { get; set; }
        public string EmailID { get; set; }
        public string CareerBandName { get; set; }
        public string PatternName { get; set; }

    }
    public class SpecialInstructionVerViewModel
    {

        public string Instructions { get; set; }
        public string Department { get; set; }
        public int Priority { get; set; }

    }

    public class PQAddressVerViewModel
    {
        [ScaffoldColumn(false)]
        public int AddressVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }

        [ScaffoldColumn(false)]
        public int AddressRowID { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        public int CheckStatusId { get; set; }
        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }

        //public string Disposition { get; set; }
        //public string DispositionID { get; set; }
        //public int ClientDispositionRowId { get; set; }
        //public string ActionHistory { get; set; }

        #region Address Verification Antecedents

        [MaxLength(200)]
        public string AV_Add { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_Add_Line1 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_Add_Line2 { get; set; }           //TextArea

        [MaxLength(100)]
        public string AV_Buldng_Street_name { get; set; }           //TextBox

        public short AV_State { get; set; }           // Drop Down - State ref id drop down//change add string to short  5-12-2016
        public string StateName { get; set; }
        public short AV_Country { get; set; }
        public string CountryName { get; set; }
        public short AV_District { get; set; }           // Drop Down - District ref id drop down //change add string to short  5-12-2016
        public string DistrictName { get; set; }

        public int AV_City { get; set; }           // Drop Down - City Master

        public string CityName { get; set; }

        public int AV_Pincode { get; set; }           //TextBox - Pincode ref id drop down//change add string to short  5-12-2016

        [MaxLength(100)]
        public string AV_Landmark { get; set; }           //TextBox

        [MaxLength(100)]
        public string AV_Add_Type { get; set; }           // Drop Down - Own/Rented/company provide/PG/Relative house/Others

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [MaxLength(100)]
        public string AV_Dur_of_Stay { get; set; }           //To & From Calender

        [MaxLength(50)]
        public string AV_2_3BHK { get; set; }           //TextBox

        [MaxLength(50)]
        public string AV_Applcnt_Known_Hw_Mny_Yrs { get; set; }           //TextBox

        [MaxLength(100)]
        public string AV_Resp_Name { get; set; }           //TextBox

        [MaxLength(50)]
        public string AV_Resp_Relation_With_Cand { get; set; }           //drop down Respondent Relationship with candidate -Relationship master

        [MaxLength(200)]
        public string AV_Resp_ConDet { get; set; }           //TextBox

        [MaxLength(20)]
        public string AV_Resp_ConNo { get; set; }           //TextBox

        [MaxLength(100)]
        public string AV_Neighbour_Name_No1 { get; set; }           //TextArea

        [MaxLength(100)]
        public string AV_Neighbour_Name_No2 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_Resp_Remarks { get; set; }           //TextArea

        [MaxLength(100)]
        public string AV_Security_Confirm { get; set; }           //Dropdown -security type

        [MaxLength(100)]
        public string AV_Owned_Rent_Hypoth { get; set; }           // Dropdown -Owned or Rented or Hypothecated --

        [MaxLength(200)]
        public string AV_New_Add_Det_Obtain { get; set; }           //TextArea       

        [MaxLength(20)]
        public string AV_House_Photo_taken { get; set; }           //Drop Down Yes/No

        [MaxLength(100)]
        public string HousePhotographName { get; set; }           //Not show on page

        [MaxLength(100)]
        public string AV_Buldng_Status { get; set; }           //TextBox        

        public byte AV_Family_Member { get; set; }           // Drop Down - Nos dropdown 

        [MaxLength(30)]
        public string AV_Color_of_Buldng { get; set; }           // Drop Down- color master

        [MaxLength(50)]
        public string AV_Interior_Condition { get; set; }           //Drop Down -Good/Bad 

        public byte AV_Total_Earning_Member { get; set; }           //Drop Down -Nos dropdown //change add byte to string 5-12-2016

        [MaxLength(100)]
        public string AV_Ease_Locating { get; set; }           // Drop Down-Ease type

        [MaxLength(30)]
        public string AV_Gate_Color { get; set; }           //Drop Down-color master

        public byte AV_No_Vech { get; set; }           //Drop Down -Nos dropdown //change add byte to string 5-12-2016

        [MaxLength(20)]
        public string AV_Applcnt_Demons_Unsual_Behv { get; set; }           //Drop Down -yes/No/cant Say

        [MaxLength(30)]
        public string AV_Idproof_Prov_Resp { get; set; }           //Drop Down -yes/Not available/refused

        [MaxLength(100)]
        public string AV_Income_Source { get; set; }           //TextBox 

        [MaxLength(30)]
        public string AV_Applcnt_Habitul_Borrow_Neigh { get; set; }           // Drop Down yes/No/cant Say

        [MaxLength(30)]
        public string AV_Any_Union_Poltcl_Affl_Fmly { get; set; }           // Drop Down yes/No/cant Say

        [MaxLength(50)]
        public string AV_Locality { get; set; }           // Drop Down -Middle 

        [MaxLength(20)]
        public string AV_Moral_Charac_Applcnt { get; set; }           // Drop Down Good/Bad

        [MaxLength(100)]
        public string AV_Sec_field_Exe_Name { get; set; }           //TextBox

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? AV_Date_Verification { get; set; }           //Date 

        [MaxLength(20)]
        public string AV_Time_Verification { get; set; }           // Time slot (store time only without date)

        [MaxLength(200)]
        public string AV_Any_Other_Cmmnts_AV { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails6 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails7 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails8 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails9 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails10 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails11 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails12 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails13 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails14 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails15 { get; set; }           //TextArea

        [MaxLength(50)]
        public string ATA_CID_No { get; set; }          //TextBox

        [MaxLength(200)]
        public string ATA_Cmpny_Addr { get; set; }          //TextArea

        [MaxLength(100)]
        public string ATA_ApprxEmpWorking { get; set; }          //TextBox

        [MaxLength(200)]
        public string ATA_Cmpny_Website { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Decoy_Call { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Reverse_Directory { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Residcl_Commrcl { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Stock_Exchnge { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Telphne_Directry_Srch { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Yellow_Pages { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Who_Domain { get; set; }           //TextArea

        [MaxLength(200)]
        public string ATA_GoogleSearch { get; set; }               //TextArea

        [MaxLength(200)]
        public string ATA_InfrastructureOfCmp { get; set; }                         //TextArea

        [MaxLength(200)]
        public string ATA_Just_Dial { get; set; }                                   //TextArea

        [MaxLength(200)]
        public string ATA_Neighbor_Check1 { get; set; }                             //TextArea

        [MaxLength(200)]
        public string ATA_NASCOM_Empanelment { get; set; }                          //TextArea

        [MaxLength(200)]
        public string ATA_Photograph { get; set; }                                  //TextArea

        [MaxLength(200)]
        public string ATA_Physical_Site { get; set; }                               //TextArea

        [MaxLength(200)]
        public string ATA_Reg_Company { get; set; }                                 //TextArea

        [MaxLength(200)]
        public string ATA_PO_Courier_Check { get; set; }                            //TextArea

        [MaxLength(200)]
        public string ATA_Network_Sol { get; set; }                                 //TextArea

        public byte Status { get; set; }

        [MaxLength(50)]
        public string CheckStatus { get; set; }

        #endregion
        
        public string FatherTitle { get; set; }
        
        public string FatherName { get; set; }
        public string ApproveStatus { get; set; }

        [MaxLength(100)]
        public string TypeRevert { get; set; }           // Drop Down - verble/writen/online/Others mode of verification
    }

    public class UpdatePQAddressVerViewModel
    {
        [ScaffoldColumn(false)]
        public int AddressVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }

        [ScaffoldColumn(false)]
        public int AddressRowID { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        public int CheckStatusId { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }

        #region Address Verification Antecedents

        [MaxLength(200)]
        public string AV_Add { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_Add_Line1 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_Add_Line2 { get; set; }           //TextArea

        [MaxLength(100)]
        public string AV_Buldng_Street_name { get; set; }           //TextBox

        public short AV_State { get; set; }           // Drop Down - State ref id drop down//change add string to short  5-12-2016
        public string StateName { get; set; }
        public short AV_Country { get; set; }
        public string CountryName { get; set; }
        public short AV_District { get; set; }           // Drop Down - District ref id drop down //change add string to short  5-12-2016
        public string DistrictName { get; set; }

        public int AV_City { get; set; }           // Drop Down - City Master

        public string CityName { get; set; }

        public int AV_Pincode { get; set; }           //TextBox - Pincode ref id drop down//change add string to short  5-12-2016

        [MaxLength(100)]
        public string AV_Landmark { get; set; }           //TextBox

        [MaxLength(100)]
        public string AV_Add_Type { get; set; }           // Drop Down - Own/Rented/company provide/PG/Relative house/Others

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [MaxLength(100)]
        public string AV_Dur_of_Stay { get; set; }           //To & From Calender

        [MaxLength(50)]
        public string AV_2_3BHK { get; set; }           //TextBox

        [MaxLength(50)]
        public string AV_Applcnt_Known_Hw_Mny_Yrs { get; set; }           //TextBox

        [MaxLength(100)]
        public string AV_Resp_Name { get; set; }           //TextBox

        [MaxLength(50)]
        public string AV_Resp_Relation_With_Cand { get; set; }           //drop down Respondent Relationship with candidate -Relationship master

        [MaxLength(200)]
        public string AV_Resp_ConDet { get; set; }           //TextBox

        [MaxLength(20)]
        public string AV_Resp_ConNo { get; set; }           //TextBox

        [MaxLength(100)]
        public string AV_Neighbour_Name_No1 { get; set; }           //TextArea

        [MaxLength(100)]
        public string AV_Neighbour_Name_No2 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_Resp_Remarks { get; set; }           //TextArea

        [MaxLength(100)]
        public string AV_Security_Confirm { get; set; }           //Dropdown -security type

        [MaxLength(100)]
        public string AV_Owned_Rent_Hypoth { get; set; }           // Dropdown -Owned or Rented or Hypothecated --

        [MaxLength(200)]
        public string AV_New_Add_Det_Obtain { get; set; }           //TextArea       

        [MaxLength(20)]
        public string AV_House_Photo_taken { get; set; }           //Drop Down Yes/No

        [MaxLength(100)]
        public string HousePhotographName { get; set; }           //Not show on page

        [MaxLength(100)]
        public string AV_Buldng_Status { get; set; }           //TextBox        

        public byte AV_Family_Member { get; set; }           // Drop Down - Nos dropdown 

        [MaxLength(30)]
        public string AV_Color_of_Buldng { get; set; }           // Drop Down- color master

        [MaxLength(50)]
        public string AV_Interior_Condition { get; set; }           //Drop Down -Good/Bad 

        public byte AV_Total_Earning_Member { get; set; }           //Drop Down -Nos dropdown //change add byte to string 5-12-2016

        [MaxLength(100)]
        public string AV_Ease_Locating { get; set; }           // Drop Down-Ease type

        [MaxLength(30)]
        public string AV_Gate_Color { get; set; }           //Drop Down-color master

        public byte AV_No_Vech { get; set; }           //Drop Down -Nos dropdown //change add byte to string 5-12-2016

        [MaxLength(20)]
        public string AV_Applcnt_Demons_Unsual_Behv { get; set; }           //Drop Down -yes/No/cant Say

        [MaxLength(30)]
        public string AV_Idproof_Prov_Resp { get; set; }           //Drop Down -yes/Not available/refused

        [MaxLength(100)]
        public string AV_Income_Source { get; set; }           //TextBox 

        [MaxLength(30)]
        public string AV_Applcnt_Habitul_Borrow_Neigh { get; set; }           // Drop Down yes/No/cant Say

        [MaxLength(30)]
        public string AV_Any_Union_Poltcl_Affl_Fmly { get; set; }           // Drop Down yes/No/cant Say

        [MaxLength(50)]
        public string AV_Locality { get; set; }           // Drop Down -Middle 

        [MaxLength(20)]
        public string AV_Moral_Charac_Applcnt { get; set; }           // Drop Down Good/Bad

        [MaxLength(100)]
        public string AV_Sec_field_Exe_Name { get; set; }           //TextBox

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? AV_Date_Verification { get; set; }           //Date 

        [MaxLength(20)]
        public string AV_Time_Verification { get; set; }           // Time slot (store time only without date)

        [MaxLength(200)]
        public string AV_Any_Other_Cmmnts_AV { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails6 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails7 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails8 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails9 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails10 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails11 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails12 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails13 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails14 { get; set; }           //TextArea

        [MaxLength(200)]
        public string AV_OtherDetails15 { get; set; }           //TextArea

        [MaxLength(50)]
        public string ATA_CID_No { get; set; }          //TextBox

        [MaxLength(200)]
        public string ATA_Cmpny_Addr { get; set; }          //TextArea

        [MaxLength(100)]
        public string ATA_ApprxEmpWorking { get; set; }          //TextBox

        [MaxLength(200)]
        public string ATA_Cmpny_Website { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Decoy_Call { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Reverse_Directory { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Residcl_Commrcl { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Stock_Exchnge { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Telphne_Directry_Srch { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Yellow_Pages { get; set; }          //TextArea

        [MaxLength(200)]
        public string ATA_Who_Domain { get; set; }           //TextArea

        [MaxLength(200)]
        public string ATA_GoogleSearch { get; set; }               //TextArea

        [MaxLength(200)]
        public string ATA_InfrastructureOfCmp { get; set; }                         //TextArea

        [MaxLength(200)]
        public string ATA_Just_Dial { get; set; }                                   //TextArea

        [MaxLength(200)]
        public string ATA_Neighbor_Check1 { get; set; }                             //TextArea

        [MaxLength(200)]
        public string ATA_NASCOM_Empanelment { get; set; }                          //TextArea

        [MaxLength(200)]
        public string ATA_Photograph { get; set; }                                  //TextArea

        [MaxLength(200)]
        public string ATA_Physical_Site { get; set; }                               //TextArea

        [MaxLength(200)]
        public string ATA_Reg_Company { get; set; }                                 //TextArea

        [MaxLength(200)]
        public string ATA_PO_Courier_Check { get; set; }                            //TextArea

        [MaxLength(200)]
        public string ATA_Network_Sol { get; set; }                                 //TextArea

        public byte Status { get; set; }

        [MaxLength(50)]
        public string CheckStatus { get; set; }

        #endregion

        #region Address Closer fields

        public VerificationCloserViewModel CloserViewModel { get; set; }
     
        #endregion

        public bool Aging { get; set; }
    }

    public class PQAddressVerGetColorNameViewModel
    {
        public string SeverityName { get; set; }
        public string ColorName { get; set; }
        public int SeverityRowId { get; set; }
    }

    public class VerificationCloserViewModel
    {
        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }
        [ScaffoldColumn(false)]
        public int AddressRowID { get; set; }
        [ScaffoldColumn(false)]
        public int AddressVerRowID { get; set; }
        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }
        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }
        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }

        public string Disposition { get; set; }

        public string DispositionID { get; set; }
       // public int ClientDispositionRowId { get; set; }
        public string ColorName { get; set; }
        public string Severity { get; set; }
        public string Remarks { get; set; }
        public string CheckStatus { get; set; }// check action and pqaddress update
        public string ActionHistory { get; set; }
        public string ShortRemarks { get; set; }
        public short InfSuffRaiseBy { get; set; }

        public DateTime? InfSuffRaisedDate { get; set; }
        public byte StoppedCheckStatus { get; set; }
        public short StoppedCheckBy { get; set; }
        public DateTime? StoppedCheckDate { get; set; }

        public List<int> CDispositionRowId { get; set; }
    }
    
}
