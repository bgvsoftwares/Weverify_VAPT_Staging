using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.VendorAPI
{
    public class ViotalUANRoot
    {
        public bool success { get; set; }
        public ViotalUANData data { get; set; }
        public string uan { get; set; }
        public int status { get; set; }
        public string apityp { get; set; }
    }

    public class ViotalUANData
    {
        public List<ViotalUANResult> result { get; set; }
        public ViotalUANAdditionalDetails additionalDetails { get; set; }
    }

    public class ViotalUANResult
    {
        public string uan { get; set; }
        public string memberId { get; set; }
        public string name { get; set; }
        public string fatherOrHusbandName { get; set; }
        public string establishmentId { get; set; }
        public string establishmentName { get; set; }
        public string dateOfJoining { get; set; }
        public string dateOfExit { get; set; }
        public int tenureOfEmployment { get; set; }
    }

    public class ViotalUANAdditionalDetails
    {
        public bool limitedOutput { get; set; }
        public List<UanDataSource> uanDataSource { get; set; }
        public viotalRecentEmployerData recentEmployerData { get; set; }
        public List<Dictionary<string, ViotalUanDetail>> uanDetails { get; set; }
    }

    public class UanDataSource
    {
        public string uan { get; set; }
        public string source { get; set; }
    }

    public class viotalRecentEmployerData
    {
        public string establishmentName { get; set; }
        public string establishmentId { get; set; }
        public string memberId { get; set; }
        public string dateOfExit { get; set; }
        public string dateOfJoining { get; set; }
        public string leaveReason { get; set; }
        public object employerConfidenceScore { get; set; }
    }

    public class ViotalUanDetail
    {
        public ViotalUANBasicDetails basicDetails { get; set; }
        public VIotalEmploymentDetails employmentDetails { get; set; }
    }

    public class ViotalUANBasicDetails
    {
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public object employeeConfidenceScore { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public int aadhaarVerificationStatus { get; set; }
    }

    public class VIotalEmploymentDetails
    {
        public string establishmentName { get; set; }
        public string establishmentId { get; set; }
        public string memberId { get; set; }
        public string dateOfExit { get; set; }
        public string dateOfJoining { get; set; }
        public string leaveReason { get; set; }
        public object employerConfidenceScore { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class ViotalCIBIL_Data
    {
        public int id { get; set; }
        public int client_id { get; set; }
        public object user_id { get; set; }
        public string check_type { get; set; }
        public object reference_id { get; set; }
        public Viotal_Input input { get; set; }
        public string status { get; set; }
        public string cost_deducted { get; set; }
        public int wallet_transaction_id { get; set; }
        public object batch_id { get; set; }
        public Viotal_Result result { get; set; }
        public string report_url { get; set; }
        public object error { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Viotal_Input
    {
        public string pan { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string _client_ip { get; set; }
    }

    public class Viotal_Result
    {
        public string pan { get; set; }
        public string gender { get; set; }
        public string status { get; set; }
        public string full_name { get; set; }
        public string last_name { get; set; }
        public string mobile_no { get; set; }
        public string first_name { get; set; }
        public string score_type { get; set; }
        public string bureau_score { get; set; }
        public string date_of_birth { get; set; }
        public string client_ref_num { get; set; }
        public bool gender_user_provided { get; set; }
    }


    public class ViotalCIBIL_Root
    {
        public bool success { get; set; }
        public ViotalCIBIL_Data data { get; set; }
    }


    public class ViotalCIBILRootobject
    {
        public bool success { get; set; }
        public ViotalCIBILData data { get; set; }
    }

    public class ViotalCIBILData
    {
        public int check_id { get; set; }
        public string status { get; set; }
        public string check_type { get; set; }
    }


    public class ViotalCIBIL
    {
        public int AutoId { get; set; }
        public string CompanyRefNo { get; set; }
        public string PANNo { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string CIBILScore { get; set; }
        public string VerifyDate { get; set; }
    }


}
