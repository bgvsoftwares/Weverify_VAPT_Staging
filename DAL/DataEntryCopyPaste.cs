using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class DataEntryCopyPaste
    {

    }

    public class AddressOCR
    {

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AddressType { get; set; }
        public string IsPermanentAddresssameasCurrentAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PincodeZipCode { get; set; }
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public int DistrictRowID { get; set; }
        public int StateRowID { get; set; }
        public int CountryRowID { get; set; }

    }
    public class CriminalCheckOCR                           
    {                                                        
        public string Title { get; set; }                   
        public string DistrictName { get; set; }                   
        public string StateName { get; set; }                    
        public string CountryName { get; set; }                    
        public int DistrictRowID { get; set; }
        public int StateRowID { get; set; }
        public int CountryRowID { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string DateofBirth { get; set; }
        public string Nationality { get; set; }
        public string MaritalStatus { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PincodeZipCode { get; set; }
        public string Phone { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AddressType { get; set; }
        public string IsPermanentAddresssameasCurrentAddress { get; set; }

    }
    public class EducationCheckOCR
    {

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Educationname { get; set; }
        public string NameOfInstituteCollege { get; set; }
        public string UniversityName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Courseorfieldofstudy { get; set; }
        public string YearofPassing { get; set; }
        public string TypeofEducation { get; set; }
        public string Doyouhavedocumentsasproofofeducation { get; set; }


    }
    public class DatabaseCheckOCR
    {

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Name { get; set; }
        public string DateofBirth { get; set; }
        public string Nationality { get; set; }
        public string MaritalStatus { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PincodeZipCode { get; set; }


    }
    public class EmploymentCheckOCR
    {

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NameofCompanyorEmployer { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string EmployeeCode { get; set; }
        public string OfficialJobTitleHeld { get; set; }
        public string NatureofEmployment { get; set; }
        public string ReasonforLeaving { get; set; }
        public string ModeofPayment { get; set; }
        public string DoyouhavePFNumber { get; set; }
        public string PFNumber { get; set; }
        public string GrossAnnualIncome { get; set; }
        public string DoyouhaveUAN { get; set; }
        public string UAN { get; set; }
        public string DoyouhaveForm16 { get; set; }
        public string Isyourlastworkingdaycompleted { get; set; }
        public string Currentlyworkingforthecompany { get; set; }
        

    }
    public class FasisCheckOCR
    {
        public string CandidateName { get; set; }
        public string FatherName { get; set; }
        public string AddressLine { get; set; }
        public string CountryName { get; set; }
        public string CountryRowID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PincodeZipCode { get; set; }
        public string AddressLine1 { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }

    }
    public class referenceOCR
    {
        public string CandidateName { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceContactNumber { get; set; }
        public string ReferenceEmailID { get; set; }
        public string RelationshipType { get; set; }
        public string NameofOrganisation { get; set; }

    }
}
