using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using BAL.HRRepository;
using DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BAL.Models;

namespace BAL
{
    public class FileSaveDownloadAPI
    {
        #region 46 server using com ref id Download Candidate Doc and Report
        #region Document download dynamic
        public static string DocAndReportDownloadDynamic(string DocumentName, string DocPath)
        {
            string res = "";
            DocPath = DocPath.Replace(@"\", @"\\");
            var url = ConfigurationManager.AppSettings["DocumentBaseURL"].ToString() + "Get-Doc-File-Document";
            //var url = "http://localhost:51656/Get-Doc-File-Document";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            string jsonData = "{\"DocumentName\":\"" + DocumentName + "\",\"Documentpath\":\"" + DocPath + "\"}";
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
                }
            }
            return res;
        }
        public static string DocReportUploadDynamic(string DocumentName, string DocPath, string DocumentValue)
        {
            string res = "";
            var url = ConfigurationManager.AppSettings["DocumentBaseURL"].ToString() + "Upload-Document-Base64-AsFile";
            //var url = "http://localhost:51656/Upload-Document-Base64-AsFile";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            DocPath = DocPath.Replace(@"\", @"\\");
            request.ContentType = "application/json";
            string jsonData = "{\"DocumentName\":\"" + DocumentName + "\",\"Documentpath\":\"" + DocPath + "\",\"DocumentValue\":\"" + DocumentValue + "\"}";
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
                    res = result;

                }
            }
            catch (WebException ex)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    string error = reader.ReadToEnd();
                    res = error;
                }
            }
            return res;
        }
        public static string DocReportdeleteDynamic(string DocumentName, string DocPath)
        {
            string res = "";
            DocPath = DocPath.Replace(@"\", @"\\");
            var url = ConfigurationManager.AppSettings["DocumentBaseURL"].ToString() + "Delete-Document-API";

            // var url = "http://localhost:51656/Delete-Document-API";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.ContentType = "application/json";
            string jsonData = "{\"DocumentName\":\"" + DocumentName + "\",\"Documentpath\":\"" + DocPath + "\"}";
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
                    res = result;

                }
            }
            catch (WebException ex)
            {
                res = "";
                using (var errorResponse = (HttpWebResponse)ex.Response)
                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    string error = reader.ReadToEnd();
                }
            }
            return res;
        }

        #endregion
        #endregion

        #region Get Client pushed details
        public static List<CheckCaseDetails> CaseListDetails(string UniqueReferenceID, int ClientRowID)
        {
            List<CheckCaseDetails> lst = new List<CheckCaseDetails>();
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "BindCheckListLTI",
                DAS.CreateParameter("@Type", SqlDbType.VarChar, "CheckCaseDetails", ParameterDirection.Input, 200, false),
                 DAS.CreateParameter("@UniqueReferenceID", SqlDbType.VarChar, UniqueReferenceID, ParameterDirection.Input, 200, false),
                 DAS.CreateParameter("@ClientRowID", SqlDbType.Int, ClientRowID, ParameterDirection.Input, 200, false)
                );
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lst.Add(new CheckCaseDetails
                    {
                        UniqueReferenceID = dt.Rows[i]["UniqueReferenceID"].ToString(),
                        Status = dt.Rows[i]["Status"].ToString(),
                        DataType = dt.Rows[i]["DataType"].ToString(),
                        DataValue = GetValues(dt.Rows[i]["DataValue"].ToString().Trim(), ClientRowID),
                        Remarks = dt.Rows[i]["Remarks"].ToString(),
                        CreatedOn = dt.Rows[i]["CreatedOn"].ToString(),
                        CreatedBy = dt.Rows[i]["CreatedBy"].ToString(),
                        SubCheckRowID = dt.Rows[i]["SubCheckRowID"].ToString(),
                    });

                }



            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
            return lst;
        }

        public static string GetValues(string values, int ClientRowID)
        {
            string SubCheckName = "";
            if (ClientRowID != 990)
            {
                try
                {
                    if (values.Contains("SubCheckRowID:"))
                    {
                        int SubCheckRowID = 0;
                        string input2 = values;
                        Match match = Regex.Match(input2, @"SubCheckRowID:(\d+)");
                        if (match.Success)
                        {
                            SubCheckRowID = Convert.ToInt32(match.Groups[1].Value);
                        }
                        DataTable dt = new DataTable();
                        dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetCheckNameDetailsAPI",
                        DAS.CreateParameter("@ClientRowID", SqlDbType.Int, ClientRowID, ParameterDirection.Input, 200, false),
                         DAS.CreateParameter("@SubCheckRowID", SqlDbType.VarChar, SubCheckRowID, ParameterDirection.Input, 200, false)
                         );
                        if (dt.Rows.Count > 0)
                        {
                            SubCheckName = dt.Rows[0]["SubCheckName"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            string input = values;
            string pattern = "";
            string pattern2 = "";
            string pattern3 = "";
            string result = "";
            if (ClientRowID == 990)
            {
                pattern3 = @"RequestData:PersonalRowId:\d+ ,SubCheckRowID:\d+ ,";
                result = Regex.Replace(input, pattern3, "");
            }
            else
            {
                if (values.Contains("SubCheckRowID:"))
                {
                    pattern = @"^PersonalRowId:\d+,SubCheckRowID:\d+,";
                    result = Regex.Replace(input, pattern, "");
                }
                else
                {
                    pattern2 = @"^PersonalRowId:\d+,";
                    result = Regex.Replace(input, pattern, "");
                }
            }
            if (ClientRowID != 990)
            {
                if (values.Contains("SubCheckRowID:"))
                {
                    result = "Check Name: " + SubCheckName + " " + result;
                }
            }
            return result;
        }

        #endregion

        #region Doc Download API
        public static string GetCandidateDetail_Documents2(string CompanyRefNo, int clientRowId)
        {
            string strFileName = "";
            DataBaseConnections Dc = new DataBaseConnections();
            try
            {
                DataTable dts = new AccountRepository().GetPersonalRowId(CompanyRefNo);
                ModelLTI.CandidateList candidate = new ModelLTI.CandidateList();
                if (dts.Rows.Count > 0)
                {
                    candidate.RequestID = "";
                    candidate.PSNO = "";
                    candidate.CandidateID = "";
                    if (dts.Rows[0]["ClientRefID"].ToString() != "")
                    {
                        candidate.RequestID = dts.Rows[0]["ClientRefID"].ToString();
                    }
                    if (dts.Rows[0]["EmployeeID"].ToString() != "")
                    {
                        candidate.CandidateID = dts.Rows[0]["EmployeeID"].ToString();
                    }
                    if (dts.Rows[0]["CandidateCode"].ToString() != "")
                    {
                        candidate.PSNO = dts.Rows[0]["CandidateCode"].ToString();
                    }
                    string username = "";
                    string password = "";
                    var ClientAuth = Dc.SP_GetClientAPIDetails(Convert.ToInt32(clientRowId), "Bearer Token", "BGVCheckStatus");
                    if (ClientAuth.Rows.Count > 0)
                    {
                        username = Convert.ToString(ClientAuth.Rows[0]["UserName"]);
                        password = Convert.ToString(ClientAuth.Rows[0]["Password"]);
                    }
                    candidate.VendorID = "9FEE5CFF";
                    ModelLTI.Token token = GetAccessToken(username, password);
                    ////string URL = "https://LTIiVerifyTestAPI.azurewebsites.net/VendorUpdateService/FetchVendorChecksNew";
                    ////=========Production========================
                    string URL = "https://ltiiverifyapiprodnew.azurewebsites.net/VendorUpdateService/FetchVendorChecksNew";
                    HttpClientHandler handler = new HttpClientHandler();
                    HttpClient client = new HttpClient(handler);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
                    var json = "{\"RequestID\":\"" + candidate.RequestID + "\",\"CandidateID\":\"" + candidate.CandidateID + "\",\"PSNO\":\"" + candidate.PSNO + "\",\"VendorID\":\"" + candidate.VendorID + "\"}";
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    var APIResponse = client.PostAsync(URL, data).Result;
                    if (APIResponse.IsSuccessStatusCode)
                    {
                        var jsonresult = APIResponse.Content.ReadAsStringAsync().Result;
                        CandidateDetails objCandidate = JsonConvert.DeserializeObject<CandidateDetails>(jsonresult);


                        JToken jdata = JObject.Parse(jsonresult);
                        ////Save Data into APIDEMO Database
                        LiCandidateBasicInfo bInfo = new LiCandidateBasicInfo();
                        var CheckViewModel = new List<LiChecksRequired>();

                        ModelLTI.UpdateCandidateChecksStatus objCheckStatus = new ModelLTI.UpdateCandidateChecksStatus();
                        objCheckStatus.CandidateID = candidate.CandidateID;
                        objCheckStatus.RequestID = candidate.RequestID;
                        objCheckStatus.Name = candidate.Name;
                        objCheckStatus.PSNO = candidate.PSNO;
                        objCheckStatus.VendorName = candidate.VendorID;

                        CaseInitiatedAcknowledgement objsAck = new CaseInitiatedAcknowledgement();
                        objsAck.CandidateID = candidate.CandidateID;
                        objsAck.RequestID = candidate.RequestID;
                        objsAck.PSNO = candidate.PSNO;
                        objsAck.VendorID = candidate.VendorID;



                        var tempAPIDocumentDetail = new List<LiCandidateDocumentInfo>();
                        string Url = "";
                        if (objCandidate.liCandidateInformation != null)
                        {

                            if (objCandidate.liCandidateInformation.LOADocuments != null)
                            {
                                Url += objCandidate.liCandidateInformation.LOADocuments + ",";
                            }
                            if (objCandidate.liCandidateInformation.PanCardDocuments != null)
                            {
                                Url += objCandidate.liCandidateInformation.PanCardDocuments + ",";
                            }
                            if (objCandidate.liCandidateInformation.AadharCardDocuments != null)
                            {
                                Url += objCandidate.liCandidateInformation.AadharCardDocuments + ",";
                            }
                            if (objCandidate.liCandidateInformation.PassportDocuments != null)
                            {
                                Url += objCandidate.liCandidateInformation.PassportDocuments + ",";
                            }
                            if (objCandidate.liCandidateInformation.DrivingLicenceDocuments != null)
                            {
                                Url += objCandidate.liCandidateInformation.DrivingLicenceDocuments + ",";
                            }
                            if (objCandidate.liCandidateInformation.CVDocuments != null)
                            {
                                Url += objCandidate.liCandidateInformation.CVDocuments + ",";
                            }
                            if (objCandidate.liCandidateInformation.GapAnalysisDocuments != null)
                            {
                                Url += Convert.ToString(objCandidate.liCandidateInformation.GapAnalysisDocuments) + ",";
                            }
                            if (objCandidate.liCandidateInformation.liCandidateAadhaarInfo != null)
                            {
                                int Count = objCandidate.liCandidateInformation.liCandidateAadhaarInfo.Count();
                                for (int i = 0; i < Count; i++)
                                {
                                    if (objCandidate.liCandidateInformation.liCandidateAadhaarInfo[i].AadhaarDocument != null)
                                    {
                                        Url += objCandidate.liCandidateInformation.liCandidateAadhaarInfo[i].AadhaarDocument + ",";
                                    }
                                }
                            }
                            if (objCandidate.liCandidateInformation.liCandidateDLInfo != null)
                            {
                                int Count = objCandidate.liCandidateInformation.liCandidateDLInfo.Count();
                                for (int i = 0; i < Count; i++)
                                {
                                    if (objCandidate.liCandidateInformation.liCandidateDLInfo[i] != null)
                                    {
                                        Url += objCandidate.liCandidateInformation.liCandidateDLInfo[i].DLDocument + ",";
                                    }
                                }
                            }
                            if (objCandidate.liCandidateInformation.liCandidatePANInfo != null)
                            {
                                int Count = objCandidate.liCandidateInformation.liCandidatePANInfo.Count();
                                for (int i = 0; i < Count; i++)
                                {
                                    if (objCandidate.liCandidateInformation.liCandidatePANInfo[i] != null)
                                    {
                                        Url += objCandidate.liCandidateInformation.liCandidatePANInfo[i].PanDocument + ",";
                                    }
                                }
                            }

                            if (objCandidate.liCandidateInformation.liCandidatePassportInfo != null)
                            {
                                int Count = objCandidate.liCandidateInformation.liCandidatePassportInfo.Count();
                                for (int i = 0; i < Count; i++)
                                {
                                    if (objCandidate.liCandidateInformation.liCandidatePassportInfo[i] != null)
                                    {
                                        Url += objCandidate.liCandidateInformation.liCandidatePassportInfo[i].PassportDocument + ",";
                                    }
                                }
                            }
                            if (objCandidate.liCandidateInformation.liCandidateVoterIDInfo != null)
                            {
                                int Count = objCandidate.liCandidateInformation.liCandidateVoterIDInfo.Count();
                                for (int i = 0; i < Count; i++)
                                {
                                    if (objCandidate.liCandidateInformation.liCandidateVoterIDInfo[i] != null)
                                    {
                                        Url += objCandidate.liCandidateInformation.liCandidateVoterIDInfo[i].VoterIDDocument + ",";
                                    }
                                }
                            }
                            if (objCandidate.liCandidateInformation.liCandidateEducationInfo != null)
                            {

                                int Count = objCandidate.liCandidateInformation.liCandidateEducationInfo.Count();
                                for (int i = 0; i < Count; i++)
                                {
                                    if (objCandidate.liCandidateInformation.liCandidateEducationInfo[i].EducationDocuments != null)
                                    {
                                        Url += objCandidate.liCandidateInformation.liCandidateEducationInfo[i].EducationDocuments + ",";
                                    }
                                }
                                // Thread.Sleep(1000);

                            }
                            if (objCandidate.liCandidateInformation.liCandidateEmploymentInfo != null)
                            {

                                int Count = objCandidate.liCandidateInformation.liCandidateEmploymentInfo.Count();
                                for (int i = 0; i < Count; i++)
                                {
                                    if (objCandidate.liCandidateInformation.liCandidateEmploymentInfo[i].EmploymentDocuments != null)
                                    {
                                        Url += objCandidate.liCandidateInformation.liCandidateEmploymentInfo[i].EmploymentDocuments + ",";
                                    }
                                }
                                //Thread.Sleep(1000);

                            }
                            //if (Url != "")
                            //{
                            //    GetDocumentDetails(Url, clientRowId, candidate.RequestID, ref tempAPIDocumentDetail);
                            //}

                        }
                        if (Url != "")
                        {
                            strFileName = GetDocumentDetail2(Url, clientRowId, candidate.RequestID, ref tempAPIDocumentDetail, CompanyRefNo, "");
                        }
                        else
                        {
                            strFileName = "LTIMindtree documents not available please check case for api  details";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string str2 = ex.StackTrace;
                string str = ex.Message;
                //new AccountRepository().InsertAPIAuditDetail(candidate.RequestID, clientRowId, "GetCandidateDetail", ex.Message, "Failed", "Fetch API Scheduler");
            }
            return strFileName;
        }
        private static string GetDocumentDetail2(string Url, int clientRowId, string unique_id, ref List<LiCandidateDocumentInfo> tempAPIDocumentDetail, string CompanyRefNo, string Insufficiency_Remarks)
        {
            string res = "";
            DataBaseConnections Dc = new DataBaseConnections();
            try
            {
                string username = "";
                string password = "";
                var ClientAuth = Dc.SP_GetClientAPIDetails(Convert.ToInt32(clientRowId), "Bearer Token", "BGVCheckStatus");
                if (ClientAuth.Rows.Count > 0)
                {
                    username = Convert.ToString(ClientAuth.Rows[0]["UserName"]);
                    password = Convert.ToString(ClientAuth.Rows[0]["Password"]);
                }
                ModelLTI.Token token = GetAccessToken(username, password);
                /////string URL = "https://LTIiVerifyTestAPI.azurewebsites.net/VendorUpdateService/FetchVendorChecksNew"; // Test server
                ////=========Production========================
                string[] strURL = Url.Split(',');
                foreach (string Urldetails in strURL)
                {

                    if (Urldetails.Contains("https://"))
                    {
                        string URL = Urldetails;
                        HttpClientHandler handler = new HttpClientHandler();
                        HttpClient client = new HttpClient(handler);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
                        var json = "";
                        //var json = new JavaScriptSerializer().Serialize(new
                        //{
                        //    //RequestID = candidate.RequestID,
                        //    //CandidateID = candidate.CandidateID,
                        //    //PSNO = candidate.PSNO,
                        //    //VendorID = candidate.VendorID
                        //});
                        var data = new StringContent(json, Encoding.UTF8, "application/json");

                        var APIResponse = client.PostAsync(URL, data).Result;
                        string jsonresult = "";
                        if (APIResponse.IsSuccessStatusCode)
                        {
                            jsonresult = APIResponse.Content.ReadAsStringAsync().Result;
                        }
                        res = DrillToMessage_DocumentDeatil2(jsonresult, clientRowId, unique_id, ref tempAPIDocumentDetail, CompanyRefNo, Insufficiency_Remarks);
                    }

                }
                //Thread.Sleep(10000);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                //new AccountRepository().InsertAPIAuditDetail(unique_id, clientRowId, "GetDocumentDetails", "" + "," + ex.Message, "Failed", "Fetch API Scheduler");

            }
            return res;
        }
        public static string DrillToMessage_DocumentDeatil2(string jsonresult, int clientrowid, string uniqueNo, ref List<LiCandidateDocumentInfo> tempAPIDocumentDetail, string CompanyRefNo, string Insufficiency_Remarks)
        {
            string strFileName = "";
            //if (item is JArray)
            //{
            //foreach (var v in item)
            //{
            dynamic Result = JsonConvert.DeserializeObject(jsonresult);
            string DocumentName = "";
            string FileType = "";
            string DocumentValue2 = "";
            var candDoc = new LiCandidateDocumentInfo();
            foreach (var item in Result)
            {
                if (((string)item["DocumentName"]).ToString() != "")
                {
                    DocumentName = ((string)item["DocumentName"]).ToString();
                }
                if (((string)item["FileType"]).ToString() != "")
                {
                    FileType = ((string)item["FileType"]).ToString();
                }
                if (((string)item["DocumentAttachment"]).ToString() != "")
                {
                    DocumentValue2 = ((string)item["DocumentAttachment"]).ToString();
                }
                strFileName = API_GetCandidateDocuments2(uniqueNo, DocumentValue2, clientrowid, DocumentName, CompanyRefNo, Insufficiency_Remarks);/// Without candidation
                //var suc = UploadFileByFTPToServerFolder(strFileName, PersonalRowID, SubCheckRowID);

            }
            //Thread.Sleep(1000);

            return strFileName;
        }
        public static string API_GetCandidateDocuments2(string unique_id, string file64, int clientId, string fileName, string CompanyRefNo, string Insufficiency_Remarks)
        {
            //string[] spplit_file = file_key.Split('_');
            //string ext = Path.GetExtension(fileName);
            string result = "";
            string contentType = "";
            string strfileName = unique_id + "_" + clientId + "_" + fileName;
            fileName = fileName.Replace(" ", "_").Replace("-", "_");
            fileName = unique_id + "_" + clientId + "_" + DateTime.Now.ToString("ddMMhhyymmss") + "_" + fileName;
            var fileContents = Convert.FromBase64String(file64);
            //return File(fileContents, contentType, fileName);
            result = convertbase64topdf2(file64, fileName, CompanyRefNo, Insufficiency_Remarks);
            return result;

        }

        private static ModelLTI.Token GetAccessToken(string username, string password)
        {
            ////string baseAddress = "https://LTIiVerifyTestAPI.azurewebsites.net/TOKEN";
            ////======Production==========
            string baseAddress = "https://ltiiverifyapiprodnew.azurewebsites.net/TOKEN";
            ModelLTI.Token token = new ModelLTI.Token();
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            var RequestBody = new Dictionary<string, string>
                {
                {"grant_type", "password"},
                {"username", username},
                {"password", password},
                };
            var tokenResponse = client.PostAsync(baseAddress, new FormUrlEncodedContent(RequestBody)).Result;
            if (tokenResponse.IsSuccessStatusCode)
            {
                var JsonContent = tokenResponse.Content.ReadAsStringAsync().Result;
                ModelLTI.Token Cyient_Candidate = JsonConvert.DeserializeObject<ModelLTI.Token>(JsonContent);
                token.access_token = Cyient_Candidate.access_token.ToString();
                token.error = null;
            }
            else
            {
                token.error = "Not able to generate Access Token Invalid usrename or password";
            }
            return token;
            ;
        }
        public static string convertbase64topdf2(string base64, string fileName, string CompanyRefNo, string Insufficiency_Remarks)
        {
            try
            {
                string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Documents/LTIDoc/") + CompanyRefNo;
                // string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Documents/LTI/") + CompanyRefNo + "/";
                bool exists = System.IO.Directory.Exists(filePath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(filePath);
                filePath = filePath + "/" + fileName;
                string filePathSave = System.Web.HttpContext.Current.Server.MapPath("~/Documents/LTIDoc/") + fileName;
                File.WriteAllBytes(filePath, Convert.FromBase64String(base64));
            }
            catch (Exception ex)
            {

            }
            return fileName;
        }

        #endregion
    }
}
