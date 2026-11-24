using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels.VerificationViewModel
{
    public class PQCriminalVerViewModel
    {
        [ScaffoldColumn(false)]
        public int CriminalVerRowId { get; set; }

        [ScaffoldColumn(false)]
        public int CriminalRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }
        public int CheckStatusId { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
        public string PersonalName { get; set; }

        [ScaffoldColumn(false)]
        public string UniqueComponentID { get; set; }

        public string CandidateName { get; set; }

        public string CompanyReferenceNo { get; set; }

        [ScaffoldColumn(false)]
        public string SubReferenceID { get; set; }

        public string MobileNo { get; set; }

        public string AllocatedTMBy { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }


        [MaxLength(100)]
        public string CRV_Cand_Name { get; set; }   // Text Box

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CRV_DOB { get; set; }                   //Date Text Field

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CRV_DOB1 { get; set; }                   //Date Text Field

        [MaxLength(100)]
        public string CRV_Father_Name { get; set; }             // Text Box

        [MaxLength(50)]
        public string CRV_First_Name { get; set; }              // Text Box

        [MaxLength(50)]
        public string CRV_Last_Name { get; set; }               // Text Box

        [MaxLength(50)]
        public string CRV_Nationality { get; set; }             // Text Box

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string CRV_Name_Chngd_Effect_Frm { get; set; }   // date mm/yy

        [MaxLength(50)]
        public string CRV_Nationality_On_Passport { get; set; } // Text Box

        [MaxLength(100)]
        public string CRV_Country_of_Issue { get; set; }        // Text Box

        [MaxLength(200)]
        public string CRV_Addr_Type { get; set; }               // drop down 

        [MaxLength(4000)]
        public string CRV_Current_Addr { get; set; }            //Text Area

        [MaxLength(200)]
        public string CRV_Prev_Addr1 { get; set; }              //Text Area

        [MaxLength(200)]
        public string CRV_Prev_Addr2 { get; set; }              //Text Area

        [MaxLength(200)]
        public string CRV_Prev_Addr3 { get; set; }              //Text Area

        [MaxLength(200)]
        public string CRV_Prev_Addr4 { get; set; }              //Text Area

        [MaxLength(4000)]
        public string CRV_Permanent_Addr { get; set; }          //Text Area

        [MaxLength(30)]
        public string CRV_Gender { get; set; }                  //drop  down Male / Female / Other 

        [MaxLength(100)]
        public string CRV_Identity_No { get; set; }             // text box

        [MaxLength(200)]
        public string CRV_Identity_Prov { get; set; }           //List Box	Multiple selection 

        [MaxLength(100)]
        public string CRV_Doc_Received { get; set; }            //List Box	Multiple selection 

        [MaxLength(100)]
        public string CRV_Dur_Yr_Covered { get; set; }          // Date Range

        [MaxLength(100)]
        public string CRV_Period_Of_Stay { get; set; }          // Date Range

        [MaxLength(100)]
        public string CRV_Pol_Station_Name { get; set; }        //Police Station Masters 

        [MaxLength(100)]
        public string CRV_Pol_Authority_Name { get; set; }      // Text Field

        [MaxLength(100)]
        public string CRV_Pol_Authority_Desg { get; set; }      //Text Field

        [MaxLength(100)]
        public string CRV_Criminal_Verf { get; set; }     // drop down

        [MaxLength(100)]
        public string CRV_Type_of_Revert { get; set; }     // drop down    Written / Verbal / Online / Other

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CRV_Date_Addr_Verif { get; set; }   //To be mapped from address verification check

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CRV_Date_Verf { get; set; }// text Field

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CRV_Time_Verf { get; set; }// text Field 

        [MaxLength(200)]
        public string CRV_Verfier_Cmmnts { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_St_Pol_Most_Wanted { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_CBI_Most_Wanted { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Interpol_Most_Wanted { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Allied_Terrorist_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Crimes_Dishonest_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Corruption_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Financ_Rgltry_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Fraud_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Law_Enforce_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Sex_Crime_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Violent_Crime_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Terrorist_Risk { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Money_Laund { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Civil_Court_Action { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_District_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_State_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Civil_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Special_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Session_Court { get; set; }// Drop Down 

        [MaxLength(50)]
        public string CRV_Tribunals { get; set; }// Drop Down 

        [MaxLength(200)]
        public string CRV_Any_Other_Cmmnts_CRV { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Magistrate_Court { get; set; }// Drop Down

        [MaxLength(100)]
        public string CRV_District_Civil_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_St_High_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Supreme_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Criminal_Appeal { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Private_Complain_Rpt { get; set; }// Drop Down 

        [MaxLength(200)]
        public string CRV_Others6 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others7 { get; set; }//Text Area

        [MaxLength(4000)]
        public string CRV_Others8 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others9 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others10 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others11 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others12 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others13 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others14 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others15 { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Reverse_Directory { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Residcl_Commrcl { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Stock_Exchnge { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Telphne_Directry_Srch { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Yellow_Pages { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Who_Domain { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_GoogleSearch { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_InfrastructureOfCmp { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Just_Dial { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Neighbor_Check1 { get; set; }//Text Area

        [MaxLength(100)]
        public string ATA_NASCOM_Empanelment { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Photograph { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Physical_Site { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Reg_Company { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_PO_Courier_Check { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Network_Sol { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_ApprxEmpWorking { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_CID_No { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Cmpny_Addr { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Decoy_Call { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Cmpny_Website { get; set; }//Text Area

        [MaxLength(50)]
        public string CheckStatus { get; set; }
        public string ApproveStatus { get; set; }


        public byte Status { get; set; }

    }

    public class UpdatePQCriminalVerViewModel
    {
        [ScaffoldColumn(false)]
        public int CriminalVerRowId { get; set; }

        [ScaffoldColumn(false)]
        public int CriminalRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }
       
        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
       
        [ScaffoldColumn(false)]
        public string SubReferenceID { get; set; }
        
        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }
        
        [MaxLength(100)]
        public string CRV_Cand_Name { get; set; }   // Text Box

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CRV_DOB { get; set; }                   //Date Text Field

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CRV_DOB1 { get; set; }                   //Date Text Field

        [MaxLength(100)]
        public string CRV_Father_Name { get; set; }             // Text Box

        [MaxLength(50)]
        public string CRV_First_Name { get; set; }              // Text Box

        [MaxLength(50)]
        public string CRV_Last_Name { get; set; }               // Text Box

        [MaxLength(50)]
        public string CRV_Nationality { get; set; }             // Text Box

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string CRV_Name_Chngd_Effect_Frm { get; set; }   // date mm/yy

        [MaxLength(50)]
        public string CRV_Nationality_On_Passport { get; set; } // Text Box

        [MaxLength(100)]
        public string CRV_Country_of_Issue { get; set; }        // Text Box

        [MaxLength(200)]
        public string CRV_Addr_Type { get; set; }               // drop down 

        [MaxLength(4000)]
        public string CRV_Current_Addr { get; set; }            //Text Area

        [MaxLength(200)]
        public string CRV_Prev_Addr1 { get; set; }              //Text Area

        [MaxLength(200)]
        public string CRV_Prev_Addr2 { get; set; }              //Text Area

        [MaxLength(200)]
        public string CRV_Prev_Addr3 { get; set; }              //Text Area

        [MaxLength(200)]
        public string CRV_Prev_Addr4 { get; set; }              //Text Area

        [MaxLength(4000)]
        public string CRV_Permanent_Addr { get; set; }          //Text Area

        [MaxLength(30)]
        public string CRV_Gender { get; set; }                  //drop  down Male / Female / Other 

        [MaxLength(100)]
        public string CRV_Identity_No { get; set; }             // text box

        [MaxLength(200)]
        public string CRV_Identity_Prov { get; set; }           //List Box	Multiple selection 

        [MaxLength(100)]
        public string CRV_Doc_Received { get; set; }            //List Box	Multiple selection 

        [MaxLength(100)]
        public string CRV_Dur_Yr_Covered { get; set; }          // Date Range

        [MaxLength(100)]
        public string CRV_Period_Of_Stay { get; set; }          // Date Range

        [MaxLength(100)]
        public string CRV_Pol_Station_Name { get; set; }        //Police Station Masters 

        [MaxLength(100)]
        public string CRV_Pol_Authority_Name { get; set; }      // Text Field

        [MaxLength(100)]
        public string CRV_Pol_Authority_Desg { get; set; }      //Text Field

        [MaxLength(100)]
        public string CRV_Criminal_Verf { get; set; }     // drop down

        [MaxLength(100)]
        public string CRV_Type_of_Revert { get; set; }     // drop down    Written / Verbal / Online / Other

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CRV_Date_Addr_Verif { get; set; }   //To be mapped from address verification check

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CRV_Date_Verf { get; set; }// text Field

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CRV_Time_Verf { get; set; }// text Field 

        [MaxLength(200)]
        public string CRV_Verfier_Cmmnts { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_St_Pol_Most_Wanted { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_CBI_Most_Wanted { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Interpol_Most_Wanted { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Allied_Terrorist_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Crimes_Dishonest_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Corruption_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Financ_Rgltry_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Fraud_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Law_Enforce_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Sex_Crime_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Violent_Crime_Dataset { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Terrorist_Risk { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Global_Money_Laund { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Civil_Court_Action { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_District_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_State_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Civil_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Special_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Session_Court { get; set; }// Drop Down 

        [MaxLength(50)]
        public string CRV_Tribunals { get; set; }// Drop Down 

        [MaxLength(200)]
        public string CRV_Any_Other_Cmmnts_CRV { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Magistrate_Court { get; set; }// Drop Down

        [MaxLength(100)]
        public string CRV_District_Civil_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_St_High_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Supreme_Court { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Criminal_Appeal { get; set; }// Drop Down 

        [MaxLength(100)]
        public string CRV_Private_Complain_Rpt { get; set; }// Drop Down 

        [MaxLength(200)]
        public string CRV_Others6 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others7 { get; set; }//Text Area

        [MaxLength(4000)]
        public string CRV_Others8 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others9 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others10 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others11 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others12 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others13 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others14 { get; set; }//Text Area

        [MaxLength(100)]
        public string CRV_Others15 { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Reverse_Directory { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Residcl_Commrcl { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Stock_Exchnge { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Telphne_Directry_Srch { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Yellow_Pages { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Who_Domain { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_GoogleSearch { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_InfrastructureOfCmp { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Just_Dial { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Neighbor_Check1 { get; set; }//Text Area

        [MaxLength(100)]
        public string ATA_NASCOM_Empanelment { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Photograph { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Physical_Site { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Reg_Company { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_PO_Courier_Check { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Network_Sol { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_ApprxEmpWorking { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_CID_No { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Cmpny_Addr { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Decoy_Call { get; set; }//Text Area

        [MaxLength(200)]
        public string ATA_Cmpny_Website { get; set; }//Text Area

        [MaxLength(50)]
        public string CheckStatus { get; set; }
        public int CheckStatusId { get; set; }
        public bool Aging { get; set; }

        #region Criminal Closer fields

        public CriminalVerCloserViewModel CloserViewModel { get; set; }

        #endregion
    }
    
    public class CriminalVerCloserViewModel
    {
        [ScaffoldColumn(false)]
        public int CriminalVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int CriminalRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        public int CheckStatusId { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }

        public string Disposition { get; set; }
        public string DispositionID { get; set; }
        //public int ClientDispositionRowId { get; set; }
        public string ColorName { get; set; }
        public string Severity { get; set; }

        [AllowHtml]
        public string Remarks { get; set; }
        public string CheckStatus { get; set; }// check action and pqaddress update
        public string ActionHistory { get; set; }
        public string ShortRemarks { get; set; }
        public List<int> CDispositionRowId { get; set; }
    }
}
