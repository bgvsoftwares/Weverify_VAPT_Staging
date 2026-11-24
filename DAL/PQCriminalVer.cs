using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQCriminalVer
    {
        public PQCriminalVer()
        {
            CriminalVerRowId = 0;
            CRV_Cand_Name = String.Empty;
            CRV_Father_Name = String.Empty;
            CRV_First_Name = String.Empty;
            CRV_Last_Name = String.Empty;
            CRV_Nationality = String.Empty;
            CRV_Name_Chngd_Effect_Frm = String.Empty;
            CRV_Nationality_On_Passport = String.Empty;
            CRV_Country_of_Issue = String.Empty;
            CRV_Addr_Type = String.Empty;
            CRV_Current_Addr = String.Empty;
            CRV_Prev_Addr1 = String.Empty;
            CRV_Prev_Addr2 = String.Empty;
            CRV_Prev_Addr3 = String.Empty;
            CRV_Prev_Addr4 = String.Empty;
            CRV_Permanent_Addr = String.Empty;
            CRV_Gender = String.Empty;
            CRV_Identity_No = String.Empty;
            CRV_Identity_Prov = String.Empty;
            CRV_Doc_Received = String.Empty;
            CRV_Dur_Yr_Covered = String.Empty;
            CRV_Pol_Station_Name = String.Empty;
            CRV_Pol_Authority_Name = String.Empty;
            CRV_Pol_Authority_Desg = String.Empty;
            CRV_Criminal_Verf = String.Empty;
            CRV_Type_of_Revert = String.Empty;
            CRV_Verfier_Cmmnts = String.Empty;
            CRV_St_Pol_Most_Wanted = String.Empty;
            CRV_CBI_Most_Wanted = String.Empty;
            CRV_Interpol_Most_Wanted = String.Empty;
            CRV_Allied_Terrorist_Dataset = String.Empty;
            CRV_Crimes_Dishonest_Dataset = String.Empty;
            CRV_Global_Corruption_Dataset = String.Empty;
            CRV_Global_Financ_Rgltry_Dataset = String.Empty;
            CRV_Global_Fraud_Dataset = String.Empty;
            CRV_Global_Law_Enforce_Dataset = String.Empty;
            CRV_Sex_Crime_Dataset = String.Empty;
            CRV_Violent_Crime_Dataset = String.Empty;
            CRV_Global_Terrorist_Risk = String.Empty;
            CRV_Global_Money_Laund = String.Empty;
            CRV_Civil_Court_Action = String.Empty;
            CRV_District_Court = String.Empty;
            CRV_State_Court = String.Empty;
            CRV_Civil_Court = String.Empty;
            CRV_Special_Court = String.Empty;
            CRV_Session_Court = String.Empty;
            CRV_Tribunals = String.Empty;
            CRV_Any_Other_Cmmnts_CRV = String.Empty;
            CRV_District_Civil_Court = String.Empty;
            CRV_St_High_Court = String.Empty;
            CRV_Supreme_Court = String.Empty;
            CRV_Criminal_Appeal = String.Empty;
            CRV_Private_Complain_Rpt = String.Empty;
            CRV_Others6 = String.Empty;
            CRV_Others7 = String.Empty;
            CRV_Others8 = String.Empty;
            CRV_Others9 = String.Empty;
            CRV_Others10 = String.Empty;
            CRV_Others11 = String.Empty;
            CRV_Others12 = String.Empty;
            CRV_Others13 = String.Empty;
            CRV_Others14 = String.Empty;
            CRV_Others15 = String.Empty;
            ATA_Reverse_Directory = String.Empty;
            ATA_Residcl_Commrcl = String.Empty;
            ATA_Stock_Exchnge = String.Empty;
            ATA_Telphne_Directry_Srch = String.Empty;
            ATA_Yellow_Pages = String.Empty;
            ATA_Who_Domain = String.Empty;
            ATA_GoogleSearch = String.Empty;
            ATA_InfrastructureOfCmp = String.Empty;
            ATA_Just_Dial = String.Empty;
            ATA_Neighbor_Check1 = String.Empty;
            ATA_NASCOM_Empanelment = String.Empty;
            ATA_Photograph = String.Empty;
            ATA_Physical_Site = String.Empty;
            ATA_Reg_Company = String.Empty;
            ATA_PO_Courier_Check = String.Empty;
            ATA_Network_Sol = String.Empty;
            ATA_ApprxEmpWorking = String.Empty;
            ATA_CID_No = String.Empty;
            ATA_Cmpny_Addr = String.Empty;
            ATA_Decoy_Call = String.Empty;
            ATA_Cmpny_Website = String.Empty;

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

        public int       CriminalVerRowId                { get; set; } 
        public int       CriminalRowID                   { get; set; }
        public virtual PQCriminal PQCriminal                { get; set; }

        public string    CRV_Cand_Name                   { get; set; }   // Text Box
        public DateTime?  CRV_DOB                         { get; set; }                   //Date Text Field
        public string    CRV_Father_Name                 { get; set; }             // Text Box
        public string    CRV_First_Name                  { get; set; }              // Text Box
        public string    CRV_Last_Name                   { get; set; }               // Text Box
        public string    CRV_Nationality                 { get; set; }             // Text Box
        public string    CRV_Name_Chngd_Effect_Frm       { get; set; }   // date mm/yy
        public string    CRV_Nationality_On_Passport     { get; set; } // Text Box
        public string    CRV_Country_of_Issue            { get; set; }        // Text Box
        public string    CRV_Addr_Type                   { get; set; }               // drop down 
        public string    CRV_Current_Addr                { get; set; }            //Text Area
        public string    CRV_Prev_Addr1                  { get; set; }              //Text Area
        public string    CRV_Prev_Addr2                  { get; set; }              //Text Area
        public string    CRV_Prev_Addr3                  { get; set; }              //Text Area
        public string    CRV_Prev_Addr4                  { get; set; }              //Text Area
        public string    CRV_Permanent_Addr              { get; set; }          //Text Area
        public string    CRV_Gender                      { get; set; }                  //drop  down
        public string    CRV_Identity_No                 { get; set; }             // text box
        public string    CRV_Identity_Prov               { get; set; }           //List Box	Multiple selection 
        public string    CRV_Doc_Received                { get; set; }            //List Box	Multiple selection 
        public string    CRV_Dur_Yr_Covered              { get; set; }          // Date Range
        public string    CRV_Pol_Station_Name            { get; set; }        //Police Station Masters 
        public string    CRV_Pol_Authority_Name          { get; set; }      // Text Field
        public string    CRV_Pol_Authority_Desg          { get; set; }      //Text Field
        public string    CRV_Criminal_Verf               { get; set; }           // drop down
        public string    CRV_Type_of_Revert              { get; set; }          // drop down    Written / Verbal / Online / Other
        public DateTime? CRV_Date_Addr_Verif             { get; set; }   //To be mapped from address verification check
        public DateTime? CRV_Date_Verf                   { get; set; }// text Field
        public DateTime? CRV_Time_Verf                   { get; set; }// text Field 
        public string    CRV_Verfier_Cmmnts              { get; set; }//Text Area
        public string    CRV_St_Pol_Most_Wanted          { get; set; }// Drop Down 
        public string    CRV_CBI_Most_Wanted             { get; set; }// Drop Down 
        public string    CRV_Interpol_Most_Wanted        { get; set; }// Drop Down 
        public string    CRV_Allied_Terrorist_Dataset    { get; set; }// Drop Down 
        public string    CRV_Crimes_Dishonest_Dataset    { get; set; }// Drop Down 
        public string    CRV_Global_Corruption_Dataset   { get; set; }// Drop Down 
        public string    CRV_Global_Financ_Rgltry_Dataset{ get; set; }// Drop Down 
        public string    CRV_Global_Fraud_Dataset        { get; set; }// Drop Down 
        public string    CRV_Global_Law_Enforce_Dataset  { get; set; }// Drop Down 
        public string    CRV_Sex_Crime_Dataset           { get; set; }// Drop Down 
        public string    CRV_Violent_Crime_Dataset       { get; set; }// Drop Down 
        public string    CRV_Global_Terrorist_Risk       { get; set; }// Drop Down 
        public string    CRV_Global_Money_Laund          { get; set; }// Drop Down 
        public string    CRV_Civil_Court_Action          { get; set; }// Drop Down 
        public string    CRV_District_Court              { get; set; }// Drop Down 
        public string    CRV_State_Court                 { get; set; }// Drop Down 
        public string    CRV_Civil_Court                 { get; set; }// Drop Down 
        public string    CRV_Special_Court               { get; set; }// Drop Down 
        public string    CRV_Session_Court               { get; set; }// Drop Down 
        public string    CRV_Tribunals                   { get; set; }// Drop Down 
        public string    CRV_Any_Other_Cmmnts_CRV        { get; set; }// Drop Down 
        public string    CRV_Magistrate_Court            { get; set; }// Drop Down
        public string    CRV_District_Civil_Court        { get; set; }// Drop Down 
        public string    CRV_St_High_Court               { get; set; }// Drop Down 
        public string    CRV_Supreme_Court               { get; set; }// Drop Down 
        public string    CRV_Criminal_Appeal             { get; set; }// Drop Down 
        public string    CRV_Private_Complain_Rpt        { get; set; }// Drop Down 
        public string    CRV_Others6                     { get; set; }//Text Area
        public string    CRV_Others7                     { get; set; }//Text Area
        public string    CRV_Others8                     { get; set; }//Text Area
        public string    CRV_Others9                     { get; set; }//Text Area
        public string    CRV_Others10                    { get; set; }//Text Area
        public string    CRV_Others11                    { get; set; }//Text Area
        public string    CRV_Others12                    { get; set; }//Text Area
        public string    CRV_Others13                    { get; set; }//Text Area
        public string    CRV_Others14                    { get; set; }//Text Area
        public string    CRV_Others15                    { get; set; }//Text Area
        public string    ATA_Reverse_Directory           { get; set; }//Text Area
        public string    ATA_Residcl_Commrcl             { get; set; }//Text Area
        public string    ATA_Stock_Exchnge               { get; set; }//Text Area
        public string    ATA_Telphne_Directry_Srch       { get; set; }//Text Area
        public string    ATA_Yellow_Pages                { get; set; }//Text Area
        public string    ATA_Who_Domain                  { get; set; }//Text Area
        public string    ATA_GoogleSearch                { get; set; }//Text Area
        public string    ATA_InfrastructureOfCmp         { get; set; }//Text Area
        public string    ATA_Just_Dial                   { get; set; }//Text Area
        public string    ATA_Neighbor_Check1             { get; set; }//Text Area
        public string    ATA_NASCOM_Empanelment          { get; set; }//Text Area
        public string    ATA_Photograph                  { get; set; }//Text Area
        public string    ATA_Physical_Site               { get; set; }//Text Area
        public string    ATA_Reg_Company                 { get; set; }//Text Area
        public string    ATA_PO_Courier_Check            { get; set; }//Text Area
        public string    ATA_Network_Sol                 { get; set; }//Text Area
        public string    ATA_ApprxEmpWorking             { get; set; }//Text Area
        public string    ATA_CID_No                      { get; set; }//Text Area
        public string    ATA_Cmpny_Addr                  { get; set; }//Text Area
        public string    ATA_Decoy_Call                  { get; set; }//Text Area
        public string    ATA_Cmpny_Website               { get; set; }//Text Area
                         
        public string    TypeRevert                      { get; set; }
        public string    CheckStatus                     { get; set; }
        public string    Severity                        { get; set; }
        public string    ColorName                       { get; set; }
        public string    ReportComments                  { get; set; }
        public byte      Status                          { get; set; }
                         
        public short     VerifiedBy                      { get; set; }
        public string    VerifierDesignation             { get; set; }
        public string    VerifierContactNo               { get; set; }
        public string    VerifierMobileNo                { get; set; }
        public string    VerifierEmailId                 { get; set; }
        public DateTime? VerifiedDate                    { get; set; }
    }
}
