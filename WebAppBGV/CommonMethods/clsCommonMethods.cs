using BAL;
using BAL.ProvidedInfoRepository;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using ViewModels;
using ViewModels.ProvidedInfoViewModel;
using System.Text.RegularExpressions;

namespace WebAppBGV.CommonMethods
{
    public class clsCommonMethods
    {
        #region Encryption and Decryption Implementation


       // static string key { get; set; } = "Sec@123";
        public static string PEncrypt1(string text)
        {
            string key = "Sec@123";
            string EnCrt = "";
            try
            {

                using (var md5 = new MD5CryptoServiceProvider())
                {
                    using (var tdes = new TripleDESCryptoServiceProvider())
                    {
                        tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                        tdes.Mode = CipherMode.ECB;
                        tdes.Padding = PaddingMode.PKCS7;
                                               
                        using (var transform = tdes.CreateEncryptor())
                        {
                            byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
                            byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                            EnCrt=Convert.ToBase64String(bytes, 0, bytes.Length);
                            //return EnCrt;
                            byte[] _encodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(EnCrt);
                            string _encryptedString = System.Convert.ToBase64String(_encodeAsBytes);
                            //return EnCrt.Replace("/", "$$$").Replace("+", "^^^");
                            return _encryptedString;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string PDecrypt1(string cipher)
        {
            try
            {
                string key = "Sec@123";
                //cipher = cipher.Replace("$$$","/").Replace("^^^","+");
                byte[] _encodedAsBytes = System.Convert.FromBase64String(cipher);
                cipher = System.Text.ASCIIEncoding.ASCII.GetString(_encodedAsBytes);
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    using (var tdes = new TripleDESCryptoServiceProvider())
                    {
                        tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                        tdes.Mode = CipherMode.ECB;
                        tdes.Padding = PaddingMode.PKCS7;

                        using (var transform = tdes.CreateDecryptor())
                        {
                            byte[] cipherBytes = Convert.FromBase64String(cipher);
                            byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                            return UTF8Encoding.UTF8.GetString(bytes);
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        public static string PEncrypt(string strText)
        {
            string strEncrKey;
            strEncrKey = "&%#@?,:*";
            if(strText!="")
            {
                byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            try
            {
                byte[] bykey = System.Text.Encoding.UTF8.GetBytes(strEncrKey);
                byte[] InputByteArray = System.Text.Encoding.UTF8.GetBytes(strText);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(bykey, IV), CryptoStreamMode.Write);
                cs.Write(InputByteArray, 0, InputByteArray.Length);
                cs.FlushFinalBlock();
                string plainText = Convert.ToBase64String(ms.ToArray());
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);                
            }
            catch (Exception ex)
            {
                return "";
            }
        }
            else
            {
                return "";
            }

        }

        public static string PDecrypt(string strText)
        {
            if(strText!="")
            {
                var base64EncodedBytes = System.Convert.FromBase64String(strText);
                strText = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                string sDecrKey;
                sDecrKey = "&%#@?,:*";

                byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
                byte[] inputByteArray = new byte[strText.Length];
                try
                {
                    byte[] byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey);
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    inputByteArray = Convert.FromBase64String(strText);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                    return encoding.GetString(ms.ToArray());
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

            }

        //public static (byte[] Key, byte[] IV) GenerateKeyAndIV()
        //{
        //     var aes = Aes.Create();
        //    aes.KeySize = 256;
        //    aes.GenerateKey();
        //    aes.GenerateIV();
        //    return (aes.Key, aes.IV);
        //}

        public static string PasswordEncrypt(string plainText )
        {
            byte[] key = Convert.FromBase64String("H/+bkiwjZDMxExCZgo7UWgm2L4FymrvKYDSkbe/Xb60=");
            byte[] iv = Convert.FromBase64String("Ew/0Rfn6y/EABGg8E939ow==");

            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

             var encryptor = aes.CreateEncryptor();
             var ms = new MemoryStream();
             var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
             var writer = new StreamWriter(cs, Encoding.UTF8);

            writer.Write(plainText);
            writer.Close(); // flush

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string PasswordDecrypt(string cipherText)
        {
            try
            {
                byte[] key = Convert.FromBase64String("H/+bkiwjZDMxExCZgo7UWgm2L4FymrvKYDSkbe/Xb60=");
                byte[] iv = Convert.FromBase64String("Ew/0Rfn6y/EABGg8E939ow==");

                var cipherBytes = Convert.FromBase64String(cipherText);

                var aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                var decryptor = aes.CreateDecryptor();
                var ms = new MemoryStream(cipherBytes);
                var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                var reader = new StreamReader(cs, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (Exception ex )
            {
                return "";
            }             
        }

        
        public static string PWDEncrypt(string strText)
        {
            string strEncrKey;
            strEncrKey = "&%#@?,:*";

            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            try
            {
                byte[] bykey = System.Text.Encoding.UTF8.GetBytes(strEncrKey);
                byte[] InputByteArray = System.Text.Encoding.UTF8.GetBytes(strText);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(bykey, IV), CryptoStreamMode.Write);
                cs.Write(InputByteArray, 0, InputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string PWDDecrypt(string strText)
        {
            string sDecrKey;
            sDecrKey = "&%#@?,:*";

            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[strText.Length];
            try
            {
                byte[] byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion

        #region "* Data Security Implementation *"

        /// <summary> 
        /// This function will Clean the Input data and replace Single Quotes and "SCRIPT" tag and make it WEB Save and prevent Hacking 
        /// </summary> 
        /// <param name="SourceData">Variable to be replaced</param> 
        /// <returns>Cleaned Data</returns> 
        /// <remarks></remarks> 
        public string Clean(string SourceData)
        {
            string CleanedData = Clean(SourceData, true, false);
            return CleanedData;
        }

        public string Clean(string SourceData, bool HTMLAllowed, bool scriptTagAllowed)
        {
            string SRC;
            SRC = "";
            if (SourceData != "" & SourceData != null)
            {

                SRC = SourceData;
                // SRC = SourceData.Trim();
                // Replace single quote 
                SRC = SRC.Replace("'", "''");
                SRC = SRC.Replace("<", "&lt;");
                SRC = SRC.Replace(">", "&gt;");

                if (HTMLAllowed == false)
                {
                    SRC = HttpContext.Current.Server.UrlEncode(SRC);
                }
                else
                {
                    if (scriptTagAllowed == false)
                    {
                        // Replace SCRIPT Tag 

                        SRC = SRC.Replace("<script", "&lt;script");
                        SRC = SRC.Replace("</script", "&lt;/script");

                        SRC = SRC.Replace("<input", "&lt;input");
                        SRC = SRC.Replace("</input", "&lt;/input");

                        SRC = SRC.Replace("<form", "&lt;form");
                        SRC = SRC.Replace("</form", "&lt;/form");

                        SRC = SRC.Replace("<embed", "&lt;embed");
                        SRC = SRC.Replace("</embed", "&lt;/embed");


                        SRC = SRC.Replace("<textarea", "&lt;textarea");
                        SRC = SRC.Replace("</textarea", "&lt;/textarea");

                        SRC = SRC.Replace("<select", "&lt;select");
                        SRC = SRC.Replace("</select", "&lt;/select");

                        SRC = SRC.Replace("<img", "&lt;img");
                        SRC = SRC.Replace("</img", "&lt;/img");
                    }
                }
            }

            return SRC;
        }

        #endregion

        #region Error Log Implementation
        
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
        public static void APILogLog(string sPathName, string sErrMsg, string APIName,string CompanyRefNo)
        {
            CreateLogFiles();
            StreamWriter streamWriter = new StreamWriter(string.Concat(sPathName, "(", sErrorTime, ").txt"), true);
            streamWriter.WriteLine("Case Id" + CompanyRefNo + ":" + APIName);
            streamWriter.WriteLine(string.Concat(sLogFormat, sErrMsg));          
            streamWriter.WriteLine("__________________________________________________________________");
            streamWriter.Flush();
            streamWriter.Close();
        }

        public static void UserLog(string sPathName, string sErrMsg, string sStackTrace, string sUType)
        {
            CreateLogFiles();
            StreamWriter streamWriter = new StreamWriter(string.Concat(sPathName, "(", sErrorTime, ").txt"), true);
            streamWriter.WriteLine(sLogFormat);
            streamWriter.WriteLine("Unauthorized Attempted User Details");
            streamWriter.WriteLine(string.Concat("User Type  : ", sUType));
            streamWriter.WriteLine(string.Concat("User Name : ", sErrMsg));
            streamWriter.WriteLine(string.Concat("Password   : ", sStackTrace));
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

        public static void AddPQLogTrasaction(int PersonalRowID, string PageName, string CaseStatus, string UniqueComponentID, string TransactionAction)
        {
            try
            {
                AddPQLogTrasactionViewModel logModel = new AddPQLogTrasactionViewModel();
                logModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                logModel.TeamMemberRowID = Convert.ToInt16(HttpContext.Current.Session["TeamMemberRowID"]);
                logModel.UserType = HttpContext.Current.Session["Designation"].ToString();
                logModel.PageName = PageName;   // "Add Candidate";
                logModel.CaseStatus = CaseStatus;   // "Candidate Reference Generated";
                logModel.UniqueComponentID = UniqueComponentID; // "Input";
                logModel.TransactionDate = DateTime.Now;
                logModel.TransactionAction = TransactionAction; // "Insert";
                logModel.Status = 1;

                IPQLogTrasactionRepository repoPQLogTrasaction = new PQLogTrasactionRepository();
                repoPQLogTrasaction.AddPQLogTrasaction(logModel);
                repoPQLogTrasaction.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddPQLogTrasactionByCandidate(int PersonalRowID, string PageName, string CaseStatus, string UniqueComponentID, string TransactionAction,int ClientRowId)
        {
            try
            {
                AddPQLogTrasactionViewModel logModel = new AddPQLogTrasactionViewModel();
                logModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                logModel.TeamMemberRowID = Convert.ToInt16(ClientRowId);
                logModel.UserType = "Candidate";
                logModel.PageName = PageName;   // "Add Candidate";
                logModel.CaseStatus = CaseStatus;   // "Candidate Reference Generated";
                logModel.UniqueComponentID = UniqueComponentID; // "Input";
                logModel.TransactionDate = DateTime.Now;
                logModel.TransactionAction = TransactionAction; // "Insert";
                logModel.Status = 1;
                IPQLogTrasactionRepository repoPQLogTrasaction = new PQLogTrasactionRepository();
                repoPQLogTrasaction.AddPQLogTrasaction(logModel);
                repoPQLogTrasaction.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }



        public static void AddPQLogTrasactionByClient(int PersonalRowID, string PageName, string CaseStatus, string UniqueComponentID, string TransactionAction)
        {
            try
            {
                AddPQLogTrasactionViewModel logModel = new AddPQLogTrasactionViewModel();
                logModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                logModel.TeamMemberRowID = Convert.ToInt16(HttpContext.Current.Session["ClientRowID"]);
                logModel.UserType = HttpContext.Current.Session["ClientName"].ToString();
                logModel.PageName = PageName;   // "Add Candidate";
                logModel.CaseStatus = CaseStatus;   // "Candidate Reference Generated";
                logModel.UniqueComponentID = UniqueComponentID; // "Input";
                logModel.TransactionDate = DateTime.Now;
                logModel.TransactionAction = TransactionAction; // "Insert";
                logModel.Status = 1;

                IPQLogTrasactionRepository repoPQLogTrasaction = new PQLogTrasactionRepository();
                repoPQLogTrasaction.AddPQLogTrasaction(logModel);
                repoPQLogTrasaction.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddPQLogTrasactionByCandidate(int PersonalRowID, string PageName, string CaseStatus, string UniqueComponentID, string TransactionAction)
        {
            try
            {
                AddPQLogTrasactionViewModel logModel = new AddPQLogTrasactionViewModel();
                logModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                logModel.TeamMemberRowID = Convert.ToInt16(HttpContext.Current.Session["TempPersonalRowID"]);
                logModel.UserType = "Candidate";
                logModel.PageName = PageName;   // "Add Candidate";
                logModel.CaseStatus = CaseStatus;   // "Candidate Reference Generated";
                logModel.UniqueComponentID = UniqueComponentID; // "Input";
                logModel.TransactionDate = DateTime.Now;
                logModel.TransactionAction = TransactionAction; // "Insert";
                logModel.Status = 1;

                IPQLogTrasactionRepository repoPQLogTrasaction = new PQLogTrasactionRepository();
                repoPQLogTrasaction.AddPQLogTrasaction(logModel);
                repoPQLogTrasaction.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddPQLogTrasactionByPartner(int PersonalRowID, string PageName, string CaseStatus, string UniqueComponentID, string TransactionAction)
        {
            try
            {
                AddPQLogTrasactionViewModel logModel = new AddPQLogTrasactionViewModel();
                logModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                logModel.TeamMemberRowID = Convert.ToInt16(HttpContext.Current.Session["VendorRowID"]);
                logModel.UserType ="Partner";
                logModel.PageName = PageName;   // "Add Candidate";
                logModel.CaseStatus = CaseStatus;   // "Candidate Reference Generated";
                logModel.UniqueComponentID = UniqueComponentID; // "Input";
                logModel.TransactionDate = DateTime.Now;
                logModel.TransactionAction = TransactionAction; // "Insert";
                logModel.Status = 1;

                IPQLogTrasactionRepository repoPQLogTrasaction = new PQLogTrasactionRepository();
                repoPQLogTrasaction.AddPQLogTrasaction(logModel);
                repoPQLogTrasaction.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Case/Check Action History Save 

        public static void AddCaseActionHistory(int PersonalRowID, string CaseStatus, string Remarks)
        {
            try
            {
                AddCaseActionHistoryViewModel caseModel = new AddCaseActionHistoryViewModel();
                caseModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                caseModel.Remarks = Remarks;      // "Description of the case status";
                caseModel.CaseStatus = CaseStatus;   // "Case Status Of Case";
                caseModel.UpdatedBy = Convert.ToInt16(HttpContext.Current.Session["TeamMemberRowID"]); ;    // "UpdatedBy";
                caseModel.UpdatedByNameDesig = HttpContext.Current.Session["TeamMemberName"].ToString() + " (" + HttpContext.Current.Session["Designation"].ToString() + ")";
                caseModel.UpdatedDate = DateTime.Now;
                caseModel.Status = 1;

                ICaseActionHistoryRepository repoCaseActionHistory = new CaseActionHistoryRepository();
                repoCaseActionHistory.AddCaseActionHistory(caseModel);
                repoCaseActionHistory.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddCaseActionHistoryByClient(int PersonalRowID, string CaseStatus, string Remarks)
        {
            try
            {
                AddCaseActionHistoryViewModel caseModel = new AddCaseActionHistoryViewModel();
                caseModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                caseModel.Remarks = Remarks;      // "Description of the case status";
                caseModel.CaseStatus = CaseStatus;   // "Case Status Of Case";
                caseModel.UpdatedBy = Convert.ToInt16(HttpContext.Current.Session["ClientRowID"]);  // "UpdatedBy";
                caseModel.UpdatedByNameDesig = HttpContext.Current.Session["ClientName"].ToString();
                caseModel.UpdatedDate = DateTime.Now;
                caseModel.Status = 1;

                ICaseActionHistoryRepository repoCaseActionHistory = new CaseActionHistoryRepository();
                repoCaseActionHistory.AddCaseActionHistory(caseModel);
                repoCaseActionHistory.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddCaseActionHistoryByCandidate(int PersonalRowID, string CaseStatus, string Remarks)
        {
            try
            {
                AddCaseActionHistoryViewModel caseModel = new AddCaseActionHistoryViewModel();
                caseModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                caseModel.Remarks = Remarks;      // "Description of the case status";
                caseModel.CaseStatus = CaseStatus;   // "Case Status Of Case";
                caseModel.UpdatedBy = Convert.ToInt16(HttpContext.Current.Session["TempPersonalRowID"]);  // "UpdatedBy";
                caseModel.UpdatedByNameDesig = HttpContext.Current.Session["LoginCandidateName"].ToString();
                caseModel.UpdatedDate = DateTime.Now;
                caseModel.Status = 1;

                ICaseActionHistoryRepository repoCaseActionHistory = new CaseActionHistoryRepository();
                repoCaseActionHistory.AddCaseActionHistory(caseModel);
                repoCaseActionHistory.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddCheckActionHistory(int PersonalRowID, short SubCheckRowID, string CheckStatus, string Remarks)
        {
            try
            {
                AddCheckActionHistoryViewModel checkModel = new AddCheckActionHistoryViewModel();
                checkModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                checkModel.SubCheckRowID = SubCheckRowID;  // SubCheckRo
                checkModel.Remarks = Remarks;   // "Description of the check status";
                checkModel.CheckStatus = CheckStatus;   // "Candidate check status";
                checkModel.UpdatedBy = Convert.ToInt16(HttpContext.Current.Session["TeamMemberRowID"]);
                checkModel.UpdatedByNameDesig = HttpContext.Current.Session["TeamMemberName"].ToString() + " (" + HttpContext.Current.Session["Designation"].ToString() + ")";
                checkModel.UpdatedDate = DateTime.Now;
                checkModel.Status = 1;


                //ICheckActionHistoryRepository repoCaseActionHistory = new CheckActionHistoryRepository();
                //repoCaseActionHistory.AddCheckActionHistory(checkModel);
                //repoCaseActionHistory.SaveChanges();

                ClientCustomize cc = new ClientCustomize();

                cc.InsertCheckActionHistories(checkModel.PersonalRowID, checkModel.SubCheckRowID, checkModel.CheckStatus, checkModel.Remarks, checkModel.UpdatedBy, checkModel.UpdatedByNameDesig);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void AddCheckActionHistoryByClient(int PersonalRowID, short SubCheckRowID, string CheckStatus, string Remarks)
        {
            try
            {
                AddCheckActionHistoryViewModel checkModel = new AddCheckActionHistoryViewModel();
                checkModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                checkModel.SubCheckRowID = SubCheckRowID;  // SubCheckRo
                checkModel.Remarks = Remarks;   // "Description of the check status";
                checkModel.CheckStatus = CheckStatus;   // "Candidate check status";
                checkModel.UpdatedBy = Convert.ToInt16(HttpContext.Current.Session["ClientRowID"]);
                checkModel.UpdatedByNameDesig = HttpContext.Current.Session["ClientName"].ToString();
                checkModel.UpdatedDate = DateTime.Now;
                checkModel.Status = 1;

                ICheckActionHistoryRepository repoCaseActionHistory = new CheckActionHistoryRepository();
                repoCaseActionHistory.AddCheckActionHistory(checkModel);
                repoCaseActionHistory.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddCheckActionHistoryByCandidate(int PersonalRowID, short SubCheckRowID, string CheckStatus, string Remarks,int ClientRowid)
        {
            try
            {
                AddCheckActionHistoryViewModel checkModel = new AddCheckActionHistoryViewModel();
                checkModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                checkModel.SubCheckRowID = SubCheckRowID;  // SubCheckRo
                checkModel.Remarks = Remarks;   // "Description of the check status";
                checkModel.CheckStatus = CheckStatus;   // "Candidate check status";
                checkModel.UpdatedBy = Convert.ToInt16(ClientRowid);
                checkModel.UpdatedByNameDesig = "Candidate";
                checkModel.UpdatedDate = DateTime.Now;
                checkModel.Status = 1;

                ICheckActionHistoryRepository repoCaseActionHistory = new CheckActionHistoryRepository();
                repoCaseActionHistory.AddCheckActionHistory(checkModel);
                repoCaseActionHistory.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddCheckActionHistoryByCandidate(int PersonalRowID, short SubCheckRowID, string CheckStatus, string Remarks)
        {
            try
            {
                AddCheckActionHistoryViewModel checkModel = new AddCheckActionHistoryViewModel();
                checkModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                checkModel.SubCheckRowID = SubCheckRowID;  // SubCheckRo
                checkModel.Remarks = Remarks;   // "Description of the check status";
                checkModel.CheckStatus = CheckStatus;   // "Candidate check status";
                checkModel.UpdatedBy = Convert.ToInt16(HttpContext.Current.Session["TempPersonalRowID"]);
                checkModel.UpdatedByNameDesig = HttpContext.Current.Session["LoginCandidateName"].ToString();
                checkModel.UpdatedDate = DateTime.Now;
                checkModel.Status = 1;

                ICheckActionHistoryRepository repoCaseActionHistory = new CheckActionHistoryRepository();
                repoCaseActionHistory.AddCheckActionHistory(checkModel);
                repoCaseActionHistory.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddCheckActionHistoryByPartner(int PersonalRowID, short SubCheckRowID, string CheckStatus, string Remarks)
        {
            try
            {
                AddCheckActionHistoryViewModel checkModel = new AddCheckActionHistoryViewModel();
                checkModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                checkModel.SubCheckRowID = SubCheckRowID;  // SubCheckRo
                checkModel.Remarks = Remarks;   // "Description of the check status";
                checkModel.CheckStatus = CheckStatus;   // "Candidate check status";
                checkModel.UpdatedBy = Convert.ToInt16(HttpContext.Current.Session["VendorRowID"]);
                checkModel.UpdatedByNameDesig = HttpContext.Current.Session["VendorName"].ToString();
                checkModel.UpdatedDate = DateTime.Now;
                checkModel.Status = 1;
               
                //ICheckActionHistoryRepository repoCaseActionHistory = new CheckActionHistoryRepository();
                //repoCaseActionHistory.AddCheckActionHistory(checkModel);
                //repoCaseActionHistory.SaveChanges();

                ClientCustomize cc = new ClientCustomize();
                cc.InsertCheckActionHistories(checkModel.PersonalRowID, checkModel.SubCheckRowID, checkModel.CheckStatus, checkModel.Remarks, checkModel.UpdatedBy, checkModel.UpdatedByNameDesig);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        public static string Encryption_Heptagon(string s1)
        {
            var key = "aFT5J4xU7P6vmvJUUygqd6KUeGKBuv28";
            var iv = "Xwizyqm36ihpjdfr";
            var plainText = s1;

            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");

            byte[] encrypted;
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt_Heptagon(string s2)
        {
            var key = "aFT5J4xU7P6vmvJUUygqd6KUeGKBuv28";
            var iv = "Xwizyqm36ihpjdfr";
            var cipherText = s2;

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");

            string plaintext = null;
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;


        }

        public static bool ValidateSQLInjuctionInput(string value)
        {
            var Result = true;
            if(value!="")
            {
                string[] strArrays2 = { "SELECT ","ALTER TABLE","BETWEEN","CONSTRAINT","PROCEDURE","DELETE","DROP TABLE",
            "EXEC ","EXISTS","GROUP BY" ,"HAVING","INSERT","ORDER BY" ,"TRUNCATE","UPDATE" ,"HYPERLINK","WWW "};
                for (int i = 0; i < strArrays2.Length; i++)
                {                    
                        if (value.ToUpper().Contains(strArrays2[i].ToString()))
                        {
                            Result = false;
                            break;
                        }          
                }
            } 
            return Result;
        }

        public static string ReplaceSQLInjuctionInputs(string value)
        {
            var Result = value;
            if (value != "")
            {
                string[] strArrays2 = { "SELECT ", "ALTER","BETWEEN","CONSTRAINT","PROCEDURE","DELETE","DROP TABLE",
            "EXEC ","EXISTS","GROUP BY" ,"HAVING","INSERT","ORDER BY" ,"TRUNCATE","UPDATE" ,"HYPERLINK","WWW "};
                for (int i = 0; i < strArrays2.Length; i++)
                {
                    if (value.ToUpper().Contains(strArrays2[i].ToString()))
                    {
                        Result = Result.ToUpper().Replace(strArrays2[i].ToString(), "");

                    }
                }
            }


            return Result;
        }

        public static string HTMLToText(string HTMLCode)
        {           
            HTMLCode = HTMLCode.Replace("\n", " ");            
            HTMLCode = HTMLCode.Replace("\t", " ");            
            HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");          
            HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);            
            HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);           
            StringBuilder sbHTML = new StringBuilder(HTMLCode);           
            string[] OldWords = { "&nbsp;", "&amp;", "&quot;", "&lt;", "&gt;", "&reg;", "&copy;", "&bull;", "&trade;", "&#39;" };
            string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢", "\'" };
            for (int i = 0; i < OldWords.Length; i++)
            {
                sbHTML.Replace(OldWords[i], NewWords[i]);
            }            
            sbHTML.Replace("<br>", "\n<br>");
            sbHTML.Replace("<br ", "\n<br ");
            sbHTML.Replace("<p ", "\n<p ");            
            return System.Text.RegularExpressions.Regex.Replace(sbHTML.ToString(), "<[^>]*>", "");
        }

        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return ".png";
                case "/9J/4":
                    return ".jpg";
                case "AAAAF":
                    return ".mp4";
                case "JVBER":
                    return ".pdf";
                case "AAABA":
                    return ".ico";
                case "UMFYI":
                    return ".rar";
                case "E1XYD":
                    return ".rtf";
                case "U1PKC":
                    return ".txt";
                case "MQOWM":
                case "77U/M":
                    return ".srt";
                default:
                    return string.Empty;
            }
        }
        public static string GetSpecialCharacterReplaced(string FileName)
        {
            string OutputFileName = FileName;
            if (FileName != "")
            {
                string[] StrArray = { "+", " ", "-", "&" };

                for (int i = 0; i < StrArray.Length; i++)
                {
                    if (FileName.Contains(StrArray[i].ToString()))
                    {
                        OutputFileName = OutputFileName.Replace(StrArray[i].ToString(), "_");

                    }
                }
            }
            return OutputFileName;
        }        
        public static string SanitizeForCsvAndPreventCSVInjection(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            char[] dangerousChars = { '=', '+', '-', '@' };
            if (dangerousChars.Contains(input[0]))
            {
                return $"'{input}";
            }

            return input;
        }

    }
}