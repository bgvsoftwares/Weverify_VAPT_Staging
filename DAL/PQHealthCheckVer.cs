using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class PQHealthCheckVer
    {
        public PQHealthCheckVer()
        {
            HC_DLC                          =string.Empty;
            HC_Cholesterol                  =string.Empty;
            HC_Blood_Group                  =string.Empty;
            HC_ESR                          =string.Empty;
            HC_HB                           =string.Empty;
            HC_CT_Scan                      =string.Empty;
            HC_BP_Check                     =string.Empty;
            HC_Full_Body_Check              =string.Empty;
            HC_Diabetes                     =string.Empty;
            HC_Vision_Test                  =string.Empty;
            HC_Other                        =string.Empty;
            HC_Test_Conducted               =string.Empty;
            HC_TLC                          =string.Empty;
            HC_Urine                        =string.Empty;
            HC_VDRL                         =string.Empty;
            HC_Serum                        =string.Empty;
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
            HC_Others                       =string.Empty;
            HC_Others2                      =string.Empty;
            HC_Others3                      =string.Empty;
            HC_Others4                      =string.Empty;
            HC_Others5                      =string.Empty;
            HC_OtherProof = string.Empty;

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
        public int HealthCheckVerRowID { get; set; }
        public int HealthCheckRowID { get; set; }
        public virtual PQHealthCheck PQHealthCheck { get; set; }
public string HC_DLC                        {get;set;}//Text Area
public string HC_Cholesterol                {get;set;}//Text Area
public string HC_Blood_Group                {get;set;}//Text Area
public string HC_ESR                        {get;set;}//Text Area
public string HC_HB                         {get;set;}//Text Area
public string HC_CT_Scan                    {get;set;}//Text Area
public string HC_BP_Check                   {get;set;}//Text Area
public string HC_Full_Body_Check            {get;set;}//Text Area
public string HC_Diabetes                   {get;set;}//Text Area
public string HC_Vision_Test                {get;set;}//Text Area
public string HC_Other                      {get;set;}//Text Area
public string HC_Test_Conducted             {get;set;}//Text Area
public string HC_TLC                        {get;set;}//Text Area
public string HC_Urine                      {get;set;}//Text Area
public string HC_VDRL                       {get;set;}//Text Area
public string HC_Serum                      {get;set;}//Text Area
public string ATA_Reverse_Directory         {get;set;}//Text Area
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
public string ATA_CID_No                    {get;set;}//text Filed
public string ATA_Cmpny_Addr                {get;set;}//text Filed
public string ATA_Decoy_Call                {get;set;}     //Text Area
public string ATA_Cmpny_Website             {get;set;}     //Text Area
public string HC_Others                     {get;set;}     //Text Area
public string HC_Others2                    {get;set;}     //Text Area
public string HC_Others3                    {get;set;}     //Text Area
public string HC_Others4                    {get;set;}     //Text Area
public string HC_Others5                    {get;set;}     //Text Area
public string HC_OtherProof                 { get; set; }  //Text Area

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
