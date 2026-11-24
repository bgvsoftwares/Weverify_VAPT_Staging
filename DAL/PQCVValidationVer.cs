using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public  class PQCVValidationVer
    {
        public PQCVValidationVer()
        {
            CV_Yr_Passing               =string.Empty;
            CV_Prev_Emplymnt            =string.Empty;
            CV_Qualification            =string.Empty;
            CV_Period_Emplymnt          =string.Empty;
            CV_Edu_Details              =string.Empty;
            CV_Emp_Details              =string.Empty;
            CV_DOB                      =string.Empty;
            CV_Father_Name              =string.Empty;
            CV_Name_Institute           =string.Empty;
            CV_Current_Emplymnt         =string.Empty;
            CV_Affiliated_University    =string.Empty;
            ATA_Reverse_Directory       =string.Empty;
            ATA_Residcl_Commrcl         =string.Empty;
            ATA_Stock_Exchnge           =string.Empty;
            ATA_Telphne_Directry_Srch   =string.Empty;
            ATA_Yellow_Pages            =string.Empty;
            ATA_Who_Domain              =string.Empty;
            ATA_GoogleSearch            =string.Empty;
            ATA_InfrastructureOfCmp     =string.Empty;
            ATA_Just_Dial               =string.Empty;
            ATA_Neighbor_Check1         =string.Empty;
            ATA_NASCOM_Empanelment      =string.Empty;
            ATA_Photograph              =string.Empty;
            ATA_Physical_Site           =string.Empty;
            ATA_Reg_Company             =string.Empty;
            ATA_PO_Courier_Check        =string.Empty;
            ATA_Network_Sol             =string.Empty;
            ATA_ApprxEmpWorking         =string.Empty;
            ATA_CID_No                  =string.Empty;
            ATA_Cmpny_Addr              =string.Empty;
            ATA_Decoy_Call              =string.Empty;
            ATA_Cmpny_Website           =string.Empty;
            CV_Others                   =string.Empty;
            CV_Others2                  =string.Empty;
            CV_Others3                  =string.Empty;
            CV_Others4                  =string.Empty;
            CV_Others5                  =string.Empty;
            CV_OtherProof = string.Empty;

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
public int CVValidationVerRowID { get; set; }
public int CVValidationRowID { get; set; }
 public virtual PQCVValidation PQCVValidation { get; set; }
public string CV_Yr_Passing                 {get;set;}
public string CV_Prev_Emplymnt              {get;set;}
public string CV_Qualification              {get;set;}//text filed
public string CV_Period_Emplymnt            {get;set;}
public string CV_Edu_Details                {get;set;}
public string CV_Emp_Details                {get;set;}
public string CV_DOB                        {get;set;}
public string CV_Father_Name                {get;set;}
public string CV_Name_Institute             {get;set;}
public string CV_Current_Emplymnt           {get;set;}
public string CV_Affiliated_University      {get;set;}
public string ATA_Reverse_Directory         {get;set;}
public string ATA_Residcl_Commrcl           {get;set;}
public string ATA_Stock_Exchnge             {get;set;}
public string ATA_Telphne_Directry_Srch     {get;set;}
public string ATA_Yellow_Pages              {get;set;}
public string ATA_Who_Domain                {get;set;}
public string ATA_GoogleSearch              {get;set;}
public string ATA_InfrastructureOfCmp       {get;set;}
public string ATA_Just_Dial                 {get;set;}
public string ATA_Neighbor_Check1           {get;set;}
public string ATA_NASCOM_Empanelment        {get;set;}
public string ATA_Photograph                {get;set;}
public string ATA_Physical_Site             {get;set;}
public string ATA_Reg_Company               {get;set;}
public string ATA_PO_Courier_Check          {get;set;}
public string ATA_Network_Sol               {get;set;}
public string ATA_ApprxEmpWorking           {get;set;}
public string ATA_CID_No                    {get;set;}//text Filed
public string ATA_Cmpny_Addr                {get;set;}//text Filed
public string ATA_Decoy_Call                {get;set;}
public string ATA_Cmpny_Website             {get;set;}
public string CV_Others                     {get;set;}
public string CV_Others2                    {get;set;}
public string CV_Others3                    {get;set;}
public string CV_Others4                    {get;set;}
public string CV_Others5                    {get;set;}
public string CV_OtherProof                 {get;set;}

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
