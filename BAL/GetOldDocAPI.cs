using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Newtonsoft.Json.Linq;

namespace BAL
{
    public class GetOldDocAPI
    {
        #region 46 server using com ref id Download Candidate Doc and Report
        public static string DownloadCandidateDocReport2(string DocumentName, string DocumentType, string DocumentValue)
        {
            string res = "";
            StringBuilder StrBuilderObj = new StringBuilder();
            StrBuilderObj.Clear();
            try
            {
                string url = ConfigurationManager.AppSettings["DocumentBaseURL"].ToString() + "Get-Base64-AsFile-Document/";
                string jsonData = "{\"DocumentType\":\"" + DocumentType + "\",\"DocumentName\":\"" + DocumentName + "\",\"DocumentValue\":\"" + DocumentValue + "\"}";
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
        #endregion
        #region Document download dynamic
        public static string DocumentDownloadDynamic(string DocumentName)
        {
            string res = "";
            var url = "http://10.83.0.76:8091/Get-Doc-File-Document";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            string jsonData = "{\"DocumentName\":\"" + DocumentName + "\",\"Documentpath\":\"" + "D:\\\\BGVApplicationV2ArchivalFrom_Jan2025\\\\WeVerifyDocument\\\\Documents\\\\CandidateDocuments\\\\ProvidedDocuments\\\\" + "\"}";
            // Write JSON data to request stream
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonData);
                streamWriter.Flush();
            }

            // Get response
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    JObject obj = JObject.Parse(result);
                    res = obj["DocumentValue"].ToString();

                }
            }
            catch (WebException ex)
            {
                res = "";
                using (var errorResponse = (HttpWebResponse)ex.Response)
                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    string error = reader.ReadToEnd();
                    Console.WriteLine("Error Response:");
                    Console.WriteLine(error);
                }
            }
            return res;
        }
        public static string ReportDownloadDynamic(string DocumentName, string PathDoc)
        {
            string res = "";
            var url = "http://10.83.0.76:8091/Get-Doc-File-Document";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            PathDoc = "CandidateReport" + PathDoc;
            string jsonData = "{\"DocumentName\":\"" + DocumentName + "\",\"Documentpath\":\"" + "D:\\\\BGVApplicationV2ArchivalFrom_Jan2025\\\\WeVerifyDocument\\\\Documents\\\\" + PathDoc + "\\\\" + "\"}";
            // Write JSON data to request stream
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonData);
                streamWriter.Flush();
            }

            // Get response
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    JObject obj = JObject.Parse(result);
                    res = obj["DocumentValue"].ToString();

                }
            }
            catch (WebException ex)
            {
                res = "";
                using (var errorResponse = (HttpWebResponse)ex.Response)
                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    string error = reader.ReadToEnd();
                    Console.WriteLine("Error Response:");
                    Console.WriteLine(error);
                }
            }
            return res;
        }

        #endregion
        #region Document download 55 old
        public static string DocumentDownloadOld(string fileName, string Qtype)
        {
            string res = "";
            StringBuilder StrBuilderObj = new StringBuilder();
            StrBuilderObj.Clear();
            try
            {
                string url = "http://10.83.0.55:8888/Get-File-Doc-AsBase64-Document/" + fileName + "/" + Qtype;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "application/json";
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

        #endregion
        #region Database access
        public DataTable Get_DocList2(string CompanyRefNo, string QType)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetAllDocumentListAPI",
                DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
                DAS.CreateParameter("@QType", SqlDbType.VarChar, QType, ParameterDirection.Input, 100, false)

                );
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_DocListOld(string CompanyRefNo)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetAllDocumentCaswWiseListAPI",
                DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false)

                );
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable Get_ReportHistories(string CompanyRefNo)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_CaseReportHistories_API",
                DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false)

                );
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetConvertbase64Document(string DocumentPath)
        {
            string AsBase64String = "";
            try
            {


                using (WebClient client = new WebClient())
                {
                    byte[] image = client.DownloadData(DocumentPath);
                    AsBase64String = Convert.ToBase64String(image);
                }
            }
            catch (Exception ex)
            {
                AsBase64String = "";
            }
            return AsBase64String;
        }

        #endregion
    }
}
