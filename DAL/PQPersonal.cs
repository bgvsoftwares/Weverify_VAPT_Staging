using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQPersonal
    {
        public PQPersonal()
        {
            PersonalRowID = 0;
            ClientRowID = 0;
            ClientRefID = string.Empty;
            CompanyRefNo = string.Empty;
            CandidateCode = string.Empty;
            CandTitle = string.Empty;
            CandidateFullName= string.Empty;
            Name = string.Empty;
            MiddleName = string.Empty;
            LastName = string.Empty;
            FatherTitle = string.Empty;
            FatherName = string.Empty;
            FatherMiddleName = string.Empty;
            FatherLastName = string.Empty;
            Gender = string.Empty;
            MaritalStatus = string.Empty;
            MobileNo = string.Empty;
            AlternateNo = string.Empty;
            EmailID = string.Empty;
            AlternateEmailID = string.Empty;
            STD = string.Empty;
            ResidencePhoneNo = string.Empty;
            PanCard = 0;
            PanCardNo = string.Empty;
            Passport = 0;
            PassportNo = string.Empty;
            AadhaarCard = 0;
            AadhaarCardNo = string.Empty;
            DrivingLicense = 0;
            DrivingLicenseNo = string.Empty;
            VoterIDCard = 0;
            VoterIDCardNo = string.Empty;
            CandidateType = string.Empty;
            Catagory = string.Empty;
            CaseType = string.Empty;
            TAT = 0;
            Nationality = string.Empty;
            EmployeeID = string.Empty;
            Designation = string.Empty;
            PlaceofJoining = string.Empty;
            NationalSkills = string.Empty;
            SSNNo = string.Empty;
            CreatedBy = 0;
            Remark = string.Empty;
            FromType = string.Empty;
            ExitIntrvwApplicable = 0;
            ProcessName = string.Empty;
            BusinessName = string.Empty;
            Type = string.Empty;
            CandidateUserID = 0;
            SpouseName = string.Empty;
            SpouseMiddleName = string.Empty;
            SpouseLastName = string.Empty;
            AllocatedBy = 0;
            AllocatedToDEScan = 0;
            AllocatedToDE = 0;
            AllocatedToDEQC = 0;
            DocUploaded = 0;
            CaseStatus = string.Empty;
            FinalStatus = string.Empty;
            FinalColor = string.Empty;
            Status = 1;

            DEScanCaseStatus = string.Empty;
            DECaseStatus = string.Empty;
            DEQCCaseStatus = string.Empty;
            RWQCMgrCaseStatus = string.Empty;
            RWQCManagerTo = 0;
            RWQCTLCaseStatus = string.Empty;
            RWQCTLTo = 0;
            RWQCTMCaseStatus = string.Empty;
            RWQCTMTo = 0;
            FinalRptSendStatus = string.Empty;
            IsReportUploaded = false;
            ReportUploadedName = string.Empty;
            //IsCaseDeleted = false;
            CaseStausID = 0;
        }
        public int PersonalRowID { get; set; }

        public short ClientRowID { get; set; }
        public virtual PQClientMaster PQClientMaster { get; set; }

        public int ClientUserRowID { get; set; }
        public int CandidateUserID { get; set; }
        public string ClientRefID { get; set; }
        public string CompanyRefNo { get; set; }
        public string CandidateCode { get; set; }
        public string CandTitle { get; set; }
        public string CandidateFullName { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherTitle { get; set; }
        public string FatherName { get; set; }
        public string FatherMiddleName { get; set; }
        public string FatherLastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string MaritalStatus { get; set; }
        public string MobileNo { get; set; }
        public string AlternateNo { get; set; }
        public string EmailID { get; set; }
        public string AlternateEmailID { get; set; }
        public string STD { get; set; }
        public string ResidencePhoneNo { get; set; }
        public byte PanCard { get; set; }
        public string PanCardNo { get; set; }
        public byte Passport { get; set; }
        public string PassportNo { get; set; }
        public byte AadhaarCard { get; set; }
        public string AadhaarCardNo { get; set; }
        public byte DrivingLicense { get; set; }
        public string DrivingLicenseNo { get; set; }
        public byte VoterIDCard { get; set; }
        public string VoterIDCardNo { get; set; }
        public DateTime? CheckDateTime { get; set; }
        public string CandidateType { get; set; }
        public string Catagory { get; set; }
        public string CaseType { get; set; }
        public byte TAT { get; set; }
        public string Nationality { get; set; }
        public string EmployeeID { get; set; }
        public string Designation { get; set; }
        public string PlaceofJoining { get; set; }
        public DateTime? DateofJoining { get; set; }
        public string NationalSkills { get; set; }
        public string SSNNo { get; set; }
        public string FromType { get; set; }
        public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate
        public string ProcessName { get; set; }
        public string BusinessName { get; set; }
        public string Type { get; set; }
        public string SpouseName { get; set; }
        public string SpouseMiddleName { get; set; }
        public string SpouseLastName { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public short AllocatedBy { get; set; }
        public short AllocatedToDEScan { get; set; }
        public DateTime? AllocatedToDEScanDate { get; set; }
        public short AllocatedToDE { get; set; }
        public DateTime? AllocatedToDEDate { get; set; }
        public short AllocatedToDEQC { get; set; }
        public DateTime? AllocatedToDEQCDate { get; set; }
        public byte DocUploaded { get; set; }
        public string CaseStatus { get; set; }
        public string FinalStatus { get; set; }
        public string FinalColor { get; set; }
        public byte ExitIntrvwApplicable { get; set; }        // yes=1/No=0
        public string Remark { get; set; }
        public byte Status { get; set; }
        public string OtherDetails { get; set; }
        public string OtherDetails1 { get; set; }
        public string OtherDetails2 { get; set; }

        public string DEScanCaseStatus { get; set; }
        public string DECaseStatus { get; set; }
        public string DEQCCaseStatus { get; set; }

        public string RWQCMgrCaseStatus { get; set; }
        public short RWQCManagerTo { get; set; }
        public DateTime? RWQCManagerDate { get; set; }

        public string RWQCTLCaseStatus { get; set; }
        public short RWQCTLTo { get; set; }
        public DateTime? RWQCTLDate { get; set; }

        public string RWQCTMCaseStatus { get; set; }
        public short RWQCTMTo { get; set; }
        public DateTime? RWQCTMDate { get; set; }

        public string FinalRptSendStatus { get; set; }
        public DateTime? FinalRptSendDate { get; set; }

        public string InterimRptSendStatus { get; set; }
        public DateTime? InterimRptSendDate { get; set; }

        public DateTime? DEScanCompletedDate { get; set; }
        public  DateTime? DECompletedDate { get; set; }
        public DateTime? DEQCCompletedDate { get; set; }
        public DateTime? RWQCTMCompletedDate { get; set; }

        public bool IsReportUploaded { get; set; }
        public string ReportUploadedName { get; set; }
        public DateTime? ReportUploadedDate { get; set; }
        //public bool IsCaseDeleted { get; set; }
        public int CaseStausID { get; set; }
    }
}
