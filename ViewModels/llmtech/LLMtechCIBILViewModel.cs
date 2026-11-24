using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.llmtech
{
    class LLMtechCIBILViewModel
    {
    }

    public class CIBILRoot
    {
        [JsonProperty("success")]   
        public bool success { get; set; }

        [JsonProperty("data")]
        public CIBILData data { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }

        [JsonProperty("responseTime")]
        public int responseTime { get; set; }

        [JsonProperty("serviceType")]
        public string serviceType { get; set; }

        [JsonProperty("requestId")]
        public string requestId { get; set; }

        [JsonProperty("timestamp")]
        public string timestamp { get; set; }

        [JsonProperty("billable")]
        public bool billable { get; set; }

        [JsonProperty("result_code")]
        public int resultCode { get; set; }

        [JsonProperty("request_id")]
        public string request_id { get; set; }

        [JsonProperty("client_ref_num")]
        public string client_ref_num { get; set; }
    }
     public class CIBILData
    {
        public string status { get; set; }
        public string bureau_score { get; set; }
        public string result_xml { get; set; }
        public string result_pdf { get; set; }
    }

    public class CIBILCheckResult
    {
        public string credit_score { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
    }

}
