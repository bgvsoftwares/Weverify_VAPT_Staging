using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;

namespace ViewModels.ReportQCViewModel
{
    public class ReportGenerateViewModel
    {

    }


    public class LivenessResult
    {
        [JsonProperty("isFaceDetected")]
        public bool? IsFaceDetected { get; set; }

        [JsonProperty("areMultipleFacesDetected")]
        public bool? AreMultipleFacesDetected { get; set; }

        [JsonProperty("isOtpMatch")]
        public bool? IsOtpMatch { get; set; }

        [JsonProperty("otpMatchConfidence")]
        public double? OtpMatchConfidence { get; set; }

        [JsonProperty("otpText")]
        public string OtpText { get; set; }

        [JsonProperty("isFaceMatch")]
        public bool? IsFaceMatch { get; set; }  // can be null per sample

        [JsonProperty("faceMatchScore")]
        public double? FaceMatchScore { get; set; }  // can be null per sample

        [JsonProperty("isFaceConsistent")]
        public bool? IsFaceConsistent { get; set; }

        [JsonProperty("faceConsistencyScore")]
        public double? FaceConsistencyScore { get; set; }

        [JsonProperty("isLive")]
        public bool? IsLive { get; set; }

        [JsonProperty("livenessScore")]
        public double? LivenessScore { get; set; }
    }


    public class ReportViewModel
    {

        public DateTime? OrderDate { get; set; }
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }
        public string ReportPart { get; set; }
        public int SubCheckRowID { get; set; }

        public short RWQCTMTo { get; set; }
        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [Display(Name = "Client Ref ID : ")]
        public string ClientRefID { get; set; }

        [Display(Name = "Location : ")]
        public string Location { get; set; }

        [Display(Name = "Securitas ID : ")]
        public string CompanyRefNo { get; set; }

        public string OldCompanyRefNo { get; set; }

        [Display(Name = "Candidate Code : ")]
        public string CandidateCode { get; set; }

        [Display(Name = "Title : ")]
        public string CandTitle { get; set; }

        [Display(Name = "Name : ")]
        public string PersonalName { get; set; }

        [Display(Name = "Middle Name : ")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        public string LastName { get; set; }

        [Display(Name = "Title : ")]
        public string FatherTitle { get; set; }

        [Display(Name = "father Name : ")]
        public string FatherName { get; set; }

        [Display(Name = "Middle Name : ")]
        public string FatherMiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        public string FatherLastName { get; set; }

        [Display(Name = "Gender : ")]
        public string Gender { get; set; }

        [Display(Name = "Date Of Birth : ")]
        public DateTime? DOB { get; set; }
        public string  dob { get; set; }

        [Display(Name = "Marital Status : ")]
        public string MaritalStatus { get; set; }

        [Display(Name = "Mobile No : ")]
        public string MobileNo { get; set; }

        [Display(Name = "Email : ")]
        public string EmailID { get; set; }

        public string EmploymentId { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public string  DOJ { get; set; }
        public bool FinalReport { get; set; }

        public byte ReportTemplate { get; set; }

        public DateTime? AllocatedDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Designation { get; set; }
        public string TechLocation { get; set; }
        public int status { get; set; }
        public string ProcessName { get; set; }
        public string BusinessName { get; set; }
        public string Nationality { get; set; }
        public string Report_Type { get; set; }

        public string CalsheetLocation { get; set; }

        public string FinalColor { get; set; }
        public string FinalColorCode { get; set; }
        public DateTime? ReportDate { get; set; }
        public DateTime? IntrimReportDate { get; set; }
        public DateTime? FirstIntrimReportDate { get; set; }
        public string FinalReportDate { get; set; }
        public string SupplementaryReportDate { get; set; }
        public DateTime? TCSReportDate { get; set; }
        public DateTime? TCSIntrimReportDate { get; set; }
        public string livenessScore { get; set; }
        public string faceConsistencyScore { get; set; }
        public string faceMatchScore { get; set; }
        public string IsFaceMatch { get; set; }
        public string IsFaceConsistent { get; set; }
        public string VideoFileName { get; set; }
        public string AadharNumber { get; set; }
        
        public string ReportStatus { get; set; }
        public string RequestID { get; set; }

        public string FranchiseName { get; set; }


        public string BGV1ColorName { get; set; }
        public string BGV2ColorName { get; set; }
        public string BGV1ColorCode { get; set; }
        public string BGV2ColorCode { get; set; }

        public string EmploymentType { get; set; }
        public string EmploymentType1 { get; set; }

        [Display(Name = "Create By : ")]
        public short CreatedBy { get; set; }

        [Display(Name = "Create Date : ")]
        public DateTime? CreatedDate { get; set; }

        public string DocPath { get; set; }
        public string Checkclosedate { get; set; }
        public string SSNNo { get; set; }

        public string ReverificationDate { get; set; }
        public string SecondReverificationDate { get; set; }
        public string FirstSupplimentryDate { get; set; }
        public string SecondSupplimentryDate { get; set; }
        public string AmendedDate { get; set; }
        public string ReverificationCloseDate { get; set; }
        public int CaseStatusId { get; set; }
        public string ReverificationInitiateDateGENPACT { get; set; }
        public string ReverificationCloseDateGENPACT { get; set; }
        public string AddressNoteRemark { get; set; }
        public string EmpNoteRemark { get; set; }        
        public int TotalRiskScore { get; set; }        

        public List<CheckInfo> CheckInfoModel = new List<CheckInfo>();

        public List<RiskMatrixViewModel> RiskFactorInfoModel = new List<RiskMatrixViewModel>();

        public List<RiskMatrixConsolidatedItem> RiskMatrixConsolidateInfoModel = new List<RiskMatrixConsolidatedItem>();

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails { get; set; }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_OldVersionCheck { get; set; }
        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_AddCheck { get; set; }
        //AntecedentRowID,DisplayNameRep,DataEntryValue,OpsValue,PersonalRowID,SubcheckRowID
        public List<TechMRemarkModel> TechMEmploymentRemarksList = new List<TechMRemarkModel>();
        public List<TechMRemarkModel> TechMEducationRemarksList = new List<TechMRemarkModel>();

        public List<PersonalInfo> PersonalInfoModel = new List<PersonalInfo>();
        public List<PersonalInfo> AddressInfoModel = new List<PersonalInfo>();
        public List<PersonalInfo> EmploymentInfoModel = new List<PersonalInfo>();
        public List<PersonalInfo> EducationInfoModel = new List<PersonalInfo>();
        public List<PersonalInfo> NationalInfoModel = new List<PersonalInfo>();
        public List<PersonalInfo> WiproEducationCompInfoModel = new List<PersonalInfo>();
        public List<PersonalInfo> WiproEmploymentCompInfoModel = new List<PersonalInfo>();
        public List<PersonalInfo> ReferenceInfoModel = new List<PersonalInfo>();
        public List<PersonalInfo> CriminalInfoModel = new List<PersonalInfo>();
        public List<PersonalInfo> SpecialInfoModel = new List<PersonalInfo>();
        public List<CriminalCourtVerificationVM> CriminalResultModel = new List<CriminalCourtVerificationVM>();

        public List<CVVerificationDetailModel> CVValidationInfoModel = new List<CVVerificationDetailModel>();

        public IEnumerable<CandidateCaseDocumentListViewModel> CandidateCaseDocList { get; set; }
        public IEnumerable<CandidateAllDocumentListViewModel> CandidateDocList { get; set; }
        public IEnumerable<LTIUANReportDetail> LTIUANReportDetaillist { get; set; }
        public ClientViewModel.GetDynamicReport TekInfo { get; set; }
        public string InterimReportDate { get; set; }
        public string InsuffRaisedDate { get; set; }
        public string InsuffClearDate { get; set; }
        public int PendingCheck { get; set; }
        public string EntityName { get; set; }
        public int EmpCheckCount { get; set; } // Ankit
        public int NationalCount { get; set; } // Ankit
        public int ReferenceCheckCount { get; set; } // Ankit
        public string Address_currentPermanantSame { get; set; }  // Ankit
        public string Criminal_currentPermanantSame { get; set; }  // Ankit
        public string CaseType { get; set; }
        public string AccentureCheckFamily { get; set; }
        public List<ReportVerificationScope> ReportVerificationScopeList { get; set; }
        public int EducationCount { get; set; }
        public int CriminalCount { get; set; }
        public string  UID { get; set; }
        public string OHR_TaleoID { get; set; } 
        public string  band { get; set; }
        public string  COE { get; set; }
        public string  SDO { get; set; }
        public string  PID { get; set; }
        public string  PIDOldDate { get; set; }
        public string  PIDNewDate { get; set; }
        public string DBCheckDate { get; set; }
        public string DBReportDate { get; set; }
        public string DBCheckStatus { get; set; }
        public string OrganizationName { get; set; }

        public string MagistrateCourt { get; set; }
        public string SessionsCourt { get; set; }
        public string HighCourt { get; set; }
        public string SupremeCourt { get; set; }
        public string ValuePitchSearchID { get; set; }

        public string AdditionalComments { get; set; }
        public string TypeofDocument { get; set; }
        public string PFNo_TDSCertificateNumber { get; set; }
        public string PANnumberofCompany { get; set; }
        public string TANnumberofCompany { get; set; }
        public string PANnumberofEmployee { get; set; }
        public string AssessmentYear { get; set; }
        public string Undertaking { get; set; }
        public string SealandSignature { get; set; }


        public string AdditionalCommentsStatus { get; set; }
        public string TypeofDocumentStatus { get; set; }
        public string CompanyNameStatus { get; set; }
        public string PFNo_TDSCertificateNumberStatus { get; set; }
        public string PANnumberofCompanyStatus { get; set; }
        public string TANnumberofCompanyStatus { get; set; }
        public string PANnumberofEmployeeStatus { get; set; }
        public string AssessmentYearStatus { get; set; }
        public string UndertakingStatus { get; set; }
        public string SealandSignatureStatus { get; set; }

        public string CurrentAddress { get; set; }
        public string Current_Periodofstay { get; set; }
        public string Current_Status { get; set; }
        public string Current_ModeofVerification { get; set; }
        public string Current_DateandTime { get; set; }
        public string Current_Comments { get; set; }
        public bool currentAddressStatus { get; set; }
        public string PermanentAddress { get; set; }
        public string Permanent_Periodofstay { get; set; }
        public string Permanent_Status { get; set; }
        public string Permanent_ModeofVerification { get; set; }
        public string Permanent_DateandTime { get; set; }
        public string Permanent_Comments { get; set; }
        public bool PermanentAddressStatus { get; set; }
        public int EligibletoWork { get; set; }
        public string CandidatePhoto { get; set; }
        public IEnumerable<DrugTestResult> DrugTestResultList { get; set; }
        public IEnumerable<WiproUANCheck> WiproUANCheckList { get; set; }
        public IEnumerable<WiproGapChecks> WiproGAPCheckList { get; set; }
        public IEnumerable<Comperative_Tigerlsts> Tiger_Comperative { get; set; }
        public int EmploymentCount { get; set; }
        public int AddressCount { get; set; }

        public int TotalEmpExp { get; set; }
        public string YTRClearDate { get; set; }
        public int finalReportSLA { get; set; }
        public int InterimReportSLA { get; set; }
        public int FirstInterimReportSLA { get; set; }
        public int SuppleReportSLA { get; set; }
        public int SecondSuppleReportSLA { get; set; }
        public int IsCheckWiseReport { get; set; }
        public int CheckFamilyRowID { get; set; }
        public int ReportWithOutHeader { get; set; } = 0;
        public string PackageOrCheckDetails { get; set; }

        //abhishek
        
        public bool GallagherSep2026Format { get; set; }
        public bool ReinitiationApplicable { get; set; }



    }

    public class Comperative_Tigerlsts
    {
        public int ID { get; set; }
        public int PersonalRowId { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        [Display(Name = "Company Name Onboarding")]
        public string CompanyNameOnboarding { get; set; }
        [Display(Name = "Start Date Onboarding")]
        public string StartDateOnboarding { get; set; }
        [Display(Name = "End Date Onboarding")]
        public string EndDateOnboarding { get; set; }
        [Display(Name = "Company Name UAN")]
        public string CompanyNameUAN { get; set; }
        [Display(Name = "Start Date UAN")]
        public string StartDateUAN { get; set; }
        [Display(Name = "End Date UAN")]
        public string EndDateUAN { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Email Verification")]
        public string EmailVerification { get; set; }
        public int PageSize { get; set; }
    }

    public class PersonalInfo
    {
        public string AntecedentRowID { get; set; }
        public string DisplayNameRep { get; set; }
        public string DisplayNameDE { get; set; }
        public string DataEntryValue { get; set; }
        public string OpsValue { get; set; }
        public int PersonalRowID { get; set; }
        public int SubcheckRowID { get; set; }
        public string SubCheckName { get; set; }
        public string CheckColorName { get; set; }
        public string CheckColorCode { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ReportComment { get; set; }
        public string modeofverification { get; set; }
        public string CheckStatusId { get; set; }
        public int ClientRowid { get; set; }
        public string comment { get; set; }
        public int Status { get; set; }
        public string AnticidentStatus { get; set; }
        public int CheckFamilyRowid { get; set; }
        public DateTime CreationDate { get; set; }
        public string CheckType { get; set; }
        public DateTime checkcompleteDate { get; set; }
        public string EmpNewEmpVerAlert { get; set; }


    }

    public class CheckInfo
    {
        public int SubCheckRowID { get; set; }
        public string CheckNameFDD { get; set; }
        public string Location { get; set; }
    }
    public class HtmlInputModel
    {
        public string Html { get; set; }
    }
    public class AddressInfo
    {
        public string AntecedentRowID { get; set; }
        public string DisplayNameRep { get; set; }
        public string DisplayNameDE { get; set; }
        public string DataEntryValue { get; set; }
        public string OpsValue { get; set; }
        public int PersonalRowID { get; set; }
        public int SubcheckRowID { get; set; }
    }

    public class InterimReportGenerateViewModel
    {
        public DateTime? OrderDate { get; set; }
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        public short RWQCTMTo { get; set; }
        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [Display(Name = "Client Ref ID : ")]
        public string ClientRefID { get; set; }

        //public short? SubCheckRowID { get; set; }

        [Display(Name = "Location : ")]
        public string Location { get; set; }

        [Display(Name = "Securitas ID : ")]
        public string CompanyRefNo { get; set; }

        [Display(Name = "Candidate Code : ")]
        public string CandidateCode { get; set; }

        [Display(Name = "Title : ")]
        public string CandTitle { get; set; }

        [Display(Name = "Name : ")]
        public string PersonalName { get; set; }

        [Display(Name = "Middle Name : ")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        public string LastName { get; set; }

        [Display(Name = "Title : ")]
        public string FatherTitle { get; set; }

        [Display(Name = "father Name : ")]
        public string FatherName { get; set; }

        [Display(Name = "Middle Name : ")]
        public string FatherMiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        public string FatherLastName { get; set; }

        [Display(Name = "Gender : ")]
        public string Gender { get; set; }

        [Display(Name = "Date Of Birth : ")]
        public DateTime? DOB { get; set; }

        [Display(Name = "Marital Status : ")]
        public string MaritalStatus { get; set; }

        [Display(Name = "Mobile No : ")]
        public string MobileNo { get; set; }

        [Display(Name = "Email : ")]
        public string EmailID { get; set; }

        public string EmploymentId { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public bool FinalReport { get; set; }

        public byte ReportTemplate { get; set; }

        public DateTime? AllocatedDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Designation { get; set; }
        public string TechLocation { get; set; }
        public int status { get; set; }
        public string ProcessName { get; set; }
        public string BusinessName { get; set; }
        public string Nationality { get; set; }
        public string DOJ { get; set; }
        //[Display(Name = "Pan Card : ")]
        //public byte PanCard { get; set; }

        //[Display(Name = "Pan Card : ")]
        //public string PanCardNo { get; set; }

        //[Display(Name = "Passport : ")]
        //public byte Passport { get; set; }

        //[Display(Name = "Passport : ")]
        //public string PassportNo { get; set; }

        //[Display(Name = "Aadhaar Card : ")]
        //public byte AadhaarCard { get; set; }

        //[Display(Name = "Aadhaar Card : ")]
        //public string AadhaarCardNo { get; set; }

        //[Display(Name = "Driving License : ")]
        //public byte DrivingLicense { get; set; }

        //[Display(Name = "Driving License : ")]
        //public string DrivingLicenseNo { get; set; }

        //[Display(Name = "Voter ID Card : ")]
        //public byte VoterIDCard { get; set; }

        //[Display(Name = "Voter ID Card : ")]
        //public string VoterIDCardNo { get; set; }
        public string CalsheetLocation { get; set; }

        public string FinalColor { get; set; }
        public string FinalColorCode { get; set; }
        public DateTime? ReportDate { get; set; }
        public DateTime? IntrimReportDate { get; set; }
        public string FinalReportDate { get; set; }

        [Display(Name = "Create By : ")]
        public short CreatedBy { get; set; }

        [Display(Name = "Create Date : ")]
        public DateTime? CreatedDate { get; set; }

        public string DocPath { get; set; }
        public DateTime? Checkclosedate { get; set; }
        public string SSNNo { get; set; }
        
        public string ReverificationDate { get; set; }
        public string ReverificationCloseDate { get; set; }
        public int CaseStatusId { get; set; }
        public string ReverificationInitiateDateGENPACT { get; set; }
        public string ReverificationCloseDateGENPACT { get; set; }


        public IEnumerable<CandidateCaseDocumentListViewModel> CandidateCaseDocList { get; set; }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails { get; set; }
        public IEnumerable<WiproCheckSOWViewModel> WiproChecklist { get; set; }
        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetailsDBCheck { get; set; }
        public IEnumerable<CandidateAllDocumentListViewModel> CandidateDocList { get; set; }
        public IEnumerable<AntecedentListViewModel> AntecedentList { get; set; }

        public IEnumerable<AddresssReportViewModel> AddressProvidedList { get; set; }
        public IEnumerable<AddresssReportViewModel> AddressVerifiedList { get; set; }
        public IEnumerable<AddresssReportViewModel> AddressRWQCCommentList { get; set; }


        public IEnumerable<EmploymentReportViewModel> EmploymentProvidedList { get; set; }
        public IEnumerable<EmploymentReportViewModel> EmploymentVerifiedList { get; set; }
        public IEnumerable<EmploymentReportViewModel> EmploymentRWQCCommentList { get; set; }

        public IEnumerable<EducationReportViewModel> EducationProvidedList { get; set; }
        public IEnumerable<EducationReportViewModel> EducationVerifiedList { get; set; }
        public IEnumerable<EducationReportViewModel> EducationRWQCCommentList { get; set; }

        public IEnumerable<CriminalReportViewModel> CriminalProvidedList { get; set; }
        public IEnumerable<CriminalReportViewModel> CriminalVerifiedList { get; set; }
        public IEnumerable<CriminalReportViewModel> CriminalRWQCCommentList { get; set; }

        public IEnumerable<CriminalReportViewModel> CriminalProvidedListTCS { get; set; }
        public IEnumerable<CriminalReportViewModel> CriminalVerifiedListTCS { get; set; }


        public IEnumerable<CriminalReportViewModel> CriminalProvidedListTCSCourt { get; set; }
        public IEnumerable<CriminalReportViewModel> CriminalVerifiedListTCSCourt { get; set; }

        public IEnumerable<ReferenceReportViewModel> ReferenceProvidedList { get; set; }
        public IEnumerable<ReferenceReportViewModel> ReferenceVerifiedList { get; set; }
        public IEnumerable<ReferenceReportViewModel> ReferenceRWQCCommentList { get; set; }

        public IEnumerable<NationalIdentityReportViewModel> NationalIdentityProvidedList { get; set; }
        public IEnumerable<NationalIdentityReportViewModel> NationalIdentityVerifiedList { get; set; }
        public IEnumerable<NationalIdentityReportViewModel> NationalIdentityRWQCCommentList { get; set; }

        public IEnumerable<SpecialCheckReportViewModel> SpecialCheckProvidedList { get; set; }
        public IEnumerable<SpecialCheckReportViewModel> SpecialCheckVerifiedList { get; set; }
        public IEnumerable<SpecialCheckReportViewModel> SpecialCheckRWQCCommentList { get; set; }

        public IEnumerable<SpecialCheckReportViewModel> SpecialCheckProvidedListDurgTechM { get; set; }
        public IEnumerable<SpecialCheckReportViewModel> SpecialCheckVerifiedListDurgTechM { get; set; }

        public IEnumerable<SpecialCheckReportViewModel> SpecialCheckProvidedListTechM { get; set; }
        public IEnumerable<SpecialCheckReportViewModel> SpecialCheckVerifiedListTechM { get; set; }

        public IEnumerable<colorCodeViewModel> ClientColorCodelist { get; set; }
        public IEnumerable<colorCodeOrderingViewModel> colorCodeOrderinglist { get; set; }
        public IEnumerable<WiproAnnexure> WiproAnnexurelist { get; set; }


        public IEnumerable<TCS_BPO_Cal_Sheet> TCS_BPO_Cal_Sheet { get; set; }
        public IEnumerable<CaseAntecedentComment> CaseAntecedentComment { get; set; }
        //public IEnumerable<ReportVerificationScope> ReportVerificationScope { get; set; }
        public ReportVerificationScope ReportVerificationScope { get; set; }

        public ClientViewModel.GetDynamicReport TekInfo { get; set; }


        public double TCS_BaseAmount { get; set; }
        public double TCS_Total_Amount { get; set; }
        public string Report_Type { get; set; }
        public decimal PenaltyAmount { get; set; }
        public string LocationTCS { get; set; }
        public string SOW { get; set; }
        public decimal IncentiveAmount { get; set; }
        public IEnumerable<CriminalCheck> CriminalCheck { get; set; }
        public string VerticalLocation { get; set; }
        public string ApplicationId { get; set; }
        public string ReportTypeBYSystum {get;set;}
        public string InterimReportDate { get; set; }
        public string InsuffRaisedDate { get; set; }
        public string InsuffClearDate { get; set; }
        public int PendingCheck { get; set; }
        //public string designation { get; set; }
        public DateTime? ReportUploadedDate { get; set; }
        public DateTime? Supplementaryreportsentdate { get; set; }
        public string ReportPart { get; set; }
        public string EmpNoteRemark { get; set; }
        public string AddressNoteRemark { get; set; }
        public string BGV1ColorName { get; set; }
        public string BGV2ColorName { get; set; }
        public string BGV1ColorCode { get; set; }
        public string BGV2ColorCode { get; set; }
        public int EducationCount { get; set; }
        public string EmploymentType { get; set; }
        public List<PersonalInfo> CriminalInfoModel = new List<PersonalInfo>();
        public List<PersonalInfo> SpecialInfoModel = new List<PersonalInfo>();
    }

    public class CriminalCheck
    {
        public short ClientRowID { get; set; }
        public short CheckFamilyRowID { get; set; }
        public short SubCheckRowID { get; set; }
    }
    public class AntecedentListViewModel
    {
        public short CheckFamilyRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public short AntecedentRowId { get; set; }
        public string AntecedentName { get; set; }
        public string FieldName { get; set; }
        public byte AntecedentTypeRowId { get; set; }

    }

    public class CandidateCaseDocumentListViewModel
    {
        public Int64 CaseDocRowID { get; set; }
        public string DocFile { get; set; }
        public string DocumentType { get; set; }
        public string Remarks { get; set; }
        public byte ArtifactOrder { get; set; }
    }

    public class CandidateAllDocumentListViewModel
    {
        public Int64 VerifiedUploadRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string DocFile { get; set; }
        public string DocumentType { get; set; }
        public string Remarks { get; set; }
        public byte ArtifactOrder { get; set; }
        public int? ReportArtifactsOrder { get; set; }

        public string ImageFileName { get; set; }
        public int Count { get; set; }
    }

    public class PersonalCheckBriefDetailsViewModel
    {
        public string CheckFName { get; set; }
        public string AddressNoteRemark { get; set; }
        public string SubCheckFName { get; set; }
        public string SubCheckDetail { get; set; }
        public string Status { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }

        public string UniqueComponentId { get; set; }
        public int subCheckId { get; set; }
        public string CRV_Period_Of_Stay { get; set; }
        public string Severity { get; set; }
        public string ModeofVerification { get; set; }
        public string checkStatus { get; set; }
        public int CheckFamilyRowID { get; set; }
        public string MCARegister { get; set; }
        public string Particilars { get; set; }
        public DateTime? CheckCompleteDate { get; set; }
        public string ModeOfVerification { get; set; }
        public string DateOfVerification { get; set; }
        public DateTime? CheckInitiateDate { get; set; }
        public int CheckNo { get; set; }
        public string detail1 { get; set; }
        public string detail2 { get; set; }
        public string detail3 { get; set; }
        public string detail4 { get; set; }
        public int EmpTenureMonth { get; set; }
        public string VerificationStatus { get; set; }

        public string DateVerified { get; set; }
    }
    public class WiproCheckSOWViewModel
    {
        public string CheckDetails { get; set; }
        public string CheckSOW { get; set; }

        public int ReportSequence { get; set; }

    }

    public class AddresssReportViewModel
    {
        public int AddressVerRowID { get; set; }
        public int AddressRowID { get; set; }
        public short ClientRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short CheckFamilyRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }
        public string checkOrdering { get; set; }
        public string VerifierName { get; set; }
        public string AnnexureNo { get; set; }

        #region Address Verification Antecedents
        public string UniqueComponentID { get; set; }
        public string AV_Cand_Name { get; set; }
        public string AV_Sec_Ref_No { get; set; }
        public string AV_Add { get; set; }
        public string AV_Add_Line1 { get; set; }
        public string AV_Add_Line2 { get; set; }
        public string AV_Buldng_Street_name { get; set; }
        public short AV_State { get; set; }
        public string StateName { get; set; }
        public short AV_Country { get; set; }
        public string CountryName { get; set; }
        public short AV_District { get; set; }
        public string DistrictName { get; set; }
        public int AV_City { get; set; }
        public string CityName { get; set; }
        public int AV_Pincode { get; set; }
        public string AV_Landmark { get; set; }
        public string AV_Add_Type { get; set; }
        public string AV_Dur_of_Stay { get; set; }
        public string AV_2_3BHK { get; set; }
        public string AV_Applcnt_Known_Hw_Mny_Yrs { get; set; }
        public string AV_Resp_Name { get; set; }
        public string AV_Resp_Relation_With_Cand { get; set; }
        public string AV_Resp_ConDet { get; set; }
        public string AV_Resp_ConNo { get; set; }
        public string AV_Neighbour_Name_No1 { get; set; }
        public string AV_Neighbour_Name_No2 { get; set; }
        public string AV_Resp_Remarks { get; set; }
        public string AV_Security_Confirm { get; set; }
        public string AV_Owned_Rent_Hypoth { get; set; }
        public string AV_New_Add_Det_Obtain { get; set; }
        public string AV_House_Photo_taken { get; set; }
        public string HousePhotographName { get; set; }
        public string AV_Buldng_Status { get; set; }
        public byte AV_Family_Member { get; set; }
        public string AV_Color_of_Buldng { get; set; }
        public string AV_Interior_Condition { get; set; }
        public byte AV_Total_Earning_Member { get; set; }
        public string AV_Ease_Locating { get; set; }
        public string AV_Gate_Color { get; set; }
        public byte AV_No_Vech { get; set; }
        public string AV_Applcnt_Demons_Unsual_Behv { get; set; }
        public string AV_Idproof_Prov_Resp { get; set; }
        public string AV_Income_Source { get; set; }
        public string AV_Applcnt_Habitul_Borrow_Neigh { get; set; }
        public string AV_Any_Union_Poltcl_Affl_Fmly { get; set; }
        public string AV_Locality { get; set; }
        public string AV_Moral_Charac_Applcnt { get; set; }
        public string AV_Sec_field_Exe_Name { get; set; }
        public DateTime? AV_Date_Verification { get; set; }
        public string AV_Time_Verification { get; set; }
        public string AV_Any_Other_Cmmnts_AV { get; set; }
        public string AV_OtherDetails6 { get; set; }
        public string AV_OtherDetails7 { get; set; }
        public string AV_OtherDetails8 { get; set; }
        public string AV_OtherDetails9 { get; set; }
        public string AV_OtherDetails10 { get; set; }
        public string AV_OtherDetails11 { get; set; }
        public string AV_OtherDetails12 { get; set; }
        public string AV_OtherDetails13 { get; set; }
        public string AV_OtherDetails14 { get; set; }
        public string AV_OtherDetails15 { get; set; }
        public string ATA_CID_No { get; set; }
        public string ATA_Cmpny_Addr { get; set; }
        public string ATA_ApprxEmpWorking { get; set; }
        public string ATA_Cmpny_Website { get; set; }
        public string ATA_Decoy_Call { get; set; }
        public string ATA_Reverse_Directory { get; set; }
        public string ATA_Residcl_Commrcl { get; set; }
        public string ATA_Stock_Exchnge { get; set; }
        public string ATA_Telphne_Directry_Srch { get; set; }
        public string ATA_Yellow_Pages { get; set; }
        public string ATA_Who_Domain { get; set; }
        public string ATA_GoogleSearch { get; set; }
        public string ATA_InfrastructureOfCmp { get; set; }
        public string ATA_Just_Dial { get; set; }
        public string ATA_Neighbor_Check1 { get; set; }
        public string ATA_NASCOM_Empanelment { get; set; }
        public string ATA_Photograph { get; set; }
        public string ATA_Physical_Site { get; set; }
        public string ATA_Reg_Company { get; set; }
        public string ATA_PO_Courier_Check { get; set; }
        public string ATA_Network_Sol { get; set; }
        public byte Status { get; set; }
        public string CheckStatus { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ReportComment { get; set; }
        public string ModeOfVerification { get; set; }
        public string GeoCoordinate { get; set; }
        public string NatureofLocation { get; set; }
        public string AddressProximity { get; set; }
        public string SelfieOfRespObt { get; set; }
        public string RespIDProof { get; set; }
        public string HousePhotoOfResp { get; set; }
        public string AddProofOfTheResp { get; set; }

        #endregion

    }

    public class EmploymentReportViewModel
    {
        public int EmploymentVerRowID { get; set; }
        public int EmploymentRowID { get; set; }
        public short ClientRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short CheckFamilyRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }
        public string UniqueComponentID { get; set; }
        public string AV_Sec_Ref_No { get; set; }

        public string EV_CandidateName { get; set; }
        public string EV_OrganisationName { get; set; }
        public string EV_OrganisationNameId { get; set; }
        public string EV_CmpLocation { get; set; }
        public string EV_CmpNameAddress { get; set; }
        public string EV_EmployeeName { get; set; }
        public string EV_EmployeeCode { get; set; }
        public string EV_EmploymentPeriod { get; set; }
        public string EV_EmploymentDual { get; set; }
        public string EV_EmployedSelf { get; set; }
        public string EV_lappingOver { get; set; }
        public string EV_CandidateTenureWorked { get; set; }
        public string EV_EmployeeTenure { get; set; }
        public string EV_Designation { get; set; }
        public string EV_EmployedEarlierAlso { get; set; }
        public string EV_nature_emplo { get; set; }
        public string EV_typ_employ { get; set; }
        public string EV_EmpType { get; set; }
        public string EV_Department { get; set; }
        public string EV_CmpCost { get; set; }
        public string EV_last_sal { get; set; }
        public string EV_leavingReason { get; set; }
        public string EV_termina { get; set; }
        public string EV_DiscontinueServiceReason { get; set; }
        public string EV_EligibilityforRehire { get; set; }
        public string EV_EligibilityOfRehire { get; set; }
        public string EV_EligibileForRehire { get; set; }
        public string EV_Rhre_Aplcnt_Situt_Ars_Ftre { get; set; }
        public string EV_Reported { get; set; }
        public string EV_ReportingManagerName { get; set; }
        public string EV_Rptng_Mgr_Dsg_Eid_Tno { get; set; }
        public string EV_Rptng_Mgr_Name_Ph { get; set; }
        public string EV_Rptng_Mgr_Name_ConDet { get; set; }
        public string EV_Sub_Rptng_Mgr_Name_Dsg_EidPh { get; set; }
        public string EV_Supervisor { get; set; }
        public string EV_Sup_Name_Dsg { get; set; }
        public string EV_FormalitiesExit { get; set; }
        public string EV_FullFinalFormalities { get; set; }
        public string EV_ExitFormalitiesPending { get; set; }
        public string EV_HandledResponsibilities { get; set; }
        public string EV_ResponsibilitiesHandled { get; set; }
        public string EV_OverallJobPerformance { get; set; }
        public string EV_PerfromanceLevel { get; set; }
        public string EV_KnowledgeProcess { get; set; }
        public string EV_ValidationBusinessOperation { get; set; }
        public string EV_DutiesNature { get; set; }
        public string EV_Prof_Ablts_Aplcnt { get; set; }
        public string EV_Dsplnry_Perf_Iss_Rptd { get; set; }
        public string EV_DisciplinaryIssue { get; set; }
        public string EV_Cmplnts_Rslt_Dsplnry_Sub { get; set; }
        public string EV_Emp_Invol_Fraud_Actvty { get; set; }
        public string EV_PerformanceIssues { get; set; }
        public string EV_TeamConduct { get; set; }
        public string EV_TeamPlayerNot { get; set; }
        public string EV_TechnologicalKnowledge { get; set; }
        public string EV_Fback_Accnt_Cduct_Jb { get; set; }
        public string EV_DisciplinedHowHe { get; set; }
        public string EV_LongKnown { get; set; }
        public string EV_LongWork { get; set; }
        public string EV_LeavesTook { get; set; }
        public string EV_People_Rptd { get; set; }
        public string EV_Conduct { get; set; }
        public string EV_Performance { get; set; }
        public string EV_Perf_Rate { get; set; }
        public string EV_Mtng_Deadline { get; set; }
        public string EV_Comm_Skills { get; set; }
        public string EV_Wrtn_Comm_Skills { get; set; }
        public string EV_Verb_Comm_Skills { get; set; }
        public string EV_Procs_Knowlg { get; set; }
        public string EV_Attdnce_Punct { get; set; }
        public string EV_Ldrshp_Qualty { get; set; }
        public string EV_Mngng_Mtng_Int_Cust_Expect { get; set; }
        public string EV_OtherIssues { get; set; }
        public string EV_AdherenceTAT { get; set; }
        public string EV_CompletesTATWork { get; set; }
        public string EV_Atude_Hon_Per_Aplcnt { get; set; }
        public string EV_WorkAttitude { get; set; }
        public string EV_RatingTeaming { get; set; }
        public string EV_Rate_Tech { get; set; }
        public string EV_AttendancePunctuality { get; set; }
        public string EV_Cmnts_Abt_Sub { get; set; }
        public string EV_SubjectComments { get; set; }
        public string EV_AdditionalComments { get; set; }
        public string EV_OtherComments { get; set; }
        public string EV_OtherRemarks { get; set; }
        public string EV_AttachedDocumentGenuine { get; set; }
        public string EV_ResponseMode { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_OnlineVerificationCondctedOn { get; set; }
        public string EV_RefereeResponse { get; set; }
        public string EV_RespondentName { get; set; }
        public string EV_RefereeDetails { get; set; }
        public string EV_RespondentDesignation { get; set; }
        public string EV_RefereeNameDetails { get; set; }
        public string EV_DetailsReferee { get; set; }
        public string EV_ContactDetail { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_DateTime { get; set; }
        public string EV_Verfson_Poss_Phy_Visit { get; set; }
        public string EV_Details_Verfson_EidPh_Phyvst { get; set; }
        public string EV_Nme_Add_Cmpny_Exist_Now { get; set; }
        public string EV_Name_Dsg_Verfyng_Auth { get; set; }
        public string EV_Verified { get; set; }
        public string EV_Verified_by { get; set; }
        public string EV_Verified_by_dsg_ConNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_Date_Time_Contacted { get; set; }
        public string EV_OtherDetails6 { get; set; }
        public string EV_OtherDetails7 { get; set; }
        public string EV_OtherDetails8 { get; set; }
        public string EV_OtherDetails9 { get; set; }
        public string EV_OtherDetails10 { get; set; }
        public string EV_OtherDetails11 { get; set; }
        public string EV_OtherDetails12 { get; set; }
        public string EV_OtherDetails13 { get; set; }
        public string EV_OtherDetails14 { get; set; }
        public string EV_OtherDetails15 { get; set; }
        public string ATA_Reverse_Directory { get; set; }
        public string ATA_Residcl_Commrcl { get; set; }
        public string ATA_Stock_Exchnge { get; set; }
        public string ATA_Telphne_Directry_Srch { get; set; }
        public string ATA_Yellow_Pages { get; set; }
        public string ATA_Who_Domain { get; set; }
        public string ATA_GoogleSearch { get; set; }
        public string ATA_InfrastructureOfCmp { get; set; }
        public string ATA_Just_Dial { get; set; }
        public string ATA_Neighbor_Check1 { get; set; }
        public string ATA_NASCOM_Empanelment { get; set; }
        public string ATA_Photograph { get; set; }
        public string ATA_Physical_Site { get; set; }
        public string ATA_Reg_Company { get; set; }
        public string ATA_PO_Courier_Check { get; set; }
        public string ATA_Network_Sol { get; set; }
        public string ATA_ApprxEmpWorking { get; set; }
        public string ATA_CID_No { get; set; }
        public string ATA_Cmpny_Addr { get; set; }
        public string ATA_Decoy_Call { get; set; }
        public string ATA_Cmpny_Website { get; set; }

        public string CheckStatus { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ReportComment { get; set; }
        public string checkOrdering { get; set; }
        public string VerifierName { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string Organization_City { get; set; }
        public string AnnexureNo { get; set; }
    }


    #region Education View Model

    public class EducationReportViewModel
    {
        public int EducationVerRowID { get; set; }
        public int EducationRowID { get; set; }
        public short ClientRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short CheckFamilyRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }
        public string UniqueComponentID { get; set; }
        public string EV_Sec_Ref_id { get; set; }
        public string AnnexureNo { get; set; }

        #region Education Verification Antecedents

        public string EV_Cand_Name { get; set; }
        public string EV_Cand_Name_Supp_Edu_Doc { get; set; }
        public string EV_Colg_Schl_Inst_Name { get; set; }
        public int EV_Colg_Schl_Inst_NameId { get; set; }
        public string EV_Colg_Schl_Inst_Add_ConDet { get; set; }
        public string EV_Affl_University_Name { get; set; }
        public string EV_Degree_Name { get; set; }
        public string EV_Mode_Of_Edu { get; set; }
        public string EV_Enroll_RollNo { get; set; }
        public string EV_RegNo_RegCode { get; set; }
        public string EV_Dur_Course { get; set; }
        public string EV_Course_Status { get; set; }
        public string EV_Specialization { get; set; }
        public string EV_Period_Course { get; set; }
        public string EV_Yr_Passing { get; set; }
        public string EV_Rslt_Obtain { get; set; }
        public string EV_Mode_of_Verf { get; set; }
        public string EV_Refre_Name_Dsg { get; set; }
        public string EV_Refre_Eid { get; set; }
        public string EV_Refre_ConNo { get; set; }
        public string EV_Attch_Doc_Genuine { get; set; }
        public string EV_Any_Other_Cmmnts_EduV { get; set; }
        public DateTime? EV_Date_Time_Verf { get; set; }
        public DateTime? EV_Verb_Verf_Date { get; set; }
        public DateTime? EV_Wrtn_Verf_Date { get; set; }
        public string EV_Other_Remarks { get; set; }
        public string EV_Division_CGPA { get; set; }
        public string EV_Official_Fee_Proof { get; set; }
        public string EV_Colg_Unvrsty_Affl { get; set; }
        public string EV_Others { get; set; }
        public string EV_Others2 { get; set; }
        public string EV_Others3 { get; set; }
        public string EV_Others4 { get; set; }
        public string EV_Others5 { get; set; }
        public string EV_Others6 { get; set; }
        public string EV_Others7 { get; set; }
        public string EV_Others8 { get; set; }
        public string EV_Others9 { get; set; }
        public string EV_Others10 { get; set; }
        public string EV_Others11 { get; set; }
        public string EV_Others12 { get; set; }
        public string EV_Others13 { get; set; }
        public string EV_Others14 { get; set; }
        public string EV_Others15 { get; set; }
        public string ATA_Reverse_Directory { get; set; }
        public string ATA_Residcl_Commrcl { get; set; }
        public string ATA_Stock_Exchnge { get; set; }
        public string ATA_Telphne_Directry_Srch { get; set; }
        public string ATA_Yellow_Pages { get; set; }
        public string ATA_Who_Domain { get; set; }
        public string ATA_GoogleSearch { get; set; }
        public string ATA_InfrastructureOfCmp { get; set; }
        public string ATA_Just_Dial { get; set; }
        public string ATA_Neighbor_Check1 { get; set; }
        public string ATA_NASCOM_Empanelment { get; set; }
        public string ATA_Photograph { get; set; }
        public string ATA_Physical_Site { get; set; }
        public string ATA_Reg_Company { get; set; }
        public string ATA_PO_Courier_Check { get; set; }
        public string ATA_Network_Sol { get; set; }
        public string ATA_ApprxEmpWorking { get; set; }
        public string ATA_CID_No { get; set; }
        public string ATA_Cmpny_Addr { get; set; }
        public string ATA_Decoy_Call { get; set; }
        public string ATA_Cmpny_Website { get; set; }

        public string CheckStatus { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ReportComment { get; set; }
        public string checkOrdering { get; set; }
        public string VerifierName { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string city { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        #endregion

    }
    #endregion

    #region ******* Criminal *********
    public class CriminalReportViewModel
    {
        public int CriminalVerRowID { get; set; }
        public int CriminalRowID { get; set; }
        public short ClientRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short CheckFamilyRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }
        public string UniqueComponentID { get; set; }
        public string CRV_Sec_Ref_id { get; set; }
        public string VerifierName { get; set; }
        public string CRV_Cand_Name { get; set; }
        public DateTime? CRV_DOB { get; set; }
        public string CRV_Father_Name { get; set; }
        public string CRV_First_Name { get; set; }
        public string CRV_Last_Name { get; set; }
        public string CRV_Nationality { get; set; }
        public string CRV_Name_Chngd_Effect_Frm { get; set; }
        public string CRV_Nationality_On_Passport { get; set; }
        public string CRV_Country_of_Issue { get; set; }
        public string CRV_Addr_Type { get; set; }
        public string CRV_Current_Addr { get; set; }
        public string CRV_Prev_Addr1 { get; set; }
        public string CRV_Prev_Addr2 { get; set; }
        public string CRV_Prev_Addr3 { get; set; }
        public string CRV_Prev_Addr4 { get; set; }
        public string CRV_Permanent_Addr { get; set; }
        public string CRV_Gender { get; set; }
        public string CRV_Identity_No { get; set; }
        public string CRV_Identity_Prov { get; set; }
        public string CRV_Doc_Received { get; set; }
        public string CRV_Dur_Yr_Covered { get; set; }
        public string CRV_Pol_Station_Name { get; set; }
        public string CRV_Pol_Authority_Name { get; set; }
        public string CRV_Pol_Authority_Desg { get; set; }
        public string CRV_Criminal_Verf { get; set; }
        public string CRV_Type_of_Revert { get; set; }
        public DateTime? CRV_Date_Addr_Verif { get; set; }
        public DateTime? CRV_Date_Verf { get; set; }
        public DateTime? CRV_Time_Verf { get; set; }
        public string CRV_Verfier_Cmmnts { get; set; }
        public string CRV_St_Pol_Most_Wanted { get; set; }
        public string CRV_CBI_Most_Wanted { get; set; }
        public string CRV_Interpol_Most_Wanted { get; set; }
        public string CRV_Allied_Terrorist_Dataset { get; set; }
        public string CRV_Crimes_Dishonest_Dataset { get; set; }
        public string CRV_Global_Corruption_Dataset { get; set; }
        public string CRV_Global_Financ_Rgltry_Dataset { get; set; }
        public string CRV_Global_Fraud_Dataset { get; set; }
        public string CRV_Global_Law_Enforce_Dataset { get; set; }
        public string CRV_Sex_Crime_Dataset { get; set; }
        public string CRV_Violent_Crime_Dataset { get; set; }
        public string CRV_Global_Terrorist_Risk { get; set; }
        public string CRV_Global_Money_Laund { get; set; }
        public string CRV_Civil_Court_Action { get; set; }
        public string CRV_District_Court { get; set; }
        public string CRV_State_Court { get; set; }
        public string CRV_Civil_Court { get; set; }
        public string CRV_Special_Court { get; set; }
        public string CRV_Session_Court { get; set; }
        public string CRV_Tribunals { get; set; }
        public string CRV_Any_Other_Cmmnts_CRV { get; set; }
        public string CRV_Magistrate_Court { get; set; }
        public string CRV_District_Civil_Court { get; set; }
        public string CRV_St_High_Court { get; set; }
        public string CRV_Supreme_Court { get; set; }
        public string CRV_Criminal_Appeal { get; set; }
        public string CRV_Private_Complain_Rpt { get; set; }
        public string CRV_Others6 { get; set; }
        public string CRV_Others7 { get; set; }
        public string CRV_Others8 { get; set; }
        public string CRV_Others9 { get; set; }
        public string CRV_Others10 { get; set; }
        public string CRV_Others11 { get; set; }
        public string CRV_Others12 { get; set; }
        public string CRV_Others13 { get; set; }
        public string CRV_Others14 { get; set; }
        public string CRV_Others15 { get; set; }
        public string ATA_Reverse_Directory { get; set; }
        public string ATA_Residcl_Commrcl { get; set; }
        public string ATA_Stock_Exchnge { get; set; }
        public string ATA_Telphne_Directry_Srch { get; set; }
        public string ATA_Yellow_Pages { get; set; }
        public string ATA_Who_Domain { get; set; }
        public string ATA_GoogleSearch { get; set; }
        public string ATA_InfrastructureOfCmp { get; set; }
        public string ATA_Just_Dial { get; set; }
        public string ATA_Neighbor_Check1 { get; set; }
        public string ATA_NASCOM_Empanelment { get; set; }
        public string ATA_Photograph { get; set; }
        public string ATA_Physical_Site { get; set; }
        public string ATA_Reg_Company { get; set; }
        public string ATA_PO_Courier_Check { get; set; }
        public string ATA_Network_Sol { get; set; }
        public string ATA_ApprxEmpWorking { get; set; }
        public string ATA_CID_No { get; set; }
        public string ATA_Cmpny_Addr { get; set; }
        public string ATA_Decoy_Call { get; set; }
        public string ATA_Cmpny_Website { get; set; }

        public string CheckStatus { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ReportComment { get; set; }
        public string checkOrdering { get; set; }
        public string CRV_Period_Of_Stay { get; set; }
        public string AnnexureNo { get; set; }

    }
    #endregion

    #region ******* Reference *********
    public class ReferenceReportViewModel
    {
        public int ReferenceVerRowID { get; set; }
        public int ReferenceRowID { get; set; }
        public short ClientRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short CheckFamilyRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }
        public string UniqueComponentID { get; set; }
        public string RV_Sec_Ref_id { get; set; }

        public string RV_Cand_Name { get; set; }
        public string RV_Cmplnts_Result_Dsplnry_Sub { get; set; }
        public string RV_Reliable_Honest_Person { get; set; }
        public string RV_Cand_Fmly_Associat_Politics { get; set; }
        public string RV_Duration { get; set; }
        public string RV_Relation_Sub { get; set; }
        public string RV_Response_Frm_Rfre { get; set; }
        public string RV_CandidateName { get; set; }
        public string RV_OrganisationName { get; set; }
        public string RV_RefereeName { get; set; }
        public string RV_RefereeDesignation { get; set; }
        public string RV_Refre_Prsnt_Emp { get; set; }
        public string RV_Refre_ConDet { get; set; }
        public string RV_Refere_Desig { get; set; }
        public string RV_Refere_Add { get; set; }
        public string RV_Refre_MobNo { get; set; }
        public string RV_Refre_Eid { get; set; }
        public string RV_Prof_Relationship_Cand_Dur { get; set; }
        public string RV_Know_Long { get; set; }
        public string RV_Tenure_Emp { get; set; }
        public string RV_Know_Subject { get; set; }
        public string RV_Was_Cand_Honest { get; set; }
        public string RV_Rate_Prof_Ability { get; set; }
        public string RV_Commun_Jr_Sr_Colleague { get; set; }
        public string RV_Relationship_With_Sub { get; set; }
        public string RV_Overall_Jb_Perf { get; set; }
        public string RV_Response_Wrtn_Verb { get; set; }
        public string RV_State_Specify_Medcl_Prblm { get; set; }
        public string RV_Sate_Specify_Per_Prblm_Perf { get; set; }
        public string RV_Sub_Perf_Intgrty_Wrk { get; set; }
        public string RV_Particular_Gud_Least_Effect { get; set; }
        public string RV_Comment_Per_Qualities { get; set; }
        public string RV_Aware_Son_Daughtr_Appld_Jb { get; set; }
        public string RV_Character { get; set; }
        public string RV_Anger_Mngmt { get; set; }
        public string RV_Behaviour { get; set; }
        public string RV_Intgrty_Commitmnt { get; set; }
        public string RV_Nature { get; set; }
        public string RV_Perf_level { get; set; }
        public string RV_SoftSkill { get; set; }
        public string RV_Action_Orient { get; set; }
        public string RV_Algmnt_Skills { get; set; }
        public string RV_Anger_Mgmt_Skills { get; set; }
        public string RV_Reliability { get; set; }
        public string RV_Aptitutde { get; set; }
        public string RV_Influence_Style { get; set; }
        public string RV_Initiative { get; set; }
        public string RV_Interper_Skills { get; set; }
        public string RV_Comm_Skills { get; set; }
        public string RV_Energy { get; set; }
        public string RV_Performance { get; set; }
        public string RV_Team_Mgmt_Skills { get; set; }
        public string RV_Teamng_Skills { get; set; }
        public string RV_Tech_Competence { get; set; }
        public string RV_Time_Mgmt_Org_Skills { get; set; }
        public string RV_Value_System { get; set; }
        public string RV_Voice_Accent { get; set; }
        public string RV_Plan_Fr_Higher_Studies { get; set; }
        public string RV_What_kind_of_Jb { get; set; }
        public string RV_Family_Background { get; set; }
        public string RV_His_Behaviour { get; set; }
        public string RV_His_Conduct { get; set; }
        public string RV_His_Perf { get; set; }
        public string RV_Recommd_Work_MNC { get; set; }
        public string RV_Manag_Analyz_MIS_Plng { get; set; }
        public string RV_Mngng_Int_Ext_Cust_Expect { get; set; }
        public string RV_Additional_Cmmnts_RC { get; set; }
        public string RV_Cmplnts_Rslt_Dsplnry_Sub { get; set; }
        public string RV_Achivement_During_Tenure { get; set; }
        public string RV_Other_cmmnts_RC { get; set; }
        public string RV_Other_Qualitative_Input { get; set; }
        public string RV_Qualitative_Input_Plng { get; set; }
        public string RV_Cand_Record_Misreprsnt_Facts { get; set; }
        public string RV_Area_Development { get; set; }
        public string RV_Attendance_Punct_RC { get; set; }
        public string RV_Cand_Attitude_Task_Hndld { get; set; }
        public string RV_Comm_Skills_RC { get; set; }
        public string RV_Contribution_Intro_Concept { get; set; }
        public string RV_Dsg_Refre { get; set; }
        public string RV_Dsg_RC { get; set; }
        public string RV_CTC { get; set; }
        public string RV_Reason_Leav { get; set; }
        public string RV_Relationship_With_Sub_Dur { get; set; }
        public string RV_Sub_Elgblty_Rhre { get; set; }
        public string RV_Sub_Meet_Pos_Perf_Req { get; set; }
        public string RV_Recall_Edu_Credential { get; set; }
        public string RV_Recall_Emp_Back { get; set; }
        public string RV_Do_you_trust { get; set; }
        public string RV_Effect_Settng_Meetng_Goals { get; set; }
        public string RV_Cand_Interpersonal_Skills { get; set; }
        public string RV_Cand_Mngmt_Skills { get; set; }
        public string RV_Acquainted_Cand_how_long { get; set; }
        public string RV_Diligent_Twds_Work { get; set; }
        public string RV_How_Discipline { get; set; }
        public string RV_How_Ppl_Respond { get; set; }
        public string RV_Rate_Perf_Ability { get; set; }
        public string RV_Respond_Work_Pressure { get; set; }
        public string RV_Cand_Mtng_Target_Dline { get; set; }
        public string RV_As_Team_Player { get; set; }
        public string RV_Atude_Twds_Work { get; set; }
        public string RV_Was_Cand_Motivated { get; set; }
        public string RV_How_Was_Behaviour { get; set; }
        public string RV_Perform_Task_Time_Cost { get; set; }
        public string RV_Rate_Drive_Fr_Result { get; set; }
        public string RV_Rate_Resilience_Perserve { get; set; }
        public string RV_Term_Resource_Discipline { get; set; }
        public string RV_Rhre_Cmpny_Again { get; set; }
        public string RV_Is_Reliable_Hon_Person { get; set; }
        public string RV_Cand_High_Intgrty { get; set; }
        public string RV_Cand_Sincere_Hon { get; set; }
        public string RV_Knowlge_Pos_Responsiblts { get; set; }
        public string RV_Major_Carrer_Changes_Sub { get; set; }
        public string RV_Mngng_Mtng_Int_Cust_Expect { get; set; }
        public string RV_Mgng_Complexity { get; set; }
        public string RV_Meeting_Deadlines { get; set; }
        public string RV_Mode_Of_Exit { get; set; }
        public string RV_Moral_Values { get; set; }
        public string RV_Oral_Comm_Skills { get; set; }
        public string RV_Commnt_Ldrshp_Abilities { get; set; }
        public string RV_Specify_Cand_Area_Improve { get; set; }
        public string RV_Specify_Cand_Strength { get; set; }
        public string RV_Prblm_Solving_Ablts { get; set; }
        public string RV_Prof_Conduct { get; set; }
        public string RV_Prof_Strength { get; set; }
        public string RV_Prof_Values { get; set; }
        public string RV_Quality_Of_Work { get; set; }
        public string RV_Rating_On_Scale { get; set; }
        public string RV_Rating_Que_1 { get; set; }
        public string RV_Rating_Que_2 { get; set; }
        public string RV_Rating_Que_3 { get; set; }
        public string RV_Rating_Que_4 { get; set; }
        public string RV_Recommd_Similar_Pos { get; set; }
        public string RV_Reslt_Orient { get; set; }
        public string RV_Role_Responsibilities_Cand { get; set; }
        public string RV_Stress_Mgmt { get; set; }
        public string RV_Sub_Perf_Intgrty_Wrk_RC { get; set; }
        public string RV_Suff_Notice_Period_Prov { get; set; }
        public string RV_Team_Conduct { get; set; }
        public string RV_Team_Ldrshp { get; set; }
        public string RV_Team_Mgmt { get; set; }
        public string RV_Was_Sub_Team_Player { get; set; }
        public string RV_Was_Cand_Involve_Union_Act { get; set; }
        public string RV_Weakness { get; set; }
        public string RV_Strength_Area_Developmnt { get; set; }
        public string RV_Cand_Edu_Prof_Qualification { get; set; }
        public string RV_Sub_Area_Develop_Weak { get; set; }
        public string RV_Say_Abt_Enterpren_Skills { get; set; }
        public string RV_Particiular_Gud_Least_Effect { get; set; }
        public string RV_Pos_Cand_Working { get; set; }
        public string RV_TimePeriod_Cand_Work_Under { get; set; }
        public string RV_Work_Ethnics_Moral_Intgrty { get; set; }
        public string RV_Commnt_Per_Abilities { get; set; }
        public string RV_Wrtn_Comm_Skills_RC { get; set; }
        public string RV_AnyIssues { get; set; }
        public string RV_Sub_Meet_Pos_Perf_Req_RC { get; set; }
        public string RV_Sub_Comm_Interper_Skills { get; set; }
        public string RV_Sub_Effect_Gettng_Rslt { get; set; }
        public string RV_Intgrty_Commitmnt_RC { get; set; }
        public string RV_Is_Reliable_Hon_Person_RC { get; set; }
        public string RV_Nature_Job { get; set; }
        public string RV_Overall_Jb_Perf_RC { get; set; }
        public string RV_Social_Conduct { get; set; }
        public string RV_Social_Conduct_RC { get; set; }
        public string RV_Sub_Per_Prof_Weak { get; set; }
        public string RV_Work_Ethics_Moral_Intgrty_Rc { get; set; }
        public string RV_Recommnd_Sub_Emp_Top_MNC { get; set; }
        public string RV_Cmmnts_Sub_Strngth_Weak { get; set; }
        public string RV_Cmplnts_Rslt_Dsplnry_Sub_RC { get; set; }
        public string RV_Contribution_Intro_Concept_RC { get; set; }
        public string RV_Sub_Pos_Perf_Req_RC { get; set; }
        public string RV_Confrontation_Tackled { get; set; }
        public string RV_Pressure_Sit_Hndld { get; set; }
        public string RV_React_To_Change { get; set; }
        public string RV_Enforcing_Gud_Qualt_Work_Jrs { get; set; }
        public string RV_Sub_Team_Leader { get; set; }
        public string RV_Any_Other_Cmmnts_Fdback { get; set; }
        public string RV_Exp_During_Financ_Dealing { get; set; }
        public string RV_Rel_Busines_Entity { get; set; }
        public string RV_Dur_Busines_Rel_Cmpny { get; set; }
        public string RV_OtherDetails6 { get; set; }
        public string RV_OtherDetails7 { get; set; }
        public string RV_OtherDetails8 { get; set; }
        public string RV_OtherDetails9 { get; set; }
        public string RV_OtherDetails10 { get; set; }
        public string RV_OtherDetails11 { get; set; }
        public string RV_OtherDetails12 { get; set; }
        public string RV_OtherDetails13 { get; set; }
        public string RV_OtherDetails14 { get; set; }
        public string RV_OtherDetails15 { get; set; }
        public string ATA_Reverse_Directory { get; set; }
        public string ATA_Residcl_Commrcl { get; set; }
        public string ATA_Stock_Exchnge { get; set; }
        public string ATA_Telphne_Directry_Srch { get; set; }
        public string ATA_Yellow_Pages { get; set; }
        public string ATA_Who_Domain { get; set; }
        public string ATA_GoogleSearch { get; set; }
        public string ATA_InfrastructureOfCmp { get; set; }
        public string ATA_Just_Dial { get; set; }
        public string ATA_Neighbor_Check1 { get; set; }
        public string ATA_NASCOM_Empanelment { get; set; }
        public string ATA_Photograph { get; set; }
        public string ATA_Physical_Site { get; set; }
        public string ATA_Reg_Company { get; set; }
        public string ATA_PO_Courier_Check { get; set; }
        public string ATA_Network_Sol { get; set; }
        public string ATA_ApprxEmpWorking { get; set; }
        public string ATA_CID_No { get; set; }
        public string ATA_Cmpny_Addr { get; set; }
        public string ATA_Decoy_Call { get; set; }
        public string ATA_Cmpny_Website { get; set; }
        public string RV_OtherDetails2 { get; set; }
        public string CheckStatus { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ReportComment { get; set; }
        public string checkOrdering { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string AnnexureNo { get; set; }
    }
    #endregion

    #region ******* NationalIdentity *********
    public class NationalIdentityReportViewModel
    {
        public int NationalIdentityVerRowID { get; set; }
        public int NationalIdentityRowID { get; set; }
        public short ClientRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short CheckFamilyRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }
        public string UniqueComponentID { get; set; }
        public string NIC_Sec_Ref_id { get; set; }

        public string NIC_Cand_Name { get; set; }
        public string NIC_Any_Other_Cmmnts_NIC { get; set; }
        public DateTime? NIC_DOB1 { get; set; }
        public string NIC_ID_No { get; set; }
        public string NIC_Verified_By { get; set; }
        public string NIC_Date_Issue { get; set; }
        public string NIC_Expiry_Date { get; set; }
        public string NIC_Validity_Status { get; set; }

        public string NIC_Passport_No { get; set; }
        public string NIC_SSN_NO { get; set; }
        public string NIC_Pan_No { get; set; }
        public string NIC_Voter_Id_No { get; set; }
        public string NIC_Dvng_Liscense_No { get; set; }
        public string NIC_Aadhar_Card_No { get; set; }
        public string NIC_Place_Issue { get; set; }
        public string NIC_Place_Residnce { get; set; }
        public string NIC_Time_Verf { get; set; }
        public string NIC_Type_Id_Card { get; set; }
        public string NIC_Reg_Doc_Type { get; set; }
        public string NIC_RegNo { get; set; }
        public string NIC_Name_Field_Executive { get; set; }
        public string NIC_PinCode { get; set; }
        public string NIC_Relation_Key_Personal { get; set; }
        public string NIC_Verified_Frm { get; set; }
        public string NIC_Auth_Details_Name { get; set; }
        public string NIC_Dsg { get; set; }
        public string NIC_Eid { get; set; }
        public string NIC_MobNo { get; set; }
        public string NIC_Name_Key_Personnel { get; set; }
        public string NIC_AliasFname { get; set; }
        public string NIC_Alias_Lname { get; set; }
        public string NIC_Country_Iss_Passport { get; set; }
        public DateTime? NIC_Date_Verif { get; set; }
        public string NIC_Id_CardNo { get; set; }
        public string NIC_Identity_Prov { get; set; }
        public string NIC_Name_Changed_Effect_Frm { get; set; }
        public string NIC_Nationality { get; set; }
        public string NIC_PhotoID_Verif { get; set; }
        public string NIC_Any_Other_Remarks_NIC { get; set; }
        public string NIC_OtherDetails6 { get; set; }
        public string NIC_OtherDetails7 { get; set; }
        public string NIC_OtherDetails8 { get; set; }
        public string NIC_OtherDetails9 { get; set; }
        public string NIC_OtherDetails10 { get; set; }
        public string NIC_OtherDetails11 { get; set; }
        public string NIC_OtherDetails12 { get; set; }
        public string NIC_OtherDetails13 { get; set; }
        public string NIC_OtherDetails14 { get; set; }
        public string NIC_OtherDetails15 { get; set; }
        public string ATA_Reverse_Directory { get; set; }
        public string ATA_Residcl_Commrcl { get; set; }
        public string ATA_Stock_Exchnge { get; set; }
        public string ATA_Telphne_Directry_Srch { get; set; }
        public string ATA_Yellow_Pages { get; set; }
        public string ATA_Who_Domain { get; set; }
        public string ATA_GoogleSearch { get; set; }
        public string ATA_InfrastructureOfCmp { get; set; }
        public string ATA_Just_Dial { get; set; }
        public string ATA_Neighbor_Check1 { get; set; }
        public string ATA_NASCOM_Empanelment { get; set; }
        public string ATA_Photograph { get; set; }
        public string ATA_Physical_Site { get; set; }
        public string ATA_Reg_Company { get; set; }
        public string ATA_PO_Courier_Check { get; set; }
        public string ATA_Network_Sol { get; set; }

        public string ATA_ApprxEmpWorking { get; set; }
        public string ATA_CID_No { get; set; }
        public string ATA_Cmpny_Addr { get; set; }
        public string ATA_Decoy_Call { get; set; }
        public string ATA_Cmpny_Website { get; set; }

        public string CheckStatus { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ReportComment { get; set; }
        public string checkOrdering { get; set; }
        public string AnnexureNo { get; set; }
        public DateTime? VerifiedDate { get; set; }
    }
    #endregion

    #region ******* SpecialCheck *********
    public class SpecialCheckReportViewModel
    {
        public int SpecialCheckVerRowID { get; set; }
        public int SpecialCheckRowID { get; set; }
        public short ClientRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short CheckFamilyRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }
        public string UniqueComponentID { get; set; }

        public string SC_Cand_Name { get; set; }
        public string SC_Father_Name { get; set; }
        public string SC_SecuritasID { get; set; }
        public DateTime? SC_DOB { get; set; }

        //Following not show on page. It is for future use only
        public string SC_Others1 { get; set; }
        public string SC_Others2 { get; set; }
        public string SC_Others3 { get; set; }
        public string SC_Others4 { get; set; }
        public string SC_Others5 { get; set; }

        public string CheckStatus { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string ReportComment { get; set; }
        public string checkOrdering { get; set; }
        public string AnnexureNo { get; set; }
        public string SC_Mode_VerOther { get; set; }
        public string SC_AnyOtherComments { get; set; }
        public DateTime ? SC_Date_Verify { get; set; }
        public string SC_Mode_Verify { get; set; }
    }
    #endregion

    public class colorCodeViewModel
    {
        public string  ColorId { get; set; }
        public string ColorName { get; set; }
    }


    public class ArtifactDocumentViewModel
    {
        public int DocumentId { get; set; }
        public string DocumentType { get; set; }
    }

    public class colorCodeOrderingViewModel
    {
        public int PersonalRowId { get; set; }
        public int SubCheckFamilyID { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public string checkOrdering { get; set; }
        public string checkstatus { get; set; }
        public string subcheckSequence { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string reportComment { get; set; }
    }
    public class PQVerifiedUploadPDFImagesViewModel
    {
        public IEnumerable<PQVerifiedUploadPDFImages> PQVerifiedUploadPDFImages { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int rowNum { get; set; }
    }


    public class PQVerifiedUploadPDFImages
    {
        public int VerifiedUploadRowID { get; set; }
        public string ImageFile { get; set; }
        public bool ActiveStatus { get; set; }
        public HttpPostedFileBase UploadDoc { get; set; }
    }

    public class TCS_BPO_Cal_Sheet
    {
        public string SubCheckName { get; set; }
        public double Charge { get; set; }
        public string ModeOfVerification { get; set; }
        public string CheckStatus { get; set; }

    }

    public class CaseAntecedentComment
    {
        public int PersonalRowID { get; set; }
        public int SubCheckFamilyID { get; set; }
        public string AntecedentName { get; set; }
        public string Comment { get; set; }
        public string CompanyRefNo { get; set; }
        public int CheckFamilyRowID { get; set; }

    }

    public class CaseAntecedentCommentInfoListPagedModel
    {
        public CaseAntecedentComment CaseAntecedentComment { get; set; }
        public IEnumerable<CaseAntecedentComment> CaseAntecedentCommentList { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class WiproAnnexure
    {
        public int PersonalRowID { get; set; }
        public int SubCheckRowID { get; set; }
        public string AnnexureNo { get; set; }
        public string AnnexureSrNo { get; set; }

    }

    public class AddFordRemark
    {
        public string ActionRemark { get; set; }
        public string ClientRowID { get; set; }
        public string PersonalRowID { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class AddFordRemarkInfoListPagedModel
    {
        public AddFordRemark AddFordRemark { get; set; }
        public IEnumerable<AddFordRemark> AddFordRemarkList { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class DrugTestResult
    {

        public int PersonalRowid { get; set; }
        public string PanelName { get; set; }
        public string TestName { get; set; }
        public string TestResult { get; set; }
        public string TestValue { get; set; }
    }

    public class WiproUANCheck
    {
        public string PersonalrowId { get; set; }
        public string Employment { get; set; }
        public string UAN_DOJ { get; set; }
        public string UAN_LWD { get; set; }
        public string HR_DOJ { get; set; }
        public string HR_LWD { get; set; }
        public string IVerify_DOJ { get; set; }
        public string IVerify_LWD { get; set; }
        public string iVerify_Y_N { get; set; }
    }

    public class PersonalDocDetails
    {
        public string PersonalRowID { get; set; }
        public string FileName { get; set; }
        public string Remarks { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandidateFullName { get; set; }

    }
    public class InstantCheckResultStatus
    {
        public string PersonalRowID { get; set; }
        public string FileName { get; set; }        
        public string CompanyRefNo { get; set; }
        public string UAN { get; set; }
        public string IdNumber { get; set; }
        public string Action { get; set; }
    }


    public class InstantIDCheckResultStatus
    {
        public string PersonalRowID { get; set; }
        public string FileName { get; set; }
        public string CompanyRefNo { get; set; }      
        public string IdNumber { get; set; }
        public string SubCheckName { get; set; }
        public string IDType { get; set; }
        public string Action { get; set; }
    }
    public class WiproGapChecks
    {
        public string PersonalrowId { get; set; }
        public string GapType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string GapTo { get; set; }
        public string GapFrom { get; set; }
        public string Remarks { get; set; }
    }

    public class LTIUANReportDetail
    {
        public int PersonalRowID { get; set; }
        public int subcheckrowid { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandidateName { get; set; }
        public string OrganisationName { get; set; }
        public string EmployeeCode { get; set; }
        public string UANNumber { get; set; }
        public string DOJ { get; set; }
        public string DOL { get; set; }
        public string ModeOfVerification { get; set; }
        public string DateOfVerification { get; set; }
        public string ColorName { get; set; }
        public string ClientColorCode { get; set; }


    }
    public class BackgroundCheck
    {
        public string TypeOfCheck { get; set; }
        public string VerificationStatus { get; set; }
        public string BgcStatus { get; set; }
        public int PersonalRowID { get; set; }

        public int SubCheckID { get; set; }
    }

    public class CriminalCourtVerificationVM
    {
        public int PersonalRowID { get; set; }
        public int SubCheckRowID { get; set; }
        public string TypeOfCheck { get; set; }
        public string CourtType { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class ItemViewModel
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
    }

    public class MasterArtifactMappingsOrdering
    {
        public int DocumentSeqNo { get; set; }
        public string DocumentType { get; set; }
        public int DocumentTypeId { get; set; }
        public int CheckFamilyRowID { get; set; }
    }

    public class TechMRemarkModel
    {
        public string Remarks { get; set; }
        public int SubCheckRowID { get; set; }
        public string CheckStatus { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    public class CVVerificationDetailModel
    {
        public int PersonalRowID { get; set; }
        public int ClientRowID { get; set; }
        public int SubCheckID { get; set; }
        public string DataField { get; set; }
        public string ApplicantSubmittedDetails { get; set; }
        public string ResumeCVDetails { get; set; }
    }

}
