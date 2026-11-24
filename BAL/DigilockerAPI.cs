using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BAL
{
    public class DigilockerAPI
    {
        ClientCustomize cc = new ClientCustomize();
        StringBuilder StrBuilderObj = new StringBuilder();
        private static Random random = new Random();
        public static int RandomIntFromInterval(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        public static string GenerateCodeVerifier(int length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
            StringBuilder codeVerifier = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(characters.Length);
                codeVerifier.Append(characters[randomIndex]);
            }
            return codeVerifier.ToString();
        }

        public static async Task<byte[]> Sha256(string plain)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(plain);
                return await Task.FromResult(sha256.ComputeHash(data));
            }
        }

        public static string Base64UrlEncode(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);
            return base64.Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        public static async Task<string> GenerateCodeChallenge(string codeVerifier)
        {
            byte[] hashedBuffer = await Sha256(codeVerifier);
            return Base64UrlEncode(hashedBuffer);
        }

        public string SendDigiLockerLink(string CompanyRefNo)
        {
            string res = "";
            int randomByte = RandomIntFromInterval(32, 96);
            string codeVerifier = GenerateCodeVerifier(randomByte);
            var codeChallenge = GenerateCodeChallenge(codeVerifier);

            string NSSOauthorizeURL = "https://digilocker.meripehchaan.gov.in/public/oauth2/1/authorize?response_type=code&client_id=EY2460D287&state=" + CompanyRefNo + "&redirect_uri=";
            NSSOauthorizeURL = NSSOauthorizeURL + "https://localhost:44386/DigiLocker/DigiLockerRedirect_uri";
            NSSOauthorizeURL = NSSOauthorizeURL + "&code_challenge=" + codeChallenge.Result;
            NSSOauthorizeURL = NSSOauthorizeURL + "&code_challenge_method=S256&scope=files.issueddocs+files.uploadeddocs";

            int rst = InsertDigiLockerUserDetails(CompanyRefNo, NSSOauthorizeURL, codeVerifier, 0);
            if (rst > 0)
            {
                // SendAlertMassageForDigiLocker(CompanyRefNo, NSSOauthorizeURL);
            }
            return res;

        }

        // GET: DigiLocker
        public string DigiLockerRedirect_uri(string code, string State)
        {
            string resp = "";
            string CompanyRefNo = "";
            try
            {
                var codeVerifierDT = Sp_GetDigiLockerUserDetails(State);
                if (codeVerifierDT.Rows.Count > 0)
                {
                    // string codeVerifier = Convert.ToString("bofrvPgwSJzpqaTnt81C5quwK7SL6EeobmP");
                    string codeVerifier = Convert.ToString(codeVerifierDT.Rows[0]["codeVerifier"]);
                    CompanyRefNo = Convert.ToString(codeVerifierDT.Rows[0]["CompanyRefNo"]);
                    var access_token = GetAccessTokenAsync(code, codeVerifier, CompanyRefNo);
                    var documentslst = GetDocumentsIssuedList(access_token.access_token, CompanyRefNo);
                    foreach (var item in documentslst.items)
                    {
                        var Doc = GetfilefromURI(CompanyRefNo, access_token.access_token, item.uri, CompanyRefNo);
                    }
                    var res = RevokeToken(access_token.access_token);
                    resp = "Documents Received";
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("----------------------------API DigiLockerRedirect_uri-----------------");
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("Response:Exception" + msg);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "DigiLocker", "DigiLockerRedirect_uri", CompanyRefNo + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
            }
            return resp;

        }
        public tokenobject GetAccessTokenAsync(string authorizationCode = "", string codeVerifier = "", string PersonalRowId = "")
        {
            tokenobject myDeserializedClass = new tokenobject();
            try
            {
                string URL = "https://digilocker.meripehchaan.gov.in/public/oauth2/1/token";
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                var json = new JavaScriptSerializer().Serialize(new
                {
                    grant_type = "authorization_code",
                    code = authorizationCode,
                    redirect_uri = "https://bgv.securitas-india.com/Account/DigiLockerRedirect_uri",
                    code_verifier = codeVerifier,
                    client_id = "EY2460D287",
                    client_secret = "61e9abf64d79baa9ebdb"
                });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var APIResponse = client.PostAsync(URL, data).Result;
                if (APIResponse.IsSuccessStatusCode)
                {
                    var jsonresult = APIResponse.Content.ReadAsStringAsync().Result;
                    myDeserializedClass = JsonConvert.DeserializeObject<tokenobject>(jsonresult);

                    return myDeserializedClass;
                }
                else
                {
                    StrBuilderObj.Append("----------------------------API GetAccessTokenAsync-----------------");
                    StrBuilderObj.Append(Environment.NewLine);
                    StrBuilderObj.Append("Response:" + APIResponse.RequestMessage.ToString());
                    API.WriteLog(StrBuilderObj.ToString(), "", "", "DigiLocker", "GetAccessTokenAsync", PersonalRowId + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                StrBuilderObj.Append("----------------------------API GetAccessTokenAsync-----------------");
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("Response:Exception" + msg);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "DigiLocker", "GetAccessTokenAsync", PersonalRowId + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));

            }
            return myDeserializedClass;
        }


        public IssueDocListobject GetDocumentsIssuedList(string accessToken = "", string PersonalRowId = "")
        {
            IssueDocListobject myDeserializedClass = new IssueDocListobject();
            try
            {
                string URL = "https://digilocker.meripehchaan.gov.in/public/oauth2/1/files/issued";
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var APIResponse = client.GetAsync(URL).Result;
                if (APIResponse.IsSuccessStatusCode)
                {
                    var jsonresult = APIResponse.Content.ReadAsStringAsync().Result;
                    myDeserializedClass = JsonConvert.DeserializeObject<IssueDocListobject>(jsonresult);

                    return myDeserializedClass;
                }
                else
                {
                    StrBuilderObj.Append("----------------------------API GetDocumentsIssuedList-----------------");
                    StrBuilderObj.Append(Environment.NewLine);
                    StrBuilderObj.Append("Response:" + APIResponse.RequestMessage.ToString());
                    API.WriteLog(StrBuilderObj.ToString(), "", "", "DigiLocker", "GetDocumentsIssuedList", PersonalRowId + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                StrBuilderObj.Append("----------------------------API GetDocumentsIssuedList-----------------");
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("Response:Exception" + msg);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "DigiLocker", "GetDocumentsIssuedList", PersonalRowId + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));

            }
            return myDeserializedClass;
        }
        public string GetfilefromURI(string accessToken = "", string Fileid = "", string Path = "", string PersonalRowID = "",string CaseId="")
        {
            IssueDocListobject myDeserializedClass = new IssueDocListobject();
            String AsBase64String = "";
            try
            {
                string URL = "https://digilocker.meripehchaan.gov.in/public/oauth2/1/file/" + Fileid;
                HttpClientHandler handler = new HttpClientHandler();
                Path = Path + "/" + Fileid + ".pdf";
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var APIResponse = client.GetAsync(URL).Result;
                Stream srcImgStream = new MemoryStream();
                if (APIResponse.IsSuccessStatusCode)
                {
                    srcImgStream = APIResponse.Content.ReadAsStreamAsync().Result;

                    byte[] bytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        srcImgStream.CopyTo(memoryStream);
                        bytes = memoryStream.ToArray();
                    }

                    System.IO.File.WriteAllBytes(Path, bytes);
                    if (PersonalRowID != "")
                    {
                        int rst = InsertDigiLockerDocumentDetails(Convert.ToInt32(PersonalRowID), Fileid + ".pdf", Path, "Done by Digilocker API","");

                    }
                    else
                    {
                        int rst = InsertDigiLockerDocumentDetails(0, Fileid + ".pdf", Path, "Done by Digilocker API", CaseId);

                    }
                }
                else
                {
                    StrBuilderObj.Append("----------------------------API GetfilefromURI-----------------");
                    StrBuilderObj.Append(Environment.NewLine);
                    StrBuilderObj.Append("Response:" + APIResponse.RequestMessage.ToString());
                    API.WriteLog(StrBuilderObj.ToString(), "", "", "DigiLocker", "GetfilefromURI", PersonalRowID + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                StrBuilderObj.Append("----------------------------API GetfilefromURI-----------------");
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("Response:Exception" + msg);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "DigiLocker", "GetfilefromURI", PersonalRowID + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));

            }
            return AsBase64String;

        }
        public bool RevokeToken(string accessToken = "")
        {
            bool res = false;
            string URL = "https://api.digitallocker.gov.in/public/oauth2/1/revoke";
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            var json = new JavaScriptSerializer().Serialize(new
            {
                client_id = "EY2460D287",
                client_secret = "61e9abf64d79baa9ebdb",
                token = accessToken,
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var APIResponse = client.PostAsync(URL, data).Result;
            if (APIResponse.IsSuccessStatusCode)
            {
                var jsonresult = APIResponse.Content.ReadAsStringAsync().Result;
                if (jsonresult.Contains("true"))
                {
                    res = true;
                }
                return res;
            }
            return res;
        }
        #region Digilocker API
        public DataTable GetPersonalandClientDetails(string CompanyRefNo)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetPersonalandClientDetails",
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false)
            );
            return dt;
        }
        public DataTable Get_Personal_University_Details(string CheckUniqueID)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Get_Personal_University_Details",
            DAS.CreateParameter("@CheckUniqueID", SqlDbType.VarChar, CheckUniqueID, ParameterDirection.Input, 100, false)
            );
            return dt;
        }
        public DataTable Sp_GetListDigiLockerUserDetails()
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetListDigiLockerUserDetails"
               );
            return dt;
        }
        public DataTable Sp_GetDigiLockerUserDetails(string State)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetDigiLockerUserDetails",
                    DAS.CreateParameter("@State", SqlDbType.VarChar, State, ParameterDirection.Input, 100, false)

               );
            return dt;
        }
        public DataTable GetDigiLockerUserDetails(string State)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetDigiLockerUserDetails",
                    DAS.CreateParameter("@State", SqlDbType.VarChar, State, ParameterDirection.Input, 100, false)

               );
            return dt;
        }
        public DataTable Sp_GetDigiLockerUserPendinDetails(string State)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetDigiLockerUserPendingDetails",
                    DAS.CreateParameter("@State", SqlDbType.VarChar, State, ParameterDirection.Input, 100, false)

               );
            return dt;
        }
        public DataTable Get_DigiLockerDocumentDetails(int PersonalRowId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Get_DigiLockerDocumentDetails",
                    DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 100, false)

               );
            return dt;
        }
        public DataTable GetDigiLockerDocumentDetails(int PersonalRowId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetDigiLockerDocumentDetails",
                    DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 100, false)

               );
            return dt;
        }
        public int InsertDigiLockerUserDetails(string CompanyRefNo, string NSSOauthorizeURL, string codeVerifier, int LinksendBy)
        {
            int intReturnValue = 0;
            string SP_PARAM_RETURN_VALUE = null;
            DataTable dt = new DataTable();
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertDigiLockerUserDetails",
                 DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
                     DAS.CreateParameter("@NSSOauthorizeURL", SqlDbType.VarChar, NSSOauthorizeURL, ParameterDirection.Input, 2000, false),
                       DAS.CreateParameter("@codeVerifier", SqlDbType.VarChar, codeVerifier, ParameterDirection.Input, 1000, false),
                        DAS.CreateParameter("@LinksendBy", SqlDbType.Int, LinksendBy, ParameterDirection.Input, 10, false),

                        objReturnValueParam
                        );
            return intReturnValue = Convert.ToInt32(objReturnValueParam.Value);

        }
        public DataTable Sp_GetMasterCompanies(int CompanyRowID)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetMasterCompanies",
                    DAS.CreateParameter("@CompanyRowID", SqlDbType.VarChar, CompanyRowID, ParameterDirection.Input, 10, false)

               );
            return dt;
        }
        public int InsertDigiLockerDocumentDetails(int CompanyRefNo, string @FileName, string Path, string Remarks,string CaseId)
        {
            int intReturnValue = 0;
            string SP_PARAM_RETURN_VALUE = null;
            DataTable dt = new DataTable();
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "Insert_DigiLockerDocumentDetails",
                 DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, CompanyRefNo, ParameterDirection.Input, 50, false),
                     DAS.CreateParameter("@FileName", SqlDbType.VarChar, @FileName, ParameterDirection.Input, 500, false),
                       DAS.CreateParameter("@Path", SqlDbType.VarChar, Path, ParameterDirection.Input, 500, false),
                        DAS.CreateParameter("@Remarks", SqlDbType.VarChar, Remarks, ParameterDirection.Input, 200, false),
                        DAS.CreateParameter("@CaseId", SqlDbType.VarChar, CaseId, ParameterDirection.Input, 200, false),

                        objReturnValueParam
                        );
            return intReturnValue = Convert.ToInt32(objReturnValueParam.Value);

        }
        public DataTable GetDigiLockerUserDetail(string CompanyRefNo, int CandidateStatus)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_DigiLockerUserDetails",
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@CandidateStatus", SqlDbType.Int, CandidateStatus, ParameterDirection.Input, 100, false)

            );
            return dt;
        }

        public int UpdateCandidateStatus(string personalRowID)
        {
            int intReturnValue = 0;
            string SP_PARAM_RETURN_VALUE = null;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "UpdateCandidateStatus",
            DAS.CreateParameter("@personalRowID", SqlDbType.NVarChar, personalRowID, ParameterDirection.Input, 100, false),
            objReturnValueParam
           );
           return intReturnValue = Convert.ToInt32(objReturnValueParam.Value);
        }
        #endregion
        #region Perfios API
        public GetOtpmodel GetOtpDetails(string UanNo = "", string CompanyRefNo = "")
        {
            GetOtpmodel myDeserializedClass = new GetOtpmodel();
            try
            {
                string URL = "https://testapi.karza.in/v2/epf-get-otp";   ///Test url
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("x-karza-key", "LLldGYAwhhklJ5w5");
                var json = new JavaScriptSerializer().Serialize(new
                {
                    consent = "Y",
                    uan = UanNo,
                });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var APIResponse = client.PostAsync(URL, data).Result;
                if (APIResponse.IsSuccessStatusCode)
                {
                    var jsonresult = APIResponse.Content.ReadAsStringAsync().Result;
                    myDeserializedClass = JsonConvert.DeserializeObject<GetOtpmodel>(jsonresult);
                    return myDeserializedClass;
                }
                else
                {
                    StrBuilderObj.Append("----------------------------API GetOtpPerfiosAPI-----------------");
                    StrBuilderObj.Append(Environment.NewLine);
                    StrBuilderObj.Append("Response:" + APIResponse.RequestMessage.ToString());
                    API.WriteLog(StrBuilderObj.ToString(), "", "", "PerfiosAPI", "GetOtpPerfiosAPI", CompanyRefNo + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                StrBuilderObj.Append("----------------------------API GetOtpPerfiosAPI-----------------");
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("Response:Exception" + msg);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "PerfiosAPI", "GetOtpPerfiosAPI", CompanyRefNo + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));

            }
            return myDeserializedClass;
        }
        public GetUanDetails GetUanDetails(string request_id = "", string otp = "", string CompanyRefNo = "", string FilePath = "")
        {
            GetUanDetails myDeserializedClass = new GetUanDetails();
            try
            {
                string URL = "https://testapi.karza.in/v2/epf-get-passbook";   ///Test url
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("x-karza-key", "LLldGYAwhhklJ5w5");
                var json = new JavaScriptSerializer().Serialize(new
                {
                    request_id = request_id,
                    otp = otp,
                    is_pdf_required = "y",
                    partial_data = "n",
                    epf_balance = "y"
                });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var APIResponse = client.PostAsync(URL, data).Result;
                if (APIResponse.IsSuccessStatusCode)
                {
                    var jsonresult = APIResponse.Content.ReadAsStringAsync().Result;
                    myDeserializedClass = JsonConvert.DeserializeObject<GetUanDetails>(jsonresult);
                    ModelPerfiosUanDetails Objda = new ModelPerfiosUanDetails();
                    ModelPerfiosUan_est_details ObjEst = new ModelPerfiosUan_est_details();
                    ModelPerfiosUan_est_details_Passbook ObjPass = new ModelPerfiosUan_est_details_Passbook();
                    ModelPerfiosUan_Pdf_Details ObjPdf = new ModelPerfiosUan_Pdf_Details();
                    if (myDeserializedClass.result.overall_pf_balance != null)
                    {
                        Objda.CompanyRefNo = Convert.ToString(CompanyRefNo);
                        Objda.PersonalRowID = 0;
                        Objda.request_id = Convert.ToString(myDeserializedClass.request_id);
                        Objda.Member_name = Convert.ToString(myDeserializedClass.result.employee_details.member_name);
                        Objda.Father_name = Convert.ToString(myDeserializedClass.result.employee_details.father_name);
                        Objda.Dob = Convert.ToString(myDeserializedClass.result.employee_details.dob);
                        Objda.Pension_balance = Convert.ToString(myDeserializedClass.result.overall_pf_balance.pension_balance);
                        Objda.Current_pf_balance = Convert.ToString(myDeserializedClass.result.overall_pf_balance.current_pf_balance);
                        Objda.Employee_Share_Total_Debit = Convert.ToString(myDeserializedClass.result.overall_pf_balance.employee_share_total.debit);
                        Objda.Employee_Share_Total_credit = Convert.ToString(myDeserializedClass.result.overall_pf_balance.employee_share_total.credit);
                        Objda.Employee_Share_Total_balance = Convert.ToString(myDeserializedClass.result.overall_pf_balance.employee_share_total.balance);
                        Objda.Employer_Share_Total_Debit = Convert.ToString(myDeserializedClass.result.overall_pf_balance.employer_share_total.debit);
                        Objda.Employer_Share_Total_credit = Convert.ToString(myDeserializedClass.result.overall_pf_balance.employer_share_total.credit);
                        Objda.Employer_Share_Total_balance = Convert.ToString(myDeserializedClass.result.overall_pf_balance.employer_share_total.balance);
                        Objda.CreatedBy = "0";
                        int APIAutoID = InsertPerfiosUanDetails(Objda);
                        foreach (var Items in myDeserializedClass.result.est_details)
                        {
                            ObjEst.APIAutoID = APIAutoID;
                            ObjEst.Est_name = Convert.ToString(Items.est_name);
                            ObjEst.Member_id = Convert.ToString(Items.member_id);
                            ObjEst.Office = Convert.ToString(Items.office);
                            ObjEst.Doj_epf = Convert.ToString(Items.doj_epf);
                            ObjEst.Doe_epf = Convert.ToString(Items.doe_epf);
                            ObjEst.Doe_eps = Convert.ToString(Items.doe_eps);
                            ObjEst.Pf_Balance_net_balance = Convert.ToString(Items.pf_balance.net_balance);
                            ObjEst.Pf_Balance_is_pf_full_withdrawn = Convert.ToString(Items.pf_balance.is_pf_full_withdrawn);
                            ObjEst.Pf_Balance_is_pf_partial_withdrawn = Convert.ToString(Items.pf_balance.is_pf_partial_withdrawn);
                            ObjEst.Employee_Share_Total_Debit = Convert.ToString(Items.pf_balance.employee_share.debit);
                            ObjEst.Employee_Share_Total_credit = Convert.ToString(Items.pf_balance.employee_share.credit);
                            ObjEst.Employee_Share_Total_balance = Convert.ToString(Items.pf_balance.employee_share.balance);
                            ObjEst.Employer_Share_Total_Debit = Convert.ToString(Items.pf_balance.employer_share.debit);
                            ObjEst.Employer_Share_Total_credit = Convert.ToString(Items.pf_balance.employer_share.credit);
                            ObjEst.Employer_Share_Total_balance = Convert.ToString(Items.pf_balance.employer_share.balance);
                            ObjEst.CreatedBy = "0";
                            int AID = InsertPerfiosUan_est_details(ObjEst);
                            foreach (var Item in Items.passbook)
                            {
                                ObjPass.APIAutoID = APIAutoID;
                                ObjPass.Tr_date_my = Convert.ToString(Item.tr_date_my);
                                ObjPass.Approved_on = Convert.ToString(Item.approved_on);
                                ObjPass.Cr_ee_share = Convert.ToString(Item.cr_ee_share);
                                ObjPass.Cr_er_share = Convert.ToString(Item.cr_er_share);
                                ObjPass.Cr_pen_bal = Convert.ToString(Item.cr_pen_bal);
                                ObjPass.Db_cr_flag = Convert.ToString(Item.db_cr_flag);
                                ObjPass.Particular = Convert.ToString(Item.particular);
                                ObjPass.Month_year = Convert.ToString(Item.month_year);
                                ObjPass.Tr_approved = Convert.ToString(Item.tr_approved);
                                int PassID = InsertPerfiosUan_est_details_Passbook(ObjPass);
                            }

                        }
                        foreach (var Items in myDeserializedClass.result.pdf)
                        {
                            ObjPdf.CompanyRefNo = CompanyRefNo;
                            string FileName = Convert.ToString(Items.est_id) + "_" + DateTime.Now.ToString("ddMMhhyymmss") + ".pdf";
                            ObjPdf.APIAutoID = APIAutoID;
                            ObjPdf.Est_id = Convert.ToString(Items.est_id);
                            ObjPdf.FileName = FileName;
                            ObjPdf.FilePath = FilePath;
                            ObjPdf.Remarks = "Uan API";
                            Convertbase64(Items.pdf_data, FileName, FilePath);
                            int PdfID = InsertPerfiosUan_Pdf_Details(ObjPdf);

                        }
                    }
                    return myDeserializedClass;
                }
                else
                {
                    StrBuilderObj.Append("----------------------------API GetOtpPerfiosAPI-----------------");
                    StrBuilderObj.Append(Environment.NewLine);
                    StrBuilderObj.Append("Response:" + APIResponse.RequestMessage.ToString());
                    API.WriteLog(StrBuilderObj.ToString(), "", "", "PerfiosAPI", "GetOtpPerfiosAPI", CompanyRefNo + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                StrBuilderObj.Append("----------------------------API GetOtpPerfiosAPI-----------------");
                StrBuilderObj.Append(Environment.NewLine);
                StrBuilderObj.Append("Response:Exception" + msg);
                API.WriteLog(StrBuilderObj.ToString(), "", "", "PerfiosAPI", "GetOtpPerfiosAPI", CompanyRefNo + "-Exception-" + System.DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));

            }
            return myDeserializedClass;
        }
        public int InsertPerfiosUanDetails(ModelPerfiosUanDetails Objda)
        {
            int intReturnValue = 0;
            string SP_PARAM_RETURN_VALUE = null;
            DataTable dt = new DataTable();
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertPerfiosUanDetails",
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, Objda.CompanyRefNo, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, Objda.PersonalRowID, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@request_id", SqlDbType.VarChar, Objda.request_id, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@Member_name", SqlDbType.VarChar, Objda.Member_name, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@Father_name", SqlDbType.VarChar, Objda.Father_name, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@Dob", SqlDbType.VarChar, Objda.Dob, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Pension_balance", SqlDbType.VarChar, Objda.Pension_balance, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Current_pf_balance", SqlDbType.VarChar, Objda.Current_pf_balance, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employee_Share_Total_Debit", SqlDbType.Int, Objda.Employee_Share_Total_Debit, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employee_Share_Total_credit", SqlDbType.VarChar, Objda.Employee_Share_Total_credit, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employee_Share_Total_balance", SqlDbType.VarChar, Objda.Employee_Share_Total_balance, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employer_Share_Total_Debit", SqlDbType.VarChar, Objda.Employer_Share_Total_Debit, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employer_Share_Total_credit", SqlDbType.Int, Objda.Employer_Share_Total_credit, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employer_Share_Total_balance", SqlDbType.VarChar, Objda.Employer_Share_Total_balance, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@CreatedBy", SqlDbType.VarChar, Objda.CreatedBy, ParameterDirection.Input, 20, false),
                   objReturnValueParam
                   );
            return intReturnValue = Convert.ToInt32(objReturnValueParam.Value);

        }
        public int InsertPerfiosUan_est_details(ModelPerfiosUan_est_details Objda)
        {
            int intReturnValue = 0;
            string SP_PARAM_RETURN_VALUE = null;
            DataTable dt = new DataTable();
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertPerfiosUan_est_details",
            DAS.CreateParameter("@APIAutoID", SqlDbType.Int, Objda.APIAutoID, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@Est_name", SqlDbType.VarChar, Objda.Est_name, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@Member_id", SqlDbType.VarChar, Objda.Member_id, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@Office", SqlDbType.VarChar, Objda.Office, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@Doj_epf", SqlDbType.VarChar, Objda.Doj_epf, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@Doe_epf", SqlDbType.VarChar, Objda.Doe_epf, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Doe_eps", SqlDbType.VarChar, Objda.Doe_eps, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Pf_Balance_net_balance", SqlDbType.VarChar, Objda.Pf_Balance_net_balance, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Pf_Balance_is_pf_full_withdrawn", SqlDbType.Int, Convert.ToBoolean(Objda.Pf_Balance_is_pf_full_withdrawn), ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Pf_Balance_is_pf_partial_withdrawn", SqlDbType.Int, Convert.ToBoolean(Objda.Pf_Balance_is_pf_partial_withdrawn), ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employee_Share_debit", SqlDbType.VarChar, Objda.Employee_Share_Total_Debit, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employee_Share_credit", SqlDbType.VarChar, Objda.Employee_Share_Total_credit, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employee_Share_balance", SqlDbType.VarChar, Objda.Employee_Share_Total_balance, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employer_Share_debit", SqlDbType.VarChar, Objda.Employer_Share_Total_Debit, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employer_Share_credit", SqlDbType.VarChar, Objda.Employer_Share_Total_credit, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Employer_Share_balance", SqlDbType.VarChar, Objda.Employer_Share_Total_balance, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@CreatedBy", SqlDbType.VarChar, Objda.CreatedBy, ParameterDirection.Input, 20, false),
                   objReturnValueParam
                   );
            return intReturnValue = Convert.ToInt32(objReturnValueParam.Value);

        }
        public int InsertPerfiosUan_est_details_Passbook(ModelPerfiosUan_est_details_Passbook Objda)
        {
            int intReturnValue = 0;
            string SP_PARAM_RETURN_VALUE = null;
            DataTable dt = new DataTable();
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertPerfiosUan_est_details_Passbook",
            DAS.CreateParameter("@APIAutoID", SqlDbType.Int, Objda.APIAutoID, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@Tr_date_my", SqlDbType.VarChar, Objda.Tr_date_my, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@Approved_on", SqlDbType.VarChar, Objda.Approved_on, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@Cr_ee_share", SqlDbType.VarChar, Objda.Cr_ee_share, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@Cr_er_share", SqlDbType.VarChar, Objda.Cr_er_share, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Cr_pen_bal", SqlDbType.VarChar, Objda.Cr_pen_bal, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Db_cr_flag", SqlDbType.VarChar, Objda.Db_cr_flag, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Particular", SqlDbType.VarChar, Objda.Particular, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Month_year", SqlDbType.VarChar, Objda.Month_year, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Tr_approved", SqlDbType.VarChar, Objda.Tr_approved, ParameterDirection.Input, 20, false),
                   objReturnValueParam
                   );
            return intReturnValue = Convert.ToInt32(objReturnValueParam.Value);

        }
        public int InsertPerfiosUan_Pdf_Details(ModelPerfiosUan_Pdf_Details Objda)
        {
            int intReturnValue = 0;
            string SP_PARAM_RETURN_VALUE = null;
            DataTable dt = new DataTable();
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertPerfiosUan_Pdf_Details",
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, Objda.CompanyRefNo, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@APIAutoID", SqlDbType.Int, Objda.APIAutoID, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@Est_id", SqlDbType.VarChar, Objda.Est_id, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@FileName", SqlDbType.VarChar, Objda.FileName, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@FilePath", SqlDbType.VarChar, Objda.FilePath, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Remarks", SqlDbType.VarChar, Objda.Remarks, ParameterDirection.Input, 20, false),
                   objReturnValueParam
                   );
            return intReturnValue = Convert.ToInt32(objReturnValueParam.Value);

        }
        public static string Convertbase64(string base64, string fileName, string filePath)
        {
            string resp = "";
            try
            {
                filePath = filePath + fileName;
                File.WriteAllBytes(filePath, Convert.FromBase64String(base64));
                resp = "Ok";
            }
            catch (Exception ex)
            {
                resp = ex.Message.ToString();
            }
            return resp;
        }
        public int InsertPerfiosUanUserDetails(string CompanyRefNo, string NSSOauthorizeURL, int LinksendBy)
        {
            int intReturnValue = 0;
            string SP_PARAM_RETURN_VALUE = null;
            DataTable dt = new DataTable();
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertPerfiosUanUserDetails",
                 DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
                     DAS.CreateParameter("@NSSOauthorizeURL", SqlDbType.VarChar, NSSOauthorizeURL, ParameterDirection.Input, 2000, false),
                        DAS.CreateParameter("@LinksendBy", SqlDbType.Int, LinksendBy, ParameterDirection.Input, 10, false),

                        objReturnValueParam
                        );
            return intReturnValue = Convert.ToInt32(objReturnValueParam.Value);

        }
        public DataTable Sp_GetListPerfiosUANUserDetails()
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetListPerfiosUanUserDetails"
               );
            return dt;
        }
        public DataTable GetPerfiosUANUserDetail(string CompanyRefNo, int CandidateStatus)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_PerfiosUanUserDetails",
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@CandidateStatus", SqlDbType.Int, CandidateStatus, ParameterDirection.Input, 100, false)

            );
            return dt;
        }
        public DataTable Sp_GetPerfiosUANUserDetails(string CompanyRefNo)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetPerfiosUanUserDetails",
                    DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false)

               );
            return dt;
        }
        public DataTable Get_PerfiosUANDocumentDetails(int PersonalRowId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Get_PerfiosUanDocumentDetails",
                    DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 100, false)

               );
            return dt;
        }
        #endregion

        #region Domino's
        public DataTable GetDominoDocumentDetails(string Mobile_No, string DominosURL, int CandidateStatus, string Type)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_DominosUserDetails",
                    DAS.CreateParameter("@Mobile_No", SqlDbType.VarChar, Mobile_No, ParameterDirection.Input, 12, false),
                     DAS.CreateParameter("@DominosURL", SqlDbType.VarChar, DominosURL, ParameterDirection.Input, 2000, false),
                       DAS.CreateParameter("@CandidateStatus", SqlDbType.Int, CandidateStatus, ParameterDirection.Input, 30, false),
                        DAS.CreateParameter("@Type", SqlDbType.VarChar, Type, ParameterDirection.Input, 30, false)

               );
            return dt;
        }
        public int InsertDominoUserDetails(string Mobile_No, string DominosURL, int CandidateStatus)
        {
            int intReturnValue = 0;
            string SP_PARAM_RETURN_VALUE = null;
            DataTable dt = new DataTable();
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_DominosUserDetails",
                 DAS.CreateParameter("@Mobile_No", SqlDbType.VarChar, Mobile_No, ParameterDirection.Input, 12, false),
                     DAS.CreateParameter("@DominosURL", SqlDbType.VarChar, DominosURL, ParameterDirection.Input, 2000, false),
                       DAS.CreateParameter("@CandidateStatus", SqlDbType.Int, CandidateStatus, ParameterDirection.Input, 30, false),
                        DAS.CreateParameter("@Type", SqlDbType.VarChar, "Insert", ParameterDirection.Input, 30, false),

                        objReturnValueParam
                        );
            return intReturnValue = Convert.ToInt32(objReturnValueParam.Value);

        }

        #endregion
        #region Mail ch
        public DataTable GetBounceDetails(int TeamMemberRoWID)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetBounceDetails",
            DAS.CreateParameter("@TeamMemberRoWID", SqlDbType.Int, @TeamMemberRoWID, ParameterDirection.Input, 100, false)
            );
            return dt;
        }
        #endregion

    }
    #region Digilocker API models
    public class DocumentAPI
    {
        public string FileName { get; set; }
        public string IssuerId { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }

    public class tokenobject
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string consent_valid_till { get; set; }
        public string digilockerid { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string eaadhaar { get; set; }
        public string reference_key { get; set; }
        public string mobile { get; set; }
        public string refresh_token { get; set; }
        public string new_account { get; set; }
    }


    public class Tokenobject
    {
        public string grant_type { get; set; }
        public string code { get; set; }
        public string redirect_uri { get; set; }
        public string code_verifier { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }

    public class IssueDocListobject
    {
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string type { get; set; }
        public string size { get; set; }
        public string date { get; set; }
        public string parent { get; set; }
        public string mime { get; set; }
        public string uri { get; set; }
        public string doctype { get; set; }
        public string description { get; set; }
        public string issuerid { get; set; }
        public string issuer { get; set; }
    }
    #endregion
    #region Perfios API models
    public class GetOtpmodel
    {
        public Result result { get; set; }
        public string request_id { get; set; }
        public string statuscode { get; set; }
    }

    public class Result
    {
        public string message { get; set; }
    }


    public class GetUanDetails
    {
        public ResultDetails result { get; set; }
        public string request_id { get; set; }
        public string statuscode { get; set; }
    }

    public class ResultDetails
    {
        public Employee_Details employee_details { get; set; }
        public List<Est_Details> est_details { get; set; }
        public Overall_Pf_Balance overall_pf_balance { get; set; }
        public List<Pdf> pdf { get; set; }
    }

    public class Employee_Details
    {
        public string member_name { get; set; }
        public string father_name { get; set; }
        public string dob { get; set; }
    }

    public class Overall_Pf_Balance
    {
        public int pension_balance { get; set; }
        public int current_pf_balance { get; set; }
        public Employee_Share_Total employee_share_total { get; set; }
        public Employer_Share_Total employer_share_total { get; set; }
    }

    public class Employee_Share_Total
    {
        public int debit { get; set; }
        public int credit { get; set; }
        public int balance { get; set; }
    }

    public class Employer_Share_Total
    {
        public int debit { get; set; }
        public int credit { get; set; }
        public int balance { get; set; }
    }

    public class Est_Details
    {
        public string est_name { get; set; }
        public string member_id { get; set; }
        public string office { get; set; }
        public string doj_epf { get; set; }
        public string doe_epf { get; set; }
        public string doe_eps { get; set; }
        public Pf_Balance pf_balance { get; set; }
        public Passbook[] passbook { get; set; }
    }

    public class Pf_Balance
    {
        public int net_balance { get; set; }
        public bool is_pf_full_withdrawn { get; set; }
        public bool is_pf_partial_withdrawn { get; set; }
        public Employee_Share employee_share { get; set; }
        public Employer_Share employer_share { get; set; }
    }

    public class Employee_Share
    {
        public int debit { get; set; }
        public int credit { get; set; }
        public int balance { get; set; }
    }

    public class Employer_Share
    {
        public int debit { get; set; }
        public int credit { get; set; }
        public int balance { get; set; }
    }

    public class Passbook
    {
        public string tr_date_my { get; set; }
        public string approved_on { get; set; }
        public string cr_ee_share { get; set; }
        public string cr_er_share { get; set; }
        public string cr_pen_bal { get; set; }
        public string db_cr_flag { get; set; }
        public string particular { get; set; }
        public string month_year { get; set; }
        public string tr_approved { get; set; }
    }

    public class Pdf
    {
        public string est_id { get; set; }
        public string pdf_data { get; set; }
    }

    public class ModelPerfiosUanDetails
    {
        public string request_id { get; set; }
        public string CompanyRefNo { get; set; }
        public int PersonalRowID { get; set; }
        public string Member_name { get; set; }
        public string Father_name { get; set; }
        public string Dob { get; set; }
        public string Pension_balance { get; set; }
        public string Current_pf_balance { get; set; }
        public string Employee_Share_Total_Debit { get; set; }
        public string Employee_Share_Total_credit { get; set; }
        public string Employee_Share_Total_balance { get; set; }
        public string Employer_Share_Total_Debit { get; set; }
        public string Employer_Share_Total_credit { get; set; }
        public string Employer_Share_Total_balance { get; set; }
        public string CreatedBy { get; set; }
    }
    public class ModelPerfiosUan_est_details
    {
        public int APIAutoID { get; set; }
        public string Est_name { get; set; }
        public string Member_id { get; set; }
        public string Office { get; set; }
        public string Doj_epf { get; set; }
        public string Doe_epf { get; set; }
        public string Doe_eps { get; set; }
        public string Pf_Balance_net_balance { get; set; }
        public string Pf_Balance_is_pf_full_withdrawn { get; set; }
        public string Pf_Balance_is_pf_partial_withdrawn { get; set; }
        public string Employee_Share_Total_Debit { get; set; }
        public string Employee_Share_Total_credit { get; set; }
        public string Employee_Share_Total_balance { get; set; }
        public string Employer_Share_Total_Debit { get; set; }
        public string Employer_Share_Total_credit { get; set; }
        public string Employer_Share_Total_balance { get; set; }
        public string CreatedBy { get; set; }
    }
    public class ModelPerfiosUan_est_details_Passbook
    {
        public int APIAutoID { get; set; }
        public string Tr_date_my { get; set; }
        public string Approved_on { get; set; }
        public string Cr_ee_share { get; set; }
        public string Cr_er_share { get; set; }
        public string Cr_pen_bal { get; set; }
        public string Db_cr_flag { get; set; }
        public string Particular { get; set; }
        public string Month_year { get; set; }
        public string Tr_approved { get; set; }
    }
    public class ModelPerfiosUan_Pdf_Details
    {
        public string CompanyRefNo { get; set; }
        public int APIAutoID { get; set; }
        public string Est_id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Remarks { get; set; }
    }
    #endregion


}
