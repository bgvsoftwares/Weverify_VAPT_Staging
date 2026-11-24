using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using ViewModels;
using System.Net.Http.Headers;
using System.Net.Http;
using ViewModels.DigiTap;
using ViewModels.VendorAPI;
using BAL.OCR;
using ViewModels.llmtech;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Threading;
using System.Net;
using System.Web.UI.WebControls;
using ViewModels.refinitiv;
using static MsgReader.Outlook.Storage;
using System.Configuration;
using ViewModels.ProvidedInfoViewModel;
using static BAL.WhatsAppAPI;
using System.Threading.Tasks;
using ViewModels.Perfios;

namespace BAL.ProvidedInfoRepository
{
   public class AddUANCheckDetailsByAPI
    {
        public string UploadUanArtefacts(string UANno)
        {

            return "";
        }
        InstantCheckRepository InstantcheckObj = new InstantCheckRepository();

        public List<Result_Source> GetUANThroughtMobileOrPAN_DigiTap(int PersonalRowId,int SubCheckRowId,string mobileNo, string PanNo, string CaseId, string Path,int CreatedBy)
        {
            DataBaseConnections DC = new DataBaseConnections();
            List<Result_Source> lst = new List<Result_Source>();
            try
            {
                string Mobile_No = "";
                string PANNo = "";
                var uanDt = DC.GetDigitapUANThroughMobilePAN(0, PersonalRowId, SubCheckRowId, "", PanNo, mobileNo, "", "");
                if (uanDt.Rows.Count > 0)
                {
                    Mobile_No = Convert.ToString(uanDt.Rows[0]["MobileNo"]);
                    PANNo = Convert.ToString(uanDt.Rows[0]["PanNo"]);
                    if (Mobile_No == mobileNo && PanNo == PANNo)
                    {
                        string JsonString = Convert.ToString(uanDt.Rows[0]["JsonString"]);
                        if (JsonString != "")
                        {
                            MobileToUANRootobject model = JsonConvert.DeserializeObject<MobileToUANRootobject>(JsonString);
                          
                            if (model.result_code == 101)
                            {
                                foreach (var item in model.result_source)
                                {
                                    Result_Source uan = new Result_Source();
                                    uan.uan = item.uan;
                                    uan.source = item.source;
                                    lst.Add(uan);
                                }
                            }                            
                        }
                    }
                    else
                    {
                        string JsonString = DigiTapMobileToUAN(CaseId, mobileNo, PanNo, Path);
                        if (JsonString != "")
                        {
                            DC.InsertUpdateDigitapUANThroughMobilePAN(PersonalRowId, SubCheckRowId, CaseId, "", mobileNo, PanNo, JsonString, CreatedBy);
                            MobileToUANRootobject model = JsonConvert.DeserializeObject<MobileToUANRootobject>(JsonString);

                            if (model.result_code == 101)
                            {
                                foreach (var item in model.result_source)
                                {
                                    Result_Source uan = new Result_Source();
                                    uan.uan = item.uan;
                                    uan.source = item.source;
                                    lst.Add(uan);
                                }
                            }
                        }
                    }
                }
                else
                {
                    string JsonString = DigiTapMobileToUAN(CaseId, mobileNo, PanNo, Path);
                    if (JsonString != "")
                    {
                        DC.InsertUpdateDigitapUANThroughMobilePAN(PersonalRowId, SubCheckRowId, CaseId, "", mobileNo, PanNo, JsonString, CreatedBy);
                        MobileToUANRootobject model = JsonConvert.DeserializeObject<MobileToUANRootobject>(JsonString);
                        if (model.result_code == 101)
                        {
                            foreach (var item in model.result_source)
                            {
                                Result_Source uan = new Result_Source();

                                uan.uan = item.uan;
                                uan.source = item.source;
                                lst.Add(uan);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {

            }
            return lst;

        }
        public string AddPassportRootDetails(int PersonalRowId, int SubCheckRowId, int ClientRowId, string Passport, string CandidateName, int CreatedBy = 0,string DOB="")
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetPassportRoot(0, Passport, PersonalRowId);
                if (tbl.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    string name = CandidateName;
                    dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                            int rst = CC.InsertUpdatePassportDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, Passport, name);
                        }
                    }
                }
                else
                {
                    var Result = InstantcheckObj.GetpassportLive(Passport,DOB);
                    if (Result.data != null)
                    {   
                        result = CC.InsertIntoPassport(Result, PersonalRowId, Passport, CreatedBy);
                        tbl = CC.GetPassportRoot(Convert.ToInt32(result), Passport, PersonalRowId);
                        if (tbl.Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            string name = CandidateName;
                            dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                    int rst = CC.InsertUpdatePassportDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, Passport, name);

                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }


            return result;
        }


        public string AddDrivingLicenseDetails(int PersonalRowId, int SubCheckRowId, int ClientRowId, string License, string CandidateName,int CreatedBy,string filePath,string dob, string CheckUniqueID)
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();          
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetDriving_License(0, License, PersonalRowId, SubCheckRowId);
                if (tbl.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    string name = CandidateName;
                    result = tbl.Rows[0]["AutoId"].ToString();
                    dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                            int rst = CC.InsertUpdateDriving_LicenseDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, License, name);
                        }
                    }
                }
                else
                {
                    var Result = InstantcheckObj.GetDrivinglicenseLive(License, dob, CheckUniqueID);
                    if (Result.data != null)
                    {
                        if (Result.statuscode == 200)
                        {
                            var additional_check = "";
                            var vehicle_classes = "";
                            foreach (var item in Result.data.additional_check)
                            {
                                additional_check = additional_check + "," + item.ToString().Trim();
                            }
                            foreach (var item in Result.data.vehicle_classes)
                            {
                                vehicle_classes = vehicle_classes + "," + item.ToString().Trim();
                            }
                            if (additional_check.Length > 0)
                            {
                                additional_check = additional_check.Substring(1, additional_check.Length);
                            }
                            if (vehicle_classes.Length > 0)
                            {
                                vehicle_classes = vehicle_classes.Substring(1, vehicle_classes.Length - 1);
                            }
                            var filename = Convert.ToString(PersonalRowId) + "_" + License + ".jpg";
                            System.IO.File.WriteAllBytes(filePath + filename, Convert.FromBase64String(Result.data.profile_image));
                            Result.data.profile_image = filename;

                            result = CC.InsertIntoDriving_License(Result, PersonalRowId, License, CreatedBy, vehicle_classes, additional_check, CheckUniqueID, SubCheckRowId);
                            tbl = CC.GetDriving_License(Convert.ToInt32(result), License, PersonalRowId, SubCheckRowId);
                            if (tbl.Rows.Count > 0)
                            {
                                DataTable dt = new DataTable();
                                string name = CandidateName;
                                dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                        int rst = CC.InsertUpdateDriving_LicenseDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, License, name);

                                    }
                                }
                            }
                        }
                           
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }


            return result;
        }

        public string AddDriving_LicenseDetails( string License, int CreatedBy, string filePath, string dob, string CompanyRefNo)
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetDrivingLicense(0, License, CompanyRefNo);
                if (tbl.Rows.Count > 0)
                {                               
                    result = tbl.Rows[0]["AutoId"].ToString();                   
                }
                else
                {
                    var Result = InstantcheckObj.GetDrivinglicenseLive(License, Convert.ToDateTime(dob).ToString("yyyy-MM-dd"), CompanyRefNo+"-Offline");
                    if (Result.data != null)
                    {
                        if (Result.statuscode == 200)
                        {
                            var additional_check = "";
                            var vehicle_classes = "";
                            foreach (var item in Result.data.additional_check)
                            {
                                additional_check = additional_check + "," + item.ToString().Trim();
                            }
                            foreach (var item in Result.data.vehicle_classes)
                            {
                                vehicle_classes = vehicle_classes + "," + item.ToString().Trim();
                            }
                            if (additional_check.Length > 0)
                            {
                                additional_check = additional_check.Substring(1, additional_check.Length);
                            }
                            if (vehicle_classes.Length > 0)
                            {
                                vehicle_classes = vehicle_classes.Substring(1, vehicle_classes.Length - 1);
                            }
                            var filename = Convert.ToString(CompanyRefNo) + "_" + License + ".jpg";
                            System.IO.File.WriteAllBytes(filePath + filename, Convert.FromBase64String(Result.data.profile_image));
                            Result.data.profile_image = filename;

                            result = CC.InsertIntoDrivingLicense(Result, CompanyRefNo, License, CreatedBy, vehicle_classes, additional_check, CompanyRefNo+"-Offline");                          
                           
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }


            return result;
        }

        public string BulkAddDrivingLicenseDetails(int PersonalRowId, int SubCheckRowId, int ClientRowId, string License, string CandidateName, int CreatedBy = 0, string filePath = "", string dob = "" ,string CheckUniqueID="")
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetDriving_License(0, License, PersonalRowId);
                if (tbl.Rows.Count > 0)
                {
                    result = tbl.Rows[0]["AutoId"].ToString();
                }
                else
                {
                    var Result = InstantcheckObj.GetDrivinglicenseLive(License, dob, CheckUniqueID);
                    if (Result.data != null)
                    {
                        var additional_check = "";
                        var vehicle_classes = "";
                        foreach (var item in Result.data.additional_check)
                        {
                            additional_check = additional_check + "," + item.ToString().Trim();
                        }
                        foreach (var item in Result.data.vehicle_classes)
                        {
                            vehicle_classes = vehicle_classes + "," + item.ToString().Trim();
                        }
                        if (additional_check.Length > 0)
                        {
                            additional_check = additional_check.Substring(1, additional_check.Length);
                        }
                        if (vehicle_classes.Length > 0)
                        {
                            vehicle_classes = vehicle_classes.Substring(1, vehicle_classes.Length - 1);
                        }
                        var filename = Convert.ToString(PersonalRowId) + "_" + License + ".jpg";
                        System.IO.File.WriteAllBytes(filePath + filename, Convert.FromBase64String(Result.data.profile_image));
                        Result.data.profile_image = filename;

                        result = CC.InsertIntoDriving_License(Result, PersonalRowId, License, CreatedBy, vehicle_classes, additional_check, CheckUniqueID);
                        
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }


            return result;
        }

        public string AddAadhaarNumberDetails(int PersonalRowId, int SubCheckRowId, int ClientRowId, string AadhaarNumber, string CandidateName, int CreatedBy, string CheckUniqueID)
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            List<VoterIdList> lst = new List<VoterIdList>();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetAadhaar(0, AadhaarNumber, PersonalRowId, SubCheckRowId);
                if (tbl.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    string name = CandidateName;
                    dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                            int rst = CC.InsertUpdateAadhaarNumberCheckDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, AadhaarNumber, name);
                        }
                    }
                }
                else
                {
                    var Result = InstantcheckObj.Getaadhaar_without_otpLive(AadhaarNumber, CheckUniqueID);
                    if (Result.data != null)
                    {
                        result = CC.InsertIntoAadhaar(Result, PersonalRowId, AadhaarNumber, CreatedBy, CheckUniqueID, SubCheckRowId);
                        tbl = CC.GetAadhaar(Convert.ToInt32(result), AadhaarNumber, PersonalRowId, SubCheckRowId);
                        if (tbl.Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            string name = CandidateName;
                            dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                    int rst = CC.InsertUpdateAadhaarNumberCheckDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, AadhaarNumber, name);

                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }


            return result;
        }

        public string AddAadhaarNumberDetails(int PersonalRowId, int SubCheckRowId,  string AadhaarNumber, string CandidateName, int CreatedBy)
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.PaySprintVerifyAadhaarNumber(0, AadhaarNumber, PersonalRowId, SubCheckRowId);
                if (tbl.Rows.Count > 0)
                {
                    string name = CandidateName;
                    result = Convert.ToString(tbl.Rows[0]["AutoId"]);
                }
                else
                {
                    var Result = InstantcheckObj.Getaadhaar_without_otpLive(AadhaarNumber, PersonalRowId.ToString());
                    if (Result.data != null)
                    {
                        result = CC.TempInsertIntoAadhaar(Result, PersonalRowId, AadhaarNumber, CreatedBy, PersonalRowId.ToString(), SubCheckRowId);                       
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public string AddAadhaar_NumberDetails(string AadhaarNumber, int CreatedBy, string CompanyRefNo)
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            List<VoterIdList> lst = new List<VoterIdList>();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetAadhaarNo(0, AadhaarNumber, CompanyRefNo);
                if (tbl.Rows.Count > 0)
                {
                    result = Convert.ToString(tbl.Rows[0]["AutoId"]);
                }
                else
                {
                    var Result = InstantcheckObj.Getaadhaar_without_otpLive(AadhaarNumber, CompanyRefNo+"Offline");
                    if (Result.data != null)
                    {
                        result = CC.InsertAadhaarDetails(Result, CompanyRefNo, AadhaarNumber, CreatedBy, CompanyRefNo + "Offline");                        
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }


            return result;
        }


        public string BulkAddAadhaarNumberDetails(int PersonalRowId, int SubCheckRowId, int ClientRowId, string AadhaarNumber, string CandidateName, int CreatedBy = 0, string CheckUniqueID="")
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            List<VoterIdList> lst = new List<VoterIdList>();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetAadhaar(0, AadhaarNumber, PersonalRowId, SubCheckRowId);
                if (tbl.Rows.Count > 0)
                {
                    result = tbl.Rows[0]["AutoId"].ToString();
                }
                else
                {
                    var Result = InstantcheckObj.Getaadhaar_without_otpLive(AadhaarNumber, CheckUniqueID);
                    if (Result.data != null)
                    {
                        if (Result.statuscode == 200)
                        {
                            result = CC.InsertIntoAadhaar(Result, PersonalRowId, AadhaarNumber, CreatedBy, CheckUniqueID);
                        }
                                          
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return result;
        }




        public string AddPanNumberDetails(int PersonalRowId, int SubCheckRowId, int ClientRowId, string PanNumber, int CreatedBy, string CheckUniqueID,string APILog="")
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            List<VoterIdList> lst = new List<VoterIdList>();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetPAN(0, PanNumber, PersonalRowId, SubCheckRowId);
                if (tbl.Rows.Count > 0)
                {
                    result = tbl.Rows[0]["AutoId"].ToString();
                    DataTable dt = new DataTable();
                    string name = tbl.Rows[0]["full_name"].ToString();
                    dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                            int rst = CC.InsertUpdatePanNumberCheckDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, PanNumber, name);

                        }
                    }
                }
                else
                {
                    var Result = InstantcheckObj.GetPANLive(PanNumber, CheckUniqueID, APILog);
                    if (Result.data != null)
                    {
                        result = CC.InsertIntoPAN(Result, PersonalRowId, PanNumber, CreatedBy, CheckUniqueID, SubCheckRowId);
                        tbl = CC.GetPAN(Convert.ToInt32(result), PanNumber, PersonalRowId, SubCheckRowId);                      
                        if (tbl.Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            string name = tbl.Rows[0]["full_name"].ToString();
                            dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                    int rst = CC.InsertUpdatePanNumberCheckDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, PanNumber, name);

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public string AddOfflinePanNumberDetails(string PanNumber, int CreatedBy, string CompanyRefNo,string APIlog)
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            List<VoterIdList> lst = new List<VoterIdList>();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetPaysPrintPANNumber(0, PanNumber, CompanyRefNo);
                if (tbl.Rows.Count > 0)
                {
                    result = tbl.Rows[0]["AutoId"].ToString();                                 

                }
                else
                {
                    var Result = InstantcheckObj.GetPANLive(PanNumber, CompanyRefNo+ "-Offline", APIlog);
                    if (Result.data != null)
                    {
                        result = CC.InsertPaysPrintPANNumber(Result, CompanyRefNo, PanNumber, CreatedBy, CompanyRefNo + "-Offline");                       
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public string BulkAddPanNumberDetails(int PersonalRowId, int SubCheckRowId, int ClientRowId, string PanNumber, int CreatedBy = 0, string CheckUniqueID="",string APIlog="")
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            List<VoterIdList> lst = new List<VoterIdList>();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetPAN(0, PanNumber, PersonalRowId, SubCheckRowId);
                if (tbl.Rows.Count > 0)
                {
                    result = tbl.Rows[0]["AutoId"].ToString();
                }
                else
                {
                    var Result = InstantcheckObj.GetPANLive(PanNumber, CheckUniqueID, APIlog);
                    if (Result.data != null)
                    {
                        if (Result.statuscode == 200)
                        {
                            result = CC.InsertIntoPAN(Result, PersonalRowId, PanNumber, CreatedBy, CheckUniqueID, SubCheckRowId);
                        }                            
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public string GetAadhaarNumberDetails(int PersonalRowId, int SubCheckRowId, int ClientRowId, string AadhaarNumber, string CandidateName, int CreatedBy = 0)
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            List<VoterIdList> lst = new List<VoterIdList>();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetAadhaar(0, AadhaarNumber, PersonalRowId, SubCheckRowId);
                if (tbl.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    string name = CandidateName;
                    result = tbl.Rows[0]["AutoId"].ToString();
                    dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                            int rst = CC.InsertUpdateAadhaarNumberCheckDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, AadhaarNumber, name);
                        }
                    }
                }
                else
                {
                    var Result = InstantcheckObj.Getaadhaar_withoutotpLive(AadhaarNumber);
                    if (Result.statuscode == "200")
                    {
                        result = CC.InsertInto_Aadhaar(Result, PersonalRowId, AadhaarNumber, CreatedBy);
                        tbl = CC.GetAadhaar(Convert.ToInt32(result), AadhaarNumber, PersonalRowId, SubCheckRowId);
                        if (tbl.Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            string name = CandidateName;
                            dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                    int rst = CC.InsertUpdateAadhaarNumberCheckDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, AadhaarNumber, name);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
            return result;
        }

        public string VoterId(int PersonalRowId, int SubCheckRowId, int ClientRowId, string voterId, int CreatedBy, string CheckUniqueID)
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            List<VoterIdList> lst = new List<VoterIdList>();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetPaySprintVerifyVoter_verifyDetails(0, voterId, PersonalRowId, SubCheckRowId);
                if (tbl.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    string name = tbl.Rows[0]["name"].ToString();
                    dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                            int rst = CC.InsertUpdateIDCheckDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, voterId, name);

                        }
                    }                   
                }
                else
                {
                    var Result = InstantcheckObj.GetVoterIdCardVerificationLive(voterId, CheckUniqueID);
                    if (Result.data != null)
                    {
                        result = CC.InsertintoSprintVerifyVoter_verify(Result, PersonalRowId, voterId, CreatedBy, CheckUniqueID, SubCheckRowId);
                        tbl = CC.GetPaySprintVerifyVoter_verifyDetails(Convert.ToInt32(result), voterId, PersonalRowId, SubCheckRowId);
                        if (tbl.Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            string name = tbl.Rows[0]["name"].ToString();
                            dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                    int rst = CC.InsertUpdateIDCheckDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, voterId, name);

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public string Voter_Id(string voterId, int CreatedBy, string CompanyRefNo)
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            List<VoterIdList> lst = new List<VoterIdList>();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetPaysPrintVoterIDDetails(0, voterId, CompanyRefNo);
                if (tbl.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    string name = tbl.Rows[0]["name"].ToString();
                    result = tbl.Rows[0]["AutoId"].ToString();

                }
                else
                {
                    var Result = InstantcheckObj.GetVoterIdCardVerificationLive(voterId, CompanyRefNo + "-Offline");
                    if (Result.data != null)
                    {
                        result = CC.InsertintoPaysPrintVoterId_verify(Result, CompanyRefNo, voterId, CreatedBy, CompanyRefNo + "-Offline");                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public string BulkVoterId(int PersonalRowId, int SubCheckRowId, int ClientRowId, string voterId, string CheckUniqueID, int CreatedBy = 0)
        {
            var result = "";
            ClientCustomize CC = new ClientCustomize();
            List<VoterIdList> lst = new List<VoterIdList>();
            DataTable tbl = new DataTable();
            try
            {
                tbl = CC.GetPaySprintVerifyVoter_verifyDetails(0, voterId, PersonalRowId);
                if (tbl.Rows.Count > 0)
                {
                    result = tbl.Rows[0]["AutoId"].ToString();
                }
                else
                {
                    var Result = InstantcheckObj.GetVoterIdCardVerificationLive(voterId, CheckUniqueID);
                    if (Result.data != null)
                    {
                        if(Result.statuscode == 200)
                        {
                            result = CC.InsertintoSprintVerifyVoter_verify(Result, PersonalRowId, voterId, CreatedBy,CheckUniqueID);
                        }                                               
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        public string AddUANCheckDetails(int PersonalRowId, string UANno, int ClientRowId, int SubCheckId, int CreatedBy = 0)
        {
            ClientCustomize CC = new ClientCustomize();
            List<Employment_History> _History = new List<Employment_History>();
            string result;
            try
            {               
                var UNDB = CC.ExistsUANNO(UANno);
                if (UNDB.Rows.Count > 0)
                {
                    result = UNDB.Rows[0]["AutoId"].ToString();
                    DataTable tbl = new DataTable();
                    tbl = CC.GetSprintVerify_epfoReport(Convert.ToInt32(UNDB.Rows[0]["AutoId"]));
                    
                    string name = tbl.Rows[0]["name"].ToString();
                    string guardian_name = tbl.Rows[0]["guardian_name"].ToString();
                    DataTable dt = new DataTable();
                    dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckId, "Operation", "Check");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                            int rst = CC.InsertUpdateUANCheckDetails(PersonalRowId, SubCheckId, Antecedents, ClientRowId, UANno, name, guardian_name);
                        }
                    }
                }
                else
                {
                    var DataObj = InstantcheckObj.GetEPFO_without_otpTest(UANno);
                    if (DataObj.message.ToLower() == "success")
                    {
                         result = CC.InsertintoSprintVerifyEPFO_history(DataObj, PersonalRowId.ToString(), UANno, CreatedBy);
                        DataTable tbl = new DataTable();
                        tbl = CC.GetSprintVerify_epfoReport(Convert.ToInt32(result));
                        string name = tbl.Rows[0]["name"].ToString();
                        string guardian_name = tbl.Rows[0]["guardian_name"].ToString();

                        DataTable dt = new DataTable();
                        dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckId, "Operation", "Check");
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                              int rst = CC.InsertUpdateUANCheckDetails(PersonalRowId, SubCheckId, Antecedents, ClientRowId, UANno, name, guardian_name);

                            }
                        }
                    }
                }   

            }
            catch (Exception ex)
            {
               
            }
          
            return "";
        }

        public string AddUANCheckDetailsLive(int PersonalRowId, string UANno, int ClientRowId, int SubCheckId, int CreatedBy = 0)
        {
            ClientCustomize CC = new ClientCustomize();
            List<Employment_History> _History = new List<Employment_History>();
            string result = "";
            try
            {
                var UNDB = CC.ExistsUANNO(UANno);
                if (UNDB.Rows.Count > 0)
                {
                    result = UNDB.Rows[0]["AutoId"].ToString();
                    DataTable tbl = new DataTable();
                    tbl = CC.GetSprintVerify_epfoReport(Convert.ToInt32(UNDB.Rows[0]["AutoId"]));

                    string name = tbl.Rows[0]["name"].ToString();
                    string guardian_name = tbl.Rows[0]["guardian_name"].ToString();
                    DataTable dt = new DataTable();
                    dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckId, "Data Entry", "Check");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                            int rst = CC.InsertUpdateUANCheckDetails(PersonalRowId, SubCheckId, Antecedents, ClientRowId, UANno, name, guardian_name);

                        }
                    }
                }
                else
                {
                    var DataObj = InstantcheckObj.GetEPFO_without_otpLive(UANno);
                    if (DataObj.message.ToUpper().Contains("SUCCESS"))
                    {
                        result = CC.InsertintoSprintVerifyEPFO_history(DataObj, PersonalRowId.ToString(), UANno, CreatedBy);
                        DataTable tbl = new DataTable();
                        tbl = CC.GetSprintVerify_epfoReport(Convert.ToInt32(result));
                        string name = tbl.Rows[0]["name"].ToString();
                        string guardian_name = tbl.Rows[0]["guardian_name"].ToString();

                        DataTable dt = new DataTable();
                        dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckId, "Operation", "Check");
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                int rst = CC.InsertUpdateUANCheckDetails(PersonalRowId, SubCheckId, Antecedents, ClientRowId, UANno, name, guardian_name);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public string AddUANDetailsLive(int PersonalRowId, string UANno, int CreatedBy = 0)
        {
            ClientCustomize CC = new ClientCustomize();
            List<Employment_History> _History = new List<Employment_History>();
            string result = "";
            try
            {
                var UNDB = CC.ExistsUANNO(UANno);
                if (UNDB.Rows.Count > 0)
                {
                    result = UNDB.Rows[0]["AutoId"].ToString();                   
                }
                else
                {
                    var DataObj = InstantcheckObj.GetEPFO_without_otpLive(UANno);
                    if (DataObj.message.ToLower().Contains("success"))
                    {
                        result = CC.InsertintoSprintVerifyEPFO_history(DataObj, PersonalRowId.ToString(), UANno, CreatedBy);                        
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public string AddUANCheckDetailsWithMobileNoAndPan(int PersonalRowId, string MobileNo,string Pan,string uan,string type, int ClientRowId, int SubCheckId, int CreatedBy = 0)
        {
            ClientCustomize CC = new ClientCustomize();            
            string result="";
            try
            {
                var UANDB = CC.SearchExistsUANNoWithMobileNoAndPan(0,MobileNo,Pan, uan, Convert.ToInt32(type));
                if (UANDB.Rows.Count > 0)
                {
                    result = UANDB.Rows[0]["AutoId"].ToString();                   
                }
                else
                {
                    var DataObj = InstantcheckObj.GetEPFO_withMobileNoLive(MobileNo,Pan, uan, type);
                    if (DataObj.statuscode == "200")
                    {
                        if(DataObj.message== "UAN details fetched successfully.")
                        {
                            result = CC.InsertintoSprintVerifyEPFO_withMobilePan(DataObj, PersonalRowId.ToString(), MobileNo, Pan, uan, CreatedBy);
                        }                                             
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }



        public List<VoterIdList> GetPaySprintVerifyVoter_verifyDetails(int AutoId = 0, string CompanyRefNo = "", string VoterId = "")
        {
            List<VoterIdList> lst = new List<VoterIdList>();
            try
            {
                ClientCustomize CE = new ClientCustomize();
                DataTable tbl = new DataTable();
                tbl = CE.GetPaySprintVerifyVoter_verifyDetails(AutoId, VoterId);

                if (tbl.Rows.Count > 0)
                {
                    lst = (from DataRow dr in tbl.Rows
                           select new VoterIdList()
                           {
                               AutoId = Convert.ToInt32(dr["AutoId"]),
                               CompanyRefNo = Convert.ToString(dr["CompanyRefNo"]),
                               VoterId = Convert.ToString(dr["VoterId"]),
                               status = Convert.ToBoolean(dr["status"]),
                               message = Convert.ToString(dr["message"]),
                               name = Convert.ToString(dr["name"]),
                               statuscode = Convert.ToInt32(dr["statuscode"]),
                               reference_id = Convert.ToString(dr["reference_id"]),
                               client_id = Convert.ToString(dr["client_id"]),
                               epic_no = Convert.ToString(dr["epic_no"]),
                               gender = Convert.ToString(dr["gender"]),
                               state = Convert.ToString(dr["state"]),
                               relation_name = Convert.ToString(dr["relation_name"]),
                               relation_type = Convert.ToString(dr["relation_type"]),
                               house_no = Convert.ToString(dr["house_no"]),
                               dob = Convert.ToString(dr["dob"]),
                               age = Convert.ToString(dr["age"]),
                               area = Convert.ToString(dr["area"]),
                               district = Convert.ToString(dr["district"]),
                               multiple = Convert.ToString(dr["multiple"]),
                               last_update = Convert.ToString(dr["last_update"]),
                               assembly_constituency = Convert.ToString(dr["assembly_constituency"]),
                               assembly_constituency_number = Convert.ToString(dr["assembly_constituency_number"]),
                               polling_station = Convert.ToString(dr["polling_station"]),
                               part_number = Convert.ToString(dr["part_number"]),
                               part_name = Convert.ToString(dr["part_name"]),
                               slno_inpart = Convert.ToString(dr["slno_inpart"]),
                               ps_lat_long = Convert.ToString(dr["ps_lat_long"]),
                               rln_name_v1 = Convert.ToString(dr["rln_name_v1"]),
                               rln_name_v2 = Convert.ToString(dr["rln_name_v2"]),
                               rln_name_v3 = Convert.ToString(dr["rln_name_v3"]),
                               section_no = Convert.ToString(dr["section_no"]),
                               name_v1 = Convert.ToString(dr["name_v1"]),
                               name_v2 = Convert.ToString(dr["name_v2"]),
                               name_v3 = Convert.ToString(dr["name_v3"]),
                               st_code = Convert.ToString(dr["st_code"]),
                               parliamentary_constituency = Convert.ToString(dr["parliamentary_constituency"]),
                               id = Convert.ToString(dr["id"]),
                           }).ToList();

                }

            }
            catch (Exception ex)
            {
                var rst = ex.Message;
                throw;
            }
            return lst;
        }

        public string DigiTapMobileToUAN(string CheckId, string MobileNo,string PanNo, string Path)
        {
            string JsonString = "";
            try
            {
                var url = "https://svc.digitap.ai/cv/uan_lookup";
                var client = new HttpClient();
                // Set headers
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", "MTU3ODE3ODU6TnhpUmI2ZG0wWVFHZzlIOGxIbzZIbTFnZ2NiYjZOVTQ=");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                // Prepare request body
                var requestBody = new
                {
                    client_ref_num = CheckId,
                    mobile = MobileNo,
                    pan = PanNo
                };
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    // Send POST request synchronously
                    var response = client.PostAsync(url, content).Result;
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    clsCommonMethods1.APILogLog(Path, "Input:"+json.ToString() + " Response:"+responseBody, url, CheckId);
                    if (response.IsSuccessStatusCode)
                    {
                        // Optional: Deserialize into your C# model
                        JsonString = responseBody;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return JsonString;
        }

        public string DigiTapUAN(string CheckId, string UAN, string Path)
        {
            string JsonString = "";
            try
            {
                var url = "https://svc.digitap.ai/cv/v4/uan_basic/sync";
                var client = new HttpClient();

                // Set headers
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", "MTU3ODE3ODU6TnhpUmI2ZG0wWVFHZzlIOGxIbzZIbTFnZ2NiYjZOVTQ=");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Prepare request body
                var requestBody = new
                {
                    client_ref_num = CheckId,
                    uan = UAN,
                    additional_details = true
                };                               

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    // Send POST request synchronously
                    var response = client.PostAsync(url, content).Result;
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    clsCommonMethods1.APILogLog(Path, responseBody, url, CheckId);
                    if (response.IsSuccessStatusCode)
                    {
                        // Optional: Deserialize into your C# model
                        JsonString = responseBody;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return JsonString;

        }
        DataBaseConnections DBC = new DataBaseConnections();
        ClientCustomize CC = new ClientCustomize();

        public UANDetails getDigiTapUAN(int PId, string CompanyRefNo, string UANNo, string Path)
        {           
            UANDetails uANDetails = new UANDetails();
            List<EmploymentHistory> lst = new List<EmploymentHistory>();
            // var Path = Server.MapPath("~\\APILogs\\Logfiles");
            try
            {
                var UANDT = DBC.GetDigitapUAN(0, PId, CompanyRefNo, UANNo);
                if (UANDT.Rows.Count > 0)
                {
                    DateTime startDate = Convert.ToDateTime(UANDT.Rows[0]["VerifiedDate"]);
                    DateTime endDate = DateTime.Now;
                    TimeSpan difference = endDate - startDate;
                    int days = Convert.ToInt32(difference.TotalDays);
                    if (days < 30)
                    {
                        uANDetails.AutoId = Convert.ToInt32(UANDT.Rows[0]["AutoId"]);
                        uANDetails.PersonalRowId = Convert.ToInt32(UANDT.Rows[0]["PersonalRowId"]);
                        uANDetails.CompanyRefNo = Convert.ToString(UANDT.Rows[0]["CompanyRefNo"]);
                        uANDetails.UanNo = Convert.ToString(UANDT.Rows[0]["uan"]);
                        uANDetails.name = Convert.ToString(UANDT.Rows[0]["name"]);
                        uANDetails.date_of_birth = Convert.ToString(UANDT.Rows[0]["date_of_birth"]);
                        uANDetails.mobile = Convert.ToString(UANDT.Rows[0]["mobile"]);
                        uANDetails.client_ref_num = Convert.ToString(UANDT.Rows[0]["client_ref_num"]);
                        uANDetails.request_id = Convert.ToString(UANDT.Rows[0]["request_id"]);
                        uANDetails.gender = Convert.ToString(UANDT.Rows[0]["gender"]);
                        uANDetails.aadhaar_verification_status = Convert.ToString(UANDT.Rows[0]["aadhaar_verification_status"]);
                        uANDetails.is_employed = Convert.ToInt32(UANDT.Rows[0]["is_employed"]);
                        uANDetails.relation = Convert.ToString(UANDT.Rows[0]["relation"]);
                        uANDetails.relative_name = Convert.ToString(UANDT.Rows[0]["relative_name"]);
                        lst = (from DataRow dr in UANDT.Rows
                               select new EmploymentHistory()
                               {
                                   establishment_name = Convert.ToString(dr["establishment_name"]),
                                   establishment_id = Convert.ToString(dr["establishment_id"]),
                                   member_id = Convert.ToString(dr["member_id"]),
                                   date_of_joining = Convert.ToString(dr["date_of_joining"]),
                                   date_of_exit = Convert.ToString(dr["date_of_exit"]),
                                   leave_reason = Convert.ToString(dr["leave_reason"]),
                                   employment_period_in_months = Convert.ToString(dr["employment_period_in_months"]),
                                   employer_confidence_score = Convert.ToString(dr["employer_confidence_score"]),
                               }).ToList();
                        uANDetails.EmploymentHistory = lst;
                    }
                    else
                    {
                        string JsonString = DigiTapUAN(CompanyRefNo, UANNo, Path);

                        DigiTapUANViewModel result = JsonConvert.DeserializeObject<DigiTapUANViewModel>(JsonString);

                        if (result.result_code == 101)
                        {
                            var AutoId = DBC.InsertUpdateDigitapUAN(result, PId, CompanyRefNo, UANNo, JsonString);
                            if (AutoId > 0)
                            {
                                var UAN_DT = DBC.GetDigitapUAN(AutoId, PId, CompanyRefNo, UANNo);
                                if (UAN_DT.Rows.Count > 0)
                                {
                                    uANDetails.AutoId = Convert.ToInt32(UAN_DT.Rows[0]["AutoId"]);
                                    uANDetails.PersonalRowId = Convert.ToInt32(UAN_DT.Rows[0]["PersonalRowId"]);
                                    uANDetails.CompanyRefNo = Convert.ToString(UAN_DT.Rows[0]["CompanyRefNo"]);
                                    uANDetails.UanNo = Convert.ToString(UAN_DT.Rows[0]["uan"]);
                                    uANDetails.name = Convert.ToString(UAN_DT.Rows[0]["name"]);
                                    uANDetails.date_of_birth = Convert.ToString(UAN_DT.Rows[0]["date_of_birth"]);
                                    uANDetails.mobile = Convert.ToString(UAN_DT.Rows[0]["mobile"]);
                                    uANDetails.client_ref_num = Convert.ToString(UAN_DT.Rows[0]["client_ref_num"]);
                                    uANDetails.request_id = Convert.ToString(UAN_DT.Rows[0]["request_id"]);
                                    uANDetails.gender = Convert.ToString(UAN_DT.Rows[0]["gender"]);
                                    uANDetails.aadhaar_verification_status = Convert.ToString(UAN_DT.Rows[0]["aadhaar_verification_status"]);
                                    uANDetails.is_employed = Convert.ToInt32(UAN_DT.Rows[0]["is_employed"]);
                                    uANDetails.relation = Convert.ToString(UANDT.Rows[0]["relation"]);
                                    uANDetails.relative_name = Convert.ToString(UANDT.Rows[0]["relative_name"]);
                                    lst = (from DataRow dr in UAN_DT.Rows
                                           select new EmploymentHistory()
                                           {
                                               establishment_name = Convert.ToString(dr["establishment_name"]),
                                               establishment_id = Convert.ToString(dr["establishment_id"]),
                                               member_id = Convert.ToString(dr["member_id"]),
                                               date_of_joining = Convert.ToString(dr["date_of_joining"]),
                                               date_of_exit = Convert.ToString(dr["date_of_exit"]),
                                               leave_reason = Convert.ToString(dr["leave_reason"]),
                                               employment_period_in_months = Convert.ToString(dr["employment_period_in_months"]),
                                               employer_confidence_score = Convert.ToString(dr["employer_confidence_score"]),
                                           }).ToList();
                                    uANDetails.EmploymentHistory = lst;
                                }
                            }
                        }         
                        
                    }
                }
                else
                {
                    var JsonString = DigiTapUAN(CompanyRefNo, UANNo, Path);
                    DigiTapUANViewModel result = JsonConvert.DeserializeObject<DigiTapUANViewModel>(JsonString);

                    if (result.result_code == 101)
                    {
                        var AutoId = DBC.InsertUpdateDigitapUAN(result, PId, CompanyRefNo, UANNo, JsonString);
                        if (AutoId > 0)
                        {
                            var UAN_DT = DBC.GetDigitapUAN(AutoId, PId, CompanyRefNo, UANNo);
                            if (UAN_DT.Rows.Count > 0)
                            {
                                uANDetails.AutoId = Convert.ToInt32(UAN_DT.Rows[0]["AutoId"]);
                                uANDetails.PersonalRowId = Convert.ToInt32(UAN_DT.Rows[0]["PersonalRowId"]);
                                uANDetails.CompanyRefNo = Convert.ToString(UAN_DT.Rows[0]["CompanyRefNo"]);
                                uANDetails.UanNo = Convert.ToString(UAN_DT.Rows[0]["uan"]);
                                uANDetails.name = Convert.ToString(UAN_DT.Rows[0]["name"]);
                                uANDetails.date_of_birth = Convert.ToString(UAN_DT.Rows[0]["date_of_birth"]);
                                uANDetails.mobile = Convert.ToString(UAN_DT.Rows[0]["mobile"]);
                                uANDetails.client_ref_num = Convert.ToString(UAN_DT.Rows[0]["client_ref_num"]);
                                uANDetails.request_id = Convert.ToString(UAN_DT.Rows[0]["request_id"]);
                                uANDetails.gender = Convert.ToString(UAN_DT.Rows[0]["gender"]);
                                uANDetails.aadhaar_verification_status = Convert.ToString(UAN_DT.Rows[0]["aadhaar_verification_status"]);
                                uANDetails.is_employed = Convert.ToInt32(UAN_DT.Rows[0]["is_employed"]);
                                uANDetails.relation = Convert.ToString(UAN_DT.Rows[0]["relation"]);
                                uANDetails.relative_name = Convert.ToString(UAN_DT.Rows[0]["relative_name"]);

                                lst = (from DataRow dr in UAN_DT.Rows
                                       select new EmploymentHistory()
                                       {
                                           establishment_name = Convert.ToString(dr["establishment_name"]),
                                           establishment_id = Convert.ToString(dr["establishment_id"]),
                                           member_id = Convert.ToString(dr["member_id"]),
                                           date_of_joining = Convert.ToString(dr["date_of_joining"]),
                                           date_of_exit = Convert.ToString(dr["date_of_exit"]),
                                           leave_reason = Convert.ToString(dr["leave_reason"]),
                                           employment_period_in_months = Convert.ToString(dr["employment_period_in_months"]),
                                           employer_confidence_score = Convert.ToString(dr["employer_confidence_score"]),
                                       }).ToList();
                                uANDetails.EmploymentHistory = lst;
                            }
                        }
                    }
                    else if (result.http_response_code == 505)
                    {
                        Thread.Sleep(5000);
                         JsonString = DigiTapUAN(CompanyRefNo, UANNo, Path);
                         result = JsonConvert.DeserializeObject<DigiTapUANViewModel>(JsonString);

                        if (result.result_code == 101)
                        {
                            var AutoId = DBC.InsertUpdateDigitapUAN(result, PId, CompanyRefNo, UANNo, JsonString);
                            if (AutoId > 0)
                            {
                                var UAN_DT = DBC.GetDigitapUAN(AutoId, PId, CompanyRefNo, UANNo);
                                if (UAN_DT.Rows.Count > 0)
                                {
                                    uANDetails.AutoId = Convert.ToInt32(UAN_DT.Rows[0]["AutoId"]);
                                    uANDetails.PersonalRowId = Convert.ToInt32(UAN_DT.Rows[0]["PersonalRowId"]);
                                    uANDetails.CompanyRefNo = Convert.ToString(UAN_DT.Rows[0]["CompanyRefNo"]);
                                    uANDetails.UanNo = Convert.ToString(UAN_DT.Rows[0]["uan"]);
                                    uANDetails.name = Convert.ToString(UAN_DT.Rows[0]["name"]);
                                    uANDetails.date_of_birth = Convert.ToString(UAN_DT.Rows[0]["date_of_birth"]);
                                    uANDetails.mobile = Convert.ToString(UAN_DT.Rows[0]["mobile"]);
                                    uANDetails.client_ref_num = Convert.ToString(UAN_DT.Rows[0]["client_ref_num"]);
                                    uANDetails.request_id = Convert.ToString(UAN_DT.Rows[0]["request_id"]);
                                    uANDetails.gender = Convert.ToString(UAN_DT.Rows[0]["gender"]);
                                    uANDetails.aadhaar_verification_status = Convert.ToString(UAN_DT.Rows[0]["aadhaar_verification_status"]);
                                    uANDetails.is_employed = Convert.ToInt32(UAN_DT.Rows[0]["is_employed"]);
                                    uANDetails.relation = Convert.ToString(UAN_DT.Rows[0]["relation"]);
                                    uANDetails.relative_name = Convert.ToString(UAN_DT.Rows[0]["relative_name"]);

                                    lst = (from DataRow dr in UAN_DT.Rows
                                           select new EmploymentHistory()
                                           {
                                               establishment_name = Convert.ToString(dr["establishment_name"]),
                                               establishment_id = Convert.ToString(dr["establishment_id"]),
                                               member_id = Convert.ToString(dr["member_id"]),
                                               date_of_joining = Convert.ToString(dr["date_of_joining"]),
                                               date_of_exit = Convert.ToString(dr["date_of_exit"]),
                                               leave_reason = Convert.ToString(dr["leave_reason"]),
                                               employment_period_in_months = Convert.ToString(dr["employment_period_in_months"]),
                                               employer_confidence_score = Convert.ToString(dr["employer_confidence_score"]),
                                           }).ToList();
                                    uANDetails.EmploymentHistory = lst;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {

                throw;
            }
           

            return uANDetails;
        }
        public ViotalUANRoot uan_Detailv2(string CaseId, string UANNo, string MapPath, string APIPath)
        {
            ViotalUANRoot model = new ViotalUANRoot();
            string result = "";
            try
            {
                string url = "https://api.swiftverify.digital";

                using (var client = new HttpClient())
                using (var form = new MultipartFormDataContent())
                {
                    // Add headers
                    client.DefaultRequestHeaders.Add("X_SWIFTCX", "MTM5");
                    client.DefaultRequestHeaders.Add("X_SWIFTKEY", "09026456569e67984915960d7f9cacd21c0c135ffc26e83c27599e1ab65aacfc");

                    // Add form fields
                    form.Add(new StringContent("uan_detail"), "type");
                    form.Add(new StringContent(UANNo), "num");

                    // Send POST request synchronously
                    HttpResponseMessage response = client.PostAsync(url, form).Result;
                    result = response.Content.ReadAsStringAsync().Result;
                    clsCommonMethods1.APILogLog(APIPath, result.ToString(), "https://api.swiftverify.digital", CaseId);
                    ViotalUANRoot myDeserializedClass = JsonConvert.DeserializeObject<ViotalUANRoot>(result);
                    model = myDeserializedClass;
                }

            }
            catch (Exception ex)
            {
                clsCommonMethods1.ErrorLog(MapPath, "input:" + UANNo + "Output:" + result, "uan_Detailv2");
            }
            return model;
        }

        public string AddUANCheckDetails_Viotal(int PersonalRowId,string CompanyRefNo, string UANno, int ClientRowId, int SubCheckId, int CreatedBy = 0,string ErrorPath="",string APILogPath="",string UANArtPath="")
        {
            DataBaseConnections DC = new DataBaseConnections();
            ClientCustomize CC = new ClientCustomize();
            ViotalUANRoot uan_Detail = new ViotalUANRoot();
            string body = "";
            using (StreamReader reader = new StreamReader(UANArtPath))
            {
                body = reader.ReadToEnd();
            }
            try
            {
                var DataObj = uan_Detailv2(UANno, UANno, ErrorPath, APILogPath);
                if (DataObj.status == 200)
                {
                    var AutoId = DC.InsertUpdateViotalUANVerification(DataObj, CompanyRefNo, 0, UANno, SubCheckId);
                    var UAN_DT = DC.GetViotalUANVerification(AutoId, PersonalRowId, "", UANno, SubCheckId);
                    if (UAN_DT.Rows.Count > 0)
                    {
                        string name = Convert.ToString(DataObj.data.result[0].name);
                        string guardian_name = Convert.ToString(DataObj.data.result[0].fatherOrHusbandName);
                        DataTable dt = new DataTable();
                        dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckId, "Operation", "Check");
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                int rst = CC.InsertUpdateUANCheckDetails(PersonalRowId, SubCheckId, Antecedents, ClientRowId, UANno, name, guardian_name);
                            }
                        }
                        if (DataObj != null)
                        {
                            var uanDetailWrapper = DataObj.data.additionalDetails.uanDetails[0];                            
                             var firstUanDetail = uanDetailWrapper[UANno];                           
                             var basicDetails = firstUanDetail.basicDetails;                           
                            string data = "";

                            foreach (var item in DataObj.data.result)
                            {
                                string date_of_joining = "";
                                string date_of_exit = "";
                                if (Convert.ToString(item.dateOfJoining) != "")
                                {
                                    DateTime doj = Convert.ToDateTime(item.dateOfJoining);
                                    date_of_joining = doj.ToString("dd-MMM-yyyy");
                                }
                                if (Convert.ToString(item.dateOfExit) != "")
                                {
                                    DateTime doe = Convert.ToDateTime(item.dateOfExit);
                                    date_of_exit = doe.ToString("dd-MMM-yyyy");
                                }
                                data += "<tr>";
                                data += "<td> " + Convert.ToString(item.establishmentName) + " (" + Convert.ToString(item.establishmentId) + ")" + " </td>";
                                data += "<td> " + date_of_joining + " </td>";
                                data += "<td> " + date_of_exit + " </td>";
                                data += "</tr>";
                            }
                            body = body.Replace("{companyRefNo}", CompanyRefNo);
                            body = body.Replace("{name}", DataObj.data.result[0].name);
                            body = body.Replace("{guardian_name}", DataObj.data.result[0].fatherOrHusbandName);
                            body = body.Replace("{uanNumberId}", UANno);
                            body = body.Replace("{Gender}", basicDetails.gender);
                            body = body.Replace("{MobileNo}", basicDetails.mobile);
                            body = body.Replace("{date_of_birth}", basicDetails.dateOfBirth);
                            body = body.Replace("{EstablishedDetails}", data);
                            body = body.Replace("{verifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));

                            return body;                          
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(ErrorPath, Ex.Message, Ex.StackTrace);
                return "";
            }
            return body;
        }


        public bool ViotalCIBIL(int Pid, int Sid, string RefId, string name, string pan, string mobile, string path,string FilePath,string Gender,string dob)
        {
            bool status = false;
            ViotalCIBIL_Root model = new ViotalCIBIL_Root();
            ViotalCIBILRootobject Model = new ViotalCIBILRootobject();
            try
            {
                DataBaseConnections CE = new DataBaseConnections();
                ClientCustomize cc = new ClientCustomize();
                var JosnResult = CheckCIBIL(RefId, name, pan, mobile, path, Gender,dob);
                if (JosnResult != "")
                {
                    Model = JsonConvert.DeserializeObject<ViotalCIBILRootobject>(JosnResult);
                    if (Model.success)
                    {
                        Thread.Sleep(5000);
                        string FinalJson = CIBILjson(Model.data.check_id.ToString(), path);
                        model = JsonConvert.DeserializeObject<ViotalCIBIL_Root>(FinalJson);
                        string CibilCore = model?.data?.result?.bureau_score ?? "";
                        CE.AddViotalCibilDetails(Pid, Sid, RefId, pan, name, mobile, CibilCore, JosnResult, FinalJson);
                        
                        try
                        {
                            var fileBytes = DownloadCRCReport(Model.data.check_id.ToString());

                            if (fileBytes != null && fileBytes.Length > 0)
                            {
                                File.WriteAllBytes(FilePath, fileBytes);

                                DataTable dt = cc.Antecedents_client_select_form(Pid, Sid, "Operation", "Check");
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        int antecedents = Convert.ToInt32(dr["AntecedentsRowID"]);
                                        cc.InsertUpdateCibilCheckDetails(Pid, Sid, antecedents, CibilCore);
                                    }
                                }
                                return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            clsCommonMethods1.ErrorLog(path, ex.ToString(), ex.StackTrace);
                        }

                    }
                    else
                    {
                        CE.AddViotalCibilDetails(Pid, Sid, RefId, pan, name, mobile, "", JosnResult, "");
                        return false;

                    }
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(path, Ex.Message, Ex.StackTrace);

            }
            return status;
        }

        public byte[] DownloadCRCReport(string report)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string url = "https://vio-api.viotalinformatics.com/api/external/v1/checks/" + report + "/report";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", "vt_04a19706c00e8fb0d12ed7611a1f32ea128ffd409c913c40d335f2e211eb6110");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                byte[] fileBytes = response.Content.ReadAsByteArrayAsync().Result;

                return fileBytes;
            }
        }

        public string CIBILjson(string check_id, string path)
        {
            string result = "";
            try
            {
                string url = "https://vio-api.viotalinformatics.com/api/external/v1/checks/"+ check_id + "";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-API-Key", "vt_04a19706c00e8fb0d12ed7611a1f32ea128ffd409c913c40d335f2e211eb6110");
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    result = response.Content.ReadAsStringAsync().Result;
                }

            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(path, Ex.Message, Ex.StackTrace);

            }
            return result;
        }

        public string CheckCIBIL(string RefID, string name, string pan, string mobile, string path, string gender, string dob)
        {
            string result = "";
            try
            {
                string url = "https://vio-api.viotalinformatics.com/api/external/v1/checks/cbr_v4_llm";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-API-Key", "vt_04a19706c00e8fb0d12ed7611a1f32ea128ffd409c913c40d335f2e211eb6110");

                    var payload = new
                    {
                        input = new
                        {
                            name = name,
                            pan = pan,
                            mobile = mobile,
                            dob = dob,
                            gender = gender
                        },
                        reference_id = RefID
                    };

                    string json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(url, content).GetAwaiter().GetResult();
                    result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(path, Ex.Message, Ex.StackTrace);
            }
            return result;

        }


        private static readonly string svcCx = "MTM5";
        private static readonly string svcKey = "09026456569e67984915960d7f9cacd21c0c135ffc26e83c27599e1ab65aacfc";
        public string UANCheckDetails_llmTech(int PersonalRowId, int SubCheckRowId, int ClientRowId, string CompanyRefNo, string UanNo, string logPath, string ArtifactPath)
        {
            string body = "";
            try
            {
                llmtechUANRoot model = new llmtechUANRoot();
                string JsonString = InstantcheckObj.Getepfo(UanNo, logPath);
                DataBaseConnections con = new DataBaseConnections();
                model = JsonConvert.DeserializeObject<llmtechUANRoot>(JsonString);
                if (model.success)
                {
                    int AutoId = con.InsertUpdatellmtechUANDetails(PersonalRowId, UanNo, CompanyRefNo, SubCheckRowId, JsonString, 0);                   
                    if (AutoId > 0)
                    {
                        body = UpoadUAN_Airtfact(model, PersonalRowId, SubCheckRowId, ClientRowId, CompanyRefNo, UanNo, ArtifactPath);
                        
                        if(body!="")
                        {
                            var uanDetails = model.data.result;
                            string name = uanDetails[0].name;
                            string guardian_name = uanDetails[0].fatherOrHusbandName;

                            DataTable dt = new DataTable();
                            dt = CC.Antecedents_client_select_form(ClientRowId, SubCheckRowId, "Operation", "Check");
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                    CC.InsertUpdateUANCheckDetails(PersonalRowId, SubCheckRowId, Antecedents, ClientRowId, UanNo, name, guardian_name);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(logPath, UanNo + " : " + Ex.Message, "https://apis.llmtech.in/api/v1/services/uan/employment-history-v2");

            }
            return body;
        }
        public string UpoadUAN_Airtfact(llmtechUANRoot model, int PersonalRowID, int SubCheckFamilyID, int ClientRowID, string CompanyRefNo, string UAN, string ArtifactPath)
        {
            string body = string.Empty;

            try
            {
                if (model.success)
                {
                    var uanDetails = model.data.result;
                  
                    string data = "";
                    using (StreamReader reader = new StreamReader(ArtifactPath))
                    {
                        body = reader.ReadToEnd();
                    }
                    foreach (var item in uanDetails)
                    {
                        string date_of_joining = "";
                        string date_of_exit = "";
                        if (Convert.ToString(item.dateOfJoining) != "")
                        {
                            DateTime doj = Convert.ToDateTime(item.dateOfJoining);
                            date_of_joining = doj.ToString("dd-MMM-yyyy");
                        }
                        if (Convert.ToString(item.dateOfExit) != "")
                        {
                            DateTime doe = Convert.ToDateTime(item.dateOfExit);
                            date_of_exit = doe.ToString("dd-MMM-yyyy");
                        }
                        data += "<tr>";
                        data += "<td> " + Convert.ToString(item.establishmentName) + " (" + Convert.ToString(item.establishmentId) + ")" + " </td>";
                        data += "<td> " + date_of_joining + " </td>";
                        data += "<td> " + date_of_exit + " </td>";
                        data += "</tr>";
                    }
                    body = body.Replace("{name}", uanDetails[0].name);
                    body = body.Replace("{guardian_name}", uanDetails[0].fatherOrHusbandName);
                    body = body.Replace("{uanNumberId}", UAN);                  
                    body = body.Replace("{EstablishedDetails}", data);

                    body = body.Replace("{companyRefNo}", CompanyRefNo);
                    body = body.Replace("{VerifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));
                }
            }
            catch (Exception Ex)
            {

            }
            return body;

        }

        public string GetCibilReport(string pan,string name,string mobile,string refNo,string logPath,string filePath)
        {
            try
            {
                string url = "https://apis.llmtech.in/api/v1/services/profiling/cbr-v2";

                var json = $@"{{
            ""pan"": ""{pan}"",
            ""name"": ""{name}"",
            ""mobile"": ""{mobile}"",
            ""consent"": ""Y"",
            ""client_ref_num"": ""{refNo}"",
            ""response_format"": ""pdf""
        }}";
                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("X-API-Key", "llm_846ce3bea8912b0115048944ec711823c9644e0550599b9219d83c7cdeb7938c");
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // Synchronous call
                HttpResponseMessage response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Get PDF bytes
                    byte[] pdfBytes = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();

                    // Save to file
                    System.IO.File.WriteAllBytes(filePath, pdfBytes);
                    clsCommonMethods1.ErrorLog(logPath, json.ToString(), "https://apis.llmtech.in/api/v1/services/profiling/cbr-v2");

                    return "Success";
                }
                else
                {
                    var JsonString = response.Content.ReadAsStringAsync().Result;
                    clsCommonMethods1.ErrorLog(logPath, pan +" "+ refNo + " " + JsonString, "https://apis.llmtech.in/api/v1/services/profiling/cbr-v2");
                   
                    return "";
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(logPath, pan + " : " + Ex.Message, "https://apis.llmtech.in/api/v1/services/profiling/cbr-v2");
            }
            return "";
        }
        
        public CIBILRoot GetCibilReportJson(string pan, string name, string mobile,string refNo, string logPath)
        {
            CIBILRoot myDeserializedClass = new CIBILRoot();
            try
            {
                string apiKey = "llm_846ce3bea8912b0115048944ec711823c9644e0550599b9219d83c7cdeb7938c";
                string url = "https://apis.llmtech.in/api/v1/services/profiling/cbr-v4";
                var json = $@"{{
            ""pan"": ""{pan}"",
            ""name"": ""{name}"",
            ""mobile_no"": ""{mobile}"",
            ""consent"": ""Y"",
            ""client_ref_num"": ""{refNo}""           
        }}";
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Headers.Add("X-API-Key", apiKey);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Synchronous execution (no await)
                    HttpResponseMessage response = client.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string JsonString = response.Content.ReadAsStringAsync().Result;
                        clsCommonMethods1.ErrorLog(logPath, pan + " " + refNo + " " + JsonString, "https://apis.llmtech.in/api/v1/services/profiling/cbr-v4");
                        myDeserializedClass = JsonConvert.DeserializeObject<CIBILRoot>(JsonString);
                    }
                    else
                    {
                        string errorDetails = response.Content.ReadAsStringAsync().Result;
                        myDeserializedClass = JsonConvert.DeserializeObject<CIBILRoot>(errorDetails);
                    }
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(logPath, pan + " " + refNo + " " + Ex.Message.ToString(), "https://apis.llmtech.in/api/v1/services/profiling/cbr-v4");
            }
            return myDeserializedClass;
        }
        public CIBILCheckResult CIBILReportWithScore(int Pid,int Sid,string pan, string name, string mobile,  string refNo, string logPath,string FilePath)
        {
            CIBILCheckResult OutObj = new CIBILCheckResult();
            try
            {
                DataBaseConnections conObj = new DataBaseConnections();
                string apiKey = "llm_846ce3bea8912b0115048944ec711823c9644e0550599b9219d83c7cdeb7938c";
                var Model = GetCibilReportJson(pan, name, mobile, refNo, logPath);
                if(Model !=null)
                {
                    if(Model.success)
                    {
                        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                        string fileName = $"Cibil_{refNo}_{timestamp}.pdf";
                        string fullDocPath = Path.Combine(FilePath, fileName);
                        var Result= DownloadPdf(Model.requestId, apiKey, FilePath+ fileName, logPath);
                        if(Result== "Success")
                        {
                            OutObj.FileName = fileName;
                            OutObj.Status = "Success";
                            OutObj.credit_score = Model.data.bureau_score;
                        }
                        else
                        {
                            OutObj.FileName = "";
                            OutObj.Status = "Failed";
                            OutObj.credit_score = "";
                        }
                    }                   
                }
            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(logPath, pan + " " + refNo + " " + Ex.Message.ToString(), "https://apis.llmtech.in/api/v1/services/profiling/cbr-v2");
            }
            return OutObj;
        }     
        
        public string DownloadPdf(string requestId, string apiKey, string outputFilePath,string logPath)
        {
            string url = $"https://apis.llmtech.in/api/v1/services/response/{requestId}/download?format=pdf";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-API-Key", apiKey);

            // Call async methods synchronously
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {               
                byte[] pdfBytes = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                // Save to file
                System.IO.File.WriteAllBytes(outputFilePath, pdfBytes);
                return "Success";
            }
            else
            {
                var JsonString = response.Content.ReadAsStringAsync().Result;
                clsCommonMethods1.ErrorLog(logPath, requestId+": "+JsonString, "https://apis.llmtech.in/api/v1/services/profiling/cbr-v2");

                return "";
            }
        }
     
        public string PanDetailsfromLlmtechAPI(string pan, string refNo, string logPath)
        {
            try
            {
                var url = "https://apis.llmtech.in/api/v1/services/profiling/pan-details-lite";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers["X-API-Key"] = "llm_846ce3bea8912b0115048944ec711823c9644e0550599b9219d83c7cdeb7938c";
                string jsonData = $"{{\"client_ref_num\":\"{refNo}\",\"pan\":\"{pan}\",\"consent\":\"Y\"}}";
                byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);

                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(dataBytes, 0, dataBytes.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string JsonString = reader.ReadToEnd();
                    clsCommonMethods1.ErrorLog(logPath, pan + " " + refNo + " " + JsonString, "https://apis.llmtech.in/api/v1/services/profiling/pan-details-lite");
                    return JsonString;
                }
            }
            catch (WebException ex)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    string errorText = reader.ReadToEnd();
                    clsCommonMethods1.ErrorLog(logPath, pan + " " + refNo + " " + errorText, "https://apis.llmtech.in/api/v1/services/profiling/pan-details-lite");
                }
                return "";
            }
            catch (Exception ex)
            {
                clsCommonMethods1.ErrorLog(logPath, pan + " " + refNo + " " + ex.Message, "https://apis.llmtech.in/api/v1/services/profiling/pan-details-lite");

                return "";
            }
        }
        public llmtechPANRoot GetLlmtechPAN(int Pid, int Sid, string PanNo, string RefNo, string LogPath)
        {
            llmtechPANRoot model = new llmtechPANRoot();           
            DataBaseConnections DC = new DataBaseConnections();
            try
            {
                var JsonString = PanDetailsfromLlmtechAPI(PanNo, RefNo, LogPath);
                model = JsonConvert.DeserializeObject<llmtechPANRoot>(JsonString);
                if (model.success)  
                {
                    DC.AddLlmtechPanDetails(Pid, Sid, RefNo, PanNo, JsonString, 200);
                    return model;
                }
            }
            catch (Exception Ex)
            {

                clsCommonMethods1.ErrorLog(LogPath, Ex.Message, Ex.StackTrace);

            }
            return model;
        }
        public string PAN_DIN(string CheckId, string PAN, string Path)
        {
            string JsonString = "";
            try
            {
                var url = "https://svc.digitap.ai/validation/kyb/v1/pan_to_din";
                var client = new HttpClient();

                // Set headers
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", "MTU3ODE3ODU6TnhpUmI2ZG0wWVFHZzlIOGxIbzZIbTFnZ2NiYjZOVTQ=");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Prepare request body              
                var requestBody = new
                {
                    client_ref_num = CheckId,
                    pan = PAN
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    // Send POST request synchronously
                    var response = client.PostAsync(url, content).Result;
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    clsCommonMethods1.ErrorLog(Path, responseBody, url);
                    if (response.IsSuccessStatusCode)
                    {
                        JsonString = responseBody;
                    }
                }
                catch (Exception ex)
                {
                    clsCommonMethods1.ErrorLog(Path, ex.Message, url);
                }

            }
            catch (Exception ex)
            {
                clsCommonMethods1.ErrorLog(Path, ex.Message, CheckId + " " + PAN);
                return JsonString;
            }
            return JsonString;
        }

        public string GetPanToDin(int PId, int ClientRowId, int Sid, string CheckId, string PAN, string Path,string APIPath)
        {
            string body = string.Empty;
            PAN_To_DINRootobject model = new PAN_To_DINRootobject();
            try
            {
                DataBaseConnections DC = new DataBaseConnections();
                string JsonString = PAN_DIN(CheckId, PAN, APIPath);
                model = JsonConvert.DeserializeObject<PAN_To_DINRootobject>(JsonString);
                DC.InsertUpdateDigiTapPAN_To_DIN(PId, PAN, CheckId, JsonString, 0);
                if (model.http_response_code == 200)
                {
                    using (StreamReader reader = new StreamReader(Path))
                    {
                        body = reader.ReadToEnd();
                    }
                    if (model.result_code == 101)
                    {                         
                        string Din = model.result.din;

                        DataTable dt = new DataTable();
                        dt = CC.Antecedents_client_select_form(ClientRowId, Sid, "Operation", "Check");
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                int AutoId = DC.InsertUpdateDINCheckDetails(PId, Sid, Antecedents, Din);
                            }
                        }

                        body = body.Replace("{companyRefNo}", CheckId);
                        body = body.Replace("{PanNo}", PAN);
                        body = body.Replace("{DINCaption}", "DIN");
                        body = body.Replace("{DIN}", model.result.din);
                        body = body.Replace("{verifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));

                        return body;
                    }
                    else if (model.result_code == 103)
                    {
                        DataTable dt = new DataTable();
                        dt = CC.Antecedents_client_select_form(ClientRowId, Sid, "Operation", "Check");
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                int AutoId = DC.InsertUpdateDINCheckDetails(PId, Sid, Antecedents, "");
                            }
                        }

                        body = body.Replace("{companyRefNo}", CheckId);
                        body = body.Replace("{PanNo}", PAN);
                        body = body.Replace("{DINCaption}", "Message");
                        body = body.Replace("{DIN}", model.message);
                        body = body.Replace("{verifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));
                        return body;
                    }
                }

            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(APIPath, Ex.Message, Ex.StackTrace);
            }
            return body;
        }



        public string getPanToGst(int PId, int Sid, string CheckId, string PAN, string Path, string APIPath)
        {
            string body = "";
            try
            {
                DataBaseConnections DC = new DataBaseConnections();
                gstRoot model = new gstRoot();
                string JsonString = PAN_To_Gst(CheckId, PAN, APIPath);
                model = JsonConvert.DeserializeObject<gstRoot>(JsonString);
                DC.AddDigiTapPanToGst(PId, Sid, CheckId, PAN, JsonString, model.result_code);
                if (model.result_code == 101)
                {

                    using (StreamReader reader = new StreamReader(Path))
                    {
                        body = reader.ReadToEnd();
                    }

                    StringBuilder html = new StringBuilder();

                    html.Append("<tr>");
                    html.Append("<td>Company Ref No</td>");
                    html.Append("<td colspan='2'>" + CheckId + "</td>");
                    html.Append("</tr>");

                    html.Append("<tr>");
                    html.Append("<td>Pan Number</td>");
                    html.Append("<td colspan='2'>" + PAN + "</td>");
                    html.Append("</tr>");

                    html.Append("<tr>");
                    html.Append("<td colspan='3' style='background:#031f30;color:white;text-align:center;'>GST Details</td>");
                    html.Append("</tr>");

                    if (model.result != null)
                    {
                        foreach (var item in model.result)
                        {
                            html.Append("<tr><td colspan='3'>");

                            html.Append("<table border='1' style='width:100%;border-collapse:collapse;'>");
                            html.Append("<tr><th>Type</th><th>Values</th></tr>");

                            html.Append("<tr><td>GSTIN</td><td>" + item.taxpayerDetails.gstin + "</td></tr>");
                            html.Append("<tr><td>Legal Name</td><td>" + item.taxpayerDetails.lgnm + "</td></tr>");
                            html.Append("<tr><td>State Jurisdiction</td><td>" + item.taxpayerDetails.stj + "</td></tr>");
                            html.Append("<tr><td>Taxpayer Type</td><td>" + item.taxpayerDetails.dty + "</td></tr>");
                            html.Append("<tr><td>Cancellation Date</td><td>" + item.taxpayerDetails.cxdt + "</td></tr>");
                            html.Append("<tr><td>Registration Date</td><td>" + item.taxpayerDetails.rgdt + "</td></tr>");
                            html.Append("<tr><td>Constitution of Business</td><td>" + item.taxpayerDetails.ctb + "</td></tr>");
                            html.Append("<tr><td>Principal Place of Business Address</td><td>" + item.taxpayerDetails.pradr.adr + "</td></tr>");
                            html.Append("<tr><td>GST Status</td><td>" + item.taxpayerDetails.sts + "</td></tr>");
                            html.Append("<tr><td>Trade Name / Business Name</td><td>" + item.taxpayerDetails.tradeNam + "</td></tr>");
                            html.Append("<tr><td>Field Visit Conducted</td><td>" + item.taxpayerDetails.isFieldVisitConducted + "</td></tr>");
                            html.Append("<tr><td>Central Tax Jurisdiction</td><td>" + item.taxpayerDetails.ctj + "</td></tr>");
                            html.Append("<tr><td>e-Invoice Status</td><td>" + item.taxpayerDetails.einvoiceStatus + "</td></tr>");

                            if (item.taxpayerReturnDetails != null &&
                                item.taxpayerReturnDetails.filingStatus != null)
                            {
                                html.Append("<tr><td colspan='2'>");

                                html.Append("<table border='1' style='width:100%;border-collapse:collapse;'>");
                                html.Append("<tr>");
                                html.Append("<th colspan='7' style='background:#031f30;color:white;'>Tax Payer Return Details</th>");
                                html.Append("</tr>");

                                html.Append("<tr>");
                                html.Append("<th>Financial Year</th>");
                                html.Append("<th>Tax Period</th>");
                                html.Append("<th>Mode of Filing</th>");
                                html.Append("<th>Date of Filing</th>");
                                html.Append("<th>Return Type</th>");
                                html.Append("<th>ARN</th>");
                                html.Append("<th>Return Filing Status</th>");
                                html.Append("</tr>");

                                foreach (var filingStatus in item.taxpayerReturnDetails.filingStatus)
                                {
                                    html.Append("<tr>");
                                    html.Append("<td>" + filingStatus.fy + "</td>");
                                    html.Append("<td>" + filingStatus.taxp + "</td>");
                                    html.Append("<td>" + filingStatus.mof + "</td>");
                                    html.Append("<td>" + filingStatus.dof + "</td>");
                                    html.Append("<td>" + filingStatus.rtntype + "</td>");
                                    html.Append("<td>" + filingStatus.arn + "</td>");
                                    html.Append("<td>" + filingStatus.status + "</td>");
                                    html.Append("</tr>");
                                }

                                html.Append("</table>");
                                html.Append("</td></tr>");
                            }

                            html.Append("</table>");
                            html.Append("</td></tr>");
                        }
                    }

                    body = body.Replace("{HtmlData}", html.ToString());
                    body = body.Replace("{verifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));
                    return body;



                }
                else
                {
                    using (StreamReader reader = new StreamReader(Path))
                    {
                        body = reader.ReadToEnd();
                    }

                    StringBuilder html = new StringBuilder();

                    html.Append("<tr>");
                    html.Append("<td>Company Ref No</td>");
                    html.Append("<td colspan='2'>" + CheckId + "</td>");
                    html.Append("</tr>");

                    html.Append("<tr>");
                    html.Append("<td>Pan Number</td>");
                    html.Append("<td colspan='2'>" + PAN + "</td>");
                    html.Append("</tr>");

                    html.Append("<tr>");
                    html.Append("<td colspan='3' style='background:#031f30;color:white;text-align:center;'>GST Details</td>");
                    html.Append("</tr>");

                    html.Append("<tr>");
                    html.Append("<td>Message</td>");
                    html.Append("<td colspan='2'>No Record Found</td>");
                    html.Append("</tr>");

                    body = body.Replace("{HtmlData}", html.ToString());
                    body = body.Replace("{verifiedDate}", DateTime.Now.Date.ToString("dd-MMM-yyyy"));
                    return body;
                }



            }
            catch (Exception Ex)
            {
                clsCommonMethods1.ErrorLog(APIPath, Ex.Message, Ex.StackTrace);
            }

            return "";
        }
       public string PAN_To_Gst(string CheckId, string PAN, string Path)
        {
            string JsonString = "";
            try
            {
                var url = "https://svc.digitap.ai/validation/kyb/v1/pan_to_gst_details";
                var client = new HttpClient();

                // Set headers
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", "MTU3ODE3ODU6TnhpUmI2ZG0wWVFHZzlIOGxIbzZIbTFnZ2NiYjZOVTQ=");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Prepare request body              
                var requestBody = new
                {
                    client_ref_num = CheckId.Trim(),
                    pan = PAN.Trim()
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    // Send POST request synchronously
                    var response = client.PostAsync(url, content).Result;
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    clsCommonMethods1.ErrorLog(Path, responseBody, url);
                    if (response.IsSuccessStatusCode)
                    {
                        JsonString = responseBody;
                    }
                }
                catch (Exception ex)
                {
                    clsCommonMethods1.ErrorLog(Path, ex.Message, url);
                }

            }
            catch (Exception ex)
            {
                clsCommonMethods1.ErrorLog(Path, ex.Message, CheckId + " " + PAN);
                return JsonString;
            }
            return JsonString;
        }

        public string AadharDetailsfromLlmtechAPI(string Aadhar, string refNo, string logPath)
        {
            try
            {
                var url = "https://apis.llmtech.in/api/v1/services/kyc/aadhaar-validation";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers["X-API-Key"] = "llm_846ce3bea8912b0115048944ec711823c9644e0550599b9219d83c7cdeb7938c";
                string jsonData = $"{{\"client_ref_num\":\"{refNo}\",\"aadhaar_number\":\"{Aadhar}\",\"consent\":\"Y\"}}";

                byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);

                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(dataBytes, 0, dataBytes.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string JsonString = reader.ReadToEnd();
                    clsCommonMethods1.ErrorLog(logPath, Aadhar + " " + refNo + " " + JsonString, "https://apis.llmtech.in/api/v1/services/kyc/aadhaar-validation");
                    return JsonString;
                }
            }
            catch (WebException ex)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    string errorText = reader.ReadToEnd();
                    clsCommonMethods1.ErrorLog(logPath, Aadhar + " " + refNo + " " + errorText, "https://apis.llmtech.in/api/v1/services/kyc/aadhaar-validation");
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public AadharRootobject GetLlmtechAadhar(int Pid, int Sid, string Aadhar, string RefNo, string LogPath)
        {
            AadharRootobject model = new AadharRootobject();
            DataBaseConnections CE = new DataBaseConnections();
            try
            {
                var JsonString = AadharDetailsfromLlmtechAPI(Aadhar, RefNo, LogPath);
                model = JsonConvert.DeserializeObject<AadharRootobject>(JsonString);

                if (model.success)
                {
                    CE.AddLlmtechAadharDetails(Pid, Sid, RefNo, Aadhar, JsonString, 200);
                    return model;
                }
            }
            catch (Exception Ex)
            {

                clsCommonMethods1.ErrorLog(LogPath, Ex.Message, Ex.StackTrace);

            }
            return model;
        }

        public string CoinCircleTrustCRC(string CaseId, string candidateName,string fatherName, string dateOfBirth,string address,string Path)
        {
            string responseString = string.Empty;
            try
            {
                var requestBody = new
                {
                    candidateName = candidateName,
                    fatherName = fatherName,
                    dateOfBirth = dateOfBirth,
                    lookupYears = 5,
                    lookupScope = "district_and_state_wide",
                    locations = new[]
                    {
                        new
                        {                           
                            address = address
                        }
                    },
                    requestId = CaseId
                };

                string requestJson = JsonConvert.SerializeObject(requestBody);

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(2);
                    client.DefaultRequestHeaders.Add("x-api-key", "bab937ff39df8619790b69fa2af595b84bd8750f1b3b49e368b1a13e7a970fa5");
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    // Request Log
                    HttpResponseMessage response = client.PostAsync("https://apis.coincircletrust.com/api/v1/court-case-search", content).Result;
                    responseString = response.Content.ReadAsStringAsync().Result;
                    // Response Log
                    if (!response.IsSuccessStatusCode)
                    {
                        var ErrorResponce = $"Error | StatusCode:{(int)response.StatusCode} | Response:{responseString}";

                        clsCommonMethods1.ErrorLog(Path, ErrorResponce, candidateName);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                responseString = "HTTP Error : " + ex.Message;
                clsCommonMethods1.ErrorLog(Path, responseString, candidateName);
            }
            catch (TaskCanceledException ex)
            {
                responseString = "Timeout Error : " + ex.Message;
                clsCommonMethods1.ErrorLog(Path, responseString, candidateName);

            }
            catch (Exception ex)
            {
                responseString = "Exception : " + ex.Message;
                clsCommonMethods1.ErrorLog(Path, responseString, candidateName);
            }
            return responseString;
        }





        public string GetCourtCaseStatus(string RequestId)
        {
            string url = "https://apis.coincircletrust.com/api/v1/court-case-search/requests/" + RequestId + "";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", "bab937ff39df8619790b69fa2af595b84bd8750f1b3b49e368b1a13e7a970fa5");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string JsonString = response.Content.ReadAsStringAsync().Result;

                    return JsonString;

                }
                return "";
            }
        }

        public byte[] GetReportBytes(string requestId)
        {
            string url = $"https://apis.coincircletrust.com/api/v1/court-case-search/requests/{requestId}/Report";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(
                "x-api-key",
                "bab937ff39df8619790b69fa2af595b84bd8750f1b3b49e368b1a13e7a970fa5");

                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsByteArrayAsync().Result;
                }

                return null;
            }
        }
        public stateRootobject GetState()
        {
            stateRootobject state = new stateRootobject();
            try
            {
                string url = "https://apis.coincircletrust.com/court-case-search/states";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("x-api-key", "bab937ff39df8619790b69fa2af595b84bd8750f1b3b49e368b1a13e7a970fa5");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseString = reader.ReadToEnd();

                        state = JsonConvert.DeserializeObject<stateRootobject>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                state = new stateRootobject
                {
                    success = false,
                    message = ex.Message
                };
            }

            return state;
        }
        public districtRootobject Getdistrict(string stateName)
        {
            districtRootobject district = new districtRootobject();

            try
            {
                string url = "https://apis.coincircletrust.com/court-case-search/states/"
                             + Uri.EscapeDataString(stateName)
                             + "/districts";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";

                request.Headers.Add(
                    "x-api-key",
                    "bab937ff39df8619790b69fa2af595b84bd8750f1b3b49e368b1a13e7a970fa5"
                );

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseString = reader.ReadToEnd();

                        district = JsonConvert.DeserializeObject<districtRootobject>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                district = new districtRootobject
                {
                    success = false,
                    message = ex.Message
                };
            }

            return district;
        }

        public class Cryptography
        {
            public static class JsonWebToken
            {
                private static Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms;

                static JsonWebToken()
                {
                    HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>>
            {
                { JwtHashAlgorithm.HS256, (key, value) => { using (var sha = new HMACSHA256(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.HS384, (key, value) => { using (var sha = new HMACSHA384(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.HS512, (key, value) => { using (var sha = new HMACSHA512(key)) { return sha.ComputeHash(value); } } }
            };
                }

                public static string Encode(string payload, string key, JwtHashAlgorithm algorithm)
                {
                    return Encode(payload, Encoding.UTF8.GetBytes(key), algorithm);
                }

                public static string Encode(string payload, byte[] keyBytes, JwtHashAlgorithm algorithm)
                {
                    var segments = new List<string>();
                    var header = new { alg = algorithm.ToString(), typ = "JWT" };

                    byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
                    byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

                    segments.Add(Base64UrlEncode(headerBytes));
                    segments.Add(Base64UrlEncode(payloadBytes));

                    var stringToSign = string.Join(".", segments.ToArray());

                    var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

                    byte[] signature = HashAlgorithms[algorithm](keyBytes, bytesToSign);
                    segments.Add(Base64UrlEncode(signature));

                    return string.Join(".", segments.ToArray());
                }

                public static string Decode(string token, string key)
                {
                    return Decode(token, key, true);
                }

                public static string Decode(string token, string key, bool verify)
                {
                    var parts = token.Split('.');
                    var header = parts[0];
                    var payload = parts[1];
                    byte[] crypto = Base64UrlDecode(parts[2]);

                    var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
                    var headerData = JObject.Parse(headerJson);
                    var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
                    //var payloadData = JObject.Parse(payloadJson);

                    if (verify)
                    {
                        var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
                        var keyBytes = Encoding.UTF8.GetBytes(key);
                        var algorithm = (string)headerData["alg"];

                        var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](keyBytes, bytesToSign);
                        var decodedCrypto = Convert.ToBase64String(crypto);
                        var decodedSignature = Convert.ToBase64String(signature);

                        if (decodedCrypto != decodedSignature)
                        {
                            throw new ApplicationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
                        }
                    }

                    return payloadJson.ToString();
                }

                private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
                {
                    switch (algorithm)
                    {
                        case "HS256": return JwtHashAlgorithm.HS256;
                        case "HS384": return JwtHashAlgorithm.HS384;
                        case "HS512": return JwtHashAlgorithm.HS512;
                        default: throw new InvalidOperationException("Algorithm not supported.");
                    }
                }

                // from JWT spec
                private static string Base64UrlEncode(byte[] input)
                {
                    var output = Convert.ToBase64String(input);
                    output = output.Split('=')[0]; // Remove any trailing '='s
                    output = output.Replace('+', '-'); // 62nd char of encoding
                    output = output.Replace('/', '_'); // 63rd char of encoding
                    return output;
                }

                // from JWT spec
                private static byte[] Base64UrlDecode(string input)
                {
                    var output = input;
                    output = output.Replace('-', '+'); // 62nd char of encoding
                    output = output.Replace('_', '/'); // 63rd char of encoding
                    switch (output.Length % 4) // Pad with trailing '='s
                    {
                        case 0: break; // No pad chars in this case
                        case 2: output += "=="; break; // Two pad chars
                        case 3: output += "="; break; // One pad char
                        default: throw new System.Exception("Illegal base64url string!");
                    }
                    var converted = Convert.FromBase64String(output); // Standard base64 decoder
                    return converted;
                }


            }
            public enum JwtHashAlgorithm
            {
                HS256,
                HS384,
                HS512
            }
            public class wJwTJson
            {
                public String timestamp { get; set; }
                public String partnerId { get; set; }
                public String reqid { get; set; }
            }

        }

    }
}
