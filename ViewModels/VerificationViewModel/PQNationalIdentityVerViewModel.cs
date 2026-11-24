using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels.VerificationViewModel
{
    public class PQNationalIdentityVerViewModel
    {
        [ScaffoldColumn(false)]
        public int NationalIdentityVerRowID { get; set; }


        [ScaffoldColumn(false)]
        public int NationalIdentityRowID { get; set; }

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
        public string NIC_Cand_Name { get; set; }        //text filed

        [MaxLength(200)]
        public string NIC_Any_Other_Cmmnts_NIC { get; set; }         // text Area

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NIC_DOB1 { get; set; }          // date                      

        [MaxLength(100)]
        public string NIC_ID_No { get; set; }            // text filed                       

        [MaxLength(100)]
        public string NIC_Verified_By { get; set; }      // text filed                       


        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string NIC_Date_Issue { get; set; }       // text filed                       


        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string NIC_Expiry_Date { get; set; }      // text filed  

        [MaxLength(100)]
        public string NIC_Validity_Status { get; set; }       // Drop down -Valid / Expired / Suspended / Revoked / Others  

        [MaxLength(100)]
        public string NIC_Passport_No { get; set; }       //text filed  

        [MaxLength(100)]
        public string NIC_SSN_NO { get; set; }        //text filed  

        [MaxLength(100)]
        public string NIC_Pan_No { get; set; }        //text filed  

        [MaxLength(100)]
        public string NIC_Voter_Id_No { get; set; }   //text filed  

        [MaxLength(100)]
        public string NIC_Dvng_Liscense_No { get; set; }         //text filed  

        [MaxLength(100)]
        public string NIC_Aadhar_Card_No { get; set; }           //text filed  

        [MaxLength(100)]
        public string NIC_Place_Issue { get; set; }              //text filed  

        [MaxLength(100)]
        public string NIC_Place_Residnce { get; set; }  //text filed  

        [MaxLength(100)]
        public string NIC_Time_Verf { get; set; }      //text filed   

        [MaxLength(100)]
        public string NIC_Type_Id_Card { get; set; }   //text filedpro   

        [MaxLength(100)]
        public string NIC_Reg_Doc_Type { get; set; }   //text filed   

        [MaxLength(100)]
        public string NIC_RegNo { get; set; }          //text filed  

        [MaxLength(100)]
        public string NIC_Name_Field_Executive { get; set; }  //text filed    

        [MaxLength(100)]
        public string NIC_PinCode { get; set; }                  //text filed  

        [MaxLength(100)]
        public string NIC_Relation_Key_Personal { get; set; }     //text filed   

        [MaxLength(100)]
        public string NIC_Verified_Frm { get; set; }             //text filed  

        [MaxLength(100)]
        public string NIC_Auth_Details_Name { get; set; }        //text filed  

        [MaxLength(100)]
        public string NIC_Dsg { get; set; }       //text filed  

        [MaxLength(100)]
        public string NIC_Eid { get; set; }       //text filed  

        [MaxLength(100)]
        public string NIC_MobNo { get; set; }     //text filed  

        [MaxLength(100)]
        public string NIC_Name_Key_Personnel { get; set; }       //text filed   

        [MaxLength(100)]
        public string NIC_AliasFname { get; set; }               //text filed  

        [MaxLength(100)]
        public string NIC_Alias_Lname { get; set; }              //text filed  

        [MaxLength(100)]
        public string NIC_Country_Iss_Passport { get; set; }     //text filed   

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NIC_Date_Verif { get; set; }               //date   

        [MaxLength(100)]
        public string NIC_Id_CardNo { get; set; }//text filed  

        [MaxLength(100)]
        public string NIC_Identity_Prov { get; set; }// drop down   

        [MaxLength(100)]
        public string NIC_Name_Changed_Effect_Frm { get; set; }// text filed   

        [MaxLength(100)]
        public string NIC_Nationality { get; set; }// text filed   

        [MaxLength(100)]
        public string NIC_PhotoID_Verif { get; set; }// Verified / Not verified / Not possible / Other  

        [MaxLength(100)]
        public string NIC_Any_Other_Remarks_NIC { get; set; }// text filed  

        [MaxLength(200)]
        public string NIC_OtherDetails6 { get; set; }   //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails7 { get; set; }   //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails8 { get; set; }   //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails9 { get; set; }   //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails10 { get; set; }  //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails11 { get; set; }  //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails12 { get; set; }  //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails13 { get; set; }  //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails14 { get; set; }  //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails15 { get; set; }  //text area  

        [MaxLength(200)]
        public string ATA_Reverse_Directory { get; set; }   //text area  

        [MaxLength(200)]
        public string ATA_Residcl_Commrcl { get; set; }         //text area  

        [MaxLength(200)]
        public string ATA_Stock_Exchnge { get; set; }           //text area  

        [MaxLength(200)]
        public string ATA_Telphne_Directry_Srch { get; set; }   //text area  

        [MaxLength(200)]
        public string ATA_Yellow_Pages { get; set; }            //text area  

        [MaxLength(200)]
        public string ATA_Who_Domain { get; set; }              //text area  

        [MaxLength(200)]
        public string ATA_GoogleSearch { get; set; }            //text area  

        [MaxLength(200)]
        public string ATA_InfrastructureOfCmp { get; set; }     //text area  

        [MaxLength(200)]
        public string ATA_Just_Dial { get; set; }               //text area   

        [MaxLength(200)]
        public string ATA_Neighbor_Check1 { get; set; }         //text area   

        [MaxLength(200)]
        public string ATA_NASCOM_Empanelment { get; set; }  //text area  

        [MaxLength(200)]
        public string ATA_Photograph { get; set; }              //text area  

        [MaxLength(200)]
        public string ATA_Physical_Site { get; set; }           //text area  

        [MaxLength(200)]
        public string ATA_Reg_Company { get; set; }             //text area  

        [MaxLength(200)]
        public string ATA_PO_Courier_Check { get; set; }        //text area    

        [MaxLength(200)]
        public string ATA_Network_Sol { get; set; }  //text area  

        [MaxLength(200)]
        public string ATA_ApprxEmpWorking { get; set; } //text area  

        [MaxLength(200)]
        public string ATA_CID_No { get; set; }          //text area   

        [MaxLength(200)]
        public string ATA_Cmpny_Addr { get; set; }      //text area  

        [MaxLength(200)]
        public string ATA_Decoy_Call { get; set; }      //text area  

        [MaxLength(200)]
        public string ATA_Cmpny_Website { get; set; }   //text area

        public string CheckStatus { get; set; }
        public byte Status { get; set; }
        public string ApproveStatus { get; set; }
    }

    public class UpdatePQNationalIdentityVerViewModel
    {
        [ScaffoldColumn(false)]
        public int NationalIdentityVerRowID { get; set; }
        
        [ScaffoldColumn(false)]
        public int NationalIdentityRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }
       
        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
       
        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        [MaxLength(100)]
        public string NIC_Cand_Name { get; set; }        //text filed

        [MaxLength(200)]
        public string NIC_Any_Other_Cmmnts_NIC { get; set; }         // text Area

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NIC_DOB1 { get; set; }          // date                      

        [MaxLength(100)]
        public string NIC_ID_No { get; set; }            // text filed                       

        [MaxLength(100)]
        public string NIC_Verified_By { get; set; }      // text filed                       


        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string NIC_Date_Issue { get; set; }       // text filed                       


        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string NIC_Expiry_Date { get; set; }      // text filed  

        [MaxLength(100)]
        public string NIC_Validity_Status { get; set; }       // Drop down -Valid / Expired / Suspended / Revoked / Others  

        [MaxLength(100)]
        public string NIC_Passport_No { get; set; }       //text filed  

        [MaxLength(100)]
        public string NIC_SSN_NO { get; set; }        //text filed  

        [MaxLength(100)]
        public string NIC_Pan_No { get; set; }        //text filed  

        [MaxLength(100)]
        public string NIC_Voter_Id_No { get; set; }   //text filed  

        [MaxLength(100)]
        public string NIC_Dvng_Liscense_No { get; set; }         //text filed  

        [MaxLength(100)]
        public string NIC_Aadhar_Card_No { get; set; }           //text filed  

        [MaxLength(100)]
        public string NIC_Place_Issue { get; set; }              //text filed  

        [MaxLength(100)]
        public string NIC_Place_Residnce { get; set; }  //text filed  

        [MaxLength(100)]
        public string NIC_Time_Verf { get; set; }      //text filed   

        [MaxLength(100)]
        public string NIC_Type_Id_Card { get; set; }   //text filedpro   

        [MaxLength(100)]
        public string NIC_Reg_Doc_Type { get; set; }   //text filed   

        [MaxLength(100)]
        public string NIC_RegNo { get; set; }          //text filed  

        [MaxLength(100)]
        public string NIC_Name_Field_Executive { get; set; }  //text filed    

        [MaxLength(100)]
        public string NIC_PinCode { get; set; }                  //text filed  

        [MaxLength(100)]
        public string NIC_Relation_Key_Personal { get; set; }     //text filed   

        [MaxLength(100)]
        public string NIC_Verified_Frm { get; set; }             //text filed  

        [MaxLength(100)]
        public string NIC_Auth_Details_Name { get; set; }        //text filed  

        [MaxLength(100)]
        public string NIC_Dsg { get; set; }       //text filed  

        [MaxLength(100)]
        public string NIC_Eid { get; set; }       //text filed  

        [MaxLength(100)]
        public string NIC_MobNo { get; set; }     //text filed  

        [MaxLength(100)]
        public string NIC_Name_Key_Personnel { get; set; }       //text filed   

        [MaxLength(100)]
        public string NIC_AliasFname { get; set; }               //text filed  

        [MaxLength(100)]
        public string NIC_Alias_Lname { get; set; }              //text filed  

        [MaxLength(100)]
        public string NIC_Country_Iss_Passport { get; set; }     //text filed   

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NIC_Date_Verif { get; set; }               //date   

        [MaxLength(100)]
        public string NIC_Id_CardNo { get; set; }//text filed  

        [MaxLength(100)]
        public string NIC_Identity_Prov { get; set; }// drop down   

        [MaxLength(100)]
        public string NIC_Name_Changed_Effect_Frm { get; set; }// text filed   

        [MaxLength(100)]
        public string NIC_Nationality { get; set; }// text filed   

        [MaxLength(100)]
        public string NIC_PhotoID_Verif { get; set; }// Verified / Not verified / Not possible / Other  

        [MaxLength(100)]
        public string NIC_Any_Other_Remarks_NIC { get; set; }// text filed  

        [MaxLength(200)]
        public string NIC_OtherDetails6 { get; set; }   //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails7 { get; set; }   //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails8 { get; set; }   //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails9 { get; set; }   //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails10 { get; set; }  //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails11 { get; set; }  //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails12 { get; set; }  //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails13 { get; set; }  //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails14 { get; set; }  //text area  

        [MaxLength(200)]
        public string NIC_OtherDetails15 { get; set; }  //text area  

        [MaxLength(200)]
        public string ATA_Reverse_Directory { get; set; }   //text area  

        [MaxLength(200)]
        public string ATA_Residcl_Commrcl { get; set; }         //text area  

        [MaxLength(200)]
        public string ATA_Stock_Exchnge { get; set; }           //text area  

        [MaxLength(200)]
        public string ATA_Telphne_Directry_Srch { get; set; }   //text area  

        [MaxLength(200)]
        public string ATA_Yellow_Pages { get; set; }            //text area  

        [MaxLength(200)]
        public string ATA_Who_Domain { get; set; }              //text area  

        [MaxLength(200)]
        public string ATA_GoogleSearch { get; set; }            //text area  

        [MaxLength(200)]
        public string ATA_InfrastructureOfCmp { get; set; }     //text area  

        [MaxLength(200)]
        public string ATA_Just_Dial { get; set; }               //text area   

        [MaxLength(200)]
        public string ATA_Neighbor_Check1 { get; set; }         //text area   

        [MaxLength(200)]
        public string ATA_NASCOM_Empanelment { get; set; }  //text area  

        [MaxLength(200)]
        public string ATA_Photograph { get; set; }              //text area  

        [MaxLength(200)]
        public string ATA_Physical_Site { get; set; }           //text area  

        [MaxLength(200)]
        public string ATA_Reg_Company { get; set; }             //text area  

        [MaxLength(200)]
        public string ATA_PO_Courier_Check { get; set; }        //text area    

        [MaxLength(200)]
        public string ATA_Network_Sol { get; set; }  //text area  

        [MaxLength(200)]
        public string ATA_ApprxEmpWorking { get; set; } //text area  

        [MaxLength(200)]
        public string ATA_CID_No { get; set; }          //text area   

        [MaxLength(200)]
        public string ATA_Cmpny_Addr { get; set; }      //text area  

        [MaxLength(200)]
        public string ATA_Decoy_Call { get; set; }      //text area  

        [MaxLength(200)]
        public string ATA_Cmpny_Website { get; set; }   //text area

        public string CheckStatus { get; set; }

        public byte Status { get; set; }
        public int CheckStatusid { get; set; }
        public bool Aging { get; set; }

        #region Employment Closer fields

        public NationalIdentityVerCloserViewModel CloserViewModel { get; set; }

        #endregion
    }

    public class NationalIdentityVerCloserViewModel
    {

        [ScaffoldColumn(false)]
        public int NationalIdentityVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int NationalIdentityRowID { get; set; }

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
        public int CheckStatusId { get; set; }
        public string ShortRemarks { get; set; }
        public List<int> CDispositionRowId { get; set; }
    }
}
