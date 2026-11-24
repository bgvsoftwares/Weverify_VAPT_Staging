using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VendorAPI;

namespace ViewModels.llmtech
{
   public  class llmtech_APIViewModel
    {
    }
    public class ErrorDetails
    {
        public string name { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string type { get; set; }
        public string statusCode { get; set; }

    }
    public class llmtechUANAdditionalDetails
    {
    }

    public class llmtechUANData
    {
        public List<llmtechUANResult> result { get; set; }
        public llmtechUANAdditionalDetails additionalDetails { get; set; }
        public object recentEmploymentSummary { get; set; }
    }

    public class llmtechUANResult
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

    public class llmtechUANRoot
    {
        public bool success { get; set; }
        public llmtechUANData data { get; set; }
        public int responseTime { get; set; }
        public string serviceType { get; set; }
        public string requestId { get; set; }
        public DateTime timestamp { get; set; }
        public string error { get; set; }
        public ErrorDetails errorDetails { get; set; }
    }

    public class llmtechPANRoot
    {
        public bool success { get; set; }
        public PAN_Data data { get; set; }
        public int responseTime { get; set; }
        public string serviceType { get; set; }
        public string requestId { get; set; }
        public DateTime timestamp { get; set; }
        public bool cached { get; set; }
        public string message { get; set; }
    }

    public class PAN_Data
    {
        public string gender { get; set; }
        public string fullname { get; set; }
        public string pan_type { get; set; }
        public string pan_number { get; set; }
        public string date_of_birth { get; set; }
        public int calculated_age { get; set; }
        public string client_ref_num { get; set; }
    }
    public class PAN_To_DINRootobject
    {
        public int http_response_code { get; set; }
        public string request_id { get; set; }
        public string client_ref_num { get; set; }
        public string message { get; set; }
        public int result_code { get; set; }

        public PAN_To_DINResult result { get; set; }
    }

    public class PAN_To_DINResult
    {
        public string din { get; set; }
    }

    public class Bzsdtl
    {
        public string saccd { get; set; }
        public string sdes { get; set; }
    }

    public class gstFilingStatus
    {
        public string fy { get; set; }
        public string taxp { get; set; }
        public string mof { get; set; }
        public string dof { get; set; }
        public string rtntype { get; set; }
        public string arn { get; set; }
        public string status { get; set; }
    }

    public class GoodsService
    {
        public List<object> bzgddtls { get; set; }
        public List<Bzsdtl> bzsdtls { get; set; }
    }

    public class Pradr
    {
        public string adr { get; set; }
    }

    public class gstResult
    {
        public TaxpayerDetails taxpayerDetails { get; set; }
        public TaxpayerReturnDetails taxpayerReturnDetails { get; set; }
        public GoodsService goods_service { get; set; }
    }

    public class gstRoot
    {
        public int http_status_code { get; set; }
        public string client_ref_num { get; set; }
        public string request_id { get; set; }       
        public int result_code { get; set; }
        public string message { get; set; }
        public List<gstResult> result { get; set; }
        public int count { get; set; }
    }

    public class TaxpayerDetails
    {
        public string ntcrbs { get; set; }
        public string adhrVFlag { get; set; }
        public string lgnm { get; set; }
        public string stj { get; set; }
        public string dty { get; set; }
        public string cxdt { get; set; }
        public string gstin { get; set; }
        public List<string> nba { get; set; }
        public string ekycVFlag { get; set; }
        public string cmpRt { get; set; }
        public string rgdt { get; set; }
        public string ctb { get; set; }
        public Pradr pradr { get; set; }
        public string sts { get; set; }
        public string tradeNam { get; set; }
        public string isFieldVisitConducted { get; set; }
        public string ctj { get; set; }
        public string einvoiceStatus { get; set; }
    }

    public class TaxpayerReturnDetails
    {
        public List<gstFilingStatus> filingStatus { get; set; }
        public int array_length { get; set; }
    }



    public class llmtchAadharDetails
    {
        public int AutoId { get; set; }
        public string CompanyRefNo { get; set; }
        public string AadharNo { get; set; }
        public string JsonString { get; set; }
        public string VerifyDate { get; set; }

    }

    public class AadharRootobject
    {
        public bool success { get; set; }
        public AadharData data { get; set; }
        public int responseTime { get; set; }
        public string serviceType { get; set; }
        public string requestId { get; set; }
        public DateTime timestamp { get; set; }
        public bool cached { get; set; }
        public string message { get; set; }
    }

    public class AadharData
    {
        public string state { get; set; }
        public string gender { get; set; }
        public string remarks { get; set; }
        public string age_range { get; set; }
        public bool is_mobile { get; set; }
        public string aadhaar_number { get; set; }
        public string last_digits_of_mobile { get; set; }
    }

}
