using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQNationalIdentityVer
    {
        public PQNationalIdentityVer()
        {
            NationalIdentityVerRowID = 0;
            NationalIdentityRowID = 0;
            NIC_Cand_Name                           =string.Empty;
            NIC_Any_Other_Cmmnts_NIC                =string.Empty;
            NIC_ID_No                               =string.Empty;
            NIC_Verified_By                         =string.Empty;
            NIC_Date_Issue                          =string.Empty;
            NIC_Expiry_Date                         =string.Empty;
            NIC_Validity_Status                     =string.Empty;
            NIC_Passport_No                         =string.Empty;
            NIC_SSN_NO                              =string.Empty;
            NIC_Pan_No                              =string.Empty;
            NIC_Voter_Id_No                         =string.Empty;
            NIC_Dvng_Liscense_No                    =string.Empty;
            NIC_Aadhar_Card_No                      =string.Empty;
            NIC_Place_Issue                         =string.Empty;
            NIC_Place_Residnce                      =string.Empty;
            NIC_Time_Verf                           =string.Empty;
            NIC_Type_Id_Card                        =string.Empty;
            NIC_Reg_Doc_Type                        =string.Empty;
            NIC_RegNo                               =string.Empty;
            NIC_Name_Field_Executive                =string.Empty;
            NIC_PinCode                             =string.Empty;
            NIC_Relation_Key_Personal               =string.Empty;
            NIC_Verified_Frm                        =string.Empty;
            NIC_Auth_Details_Name                   =string.Empty;
            NIC_Dsg                                 =string.Empty;
            NIC_Eid                                 =string.Empty;
            NIC_MobNo                               =string.Empty;
            NIC_Name_Key_Personnel                  =string.Empty;
            NIC_AliasFname                          =string.Empty;
            NIC_Alias_Lname                         =string.Empty;
            NIC_Country_Iss_Passport                =string.Empty;
            NIC_Id_CardNo                           =string.Empty;
            NIC_Identity_Prov                       =string.Empty;
            NIC_Name_Changed_Effect_Frm              =string.Empty;
            NIC_Nationality                         =string.Empty;
            NIC_PhotoID_Verif                       =string.Empty;
            NIC_Any_Other_Remarks_NIC               =string.Empty;
            NIC_OtherDetails6                       =string.Empty;
            NIC_OtherDetails7                       =string.Empty;
            NIC_OtherDetails8                       =string.Empty;
            NIC_OtherDetails9                       =string.Empty;
            NIC_OtherDetails10                      =string.Empty;
            NIC_OtherDetails11                      =string.Empty;
            NIC_OtherDetails12                      =string.Empty;
            NIC_OtherDetails13                      =string.Empty;
            NIC_OtherDetails14                      =string.Empty;
            NIC_OtherDetails15                      =string.Empty;
            ATA_Reverse_Directory                   =string.Empty;
            ATA_Residcl_Commrcl                     =string.Empty;
            ATA_Stock_Exchnge                       =string.Empty;
            ATA_Telphne_Directry_Srch               =string.Empty;
            ATA_Yellow_Pages                        =string.Empty;
            ATA_Who_Domain                          =string.Empty;
            ATA_GoogleSearch                        =string.Empty;
            ATA_InfrastructureOfCmp                 =string.Empty;
            ATA_Just_Dial                           =string.Empty;
            ATA_Neighbor_Check1                     =string.Empty;
            ATA_NASCOM_Empanelment                  =string.Empty;
            ATA_Photograph                          =string.Empty;
            ATA_Physical_Site                       =string.Empty;
            ATA_Reg_Company                         =string.Empty;
            ATA_PO_Courier_Check                    =string.Empty;
            ATA_Network_Sol                         =string.Empty;
            ATA_ApprxEmpWorking                     =string.Empty;
            ATA_CID_No                              =string.Empty;
            ATA_Cmpny_Addr                          =string.Empty;
            ATA_Decoy_Call                          =string.Empty;
            ATA_Cmpny_Website                       =string.Empty;
            
            TypeRevert = string.Empty;
            CheckStatus = string.Empty;
            Severity = string.Empty;
            ColorName = string.Empty;
            ReportComments = string.Empty;
            Status = 1;

            VerifiedBy = 0;
            VerifierDesignation = string.Empty;
            VerifierContactNo = string.Empty;
            VerifierMobileNo = string.Empty;
            VerifierEmailId = string.Empty;
        }

        public int NationalIdentityVerRowID { get; set; }

        public int NationalIdentityRowID { get; set; }
        public virtual PQNationalIdentity PQNationalIdentity { get; set; }

        public string        NIC_Cand_Name                 { get; set; }                       //text filed
        public string        NIC_Any_Other_Cmmnts_NIC      { get; set; }            // text Area
        public DateTime?     NIC_DOB1                      { get; set; }                         // date
        public string        NIC_ID_No                     { get; set; }                           // text filed
        public string        NIC_Verified_By               { get; set; }                     // text filed
        public string        NIC_Date_Issue                { get; set; }                      // text filed
        public string        NIC_Expiry_Date               { get; set; }                     // text filed
        public string        NIC_Validity_Status           { get; set; }                 // Drop down -Valid / Expired / Suspended / Revoked / Others 
        public string        NIC_Passport_No               { get; set; }                 //text filed
        public string        NIC_SSN_NO                    { get; set; }                      //text filed
        public string        NIC_Pan_No                    { get; set; }                      //text filed
        public string        NIC_Voter_Id_No               { get; set; }                 //text filed
        public string        NIC_Dvng_Liscense_No          { get; set; }            //text filed
        public string        NIC_Aadhar_Card_No            { get; set; }              //text filed
        public string        NIC_Place_Issue               { get; set; }                 //text filed
        public string        NIC_Place_Residnce            { get; set; }              //text filed
        public string        NIC_Time_Verf                 { get; set; }                   //text filed
        public string        NIC_Type_Id_Card              { get; set; }                //text filedpro 
        public string        NIC_Reg_Doc_Type              { get; set; }                //text filed
        public string        NIC_RegNo                     { get; set; }                       //text filed
        public string        NIC_Name_Field_Executive      { get; set; }        //text filed
        public string        NIC_PinCode                   { get; set; }                     //text filed
        public string        NIC_Relation_Key_Personal     { get; set; }       //text filed
        public string        NIC_Verified_Frm              { get; set; }                //text filed
        public string        NIC_Auth_Details_Name         { get; set; }           //text filed
        public string        NIC_Dsg                       { get; set; }                         //text filed
        public string        NIC_Eid                       { get; set; }                         //text filed
        public string        NIC_MobNo                     { get; set; }                       //text filed
        public string        NIC_Name_Key_Personnel        { get; set; }          //text filed
        public string        NIC_AliasFname                { get; set; }                  //text filed
        public string        NIC_Alias_Lname               { get; set; }                 //text filed
        public string        NIC_Country_Iss_Passport      { get; set; }        //text filed
        public DateTime?     NIC_Date_Verif                { get; set; }                  //date
        public string        NIC_Id_CardNo                 { get; set; }//text filed
        public string        NIC_Identity_Prov             { get; set; }// drop down 
        public string        NIC_Name_Changed_Effect_Frm   { get; set; }// text filed 
        public string        NIC_Nationality               { get; set; }// text filed 
        public string        NIC_PhotoID_Verif             { get; set; }// Verified / Not verified / Not possible / Other 
        public string        NIC_Any_Other_Remarks_NIC     { get; set; }// text filed
        public string        NIC_OtherDetails6             { get; set; }   //text area
        public string        NIC_OtherDetails7             { get; set; }   //text area
        public string        NIC_OtherDetails8             { get; set; }   //text area
        public string        NIC_OtherDetails9             { get; set; }   //text area
        public string        NIC_OtherDetails10            { get; set; }  //text area
        public string        NIC_OtherDetails11            { get; set; }  //text area
        public string        NIC_OtherDetails12            { get; set; }  //text area
        public string        NIC_OtherDetails13            { get; set; }  //text area
        public string        NIC_OtherDetails14            { get; set; }  //text area
        public string        NIC_OtherDetails15            { get; set; }  //text area
        public string        ATA_Reverse_Directory         { get; set; }   //text area
        public string        ATA_Residcl_Commrcl           { get; set; }         //text area
        public string        ATA_Stock_Exchnge             { get; set; }           //text area
        public string        ATA_Telphne_Directry_Srch     { get; set; }   //text area
        public string        ATA_Yellow_Pages              { get; set; }            //text area
        public string        ATA_Who_Domain                { get; set; }              //text area
        public string        ATA_GoogleSearch              { get; set; }            //text area
        public string        ATA_InfrastructureOfCmp       { get; set; }     //text area
        public string        ATA_Just_Dial                 { get; set; }               //text area
        public string        ATA_Neighbor_Check1           { get; set; }         //text area
        public string        ATA_NASCOM_Empanelment        { get; set; }      //text area
        public string        ATA_Photograph                { get; set; }              //text area
        public string        ATA_Physical_Site             { get; set; }           //text area
        public string        ATA_Reg_Company               { get; set; }             //text area
        public string        ATA_PO_Courier_Check          { get; set; }        //text area
        public string        ATA_Network_Sol               { get; set; }             //text area
        public string        ATA_ApprxEmpWorking           { get; set; }         //text area
        public string        ATA_CID_No                    { get; set; }                  //text area
        public string        ATA_Cmpny_Addr                { get; set; }              //text area
        public string        ATA_Decoy_Call                { get; set; }              //text area
        public string        ATA_Cmpny_Website             { get; set; }           //text area
        public string        CheckStatus                   { get; set; }
        public byte          Status                        { get; set; }

        public string TypeRevert { get; set; }       
        public string Severity { get; set; }
        public string ColorName { get; set; }
        public string ReportComments { get; set; }
      

        public short VerifiedBy { get; set; }
        public string VerifierDesignation { get; set; }
        public string VerifierContactNo { get; set; }
        public string VerifierMobileNo { get; set; }
        public string VerifierEmailId { get; set; }
        public DateTime? VerifiedDate { get; set; }
    }
}
