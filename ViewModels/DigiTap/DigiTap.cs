using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DigiTap
{
    public class DigiTapUANViewModel
    {
        public int http_response_code { get; set; }
        public string client_ref_num { get; set; }
        public string request_id { get; set; }
        public int result_code { get; set; }
        public string employee_name { get; set; }
        public string employer_name { get; set; }
        public Result result { get; set; }
        public InputData input_data { get; set; }
    }

    public class InputData
    {
        public string uan { get; set; }
    }

    public class Result
    {
        public List<string> uan { get; set; }
        public Summary summary { get; set; }
        public Dictionary<string, UanDetails> uan_details { get; set; }
        public bool partial_output { get; set; }
        public List<UanSource> uan_source { get; set; }
        public object name_dob_filtering_score { get; set; }
    }

    public class Summary
    {
        public EmployerData recent_employer_data { get; set; }
        public string matching_uan { get; set; }
        public bool is_employed { get; set; }
        public object employee_name_match { get; set; }
        public object employer_name_match { get; set; }
        public int uan_count { get; set; }
        public bool date_of_exit_marked { get; set; }
    }

    public class EmployerData
    {
        public string establishment_name { get; set; }
        public string establishment_id { get; set; }
        public string member_id { get; set; }
        public string date_of_exit { get; set; }
        public string date_of_joining { get; set; }
        public string leave_reason { get; set; }
        public object employer_confidence_score { get; set; }
    }

    public class UanDetails
    {
        public BasicDetails basic_details { get; set; }
        public EmployerData employment_details { get; set; }
        public additionaldetails additional_details { get; set; }

        public List<EmploymentHistory> employment_history { get; set; }
    }
    public class additionaldetails
    {
        public string aadhaar { get; set; }
        public string member_id { get; set; }
        public string email { get; set; }
        public string pan { get; set; }
        public string bank_ifsc { get; set; }
        public string bank_acc_no { get; set; }
        public string bank_address { get; set; }
        public string relation { get; set; }
        public string relative_name { get; set; }
    }

    public class BasicDetails
    {
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public object employee_confidence_score { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public int aadhaar_verification_status { get; set; }
    }

    public class EmploymentHistory
    {
        public string establishment_name { get; set; }
        public string establishment_id { get; set; }
        public string member_id { get; set; }
        public string date_of_exit { get; set; }
        public string date_of_joining { get; set; }
        public string leave_reason { get; set; }
        public object employment_period_in_months { get; set; }
        public object employer_confidence_score { get; set; }
    }

    public class UanSource
    {
        public string uan { get; set; }
        public string source { get; set; }
    }
    public class UANDetails
    {
        public int AutoId { get; set; }
        public int PersonalRowId { get; set; }
        public string CompanyRefNo { get; set; }
        public string client_ref_num { get; set; }
        public string request_id { get; set; }
        public string UanNo { get; set; }
        public int is_employed { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string name { get; set; }
        public string relation { get; set; }
        public string relative_name { get; set; }
        public string mobile { get; set; }
        public string VerifiedDate { get; set; }
        public string aadhaar_verification_status { get; set; }

        public List<EmploymentHistory> EmploymentHistory { get; set; }

    }


    public class MobileToUANRootobject
    {
        public int http_response_code { get; set; }
        public string request_id { get; set; }
        public string client_ref_num { get; set; }
        public int result_code { get; set; }
        public Input_Data input_data { get; set; }
        public Result_Source[] result_source { get; set; }
        public long[] result { get; set; }
    }

    public class Input_Data
    {
        public string client_ref_num { get; set; }
        public string mobile { get; set; }
    }
    public class Result_Source
    {
        public string uan { get; set; }
        public string source { get; set; }
    }

}
