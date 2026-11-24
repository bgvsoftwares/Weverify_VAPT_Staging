using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQClientMaster
    {
        public PQClientMaster()
        {
            ClientRowID = 0;
            

            ClientCode1 = string.Empty;
            ClientAbbRowID = 0;
            ClientSubgroupID = 0;
            CountryRowID = 0;
            StateRowID = 0;
            DistrictRowID = 0;
            LocationRowID = 0;
            PinCode = 0;

            Address = string.Empty;
            RegisteredAddress = string.Empty;
            CorporateOfficeAddress = string.Empty;

            CodeGeneration = 0;
            BORowID = 0;
            BillingRowID = 0;
            ReportSentBy = 0;
            InterimReport = 0;
            ContractDate = DateTime.Now;
            PricingType = 0;
            PaymentTermIndays = 0;
            SpecialInstructions = string.Empty;

            MapAntecedent = 0;
            MapDisposition = 0;
            MapSeverity = 0;
            MapHolidays = 0;
            MailSendBy = 0;
            HRApprovalRequired = 0;
            DaysCalculation = 0;
            ReOpenCases = 0;
            ReOpenBilling = 0;
            InsuffBilling = 0;
            OtherThanGreenBilling = 0;
            StopCaseBilling = 0;
            WIPOrClosedBilling = 0;
            POApplicable = 0;
            EmploymentPV = 0;
            YTRStatus = 0;
            VerbalReportStatus = 0;
            CandidateContact = 0;
            ClientNameDisclosure = 0;
            ClientStatus = 0;

            ExtraExpenses = 0;
            EEAllowedAmount = 0.0;
            BusinessCommitment = 0;
            BusinsComtntNoOfCase = 0;
            InsuffClientReport = 0;
            InsuffHoldDays = 0;
            Incentive = 0;
            IncentiveInstruction = string.Empty;
            Penalty = 0;
            PenaltyDetails = string.Empty;
            Liability = 0;
            LiabilityDetails = string.Empty;
            BillingAprvl = 0;
            BillingAprvlDetails = string.Empty;

            

           
            CSpocDesignation = string.Empty;
            CSpocContactNo = string.Empty;
            CSpocMobileNo = string.Empty;
          

            CBillingSpocName = string.Empty;
            CBillingSpocDesignation = string.Empty;
            CBillingSpocMobileNo = string.Empty;
            CBillingSpocEmailID = string.Empty;
            CBillingInstructions = string.Empty;
            CBillingAddress = string.Empty;

            CEsclationSpocName = string.Empty;
            CEsclationSpocDesignation = string.Empty;
            CEsclationSpocMobileNo = string.Empty;
            CEsclationSpocEmailID = string.Empty;

            CSendInsuffDisplay = string.Empty;
            CSendInsuffEmail = string.Empty;
            CSendReportDisplay = string.Empty;
            CSendReportEmail = string.Empty;
            CSendRedReportDisplay = string.Empty;
            CSendRedReportEmail = string.Empty;
            CSendBillingAprvlDisplay = string.Empty;
            CSendBillingAprvlEmail = string.Empty;

            Remark = string.Empty;

            SMTPServer = string.Empty;
            SMTPPort = string.Empty;
            SMTPUserName = string.Empty;
            SMTPPassword = string.Empty;
            EnableSsl = 0;

            Status = 0;
            CreatedDate = DateTime.Now;
            SLAUploaded = 0;
            CoverPage = 0;
            AgreementType = 0;
            PrevClientRowID = 0;
            HardCopy = 0;
            ClientCategory = string.Empty;
            CSalesSPOCName = string.Empty;
            CSalesStatus = 0;
            CSalesCount = 0;

            Other1 = string.Empty;
            Other2 = string.Empty;
            Other3 = string.Empty;
            Other4 = string.Empty;
            Other5 = string.Empty;
            ExtraCostNoLimit = 0;

            AutoReportGenerate = 0;
        }

        public byte AutoReportGenerate { get; set; }
        public short        ClientRowID     { get; set; }
        public string       ClientCode      { get; set; }
        public string       ClientCode1     { get; set; }

        public short        ClientAbbRowID  { get; set; }
        public virtual MasterClientAbbreviation MasterAbbreviation { get; set; }

        public short ClientSubgroupID { get; set; }
        public virtual MasterClientSubGroup MasterClientSubGroup { get; set; }

        public short CountryRowID { get; set; }
        public virtual MasterCountry MasterCountry { get; set; }

        public short StateRowID { get; set; }
        public virtual MasterState MasterState { get; set; }

        public short DistrictRowID { get; set; }
        public virtual MasterDistrict MasterDistrict { get; set; }

        public int LocationRowID { get; set; }
        public virtual MasterLocation MasterLocation { get; set; }

        public int PinCode { get; set; }
        public string Address { get; set; }
        public string RegisteredAddress { get; set; }
        public string CorporateOfficeAddress { get; set; }
        public byte CodeGeneration { get; set; }    //{1-Yes & 0-No}

        public short BORowID { get; set; }
        public virtual MasterCompanyBranch MasterCompanyBranch { get; set; }

        public byte BillingRowID { get; set; }
        public virtual MasterBillingCycle MasterBillingCycle { get; set; }

        public byte ReportSentBy { get; set; }      //{1-Mail & 2-Physical Copy, 3-Both}
        public byte InterimReport { get; set; }     //{1-Yes or 0-No)
        public DateTime? ContractDate { get; set; }
        public DateTime? ContractComplitionDate { get; set; }
        public byte PricingType { get; set; }               //{1-Package wise or 2-Check wise or 3-Both}
        public byte PaymentTermIndays { get; set; }
        public string SpecialInstructions { get; set; }
        public byte ContactNumberVisibility { get; set; }             //{1-Yes or 0-No)
        //Conditions
        public byte MapAntecedent { get; set; }         //{1-Client or 0-Default)   (All ask for manage screen if selected client)
        public byte MapDisposition { get; set; }        //{1-Client or 0-Default)   (All ask for manage screen if selected client)
        public byte MapSeverity { get; set; }           //{1-Client or 0-Default)   (All ask for manage screen if selected client)
        public byte MapHolidays { get; set; }           //{1-Client or 0-Default) (All ask for manage screen if selected client)
        public byte MailSendBy { get; set; }            //{1-Client or 0-Default)   (All ask for manage screen if selected client)
        public byte HRApprovalRequired { get; set; }    //HRApproval Required {1-Yes or 0-No)
        public byte DaysCalculation { get; set; }       //{1-Working Days or 0-Calendar Days)
        public byte ReOpenCases { get; set; }               //{1-Billable & 0-Not Billable}        
        public byte ReOpenBilling { get; set; }             //{1-Yes or 0-No)
        public byte InsuffBilling { get; set; }             //{1-Yes or 0-No)
        public byte OtherThanGreenBilling { get; set; }             //{1-Yes or 0-No)
        public byte StopCaseBilling { get; set; }             //{1-Yes or 0-No)
        public byte WIPOrClosedBilling { get; set; }             //{1-Yes or 0-No)
        public byte POApplicable { get; set; }             //{1-Yes or 0-No)
        public byte EmploymentPV { get; set; }             //{1-Yes or 0-No)
        public byte YTRStatus { get; set; }             //{1-Yes or 0-No)
        public byte VerbalReportStatus { get; set; }             //{1-Yes or 0-No)
        public byte CandidateContact { get; set; }             //{1-Yes or 0-No)
        public byte ClientNameDisclosure { get; set; }             //{1-Yes or 0-No)
        public byte ClientStatus { get; set; }             //{1-Active or 0-InActive)

        //Conditions with Remarks
        public byte ExtraExpenses { get; set; }             //{1-Yes or 0-No)
        public double EEAllowedAmount { get; set; }
        public byte BusinessCommitment { get; set; }        //{1-Yes or 0-No)
        public short BusinsComtntNoOfCase { get; set; }
        public byte InsuffClientReport { get; set; }        //{1-Accepted & 0-Not Accepted}
        public byte InsuffHoldDays { get; set; }
        public byte Incentive { get; set; }                 //{1-Yes or 0-No)
        public string IncentiveInstruction { get; set; }
        public byte Penalty { get; set; }                   //{1-Yes or 0-No)
        public string PenaltyDetails { get; set; }
        public byte Liability { get; set; }                 //{1-Yes or 0-No)
        public string LiabilityDetails { get; set; }
        public byte BillingAprvl { get; set; }           //{1-Yes or 0-No)
        public string BillingAprvlDetails { get; set; }

        public string SpocName { get; set; }                //this field can accept multiple names
        public string SpocEmailID { get; set; }             //this field can accept multiple email ids

        public string CSpocName { get; set; }
        public string CSpocDesignation { get; set; }
        public string CSpocContactNo { get; set; }
        public string CSpocMobileNo { get; set; }
        public string CSpocEmailID { get; set; }                //this field can accept multiple email ids (means TextArea)

        public string CBillingSpocName { get; set; }
        public string CBillingSpocDesignation { get; set; }
        public string CBillingSpocMobileNo { get; set; }
        public string CBillingSpocEmailID { get; set; }         //this field can accept multiple email ids (means TextArea)
        public string CBillingInstructions { get; set; }
        public string CBillingAddress { get; set; }

        public string CEsclationSpocName { get; set; }
        public string CEsclationSpocDesignation { get; set; }
        public string CEsclationSpocMobileNo { get; set; }
        public string CEsclationSpocEmailID { get; set; }         //this field can accept multiple email ids (means TextArea)

        public string CSendInsuffDisplay { get; set; }
        public string CSendInsuffEmail { get; set; }
        public string CSendReportDisplay { get; set; }
        public string CSendReportEmail { get; set; }
        public string CSendRedReportDisplay { get; set; }
        public string CSendRedReportEmail { get; set; }
        public string CSendBillingAprvlDisplay { get; set; }
        public string CSendBillingAprvlEmail { get; set; }

        public string Remark { get; set; }
        
        //For Client Mail Configuration
        public string SMTPServer { get; set; }
        public string SMTPPort { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public byte EnableSsl { get; set; }

        //For internal use only Not Show on client creation page
        public byte Status { get; set; }                    //Status 1 or 0
        public DateTime? CreatedDate { get; set; }
        public byte SLAUploaded { get; set; }               //{1-Yes or 0-No) By Default 0 and update when client contact uploaded
        public byte CoverPage { get; set; }                 //{for Select Template}
        public byte AgreementType { get; set; }             //Dropdown(1-New, 2-Re-New, 3-Expired, 4-OnHold) By Default 'New'
        public short PrevClientRowID { get; set; }           //If (Re-New) then Previous ClientRowID
        public byte HardCopy { get; set; }                  //{1-Yes or 0-No)
        public string ClientCategory { get; set; }          //Dropdown(Platinum,Gold,Silver,Bronze)
        public string CSalesSPOCName { get; set; }
        public byte CSalesStatus { get; set; }              //ClientSalesStatus 1 or 0 
        public short CSalesCount { get; set; }

        public string Other1 { get; set; }
        public string Other2 { get; set; }
        public string Other3 { get; set; }
        public string Other4 { get; set; }
        public string Other5 { get; set; }

        public string RamcoId { get; set; }
        public string SpocId { get; set; }
        public byte ExtraCostNoLimit { get; set; }             //{1-Yes or 0-No)
    }
}
