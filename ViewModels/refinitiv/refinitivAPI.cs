using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Policy;

namespace ViewModels.refinitiv
{
    public class refinitivAPI
    {       
        private static readonly string apiKey = System.Configuration.ConfigurationManager.AppSettings["WorldCheckAPIKey"].ToString();   
        private static readonly string secretKey = System.Configuration.ConfigurationManager.AppSettings["WorldChecksecretKey"].ToString();

        public string PostRequest(string baseUrl, string requestBody,string Path)
        {
            string result = "";
            try
            {
                Uri uri = new Uri(baseUrl);

                HttpMethod httpMethod = HttpMethod.Post;
                Encoding encoding = Encoding.UTF8;
                string contentType = "application/json";

                HttpClient client = new HttpClient();
                AuthHeadersBuilder authHeadersBuilder = new AuthHeadersBuilder();
                HttpRequestMessage httpRequest = new HttpRequestMessage(httpMethod, uri);
                Dictionary<string, string> headers = AuthHeadersBuilder.GenerateAuthHeaders(apiKey, secretKey, httpMethod.Method.ToLower(), uri, contentType, requestBody);
                foreach (KeyValuePair<string, string> header in headers)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }
                httpRequest.Content = new StringContent(requestBody, encoding, contentType);
                httpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

                var response = client.SendAsync(httpRequest).Result;
                result = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    ErrorLog(Path, response.ToString(), "Request:" + requestBody);
                    return "";
                }
                else
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(Path, ex.Message.ToString(), ex.StackTrace);
            }
             return result;
                
        }
        static string sLogFormat = string.Empty;
        static string sErrorTime = string.Empty;
        public static void ErrorLog(string sPathName, string sErrMsg, string sStackTrace)
        {
            CreateLogFiles();
            StreamWriter streamWriter = new StreamWriter(string.Concat(sPathName, "(", sErrorTime, ").txt"), true);
            streamWriter.WriteLine(string.Concat(sLogFormat, sErrMsg));
            streamWriter.WriteLine(sStackTrace);
            streamWriter.WriteLine("__________________________________________________________________");
            streamWriter.Flush();
            streamWriter.Close();
        }
        protected static void CreateLogFiles()
        {
            string str = DateTime.Now.ToShortDateString().ToString();
            DateTime now = DateTime.Now;
            sLogFormat = string.Concat(str, " ", now.ToLongTimeString().ToString(), " ==> ");
            int year = DateTime.Now.Year;
            string str1 = year.ToString();
            year = DateTime.Now.Month;
            string str2 = year.ToString();
            year = DateTime.Now.Day;
            string str3 = year.ToString();
            string[] strArrays = new string[] { str3, "-", str2, "-", str1 };
            sErrorTime = string.Concat(strArrays);
        }                


        public string GetRequest(string URL, string requestBody,string Path="")
        { 
            Uri uri = new Uri(URL);
            HttpMethod httpMethod = HttpMethod.Get;

            HttpClient client = new HttpClient();
            HttpRequestMessage httpRequest = new HttpRequestMessage(httpMethod, uri);
            Dictionary<string, string> headers = AuthHeadersBuilder.GenerateAuthHeaders(apiKey, secretKey, httpMethod.Method.ToLower(), uri);
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }
            var response = client.SendAsync(httpRequest).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                ErrorLog(Path, response.ToString(), URL);
            }
            return result;
        }
        public HttpResponseMessage GetFileRequest(string URL, string requestBody, string Path = "")
        {
            Uri uri = new Uri(URL);
            HttpMethod httpMethod = HttpMethod.Get;

            HttpClient client = new HttpClient();
            HttpRequestMessage httpRequest = new HttpRequestMessage(httpMethod, uri);
            Dictionary<string, string> headers = AuthHeadersBuilder.GenerateAuthHeaders(apiKey, secretKey, httpMethod.Method.ToLower(), uri);
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }
            var response = client.SendAsync(httpRequest).Result;
            
            if (!response.IsSuccessStatusCode)
            {
                ErrorLog(Path, response.ToString(), URL);
            }
            return response;
        }
        public bool PutRequest(string REQUEST_BODY,string BaseURL, string Path = "")
        {         

            Uri uri = new Uri(BaseURL);
            string data = REQUEST_BODY;

            HttpMethod httpMethod = HttpMethod.Put;
            Encoding encoding = Encoding.UTF8;
            string contentType = "application/json";

            HttpClient client = new HttpClient();
            HttpRequestMessage httpRequest = new HttpRequestMessage(httpMethod, uri);
            Dictionary<string, string> headers = AuthHeadersBuilder.GenerateAuthHeaders( apiKey,secretKey,httpMethod.Method.ToLower(),uri,contentType,data);
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }
            httpRequest.Content = new StringContent(data, encoding, contentType);
            httpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

            var response = client.SendAsync(httpRequest).Result;

            if (!response.IsSuccessStatusCode)
            {
                var responc=response.Content.ReadAsStringAsync().Result;
                ErrorLog(Path, REQUEST_BODY+"    Erorr Resopnse:"+ responc, response.ToString());
            }
                return response.EnsureSuccessStatusCode().IsSuccessStatusCode;

        }
        public static void DeleteRequest(string BaseUrl,string BaseURL, string Path = "")
        {
            Uri uri = new Uri(BaseUrl);

            HttpMethod httpMethod = HttpMethod.Delete;
            HttpClient client = new HttpClient();

            HttpRequestMessage httpRequest = new HttpRequestMessage(httpMethod, uri);
            Dictionary<string, string> headers = AuthHeadersBuilder.GenerateAuthHeaders(apiKey,secretKey,httpMethod.Method.ToLower(),uri);
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }

            var response = client.SendAsync(httpRequest).Result;

            if (!response.IsSuccessStatusCode)
            {
                ErrorLog(Path, "", BaseURL);
            }
        }

    }
}
