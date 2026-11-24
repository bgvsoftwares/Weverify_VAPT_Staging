using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using ViewModels.Perfios;



namespace BAL.Perfios
{
    public class PerfiosAPI
    {
        public Aadhaar_verificationRoot GetPerfiosAadhaar(string Name, string caseId = "", string AadhaarNo = "", string MapPath = "",string APIPath="")
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            var client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.karza.in/v3/aadhaar-consent");
            httpRequest.Headers.Add("x-karza-key", "uLrWYikM3MniDQR");
            var content = new StringContent("{\"consent\":\"Y\",\"name\":\"" + Name + "\",\"consentTime\":\"" + Timestamp + "\",\"clientData\":{\"caseId\":\"" + caseId + "\"}}", null, "application/json");
            httpRequest.Content = content;
            var response = client.SendAsync(httpRequest).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            aadhaarconsentRootobject ContentObject = JsonConvert.DeserializeObject<aadhaarconsentRootobject>(result);
            var accessKey = ContentObject.requestId;


            var client1 = new HttpClient();
            var httpRequest1 = new HttpRequestMessage(HttpMethod.Post, "https://api.karza.in/v2/aadhaar-verification");
            httpRequest1.Headers.Add("x-karza-key", "uLrWYikM3MniDQR");
            var content1 = new StringContent("{\"aadhaarNo\":\"" + AadhaarNo + "\",\"consent\":\"Y\",\"checkValidation\":true,\"accessKey\":\"" + accessKey + "\",\"clientData\":{\"caseId\":\"" + caseId + "\"}}", null, "application/json");
            httpRequest1.Content = content1;
            var response1 = client1.SendAsync(httpRequest1).Result;
            var result1 = response1.Content.ReadAsStringAsync().Result;

            Aadhaar_verificationRoot myDeserializedClass = JsonConvert.DeserializeObject<Aadhaar_verificationRoot>(result1);
            clsCommonMethods1.APILogLog(APIPath, result1.ToString(), "https://api.karza.in/v2/aadhaar-verification", caseId);



            if (Convert.ToString(myDeserializedClass) == "")
            {
                clsCommonMethods1.ErrorLog(MapPath, "input:" + Name + "," + caseId + "Output:" + result, "GetPerfiosDrivinglicenseLive");
            }
            return myDeserializedClass;
        }

        public PanRootobject GetPerfiosPAN(string caseId = "", string PanNo = "", string MapPath = "")
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            var client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.karza.in/v2/pan");
            httpRequest.Headers.Add("x-karza-key", "uLrWYikM3MniDQR");
            var content = new StringContent("{\"consent\":\"Y\",\"pan\":\"" + PanNo + "\"}", null, "application/json");
            httpRequest.Content = content;
            var response = client.SendAsync(httpRequest).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            PanRootobject myDeserializedClass = JsonConvert.DeserializeObject<PanRootobject>(result);

            if (Convert.ToString(myDeserializedClass) == "")
            {
                clsCommonMethods1.ErrorLog(MapPath, "input:" + PanNo + "," + caseId + "Output:" + result, "GetPerfiosPAN");
            }
            return myDeserializedClass;
        }
        public PerfiosPANRoot GetPerfiosPANAPI(string PersonalRowId , string PanNo , string MapPath ,string CaseId,string APIPath)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            var client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.karza.in/v3/pan-profile-basic");
            httpRequest.Headers.Add("x-karza-key", "uLrWYikM3MniDQR");
            string json = "{\"pan\":\""+ PanNo+"\",\"consent\":\"Y\",\"clientData\":{\"caseId\":\""+ CaseId + "\"}}";

            var content = new StringContent(json, null, "application/json");
            //var content = new StringContent("{\"pan\":\"" + PanNo + "\",\"consent\":\"Y\"}", null, "application/json");          

            httpRequest.Content = content;
            var response = client.SendAsync(httpRequest).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            PerfiosPANRoot myDeserializedClass = JsonConvert.DeserializeObject<PerfiosPANRoot>(result);

            clsCommonMethods1.APILogLog(APIPath, result.ToString(), "https://api.karza.in/v3/pan-profile-basic", CaseId);

            if (Convert.ToString(myDeserializedClass) == "")
            {
                clsCommonMethods1.ErrorLog(MapPath, "input:" + PanNo + "," + CaseId + "Output:" + result, "GetPerfiosPAN");
            }
            return myDeserializedClass;
        }
        public PerfiosDLRoot GetPerfiosDL(string DOB , string DLNo ,string CaseId,string APIPath)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            var client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.karza.in/v3/dl");
            httpRequest.Headers.Add("x-karza-key", "uLrWYikM3MniDQR");
            string json = "{\"dlNo\":\"" + DLNo + "\",\"dob\":\"" + DOB + "\",\"additionalDetails\":true,\"consent\":\"Y\",\"clientData\":{\"caseId\":\""+ CaseId + "\"}}";
            var content = new StringContent(json, null, "application/json");

            httpRequest.Content = content;
            var response = client.SendAsync(httpRequest).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            PerfiosDLRoot myDeserializedClass = JsonConvert.DeserializeObject<PerfiosDLRoot>(result);
            clsCommonMethods1.APILogLog(APIPath, result.ToString(), "https://api.karza.in/v3/dl", CaseId);

            return myDeserializedClass;
        }

        public PerfiosVoterIdRoot GetPerfiosVoterId(string CompanyRefNo , string VoterId )
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            var client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.karza.in/v3/voter");
            httpRequest.Headers.Add("x-karza-key", "uLrWYikM3MniDQR");
            var content = new StringContent("{\r\n  \"consent\": \"Y\",\r\n  \"epicNo\": \"" + VoterId + "\",\r\n  \"clientData\": {\r\n    \"caseId\": \"" + CompanyRefNo + "\"\r\n  }\r\n}", null, "application/json");
            httpRequest.Content = content;
            var response = client.SendAsync(httpRequest).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            PerfiosVoterIdRoot myDeserializedClass = JsonConvert.DeserializeObject<PerfiosVoterIdRoot>(result);

            return myDeserializedClass;
        }

        public DigilockerAuthenticateRootobject GetPerfiosDigilockerAuthenticate(string caseId, string redirectUrl, string MapPath = "")
        {
            DigilockerAuthenticateRootobject myDeserializedClass = new DigilockerAuthenticateRootobject();
            DataBaseConnections dataBase = new DataBaseConnections();
            try
            {
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                var client = new HttpClient();
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api-in.perfios.com/kyc/api/v1/digilocker/link");
                httpRequest.Headers.Add("x-api-key", "BPV9Ah1dq52700Srdwex");
                //var content = new StringContent("{\"consent\":\"Y\",\"pan\":\"" + PanNo + "\"}", null, "application/json");
                var content = new StringContent("{\"redirectUrl\":\"" + redirectUrl + "\",\"oAuthState\":\""+ caseId + "\",\"aadhaarFlowRequired\":true,\"pinlessAuth\":true,\"customDocList\":\"ADHAR\",\"consent\":\"Y\",\"clientData\":{\"caseId\":\"" + caseId + "\"}}", null, "application/json");
                httpRequest.Content = content;
                var response = client.SendAsync(httpRequest).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                dataBase.AddPerfiosDigilockerDetails(Convert.ToInt32(caseId),890, result, "https://api-in.perfios.com/kyc/api/v1/digilocker/link");
                clsCommonMethods1.APILogLog(MapPath, result.ToString(), "https://api-in.perfios.com/kyc/api/v1/digilocker/link", caseId);
                myDeserializedClass = JsonConvert.DeserializeObject<DigilockerAuthenticateRootobject>(result);

            }
            catch (Exception ex)
            { 
                clsCommonMethods1.ErrorLog(MapPath, "input:" + caseId + "," + redirectUrl + "Output:" + ex .Message.ToString(), "GetPerfiosDigilockerAuthenticate");

            }
            return myDeserializedClass;
        }
        public digilockerdocumentsRootobject GetPerfiosdigilockerdocuments(string caseId, string RequestId, string MapPath = "")
        {
            digilockerdocumentsRootobject myDeserializedClass = new digilockerdocumentsRootobject();
            try
            {
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                var client = new HttpClient();
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api-in.perfios.com/kyc/api/v1/digilocker/documents");
                httpRequest.Headers.Add("x-api-key", "BPV9Ah1dq52700Srdwex");
                var content = new StringContent("{\"accessRequestId\":\""+ RequestId + "\",\"consent\":\"Y\",\"clientData\":{\"caseId\":\""+ caseId + "\"}}", null, "application/json");

                httpRequest.Content = content;
                var response = client.SendAsync(httpRequest).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                clsCommonMethods1.APILogLog(MapPath, result.ToString(), "https://api-in.perfios.com/kyc/api/v1/digilocker/documents", caseId);
                myDeserializedClass = JsonConvert.DeserializeObject<digilockerdocumentsRootobject>(result);

            }
            catch (Exception ex)
            {
                clsCommonMethods1.ErrorLog(MapPath, "input:" + caseId + "," + RequestId + "Output:" + ex.Message.ToString(), "GetPerfiosDigilockerAuthenticate");
            }
            return myDeserializedClass;
        }
        public digilockerDownloadRootobject GetPerfiosdigilockerdownload(string caseId, string RequestId,string uri, string MapPath = "")
        {
            digilockerDownloadRootobject myDeserializedClass = new digilockerDownloadRootobject();
            try
            {
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                var client = new HttpClient();
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api-in.perfios.com/kyc/api/v1/digilocker/download");
                httpRequest.Headers.Add("x-api-key", "BPV9Ah1dq52700Srdwex");
                var content = new StringContent("{\r\n    \"accessRequestId\": \"" + RequestId + "\",\r\n    \"files\": [\r\n        {\r\n            \"uri\": \""+ uri + "\",\r\n            \"pdfB64\": true,\r\n            \"parsed\": true\r\n        }\r\n    ],\r\n    \"consent\": \"Y\",\r\n    \"clientData\": {\r\n        \"caseId\": \""+ caseId + "\"\r\n    }\r\n}", null, "application/json");
                httpRequest.Content = content;
                var response = client.SendAsync(httpRequest).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                clsCommonMethods1.APILogLog(MapPath, result.ToString(), "https://api-in.perfios.com/kyc/api/v1/digilocker/download", caseId);
                myDeserializedClass = JsonConvert.DeserializeObject<digilockerDownloadRootobject>(result);
            }
            catch (Exception ex)
            {
                clsCommonMethods1.ErrorLog(MapPath, "input:" + caseId + "," + RequestId + "Output:" + ex.Message.ToString(), "GetPerfiosDigilockerAuthenticate");

            }
            return myDeserializedClass;
        }
    }
}
