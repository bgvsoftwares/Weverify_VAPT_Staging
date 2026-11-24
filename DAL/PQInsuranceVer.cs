using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQInsuranceVer
    {
        public PQInsuranceVer()
        {
            InsuranceVerRowID               =0;
            IV_Standard_Living              =string.Empty;
            IV_State                        =string.Empty;
            IV_Self_Emplyd_Remarks          =string.Empty;
            IV_Remarks_IV                   =string.Empty;
            IV_Relation_Applcnt             =string.Empty;
            IV_Report_Shared_date           =string.Empty;
            IV_Resdnce_Area                 =string.Empty;
            IV_Resdnce_Status               =string.Empty;
            IV_Resdnce_Type                 =string.Empty;
            IV_Vech_Owned                   =string.Empty;
            IV_Verif_Grid                   =string.Empty;
            IV_Yrly_Premium                 =string.Empty;
            IV_Sum_Assured                  =string.Empty;
            IV_Time_Verification            =string.Empty;
            IV_Status                       =string.Empty;
            IV_Medical_Test_Insurance_App   =string.Empty;
            IV_ID_CardNo                    =string.Empty;
            IV_ID_Card_Seen                 =string.Empty;
            IV_ID_Proof_Relation_Life_Ass   =string.Empty;
            IV_Applcnt_Other_Loc_frm_Addr   =string.Empty;
            IV_Income                       =string.Empty;
            IV_InsurancePolicy_Taken        =string.Empty;
            IV_Insured_Name                 =string.Empty;
            IV_Investigator                 =string.Empty;
            IV_Locality                     =string.Empty;
            IV_Mode_Of_Verif                =string.Empty;
            IV_Modern_Appliance_Seen        =string.Empty;
            IV_Name_Field_Executive         =string.Empty;
            IV_Name_Met_Person              =string.Empty;
            IV_Physical_Disability          =string.Empty;
            IV_Policy_No                    =string.Empty;
            IV_Proposer_Name                =string.Empty;
            IV_Pincode                      =string.Empty;
            IV_Premium                      =string.Empty;
            IV_Reason_Check                 =string.Empty;
            IV_Received_Policy              =string.Empty;
            IV_Near_Landmark                =string.Empty;
            IV_Neighbor_Check               =string.Empty;
            IV_New_Addr_Obtain              =string.Empty;
            IV_No_Yrs_Current_Residence     =string.Empty;
            IV_Office_Addr                  =string.Empty;
            IV_Office_Locality              =string.Empty;
            IV_Office_Appearance            =string.Empty;
            IV_Original_Doc_Seen            =string.Empty;
            IV_Applcnt_DOB_Age              =string.Empty;
            IV_Applcnt_Health_Cond          =string.Empty;
            IV_Applied_Policy               =string.Empty;
            IV_Action_Req                   =string.Empty;
            IV_Address                      =string.Empty;
            IV_Age                          =string.Empty;
            IV_Agent_Name                   =string.Empty;
            IV_Check_Type                   =string.Empty;
            ATA_Reverse_Directory           =string.Empty;
            ATA_Residcl_Commrcl             =string.Empty;
            ATA_Stock_Exchnge               =string.Empty;
            ATA_Telphne_Directry_Srch       =string.Empty;
            ATA_Yellow_Pages                =string.Empty;
            ATA_Who_Domain                  =string.Empty;
            ATA_GoogleSearch                =string.Empty;
            ATA_InfrastructureOfCmp         =string.Empty;
            ATA_Just_Dial                   =string.Empty;
            ATA_Neighbor_Check1             =string.Empty;
            ATA_NASCOM_Empanelment          =string.Empty;
            ATA_Photograph                  =string.Empty;
            ATA_Physical_Site               =string.Empty;
            ATA_Reg_Company                 =string.Empty;
            ATA_PO_Courier_Check            =string.Empty;
            ATA_Network_Sol                 =string.Empty;
            ATA_ApprxEmpWorking             =string.Empty;
            ATA_CID_No                      =string.Empty;
            ATA_Cmpny_Addr                  =string.Empty;
            ATA_Decoy_Call                  =string.Empty;
            ATA_Cmpny_Website               =string.Empty;
            IV_Others                       =string.Empty;
            IV_Others2                      =string.Empty;
            IV_Others3                      =string.Empty;
            IV_Others4                      =string.Empty;
            IV_Others5                      =string.Empty;
            IV_OtherProof                   = string.Empty;

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

        public int    InsuranceVerRowID                     {get;set;}
        public int InsuranceRowID { get; set; }
        public virtual PQInsurance PQInsurance { get; set; }
        public string IV_Standard_Living                    {get;set;}//text Area
        public string IV_State                              {get;set;}//text Area
        public string IV_Self_Emplyd_Remarks                {get;set;}//text Area
        public string IV_Remarks_IV                         {get;set;}//text Area
        public string IV_Relation_Applcnt                   {get;set;}//text Area
        public string IV_Report_Shared_date                 {get;set;}//text Area
        public string IV_Resdnce_Area                       {get;set;}//text Area
        public string IV_Resdnce_Status                     {get;set;}//text Area
        public string IV_Resdnce_Type                       {get;set;}//text Area
        public string IV_Vech_Owned                         {get;set;}//text Area
        public string IV_Verif_Grid                         {get;set;}//text Area
        public string IV_Yrly_Premium                       {get;set;}//text Area
        public string IV_Sum_Assured                        {get;set;}//text Area
        public string IV_Time_Verification                  {get;set;}//text Area
        public string IV_Status                             {get;set;}//text Area
        public string IV_Medical_Test_Insurance_App         {get;set;}//text Area
        public string IV_ID_CardNo                          {get;set;}//text Area
        public string IV_ID_Card_Seen                       {get;set;}//text Area
        public string IV_ID_Proof_Relation_Life_Ass         {get;set;}//text Area
        public string IV_Applcnt_Other_Loc_frm_Addr         {get;set;}//text Area
        public string IV_Income                             {get;set;}//text Area
        public string IV_InsurancePolicy_Taken              {get;set;}//text Area
        public string IV_Insured_Name                       {get;set;}//text Area
        public string IV_Investigator                       {get;set;}//text Area
        public string IV_Locality                           {get;set;}//text Area
        public string IV_Mode_Of_Verif                      {get;set;}//text Area
        public string IV_Modern_Appliance_Seen              {get;set;}//text Area
        public string IV_Name_Field_Executive               {get;set;}//text Area
        public string IV_Name_Met_Person                    {get;set;}//text Area
        public string IV_Physical_Disability                {get;set;}//text Area
        public string IV_Policy_No                          {get;set;}//text Area
        public string IV_Proposer_Name                      {get;set;}//text Area
        public string IV_Pincode                            {get;set;}//text Area
        public string IV_Premium                            {get;set;}//text Area
        public string IV_Reason_Check                       {get;set;}//text Area
        public string IV_Received_Policy                    {get;set;}//text Area
        public string IV_Near_Landmark                      {get;set;}//text Area
        public string IV_Neighbor_Check                     {get;set;}//text Area
        public string IV_New_Addr_Obtain                    {get;set;}//text Area
        public string IV_No_Yrs_Current_Residence           {get;set;}//text Area
        public string IV_Office_Addr                        {get;set;}//text Area
        public string IV_Office_Locality                    {get;set;}//text Area
        public string IV_Office_Appearance                  {get;set;}//text Area
        public string IV_Original_Doc_Seen                  {get;set;}//text Area
        public string IV_Applcnt_DOB_Age                    {get;set;}//text Area
        public string IV_Applcnt_Health_Cond                {get;set;}//text Area
        public string IV_Applied_Policy                     {get;set;}//text Area
        public string IV_Action_Req                         {get;set;}//text Area
        public string IV_Address                            {get;set;}//text Area
        public string IV_Age                                {get;set;}//text Area
        public string IV_Agent_Name                         {get;set;}//text Area
        public string IV_Check_Type                         {get;set;}//text Area
        public string ATA_Reverse_Directory                 {get;set;}//text Area
        public string ATA_Residcl_Commrcl                   {get;set;}//text Area
        public string ATA_Stock_Exchnge                     {get;set;}//text Area
        public string ATA_Telphne_Directry_Srch             {get;set;}//text Area
        public string ATA_Yellow_Pages                      {get;set;}//text Area
        public string ATA_Who_Domain                        {get;set;}//text Area
        public string ATA_GoogleSearch                      {get;set;}//text Area
        public string ATA_InfrastructureOfCmp               {get;set;}//text Area
        public string ATA_Just_Dial                         {get;set;}//text Area
        public string ATA_Neighbor_Check1                   {get;set;}//text Area
        public string ATA_NASCOM_Empanelment                {get;set;}//text Area
        public string ATA_Photograph                        {get;set;}//text Area
        public string ATA_Physical_Site                     {get;set;}//text Area
        public string ATA_Reg_Company                       {get;set;}//text Area
        public string ATA_PO_Courier_Check                  {get;set;}//text Area
        public string ATA_Network_Sol                       {get;set;}//text Area
        public string ATA_ApprxEmpWorking                   {get;set;}//text Area
        public string ATA_CID_No                            {get;set;}//text Filed
        public string ATA_Cmpny_Addr                        {get;set;}//text Filed
        public string ATA_Decoy_Call                        {get;set;}//text Area
        public string ATA_Cmpny_Website                     {get;set;}//text Area
        public string IV_Others                             {get;set;}//text Area
        public string IV_Others2                            {get;set;}//text Area
        public string IV_Others3                            {get;set;}//text Area
        public string IV_Others4                            {get;set;}//text Area
        public string IV_Others5                            {get;set;}//text Area
        public string IV_OtherProof                         {get;set;}//text Area


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
