using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQCompanyInvestigationVer
    {
        public PQCompanyInvestigationVer()
        {
            CompanyInvestigationVarRowID = 0;
            CompanyInvestigationRowID = 0;
            CI_Owner_Name                       =string.Empty;
            CI_Any_Other_Commnts_FDdback        =string.Empty;
            CI_Apptmnt_Details                  =string.Empty;
            CI_Area_Of_Business                 =string.Empty;
            CI_Company_IdentityNO               =string.Empty;
            CI_Dur_Existence                    =string.Empty;
            CI_Exp_During_Business_Dealing      =string.Empty;
            CI_Exp_During_Financ_Dealing        =string.Empty;
            CI_Name_Cmpny                       =string.Empty;
            CI_Property                         =string.Empty;
            CI_Quality_Recognition              =string.Empty;
            CI_Reg_Name_Cmpny                   =string.Empty;
            CI_Reg_Doc_Type                     =string.Empty;
            CI_RegNo                            =string.Empty;
            CI_Relation_Business_Entity         =string.Empty;
            CI_Rent_Own                         =string.Empty;
            CI_Tax_IdentityNo                   =string.Empty;
            CI_Type_Org_Legal_Status            =string.Empty;
            CI_Yr_Incorporation                 =string.Empty;
            CI_Dur_Business_Relation_Cmpny      =string.Empty;
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
            CI_Others                           =string.Empty;
            CI_Others2                          =string.Empty;
            CI_Others3                          =string.Empty;
            CI_Others4                          =string.Empty;
            CI_Others5                          =string.Empty;
            CI_OtherProof = string.Empty;

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

public    int CompanyInvestigationVarRowID      { get; set; }

        public int CompanyInvestigationRowID { get; set; }
        public virtual PQCompanyInvestigation PQCompanyInvestigation { get; set; }
public string CI_Owner_Name                     {get;set;}// Text filed
public string CI_Any_Other_Commnts_FDdback      {get;set;}//text area
public string CI_Apptmnt_Details                {get;set;}//text area
public string CI_Area_Of_Business               {get;set;}//text filed
public string CI_Company_IdentityNO             {get;set;}// text filed
public string CI_Dur_Existence                  {get;set;}// text area
public string CI_Exp_During_Business_Dealing    {get;set;}//text area
public string CI_Exp_During_Financ_Dealing      {get;set;}//text area
public string CI_Name_Cmpny                     {get;set;}// Text Area
public string CI_Property                       {get;set;}// Text Area
public string CI_Quality_Recognition            {get;set;}//Text Area
public string CI_Reg_Name_Cmpny                 {get;set;}//Text Filed
public string CI_Reg_Doc_Type                   {get;set;}//Text Area
public string CI_RegNo                          {get;set;}// Text Area
public string CI_Relation_Business_Entity       {get;set;}//Text Area
public string CI_Rent_Own                       {get;set;}//Text Area
public string CI_Tax_IdentityNo                 {get;set;}//Text Area
public string CI_Type_Org_Legal_Status          {get;set;}//Text Area
public string CI_Yr_Incorporation               {get;set;}//Text Area
public string CI_Dur_Business_Relation_Cmpny    {get;set;}//Text Area
public string ATA_Reverse_Directory             {get;set;}//Text Area
public string ATA_Residcl_Commrcl               {get;set;}//Text Area
public string ATA_Stock_Exchnge                 {get;set;}//Text Area
public string ATA_Telphne_Directry_Srch         {get;set;}//Text Area
public string ATA_Yellow_Pages                  {get;set;}//Text Area
public string ATA_Who_Domain                    {get;set;}//Text Area
public string ATA_GoogleSearch                  {get;set;}// text area
public string ATA_InfrastructureOfCmp           {get;set;}// Text Area
public string ATA_Just_Dial                     {get;set;}// Text Area
public string ATA_Neighbor_Check1               {get;set;}// Text Area
public string ATA_NASCOM_Empanelment            {get;set;}// Text Area
public string ATA_Photograph                    {get;set;}// Text Area
public string ATA_Physical_Site                 {get;set;}//Text area
public string ATA_Reg_Company                   {get;set;}// Teat Area
public string ATA_PO_Courier_Check              {get;set;}// Teat Area
public string ATA_Network_Sol                   {get;set;}// Text Area
public string ATA_ApprxEmpWorking               {get;set;}// Teat fited
public string ATA_CID_No                        {get;set;}// text filed
public string ATA_Cmpny_Addr                    {get;set;}// text filed
public string ATA_Decoy_Call                    {get;set;}// text Area
public string ATA_Cmpny_Website                 {get;set;}// Text Area
public string CI_Others                         {get;set;}      //text Area
public string CI_Others2                        {get;set;}      //text Area
public string CI_Others3                        {get;set;}      //text Area
public string CI_Others4                        {get;set;}      //text Area
public string CI_Others5                        {get;set;}      //text Area
public string CI_OtherProof                     { get; set; }   //text Area

       
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
