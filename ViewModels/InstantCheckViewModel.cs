using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class InstantCheckViewModel
    {
    }
    public class RecentEmployerData
    {
        public string member_id { get; set; }
        public string establishment_id { get; set; }
        public string date_of_exit { get; set; }
        public string date_of_joining { get; set; }
        public string establishment_name { get; set; }
        public string employer_confidence_score { get; set; }
        public string matching_uan { get; set; }
    }

    public class summaryRoot
    {
        public RecentEmployerData recent_employer_data { get; set; }
        public string is_employed { get; set; }
        public string employee_name_match { get; set; }
        public string employer_name_match { get; set; }
        public int uan_count { get; set; }
        public string date_of_exit_marked { get; set; }
    }


    public class BasicDetails
    {
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string employee_confidence_score { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public int aadhaar_verification_status { get; set; }
    }

    public class EmploymentDetails
    {
        public string member_id { get; set; }
        public string establishment_id { get; set; }
        public string date_of_exit { get; set; }
        public string date_of_joining { get; set; }
        public string leave_reason { get; set; }
        public string establishment_name { get; set; }
        public string employer_confidence_score { get; set; }
    }

    public class UANRoot
    {
        public BasicDetails basic_details { get; set; }
        public EmploymentDetails employment_details { get; set; }
    }

    public class UAN_Root
    {
        public string MobileNo { get; set; }
        public string PAN { get; set; }
        public string statuscode { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public summaryRoot summary { get; set; }
        public IEnumerable<EPFO_basic_details_withemployment_details_Lst> basic_details { get; set; }
    }

    public class EPFO_basic_details_withemployment_details_Lst
    {
        public string uan { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string employee_confidence_score { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public int aadhaar_verification_status { get; set; }

        public string member_id { get; set; }
        public string establishment_id { get; set; }
        public string date_of_exit { get; set; }
        public string date_of_joining { get; set; }
        public string leave_reason { get; set; }
        public string establishment_name { get; set; }
        public string employer_confidence_score { get; set; }
    }
    public class passportData
    {
        public string client_id { get; set; }
        public string passport_number { get; set; }
        public string full_name { get; set; }
        public string dob { get; set; }
        public string date_of_application { get; set; }
        public string file_number { get; set; }
    }

    public class passportRoot
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public passportData data { get; set; }
    }
    public class aadhaar_without_otp_object
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public aadhaar_without_otpData data { get; set; }
    }
    public class aadhaar_withoutotpobject
    {
        public string statuscode { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public aadhaarwithoutotpData data { get; set; }
    }

    public class aadhaar_without_otpData
    {
        public string client_id { get; set; }
        public string age_range { get; set; }
        public string aadhaar_number { get; set; }
        public string state { get; set; }
        public string gender { get; set; }
        public string last_digits { get; set; }
        public bool is_mobile { get; set; }
        public string remarks { get; set; }
        public bool less_info { get; set; }
    }
    public class aadhaarwithoutotpData
    {
        public string client_id { get; set; }
        public string age_range { get; set; }
        public string aadhaar_number { get; set; }
        public string state { get; set; }
        public string gender { get; set; }
        public string last_digits { get; set; }
        public string is_mobile { get; set; }
        public string remarks { get; set; }
        public string less_info { get; set; }
    }
    public class PANRootobject
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public PANData data { get; set; }
    }

    public class PANData
    {
        public string client_id { get; set; }
        public string pan_number { get; set; }
        public string full_name { get; set; }
        public string category { get; set; }
    }
    public class PANList
    {
        public int AutoId { get; set; }
        public string CompanyRefNo { get; set; }
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public string client_id { get; set; }
        public string pan_number { get; set; }
        public string full_name { get; set; }
        public string category { get; set; }
    }

    public class objectRoot
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public Datalist data { get; set; }
    }

    public class Datalist
    {
        public string client_id { get; set; }
        public string epic_no { get; set; }
        public string gender { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public string relation_name { get; set; }
        public string relation_type { get; set; }
        public object house_no { get; set; }
        public object dob { get; set; }
        public string age { get; set; }
        public string area { get; set; }
        public string district { get; set; }
        public object[] additional_check { get; set; }
        public string multiple { get; set; }
        public string last_update { get; set; }
        public string assembly_constituency { get; set; }
        public string assembly_constituency_number { get; set; }
        public string polling_station { get; set; }
        public string part_number { get; set; }
        public string part_name { get; set; }
        public string slno_inpart { get; set; }
        public string ps_lat_long { get; set; }
        public string rln_name_v1 { get; set; }
        public string rln_name_v2 { get; set; }
        public string rln_name_v3 { get; set; }
        public string section_no { get; set; }
        public string name_v1 { get; set; }
        public string name_v2 { get; set; }
        public string name_v3 { get; set; }
        public string st_code { get; set; }
        public string parliamentary_constituency { get; set; }
        public string id { get; set; }
    }
    public class Rootobject
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public Employment_HistoryData data { get; set; }
    }

    public class Employment_HistoryData
    {
        public string client_id { get; set; }
        public Employment_History[] employment_history { get; set; }
    }

    public class Employment_History
    {
        public string name { get; set; }
        public string guardian_name { get; set; }
        public string establishment_name { get; set; }
        public string member_id { get; set; }
        public string date_of_joining { get; set; }
        public object date_of_exit { get; set; }
        public object last_pf_submitted { get; set; }
    }

    public class VoterIdList
    {
        public int AutoId { get; set; }
        public string CompanyRefNo { get; set; }
        public string VoterId { get; set; }
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public string client_id { get; set; }
        public string epic_no { get; set; }
        public string gender { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public string relation_name { get; set; }
        public string relation_type { get; set; }
        public string house_no { get; set; }
        public string dob { get; set; }
        public string age { get; set; }
        public string area { get; set; }
        public string district { get; set; }
        public string multiple { get; set; }

        public string last_update { get; set; }
        public string assembly_constituency { get; set; }
        public string assembly_constituency_number { get; set; }
        public string polling_station { get; set; }
        public string part_number { get; set; }
        public string part_name { get; set; }
        public string slno_inpart { get; set; }
        public string ps_lat_long { get; set; }
        public string rln_name_v1 { get; set; }
        public string rln_name_v2 { get; set; }
        public string rln_name_v3 { get; set; }
        public string section_no { get; set; }
        public string name_v1 { get; set; }
        public string name_v2 { get; set; }
        public string name_v3 { get; set; }
        public string st_code { get; set; }
        public string parliamentary_constituency { get; set; }
        public string id { get; set; }
    }

    public class D_license
    {
        public string client_id { get; set; }
        public string license_number { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public string permanent_address { get; set; }
        public string permanent_zip { get; set; }
        public string temporary_address { get; set; }
        public string temporary_zip { get; set; }
        public string citizenship { get; set; }
        public string ola_name { get; set; }
        public string ola_code { get; set; }
        public string gender { get; set; }
        public string father_or_husband_name { get; set; }
        public string dob { get; set; }
        public string doe { get; set; }
        public string transport_doe { get; set; }
        public string doi { get; set; }
        public string transport_doi { get; set; }
        public string profile_image { get; set; }
        public bool has_image { get; set; }
        public string blood_group { get; set; }
        public List<string> vehicle_classes { get; set; }
        public bool less_info { get; set; }
        public List<object> additional_check { get; set; }
        public string initial_doi { get; set; }
        public string current_status { get; set; }
    }

    public class Root_license
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public D_license data { get; set; }
    }


    public class aadhaar_to_uanRootobject
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public aadhaar_to_uanData data { get; set; }
    }

    public class aadhaar_to_uanData
    {
        public string aadhaar_number { get; set; }
        public string client_id { get; set; }
        public string pf_uan { get; set; }
    }

    public class aadhaar_with_otpData
    {
        public string client_id { get; set; }
        public bool otp_sent { get; set; }
        public bool if_number { get; set; }
        public bool valid_aadhaar { get; set; }
        public string status { get; set; }
    }

    public class aadhaar_with_otp_objectRoot
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public aadhaar_with_otpData data { get; set; }
    }
    public class aadhaar_verifyotpAddress
    {
        public string country { get; set; }
        public string dist { get; set; }
        public string state { get; set; }
        public string po { get; set; }
        public string loc { get; set; }
        public string vtc { get; set; }
        public string subdist { get; set; }
        public string street { get; set; }
        public string house { get; set; }
        public string landmark { get; set; }
    }

    public class aadhaar_verifyotpData
    {
        public string client_id { get; set; }
        public string full_name { get; set; }
        public string aadhaar_number { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public aadhaar_verifyotpAddress address { get; set; }
        public bool face_status { get; set; }
        public int face_score { get; set; }
        public string zip { get; set; }
        public string profile_image { get; set; }
        public bool has_image { get; set; }
        public string email_hash { get; set; }
        public string mobile_hash { get; set; }
        public string raw_xml { get; set; }
        public string zip_data { get; set; }
        public string care_of { get; set; }
        public string share_code { get; set; }
        public bool mobile_verified { get; set; }
        public string reference_id { get; set; }
        public object aadhaar_pdf { get; set; }
        public string status { get; set; }
        public string uniqueness_id { get; set; }
    }

    public class aadhaar_verifyotpRoot
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public aadhaar_verifyotpData data { get; set; }
    }
    public class verifyAadhaar
    {
        public int PersonalRowId { get; set; }
        public int InstantId { get; set; }
        public string MobileNo { get; set; }
        [Display(Name = "Aadhaar Number")]
        public string AadhaarNo { get; set; }
        [Display(Name = "Aadhaar OTP")]
        public int otp { get; set; }
        public int AadhaarStatus { get; set; }

    }
    public class InstantMultiCheckSolution
    {
        public int InstantId { get; set; }
        public int PersonalRowId { get; set; }
        public string CompanyRefNo { get; set; }
        public string PanNumber { get; set; }
        public string MobileNo { get; set; }
        public string DocLoCation { get; set; }
        public string DocName { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string PinNo { get; set; }
        public string Landmark { get; set; }
        public bool isAddress { get; set; }
        public int FacialMatchStatus { get; set; }
        public int AadhaarStatus { get; set; }
        public int PANStatus { get; set; }
        public int UANStatus { get; set; }
        public int CRCStatus { get; set; }
        public int CurrentAddressStatus { get; set; }
        public int PermanentAddressStatus { get; set; }
        public int FacialMatchId { get; set; }
        public int AadhaarId { get; set; }
        public int PANId { get; set; }
        public int UANId { get; set; }
        public int CreatedBy { get; set; }
    }


    public class matchData
    {
        public string status { get; set; }
        public string match { get; set; }
    }
    public class FaceMatchRoot
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string reference_id { get; set; }
        public matchData data { get; set; }
    }


}
