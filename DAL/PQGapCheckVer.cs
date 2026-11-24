using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQGapCheckVer
    {
        public PQGapCheckVer()
        {
            GapCheckVerRowID = 0;
            GapCheckRowID = 0;
            GC_Father_Name_PVerification        =string.Empty;
            GC_Father_Name_Pverification1       =string.Empty;
            GC_Landmark                         =string.Empty;
            GC_ID_Proof_No                      =string.Empty;
            GC_Period_To                        =string.Empty;
            GC_Period_Frm                       =string.Empty;
            GC_Own_Rent_Hypoth                  =string.Empty;
            GC_Particulars                      =string.Empty;
            GC_Particulars1                     =string.Empty;
            GC_Neighbor_Check2                  =string.Empty;
            GC_Neighbor_Check4                  =string.Empty;
            GC_Police_Verif                     =string.Empty;
            GC_Any_Other_Commnts                =string.Empty;
            GC_Building_Status                  =string.Empty;
            GC_Color_Building                   =string.Empty;
            GC_DOB_Sub_Police_Verif             =string.Empty;
            GC_DOB_Sub_Police_Verif1            =string.Empty;
            GC_ConNo                            =string.Empty;
            GC_Addr_Dur_Gap                     =string.Empty;
            GC_Addr_Line1                       =string.Empty;
            GC_Addr_Line2                       =string.Empty;
            GC_Refre_Name_Detail                =string.Empty;
            GC_Stated_Reason_Gap                =string.Empty;
            GC_Verified_As                      =string.Empty;
            ATA_Reverse_Directory               =string.Empty;
            ATA_Residcl_Commrcl                 =string.Empty;
            ATA_Stock_Exchnge                   =string.Empty;
            ATA_Telphne_Directry_Srch           =string.Empty;
            ATA_Yellow_Pages                    =string.Empty;
            ATA_Who_Domain                      =string.Empty;
            ATA_GoogleSearch                    =string.Empty;
            ATA_InfrastructureOfCmp             =string.Empty;
            ATA_Just_Dial                       =string.Empty;
            ATA_Neighbor_Check1                 =string.Empty;
            ATA_NASCOM_Empanelment              =string.Empty;
            ATA_Photograph                      =string.Empty;
            ATA_Physical_Site                   =string.Empty;
            ATA_Reg_Company                     =string.Empty;
            ATA_PO_Courier_Check                =string.Empty;
            ATA_Network_Sol                     =string.Empty;
            ATA_ApprxEmpWorking                 =string.Empty;
            ATA_CID_No                          =string.Empty;
            ATA_Cmpny_Addr                      =string.Empty;
            ATA_Decoy_Call                      =string.Empty;
            ATA_Cmpny_Website                   =string.Empty;
            GC_Others                           =string.Empty;
            GC_Others2                          =string.Empty;
            GC_Others3                          =string.Empty;
            GC_Others4                          =string.Empty;
            GC_Others5                          =string.Empty;
            GC_OtherProof                       =string.Empty;

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

public int      GapCheckVerRowID { get; set; }
public int      GapCheckRowID      { get; set; }
public virtual PQGapCheck PQGapCheck { get; set; }
public string GC_Father_Name_PVerification      {get;set;}//text filed
public string GC_Father_Name_Pverification1     {get;set;}//text filed
public string GC_Landmark                       {get;set;}//text filed
public string GC_ID_Proof_No                    {get;set;}//Text Area
public string GC_Period_To                      {get;set;}//text filed
public string GC_Period_Frm                     {get;set;}//text filed
public string GC_Own_Rent_Hypoth                {get;set;}//Text Area
public string GC_Particulars                    {get;set;}//Text Area
public string GC_Particulars1                   {get;set;}//Text Area
public string GC_Neighbor_Check2                {get;set;}//Text Area
public string GC_Neighbor_Check4                {get;set;}//Text Area
public string GC_Police_Verif                   {get;set;}//Text Area
public string GC_Any_Other_Commnts              {get;set;}//Text Area
public string GC_Building_Status                {get;set;}//Text Area
public string GC_Color_Building                 {get;set;}//Text Area
public string GC_DOB_Sub_Police_Verif           {get;set;}//Text Area
public string GC_DOB_Sub_Police_Verif1          {get;set;}//Text Area
public string GC_ConNo                          {get;set;}//Text Area
public string GC_Addr_Dur_Gap                   {get;set;}//Text Filed
public string GC_Addr_Line1                     {get;set;}//Text Filed
public string GC_Addr_Line2                     {get;set;}//Text Filed
public string GC_Refre_Name_Detail              {get;set;}//text Area
public string GC_Stated_Reason_Gap              {get;set;}//text Area
public string GC_Verified_As                    {get;set;}//text Area
public string ATA_Reverse_Directory             {get;set;}//text Area
public string ATA_Residcl_Commrcl               {get;set;}//text Area
public string ATA_Stock_Exchnge                 {get;set;}//text Area
public string ATA_Telphne_Directry_Srch         {get;set;}//text Area
public string ATA_Yellow_Pages                  {get;set;}//text Area
public string ATA_Who_Domain                    {get;set;}//text Area
public string ATA_GoogleSearch                  {get;set;}//text Area
public string ATA_InfrastructureOfCmp           {get;set;}//text Area
public string ATA_Just_Dial                     {get;set;}//text Area
public string ATA_Neighbor_Check1               {get;set;}//text Area
public string ATA_NASCOM_Empanelment            {get;set;}//text Area
public string ATA_Photograph                    {get;set;}//text Area
public string ATA_Physical_Site                 {get;set;}//text Area
public string ATA_Reg_Company                   {get;set;}//text Area
public string ATA_PO_Courier_Check              {get;set;}//text Area
public string ATA_Network_Sol                   {get;set;}//text Area
public string ATA_ApprxEmpWorking               {get;set;}//text Area
public string ATA_CID_No                        {get;set;}//text Area
public string ATA_Cmpny_Addr                    {get;set;}//text Area
public string ATA_Decoy_Call                    {get;set;}//text Area
public string ATA_Cmpny_Website                 {get;set;}//text Area
public string GC_Others                         {get;set;}//text Area
public string GC_Others2                        {get;set;}//text Area
public string GC_Others3                        {get;set;}//text Area
public string GC_Others4                        {get;set;}//text Area
public string GC_Others5                        {get;set;}//text Area
public string GC_OtherProof                     { get; set; }//text Area

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
