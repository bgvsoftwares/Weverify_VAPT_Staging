using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DigiTap;
using ViewModels.llmtech;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.refinitiv;
using ViewModels.VendorAPI;

namespace BAL
{
    public class DataBaseConnections
    {
        public DataTable Sp_GetClientWIPCheckDetails(int ClientRowId)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetClientWIPCheckDetails",
            DAS.CreateParameter("@ClientRowId", SqlDbType.Int, ClientRowId, ParameterDirection.Input, 10, false));
            return ds;
        }
        public DataTable GetIndentityDataFromtblDataEntry(int PersonalRowId,int SubCheckRowId)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetIndentityDataFromtblDataEntry",
            DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubcheckRowID", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false));
            return ds;
        }         
        public DataTable Sp_GetFaildStatusIdCheck(int Days)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetFaildStatusIdCheck",
            DAS.CreateParameter("@Days", SqlDbType.Int, Days, ParameterDirection.Input, 10, false));
            return ds;
        }

        public DataTable GetDigitapUAN(int AutoId, int PersonalRowId, string CompanyRefNo, string Uan)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetDigitapUAN",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@UanNo", SqlDbType.VarChar, Uan, ParameterDirection.Input, 20, false)
            );
            return ds;
        }
        private const string SP_PARAM_RETURN_VALUE = "@ReturnValue";
        public int InsertUpdateDigitapUAN(DigiTapUANViewModel Model, int PersonalRowId, string CompanyRefNo, string UanNo, string JsonString)
        {

            List<string> uanList = Model.result.uan;
            BasicDetails basic_details = new BasicDetails();
            List<EmploymentHistory> employment_history = new List<EmploymentHistory>();
            additionaldetails additionaldetails = new additionaldetails();
            for (int i = 0; i < uanList.Count; i++)
            {
                string uanNo = uanList[0];
                if (Model.result.uan_details.ContainsKey(uanNo))
                {
                    basic_details = Model.result.uan_details[uanNo].basic_details;
                    employment_history = Model.result.uan_details[uanNo].employment_history;
                    additionaldetails = Model.result.uan_details[uanNo].additional_details;
                }
                break;
            }
            int is_employed = 0;
            if (Model.result.summary.is_employed == true)
            {
                is_employed = 1;
            }
            DataTable EmploymentHistory = new DataTable();
            EmploymentHistory.Columns.Add("establishment_name", typeof(string));
            EmploymentHistory.Columns.Add("establishment_id", typeof(string));
            EmploymentHistory.Columns.Add("member_id", typeof(string));
            EmploymentHistory.Columns.Add("date_of_exit", typeof(string));
            EmploymentHistory.Columns.Add("date_of_joining", typeof(string));
            EmploymentHistory.Columns.Add("leave_reason", typeof(string));
            EmploymentHistory.Columns.Add("employment_period_in_months", typeof(string));
            EmploymentHistory.Columns.Add("employer_confidence_score", typeof(string));

            foreach (var item in employment_history)
            {
                EmploymentHistory.Rows.Add(item.establishment_name, item.establishment_id, item.member_id, item.date_of_exit, item.date_of_joining, item.leave_reason, Convert.ToString(item.employment_period_in_months), item.employer_confidence_score);
            }
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertUpdateDigiTapUAN",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@client_ref_num", SqlDbType.VarChar, Model.client_ref_num, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@request_id", SqlDbType.VarChar, Model.request_id, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@uan", SqlDbType.VarChar, UanNo, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@is_employed", SqlDbType.Int, is_employed, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@gender", SqlDbType.VarChar, basic_details.gender, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@date_of_birth", SqlDbType.VarChar, basic_details.date_of_birth, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@name", SqlDbType.VarChar, basic_details.name, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@mobile", SqlDbType.VarChar, basic_details.mobile, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@aadhaar_verification_status", SqlDbType.VarChar, basic_details.mobile, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@relation", SqlDbType.VarChar, additionaldetails.relation, ParameterDirection.Input, 15, false),
            DAS.CreateParameter("@relative_name", SqlDbType.VarChar, additionaldetails.relative_name, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@JsonString", SqlDbType.NVarChar, JsonString, ParameterDirection.Input, int.MaxValue, false),
            DAS.CreateParameter("@EmploymentHistory", SqlDbType.Structured, EmploymentHistory, ParameterDirection.Input, 30, false),
            objReturnValueParam);
            intReturnValue = (int)objReturnValueParam.Value;

            return intReturnValue;

        }
       
        public DataTable GetGetDigitapUANList(string CompanyRefNo, string Uan)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetGetDigitapUANList",
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@UanNo", SqlDbType.VarChar, Uan, ParameterDirection.Input, 20, false)
            );
            return ds;
        }
       public bool IsPackageExistsInCase(int PersonalRowId ,int ClientRowId,string ClientPackageName)
        {
            bool result = false;
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "IsPackageExistsInCase",
                DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@ClientRowId", SqlDbType.Int, ClientRowId, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@ClientPackageName", SqlDbType.VarChar, ClientPackageName, ParameterDirection.Input, 200, false)
                );
                if(dt.Rows.Count>0)
                {
                    result = true;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        public DataTable SP_GetClientAPIDetails(int ClientRowId, string AuthType, string APIType)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionStringAPI, CommandType.StoredProcedure, "SP_GetClientAPIDetails",
            DAS.CreateParameter("@ClientRowID", SqlDbType.Int, ClientRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@AuthorizationType", SqlDbType.VarChar, AuthType, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@APIType", SqlDbType.VarChar, APIType, ParameterDirection.Input, 100, false)
            );
            return ds;
        }
        public DataTable GetPQPersonalWithMobileNo(int PersonalRowId, string MobileNo)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetPQPersonalWithMobileNo",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@MobileNo", SqlDbType.VarChar, MobileNo, ParameterDirection.Input, 20, false)
            );
            return ds;
        }
        public DataTable GetPQPersonalWithEmailId(int PersonalRowId, string EmailID)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetPQPersonalWithEmailID",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@EmailID", SqlDbType.VarChar, EmailID, ParameterDirection.Input, 100, false)
            );
            return ds;
        }
        public int InsertUpdateLivenessApiResponse(int personalRowId, int subCheckRowId, string jsonString, string VideoFileName, string submitResponse = "", string CheckStatus = "",string AdharNo="")
        {
            int autoId = 0;

            SqlConnection conn = new SqlConnection(DAS.ConnectionString);
            SqlCommand cmd = new SqlCommand("insertUpdateLivenessApiResponse", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PersonalRowId", personalRowId);
            cmd.Parameters.AddWithValue("@SubCheckRowId", subCheckRowId);
            cmd.Parameters.AddWithValue("@JsonString", jsonString);
            cmd.Parameters.AddWithValue("@VideofileName", VideoFileName);
            cmd.Parameters.AddWithValue("@submitJson", submitResponse);
            cmd.Parameters.AddWithValue("@CheckStatus", CheckStatus);
            cmd.Parameters.AddWithValue("@AdharNumber", AdharNo);


            // Return value parameter
            SqlParameter returnParameter = new SqlParameter();
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnParameter);

            try
            {
                conn.Open();              // 🔓 OPEN connection
                cmd.ExecuteNonQuery();
                autoId = (int)returnParameter.Value;
            }
            finally
            {
                conn.Close();             // 🔒 CLOSE connection
                conn.Dispose();
                cmd.Dispose();
            }
            return autoId;
        }
        public DataTable fractalLivenessCase()
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "fractalLivenessCase"
            );
            return ds;
        }
        public DataTable getDatabaseChecksIsIndividual(int SubCheckRowId)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "getDatabaseChecksIsIndividual",
            DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false)
            );
            return ds;
        }
        public DataTable GetViotalUANVerification(int AutoId, int PersonalRowId, string CompanyRefNo, string UAN,int SubCheckRowId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetViotalUANVerification",
                DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
                 DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@UanNo", SqlDbType.VarChar, UAN, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@SubCheckRowID", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 100, false)
                );
            return dt;
        }
        public int InsertUpdateViotalUANVerification(ViotalUANRoot Obj, string CompanyRefNo, int PersonalRowId, string UANNo,int SubCheckRowId)
        {
            DataTable EmploymentHistory = new DataTable();
            EmploymentHistory.Columns.Add("name", typeof(string));
            EmploymentHistory.Columns.Add("guardian_name", typeof(string));
            EmploymentHistory.Columns.Add("establishment_name", typeof(string));
            EmploymentHistory.Columns.Add("member_id", typeof(string));
            EmploymentHistory.Columns.Add("date_of_joining", typeof(string));
            EmploymentHistory.Columns.Add("date_of_exit", typeof(string));
            var employmenthistory = Obj.data.result;                     
            foreach (var item in employmenthistory)
            {
                EmploymentHistory.Rows.Add(item.name, item.fatherOrHusbandName, item.establishmentName, item.memberId, item.dateOfJoining, item.dateOfExit);
            }
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertUpdateViotalUANVerification",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowID", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@UANNo", SqlDbType.VarChar, UANNo, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@request_id", SqlDbType.VarChar, "", ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@transaction_id", SqlDbType.VarChar, "", ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@ViotalEmploymentHistory", SqlDbType.Structured, EmploymentHistory, ParameterDirection.Input, 30, false),
            objReturnValueParam);
            intReturnValue = (int)objReturnValueParam.Value;

            return intReturnValue;
        }
        public int UpdateCandidateClientRefNo(int PersonalRowId, string ClientRefNo)
        {
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "UpdateCandidateClientRefNo",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),          
            DAS.CreateParameter("@ClientRefID", SqlDbType.VarChar, ClientRefNo, ParameterDirection.Input, 100, false),
            objReturnValueParam);
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;
        }
        public DataTable GetPersonalDetailsForPersistantClient(int PersonalRowId)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetPersonalDetailsForPersistantClient",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false)
            );
            return ds;
        }
        public int UpdatelivenessChecktbldataEntryData(int PersonalRowId, int SubCheckRowId,string Status)
        {
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "UpdatelivenessChecktbldataEntryData",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowID", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@Status", SqlDbType.VarChar, Status, ParameterDirection.Input, 100, false),
            objReturnValueParam);
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;
        }
        public string GetUploadArtifact(int PersonalRowId,int SubCheckRowId)
        {
            string fileName="";
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetUploadArtifact",
            DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowID", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false)
            );
            if (ds.Rows.Count > 0)
            {
                fileName = Convert.ToString(ds.Rows[0]["FileName"]);
            }
            return fileName;
               
        }
        public int InsertUpdatellmtechUANDetails(int PersonalRowId, string UANNO, string CompanyRefNo, int SubCheckRowId, string JsonString, int BatchNo)
        {
            int a = 0;
            llmtechUANRoot model = JsonConvert.DeserializeObject<llmtechUANRoot>(JsonString);
            
            DataTable EmploymentHistory = new DataTable();
            EmploymentHistory.Columns.Add("memberId", typeof(string));
            EmploymentHistory.Columns.Add("name", typeof(string));
            EmploymentHistory.Columns.Add("fatherOrHusbandName", typeof(string));
            EmploymentHistory.Columns.Add("establishmentId", typeof(string));
            EmploymentHistory.Columns.Add("establishmentName", typeof(string));
            EmploymentHistory.Columns.Add("dateOfJoining", typeof(string));
            EmploymentHistory.Columns.Add("dateOfExit", typeof(string));
            EmploymentHistory.Columns.Add("tenureOfEmployment", typeof(string));
            if (model.success)
            {
                var employmenthistory = model.data.result;
                foreach (var item in employmenthistory)
                {
                    EmploymentHistory.Rows.Add(item.memberId, item.name, item.fatherOrHusbandName, item.establishmentId, item.establishmentName,item.dateOfJoining, item.dateOfExit,item.tenureOfEmployment);
                }
            }
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_AddllmtechUANDetails",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
           DAS.CreateParameter("@CheckUniqueId", SqlDbType.VarChar, "", ParameterDirection.Input, 100, false),
           DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@UANNO", SqlDbType.VarChar, UANNO, ParameterDirection.Input, 20, false),
           DAS.CreateParameter("@JsonString", SqlDbType.NVarChar, JsonString, ParameterDirection.Input, int.MaxValue, false),
           DAS.CreateParameter("@BatchNo", SqlDbType.Int, BatchNo, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@CreatedBy", SqlDbType.Int, 0, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@llmtechEmploymentHistory", SqlDbType.Structured, EmploymentHistory, ParameterDirection.Input, 100, false)
           );
            return a;
        }
        public DataTable GetllmtechUANDetails(int AutoId, int PersonalRowId, string CompanyRefNo, string UANNO, string DateFrom, string DateTo, int BatchNo,int SubCheckRowId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetllmtechUANDetails",
           DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
           DAS.CreateParameter("@UANNO", SqlDbType.VarChar, UANNO, ParameterDirection.Input, 20, false),
           DAS.CreateParameter("@DateFrom", SqlDbType.VarChar, DateFrom, ParameterDirection.Input, 50, false),
           DAS.CreateParameter("@DateTo", SqlDbType.VarChar, DateTo, ParameterDirection.Input, 50, false)
           );
            return dt;
        }

        public int AddUANVendorName(int PersonalRowId, int SubCheckRowID, string VendorName, int Status)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "SP_insertUANVendorName",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowID, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@VendorName", SqlDbType.VarChar, VendorName, ParameterDirection.Input, 100, false),
           DAS.CreateParameter("@Status", SqlDbType.Int, Status, ParameterDirection.Input, 10, false)
           );
            return a;
        }

        public string GetUANVendorName()
        {
            string fileName = "";
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetUANVendorName");
            if (ds.Rows.Count > 0)
            {
                fileName = Convert.ToString(ds.Rows[0]["VendorName"]);
            }
            else
            {
                fileName = "DigiTap";
            }
            return fileName;

        }
        public DataTable GetCandidateBGCForm(int Pid)
        {          
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetCandidateBGCForm",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, Pid, ParameterDirection.Input, 10, false));           
            return ds;
        }
        public DataTable GetUANCheckToBGVFormCheckStatus(int Pid,int Sid)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetUANCheckToBGVFormCheckStatus",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, Pid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, Sid, ParameterDirection.Input, 10, false)
            );
            return ds;
        }
        public int AddUANCheckToBGVFormCheckStatus(int PersonalRowId, int SubCheckRowID, string VerifiedFieldName, string Status)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "AddUANCheckToBGVFormCheckStatus",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowID, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@VerifiedFieldName", SqlDbType.VarChar, VerifiedFieldName, ParameterDirection.Input, 200, false),
           DAS.CreateParameter("@Status", SqlDbType.VarChar, Status, ParameterDirection.Input, 20, false)
           );
            return a;
        }
        public DataTable GetDigitapUANThroughMobilePAN(int AutoId, int PersonalRowId, int SubCheckRowId, string CompanyRefNo, string PanNo, string MobileNo, string DateFrom, string DateTo)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetDigitapUANThroughMobilePAN",
           DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
           DAS.CreateParameter("@MobileNo", SqlDbType.VarChar, MobileNo, ParameterDirection.Input, 15, false),
           DAS.CreateParameter("@PanNo", SqlDbType.VarChar, PanNo, ParameterDirection.Input, 15, false),
           DAS.CreateParameter("@DateFrom", SqlDbType.VarChar, DateFrom, ParameterDirection.Input, 20, false),
           DAS.CreateParameter("@DateTo", SqlDbType.VarChar, DateTo, ParameterDirection.Input, 20, false)
           );
            return dt;
        }
        public int InsertUpdateDigitapUANThroughMobilePAN(int PersonalRowId, int SubCheckRowId, string CompanyRefNo, string CheckUniqueId,string MobileNo,string PanNo,string JsonString,int CreatedBy)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "SP_InsertUpdateDigitapUANThroughMobilePAN",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
           DAS.CreateParameter("@CheckUniqueId", SqlDbType.VarChar, CheckUniqueId, ParameterDirection.Input, 100, false),
           DAS.CreateParameter("@MobileNo", SqlDbType.VarChar, MobileNo, ParameterDirection.Input, 15, false),
           DAS.CreateParameter("@PanNo", SqlDbType.VarChar, PanNo, ParameterDirection.Input, 15, false),
           DAS.CreateParameter("@JsonString", SqlDbType.NVarChar, JsonString, ParameterDirection.Input, int.MaxValue, false),
           DAS.CreateParameter("@CreatedBy", SqlDbType.Int, CreatedBy, ParameterDirection.Input, 10, false)
           );
            return a;
        }
        #region Wipro Get And store token
        public DataTable sp_GetTokenDetails(int ClientRowId)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "sp_GetTokenDetails",
            DAS.CreateParameter("@ClientRowId", SqlDbType.Int, ClientRowId, ParameterDirection.Input, 10, false)
            );
            return ds;
        }
        public int sp_SaveTokenDetails(int ClientRowId, string Token)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "sp_SaveTokenDetails",
              DAS.CreateParameter("@ClientRowId", SqlDbType.Int, ClientRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@Token", SqlDbType.VarChar, Token, ParameterDirection.Input, -1, false)
           );
            return a;
        }
        #endregion

        public int AddPQVerifiedUploadDoc(PQAddVerifiedUploadDocViewModel model)
        {
            try
            {

                if (model != null)
                {
                    int intReturnValue = 0;
                    string SP_PARAM_RETURN_VALUE = null;
                    SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
                    DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertUpdatePQVerifiedUploadDocs",
            DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, model.PersonalRowID, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@SubCheckRowID", SqlDbType.SmallInt, model.SubCheckRowID, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@DocumentUploadFrom", SqlDbType.NVarChar, model.DocumentUploadFrom, ParameterDirection.Input, 20, false),
             DAS.CreateParameter("@DocumentType", SqlDbType.NVarChar, model.DocumentType, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@FileName", SqlDbType.NVarChar, model.FileName, ParameterDirection.Input, 300, false),
            DAS.CreateParameter("@FilePath", SqlDbType.NVarChar, model.FilePath, ParameterDirection.Input, 600, false),
            DAS.CreateParameter("@Remarks", SqlDbType.NVarChar, model.Remarks, ParameterDirection.Input, 500, false),
            DAS.CreateParameter("@UploadDate", SqlDbType.DateTime, model.UploadDate, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@StatusDocxConvert", SqlDbType.TinyInt, model.StatusDocxConvert, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@StatusPDF2DOCXConvert", SqlDbType.TinyInt, model.StatusPDF2DOCXConvert, ParameterDirection.Input, 25, false),
          DAS.CreateParameter("@PDF_FileName", SqlDbType.NVarChar, model.PDF_FileName, ParameterDirection.Input, 300, false),
           DAS.CreateParameter("@Status", SqlDbType.TinyInt, model.Status, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@InsuffDoc", SqlDbType.TinyInt, model.InsuffDoc, ParameterDirection.Input, 25, false),
            objReturnValueParam
            );
                    intReturnValue = Convert.ToInt32(objReturnValueParam.Value);
                    return intReturnValue;
                }
                else
                {
                    throw new Exception("Invalid Client Details!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int InsertPQVerifiedUploadCaseDoc(AddPQVerifiedUploadCaseDocViewModel Model)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertBulkCaseDoc",
            DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, Model.PersonalRowID, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@DocumentUploadFrom", SqlDbType.NVarChar, Model.DocumentUploadFrom, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@DocumentType", SqlDbType.NVarChar, Model.DocumentType, ParameterDirection.Input, 20, false),
             DAS.CreateParameter("@FileName", SqlDbType.NVarChar, Model.FileName, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@FilePath", SqlDbType.NVarChar, Model.FilePath, ParameterDirection.Input, 300, false),
            DAS.CreateParameter("@Remarks", SqlDbType.NVarChar, Model.Remarks, ParameterDirection.Input, 600, false),
            DAS.CreateParameter("@UploadDate", SqlDbType.DateTime, Model.UploadDate, ParameterDirection.Input, 500, false),
            DAS.CreateParameter("@Status", SqlDbType.Int, Model.Status, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@PdfConvertedToImage", SqlDbType.Int, 0, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@ImageFileName", SqlDbType.NVarChar, "", ParameterDirection.Input, 25, false),
          DAS.CreateParameter("@RptArtiFacts", SqlDbType.NVarChar, 0, ParameterDirection.Input, 300, false),
           DAS.CreateParameter("@RptArtiFactsDate", SqlDbType.DateTime, null, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@RptDisplayOrder", SqlDbType.Int, 0, ParameterDirection.Input, 25, false)
           );
            return a;
        }
        public int GetClientMaster(int Cid)
        {
            int Status = 0;
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetClientMaster",
            DAS.CreateParameter("@ClientRowId", SqlDbType.Int, Cid, ParameterDirection.Input, 10, false));
            if(ds.Rows.Count>0)
            {
                Status = Convert.ToInt32(ds.Rows[0]["LivenessWithAadhar"]);
            }
            return Status;
        }
        public int AddPerfiosDigilockerDetails(int PersonalRowId, int SubCheckRowId, string JsonString,string RequestURL)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "AddPerfiosDigilockerDetails",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@JsonString", SqlDbType.NVarChar, JsonString, ParameterDirection.Input, int.MaxValue, false),
           DAS.CreateParameter("@RequestURL", SqlDbType.NVarChar, RequestURL, ParameterDirection.Input, int.MaxValue, false)
           );
            return a;
        }
        public DataTable GetTempPQVerifiedUploadDocs( int PersonalRowId, int SubCheckRowId, string Search, int pageNo, int pageSize, string sort)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetTempPQVerifiedUploadDocs",
           DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@Search", SqlDbType.VarChar, Search, ParameterDirection.Input, 500, false),
           DAS.CreateParameter("@pageNo", SqlDbType.Int, pageNo, ParameterDirection.Input, 15, false),
           DAS.CreateParameter("@pageSize", SqlDbType.Int, pageSize, ParameterDirection.Input, 15, false),
           DAS.CreateParameter("@sort", SqlDbType.VarChar, sort, ParameterDirection.Input, 100, false),
           DAS.CreateParameter("@sortDir", SqlDbType.VarChar, sort, ParameterDirection.Input, 100, false)
           );
            return dt;
        }
        public int AddLivenessAPIlogs(int PersonalRowId, int SubCheckRowId, string JsonString, string RequestURL)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_InsertLivenessAPIlogs",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@JsonString", SqlDbType.NVarChar, JsonString, ParameterDirection.Input, int.MaxValue, false),
           DAS.CreateParameter("@APIURL", SqlDbType.NVarChar, RequestURL, ParameterDirection.Input, 500, false)
           );
            return a;
        }
        public DataTable CheckInfoFromDataEntry(int PersonalRowId, int SubCheckRowId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "CheckInfoFromDataEntry",
                DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false));
            return dt;
        }
        public DataTable GetCIBILChecklist(int SubCheckRowId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetCIBILChecklist",               
                DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false));
            return dt;
        } 
        public DataTable GetClientListFormCIBILCheck(int ClientRowId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetClientListFormCIBILCheck",               
                DAS.CreateParameter("@ClientRowId", SqlDbType.Int, ClientRowId, ParameterDirection.Input, 10, false));
            return dt;
        }

        public DataTable GetSepcialCheckDataFromtblDataEntry(int PersonalRowid , int SubCheckRowId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetSepcialCheckDataFromtblDataEntry",
                DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, PersonalRowid, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@SubcheckRowID", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false));
            return dt;
        }
        public int AddCIBILDetails(int PersonalRowId, int SubCheckRowId, string CompanyRefNo, string PAN, string Name, string MobileNo, int StatusCode, string FileName,string credit_score)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "AddCIBILDetails",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@PAN", SqlDbType.VarChar, PAN, ParameterDirection.Input, 20, false),
              DAS.CreateParameter("@Name", SqlDbType.VarChar, Name, ParameterDirection.Input, 200, false),
              DAS.CreateParameter("@MobileNo", SqlDbType.VarChar, MobileNo, ParameterDirection.Input, 50, false),
              DAS.CreateParameter("@StatusCode", SqlDbType.Int, StatusCode, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@FileName", SqlDbType.VarChar, FileName, ParameterDirection.Input, 200, false),
              DAS.CreateParameter("@credit_score", SqlDbType.VarChar, credit_score, ParameterDirection.Input, 100, false)
           );
            return a;
        }
        public DataTable GetEmpCheckListfromPQCandidateChecks(int PersonalRowid, int ClientRowId,int CheckFamilyRowID)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetEmpCheckListfromPQCandidateChecks",
                DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowid, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@ClientRowId", SqlDbType.Int, ClientRowId, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@CheckFamilyRowID", SqlDbType.Int, CheckFamilyRowID, ParameterDirection.Input, 10, false));
            return dt;
        }

        public int AddLlmtechPanDetails(int PersonalRowId, int SubCheckRowId, string CompanyRefNo, string PAN, string JsonString, int StatusCode)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "AddLlmtechPanDetails",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
              DAS.CreateParameter("@PanNo", SqlDbType.VarChar, PAN, ParameterDirection.Input, 20, false),
              DAS.CreateParameter("@JsonString", SqlDbType.NVarChar, JsonString, ParameterDirection.Input, int.MaxValue, false),
              DAS.CreateParameter("@StatusCode", SqlDbType.Int, StatusCode, ParameterDirection.Input, 10, false)
              );
            return a;
        }
        public DataTable GetLlmtechPanDetails(int AutoId, int Pid, int Sid, string CompanyRefNo, string PAN, string DateFrom, string DateTo)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetLlmtechPanDetails",
            DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, Pid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, Sid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@PanNo", SqlDbType.VarChar, PAN, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@DateFrom", SqlDbType.VarChar, DateFrom, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@DateTo", SqlDbType.VarChar, DateTo, ParameterDirection.Input, 50, false)
           );
            return dt;
        }
        public DataTable GetDInChecksList(int SubCheckRowId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetDInChecksList",
                DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false));
            return dt;
        }
        public int InsertUpdateDigiTapPAN_To_DIN(int PersonalRowId, string PanNo, string CompanyRefNo, string JsonString, int BatchNo)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "SP_InsertUpdateDigiTapPAN_To_DIN",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
           DAS.CreateParameter("@PanNo", SqlDbType.VarChar, PanNo, ParameterDirection.Input, 20, false),
           DAS.CreateParameter("@JsonString", SqlDbType.VarChar, JsonString, ParameterDirection.Input, int.MaxValue, false),
           DAS.CreateParameter("@BatchNo", SqlDbType.Int, BatchNo, ParameterDirection.Input, 10, false)
           );
            return a;
        }
        public int InsertUpdateDINCheckDetails(int PersonalRowId, int SubcheckRowId, int AntecedentRowID, string DIN)
        {
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "UpdateDINCheckDetails",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubcheckRowId", SqlDbType.Int, SubcheckRowId, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@AntecedentRowID", SqlDbType.Int, AntecedentRowID, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@DIN", SqlDbType.VarChar, DIN, ParameterDirection.Input, 20, false),
            objReturnValueParam);
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;
        }
        public DataTable GetDigiTapPAN_To_DIN(int AutoId, int PersonalRowId, string CompanyRefNo, string PanNo, string DateFrom, string DateTo, int BatchNo)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_GetDigiTapPAN_To_DIN",
           DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
           DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
           DAS.CreateParameter("@PanNo", SqlDbType.VarChar, PanNo, ParameterDirection.Input, 20, false),
           DAS.CreateParameter("@Datefrom", SqlDbType.VarChar, DateFrom, ParameterDirection.Input, 50, false),
           DAS.CreateParameter("@DateTo", SqlDbType.VarChar, DateTo, ParameterDirection.Input, 50, false)
           );
            return dt;
        }
        public DataTable GetDigiTapPanToGst(int AutoId, int Pid, int Sid, string CompanyRefNo, string PAN, string DateFrom, string DateTo)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetDigiTapPanToGst",
            DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, Pid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, Sid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@PanNo", SqlDbType.VarChar, PAN, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@DateFrom", SqlDbType.VarChar, DateFrom, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@DateTo", SqlDbType.VarChar, DateTo, ParameterDirection.Input, 50, false)
           );
            return dt;
        }
        public int AddDigiTapPanToGst(int PersonalRowId, int SubCheckRowId, string CompanyRefNo, string PAN, string JsonString, int StatusCode)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "AddDigiTapPanToGst",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
              DAS.CreateParameter("@PanNo", SqlDbType.VarChar, PAN, ParameterDirection.Input, 20, false),
              DAS.CreateParameter("@JsonString", SqlDbType.NVarChar, JsonString, ParameterDirection.Input, int.MaxValue, false),
              DAS.CreateParameter("@StatusCode", SqlDbType.Int, StatusCode, ParameterDirection.Input, 10, false)
              );
            return a;
        }
        public int InsertUpdateGSTCheckDetails(int PersonalRowId, int SubcheckRowId, int AntecedentRowID, string GST)
        {
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "UpdateGSTCheckDetails",
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubcheckRowId", SqlDbType.Int, SubcheckRowId, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@AntecedentRowID", SqlDbType.Int, AntecedentRowID, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@GST", SqlDbType.VarChar, GST, ParameterDirection.Input, 20, false),
            objReturnValueParam);
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;
        }
        public DataTable GetTempPqPersonalData(int PId)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetTempPqPersonalData",
                DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PId, ParameterDirection.Input, 10, false));
            return dt;
        }


        public int AddCoinCircleTrustCRC(int PersonalRowId, int SubCheckRowId, string CompanyRefNo, string CheckUniqueId, CRCInputRequest model, string JsonResult)
        {
            try
            {
                courtCaseSearchRoot modelResult = new courtCaseSearchRoot();
                if (JsonResult != "")
                {
                    modelResult = Newtonsoft.Json.JsonConvert.DeserializeObject<courtCaseSearchRoot>(JsonResult);
                }
                if (modelResult != null)
                {
                    int intReturnValue = 0;
                    string SP_PARAM_RETURN_VALUE = null;
                    SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
                    DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "SP_InsertCoinCircleTrustCRC",
             DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
              DAS.CreateParameter("@CheckUniqueId", SqlDbType.VarChar, CheckUniqueId, ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@requestId", SqlDbType.VarChar, modelResult.data.requestIds[0], ParameterDirection.Input, 1000, false),
              DAS.CreateParameter("@candidateName", SqlDbType.VarChar, model.candidateName, ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@fatherName", SqlDbType.VarChar, model.fatherName, ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@dateOfBirth", SqlDbType.VarChar, model.dateOfBirth, ParameterDirection.Input, 50, false),
              DAS.CreateParameter("@state", SqlDbType.VarChar, model.state, ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@District", SqlDbType.VarChar, model.district, ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@address", SqlDbType.VarChar, model.address, ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@lookupYears", SqlDbType.VarChar, "", ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@LookupScope", SqlDbType.VarChar, "", ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@overallRiskScore", SqlDbType.VarChar, "", ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@OverallRiskLevel", SqlDbType.VarChar, "", ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@status", SqlDbType.VarChar, "", ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@result", SqlDbType.VarChar, "", ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@JsonRequest", SqlDbType.NVarChar, JsonResult, ParameterDirection.Input, int.MaxValue, false),
              DAS.CreateParameter("@JsonResult", SqlDbType.NVarChar, "", ParameterDirection.Input, int.MaxValue, false),
              DAS.CreateParameter("@CreatedBy", SqlDbType.Int, 0, ParameterDirection.Input, 10, false),
            objReturnValueParam
            );
                    intReturnValue = Convert.ToInt32(objReturnValueParam.Value);
                    return intReturnValue;
                }
                else
                {
                    throw new Exception("Invalid Client Details!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int UpdateCoinCircleTrustCRC(int AutoId, string JsonResult)
        {
            CRCResultRoot modelResult = new CRCResultRoot();
            
            if (JsonResult != "")
            {
                modelResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CRCResultRoot>(JsonResult);
            }
           
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "USP_UpdateCoinCircleTrustCRC",
                  DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
                  DAS.CreateParameter("@lookupYears", SqlDbType.VarChar, modelResult.data.requestCriteria.lookupYears, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@LookupScope", SqlDbType.VarChar, modelResult.data.requestCriteria.lookupScope, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@overallRiskScore", SqlDbType.VarChar, "", ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@OverallRiskLevel", SqlDbType.VarChar, "", ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@status", SqlDbType.VarChar, modelResult.data.status, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@result", SqlDbType.VarChar, modelResult.data.result, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@JsonResult", SqlDbType.NVarChar, JsonResult, ParameterDirection.Input, int.MaxValue, false)
                  );
            return a;
        }
        public DataTable getCoinCircleTrustCRC(int AutoId, int Pid, int Sid, string CompanyRefNo, string DateFrom, string DateTo)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "getCoinCircleTrustCRC",
            DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, Pid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, Sid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@DateFrom", SqlDbType.VarChar, DateFrom, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@DateTo", SqlDbType.VarChar, DateTo, ParameterDirection.Input, 50, false)
           );
            return dt;
        }

        public int AddStateDistrict(string StateCode, string StateName, string DistrictName)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "USP_InsertStateDistrict",
              DAS.CreateParameter("@StateCode", SqlDbType.VarChar, StateCode, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@StateName", SqlDbType.VarChar, StateName, ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@DistrictName", SqlDbType.VarChar, DistrictName, ParameterDirection.Input, 100, false)
              );
            return a;
        }
        public DataTable getStateMaster(string StateName, int StateID = 0)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "getStateMaster",
            DAS.CreateParameter("@StateName", SqlDbType.VarChar, StateName, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@StateID", SqlDbType.Int, StateID, ParameterDirection.Input, 10, false)
           );
            return dt;
        }
        public DataTable getDistrictMaster(int StateID, string DistrictName, int DistrictCode = 0)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "getDistrictMaster",
            DAS.CreateParameter("@StateID", SqlDbType.Int, StateID, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@DistrictName", SqlDbType.VarChar, DistrictName, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@DistrictCode", SqlDbType.Int, DistrictCode, ParameterDirection.Input, 10, false)
           );
            return dt;
        }
        public DataTable GetCreminalInfoFromtbl(int PersonalRowId, int SubCheckRowId)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetCreminalInfoFromtbl",
            DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubcheckRowID", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false));
            return ds;
        }
        public DataTable GetCRCnotExecutetbl(int SubCheckRowId)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetCRCnotExecutetbl",
            DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false));
            return ds;
        }
        public DataTable GetCRCRequestData()
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetCRCRequestData");
            return ds;
        }

        public DataTable GetLlmtechAadharDetails(int AutoId, int Pid, int Sid, string CompanyRefNo, string Aadhar, string DateFrom, string DateTo)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetLlmtechAadharDetails",
            DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, Pid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, Sid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@AadharNo", SqlDbType.VarChar, Aadhar, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@DateFrom", SqlDbType.VarChar, DateFrom, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@DateTo", SqlDbType.VarChar, DateTo, ParameterDirection.Input, 50, false)
           );
            return dt;
        }
        public int AddLlmtechAadharDetails(int PersonalRowId, int SubCheckRowId, string CompanyRefNo, string Aadhar, string JsonString, int StatusCode)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "AddLlmtechAadharDetails",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
              DAS.CreateParameter("@AadharNo", SqlDbType.VarChar, Aadhar, ParameterDirection.Input, 20, false),
              DAS.CreateParameter("@JsonString", SqlDbType.NVarChar, JsonString, ParameterDirection.Input, int.MaxValue, false),
              DAS.CreateParameter("@StatusCode", SqlDbType.Int, StatusCode, ParameterDirection.Input, 10, false)
              );
            return a;
        }
        public int DataEntryCRCValueUpdate(int PId, int SId, int AntecedentRowID)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "DataEntryCRCValueUpdate",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@AntecedentRowID", SqlDbType.Int, AntecedentRowID, ParameterDirection.Input, 10, false)
              );
            return a;
        }
        public int AddViotalCibilDetails(int PersonalRowId, int SubCheckRowId, string CompanyRefNo, string Pan, string Name, string MobileNo, string CIBILScore, string FirstResponce, string JosnString)
        {
            int a = 0;
            a = (int)DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "AddViotalCibilDetails",
              DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, PersonalRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, SubCheckRowId, ParameterDirection.Input, 10, false),
              DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
              DAS.CreateParameter("@Name", SqlDbType.VarChar, Name, ParameterDirection.Input, 200, false),
              DAS.CreateParameter("@PanNo", SqlDbType.VarChar, Pan, ParameterDirection.Input, 20, false),
              DAS.CreateParameter("@MobileNo", SqlDbType.VarChar, MobileNo, ParameterDirection.Input, 20, false),
              DAS.CreateParameter("@CIBILScore", SqlDbType.VarChar, CIBILScore, ParameterDirection.Input, 20, false),
              DAS.CreateParameter("@FirstResponce", SqlDbType.NVarChar, FirstResponce, ParameterDirection.Input, int.MaxValue, false),
              DAS.CreateParameter("@JosnString", SqlDbType.NVarChar, JosnString, ParameterDirection.Input, int.MaxValue, false)
              );
            return a;
        }

        public DataTable GetViotalCibilDetails(int AutoId, int Pid, int Sid, string CompanyRefNo, string DateFrom, string DateTo)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetViotalCibilDetails",
            DAS.CreateParameter("@AutoId", SqlDbType.Int, AutoId, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@PersonalRowId", SqlDbType.Int, Pid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@SubCheckRowId", SqlDbType.Int, Sid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, CompanyRefNo, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@FromDate", SqlDbType.VarChar, DateFrom, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@FromTo", SqlDbType.VarChar, DateTo, ParameterDirection.Input, 50, false)
           );
            return dt;
        }

    }
}
