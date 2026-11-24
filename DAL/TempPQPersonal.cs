using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TempPQPersonal
    {
        public TempPQPersonal()
        {
            PersonalRowID = 0;
            ClientRowID = 0;
            ClientRefID = string.Empty;
            CompanyRefNo = string.Empty;
            CandidateCode = string.Empty;
            CandTitle = string.Empty;
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
            ProcessName = string.Empty;
            BusinessName = string.Empty;
            Type = string.Empty;
            CandidateUserID = 0;
            SpouseName = string.Empty;
            SpouseMiddleName = string.Empty;
            SpouseLastName = string.Empty;
            CaseStatus = 0;
            Status = 0;
            Agreementsign = false;
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
        public string FromType { get; set; }                    //Client Cases
        public string DetailsEnteredBy { get; set; }            //Company/Client/Candidate
        public string ProcessName { get; set; }
        public string BusinessName { get; set; }
        public string Type { get; set; }
        public string SpouseName { get; set; }
        public string SpouseMiddleName { get; set; }
        public string SpouseLastName { get; set; }
        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public short AllocatedBy { get; set; }
        public short AllocatedToDETL { get; set; }
        public DateTime? AllocatedToDETLDate { get; set; }
        public string Remark { get; set; }
        public byte CaseStatus { get; set; }
        public byte Status { get; set; }
        public string OtherDetails { get; set; }
        public string OtherDetails1 { get; set; }
        public string OtherDetails2 { get; set; }
        public bool Agreementsign { get; set; }
    }
}
