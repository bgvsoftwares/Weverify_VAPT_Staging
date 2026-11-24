using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Perfios
{
    public class Aadhaar_verification
    {

    }

    public class Aadhaar_verificationRoot
    {
        public Result result { get; set; }
        public string request_id { get; set; }
        [JsonProperty("status-code")]
        public string statuscode { get; set; }
        public Clientdata clientData { get; set; }
    }

    public class Result
    {
        public string mobile { get; set; }
        public string gender { get; set; }
        public string ageBand { get; set; }
        public string state { get; set; }
        public string isIssued { get; set; }
    }

    public class Clientdata
    {
        public string caseId { get; set; }
    }




    public class aadhaarconsentRootobject
    {
        public string requestId { get; set; }
        public consentResult result { get; set; }
        [JsonProperty("status-code")]
        public int statusCode { get; set; }
        public Clientdata clientData { get; set; }
    }

    public class consentResult
    {
    }



    public class PanRootobject
    {
        public PanResult result { get; set; }
        public string request_id { get; set; }
        [JsonProperty("status-code")]
        public string statuscode { get; set; }
    }

    public class PanResult
    {
        public string name { get; set; }
    }


    public class PerfiosPANResult
    {
        public string name { get; set; }
        public string dob { get; set; }
        public string status { get; set; }
    }

    public class PerfiosPANRoot
    {
        public string requestId { get; set; }
        public PerfiosPANResult result { get; set; }
        public int statusCode { get; set; }
        public ClientData clientData { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CovDetail
    {
        public string cov { get; set; }
        public string issueDate { get; set; }
    }

    public class EndorsementAndHazardousDetails
    {
        public string initialIssuingOffice { get; set; }
        public string lastEndorsementDate { get; set; }
        public string lastEndorsedOffice { get; set; }
        public string endorsementReason { get; set; }
        public string hazardousValidTill { get; set; }
        public string hillValidTill { get; set; }
    }

    public class DLResult
    {
        public string issueDate { get; set; }

        [JsonProperty("father/husband")]
        public string fatherhusband { get; set; }
        public string name { get; set; }
        public string img { get; set; }
        public string bloodGroup { get; set; }
        public string dob { get; set; }
        public string dlNumber { get; set; }
        public Validity validity { get; set; }
        public List<CovDetail> covDetails { get; set; }
        public Address[] address { get; set; }
        public string status { get; set; }
        public StatusDetails statusDetails { get; set; }
        public EndorsementAndHazardousDetails endorsementAndHazardousDetails { get; set; }
    }

    public class PerfiosDLRoot
    {
        public string requestId { get; set; }
        public DLResult result { get; set; }
        public int statusCode { get; set; }
    }

    public class StatusDetails
    {
        public string from { get; set; }
        public string to { get; set; }
        public string remarks { get; set; }
    }

    public class Validity
    {
        public string nonTransport { get; set; }
        public string transport { get; set; }
    }


    public class Address
    {
        public string addressLine1 { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string pin { get; set; }
        public string completeAddress { get; set; }
        public string country { get; set; }
        public string type { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ClientData
    {
        public string caseId { get; set; }
    }

    public class PerfiosResult
    {
        public string name { get; set; }
        public string rlnName { get; set; }
        public string rlnType { get; set; }
        public string gender { get; set; }
        public string district { get; set; }
        public string acName { get; set; }
        public string pcName { get; set; }
        public string state { get; set; }
        public string epicNo { get; set; }
        public string dob { get; set; }
        public int age { get; set; }
        public string partNo { get; set; }
        public string slNoInPart { get; set; }
        public string psName { get; set; }
        public string partName { get; set; }
        public string lastUpdate { get; set; }
        public string psLatLong { get; set; }
        public string rlnNameV1 { get; set; }
        public string rlnNameV2 { get; set; }
        public string rlnNameV3 { get; set; }
        public string sectionNo { get; set; }
        public string id { get; set; }
        public string nameV1 { get; set; }
        public string nameV2 { get; set; }
        public string nameV3 { get; set; }
        public string acNo { get; set; }
        public string stCode { get; set; }
        public string houseNo { get; set; }
        public string pin { get; set; }
    }

    public class PerfiosVoterIdRoot
    {
        public string requestId { get; set; }
        public PerfiosResult result { get; set; }
        public int statusCode { get; set; }
        public ClientData clientData { get; set; }
    }


    public class DigilockerAuthenticateRootobject
    {
        public string requestId { get; set; }
        public DigilockerResult result { get; set; }
        public int statusCode { get; set; }
        public Clientdata clientData { get; set; }
    }

    public class DigilockerResult
    {
        public string link { get; set; }
    }


    public class digilockerdocumentsRootobject
    {
        public string requestId { get; set; }
        public digilockerdocumentsResult[] result { get; set; }
        public int statusCode { get; set; }
        public Clientdata clientData { get; set; }
    }

    public class digilockerdocumentsResult
    {
        public string name { get; set; }
        public string date { get; set; }
        public string uri { get; set; }
        public string doctype { get; set; }
        public string description { get; set; }
        public string issuerId { get; set; }
        public string issuer { get; set; }
        public string[] mimes { get; set; }
        public bool isParseable { get; set; }
    }


    public class digilockerDownloadRootobject
    {
        public string requestId { get; set; }
        public digilockerDownloadResult[] result { get; set; }
        public int statusCode { get; set; }
        public Clientdata clientData { get; set; }
    }    

    public class digilockerDownloadResult
    {
        public string documentUri { get; set; }
        public Parsedfile parsedFile { get; set; }
        public Rawfiles rawFiles { get; set; }
    }

    public class Parsedfile
    {
        public bool xmlSignatureVerified { get; set; }
        public string status { get; set; }
        public digilockerDownloadData data { get; set; }
    }

    public class digilockerDownloadData
    {
        public Additionaldata additionalData { get; set; }
        public Issuedto issuedTo { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string language { get; set; }
        public string issueDate { get; set; }
        public string number { get; set; }
        public string issuedAt { get; set; }
        public string validFromDate { get; set; }
        public Issuedby issuedBy { get; set; }
    }

    public class Additionaldata
    {
        public DateTime ts { get; set; }
        public DateTime ttl { get; set; }
    }

    public class Issuedto
    {
        public string uid { get; set; }
        public Photo photo { get; set; }
        public digilockerDownloadAddress address { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string name { get; set; }
        public string maritalStatus { get; set; }
        public string title { get; set; }
        public string swd { get; set; }
        public string phone { get; set; }
        public string swdIndicator { get; set; }
        public string religion { get; set; }
        public string email { get; set; }
    }

    public class Photo
    {
        public string content { get; set; }
        public string format { get; set; }
    }

    public class digilockerDownloadAddress
    {
        public string co { get; set; }
        public string country { get; set; }
        public string house { get; set; }
        public string loc { get; set; }
        public string state { get; set; }
        public string vtc { get; set; }
        public string pin { get; set; }
        public string landmark { get; set; }
        public string locality { get; set; }
        public string district { get; set; }
        public string line2 { get; set; }
        public string line1 { get; set; }
        public string type { get; set; }
    }

    public class Issuedby
    {
        public string code { get; set; }
        public string uid { get; set; }
        public string tin { get; set; }
        public Address1 address { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Address1
    {
        public string vtc { get; set; }
        public string pin { get; set; }
        public string district { get; set; }
        public string locality { get; set; }
        public string house { get; set; }
        public string line2 { get; set; }
        public string line1 { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string landmark { get; set; }
        public string type { get; set; }
    }

    public class Rawfiles
    {
        public object pdfB64 { get; set; }
        public object json { get; set; }
        public object xml { get; set; }
    }







}
