using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class CheckCaseDetails
    {
        public string UniqueReferenceID { get; set; }
        public string Status { get; set; }
        public string DataType { get; set; }
        public string DataValue { get; set; }
        public string Remarks { get; set; }
        public string CreatedOn { get; set; }
        public string SubCheckRowID { get; set; }
        public string CreatedBy { get; set; }
        public List<CheckCaseDetails> ShowallCheckCaseDetails { get; set; }

    }

    public class ModelLTI
    {
        public class Token
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string userName { get; set; }
            public string issued { get; set; }
            public string expires { get; set; }
            public string error { get; set; }
            public string error_description { get; set; }
        }

        public class CandidateList
        {
            public string CandidateID { get; set; }
            public string PSNO { get; set; }
            public string Name { get; set; }
            public string RequestID { get; set; }
            public string RequestType { get; set; }
            public string VendorID { get; set; }
            public string FastTrackStatus { get; set; }
        }


        public class UpdateCandidateChecksStatus
        {
            public string CandidateID { get; set; }
            public string PSNO { get; set; }
            public string Name { get; set; }
            public string RequestID { get; set; }
            public string VendorName { get; set; }
            public List<LiChecksDetail> liChecksDetails { get; set; }
            public List<LiReportDetail> liReportDetails { get; set; }
        }

        public class LiChecksDetail
        {
            public string Check_Unique_ID { get; set; }
            public string CheckCode { get; set; }
            public string CheckName { get; set; }
            public string CheckStatus { get; set; }
            public string CheckRemarks { get; set; }
            public string ModeOfVerficationRequired { get; set; }
            public string ModeOfVerficationPerformed { get; set; }
            public string CompletedDate { get; set; }
        }

        public class LiReportDetail
        {
            public string ReportFileName { get; set; }
            public string ReportFileExtention { get; set; }
            public string ReportAttachment { get; set; }
            public string VendorReferenceID { get; set; }
            public string ReportType { get; set; }
            public string ReportStatus { get; set; }
        }

    }
    public class LiCandidateAddressInfo
    {
        public string AddressType { get; set; }
        public string HouseNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string ProminentLandmark { get; set; }
        public string StayFromDate { get; set; }
        public string StayToDate { get; set; }
        public string HouseType { get; set; }
        public string ContactInfo { get; set; }
        public string Insufficiency_Remarks { get; set; }
    }

    public class MasterSubCheckFamilies
    {
        public int SubCheckRowID { get; set; }
        public int CheckCode { get; set; }
        public string SubCheckName { get; set; }
    }

    public class LiCandidateBasicInfo
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Birthplace { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailID { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string FatherName { get; set; }
        public string FatherDOB { get; set; }
        public string Insufficiency_Remarks { get; set; }
    }

    public class LiCandidateDocumentInfo
    {
        public string DocumentName { get; set; }
        public string FileType { get; set; }
        public string DocumentAttachment { get; set; }
    }

    public class LiCandidateEducationInfo
    {
        public string DegreeType { get; set; }
        public string CollegeName { get; set; }
        public string UniversityName { get; set; }
        public string MonthYearOfPassing { get; set; }
        public string EducationType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Insufficiency_Remarks { get; set; }
        public string EducationDocuments { get; set; }

    }

    public class LiCandidateEmploymentInfo
    {
        public string EmploymentType { get; set; }
        public string CompanyName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Duration { get; set; }
        public string Designation { get; set; }
        public string EmployeeCode { get; set; }
        public string HRName { get; set; }
        public string HRContactNumber { get; set; }
        public string HREmailID { get; set; }
        public string SuperiorName { get; set; }
        public string SuperiorContactNumber { get; set; }
        public string SuperiorEmailID { get; set; }
        public string SuperiorDesignation { get; set; }
        public string LastSalary { get; set; }
        public string GrossSalary { get; set; }
        public string Insufficiency_Remarks { get; set; }
        public string EmploymentDocuments { get; set; }

    }

    public class LiCandidateInformation
    {
        public List<LiCandidateBasicInfo> liCandidateBasicInfo { get; set; }
        public List<LiCandidateAddressInfo> liCandidateAddressInfo { get; set; }
        public List<LiCandidateEducationInfo> liCandidateEducationInfo { get; set; }
        public List<LiCandidateEmploymentInfo> liCandidateEmploymentInfo { get; set; }
        public object liCandidateReferenceInfo { get; set; }
        public object liCandidateDrugInfo { get; set; }
        public string PanCardDocuments { get; set; }
        public string AadharCardDocuments { get; set; }
        public string PassportDocuments { get; set; }
        public string DrivingLicenceDocuments { get; set; }
        public string CVDocuments { get; set; }
        public object GapAnalysisDocuments { get; set; }
        public string LOADocuments { get; set; }
        public List<LiCandidateDocumentInfo> liCandidateDocumentInfo { get; set; }
    }

    public class LiChecksRequired
    {
        public string Check_Unique_ID { get; set; }
        public string CheckCode { get; set; }
        public string CheckName { get; set; }
        public string CheckStatus { get; set; }
        public string CheckRemarks { get; set; }
        public string ModeOfVerficationRequired { get; set; }
        public string ModeOfVerficationPerformed { get; set; }
        public string DateToComplete { get; set; }
        public string CompletedDate { get; set; }
    }
    public class Licheckstoperform
    {
        public Lichecksrequired[] liChecksRequired { get; set; }
        public Lireportformat[] liReportFormat { get; set; }
    }
    public class Lichecksrequired
    {
        public string Check_Unique_ID { get; set; }
        public string CheckCode { get; set; }
        public string CheckName { get; set; }
        public string CheckStatus { get; set; }
        public string CheckRemarks { get; set; }
        public string ModeOfVerficationRequired { get; set; }
        public string ModeOfVerficationPerformed { get; set; }
        public string DateToComplete { get; set; }
        public string CompletedDate { get; set; }
    }

    public class Lireportformat
    {
        public string ReportFileName { get; set; }
        public string ReportFileExtention { get; set; }
        public string ReportAttachment { get; set; }
        public string VendorReferenceID { get; set; }
        public string ReportType { get; set; }
        public string ReportStatus { get; set; }
    }


    public class LiChecksToPerform
    {
        public List<LiChecksRequired> liChecksRequired { get; set; }
        public List<LiReportFormat> liReportFormat { get; set; }
    }

    public class Listopcheck
    {
        public string Check_Unique_ID { get; set; }
        public string CheckCode { get; set; }
        public string CheckName { get; set; }
        public string CheckStatus { get; set; }
        public string CheckRemarks { get; set; }
        public string ModeOfVerficationRequired { get; set; }
        public string ModeOfVerficationPerformed { get; set; }
        public string DateToComplete { get; set; }
        public string CompletedDate { get; set; }
    }

    public class LiReportFormat
    {
        public string ReportFileName { get; set; }
        public string ReportFileExtention { get; set; }
        public string ReportAttachment { get; set; }
        public string VendorReferenceID { get; set; }
        public string ReportType { get; set; }
        public string ReportStatus { get; set; }
    }
    public class CandidateDetails
    {
        public string CandidateID { get; set; }
        public string PSNO { get; set; }
        public string Name { get; set; }
        public string RequestID { get; set; }
        public string VendorName { get; set; }
        public string FastTrackStatus { get; set; }
        public Licandidateinformation liCandidateInformation { get; set; }
        public LiChecksToPerform liChecksToPerform { get; set; }
        public Listopchecks liStopChecks { get; set; }

    }

    public class Licandidateinformation
    {
        public LiCandidateBasicInfo[] liCandidateBasicInfo { get; set; }
        public Licandidateaddressinfo[] liCandidateAddressInfo { get; set; }
        public Licandidateeducationinfo[] liCandidateEducationInfo { get; set; }
        public Licandidateemploymentinfo[] liCandidateEmploymentInfo { get; set; }
        public Licandidatereferenceinfo[] liCandidateReferenceInfo { get; set; }
        public Licandidatedruginfo[] liCandidateDrugInfo { get; set; }
        public Licandidatepaninfo[] liCandidatePANInfo { get; set; }
        public Licandidatepassportinfo[] liCandidatePassportInfo { get; set; }
        public Licandidatevoteridinfo[] liCandidateVoterIDInfo { get; set; }
        public Licandidatedlinfo[] liCandidateDLInfo { get; set; }
        public Licandidateaadhaarinfo[] liCandidateAadhaarInfo { get; set; }
        public string PanCardDocuments { get; set; }
        public string AadharCardDocuments { get; set; }
        public string PassportDocuments { get; set; }
        public string DrivingLicenceDocuments { get; set; }
        public string CVDocuments { get; set; }
        public object GapAnalysisDocuments { get; set; }
        public string LOADocuments { get; set; }
    }
    public class Licandidateaddressinfo
    {
        public string AddressType { get; set; }
        public string HouseNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string ProminentLandmark { get; set; }
        public string StayFromDate { get; set; }
        public string StayToDate { get; set; }
        public string HouseType { get; set; }
        public string ContactInfo { get; set; }
        public string Insufficiency_Remarks { get; set; }
    }

    public class Licandidateeducationinfo
    {
        public string DegreeType { get; set; }
        public string QualificationType { get; set; }
        public string CollegeName { get; set; }
        public string UniversityName { get; set; }
        public string MonthYearOfPassing { get; set; }
        public string EducationType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Country { get; set; }
        public string Insufficiency_Remarks { get; set; }
        public string EducationDocuments { get; set; }
    }

    public class Licandidateemploymentinfo
    {
        public string EmploymentType { get; set; }
        public string CompanyName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Duration { get; set; }
        public string Designation { get; set; }
        public string EmployeeCode { get; set; }
        public string HRName { get; set; }
        public string HRContactNumber { get; set; }
        public string HREmailID { get; set; }
        public string SuperiorName { get; set; }
        public string SuperiorContactNumber { get; set; }
        public string SuperiorEmailID { get; set; }
        public string SuperiorDesignation { get; set; }
        public string LastSalary { get; set; }
        public string GrossSalary { get; set; }
        public string Country { get; set; }
        public string Insufficiency_Remarks { get; set; }
        public string EmploymentDocuments { get; set; }
    }

    public class Licandidatereferenceinfo
    {
        public string ReferenceNumber { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string ContactNumber { get; set; }
        public string DurationKnown { get; set; }
        public string Designation { get; set; }
        public string EmailID { get; set; }
        public string ProfessionalRelation { get; set; }
        public string Insufficiency_Remarks { get; set; }
    }

    public class Licandidatedruginfo
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string SampleCollectionDate { get; set; }
        public string Remarks { get; set; }
        public string HouseNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string ProminentLandmark { get; set; }
    }

    public class Licandidatepaninfo
    {
        public string PANNumber { get; set; }
        public string NameInPAN { get; set; }
        public string PanDocument { get; set; }
    }

    public class Licandidatepassportinfo
    {
        public string PassportNumber { get; set; }
        public string NameInPassport { get; set; }
        public string DOBInPassport { get; set; }
        public string Country { get; set; }
        public string DateOfIssue { get; set; }
        public string ExpiryDate { get; set; }
        public string PassportDocument { get; set; }
    }

    public class Licandidatevoteridinfo
    {
        public string VoterIDNumber { get; set; }
        public string DOBInVoterID { get; set; }
        public string VoterIDDocument { get; set; }
    }

    public class Licandidatedlinfo
    {
        public string DLNumber { get; set; }
        public string DateOfIssue { get; set; }
        public string DLDocument { get; set; }
    }

    public class Licandidateaadhaarinfo
    {
        public string AadhaarNumber { get; set; }
        public string NameInAadhaar { get; set; }
        public string DOBInAadhaar { get; set; }
        public string AadhaarDocument { get; set; }
    }

    public class Rootobject2
    {
        public string CandidateID { get; set; }
        public string PSNO { get; set; }
        public string Name { get; set; }
        public string RequestID { get; set; }
        public string VendorName { get; set; }
        public object FastTrackStatus { get; set; }
        public object liCandidateInformation { get; set; }
        public object liChecksToPerform { get; set; }
        public Listopchecks liStopChecks { get; set; }
    }
    //CandidateDetails objCandidate = JsonConvert.DeserializeObject<CandidateDetails>(myJsonResponse);

    public class Listopchecks
    {
        public Licheckstostop[] liChecksToStop { get; set; }
    }

    public class Licheckstostop
    {
        public string Check_Unique_ID { get; set; }
        public string CheckCode { get; set; }
        public string CheckName { get; set; }
        public string CheckStatus { get; set; }
        public string CheckRemarks { get; set; }
        public object DatetoComplete { get; set; }
        public string ModeOfVerficationRequired { get; set; }
        public string ModeOfVerficationPerformed { get; set; }
        public string CompletedDate { get; set; }
    }
    public class CaseInitiatedAcknowledgement
    {
        public string RequestID { get; set; }
        public string CandidateID { get; set; }
        public string PSNO { get; set; }
        public string VendorID { get; set; }
        public string VendorReferenceID { get; set; }
    }
    public class ComapnyRefdata
    {
        public string ComapnyRefID { get; set; }
    }
    public class InsuffCheckClearViewModel
    {
        public int PersonalRowID { get; set; }
        public int CheckRowID { get; set; }
        public int ClientRowID { get; set; }
        public int SubCheckRowID { get; set; }
        public string DocumentUploadFrom { get; set; }   //Provided / Verified    
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }


    }

    public class CompanyMailAuthenticationViewModel
    {
        public short CompanyRowID { get; set; }
        public string SMTPServer { get; set; }
        public string Port { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public bool EnableSsl { get; set; }
    }

    public class CompanyMailSignatureViewModel
    {
        public short CompanyRowID { get; set; }
        public string CompanyName { get; set; }
        public string GeneralEmail { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
    }
    public class AddPQVerifiedUploadCaseDocViewModel2
    {
        public int PersonalRowID { get; set; }
        public string DocumentUploadFrom { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
    }
    public class ChackNames
    {
        public int Id { get; set; }
        public string CheckName { get; set; }
    }
    public class APICaseDetails
    {
        public string UniqueReferenceID { get; set; }
        public string Antecedent { get; set; }
        public string AntecedentValue { get; set; }
        public string Remarks { get; set; }
    }
    #region Get GST no
    public class GstRootObject
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public int reference_id { get; set; }
        public GstData data { get; set; }
    }

    public class GstData
    {
        public string client_id { get; set; }
        public string gstin { get; set; }
        public string pan_number { get; set; }
        public string business_name { get; set; }
        public string legal_name { get; set; }
        public string center_jurisdiction { get; set; }
        public string state_jurisdiction { get; set; }
        public string date_of_registration { get; set; }
        public string constitution_of_business { get; set; }
        public string taxpayer_type { get; set; }
        public string gstin_status { get; set; }
        public string date_of_cancellation { get; set; }
        public string field_visit_conducted { get; set; }
        public string[] nature_bus_activities { get; set; }
        public string nature_of_core_business_activity_code { get; set; }
        public string nature_of_core_business_activity_description { get; set; }
        public string aadhaar_validation { get; set; }
        public string aadhaar_validation_date { get; set; }
        public object[] filing_status { get; set; }
        public string address { get; set; }
        public object[] hsn_info { get; set; }
        public object[] filing_frequency { get; set; }
    }
    public class PaysPrintApiResponce
    {
        public string statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    #endregion

    #region What's App

    public class Rootobject4
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public string topic { get; set; }
        public string project_id { get; set; }
        public string delivery_attempt { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string type { get; set; }
        public string id { get; set; }
        public object[] meta_data { get; set; }
        public string project_id { get; set; }
        public string phone_number { get; set; }
        public string contact_id { get; set; }
        public object campaign { get; set; }
        public string sender { get; set; }
        public Message_Content message_content { get; set; }
        public string message_type { get; set; }
        public string status { get; set; }
        public bool is_HSM { get; set; }
        public object chatbot_response { get; set; }
        public object agent_id { get; set; }
        public long sent_at { get; set; }
        public long delivered_at { get; set; }
        public object read_at { get; set; }
        public Failureresponse failureResponse { get; set; }
        public string userName { get; set; }
        public string countryCode { get; set; }
        public string submitted_message_id { get; set; }
        public int message_price { get; set; }
        public object deductionType { get; set; }
        public object mau_details { get; set; }
        public object whatsapp_conversation_details { get; set; }
        public object context { get; set; }
        public string messageId { get; set; }
    }

    public class Message_Content
    {
        public string caption { get; set; }
        public string url { get; set; }
        public string urlExpiry { get; set; }
    }

    public class Failureresponse
    {
    }
    #endregion

    #region Mic
    public class RootobjectMic
    {
        public Userdetails2 UserDetails { get; set; }
        public Employeedetails EmployeeDetails { get; set; }
        public List<Documentdetail2> DocumentDetails { get; set; }
    }

    public class Userdetails2
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
    }

    public class Employeedetails
    {
        public string Title { get; set; }
        [Required(ErrorMessage = "Candidate name is required")]
        public string CandidateName { get; set; }
        public string FatherName { get; set; }
        [Required(ErrorMessage = "EMailID is required")]
        public string EMailID { get; set; }
        [Required(ErrorMessage = "Mobile no is required")]
        public string MobileNo { get; set; }
        public string AlternateMobileNo { get; set; }
        public string Gender { get; set; }
        [Required(ErrorMessage = "DateOfBirth is required")]
        public string DateOfBirth { get; set; }
        [Required(ErrorMessage = "Emp_CandidateNumber is required")]
        public string Emp_CandidateNumber { get; set; }
        public string EmployeeID { get; set; }
        public string PlaceOfJoining { get; set; }
        public string Designation { get; set; }
        public string DateOfJoining { get; set; }
        public string PanNumber { get; set; }
        public string AdharNumber { get; set; }
        public string UANNumber { get; set; }
        public string Package_Name { get; set; }
        public List<Educationdetail> EducationDetails { get; set; }
        public List<Addressdetail> AddressDetails { get; set; }
        public List<Employmentdetail> EmploymentDetails { get; set; }
        public List<Referencedetail> ReferenceDetails { get; set; }
    }

    public class Educationdetail
    {
        public string DegreeQualificationName { get; set; }
        public string EnrollmentNo { get; set; }
        public string YearOfPassing { get; set; }
        public string InstituteName { get; set; }
        public string UniversityOrBoard { get; set; }
        public string PercentageObtained { get; set; }
        public string ModeofEducation { get; set; }
    }

    public class Addressdetail
    {
        public string AddressType { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string MobileNo { get; set; }
    }

    public class Employmentdetail
    {
        public string PreviousCompanyName { get; set; }
        public string Designation { get; set; }
        public string EmploymentMode { get; set; }
        public string DateOfJoining { get; set; }
        public string DateOfRelieving { get; set; }
        public string LastSalaryDrawn { get; set; }
        public string PreviousEmployeeID { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerContactNumber { get; set; }
        public string ReportingManagerMailID { get; set; }
    }

    public class Referencedetail
    {
        public string ReferenceName { get; set; }
        public string Mobile { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
    }

    public class Documentdetail2
    {
        public string DocumentName { get; set; }
        public string DocumentUrl { get; set; }
    }


    #endregion

    #region MICROLAND

    public class RootobjectMicdata
    {
        public string UserName { get; set; }
        public string Title { get; set; }
        public string CandidateName { get; set; }
        public string FatherName { get; set; }
        public string EMailID { get; set; }
        public string MobileNo { get; set; }
        public string AlternateMobileNo { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Emp_CandidateNumber { get; set; }
        public string EmployeeID { get; set; }
        public string PlaceOfJoining { get; set; }
        public string Designation { get; set; }
        public string DateOfJoining { get; set; }
        public string PanNumber { get; set; }
        public string AdharNumber { get; set; }
        public string UANNumber { get; set; }
        public string Package_Name { get; set; }
        public Educationdetail2[] EducationDetails { get; set; }
        public Addressdetail2[] AddressDetails { get; set; }
        public Employmentdetail2[] EmploymentDetails { get; set; }
        public Referencedetail2[] ReferenceDetails { get; set; }
        public Documentdetail4[] DocumentDetails { get; set; }
    }

    public class Educationdetail2
    {
        public string DegreeQualificationName { get; set; }
        public string EnrollmentNo { get; set; }
        public string YearOfPassing { get; set; }
        public string InstituteName { get; set; }
        public string UniversityOrBoard { get; set; }
        public string PercentageObtained { get; set; }
        public string ModeofEducation { get; set; }
    }

    public class Addressdetail2
    {
        public string AddressType { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string MobileNo { get; set; }
    }

    public class Employmentdetail2
    {
        public string PreviousCompanyName { get; set; }
        public string Designation { get; set; }
        public string EmploymentMode { get; set; }
        public string DateOfJoining { get; set; }
        public string DateOfRelieving { get; set; }
        public string LastSalaryDrawn { get; set; }
        public string PreviousEmployeeID { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerContactNumber { get; set; }
        public string ReportingManagerMailID { get; set; }
    }

    public class Referencedetail2
    {
        public string ReferenceName { get; set; }
        public string Mobile { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
    }

    public class Documentdetail4
    {
        public string DocumentName { get; set; }
        public string DocumentUrl { get; set; }
    }

    #endregion

    

}