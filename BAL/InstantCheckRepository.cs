using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using static BAL.ProvidedInfoRepository.AddUANCheckDetailsByAPI;

namespace BAL
{
    public class InstantCheckRepository
    {

        public FaceMatchRoot GetFatchMatch(string Image1, string Image2, string threshold, string MapPath = "",string CheckUniqueID="")
        {

            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP0000630";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();         
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);         
            var client = new RestClient("https://api.verifya2z.com");
            var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/face_match", Method.POST);
            var FinalImage1 = File.ReadAllBytes(Image1);
            var Finaltest2 = File.ReadAllBytes(Image2);
            string ContentType1 = System.Web.MimeMapping.GetMimeMapping(Image1);
            string ContentType2 = System.Web.MimeMapping.GetMimeMapping(Image2);
            string filename1 = Path.GetFileName(Image1);
            string filename2 = Path.GetFileName(Image2);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP0000630");
            request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
            request.AddFile("image1", FinalImage1, filename1, ContentType1);
            request.AddParameter("threshold", threshold);
            request.AddParameter("refid", CheckUniqueID);
            request.AddFile("image2", Finaltest2, filename2, ContentType2);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            FaceMatchRoot myDeserializedClass = JsonConvert.DeserializeObject<FaceMatchRoot>(result1);
            return myDeserializedClass;
        }

        public string GetFatchMatchNew(string Image1, string Image2, string threshold, string MapPath = "", string CheckUniqueID = "")
        {

            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP0000630";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);
            var client = new RestClient("https://api.verifya2z.com");
            var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/face_match", Method.POST);
            var FinalImage1 = File.ReadAllBytes(Image1);
            var Finaltest2 = File.ReadAllBytes(Image2);
            string ContentType1 = System.Web.MimeMapping.GetMimeMapping(Image1);
            string ContentType2 = System.Web.MimeMapping.GetMimeMapping(Image2);
            string filename1 = Path.GetFileName(Image1);
            string filename2 = Path.GetFileName(Image2);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP0000630");
            request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
            request.AddFile("image1", FinalImage1, filename1, ContentType1);
            request.AddParameter("threshold", threshold);
            request.AddParameter("refid", CheckUniqueID);
            request.AddFile("image2", Finaltest2, filename2, ContentType2);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;


            return result1;
        }

        public Rootobject GetEPFO_without_otpLive(string uanNumberId)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP0000630";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);

            var client = new RestClient("https://api.verifya2z.com");
            var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/epfo_without_otp", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP0000630");
            request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
            request.AddParameter("uan_number", uanNumberId);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            Rootobject myDeserializedClass = JsonConvert.DeserializeObject<Rootobject>(result1);
            return myDeserializedClass;
        }

        public Rootobject GetEPFO_without_otpTest(string uanNumberId)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP00001";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();

            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREF4VFZSSmVrNUVWVEpPZWxVd1RuYzlQUT09");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);

            var client = new RestClient("https://uat.paysprint.in");
            var request = new RestRequest("https://uat.paysprint.in/sprintverify-uat/api/v1/verification/epfo_without_otp", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP00001");
            request.AddHeader("Authorisedkey", "TVRJek5EVTJOelUwTnpKRFQxSlFNREF3TURFPQ==");
            request.AddParameter("uan_number", "101166195387");
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            Rootobject myDeserializedClass = JsonConvert.DeserializeObject<Rootobject>(result1);
            return myDeserializedClass;
        }

        public objectRoot GetVoterIdCardVerificationLive(string id_number, string CheckUniqueID)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP0000630";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);
            var client = new RestClient("https://api.verifya2z.com");
            var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/voter_verify", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP0000630");
            request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
            request.AddParameter("id_number", id_number);
            request.AddParameter("refid", CheckUniqueID);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            objectRoot myDeserializedClass = JsonConvert.DeserializeObject<objectRoot>(result1);
            return myDeserializedClass;
        }

        public objectRoot GetVoterIdCardVerificationTest(string id_number)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP00001";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREF4VFZSSmVrNUVWVEpPZWxVd1RuYzlQUT09");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);
            var client = new RestClient("https://uat.paysprint.in");
            var request = new RestRequest("https://uat.paysprint.in/sprintverify-uat/api/v1/verification/voter_verify", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP00001");
            request.AddHeader("Authorisedkey", "TVRJek5EVTJOelUwTnpKRFQxSlFNREF3TURFPQ==");
            request.AddParameter("id_number", id_number);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            objectRoot myDeserializedClass = JsonConvert.DeserializeObject<objectRoot>(result1);
            return myDeserializedClass;
        }

        public PANRootobject GetPANLive(string panNumber, string CheckUniqueID,string APIlog="")
        {
            PANRootobject myDeserializedClass=new PANRootobject();
            try
            {
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                Cryptography.wJwTJson js = new Cryptography.wJwTJson();
                js.partnerId = "CORP0000630";
                js.timestamp = Timestamp.ToString();
                js.reqid = Timestamp.ToString();
                byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
                var jsonText = JsonConvert.SerializeObject(js).ToString();
                var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);

                var client = new RestClient("https://api.verifya2z.com");
                var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/pan_verify", Method.POST);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Token", encodedHeader);
                request.AddHeader("User-Agent", "CORP0000630");
                request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
                request.AddParameter("pannumber", panNumber);
                request.AddParameter("refid", CheckUniqueID);
                var response = client.ExecuteAsync(request);
                var test = response.Result;
                var result1 = response.Result.Content;
                clsCommonMethods1.APILogLog(APIlog, result1.ToString(), "https://api.verifya2z.com/api/v1/verification/pan_verify", "Pan Number:" + panNumber + ", refid:" + CheckUniqueID);
                myDeserializedClass = JsonConvert.DeserializeObject<PANRootobject>(result1);
            }
            catch (Exception Ex)
            {
                clsCommonMethods1.APILogLog(APIlog, Ex.Message, "https://api.verifya2z.com/api/v1/verification/pan_verify", "Pan Number:" + panNumber + ", refid:" + CheckUniqueID);
            }
            return myDeserializedClass;
        }
        public PANRootobject GetPANTest(string panNumber)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP00001";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREF4VFZSSmVrNUVWVEpPZWxVd1RuYzlQUT09");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);

            var client = new RestClient("https://uat.paysprint.in");
            var request = new RestRequest("https://uat.paysprint.in/sprintverify-uat/api/v1/verification/pan_verify", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP00001");
            request.AddHeader("Authorisedkey", "TVRJek5EVTJOelUwTnpKRFQxSlFNREF3TURFPQ==");
            request.AddParameter("pannumber", "CPFPG7333N");
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            PANRootobject myDeserializedClass = JsonConvert.DeserializeObject<PANRootobject>(result1);
            return myDeserializedClass;
        }

        public aadhaar_without_otp_object Getaadhaar_without_otpLive(string id_number, string CheckUniqueID)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP0000630";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);

            var client = new RestClient("https://api.verifya2z.com");
            var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/aadhaar_without_otp", Method.POST);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP0000630");
            request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
            request.AddParameter("id_number", id_number);
            request.AddParameter("refid", CheckUniqueID);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            aadhaar_without_otp_object myDeserializedClass = JsonConvert.DeserializeObject<aadhaar_without_otp_object>(result1);
            return myDeserializedClass;
        }

        public aadhaar_withoutotpobject Getaadhaar_withoutotpLive(string id_number)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP0000630";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);

            var client = new RestClient("https://api.verifya2z.com");
            var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/aadhaar_without_otp", Method.POST);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP0000630");
            request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
            request.AddParameter("id_number", id_number);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            aadhaar_withoutotpobject myDeserializedClass = JsonConvert.DeserializeObject<aadhaar_withoutotpobject>(result1);
            return myDeserializedClass;
        }

        public aadhaar_without_otp_object Getaadhaar_without_otpTest(string id_number)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP00001";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREF4VFZSSmVrNUVWVEpPZWxVd1RuYzlQUT09");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);

            var client = new RestClient("https://uat.paysprint.in");
            var request = new RestRequest("https://uat.paysprint.in/sprintverify-uat/api/v1/verification/aadhaar_without_otp", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP00001");
            request.AddHeader("Authorisedkey", "TVRJek5EVTJOelUwTnpKRFQxSlFNREF3TURFPQ==");
            request.AddParameter("id_number", id_number);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            aadhaar_without_otp_object myDeserializedClass = JsonConvert.DeserializeObject<aadhaar_without_otp_object>(result1);
            return myDeserializedClass;
        }


        public Root_license GetDrivinglicenseLive(string id_number, string dob , string CheckUniqueID)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP0000630";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);

            var client = new RestClient("https://api.verifya2z.com");
            var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/drivinglicense_verify", Method.POST);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP0000630");
            request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
            request.AddParameter("id_number", id_number);
            request.AddParameter("dob", dob);
            request.AddParameter("refid", CheckUniqueID);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            Root_license myDeserializedClass = JsonConvert.DeserializeObject<Root_license>(result1);
            return myDeserializedClass;
        }

        public Root_license GetDrivinglicenseTest(string id_number, string dob = "")
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP00001";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREF4VFZSSmVrNUVWVEpPZWxVd1RuYzlQUT09");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);
            var client = new RestClient("https://uat.paysprint.in");
            var request = new RestRequest("https://uat.paysprint.in/sprintverify-uat/api/v1/verification/drivinglicense_verify", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP00001");
            request.AddHeader("Authorisedkey", "TVRJek5EVTJOelUwTnpKRFQxSlFNREF3TURFPQ==");
            request.AddParameter("id_number", id_number);
            request.AddParameter("dob", dob);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            Root_license myDeserializedClass = JsonConvert.DeserializeObject<Root_license>(result1);
            return myDeserializedClass;
        }

        public passportRoot GetpassportLive(string id_number, string dob = "")
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP0000630";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);

            var client = new RestClient("https://api.verifya2z.com");
            var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/passport_verify", Method.POST);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP0000630");
            request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
            request.AddParameter("id_number", id_number);
            request.AddParameter("dob", dob);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            passportRoot myDeserializedClass = JsonConvert.DeserializeObject<passportRoot>(result1);
            return myDeserializedClass;
        }

        public passportRoot GetpassportTest(string id_number, string dob = "")
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP00001";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            // var token = GetTokenTest();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREF4VFZSSmVrNUVWVEpPZWxVd1RuYzlQUT09");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);
            var client = new RestClient("https://uat.paysprint.in");
            var request = new RestRequest("https://uat.paysprint.in/sprintverify-uat/api/v1/verification/passport_verify", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP00001");
            request.AddHeader("Authorisedkey", "TVRJek5EVTJOelUwTnpKRFQxSlFNREF3TURFPQ==");
            request.AddParameter("id_number", id_number);
            request.AddParameter("dob", dob);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            passportRoot myDeserializedClass = JsonConvert.DeserializeObject<passportRoot>(result1);
            return myDeserializedClass;
        }
        public UAN_Root GetEPFO_withMobileNoTest(string mobileNo, string pan,string UAN="",string type="1")
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP00001";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();           
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREF4VFZSSmVrNUVWVEpPZWxVd1RuYzlQUT09");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);          
            var client = new RestClient("https://uat.paysprint.in");
            var request = new RestRequest("https://uat.paysprint.in/sprintverify-uat/api/v1/verification/uan_basic_v1", Method.POST);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP00001");
            request.AddHeader("Authorisedkey", "TVRJek5EVTJOelUwTnpKRFQxSlFNREF3TURFPQ==");
            request.AddParameter("type", type);
            request.AddParameter("mobile", "8860020552");
            request.AddParameter("pan_number", "CPFPG7333N");
            request.AddParameter("uan", "");
            var response = client.ExecuteAsync(request);
            var result1 = response.Result.Content;
            dynamic convertObj = JObject.Parse(result1);

            var data = convertObj["data"];
            var result = data["result"];
            var UANLst = result["uan"];
            var count = UANLst.Count;
            summaryRoot summaryRootclass = JsonConvert.DeserializeObject<summaryRoot>(Convert.ToString(result["summary"]));
            List<EPFO_basic_details_withemployment_details_Lst> lmd = new List<EPFO_basic_details_withemployment_details_Lst>();

            for (int i = 0; i < count; i++)
            {
                var UANRootdata = result["uan_details"][Convert.ToString(UANLst[i])];
                UANRoot UANNoClass = JsonConvert.DeserializeObject<UANRoot>(Convert.ToString(UANRootdata));
                lmd.Add(new EPFO_basic_details_withemployment_details_Lst
                {
                    uan = Convert.ToString(UANLst[i]),
                    gender = UANNoClass.basic_details.gender,
                    date_of_birth = UANNoClass.basic_details.date_of_birth,
                    employee_confidence_score = UANNoClass.basic_details.employee_confidence_score,
                    name = UANNoClass.basic_details.name,
                    mobile = UANNoClass.basic_details.mobile,
                    aadhaar_verification_status = UANNoClass.basic_details.aadhaar_verification_status,
                    member_id = UANNoClass.employment_details.member_id,
                    establishment_id = UANNoClass.employment_details.establishment_id,
                    date_of_exit = UANNoClass.employment_details.date_of_exit,
                    date_of_joining = UANNoClass.employment_details.date_of_joining,
                    leave_reason = UANNoClass.employment_details.leave_reason,
                    establishment_name = UANNoClass.employment_details.establishment_name,
                    employer_confidence_score = UANNoClass.employment_details.employer_confidence_score
                });
            }
            UAN_Root uANRoot = new UAN_Root();
            uANRoot.MobileNo = mobileNo;
            uANRoot.PAN = pan;
            uANRoot.status = Convert.ToInt32(convertObj["status"]).ToString();
            uANRoot.statuscode = convertObj["statuscode"];
            uANRoot.message = convertObj["message"];
            uANRoot.basic_details = lmd;
            uANRoot.summary = summaryRootclass;
            return uANRoot;
        }

        public UAN_Root GetEPFO_withMobileNoLive(string mobileNo, string pan="", string UAN = "", string type = "1")
        {
            UAN_Root uANRoot = new UAN_Root();
            List<EPFO_basic_details_withemployment_details_Lst> lmd = new List<EPFO_basic_details_withemployment_details_Lst>();
            try
            {
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                Cryptography.wJwTJson js = new Cryptography.wJwTJson();
                js.partnerId = "CORP0000630";
                js.timestamp = Timestamp.ToString();
                js.reqid = Timestamp.ToString();
                byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
                var jsonText = JsonConvert.SerializeObject(js).ToString();
                var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);
                var client = new RestClient("https://api.verifya2z.com");
                var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/uan_basic_v1", Method.POST);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Token", encodedHeader);
                request.AddHeader("User-Agent", "CORP0000630");
                request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
                request.AddParameter("type", type);
                request.AddParameter("mobile", mobileNo);
                request.AddParameter("pan_number", pan);
                request.AddParameter("uan", UAN);
                var response = client.ExecuteAsync(request);
                var result1 = response.Result.Content;
                dynamic convertObj = JObject.Parse(result1);
           
                var data = convertObj["data"];
                if (convertObj["message"].ToString()== "UAN details fetched successfully.")
                {
                    var result = data["result"];
                    var UANLst = result["uan"];
                    var count = UANLst.Count;
                    summaryRoot summaryRootclass = JsonConvert.DeserializeObject<summaryRoot>(Convert.ToString(result["summary"]));                   
                    uANRoot.summary = summaryRootclass;

                    for (int i = 0; i < count; i++)
                    {
                        var UANRootdata = result["uan_details"][Convert.ToString(UANLst[i])];
                        UANRoot UANNoClass = JsonConvert.DeserializeObject<UANRoot>(Convert.ToString(UANRootdata));
                        lmd.Add(new EPFO_basic_details_withemployment_details_Lst
                        {
                            uan = Convert.ToString(UANLst[i]),
                            gender = UANNoClass.basic_details.gender,
                            date_of_birth = UANNoClass.basic_details.date_of_birth,
                            employee_confidence_score = UANNoClass.basic_details.employee_confidence_score,
                            name = UANNoClass.basic_details.name,
                            mobile = UANNoClass.basic_details.mobile,
                            aadhaar_verification_status = UANNoClass.basic_details.aadhaar_verification_status,
                            member_id = UANNoClass.employment_details.member_id,
                            establishment_id = UANNoClass.employment_details.establishment_id,
                            date_of_exit = UANNoClass.employment_details.date_of_exit,
                            date_of_joining = UANNoClass.employment_details.date_of_joining,
                            leave_reason = UANNoClass.employment_details.leave_reason,
                            establishment_name = UANNoClass.employment_details.establishment_name,
                            employer_confidence_score = UANNoClass.employment_details.employer_confidence_score
                        });
                    }
                }              
               
                uANRoot.MobileNo = mobileNo;
                uANRoot.PAN = pan;
                uANRoot.status = Convert.ToInt32(convertObj["status"]).ToString();
                uANRoot.statuscode = convertObj["statuscode"];
                uANRoot.message = convertObj["message"];
                uANRoot.basic_details = lmd;
               
            }
            catch (Exception ex)
            {

                
            }
            
            return uANRoot;
        }

        public aadhaar_with_otp_objectRoot Getaadhaar_sendotpLive(string id_number)
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP0000630";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            //var token = GetTokenTest();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);
            var client = new RestClient("https://api.verifya2z.com");
            var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/aadhaar_sendotp", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP0000630");
            request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
            request.AddParameter("id_number", id_number);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
            aadhaar_with_otp_objectRoot myDeserializedClass = JsonConvert.DeserializeObject<aadhaar_with_otp_objectRoot>(result1);
            return myDeserializedClass;
        }
        public aadhaar_verifyotpRoot Getaadhaar_verify_otpLive(string Otp, string client_id)
        {
            
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Cryptography.wJwTJson js = new Cryptography.wJwTJson();
            js.partnerId = "CORP0000630";
            js.timestamp = Timestamp.ToString();
            js.reqid = Timestamp.ToString();
            // var token = GetTokenTest();
            byte[] authToken = System.Text.Encoding.UTF8.GetBytes("UTA5U1VEQXdNREEyTXpCUFZHY3pUVVJGTlU1cVdUVk9RVDA5");
            var jsonText = JsonConvert.SerializeObject(js).ToString();
            var encodedHeader = Cryptography.JsonWebToken.Encode(jsonText.ToString(), authToken, Cryptography.JwtHashAlgorithm.HS256);
            var client = new RestClient("https://api.verifya2z.com");
            var request = new RestRequest("https://api.verifya2z.com/api/v1/verification/aadhaar_verifyotp", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", encodedHeader);
            request.AddHeader("User-Agent", "CORP0000630");
            request.AddHeader("Authorisedkey", "T1RnM01ERTVOalk1TkRjd01VTlBVbEF3TURBd05qTXc=");
            request.AddParameter("otp", Otp);
            request.AddParameter("client_id", client_id);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;
             aadhaar_verifyotpRoot myDeserializedClass = JsonConvert.DeserializeObject<aadhaar_verifyotpRoot>(result1);
            return myDeserializedClass;
        }


        public InstantMultiCheckSolution GetInstantSolution(int InstantId = 0, string CompanyRefNo = "", string datefrom = "", string dateTo = "")
        {
            InstantMultiCheckSolution model = new InstantMultiCheckSolution();
            try
            {
                ClientCustomize CE = new ClientCustomize();
                DataTable tbl = new DataTable();
                tbl = CE.GetInstantMultiCheckSolution(InstantId, CompanyRefNo, datefrom, dateTo);
                if (tbl.Rows.Count > 0)
                {
                    model.InstantId = Convert.ToInt32(tbl.Rows[0]["InstantId"]);
                    model.PersonalRowId = Convert.ToInt32(tbl.Rows[0]["PersonalRowId"]);
                    model.CompanyRefNo = Convert.ToString(tbl.Rows[0]["CompanyRefNo"]);
                    model.PanNumber = Convert.ToString(tbl.Rows[0]["PanNumber"]);
                    model.MobileNo = Convert.ToString(tbl.Rows[0]["MobileNo"]);
                    model.DocLoCation = Convert.ToString(tbl.Rows[0]["DocLoCation"]);
                    model.DocName = Convert.ToString(tbl.Rows[0]["DocName"]);
                    model.CurrentAddress = Convert.ToString(tbl.Rows[0]["CurrentAddress"]);
                    model.PermanentAddress = Convert.ToString(tbl.Rows[0]["PermanentAddress"]);
                    model.PinNo = Convert.ToString(tbl.Rows[0]["PinNo"]);
                    model.Landmark = Convert.ToString(tbl.Rows[0]["Landmark"]);
                    model.isAddress = Convert.ToBoolean(tbl.Rows[0]["isAddress"]);
                    model.FacialMatchStatus = Convert.ToInt32(tbl.Rows[0]["FacialMatchStatus"]);
                    model.AadhaarStatus = Convert.ToInt32(tbl.Rows[0]["AadhaarStatus"]);
                    model.PANStatus = Convert.ToInt32(tbl.Rows[0]["PANStatus"]);
                    model.UANStatus = Convert.ToInt32(tbl.Rows[0]["UANStatus"]);
                    model.CRCStatus = Convert.ToInt32(tbl.Rows[0]["CRCStatus"]);
                    model.CurrentAddressStatus = Convert.ToInt32(tbl.Rows[0]["CurrentAddressStatus"]);
                    model.PermanentAddressStatus = Convert.ToInt32(tbl.Rows[0]["PermanentAddressStatus"]);
                    model.FacialMatchId = Convert.ToInt32(tbl.Rows[0]["FacialMatchId"]);
                    model.AadhaarId = Convert.ToInt32(tbl.Rows[0]["AadhaarId"]);
                    model.PANId = Convert.ToInt32(tbl.Rows[0]["PANId"]);
                    model.UANId = Convert.ToInt32(tbl.Rows[0]["UANId"]);
                }
            }
            catch (Exception ex)
            {
                var rst = ex.Message;
                throw;
            }
            return model;
        }

        #region llmtech Apis

        public string Getepfo(string UAN, string Path)
        {
            string JsonString = "";
            try
            {
                var url = "https://apis.llmtech.in/api/v1/services/uan/employment-history-v2";

                using (var client = new HttpClient())
                {
                    // Add headers
                    client.DefaultRequestHeaders.Add("X-API-Key", "llm_846ce3bea8912b0115048944ec711823c9644e0550599b9219d83c7cdeb7938c");
                    // Prepare JSON body
                    var payload = new
                    {
                        consent = "Y",
                        uan = UAN
                    };
                    var json = JsonConvert.SerializeObject(payload);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    // Send POST request synchronously
                    var response = client.PostAsync(url, content).Result;
                    // Read response synchronously
                    JsonString = response.Content.ReadAsStringAsync().Result;
                    clsCommonMethods1.ErrorLog(Path, "UAN NO:" + UAN + " " + JsonString, "https://apis.llmtech.in/api/v1/services/uan/employment-history-v2");
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(Path, UAN + " : " + Ex.Message, "https://apis.llmtech.in/api/v1/services/uan/employment-history-v2");
            }
            return JsonString;
        }

        #endregion
    }

}
