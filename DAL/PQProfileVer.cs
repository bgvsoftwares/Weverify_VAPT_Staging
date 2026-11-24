using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQProfileVer
    {
        public PQProfileVer()
        {
            PQProfileVerRowID = 0;
            PQProfileRowID = 0;
            PC_Cand_Name                         =string.Empty;
            PC_Address                           =string.Empty;
            PC_Age                               =string.Empty;
            PC_Agnt_Name                         =string.Empty;
            PC_Applcnt_Age_DOB                   =string.Empty;
            PC_Applcnt_Health_Cond               =string.Empty;
            PC_Applied_Policy                    =string.Empty;
            PC_Cmpny_Name                        =string.Empty;
            PC_Criminal_Back                     =string.Empty;            
            PC_Dsg                               =string.Empty;
            PC_Edu                               =string.Empty;
            PC_Employer                          =string.Empty;
            PC_Existence_Addr                    =string.Empty;
            PC_Fmly_History                      =string.Empty;
            PC_Habits                            =string.Empty;
            PC_Income                            =string.Empty;
            PC_InsurancePolicy_Taken             =string.Empty;
            PC_Investigator                      =string.Empty;
            PC_Locality                          =string.Empty;
            PC_Mode                              =string.Empty;
            PC_Modern_Appliance_Seen             =string.Empty;
            PC_Name_Field_Executive              =string.Empty;
            PC_Name_Met_Per                      =string.Empty;
            PC_Neigh_Check                       =string.Empty;
            PC_Neigh_Check2                      =string.Empty;
            PC_Neigh_Check3                      =string.Empty;
            PC_Neigh_Check4                      =string.Empty;
            PC_New_Addr_Obtain                   =string.Empty;
            PC_No_Yrs_Current_Residence          =string.Empty;
            PC_Occupation                        =string.Empty;
            PC_Occupation_Addr                   =string.Empty;
            PC_Office_Shop_Appearance            =string.Empty;
            PC_Policy_No                         =string.Empty;
            PC_Premium                           =string.Empty;
            PC_Proposer_Name                     =string.Empty;
            PC_Reason_Check                      =string.Empty;
            PC_Received_Policy                   =string.Empty;
            PC_Relation_With_Applcnt             =string.Empty;
            PC_Remarks_PC                        =string.Empty;
            PC_Residence_Area                    =string.Empty;
            PC_Residence_Status                  =string.Empty;
            PC_Residence_Type                    =string.Empty;
            PC_Self_Emplyd_Remarks               =string.Empty;
            PC_Standard_Living                   =string.Empty;
            PC_Status                            =string.Empty;
            PC_Sum_Assured                       =string.Empty;
            PC_Time_Verf                         =string.Empty;
            PC_Vech_Owned                        =string.Empty;
            PC_Verf_Grid                         =string.Empty;
            PC_Yrly_Premium                      =string.Empty;
            PC_OtherDetails6                     =string.Empty;
            PC_OtherDetails7                     =string.Empty;
            PC_OtherDetails8                     =string.Empty;
            PC_OtherDetails9                     =string.Empty;
            PC_OtherDetails10                    =string.Empty;
            PC_OtherDetails11                    =string.Empty;
            PC_OtherDetails12                    =string.Empty;
            PC_OtherDetails13                    =string.Empty;
            PC_OtherDetails14                    =string.Empty;
            PC_OtherDetails15                    =string.Empty;
            ATA_Reverse_Directory                =string.Empty;
            ATA_Residcl_Commrcl                  =string.Empty;
            ATA_Stock_Exchnge                    =string.Empty;
            ATA_Telphne_Directry_Srch            =string.Empty;
            ATA_Yellow_Pages                     =string.Empty;
            ATA_Who_Domain                       =string.Empty;
            ATA_GoogleSearch                     =string.Empty;
            ATA_InfrastructureOfCmp              =string.Empty;
            ATA_Just_Dial                        =string.Empty;
            ATA_Neighbor_Check1                  =string.Empty;
            ATA_NASCOM_Empanelment               =string.Empty;
            ATA_Photograph                       =string.Empty;
            ATA_Physical_Site                    =string.Empty;
            ATA_Reg_Company                      =string.Empty;
            ATA_PO_Courier_Check                 =string.Empty;
            ATA_Network_Sol                      =string.Empty;
            ATA_ApprxEmpWorking                  =string.Empty;
            ATA_CID_No                           =string.Empty;
            ATA_Cmpny_Addr                       =string.Empty;
            ATA_Decoy_Call                       =string.Empty;
            ATA_Cmpny_Website                    =string.Empty;
            PC_OtherProof                        =string.Empty;


            TypeRevert = string.Empty;
            CheckStatus = string.Empty;
            CaseStatus = string.Empty;
            ColorName = string.Empty;
            Remarks = string.Empty;
            Status = 0;

            VerifiedBy = 0;
            VerifierDesignation = string.Empty;
            VerifierContactNo = string.Empty;
            VerifierMobileNo = string.Empty;
            VerifierEmailId = string.Empty;
        }
public int PQProfileVerRowID { get; set; }
public int PQProfileRowID { get; set; }
public virtual PQProfile PQProfile { get; set; }
public string PC_Cand_Name                  {get;set;}//text filed
public string PC_Address                    {get;set;}//Text Area
public string PC_Age                        {get;set;}//Text Filed
public string PC_Agnt_Name                  {get;set;}//Text Filed
public string PC_Applcnt_Age_DOB            {get;set;}//Text filed
public string PC_Applcnt_Health_Cond        {get;set;}//Text Area
public string PC_Applied_Policy             {get;set;}//Drop down -Yes / No/ Not aware / Other 
public string PC_Cmpny_Name                 {get;set;}//Text filed 
public string PC_Criminal_Back              {get;set;}//Text Area
public DateTime? PC_Date_Verf                  {get;set;}//Date
public string PC_Dsg                        {get;set;}//Drop down -Mapped with designation masters 
public string PC_Edu                        {get;set;}//Drop down -Mapped with education masters 
public string PC_Employer                   {get;set;}//Drop down -Mapped with employer masters 
public string PC_Existence_Addr             {get;set;}// Drop Down -Exist / Doesnot exist / Untraced / Others 
public string PC_Fmly_History               {get;set;}//text Area 
public string PC_Habits                     {get;set;}//text Area 
public string PC_Income                     {get;set;}//text filed 
public string PC_InsurancePolicy_Taken      {get;set;}//text filed 
public string PC_Investigator               {get;set;}//text filed 
public string PC_Locality                   {get;set;}//text filed 
public string PC_Mode                       {get;set;}// drop down -Verbal / Written / Others 
public string PC_Modern_Appliance_Seen      {get;set;}// drop down -Yes / No/ Not aware / Other 
public string PC_Name_Field_Executive       {get;set;}//Text filed
public string PC_Name_Met_Per               {get;set;}//Text filed
public string PC_Neigh_Check                {get;set;}//Text filed
public string PC_Neigh_Check2               {get;set;}//Text filed
public string PC_Neigh_Check3               {get;set;}//Text filed
public string PC_Neigh_Check4               {get;set;}//Text filed
public string PC_New_Addr_Obtain            {get;set;}//Text Area
public string PC_No_Yrs_Current_Residence   {get;set;}//Text Area
public string PC_Occupation                 {get;set;}//Text Area
public string PC_Occupation_Addr            {get;set;}//Text Area
public string PC_Office_Shop_Appearance     {get;set;}//Text Area
public string PC_Policy_No                  {get;set;}//Text Filed
public string PC_Premium                    {get;set;}//Text Filed
public string PC_Proposer_Name              {get;set;}//Text Filed
public string PC_Reason_Check               {get;set;}//Text Filed
public string PC_Received_Policy            {get;set;}//Text Filed
public string PC_Relation_With_Applcnt      {get;set;}//Text Filed
public string PC_Remarks_PC                 {get;set;}//Text Filed
public string PC_Residence_Area             {get;set;}//Text Filed
public string PC_Residence_Status           {get;set;}//Text Filed
public string PC_Residence_Type             {get;set;}//Text Filed
public string PC_Self_Emplyd_Remarks        {get;set;}//Text Filed
public string PC_Standard_Living            {get;set;}//Text Filed
public string PC_Status                     {get;set;}//Text Filed
public string PC_Sum_Assured                {get;set;}//Text Filed
public string PC_Time_Verf                  {get;set;}//Text Filed
public string PC_Vech_Owned                 {get;set;}//Text Filed
public string PC_Verf_Grid                  {get;set;}//Text Filed
public string PC_Yrly_Premium               {get;set;}//text Area
public string PC_OtherDetails6              {get;set;}//text Area
public string PC_OtherDetails7              {get;set;}//text Area
public string PC_OtherDetails8              {get;set;}//text Area
public string PC_OtherDetails9              {get;set;}//text Area
public string PC_OtherDetails10             {get;set;}//text Area
public string PC_OtherDetails11             {get;set;}//text Area
public string PC_OtherDetails12             {get;set;}//text Area
public string PC_OtherDetails13             {get;set;}//text Area
public string PC_OtherDetails14             {get;set;}//text Area
public string PC_OtherDetails15             {get;set;}//text Area
public string ATA_Reverse_Directory         {get;set;}//text Area
public string ATA_Residcl_Commrcl           {get;set;}//Text Area
public string ATA_Stock_Exchnge             {get;set;}//Text Area
public string ATA_Telphne_Directry_Srch     {get;set;}//Text Area
public string ATA_Yellow_Pages              {get;set;}//Text Area
public string ATA_Who_Domain                {get;set;}//Text Area
public string ATA_GoogleSearch              {get;set;}//Text Area
public string ATA_InfrastructureOfCmp       {get;set;}//Text Area
public string ATA_Just_Dial                 {get;set;}//Text Area
public string ATA_Neighbor_Check1           {get;set;}//Text Area
public string ATA_NASCOM_Empanelment        {get;set;}//Text Area
public string ATA_Photograph                {get;set;}//Text Area
public string ATA_Physical_Site             {get;set;}//Text Area
public string ATA_Reg_Company               {get;set;}//Text Area
public string ATA_PO_Courier_Check          {get;set;}//Text Area
public string ATA_Network_Sol               {get;set;}//Text Area
public string ATA_ApprxEmpWorking           {get;set;}//Text Area
public string ATA_CID_No                    {get;set;}//Text filed
public string ATA_Cmpny_Addr                {get;set;}//Text Area
public string ATA_Decoy_Call                {get;set;}//Text Area
public string ATA_Cmpny_Website             {get;set;}//Text filed
public string PC_OtherProof                 {get;set;}//Text Area

        public string TypeRevert { get; set; }
        public string CheckStatus { get; set; }
        public string CaseStatus { get; set; }
        public string ColorName { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }

        public short VerifiedBy { get; set; }
        public string VerifierDesignation { get; set; }
        public string VerifierContactNo { get; set; }
        public string VerifierMobileNo { get; set; }
        public string VerifierEmailId { get; set; }
        public DateTime? VerifiedDate { get; set; }
    }
}
