using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebAppBGV.DocumentServices
{
    public class DocumentService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient76;
        private readonly HttpClient _httpClient55;
        string DocumentURl = ConfigurationManager.AppSettings["DocumentBaseURL"].ToString();

        public DocumentService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(DocumentURl);

            _httpClient76 = new HttpClient();
            _httpClient76.BaseAddress = new Uri("http://10.83.0.76:8091/");

            _httpClient55 = new HttpClient();
            _httpClient55.BaseAddress = new Uri("http://10.83.0.55:8888/");
        }

        public async Task<HttpResponseMessage> GetDocumentAsync(string documentName)
        {
            var requestBody = new
            {
                DocumentName = documentName,
                Documentpath = @"D:\wwwroot2\Documents\CandidateDocuments\ProvidedDocuments\"
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync("Get-Doc-File-Document", content);
        }
        public async Task<HttpResponseMessage> GetDocumentAsync(string documentName, string path)
        {
            var requestBody = new
            {
                DocumentName = documentName,
                Documentpath = path
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync("Get-Doc-File-Document", content);
        }
        public async Task<HttpResponseMessage> GetDocumentAsync46(string documentName, string path)
        {
            var requestBody = new
            {
                DocumentName = documentName,
                Documentpath = path
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync("Get-Doc-File-Document", content);
        }
        public async Task<HttpResponseMessage> GetDocumentAsync76(string documentName, string path)
        {
            var requestBody = new
            {
                DocumentName = documentName,
                Documentpath = path
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient76.PostAsync("Get-Doc-File-Document", content);
        }

        public async Task<HttpResponseMessage> GetDocumentAsync55(string documentName, string path)
        {
            var requestBody = new
            {
                DocumentName = documentName,
                Documentpath = path
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient55.PostAsync("Get-Doc-File-Document", content);
        }
        public async Task<HttpResponseMessage> DeleteDocumentAsync(string documentName, string path)
        {
            var requestBody = new
            {
                DocumentName = documentName,
                Documentpath = path
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(new Uri(DocumentURl), "Delete-Document-API"),
                Content = content
            };

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteDocumentAsync46(string documentName, string path)
        {
            var requestBody = new
            {
                DocumentName = documentName,
                Documentpath = path                                                                                                 
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(new Uri(DocumentURl), "Delete-Document-API"),
                Content = content
            };

            return await _httpClient.SendAsync(request);
        }
        public async Task<HttpResponseMessage> DeleteDocumentAsync76(string documentName, string path)
        {
            var requestBody = new
            {
                DocumentName = documentName,
                Documentpath = path
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://10.83.0.76:8091/Delete-Document-API"),
                Content = content
            };

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteDocumentAsync55(string documentName, string path)
        {
            var requestBody = new
            {
                DocumentName = documentName,
                Documentpath = path
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://10.83.0.55:8888/Delete-Document-API"),
                Content = content
            };

            return await _httpClient.SendAsync(request);
        }
        public string UploadFile(string DocumentName, string base64File, string DocumentType)
        {
            string res = "";
            StringBuilder StrBuilderObj = new StringBuilder();
            StrBuilderObj.Clear();
            try
            {
                string url = new Uri(new Uri(DocumentURl), "Get-Base64-AsFile-Document/").ToString();
                string jsonData = "{\"DocumentType\":\"" + DocumentType + "\",\"DocumentName\":\"" + DocumentName + "\",\"DocumentValue\":\"" + base64File + "\"}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json"; // Change to "application/x-www-form-urlencoded" if needed
                request.Accept = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                    streamWriter.Flush();
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    res = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {

                res = "";
            }


            return res;
        }

        public string UploadDynamicCandidateFiles(string DocumentName, string DocumentValue, string Documentpath)
        {
            string res = "";
            StringBuilder StrBuilderObj = new StringBuilder();
            StrBuilderObj.Clear();
            try
            {
                string url = new Uri(new Uri(DocumentURl), "Upload-Document-Base64-AsFile").ToString();
                string jsonData = "{\"DocumentName\":\"" + DocumentName + "\",\"DocumentValue\":\"" + DocumentValue + "\",\"Documentpath\":\"" + Documentpath + "\"}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json"; // Change to "application/x-www-form-urlencoded" if needed
                request.Accept = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                    streamWriter.Flush();
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    res = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                res = "";
            }
            return res;

        }


    }

    //  private readonly DocumentService _documentService;
}
