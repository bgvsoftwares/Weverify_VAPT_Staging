using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ViewModels.ProvidedInfoViewModel;

namespace ViewModels.TempViewModel
{
    public class CandidateDEModel
    {
        public Int64 PersonalRowID { get; set; }
        public int SubCheckRowID { get; set; }
        public int clientRowId { get; set; }
        public Int64 AntecedentRowID { get; set; }
        public string DisplayName { get; set; }
        public string DataEntryValue { get; set; }
        public int CreatedBy { get; set; }
        public string moduleName { get; set; }
    }

    public class TempPQPersonalViewModel
    {
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        //[Display(Name = "ID : ")]
        //public int ClientUserRowID { get; set; }

        //[Display(Name = "ID : ")]
        //public int CandidateUserID { get; set; }

        [Display(Name = "Client Ref ID : ")]
        public string ClientRefID { get; set; }

        [Display(Name = "Location : ")]
        public string Location { get; set; }

        [Display(Name = "Company Ref No : ")]
        public string CompanyRefNo { get; set; }

        [Display(Name = "Candidate Code : ")]
        public string CandidateCode { get; set; }

        [Required]
        [Display(Name = "Title : ")]
        public string CandTitle { get; set; }

        [Required]
        [Display(Name = "Name : ")]
        public string Name { get; set; }

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

        //[Display(Name = "Alternate No : ")]
        //public string AlternateNo { get; set; }

        [Display(Name = "Email : ")]
        public string EmailID { get; set; }

        //[Display(Name = "Alternate Email : ")]
        //public string AlternateEmailID { get; set; }

        //[Display(Name = "STD : ")]
        //public string STD { get; set; }

        //[Display(Name = "Residence Phone No : ")]
        //public string ResidencePhoneNo { get; set; }

        [Display(Name = "Pan Card : ")]
        public byte PanCard { get; set; }

        [Display(Name = "Pan Card : ")]
        public string PanCardNo { get; set; }

        [Display(Name = "Passport : ")]
        public byte Passport { get; set; }

        [Display(Name = "Passport : ")]
        public string PassportNo { get; set; }

        [Display(Name = "Aadhaar Card : ")]
        public byte AadhaarCard { get; set; }

        [Display(Name = "Aadhaar Card : ")]
        public string AadhaarCardNo { get; set; }

        [Display(Name = "Driving License : ")]
        public byte DrivingLicense { get; set; }

        [Display(Name = "Driving License : ")]
        public string DrivingLicenseNo { get; set; }

        [Display(Name = "Voter ID Card : ")]
        public byte VoterIDCard { get; set; }

        [Display(Name = "Voter ID Card : ")]
        public string VoterIDCardNo { get; set; }

        //[Display(Name = "CheckDateTime : ")]
        //public DateTime? CheckDateTime { get; set; }

        //[Display(Name = "Candidate Type : ")]
        //public string CandidateType { get; set; }

        //[Display(Name = "Catagory : ")]
        //public string Catagory { get; set; }

        //[Display(Name = "CaseType : ")]
        //public string CaseType { get; set; }

        //[Display(Name = "TAT : ")]
        //public byte TAT { get; set; }

        //[Display(Name = "Nationality : ")]
        //public string Nationality { get; set; }

        //[Display(Name = "EmployeeID : ")]
        //public string EmployeeID { get; set; }

        //[Display(Name = "Designation : ")]
        //public string Designation { get; set; }

        //[Display(Name = "Place of Joining : ")]
        //public string PlaceofJoining { get; set; }

        //[Display(Name = "DateofJoining : ")]
        //public DateTime? DateofJoining { get; set; }

        //[Display(Name = "Details Entered By : ")]
        //public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate

        //[Display(Name = "ProcessName : ")]
        //public string ProcessName { get; set; }

        //[Display(Name = "BusinessName : ")]
        //public string BusinessName { get; set; }

        //[Display(Name = "Type : ")]
        //public string Type { get; set; }

        //[Display(Name = "SpouseName : ")]
        //public string SpouseName { get; set; }

        //[Display(Name = "Spouse Middle Name : ")]
        //public string SpouseMiddleName { get; set; }

        //[Display(Name = "Spouse Last Name : ")]
        //public string SpouseLastName { get; set; }

        [Display(Name = "Create By : ")]
        public string CreatedBy { get; set; }

        [Display(Name = "Create Date : ")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Allocated By : ")]
        public short AllocatedBy { get; set; }

        [Display(Name = "Allocated To DE : ")]
        public short AllocatedToDE { get; set; }

        [Display(Name = "Allocated To DE Date : ")]
        public DateTime? AllocatedToDEDate { get; set; }

        [Display(Name = "Allocated To DEQC : ")]
        public short AllocatedToDEQC { get; set; }

        [Display(Name = "Allocated To DEQC Date : ")]
        public DateTime? AllocatedToDEQCDate { get; set; }

        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        public byte Status { get; set; }

        [Display(Name = "OtherDetails : ")]
        public string OtherDetails { get; set; }

        [Display(Name = "OtherDetails1 : ")]
        public string OtherDetails1 { get; set; }

        [Display(Name = "OtherDetails2 : ")]
        public string OtherDetails2 { get; set; }

        public byte CaseStatus { get; set; }

        public IEnumerable<MailHistortcount> MailHistortcount { get; set; }


    }

    public class AddTempPQPersonalViewModel
    {
        [ScaffoldColumn(false)]
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        [Required(ErrorMessage = "Select Client")]
        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }

        public int ClientUserRowID { get; set; }

        public int CandidateUserID { get; set; }

        [Display(Name = "Client Ref ID : ")]
        [MaxLength(100)]
        public string ClientRefID { get; set; }

        [Display(Name = "Company Ref No : ")]
        [MaxLength(100)]
        public string CompanyRefNo { get; set; }


        [Display(Name = "Candidate Code : ")]
        [MaxLength(100)]
        public string CandidateCode { get; set; }

        [Required(ErrorMessage = "Select title")]
        [Display(Name = "Title : ")]
        [MaxLength(15)]
        public string CandTitle { get; set; }

        [Required(ErrorMessage = "Enter Candidate Name")]
        [Display(Name = "Name : ")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Middle Name : ")]
        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter mobile number")]
        [Display(Name = "Mobile No : ")]
        [StringLength(20)]
        [RegularExpression(@"^[7-9]{1}.*[0-9]{9}$", ErrorMessage = "Please enter valid Mobile Number")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Enter Email id")]
        [Display(Name = "Email : ")]
        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string EmailID { get; set; }

        [MaxLength(50)]
        [Display(Name = "CaseType : ")]
        public string CaseType { get; set; }

        [MaxLength(20)]
        [Display(Name = "From Type : ")]
        public string FromType { get; set; }

        [MaxLength(100)]
        [Display(Name = "Details Entered By : ")]
        public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate

        [Display(Name = "Create By : ")]
        public short CreatedBy { get; set; }

        [Display(Name = "Create Date : ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Allocated By : ")]
        public short AllocatedBy { get; set; }

        [Display(Name = "Allocated To DE : ")]
        public short AllocatedToDE { get; set; }

        [Display(Name = "Allocated To DE Date : ")]
        public DateTime? AllocatedToDEDate { get; set; }

        [Display(Name = "Allocated To DEQC : ")]
        public short AllocatedToDEQC { get; set; }

        [Display(Name = "Allocated To DEQC Date : ")]
        public DateTime? AllocatedToDEQCDate { get; set; }

        public string CandidateCheckOrPackage { get; set; }


        public string OtherDetails { get; set; }
        public string OtherDetails1 { get; set; }

        public short TAT { get; set; }

        public byte CaseStatus { get; set; }
        public byte Status { get; set; }
    }

    public class AddClosureBulkUpViewModel
    {

        public string ClientName { get; set; }
        public string TMName { get; set; }

        public string CompanyRefNo { get; set; }

        public string CandidateName { get; set; }

        public string CheckUniqueID { get; set; }

        public string TypeOfEducation { get; set; }

        public string CollegeName { get; set; }

        public string Other_Comment { get; set; }

        public string UniversityName { get; set; }

        public string CourseName { get; set; }

        public string YearOfPassing { get; set; }

        public string RollNo { get; set; }

        public string Respondents_Name_and_Designation { get; set; }

        public string Check_status { get; set; }

        public string Closure_date { get; set; }

        public string Mode_of_Verification { get; set; }

        public string Link { get; set; }

        public string Verified_From { get; set; }

        public string Attachments { get; set; }


    }


    public class AddClosureBulkUpEmpViewModel
    {

        public string ClientName { get; set; }
        public string SubCheckName { get; set; }

        public string CompanyRefNo { get; set; }

        public string Attachments { get; set; }

        public string CandidateName { get; set; }

        public string MobileNo { get; set; }
        public string TAT { get; set; }

        public string UniqueComponentID { get; set; }

        public string CompanyName { get; set; }

        public string DOJ { get; set; }

        public string DOL { get; set; }

        public string Tenure { get; set; }

        public string Designation { get; set; }

        public string EmployeeCode { get; set; }

        public string Documents { get; set; }

        public string Exit_Formalities_Status { get; set; }

        public string Country_Region { get; set; }

        public string Date_to_Complete { get; set; }

        public string Outsourcing_Agency_Name { get; set; }

        public string IsSuspect { get; set; }

        public string Reason_For_Suspect { get; set; }
        public string Remuneration_Salary { get; set; }
        public string  Reason_for_leaving { get; set; }
        public string Reporting_Managers_Name { get; set; }
        public string Eligibility_for_rehire { get; set; }
        
        public string Company_registered_under_regulatory_authorities { get; set; }
        public string Reporting_Managers_Contact_Number { get; set; }
        public string Verifiers_Details { get; set; }
        public string Mode_of_Verification { get; set; }
        public string Reporting_Managers_Email_ID { get; set; }
        public string Discrepancy_status_if_any { get; set; }
        public string Required_Mode_of_Verification { get; set; }
        public string Date_of_Verification { get; set; }
        public string HR_Managers_Name__Designation { get; set; }
        public string HR_Managers_Contact_Number { get; set; }
        public string HR_Managers_Email_ID { get; set; }
        public string Any_Other_Comments { get; set; }




    }

    public class AddClosureBulkUpCRCViewModel
    {

        public string ClientName { get; set; }
        public string CompanyRefNo { get; set; }

        public string CandidateFullName { get; set; }

        public string SubCheckName  { get; set; }

        public string CheckUniqueID  { get; set; }
        public string DOB  { get; set; }

        public string FatherName   { get; set; }

        public string Address   { get; set; }

        public string DurationOfStay   { get; set; }

        public string Status   { get; set; }

        public string DurationofYearCovered { get; set; }

        public string Mode_of_Verification    { get; set; }

        public string Date_of_Verification    { get; set; }

        public string Any_Other_Comments { get; set; }
        public string Select_Disposition { get; set; }

        public string Verifiers_Name { get; set; }

        public string Verifiers_Designation { get; set; }

        public string Discrepancy_status_if_any { get; set; }

        public string Required_Mode_of_Verification { get; set; }


        public string Attachments { get; set; }


        public string Designation { get; set; }
        public string Contacted_Person { get; set; }

        public string Police_Station { get; set; }

        public string Contact_No { get; set; }




    }
    public class AddClosureBulkUpIDEViewModel
    {

        public string ClientName { get; set; }
        public string CompanyRefNo { get; set; }

        public string CandidateFullName { get; set; }

        public string SubCheckName { get; set; }

        public string CheckUniqueID { get; set; }
        public string IDNumber { get; set; }
        public string IdentityType { get; set; }
        public string DOB { get; set; }

        public string FatherName { get; set; }

        public string Address { get; set; }

        public string DurationOfStay { get; set; }

        public string Status { get; set; }

        public string DurationofYearCovered { get; set; }

        public string Mode_of_Verification { get; set; }

        public string Date_of_Verification { get; set; }

        public string Any_Other_Comments { get; set; }
        public string Select_Disposition { get; set; }

        public string Verifiers_Name { get; set; }

        public string Verifiers_Designation { get; set; }

        public string Discrepancy_status_if_any { get; set; }

        public string Required_Mode_of_Verification { get; set; }


        public string Attachments { get; set; }




    }

    public class AddClosureBulkUpGlobalViewModel
    {

        public string ClientName { get; set; }
        public string CompanyRefNo { get; set; }

        public string CandidateFullName { get; set; }

        public string SubCheckName { get; set; }

        public string IdentityType { get; set; }

        public string CheckUniqueID { get; set; }
        public string DOB { get; set; }

        public string FatherName { get; set; }

        public string Gender  { get; set; }

        public string Attachments { get; set; }



        public string Status { get; set; }

        public string Mode_of_Verification { get; set; }

        public string Date_of_Verification { get; set; }

        public string Any_Other_Comments { get; set; }
        public string Severity  { get; set; }
        public string Color_code { get; set; }
        public string CheckStatus { get; set; }
        public string ReportComment { get; set; }

        public string Address { get; set; }
        public string UAN_No { get; set; }
        public string Contact_No { get; set; }
        public string Pan_No { get; set; }
        public string Discrepancy_status_if_any { get; set; }
        public string Required_Mode_of_Verification { get; set; }




    }
    public class AddAddressRecordsViewModel
    {
        public int PersonalRowId { get; set; }
        public int SubCheckRowId { get; set; }
        public int ClientRowId { get; set; }
        public string CandidateName { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string MobileNo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string IDNumber { get; set; }
        public string IDType { get; set; }
        public string DOB { get; set; }        

    }

    public class AddTempPQPersonalForBulkUpViewModel
    {
        public string PID { get; set; }
        // ── Client 684 ──
        public string OfferType { get; set; }
        // ── end 684 ──


        public string COE_Code { get; set; }

        public string SDO_Code { get; set; }

        public string Grade { get; set; }

        public string UAN { get; set; }

        public string UID { get; set; }

        public string OHR_Taleo_ID { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        [Required(ErrorMessage = "Select Client")]
        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }

        public int ClientUserRowID { get; set; }

        public int CandidateUserID { get; set; }

        [Display(Name = "Client Ref ID : ")]
        [MaxLength(100)]
        public string ClientRefID { get; set; }

        [Display(Name = "EmployeID : ")]
        [MaxLength(100)]
        public string EmployeID { get; set; }

        //[Display(Name = "Company Ref No : ")]
        //[MaxLength(100)]
        //public string CompanyRefNo { get; set; }

        [Display(Name = "Candidate Code : ")]
        [MaxLength(100)]
        public string CandidateCode { get; set; }

        [Required(ErrorMessage = "Select title")]
        [Display(Name = "Title : ")]
        [MaxLength(15)]
        public string CandTitle { get; set; }

        [Required(ErrorMessage = "Enter Candidate Name")]
        [Display(Name = "Name : ")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Middle Name : ")]
        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Gender : ")]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Enter mobile number")]
        [Display(Name = "Mobile No : ")]
        [StringLength(20)]
        //[RegularExpression(@"^[5-9]{1}.*[0-9]{9}$", ErrorMessage = "Please enter valid Mobile Number")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Enter Email id")]
        [Display(Name = "Email : ")]
        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string EmailID { get; set; }

        [MaxLength(50)]
        [Display(Name = "CaseType : ")]
        public string CaseType { get; set; }

        [Display(Name = "Pan Card : ")]
        [RegularExpression(@"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$", ErrorMessage = "Please enter valid Pan Card")]
        [MaxLength(10)]
        public string PanCardNo { get; set; }

        [Display(Name = "Passport : ")]
        [MaxLength(100)]
        public string PassportNo { get; set; }

        [Display(Name = "Aadhaar Card : ")]
        [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Please enter valid Aadhaar Card")]
        [MaxLength(12)]
        public string AadhaarCardNo { get; set; }

        [Display(Name = "Driving License : ")]
        [MaxLength(100)]
        public string DrivingLicenseNo { get; set; }

        [Display(Name = "Voter ID Card : ")]
        [MaxLength(100)]
        public string VoterIDCardNo { get; set; }

        [MaxLength(20)]
        [Display(Name = "From Type : ")]
        public string FromType { get; set; }

        [MaxLength(100)]
        [Display(Name = "Details Entered By : ")]
        public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate

        [Display(Name = "Created By : ")]
        public short CreatedBy { get; set; }

        [Display(Name = "Create Date : ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Allocated By : ")]
        public short AllocatedBy { get; set; }

        [Display(Name = "Allocated To DE : ")]
        public short AllocatedToDE { get; set; }

        [Display(Name = "Allocated To DE Date : ")]
        public DateTime? AllocatedToDEDate { get; set; }

        [Display(Name = "Allocated To DEQC : ")]
        public short AllocatedToDEQC { get; set; }

        [Display(Name = "Allocated To DEQC Date : ")]
        public DateTime? AllocatedToDEQCDate { get; set; }

        public string CandidateCheckOrPackage { get; set; }

        [Display(Name = "Upload Docs : ")]
        [ValidateLargeFileClientCandidateUploadExcelDocs]
        public HttpPostedFileBase UploadDoc { get; set; }

        [Display(Name = "Template : ")]
        public string Template { get; set; }

        public string OtherDetails { get; set; }
        public string OtherDetails1 { get; set; }

        public short TAT { get; set; }

        public byte CaseStatus { get; set; }
        public byte Status { get; set; }
        public string SSNNo { get; set; }
        public string NationalSkills { get; set; }

        [Display(Name = "Upload CV : ")]
        //[ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase UploadCV { get; set; }
        public IEnumerable<AddTempPQPersonalForBulkUpViewModel> CandidateDuplicateList { get; set; }
        public string CandidateName { get; set; }
        //public string FatherName { get; set; }

        [Required(ErrorMessage = "Select title")]
        [Display(Name = "Title : ")]
        [MaxLength(15)]
        public string FatherTitle { get; set; }

        [Required(ErrorMessage = "Enter Candidate Name")]
        [Display(Name = "FatherName : ")]
        [MaxLength(50)]
        public string FatherName { get; set; }

        [Display(Name = "Father Middle Name : ")]
        [MaxLength(50)]
        public string FatherMiddleName { get; set; }

        [Display(Name = "Father Last Name : ")]
        [MaxLength(50)]
        public string FatherLastName { get; set; }
        public string CompanyRefNo { get; set; }

        //[Required(ErrorMessage = "Enter Email id")]
        [Display(Name = "Alternate Email : ")]
        [MaxLength(100)]
        //[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string AlternateEmailID { get; set; }

        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string ClientUserName { get; set; }

        [Display(Name = "Date Of Birth : ")]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }
        public string Nationality { get; set; }
        public string Band { get; set; }
        [Display(Name = "CandidateType : ")]
        public string CandidateType { get; set; }
        public string BusinessName { get; set; }
        public DateTime? DateofJoining { get; set; }
        public string PlaceOfJoining { get; set; }
        public string AlternateNo { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string CreatedBySource { get; set; }
        public string Entity { get; set; }
        public string ProcessName { get; set; }
        public string Designation { get; set; }
        public string Zone { get; set; }
        public string Location { get; set; }
        public string RecruiterEmailID { get; set; }
        public string TCSClientType { get; set; }

    }

    public class UpdateTempPQPersonalViewModel
    {
        [Required]
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }

        [Display(Name = "ID : ")]
        public int ClientUserRowID { get; set; }

        [Display(Name = "ID : ")]
        public int CandidateUserID { get; set; }

        [Display(Name = "Client Ref ID : ")]
        [MaxLength(100)]
        public string ClientRefID { get; set; }

        [Display(Name = "Location : ")]
        public string Location { get; set; }

        [Display(Name = "Company Ref No : ")]
        [MaxLength(100)]
        public string CompanyRefNo { get; set; }

        [Display(Name = "Candidate Code : ")]
        [MaxLength(100)]
        public string CandidateCode { get; set; }

        [Display(Name = "Title : ")]
        [MaxLength(15)]
        public string CandTitle { get; set; }

        [Display(Name = "Name : ")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Middle Name : ")]
        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Title : ")]
        [MaxLength(15)]
        public string FatherTitle { get; set; }

        [Display(Name = "father Name : ")]
        [MaxLength(50)]
        public string FatherName { get; set; }

        [Display(Name = "Middle Name : ")]
        [MaxLength(50)]
        public string FatherMiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        [MaxLength(50)]
        public string FatherLastName { get; set; }

        [Display(Name = "Gender : ")]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Date Of Birth : ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }

        [Display(Name = "Marital Status : ")]
        [MaxLength(15)]
        public string MaritalStatus { get; set; }

        [Display(Name = "Mobile No : ")]
        [StringLength(20)]
        //[RegularExpression(@"^[7-9]{1}.*[0-9]{9}$", ErrorMessage = "Please enter valid Mobile Number")]
        public string MobileNo { get; set; }

        [Display(Name = "Alternate No : ")]
        public string AlternateNo { get; set; }

        [Display(Name = "Email : ")]
        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string EmailID { get; set; }

        [MaxLength(100)]
        [Display(Name = "Alternate Email : ")]
        public string AlternateEmailID { get; set; }

        [MaxLength(10)]
        [Display(Name = "STD : ")]
        public string STD { get; set; }

        [MaxLength(20)]
        [Display(Name = "STD Residence Phone No : ")]
        public string ResidencePhoneNo { get; set; }

        [Display(Name = "Pan Card : ")]
        public byte PanCard { get; set; }

        [Display(Name = "Pan Card No : ")]
        [MaxLength(100)]
        public string PanCardNo { get; set; }

        [Display(Name = "Passport : ")]
        public byte Passport { get; set; }

        [Display(Name = "Passport No : ")]
        [MaxLength(100)]
        public string PassportNo { get; set; }

        [Display(Name = "Aadhaar Card : ")]
        public byte AadhaarCard { get; set; }

        [Display(Name = "Aadhaar Card No : ")]
        [MaxLength(100)]
        public string AadhaarCardNo { get; set; }

        [Display(Name = "Driving License : ")]
        public byte DrivingLicense { get; set; }

        [Display(Name = "Driving License No : ")]
        [MaxLength(100)]
        public string DrivingLicenseNo { get; set; }

        [Display(Name = "Voter ID Card : ")]
        public byte VoterIDCard { get; set; }

        [Display(Name = "Voter ID Card No : ")]
        [MaxLength(100)]
        public string VoterIDCardNo { get; set; }

        [Display(Name = "CheckDateTime : ")]
        public DateTime? CheckDateTime { get; set; }

        [MaxLength(50)]
        [Display(Name = "Candidate Type : ")]
        public string CandidateType { get; set; }

        [MaxLength(50)]
        [Display(Name = "Catagory : ")]
        public string Catagory { get; set; }

        [MaxLength(50)]
        [Display(Name = "Case Type : ")]
        public string CaseType { get; set; }

        [Display(Name = "TAT : ")]
        public byte TAT { get; set; }

        [MaxLength(50)]
        [Display(Name = "Nationality : ")]
        public string Nationality { get; set; }

        [MaxLength(100)]
        [Display(Name = "EmployeeID : ")]
        public string EmployeeID { get; set; }

        [MaxLength(100)]
        [Display(Name = "Designation : ")]
        public string Designation { get; set; }

        [MaxLength(100)]
        [Display(Name = "Place of Joining : ")]
        public string PlaceofJoining { get; set; }

        [Display(Name = "Date of Joining : ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateofJoining { get; set; }

        [MaxLength(100)]
        [Display(Name = "National Skills : ")]
        public string NationalSkills { get; set; }

        [MaxLength(50)]
        [Display(Name = "SSN No : ")]
        public string SSNNo { get; set; }

        [MaxLength(20)]
        [Display(Name = "From Type : ")]
        public string FromType { get; set; }

        [MaxLength(100)]
        [Display(Name = "Details Entered By : ")]
        public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate

        [MaxLength(100)]
        [Display(Name = "Process Name : ")]
        public string ProcessName { get; set; }

        [MaxLength(200)]
        [Display(Name = "Business Name : ")]
        public string BusinessName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Type : ")]
        public string Type { get; set; }

        [MaxLength(50)]
        [Display(Name = "Spouse Name : ")]
        public string SpouseName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Spouse Middle Name : ")]
        public string SpouseMiddleName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Spouse Last Name : ")]
        public string SpouseLastName { get; set; }

        [Display(Name = "Order Date : ")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Delivery Date : ")]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Create By : ")]
        public short CreatedBy { get; set; }

        [Display(Name = "Create Date : ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Allocated By : ")]
        public short AllocatedBy { get; set; }

        [Display(Name = "Allocated To DE : ")]
        public short AllocatedToDE { get; set; }

        [Display(Name = "Allocated To DE Date : ")]
        public DateTime? AllocatedToDEDate { get; set; }

        [Display(Name = "Allocated To DEQC : ")]
        public short AllocatedToDEQC { get; set; }

        [Display(Name = "Allocated To DEQC Date : ")]
        public DateTime? AllocatedToDEQCDate { get; set; }

        [MaxLength(200)]
        [Display(Name = "Remark : ")]
        public string Remark { get; set; }

        [MaxLength(50)]
        [Display(Name = "Final Color : ")]
        public string FinalColor { get; set; }

        [Display(Name = "ExitIntrvwApplicable : ")]
        public byte ExitIntrvwApplicable { get; set; }        // yes=1/No=0

        [Display(Name = "Case Status : ")]
        public byte CaseStatus { get; set; }

        [Display(Name = "Final Status : ")]
        public byte FinalStatus { get; set; }

        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        public byte Status { get; set; }


        [Display(Name = "Signed LOA : ")]
        [ValidateImageFileWithNullCheckAttribute]
        public HttpPostedFileBase CandidateTermsFile { get; set; }

        [MaxLength(100)]
        [Display(Name = "OtherDetails : ")]
        public string OtherDetails { get; set; }

        [MaxLength(100)]
        [Display(Name = "OtherDetails1 : ")]
        public string OtherDetails1 { get; set; }

        [MaxLength(100)]
        [Display(Name = "OtherDetails2 : ")]
        public string OtherDetails2 { get; set; }

        [MaxLength(50)]
        [Display(Name = "National ID Proof : ")]
        public string NationalIDProof { get; set; }
        public byte VisibleLOA { get; set; }

        [Display(Name = "Upload Id proof: ")]
        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase UploadIdProof { get; set; }
        //public IEnumerable<AddTempPQPersonalForBulkUpViewModel> CandidateDuplicateList { get; set; }
    }

    public class TempPQPersonalDetailViewModel
    {
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [Display(Name = "ID : ")]
        public int ClientUserRowID { get; set; }

        [Display(Name = "ID : ")]
        public int CandidateUserID { get; set; }

        [Display(Name = "Client Ref ID : ")]
        public string ClientRefID { get; set; }

        //[Display(Name = "Location : ")]
        //public string Location { get; set; }

        [Display(Name = "Company Ref No : ")]
        public string CompanyRefNo { get; set; }

        [Display(Name = "Candidate Code : ")]
        public string CandidateCode { get; set; }

        [Required]
        [Display(Name = "Title : ")]
        public string CandTitle { get; set; }

        [Required]
        [Display(Name = "Name : ")]
        public string Name { get; set; }

        [Display(Name = "Middle Name : ")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        public string LastName { get; set; }

        //[Display(Name = "Title : ")]
        //public string FatherTitle { get; set; }

        //[Display(Name = "father Name : ")]
        //public string FatherName { get; set; }

        //[Display(Name = "Middle Name : ")]
        //public string FatherMiddleName { get; set; }

        //[Display(Name = "Last Name : ")]
        //public string FatherLastName { get; set; }

        [Display(Name = "Gender : ")]
        public string Gender { get; set; }

        //[Display(Name = "Date Of Birth : ")]
        //public DateTime? DOB { get; set; }

        //[Display(Name = "Marital Status : ")]
        //public string MaritalStatus { get; set; }

        [Display(Name = "Mobile No : ")]
        public string MobileNo { get; set; }

        //[Display(Name = "Alternate No : ")]
        //public string AlternateNo { get; set; }

        [Display(Name = "Email : ")]
        public string EmailID { get; set; }

        public int SendMailStatus { get; set; }
        public byte CaseStatusId { get; set; }

        //[Display(Name = "Alternate Email : ")]
        //public string AlternateEmailID { get; set; }

        //[Display(Name = "STD : ")]
        //public string STD { get; set; }

        //[Display(Name = "Residence Phone No : ")]
        //public string ResidencePhoneNo { get; set; }

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

        //[Display(Name = "CheckDateTime : ")]
        //public DateTime? CheckDateTime { get; set; }

        //[Display(Name = "Candidate Type : ")]
        //public string CandidateType { get; set; }

        //[Display(Name = "Catagory : ")]
        //public string Catagory { get; set; }

        //[Display(Name = "CaseType : ")]
        //public string CaseType { get; set; }

        //[Display(Name = "TAT : ")]
        //public byte TAT { get; set; }

        //[Display(Name = "Nationality : ")]
        //public string Nationality { get; set; }

        //[Display(Name = "EmployeeID : ")]
        //public string EmployeeID { get; set; }

        //[Display(Name = "Designation : ")]
        //public string Designation { get; set; }

        //[Display(Name = "Place of Joining : ")]
        //public string PlaceofJoining { get; set; }

        //[Display(Name = "DateofJoining : ")]
        //public DateTime? DateofJoining { get; set; }

        //[Display(Name = "Details Entered By : ")]
        //public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate

        //[Display(Name = "ProcessName : ")]
        //public string ProcessName { get; set; }

        //[Display(Name = "BusinessName : ")]
        //public string BusinessName { get; set; }

        //[Display(Name = "Type : ")]
        //public string Type { get; set; }

        //[Display(Name = "SpouseName : ")]
        //public string SpouseName { get; set; }

        //[Display(Name = "Spouse Middle Name : ")]
        //public string SpouseMiddleName { get; set; }

        //[Display(Name = "Spouse Last Name : ")]
        //public string SpouseLastName { get; set; }

        [Display(Name = "Create By : ")]
        public short CreatedBy { get; set; }

        [Display(Name = "Create Date : ")]
        public DateTime? CreatedDate { get; set; }

        //[Display(Name = "Allocated By : ")]
        //public short AllocatedBy { get; set; }

        //[Display(Name = "Allocated To DE : ")]
        //public short AllocatedToDE { get; set; }
        //public string AllocatedToDEName { get; set; }

        //[Display(Name = "Allocated To DE Date : ")]
        //public DateTime? AllocatedToDEDate { get; set; }

        //[Display(Name = "Allocated To DEQC : ")]
        //public short AllocatedToDEQC { get; set; }
        //public string AllocatedToDEQCName { get; set; }

        //[Display(Name = "Allocated To DEQC Date : ")]
        //public DateTime? AllocatedToDEQCDate { get; set; }

        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        public byte Status { get; set; }

        [Display(Name = "OtherDetails : ")]
        public string OtherDetails { get; set; }

        [Display(Name = "OtherDetails1 : ")]
        public string OtherDetails1 { get; set; }

        [Display(Name = "OtherDetails2 : ")]
        public string OtherDetails2 { get; set; }
        public string Entityname { get; set; }
        public string PackageName { get; set; }
    }

    public class TempPQPersonalForSendMailViewModel
    {
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [Display(Name = "ID : ")]
        public int ClientUserRowID { get; set; }

        [Display(Name = "ID : ")]
        public int CandidateUserID { get; set; }

        [Display(Name = "Client Ref ID : ")]
        public string ClientRefID { get; set; }

        [Display(Name = "Company Ref No : ")]
        public string CompanyRefNo { get; set; }

        [Display(Name = "Candidate Code : ")]
        public string CandidateCode { get; set; }

        [Required]
        [Display(Name = "Title : ")]
        public string CandTitle { get; set; }

        [Required]
        [Display(Name = "Name : ")]
        public string Name { get; set; }

        [Display(Name = "Middle Name : ")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        public string LastName { get; set; }

        [Display(Name = "Gender : ")]
        public string Gender { get; set; }

        [Display(Name = "Mobile No : ")]
        public string MobileNo { get; set; }

        [Display(Name = "Email : ")]
        public string EmailID { get; set; }

        //[Display(Name = "Status : ")]
        //[ScaffoldColumn(false)]
        //public byte Status { get; set; }

        //[Display(Name = "OtherDetails : ")]
        //public string OtherDetails { get; set; }
        public string AlternateEmail { get; set; }

        [Display(Name = "OtherDetails1 : ")]
        public string OtherDetails2 { get; set; }
        public string ProcessName { get; set; }
        public string BusinessName { get; set; }

    }

    public class TempPQPersonalInfoListPagedModel
    {
        public IEnumerable<TempPQPersonalDetailViewModel> TempPQPersonalInfo { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public PQPersonalSearchPagedModel ItemSearch { get; set; }
    }

    public class TempClientHrApprovalCasesViewModel
    {
        public int PersonalRowID { get; set; }
        public string CandidateName { get; set; }
        public string PackageName { get; set; }
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string ClientRefID { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandidateCode { get; set; }
        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? SentMailDate { get; set; }
        public int SentMailStatus { get; set; }
        public string DetailsEnterdBy { get; set; }
        public string FromType { get; set; }
        public string OtherDetails { get; set; }
        public string OtherDetails1 { get; set; }
        public string OtherDetails2 { get; set; }
        public string Entity { get; set; }
        public string Emailid { get; set; }
        public string MobileNo { get; set; }
        public string Package { get; set; }

        public string HRRecruiterName { get; set; }

    }

    public class TempClientHrApprovalCasesListPagedModel
    {
        public IEnumerable<TempClientHrApprovalCasesViewModel> TempHrAprvlCases { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class MailHistortcount
    {
        public string Sno { get; set; }
        public string ToMail { get; set; }
        public string SubjectMail { get; set; }
        public string MailObject { get; set; }

    }

    public class ClientCandidateSearch
    {
        public int PersonalRowID { get; set; }
        public string CandidateName { get; set; }
        public string FinalStatus { get; set; }
        public string dob { get; set; }
        public string fatherName { get; set; }
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string ClientRefID { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandidateCode { get; set; }
        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string DetailsEnterdBy { get; set; }
        public string FromType { get; set; }
        public string OtherDetails { get; set; }
        public string OtherDetails1 { get; set; }
        public string OtherDetails2 { get; set; }
        public string ClientPackageName { get; set; }
        public string ActiveStatus { get; set; }
        public bool IsReportUploaded { get; set; }
        public bool ReportVisibality { get; set; }
        public string CaseStatus { get; set; }
        public string MobileNo { get; set; }
        public string Emailid { get; set; }
        public string EmployeeID { get; set; }
        public bool IntrimReportStatus { get; set; }
        public string tempPersonalRowId { get; set; }
        public bool IsReVerification { get; set; }
    }

    public class ClientCandidateSearchListPagedModel
    {
        public IEnumerable<ClientCandidateSearch> ClientCandidateSearch { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string TicketId { get; set; }
        public IEnumerable<CandidateDuplicateReferenceDetails> CandidateDuplicateReferenceDetails { get; set; }
        public PQPersonalSearchPagedModel ItemSearch { get; set; }
    }


    public class CandidateDuplicateReferenceDetails
    {
        public int Id { get; set; }
        public string TicketId { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string MobileNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DuplicateStatus { get; set; }
        public string EmailId { get; set; }
        public string FatherName { get; set; }


    }

    public class ClientBulkDownlode
    {
        public FileInfo fileDetails { get; set; }
        public string FileDownloadName { get; set; }
        public string CompanyRefno { get; set; }
    }

    public class APICandidateSummary
    {
        public string UniqueReferenceID { get; set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public string CreationDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string CompanyRefNo { get; set; }
        public int PersonalRowID { get; set; }



    }

    public class APICandidateSummaryListPagedModel
    {
        public IEnumerable<APICandidateSummary> APICandidateSummary { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class APICandidateSummaryDetail
    {
        public int APIAutoID { get; set; }
        public string UniqueReferenceID { get; set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public string Antecedent { get; set; }
        public string AntecedentValue { get; set; }
        public string UserName { get; set; }


    }
    public class jsonList
    {
        public object Property { get; set; }
        public object value { get; set; }
    }

    public class APICandidateSummaryDetailListPagedModel
    {
        public IEnumerable<APICandidateSummaryDetail> APICandidateSummaryDetail { get; set; }
        public List<jsonList> jsonList { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class RemarksList
    {
        public string Remarks { get; set; }
    }

    public class TempCandidateDetails
    {
        public string PersonalRowID { get; set; }
        public string DisplayName { get; set; }
        public string DataEntryValue { get; set; }
        public string SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
    }

    public class MediaGallary
    {
        public IEnumerable<MediaGallarydata> MediaGallarydata { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class MediaGallarydata
    {
        public int TableRowID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public string ImageType { get; set; }
        public int Status { get; set; }
    }

    public class MailAttachmentDoc
    {
        public string DocFileName { get; set; }
        public string DocFilePath { get; set; }
    }


    public class AddBulkUpDEViewModel
    {

        public string C_Country { get; set; }
        public string P_Country { get; set; }

        public string C_PinCode { get; set; }
        public string P_PinCode { get; set; }

        public string Name { get; set; }
        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string DOB { get; set; }

        public string FatherName { get; set; }
        public string Gender { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string EmployeID { get; set; }

        public string ClientRefID { get; set; }

        public string VoterIDCardNo { get; set; }

        public string DrivingLicenseNo { get; set; }

        public string AadhaarCardNo { get; set; }

        public string PassportNo { get; set; }

        public string PanCardNo { get; set; }

        public string ClientUserName { get; set; }

        public string AlternateEmail { get; set; }

        public string DateofJoining { get; set; }

        public string PlaceOfJoining { get; set; }

        public string AlternateNo { get; set; }
        public string ProcessName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Entity { get; set; }

        public string CandidateType { get; set; }
        public string PID_Number { get; set; }
        public string SDO_Code { get; set; }
        public string COE_Code { get; set; }
        public string UAN { get; set; }
        public string UID_Number { get; set; }
        public string OHR_Taleo_ID { get; set; }
        public string Band { get; set; }
        public string Designation { get; set; }
        public string Roles { get; set; }
        public string Grade { get; set; }
        public string Zone { get; set; }
        public string Permanent_Address { get; set; }
        public string PPOS { get; set; }
        public string P_Landmark { get; set; }
        public string Current_Address { get; set; }
        public string CPOS { get; set; }
        public string C_Landmark { get; set; }
        public string Previous1_Address { get; set; }
        public string P1POS { get; set; }
        public string P1_Landmark { get; set; }
        public string ID_Type { get; set; }
        public string ID_Number { get; set; }
        public string CIBIL_PAN_Card { get; set; }
        public string CIBIL_Address { get; set; }
        public string Drug_Test_Address { get; set; }
        public string Drug_Test_Mob_No { get; set; }
        public string FACIS_Address { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int PersonalRowId { get; set; }
        public string PAdd_Country { get; set; }

    }
    public class DownloadCandidateDocuments
    {
        public string DocumentName { get; set; }
        public string DocumentValue { get; set; }
    }


    public class CandidateFaceMatchViewModel
    {
        public int PersonalRowId { get; set; }
        public int SubCheckRowId { get; set; }
        public string MobileNo { get; set; }
        public string Face_Match { get; set; }
        public string CandidateName { get; set; }
        public string RendomOTP { get; set; }
        public string selfie { get; set; }
        public string FaceMatchStatus { get; set; }
        public string Status { get; set; }
        public int LivenessWithAdharStatus { get; set; }
}
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class LivenessApiResponse
    {
        public int statusCode { get; set; }
        public string requestId { get; set; }
        public LivenessResult result { get; set; }
    }

    public class LivenessResult
    {
        public string apiExecutionStatus { get; set; }
        public LivenessApiResponse apiResponse { get; set; }
        public bool isFaceDetected { get; set; } 
        public bool areMultipleFacesDetected { get; set; }
        public bool isOtpMatch { get; set; }
        public double otpMatchConfidence { get; set; }
        public string otpText { get; set; }
        public bool isFaceMatch { get; set; }
        public double faceMatchScore { get; set; }
        public bool isFaceConsistent { get; set; }
        public double faceConsistencyScore { get; set; }
        public bool isLive { get; set; }
        public double livenessScore { get; set; }
    }

    public class LivenessRoot
    {
        public string requestId { get; set; }
        public LivenessResult result { get; set; }
        public int statusCode { get; set; }
    }

    public class PatternDetailCheckModel
    {
        public string DISPLAYNAME { get; set; }
        public string RESUMENUMBER { get; set; }
        public string NOOFYEARS { get; set; }
        public string ENTITYID { get; set; }
        public string ADDITIONALINFO { get; set; }
        public string NOOFENTITIES { get; set; }
        public int N_MASTER_ID { get; set; }
        public string CHECKID { get; set; }
        public string MODULETYPE { get; set; }
        public string DOCSREQ { get; set; }
    }

    public class CaseRemarksAPIModel
    {
       public string CHECKDESC { get; set; }
       public string COMMENTS { get; set; }
       public string CHECKID { get; set; }
       public string STATUSCHANGEDDATE { get; set; }
       public string BGVTYPE { get; set; }
       public string CHANGEDBY { get; set; }
    }

    public class GapListModel
    {
        public string GAPFROM { get; set; }
        public string GAPFROMDATE { get; set; }
        public string GAPTO { get; set; }
        public string GAPTODATE { get; set; }
        public string MASTERID { get; set; }
        public string REMARKS { get; set; }
    }

    public class CheckItem
    {
        public string CHECKID { get; set; }
        public string CHECKNAME { get; set; }
        public string CHECKSTATUS { get; set; }
        public List<EntityDoc> entityDocList { get; set; }
        public List<EntityDetail> entitydetails { get; set; }
    }

    public class EntityDoc
    {
        public string UPLOADED_DATE { get; set; }
        public string FILEID { get; set; }
        public string SUBSQUENCEID { get; set; }
        public string FILENAME { get; set; }
    }

    public class EntityDetail
    {
        public int orderNumber { get; set; }
        public string ENTITYID { get; set; }
        public int ENTITYDATAID { get; set; }
        public int SEQUENCEID { get; set; }
        public string subentityid { get; set; }
        public string subentityvalue { get; set; }
        public string subentitydesc { get; set; }
    }


}