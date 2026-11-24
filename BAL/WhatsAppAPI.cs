using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BAL
{
    public class WhatsAppAPI
    {
        ClientCustomize cc = new ClientCustomize();

        /// <summary>
        /// MessageStatus objMessage = new MessageStatus();
        /// WhatsAppAPI objs = new WhatsAppAPI();
        /// objMessage = objs.SendWhatsAppMessage("+919958274192", "pre_sms_digital1","Dilip", "media_template","",""); 
        /// objMessage = objs.SendWhatsAppMessage("+919958274192", "post_sms","Dilip", "media_template","9958274192","dilip.mahto@securitas-india.com"); 
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="templateName"></param>
        /// <param name="textApplicant"></param>
        /// <param name="mediaType"></param>
        /// <param name="adminPhone"></param>
        /// <param name="adminEmail"></param>
        /// <returns></returns>
        public MessageStatus SendWhatsAppMessage(string phone, string templateName, string textApplicant, string mediaType= "media_template",string parameter1= "",string parameter2= "",string parameter3 = "")
        {
            MessageStatus msgDetails = new MessageStatus();
            string strToken = GetToken("WalsonsServices", "Walson@qY2r");
            RequestBody rbody = new RequestBody();
            rbody.phone = phone;
            rbody.enable_acculync = true;

            Media media = new Media();
            media.type = mediaType;
            media.lang_code = "en";
            media.template_name = templateName;

            List<Body> lbody = new List<Body>();
            Body bObj = new Body();
            if (templateName != "insuff_message_to_candidate_withoutlinkg")
            {
                bObj.text = textApplicant;
                lbody.Add(bObj);
            }

            if(templateName== "post_sms")
            {
                if (parameter1 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter1;
                    lbody.Add(bObj);
                }
                if (parameter2 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter2.Split('@')[0];
                    lbody.Add(bObj);
                }
            }
            else if(templateName== "case_registration_candidate_invite_mode")
            {
                if (parameter1 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter1;
                    lbody.Add(bObj);
                }
                if (parameter2 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter2;
                    lbody.Add(bObj);
                }
                if (parameter3 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter3;
                    lbody.Add(bObj);
                }
            }
            else if (templateName == "insuff_message_to_candidate")
            {
                if (parameter1 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter1;
                    lbody.Add(bObj);
                }
                if (parameter2 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter2;
                    lbody.Add(bObj);
                }
            }
            else if (templateName == "insuff_message_to_candidate_withoutlinkg")
            {
                if (parameter1 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter1;
                    lbody.Add(bObj);
                }
                if (parameter2 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter2;
                    lbody.Add(bObj);
                }
            }
            else if (templateName == "post_message_digital_with_link")
            {
                if (parameter1 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter1;
                    lbody.Add(bObj);
                }
                if (parameter2 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter2;
                    lbody.Add(bObj);
                }
            }
            else if (templateName == "post_message_feedback")
            {
                if (parameter1 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter1;
                    lbody.Add(bObj);
                }
            }
            else if (templateName == "digilocker_verification_temp")
            {
                if (parameter1 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter1;
                    lbody.Add(bObj);
                }
                if (parameter2 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter2;
                    lbody.Add(bObj);
                }
                if (parameter3 != "")
                {
                    bObj = new Body();
                    bObj.text = parameter3;
                    lbody.Add(bObj);
                }

            }

            media.body = lbody;
            rbody.media = media;
            string json = JsonConvert.SerializeObject(rbody);
            string URL = "https://apis.rmlconnect.net/wba/v1/messages";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";           
            request.Headers.Add("Authorization", strToken);       
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            using (var streamWriter = new
            StreamWriter(request.GetRequestStream()))
            {                
                streamWriter.Write(json);
            }
            var httpResponse = (HttpWebResponse)request.GetResponse();
            var apiResponse = httpResponse.StatusCode.ToString();
            if(apiResponse == "Accepted")
            {
                Stream receiveStream = httpResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var content = readStream.ReadToEnd();
                msgDetails = new JavaScriptSerializer().Deserialize<MessageStatus>(content);
            }
            else
            {
                msgDetails.message = "";
            }
            return msgDetails;

        }

        public MessageStatus SendWhatsAppMessage1(string phone, string templateName, string textApplicant)
        {
            MessageStatus msgDetails = new MessageStatus();
            string strToken = GetToken("WalsonsServices", "Walson@qY2r");

            RequestBody rbody = new RequestBody();
            rbody.phone = phone; // e.g., 919876543210 (ensure it includes country code)
            rbody.enable_acculync = true;

            Media media = new Media();
            media.type = "media_template";
            media.lang_code = "en"; // Change if your template language is different (e.g., "hi")
            media.template_name = templateName;

            List<Body> lbody = new List<Body>();
            Body bObj = new Body();
            if (templateName == "otp_temp")
            {
                bObj.text = textApplicant;
                lbody.Add(bObj);
            }            
            rbody.media = media;
            media.body = lbody;
            string json = JsonConvert.SerializeObject(rbody);
            string URL = "https://apis.rmlconnect.net/wba/v1/messages";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", strToken);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            try
            {
                using (var httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream receiveStream = httpResponse.GetResponseStream())
                    {
                        using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                        {
                            var content = readStream.ReadToEnd();
                            msgDetails = new JavaScriptSerializer().Deserialize<MessageStatus>(content);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // This will extract the exact reason Route Mobile rejected the request with a 400
                using (var stream = ex.Response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            msgDetails.message = "Route Mobile 400 Error Details: " + reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        msgDetails.message = ex.Message;
                    }
                }
            }

            return msgDetails;
        }

        private string GetToken(string username,string password)
        {
            string strToken = "";           
            try
            {
                tokenFields Cridencial= new tokenFields();
                Cridencial.username = username;
                Cridencial.password = password; 
                Token token = new Token();
                string baseAddress = "https://apis.rmlconnect.net/auth/v1/login/";

                string json = JsonConvert.SerializeObject(Cridencial);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseAddress);
                request.Method = "POST";
                request.ContentType = "application/json";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                using (var streamWriter = new
                StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)request.GetResponse();
                var apiResponse = httpResponse.StatusCode.ToString();
                if (apiResponse == "Created")
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    var content = readStream.ReadToEnd();
                    token = new JavaScriptSerializer().Deserialize<Token>(content);
                }
                else
                {
                    token.JWTAUTH = "Not able to generate Access Token Invalid usrename or password";
                }

                //HttpClientHandler handler = new HttpClientHandler();
                //HttpClient client = new HttpClient(handler);               
                //var RequestBody = new Dictionary<string, string>
                //{                
                //{"username", username},
                //{"password", password},
                //};
                //var tokenResponse = client.PostAsync(baseAddress, new FormUrlEncodedContent(RequestBody)).Result;
                //if (tokenResponse.IsSuccessStatusCode)
                //{
                //    var JsonContent = tokenResponse.Content.ReadAsStringAsync().Result;
                //    token = new JavaScriptSerializer().Deserialize<Token>(JsonContent);                    
                //}
               
                return token.JWTAUTH;

            }
            catch (Exception ex)
            {
                string strMsg = ex.Message;
            }

            return strToken;
        }



        #region PropertyDetails
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public class RequestBody
        {
            public string phone { get; set; }
            public bool enable_acculync { get; set; }
            public Media media { get; set; }
        }

        public class Body
        {
            public string text { get; set; }
        }

        public class Media
        {
            public string type { get; set; }
            public string lang_code { get; set; }
            public string template_name { get; set; }
            public List<Body> body { get; set; }
        }

       
        public class Token
        {
            public string JWTAUTH { get; set; }
            public UserData user_data { get; set; }
        }

        public class UserData
        {
            public string username { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string phone_number { get; set; }
            public bool password_reset { get; set; }
            public bool is_active { get; set; }
            public bool is_staff { get; set; }
        }

        public class MessageStatus
        {
            public string message { get; set; }
            public string status { get; set; }
            public string request_id { get; set; }
        }


        #endregion

        public class tokenFields
        {
            public string username { get; set; }
            public string password { get; set; }
        }

    }
}
