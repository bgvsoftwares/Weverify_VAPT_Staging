using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ViewModels.ProvidedInfoViewModel
{
    public class PQPersonalInfoViewModel
    {
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }
        public string EncPersonalID { get; set; }
        public int ClientRowID { get; set; }
        public int SubCheckId { get; set; }
        [Display(Name = "Client Name : ")]
        public string ClientName { get; set; }
        [Display(Name = "Title : ")]
        public string CandTitle { get; set; }
        [Required]
        [Display(Name = "Name : ")]
        public string Name { get; set; }
        [Display(Name = "Gender : ")]
        public string Gender { get; set; }
        [Display(Name = "Father Name : ")]
        public string FatherName { get; set; }
        [Display(Name = "Date Of Birth : ")]
        public string DOB { get; set; }
        [Display(Name = "Mobile No : ")]
        public string MobileNo { get; set; }
        [Display(Name = "Alternate No. : ")]
        public string AlternateNo { get; set; }
        [Display(Name = "Official Email : ")]
        public string EmailID { get; set; }
        [Display(Name = "Personal Email : ")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string AlternateEmailID { get; set; }
        [Display(Name = "Duration Of Stay : ")]
        public string DurationOfStay { get; set; }
        [Display(Name = "ID Type : ")]
        public string IDType { get; set; }
        [Display(Name = "ID Number : ")]
        public string IDNumber { get; set; }
        [Display(Name = "Pan Card : ")]
        public int PanCard { get; set; }
        [Display(Name = "Pan Card : ")]
        public string PanCardNo { get; set; }
        [Display(Name = "Passport : ")]
        public int Passport { get; set; }
        [Display(Name = "Passport No: ")]
        public string PassportNo { get; set; }
        [Display(Name = "Aadhaar Card : ")]
        public int AadhaarCard { get; set; }
        [Display(Name = "Aadhaar Card : ")]
        public string AadhaarCardNo { get; set; }
        [Display(Name = "Voter ID Card : ")]
        public int VoterIDCard { get; set; }
        [Display(Name = "Voter ID Card : ")]
        public string VoterIDCardNo { get; set; }
        [Display(Name = "Address : ")]
        public string Address { get; set; }
        [Display(Name = "Name")]
        public string LOA_Name { get; set; }
        [Display(Name = "Signature : ")]
        public string hfSign { get; set; }
        public string LOA_TimeStamp { get; set; }

    }
    public class PQPersonalViewModel
    {
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [Display(Name = "Client Ref ID : ")]
        public string ClientRefID { get; set; }

        [Display(Name = "Location : ")]
        public string Location { get; set; }

        [Display(Name = "Securitas ID : ")]
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

        [Display(Name = "Email : ")]
        public string EmailID { get; set; }

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

        [Display(Name = "Create By : ")]
        public short CreatedBy { get; set; }

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
    }

    public class AddPQPersonalViewModel
    {
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        [Required(ErrorMessage = "Select Client")]
        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }

        [Display(Name = "ID : ")]
        public int ClientUserRowID { get; set; }

        [Display(Name = "Client Ref ID : ")]
        [MaxLength(100)]
        public string ClientRefID { get; set; }

        [Display(Name = "Location : ")]
        public string Location { get; set; }

        [Display(Name = "Securitas ID : ")]
        [MaxLength(100)]
        public string CompanyRefNo { get; set; }

        [Required(ErrorMessage = "Enter candidate code")]
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

        [Display(Name = "Title : ")]
        [MaxLength(15)]
        public string FatherTitle { get; set; }

        // ── Client 684 ──
        public string OfferType { get; set; }
        // ── end 684 ──

        public string Entity { get; set; }

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

        [Display(Name = "Date Of Birth : ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }

        [Display(Name = "Marital Status : ")]
        [MaxLength(15)]
        public string MaritalStatus { get; set; }

        //[Required(ErrorMessage = "Enter mobile number")]
        [Display(Name = "Mobile No : ")]
        [StringLength(20)]
        [RegularExpression(@"^[6-9]{1}.*[0-9]{9}$", ErrorMessage = "Please enter valid Mobile Number")]
        public string MobileNo { get; set; }

        //[Required(ErrorMessage = "Enter Email id")]
        [Display(Name = "Email : ")]
        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string EmailID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Case Type : ")]
        public string CaseType { get; set; }

        [Display(Name = "TAT : ")]
        public byte TAT { get; set; }

        [Display(Name = "Order Date : ")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Delivery Date : ")]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Is Back Date : ")]
        public bool IsBackdate { get; set; }

        [Display(Name = "How Many Back Days : ")]
        public byte HowManyBackDays { get; set; }

        [Display(Name = "Pan Card : ")]
        public byte PanCard { get; set; }

        [Display(Name = "Designation : ")]
        [MaxLength(10)]
        public string Designation { get; set; }


        [Display(Name = "Pan Card : ")]
        [RegularExpression(@"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$", ErrorMessage = "Please enter valid Pan Card")]
        [MaxLength(10)]
        public string PanCardNo { get; set; }

        [Display(Name = "Passport : ")]
        public byte Passport { get; set; }

        [Display(Name = "Passport : ")]
        [MaxLength(100)]
        public string PassportNo { get; set; }

        [Display(Name = "Aadhaar Card : ")]
        public byte AadhaarCard { get; set; }

        [Display(Name = "Aadhaar Card : ")]
        // [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Please enter valid Aadhaar Card")]
        [MaxLength(12)]
        public string AadhaarCardNo { get; set; }

        [Display(Name = "Driving License : ")]
        public byte DrivingLicense { get; set; }

        [Display(Name = "Driving License : ")]
        [MaxLength(100)]
        public string DrivingLicenseNo { get; set; }

        [Display(Name = "Voter ID Card : ")]
        public byte VoterIDCard { get; set; }

        [Display(Name = "Voter ID Card : ")]
        [MaxLength(100)]
        public string VoterIDCardNo { get; set; }

        [Display(Name = "CheckDateTime : ")]
        public DateTime? CheckDateTime { get; set; }

        [MaxLength(100)]
        [Display(Name = "EmployeeID : ")]
        public string EmployeeID { get; set; }

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

        [Display(Name = "Case Status : ")]
        public string CaseStatus { get; set; }

        //[Display(Name = "Final Status : ")]
        //public byte FinalStatus { get; set; }

        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        public byte Status { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase UploadDoc { get; set; }

        [MaxLength(100)]
        [Display(Name = "OtherDetails : ")]
        public string OtherDetails { get; set; }

        public string CandidateCheckOrPackage { get; set; }

        [Display(Name = "Previous Refernce Id : ")]
        public string OtherDetails1 { get; set; }

        [Display(Name = "MasterCaseStausID : ")]
        public int MasterCaseStausID { get; set; }
        public IEnumerable<AddPQPersonalViewModel> CandidateDuplicateList { get; set; }
        public string CandidateName { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public string ProcessName { get; set; }
        public string BusinessName { get; set; }
        public string Band { get; set; }
        public string GenpactBand { get; set; }
        public string CampusCase { get; set; }
        public string NationalSkills { get; set; }
        public string SSNNo { get; set; }
        public string PackageOrCheckDetails { get; set; }
        public string CreatedBySource { get; set; }
        public string TCSClientType { get; set; }

    }

    public class AddPQPersonalBulkImportViewModel
    {
        [AllowHtml]
        public string UAN { get; set; }

        [AllowHtml]
        public string AccountName { get; set; }
        [AllowHtml]
        public string PatternName { get; set; }
        [AllowHtml]
        public string UID { get; set; }
        [AllowHtml]
        public string Education { get; set; }
        [AllowHtml]
        public string Employment { get; set; }
        [AllowHtml]
        public string Address { get; set; }
        [AllowHtml]
        public string Criminal { get; set; }
        [AllowHtml]
        public string Identity { get; set; }
        [AllowHtml]
        public string Database { get; set; }
        [AllowHtml]
        public string Additional_Professional_Certifications { get; set; }
        [AllowHtml]
        public string Drug_Test_Verification { get; set; }
        [AllowHtml]
        public string Credit_History_Verification { get; set; }
        [AllowHtml]
        public string Family_Relationship { get; set; }
        [AllowHtml]
        public string External_Directorship { get; set; }
        [AllowHtml]
        public string Self_Employment_Directorship { get; set; }
        [AllowHtml]
        public string Facis_III { get; set; }
        [AllowHtml]
        public string Entitlement_to_Work { get; set; }
        [AllowHtml]
        public string Professional_Reference_Verification { get; set; }
        [AllowHtml]
        public string Professional_License_Check { get; set; }
        [AllowHtml]
        public string OHR_Taleo_ID { get; set; }
        [AllowHtml]
        public string Band { get; set; }
        [AllowHtml]
        public string Designation_Genpact { get; set; }
        [AllowHtml]
        public string PID { get; set; }
        [AllowHtml]
        public string SDO_Code { get; set; }
        [AllowHtml]
        public string COE_Code { get; set; }
        [AllowHtml]
        public string Grade { get; set; }

        [Display(Name = "ID : ")]
        [AllowHtml]
        public int PersonalRowID { get; set; }

        [Required(ErrorMessage = "Select Client")]
        [Display(Name = "Client Name : ")]
        [AllowHtml]
        public short ClientRowID { get; set; }

        [Display(Name = "ID : ")]
        [AllowHtml]
        public int ClientUserRowID { get; set; }

        [Display(Name = "Client Ref ID : ")]
        [MaxLength(100)]
        [AllowHtml]
        public string ClientRefID { get; set; }

        [Display(Name = "Location : ")]
        [AllowHtml]
        public string Location { get; set; }

        [Display(Name = "Roles : ")]
        [AllowHtml]
        public string Roles { get; set; }

        [Display(Name = "Securitas ID : ")]
        [MaxLength(100)]
        [AllowHtml]
        public string CompanyRefNo { get; set; }

        [Required(ErrorMessage = "Enter candidate code")]
        [Display(Name = "Candidate Code : ")]
        [MaxLength(100)]
        [AllowHtml]
        public string CandidateCode { get; set; }

        [Required(ErrorMessage = "Select title")]
        [Display(Name = "Title : ")]
        [MaxLength(15)]
        [AllowHtml]
        public string CandTitle { get; set; }

        [Required(ErrorMessage = "Enter Candidate Name")]
        [Display(Name = "Name : ")]
        [MaxLength(50)]
        [AllowHtml]
        public string Name { get; set; }

        [Display(Name = "Middle Name : ")]
        [MaxLength(50)]
        [AllowHtml]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name : ")]
        [MaxLength(50)]
        [AllowHtml]
        public string LastName { get; set; }

        [Display(Name = "Gender : ")]
        [MaxLength(10)]
        [AllowHtml]
        public string Gender { get; set; }

        [Display(Name = "Date Of Birth : ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [AllowHtml]
        public DateTime? DOB { get; set; }

        [Display(Name = "Marital Status : ")]
        [MaxLength(15)]
        [AllowHtml]
        public string MaritalStatus { get; set; }

        [Required(ErrorMessage = "Enter mobile number")]
        [Display(Name = "Mobile No : ")]
        [StringLength(20)]
        [RegularExpression(@"^[7-9]{1}.*[0-9]{9}$", ErrorMessage = "Please enter valid Mobile Number")]
        [AllowHtml]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Enter Email id")]
        [Display(Name = "Email : ")]
        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        [AllowHtml]
        public string EmailID { get; set; }

        [MaxLength(100)]
        [Display(Name = "Alternate Email : ")]
        [AllowHtml]
        public string AlternateEmailID { get; set; }
        [AllowHtml]
        public string EmployeeID { get; set; }
        [AllowHtml]
        public string CaseType { get; set; }
        [AllowHtml]
        public string FatherName { get; set; }
        [AllowHtml]
        public string PackageOrCheckDetails { get; set; }

        [Required]
        [AllowHtml]
        public byte TAT { get; set; }
        [AllowHtml]
        public DateTime? OrderDate { get; set; }

        [Required]
        [AllowHtml]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Pan Card : ")]
        [RegularExpression(@"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$", ErrorMessage = "Please enter valid Pan Card")]
        [MaxLength(10)]
        [AllowHtml]
        public string PanCardNo { get; set; }

        [Display(Name = "Passport : ")]
        [MaxLength(100)]
        [AllowHtml]
        public string PassportNo { get; set; }

        [Display(Name = "Aadhaar Card : ")]
        [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Please enter valid Aadhaar Card")]
        [MaxLength(12)]
        [AllowHtml]
        public string AadhaarCardNo { get; set; }

        [Display(Name = "Driving License : ")]
        [MaxLength(100)]
        [AllowHtml]
        public string DrivingLicenseNo { get; set; }

        [Display(Name = "Voter ID Card : ")]
        [MaxLength(100)]
        [AllowHtml]
        public string VoterIDCardNo { get; set; }

        [MaxLength(20)]
        [Display(Name = "From Type : ")]
        [AllowHtml]
        public string FromType { get; set; }

        [MaxLength(100)]
        [Display(Name = "Details Entered By : ")]
        [AllowHtml]
        public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate

        [Display(Name = "Create By : ")]
        [AllowHtml]
        public short CreatedBy { get; set; }

        [Display(Name = "Create Date : ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [AllowHtml]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Case Status : ")]
        [AllowHtml]
        public string CaseStatus { get; set; }

        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        [AllowHtml]
        public byte Status { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        [AllowHtml]
        public HttpPostedFileBase UploadDoc { get; set; }

        [ValidateLargeFileCandidateUploadDocs]
        [AllowHtml]
        public HttpPostedFileBase UploadDoc1 { get; set; }

        [Display(Name = "Template : ")]
        [AllowHtml]
        public string Template { get; set; }

        [MaxLength(100)]
        [Display(Name = "OtherDetails : ")]
        [AllowHtml]
        public string OtherDetails { get; set; }
        [AllowHtml]
        public string CandidateType { get; set; }
        [AllowHtml]
        public string CandidateCheckOrPackage { get; set; }
        [AllowHtml]
        public string ClientUserName { get; set; }
        [AllowHtml]
        public DateTime? DateofJoining { get; set; }
        [AllowHtml]
        public string PlaceOfJoining { get; set; }
        [AllowHtml]
        public string AlternateNo { get; set; }
        [AllowHtml]
        public string ProcessName { get; set; }
        [AllowHtml]
        public string ProjectId { get; set; }
        [AllowHtml]
        public string ProjectName { get; set; }
        [AllowHtml]
        public string CreatedBySource { get; set; }
        [AllowHtml]
        public string BatchFileName { get; set; }
        [AllowHtml]
        public string Entity { get; set; }
        [AllowHtml]
        public string Designation { get; set; }
        [AllowHtml]

        public string Region { get; set; }
        [AllowHtml]

        public string State { get; set; }
        [AllowHtml]

        public string HubLocation { get; set; }
        [AllowHtml]
        public IEnumerable<AddPQPersonalBulkImportViewModel> CandidateDuplicateList { get; set; }
        public string PAdd_Country { get; set; }
    }

    public class UpdatePQPersonalViewModel
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

        [Display(Name = "Securitas ID : ")]
        [MaxLength(100)]
        public string CompanyRefNo { get; set; }

        [Display(Name = "Candidate Code : ")]
        [MaxLength(100)]
        public string CandidateCode { get; set; }

        [Display(Name = "Title : ")]
        [MaxLength(15)]
        public string CandTitle { get; set; }

        [Required]
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

        [Display(Name = "Date Of Birth : ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }

        [Display(Name = "Marital Status : ")]
        [MaxLength(15)]
        public string MaritalStatus { get; set; }

        [Display(Name = "Mobile No : ")]
        [StringLength(20)]
        [RegularExpression(@"^[7-9]{1}.*[0-9]{9}$", ErrorMessage = "Please enter valid Mobile Number")]
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

        [Display(Name = "Pan Card : ")]
        [MaxLength(100)]
        public string PanCardNo { get; set; }

        [Display(Name = "Passport : ")]
        public byte Passport { get; set; }

        [Display(Name = "Passport : ")]
        [MaxLength(100)]
        public string PassportNo { get; set; }

        [Display(Name = "Aadhaar Card : ")]
        public byte AadhaarCard { get; set; }

        [Display(Name = "Aadhaar Card : ")]
        [MaxLength(100)]
        public string AadhaarCardNo { get; set; }

        [Display(Name = "Driving License : ")]
        public byte DrivingLicense { get; set; }

        [Display(Name = "Driving License : ")]
        [MaxLength(100)]
        public string DrivingLicenseNo { get; set; }

        [Display(Name = "Voter ID Card : ")]
        public byte VoterIDCard { get; set; }

        [Display(Name = "Voter ID Card : ")]
        [MaxLength(100)]
        public string VoterIDCardNo { get; set; }

        [Display(Name = "CheckDateTime : ")]
        public DateTime? CheckDateTime { get; set; }

        [MaxLength(50)]
        [Display(Name = "Candidate Type : ")]
        public string CandidateType { get; set; }

        [MaxLength(50)]
        [Display(Name = "Category : ")]
        public string Catagory { get; set; }

        //[Required]
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

        [Display(Name = "DEQC Date : ")]
        public DateTime? AllocatedToDEQCDate { get; set; }

        [Display(Name = "DE-Scan : ")]
        public short AllocatedToDEScan { get; set; }

        [Display(Name = "DE-Scan Date : ")]
        public DateTime? AllocatedToDEScanDate { get; set; }

        [MaxLength(200)]
        [Display(Name = "Remark : ")]
        [AllowHtml]
        public string Remark { get; set; }

        [MaxLength(50)]
        [Display(Name = "Final Color : ")]
        public string FinalColor { get; set; }

        [Display(Name = "ExitIntrvwApplicable : ")]
        public byte ExitIntrvwApplicable { get; set; }        // yes=1/No=0

        [Display(Name = "Case Status : ")]
        public string CaseStatus { get; set; }

        [Display(Name = "Final Status : ")]
        public string FinalStatus { get; set; }

        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        public byte Status { get; set; }

        [MaxLength(100)]
        [Display(Name = "OtherDetails : ")]
        public string OtherDetails { get; set; }

        [MaxLength(100)]
        [Display(Name = "OtherDetails1 : ")]
        public string OtherDetails1 { get; set; }

        [MaxLength(100)]
        [Display(Name = "OtherDetails2 : ")]
        public string OtherDetails2 { get; set; }
    }

    public class PQPersonalDetailViewModel
    {
        [Display(Name = "ID : ")]
        public int PersonalRowID { get; set; }

        public string EPersonalRowID { get; set; }

        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }
        public string EClientRowID { get; set; }

        public string IsSeniorCase { get; set; }
        public string PackageNamee { get; set; }
        public string EntityNamee { get; set; }
        public string ClientName { get; set; }

        [Display(Name = "Client Ref ID : ")]
        public string ClientRefID { get; set; }

        [Display(Name = "Securitas ID : ")]
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

        [Display(Name = "Details Entered By : ")]
        public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate


        [Display(Name = "Create By : ")]
        public short CreatedBy { get; set; }

        [Display(Name = "Create Date : ")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Rejection Date : ")]
        public DateTime? RejectionDate { get; set; }

        [Display(Name = "Allocated By : ")]
        public short AllocatedBy { get; set; }

        [Display(Name = "Allocated To DE : ")]
        public short AllocatedToDE { get; set; }
        public string AllocatedToDEName { get; set; }

        [Display(Name = "Allocated To DE Date : ")]
        public DateTime? AllocatedToDEDate { get; set; }

        [Display(Name = "Allocated To DEQC : ")]
        public short AllocatedToDEQC { get; set; }
        public string AllocatedToDEQCName { get; set; }

        [Display(Name = "Allocated To DE-Scan : ")]
        public short AllocatedToDEScan { get; set; }
        public string AllocatedToDEScanName { get; set; }

        [Display(Name = "Allocated Date : ")]
        public DateTime? AllocatedToDEScanDate { get; set; }

        [Display(Name = "Allocated To DEQC Date : ")]
        public DateTime? AllocatedToDEQCDate { get; set; }

        public string CaseStatus { get; set; }

        public string FineStatus { get; set; }

        [Display(Name = "Status : ")]
        [ScaffoldColumn(false)]
        public byte Status { get; set; }

        [Display(Name = "OtherDetails : ")]
        public string OtherDetails { get; set; }

        [Display(Name = "OtherDetails1 : ")]
        public string OtherDetails1 { get; set; }

        [Display(Name = "OtherDetails2 : ")]
        public string OtherDetails2 { get; set; }

        public string DEScanCaseStatus { get; set; }
        public string DECaseStatus { get; set; }
        public string DEQCCaseStatus { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public int SubCheckRowID { get; set; }
        public string CheckStatusId { get; set; }
        public string ECheckStatusId { get; set; }
        public string CaseStatusId { get; set; }
        public bool ReportVisibality { get; set; }
        public bool IsReportUploaded { get; set; }
        public int ClientUserRowId { get; set; }
        public string Package { get; set; }
        public string Emailid { get; set; }
        public string Entity { get; set; }
        public string MobileNo { get; set; }
        public string EmployeeID { get; set; }
        public string ModuleType { get; set; }
    }

    public class PQPersonalInfoListPagedModel
    {
        public IEnumerable<PQPersonalDetailViewModel> PQPersonalInfo { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public PQPersonalSearchPagedModel ItemSearch { get; set; }
    }

    public class PQPersonalSearchPagedModel
    {
        public string ClientRowId { get; set; }
        public string SearchType { get; set; }
        public string Search { get; set; }
        public string DataEntryScannerId { get; set; }
        public string DataEntryId { get; set; }
        public string QualityCheckId { get; set; }
        public string RecievingFromDate { get; set; }
        public string CompletedFromDate { get; set; }
        public string RecievingToDate { get; set; }
        public string CompletedToDate { get; set; }
        public string Initiation { get; set; }
        public string UploadfromDate { get; set; }
        public string UploadToDate { get; set; }
        public string YTRDate { get; set; }

    }

    public class PQAllChecksCountViewModel
    {
        public int AddressCount { get; set; }
        public int EmploymentCount { get; set; }
        public int EducationCount { get; set; }
        public int CriminalCount { get; set; }
        public int NatIdentityCount { get; set; }
        public int ReferenceCount { get; set; }
        public int SpecialcheckCount { get; set; }

    }

    public class PQPersonalSessionViewModel
    {
        public int PersonalRowID { get; set; }
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string ClientRefID { get; set; }
        public string CandidateCode { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandTitle { get; set; }
        public string CandidateFullName { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string FatherMiddleName { get; set; }
        public string FatherLastName { get; set; }
        public string SpouseName { get; set; }
        public string SpouseMiddleName { get; set; }
        public string SpouseLastName { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string PanCardNo { get; set; }
        public string PassportNo { get; set; }
        public string AadhaarCardNo { get; set; }
        public string DrivingLicenseNo { get; set; }
        public string VoterIDCardNo { get; set; }
        public int ClientUserRowId { get; set; }
    }

    public class TempPQPersonalSessionViewModel
    {
        public int PersonalRowID { get; set; }
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string ClientRefID { get; set; }
        public string CandidateCode { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandTitle { get; set; }

        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string FatherMiddleName { get; set; }
        public string FatherLastName { get; set; }
        public string SpouseName { get; set; }
        public string SpouseMiddleName { get; set; }
        public string SpouseLastName { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string PanCardNo { get; set; }
        public string PassportNo { get; set; }
        public string AadhaarCardNo { get; set; }
        public string DrivingLicenseNo { get; set; }
        public string VoterIDCardNo { get; set; }
    }

    public class BGVInstructionslist
    {
        public string SubCheckName { get; set; }
        public string DocumentForVerification { get; set; }
    }

    public class PQPersonalForSendMailViewModel
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

        [Display(Name = "OtherDetails1 : ")]
        public string OtherDetails2 { get; set; }
    }

    public class PQPersonalOrCheckForSendMailViewModel
    {
        public int PersonalRowID { get; set; }

        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        public string Tenure { get; set; }
        public string EmployerName { get; set; }
        public string Qualification { get; set; }
        public string university { get; set; }
        public string Amount { get; set; }
        public string DocuemtRequired { get; set; }
        public string ExpectedTAT { get; set; }


        public short CheckFamilyRowID { get; set; }
        public string CheckFamilyName { get; set; }

        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }

        public DateTime? InternalTAT { get; set; }

        public string ClientRefID { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandidateCode { get; set; }
        public string CandidateName { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }


        public string ClientEmailID { get; set; }
        public string SpocEmailID { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        [AllowHtml]
        public string InsuffRemarks { get; set; }
        public string Verificationsource { get; set; }
        public int ClientUserRowID { get; set; }
        public int CandidateIncludeInInsuffMail { get; set; }
        public string locationName { get; set; }
        public string AlternateEmailID { get; set; }
        public string verificationid { get; set; }
        public string PackageName { get; set; }
    }

    public class PQPersonalCheckListForClientViewModel
    {
        public int PersonalRowID { get; set; }

        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        public short CheckFamilyRowID { get; set; }
        public string CheckFamilyName { get; set; }

        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }

        public string CompanyRefNo { get; set; }
        public string CandidateCode { get; set; }
        public string CandidateName { get; set; }
        public string ColorName { get; set; }
        public string CheckStatus { get; set; }
        public string Result { get; set; }
        public string IdentityName { get; set; }
        public string CompleteDate { get; set; }
    }

    public class ExportPQPersonalDetailViewModel
    {
        public int PersonalRowID { get; set; }
        public string CandidateName { get; set; }
        public string casestatusname { get; set; }
        public string CandidateCode { get; set; }
        public string SecuritasID { get; set; }
        public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate        
        public string CaseStatus { get; set; }
        public string ClientRefID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public string PositionID { get; set; }
        public string CostCenter { get; set; }
        public string EmployeeID { get; set; }
        public string CheckType { get; set; }
        public string InternalStatus { get; set; }
        public string CheckID { get; set; }


    }

    public class ExportPQPersonalCandidateViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }
        public string CandidateName { get; set; }
        public string CheckName { get; set; }
        public string HrRecuiterName { get; set; }

        public string CandidateCode { get; set; }
        public string CompanyRefNo { get; set; }
        public string CaseStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ReportType { get; set; }
        public DateTime? ReportDate { get; set; }
        public string ClientRefID { get; set; }
        public string ClientLocation { get; set; }
        public string EmployeeID { get; set; }
        public string ContactNo { get; set; }
        public DateTime? CaseInitiationDate { get; set; }
        public string FinalStatus { get; set; }
        public DateTime? reportSendDate { get; set; }
        public string CaseColorStatus { get; set; }
        public string GSTColorName { get; set; }
        public string GSTComments { get; set; }
        public string AddressProofColorName { get; set; }
        public string AddressProofComments { get; set; }
        public string BusinessAddColorName { get; set; }

        public string BusinessAddComments { get; set; }
        public string PANNoVerificationcolorCode { get; set; }
        public string PANNoVerificationComments { get; set; }
        public string InterimDate { get; set; }
        public string InterimColor { get; set; }
        public string YTRdate { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public string Reference { get; set; }
        public string EntityName { get; set; }
        public string CandidateType { get; set; }
        public string PackageOrCheckDetails { get; set; }
        public string CheckComponentID { get; set; }

        public string InternalStatus { get; set; }
        public string InsuffRaiseDate { get; set; }

        public string InsuffRemarks { get; set; }





    }

    public class ReportDownloadID
    {
        public string personalrowid { get; set; }
    }
    public class ReportDownload
    {
        public string ClientRowId { get; set; }
        public string CompanyRefNo { get; set; }
        public string reportId { get; set; }
        public string reportName { get; set; }
        public string reportType { get; set; }
    }

    public class ClientsActivation
    {
        [Required(ErrorMessage = "Select Client")]
        [Display(Name = "Client Name : ")]
        public int ClientRowID { get; set; }

        [Display(Name = "Remark : ")]
        [AllowHtml]
        public string Remark { get; set; }


        public string ActiveStatus { get; set; }
        [Display(Name = "Approval Mail : ")]

        public HttpPostedFileBase ApprovalMail { get; set; }

        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class QuickCaseSearch
    {
        public int CaseStatusID { get; set; }
        public string CaseStatus { get; set; }
        public string CreatedDate { get; set; }
        public string ColorCode { get; set; }
        public string FinalReport { get; set; }
        public string UploadedDate { get; set; }
        public string PersonalRowID { get; set; }
        public string companyrefno { get; set; }
    }

    public class GetLocationByPin
    {
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public int DistrictRowID { get; set; }
        public int StateRowID { get; set; }
        public int CountryRowID { get; set; }
    }

    public class Candidatelist
    {
        public int ClientRowID { get; set; }
        public string CandidateFullName { get; set; }
    }
    public class ImportViewModel
    {
        public short ClientRowID { get; set; }
        public HttpPostedFileBase UploadDoc { get; set; }

    }
    public class CompressimageViewModel
    {
        [ValidateLargeFileCandidateUploadDocs]
        public HttpPostedFileBase UploadDoc { get; set; }
    }
    public class CandidateCaseDetailsWithSoruce
    {
        public int PersonalRowID { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandidateName { get; set; }
        public string ClientRefNo { get; set; }
        public string BatchFilename { get; set; }
        public string IsReportUploaded { get; set; }
        public string ReportUploadedName { get; set; }
        public string ReportUploadedDate { get; set; }


    }
    public class BatchFileList
    {
        public string BatchFileId { get; set; }
        public string BatchFileName { get; set; }
    }
    public class UserEmailDetails
    {
        public int Teammemberid { get; set; }

        public int ClientUserRowID { get; set; }
        public string UserID { get; set; }

        public string ClientName { get; set; }
        public string UserEmailID { get; set; }
        public string CaseInitiationMail { get; set; }
        public string SendInsuffEmail { get; set; }
        public string SendRedReportEmail { get; set; }
        public string SendFundsApprovedMail { get; set; }
        public string SendYTRMail { get; set; }
        public string SendReportEmail { get; set; }
    }
    public class GetClietDetails
    {
        public int ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public string Address { get; set; }

    }

    }